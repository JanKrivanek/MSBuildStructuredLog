// using System;
// using System.Collections.Generic;
// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Xunit;
// 
// namespace Microsoft.Build.Framework.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ExtendedBuildWarningEventArgs"/> class.
//     /// </summary>
//     public class ExtendedBuildWarningEventArgsTests
//     {
//         /// <summary>
//         /// Tests that the default (internal) constructor sets ExtendedType to "undefined".
//         /// </summary>
//         [Fact]
//         public void DefaultConstructor_ShouldSetExtendedTypeToUndefined()
//         {
//             // Act
//             var eventArgs = new ExtendedBuildWarningEventArgs();
//             
//             // Assert
//             eventArgs.ExtendedType.Should().Be("undefined");
//         }
//         
//         /// <summary>
//         /// Tests that the constructor accepting only the type parameter sets ExtendedType correctly.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithType_ShouldSetExtendedType()
//         {
//             // Arrange
//             string expectedType = "CustomWarning";
//             
//             // Act
//             var eventArgs = new ExtendedBuildWarningEventArgs(expectedType);
//             
//             // Assert
//             eventArgs.ExtendedType.Should().Be(expectedType);
//         }
//         
//         /// <summary>
//         /// Tests that the constructor with base parameters (without timestamp) initializes both its own and base properties properly.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithParameters_ShouldInitializeProperties()
//         {
//             // Arrange
//             string expectedType = "WarningType";
//             string expectedSubcategory = "Sub";
//             string expectedCode = "W123";
//             string expectedFile = "file.cs";
//             int expectedLineNumber = 10;
//             int expectedColumnNumber = 5;
//             int expectedEndLineNumber = 10;
//             int expectedEndColumnNumber = 15;
//             string expectedMessage = "Warning message";
//             string expectedHelpKeyword = "HelpKey";
//             string expectedSenderName = "Sender";
//             
//             // Act
//             var eventArgs = new ExtendedBuildWarningEventArgs(
//                 expectedType,
//                 expectedSubcategory,
//                 expectedCode,
//                 expectedFile,
//                 expectedLineNumber,
//                 expectedColumnNumber,
//                 expectedEndLineNumber,
//                 expectedEndColumnNumber,
//                 expectedMessage,
//                 expectedHelpKeyword,
//                 expectedSenderName);
//             
//             // Assert
//             eventArgs.ExtendedType.Should().Be(expectedType);
//             eventArgs.Subcategory.Should().Be(expectedSubcategory);
//             eventArgs.Code.Should().Be(expectedCode);
//             eventArgs.File.Should().Be(expectedFile);
//             eventArgs.LineNumber.Should().Be(expectedLineNumber);
//             eventArgs.ColumnNumber.Should().Be(expectedColumnNumber);
//             eventArgs.EndLineNumber.Should().Be(expectedEndLineNumber);
//             eventArgs.EndColumnNumber.Should().Be(expectedEndColumnNumber);
//             eventArgs.Message.Should().Be(expectedMessage);
//             eventArgs.HelpKeyword.Should().Be(expectedHelpKeyword);
//             eventArgs.SenderName.Should().Be(expectedSenderName);
//         }
//         
//         /// <summary>
//         /// Tests that the constructor with a timestamp initializes properties including the timestamp.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithTimestamp_ShouldInitializePropertiesIncludingTimestamp()
//         {
//             // Arrange
//             string expectedType = "WarningWithTimestamp";
//             string expectedSubcategory = "Sub";
//             string expectedCode = "W124";
//             string expectedFile = "file2.cs";
//             int expectedLineNumber = 20;
//             int expectedColumnNumber = 10;
//             int expectedEndLineNumber = 20;
//             int expectedEndColumnNumber = 30;
//             string expectedMessage = "Warning message 2";
//             string expectedHelpKeyword = "HelpKeyword2";
//             string expectedSenderName = "Sender2";
//             DateTime expectedTimestamp = new DateTime(2022, 1, 1);
//             
//             // Act
//             var eventArgs = new ExtendedBuildWarningEventArgs(
//                 expectedType,
//                 expectedSubcategory,
//                 expectedCode,
//                 expectedFile,
//                 expectedLineNumber,
//                 expectedColumnNumber,
//                 expectedEndLineNumber,
//                 expectedEndColumnNumber,
//                 expectedMessage,
//                 expectedHelpKeyword,
//                 expectedSenderName,
//                 expectedTimestamp);
//             
//             // Assert
//             eventArgs.ExtendedType.Should().Be(expectedType);
//             eventArgs.Subcategory.Should().Be(expectedSubcategory);
//             eventArgs.Code.Should().Be(expectedCode);
//             eventArgs.File.Should().Be(expectedFile);
//             eventArgs.LineNumber.Should().Be(expectedLineNumber);
//             eventArgs.ColumnNumber.Should().Be(expectedColumnNumber);
//             eventArgs.EndLineNumber.Should().Be(expectedEndLineNumber);
//             eventArgs.EndColumnNumber.Should().Be(expectedEndColumnNumber);
//             eventArgs.Message.Should().Be(expectedMessage);
//             eventArgs.HelpKeyword.Should().Be(expectedHelpKeyword);
//             eventArgs.SenderName.Should().Be(expectedSenderName);
//             eventArgs.Timestamp.Should().Be(expectedTimestamp);
//         }
// //          // [Error] (190-23)CS1061 'ExtendedBuildWarningEventArgs' does not contain a definition for 'MessageArgs' and no accessible extension method 'MessageArgs' accepting a first argument of type 'ExtendedBuildWarningEventArgs' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the constructor with a timestamp and message arguments initializes properties including message arguments.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithTimestampAndMessageArgs_ShouldInitializePropertiesIncludingMessageArgs()
// //         {
// //             // Arrange
// //             string expectedType = "WarningWithArgs";
// //             string expectedSubcategory = "SubArgs";
// //             string expectedCode = "W125";
// //             string expectedFile = "file3.cs";
// //             int expectedLineNumber = 30;
// //             int expectedColumnNumber = 15;
// //             int expectedEndLineNumber = 30;
// //             int expectedEndColumnNumber = 35;
// //             string expectedMessage = "Warning message with args";
// //             string expectedHelpKeyword = "HelpKeyword3";
// //             string expectedSenderName = "Sender3";
// //             DateTime expectedTimestamp = new DateTime(2023, 1, 1);
// //             object[] expectedMessageArgs = new object[] { "arg1", 42 };
// //             
// //             // Act
// //             var eventArgs = new ExtendedBuildWarningEventArgs(
// //                 expectedType,
// //                 expectedSubcategory,
// //                 expectedCode,
// //                 expectedFile,
// //                 expectedLineNumber,
// //                 expectedColumnNumber,
// //                 expectedEndLineNumber,
// //                 expectedEndColumnNumber,
// //                 expectedMessage,
// //                 expectedHelpKeyword,
// //                 expectedSenderName,
// //                 expectedTimestamp,
// //                 expectedMessageArgs);
// //             
// //             // Assert
// //             eventArgs.ExtendedType.Should().Be(expectedType);
// //             eventArgs.Subcategory.Should().Be(expectedSubcategory);
// //             eventArgs.Code.Should().Be(expectedCode);
// //             eventArgs.File.Should().Be(expectedFile);
// //             eventArgs.LineNumber.Should().Be(expectedLineNumber);
// //             eventArgs.ColumnNumber.Should().Be(expectedColumnNumber);
// //             eventArgs.EndLineNumber.Should().Be(expectedEndLineNumber);
// //             eventArgs.EndColumnNumber.Should().Be(expectedEndColumnNumber);
// //             eventArgs.Message.Should().Be(expectedMessage);
// //             eventArgs.HelpKeyword.Should().Be(expectedHelpKeyword);
// //             eventArgs.SenderName.Should().Be(expectedSenderName);
// //             eventArgs.Timestamp.Should().Be(expectedTimestamp);
// //             eventArgs.MessageArgs.Should().BeEquivalentTo(expectedMessageArgs);
// //         }
// //          // [Error] (246-23)CS1061 'ExtendedBuildWarningEventArgs' does not contain a definition for 'MessageArgs' and no accessible extension method 'MessageArgs' accepting a first argument of type 'ExtendedBuildWarningEventArgs' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the constructor with a help link, timestamp, and message arguments initializes all properties correctly.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithTimestampHelpLinkAndMessageArgs_ShouldInitializeAllProperties()
//         {
//             // Arrange
//             string expectedType = "WarningFull";
//             string expectedSubcategory = "SubFull";
//             string expectedCode = "W126";
//             string expectedFile = "file4.cs";
//             int expectedLineNumber = 40;
//             int expectedColumnNumber = 20;
//             int expectedEndLineNumber = 40;
//             int expectedEndColumnNumber = 45;
//             string expectedMessage = "Full warning message";
//             string expectedHelpKeyword = "HelpKeyword4";
//             string expectedSenderName = "Sender4";
//             string expectedHelpLink = "http://help.link";
//             DateTime expectedTimestamp = new DateTime(2024, 1, 1);
//             object[] expectedMessageArgs = new object[] { "fullArg", 100 };
//             
//             // Act
//             var eventArgs = new ExtendedBuildWarningEventArgs(
//                 expectedType,
//                 expectedSubcategory,
//                 expectedCode,
//                 expectedFile,
//                 expectedLineNumber,
//                 expectedColumnNumber,
//                 expectedEndLineNumber,
//                 expectedEndColumnNumber,
//                 expectedMessage,
//                 expectedHelpKeyword,
//                 expectedSenderName,
//                 expectedHelpLink,
//                 expectedTimestamp,
//                 expectedMessageArgs);
//             
//             // Assert
//             eventArgs.ExtendedType.Should().Be(expectedType);
//             eventArgs.Subcategory.Should().Be(expectedSubcategory);
//             eventArgs.Code.Should().Be(expectedCode);
//             eventArgs.File.Should().Be(expectedFile);
//             eventArgs.LineNumber.Should().Be(expectedLineNumber);
//             eventArgs.ColumnNumber.Should().Be(expectedColumnNumber);
//             eventArgs.EndLineNumber.Should().Be(expectedEndLineNumber);
//             eventArgs.EndColumnNumber.Should().Be(expectedEndColumnNumber);
//             eventArgs.Message.Should().Be(expectedMessage);
//             eventArgs.HelpKeyword.Should().Be(expectedHelpKeyword);
//             eventArgs.SenderName.Should().Be(expectedSenderName);
//             eventArgs.HelpLink.Should().Be(expectedHelpLink);
//             eventArgs.Timestamp.Should().Be(expectedTimestamp);
//             eventArgs.MessageArgs.Should().BeEquivalentTo(expectedMessageArgs);
//         }
//         
//         /// <summary>
//         /// Tests that the ExtendedMetadata and ExtendedData properties are null by default.
//         /// </summary>
//         [Fact]
//         public void ExtendedProperties_Defaults_ShouldBeNull()
//         {
//             // Arrange & Act
//             var eventArgs = new ExtendedBuildWarningEventArgs("Test");
//             
//             // Assert
//             eventArgs.ExtendedMetadata.Should().BeNull();
//             eventArgs.ExtendedData.Should().BeNull();
//         }
//     }
// }