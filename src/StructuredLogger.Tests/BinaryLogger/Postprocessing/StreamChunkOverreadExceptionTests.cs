using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="StreamChunkOverReadException"/> class.
    /// </summary>
    public class StreamChunkOverReadExceptionTests
    {
        /// <summary>
        /// Tests the default constructor of <see cref="StreamChunkOverReadException"/> to ensure it initializes with default values.
        /// The test verifies that the InnerException property is null and the Message is not null or empty.
        /// </summary>
        [Fact]
        public void DefaultConstructor_WhenCalled_SetsDefaultValues()
        {
            // Act
            var exception = new StreamChunkOverReadException();

            // Assert
            Assert.Null(exception.InnerException);
            Assert.False(string.IsNullOrEmpty(exception.Message), "Expected non-empty default message.");
        }

        /// <summary>
        /// Tests the constructor of <see cref="StreamChunkOverReadException"/> with a message parameter
        /// to ensure that the Message property is set correctly and InnerException remains null.
        /// </summary>
        [Fact]
        public void ConstructorWithMessage_WhenCalled_SetsMessageProperty()
        {
            // Arrange
            string expectedMessage = "Custom exception message.";

            // Act
            var exception = new StreamChunkOverReadException(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
            Assert.Null(exception.InnerException);
        }

        /// <summary>
        /// Tests the constructor of <see cref="StreamChunkOverReadException"/> with both a message and an inner exception,
        /// verifying that both the Message and InnerException properties are correctly assigned.
        /// </summary>
        [Fact]
        public void ConstructorWithMessageAndInner_WhenCalled_SetsMessageAndInnerExceptionProperties()
        {
            // Arrange
            string expectedMessage = "Custom exception message with inner exception.";
            var expectedInner = new Exception("Inner exception");

            // Act
            var exception = new StreamChunkOverReadException(expectedMessage, expectedInner);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
            Assert.Equal(expectedInner, exception.InnerException);
        }
    }
}
