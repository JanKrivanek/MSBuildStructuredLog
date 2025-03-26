using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Property"/> class.
    /// </summary>
//     public class PropertyTests [Error] (22-25)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.TaskParameterProperty' to 'Microsoft.Build.Logging.StructuredLogger.Property'
//     {
//         private readonly Property _property;
// 
//         /// <summary>
//         /// Initializes a new instance of the <see cref="PropertyTests"/> class.
//         /// </summary>
//         public PropertyTests()
//         {
//             // Instantiate a derived type if Property cannot be instantiated directly.
//             // Here, TaskParameterProperty is used as it derives from Property.
//             _property = new TaskParameterProperty();
//         }
// 
//         /// <summary>
//         /// Tests the TypeName property getter to ensure it returns the string "Property".
//         /// </summary>
//         [Fact] [Error] (32-41)CS1061 'Property' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Property' could be found (are you missing a using directive or an assembly reference?)
//         public void TypeName_WhenCalled_ReturnsProperty()
//         {
//             // Act
//             string typeName = _property.TypeName;
// 
//             // Assert
//             Assert.Equal("Property", typeName);
//         }
//     }

    /// <summary>
    /// Unit tests for the <see cref="TaskParameterProperty"/> class.
    /// </summary>
    public class TaskParameterPropertyTests
    {
        private readonly TaskParameterProperty _taskParameterProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskParameterPropertyTests"/> class.
        /// </summary>
        public TaskParameterPropertyTests()
        {
            _taskParameterProperty = new TaskParameterProperty();
        }

        /// <summary>
        /// Tests that the default value of ParameterName is null.
        /// </summary>
        [Fact]
        public void ParameterName_DefaultValue_IsNull()
        {
            // Act
            string parameterName = _taskParameterProperty.ParameterName;

            // Assert
            Assert.Null(parameterName);
        }

        /// <summary>
        /// Tests that setting the ParameterName property retains the assigned value.
        /// </summary>
        [Fact]
        public void ParameterName_SetAndGet_ReturnsSameValue()
        {
            // Arrange
            string expectedValue = "TestParameter";

            // Act
            _taskParameterProperty.ParameterName = expectedValue;
            string actualValue = _taskParameterProperty.ParameterName;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        /// <summary>
        /// Tests that the TypeName property inherited from Property returns "Property".
        /// </summary>
        [Fact]
        public void TypeName_Inherited_ReturnsProperty()
        {
            // Act
            string typeName = _taskParameterProperty.TypeName;

            // Assert
            Assert.Equal("Property", typeName);
        }
    }
}
