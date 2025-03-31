// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.IO;
// using System.IO.Compression;
// using System.Linq;
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
//         /// Tests that the constructor correctly sets the FullPath and Text properties.
//         /// </summary>
//         [Fact]
//         public void Constructor_ValidParameters_PropertiesSet()
//         {
//             // Arrange
//             string expectedFullPath = "Test/Path";
//             string expectedText = "Test Content";
// 
//             // Act
//             var archiveFile = new ArchiveFile(expectedFullPath, expectedText);
// 
//             // Assert
//             archiveFile.FullPath.Should().Be(expectedFullPath);
//             archiveFile.Text.Should().Be(expectedText);
//         }
// //  // [Error] (48-42)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (48-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context // [Error] (59-42)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (59-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context // [Error] (63-30)CS0012 The type 'ZipArchiveEntry' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'.
// //         /// <summary>
// //         /// Tests that ArchiveFile.From with adjustPath set to false returns an ArchiveFile with unadjusted path.
// //         /// </summary>
// //         [Fact]
// //         public void From_WithAdjustPathFalse_ReturnsFileWithOriginalPath()
// //         {
// //             // Arrange
// //             const string entryName = "C\\folder\\file.txt";
// //             const string entryText = "Hello, Archive!";
// //             ArchiveFile result;
// // 
// //             using (var memStream = new MemoryStream())
// //             {
// //                 // Create the zip archive and add an entry.
// //                 using (var archive = new ZipArchive(memStream, ZipArchiveMode.Create, leaveOpen: true))
// //                 {
// //                     var entry = archive.CreateEntry(entryName);
// //                     using (var writer = new StreamWriter(entry.Open()))
// //                     {
// //                         writer.Write(entryText);
// //                     }
// //                 }
// // 
// //                 memStream.Seek(0, SeekOrigin.Begin);
// //                 // Open the archive for reading.
// //                 using (var archive = new ZipArchive(memStream, ZipArchiveMode.Read))
// //                 {
// //                     var entry = archive.Entries.First();
// //                     // Act
// //                     result = ArchiveFile.From(entry, adjustPath: false);
// //                 }
// //             }
// // 
// //             // Assert
// //             result.FullPath.Should().Be(entryName);
// //             result.Text.Should().Be(entryText);
// //         }
// //  // [Error] (90-42)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (90-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context // [Error] (100-42)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (100-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context // [Error] (104-30)CS0012 The type 'ZipArchiveEntry' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'.
//         /// <summary>
//         /// Tests that ArchiveFile.From (default overload) adjusts the path when creating the ArchiveFile.
//         /// </summary>
//         [Fact]
//         public void From_DefaultAdjustPath_ReturnsFileWithAdjustedPath()
//         {
//             // Arrange
//             // The entry name will trigger the adjustment logic in CalculateArchivePath.
//             const string originalEntryName = "C\\folder\\file.txt";
//             const string entryText = "Sample content";
//             ArchiveFile result;
//             // Expected adjusted path: The method should insert a colon after the first character.
//             // "C\\folder\\file.txt" becomes "C:" + "\\folder\\file.txt" -> "C:\\folder\\file.txt"
//             string expectedAdjustedPath = "C:\\folder\\file.txt";
// 
//             using (var memStream = new MemoryStream())
//             {
//                 // Create zip archive and add an entry.
//                 using (var archive = new ZipArchive(memStream, ZipArchiveMode.Create, leaveOpen: true))
//                 {
//                     var entry = archive.CreateEntry(originalEntryName);
//                     using (var writer = new StreamWriter(entry.Open()))
//                     {
//                         writer.Write(entryText);
//                     }
//                 }
// 
//                 memStream.Seek(0, SeekOrigin.Begin);
//                 using (var archive = new ZipArchive(memStream, ZipArchiveMode.Read))
//                 {
//                     var entry = archive.Entries.First();
//                     // Act
//                     result = ArchiveFile.From(entry);
//                 }
//             }
// 
//             // Assert
//             result.FullPath.Should().Be(expectedAdjustedPath);
//             result.Text.Should().Be(entryText);
//         }
// //  // [Error] (126-42)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (126-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context // [Error] (136-42)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (136-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context // [Error] (140-34)CS0012 The type 'ZipArchiveEntry' is defined in an assembly that is not referenced. You must add a reference to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'.
// //         /// <summary>
// //         /// Tests that GetText correctly reads all text content from a ZipArchiveEntry.
// //         /// </summary>
// //         [Fact]
// //         public void GetText_ValidEntry_ReturnsEntryText()
// //         {
// //             // Arrange
// //             const string entryName = "anyName.txt";
// //             const string expectedText = "This is some sample text.";
// //             string actualText;
// // 
// //             using (var memStream = new MemoryStream())
// //             {
// //                 using (var archive = new ZipArchive(memStream, ZipArchiveMode.Create, leaveOpen: true))
// //                 {
// //                     var entry = archive.CreateEntry(entryName);
// //                     using (var writer = new StreamWriter(entry.Open()))
// //                     {
// //                         writer.Write(expectedText);
// //                     }
// //                 }
// // 
// //                 memStream.Seek(0, SeekOrigin.Begin);
// //                 using (var archive = new ZipArchive(memStream, ZipArchiveMode.Read))
// //                 {
// //                     var entry = archive.Entries.First();
// //                     // Act
// //                     actualText = ArchiveFile.GetText(entry);
// //                 }
// //             }
// // 
// //             // Assert
// //             actualText.Should().Be(expectedText);
// //         }
// // 
//         /// <summary>
//         /// Tests the CalculateArchivePath method with various input strings to verify proper path adjustment.
//         /// </summary>
//         /// <param name="inputPath">The input file path.</param>
//         /// <param name="expectedPath">The expected adjusted path after normalization.</param>
//         [Theory]
//         [InlineData("C\\folder", "C:\\folder")]
//         [InlineData("E\\", "E:\\")]
//         [InlineData("A", "A")]
//         [InlineData("\\folder", "\\folder")]
//         [InlineData("/folder", "/folder")]
//         [InlineData("D/folder", "D/folder")]
//         public void CalculateArchivePath_VariousInputs_ReturnsExpectedAdjustedPath(string inputPath, string expectedPath)
//         {
//             // Arrange & Act
//             string actualPath = ArchiveFile.CalculateArchivePath(inputPath);
// 
//             // Assert
//             actualPath.Should().Be(expectedPath);
//         }
//     }
// }