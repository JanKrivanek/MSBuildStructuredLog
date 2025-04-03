using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.IO;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="PathUtils"/> class.
    /// </summary>
    public class PathUtilsTests
    {
        #region HasInvalidVolumeSeparator tests

        /// <summary>
        /// Tests HasInvalidVolumeSeparator with a valid drive path that should return false.
        /// </summary>
        [Theory]
        [InlineData("C:\\folder")]
        [InlineData("D:\\folder\\subfolder")]
        [InlineData("  E:\\folder")] // Leading spaces but valid drive after spaces.
        public void HasInvalidVolumeSeparator_ValidDrivePath_ReturnsFalse(string path)
        {
            // Act
            bool result = PathUtils.HasInvalidVolumeSeparator(path);

            // Assert
            result.Should().BeFalse("because the drive letter and volume separator are valid");
        }

        /// <summary>
        /// Tests HasInvalidVolumeSeparator with a path that starts with a colon, which should be invalid.
        /// </summary>
        [Theory]
        [InlineData(":folder")]
        [InlineData("   :folder")]
        public void HasInvalidVolumeSeparator_PathStartingWithColon_ReturnsTrue(string path)
        {
            // Act
            bool result = PathUtils.HasInvalidVolumeSeparator(path);

            // Assert
            result.Should().BeTrue("because the path starts with a colon, which is invalid");
        }

        /// <summary>
        /// Tests HasInvalidVolumeSeparator with a path having an invalid drive letter followed by a volume separator, which should be invalid.
        /// </summary>
        [Theory]
        [InlineData("1:\\folder")]
        [InlineData("  9:\\folder")]
        public void HasInvalidVolumeSeparator_InvalidDriveLetter_ReturnsTrue(string path)
        {
            // Act
            bool result = PathUtils.HasInvalidVolumeSeparator(path);

            // Assert
            result.Should().BeTrue("because the drive letter is not an alphabetical character");
        }

        /// <summary>
        /// Tests HasInvalidVolumeSeparator with a valid drive but extra colon characters later in the path, which should be invalid.
        /// </summary>
        [Theory]
        [InlineData("C:\\folder:extra")]
        [InlineData("  C:\\folder:more:info")]
        public void HasInvalidVolumeSeparator_ExtraColonBeyondDrive_ReturnsTrue(string path)
        {
            // Act
            bool result = PathUtils.HasInvalidVolumeSeparator(path);

            // Assert
            result.Should().BeTrue("because having additional colons beyond the valid drive specifier is invalid");
        }

        /// <summary>
        /// Tests HasInvalidVolumeSeparator with an extended path that is valid and should return false.
        /// </summary>
        [Theory]
        [InlineData(@"\\?\C:\folder")]
        [InlineData(@"\\?\D:\folder\subfolder")]
        public void HasInvalidVolumeSeparator_ExtendedValidPath_ReturnsFalse(string path)
        {
            // Act
            bool result = PathUtils.HasInvalidVolumeSeparator(path);

            // Assert
            result.Should().BeFalse("because extended paths with valid drive letters are allowed");
        }

        /// <summary>
        /// Tests HasInvalidVolumeSeparator with an extended path that has an invalid volume (e.g. starting with colon after prefix), which should return true.
        /// </summary>
        [Theory]
        [InlineData(@"\\?\:folder")]
        public void HasInvalidVolumeSeparator_ExtendedPathWithInvalidVolume_ReturnsTrue(string path)
        {
            // Act
            bool result = PathUtils.HasInvalidVolumeSeparator(path);

            // Assert
            result.Should().BeTrue("because an extended path with a colon immediately after the prefix is invalid");
        }

        #endregion

        #region IsExtended tests

        /// <summary>
        /// Tests IsExtended with a properly formatted extended path, which should return true.
        /// </summary>
        [Theory]
        [InlineData(@"\\?\C:\folder")]
        [InlineData(@"\\?\D:\folder\subfolder")]
        public void IsExtended_ExtendedPath_ReturnsTrue(string path)
        {
            // Act
            bool result = PathUtils.IsExtended(path);

            // Assert
            result.Should().BeTrue("because the path follows the extended path format");
        }

        /// <summary>
        /// Tests IsExtended with a non-extended path format, which should return false.
        /// </summary>
        [Theory]
        [InlineData("C:\\folder")]
        [InlineData("D:/folder/subfolder")]
        public void IsExtended_NonExtendedPath_ReturnsFalse(string path)
        {
            // Act
            bool result = PathUtils.IsExtended(path);

            // Assert
            result.Should().BeFalse("because the path does not have the extended path prefix");
        }

        /// <summary>
        /// Tests IsExtended with a string shorter than the required length, which should return false.
        /// </summary>
        [Fact]
        public void IsExtended_ShortString_ReturnsFalse()
        {
            // Arrange
            string path = "abc";

            // Act
            bool result = PathUtils.IsExtended(path);

            // Assert
            result.Should().BeFalse("because a short string cannot be an extended path");
        }

        #endregion

        #region IsDirectorySeparator tests

        /// <summary>
        /// Tests IsDirectorySeparator with the default directory separator character ('\\'), which should return true.
        /// </summary>
        [Fact]
        public void IsDirectorySeparator_BackSlash_ReturnsTrue()
        {
            // Arrange
            char separator = Path.DirectorySeparatorChar; // typically '\'

            // Act
            bool result = PathUtils.IsDirectorySeparator(separator);

            // Assert
            result.Should().BeTrue("because the backslash is a valid directory separator");
        }

        /// <summary>
        /// Tests IsDirectorySeparator with the alternative directory separator character ('/'), which should return true.
        /// </summary>
        [Fact]
        public void IsDirectorySeparator_Slash_ReturnsTrue()
        {
            // Arrange
            char separator = Path.AltDirectorySeparatorChar; // typically '/'

            // Act
            bool result = PathUtils.IsDirectorySeparator(separator);

            // Assert
            result.Should().BeTrue("because the forward slash is a valid directory separator");
        }

        /// <summary>
        /// Tests IsDirectorySeparator with a character that is not a directory separator, which should return false.
        /// </summary>
        [Theory]
        [InlineData('A')]
        [InlineData('1')]
        [InlineData('-')]
        public void IsDirectorySeparator_NonSeparator_ReturnsFalse(char input)
        {
            // Act
            bool result = PathUtils.IsDirectorySeparator(input);

            // Assert
            result.Should().BeFalse("because the character is not a directory separator");
        }

        #endregion

        #region IsValidDriveChar tests

        /// <summary>
        /// Tests IsValidDriveChar with alphabetical characters, which should return true.
        /// </summary>
        [Theory]
        [InlineData('A')]
        [InlineData('Z')]
        [InlineData('a')]
        [InlineData('m')]
        public void IsValidDriveChar_Alphabet_ReturnsTrue(char drive)
        {
            // Act
            bool result = PathUtils.IsValidDriveChar(drive);

            // Assert
            result.Should().BeTrue("because the character is an alphabetical letter representing a valid drive");
        }

        /// <summary>
        /// Tests IsValidDriveChar with non-alphabetical characters, which should return false.
        /// </summary>
        [Theory]
        [InlineData('1')]
        [InlineData('@')]
        [InlineData(' ')]
        public void IsValidDriveChar_NonAlphabet_ReturnsFalse(char drive)
        {
            // Act
            bool result = PathUtils.IsValidDriveChar(drive);

            // Assert
            result.Should().BeFalse("because the character is not an alphabetical letter");
        }

        #endregion

        #region PathStartSkip tests

        /// <summary>
        /// Tests PathStartSkip with a path that has leading spaces followed by a valid drive letter and volume separator, which should return the count of leading spaces.
        /// </summary>
        [Fact]
        public void PathStartSkip_LeadingSpacesAndDrive_ReturnsLeadingSpacesCount()
        {
            // Arrange
            string path = "   C:\\folder";
            int expected = 3;

            // Act
            int result = PathUtils.PathStartSkip(path);

            // Assert
            result.Should().Be(expected, "because the method should return the number of leading spaces when followed by a valid drive specification");
        }

        /// <summary>
        /// Tests PathStartSkip with a path that has leading spaces followed by a directory separator, which should return the count of leading spaces.
        /// </summary>
        [Fact]
        public void PathStartSkip_LeadingSpacesAndSeparator_ReturnsLeadingSpacesCount()
        {
            // Arrange
            string path = "   \\folder";
            int expected = 3;

            // Act
            int result = PathUtils.PathStartSkip(path);

            // Assert
            result.Should().Be(expected, "because the method should return the number of leading spaces when followed by a directory separator");
        }

        /// <summary>
        /// Tests PathStartSkip with a path that has no leading spaces, which should return zero.
        /// </summary>
        [Theory]
        [InlineData("C:\\folder")]
        [InlineData("\\folder")]
        public void PathStartSkip_NoLeadingSpaces_ReturnsZero(string path)
        {
            // Act
            int result = PathUtils.PathStartSkip(path);

            // Assert
            result.Should().Be(0, "because there are no leading spaces to skip");
        }

        /// <summary>
        /// Tests PathStartSkip with a path composed only of spaces, which should return zero.
        /// </summary>
        [Fact]
        public void PathStartSkip_OnlySpaces_ReturnsZero()
        {
            // Arrange
            string path = "     ";

            // Act
            int result = PathUtils.PathStartSkip(path);

            // Assert
            result.Should().Be(0, "because even though all characters are spaces, no valid drive or directory separator follows to trigger the skip");
        }

        /// <summary>
        /// Tests PathStartSkip with an empty string, which should return zero.
        /// </summary>
        [Fact]
        public void PathStartSkip_EmptyString_ReturnsZero()
        {
            // Arrange
            string path = string.Empty;

            // Act
            int result = PathUtils.PathStartSkip(path);

            // Assert
            result.Should().Be(0, "because an empty string has no characters to skip");
        }

        #endregion
    }
}
