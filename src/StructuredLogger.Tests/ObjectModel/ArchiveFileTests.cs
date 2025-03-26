using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Moq;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ArchiveFile"/> class.
    /// </summary>
    public class ArchiveFileTests
    {
        /// <summary>
        /// Tests the constructor of <see cref="ArchiveFile"/> to verify that the properties are correctly assigned.
        /// </summary>
        [Fact]
        public void Constructor_WithValidParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedFullPath = "TestPath";
            string expectedText = "Sample text content";

            // Act
            var archiveFile = new ArchiveFile(expectedFullPath, expectedText);

            // Assert
            Assert.Equal(expectedFullPath, archiveFile.FullPath);
            Assert.Equal(expectedText, archiveFile.Text);
        }

        /// <summary>
        /// Tests the <see cref="ArchiveFile.From(ZipArchiveEntry)"/> method to ensure it uses the default adjustPath parameter.
        /// </summary>
//         [Fact] [Error] (44-38)CS1069 The type name 'ZipArchiveEntry' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. [Error] (45-54)CS1503 Argument 1: cannot convert from 'string' to '?' [Error] (49-34)CS0012 The type 'ZipArchiveEntry' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'.
//         public void From_WithAdjustPath_UsesCalculatedArchivePathAndReadsText()
//         {
//             // Arrange
//             string entryFullName = "C\\folder\\file.txt";
//             string sampleText = "File contents for testing";
//             string expectedFullPath = ArchiveFile.CalculateArchivePath(entryFullName);
//             var mockEntry = new Mock<ZipArchiveEntry>();
//             mockEntry.Setup(e => e.FullName).Returns(entryFullName);
//             mockEntry.Setup(e => e.Open()).Returns(() => new MemoryStream(Encoding.UTF8.GetBytes(sampleText)));
// 
//             // Act
//             ArchiveFile result = ArchiveFile.From(mockEntry.Object);
// 
//             // Assert
//             Assert.Equal(expectedFullPath, result.FullPath);
//             Assert.Equal(sampleText, result.Text);
//         }

        /// <summary>
        /// Tests the <see cref="ArchiveFile.From(ZipArchiveEntry, bool)"/> method when adjustPath is false.
        /// </summary>
//         [Fact] [Error] (65-38)CS1069 The type name 'ZipArchiveEntry' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. [Error] (66-54)CS1503 Argument 1: cannot convert from 'string' to '?' [Error] (70-34)CS0012 The type 'ZipArchiveEntry' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'.
//         public void From_WithoutAdjustPath_DoesNotModifyFullPathButReadsText()
//         {
//             // Arrange
//             string entryFullName = "C\\folder\\file.txt";
//             string sampleText = "Content without path adjustment";
//             var mockEntry = new Mock<ZipArchiveEntry>();
//             mockEntry.Setup(e => e.FullName).Returns(entryFullName);
//             mockEntry.Setup(e => e.Open()).Returns(() => new MemoryStream(Encoding.UTF8.GetBytes(sampleText)));
// 
//             // Act
//             ArchiveFile result = ArchiveFile.From(mockEntry.Object, adjustPath: false);
// 
//             // Assert
//             Assert.Equal(entryFullName, result.FullPath);
//             Assert.Equal(sampleText, result.Text);
//         }

        /// <summary>
        /// Tests the <see cref="ArchiveFile.GetText(ZipArchiveEntry)"/> method to ensure it correctly reads text from the provided stream.
        /// </summary>
//         [Fact] [Error] (86-38)CS1069 The type name 'ZipArchiveEntry' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. [Error] (87-52)CS1503 Argument 1: cannot convert from 'System.IO.MemoryStream' to '?' [Error] (90-33)CS0012 The type 'ZipArchiveEntry' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'.
//         public void GetText_WithValidStream_ReturnsCorrectText()
//         {
//             // Arrange
//             string expectedText = "Stream content for testing";
//             var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(expectedText));
//             var mockEntry = new Mock<ZipArchiveEntry>();
//             mockEntry.Setup(e => e.Open()).Returns(memoryStream);
// 
//             // Act
//             string actualText = ArchiveFile.GetText(mockEntry.Object);
// 
//             // Assert
//             Assert.Equal(expectedText, actualText);
//         }

        /// <summary>
        /// Tests the <see cref="ArchiveFile.CalculateArchivePath(string)"/> method for a file path that meets the condition for drive letter adjustment.
        /// </summary>
        [Fact]
        public void CalculateArchivePath_WhenPathMeetsCondition_InsertsColonAfterDriveLetter()
        {
            // Arrange
            string inputPath = "C\\folder\\file.txt";
            // Expected transformation: "C:" + "\folder\file.txt"
            string expectedPath = "C:" + inputPath.Substring(1);

            // Act
            string actualPath = ArchiveFile.CalculateArchivePath(inputPath);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        /// <summary>
        /// Tests the <see cref="ArchiveFile.CalculateArchivePath(string)"/> method for file paths that do not meet the adjustment condition.
        /// </summary>
        /// <param name="inputPath">The input file path.</param>
        [Theory]
        [InlineData("folder\\file.txt")]
        [InlineData("\\folder\\file.txt")]
        [InlineData("/folder/file.txt")]
        public void CalculateArchivePath_WhenPathDoesNotMeetCondition_ReturnsNormalizedPath(string inputPath)
        {
            // Arrange
            string expectedPath = inputPath;

            // Act
            string actualPath = ArchiveFile.CalculateArchivePath(inputPath);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        /// <summary>
        /// Tests the <see cref="ArchiveFile.CalculateArchivePath(string)"/> method for minimal input scenarios.
        /// </summary>
        /// <param name="inputPath">The minimal input file path.</param>
        [Theory]
        [InlineData("")]
        [InlineData("a")]
        public void CalculateArchivePath_WithMinimalInput_ReturnsNormalizedPath(string inputPath)
        {
            // Arrange
            string expectedPath = inputPath;

            // Act
            string actualPath = ArchiveFile.CalculateArchivePath(inputPath);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }
    }
}
