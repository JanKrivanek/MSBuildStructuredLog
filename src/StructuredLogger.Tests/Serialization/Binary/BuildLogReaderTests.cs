using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildLogReader"/> class.
    /// </summary>
    public class BuildLogReaderTests
    {
        /// <summary>
        /// Tests that calling Read(string) with a non-existing file path throws a FileNotFoundException.
        /// </summary>
        [Fact]
        public void Read_String_NonExistingFile_ThrowsFileNotFoundException()
        {
            // Arrange
            string nonExistingFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".binlog");

            // Act
            Action act = () => BuildLogReader.Read(nonExistingFilePath);

            // Assert
            act.Should().Throw<FileNotFoundException>();
        }

        /// <summary>
        /// Tests that calling Read(Stream) with an invalid or corrupt stream causes the log format validation to fail,
        /// resulting in an Exception with the message "Invalid log file format".
        /// </summary>
        [Fact]
        public void Read_Stream_InvalidFormat_ThrowsException()
        {
            // Arrange
            // Provide a MemoryStream with arbitrary data that does not represent a valid log file.
            using var memoryStream = new MemoryStream(new byte[10]);

            // Act
            Action act = () => BuildLogReader.Read(memoryStream);

            // Assert
            act.Should().Throw<Exception>()
                .WithMessage("Invalid log file format");
        }

        /// <summary>
        /// Tests that calling Read(Stream, byte[], Version) with an invalid or corrupt stream and a specified version
        /// causes the log format validation to fail, resulting in an Exception with the message "Invalid log file format".
        /// </summary>
        [Fact]
        public void Read_Stream_WithVersion_InvalidFormat_ThrowsException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[10]);
            Version version = new Version(1, 0, 0);

            // Act
            Action act = () => BuildLogReader.Read(memoryStream, null, version);

            // Assert
            act.Should().Throw<Exception>()
                .WithMessage("Invalid log file format");
        }

        /// <summary>
        /// Tests that calling Dispose multiple times on a BuildLogReader instance does not throw an exception.
        /// Since BuildLogReader is normally instantiated internally through its static methods,
        /// reflection is used to invoke the non-public constructor.
        /// </summary>
        [Fact]
        public void Dispose_CalledMultipleTimes_DoesNotThrow()
        {
            // Arrange
            // Create a dummy stream; its content is irrelevant because we will not progress past construction.
            using var dummyStream = new MemoryStream(new byte[10]);
            Version version = new Version(1, 0, 0);
            ConstructorInfo? constructor = typeof(BuildLogReader)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Stream), typeof(Version) }, null);
            constructor.Should().NotBeNull("Expected to find a non-public constructor with parameters (Stream, Version)");

            var instance = (BuildLogReader)constructor!.Invoke(new object[] { dummyStream, version });

            // Act
            Action act = () =>
            {
                instance.Dispose();
                // Calling Dispose again should not throw.
                instance.Dispose();
            };

            // Assert
            act.Should().NotThrow();
        }
    }
}
