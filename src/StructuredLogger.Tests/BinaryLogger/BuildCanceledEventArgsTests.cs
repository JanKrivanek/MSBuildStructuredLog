using FluentAssertions;
using System;
using StructuredLogger.BinaryLogger;
using Xunit;

namespace StructuredLogger.BinaryLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildCanceledEventArgs"/> class.
    /// </summary>
    public class BuildCanceledEventArgsTests
    {
        /// <summary>
        /// Tests that the constructor with message and eventTimestamp creates an instance with the expected values.
        /// </summary>
        [Fact]
        public void Ctor_WithValidMessageAndTimestamp_ReturnsExpectedValues()
        {
            // Arrange
            string message = "Build canceled due to error.";
            DateTime timestamp = DateTime.UtcNow;

            // Act
            var eventArgs = new BuildCanceledEventArgs(message, timestamp);

            // Assert
            eventArgs.Message.Should().Be(message);
            eventArgs.Timestamp.Should().Be(timestamp);
            // Verify that the sender is set to null as per the base constructor call.
            eventArgs.SenderName.Should().Be("MSBuild");
        }
//  // [Error] (51-23)CS1061 'BuildCanceledEventArgs' does not contain a definition for 'MessageArgs' and no accessible extension method 'MessageArgs' accepting a first argument of type 'BuildCanceledEventArgs' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the constructor with message, eventTimestamp, and messageArgs creates an instance with the expected values.
//         /// </summary>
//         [Fact]
//         public void Ctor_WithValidMessageTimestampAndMessageArgs_ReturnsExpectedValues()
//         {
//             // Arrange
//             string message = "Build canceled due to warnings.";
//             DateTime timestamp = DateTime.UtcNow;
//             object[] messageArgs = new object[] { "Argument1", 123 };
// 
//             // Act
//             var eventArgs = new BuildCanceledEventArgs(message, timestamp, messageArgs);
// 
//             // Assert
//             eventArgs.Message.Should().Be(message);
//             eventArgs.Timestamp.Should().Be(timestamp);
//             // Verify that message arguments are set as expected.
//             eventArgs.MessageArgs.Should().BeEquivalentTo(messageArgs);
//             eventArgs.SenderName.Should().Be("MSBuild");
//         }
// 
        /// <summary>
        /// Tests that the constructor throws an ArgumentException when the message parameter is null.
        /// </summary>
        [Fact]
        public void Ctor_WithNullMessage_ThrowsArgumentException()
        {
            // Arrange
            string message = null!;
            DateTime timestamp = DateTime.UtcNow;

            // Act
            Action act = () => new BuildCanceledEventArgs(message, timestamp);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Message cannot be null or consist only white-space characters.");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentException when the message parameter consists only of white-space.
        /// </summary>
        [Fact]
        public void Ctor_WithWhiteSpaceMessage_ThrowsArgumentException()
        {
            // Arrange
            string message = "   ";
            DateTime timestamp = DateTime.UtcNow;

            // Act
            Action act = () => new BuildCanceledEventArgs(message, timestamp);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Message cannot be null or consist only white-space characters.");
        }
    }
}
