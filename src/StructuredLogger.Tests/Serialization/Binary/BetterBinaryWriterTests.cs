using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.IO;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BetterBinaryWriter"/> class.
    /// </summary>
    public class BetterBinaryWriterTests
    {
        /// <summary>
        /// Tests that the Write method correctly encodes positive integers using 7-bit encoding.
        /// This test uses multiple test cases to ensure that the produced byte array matches the expected outcome.
        /// </summary>
        /// <param name="value">The integer value to write.</param>
        /// <param name="expectedBytes">The expected byte array after 7-bit encoding.</param>
        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 0x01 })]
        [InlineData(127, new byte[] { 0x7F })]
        [InlineData(128, new byte[] { 0x80, 0x01 })]
        [InlineData(255, new byte[] { 0xFF, 0x01 })]
        [InlineData(16384, new byte[] { 0x80, 0x80, 0x01 })]
        public void Write_ValidValue_Writes7BitEncodedIntEquivalent(int value, byte[] expectedBytes)
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            using var writer = new BetterBinaryWriter(memoryStream);

            // Act
            writer.Write(value);
            writer.Flush();
            byte[] actualBytes = memoryStream.ToArray();

            // Assert
            actualBytes.Should().Equal(expectedBytes);
        }

        /// <summary>
        /// Tests that the Write method throws an ArgumentOutOfRangeException when writing a negative value.
        /// The 7-bit encoding method requires non-negative integers.
        /// </summary>
        [Fact]
        public void Write_NegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            using var writer = new BetterBinaryWriter(memoryStream);
            int negativeValue = -1;

            // Act
            Action act = () => writer.Write(negativeValue);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        /// <summary>
        /// Tests that writing with the BetterBinaryWriter after disposing the underlying stream throws an ObjectDisposedException.
        /// This verifies that the writer does not allow operations on a disposed stream.
        /// </summary>
        [Fact]
        public void Write_AfterStreamDisposed_ThrowsObjectDisposedException()
        {
            // Arrange
            var memoryStream = new MemoryStream();
            var writer = new BetterBinaryWriter(memoryStream);
            memoryStream.Dispose();

            // Act
            Action act = () => writer.Write(42);

            // Assert
            act.Should().Throw<ObjectDisposedException>();
        }
    }
}
