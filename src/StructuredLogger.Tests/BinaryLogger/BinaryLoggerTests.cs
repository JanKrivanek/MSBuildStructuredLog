using System;
using System.IO;
using FluentAssertions;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BinaryLogger"/> class.
    /// </summary>
    public class BinaryLoggerTests : IDisposable
    {
        private readonly string _tempFilePath;

        public BinaryLoggerTests()
        {
            // Use a unique temporary file for each test.
            _tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".binlog");
        }

        public void Dispose()
        {
            // Cleanup temporary file if exists.
            if (File.Exists(_tempFilePath))
            {
                try
                {
                    File.Delete(_tempFilePath);
                }
                catch
                {
                    // ignore any file deletion errors
                }
            }
        }

        /// <summary>
        /// Tests that calling Initialize without setting the Parameters property throws a LoggerException.
        /// </summary>
        [Fact]
        public void Initialize_WithoutParameters_ThrowsLoggerException()
        {
            // Arrange
            var logger = new BinaryLogger();
            // Do not set Parameters so it remains null.
            var mockEventSource = new Mock<IEventSource>();

            // Act
            Action act = () => logger.Initialize(mockEventSource.Object);

            // Assert
            act.Should().Throw<LoggerException>()
                .WithMessage("*InvalidBinaryLoggerParameters*");
        }

        /// <summary>
        /// Tests that calling Initialize with an invalid parameter string throws a LoggerException.
        /// </summary>
        [Fact]
        public void Initialize_WithInvalidParameter_ThrowsLoggerException()
        {
            // Arrange
            var logger = new BinaryLogger
            {
                Parameters = "InvalidParam"
            };
            var mockEventSource = new Mock<IEventSource>();

            // Act
            Action act = () => logger.Initialize(mockEventSource.Object);

            // Assert
            act.Should().Throw<LoggerException>()
                .WithMessage("*InvalidBinaryLoggerParameters*");
        }

        /// <summary>
        /// Tests that Initialize with a valid file parameter creates the log file.
        /// </summary>
        [Fact]
        public void Initialize_WithValidParameter_CreatesFileAndShutdown_WorksSuccessfully()
        {
            // Arrange
            var logger = new BinaryLogger
            {
                // Provide a valid file parameter. Using explicit "LogFile=" to simulate parameter parsing.
                Parameters = $"LogFile=\"{_tempFilePath}\";OmitInitialInfo"
            };

            // Create a mock for IEventSource with no special behavior.
            var mockEventSource = new Mock<IEventSource>();

            // Act
            logger.Initialize(mockEventSource.Object);
            // simulate raising an event so that Write method is invoked.
            // Create a dummy BuildMessageEventArgs and raise the AnyEventRaised event.
            var testEventArgs = new BuildMessageEventArgs("Test message", null, "BinaryLogger", MessageImportance.Normal)
            {
                BuildEventContext = BuildEventContext.Invalid
            };
            // Using Moq's Raise to trigger event - must use the event on the mock.
            mockEventSource.Raise(es => es.AnyEventRaised += null, testEventArgs);
            logger.Shutdown();

            // Assert
            File.Exists(_tempFilePath).Should().BeTrue("because a valid file should have been created during initialization");
            // Additionally, check that the file is not empty.
            new FileInfo(_tempFilePath).Length.Should().BeGreaterThan(0, "because the log file should contain data");
        }

        /// <summary>
        /// Tests that Shutdown resets the environment variables to their original values.
        /// </summary>
        [Fact]
        public void Shutdown_ResetsEnvironmentVariables()
        {
            // Arrange
            const string initialTargetOutputLogging = "initialTarget";
            const string initialLogImports = "initialImports";
            Environment.SetEnvironmentVariable("MSBUILDTARGETOUTPUTLOGGING", initialTargetOutputLogging);
            Environment.SetEnvironmentVariable("MSBUILDLOGIMPORTS", initialLogImports);

            var logger = new BinaryLogger
            {
                Parameters = $"LogFile=\"{_tempFilePath}\";OmitInitialInfo"
            };

            var mockEventSource = new Mock<IEventSource>();
            logger.Initialize(mockEventSource.Object);

            // Change environment variables as they are overwritten in Initialize.
            Environment.SetEnvironmentVariable("MSBUILDTARGETOUTPUTLOGGING", "changed");
            Environment.SetEnvironmentVariable("MSBUILDLOGIMPORTS", "changed");

            // Act
            logger.Shutdown();

            // Assert
            Environment.GetEnvironmentVariable("MSBUILDTARGETOUTPUTLOGGING").Should().Be(initialTargetOutputLogging);
            Environment.GetEnvironmentVariable("MSBUILDLOGIMPORTS").Should().Be(initialLogImports);
        }
