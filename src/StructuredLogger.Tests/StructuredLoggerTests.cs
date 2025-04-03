using System;
using FluentAssertions;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="StructuredLogger"/> class.
    /// </summary>
    public class StructuredLoggerTests
    {
        private const string InvalidParameterMessage = "Need to specify a log file using the following pattern: '/logger:StructuredLogger,StructuredLogger.dll;log.buildlog";
        private readonly StructuredLogger _logger;
        private readonly Mock<IEventSource> _mockEventSource;

        public StructuredLoggerTests()
        {
            _logger = new StructuredLogger();
            _mockEventSource = new Mock<IEventSource>();
        }

        /// <summary>
        /// Tests that Initialize throws a LoggerException when Parameters is null.
        /// Arrange: Sets Parameters to null.
        /// Act: Calls Initialize.
        /// Assert: Expects a LoggerException with the invalid parameters message.
        /// </summary>
        [Fact]
        public void Initialize_WithNullParameters_ThrowsLoggerException()
        {
            // Arrange
            _logger.Parameters = null;

            // Act
            Action act = () => _logger.Initialize(_mockEventSource.Object);

            // Assert
            act.Should().Throw<LoggerException>()
                .WithMessage(InvalidParameterMessage + "*");
        }

        /// <summary>
        /// Tests that Initialize throws a LoggerException when Parameters do not split into exactly one part.
        /// Arrange: Sets Parameters with an invalid format.
        /// Act: Calls Initialize.
        /// Assert: Expects a LoggerException with the invalid parameters message.
        /// </summary>
        [Fact]
        public void Initialize_WithInvalidParametersFormat_ThrowsLoggerException()
        {
            // Arrange
            _logger.Parameters = "param1;param2";

            // Act
            Action act = () => _logger.Initialize(_mockEventSource.Object);

            // Assert
            act.Should().Throw<LoggerException>()
                .WithMessage(InvalidParameterMessage + "*");
        }

        /// <summary>
        /// Tests that Initialize correctly processes valid parameters, sets environment variables, and initializes internal construction.
        /// Arrange: Sets Parameters with a valid log file path in quotes.
        /// Act: Calls Initialize.
        /// Assert: Environment variables are correctly set and Construction property is not null.
        /// </summary>
        [Fact]
        public void Initialize_WithValidParameters_SetsEnvironmentVariablesAndInitializesConstruction()
        {
            // Arrange
            // Using a relative path to avoid directory creation in Shutdown.
            _logger.Parameters = "\"relative.log\"";

            // Act
            _logger.Initialize(_mockEventSource.Object);

            // Assert
            Environment.GetEnvironmentVariable("MSBUILDTARGETOUTPUTLOGGING").Should().Be("true");
            Environment.GetEnvironmentVariable("MSBUILDLOGIMPORTS").Should().Be("1");
            _logger.Construction.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that Shutdown sets the static CurrentBuild when SaveLogToDisk is false.
        /// Arrange: Sets valid Parameters and disables disk saving.
        /// Act: Calls Initialize and then Shutdown.
        /// Assert: The static CurrentBuild property holds the build from the logger's construction.
        /// </summary>
        [Fact]
        public void Shutdown_WhenSaveLogToDiskFalse_SetsCurrentBuild()
        {
            // Arrange
            _logger.Parameters = "\"relative.log\"";
            // Set SaveLogToDisk to false to avoid file IO in Shutdown.
            StructuredLogger.SaveLogToDisk = false;
            _logger.Initialize(_mockEventSource.Object);

            // Act
            _logger.Shutdown();

            // Assert
            StructuredLogger.CurrentBuild.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that Shutdown does not throw an exception when SaveLogToDisk is true.
        /// Arrange: Sets valid Parameters and enables disk saving.
        /// Act: Calls Initialize and then Shutdown.
        /// Assert: Shutdown completes without throwing any exceptions.
        /// </summary>
        [Fact]
        public void Shutdown_WhenSaveLogToDiskTrue_DoesNotThrow()
        {
            // Arrange
            _logger.Parameters = "\"relative.log\"";
            StructuredLogger.SaveLogToDisk = true;
            _logger.Initialize(_mockEventSource.Object);

            // Act
            Action act = () => _logger.Shutdown();

            // Assert
            act.Should().NotThrow();
        }
    }
}
