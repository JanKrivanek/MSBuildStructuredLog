// using System;
// using System.Collections.Generic;
// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Xunit;
// 
// namespace Microsoft.Build.Framework.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ExtendedBuildMessageEventArgs"/> class.
//     /// </summary>
//     public class ExtendedBuildMessageEventArgsTests
//     {
//         /// <summary>
//         /// Tests that the default (internal) constructor sets ExtendedType to "undefined" and leaves other extended properties as null.
//         /// </summary>
//         [Fact]
//         public void DefaultConstructor_ShouldSetExtendedTypeToUndefined()
//         {
//             // Arrange & Act
//             var eventArgs = new ExtendedBuildMessageEventArgs();
// 
//             // Assert
//             eventArgs.ExtendedType.Should().Be("undefined");
//             eventArgs.ExtendedMetadata.Should().BeNull();
//             eventArgs.ExtendedData.Should().BeNull();
//         }
// 
//         /// <summary>
//         /// Tests that the constructor with a type parameter correctly sets the ExtendedType.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithType_SetsExtendedType()
//         {
//             // Arrange
//             string expectedType = "CustomType";
// 
//             // Act
//             var eventArgs = new ExtendedBuildMessageEventArgs(expectedType);
// 
//             // Assert
//             eventArgs.ExtendedType.Should().Be(expectedType);
//         }
// 
//         /// <summary>
//         /// Tests that the constructor with basic message properties sets ExtendedType and base properties correctly.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithBasicProperties_SetsPropertiesProperly()
//         {
//             // Arrange
//             string expectedType = "BasicType";
//             string expectedMessage = "Test message";
//             string expectedHelpKeyword = "HelpKey";
//             string expectedSenderName = "TestSender";
//             MessageImportance expectedImportance = MessageImportance.High;
// 
//             // Act
//             var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, expectedMessage, expectedHelpKeyword, expectedSenderName, expectedImportance);
// 
//             // Assert
//             eventArgs.ExtendedType.Should().Be(expectedType);
//             eventArgs.Message.Should().Be(expectedMessage);
//             eventArgs.HelpKeyword.Should().Be(expectedHelpKeyword);
//             eventArgs.SenderName.Should().Be(expectedSenderName);
//             eventArgs.Importance.Should().Be(expectedImportance);
//         }
// //  // [Error] (92-23)CS1061 'ExtendedBuildMessageEventArgs' does not contain a definition for 'EventTimestamp' and no accessible extension method 'EventTimestamp' accepting a first argument of type 'ExtendedBuildMessageEventArgs' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the constructor with a timestamp sets the EventTimestamp along with other properties.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithTimestamp_SetsPropertiesProperly()
// //         {
// //             // Arrange
// //             string expectedType = "TimestampType";
// //             string expectedMessage = "Timestamp message";
// //             string expectedHelpKeyword = "TimestampHelp";
// //             string expectedSenderName = "TimestampSender";
// //             MessageImportance expectedImportance = MessageImportance.Low;
// //             DateTime expectedTimestamp = DateTime.UtcNow;
// // 
// //             // Act
// //             var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, expectedMessage, expectedHelpKeyword, expectedSenderName, expectedImportance, expectedTimestamp);
// // 
// //             // Assert
// //             eventArgs.ExtendedType.Should().Be(expectedType);
// //             eventArgs.Message.Should().Be(expectedMessage);
// //             eventArgs.HelpKeyword.Should().Be(expectedHelpKeyword);
// //             eventArgs.SenderName.Should().Be(expectedSenderName);
// //             eventArgs.Importance.Should().Be(expectedImportance);
// //             eventArgs.EventTimestamp.Should().Be(expectedTimestamp);
// //         }
// //  // [Error] (127-23)CS1061 'ExtendedBuildMessageEventArgs' does not contain a definition for 'EventTimestamp' and no accessible extension method 'EventTimestamp' accepting a first argument of type 'ExtendedBuildMessageEventArgs' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the constructor with message arguments sets ExtendedType correctly.
//         /// Note: Detailed verification of messageArgs is assumed to be handled by the base class.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithMessageArgs_SetsExtendedTypeProperly()
//         {
//             // Arrange
//             string expectedType = "ArgsType";
//             string expectedMessage = "Message with args";
//             string expectedHelpKeyword = "ArgsHelp";
//             string expectedSenderName = "ArgsSender";
//             MessageImportance expectedImportance = MessageImportance.Normal;
//             DateTime expectedTimestamp = DateTime.UtcNow;
//             object[] messageArgs = { "arg1", 2, null };
// 
//             // Act
//             var eventArgs = new ExtendedBuildMessageEventArgs(
//                 expectedType,
//                 expectedMessage,
//                 expectedHelpKeyword,
//                 expectedSenderName,
//                 expectedImportance,
//                 expectedTimestamp,
//                 messageArgs);
// 
//             // Assert
//             eventArgs.ExtendedType.Should().Be(expectedType);
//             eventArgs.Message.Should().Be(expectedMessage);
//             eventArgs.HelpKeyword.Should().Be(expectedHelpKeyword);
//             eventArgs.SenderName.Should().Be(expectedSenderName);
//             eventArgs.Importance.Should().Be(expectedImportance);
//             eventArgs.EventTimestamp.Should().Be(expectedTimestamp);
//         }
// 
//         /// <summary>
//         /// Tests that the ExtendedMetadata and ExtendedData properties can be set and retrieved.
//         /// </summary>
//         [Fact]
//         public void ExtendedProperties_GetSet_WorksCorrectly()
//         {
//             // Arrange
//             var eventArgs = new ExtendedBuildMessageEventArgs("PropertyTest");
//             var metadata = new Dictionary<string, string?> { { "Key1", "Value1" }, { "Key2", null } };
//             string extendedData = "Some extended data";
// 
//             // Act
//             eventArgs.ExtendedMetadata = metadata;
//             eventArgs.ExtendedData = extendedData;
// 
//             // Assert
//             eventArgs.ExtendedMetadata.Should().BeEquivalentTo(metadata);
//             eventArgs.ExtendedData.Should().Be(extendedData);
//         }
// 
//         /// <summary>
//         /// Tests that the constructor with file and location parameters sets all properties from the base and extended class.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithFileParameters_SetsPropertiesProperly()
//         {
//             // Arrange
//             string expectedType = "FileType";
//             string expectedSubcategory = "Subcat";
//             string expectedCode = "001";
//             string expectedFile = "file.cs";
//             int expectedLineNumber = 10;
//             int expectedColumnNumber = 5;
//             int expectedEndLineNumber = 10;
//             int expectedEndColumnNumber = 15;
//             string expectedMessage = "File message";
//             string expectedHelpKeyword = "FileHelp";
//             string expectedSenderName = "FileSender";
//             MessageImportance expectedImportance = MessageImportance.High;
// 
//             // Act
//             var eventArgs = new ExtendedBuildMessageEventArgs(
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
//                 expectedImportance);
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
//             eventArgs.Importance.Should().Be(expectedImportance);
//         }
// //  // [Error] (250-23)CS1061 'ExtendedBuildMessageEventArgs' does not contain a definition for 'EventTimestamp' and no accessible extension method 'EventTimestamp' accepting a first argument of type 'ExtendedBuildMessageEventArgs' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the constructor with file parameters and a timestamp sets all corresponding properties.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithFileParametersAndTimestamp_SetsPropertiesProperly()
// //         {
// //             // Arrange
// //             string expectedType = "FileTimeType";
// //             string expectedSubcategory = "SubcatTime";
// //             string expectedCode = "002";
// //             string expectedFile = "fileTime.cs";
// //             int expectedLineNumber = 20;
// //             int expectedColumnNumber = 8;
// //             int expectedEndLineNumber = 20;
// //             int expectedEndColumnNumber = 18;
// //             string expectedMessage = "File time message";
// //             string expectedHelpKeyword = "FileTimeHelp";
// //             string expectedSenderName = "FileTimeSender";
// //             MessageImportance expectedImportance = MessageImportance.Low;
// //             DateTime expectedTimestamp = DateTime.UtcNow;
// // 
// //             // Act
// //             var eventArgs = new ExtendedBuildMessageEventArgs(
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
// //                 expectedImportance,
// //                 expectedTimestamp);
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
// //             eventArgs.Importance.Should().Be(expectedImportance);
// //             eventArgs.EventTimestamp.Should().Be(expectedTimestamp);
// //         }
// //  // [Error] (305-23)CS1061 'ExtendedBuildMessageEventArgs' does not contain a definition for 'EventTimestamp' and no accessible extension method 'EventTimestamp' accepting a first argument of type 'ExtendedBuildMessageEventArgs' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the constructor with file parameters, timestamp, and message arguments sets all properties appropriately.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithFileParametersTimestampAndMessageArgs_SetsPropertiesProperly()
//         {
//             // Arrange
//             string expectedType = "FullType";
//             string expectedSubcategory = "FullSubcat";
//             string expectedCode = "003";
//             string expectedFile = "full.cs";
//             int expectedLineNumber = 30;
//             int expectedColumnNumber = 12;
//             int expectedEndLineNumber = 30;
//             int expectedEndColumnNumber = 22;
//             string expectedMessage = "Full message";
//             string expectedHelpKeyword = "FullHelp";
//             string expectedSenderName = "FullSender";
//             MessageImportance expectedImportance = MessageImportance.Normal;
//             DateTime expectedTimestamp = DateTime.UtcNow;
//             object[] messageArgs = { "param1", 42 };
// 
//             // Act
//             var eventArgs = new ExtendedBuildMessageEventArgs(
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
//                 expectedImportance,
//                 expectedTimestamp,
//                 messageArgs);
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
//             eventArgs.Importance.Should().Be(expectedImportance);
//             eventArgs.EventTimestamp.Should().Be(expectedTimestamp);
//         }
//     }
// }