using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Parameter"/> class.
    /// </summary>
    public class ParameterTests
    {
        private readonly Parameter _parameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterTests"/> class.
        /// </summary>
        public ParameterTests()
        {
            _parameter = new Parameter();
        }

        /// <summary>
        /// Tests that the <see cref="Parameter.TypeName"/> property returns "Parameter".
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsParameter()
        {
            // Act
            string typeName = _parameter.TypeName;

            // Assert
            Assert.Equal("Parameter", typeName);
        }

        /// <summary>
        /// Tests that setting and getting a non-null value to the <see cref="Parameter.ParameterName"/> property returns the same value.
        /// </summary>
        /// <param name="testValue">A string value to assign to ParameterName.</param>
        [Theory]
        [InlineData("TestValue")]
        [InlineData("")]
        [InlineData(" ")]
        public void ParameterName_SetAndGet_WithValidValue_ReturnsSameValue(string testValue)
        {
            // Arrange
            _parameter.ParameterName = testValue;

            // Act
            string actualValue = _parameter.ParameterName;

            // Assert
            Assert.Equal(testValue, actualValue);
        }

        /// <summary>
        /// Tests that setting the <see cref="Parameter.ParameterName"/> property to null returns null.
        /// </summary>
        [Fact]
        public void ParameterName_SetToNull_ReturnsNull()
        {
            // Arrange
            _parameter.ParameterName = null;

            // Act
            string actualValue = _parameter.ParameterName;

            // Assert
            Assert.Null(actualValue);
        }
    }
}
