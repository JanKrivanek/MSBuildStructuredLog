using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Span"/> struct.
    /// </summary>
    public class SpanTests
    {
        /// <summary>
        /// Tests the End property to ensure it correctly returns Start + Length.
        /// </summary>
        /// <param name="start">The starting position of the span.</param>
        /// <param name="length">The length of the span.</param>
        /// <param name="expectedEnd">The expected end position (Start + Length).</param>
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(5, 3, 8)]
        [InlineData(-2, 4, 2)]
        [InlineData(10, 0, 10)]
        public void EndProperty_WhenCalled_ReturnsCorrectSum(int start, int length, int expectedEnd)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            int actualEnd = span.End;

            // Assert
            actualEnd.Should().Be(expectedEnd);
        }

        /// <summary>
        /// Tests the ToString method to verify it returns the formatted string "(Start, Length)".
        /// </summary>
        /// <param name="start">The starting position of the span.</param>
        /// <param name="length">The length of the span.</param>
        /// <param name="expectedString">The expected string representation of the span.</param>
        [Theory]
        [InlineData(0, 0, "(0, 0)")]
        [InlineData(2, 10, "(2, 10)")]
        [InlineData(-5, 3, "(-5, 3)")]
        public void ToString_WhenCalled_ReturnsCorrectFormat(int start, int length, string expectedString)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            string result = span.ToString();

            // Assert
            result.Should().Be(expectedString);
        }

        /// <summary>
        /// Tests the Skip method when the skip amount is less than the current span's length.
        /// The expected outcome is a new Span with Start incremented by the skip amount and Length reduced accordingly.
        /// </summary>
        [Fact]
        public void Skip_WhenSkipLessThanLength_ReturnsAdjustedSpan()
        {
            // Arrange
            var originalSpan = new Span(10, 5);
            int skipAmount = 3;
            var expectedSpan = new Span(10 + skipAmount, 5 - skipAmount);

            // Act
            Span result = originalSpan.Skip(skipAmount);

            // Assert
            result.Start.Should().Be(expectedSpan.Start);
            result.Length.Should().Be(expectedSpan.Length);
        }

        /// <summary>
        /// Tests the Skip method when the skip amount equals the current span's length.
        /// The expected outcome is a new Span with the Start increased by the skip amount and a zero Length.
        /// </summary>
        [Fact]
        public void Skip_WhenSkipEqualsLength_ReturnsSpanWithZeroLength()
        {
            // Arrange
            var originalSpan = new Span(7, 4);
            int skipAmount = 4;
            var expectedSpan = new Span(7 + skipAmount, 0);

            // Act
            Span result = originalSpan.Skip(skipAmount);

            // Assert
            result.Start.Should().Be(expectedSpan.Start);
            result.Length.Should().Be(expectedSpan.Length);
        }

        /// <summary>
        /// Tests the Skip method when the skip amount is greater than the current span's length.
        /// The expected outcome is an empty Span.
        /// </summary>
        [Fact]
        public void Skip_WhenSkipGreaterThanLength_ReturnsEmptySpan()
        {
            // Arrange
            var originalSpan = new Span(7, 4);
            int skipAmount = 5;

            // Act
            Span result = originalSpan.Skip(skipAmount);

            // Assert
            result.Should().Be(Span.Empty);
        }

        /// <summary>
        /// Tests the ContainsEndInclusive method to verify it correctly determines if a given position is within the span, including the end.
        /// </summary>
        /// <param name="start">The starting position of the span.</param>
        /// <param name="length">The length of the span.</param>
        /// <param name="position">The position to test.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [InlineData(10, 10, 10, true)]  // Position at start boundary.
        [InlineData(10, 10, 20, true)]  // Position at end boundary (inclusive).
        [InlineData(10, 10, 15, true)]  // Position inside the span.
        [InlineData(10, 10, 9, false)]  // Position before the span.
        [InlineData(10, 10, 21, false)] // Position after the span.
        public void ContainsEndInclusive_WhenCalled_ReturnsExpectedResult(int start, int length, int position, bool expected)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            bool result = span.ContainsEndInclusive(position);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the Contains method to verify it correctly determines if a given position is within the span, excluding the end.
        /// </summary>
        /// <param name="start">The starting position of the span.</param>
        /// <param name="length">The length of the span.</param>
        /// <param name="position">The position to test.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [InlineData(10, 10, 10, true)]   // Position at start boundary.
        [InlineData(10, 10, 19, true)]   // Position just before the end boundary since End is exclusive.
        [InlineData(10, 10, 20, false)]  // Position at end boundary (exclusive).
        [InlineData(10, 10, 9, false)]   // Position before the span.
        [InlineData(10, 10, 21, false)]  // Position after the span.
        public void Contains_WhenCalled_ReturnsExpectedResult(int start, int length, int position, bool expected)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            bool result = span.Contains(position);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the Empty static property to verify it returns a default Span with both Start and Length equal to zero.
        /// </summary>
        [Fact]
        public void EmptyProperty_WhenCalled_ReturnsDefaultSpan()
        {
            // Arrange & Act
            Span emptySpan = Span.Empty;
            Span defaultSpan = new Span();

            // Assert
            emptySpan.Start.Should().Be(0);
            emptySpan.Length.Should().Be(0);
            emptySpan.Should().Be(defaultSpan);
        }
    }
}
