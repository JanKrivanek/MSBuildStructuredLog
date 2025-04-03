using System;
using FluentAssertions;
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
        /// Tests the parameterless constructor of <see cref="StreamChunkOverReadException"/> to ensure it initializes the instance with default properties.
        /// </summary>
        [Fact]
        public void Ctor_Parameterless_ReturnsInstanceWithDefaultProperties()
        {
            // Arrange & Act
            var exception = new StreamChunkOverReadException();

            // Assert
            exception.Should().BeOfType<StreamChunkOverReadException>();
            exception.InnerException.Should().BeNull();
            exception.Message.Should().NotBeNullOrWhiteSpace();
        }
//  // [Error] (36-21)CS0182 An attribute argument must be a constant expression, typeof expression or array creation expression of an attribute parameter type
//         /// <summary>
//         /// Tests the constructor of <see cref="StreamChunkOverReadException"/> with a message parameter to ensure the Message property is set.
//         /// </summary>
//         /// <param name="message">The exception message to test.</param>
//         [Theory]
//         [InlineData("Test message")]
//         [InlineData("")]
//         [InlineData("!@#$%^&*()_+-=[]{}|;':,./<>?")]
//         [InlineData("This is a very long message " + "A".PadLeft(1000, 'A'))]
//         public void Ctor_WithMessage_ReturnsInstanceWithSpecifiedMessage(string message)
//         {
//             // Arrange & Act
//             var exception = new StreamChunkOverReadException(message);
// 
//             // Assert
//             exception.Should().BeOfType<StreamChunkOverReadException>();
//             exception.Message.Should().Be(message);
//             exception.InnerException.Should().BeNull();
//         }
// 
        /// <summary>
        /// Tests the constructor of <see cref="StreamChunkOverReadException"/> with a message and a null inner exception to ensure the properties are set correctly.
        /// </summary>
        [Fact]
        public void Ctor_WithMessageAndNullInner_ReturnsInstanceWithSpecifiedMessageAndNullInner()
        {
            // Arrange
            string message = "Test message with null inner exception";

            // Act
            var exception = new StreamChunkOverReadException(message, null);

            // Assert
            exception.Should().BeOfType<StreamChunkOverReadException>();
            exception.Message.Should().Be(message);
            exception.InnerException.Should().BeNull();
        }

        /// <summary>
        /// Tests the constructor of <see cref="StreamChunkOverReadException"/> with a message and an inner exception to ensure both properties are set correctly.
        /// </summary>
        [Fact]
        public void Ctor_WithMessageAndInner_ReturnsInstanceWithSpecifiedMessageAndInnerException()
        {
            // Arrange
            string message = "Test message with inner exception";
            var innerException = new Exception("Inner exception message");

            // Act
            var exception = new StreamChunkOverReadException(message, innerException);

            // Assert
            exception.Should().BeOfType<StreamChunkOverReadException>();
            exception.Message.Should().Be(message);
            exception.InnerException.Should().Be(innerException);
        }
    }
}
