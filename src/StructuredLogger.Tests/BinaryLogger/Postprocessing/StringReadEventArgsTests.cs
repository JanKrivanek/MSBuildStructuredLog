// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="StringReadEventArgs"/> class.
//     /// </summary>
//     public class StringReadEventArgsTests
//     {
// //         /// <summary> // [Error] (29-23)CS1061 'StringReadEventArgs' does not contain a definition for 'OriginalString' and no accessible extension method 'OriginalString' accepting a first argument of type 'StringReadEventArgs' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests the constructor of <see cref="StringReadEventArgs"/> to ensure that both OriginalString and StringToBeUsed
// //         /// are set to the provided string value.
// //         /// </summary>
// //         /// <param name="input">The input string to use for initialization.</param>
// //         [Theory]
// //         [InlineData("Test string")]
// //         [InlineData("")]
// //         [InlineData("   ")]
// //         [InlineData("Special!@#$%^&*()")]
// //         public void Constructor_WithValidString_SetsPropertiesCorrectly(string input)
// //         {
// //             // Arrange & Act
// //             var eventArgs = new StringReadEventArgs(input);
// // 
// //             // Assert
// //             eventArgs.OriginalString.Should().Be(input, "the original string should be set during construction");
// //             eventArgs.StringToBeUsed.Should().Be(input, "the string to be used should be initialized to the original string");
// //         }
// //  // [Error] (46-23)CS1061 'StringReadEventArgs' does not contain a definition for 'OriginalString' and no accessible extension method 'OriginalString' accepting a first argument of type 'StringReadEventArgs' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the constructor of <see cref="StringReadEventArgs"/> using an excessively long string to verify initialization.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithExcessivelyLongString_SetsPropertiesCorrectly()
//         {
//             // Arrange
//             var longString = new string('a', 10000);
// 
//             // Act
//             var eventArgs = new StringReadEventArgs(longString);
// 
//             // Assert
//             eventArgs.OriginalString.Should().Be(longString, "the original string should match the provided long string");
//             eventArgs.StringToBeUsed.Should().Be(longString, "the string to be used should be initialized to the original long string");
//         }
// //  // [Error] (67-23)CS1061 'StringReadEventArgs' does not contain a definition for 'Reuse' and no accessible extension method 'Reuse' accepting a first argument of type 'StringReadEventArgs' could be found (are you missing a using directive or an assembly reference?) // [Error] (70-23)CS1061 'StringReadEventArgs' does not contain a definition for 'OriginalString' and no accessible extension method 'OriginalString' accepting a first argument of type 'StringReadEventArgs' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests the internal Reuse method of <see cref="StringReadEventArgs"/> to ensure it updates both
// //         /// OriginalString and StringToBeUsed to the new value.
// //         /// </summary>
// //         /// <param name="newValue">The new string value to reuse.</param>
// //         [Theory]
// //         [InlineData("New value")]
// //         [InlineData("")]
// //         [InlineData("   ")]
// //         [InlineData("Special*&^%$#@!")]
// //         public void Reuse_WhenCalled_UpdatesPropertiesCorrectly(string newValue)
// //         {
// //             // Arrange
// //             var initialValue = "Initial value";
// //             var eventArgs = new StringReadEventArgs(initialValue);
// // 
// //             // Act
// //             eventArgs.Reuse(newValue);
// // 
// //             // Assert
// //             eventArgs.OriginalString.Should().Be(newValue, "the original string should be updated by the Reuse method");
// //             eventArgs.StringToBeUsed.Should().Be(newValue, "the string to be used should be updated by the Reuse method");
// //         }
// //  // [Error] (86-23)CS1061 'StringReadEventArgs' does not contain a definition for 'Reuse' and no accessible extension method 'Reuse' accepting a first argument of type 'StringReadEventArgs' could be found (are you missing a using directive or an assembly reference?) // [Error] (89-23)CS1061 'StringReadEventArgs' does not contain a definition for 'OriginalString' and no accessible extension method 'OriginalString' accepting a first argument of type 'StringReadEventArgs' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the internal Reuse method with an excessively long string to verify that both properties are updated correctly.
//         /// </summary>
//         [Fact]
//         public void Reuse_WithExcessivelyLongString_UpdatesPropertiesCorrectly()
//         {
//             // Arrange
//             var initialValue = "Initial";
//             var longString = new string('z', 10000);
//             var eventArgs = new StringReadEventArgs(initialValue);
// 
//             // Act
//             eventArgs.Reuse(longString);
// 
//             // Assert
//             eventArgs.OriginalString.Should().Be(longString, "the original string should match the excessively long new value");
//             eventArgs.StringToBeUsed.Should().Be(longString, "the string to be used should match the excessively long new value");
//         }
//     }
// }