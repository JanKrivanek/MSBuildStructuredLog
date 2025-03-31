using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TreeBinaryReader"/> class.
    /// </summary>
    public class TreeBinaryReaderTests
    {
        /// <summary>
        /// Creates a gzip compressed payload from the provided content bytes.
        /// </summary>
        /// <param name="content">The content to compress.</param>
        /// <returns>Compressed gzip bytes.</returns>
        private byte[] CreateGzipPayload(byte[] content)
        {
            using var memoryStream = new MemoryStream();
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true))
            {
                gzipStream.Write(content, 0, content.Length);
            }
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Creates a MemoryStream simulating the file format of TreeBinaryReader.
        /// The stream consists of a header (3 bytes for version) followed by a gzip compressed payload.
        /// </summary>
        /// <param name="major">Major version byte.</param>
        /// <param name="minor">Minor version byte.</param>
        /// <param name="build">Build version byte.</param>
        /// <param name="payloadContent">The decompressed payload content.</param>
        /// <returns>A MemoryStream ready for reading from the beginning.</returns>
        private MemoryStream CreateTreeReaderStream(byte major, byte minor, byte build, byte[] payloadContent)
        {
            var payload = CreateGzipPayload(payloadContent);
            var ms = new MemoryStream();
            ms.WriteByte(major);
            ms.WriteByte(minor);
            ms.WriteByte(build);
            ms.Write(payload, 0, payload.Length);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// Helper method to create payload content using a BinaryWriter.
        /// </summary>
        /// <param name="action">Action that writes into the BinaryWriter.</param>
        /// <returns>The payload bytes.</returns>
        private byte[] CreatePayload(Action<BinaryWriter> action)
        {
            using var ms = new MemoryStream();
            using (var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true))
            {
                action(writer);
            }
            return ms.ToArray();
        }

        /// <summary>
        /// Tests that the constructor using a stream initializes correctly with a valid gzip payload.
        /// </summary>
        [Fact]
        public void Constructor_StreamValidPayload_InitializesCorrectly()
        {
            // Arrange: Create payload with string table count and strings.
            byte[] payload = CreatePayload(writer =>
            {
                // Write string table (count = 2)
                writer.Write(2);
                writer.Write("first");
                writer.Write("second");
            });

            using var stream = CreateTreeReaderStream(1, 0, 0, payload);

            // Act
            using var reader = new TreeBinaryReader(stream);

            // Assert
            reader.IsValid().Should().BeTrue("because the stream has a valid header and payload");
            reader.Version.Should().Be(new Version(1, 0, 0, 0));
            reader.StringTable.Should().BeEquivalentTo(new[] { "first", "second" });
        }

        /// <summary>
        /// Tests that the constructor returns an invalid reader when the stream is too short.
        /// </summary>
        [Fact]
        public void Constructor_StreamTooShort_IsValidFalse()
        {
            // Arrange: Create a stream with less than 8 bytes.
            byte[] shortData = new byte[7];
            using var stream = new MemoryStream(shortData);

            // Act
            using var reader = new TreeBinaryReader(stream);

            // Assert
            reader.IsValid().Should().BeFalse("because the stream is too short to be valid");
        }

        /// <summary>
        /// Tests that the constructor disposes the stream and returns an invalid reader when the version is unsupported.
        /// </summary>
        [Fact]
        public void Constructor_UnsupportedVersion_IsValidFalse()
        {
            // Arrange: Use an unsupported major version (e.g. 3, since valid range is 1-2).
            byte[] payload = CreatePayload(writer =>
            {
                writer.Write(0); // string table count (can be 0)
            });

            using var stream = CreateTreeReaderStream(3, 0, 0, payload);

            // Act
            using var reader = new TreeBinaryReader(stream);

            // Assert
            reader.IsValid().Should().BeFalse("because the major version is not supported");
        }

        /// <summary>
        /// Tests that the constructor using a stream and Version parameter initializes correctly, overriding header bytes.
        /// </summary>
        [Fact]
        public void Constructor_StreamWithVersionParameter_InitializesCorrectly()
        {
            // Arrange: Provide a valid payload with a string table.
            byte[] payload = CreatePayload(writer =>
            {
                writer.Write(1); // string table count
                writer.Write("override");
            });

            using var stream = CreateTreeReaderStream(0, 0, 0, payload); // header bytes ignored due to version parameter
            var expectedVersion = new Version(2, 0, 0);

            // Act
            using var reader = new TreeBinaryReader(stream, expectedVersion);

            // Assert
            reader.IsValid().Should().BeTrue("because a valid version parameter and payload were provided");
            reader.Version.Should().Be(new Version(2, 0, 0, 0), "because the provided version overrides header values");
            reader.StringTable.Should().BeEquivalentTo(new[] { "override" });
        }

        /// <summary>
        /// Tests that ReadByteArray returns the correct byte array when the length is greater than zero.
        /// </summary>
        [Fact]
        public void ReadByteArray_LengthGreaterThanZero_ReturnsByteArray()
        {
            // Arrange: Create a payload with a string table and a byte array block.
            byte[] payload = CreatePayload(writer =>
            {
                // String table: count 1, with one dummy string.
                writer.Write(1);
                writer.Write("dummy");
                // Byte array data:
                writer.Write(3); // length of byte array
                writer.Write(new byte[] { 1, 2, 3 });
            });

            using var stream = CreateTreeReaderStream(1, 0, 0, payload);
            using var reader = new TreeBinaryReader(stream);

            // Act
            byte[] result = reader.ReadByteArray();

            // Assert
            result.Should().BeEquivalentTo(new byte[] { 1, 2, 3 }, "because the payload contains a byte array of {1,2,3}");
        }

        /// <summary>
        /// Tests that ReadByteArray returns null when the byte array length is zero.
        /// </summary>
        [Fact]
        public void ReadByteArray_ZeroLength_ReturnsNull()
        {
            // Arrange: Create a payload with a string table and a zero-length byte array block.
            byte[] payload = CreatePayload(writer =>
            {
                writer.Write(1);
                writer.Write("dummy");
                writer.Write(0); // length = 0
            });

            using var stream = CreateTreeReaderStream(1, 0, 0, payload);
            using var reader = new TreeBinaryReader(stream);

            // Act
            byte[] result = reader.ReadByteArray();

            // Assert
            result.Should().BeNull("because the byte array length in the payload is zero");
        }

        /// <summary>
        /// Tests that ReadString returns the correct string based on the index provided.
        /// </summary>
        [Fact]
        public void ReadString_ValidIndex_ReturnsCorrectString()
        {
            // Arrange: Create a payload with a string table and an additional integer representing the index.
            byte[] payload = CreatePayload(writer =>
            {
                // String table: count = 1, contains "hello"
                writer.Write(1);
                writer.Write("hello");
                // For ReadString method, write an index (1 means first string).
                writer.Write(1);
            });

            using var stream = CreateTreeReaderStream(1, 0, 0, payload);
            using var reader = new TreeBinaryReader(stream);

            // Act
            string result = reader.ReadString();

            // Assert
            result.Should().Be("hello", "because index 1 in the string table corresponds to 'hello'");
        }

        /// <summary>
        /// Tests that ReadInt32 returns the correct integer value from the payload.
        /// </summary>
        [Fact]
        public void ReadInt32_ReturnsCorrectInteger()
        {
            // Arrange: Create a payload with a string table followed by an integer.
            byte[] payload = CreatePayload(writer =>
            {
                writer.Write(1);
                writer.Write("dummy");
                writer.Write(12345);
            });

            using var stream = CreateTreeReaderStream(1, 0, 0, payload);
            using var reader = new TreeBinaryReader(stream);

            // Act
            int result = reader.ReadInt32();

            // Assert
            result.Should().Be(12345, "because the payload integer was written as 12345");
        }

        /// <summary>
        /// Tests that ReadStringArray correctly enqueues strings based on provided indices.
        /// </summary>
        [Fact]
        public void ReadStringArray_ValidData_PopulatesQueue()
        {
            // Arrange: Create a payload with a string table and a string array block.
            byte[] payload = CreatePayload(writer =>
            {
                // String table: count = 2, contains "a" and "b"
                writer.Write(2);
                writer.Write("a");
                writer.Write("b");
                // For ReadStringArray method, write a count followed by indices.
                writer.Write(2); // count for string array
                writer.Write(1); // index for "a"
                writer.Write(2); // index for "b"
            });

            using var stream = CreateTreeReaderStream(1, 0, 0, payload);
            using var reader = new TreeBinaryReader(stream);
            var queue = new Queue<string>();

            // Act
            reader.ReadStringArray(queue);

            // Assert
            queue.Should().HaveCount(2);
            queue.Dequeue().Should().Be("a");
            queue.Dequeue().Should().Be("b");
        }

        /// <summary>
        /// Tests that ReadStringArray clears the provided queue when the count in payload is zero.
        /// </summary>
        [Fact]
        public void ReadStringArray_ZeroCount_LeavesQueueEmpty()
        {
            // Arrange: Create a payload with a string table and then a zero count for string array.
            byte[] payload = CreatePayload(writer =>
            {
                writer.Write(1);
                writer.Write("dummy");
                writer.Write(0); // string array count = 0
            });

            using var stream = CreateTreeReaderStream(1, 0, 0, payload);
            using var reader = new TreeBinaryReader(stream);
            var queue = new Queue<string>();
            queue.Enqueue("existing");

            // Act
            reader.ReadStringArray(queue);

            // Assert
            queue.Should().BeEmpty("because the payload specified a zero count for the string array");
        }

        /// <summary>
        /// Tests that Dispose properly disposes the internal resources.
        /// </summary>
        [Fact]
        public void Dispose_WhenCalled_ResourcesAreReleased()
        {
            // Arrange: Create a valid payload.
            byte[] payload = CreatePayload(writer =>
            {
                writer.Write(1);
                writer.Write("dummy");
            });
            var stream = CreateTreeReaderStream(1, 0, 0, payload);
            var reader = new TreeBinaryReader(stream);

            // Act
            reader.Dispose();

            // Assert
            reader.IsValid().Should().BeFalse("because after disposal, internal resources are released");
        }
    }
}
