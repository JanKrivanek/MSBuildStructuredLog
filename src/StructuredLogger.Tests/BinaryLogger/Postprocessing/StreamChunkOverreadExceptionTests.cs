using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="StreamChunkOverReadException"/> class.
    /// </summary>
    public class StreamChunkOverReadExceptionTests
    {
        /// <summary>
        /// Tests the default constructor of <see cref="StreamChunkOverReadException"/> to ensure that it
        /// initializes the exception with a default message that contains the class name, and that no inner exception is set.
        /// </summary>
        [Fact]
        public void Ctor_DefaultInstance_ShouldHaveDefaultMessageAndNoInnerException()
        {
            // Act
            var exception = new StreamChunkOverReadException();

            // Assert
            exception.Should().NotBeNull();
            exception.InnerException.Should().BeNull();
            exception.Message.Should().NotBeNullOrWhiteSpace();
            exception.Message.Should().Contain(nameof(StreamChunkOverReadException));
        }

        /// <summary>
        /// Tests the constructor of <see cref="StreamChunkOverReadException"/> that accepts a message parameter.
        /// Ensures that the message is correctly assigned and that no inner exception is set.
        /// </summary>
        [Fact]
        public void Ctor_WithMessage_ShouldSetMessageAndNoInnerException()
        {
            // Arrange
            string expectedMessage = "Test exception message.";

            // Act
            var exception = new StreamChunkOverReadException(expectedMessage);

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(expectedMessage);
            exception.InnerException.Should().BeNull();
        }

        /// <summary>
        /// Tests the constructor of <see cref="StreamChunkOverReadException"/> that accepts both a message and an inner exception.
        /// Ensures that both the message and inner exception are correctly assigned.
        /// </summary>
        [Fact]
        public void Ctor_WithMessageAndInnerException_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            string expectedMessage = "Test exception message with inner exception.";
            var expectedInnerException = new Exception("Inner exception message.");

            // Act
            var exception = new StreamChunkOverReadException(expectedMessage, expectedInnerException);

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(expectedMessage);
            exception.InnerException.Should().Be(expectedInnerException);
        }
    }
}
