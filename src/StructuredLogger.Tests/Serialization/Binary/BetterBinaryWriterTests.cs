using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BetterBinaryWriter"/> class.
    /// </summary>
    public class BetterBinaryWriterTests
    {
        /// <summary>
        /// Tests that constructing a BetterBinaryWriter with a null stream throws an ArgumentNullException.
        /// </summary>
        [Fact]
        public void Constructor_NullStream_ThrowsArgumentNullException()
        {
            // Arrange
            Stream nullStream = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new BetterBinaryWriter(nullStream));
        }

        /// <summary>
        /// Tests that calling Write on BetterBinaryWriter writes the expected 7-bit encoded byte sequence for various integer values.
        /// </summary>
        /// <param name="value">The integer value to be written.</param>
        /// <param name="expectedBytes">The expected byte array produced by 7-bit encoding.</param>
        [Theory]
        [MemberData(nameof(GetIntTestData))]
        public void WriteInt_WhenCalled_WritesCorrect7BitEncodedInt(int value, byte[] expectedBytes)
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            using var writer = new BetterBinaryWriter(memoryStream);

            // Act
            writer.Write(value);
            writer.Flush();
            byte[] actualBytes = memoryStream.ToArray();

            // Assert
            Assert.Equal(expectedBytes, actualBytes);
        }

        /// <summary>
        /// Provides a collection of test cases with integer values and their corresponding expected 7-bit encoded byte arrays.
        /// </summary>
        public static IEnumerable<object[]> GetIntTestData()
        {
            yield return new object[] { 0, Get7BitEncodedIntBytes(0) };
            yield return new object[] { 127, Get7BitEncodedIntBytes(127) };
            yield return new object[] { 128, Get7BitEncodedIntBytes(128) };
            yield return new object[] { 16384, Get7BitEncodedIntBytes(16384) };
            yield return new object[] { -1, Get7BitEncodedIntBytes(-1) };
        }

        /// <summary>
        /// Mimics the behavior of BinaryWriter.Write7BitEncodedInt to generate the expected byte array for an integer.
        /// </summary>
        /// <param name="value">The integer value to encode.</param>
        /// <returns>A byte array representing the 7-bit encoded integer.</returns>
        private static byte[] Get7BitEncodedIntBytes(int value)
        {
            var bytes = new List<byte>();
            uint v = (uint)value;
            while (v >= 0x80)
            {
                bytes.Add((byte)((v & 0x7F) | 0x80));
                v >>= 7;
            }
            bytes.Add((byte)v);
            return bytes.ToArray();
        }
    }
}
