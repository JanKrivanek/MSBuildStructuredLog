// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging;
// using System;
// using System.Collections.Generic;
// using Xunit;
// 
// namespace Microsoft.Build.Framework.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "ExtendedCriticalBuildMessageEventArgs"/> class.
//     /// </summary>
//     public class ExtendedCriticalBuildMessageEventArgsTests
//     {
//         /// <summary>
//         /// Tests that the simple constructor (taking only the extended type) correctly sets the ExtendedType property.
//         /// </summary>
//         [Fact]
//         public void Ctor_WithTypeOnly_SetsExtendedType()
//         {
//             // Arrange
//             string expectedType = "CustomType";
//             // Act
//             var eventArgs = new ExtendedCriticalBuildMessageEventArgs(expectedType);
//             // Assert
//             eventArgs.ExtendedType.Should().Be(expectedType);
//             eventArgs.ExtendedMetadata.Should().BeNull();
//             eventArgs.ExtendedData.Should().BeNull();
//         }
// 
//         /// <summary>
//         /// Tests that the constructor with all parameters (including timestamp and message arguments) sets ExtendedType properly.
//         /// </summary>
//         [Fact]
//         public void Ctor_WithParametersIncludingMessageArgs_SetsExtendedType()
//         {
//             // Arrange
//             string expectedType = "ExtendedTypeValue";
//             string? subcategory = "SubCat";
//             string? code = "Code123";
//             string? file = "file.txt";
//             int lineNumber = 10;
//             int columnNumber = 5;
//             int endLineNumber = 10;
//             int endColumnNumber = 20;
//             string? message = "Test message";
//             string? helpKeyword = "HelpKey";
//             string? senderName = "Sender";
//             DateTime customTimestamp = new DateTime(2023, 1, 1);
//             object[] messageArgs = new object[]
//             {
//                 "arg1",
//                 2
//             };
//             // Act
//             var eventArgs = new ExtendedCriticalBuildMessageEventArgs(expectedType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, customTimestamp, messageArgs);
//             // Assert
//             eventArgs.ExtendedType.Should().Be(expectedType);
//         }
// 
//         /// <summary>
//         /// Tests that the constructor with parameters including a timestamp (and no message arguments) sets ExtendedType properly.
//         /// </summary>
//         [Fact]
//         public void Ctor_WithParametersAndTimestamp_SetsExtendedType()
//         {
//             // Arrange
//             string expectedType = "TimestampType";
//             string? subcategory = null;
//             string? code = null;
//             string? file = null;
//             int lineNumber = 0;
//             int columnNumber = 0;
//             int endLineNumber = 0;
//             int endColumnNumber = 0;
//             string? message = null;
//             string? helpKeyword = null;
//             string? senderName = null;
//             DateTime customTimestamp = DateTime.UtcNow;
//             // Act
//             var eventArgs = new ExtendedCriticalBuildMessageEventArgs(expectedType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, customTimestamp);
//             // Assert
//             eventArgs.ExtendedType.Should().Be(expectedType);
//         }
// //  // [Error] (95-23)CS1061 'ExtendedCriticalBuildMessageEventArgsTests.ExtendedCriticalBuildMessageEventArgs_InternalWrapper' does not contain a definition for 'ExtendedType' and no accessible extension method 'ExtendedType' accepting a first argument of type 'ExtendedCriticalBuildMessageEventArgsTests.ExtendedCriticalBuildMessageEventArgs_InternalWrapper' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the internal parameterless constructor sets the ExtendedType to "undefined" as expected.
// //         /// </summary>
// //         [Fact]
// //         public void InternalParameterlessCtor_SetsExtendedTypeToUndefined()
// //         {
// //             // Act
// //             var eventArgs = new ExtendedCriticalBuildMessageEventArgs_InternalWrapper();
// //             // Assert
// //             eventArgs.ExtendedType.Should().Be("undefined");
// //         }
// // 
//         /// <summary>
//         /// Tests that ExtendedMetadata and ExtendedData properties can be set and retrieved.
//         /// </summary>
//         [Fact]
//         public void ExtendedProperties_CanBeSetAndRetrieved()
//         {
//             // Arrange
//             var eventArgs = new ExtendedCriticalBuildMessageEventArgs("TestType");
//             var metadata = new Dictionary<string, string?>
//             {
//                 {
//                     "Key1",
//                     "Value1"
//                 },
//                 {
//                     "Key2",
//                     null
//                 }
//             };
//             string extendedDataValue = "Additional Data";
//             // Act
//             eventArgs.ExtendedMetadata = metadata;
//             eventArgs.ExtendedData = extendedDataValue;
//             // Assert
//             eventArgs.ExtendedMetadata.Should().BeEquivalentTo(metadata);
//             eventArgs.ExtendedData.Should().Be(extendedDataValue);
//         }
// //  // [Error] (129-79)CS0509 'ExtendedCriticalBuildMessageEventArgsTests.ExtendedCriticalBuildMessageEventArgs_InternalWrapper': cannot derive from sealed type 'ExtendedCriticalBuildMessageEventArgs'
// //         /// <summary>
// //         /// A wrapper class to expose the internal parameterless constructor for testing.
// //         /// </summary>
// //         private class ExtendedCriticalBuildMessageEventArgs_InternalWrapper : ExtendedCriticalBuildMessageEventArgs
// //         {
// //             public ExtendedCriticalBuildMessageEventArgs_InternalWrapper() : base()
// //             {
// //             }
// //         }
// //     }
// }