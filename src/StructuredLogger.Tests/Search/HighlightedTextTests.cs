using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="HighlightedText"/> class.
    /// </summary>
    public class HighlightedTextTests
    {
        /// <summary>
        /// Tests that the Text property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void TextProperty_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            const string expectedText = "Test text";
            var highlightedText = new HighlightedText
            {
                Text = expectedText
            };

            // Act
            string actualText = highlightedText.Text;

            // Assert
            actualText.Should().Be(expectedText);
        }

        /// <summary>
        /// Tests that the Style property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void StyleProperty_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            const string expectedStyle = "Bold";
            var highlightedText = new HighlightedText
            {
                Style = expectedStyle
            };

            // Act
            string actualStyle = highlightedText.Style;

            // Assert
            actualStyle.Should().Be(expectedStyle);
        }

        /// <summary>
        /// Tests that ToString returns the value of the Text property when Text is set.
        /// </summary>
        [Fact]
        public void ToString_WhenTextIsSet_ReturnsTextValue()
        {
            // Arrange
            const string expectedText = "Sample text";
            var highlightedText = new HighlightedText
            {
                Text = expectedText
            };

            // Act
            string result = highlightedText.ToString();

            // Assert
            result.Should().Be(expectedText);
        }

        /// <summary>
        /// Tests that ToString returns null when Text is not set.
        /// </summary>
        [Fact]
        public void ToString_WhenTextIsNotSet_ReturnsNull()
        {
            // Arrange
            var highlightedText = new HighlightedText();

            // Act
            string result = highlightedText.ToString();

            // Assert
            result.Should().BeNull();
        }
    }
}
