// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Property"/> class.
//     /// </summary>
//     public class PropertyTests
//     {
// //         /// <summary> // [Error] (22-46)CS1061 'Property' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Property' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests that the TypeName property returns the expected string "Property".
// //         /// </summary>
// //         [Fact]
// //         public void TypeName_WhenAccessed_ReturnsProperty()
// //         {
// //             // Arrange
// //             var property = new Property();
// // 
// //             // Act
// //             string actualTypeName = property.TypeName;
// // 
// //             // Assert
// //             actualTypeName.Should().Be("Property");
// //         }
// //     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="TaskParameterProperty"/> class.
//     /// </summary>
//     public class TaskParameterPropertyTests
//     {
//         /// <summary>
//         /// Tests that the ParameterName property can be set and retrieved correctly.
//         /// </summary>
//         [Fact]
//         public void ParameterName_SetValue_ReturnsSameValue()
//         {
//             // Arrange
//             var taskParameter = new TaskParameterProperty();
//             var expectedValue = "SampleParameter";
// 
//             // Act
//             taskParameter.ParameterName = expectedValue;
//             var actualValue = taskParameter.ParameterName;
// 
//             // Assert
//             actualValue.Should().Be(expectedValue);
//         }
// 
//         /// <summary>
//         /// Tests that the ParameterName property correctly returns an empty string when set.
//         /// </summary>
//         [Fact]
//         public void ParameterName_SetEmptyString_ReturnsEmptyString()
//         {
//             // Arrange
//             var taskParameter = new TaskParameterProperty();
//             var expectedValue = string.Empty;
// 
//             // Act
//             taskParameter.ParameterName = expectedValue;
//             var actualValue = taskParameter.ParameterName;
// 
//             // Assert
//             actualValue.Should().Be(expectedValue);
//         }
// 
//         /// <summary>
//         /// Tests that TaskParameterProperty inherits the TypeName property from Property.
//         /// </summary>
//         [Fact]
//         public void TypeName_Inherited_ReturnsProperty()
//         {
//             // Arrange
//             var taskParameter = new TaskParameterProperty();
// 
//             // Act
//             var actualTypeName = taskParameter.TypeName;
// 
//             // Assert
//             actualTypeName.Should().Be("Property");
//         }
//     }
// }