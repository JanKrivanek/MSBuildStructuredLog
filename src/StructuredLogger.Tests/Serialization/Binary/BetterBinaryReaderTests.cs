using System;
using System.IO;
using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BetterBinaryReader"/> class.
    /// </summary>
    public class BetterBinaryReaderTests
    {
        /// <summary>
        /// Verifies that constructing a BetterBinaryReader with a null stream throws an ArgumentNullException.
        /// </summary>
        [Fact]
        public void Constructor_NullStream_ThrowsArgumentNullException()
        {
            // Arrange
            Stream nullStream = null!;

            // Act
            Action act = () => new BetterBinaryReader(nullStream);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Verifies that ReadInt32 returns 0 when the stream contains the 7-bit encoded representation for 0.
        /// </summary>
        [Fact]
        public void ReadInt32_WithZeroValue_ReturnsZero()
        {
            // Arrange: 0 encoded as a single byte (0x00).
            byte[] data = new byte[] { 0x00 };
            using var stream = new MemoryStream(data);
            using var reader = new BetterBinaryReader(stream);

            // Act
            int result = reader.ReadInt32();

            // Assert
            result.Should().Be(0);
        }

        /// <summary>
        /// Verifies that ReadInt32 returns the correct integer when the stream contains a single-byte encoded value.
        /// </summary>
        [Fact]
        public void ReadInt32_WithSingleByteEncodedValue_ReturnsCorrectValue()
        {
            // Arrange: 1 encoded as a single byte (0x01).
            byte[] data = new byte[] { 0x01 };
            using var stream = new MemoryStream(data);
            using var reader = new BetterBinaryReader(stream);

            // Act
            int result = reader.ReadInt32();

            // Assert
            result.Should().Be(1);
        }

        /// <summary>
        /// Verifies that ReadInt32 returns the correct integer for a multi-byte 7-bit encoded value.
        /// </summary>
        [Fact]
        public void ReadInt32_WithMultiByteEncodedValue_ReturnsCorrectValue()
        {
            // Arrange: 300 encoded in 7-bit format (0xAC, 0x02).
            byte[] data = new byte[] { 0xAC, 0x02 };
            using var stream = new MemoryStream(data);
            using var reader = new BetterBinaryReader(stream);

            // Act
            int result = reader.ReadInt32();

            // Assert
            result.Should().Be(300);
        }

        /// <summary>
        /// Verifies that ReadInt32 throws an EndOfStreamException when the stream ends prematurely while reading a multi-byte encoded integer.
        /// </summary>
        [Fact]
        public void ReadInt32_WhenStreamEndsPrematurely_ThrowsEndOfStreamException()
        {
            // Arrange: Incomplete encoding - 0x81 indicates continuation but next byte is missing.
            byte[] data = new byte[] { 0x81 };
            using var stream = new MemoryStream(data);
            using var reader = new BetterBinaryReader(stream);

            // Act
            Action act = () => reader.ReadInt32();

            // Assert
            act.Should().Throw<EndOfStreamException>();
        }
    }
}
