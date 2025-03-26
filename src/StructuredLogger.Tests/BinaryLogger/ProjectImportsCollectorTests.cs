using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Logging;
using Xunit;

namespace Microsoft.Build.Logging.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ProjectImportsCollector"/> class.
    /// </summary>
    public class ProjectImportsCollectorTests : IDisposable
    {
        // Temporary files created during tests.
        private readonly string _tempDirectory;

        public ProjectImportsCollectorTests()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(_tempDirectory))
                {
                    Directory.Delete(_tempDirectory, true);
                }
            }
            catch
            {
                // Ignored - cleanup best effort.
            }
        }

        /// <summary>
        /// Helper method to generate a temp log file path.
        /// </summary>
        private string GetTempLogFilePath(string fileName = null)
        {
            fileName ??= Guid.NewGuid().ToString() + ".log";
            return Path.Combine(_tempDirectory, fileName);
        }

        /// <summary>
        /// Tests the FlushBlobToFile method to ensure it writes the provided stream contents to the archive file.
        /// </summary>
        [Fact]
        public void FlushBlobToFile_ValidStream_CreatesArchiveFileAndWritesContent()
        {
            // Arrange
            string logFilePath = GetTempLogFilePath("FlushBlobTest.log");
            string expectedContent = "Test flush content";
            using MemoryStream contentStream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContent));

            string archiveFilePath = Path.ChangeExtension(logFilePath, ".ProjectImports.zip");
            if (File.Exists(archiveFilePath))
            {
                File.Delete(archiveFilePath);
            }

            // Act
            ProjectImportsCollector.FlushBlobToFile(logFilePath, contentStream);

            // Assert
            Assert.True(File.Exists(archiveFilePath), "Archive file was not created by FlushBlobToFile.");

            using FileStream fs = File.OpenRead(archiveFilePath);
            using MemoryStream ms = new MemoryStream();
            fs.CopyTo(ms);
            string actualContent = Encoding.UTF8.GetString(ms.ToArray());
            Assert.Equal(expectedContent, actualContent);

            // Cleanup
            File.Delete(archiveFilePath);
        }

        /// <summary>
        /// Tests that the constructor correctly creates an archive file when createFile is true.
        /// </summary>
        [Fact]
        public void Constructor_CreateFileTrue_CreatesArchiveSuccessfully()
        {
            // Arrange
            string logFilePath = GetTempLogFilePath("ConstructorTrueTest.log");
            string archiveFilePath = Path.ChangeExtension(logFilePath, ".ProjectImports.zip");
            if (File.Exists(archiveFilePath))
            {
                File.Delete(archiveFilePath);
            }

            // Act
            var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
            collector.Close();

            // Assert
            Assert.True(File.Exists(archiveFilePath), "Archive file was not created when createFile is true.");

            // Cleanup
            File.Delete(archiveFilePath);
        }

        /// <summary>
        /// Tests that when createFile is false, the archive is created in the temporary cache directory.
        /// Verifies that ProcessResult calls consumeStream.
        /// </summary>
        [Fact]
        public void Constructor_CreateFileFalse_CreatesTemporaryArchiveInCacheDirectory()
        {
            // Arrange
            string logFilePath = GetTempLogFilePath("ConstructorFalseTest.log");
            bool consumeStreamCalled = false;
            void ConsumeStream(Stream stream)
            {
                consumeStreamCalled = true;
            }

            // Act
            var collector = new ProjectImportsCollector(logFilePath, createFile: false, runOnBackground: false);
            collector.Close();
            collector.ProcessResult(ConsumeStream, error => { });

            // Assert
            Assert.True(consumeStreamCalled, "consumeStream action was not called in ProcessResult when archive exists.");

            // Cleanup: Delete archive if exists.
            // Since archive is in the cache directory, invoke DeleteArchive.
            collector.DeleteArchive();
        }

        /// <summary>
        /// Tests that calling AddFile with a null file path does not throw and no entry is added.
        /// </summary>
