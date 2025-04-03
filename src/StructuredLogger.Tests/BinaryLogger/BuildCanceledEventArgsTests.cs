// using FluentAssertions;
// using System;
// using System.Collections.Generic;
// using StructuredLogger.BinaryLogger;
// using Xunit;
// 
// namespace StructuredLogger.BinaryLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="BuildCanceledEventArgs"/> class.
//     /// </summary>
//     public class BuildCanceledEventArgsTests
//     {
// //         /// <summary> // [Error] (32-36)CS1061 'BuildCanceledEventArgs' does not contain a definition for 'MessageArgs' and no accessible extension method 'MessageArgs' accepting a first argument of type 'BuildCanceledEventArgs' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests that the constructor without message arguments initializes properties correctly with valid inputs.
// //         /// Expected outcome: Instance is created with provided message, timestamp, SenderName set to "MSBuild", and MessageArgs as null.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithValidMessageAndTimestamp_ShouldInitializeProperties()
// //         {
// //             // Arrange
// //             string validMessage = "Build cancelled due to error";
// //             DateTime eventTimestamp = DateTime.Now;
// // 
// //             // Act
// //             var buildCanceledEventArgs = new BuildCanceledEventArgs(validMessage, eventTimestamp);
// // 
// //             // Assert
// //             buildCanceledEventArgs.Message.Should().Be(validMessage);
// //             buildCanceledEventArgs.Timestamp.Should().Be(eventTimestamp);
// //             buildCanceledEventArgs.SenderName.Should().Be("MSBuild");
// //             buildCanceledEventArgs.MessageArgs.Should().BeNull();
// //         }
// //  // [Error] (54-36)CS1061 'BuildCanceledEventArgs' does not contain a definition for 'MessageArgs' and no accessible extension method 'MessageArgs' accepting a first argument of type 'BuildCanceledEventArgs' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the constructor with message arguments initializes properties correctly with valid inputs.
//         /// Expected outcome: Instance is created with provided message, timestamp, SenderName set to "MSBuild", and MessageArgs matching the provided array.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithValidMessageTimestampAndMessageArgs_ShouldInitializeProperties()
//         {
//             // Arrange
//             string validMessage = "Build cancelled with reason";
//             DateTime eventTimestamp = DateTime.UtcNow;
//             object[] messageArgs = new object[] { "arg1", 42 };
// 
//             // Act
//             var buildCanceledEventArgs = new BuildCanceledEventArgs(validMessage, eventTimestamp, messageArgs);
// 
//             // Assert
//             buildCanceledEventArgs.Message.Should().Be(validMessage);
//             buildCanceledEventArgs.Timestamp.Should().Be(eventTimestamp);
//             buildCanceledEventArgs.SenderName.Should().Be("MSBuild");
//             buildCanceledEventArgs.MessageArgs.Should().BeEquivalentTo(messageArgs);
//         }
// 
//         /// <summary>
//         /// Tests that the constructor throws an ArgumentException when the message is null or consists only of white-space characters.
//         /// Expected outcome: An ArgumentException is thrown with the specific message.
//         /// </summary>
//         /// <param name="invalidMessage">A string that is empty or only white-space.</param>
//         [Theory]
//         [InlineData("")]
//         [InlineData(" ")]
//         [InlineData("   ")]
//         [InlineData("\t")]
//         [InlineData("\n")]
//         public void Constructor_WithWhiteSpaceMessage_ShouldThrowArgumentException(string invalidMessage)
//         {
//             // Arrange
//             DateTime eventTimestamp = DateTime.Now;
// 
//             // Act
//             Action act = () => new BuildCanceledEventArgs(invalidMessage, eventTimestamp);
// 
//             // Assert
//             act.Should().Throw<ArgumentException>()
//                 .WithMessage("Message cannot be null or consist only white-space characters.");
//         }
// 
//         /// <summary>
//         /// Tests that the constructor initializes the Timestamp property correctly when using extreme DateTime values.
//         /// Expected outcome: The Timestamp property is set to the provided extreme DateTime value.
//         /// </summary>
//         /// <param name="eventTimestamp">An extreme DateTime value.</param>
//         [Theory]
//         [MemberData(nameof(GetExtremeTimestamps))]
//         public void Constructor_WithExtremeTimestampValues_ShouldInitializeProperties(DateTime eventTimestamp)
//         {
//             // Arrange
//             string validMessage = "Valid message for timestamp edge case";
// 
//             // Act
//             var buildCanceledEventArgs = new BuildCanceledEventArgs(validMessage, eventTimestamp);
// 
//             // Assert
//             buildCanceledEventArgs.Timestamp.Should().Be(eventTimestamp);
//         }
// 
//         /// <summary>
//         /// Provides extreme DateTime values for testing.
//         /// </summary>
//         /// <returns>An enumerable of object arrays containing DateTime values.</returns>
//         public static IEnumerable<object[]> GetExtremeTimestamps()
//         {
//             yield return new object[] { DateTime.MinValue };
//             yield return new object[] { DateTime.MaxValue };
//         }
//     }
// }