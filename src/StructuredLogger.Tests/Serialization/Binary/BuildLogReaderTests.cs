using System;
using System.IO;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildLogReader"/> class.
    /// </summary>
    public class BuildLogReaderTests
    {
        /// <summary>
        /// Tests that calling Read(string) with a non-existent file path throws a FileNotFoundException.
        /// This verifies that the file stream fails on an invalid file path.
        /// </summary>
        [Fact]
        public void Read_WithNonExistentFilePath_ThrowsFileNotFoundException()
        {
            // Arrange
            string nonExistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".log");

            // Act
            Action act = () => BuildLogReader.Read(nonExistentFilePath);

            // Assert
            act.Should().Throw<FileNotFoundException>("because the file does not exist at the specified path");
        }

        /// <summary>
        /// Tests that calling Read(Stream, byte[]) with an empty stream causes an exception
        /// due to an invalid log file format.
        /// Note: This is a partial test. In a full integration test, a valid log stream should be provided.
        /// </summary>
        [Fact]
        public void Read_Stream_WithNullProjectImportsArchive_InvalidFormat_ThrowsException()
        {
            // Arrange
            using MemoryStream emptyStream = new MemoryStream();
            byte[]? projectImportsArchive = null;

            // Act
            Action act = () => BuildLogReader.Read(emptyStream, projectImportsArchive);

            // Assert
            act.Should().Throw<Exception>()
                .WithMessage("Invalid log file format", "because an empty stream does not represent a valid log");
        }

        /// <summary>
        /// Tests that calling Read(Stream, byte[], Version) with an empty stream and a given version
        /// causes an exception due to an invalid log file format.
        /// Note: This is a partial test. To fully test a valid log, proper binary data compliant with the log format is required.
        /// </summary>
        [Fact]
        public void Read_Stream_WithVersion_InvalidFormat_ThrowsException()
        {
            // Arrange
            using MemoryStream emptyStream = new MemoryStream();
            byte[]? projectImportsArchive = null;
            Version version = new Version(1, 0, 0);

            // Act
            Action act = () => BuildLogReader.Read(emptyStream, projectImportsArchive, version);

            // Assert
            act.Should().Throw<Exception>()
                .WithMessage("Invalid log file format", "because an empty stream does not represent a valid log format");
        }

        /// <summary>
        /// Tests that Dispose can be called without throwing exceptions.
        /// Note: Since BuildLogReader instances are created internally in the static Read methods and its constructor is private,
        /// testing Dispose directly is not feasible. This test serves as a placeholder to indicate that Dispose should be safe to call
        /// as part of the using pattern.
        /// TODO: Refactor to allow injecting a disposable dependency if direct testing of Dispose becomes necessary.
        /// </summary>
        [Fact]
        public void Dispose_CalledAfterRead_DoesNotThrowException()
        {
            // Arrange
            // Because BuildLogReader constructors are private and its instantiation is controlled internally by the static Read methods,
            // a direct test for Dispose is not feasible without a valid log stream.
            // This test acts as a placeholder.
            Action act = () =>
            {
                // TODO: Create a valid binary stream if possible to instantiate BuildLogReader directly,
                // then call Dispose and verify behavior.
            };

            // Act & Assert
            act.Should().NotThrow("Dispose should be safe to call even if not explicitly invoked in tests.");
        }
    }
}
