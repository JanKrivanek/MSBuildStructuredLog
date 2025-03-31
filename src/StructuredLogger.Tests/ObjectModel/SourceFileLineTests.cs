using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="SourceFileLine"/> class.
    /// </summary>
    public class SourceFileLineTests
    {
        private readonly SourceFileLine _sourceFileLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceFileLineTests"/> class.
        /// </summary>
        public SourceFileLineTests()
        {
            _sourceFileLine = new SourceFileLine();
        }

        /// <summary>
        /// Tests that the TypeName property returns "SourceFileLine".
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsSourceFileLine()
        {
            // Act
            string typeName = _sourceFileLine.TypeName;

            // Assert
            typeName.Should().Be("SourceFileLine", "the TypeName property should return the name of the class");
        }

        /// <summary>
        /// Tests that the ToString method returns the properly padded line number concatenated with the line text.
        /// </summary>
        /// <param name="lineNumber">The line number to be set.</param>
        /// <param name="lineText">The line text to be set.</param>
        /// <param name="expectedResult">The expected string result from ToString.</param>
        [Theory]
        [InlineData(1, "hello", "1    hello")]
        [InlineData(42, "world", "42   world")]
        [InlineData(12345, "", "12345")]
        [InlineData(-5, "neg", "-5   neg")]
        public void ToString_WhenCalled_ReturnsPaddedLineNumberConcatenatedWithLineText(int lineNumber, string lineText, string expectedResult)
        {
            // Arrange
            var sourceFileLine = new SourceFileLine { LineNumber = lineNumber, LineText = lineText };

            // Act
            string result = sourceFileLine.ToString();

            // Assert
            result.Should().Be(expectedResult, "ToString should return the padded line number followed by the line text");
        }

        /// <summary>
        /// Tests that the properties LineNumber and LineText can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void Properties_SettingValues_PropertiesReturnSameValues()
        {
            // Arrange
            const int expectedLineNumber = 100;
            const string expectedLineText = "Sample text";
            var sourceFileLine = new SourceFileLine();

            // Act
            sourceFileLine.LineNumber = expectedLineNumber;
            sourceFileLine.LineText = expectedLineText;

            // Assert
            sourceFileLine.LineNumber.Should().Be(expectedLineNumber, "LineNumber should store the value that is set");
            sourceFileLine.LineText.Should().Be(expectedLineText, "LineText should store the value that is set");
        }
    }
}
