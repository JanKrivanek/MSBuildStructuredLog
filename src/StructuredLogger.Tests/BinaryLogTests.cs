using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BinaryLog"/> class.
    /// </summary>
    public class BinaryLogTests
    {
        /// <summary>
        /// Tests that ReadRecords with a nonexistent file path throws a FileNotFoundException.
        /// </summary>
        [Fact]
        public void ReadRecords_String_NonexistentFile_ThrowsFileNotFoundException()
        {
            // Arrange
            string nonExistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".binlog");

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() =>
            {
                // This should internally attempt to open a file that does not exist.
                BinaryLog.ReadRecords(nonExistentFilePath).ToList();
            });
        }

        /// <summary>
        /// Tests that ReadRecords with a null stream throws an ArgumentNullException.
        /// </summary>
        [Fact]
        public void ReadRecords_Stream_NullStream_ThrowsArgumentNullException()
        {
            // Arrange
            Stream nullStream = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Passing null stream should trigger an exception.
                BinaryLog.ReadRecords(nullStream).ToList();
            });
        }

        /// <summary>
        /// Tests that ReadRecords with a null byte array throws an ArgumentNullException.
        /// </summary>
        [Fact]
        public void ReadRecords_Bytes_NullBytes_ThrowsArgumentNullException()
        {
            // Arrange
            byte[] nullBytes = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Passing null bytes should trigger an exception.
                BinaryLog.ReadRecords(nullBytes).ToList();
            });
        }

        /// <summary>
        /// Tests that ReadBuild with a file path that does not exist throws a FileNotFoundException.
        /// </summary>
        [Fact]
        public void ReadBuild_FilePath_Nonexistent_ThrowsFileNotFoundException()
        {
            // Arrange
            string nonExistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".binlog");

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() =>
            {
                // This call is expected to throw as the file does not exist.
                BinaryLog.ReadBuild(nonExistentFilePath);
            });
        }

        /// <summary>
        /// Tests that ReadBuild when provided an empty stream returns a Build instance with error information.
        /// This verifies the edge case where the stream does not contain a valid log.
        /// </summary>
        [Fact]
        public void ReadBuild_Stream_Empty_ReturnsBuildWithError()
        {
            // Arrange
            using MemoryStream emptyStream = new MemoryStream();
            
            // Act
            Build result = BinaryLog.ReadBuild(emptyStream);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded, "Expected the build to indicate failure due to invalid stream content.");

            // Check if an error message was added.
            // We assume that Build has a property or field exposing added children (errors/warnings).
            // Since the type of Children is not defined in the test context,
            // we use reflection to verify that at least one child has a Text property containing the error message.
            var childrenProperty = result.GetType().GetProperty("Children");
            Assert.NotNull(childrenProperty);

            var children = childrenProperty.GetValue(result) as IEnumerable<object>;
            Assert.NotNull(children);
            bool hasError = children.Any(child =>
            {
                var textProperty = child.GetType().GetProperty("Text");
                if (textProperty != null)
                {
                    var text = textProperty.GetValue(child) as string;
                    return text != null && text.Contains("Error when opening the log file.");
                }
                return false;
            });
            Assert.True(hasError, "Expected error message indicating problem opening the log file was not found.");
        }

        /// <summary>
        /// Tests that ReadBuild using the file path overload correctly sets the LogFilePath and assigns a project imports archive if available.
        /// This simulates the condition where a corresponding .ProjectImports.zip file exists.
        /// </summary>
        [Fact]
        public void ReadBuild_FilePath_WithProjectImports_SetsSourceFilesArchive()
        {
            // Arrange
            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".binlog");
            string projectImportsFile = Path.ChangeExtension(tempFile, ".ProjectImports.zip");
            byte[] dummyBinLogContent = new byte[] { 0x01, 0x02, 0x03 };
            byte[] dummyProjectImports = new byte[] { 0x0A, 0x0B, 0x0C };

            try
            {
                // Write dummy content to the binlog file.
                File.WriteAllBytes(tempFile, dummyBinLogContent);
                // Write dummy project imports archive.
                File.WriteAllBytes(projectImportsFile, dummyProjectImports);

                // Act
                Build result = BinaryLog.ReadBuild(tempFile);

                // Assert
                Assert.NotNull(result);
                // Since the dummy binlog content is not valid, the build is expected to be in error.
                Assert.False(result.Succeeded, "Expected the build to indicate failure due to invalid binlog content.");
                
                // Verify that the LogFilePath was set.
                var logFilePathProperty = result.GetType().GetProperty("LogFilePath");
                Assert.NotNull(logFilePathProperty);
                string logFilePath = logFilePathProperty.GetValue(result) as string;
                Assert.Equal(tempFile, logFilePath);

                // Verify that the SourceFilesArchive was set from the project imports file.
                var sourceFilesArchiveProperty = result.GetType().GetProperty("SourceFilesArchive");
                Assert.NotNull(sourceFilesArchiveProperty);
                byte[] archiveResult = sourceFilesArchiveProperty.GetValue(result) as byte[];
                Assert.NotNull(archiveResult);
                Assert.True(dummyProjectImports.SequenceEqual(archiveResult), "The SourceFilesArchive does not match the expected project imports archive.");
            }
            finally
            {
                // Cleanup temporary files.
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
                if (File.Exists(projectImportsFile))
                {
                    File.Delete(projectImportsFile);
                }
            }
        }

        /// <summary>
        /// Tests that the overload ReadBuild(Stream, Progress, byte[], ReaderSettings) correctly uses the provided project imports archive.
        /// </summary>
        [Fact]
        public void ReadBuild_Stream_WithProvidedProjectImportsArchive_SetsSourceFilesArchive()
        {
            // Arrange
            using MemoryStream emptyStream = new MemoryStream();
            byte[] providedArchive = new byte[] { 0xAA, 0xBB, 0xCC };

            // Act
            Build result = BinaryLog.ReadBuild(emptyStream, progress: null, projectImportsArchive: providedArchive, readerSettings: null);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded, "Expected the build to indicate failure due to invalid stream content.");

            var sourceFilesArchiveProperty = result.GetType().GetProperty("SourceFilesArchive");
            Assert.NotNull(sourceFilesArchiveProperty);
            byte[] archiveResult = sourceFilesArchiveProperty.GetValue(result) as byte[];
            // Even if the stream is invalid, if the SourceFilesArchive was null, it should be assigned the provided archive.
            Assert.NotNull(archiveResult);
            Assert.True(providedArchive.SequenceEqual(archiveResult), "The provided project imports archive was not assigned correctly.");
        }
    }
}
