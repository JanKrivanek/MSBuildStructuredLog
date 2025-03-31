using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
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
        /// Tests that the TypeName property always returns "Parameter".
        /// </summary>
        [Fact]
        public void TypeName_WhenAccessed_ReturnsParameter()
        {
            // Act
            string actualTypeName = _parameter.TypeName;

            // Assert
            actualTypeName.Should().Be(nameof(Parameter));
        }

        /// <summary>
        /// Tests that the default value of ParameterName is null.
        /// </summary>
        [Fact]
        public void ParameterName_DefaultValue_IsNull()
        {
            // Act
            string? actualParameterName = _parameter.ParameterName;

            // Assert
            actualParameterName.Should().BeNull();
        }

        /// <summary>
        /// Tests that setting the ParameterName property to a valid non-empty string returns the same string.
        /// </summary>
        [Fact]
        public void ParameterName_WhenSetToValidValue_ReturnsSameValue()
        {
            // Arrange
            string expectedValue = "TestParameter";

            // Act
            _parameter.ParameterName = expectedValue;
            string actualValue = _parameter.ParameterName;

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests that setting the ParameterName property to an empty string returns an empty string.
        /// </summary>
        [Fact]
        public void ParameterName_WhenSetToEmptyString_ReturnsEmptyString()
        {
            // Arrange
            string expectedValue = string.Empty;

            // Act
            _parameter.ParameterName = expectedValue;
            string actualValue = _parameter.ParameterName;

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests that setting the ParameterName property to a whitespace string returns the same whitespace string.
        /// </summary>
        [Fact]
        public void ParameterName_WhenSetToWhitespace_ReturnsWhitespace()
        {
            // Arrange
            string expectedValue = "   ";

            // Act
            _parameter.ParameterName = expectedValue;
            string actualValue = _parameter.ParameterName;

            // Assert
            actualValue.Should().Be(expectedValue);
        }
    }
}
