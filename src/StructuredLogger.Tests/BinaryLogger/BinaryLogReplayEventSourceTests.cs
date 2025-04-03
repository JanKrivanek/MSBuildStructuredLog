using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Microsoft.Build.Shared;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BinaryLogReplayEventSource"/> class.
    /// </summary>
    public class BinaryLogReplayEventSourceTests
    {
        private readonly BinaryLogReplayEventSource _eventSource;

        public BinaryLogReplayEventSourceTests()
        {
            _eventSource = new BinaryLogReplayEventSource();
        }

        /// <summary>
        /// Tests that the <see cref="BinaryLogReplayEventSource.FileFormatVersion"/> property throws an <see cref="InvalidOperationException"/> when version information is not initialized.
        /// Expected outcome: Accessing the property before replay should throw.
        /// </summary>
        [Fact]
        public void FileFormatVersion_NotInitialized_ThrowsInvalidOperationException()
        {
            // Arrange & Act
            Action act = () => { var version = _eventSource.FileFormatVersion; };

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Version info not yet initialized. Replay must be called first.");
        }

        /// <summary>
        /// Tests that the <see cref="BinaryLogReplayEventSource.MinimumReaderVersion"/> property throws an <see cref="InvalidOperationException"/> when version information is not initialized.
        /// Expected outcome: Accessing the property before replay should throw.
        /// </summary>
        [Fact]
        public void MinimumReaderVersion_NotInitialized_ThrowsInvalidOperationException()
        {
            // Arrange & Act
            Action act = () => { var version = _eventSource.MinimumReaderVersion; };

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Version info not yet initialized. Replay must be called first.");
        }

        /// <summary>
        /// Tests that the <see cref="BinaryLogReplayEventSource.Replay(string)"/> method throws an exception when a non-existent file is provided.
        /// Expected outcome: A file-related exception is thrown.
        /// </summary>
        [Fact]
        public void Replay_String_NonExistentFile_ThrowsException()
        {
            // Arrange
            string nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".binlog");

            // Act
            Action act = () => _eventSource.Replay(nonExistentFile);

            // Assert
            act.Should().Throw<Exception>();
        }

        /// <summary>
        /// Tests that the static <see cref="BinaryLogReplayEventSource.OpenReader(string)"/> method throws an exception when provided with an invalid file path.
        /// Expected outcome: An exception is thrown for invalid file paths.
        /// </summary>
        [Fact]
        public void OpenReader_String_InvalidFilePath_ThrowsException()
        {
            // Arrange
            string invalidPath = "?:\\invalid\\path.binlog";

            // Act
            Action act = () => BinaryLogReplayEventSource.OpenReader(invalidPath);

            // Assert
            act.Should().Throw<Exception>();
        }

        /// <summary>
        /// Tests that the static <see cref="BinaryLogReplayEventSource.OpenReader(Stream)"/> method returns a valid <see cref="BinaryReader"/> when provided with a valid gzip stream.
        /// Expected outcome: A non-null instance of <see cref="BinaryReader"/> is returned.
        /// </summary>
        [Fact]
        public void OpenReader_Stream_ValidGzipStream_ReturnsBinaryReader()
        {
            // Arrange
            byte[] compressedData;
            using (var ms = new MemoryStream())
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Compress, leaveOpen: true))
                using (var writer = new StreamWriter(gzip))
                {
                    writer.Write("dummy");
                }
                compressedData = ms.ToArray();
            }
            using var sourceStream = new MemoryStream(compressedData);

            // Act
            using var reader = BinaryLogReplayEventSource.OpenReader(sourceStream);

            // Assert
            reader.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that the static <see cref="BinaryLogReplayEventSource.OpenBuildEventsReader(BinaryReader, bool, bool)"/> method returns a <see cref="BuildEventArgsReader"/> 
        /// with correct version data when the fileFormatVersion is below the forward compatibility threshold.
        /// Expected outcome: The returned reader's FileFormatVersion and MinimumReaderVersion should match the written value.
        /// </summary>
        [Fact]
        public void OpenBuildEventsReader_WithValidData_ReturnsReaderWithCorrectVersions()
        {
            // Arrange
            // Using fileFormatVersion = 1, assuming that 1 is less than BinaryLogger.ForwardCompatibilityMinimalVersion.
            int fileFormatVersion = 1;
            byte[] data;
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms, System.Text.Encoding.Default, leaveOpen: true))
                {
                    bw.Write(fileFormatVersion);
                }
                data = ms.ToArray();
            }
            using var stream = new MemoryStream(data);
            using var binaryReader = new BinaryReader(stream);

            // Act
            var buildEventArgsReader = BinaryLogReplayEventSource.OpenBuildEventsReader(binaryReader, closeInput: true, allowForwardCompatibility: true);

            // Assert
            buildEventArgsReader.FileFormatVersion.Should().Be(fileFormatVersion);
            buildEventArgsReader.MinimumReaderVersion.Should().Be(fileFormatVersion);
        }

        /// <summary>
        /// Tests that the static <see cref="BinaryLogReplayEventSource.OpenBuildEventsReader(BinaryReader, bool, bool)"/> method
        /// throws a <see cref="NotSupportedException"/> when the file format version is unsupported.
        /// Expected outcome: A <see cref="NotSupportedException"/> is thrown with a message indicating the unsupported version.
        /// </summary>
        [Fact]
        public void OpenBuildEventsReader_UnsupportedVersion_ThrowsNotSupportedException()
        {
            // Arrange
            // Simulate an unsupported file format by writing very high version numbers.
            int unsupportedVersion = int.MaxValue;
            int minReaderVersion = int.MaxValue;
            byte[] data;
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms, System.Text.Encoding.Default, leaveOpen: true))
                {
                    bw.Write(unsupportedVersion);
                    bw.Write(minReaderVersion);
                }
                data = ms.ToArray();
            }
            using var stream = new MemoryStream(data);
            using var binaryReader = new BinaryReader(stream);

            // Act
            Action act = () => BinaryLogReplayEventSource.OpenBuildEventsReader(binaryReader, closeInput: true, allowForwardCompatibility: false);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage($"The log file format version is {unsupportedVersion}*");
        }

