using System;
using System.IO;
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
        /// Tests that the RootPath property is computed correctly and contains the expected sub-path.
        /// </summary>
        [Fact]
        public void RootPath_WhenAccessed_ContainsExpectedSubPath()
        {
            // Arrange
            string rootPath = PathUtils.RootPath;
            
            // Act
            // Verify that the computed root path contains "Microsoft" and "MSBuildStructuredLog"
            bool containsMicrosoft = rootPath.Contains(Path.Combine("Microsoft", "MSBuildStructuredLog"));
            
            // Assert
            Assert.True(!string.IsNullOrWhiteSpace(rootPath), "RootPath should not be null or whitespace.");
            Assert.True(containsMicrosoft, $"RootPath '{rootPath}' does not contain expected sub-path 'Microsoft{Path.DirectorySeparatorChar}MSBuildStructuredLog'.");
        }

        /// <summary>
        /// Tests that the TempPath property is the combination of RootPath and "Temp".
        /// </summary>
        [Fact]
        public void TempPath_WhenAccessed_IsRootPathCombinedWithTemp()
        {
            // Arrange
            string expectedTempPath = Path.Combine(PathUtils.RootPath, "Temp");
            
            // Act
            string actualTempPath = PathUtils.TempPath;
            
            // Assert
            Assert.Equal(expectedTempPath, actualTempPath);
        }

        /// <summary>
        /// Tests the IsExtended method for various path inputs to determine if a path is extended.
        /// </summary>
        /// <param name="path">The path string to test.</param>
        /// <param name="expected">Expected boolean result.</param>
        [Theory]
        [InlineData(@"\\?\C:\Folder", true)]
        [InlineData(@"\\.\C:\Folder", false)]
        [InlineData(@"C:\Folder", false)]
        [InlineData(@"//?/C:/Folder", false)]
        [InlineData(@"\\?\UNC\server\share", true)]
        [InlineData(@"\\?\UNC\", true)]
        public void IsExtended_VariousInputs_ReturnsExpectedOutcome(string path, bool expected)
        {
            // Act
            bool result = PathUtils.IsExtended(path);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the HasInvalidVolumeSeparator method for various valid and invalid path inputs.
        /// </summary>
        /// <param name="path">The path string to validate.</param>
        /// <param name="expected">Expected boolean result indicating invalid volume separator presence.</param>
        [Theory]
        [InlineData(@"C:\Folder", false)]
        [InlineData(@":\Folder", true)] // Starts with colon.
        [InlineData(@"1:\Folder", true)] // Invalid drive letter.
        [InlineData(@"C:\Fol:der", true)] // Extra colon later in the path.
        [InlineData(@"C:\Fo:lder", true)] // Extra colon.
        [InlineData(@"   C:\Folder", false)] // Leading whitespace but valid drive.
        [InlineData(@"\\?\C:\Folder", false)] // Extended path with valid drive.
        [InlineData(@"\\?\:\Folder", true)] // Extended path with colon immediately after extended prefix.
        public void HasInvalidVolumeSeparator_VariousInputs_ReturnsExpectedOutcome(string path, bool expected)
        {
            // Act
            bool result = PathUtils.HasInvalidVolumeSeparator(path);
            
            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the IsDirectorySeparator method with various characters.
        /// </summary>
        /// <param name="character">The character to test.</param>
        /// <param name="expected">Expected boolean result indicating if the character is a directory separator.</param>
        [Theory]
        [InlineData('\\', true)]
        [InlineData('/', true)]
        [InlineData('-', false)]
        [InlineData('a', false)]
        public void IsDirectorySeparator_VariousCharacters_ReturnsExpectedOutcome(char character, bool expected)
        {
            // Act
            bool result = PathUtils.IsDirectorySeparator(character);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the IsValidDriveChar method with valid and invalid drive characters.
        /// </summary>
        /// <param name="driveChar">The character representing a drive letter.</param>
        /// <param name="expected">Expected boolean result indicating if it is a valid drive character.</param>
        [Theory]
        [InlineData('C', true)]
        [InlineData('z', true)]
        [InlineData('1', false)]
        [InlineData('$', false)]
        public void IsValidDriveChar_VariousCharacters_ReturnsExpectedOutcome(char driveChar, bool expected)
        {
            // Act
            bool result = PathUtils.IsValidDriveChar(driveChar);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the PathStartSkip method with various input strings to verify correct whitespace skipping.
        /// </summary>
        /// <param name="input">The input path string.</param>
        /// <param name="expectedIndex">The expected starting index after skipping.</param>
        [Theory]
        [InlineData("   C:\\Folder", 3)] // Leading spaces with valid drive letter.
        [InlineData("   \\Folder", 3)] // Leading spaces with directory separator.
        [InlineData("", 0)] // Empty string.
        [InlineData("C:\\Folder", 0)] // No leading spaces.
        [InlineData("    ", 0)] // Only spaces should return 0.
        public void PathStartSkip_VariousInputs_ReturnsExpectedIndex(string input, int expectedIndex)
        {
            // Act
            int result = PathUtils.PathStartSkip(input);

            // Assert
            Assert.Equal(expectedIndex, result);
        }
    }
}
