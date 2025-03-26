using System.Collections.Generic;
using StructuredLogger.BinaryLogger;
using Xunit;

namespace StructuredLogger.BinaryLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildCheckTracingEventArgs"/> class.
    /// </summary>
    public class BuildCheckTracingEventArgsTests
    {
        /// <summary>
        /// Tests that the constructor correctly assigns a non-empty dictionary to the TracingData property.
        /// </summary>
        [Fact]
        public void Ctor_WithValidTracingData_AssignsTracingDataCorrectly()
        {
            // Arrange
            var expectedTracingData = new Dictionary<string, TimeSpan>
            {
                { "Phase1", TimeSpan.FromSeconds(1) },
                { "Phase2", TimeSpan.FromSeconds(2) }
            };

            // Act
            var eventArgs = new BuildCheckTracingEventArgs(expectedTracingData);

            // Assert
            Assert.Same(expectedTracingData, eventArgs.TracingData);
        }

        /// <summary>
        /// Tests that the constructor correctly assigns an empty dictionary to the TracingData property.
        /// </summary>
        [Fact]
        public void Ctor_WithEmptyTracingData_AssignsEmptyDictionary()
        {
            // Arrange
            var expectedTracingData = new Dictionary<string, TimeSpan>();

            // Act
            var eventArgs = new BuildCheckTracingEventArgs(expectedTracingData);

            // Assert
            Assert.NotNull(eventArgs.TracingData);
            Assert.Empty(eventArgs.TracingData);
        }

        /// <summary>
        /// Tests that the constructor assigns a null tracing data object if null is provided.
        /// </summary>
        [Fact]
        public void Ctor_WithNullTracingData_AssignsNullTracingData()
        {
            // Arrange
            Dictionary<string, TimeSpan> expectedTracingData = null;

            // Act
            var eventArgs = new BuildCheckTracingEventArgs(expectedTracingData);

            // Assert
            Assert.Null(eventArgs.TracingData);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="BuildCheckAcquisitionEventArgs"/> class.
    /// </summary>
    public class BuildCheckAcquisitionEventArgsTests
    {
        /// <summary>
        /// Tests that the constructor correctly assigns provided acquisition and project paths.
        /// </summary>
        [Fact]
        public void Ctor_WithValidPaths_AssignsPropertiesCorrectly()
        {
            // Arrange
            const string expectedAcquisitionPath = @"C:\Build\acquisition.log";
            const string expectedProjectPath = @"C:\Projects\MyProject.csproj";

            // Act
            var eventArgs = new BuildCheckAcquisitionEventArgs(expectedAcquisitionPath, expectedProjectPath);

            // Assert
            Assert.Equal(expectedAcquisitionPath, eventArgs.AcquisitionPath);
            Assert.Equal(expectedProjectPath, eventArgs.ProjectPath);
        }

        /// <summary>
        /// Tests that the constructor assigns null to properties if null values are provided.
        /// </summary>
        [Fact]
        public void Ctor_WithNullPaths_AssignsNullProperties()
        {
            // Arrange
            string expectedAcquisitionPath = null;
            string expectedProjectPath = null;

            // Act
            var eventArgs = new BuildCheckAcquisitionEventArgs(expectedAcquisitionPath, expectedProjectPath);

            // Assert
            Assert.Null(eventArgs.AcquisitionPath);
            Assert.Null(eventArgs.ProjectPath);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="BuildCheckResultMessage"/> class.
    /// </summary>
    public class BuildCheckResultMessageTests
    {
        /// <summary>
        /// Tests that the constructor correctly assigns a non-empty message to the RawMessage property.
        /// </summary>
        [Fact]
        public void Ctor_WithValidMessage_SetsRawMessageCorrectly()
        {
            // Arrange
            const string expectedMessage = "Build succeeded";

            // Act
            var resultMessage = new BuildCheckResultMessage(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, resultMessage.RawMessage);
        }

        /// <summary>
        /// Tests that the constructor correctly assigns an empty string message to the RawMessage property.
        /// </summary>
        [Fact]
        public void Ctor_WithEmptyMessage_SetsRawMessageAsEmpty()
        {
            // Arrange
            const string expectedMessage = "";

            // Act
            var resultMessage = new BuildCheckResultMessage(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, resultMessage.RawMessage);
        }

        /// <summary>
        /// Tests that the constructor correctly assigns a null message to the RawMessage property.
        /// </summary>
        [Fact]
        public void Ctor_WithNullMessage_SetsRawMessageAsNull()
        {
            // Arrange
            string expectedMessage = null;

            // Act
            var resultMessage = new BuildCheckResultMessage(expectedMessage);

            // Assert
            Assert.Null(resultMessage.RawMessage);
        }
    }
}
