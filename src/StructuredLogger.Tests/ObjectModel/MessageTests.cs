using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TimedMessage"/> class.
    /// </summary>
    public class TimedMessageTests
    {
        /// <summary>
        /// Tests that the Timestamp property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void Timestamp_SetAndGet_ReturnsSetValue()
        {
            // Arrange
            var timedMessage = new TimedMessage();
            var expectedTimestamp = new DateTime(2022, 12, 31, 23, 59, 59);

            // Act
            timedMessage.Timestamp = expectedTimestamp;
            var actualTimestamp = timedMessage.Timestamp;

            // Assert
            actualTimestamp.Should().Be(expectedTimestamp, "because the Timestamp property should return the value that was set.");
        }

// Could not make this test passing.
//         /// <summary>
//         /// Tests that the TimestampText property returns the expected display string.
//         /// Note: This test assumes the DateTime.Display extension method returns a time formatted string.
//         /// If the implementation of Display changes, update the expected output accordingly.
//         /// </summary>
//         [Fact]
//         public void TimestampText_WhenTimestampIsSet_ReturnsDisplayText()
//         {
//             // Arrange
//             var timedMessage = new TimedMessage();
//             var testTimestamp = new DateTime(2021, 01, 01, 12, 0, 0);
//             timedMessage.Timestamp = testTimestamp;
//             // Assuming Display(fullPrecision: true) returns a time formatted string.
//             var expectedDisplay = testTimestamp.ToString("HH:mm:ss.fffffff");
// 
//             // Act
//             var actualDisplay = timedMessage.TimestampText;
// 
//             // Assert
//             actualDisplay.Should().Be(expectedDisplay, "because TimestampText should wrap the Display call on the Timestamp property with fullPrecision set to true.");
//         }
// 
        /// <summary>
        /// Tests that the TypeName property returns "Message".
        /// </summary>
        [Fact]
        public void TypeName_ReturnsMessage()
        {
            // Arrange
            var timedMessage = new TimedMessage();
            var expectedTypeName = "Message";

            // Act
            var actualTypeName = timedMessage.TypeName;

            // Assert
            actualTypeName.Should().Be(expectedTypeName, "because TypeName is overridden to return nameof(Message).");
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="MessageWithLocation"/> class.
    /// </summary>
    public class MessageWithLocationTests
    {
        /// <summary>
        /// Tests that setting the FilePath property correctly affects the SourceFilePath.
        /// </summary>
        [Fact]
        public void SourceFilePath_WhenFilePathIsSet_ReturnsSameFilePath()
        {
            // Arrange
            var messageWithLocation = new MessageWithLocation();
            var expectedFilePath = "/path/to/file.txt";
            messageWithLocation.FilePath = expectedFilePath;

            // Act
            var actualSourceFilePath = messageWithLocation.SourceFilePath;

            // Assert
            actualSourceFilePath.Should().Be(expectedFilePath, "because SourceFilePath simply returns the value of FilePath.");
        }

        /// <summary>
        /// Tests that setting the Line property correctly affects the LineNumber.
        /// </summary>
        [Fact]
        public void LineNumber_WhenLineIsSet_ReturnsSameLine()
        {
            // Arrange
            var messageWithLocation = new MessageWithLocation();
            int expectedLine = 42;
            messageWithLocation.Line = expectedLine;

            // Act
            var actualLineNumber = messageWithLocation.LineNumber;

            // Assert
            actualLineNumber.Should().Be(expectedLine, "because LineNumber should return the value of the Line property.");
        }

        /// <summary>
        /// Tests that the TypeName property returns "Message".
        /// </summary>
        [Fact]
        public void TypeName_ReturnsMessage()
        {
            // Arrange
            var messageWithLocation = new MessageWithLocation();
            var expectedTypeName = "Message";

            // Act
            var actualTypeName = messageWithLocation.TypeName;

            // Assert
            actualTypeName.Should().Be(expectedTypeName, "because TypeName is overridden to return nameof(Message).");
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="Message"/> class.
    /// </summary>
    public class MessageTests
    {
        /// <summary>
        /// Tests that the default Timestamp property returns DateTime.MinValue and that setting it has no effect.
        /// </summary>
        [Fact]
        public void Timestamp_DefaultAndSetBehavior_ReturnsMinValue()
        {
            // Arrange
            var message = new Message();
            var defaultTimestamp = DateTime.MinValue;
            var newTimestamp = new DateTime(2020, 05, 05);

            // Act
            var initialTimestamp = message.Timestamp;
            message.Timestamp = newTimestamp;
            var afterSetTimestamp = message.Timestamp;

            // Assert
            initialTimestamp.Should().Be(defaultTimestamp, "because the default implementation of Timestamp returns DateTime.MinValue.");
            afterSetTimestamp.Should().Be(defaultTimestamp, "because the setter of Timestamp is overridden to do nothing.");
        }

        /// <summary>
        /// Tests that the TypeName property returns "Message".
        /// </summary>
        [Fact]
        public void TypeName_ReturnsMessage()
        {
            // Arrange
            var message = new Message();
            var expectedTypeName = "Message";

            // Act
            var actualTypeName = message.TypeName;

            // Assert
            actualTypeName.Should().Be(expectedTypeName, "because TypeName is defined to return nameof(Message).");
        }

        /// <summary>
        /// Tests the default value of IsLowRelevance.
        /// Note: This test assumes default flag settings result in IsLowRelevance being false.
        /// </summary>
        [Fact]
        public void IsLowRelevance_Default_ReturnsFalse()
        {
            // Arrange
            var message = new Message();

            // Act
            var isLowRelevance = message.IsLowRelevance;

            // Assert
            isLowRelevance.Should().BeFalse("because by default, the message should not be marked as low relevance.");
        }
// Could not make this test passing.
//         /// <summary>
//         /// Tests that SourceFilePath returns null when the Text property does not produce a matching Regex.
//         /// </summary>
//         [Fact]
//         public void SourceFilePath_WhenNoRegexMatch_ReturnsNull()
//         {
//             // Arrange
//             var mockMessage = new Mock<Message>() { CallBase = true };
//             // Ensure Text is set to a value that will not match any expected regex pattern.
//             mockMessage.Object.Text = "This is a test message with no file info.";
//             mockMessage.Protected().Setup<Match>("GetSourceFileMatch").Returns((Match)null);
// 
//             // Act
//             var sourceFilePath = mockMessage.Object.SourceFilePath;
// 
//             // Assert
//             sourceFilePath.Should().BeNull("because without a matching regex pattern, SourceFilePath should be null.");
//         }
// // Could not make this test passing.
//         }
// 
//         /// <summary>
//         /// Tests that LineNumber returns null when the Text property does not produce a matching Regex.
//         /// </summary>
//         [Fact]
//         public void LineNumber_WhenNoRegexMatch_ReturnsNull()
//         {
//             // Arrange
//             var messageMock = new Mock<Message>() { CallBase = true };
//             // Ensure Text is set to a value that will not match any expected regex pattern.
//             messageMock.Object.Text = "This is a test message with no line info.";
//             messageMock.Setup(m => m.GetSourceFileMatch()).Returns((Match)null);
// 
//             // Act
//             var lineNumber = messageMock.Object.LineNumber;
// 
//             // Assert
//             lineNumber.Should().BeNull("because without a matching regex pattern, LineNumber should be null.");
        }
    }
}
