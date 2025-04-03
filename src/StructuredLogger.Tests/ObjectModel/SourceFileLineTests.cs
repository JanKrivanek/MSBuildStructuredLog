using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="SourceFileLine"/> class.
    /// </summary>
    public class SourceFileLineTests
    {
        /// <summary>
        /// Tests the ToString method of the SourceFileLine class to ensure it returns the line number padded to 5 characters followed by the line text.
        /// </summary>
        /// <param name="lineNumber">The line number to set.</param>
        /// <param name="lineText">The line text to set.</param>
        /// <param name="expected">The expected string output from ToString.</param>
        [Theory]
        [InlineData(42, "Hello", "42   Hello")]
        [InlineData(0, "World", "0    World")]
        [InlineData(100, "", "100  ")]
        [InlineData(-1, "Neg", "-1   Neg")]
        [InlineData(12345, "Long Line", "12345Long Line")]
        public void ToString_WhenCalled_ReturnsPaddedLineNumberConcatenatedWithLineText(int lineNumber, string lineText, string expected)
        {
            // Arrange
            var sourceFileLine = new SourceFileLine
            {
                LineNumber = lineNumber,
                LineText = lineText
            };

            // Act
            string result = sourceFileLine.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that the TypeName property returns the correct type name.
        /// </summary>
        [Fact]
        public void TypeName_WhenAccessed_ReturnsSourceFileLine()
        {
            // Arrange
            var sourceFileLine = new SourceFileLine();

            // Act
            string typeName = sourceFileLine.TypeName;

            // Assert
            typeName.Should().Be("SourceFileLine");
        }

        /// <summary>
        /// Tests that the getter and setter methods for LineNumber and LineText work as expected.
        /// </summary>
        [Fact]
        public void Properties_SetAndGet_ReturnsCorrectValues()
        {
            // Arrange
            var expectedLineNumber = int.MaxValue;
            var expectedLineText = "Test line text with special characters !@#$%^&*()";
            var sourceFileLine = new SourceFileLine();

            // Act
            sourceFileLine.LineNumber = expectedLineNumber;
            sourceFileLine.LineText = expectedLineText;

            // Assert
            sourceFileLine.LineNumber.Should().Be(expectedLineNumber);
            sourceFileLine.LineText.Should().Be(expectedLineText);
        }
    }
}
