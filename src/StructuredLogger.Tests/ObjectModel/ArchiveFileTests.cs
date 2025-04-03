// using System;
// using System.IO;
// using System.IO.Compression;
// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ArchiveFile"/> class.
//     /// </summary>
//     public class ArchiveFileTests
//     {
//         /// <summary>
//         /// Tests that the ArchiveFile constructor properly assigns the FullPath and Text properties.
//         /// </summary>
//         [Fact]
//         public void Constructor_ValidInputs_PropertiesAreAssigned()
//         {
//             // Arrange
//             string expectedPath = "test/path";
//             string expectedText = "sample text";
// 
//             // Act
//             ArchiveFile archiveFile = new ArchiveFile(expectedPath, expectedText);
// 
//             // Assert
//             archiveFile.FullPath.Should().Be(expectedPath);
//             archiveFile.Text.Should().Be(expectedText);
//         }
// 
//         /// <summary>
//         /// Helper method to create a ZipArchiveEntry with specified full name and content.
//         /// </summary>
//         /// <param name="entryFullName">The full name to assign to the entry.</param>
//         /// <param name="content">The content of the entry.</param>
//         /// <returns>A ZipArchiveEntry instance containing the specified content.</returns>
//         private ZipArchiveEntry CreateZipArchiveEntry(string entryFullName, string content)
//         {
//             // Create a temporary in-memory zip archive with one entry.
//             var memoryStream = new MemoryStream();
//             using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
//             {
//                 var entry = archive.CreateEntry(entryFullName);
//                 using (var entryStream = entry.Open())
//                 using (var writer = new StreamWriter(entryStream))
//                 {
//                     writer.Write(content);
//                 }
//             }
//             memoryStream.Seek(0, SeekOrigin.Begin);
//             var readArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read);
//             ZipArchiveEntry createdEntry = readArchive.GetEntry(entryFullName);
//             return createdEntry;
//         }
// //  // [Error] (70-51)CS0117 'ArchiveFile' does not contain a definition for 'From' // [Error] (74-47)CS0117 'ArchiveFile' does not contain a definition for 'CalculateArchivePath'
// //         /// <summary>
// //         /// Tests the From method (with adjustPath true by default) to ensure it correctly creates an ArchiveFile.
// //         /// </summary>
// //         [Fact]
// //         public void From_WithAdjustPathTrue_CreatesArchiveFileWithAdjustedPathAndText()
// //         {
// //             // Arrange
// //             string entryName = "C\\folder\\file.txt";
// //             string expectedContent = "Zip entry content";
// //             ZipArchiveEntry entry = CreateZipArchiveEntry(entryName, expectedContent);
// // 
// //             // Act
// //             ArchiveFile archiveFile = ArchiveFile.From(entry);
// // 
// //             // Assert
// //             // Expected full path should be calculated using CalculateArchivePath.
// //             string expectedPath = ArchiveFile.CalculateArchivePath(entry.FullName);
// //             archiveFile.FullPath.Should().Be(expectedPath);
// //             archiveFile.Text.Should().Be(expectedContent);
// //         }
// //  // [Error] (91-51)CS0117 'ArchiveFile' does not contain a definition for 'From'
//         /// <summary>
//         /// Tests the From method when adjustPath is false to ensure it creates an ArchiveFile without path adjustment.
//         /// </summary>
//         [Fact]
//         public void From_WithAdjustPathFalse_CreatesArchiveFileWithOriginalPathAndText()
//         {
//             // Arrange
//             string entryName = "D/folder/file.txt"; // using a forward slash to simulate a non-adjusted path.
//             string expectedContent = "Different content";
//             ZipArchiveEntry entry = CreateZipArchiveEntry(entryName, expectedContent);
// 
//             // Act
//             ArchiveFile archiveFile = ArchiveFile.From(entry, false);
// 
//             // Assert
//             archiveFile.FullPath.Should().Be(entry.FullName);
//             archiveFile.Text.Should().Be(expectedContent);
//         }
// //  // [Error] (110-45)CS0117 'ArchiveFile' does not contain a definition for 'GetText'
// //         /// <summary>
// //         /// Tests the GetText method to ensure it returns the correct text from a ZipArchiveEntry.
// //         /// </summary>
// //         [Fact]
// //         public void GetText_ValidZipArchiveEntry_ReturnsCorrectText()
// //         {
// //             // Arrange
// //             string entryName = "sample.txt";
// //             string expectedContent = "Content from zip entry";
// //             ZipArchiveEntry entry = CreateZipArchiveEntry(entryName, expectedContent);
// // 
// //             // Act
// //             string actualText = ArchiveFile.GetText(entry);
// // 
// //             // Assert
// //             actualText.Should().Be(expectedContent);
// //         }
// //  // [Error] (134-41)CS0117 'ArchiveFile' does not contain a definition for 'CalculateArchivePath'
//         /// <summary>
//         /// Tests the CalculateArchivePath method with various inputs to ensure it correctly adjusts and normalizes the file path.
//         /// </summary>
//         /// <param name="inputPath">The input file path.</param>
//         /// <param name="expected">The expected output before normalization.</param>
//         [Theory]
//         [InlineData("C\\folder\\file.txt", "C:\\folder\\file.txt")]
//         [InlineData("\\folder\\file.txt", "\\folder\\file.txt")]
//         [InlineData("/folder/file.txt", "/folder/file.txt")]
//         [InlineData("A", "A")]
//         [InlineData("", "")]
//         public void CalculateArchivePath_VariousInputs_ReturnsExpectedNormalizedPath(string inputPath, string expected)
//         {
//             // Arrange
//             // For inputs that meet the condition, a colon is inserted after the drive letter before normalization.
//             string expectedNormalized = Microsoft.Build.Logging.StructuredLogger.TextUtilities.NormalizeFilePath(expected);
// 
//             // Act
//             string actual = ArchiveFile.CalculateArchivePath(inputPath);
// 
//             // Assert
//             actual.Should().Be(expectedNormalized, $"for input path '{inputPath}', the calculated archive path should match the normalized expected output");
//         }
//     }
// }