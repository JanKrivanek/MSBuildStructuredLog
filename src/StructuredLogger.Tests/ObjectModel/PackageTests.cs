using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Package"/> class.
    /// </summary>
    public class PackageTests
    {
        private readonly Package _package;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageTests"/> class.
        /// </summary>
        public PackageTests()
        {
            // Initialize the package instance for testing.
            _package = new Package();
        }

        /// <summary>
        /// Tests that the TypeName property returns "Package".
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsPackage()
        {
            // Act
            string typeName = _package.TypeName;

            // Assert
            typeName.Should().Be("Package");
        }

        /// <summary>
        /// Tests that ToString returns the Name when both Version and VersionSpec are not set.
        /// </summary>
        [Fact]
        public void ToString_WhenNoVersionOrVersionSpec_ReturnsNameOnly()
        {
            // Arrange
            _package.Name = "TestPackage";
            _package.Version = string.Empty;
            _package.VersionSpec = string.Empty;
            string expected = "TestPackage";

            // Act
            string result = _package.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that ToString returns the Name and Version when only Version is set.
        /// </summary>
        [Fact]
        public void ToString_WhenOnlyVersionProvided_ReturnsNameAndVersion()
        {
            // Arrange
            _package.Name = "TestPackage";
            _package.Version = "1.0.0";
            _package.VersionSpec = string.Empty;
            string expected = "TestPackage 1.0.0";

            // Act
            string result = _package.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that ToString returns the Name and VersionSpec when only VersionSpec is set.
        /// </summary>
        [Fact]
        public void ToString_WhenOnlyVersionSpecProvided_ReturnsNameAndVersionSpec()
        {
            // Arrange
            _package.Name = "TestPackage";
            _package.Version = string.Empty;
            _package.VersionSpec = ">=1.0.0";
            string expected = "TestPackage >=1.0.0";

            // Act
            string result = _package.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that ToString returns the Name, Version, and VersionSpec when both are provided.
        /// </summary>
        [Fact]
        public void ToString_WhenBothVersionAndVersionSpecProvided_ReturnsNameVersionAndVersionSpec()
        {
            // Arrange
            _package.Name = "TestPackage";
            _package.Version = "1.0.0";
            _package.VersionSpec = ">=1.0.0";
            string expected = "TestPackage 1.0.0 >=1.0.0";

            // Act
            string result = _package.ToString();

            // Assert
            result.Should().Be(expected);
        }
    }
}
