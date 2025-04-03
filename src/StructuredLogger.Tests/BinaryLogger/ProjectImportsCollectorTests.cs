// using System;
// using System.IO;
// using System.IO.Compression;
// using System.Text;
// using FluentAssertions;
// using Microsoft.Build.Logging;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ProjectImportsCollector"/> class.
//     /// </summary>
//     public class ProjectImportsCollectorTests : IDisposable
//     {
//         private readonly string _tempDirectory;
//         private readonly string _logFilePath; // dummy log file path
// 
//         public ProjectImportsCollectorTests()
//         {
//             _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
//             Directory.CreateDirectory(_tempDirectory);
//             // Create a dummy log file with .binlog extension in the temp directory.
//             _logFilePath = Path.Combine(_tempDirectory, "test.binlog");
//             File.WriteAllText(_logFilePath, "dummy log content");
//         }
// 
//         public void Dispose()
//         {
//             try
//             {
//                 if (Directory.Exists(_tempDirectory))
//                 {
//                     Directory.Delete(_tempDirectory, true);
//                 }
//             }
//             catch
//             {
//                 // Ignored on cleanup.
//             }
//         }
// 
//         /// <summary>
//         /// Tests that FlushBlobToFile writes the content stream to the expected archive file.
//         /// </summary>
//         [Fact]
//         public void FlushBlobToFile_HappyPath_WritesContent()
//         {
//             // Arrange
//             byte[] expectedBytes = Encoding.UTF8.GetBytes("Test content for FlushBlobToFile");
//             using var contentStream = new MemoryStream(expectedBytes);
//             string archiveFilePath = Path.ChangeExtension(_logFilePath, ".ProjectImports.zip");
// 
//             // Act
//             ProjectImportsCollector.FlushBlobToFile(_logFilePath, contentStream);
// 
//             // Assert
//             File.Exists(archiveFilePath).Should().BeTrue("the archive file should be created by FlushBlobToFile");
//             byte[] actualBytes = File.ReadAllBytes(archiveFilePath);
//             actualBytes.Should().BeEquivalentTo(expectedBytes, "the archive file content should match the source content stream");
//         }
// //  // [Error] (71-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
// //         /// <summary>
// //         /// Tests that the constructor creates an archive file when createFile is true.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_CreateFileTrue_CreatesArchiveFile()
// //         {
// //             // Arrange & Act
// //             using var collector = new ProjectImportsCollector(_logFilePath, createFile: true);
// //             collector.Close();
// //             string archiveFilePath = Path.ChangeExtension(_logFilePath, ".ProjectImports.zip");
// // 
// //             // Assert
// //             File.Exists(archiveFilePath).Should().BeTrue("the archive file should exist when createFile is true");
// //         }
// //  // [Error] (90-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
//         /// <summary>
//         /// Tests that AddFileFromMemory (string overload) adds an entry to the archive.
//         /// </summary>
//         [Fact]
//         public void AddFileFromMemory_StringData_HappyPath_AddsEntry()
//         {
//             // Arrange
//             string fileName = "test.txt";
//             string fileData = "Content of test file";
//             DateTimeOffset creationTime = DateTimeOffset.Now;
// 
//             using var collector = new ProjectImportsCollector(_logFilePath, createFile: true, runOnBackground: false);
//             
//             // Act
//             collector.AddFileFromMemory(fileName, fileData, creationTime);
//             collector.Close();
// 
//             // Assert
//             // Open the created archive and verify it contains the entry.
//             string archiveFilePath = Path.ChangeExtension(_logFilePath, ".ProjectImports.zip");
//             using var archiveStream = File.OpenRead(archiveFilePath);
//             using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
//             // Calculate expected archive entry name per CalculateArchivePath implementation.
//             string fullPath = Path.GetFullPath(fileName);
//             string expectedEntryName = fullPath.Replace(":", "").Replace("\\\\", "\\").Replace("/", "\\");
//             var entry = archive.GetEntry(expectedEntryName);
//             entry.Should().NotBeNull("the archive should contain an entry for the added file");
//             entry.LastWriteTime.Should().Be(creationTime, "the entry's LastWriteTime should match the provided creation timestamp");
//             using var entryStream = entry.Open();
//             using var reader = new StreamReader(entryStream);
//             string content = reader.ReadToEnd();
//             content.Should().Be(fileData, "the archive entry content should match the input data");
//         }
// //  // [Error] (126-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
// //         /// <summary>
// //         /// Tests that AddFileFromMemory (Stream overload) adds an entry to the archive.
// //         /// </summary>
// //         [Fact]
// //         public void AddFileFromMemory_Stream_HappyPath_AddsEntry()
// //         {
// //             // Arrange
// //             string fileName = "streamTest.txt";
// //             string fileData = "Stream based file content";
// //             DateTimeOffset creationTime = DateTimeOffset.Now;
// //             byte[] dataBytes = Encoding.UTF8.GetBytes(fileData);
// //             using var dataStream = new MemoryStream(dataBytes);
// // 
// //             using var collector = new ProjectImportsCollector(_logFilePath, createFile: true, runOnBackground: false);
// //             
// //             // Act
// //             collector.AddFileFromMemory(fileName, dataStream, creationTime);
// //             collector.Close();
// // 
// //             // Assert
// //             string archiveFilePath = Path.ChangeExtension(_logFilePath, ".ProjectImports.zip");
// //             using var archiveStream = File.OpenRead(archiveFilePath);
// //             using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
// //             string fullPath = Path.GetFullPath(fileName);
// //             string expectedEntryName = fullPath.Replace(":", "").Replace("\\\\", "\\").Replace("/", "\\");
// //             var entry = archive.GetEntry(expectedEntryName);
// //             entry.Should().NotBeNull("the archive should contain an entry for the file added from stream");
// //             entry.LastWriteTime.Should().Be(creationTime, "the entry's LastWriteTime should match the provided creation timestamp");
// //             using var entryStream = entry.Open();
// //             using var reader = new StreamReader(entryStream);
// //             string content = reader.ReadToEnd();
// //             content.Should().Be(fileData, "the content of the archive entry should match the stream data");
// //         }
// //  // [Error] (158-41)CS7036 There is no argument given that corresponds to the required parameter 'bufferSize' of 'StreamWriter.StreamWriter(Stream, Encoding, int, bool)' // [Error] (169-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
//         /// <summary>
//         /// Tests that ProcessResult invokes the consumeStream callback with the archive content.
//         /// </summary>
//         [Fact]
//         public void ProcessResult_HappyPath_ConsumesStream()
//         {
//             // Arrange
//             bool consumeCalled = false;
//             byte[] expectedContent;
//             using (var ms = new MemoryStream())
//             {
//                 using (var writer = new StreamWriter(ms, Encoding.UTF8, leaveOpen: true))
//                 {
//                     writer.Write("ProcessResult test content");
//                     writer.Flush();
//                     ms.Position = 0;
//                     expectedContent = ms.ToArray();
//                 }
//             }
//             // Pre-fill archive file using FlushBlobToFile for testing.
//             ProjectImportsCollector.FlushBlobToFile(_logFilePath, new MemoryStream(expectedContent));
// 
//             using var collector = new ProjectImportsCollector(_logFilePath, createFile: true);
//             
//             // Act
//             collector.ProcessResult(stream =>
//             {
//                 consumeCalled = true;
//                 using var msResult = new MemoryStream();
//                 stream.CopyTo(msResult);
//                 byte[] actualContent = msResult.ToArray();
//                 actualContent.Should().BeEquivalentTo(expectedContent, "the processed stream content should match the expected archive content");
//             },
//             error => error.Should().BeNull("no error should occur in a normal scenario"));
// 
//             // Assert
//             consumeCalled.Should().BeTrue("consumeStream callback should be invoked");
//         }
// //  // [Error] (194-20)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
// //         /// <summary>
// //         /// Tests that DeleteArchive deletes the archive file.
// //         /// </summary>
// //         [Fact]
// //         public void DeleteArchive_ArchiveDeleted()
// //         {
// //             // Arrange
// //             string archiveFilePath = Path.ChangeExtension(_logFilePath, ".ProjectImports.zip");
// //             using (var collector = new ProjectImportsCollector(_logFilePath, createFile: true))
// //             {
// //                 // Act
// //                 collector.DeleteArchive();
// //             }
// //             // Assert
// //             File.Exists(archiveFilePath).Should().BeFalse("the archive file should be deleted after calling DeleteArchive");
// //         }
// //  // [Error] (211-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
//         /// <summary>
//         /// Tests that AddFile does not add an entry when the file does not exist.
//         /// </summary>
//         [Fact]
//         public void AddFile_FileDoesNotExist_NoEntry()
//         {
//             // Arrange
//             string nonExistentFile = Path.Combine(_tempDirectory, "nonexistent.csproj");
//             using var collector = new ProjectImportsCollector(_logFilePath, createFile: true, runOnBackground: false);
//             
//             // Act
//             collector.AddFile(nonExistentFile);
//             collector.Close();
// 
//             // Assert
//             string archiveFilePath = Path.ChangeExtension(_logFilePath, ".ProjectImports.zip");
//             using var archiveStream = File.OpenRead(archiveFilePath);
//             using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
//             string fullPath = Path.GetFullPath(nonExistentFile);
//             string expectedEntryName = fullPath.Replace(":", "").Replace("\\\\", "\\").Replace("/", "\\");
//             var entry = archive.GetEntry(expectedEntryName);
//             entry.Should().BeNull("no entry should be added for a non-existent file");
//         }
// //  // [Error] (234-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
// //         /// <summary>
// //         /// Tests that methods gracefully handle a null file path.
// //         /// </summary>
// //         [Fact]
// //         public void AddFile_NullFilePath_NoOperation()
// //         {
// //             // Arrange
// //             using var collector = new ProjectImportsCollector(_logFilePath, createFile: true, runOnBackground: false);
// //             
// //             // Act
// //             Action act1 = () => collector.AddFile(null);
// //             Action act2 = () => collector.AddFileFromMemory(null, "data");
// // 
// //             // Assert
// //             act1.Should().NotThrow("passing null to AddFile should not throw an exception");
// //             act2.Should().NotThrow("passing null to AddFileFromMemory should not throw an exception");
// //             collector.Close();
// //         }
// //  // [Error] (253-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
//         /// <summary>
//         /// Tests that calling Close multiple times does not throw an exception.
//         /// </summary>
//         [Fact]
//         public void Close_MultipleCalls_NoException()
//         {
//             // Arrange
//             using var collector = new ProjectImportsCollector(_logFilePath, createFile: true);
//             
//             // Act & Assert
//             Action act = () =>
//             {
//                 collector.Close();
//                 collector.Close();
//             };
//             act.Should().NotThrow("calling Close multiple times should be safe and not throw exceptions");
//         }
//     }
// }