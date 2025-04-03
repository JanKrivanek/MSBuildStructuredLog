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
        /// Tests that the constructor initializes the Start and Length fields correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithValidValues_InitializesFieldsCorrectly()
        {
            // Arrange
            int expectedStart = 5;
            int expectedLength = 10;
            int expectedEnd = expectedStart + expectedLength;

            // Act
            Span span = new Span(expectedStart, expectedLength);

            // Assert
            span.Start.Should().Be(expectedStart, "the Start field should be set by the constructor");
            span.Length.Should().Be(expectedLength, "the Length field should be set by the constructor");
            span.End.Should().Be(expectedEnd, "the End property should return the sum of Start and Length");
        }

        /// <summary>
        /// Tests that the End property correctly returns the sum of Start and Length.
        /// </summary>
        [Fact]
        public void EndProperty_ShouldReturnSumOfStartAndLength()
        {
            // Arrange
            int start = 7;
            int length = 3;
            int expectedEnd = start + length;

            // Act
            Span span = new Span(start, length);

            // Assert
            span.End.Should().Be(expectedEnd, "the End property must equal Start plus Length");
        }

        /// <summary>
        /// Tests that the ToString method returns the expected format.
        /// </summary>
        [Theory]
        [InlineData(0, 0, "(0, 0)")]
        [InlineData(1, 2, "(1, 2)")]
        [InlineData(-5, 10, "(-5, 10)")]
        public void ToString_WhenCalled_ReturnsCorrectFormat(int start, int length, string expectedString)
        {
            // Arrange
            Span span = new Span(start, length);

            // Act
            string actual = span.ToString();

            // Assert
            actual.Should().Be(expectedString, "ToString should format the Span as '(Start, Length)'");
        }

        /// <summary>
        /// Tests that Skip returns a new Span with adjusted Start and Length when length to skip is less than current Length.
        /// </summary>
        [Fact]
        public void Skip_WhenSkipLessThanLength_ReturnsAdjustedSpan()
        {
            // Arrange
            Span span = new Span(10, 5);
            int skipLength = 3;
            int expectedStart = span.Start + skipLength;
            int expectedLength = span.Length - skipLength;

            // Act
            Span result = span.Skip(skipLength);

            // Assert
            result.Start.Should().Be(expectedStart, "Skip should add the skip length to the Start");
            result.Length.Should().Be(expectedLength, "Skip should subtract the skip length from the Length");
        }

        /// <summary>
        /// Tests that Skip returns a Span with zero Length when the skip value equals the current Length.
        /// </summary>
        [Fact]
        public void Skip_WhenSkipEqualToLength_ReturnsSpanWithZeroLength()
        {
            // Arrange
            Span span = new Span(10, 5);
            int skipLength = 5;
            int expectedStart = span.Start + skipLength;
            int expectedLength = 0;

            // Act
            Span result = span.Skip(skipLength);

            // Assert
            result.Start.Should().Be(expectedStart, "Skip should adjust the Start correctly when skip equals Length");
            result.Length.Should().Be(expectedLength, "Skip should result in a Span with Length zero when skip equals original Length");
        }

        /// <summary>
        /// Tests that Skip returns an empty Span when the skip value is greater than the current Length.
        /// </summary>
        [Fact]
        public void Skip_WhenSkipGreaterThanLength_ReturnsEmptySpan()
        {
            // Arrange
            Span span = new Span(10, 5);
            int skipLength = 6;

            // Act
            Span result = span.Skip(skipLength);

            // Assert
            result.Should().Be(Span.Empty, "Skip should return an empty Span when skip value exceeds the available length");
        }

        /// <summary>
        /// Tests that Skip adjusts the Span correctly even when a negative skip value is provided.
        /// </summary>
        [Fact]
        public void Skip_WhenNegativeSkip_ReturnsAdjustedSpanAccordingly()
        {
            // Arrange
            Span span = new Span(10, 5);
            int skipLength = -2;
            int expectedStart = span.Start + skipLength;
            int expectedLength = span.Length - skipLength;

            // Act
            Span result = span.Skip(skipLength);

            // Assert
            result.Start.Should().Be(expectedStart, "A negative skip should decrease the Start accordingly");
            result.Length.Should().Be(expectedLength, "A negative skip should increase the Length accordingly");
        }

        /// <summary>
        /// Tests that ContainsEndInclusive returns true for positions that are within the inclusive range.
        /// </summary>
        [Theory]
        [InlineData(5, 10, 5)]
        [InlineData(5, 10, 10)]
        [InlineData(5, 10, 14)]
        [InlineData(5, 10, 15)]
        public void ContainsEndInclusive_WhenPositionWithinOrAtBounds_ReturnsTrue(int start, int length, int position)
        {
            // Arrange
            Span span = new Span(start, length);

            // Act
            bool contains = span.ContainsEndInclusive(position);

            // Assert
            contains.Should().BeTrue("the position is within the inclusive range defined by Start and End");
        }

        /// <summary>
        /// Tests that ContainsEndInclusive returns false for positions outside the inclusive range.
        /// </summary>
        [Theory]
        [InlineData(5, 10, 4)]
        [InlineData(5, 10, 16)]
        public void ContainsEndInclusive_WhenPositionOutsideBounds_ReturnsFalse(int start, int length, int position)
        {
            // Arrange
            Span span = new Span(start, length);

            // Act
            bool contains = span.ContainsEndInclusive(position);

            // Assert
            contains.Should().BeFalse("the position is outside the inclusive range defined by Start and End");
        }

        /// <summary>
        /// Tests that Contains returns true for positions that are within the half-open range [Start, End).
        /// </summary>
        [Theory]
        [InlineData(5, 10, 5)]
        [InlineData(5, 10, 14)]
        public void Contains_WhenPositionWithinExclusiveEnd_ReturnsTrue(int start, int length, int position)
        {
            // Arrange
            Span span = new Span(start, length);

            // Act
            bool contains = span.Contains(position);

            // Assert
            contains.Should().BeTrue("the position is within the half-open range [Start, End)");
        }

        /// <summary>
        /// Tests that Contains returns false for positions that are outside the half-open range [Start, End).
        /// </summary>
        [Theory]
        [InlineData(5, 10, 4)]
        [InlineData(5, 10, 15)]
        public void Contains_WhenPositionOutsideExclusiveEnd_ReturnsFalse(int start, int length, int position)
        {
            // Arrange
            Span span = new Span(start, length);

            // Act
            bool contains = span.Contains(position);

            // Assert
            contains.Should().BeFalse("the position is outside the half-open range [Start, End)");
        }

        /// <summary>
        /// Tests that the Empty field returns a Span with zero Start, Length, and End values.
        /// </summary>
        [Fact]
        public void EmptyField_ShouldReturnDefaultSpan()
        {
            // Act
            Span emptySpan = Span.Empty;

            // Assert
            emptySpan.Start.Should().Be(0, "Empty Span should have Start equal to 0");
            emptySpan.Length.Should().Be(0, "Empty Span should have Length equal to 0");
            emptySpan.End.Should().Be(0, "Empty Span should have End equal to 0");
            emptySpan.ToString().Should().Be("(0, 0)", "Empty Span should be represented as '(0, 0)'");
        }

        /// <summary>
        /// Tests the constructor with boundary values to ensure the End property calculates correctly with extreme inputs.
        /// </summary>
        [Fact]
        public void Constructor_WithExtremeValues_CalculatesEndCorrectly()
        {
            // Arrange
            int start = int.MinValue;
            int length = 10;
            int expectedEnd = start + length;

            // Act
            Span span = new Span(start, length);

            // Assert
            span.Start.Should().Be(start, "the Start field should be correctly assigned even for extreme values");
            span.Length.Should().Be(length, "the Length field should be correctly assigned even for extreme values");
            span.End.Should().Be(expectedEnd, "the End property should correctly compute the sum for extreme values");
        }
    }
}
