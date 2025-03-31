using System;
using System.IO;
using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="PathUtils"/> class.
    /// </summary>
    public class PathUtilsTests
    {
        /// <summary>
        /// Tests that the RootPath field is correctly computed and contains expected segments.
        /// </summary>
        [Fact]
        public void RootPath_Field_ShouldContainMSBuildStructuredLogFolder()
        {
            // Arrange
            // RootPath is computed based on temp or local app data folder,
            // but must always combine with "Microsoft" and "MSBuildStructuredLog".
            var expectedFolderSegment = Path.Combine("Microsoft", "MSBuildStructuredLog");

            // Act
            string rootPath = PathUtils.RootPath;

            // Assert
            rootPath.Should().NotBeNullOrEmpty();
            rootPath.Should().Contain(expectedFolderSegment);
        }

        /// <summary>
        /// Tests that the TempPath field equals the RootPath combined with "Temp".
        /// </summary>
        [Fact]
        public void TempPath_Field_ShouldBeRootPathCombinedWithTemp()
        {
            // Arrange
            string expectedTempPath = Path.Combine(PathUtils.RootPath, "Temp");

            // Act
            string tempPath = PathUtils.TempPath;

            // Assert
            tempPath.Should().Be(expectedTempPath);
        }

        /// <summary>
        /// Tests the <see cref="PathUtils.IsExtended(string)"/> method returns true for valid extended paths.
        /// </summary>
        [Fact]
        public void IsExtended_WithProperExtendedPath_ReturnsTrue()
        {
            // Arrange
            string extendedPath = @"\\?\C:\";

            // Act
            bool isExtended = PathUtils.IsExtended(extendedPath);

            // Assert
            isExtended.Should().BeTrue();
        }

        /// <summary>
        /// Tests the <see cref="PathUtils.IsExtended(string)"/> method returns false for non-extended paths.
        /// </summary>
        [Theory]
        [InlineData("C:\\")]
        [InlineData(@"\\.\C:\")]
        [InlineData("JustSomeRandomPath")]
        [InlineData(@"//?/C:/")]
        [InlineData(@"\\?C:\")] // missing one backslash after ?
        public void IsExtended_WithNonExtendedPath_ReturnsFalse(string path)
        {
            // Act
            bool isExtended = PathUtils.IsExtended(path);

            // Assert
            isExtended.Should().BeFalse();
        }

        /// <summary>
        /// Tests the <see cref="PathUtils.HasInvalidVolumeSeparator(string)"/> method with valid drive paths.
        /// </summary>
        [Theory]
        [InlineData(@"C:\Folder")]
        [InlineData(@"D:\")]
        [InlineData("   C:\\Folder")] // leading spaces but valid drive specifier.
        [InlineData(@"\\?\C:\Folder")]
        public void HasInvalidVolumeSeparator_WithValidPath_ReturnsFalse(string path)
        {
            // Act
            bool result = PathUtils.HasInvalidVolumeSeparator(path);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Tests the <see cref="PathUtils.HasInvalidVolumeSeparator(string)"/> method returns true when the path
        /// starts with a colon or has invalid drive letter specification.
        /// </summary>
        [Theory]
        [InlineData(":C")]
        [InlineData("1:\\Folder")]
        [InlineData(@"C::\Folder")]
        [InlineData(@"\\?\C::\Folder")]
        public void HasInvalidVolumeSeparator_WithInvalidVolumeSeparator_ReturnsTrue(string path)
        {
            // Act
            bool result = PathUtils.HasInvalidVolumeSeparator(path);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests the <see cref="PathUtils.IsDirectorySeparator(char)"/> method returns true for directory separator characters.
        /// </summary>
        [Theory]
        [InlineData('\\')]
        [InlineData('/')]
        public void IsDirectorySeparator_WithDirectorySeparatorChars_ReturnsTrue(char separator)
        {
            // Act
            bool result = PathUtils.IsDirectorySeparator(separator);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests the <see cref="PathUtils.IsDirectorySeparator(char)"/> method returns false for non-separator characters.
        /// </summary>
        [Theory]
        [InlineData('a')]
        [InlineData(' ')]
        [InlineData(':')]
        public void IsDirectorySeparator_WithNonSeparatorChars_ReturnsFalse(char testChar)
        {
            // Act
            bool result = PathUtils.IsDirectorySeparator(testChar);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Tests the <see cref="PathUtils.IsValidDriveChar(char)"/> method returns true for valid drive letters.
        /// </summary>
        [Theory]
        [InlineData('C')]
        [InlineData('Z')]
        [InlineData('a')]
        [InlineData('m')]
        public void IsValidDriveChar_WithValidDriveLetters_ReturnsTrue(char driveChar)
        {
            // Act
            bool result = PathUtils.IsValidDriveChar(driveChar);

            // Assert
            result.Should().BeTrue();
        }

        /// <summary>
        /// Tests the <see cref="PathUtils.IsValidDriveChar(char)"/> method returns false for invalid drive characters.
        /// </summary>
        [Theory]
        [InlineData('1')]
        [InlineData(':')]
        [InlineData(' ')]
        public void IsValidDriveChar_WithInvalidDriveLetters_ReturnsFalse(char driveChar)
        {
            // Act
            bool result = PathUtils.IsValidDriveChar(driveChar);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Tests the <see cref="PathUtils.PathStartSkip(string)"/> method returns 0 when no valid leading spaces or valid specifiers are found.
        /// </summary>
        [Fact]
        public void PathStartSkip_WithNoLeadingSpaces_ReturnsZero()
        {
            // Arrange
            string path = "Folder\\SubFolder";

            // Act
            int skipCount = PathUtils.PathStartSkip(path);

            // Assert
            skipCount.Should().Be(0);
        }

        /// <summary>
        /// Tests the <see cref="PathUtils.PathStartSkip(string)"/> method returns the correct count when leading spaces are present
        /// and the path represents a valid drive or starts with a directory separator.
        /// </summary>
        [Theory]
        [InlineData("   C:\\Folder", 3)]
        [InlineData("  \\Folder", 2)]
        public void PathStartSkip_WithLeadingSpacesAndValidSpecifier_ReturnsSkipCount(string path, int expectedSkip)
        {
            // Act
            int skipCount = PathUtils.PathStartSkip(path);

            // Assert
            skipCount.Should().Be(expectedSkip);
        }

        /// <summary>
        /// Tests the <see cref="PathUtils.PathStartSkip(string)"/> method returns 0 when leading spaces are present
        /// but the path does not represent a valid drive specifier or directory path.
        /// </summary>
        [Fact]
        public void PathStartSkip_WithLeadingSpacesButInvalidSpecifier_ReturnsZero()
        {
            // Arrange
            string path = "   folderWithoutDriveSpecifier";

            // Act
            int skipCount = PathUtils.PathStartSkip(path);

            // Assert
            skipCount.Should().Be(0);
        }
    }
}
