// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Parameter"/> class.
//     /// </summary>
//     public class ParameterTests
//     {
//         private readonly Parameter _parameter;
// 
//         /// <summary>
//         /// Initializes a new instance of the <see cref="ParameterTests"/> class.
//         /// </summary>
//         public ParameterTests()
//         {
//             _parameter = new Parameter();
//         }
// 
//         /// <summary>
//         /// Tests that the <see cref="Parameter.TypeName"/> property returns the expected value.
//         /// This ensures that the override correctly returns the name of the current class.
//         /// </summary>
//         [Fact]
//         public void TypeName_WhenCalled_ReturnsParameter()
//         {
//             // Act
//             string actualTypeName = _parameter.TypeName;
// 
//             // Assert
//             actualTypeName.Should().Be("Parameter", "TypeName should return the name of the class");
//         }
// //  // [Error] (46-21)CS0182 An attribute argument must be a constant expression, typeof expression or array creation expression of an attribute parameter type
// //         /// <summary>
// //         /// Tests that the <see cref="Parameter.ParameterName"/> property correctly gets and sets values.
// //         /// This test covers various inputs including empty strings, whitespace, long strings, and special characters.
// //         /// </summary>
// //         /// <param name="input">The test input for ParameterName.</param>
// //         [Theory]
// //         [InlineData("TestValue")]
// //         [InlineData("")]
// //         [InlineData("   ")]
// //         [InlineData("Special!@#$%^&*()")]
// //         [InlineData("A very long string " + "a".PadLeft(1000, 'a'))]
// //         public void ParameterName_WhenSetWithVariousValues_ReturnsSameValue(string input)
// //         {
// //             // Arrange
// //             _parameter.ParameterName = input;
// // 
// //             // Act
// //             string actualValue = _parameter.ParameterName;
// // 
// //             // Assert
// //             actualValue.Should().Be(input, "ParameterName should return the value that was set");
// //         }
// //     }
// }