using System.IO;
using System.IO.Compression;
using System.Linq;
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
        // Temporary files created during tests will be stored here to be cleaned up.
        private readonly string _tempDirectory;

        public ProjectImportsCollectorTests()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), "ProjectImportsCollectorTests", Guid.NewGuid().ToString());
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
                // Ignored cleanup errors.
            }
        }

        /// <summary>
        /// Tests that FlushBlobToFile creates an archive file with the proper content.
        /// </summary>
        [Fact]
        public void FlushBlobToFile_ValidInputs_CreatesArchiveFileWithContent()
        {
            // Arrange
            string logFilePath = Path.Combine(_tempDirectory, "test.binlog");
            File.WriteAllText(logFilePath, "dummy log file");
            string expectedArchivePath = Path.ChangeExtension(logFilePath, ".ProjectImports.zip");
            string testContent = "Test content for archive";
            using MemoryStream contentStream = new MemoryStream(Encoding.UTF8.GetBytes(testContent));

            // Act
            ProjectImportsCollector.FlushBlobToFile(logFilePath, contentStream);

            // Assert
            Assert.True(File.Exists(expectedArchivePath));
            using FileStream archiveStream = File.OpenRead(expectedArchivePath);
            using MemoryStream ms = new MemoryStream();
            archiveStream.CopyTo(ms);
            string resultContent = Encoding.UTF8.GetString(ms.ToArray());
            Assert.Contains(testContent, resultContent);

            // Cleanup
            File.Delete(expectedArchivePath);
            File.Delete(logFilePath);
        }

        /// <summary>
        /// Tests that AddFile adds an existing file into the archive.
        /// </summary>
        [Fact]
        public void AddFile_ExistingFile_AddsFileToArchive()
        {
            // Arrange
            string fileContent = "Content of existing file";
            string sourceFile = Path.Combine(_tempDirectory, "sourceFile.txt");
            File.WriteAllText(sourceFile, fileContent);

            string logFilePath = Path.Combine(_tempDirectory, "build.binlog");
            // Create an empty log file (its content is not important)
            File.WriteAllText(logFilePath, "log");

            // Use runOnBackground false for synchronous execution.
            using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);

            // Act
            collector.AddFile(sourceFile);

            // Process the result and extract archive entries.
            string extractedContent = null;
            collector.ProcessResult(stream =>
            {
                using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                // The archive entry name is calculated by removing ':' and converting slashes.
                // Compute expected entry name:
                string fullPath = Path.GetFullPath(sourceFile);
                string expectedEntryName = fullPath.Replace(":", "").Replace("\\\\", "\\").Replace("/", "\\");
                var entry = zip.Entries.FirstOrDefault(e => e.FullName.Equals(expectedEntryName, StringComparison.OrdinalIgnoreCase));
                Assert.NotNull(entry);
                using var entryStream = entry.Open();
                using var reader = new StreamReader(entryStream, Encoding.UTF8);
                extractedContent = reader.ReadToEnd();
            },
            error => Assert.False(true, $"Unexpected error: {error}"));

            // Assert
            Assert.Equal(fileContent, extractedContent);

            // Cleanup archive file and source file.
            collector.DeleteArchive();
            File.Delete(sourceFile);
            File.Delete(logFilePath);
        }

        /// <summary>
        /// Tests that AddFile does nothing when a non-existent file is given.
        /// </summary>
        [Fact]
        public void AddFile_NonExistentFile_DoesNotAddEntry()
        {
            // Arrange
            string nonExistentFile = Path.Combine(_tempDirectory, "nonexistent.txt");
            string logFilePath = Path.Combine(_tempDirectory, "build.binlog");
            File.WriteAllText(logFilePath, "log");

            using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);

            // Act
            collector.AddFile(nonExistentFile);

            // Process the result and verify that archive is empty.
            collector.ProcessResult(stream =>
            {
                using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                Assert.Empty(zip.Entries);
            },
            error => Assert.False(true, $"Unexpected error: {error}"));

            // Cleanup
            collector.DeleteArchive();
            File.Delete(logFilePath);
        }

        /// <summary>
        /// Tests that AddFileFromMemory (string overload) adds content into the archive.
        /// </summary>
        [Fact]
        public void AddFileFromMemory_String_Valid_AddsEntryToArchive()
        {
            // Arrange
            string memoryContent = "Hello from memory string";
            string filePath = "memoryFile.txt"; // relative path
            string logFilePath = Path.Combine(_tempDirectory, "build.binlog");
            File.WriteAllText(logFilePath, "log");

            using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);

            // Act
            collector.AddFileFromMemory(filePath, memoryContent);
            collector.ProcessResult(stream =>
            {
                using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                // Since AddFileFromMemory makes path absolute.
                string fullPath = Path.GetFullPath(filePath);
                string expectedEntryName = fullPath.Replace(":", "").Replace("\\\\", "\\").Replace("/", "\\");
                var entry = zip.Entries.FirstOrDefault(e => e.FullName.Equals(expectedEntryName, StringComparison.OrdinalIgnoreCase));
                Assert.NotNull(entry);
                using var entryStream = entry.Open();
                using var reader = new StreamReader(entryStream, Encoding.UTF8);
                string resultContent = reader.ReadToEnd();
                Assert.Equal(memoryContent, resultContent);
            },
            error => Assert.False(true, $"Unexpected error: {error}"));

            // Cleanup
            collector.DeleteArchive();
            File.Delete(logFilePath);
        }

        /// <summary>
        /// Tests that AddFileFromMemory (stream overload) adds content into the archive.
        /// </summary>
        [Fact]
        public void AddFileFromMemory_Stream_Valid_AddsEntryToArchive()
        {
            // Arrange
            string streamContent = "Hello from memory stream";
            string filePath = "memoryStreamFile.txt"; // relative path
            string logFilePath = Path.Combine(_tempDirectory, "build.binlog");
            File.WriteAllText(logFilePath, "log");

            using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
            using MemoryStream msContent = new MemoryStream(Encoding.UTF8.GetBytes(streamContent));

            // Act
            collector.AddFileFromMemory(filePath, msContent);
            collector.ProcessResult(stream =>
            {
                using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                string fullPath = Path.GetFullPath(filePath);
                string expectedEntryName = fullPath.Replace(":", "").Replace("\\\\", "\\").Replace("/", "\\");
                var entry = zip.Entries.FirstOrDefault(e => e.FullName.Equals(expectedEntryName, StringComparison.OrdinalIgnoreCase));
                Assert.NotNull(entry);
                using var entryStream = entry.Open();
                using var reader = new StreamReader(entryStream, Encoding.UTF8);
                string resultContent = reader.ReadToEnd();
                Assert.Equal(streamContent, resultContent);
            },
            error => Assert.False(true, $"Unexpected error: {error}"));

            // Cleanup
            collector.DeleteArchive();
            File.Delete(logFilePath);
        }

        /// <summary>
        /// Tests that ProcessResult invokes onError callback if the archive file length exceeds int.MaxValue.
        /// Since simulating a file size greater than int.MaxValue is not practical, 
        /// this test simulates the condition by creating a dummy archive file with a fake length.
        /// </summary>
        [Fact]
        public void ProcessResult_ArchiveFileTooLarge_InvokesOnError()
        {
            // Arrange
            string logFilePath = Path.Combine(_tempDirectory, "build.binlog");
            File.WriteAllText(logFilePath, "log");
            // Create collector normally.
            using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
            // Force close to finalize archive creation.
            collector.Close();

            // Manually overwrite the archive file with a dummy file that simulates huge size.
            string archivePath = Path.ChangeExtension(logFilePath, ".ProjectImports.zip");
            using (var fs = new FileStream(archivePath, FileMode.Open, FileAccess.Write))
            {
                // Write some data and set length artificially
                fs.SetLength((long)int.MaxValue + 1);
            }

            bool errorInvoked = false;
            // Act
            collector.ProcessResult(
                stream => Assert.False(true, "consumeStream should not be called when file size exceeds limit"),
                error =>
                {
                    errorInvoked = true;
                    Assert.Contains("exceeded 2GB limit", error);
                });

            // Assert
            Assert.True(errorInvoked);

            // Cleanup
            collector.DeleteArchive();
            File.Delete(logFilePath);
        }

        /// <summary>
        /// Tests that Close properly finalizes pending tasks and disposes resources.
        /// </summary>
        [Fact]
        public void Close_PendingTasksCompletedAndResourcesDisposed()
        {
            // Arrange
            string memoryContent = "Content before close";
            string filePath = "closeTest.txt";
            string logFilePath = Path.Combine(_tempDirectory, "build.binlog");
            File.WriteAllText(logFilePath, "log");
            using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: true);
            collector.AddFileFromMemory(filePath, memoryContent);

            // Act
            // Calling Close should wait for pending tasks.
            collector.Close();

            // Assert by calling ProcessResult (should work even after Close).
            bool consumeCalled = false;
            collector.ProcessResult(
                stream =>
                {
                    consumeCalled = true;
                    using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                    // Check that the entry exists.
                    string fullPath = Path.GetFullPath(filePath);
                    string expectedEntryName = fullPath.Replace(":", "").Replace("\\\\", "\\").Replace("/", "\\");
                    var entry = zip.Entries.FirstOrDefault(e => e.FullName.Equals(expectedEntryName, StringComparison.OrdinalIgnoreCase));
                    Assert.NotNull(entry);
                },
                error => Assert.False(true, $"Unexpected error: {error}"));
            Assert.True(consumeCalled);

            // Cleanup
            collector.DeleteArchive();
            File.Delete(logFilePath);
        }

        /// <summary>
        /// Tests that DeleteArchive deletes the archive file.
        /// </summary>
        [Fact]
        public void DeleteArchive_DeletesArchiveFile()
        {
            // Arrange
            string logFilePath = Path.Combine(_tempDirectory, "build.binlog");
            File.WriteAllText(logFilePath, "log");
            using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);
            // Add a dummy file to create an archive entry.
            collector.AddFileFromMemory("dummy.txt", "dummy");
            collector.Close();

            string archivePath = Path.ChangeExtension(logFilePath, ".ProjectImports.zip");
            Assert.True(File.Exists(archivePath));

            // Act
            collector.DeleteArchive();

            // Assert
            Assert.False(File.Exists(archivePath));

            // Cleanup
            File.Delete(logFilePath);
        }

        /// <summary>
        /// Tests that adding duplicate files does not result in multiple entries in the archive.
        /// </summary>
        [Fact]
        public void AddFile_DuplicateFile_NotAddedTwice()
        {
            // Arrange
            string fileContent = "Duplicate file content";
            string sourceFile = Path.Combine(_tempDirectory, "duplicate.txt");
            File.WriteAllText(sourceFile, fileContent);

            string logFilePath = Path.Combine(_tempDirectory, "build.binlog");
            File.WriteAllText(logFilePath, "log");

            using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);

            // Act
            collector.AddFile(sourceFile);
            collector.AddFile(sourceFile); // Duplicate addition

            collector.ProcessResult(stream =>
            {
                using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                // Compute expected entry name.
                string fullPath = Path.GetFullPath(sourceFile);
                string expectedEntryName = fullPath.Replace(":", "").Replace("\\\\", "\\").Replace("/", "\\");
                var entries = zip.Entries.Where(e => e.FullName.Equals(expectedEntryName, StringComparison.OrdinalIgnoreCase));
                // Assert that there is only one entry.
                Assert.Single(entries);
            },
            error => Assert.False(true, $"Unexpected error: {error}"));

            // Cleanup
            collector.DeleteArchive();
            File.Delete(sourceFile);
            File.Delete(logFilePath);
        }

        /// <summary>
        /// Tests that adding duplicate memory file entries does not result in multiple entries in the archive.
        /// </summary>
        [Fact]
        public void AddFileFromMemory_DuplicateEntry_NotAddedTwice()
        {
            // Arrange
            string memoryContent = "Duplicate memory content";
            string filePath = "duplicateMemory.txt";
            string logFilePath = Path.Combine(_tempDirectory, "build.binlog");
            File.WriteAllText(logFilePath, "log");

            using var collector = new ProjectImportsCollector(logFilePath, createFile: true, runOnBackground: false);

            // Act
            collector.AddFileFromMemory(filePath, memoryContent);
            collector.AddFileFromMemory(filePath, memoryContent);

            collector.ProcessResult(stream =>
            {
                using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                string fullPath = Path.GetFullPath(filePath);
                string expectedEntryName = fullPath.Replace(":", "").Replace("\\\\", "\\").Replace("/", "\\");
                var entries = zip.Entries.Where(e => e.FullName.Equals(expectedEntryName, StringComparison.OrdinalIgnoreCase));
                Assert.Single(entries);
            },
            error => Assert.False(true, $"Unexpected error: {error}"));

            // Cleanup
            collector.DeleteArchive();
            File.Delete(logFilePath);
        }
    }
}
