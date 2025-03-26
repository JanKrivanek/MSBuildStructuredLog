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
        /// Tests that the two-parameter constructor initializes the properties correctly when given valid inputs.
        /// </summary>
        [Fact]
        public void Constructor_TwoParameters_ValidMessageAndTimestamp_ShouldInitializeProperties()
        {
            // Arrange
            string validMessage = "Build canceled due to error.";
            DateTime eventTimestamp = DateTime.Now;
            
            // Act
            BuildCanceledEventArgs eventArgs = new BuildCanceledEventArgs(validMessage, eventTimestamp);
            
            // Assert
            Assert.Equal(validMessage, eventArgs.Message);
            Assert.Equal(eventTimestamp, eventArgs.Timestamp);
            Assert.Equal("MSBuild", eventArgs.SenderName);
            Assert.Null(eventArgs.MessageArgs);
        }

        /// <summary>
        /// Tests that the three-parameter constructor initializes the properties correctly when given valid inputs.
        /// </summary>
        [Fact]
        public void Constructor_ThreeParameters_ValidMessageTimestampAndMessageArgs_ShouldInitializeProperties()
        {
            // Arrange
            string validMessage = "Build canceled by user.";
            DateTime eventTimestamp = DateTime.Now;
            object[] messageArgs = new object[] { "arg1", 123 };

            // Act
            BuildCanceledEventArgs eventArgs = new BuildCanceledEventArgs(validMessage, eventTimestamp, messageArgs);
            
            // Assert
            Assert.Equal(validMessage, eventArgs.Message);
            Assert.Equal(eventTimestamp, eventArgs.Timestamp);
            Assert.Equal("MSBuild", eventArgs.SenderName);
            Assert.Equal(messageArgs, eventArgs.MessageArgs);
        }

        /// <summary>
        /// Tests that the two-parameter constructor throws an ArgumentException when the message is null.
        /// </summary>
        [Fact]
        public void Constructor_TwoParameters_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string? invalidMessage = null;
            DateTime eventTimestamp = DateTime.Now;

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => new BuildCanceledEventArgs(invalidMessage!, eventTimestamp));
            
            Assert.Equal("Message cannot be null or consist only white-space characters.", exception.Message);
        }
        
        /// <summary>
        /// Tests that the two-parameter constructor throws an ArgumentException when the message is empty or white-space.
        /// </summary>
        /// <param name="invalidMessage">An invalid message string.</param>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_TwoParameters_InvalidMessage_ThrowsArgumentException(string invalidMessage)
        {
            // Arrange
            DateTime eventTimestamp = DateTime.Now;
            
            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => new BuildCanceledEventArgs(invalidMessage, eventTimestamp));
            
            Assert.Equal("Message cannot be null or consist only white-space characters.", exception.Message);
        }

        /// <summary>
        /// Tests that the three-parameter constructor throws an ArgumentException when the message is null.
        /// </summary>
        [Fact]
        public void Constructor_ThreeParameters_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string? invalidMessage = null;
            DateTime eventTimestamp = DateTime.Now;
            object[] messageArgs = new object[] { "extra" };

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => new BuildCanceledEventArgs(invalidMessage!, eventTimestamp, messageArgs));
            
            Assert.Equal("Message cannot be null or consist only white-space characters.", exception.Message);
        }
        
        /// <summary>
        /// Tests that the three-parameter constructor throws an ArgumentException when the message is empty or white-space.
        /// </summary>
        /// <param name="invalidMessage">An invalid message string.</param>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_ThreeParameters_InvalidMessage_ThrowsArgumentException(string invalidMessage)
        {
            // Arrange
            DateTime eventTimestamp = DateTime.Now;
            object[] messageArgs = new object[] { "data" };
            
            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => new BuildCanceledEventArgs(invalidMessage, eventTimestamp, messageArgs));
            
            Assert.Equal("Message cannot be null or consist only white-space characters.", exception.Message);
        }
    }
}