//  // [Error] (166-85)CS0246 The type or namespace name 'EmbeddedContentReadEventArgs' could not be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that Initialize works correctly when the event source implements IBinaryLogReplaySource.
//         /// </summary>
//         [Fact]
//         public void Initialize_WithBinaryLogReplaySource_ExecutesDeferredInitialization()
//         {
//             // Arrange
//             // Create a mock for IBinaryLogReplaySource.
//             var mockReplaySource = new Mock<IBinaryLogReplaySource>();
//             mockReplaySource.SetupGet(r => r.FileFormatVersion).Returns(25);
//             mockReplaySource.SetupGet(r => r.MinimumReaderVersion).Returns(18);
//             // Setup DeferredInitialize to execute the provided actions immediately.
//             mockReplaySource
//                 .Setup(r => r.DeferredInitialize(It.IsAny<Action>(), It.IsAny<Action>()))
//                 .Callback<Action, Action>((initAction, subscribeAction) =>
//                 {
//                     initAction();
//                     subscribeAction();
//                 });
//             // Also setup an empty EmbeddedContentRead to avoid null reference.
//             mockReplaySource.SetupAdd(r => r.EmbeddedContentRead += It.IsAny<Action<EmbeddedContentReadEventArgs>>());
// 
//             // As IBinaryLogReplaySource extends IEventSource, also setup for AnyEventRaised.
//             var replaySource = mockReplaySource.Object;
//             // Provide a valid Parameters so that FilePath is set.
//             var logger = new BinaryLogger
//             {
//                 Parameters = $"LogFile=\"{_tempFilePath}\""
//             };
// 
//             // Act
//             logger.Initialize(replaySource);
//             // Raise a dummy RawLogRecordReceived event via the delegate that was attached.
//             // Since we cannot intercept the internal delegate, we simulate by calling the event if accessible.
//             // Instead, we raise an AnyEventRaised event to simulate structured events.
//             if (replaySource is IEventSource evtSource)
//             {
//                 var testEventArgs = new BuildMessageEventArgs("Replay Test", null, "BinaryLogger", MessageImportance.Normal)
//                 {
//                     BuildEventContext = BuildEventContext.Invalid
//                 };
//                 // Use Moq's Raise capability on the mock.
//                 mockReplaySource.Raise(r => r.AnyEventRaised += null, testEventArgs);
//             }
//             logger.Shutdown();
// 
//             // Assert
//             File.Exists(_tempFilePath).Should().BeTrue("because the log file should have been created during initialization with replay source");
//         }
// 
        /// <summary>
        /// Tests that raising the AnyEventRaised event results in the logger writing the event without throwing an exception.
        /// </summary>
        [Fact]
        public void AnyEventRaised_WhenInvoked_WritesEventWithoutError()
        {
            // Arrange
            var logger = new BinaryLogger
            {
                Parameters = $"LogFile=\"{_tempFilePath}\";OmitInitialInfo"
            };
            var mockEventSource = new Mock<IEventSource>();
            logger.Initialize(mockEventSource.Object);

            var testEventArgs = new BuildMessageEventArgs("Event Raised Test", null, "BinaryLogger", MessageImportance.Normal)
            {
                BuildEventContext = BuildEventContext.Invalid
            };

            // Act
            Action act = () =>
            {
                mockEventSource.Raise(es => es.AnyEventRaised += null, testEventArgs);
            };

            // Assert
            act.Should().NotThrow("because handling AnyEventRaised should not throw an exception");
            logger.Shutdown();
        }

        /// <summary>
        /// Tests that calling Shutdown multiple times does not result in an error.
        /// </summary>
        [Fact]
        public void Shutdown_CalledMultipleTimes_DoesNotThrow()
        {
            // Arrange
            var logger = new BinaryLogger
            {
                Parameters = $"LogFile=\"{_tempFilePath}\";OmitInitialInfo"
            };
            var mockEventSource = new Mock<IEventSource>();
            logger.Initialize(mockEventSource.Object);

            // Act
            Action act = () =>
            {
                logger.Shutdown();
                // Call Shutdown a second time.
                logger.Shutdown();
            };

            // Assert
            act.Should().NotThrow("because calling Shutdown multiple times should not throw an exception");
        }

        /// <summary>
        /// Tests that the default values of public properties are as expected.
        /// </summary>
        [Fact]
        public void PublicProperties_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var logger = new BinaryLogger();

            // Assert
            // Default verbosity should be Diagnostic.
            logger.Verbosity.Should().Be(LoggerVerbosity.Diagnostic);
            // Default CollectProjectImports should be Embed.
            logger.CollectProjectImports.Should().Be(BinaryLogger.ProjectImportsCollectionMode.Embed);
        }
    }
}
