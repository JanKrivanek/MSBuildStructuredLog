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
        /// <summary>
        /// Tests that the ToString method returns the Name when both Version and VersionSpec are null or empty.
        /// Expected outcome is that only the Name is present in the resulting string.
        /// </summary>
        [Fact]
        public void ToString_WhenOnlyNameProvided_ReturnsName()
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
            result.Should().Be("TestPackage");
        }

        /// <summary>
        /// Tests that the ToString method returns the correct string when Version is provided and VersionSpec is empty.
        /// Expected outcome is a concatenation of Name and Version.
        /// </summary>
        [Fact]
        public void ToString_WhenVersionProvided_ReturnsNameAndVersion()
        {
            // Arrange
            var package = new Package
            {
                Name = "TestPackage",
                Version = "1.0.0",
                VersionSpec = string.Empty
            };

            // Act
            string result = package.ToString();

            // Assert
            result.Should().Be("TestPackage 1.0.0");
        }

        /// <summary>
        /// Tests that the ToString method returns the correct string when VersionSpec is provided and Version is empty.
        /// Expected outcome is a concatenation of Name and VersionSpec.
        /// </summary>
        [Fact]
        public void ToString_WhenVersionSpecProvided_ReturnsNameAndVersionSpec()
        {
            // Arrange
            var package = new Package
            {
                Name = "TestPackage",
                Version = string.Empty,
                VersionSpec = "v>=1.0.0"
            };

            // Act
            string result = package.ToString();

            // Assert
            result.Should().Be("TestPackage v>=1.0.0");
        }

        /// <summary>
        /// Tests that the ToString method returns the correct string when both Version and VersionSpec are provided.
        /// Expected outcome is a concatenation of Name, Version, and VersionSpec in that order.
        /// </summary>
        [Fact]
        public void ToString_WhenBothVersionAndVersionSpecProvided_ReturnsNameVersionAndVersionSpec()
        {
            // Arrange
            var package = new Package
            {
                Name = "TestPackage",
                Version = "1.0.0",
                VersionSpec = "v>=1.0.0"
            };

            // Act
            string result = package.ToString();

            // Assert
            result.Should().Be("TestPackage 1.0.0 v>=1.0.0");
        }

        /// <summary>
        /// Tests that the ToString method correctly handles special characters in Name, Version, and VersionSpec.
        /// Expected outcome is a proper concatenation preserving all special characters.
        /// </summary>
        [Fact]
        public void ToString_WithSpecialCharacters_ReturnsProperConcatenation()
        {
            // Arrange
            var package = new Package
            {
                Name = "Name@#!",
                Version = "Ver$1",
                VersionSpec = "Spec*&"
            };

            // Act
            string result = package.ToString();

            // Assert
            result.Should().Be("Name@#! Ver$1 Spec*&");
        }

        /// <summary>
        /// Tests that the Version property can be get and set properly.
        /// Expected outcome is that the value set is accurately retrieved.
        /// </summary>
        [Fact]
        public void Version_Property_GetAndSet_Works()
        {
            // Arrange
            var package = new Package();
            string expectedVersion = "2.3.4";

            // Act
            package.Version = expectedVersion;
            string actualVersion = package.Version;

            // Assert
            actualVersion.Should().Be(expectedVersion);
        }

        /// <summary>
        /// Tests that the VersionSpec property can be get and set properly.
        /// Expected outcome is that the value set is accurately retrieved.
        /// </summary>
        [Fact]
        public void VersionSpec_Property_GetAndSet_Works()
        {
            // Arrange
            var package = new Package();
            string expectedVersionSpec = ">=2.3.4";

            // Act
            package.VersionSpec = expectedVersionSpec;
            string actualVersionSpec = package.VersionSpec;

            // Assert
            actualVersionSpec.Should().Be(expectedVersionSpec);
        }

        /// <summary>
        /// Tests that the TypeName property returns the correct type name for the Package class.
        /// Expected outcome is that TypeName equals "Package".
        /// </summary>
        [Fact]
        public void TypeName_Property_ReturnsCorrectValue()
        {
            // Arrange
            var package = new Package();

            // Act
            string typeName = package.TypeName;

            // Assert
            typeName.Should().Be(nameof(Package));
        }
    }
}
