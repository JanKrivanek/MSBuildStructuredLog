using System.IO;
using System.IO.Compression;
using System.Text;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ArchiveFile"/> class.
    /// </summary>
    public class ArchiveFileTests
    {
        /// <summary>
        /// Tests that the constructor correctly sets the FullPath and Text properties.
        /// </summary>
        [Fact]
        public void Constructor_ValidInputs_PropertiesAreSet()
        {
            // Arrange
            string expectedPath = "dummyPath";
            string expectedText = "dummyText";

            // Act
            var archiveFile = new ArchiveFile(expectedPath, expectedText);

            // Assert
            Assert.Equal(expectedPath, archiveFile.FullPath);
            Assert.Equal(expectedText, archiveFile.Text);
        }

        /// <summary>
        /// Helper method to create a ZipArchiveEntry with specified content.
        /// </summary>
        /// <param name="entryName">Name of the entry.</param>
        /// <param name="content">Content to write into the entry.</param>
        /// <returns>A ZipArchiveEntry containing the specified content.</returns>
        private ZipArchiveEntry CreateZipArchiveEntry(string entryName, string content)
        {
            // Create a memory stream to hold the ZIP archive.
            var memoryStream = new MemoryStream();
            // Create a ZIP archive in Create mode.
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var entry = archive.CreateEntry(entryName);
                using (var entryStream = entry.Open())
                using (var writer = new StreamWriter(entryStream, Encoding.UTF8))
                {
                    writer.Write(content);
                }
            }
            // Reset stream position.
            memoryStream.Position = 0;
            // Open the ZIP archive in Read mode.
            var readArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read);
            return readArchive.GetEntry(entryName);
        }

        /// <summary>
        /// Tests the GetText method with a valid ZipArchiveEntry to ensure it reads the complete content.
        /// </summary>
        [Fact]
        public void GetText_ValidStream_ReturnsCompleteText()
        {
            // Arrange
            string expectedContent = "This is a test content.";
            var entry = CreateZipArchiveEntry("test.txt", expectedContent);

            // Act
            string actualContent = ArchiveFile.GetText(entry);

            // Assert
            Assert.Equal(expectedContent, actualContent);
        }

        /// <summary>
        /// Tests the GetText method when the ZipArchiveEntry has an empty stream.
        /// </summary>
        [Fact]
        public void GetText_EmptyStream_ReturnsEmptyString()
        {
            // Arrange
            string expectedContent = string.Empty;
            var entry = CreateZipArchiveEntry("empty.txt", expectedContent);

            // Act
            string actualContent = ArchiveFile.GetText(entry);

            // Assert
            Assert.Equal(expectedContent, actualContent);
        }

        /// <summary>
        /// Tests the From method with adjustPath set to false to ensure it uses the entry's original FullName.
        /// </summary>
        [Fact]
        public void From_WithAdjustPathFalse_UsesOriginalEntryFullName()
        {
            // Arrange
            string entryName = "folder\\file.txt";
            string fileContent = "Sample file content";
            var entry = CreateZipArchiveEntry(entryName, fileContent);

            // Act
            ArchiveFile archiveFile = ArchiveFile.From(entry, adjustPath: false);

            // Assert
            Assert.Equal(entryName, archiveFile.FullPath);
            Assert.Equal(fileContent, archiveFile.Text);
        }

        /// <summary>
        /// Tests the From method with adjustPath set to true to ensure it adjusts the path using CalculateArchivePath.
        /// </summary>
        [Fact]
        public void From_WithAdjustPathTrue_AdjustsPathUsingCalculateArchivePath()
        {
            // Arrange
            // Use an entry name that triggers the adjustment (second character is a backslash).
            string entryName = "C\\folder\\file.txt";
            string fileContent = "Content for adjusted path test";
            var entry = CreateZipArchiveEntry(entryName, fileContent);
            // Expected adjustment as per the CalculateArchivePath logic:
            // If entryName[1] == '\\' and entryName[0] is not '\\' or '/', then adjusted = first char + ":" + remainder.
            string expectedAdjusted = "C:" + entryName.Substring(1);
            // Note: The result is then normalized by TextUtilities.NormalizeFilePath.
            // For testing purposes, we assume NormalizeFilePath returns the input unchanged.

            // Act
            ArchiveFile archiveFile = ArchiveFile.From(entry); // Defaults to adjustPath: true

            // Assert
            Assert.Equal(expectedAdjusted, archiveFile.FullPath);
            Assert.Equal(fileContent, archiveFile.Text);
        }

        /// <summary>
        /// Tests the CalculateArchivePath method when the input path meets the condition for adjustment.
        /// </summary>
        /// <param name="input">Input file path.</param>
        /// <param name="expected">Expected adjusted path.</param>
        [Theory]
        [InlineData("C\\file.txt", "C:" + "\\file.txt")]
        [InlineData("D\\folder\\subfolder\\file.txt", "D:" + "\\folder\\subfolder\\file.txt")]
        public void CalculateArchivePath_WhenConditionMet_AdjustsPath(string input, string expected)
        {
            // Act
            string result = ArchiveFile.CalculateArchivePath(input);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the CalculateArchivePath method when the input path does not meet the adjustment condition.
        /// </summary>
        /// <param name="input">Input file path.</param>
        [Theory]
        [InlineData("\\folder\\file.txt")]
        [InlineData("/folder/file.txt")]
        [InlineData("folder\\file.txt")]
        [InlineData("file.txt")]
        public void CalculateArchivePath_WhenConditionNotMet_ReturnsNormalizedPath(string input)
        {
            // Act
            string result = ArchiveFile.CalculateArchivePath(input);

            // Assert
            // Assuming TextUtilities.NormalizeFilePath returns the input unchanged for these cases.
            Assert.Equal(input, result);
        }
    }
}
