using Microsoft.Build.Logging;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref = "TimedMessage"/> class.
    /// </summary>
    public class TimedMessageTests
    {
        /// <summary>
        /// Verifies that setting the Timestamp property correctly reflects the assigned value and that TimestampText returns a non-null string.
        /// </summary>
        [Fact]
        public void Timestamp_GetterAndSetter_ReturnsSetValue_And_TimestampTextIsNotNull()
        {
            // Arrange
            var timedMessage = new TimedMessage();
            var expectedTimestamp = new DateTime(2022, 1, 1, 12, 30, 45);
            // Act
            timedMessage.Timestamp = expectedTimestamp;
            var actualTimestamp = timedMessage.Timestamp;
            var timestampText = timedMessage.TimestampText;
            // Assert
            Assert.Equal(expectedTimestamp, actualTimestamp);
            Assert.NotNull(timestampText);
            // Verify that the textual representation includes the year.
            Assert.Contains(expectedTimestamp.Year.ToString(), timestampText);
        }

        /// <summary>
        /// Verifies that the TypeName property returns the expected value "Message".
        /// </summary>
        [Fact]
        public void TypeName_ReturnsMessage()
        {
            // Arrange
            var timedMessage = new TimedMessage();
            // Act
            var typeName = timedMessage.TypeName;
            // Assert
            Assert.Equal("Message", typeName);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref = "MessageWithLocation"/> class.
    /// </summary>
    public class MessageWithLocationTests
    {
        /// <summary>
        /// Verifies that the SourceFilePath property returns the same value as the FilePath property.
        /// </summary>
        [Fact]
        public void SourceFilePath_ReturnsFilePathProperty()
        {
            // Arrange
            var messageWithLocation = new MessageWithLocation
            {
                FilePath = "testfile.cs"
            };
            // Act
            var sourceFilePath = messageWithLocation.SourceFilePath;
            // Assert
            Assert.Equal("testfile.cs", sourceFilePath);
        }

        /// <summary>
        /// Verifies that the LineNumber property returns the same integer value as the Line property.
        /// </summary>
        [Fact]
        public void LineNumber_ReturnsLineProperty()
        {
            // Arrange
            var messageWithLocation = new MessageWithLocation
            {
                Line = 42
            };
            // Act
            var lineNumber = messageWithLocation.LineNumber;
            // Assert
            Assert.Equal(42, lineNumber);
        }

        /// <summary>
        /// Verifies that the TypeName property returns the expected value "Message".
        /// </summary>
        [Fact]
        public void TypeName_ReturnsMessage()
        {
            // Arrange
            var messageWithLocation = new MessageWithLocation();
            // Act
            var typeName = messageWithLocation.TypeName;
            // Assert
            Assert.Equal("Message", typeName);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref = "Message"/> class.
    /// </summary>
    public class MessageTests
    {
        /// <summary>
        /// A testable subclass of Message which exposes the Text property assumed to be inherited from TextNode.
        /// This facilitates testing of file location and line number extraction logic.
        /// </summary>
        private class TestableMessage : Message
        {
            // Assumes that the base class TextNode defines a public virtual Text property.
            public new string Text { get; set; }
        }

        /// <summary>
        /// Verifies that the default value of Timestamp is DateTime.MinValue.
        /// </summary>
//         [Fact] [Error] (127-37)CS1061 'Message' does not contain a definition for 'Timestamp' and no accessible extension method 'Timestamp' accepting a first argument of type 'Message' could be found (are you missing a using directive or an assembly reference?)
//         public void Timestamp_DefaultValue_IsMinValue()
//         {
//             // Arrange
//             var message = new Message();
//             // Act
//             var timestamp = message.Timestamp;
//             // Assert
//             Assert.Equal(DateTime.MinValue, timestamp);
//         }

        /// <summary>
        /// Verifies that setting the Timestamp property does not modify the default value.
        /// </summary>
//         [Fact] [Error] (142-21)CS1061 'Message' does not contain a definition for 'Timestamp' and no accessible extension method 'Timestamp' accepting a first argument of type 'Message' could be found (are you missing a using directive or an assembly reference?) [Error] (143-45)CS1061 'Message' does not contain a definition for 'Timestamp' and no accessible extension method 'Timestamp' accepting a first argument of type 'Message' could be found (are you missing a using directive or an assembly reference?)
//         public void Timestamp_Set_DoesNothing()
//         {
//             // Arrange
//             var message = new Message();
//             var newTimestamp = new DateTime(2022, 5, 5, 10, 20, 30);
//             // Act
//             message.Timestamp = newTimestamp;
//             var timestampAfterSet = message.Timestamp;
//             // Assert
//             Assert.Equal(DateTime.MinValue, timestampAfterSet);
//         }

        /// <summary>
        /// Verifies that the TypeName property returns the expected value "Message".
        /// </summary>
//         [Fact] [Error] (157-36)CS1061 'Message' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Message' could be found (are you missing a using directive or an assembly reference?)
//         public void TypeName_ReturnsMessage()
//         {
//             // Arrange
//             var message = new Message();
//             // Act
//             var typeName = message.TypeName;
//             // Assert
//             Assert.Equal("Message", typeName);
//         }

        /// <summary>
        /// Verifies that SourceFilePath returns null when no file location information can be extracted from Text.
        /// </summary>
//         [Fact] [Error] (174-50)CS1061 'MessageTests.TestableMessage' does not contain a definition for 'SourceFilePath' and no accessible extension method 'SourceFilePath' accepting a first argument of type 'MessageTests.TestableMessage' could be found (are you missing a using directive or an assembly reference?)
//         public void SourceFilePath_NoMatch_ReturnsNull()
//         {
//             // Arrange
//             var testableMessage = new TestableMessage
//             {
//                 Text = "This is a log message without file location information."
//             };
//             // Act
//             var sourceFilePath = testableMessage.SourceFilePath;
//             // Assert
//             Assert.Null(sourceFilePath);
//         }

        /// <summary>
        /// Verifies that LineNumber returns null when no line number information can be extracted from Text.
        /// </summary>
//         [Fact] [Error] (191-46)CS1061 'MessageTests.TestableMessage' does not contain a definition for 'LineNumber' and no accessible extension method 'LineNumber' accepting a first argument of type 'MessageTests.TestableMessage' could be found (are you missing a using directive or an assembly reference?)
//         public void LineNumber_NoMatch_ReturnsNull()
//         {
//             // Arrange
//             var testableMessage = new TestableMessage
//             {
//                 Text = "Log message without line number info."
//             };
//             // Act
//             var lineNumber = testableMessage.LineNumber;
//             // Assert
//             Assert.Null(lineNumber);
//         }

        /// <summary>
        /// Verifies that the default value of IsLowRelevance is false.
        /// </summary>
//         [Fact] [Error] (205-42)CS1061 'Message' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'Message' could be found (are you missing a using directive or an assembly reference?)
//         public void IsLowRelevance_DefaultIsFalse()
//         {
//             // Arrange
//             var message = new Message();
//             // Act
//             var isLowRelevance = message.IsLowRelevance;
//             // Assert
//             Assert.False(isLowRelevance);
//         }

        /// <summary>
        /// Verifies that setting the IsLowRelevance property does not throw an exception.
        /// Note: The internal flag mechanism is not directly verifiable, so this test only ensures that the setter is callable.
        /// </summary>
//         [Fact] [Error] (220-36)CS0117 'Record' does not contain a definition for 'Exception'
//         public void IsLowRelevance_Setter_DoesNotThrow()
//         {
//             // Arrange
//             var message = new Message();
//             // Act and Assert
//             var exception = Record.Exception(() => message.IsLowRelevance = true);
//             Assert.Null(exception);
//         }
    }
}