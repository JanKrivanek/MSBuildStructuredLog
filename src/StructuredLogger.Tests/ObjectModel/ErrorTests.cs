using FluentAssertions;
using global::Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Error"/> class.
    /// </summary>
    public class ErrorTests
    {
        private readonly Error _error;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorTests"/> class.
        /// </summary>
        public ErrorTests()
        {
            _error = new Error();
        }

        /// <summary>
        /// Tests that the TypeName property getter returns the expected value "Error".
        /// Arrange: An instance of Error is created.
        /// Act: The TypeName property is accessed.
        /// Assert: The returned value should equal "Error".
        /// </summary>
        [Fact]
        public void TypeName_Getter_ShouldReturnError()
        {
            // Act
            string typeName = _error.TypeName;

            // Assert
            typeName.Should().Be("Error", "because the Error class TypeName should be 'Error'");
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="BuildError"/> class.
    /// </summary>
    public class BuildErrorTests
    {
        private readonly BuildError _buildError;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildErrorTests"/> class.
        /// </summary>
        public BuildErrorTests()
        {
            _buildError = new BuildError();
        }

        /// <summary>
        /// Tests that the TypeName property getter returns the expected value "Build Error".
        /// Arrange: An instance of BuildError is created.
        /// Act: The TypeName property is accessed.
        /// Assert: The returned value should equal "Build Error".
        /// </summary>
        [Fact]
        public void TypeName_Getter_ShouldReturnBuildError()
        {
            // Act
            string typeName = _buildError.TypeName;

            // Assert
            typeName.Should().Be("Build Error", "because the BuildError class TypeName should be 'Build Error'");
        }
    }
}