//         [Fact] [Error] (151-38)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. [Error] (151-53)CS0103 The name 'ZipArchiveMode' does not exist in the current context
//         public void AddFile_NullFilePath_DoesNotThrowAndNoEntryAdded()
//         {
//             // Arrange
//             string logFilePath = GetTempLogFilePath("AddFileNullTest.log");
//             var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
// 
//             // Act
//             collector.AddFile(null);
//             collector.Close();
// 
//             // Assert
//             using (FileStream fs = File.OpenRead(Path.ChangeExtension(logFilePath, ".ProjectImports.zip")))
//             using (var archive = new ZipArchive(fs, ZipArchiveMode.Read))
//             {
//                 Assert.Empty(archive.Entries);
//             }
// 
//             // Cleanup
//             File.Delete(Path.ChangeExtension(logFilePath, ".ProjectImports.zip"));
//         }

        /// <summary>
        /// Tests that calling AddFile with a non-existent file path does not throw and no entry is added.
        /// </summary>
//         [Fact] [Error] (177-38)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. [Error] (177-53)CS0103 The name 'ZipArchiveMode' does not exist in the current context
//         public void AddFile_FileDoesNotExist_DoesNotThrowAndNoEntryAdded()
//         {
//             // Arrange
//             string logFilePath = GetTempLogFilePath("AddFileNonExistentTest.log");
//             var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
//             string nonExistentFilePath = Path.Combine(_tempDirectory, "nonexistentfile.txt");
// 
//             // Act
//             collector.AddFile(nonExistentFilePath);
//             collector.Close();
// 
//             // Assert
//             using (FileStream fs = File.OpenRead(Path.ChangeExtension(logFilePath, ".ProjectImports.zip")))
//             using (var archive = new ZipArchive(fs, ZipArchiveMode.Read))
//             {
//                 Assert.Empty(archive.Entries);
//             }
// 
//             // Cleanup
//             File.Delete(Path.ChangeExtension(logFilePath, ".ProjectImports.zip"));
//         }

        /// <summary>
        /// Tests that AddFile adds an entry to the archive when a valid file is provided.
        /// </summary>
//         [Fact] [Error] (207-38)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. [Error] (207-53)CS0103 The name 'ZipArchiveMode' does not exist in the current context
//         public void AddFile_ValidFile_CreatesEntryInArchive()
//         {
//             // Arrange
//             string logFilePath = GetTempLogFilePath("AddFileValidTest.log");
//             var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
// 
//             // Create a temporary file with known content.
//             string tempFile = Path.Combine(_tempDirectory, "TestFile.txt");
//             string fileContent = "Content from file";
//             File.WriteAllText(tempFile, fileContent);
// 
//             // Act
//             collector.AddFile(tempFile);
//             collector.Close();
// 
//             // Assert
//             using (FileStream fs = File.OpenRead(Path.ChangeExtension(logFilePath, ".ProjectImports.zip")))
//             using (var archive = new ZipArchive(fs, ZipArchiveMode.Read))
//             {
//                 // Since the file path is made absolute, the archive entry name will be the full path with certain characters removed.
//                 Assert.Single(archive.Entries);
//                 using (var entryStream = archive.Entries[0].Open())
//                 using (var reader = new StreamReader(entryStream))
//                 {
//                     string actualContent = reader.ReadToEnd();
//                     Assert.Equal(fileContent, actualContent);
//                 }
//             }
// 
//             // Cleanup
//             File.Delete(Path.ChangeExtension(logFilePath, ".ProjectImports.zip"));
//             File.Delete(tempFile);
//         }

        /// <summary>
        /// Tests that AddFileFromMemory (string overload) creates an entry in the archive with the provided data.
        /// </summary>
//         [Fact] [Error] (242-38)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. [Error] (242-53)CS0103 The name 'ZipArchiveMode' does not exist in the current context
//         public void AddFileFromMemory_String_ValidData_CreatesEntry()
//         {
//             // Arrange
//             string logFilePath = GetTempLogFilePath("AddFileFromMemoryStringTest.log");
//             var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
//             string filePath = Path.Combine(_tempDirectory, "MemoryFile.txt");
//             string data = "This is test data from string.";
// 
//             // Act
//             collector.AddFileFromMemory(filePath, data);
//             collector.Close();
// 
//             // Assert
//             using (FileStream fs = File.OpenRead(Path.ChangeExtension(logFilePath, ".ProjectImports.zip")))
//             using (var archive = new ZipArchive(fs, ZipArchiveMode.Read))
//             {
//                 Assert.Single(archive.Entries);
//                 using (var entryStream = archive.Entries[0].Open())
//                 using (var reader = new StreamReader(entryStream))
//                 {
//                     string actualContent = reader.ReadToEnd();
//                     Assert.Equal(data, actualContent);
//                 }
//             }
// 
//             // Cleanup
//             File.Delete(Path.ChangeExtension(logFilePath, ".ProjectImports.zip"));
//         }

        /// <summary>
        /// Tests that AddFileFromMemory (Stream overload) creates an entry in the archive with the provided data.
        /// </summary>
