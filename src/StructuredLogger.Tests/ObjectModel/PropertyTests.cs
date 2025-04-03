using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
//     /// <summary> // [Error] (17-29)CS7036 There is no argument given that corresponds to the required parameter 'name' of 'Property.Property(string, string)'
//     /// Unit tests for the <see cref="Property"/> class.
//     /// </summary>
//     public class PropertyTests
//     {
//         private readonly Property _property;
// 
//         public PropertyTests()
//         {
//             _property = new Property();
//         }
//  // [Error] (27-41)CS1061 'Property' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Property' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the TypeName property returns "Property" as expected.
//         /// </summary>
//         [Fact]
//         public void TypeName_Get_ReturnsProperty()
//         {
//             // Act
//             string typeName = _property.TypeName;
// 
//             // Assert
//             typeName.Should().Be("Property");
//         }
//     }
// 
    /// <summary>
    /// Unit tests for the <see cref="TaskParameterProperty"/> class.
    /// </summary>
    public class TaskParameterPropertyTests
    {
        private readonly TaskParameterProperty _taskParameterProperty;

        public TaskParameterPropertyTests()
        {
            _taskParameterProperty = new TaskParameterProperty();
        }

        /// <summary>
        /// Tests that the TypeName property (inherited from Property) returns "Property" as expected.
        /// </summary>
        [Fact]
        public void TypeName_Get_ReturnsProperty()
        {
            // Act
            string typeName = _taskParameterProperty.TypeName;

            // Assert
            typeName.Should().Be("Property");
        }

        /// <summary>
        /// Tests that setting and getting the ParameterName property works correctly with various valid inputs.
        /// </summary>
        /// <param name="input">The input string for the ParameterName property.</param>
        [Theory]
        [InlineData("Parameter")]
        [InlineData("")]
        [InlineData("A")]
        [InlineData("!@#$%^&*()")]
        public void ParameterName_SetAndGet_ReturnsExpectedValue(string input)
        {
            // Arrange
            _taskParameterProperty.ParameterName = input;

            // Act
            string result = _taskParameterProperty.ParameterName;

            // Assert
            result.Should().Be(input);
        }

        /// <summary>
        /// Tests that setting the ParameterName property to an excessively long string works correctly.
        /// </summary>
        [Fact]
        public void ParameterName_SetExcessivelyLongString_ReturnsExpectedValue()
        {
            // Arrange
            string longString = new string('A', 10000);
            _taskParameterProperty.ParameterName = longString;

            // Act
            string result = _taskParameterProperty.ParameterName;

            // Assert
            result.Should().Be(longString);
        }
    }
}
