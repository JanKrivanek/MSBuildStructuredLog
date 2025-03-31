// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Microsoft.Build.Logging.StructuredLogger.UnitTests;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "BinaryLog"/> class.
//     /// </summary>
//     public class BinaryLogTests
//     {
//         /// <summary>
//         /// Tests that ReadRecords(string) throws an exception when a non-existent file path is provided.
//         /// </summary>
//         [Fact]
//         public void ReadRecords_StringFilePath_NonExistentFile_ThrowsException()
//         {
//             // Arrange
//             string nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".binlog");
//             // Act
//             Action act = () => BinaryLog.ReadRecords(nonExistentFile).ToList();
//             // Assert
//             act.Should().Throw<Exception>("because providing a non-existent file should result in an exception from the underlying reader");
//         }
// 
//         /// <summary>
//         /// Tests that ReadRecords(Stream) with an empty stream returns a non-null enumerable.
//         /// </summary>
//         [Fact]
//         public void ReadRecords_Stream_EmptyStream_ReturnsEnumerable()
//         {
//             // Arrange
//             using var emptyStream = new MemoryStream(Array.Empty<byte>());
//             // Act
//             IEnumerable<Record> records = null;
//             Action act = () => records = BinaryLog.ReadRecords(emptyStream);
//             // Assert
//             act.Should().NotThrow();
//             records.Should().NotBeNull("because even an empty stream should result in a non-null enumerable (possibly empty)");
//         }
// 
//         /// <summary>
//         /// Tests that ReadRecords(byte[]) with an empty array returns a non-null enumerable.
//         /// </summary>
//         [Fact]
//         public void ReadRecords_ByteArray_EmptyArray_ReturnsEnumerable()
//         {
//             // Arrange
//             byte[] emptyBytes = Array.Empty<byte>();
//             // Act
//             IEnumerable<Record> records = null;
//             Action act = () => records = BinaryLog.ReadRecords(emptyBytes);
//             // Assert
//             act.Should().NotThrow();
//             records.Should().NotBeNull("because even an empty byte array should result in a non-null enumerable (possibly empty)");
//         }
// 
//         /// <summary>
//         /// Tests that ReadBuild(string) throws a FileNotFoundException when the provided file path does not exist.
//         /// </summary>
//         [Fact]
//         public void ReadBuild_StringFilePath_NonExistentFile_ThrowsFileNotFoundException()
//         {
//             // Arrange
//             string nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".binlog");
//             // Act
//             Action act = () => BinaryLog.ReadBuild(nonExistentFile);
//             // Assert
//             act.Should().Throw<FileNotFoundException>("because a non-existent file should result in a file-not-found error when attempting to open it");
//         }
// //  // [Error] (89-31)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (92-23)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (93-67)CS8121 An expression of type 'TreeNode' cannot be handled by a pattern of type 'Error'.
// //         /// <summary>
// //         /// Tests that ReadBuild(string) with an empty file returns a Build object that indicates a failure and contains an error message.
// //         /// </summary>
// //         [Fact]
// //         public void ReadBuild_StringFilePath_EmptyFile_ReturnsBuildWithError()
// //         {
// //             // Arrange
// //             string tempFile = Path.GetTempFileName();
// //             try
// //             {
// //                 File.WriteAllBytes(tempFile, Array.Empty<byte>());
// //                 // Act
// //                 Build build = BinaryLog.ReadBuild(tempFile);
// //                 // Assert
// //                 build.Should().NotBeNull("because a Build object is always returned");
// //                 build.Succeeded.Should().BeFalse("because an empty file cannot be processed successfully");
// //                 build.Children.Should().Contain(child => child is Error error && error.Text.Contains("Error when opening the log file"), "because an error message should be added when the file cannot be processed");
// //             }
// //             finally
// //             {
// //                 File.Delete(tempFile);
// //             }
// //         }
// //  // [Error] (110-27)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (113-19)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (114-63)CS8121 An expression of type 'TreeNode' cannot be handled by a pattern of type 'Error'.
//         /// <summary>
//         /// Tests that ReadBuild(Stream) with an empty stream returns a Build object that indicates a failure and contains an error message.
//         /// </summary>
//         [Fact]
//         public void ReadBuild_Stream_EmptyStream_ReturnsBuildWithError()
//         {
//             // Arrange
//             using var emptyStream = new MemoryStream(Array.Empty<byte>());
//             // Act
//             Build build = BinaryLog.ReadBuild(emptyStream);
//             // Assert
//             build.Should().NotBeNull("because a Build object is always returned");
//             build.Succeeded.Should().BeFalse("because an empty stream cannot be processed successfully");
//             build.Children.Should().Contain(child => child is Error error && error.Text.Contains("Error when opening the log file"), "because an error message should be added when the stream cannot be processed");
//         }
// //  // [Error] (140-31)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (143-23)CS1061 'Build' does not contain a definition for 'SourceFilesArchive' and no accessible extension method 'SourceFilesArchive' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (144-23)CS1061 'Build' does not contain a definition for 'SourceFilesArchive' and no accessible extension method 'SourceFilesArchive' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that ReadBuild(string) correctly reads a companion project imports archive and sets the SourceFilesArchive property.
// //         /// </summary>
// //         [Fact]
// //         public void ReadBuild_StringFilePath_WithProjectImportsArchive_SetsSourceFilesArchive()
// //         {
// //             // Arrange
// //             string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".binlog");
// //             string companionFile = Path.ChangeExtension(tempFile, ".ProjectImports.zip");
// //             byte[] dummyArchive = new byte[]
// //             {
// //                 1,
// //                 2,
// //                 3,
// //                 4
// //             };
// //             try
// //             {
// //                 // Create an empty binlog file.
// //                 File.WriteAllBytes(tempFile, Array.Empty<byte>());
// //                 // Create the companion project imports archive with dummy data.
// //                 File.WriteAllBytes(companionFile, dummyArchive);
// //                 // Act
// //                 Build build = BinaryLog.ReadBuild(tempFile);
// //                 // Assert
// //                 build.Should().NotBeNull("because a Build object is always returned even if processing fails");
// //                 build.SourceFilesArchive.Should().NotBeNull("because a companion project imports archive exists");
// //                 build.SourceFilesArchive.Should().BeEquivalentTo(dummyArchive, "because the SourceFilesArchive should match the contents of the companion file");
// //             }
// //             finally
// //             {
// //                 if (File.Exists(tempFile))
// //                 {
// //                     File.Delete(tempFile);
// //                 }
// // 
// //                 if (File.Exists(companionFile))
// //                 {
// //                     File.Delete(companionFile);
// //                 }
// //             }
// //         }
// //  // [Error] (173-31)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (176-23)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (177-67)CS8121 An expression of type 'TreeNode' cannot be handled by a pattern of type 'Error'.
//         /// <summary>
//         /// Tests that ReadBuild(string, Progress) behaves similarly to the basic ReadBuild(string) overload when processing an empty file.
//         /// </summary>
//         [Fact]
//         public void ReadBuild_StringFilePath_WithProgress_ReturnsBuildWithErrorOnEmptyFile()
//         {
//             // Arrange
//             string tempFile = Path.GetTempFileName();
//             try
//             {
//                 File.WriteAllBytes(tempFile, Array.Empty<byte>());
//                 Progress progress = new Progress();
//                 // Act
//                 Build build = BinaryLog.ReadBuild(tempFile, progress);
//                 // Assert
//                 build.Should().NotBeNull("because a Build object is always returned");
//                 build.Succeeded.Should().BeFalse("because an empty file should result in a failure");
//                 build.Children.Should().Contain(child => child is Error error && error.Text.Contains("Error when opening the log file"), "because an error message should be present when processing an empty file");
//             }
//             finally
//             {
//                 File.Delete(tempFile);
//             }
//         }
// //  // [Error] (198-31)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (201-23)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (202-67)CS8121 An expression of type 'TreeNode' cannot be handled by a pattern of type 'Error'.
// //         /// <summary>
// //         /// Tests that ReadBuild(string, ReaderSettings) behaves similarly to the basic ReadBuild(string) overload when processing an empty file.
// //         /// </summary>
// //         [Fact]
// //         public void ReadBuild_StringFilePath_WithReaderSettings_ReturnsBuildWithErrorOnEmptyFile()
// //         {
// //             // Arrange
// //             string tempFile = Path.GetTempFileName();
// //             try
// //             {
// //                 File.WriteAllBytes(tempFile, Array.Empty<byte>());
// //                 ReaderSettings readerSettings = ReaderSettings.Default;
// //                 // Act
// //                 Build build = BinaryLog.ReadBuild(tempFile, readerSettings);
// //                 // Assert
// //                 build.Should().NotBeNull("because a Build object is always returned");
// //                 build.Succeeded.Should().BeFalse("because processing an empty file should fail");
// //                 build.Children.Should().Contain(child => child is Error error && error.Text.Contains("Error when opening the log file"), "because an error message should be added when the file cannot be processed");
// //             }
// //             finally
// //             {
// //                 File.Delete(tempFile);
// //             }
// //         }
// //  // [Error] (227-27)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (230-19)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (231-63)CS8121 An expression of type 'TreeNode' cannot be handled by a pattern of type 'Error'.
//         /// <summary>
//         /// Tests that ReadBuild(Stream, Progress, byte[], ReaderSettings) returns a Build with error information when provided an empty stream.
//         /// </summary>
//         [Fact]
//         public void ReadBuild_Stream_WithAllParameters_EmptyStream_ReturnsBuildWithError()
//         {
//             // Arrange
//             using var emptyStream = new MemoryStream(Array.Empty<byte>());
//             Progress progress = new Progress();
//             byte[] archive = new byte[]
//             {
//                 9,
//                 8,
//                 7
//             };
//             ReaderSettings readerSettings = ReaderSettings.Default;
//             // Act
//             Build build = BinaryLog.ReadBuild(emptyStream, progress, archive, readerSettings);
//             // Assert
//             build.Should().NotBeNull("because a Build object is always returned");
//             build.Succeeded.Should().BeFalse("because an empty stream should lead to a failed build processing");
//             build.Children.Should().Contain(child => child is Error error && error.Text.Contains("Error when opening the log file"), "because an error message should be added when the stream cannot be processed");
//         }
//     }
// }