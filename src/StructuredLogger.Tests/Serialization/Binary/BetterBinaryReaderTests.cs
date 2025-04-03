using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.IO;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BetterBinaryReader"/> class.
    /// </summary>
    public class BetterBinaryReaderTests
    {
        /// <summary>
        /// Tests that the constructor of <see cref="BetterBinaryReader"/> throws an ArgumentNullException when a null stream is provided.
        /// </summary>
        [Fact]
        public void Constructor_NullStream_ThrowsArgumentNullException()
        {
            // Arrange
            Stream inputStream = null!;
            
            // Act
            Action act = () => new BetterBinaryReader(inputStream);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Tests that <see cref="BetterBinaryReader.ReadInt32"/> correctly reads a 7-bit encoded integer from a stream (happy path).
        /// Uses encoded bytes for integer value 300.
        /// </summary>
        [Fact]
        public void ReadInt32_ValidEncoding_ReturnsExpectedValue()
        {
            // Arrange
            // The 7-bit encoded form for 300 is: 0xAC, 0x02.
            byte[] encodedValue = new byte[] { 0xAC, 0x02 };
            using var memoryStream = new MemoryStream(encodedValue);
            using var reader = new BetterBinaryReader(memoryStream);
            int expectedValue = 300;

            // Act
            int actualValue = reader.ReadInt32();

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests that <see cref="BetterBinaryReader.ReadInt32"/> correctly reads a 7-bit encoded integer for zero.
        /// </summary>
        [Fact]
        public void ReadInt32_WithZero_ReturnsZero()
        {
            // Arrange
            // Zero is encoded as a single byte: 0x00.
            byte[] encodedValue = new byte[] { 0x00 };
            using var memoryStream = new MemoryStream(encodedValue);
            using var reader = new BetterBinaryReader(memoryStream);
            int expectedValue = 0;

            // Act
            int actualValue = reader.ReadInt32();

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests that <see cref="BetterBinaryReader.ReadInt32"/> throws an EndOfStreamException when the encoded value is incomplete.
        /// For example, when the continuation bit is set but no further byte is provided.
        /// </summary>
        [Fact]
        public void ReadInt32_IncompleteEncodedValue_ThrowsEndOfStreamException()
        {
            // Arrange
            // 0x80 indicates that there is a continuation but no further bytes are provided.
            byte[] encodedValue = new byte[] { 0x80 };
            using var memoryStream = new MemoryStream(encodedValue);
            using var reader = new BetterBinaryReader(memoryStream);

            // Act
            Action act = () => reader.ReadInt32();

            // Assert
            act.Should().Throw<EndOfStreamException>();
        }

        /// <summary>
        /// Tests that <see cref="BetterBinaryReader.ReadInt32"/> throws a FormatException when the encoded integer is too large (malformed).
        /// Provides 6 bytes with continuation bits set where applicable which exceeds the 5-byte limit.
        /// </summary>
        [Fact]
        public void ReadInt32_MalformedEncodedValue_ThrowsFormatException()
        {
            // Arrange
            // A malformed 7-bit encoded integer that uses more than 5 bytes.
            // For example: 0x80, 0x80, 0x80, 0x80, 0x80, 0x00
            byte[] encodedValue = new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x00 };
            using var memoryStream = new MemoryStream(encodedValue);
            using var reader = new BetterBinaryReader(memoryStream);

            // Act
            Action act = () => reader.ReadInt32();

            // Assert
            act.Should().Throw<FormatException>();
        }
    }
}
