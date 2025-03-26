using System;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Span"/> struct.
    /// </summary>
    public class SpanTests
    {
        /// <summary>
        /// Tests that the constructor correctly sets the Start and Length properties and that the End property returns the sum of Start and Length.
        /// </summary>
        /// <param name="start">The starting position of the Span.</param>
        /// <param name="length">The length of the Span.</param>
        [Theory]
        [InlineData(0, 0)]
        [InlineData(5, 10)]
        [InlineData(-3, 7)]
        public void Constructor_WithStartAndLength_SetsPropertiesCorrectly(int start, int length)
        {
            // Arrange & Act
            var span = new Span(start, length);

            // Assert
            Assert.Equal(start, span.Start);
            Assert.Equal(length, span.Length);
            Assert.Equal(start + length, span.End);
        }

        /// <summary>
        /// Tests the ToString method to ensure it returns the expected string representation of the Span.
        /// </summary>
        /// <param name="start">The starting position of the Span.</param>
        /// <param name="length">The length of the Span.</param>
        /// <param name="expected">The expected string representation.</param>
        [Theory]
        [InlineData(0, 0, "(0, 0)")]
        [InlineData(5, 10, "(5, 10)")]
        [InlineData(-3, 7, "(-3, 7)")]
        public void ToString_ReturnsCorrectFormat(int start, int length, string expected)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            string actual = span.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the Skip method to ensure that when the skip length is within the Span's length, the returned Span has adjusted Start and reduced Length.
        /// </summary>
        /// <param name="start">Initial Start value.</param>
        /// <param name="length">Initial Length value.</param>
        /// <param name="skipLength">The length to skip.</param>
        /// <param name="expectedStart">The expected Start after skipping.</param>
        /// <param name="expectedLength">The expected Length after skipping.</param>
        [Theory]
        [InlineData(10, 20, 5, 15, 15)]
        [InlineData(0, 10, 0, 0, 10)]
        [InlineData(10, 30, 10, 20, 20)]
        public void Skip_WhenSkipLengthIsWithinSpan_ReturnsAdjustedSpan(int start, int length, int skipLength, int expectedStart, int expectedLength)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            var result = span.Skip(skipLength);

            // Assert
            Assert.Equal(expectedStart, result.Start);
            Assert.Equal(expectedLength, result.Length);
        }

        /// <summary>
        /// Tests the Skip method to ensure that when the skip length exceeds the Span's length, the returned Span is empty.
        /// </summary>
        /// <param name="start">Initial Start value.</param>
        /// <param name="length">Initial Length value.</param>
        /// <param name="skipLength">The length to skip that exceeds the current Length.</param>
        [Theory]
        [InlineData(10, 5, 6)]
        [InlineData(0, 0, 1)]
        public void Skip_WhenSkipLengthExceedsSpan_ReturnsEmptySpan(int start, int length, int skipLength)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            var result = span.Skip(skipLength);

            // Assert
            // Empty span should have default values (0 for Start and Length).
            Assert.Equal(0, result.Start);
            Assert.Equal(0, result.Length);
            Assert.Equal(0, result.End);
        }

        /// <summary>
        /// Tests the ContainsEndInclusive method to ensure it correctly includes the end of the Span in its range evaluation.
        /// </summary>
        /// <param name="start">The starting position of the Span.</param>
        /// <param name="length">The length of the Span.</param>
        /// <param name="position">The position to check.</param>
        /// <param name="expected">Expected result indicating if the position is contained (end inclusive).</param>
        [Theory]
        [InlineData(10, 5, 10, true)]
        [InlineData(10, 5, 15, true)]
        [InlineData(10, 5, 16, false)]
        [InlineData(10, 5, 9, false)]
        public void ContainsEndInclusive_ReturnsCorrectResult(int start, int length, int position, bool expected)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            bool actual = span.ContainsEndInclusive(position);

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the Contains method to ensure it correctly excludes the end of the Span in its range evaluation.
        /// </summary>
        /// <param name="start">The starting position of the Span.</param>
        /// <param name="length">The length of the Span.</param>
        /// <param name="position">The position to check.</param>
        /// <param name="expected">Expected result indicating if the position is contained (end exclusive).</param>
        [Theory]
        [InlineData(10, 5, 10, true)]
        [InlineData(10, 5, 14, true)]
        [InlineData(10, 5, 15, false)]
        [InlineData(10, 5, 9, false)]
        public void Contains_ReturnsCorrectResult(int start, int length, int position, bool expected)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            bool actual = span.Contains(position);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
