// using FluentAssertions;
// using Microsoft.Build.Framework;
// using StructuredLogger.BinaryLogger;
// using Xunit;
// 
// namespace StructuredLogger.BinaryLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ExtendedPropertyReassignmentEventArgs"/> class.
//     /// </summary>
//     public class ExtendedPropertyReassignmentEventArgsTests
//     {
//         /// <summary>
//         /// Tests that the constructor correctly assigns all provided parameters including optional ones.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithAllParameters_PropertiesAreSetCorrectly()
//         {
//             // Arrange
//             string expectedPropertyName = "TestProperty";
//             string expectedPreviousValue = "OldValue";
//             string expectedNewValue = "NewValue";
//             string expectedFile = "TestFile.cs";
//             int expectedLine = 15;
//             int expectedColumn = 8;
//             string expectedMessage = "Property reassignment occurred.";
//             string expectedHelpKeyword = "HelpKey";
//             string expectedSenderName = "TestSender";
//             MessageImportance expectedImportance = MessageImportance.High;
// 
//             // Act
//             var eventArgs = new ExtendedPropertyReassignmentEventArgs(
//                 propertyName: expectedPropertyName,
//                 previousValue: expectedPreviousValue,
//                 newValue: expectedNewValue,
//                 file: expectedFile,
//                 line: expectedLine,
//                 column: expectedColumn,
//                 message: expectedMessage,
//                 helpKeyword: expectedHelpKeyword,
//                 senderName: expectedSenderName,
//                 importance: expectedImportance);
// 
//             // Assert
//             eventArgs.PropertyName.Should().Be(expectedPropertyName);
//             eventArgs.PreviousValue.Should().Be(expectedPreviousValue);
//             eventArgs.NewValue.Should().Be(expectedNewValue);
//             eventArgs.File.Should().Be(expectedFile);
//             eventArgs.LineNumber.Should().Be(expectedLine);
//             eventArgs.ColumnNumber.Should().Be(expectedColumn);
//             eventArgs.Message.Should().Be(expectedMessage);
//             eventArgs.HelpKeyword.Should().Be(expectedHelpKeyword);
//             eventArgs.SenderName.Should().Be(expectedSenderName);
//             eventArgs.Importance.Should().Be(expectedImportance);
//         }
// 
//         /// <summary>
//         /// Tests that the constructor correctly assigns default values for optional parameters.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithDefaultOptionalParameters_OptionalPropertiesAreDefaulted()
//         {
//             // Arrange
//             string expectedPropertyName = "DefaultTestProperty";
//             string expectedPreviousValue = "DefaultOld";
//             string expectedNewValue = "DefaultNew";
//             string expectedFile = "DefaultFile.cs";
//             int expectedLine = 0;
//             int expectedColumn = 0;
//             string expectedMessage = "Default message.";
//             // Optional parameters omitted: helpKeyword and senderName, importance default is Low.
//             MessageImportance expectedImportance = MessageImportance.Low;
// 
//             // Act
//             var eventArgs = new ExtendedPropertyReassignmentEventArgs(
//                 propertyName: expectedPropertyName,
//                 previousValue: expectedPreviousValue,
//                 newValue: expectedNewValue,
//                 file: expectedFile,
//                 line: expectedLine,
//                 column: expectedColumn,
//                 message: expectedMessage);
// 
//             // Assert
//             eventArgs.PropertyName.Should().Be(expectedPropertyName);
//             eventArgs.PreviousValue.Should().Be(expectedPreviousValue);
//             eventArgs.NewValue.Should().Be(expectedNewValue);
//             eventArgs.File.Should().Be(expectedFile);
//             eventArgs.LineNumber.Should().Be(expectedLine);
//             eventArgs.ColumnNumber.Should().Be(expectedColumn);
//             eventArgs.Message.Should().Be(expectedMessage);
//             eventArgs.HelpKeyword.Should().BeNull();
//             eventArgs.SenderName.Should().BeNull();
//             eventArgs.Importance.Should().Be(expectedImportance);
//         }
// //  // [Error] (118-13)CS0200 Property or indexer 'BuildMessageEventArgs.File' cannot be assigned to -- it is read only
// //         /// <summary>
// //         /// Tests that properties can be modified after the instance has been created.
// //         /// </summary>
// //         [Fact]
// //         public void Properties_ModifyAfterCreation_ValuesChangeCorrectly()
// //         {
// //             // Arrange
// //             var eventArgs = new ExtendedPropertyReassignmentEventArgs(
// //                 propertyName: "InitialProperty",
// //                 previousValue: "InitialOld",
// //                 newValue: "InitialNew",
// //                 file: "InitialFile.cs",
// //                 line: 1,
// //                 column: 1,
// //                 message: "Initial message");
// //             
// //             // Act
// //             eventArgs.PropertyName = "ModifiedProperty";
// //             eventArgs.PreviousValue = "ModifiedOld";
// //             eventArgs.NewValue = "ModifiedNew";
// //             // Ensure other inherited properties remain unchanged
// //             eventArgs.File = "ModifiedFile.cs";
// //             eventArgs.LineNumber.Should().Be(1);  // Note: LineNumber is set via the base constructor and is read-only in BuildMessageEventArgs.
// //             eventArgs.ColumnNumber.Should().Be(1);  // Similarly, ColumnNumber is set via the base constructor.
// // 
// //             // Assert
// //             eventArgs.PropertyName.Should().Be("ModifiedProperty");
// //             eventArgs.PreviousValue.Should().Be("ModifiedOld");
// //             eventArgs.NewValue.Should().Be("ModifiedNew");
// //             eventArgs.File.Should().Be("ModifiedFile.cs");
// //         }
// //     }
// }