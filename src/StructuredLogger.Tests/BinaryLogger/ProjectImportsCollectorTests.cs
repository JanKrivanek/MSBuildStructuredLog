// using FluentAssertions;
// using Microsoft.Build.Logging;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.IO.Compression;
// using System.Text;
// using System.Threading.Tasks;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ProjectImportsCollector"/> class.
//     /// </summary>
//     public class ProjectImportsCollectorTests : IDisposable
//     {
//         private readonly List<string> _tempFiles = new List<string>();
// 
//         /// <summary>
//         /// Cleans up temporary files after each test.
//         /// </summary>
//         public void Dispose()
//         {
//             foreach (var file in _tempFiles)
//             {
//                 try
//                 {
//                     if (File.Exists(file))
//                     {
//                         File.Delete(file);
//                     }
//                 }
//                 catch
//                 {
//                     // Ignored
//                 }
//             }
//         }
// 
//         /// <summary>
//         /// Creates a temporary file path with specified extension.
//         /// </summary>
//         private string GetTempFilePath(string extension)
//         {
//             string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + extension);
//             _tempFiles.Add(tempPath);
//             return tempPath;
//         }
// 
//         /// <summary>
//         /// Computes the expected archive file path based on the log file path.
//         /// </summary>
//         private string ComputeExpectedArchiveFilePath(string logFilePath, string archiveExtension = ".ProjectImports.zip")
//         {
//             return Path.ChangeExtension(logFilePath, archiveExtension);
//         }
// 
//         /// <summary>
//         /// Tests that FlushBlobToFile writes content from the provided stream to the correct archive file.
//         /// </summary>
//         [Fact]
//         public void FlushBlobToFile_ValidStream_WritesContentToArchiveFile()
//         {
//             // Arrange
//             string logFilePath = GetTempFilePath(".binlog");
//             string expectedArchivePath = ComputeExpectedArchiveFilePath(logFilePath);
//             byte[] expectedContent = Encoding.UTF8.GetBytes("TestContent");
//             using MemoryStream contentStream = new MemoryStream(expectedContent);
// 
//             // Act
//             ProjectImportsCollector.FlushBlobToFile(logFilePath, contentStream);
// 
//             // Assert
//             File.Exists(expectedArchivePath).Should().BeTrue("the archive file should have been created.");
//             byte[] actualContent = File.ReadAllBytes(expectedArchivePath);
//             actualContent.Should().BeEquivalentTo(expectedContent, "the content copied should match the source stream.");
// 
//             // Cleanup
//             if (File.Exists(expectedArchivePath))
//             {
//                 File.Delete(expectedArchivePath);
//             }
//         }
// //  // [Error] (97-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
// //         /// <summary>
// //         /// Tests that the constructor creates an archive file when createFile parameter is true.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_CreateFileTrue_CreatesArchiveFile()
// //         {
// //             // Arrange
// //             string logFilePath = GetTempFilePath(".binlog");
// //             string expectedArchivePath = ComputeExpectedArchiveFilePath(logFilePath);
// // 
// //             // Act
// //             using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
// //             // To force archive file creation, we call Close.
// //             collector.Close();
// // 
// //             // Assert
// //             File.Exists(expectedArchivePath).Should().BeTrue("the archive file should be created alongside the log file.");
// // 
// //             // Cleanup
// //             if (File.Exists(expectedArchivePath))
// //             {
// //                 File.Delete(expectedArchivePath);
// //             }
// //         }
// //  // [Error] (121-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
//         /// <summary>
//         /// Tests that the constructor creates an archive file in the cache directory when createFile is false.
//         /// </summary>
//         [Fact]
//         public void Constructor_CreateFileFalse_CreatesArchiveFileInCacheDirectory()
//         {
//             // Arrange
//             string logFilePath = GetTempFilePath(".binlog");
// 
//             // Act
//             using var collector = new ProjectImportsCollector(logFilePath, createFile: false, runOnBackground: false);
//             collector.Close();
// 
//             // Since the archive file path is computed internally, we can use the fact that the file exists.
//             // Assert
//             // The file should exist somewhere in the file system.
//             // We attempt to locate it by checking the existence of a file that was created during the constructor.
//             // Since we cannot access the private field, we simulate by calling ProcessResult.
//             bool foundArchive = false;
//             collector.ProcessResult(
//                 stream => { foundArchive = true; },
//                 error => { /* ignore error */ });
//             foundArchive.Should().BeTrue("the archive file should have been created in the cache directory.");
//         }
// //  // [Error] (144-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'. // [Error] (155-45)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (155-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context
// //         /// <summary>
// //         /// Tests that AddFile with a null filePath does nothing and does not add an entry.
// //         /// </summary>
// //         [Fact]
// //         public void AddFile_NullFilePath_DoesNothing()
// //         {
// //             // Arrange
// //             string logFilePath = GetTempFilePath(".binlog");
// //             using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
// // 
// //             // Act
// //             collector.AddFile(null);
// //             collector.Close();
// // 
// //             // Assert
// //             int entryCount = 0;
// //             collector.ProcessResult(
// //                 stream =>
// //                 {
// //                     using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
// //                     entryCount = archive.Entries.Count;
// //                 },
// //                 error => { });
// //             entryCount.Should().Be(0, "no entry should be added when a null filePath is provided.");
// //         }
// //  // [Error] (170-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'. // [Error] (182-45)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (182-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context
//         /// <summary>
//         /// Tests that AddFile with a non-existent file path does not add an entry.
//         /// </summary>
//         [Fact]
//         public void AddFile_FileDoesNotExist_DoesNothing()
//         {
//             // Arrange
//             string logFilePath = GetTempFilePath(".binlog");
//             using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
//             string nonExistentFile = GetTempFilePath(".csproj"); // file not created
// 
//             // Act
//             collector.AddFile(nonExistentFile);
//             collector.Close();
// 
//             // Assert
//             int entryCount = 0;
//             collector.ProcessResult(
//                 stream =>
//                 {
//                     using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
//                     entryCount = archive.Entries.Count;
//                 },
//                 error => { });
//             entryCount.Should().Be(0, "no entry should be added for a non-existent file.");
//         }
// //  // [Error] (202-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'. // [Error] (214-45)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (214-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context
// //         /// <summary>
// //         /// Tests that AddFile adds an entry to the archive when provided with a valid file.
// //         /// </summary>
// //         [Fact]
// //         public void AddFile_ValidFile_AddsEntry()
// //         {
// //             // Arrange
// //             string fileContent = "Content from disk file.";
// //             string tempFile = GetTempFilePath(".csproj");
// //             File.WriteAllText(tempFile, fileContent);
// //             string logFilePath = GetTempFilePath(".binlog");
// //             string fullTempFilePath = Path.GetFullPath(tempFile);
// // 
// //             using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
// // 
// //             // Act
// //             collector.AddFile(tempFile);
// //             collector.Close();
// // 
// //             // Assert
// //             int entryCount = 0;
// //             string entryData = null;
// //             collector.ProcessResult(
// //                 stream =>
// //                 {
// //                     using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
// //                     entryCount = archive.Entries.Count;
// //                     // The entry name is manipulated by CalculateArchivePath
// //                     // The transformation removes ":" and converts directory separators.
// //                     // Therefore, we compute expected entry name.
// //                     string expectedEntryName = fullTempFilePath.Replace(":", "").Replace("\\\\", "\\").Replace("/", "\\");
// //                     var entry = archive.GetEntry(expectedEntryName);
// //                     entry.Should().NotBeNull("the archive should contain an entry for the valid file.");
// //                     using var entryStream = entry.Open();
// //                     using var reader = new StreamReader(entryStream);
// //                     entryData = reader.ReadToEnd();
// //                 },
// //                 error => { });
// //             entryCount.Should().Be(1, "there should be one entry added to the archive.");
// //             entryData.Should().Be(fileContent, "the content of the entry should match the original file content.");
// //         }
// //  // [Error] (239-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'. // [Error] (250-45)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (250-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context
//         /// <summary>
//         /// Tests that AddFileFromMemory (string overload) with a null filePath does nothing.
//         /// </summary>
//         [Fact]
//         public void AddFileFromMemory_String_NullFilePath_DoesNothing()
//         {
//             // Arrange
//             string logFilePath = GetTempFilePath(".binlog");
//             using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
// 
//             // Act
//             collector.AddFileFromMemory(null, "Some data");
//             collector.Close();
// 
//             // Assert
//             int entryCount = 0;
//             collector.ProcessResult(
//                 stream =>
//                 {
//                     using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
//                     entryCount = archive.Entries.Count;
//                 },
//                 error => { });
//             entryCount.Should().Be(0, "no entry should be added when a null filePath is provided.");
//         }
// //  // [Error] (269-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'. // [Error] (281-45)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (281-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context
// //         /// <summary>
// //         /// Tests that AddFileFromMemory (string overload) successfully adds an entry with the provided string data.
// //         /// </summary>
// //         [Fact]
// //         public void AddFileFromMemory_String_Valid_AddsEntry()
// //         {
// //             // Arrange
// //             string filePath = GetTempFilePath(".csproj");
// //             string data = "File content from memory.";
// //             string logFilePath = GetTempFilePath(".binlog");
// //             string expectedFullPath = Path.GetFullPath(filePath);
// // 
// //             using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
// // 
// //             // Act
// //             collector.AddFileFromMemory(filePath, data);
// //             collector.Close();
// // 
// //             // Assert
// //             int entryCount = 0;
// //             string entryData = null;
// //             collector.ProcessResult(
// //                 stream =>
// //                 {
// //                     using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
// //                     entryCount = archive.Entries.Count;
// //                     string expectedEntryName = expectedFullPath.Replace(":", "").Replace("\\\\", "\\").Replace("/", "\\");
// //                     var entry = archive.GetEntry(expectedEntryName);
// //                     entry.Should().NotBeNull("the archive should contain an entry for the in-memory file.");
// //                     using var entryStream = entry.Open();
// //                     using var reader = new StreamReader(entryStream);
// //                     entryData = reader.ReadToEnd();
// //                 },
// //                 error => { });
// //             entryCount.Should().Be(1, "one entry should be added for the in-memory file.");
// //             entryData.Should().Be(data, "the entry data should match the input string.");
// //         }
// //  // [Error] (303-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'. // [Error] (315-45)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (315-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context
//         /// <summary>
//         /// Tests that AddFileFromMemory (Stream overload) with a null filePath does nothing.
//         /// </summary>
//         [Fact]
//         public void AddFileFromMemory_Stream_NullFilePath_DoesNothing()
//         {
//             // Arrange
//             string logFilePath = GetTempFilePath(".binlog");
//             using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
//             using MemoryStream dummyStream = new MemoryStream(Encoding.UTF8.GetBytes("dummy"));
// 
//             // Act
//             collector.AddFileFromMemory(null, dummyStream);
//             collector.Close();
// 
//             // Assert
//             int entryCount = 0;
//             collector.ProcessResult(
//                 stream =>
//                 {
//                     using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
//                     entryCount = archive.Entries.Count;
//                 },
//                 error => { });
//             entryCount.Should().Be(0, "no entry should be added when a null filePath is provided.");
//         }
// //  // [Error] (335-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'. // [Error] (347-45)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. // [Error] (347-64)CS0103 The name 'ZipArchiveMode' does not exist in the current context
// //         /// <summary>
// //         /// Tests that AddFileFromMemory (Stream overload) successfully adds an entry with the provided stream data.
// //         /// </summary>
// //         [Fact]
// //         public void AddFileFromMemory_Stream_Valid_AddsEntry()
// //         {
// //             // Arrange
// //             string filePath = GetTempFilePath(".targets");
// //             string data = "Stream based file content.";
// //             using MemoryStream dataStream = new MemoryStream(Encoding.UTF8.GetBytes(data));
// //             string logFilePath = GetTempFilePath(".binlog");
// //             string expectedFullPath = Path.GetFullPath(filePath);
// // 
// //             using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
// // 
// //             // Act
// //             collector.AddFileFromMemory(filePath, dataStream);
// //             collector.Close();
// // 
// //             // Assert
// //             int entryCount = 0;
// //             string entryData = null;
// //             collector.ProcessResult(
// //                 stream =>
// //                 {
// //                     using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
// //                     entryCount = archive.Entries.Count;
// //                     string expectedEntryName = expectedFullPath.Replace(":", "").Replace("\\\\", "\\").Replace("/", "\\");
// //                     var entry = archive.GetEntry(expectedEntryName);
// //                     entry.Should().NotBeNull("the archive should contain an entry for the in-memory file stream.");
// //                     using var entryStream = entry.Open();
// //                     using var reader = new StreamReader(entryStream);
// //                     entryData = reader.ReadToEnd();
// //                 },
// //                 error => { });
// //             entryCount.Should().Be(1, "one entry should be added for the in-memory stream.");
// //             entryData.Should().Be(data, "the entry data should match the stream data.");
// //         }
// //  // [Error] (372-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
//         /// <summary>
//         /// Tests that ProcessResult correctly invokes the consumeStream action for a valid archive.
//         /// </summary>
//         [Fact]
//         public void ProcessResult_ValidArchive_InvokesConsumeStream()
//         {
//             // Arrange
//             string filePath = GetTempFilePath(".csproj");
//             string data = "Some content.";
//             string logFilePath = GetTempFilePath(".binlog");
// 
//             using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
//             collector.AddFileFromMemory(filePath, data);
// 
//             // Act
//             collector.Close();
//             bool consumeInvoked = false;
//             collector.ProcessResult(
//                 stream =>
//                 {
//                     consumeInvoked = true;
//                     // validate stream length is as expected (non-negative)
//                     stream.Length.Should().BeGreaterOrEqualTo(0);
//                 },
//                 error => { });
// 
//             // Assert
//             consumeInvoked.Should().BeTrue("consumeStream should be called for a valid archive file.");
//         }
// //  // [Error] (403-13)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
// //         /// <summary>
// //         /// Tests that Close waits for pending background tasks and disposes underlying streams without throwing.
// //         /// </summary>
// //         [Fact]
// //         public void Close_WaitsForPendingTasksAndDisposesStreams()
// //         {
// //             // Arrange
// //             string filePath = GetTempFilePath(".csproj");
// //             string data = "Content to force background task.";
// //             string logFilePath = GetTempFilePath(".binlog");
// // 
// //             // Using runOnBackground true to simulate chaining of tasks.
// //             using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: true);
// //             collector.AddFileFromMemory(filePath, data);
// // 
// //             // Act
// //             // Close should wait for the background task.
// //             Action act = () => collector.Close();
// // 
// //             // Assert
// //             act.Should().NotThrow("Close should wait for all pending tasks and dispose streams properly.");
// //         }
// //  // [Error] (423-20)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'. // [Error] (433-20)CS1674 'ProjectImportsCollector': type used in a using statement must implement 'System.IDisposable'.
//         /// <summary>
//         /// Tests that DeleteArchive deletes the archive file from the file system.
//         /// </summary>
//         [Fact]
//         public void DeleteArchive_DeletesArchiveFile()
//         {
//             // Arrange
//             string logFilePath = GetTempFilePath(".binlog");
//             string expectedArchivePath = ComputeExpectedArchiveFilePath(logFilePath);
//             using (var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false))
//             {
//                 // Ensure archive is created.
//                 collector.Close();
//             }
// 
//             // Pre-Assert: Archive file must exist.
//             File.Exists(expectedArchivePath).Should().BeTrue("the archive file should exist before deletion.");
// 
//             // Act
//             using (var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false))
//             {
//                 collector.DeleteArchive();
//             }
// 
//             // Assert
//             File.Exists(expectedArchivePath).Should().BeFalse("the archive file should be deleted after calling DeleteArchive.");
//         }
//     }
// }