using System;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Package"/> class.
    /// </summary>
    public class PackageTests
    {
        /// <summary>
        /// Tests that the TypeName property returns "Package".
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsPackage()
        {
            // Arrange
            var package = new Package();

            // Act
            string typeName = package.TypeName;

            // Assert
            Assert.Equal("Package", typeName);
        }

        /// <summary>
        /// Tests that ToString returns only the Name when Version and VersionSpec are null.
        /// </summary>
        [Fact]
        public void ToString_WhenOnlyNameIsSet_ReturnsName()
        {
            // Arrange
            var package = new Package
            {
                // Assuming Name is inherited from NamedNode and accessible.
                Name = "TestPackage",
                Version = null,
                VersionSpec = null
            };

            // Act
            string result = package.ToString();

            // Assert
            Assert.Equal("TestPackage", result);
        }

        /// <summary>
        /// Tests that ToString returns the concatenated Name and Version when Version is set and VersionSpec is null.
        /// </summary>
        [Fact]
        public void ToString_WhenNameAndVersionAreSet_ReturnsNameAndVersion()
        {
            // Arrange
            var package = new Package
            {
                Name = "TestPackage",
                Version = "1.0.0",
                VersionSpec = null
            };

            // Act
            string result = package.ToString();

            // Assert
            Assert.Equal("TestPackage 1.0.0", result);
        }

        /// <summary>
        /// Tests that ToString returns the concatenated Name and VersionSpec when Version is null and VersionSpec is set.
        /// </summary>
        [Fact]
        public void ToString_WhenNameAndVersionSpecAreSet_ReturnsNameAndVersionSpec()
        {
            // Arrange
            var package = new Package
            {
                Name = "TestPackage",
                Version = null,
                VersionSpec = ">=1.0.0"
            };

            // Act
            string result = package.ToString();

            // Assert
            Assert.Equal("TestPackage >=1.0.0", result);
        }

        /// <summary>
        /// Tests that ToString returns the concatenated string of Name, Version, and VersionSpec when both Version and VersionSpec are set.
        /// </summary>
        [Fact]
        public void ToString_WhenAllPropertiesAreSet_ReturnsConcatenatedString()
        {
            // Arrange
            var package = new Package
            {
                Name = "TestPackage",
                Version = "1.0.0",
                VersionSpec = ">=1.0.0"
            };

            // Act
            string result = package.ToString();

            // Assert
            Assert.Equal("TestPackage 1.0.0 >=1.0.0", result);
        }

        /// <summary>
        /// Tests that ToString returns only the Name when Version and VersionSpec are empty strings.
        /// </summary>
        [Fact]
        public void ToString_WhenVersionAndVersionSpecAreEmpty_ReturnsNameOnly()
        {
            // Arrange
            var package = new Package
            {
                Name = "TestPackage",
                Version = string.Empty,
                VersionSpec = string.Empty
            };

            // Act
            string result = package.ToString();

            // Assert
            Assert.Equal("TestPackage", result);
        }

        /// <summary>
        /// Tests that ToString behaves as expected when Name is null.
        /// In C#, interpolating a null string results in an empty string.
        /// </summary>
        [Fact]
        public void ToString_WhenNameIsNull_ReturnsConcatenatedStringWithEmptyName()
        {
            // Arrange
            var package = new Package
            {
                Name = null,
                Version = "1.0.0",
                VersionSpec = ">=1.0.0"
            };

            // Act
            string result = package.ToString();

            // Assert
            // When Name is null, $"{Name}" produces an empty string.
            // Thus the expected result will have a leading space.
            Assert.Equal(" 1.0.0 >=1.0.0", result);
        }
    }
}
