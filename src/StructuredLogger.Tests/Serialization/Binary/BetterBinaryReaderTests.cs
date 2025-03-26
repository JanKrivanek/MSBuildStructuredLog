using System;
using System.IO;
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
        /// Helper method to create a memory stream containing the 7-bit encoded representation of the provided integer.
        /// </summary>
        /// <param name="value">The integer value to encode.</param>
        /// <returns>A MemoryStream containing the 7-bit encoded representation of the value.</returns>
        private static MemoryStream Create7BitEncodedStream(int value)
        {
            var ms = new MemoryStream();
            // Emulate the 7-bit encoding used by BinaryWriter.Write7BitEncodedInt
            uint uValue = (uint)value;
            while(uValue >= 0x80)
            {
                ms.WriteByte((byte)(uValue | 0x80));
                uValue >>= 7;
            }
            ms.WriteByte((byte)uValue);
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// Verifies that the constructor of BetterBinaryReader throws an ArgumentNullException when provided with a null stream.
        /// </summary>
        [Fact]
        public void Constructor_NullStream_ThrowsArgumentNullException()
        {
            // Arrange
            Stream nullStream = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new BetterBinaryReader(nullStream));
        }

        /// <summary>
        /// Verifies that the ReadInt32 method correctly decodes valid 7-bit encoded integers from the stream.
        /// </summary>
        /// <param name="expectedValue">The expected integer value after decoding.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(127)]
        [InlineData(128)]
        [InlineData(300)]
        [InlineData(16384)]
        public void ReadInt32_WithValid7BitEncodedData_ReturnsDecodedValue(int expectedValue)
        {
            // Arrange
            using MemoryStream ms = Create7BitEncodedStream(expectedValue);
            using var reader = new BetterBinaryReader(ms);

            // Act
            int actualValue = reader.ReadInt32();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        /// <summary>
        /// Verifies that the ReadInt32 method throws an EndOfStreamException when the stream is empty.
        /// </summary>
        [Fact]
        public void ReadInt32_EmptyStream_ThrowsEndOfStreamException()
        {
            // Arrange
            using var ms = new MemoryStream();
            using var reader = new BetterBinaryReader(ms);

            // Act & Assert
            Assert.Throws<EndOfStreamException>(() => reader.ReadInt32());
        }

        /// <summary>
        /// Verifies that the ReadInt32 method throws an EndOfStreamException when the stream contains incomplete 7-bit encoded data.
        /// </summary>
        [Fact]
        public void ReadInt32_IncompleteData_ThrowsEndOfStreamException()
        {
            // Arrange
            // Create a stream with a single byte that indicates continuation (MSB set) but no additional bytes.
            using var ms = new MemoryStream(new byte[] { 0x80 });
            using var reader = new BetterBinaryReader(ms);

            // Act & Assert
            Assert.Throws<EndOfStreamException>(() => reader.ReadInt32());
        }
    }
}