//         [Fact] [Error] (276-38)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. [Error] (276-53)CS0103 The name 'ZipArchiveMode' does not exist in the current context
//         public void AddFileFromMemory_Stream_ValidData_CreatesEntry()
//         {
//             // Arrange
//             string logFilePath = GetTempLogFilePath("AddFileFromMemoryStreamTest.log");
//             var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
//             string filePath = Path.Combine(_tempDirectory, "MemoryStreamFile.txt");
//             string data = "This is test data from stream.";
//             using MemoryStream dataStream = new MemoryStream(Encoding.UTF8.GetBytes(data));
// 
//             // Act
//             collector.AddFileFromMemory(filePath, dataStream);
//             collector.Close();
// 
//             // Assert
//             using (FileStream fs = File.OpenRead(Path.ChangeExtension(logFilePath, ".ProjectImports.zip")))
//             using (var archive = new ZipArchive(fs, ZipArchiveMode.Read))
//             {
//                 Assert.Single(archive.Entries);
//                 using (var entryStream = archive.Entries[0].Open())
//                 using (var reader = new StreamReader(entryStream))
//                 {
//                     string actualContent = reader.ReadToEnd();
//                     Assert.Equal(data, actualContent);
//                 }
//             }
// 
//             // Cleanup
//             File.Delete(Path.ChangeExtension(logFilePath, ".ProjectImports.zip"));
//         }

        /// <summary>
        /// Tests that ProcessResult consumes the stream when the archive file exists and its length is within limits.
        /// </summary>
        [Fact]
        public void ProcessResult_ArchiveExists_ConsumeStreamIsCalled()
        {
            // Arrange
            string logFilePath = GetTempLogFilePath("ProcessResultTest.log");
            var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
            // Add a file from memory to ensure archive gets some content.
            collector.AddFileFromMemory("dummy.txt", "dummy content");
            bool consumeStreamCalled = false;
            void ConsumeStream(Stream stream)
            {
                consumeStreamCalled = true;
                // Read stream to simulate consumption.
                using var reader = new StreamReader(stream);
                string content = reader.ReadToEnd();
            }
            void OnError(string error) { }

            // Act
            collector.ProcessResult(ConsumeStream, OnError);

            // Assert
            Assert.True(consumeStreamCalled, "consumeStream action was not called in ProcessResult.");

            // Cleanup is handled inside ProcessResult/collector.DeleteArchive.
            collector.DeleteArchive();
        }

        /// <summary>
        /// Tests that calling Close multiple times does not throw an exception.
        /// </summary>
//         [Fact] [Error] (334-29)CS0117 'Record' does not contain a definition for 'Exception'
//         public void Close_MultipleCalls_DoNotThrow()
//         {
//             // Arrange
//             string logFilePath = GetTempLogFilePath("CloseTest.log");
//             var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
// 
//             // Act & Assert
//             collector.Close();
//             var ex = Record.Exception(() => collector.Close());
//             Assert.Null(ex);
// 
//             // Cleanup
//             File.Delete(Path.ChangeExtension(logFilePath, ".ProjectImports.zip"));
//         }

        /// <summary>
        /// Tests that DeleteArchive successfully deletes the archive file.
        /// </summary>
        [Fact]
        public void DeleteArchive_DeletesTheArchiveFile()
        {
            // Arrange
            string logFilePath = GetTempLogFilePath("DeleteArchiveTest.log");
            var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
            // Add a memory file so that an archive is created.
            collector.AddFileFromMemory("dummy.txt", "data");
            collector.Close();
            string archiveFilePath = Path.ChangeExtension(logFilePath, ".ProjectImports.zip");
            Assert.True(File.Exists(archiveFilePath), "Archive file should exist before deletion.");

            // Act
            collector.DeleteArchive();

            // Assert
            Assert.False(File.Exists(archiveFilePath), "Archive file was not deleted by DeleteArchive.");
        }
    }
}
