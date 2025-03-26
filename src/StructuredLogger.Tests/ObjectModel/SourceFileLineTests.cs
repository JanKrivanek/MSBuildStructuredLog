using System;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="SourceFileLine"/> class.
    /// </summary>
    public class SourceFileLineTests
    {
        private readonly SourceFileLine _defaultSourceFileLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceFileLineTests"/> class with a default instance.
        /// </summary>
        public SourceFileLineTests()
        {
            _defaultSourceFileLine = new SourceFileLine
            {
                LineNumber = 0,
                LineText = string.Empty
            };
        }

        /// <summary>
        /// Tests that the <see cref="SourceFileLine.TypeName"/> property returns the expected class name.
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsClassName()
        {
            // Act
            string typeName = _defaultSourceFileLine.TypeName;

            // Assert
            Assert.Equal("SourceFileLine", typeName);
        }

        /// <summary>
        /// Tests that the <see cref="SourceFileLine.ToString"/> method returns the correctly formatted string when provided with valid line number and text.
        /// </summary>
        /// <param name="lineNumber">The line number to set.</param>
        /// <param name="lineText">The line text to set.</param>
        /// <param name="expected">The expected formatted string.</param>
        [Theory]
        [InlineData(1, "Test", "1    Test")]
        [InlineData(12, "Line text", "12   Line text")]
        [InlineData(12345, "Test", "12345Test")]
        public void ToString_WhenCalled_ReturnsCorrectlyFormattedString(int lineNumber, string lineText, string expected)
        {
            // Arrange
            var sourceFileLine = new SourceFileLine
            {
                LineNumber = lineNumber,
                LineText = lineText
            };

            // Act
            string actual = sourceFileLine.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests that setting and getting the <see cref="SourceFileLine.LineNumber"/> and <see cref="SourceFileLine.LineText"/> properties returns the assigned values.
        /// </summary>
        /// <param name="lineNumber">The line number to assign.</param>
        /// <param name="lineText">The line text to assign.</param>
        [Theory]
        [InlineData(0, "")]
        [InlineData(10, "Sample text")]
        [InlineData(-5, "Negative value")]
        public void Properties_WhenSet_GetReturnsAssignedValue(int lineNumber, string lineText)
        {
            // Arrange
            var sourceFileLine = new SourceFileLine();

            // Act
            sourceFileLine.LineNumber = lineNumber;
            sourceFileLine.LineText = lineText;

            // Assert
            Assert.Equal(lineNumber, sourceFileLine.LineNumber);
            Assert.Equal(lineText, sourceFileLine.LineText);
        }

        /// <summary>
        /// Tests that the <see cref="SourceFileLine.ToString"/> method handles a null value for <see cref="SourceFileLine.LineText"/> by returning only the padded line number.
        /// </summary>
        [Fact]
        public void ToString_WhenLineTextIsNull_ReturnsLineNumberPaddedWithNoText()
        {
            // Arrange
            int lineNumber = 7;
            var sourceFileLine = new SourceFileLine
            {
                LineNumber = lineNumber,
                LineText = null
            };

            // Act
            string actual = sourceFileLine.ToString();
            string expected = lineNumber.ToString().PadRight(5, ' ');

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
