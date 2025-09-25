using System;

using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace StructuredLogger.Tests
{
    public class SpanTests
    {
        /// <summary>
        /// Tests the End property calculation under various Start and Length combinations.
        /// Verifies that End correctly returns Start + Length for all edge cases including
        /// integer overflow scenarios, boundary values, and negative numbers.
        /// </summary>
        /// <param name="start">The Start field value to test</param>
        /// <param name="length">The Length field value to test</param>
        /// <param name="expectedEnd">The expected End property result</param>
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(5, 10, 15)]
        [InlineData(-5, 10, 5)]
        [InlineData(10, -5, 5)]
        [InlineData(-10, -5, -15)]
        [InlineData(100, 200, 300)]
        [InlineData(-100, -200, -300)]
        [InlineData(int.MaxValue, 0, int.MaxValue)]
        [InlineData(0, int.MaxValue, int.MaxValue)]
        [InlineData(int.MinValue, 0, int.MinValue)]
        [InlineData(0, int.MinValue, int.MinValue)]
        [InlineData(int.MaxValue, 1, int.MinValue)] // Overflow case
        [InlineData(int.MinValue, -1, int.MaxValue)] // Underflow case
        [InlineData(1, int.MaxValue, int.MinValue)] // Overflow case
        [InlineData(-1, int.MinValue, int.MaxValue)] // Underflow case
        [InlineData(int.MaxValue / 2, int.MaxValue / 2, int.MaxValue - 1)]
        [InlineData(int.MinValue / 2, int.MinValue / 2, int.MinValue)]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void End_WithVariousStartAndLengthValues_ReturnsStartPlusLength(int start, int length, int expectedEnd)
        {
            // Arrange
            var span = new Span
            {
                Start = start,
                Length = length
            };

            // Act
            int actualEnd = span.End;

            // Assert
            actualEnd.Should().Be(expectedEnd);
        }

        /// <summary>
        /// Tests the End property calculation using the constructor.
        /// Verifies that End correctly returns Start + Length when the Span is created via constructor.
        /// </summary>
        /// <param name="start">The start parameter for the constructor</param>
        /// <param name="length">The length parameter for the constructor</param>
        /// <param name="expectedEnd">The expected End property result</param>
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(10, 20, 30)]
        [InlineData(-10, 15, 5)]
        [InlineData(50, -25, 25)]
        [InlineData(int.MaxValue, 0, int.MaxValue)]
        [InlineData(int.MinValue, 0, int.MinValue)]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void End_WithConstructorInitializedSpan_ReturnsStartPlusLength(int start, int length, int expectedEnd)
        {
            // Arrange & Act
            var span = new Span(start, length);

            // Assert
            span.End.Should().Be(expectedEnd);
        }

        /// <summary>
        /// Tests that the Span constructor correctly assigns positive values to Start and Length fields.
        /// </summary>
        /// <param name="start">The start position value</param>
        /// <param name="length">The length value</param>
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 10)]
        [InlineData(100, 50)]
        [InlineData(int.MaxValue, 0)]
        [InlineData(0, int.MaxValue)]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void Constructor_WithPositiveValues_SetsFieldsCorrectly(int start, int length)
        {
            // Arrange & Act
            var span = new Span(start, length);

            // Assert
            span.Start.Should().Be(start);
            span.Length.Should().Be(length);
            span.End.Should().Be(start + length);
        }

        /// <summary>
        /// Tests that the Span constructor correctly handles negative values for Start and Length fields.
        /// </summary>
        /// <param name="start">The start position value</param>
        /// <param name="length">The length value</param>
        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(-10, -5)]
        [InlineData(-100, 50)]
        [InlineData(50, -100)]
        [InlineData(int.MinValue, 0)]
        [InlineData(0, int.MinValue)]
        [InlineData(int.MinValue, int.MinValue)]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void Constructor_WithNegativeValues_SetsFieldsCorrectly(int start, int length)
        {
            // Arrange & Act
            var span = new Span(start, length);

            // Assert
            span.Start.Should().Be(start);
            span.Length.Should().Be(length);
            span.End.Should().Be(start + length);
        }

        /// <summary>
        /// Tests that the Span constructor correctly handles extreme integer boundary values.
        /// </summary>
        /// <param name="start">The start position value</param>
        /// <param name="length">The length value</param>
        [Theory]
        [InlineData(int.MaxValue, int.MaxValue)]
        [InlineData(int.MinValue, int.MaxValue)]
        [InlineData(int.MaxValue, int.MinValue)]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void Constructor_WithBoundaryValues_SetsFieldsCorrectly(int start, int length)
        {
            // Arrange & Act
            var span = new Span(start, length);

            // Assert
            span.Start.Should().Be(start);
            span.Length.Should().Be(length);
            span.End.Should().Be(start + length);
        }

        /// <summary>
        /// Tests that the Span constructor with zero values initializes fields to zero.
        /// </summary>
        [Fact]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void Constructor_WithZeroValues_InitializesToZero()
        {
            // Arrange & Act
            var span = new Span(0, 0);

            // Assert
            span.Start.Should().Be(0);
            span.Length.Should().Be(0);
            span.End.Should().Be(0);
        }

        /// <summary>
        /// Tests the ToString method with various combinations of Start and Length values
        /// to ensure proper string formatting in all scenarios including edge cases.
        /// </summary>
        /// <param name="start">The start position value</param>
        /// <param name="length">The length value</param>
        /// <param name="expected">The expected string representation</param>
        [Theory]
        [InlineData(0, 0, "(0, 0)")]
        [InlineData(1, 5, "(1, 5)")]
        [InlineData(10, 20, "(10, 20)")]
        [InlineData(-1, 5, "(-1, 5)")]
        [InlineData(5, -1, "(5, -1)")]
        [InlineData(-10, -20, "(-10, -20)")]
        [InlineData(int.MaxValue, int.MaxValue, "(2147483647, 2147483647)")]
        [InlineData(int.MinValue, int.MinValue, "(-2147483648, -2147483648)")]
        [InlineData(int.MaxValue, 0, "(2147483647, 0)")]
        [InlineData(0, int.MaxValue, "(0, 2147483647)")]
        [InlineData(int.MinValue, 0, "(-2147483648, 0)")]
        [InlineData(0, int.MinValue, "(0, -2147483648)")]
        [InlineData(int.MaxValue, int.MinValue, "(2147483647, -2147483648)")]
        [InlineData(int.MinValue, int.MaxValue, "(-2147483648, 2147483647)")]
        [InlineData(12345, 67890, "(12345, 67890)")]
        [InlineData(-12345, 67890, "(-12345, 67890)")]
        [InlineData(12345, -67890, "(12345, -67890)")]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void ToString_WithVariousStartAndLengthValues_ReturnsCorrectFormat(int start, int length, string expected)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            var result = span.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the ToString method with the Empty static instance to ensure
        /// it returns the correct string representation for a default span.
        /// </summary>
        [Fact]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void ToString_WithEmptySpan_ReturnsZeroZeroFormat()
        {
            // Arrange
            var span = Span.Empty;

            // Act
            var result = span.ToString();

            // Assert
            result.Should().Be("(0, 0)");
        }

        /// <summary>
        /// Tests the ToString method with a default-constructed span to ensure
        /// it behaves identically to the Empty static instance.
        /// </summary>
        [Fact]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void ToString_WithDefaultConstructedSpan_ReturnsZeroZeroFormat()
        {
            // Arrange
            var span = new Span();

            // Act
            var result = span.ToString();

            // Assert
            result.Should().Be("(0, 0)");
        }

        /// <summary>
        /// Tests the Skip method with various length parameters to verify correct span adjustment behavior.
        /// Validates that the method correctly adjusts Start and Length properties, handles boundary conditions,
        /// and returns empty spans when skipping beyond the current length.
        /// </summary>
        /// <param name="initialStart">The initial Start value of the span</param>
        /// <param name="initialLength">The initial Length value of the span</param>
        /// <param name="skipLength">The length parameter to skip</param>
        /// <param name="expectedStart">The expected Start value of the result span</param>
        /// <param name="expectedLength">The expected Length value of the result span</param>
        [Theory]
        [InlineData(1, 10, 2, 3, 8)] // Normal case: skip within bounds
        [InlineData(5, 15, 7, 12, 8)] // Normal case: skip within bounds
        [InlineData(0, 10, 5, 5, 5)] // Normal case: start at zero
        [InlineData(10, 5, 5, 15, 0)] // Boundary case: skip exactly the length (returns span at end position with zero length)
        [InlineData(1, 10, 0, 1, 10)] // Skip zero: should return same span
        [InlineData(5, 8, 15, 0, 0)] // Skip more than length: should return empty span
        [InlineData(0, 0, 0, 0, 0)] // Empty span skip zero
        [InlineData(0, 0, 5, 0, 0)] // Empty span skip positive
        [InlineData(1, 10, -2, -1, 12)] // Negative skip: mathematically valid behavior
        [InlineData(100, 50, -10, 90, 60)] // Negative skip: extending the span backwards
        [Trait("Category", "auto-generated")]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        public void Skip_WithVariousLengthParameters_ReturnsExpectedSpan(
            int initialStart,
            int initialLength,
            int skipLength,
            int expectedStart,
            int expectedLength)
        {
            // Arrange
            var span = new Span(initialStart, initialLength);

            // Act
            var result = span.Skip(skipLength);

            // Assert
            result.Start.Should().Be(expectedStart);
            result.Length.Should().Be(expectedLength);
        }

        /// <summary>
        /// Tests the Skip method with extreme integer values to verify overflow and boundary handling.
        /// Ensures the method behaves predictably with edge case numeric inputs.
        /// </summary>
        /// <param name="initialStart">The initial Start value of the span</param>
        /// <param name="initialLength">The initial Length value of the span</param>
        /// <param name="skipLength">The extreme length parameter to skip</param>
        /// <param name="shouldReturnEmpty">Whether the result should be an empty span</param>
        [Theory]
        [InlineData(0, 1000000, int.MaxValue, true)] // Skip max int: should return empty
        [InlineData(1000, 500, int.MaxValue, true)] // Skip max int with normal span: should return empty
        [InlineData(0, int.MaxValue, int.MaxValue, true)] // Max length skip max: should return empty (boundary)
        [InlineData(1000, 2000, int.MinValue, false)] // Skip min int: mathematically valid (negative skip)
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        [Trait("Category", "ProductionBugSuspected")]
        public void Skip_WithExtremeIntegerValues_HandlesOverflowCorrectly(
            int initialStart,
            int initialLength,
            int skipLength,
            bool shouldReturnEmpty)
        {
            // Arrange
            var span = new Span(initialStart, initialLength);

            // Act
            var result = span.Skip(skipLength);

            // Assert
            if (shouldReturnEmpty)
            {
                result.Start.Should().Be(0);
                result.Length.Should().Be(0);
            }
            else
            {
                // For negative skips, verify mathematical behavior
                if (skipLength < 0)
                {
                    result.Start.Should().Be(initialStart + skipLength);
                    result.Length.Should().Be(initialLength - skipLength);
                }
            }
        }

        /// <summary>
        /// Tests that Skip method returns empty span when skip length exceeds current span length.
        /// Verifies the specific condition where length > Length results in an empty span.
        /// </summary>
        [Fact]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void Skip_WhenSkipLengthExceedsSpanLength_ReturnsEmptySpan()
        {
            // Arrange
            var span = new Span(10, 5);
            var skipLength = 6; // Greater than span.Length (5)

            // Act
            var result = span.Skip(skipLength);

            // Assert
            result.Start.Should().Be(0);
            result.Length.Should().Be(0);
            result.End.Should().Be(0);
        }

        /// <summary>
        /// Tests ContainsEndInclusive method with various position values relative to span boundaries.
        /// Verifies that the method returns true when position is within [Start, End] inclusive range.
        /// </summary>
        /// <param name="start">The start position of the span</param>
        /// <param name="length">The length of the span</param>
        /// <param name="position">The position to test</param>
        /// <param name="expected">Expected result</param>
        [Theory]
        [InlineData(10, 5, 9, false)]    // Position before start
        [InlineData(10, 5, 10, true)]   // Position at start (inclusive)
        [InlineData(10, 5, 12, true)]   // Position within span
        [InlineData(10, 5, 15, true)]   // Position at end (inclusive)
        [InlineData(10, 5, 16, false)]  // Position after end
        [InlineData(0, 0, 0, true)]     // Zero-length span, position at start/end
        [InlineData(0, 0, -1, false)]   // Zero-length span, position before
        [InlineData(0, 0, 1, false)]    // Zero-length span, position after
        [InlineData(5, 1, 5, true)]     // Single-length span, position at start
        [InlineData(5, 1, 6, true)]     // Single-length span, position at end
        [InlineData(5, 1, 4, false)]    // Single-length span, position before
        [InlineData(5, 1, 7, false)]    // Single-length span, position after
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void ContainsEndInclusive_VariousPositions_ReturnsExpectedResult(int start, int length, int position, bool expected)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            bool result = span.ContainsEndInclusive(position);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests ContainsEndInclusive method with extreme integer values.
        /// Verifies proper handling of boundary conditions with int.MinValue and int.MaxValue.
        /// </summary>
        /// <param name="start">The start position of the span</param>
        /// <param name="length">The length of the span</param>
        /// <param name="position">The position to test</param>
        /// <param name="expected">Expected result</param>
        [Theory]
        [InlineData(int.MinValue, 1, int.MinValue, true)]     // Position at start with min value
        [InlineData(int.MinValue, 1, int.MinValue + 1, true)] // Position at end with min start
        [InlineData(int.MaxValue - 1, 1, int.MaxValue, true)] // Position at end near max value
        [InlineData(int.MaxValue - 1, 1, int.MaxValue - 1, true)] // Position at start near max value
        [InlineData(0, int.MaxValue, int.MaxValue - 1, true)] // Large span, position near end
        [InlineData(1, int.MaxValue - 1, int.MaxValue, true)] // Large span, position at calculated end
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void ContainsEndInclusive_ExtremeValues_ReturnsExpectedResult(int start, int length, int position, bool expected)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            bool result = span.ContainsEndInclusive(position);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that ContainsEndInclusive includes the end position while Contains excludes it.
        /// Verifies the key behavioral difference between the two methods.
        /// </summary>
        [Fact]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void ContainsEndInclusive_AtEndPosition_ReturnsTrueUnlikeContains()
        {
            // Arrange
            var span = new Span(10, 5); // End is at position 15
            int endPosition = span.End;

            // Act
            bool containsEndInclusive = span.ContainsEndInclusive(endPosition);
            bool contains = span.Contains(endPosition);

            // Assert
            containsEndInclusive.Should().BeTrue("ContainsEndInclusive should include the end position");
            contains.Should().BeFalse("Contains should exclude the end position");
        }

        /// <summary>
        /// Tests ContainsEndInclusive method with Empty span.
        /// Verifies behavior with the predefined Empty span constant.
        /// </summary>
        [Theory]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void ContainsEndInclusive_EmptySpan_ReturnsExpectedResult(int position, bool expected)
        {
            // Arrange
            var span = Span.Empty; // Start = 0, Length = 0

            // Act
            bool result = span.ContainsEndInclusive(position);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the Contains method with various position values to verify it correctly identifies
        /// whether a position falls within the span's range (inclusive start, exclusive end).
        /// </summary>
        /// <param name="start">The start position of the span</param>
        /// <param name="length">The length of the span</param>
        /// <param name="position">The position to test for containment</param>
        /// <param name="expectedResult">Whether the position should be contained in the span</param>
        [Theory]
        [InlineData(0, 5, 0, true)]     // Position at start
        [InlineData(0, 5, 4, true)]     // Position at end - 1
        [InlineData(0, 5, 5, false)]    // Position at end (exclusive)
        [InlineData(0, 5, -1, false)]   // Position before start
        [InlineData(0, 5, 6, false)]    // Position after end
        [InlineData(10, 5, 10, true)]   // Position at start (non-zero start)
        [InlineData(10, 5, 14, true)]   // Position at end - 1 (non-zero start)
        [InlineData(10, 5, 15, false)]  // Position at end (non-zero start)
        [InlineData(10, 5, 9, false)]   // Position before start (non-zero start)
        [InlineData(10, 5, 16, false)]  // Position after end (non-zero start)
        [InlineData(0, 0, 0, false)]    // Empty span at position 0
        [InlineData(5, 0, 5, false)]    // Empty span at position 5
        [InlineData(-5, 3, -5, true)]   // Negative start, position at start
        [InlineData(-5, 3, -3, true)]   // Negative start, position at end - 1
        [InlineData(-5, 3, -2, false)]  // Negative start, position at end
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void Contains_VariousPositions_ReturnsExpectedResult(int start, int length, int position, bool expectedResult)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            var result = span.Contains(position);

            // Assert
            result.Should().Be(expectedResult);
        }

        /// <summary>
        /// Tests the Contains method with extreme integer values to ensure it handles
        /// boundary cases without overflow or unexpected behavior.
        /// </summary>
        /// <param name="start">The start position of the span</param>
        /// <param name="length">The length of the span</param>
        /// <param name="position">The position to test for containment</param>
        /// <param name="expectedResult">Whether the position should be contained in the span</param>
        [Theory]
        [InlineData(0, 10, int.MinValue, false)]    // Position at minimum int value
        [InlineData(0, 10, int.MaxValue, false)]    // Position at maximum int value
        [InlineData(int.MinValue, 10, int.MinValue, true)]     // Start at min value, position at start
        [InlineData(int.MinValue, 10, int.MinValue + 5, true)] // Start at min value, position within span
        [InlineData(int.MaxValue - 5, 5, int.MaxValue - 1, true)]  // Near max value, position within span
        [InlineData(int.MaxValue - 5, 5, int.MaxValue, false)]     // Near max value, position at end
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void Contains_ExtremeValues_ReturnsExpectedResult(int start, int length, int position, bool expectedResult)
        {
            // Arrange
            var span = new Span(start, length);

            // Act
            var result = span.Contains(position);

            // Assert
            result.Should().Be(expectedResult);
        }

        /// <summary>
        /// Tests the Contains method with zero position across different span configurations
        /// to verify correct behavior when position is zero.
        /// </summary>
        /// <param name="start">The start position of the span</param>
        /// <param name="length">The length of the span</param>
        /// <param name="expectedResult">Whether position 0 should be contained in the span</param>
        [Theory]
        [InlineData(0, 1, true)]     // Span starts at 0, length 1
        [InlineData(0, 0, false)]    // Empty span at 0
        [InlineData(-1, 2, true)]    // Span from -1 to 1
        [InlineData(1, 5, false)]    // Span starts after 0
        [InlineData(-5, 3, false)]   // Span ends before 0
        [Trait("Owner", "Code Testing Agent 0.3.120-alpha+7e004b4e23.RR")]
        [Trait("Category", "auto-generated")]
        public void Contains_ZeroPosition_ReturnsExpectedResult(int start, int length, bool expectedResult)
        {
            // Arrange
            var span = new Span(start, length);
            const int position = 0;

            // Act
            var result = span.Contains(position);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}