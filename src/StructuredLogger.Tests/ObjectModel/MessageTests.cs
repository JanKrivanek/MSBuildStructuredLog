// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "TimedMessage"/> class.
//     /// </summary>
//     public class TimedMessageTests
//     {
//         private readonly TimedMessage _timedMessage;
//         public TimedMessageTests()
//         {
//             _timedMessage = new TimedMessage();
//         }
// 
//         /// <summary>
//         /// Tests that setting and getting the Timestamp property returns the set value.
//         /// </summary>
//         [Fact]
//         public void Timestamp_SetValue_GetReturnsSameValue()
//         {
//             // Arrange
//             var expectedTimestamp = DateTime.Now;
//             // Act
//             _timedMessage.Timestamp = expectedTimestamp;
//             var actualTimestamp = _timedMessage.Timestamp;
//             // Assert
//             actualTimestamp.Should().Be(expectedTimestamp, "because the Timestamp property should return the value it was set to");
//         }
// 
//         /// <summary>
//         /// Tests that the TimestampText property returns the full precision string representation of the Timestamp.
//         /// Assumes that the Display extension method returns the ISO 8601 ("O") formatted string.
//         /// </summary>
//         [Fact]
//         public void TimestampText_WhenTimestampSet_ReturnsFullPrecisionString()
//         {
//             // Arrange
//             var testTimestamp = DateTime.Now;
//             _timedMessage.Timestamp = testTimestamp;
//             var expectedText = testTimestamp.ToString("O");
//             // Act
//             var actualText = _timedMessage.TimestampText;
//             // Assert
//             actualText.Should().Be(expectedText, "because TimestampText should display the Timestamp in full precision, assumed to be ISO 8601 format");
//         }
// 
//         /// <summary>
//         /// Tests that the TypeName property returns 'Message'.
//         /// </summary>
//         [Fact]
//         public void TypeName_ReturnsMessage()
//         {
//             // Act
//             var typeName = _timedMessage.TypeName;
//             // Assert
//             typeName.Should().Be("Message", "because the TypeName is hardcoded to the name 'Message'");
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref = "MessageWithLocation"/> class.
//     /// </summary>
//     public class MessageWithLocationTests
//     {
//         private readonly MessageWithLocation _messageWithLocation;
//         public MessageWithLocationTests()
//         {
//             _messageWithLocation = new MessageWithLocation();
//         }
// 
//         /// <summary>
//         /// Tests that the SourceFilePath property returns the FilePath value.
//         /// </summary>
//         [Fact]
//         public void SourceFilePath_WhenFilePathSet_ReturnsSameFilePath()
//         {
//             // Arrange
//             var expectedFilePath = "C:\\temp\\file.txt";
//             _messageWithLocation.FilePath = expectedFilePath;
//             // Act
//             var actualSourceFilePath = _messageWithLocation.SourceFilePath;
//             // Assert
//             actualSourceFilePath.Should().Be(expectedFilePath, "because SourceFilePath should mirror the FilePath property");
//         }
// 
//         /// <summary>
//         /// Tests that the LineNumber property returns the Line value.
//         /// </summary>
//         [Fact]
//         public void LineNumber_WhenLineSet_ReturnsSameLineNumber()
//         {
//             // Arrange
//             var expectedLine = 123;
//             _messageWithLocation.Line = expectedLine;
//             // Act
//             var actualLineNumber = _messageWithLocation.LineNumber;
//             // Assert
//             actualLineNumber.Should().Be(expectedLine, "because LineNumber should return the value of the Line property");
//         }
// 
//         /// <summary>
//         /// Tests that the TypeName property returns 'Message'.
//         /// </summary>
//         [Fact]
//         public void TypeName_ReturnsMessage()
//         {
//             // Act
//             var typeName = _messageWithLocation.TypeName;
//             // Assert
//             typeName.Should().Be("Message", "because the TypeName is hardcoded to the name 'Message'");
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref = "Message"/> class.
//     /// </summary>
//     public class MessageTests
//     {
//         private readonly Message _message;
//         public MessageTests()
//         {
//             _message = new Message();
//         }
// //  // [Error] (139-22)CS1061 'Message' does not contain a definition for 'Timestamp' and no accessible extension method 'Timestamp' accepting a first argument of type 'Message' could be found (are you missing a using directive or an assembly reference?) // [Error] (140-44)CS1061 'Message' does not contain a definition for 'Timestamp' and no accessible extension method 'Timestamp' accepting a first argument of type 'Message' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the default Timestamp property returns DateTime.MinValue and that setting the Timestamp does not change its value.
// //         /// </summary>
// //         [Fact]
// //         public void Timestamp_DefaultValueAndSetOperation_DoesNotChangeValue()
// //         {
// //             // Arrange
// //             var newTimestamp = DateTime.Now;
// //             // Act
// //             _message.Timestamp = newTimestamp;
// //             var actualTimestamp = _message.Timestamp;
// //             // Assert
// //             actualTimestamp.Should().Be(DateTime.MinValue, "because the base Timestamp setter is empty and does not change the default value");
// //         }
// //  // [Error] (152-37)CS1061 'Message' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Message' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the TypeName property returns 'Message'.
//         /// </summary>
//         [Fact]
//         public void TypeName_ReturnsMessage()
//         {
//             // Act
//             var typeName = _message.TypeName;
//             // Assert
//             typeName.Should().Be("Message", "because the TypeName is hardcoded to 'Message'");
//         }
// 
//         /// <summary>
//         /// Tests that the default value of IsLowRelevance is false.
//         /// </summary>
//         [Fact]
//         public void IsLowRelevance_DefaultValue_IsFalse()
//         {
//             // Act
//             var isLowRelevance = _message.IsLowRelevance;
//             // Assert
//             isLowRelevance.Should().BeFalse("because by default no flags are set, resulting in IsLowRelevance being false");
//         }
//     }
// }