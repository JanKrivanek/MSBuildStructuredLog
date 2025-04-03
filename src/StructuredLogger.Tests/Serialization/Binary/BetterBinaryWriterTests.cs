using System;
using System.IO;
using FluentAssertions;
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
        /// Tests that the constructor throws an ArgumentNullException when passed a null stream.
        /// Expected outcome: The constructor should not accept a null stream and must throw an ArgumentNullException.
        /// </summary>
        [Fact]
        public void Constructor_NullStream_ThrowsArgumentNullException()
        {
            // Arrange
            Stream? nullStream = null;
            
            // Act
            Action act = () => new BetterBinaryWriter(nullStream!);
            
            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Tests that calling Write with a value of 0 writes the expected 7-bit encoded bytes.
        /// Expected outcome: The underlying stream should contain a single byte 0x00.
        /// </summary>
        [Fact]
        public void Write_WhenCalledWithZero_WritesExpectedBytes()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            using var writer = new BetterBinaryWriter(memoryStream);
            int value = 0;
            byte[] expectedBytes = { 0x00 };

            // Act
            writer.Write(value);
            writer.Flush();
            byte[] actualBytes = memoryStream.ToArray();

            // Assert
            actualBytes.Should().Equal(expectedBytes);
        }

        /// <summary>
        /// Tests that calling Write with the value 300 writes the correct 7-bit encoded bytes.
        /// Expected outcome: The underlying stream should contain the bytes { 0xAC, 0x02 }.
        /// </summary>
        [Fact]
        public void Write_WhenCalledWith300_WritesExpectedBytes()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            using var writer = new BetterBinaryWriter(memoryStream);
            int value = 300;
            // Calculation:
            // First iteration: (300 & 0x7F) = 44, with continuation bit becomes 44 | 0x80 = 172 (0xAC)
            // Second iteration: 300 >> 7 = 2, which is written as 0x02.
            byte[] expectedBytes = { 0xAC, 0x02 };

            // Act
            writer.Write(value);
            writer.Flush();
            byte[] actualBytes = memoryStream.ToArray();

            // Assert
            actualBytes.Should().Equal(expectedBytes);
        }

        /// <summary>
        /// Tests that calling Write with Int32.MaxValue writes the correct 7-bit encoded bytes.
        /// Expected outcome: The underlying stream should contain 5 bytes.
        /// Calculation:
        ///   Iteration 1: (int.MaxValue & 0x7F) = 127, |0x80 gives 255.
        ///   Iteration 2: (int.MaxValue >> 7) & 0x7F gives 127, |0x80 gives 255.
        ///   Iteration 3: (int.MaxValue >> 14) & 0x7F gives 127, |0x80 gives 255.
        ///   Iteration 4: (int.MaxValue >> 21) & 0x7F gives 127, |0x80 gives 255.
        ///   Iteration 5: (int.MaxValue >> 28) = 7, written as 0x07.
        /// </summary>
        [Fact]
        public void Write_WhenCalledWithInt32MaxValue_WritesExpectedBytes()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            using var writer = new BetterBinaryWriter(memoryStream);
            int value = int.MaxValue;
            byte[] expectedBytes = { 0xFF, 0xFF, 0xFF, 0xFF, 0x07 };

            // Act
            writer.Write(value);
            writer.Flush();
            byte[] actualBytes = memoryStream.ToArray();

            // Assert
            actualBytes.Should().Equal(expectedBytes);
        }

        // Note: Testing negative values for Write is omitted because Write7BitEncodedInt does not support negative numbers and may lead to infinite loops.
    }
}
