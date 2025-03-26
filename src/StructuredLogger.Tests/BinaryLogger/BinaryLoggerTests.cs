using System.IO;
using System.IO.Compression;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using Microsoft.Build.Utilities;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// A fake implementation of IEventSource and its derived interfaces to facilitate testing of BinaryLogger.
    /// </summary>
    public class FakeEventSource : IEventSource, IEventSource3, IEventSource4
    {
        public event EventHandler<BuildEventArgs> AnyEventRaised;

        /// <summary>
        /// Triggers the AnyEventRaised event.
        /// </summary>
        /// <param name="args">The build event arguments to be raised.</param>
        public void RaiseAnyEvent(BuildEventArgs args)
        {
            AnyEventRaised?.Invoke(this, args);
        }

        public void IncludeEvaluationMetaprojects()
        {
            // No operation needed for testing.
        }

        public void IncludeEvaluationPropertiesAndItems()
        {
            // No operation needed for testing.
        }
    }

    /// <summary>
    /// Contains xUnit tests for the BinaryLogger class.
    /// </summary>
    public class BinaryLoggerTests : IDisposable
    {
        private readonly string _tempFilePath;

        /// <summary>
        /// Constructor that sets up a unique temporary file path for each test.
        /// </summary>
        public BinaryLoggerTests()
        {
            _tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".binlog");
        }

        /// <summary>
        /// Performs necessary cleanup after each test.
        /// </summary>
        public void Dispose()
        {
            if (File.Exists(_tempFilePath))
            {
                try
                {
                    File.Delete(_tempFilePath);
                }
                catch
                {
                    // Suppress any exceptions during cleanup.
                }
            }
        }

        /// <summary>
        /// Tests that Initialize throws a LoggerException when Parameters is null.
        /// </summary>
        [Fact]
        public void Initialize_NullParameters_ThrowsLoggerException()
        {
            // Arrange
            var logger = new BinaryLogger { Parameters = null };
            var fakeSource = new FakeEventSource();

            // Act & Assert
            Assert.Throws<LoggerException>(() => logger.Initialize(fakeSource));
        }

        /// <summary>
        /// Tests that Initialize throws a LoggerException when an invalid parameter is provided.
        /// </summary>
        [Fact]
        public void Initialize_InvalidParameter_ThrowsLoggerException()
        {
            // Arrange
            var logger = new BinaryLogger { Parameters = "InvalidParameter" };
            var fakeSource = new FakeEventSource();

            // Act & Assert
            Assert.Throws<LoggerException>(() => logger.Initialize(fakeSource));
        }

        /// <summary>
        /// Tests that Initialize with valid parameters correctly sets the environment variables,
        /// creates the log file, and that Shutdown writes the terminator byte and disposes the stream.
        /// </summary>
        [Fact]
        public void Initialize_ValidParameters_CreatesLogFileAndSetsEnvironmentVariables()
        {
            // Arrange
            // Set initial environment variable values.
            Environment.SetEnvironmentVariable("MSBUILDTARGETOUTPUTLOGGING", "OriginalTargetValue");
            Environment.SetEnvironmentVariable("MSBUILDLOGIMPORTS", "OriginalImportsValue");

            string parameters = _tempFilePath + ";ProjectImports=Embed";
            var logger = new BinaryLogger { Parameters = parameters };
            var fakeSource = new FakeEventSource();

            // Act
            logger.Initialize(fakeSource);
            // Raise a sample event to cause writing.
            var messageEvent = new BuildMessageEventArgs("TestMessage", null, "BinaryLogger", MessageImportance.Normal);
            fakeSource.RaiseAnyEvent(messageEvent);
            logger.Shutdown();

            // Assert
            // Verify that environment variables have been restored.
            Assert.Equal("OriginalTargetValue", Environment.GetEnvironmentVariable("MSBUILDTARGETOUTPUTLOGGING"));
            Assert.Equal("OriginalImportsValue", Environment.GetEnvironmentVariable("MSBUILDLOGIMPORTS"));

            // Verify that the log file exists and has non-zero length.
            Assert.True(File.Exists(_tempFilePath));
            byte[] fileContents = File.ReadAllBytes(_tempFilePath);
            Assert.NotEmpty(fileContents);

            // The last byte should be the terminator marker.
            byte terminatorByte = fileContents[fileContents.Length - 1];
            // As written in Shutdown, the terminator is written as: stream.WriteByte((byte)BinaryLogRecordKind.EndOfFile);
            // We assume that BinaryLogRecordKind.EndOfFile is 0.
            Assert.Equal((byte)0, terminatorByte);
        }

        /// <summary>
        /// Tests that Shutdown can be safely invoked multiple times without throwing an exception.
        /// </summary>
        [Fact]
        public void Shutdown_MultipleInvocations_DoesNotThrow()
        {
            // Arrange
            string parameters = _tempFilePath + ";ProjectImports=Embed";
            var logger = new BinaryLogger { Parameters = parameters };
            var fakeSource = new FakeEventSource();
            logger.Initialize(fakeSource);

            // Act
            logger.Shutdown();
            Exception secondShutdownException = Record.Exception(() => logger.Shutdown());

            // Assert
            Assert.Null(secondShutdownException);
        }

        /// <summary>
        /// Tests that raising an event from the event source results in data being written to the log file.
        /// </summary>
        [Fact]
        public void AnyEventRaised_EventTriggered_WritesDataToLogFile()
        {
            // Arrange
            string parameters = _tempFilePath + ";ProjectImports=Embed";
            var logger = new BinaryLogger { Parameters = parameters };
            var fakeSource = new FakeEventSource();
            logger.Initialize(fakeSource);

            // Act
            var testMessage = "Hello from test event";
            var buildEvent = new BuildMessageEventArgs(testMessage, null, "BinaryLogger", MessageImportance.Normal);
            fakeSource.RaiseAnyEvent(buildEvent);
            logger.Shutdown();

            // Assert
            Assert.True(File.Exists(_tempFilePath));
            // Decompress and read the initial version headers from the log file.
            using (FileStream fileStream = new FileStream(_tempFilePath, FileMode.Open, FileAccess.Read))
            using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
            using (BinaryReader reader = new BinaryReader(gzipStream))
            {
                int fileFormatVersion = reader.ReadInt32();
                int minimumReaderVersion = reader.ReadInt32();
                Assert.Equal(BinaryLogger.FileFormatVersion, fileFormatVersion);
                Assert.Equal(BinaryLogger.MinimumReaderVersion, minimumReaderVersion);
            }
        }
    }
}
