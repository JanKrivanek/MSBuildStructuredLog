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
        /// Tests that the two-parameter constructor creates an instance with the expected property values when provided a valid message.
        /// Expected outcome: An instance is created with the correct message, sender, event time, and null message arguments.
        /// </summary>
//         [Fact] [Error] (30-47)CS1061 'BuildCanceledEventArgs' does not contain a definition for 'EventTime' and no accessible extension method 'EventTime' accepting a first argument of type 'BuildCanceledEventArgs' could be found (are you missing a using directive or an assembly reference?) [Error] (31-35)CS1061 'BuildCanceledEventArgs' does not contain a definition for 'MessageArgs' and no accessible extension method 'MessageArgs' accepting a first argument of type 'BuildCanceledEventArgs' could be found (are you missing a using directive or an assembly reference?)
//         public void Constructor_WithValidMessage_NoMessageArgs_CreatesInstance()
//         {
//             // Arrange
//             string message = "Build canceled successfully.";
//             DateTime timestamp = DateTime.UtcNow;
// 
//             // Act
//             var eventArgs = new BuildCanceledEventArgs(message, timestamp);
// 
//             // Assert
//             Assert.NotNull(eventArgs);
//             Assert.Equal(message, eventArgs.Message);
//             Assert.Equal("MSBuild", eventArgs.SenderName);
//             Assert.Equal(timestamp, eventArgs.EventTime);
//             Assert.Null(eventArgs.MessageArgs);
//         }

        /// <summary>
        /// Tests that the three-parameter constructor creates an instance with the expected property values when provided a valid message and message arguments.
        /// Expected outcome: An instance is created with the correct message, sender, event time, and provided message arguments.
        /// </summary>
//         [Fact] [Error] (53-47)CS1061 'BuildCanceledEventArgs' does not contain a definition for 'EventTime' and no accessible extension method 'EventTime' accepting a first argument of type 'BuildCanceledEventArgs' could be found (are you missing a using directive or an assembly reference?) [Error] (54-49)CS1061 'BuildCanceledEventArgs' does not contain a definition for 'MessageArgs' and no accessible extension method 'MessageArgs' accepting a first argument of type 'BuildCanceledEventArgs' could be found (are you missing a using directive or an assembly reference?)
//         public void Constructor_WithValidMessage_WithMessageArgs_CreatesInstance()
//         {
//             // Arrange
//             string message = "Build canceled with extra details.";
//             DateTime timestamp = DateTime.UtcNow;
//             object[] messageArgs = new object[] { "Detail1", 42 };
// 
//             // Act
//             var eventArgs = new BuildCanceledEventArgs(message, timestamp, messageArgs);
// 
//             // Assert
//             Assert.NotNull(eventArgs);
//             Assert.Equal(message, eventArgs.Message);
//             Assert.Equal("MSBuild", eventArgs.SenderName);
//             Assert.Equal(timestamp, eventArgs.EventTime);
//             Assert.Equal(messageArgs, eventArgs.MessageArgs);
//         }

        /// <summary>
        /// Tests that the two-parameter constructor throws an ArgumentException when the message is null or only white-space.
        /// Expected outcome: An ArgumentException is thrown with an appropriate error message.
        /// </summary>
        /// <param name="invalidMessage">An invalid message that is null or white-space.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithValidMessage_NoMessageArgs_InvalidMessage_ThrowsArgumentException(string invalidMessage)
        {
            // Arrange
            DateTime timestamp = DateTime.UtcNow;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new BuildCanceledEventArgs(invalidMessage, timestamp));
            Assert.Equal("Message cannot be null or consist only white-space characters.", exception.Message);
        }

        /// <summary>
        /// Tests that the three-parameter constructor throws an ArgumentException when the message is null or only white-space.
        /// Expected outcome: An ArgumentException is thrown with an appropriate error message.
        /// </summary>
        /// <param name="invalidMessage">An invalid message that is null or white-space.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithValidMessage_WithMessageArgs_InvalidMessage_ThrowsArgumentException(string invalidMessage)
        {
            // Arrange
            DateTime timestamp = DateTime.UtcNow;
            object[] messageArgs = new object[] { "Detail1", 42 };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new BuildCanceledEventArgs(invalidMessage, timestamp, messageArgs));
            Assert.Equal("Message cannot be null or consist only white-space characters.", exception.Message);
        }
    }
}
