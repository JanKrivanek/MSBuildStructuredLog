using FluentAssertions;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using System;
using System.IO;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="StructuredLogger"/> class.
    /// </summary>
    public class StructuredLoggerTests
    {
        // A constant error message expected when parameters are invalid.
        private const string InvalidParamSpecificationMessage = "Need to specify a log file using the following pattern: '/logger:StructuredLogger,StructuredLogger.dll;log.buildlog";

        /// <summary>
        /// Tests that Initialize throws a LoggerException when Parameters is null.
        /// </summary>
        [Fact]
        public void Initialize_NullParameters_ThrowsLoggerException()
        {
            // Arrange
            var logger = new StructuredLogger();
            logger.Parameters = null;  // Parameters not set
            var eventSourceMock = new Mock<IEventSource>();

            // Act
            Action act = () => logger.Initialize(eventSourceMock.Object);

            // Assert
            act.Should().Throw<LoggerException>()
                .WithMessage(InvalidParamSpecificationMessage);
        }

        /// <summary>
        /// Tests that Initialize throws a LoggerException when multiple parameters are provided.
        /// </summary>
        [Fact]
        public void Initialize_MultipleParameters_ThrowsLoggerException()
        {
            // Arrange
            var logger = new StructuredLogger();
            logger.Parameters = "file1.log;file2.log";  // Multiple parameters provided
            var eventSourceMock = new Mock<IEventSource>();

            // Act
            Action act = () => logger.Initialize(eventSourceMock.Object);

            // Assert
            act.Should().Throw<LoggerException>()
                .WithMessage(InvalidParamSpecificationMessage);
        }

        /// <summary>
        /// Tests that Initialize with a valid parameter sets up the expected environment variables.
        /// </summary>
        [Fact]
        public void Initialize_ValidParameters_SetsEnvironmentVariables()
        {
            // Arrange
            var logger = new StructuredLogger();
            // Providing parameter without extra quotes is acceptable; the ProcessParameters method trims quotes if present.
            logger.Parameters = "test.log";
            // Set SaveLogToDisk to false to avoid file based operations within Initialize.
            StructuredLogger.SaveLogToDisk = false;
            var eventSourceMock = new Mock<IEventSource>();

            // Act
            logger.Initialize(eventSourceMock.Object);

            // Assert - Verify that environment variables are set.
            Environment.GetEnvironmentVariable("MSBUILDTARGETOUTPUTLOGGING").Should().Be("true");
            Environment.GetEnvironmentVariable("MSBUILDLOGIMPORTS").Should().Be("1");
        }

        /// <summary>
        /// Tests that Shutdown sets the static CurrentBuild property when SaveLogToDisk is false.
        /// </summary>
        [Fact]
        public void Shutdown_SaveLogToDiskFalse_SetsCurrentBuild()
        {
            // Arrange
            StructuredLogger.SaveLogToDisk = false;
            var logger = new StructuredLogger();
            logger.Parameters = "dummy.log";
            var eventSourceMock = new Mock<IEventSource>();
            logger.Initialize(eventSourceMock.Object);

            // Act
            logger.Shutdown();

            // Assert - After shutdown, CurrentBuild should be set.
            StructuredLogger.CurrentBuild.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that Shutdown writes the log file to disk when SaveLogToDisk is true.
        /// </summary>
        [Fact]
        public void Shutdown_SaveLogToDiskTrue_WritesLogFile()
        {
            // Arrange
            StructuredLogger.SaveLogToDisk = true;
            // Create a rooted file path in the temporary directory.
            string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".log");
            // Ensure the file does not exist before test.
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }

            var logger = new StructuredLogger();
            // Wrap the file path in quotes to simulate user input.
            logger.Parameters = $"\"{tempFilePath}\"";
            var eventSourceMock = new Mock<IEventSource>();
            logger.Initialize(eventSourceMock.Object);

            try
            {
                // Act
                logger.Shutdown();

                // Assert - The file should exist after shutdown.
                File.Exists(tempFilePath).Should().BeTrue("because the log file should have been written to disk when SaveLogToDisk is true");
            }
            finally
            {
                // Cleanup the created log file if it exists.
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }
    }
}
