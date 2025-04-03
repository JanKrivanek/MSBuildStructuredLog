// using FluentAssertions;
// using Microsoft.Build.Framework;
// using System;
// using System.Collections.Generic;
// using Xunit;
// 
// namespace Microsoft.Build.Framework.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ExtendedCustomBuildEventArgs"/> class.
//     /// </summary>
//     public class ExtendedCustomBuildEventArgsTests
//     {
//         /// <summary>
//         /// Tests the default internal constructor to ensure it initializes ExtendedType to "undefined".
//         /// Note: This test assumes the test assembly has access to internal members via InternalsVisibleTo.
//         /// </summary>
//         [Fact]
//         public void DefaultConstructor_WhenCalled_SetsExtendedTypeToUndefined()
//         {
//             // Arrange & Act
//             var instance = new ExtendedCustomBuildEventArgs();
// 
//             // Assert
//             instance.ExtendedType.Should().Be("undefined");
//         }
// 
//         /// <summary>
//         /// Tests the public constructor with a type parameter to ensure ExtendedType is assigned correctly.
//         /// </summary>
//         /// <param name="inputType">The type string provided.</param>
//         [Theory]
//         [InlineData("CustomType")]
//         [InlineData("")]
//         [InlineData("Special!@#$%^&*()")]
//         [InlineData("A very very long string that exceeds normal expectations, used to test edge cases...")]
//         public void Constructor_WithTypeParameter_AssignsExtendedTypeCorrectly(string inputType)
//         {
//             // Arrange & Act
//             var instance = new ExtendedCustomBuildEventArgs(inputType);
// 
//             // Assert
//             instance.ExtendedType.Should().Be(inputType);
//         }
// 
//         /// <summary>
//         /// Tests the constructor with type, message, helpKeyword, and senderName parameters to ensure proper initialization.
//         /// </summary>
//         /// <param name="type">The type string provided.</param>
//         /// <param name="message">The message string provided.</param>
//         /// <param name="helpKeyword">The help keyword provided.</param>
//         /// <param name="senderName">The sender name provided.</param>
//         [Theory]
//         [InlineData("TypeA", "MessageA", "HelpA", "SenderA")]
//         [InlineData("TypeB", "", "", "")]
//         [InlineData("TypeC", null, null, null)]
//         public void Constructor_WithTypeMessageHelpSender_AssignsPropertiesCorrectly(string type, string? message, string? helpKeyword, string? senderName)
//         {
//             // Arrange & Act
//             var instance = new ExtendedCustomBuildEventArgs(type, message, helpKeyword, senderName);
// 
//             // Assert
//             instance.ExtendedType.Should().Be(type);
//             instance.Message.Should().Be(message);
//             instance.HelpKeyword.Should().Be(helpKeyword);
//             instance.SenderName.Should().Be(senderName);
//         }
// //  // [Error] (93-22)CS1061 'ExtendedCustomBuildEventArgs' does not contain a definition for 'EventTimestamp' and no accessible extension method 'EventTimestamp' accepting a first argument of type 'ExtendedCustomBuildEventArgs' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests the constructor with type, message, helpKeyword, senderName, and eventTimestamp parameters.
// //         /// </summary>
// //         /// <param name="type">The type string provided.</param>
// //         /// <param name="message">The message string provided.</param>
// //         /// <param name="helpKeyword">The help keyword provided.</param>
// //         /// <param name="senderName">The sender name provided.</param>
// //         [Theory]
// //         [InlineData("TypeA", "MessageA", "HelpA", "SenderA")]
// //         [InlineData("TypeB", "", "", "")]
// //         [InlineData("TypeC", null, null, null)]
// //         public void Constructor_WithEventTimestamp_AssignsPropertiesCorrectly(string type, string? message, string? helpKeyword, string? senderName)
// //         {
// //             // Arrange
// //             DateTime timestamp = DateTime.UtcNow;
// // 
// //             // Act
// //             var instance = new ExtendedCustomBuildEventArgs(type, message, helpKeyword, senderName, timestamp);
// // 
// //             // Assert
// //             instance.ExtendedType.Should().Be(type);
// //             instance.Message.Should().Be(message);
// //             instance.HelpKeyword.Should().Be(helpKeyword);
// //             instance.SenderName.Should().Be(senderName);
// //             instance.EventTimestamp.Should().Be(timestamp);
// //         }
// //  // [Error] (118-22)CS1061 'ExtendedCustomBuildEventArgs' does not contain a definition for 'EventTimestamp' and no accessible extension method 'EventTimestamp' accepting a first argument of type 'ExtendedCustomBuildEventArgs' could be found (are you missing a using directive or an assembly reference?) // [Error] (119-22)CS1061 'ExtendedCustomBuildEventArgs' does not contain a definition for 'MessageArgs' and no accessible extension method 'MessageArgs' accepting a first argument of type 'ExtendedCustomBuildEventArgs' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the constructor with type, message, helpKeyword, senderName, eventTimestamp, and messageArgs parameters.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithMessageArgs_AssignsPropertiesCorrectly()
//         {
//             // Arrange
//             string type = "TypeWithArgs";
//             string? message = "Test Message";
//             string? helpKeyword = "HelpKey";
//             string? senderName = "TestSender";
//             DateTime timestamp = DateTime.Now;
//             object[] messageArgs = new object[] { 1, "arg", 3.14 };
// 
//             // Act
//             var instance = new ExtendedCustomBuildEventArgs(type, message, helpKeyword, senderName, timestamp, messageArgs);
// 
//             // Assert
//             instance.ExtendedType.Should().Be(type);
//             instance.Message.Should().Be(message);
//             instance.HelpKeyword.Should().Be(helpKeyword);
//             instance.SenderName.Should().Be(senderName);
//             instance.EventTimestamp.Should().Be(timestamp);
//             instance.MessageArgs.Should().BeEquivalentTo(messageArgs);
//         }
// 
//         /// <summary>
//         /// Tests the ExtendedMetadata property getter and setter.
//         /// </summary>
//         [Fact]
//         public void ExtendedMetadata_Property_SetAndGet_ReturnsExpectedDictionary()
//         {
//             // Arrange
//             var instance = new ExtendedCustomBuildEventArgs("TestType");
//             var testDictionary = new Dictionary<string, string>
//             {
//                 { "Key1", "Value1" },
//                 { "Key2", "Value2" }
//             };
// 
//             // Act
//             instance.ExtendedMetadata = testDictionary;
// 
//             // Assert
//             instance.ExtendedMetadata.Should().BeEquivalentTo(testDictionary);
//         }
// 
//         /// <summary>
//         /// Tests the ExtendedData property getter and setter with various string inputs.
//         /// </summary>
//         /// <param name="data">The data string to be set.</param>
//         [Theory]
//         [InlineData("Some data")]
//         [InlineData("")]
//         [InlineData(null)]
//         public void ExtendedData_Property_SetAndGet_ReturnsExpectedValue(string? data)
//         {
//             // Arrange
//             var instance = new ExtendedCustomBuildEventArgs("TestType");
// 
//             // Act
//             instance.ExtendedData = data;
// 
//             // Assert
//             instance.ExtendedData.Should().Be(data);
//         }
//     }
// }