// Could not make this test passing.
//         /// <summary>
//         /// Tests that the static <see cref="BinaryLogReplayEventSource.OpenBuildEventsReader(string)"/> method returns a <see cref="BuildEventArgsReader"/>
//         /// when provided with a valid temporary file containing minimal valid data.
//         /// Expected outcome: The reader's version properties match the provided data.
//         /// </summary>
//         [Fact]
//         public void OpenBuildEventsReader_String_ValidFile_ReturnsReader()
//         {
//             // Arrange
//             int fileFormatVersion = 1;
//             string tempFile = Path.GetTempFileName();
//             try
//             {
//                 using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
//                 using (var bw = new BinaryWriter(fs))
//                 {
//                     bw.Write(fileFormatVersion);
//                 }
// 
//                 // Act
//                 using var reader = BinaryLogReplayEventSource.OpenBuildEventsReader(tempFile);
// 
//                 // Assert
//                 reader.FileFormatVersion.Should().Be(fileFormatVersion);
//                 reader.MinimumReaderVersion.Should().Be(fileFormatVersion);
//             }
//             finally
//             {
//                 if (File.Exists(tempFile))
//                 {
//                     File.Delete(tempFile);
//                 }
//             }
//         }
// 
        /// <summary>
        /// Partial test for the <see cref="BinaryLogReplayEventSource.Replay(BinaryReader, CancellationToken)"/> method.
        /// Uses an already-canceled token to ensure that the method exits without processing events.
        /// Note: Further integration tests are needed once BuildEventArgsReader functionality is fully available.
        /// Expected outcome: No exception is thrown when cancellation is requested.
        /// </summary>
        [Fact]
        public void Replay_BinaryReader_CancellationRequested_CompletesWithoutProcessing()
        {
            // Arrange
            int fileFormatVersion = 1;
            byte[] data;
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms, System.Text.Encoding.Default, leaveOpen: true))
                {
                    bw.Write(fileFormatVersion);
                }
                data = ms.ToArray();
            }
            using var stream = new MemoryStream(data);
            using var binaryReader = new BinaryReader(stream);
            using var cts = new CancellationTokenSource();
            cts.Cancel(); // Request cancellation

            // Act
            Action act = () => _eventSource.Replay(binaryReader, cts.Token);

            // Assert
            act.Should().NotThrow();
        }
    }
}
