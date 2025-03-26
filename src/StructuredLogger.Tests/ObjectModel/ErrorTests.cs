using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Error"/> class.
    /// </summary>
    public class ErrorTests
    {
        /// <summary>
        /// Tests that the <see cref="Error.TypeName"/> property returns "Error".
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsError()
        {
            // Arrange
            var errorInstance = new Error();

            // Act
            string result = errorInstance.TypeName;

            // Assert
            Assert.Equal("Error", result);
        }

        /// <summary>
        /// Tests that the <see cref="Error.TypeName"/> property does not return null or empty.
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_IsNeitherNullNorEmpty()
        {
            // Arrange
            var errorInstance = new Error();

            // Act
            string result = errorInstance.TypeName;

            // Assert
            Assert.False(string.IsNullOrEmpty(result));
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="BuildError"/> class.
    /// </summary>
    public class BuildErrorTests
    {
        /// <summary>
        /// Tests that the <see cref="BuildError.TypeName"/> property returns "Build Error".
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsBuildError()
        {
            // Arrange
            var buildErrorInstance = new BuildError();

            // Act
            string result = buildErrorInstance.TypeName;

            // Assert
            Assert.Equal("Build Error", result);
        }

        /// <summary>
        /// Tests that the <see cref="BuildError.TypeName"/> property does not return null or empty.
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_IsNeitherNullNorEmpty()
        {
            // Arrange
            var buildErrorInstance = new BuildError();

            // Act
            string result = buildErrorInstance.TypeName;

            // Assert
            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}
