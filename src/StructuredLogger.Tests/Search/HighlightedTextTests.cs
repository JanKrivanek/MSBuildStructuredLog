using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="HighlightedText"/> class.
    /// </summary>
    public class HighlightedTextTests
    {
        /// <summary>
        /// Tests the <see cref="HighlightedText.ToString"/> method to ensure it returns the value of the Text property when it has a typical non-empty string.
        /// </summary>
        [Fact]
        public void ToString_WithRegularText_ReturnsSameText()
        {
            // Arrange
            string expectedText = "Sample Text";
            var highlightedText = new HighlightedText
            {
                Text = expectedText,
                Style = "Bold"
            };

            // Act
            var result = highlightedText.ToString();

            // Assert
            result.Should().Be(expectedText);
        }

        /// <summary>
        /// Tests the <see cref="HighlightedText.ToString"/> method to ensure it correctly returns an empty string when the Text property is set to empty.
        /// </summary>
        [Fact]
        public void ToString_WithEmptyText_ReturnsEmptyString()
        {
            // Arrange
            var highlightedText = new HighlightedText
            {
                Text = string.Empty,
                Style = "Italic"
            };

            // Act
            var result = highlightedText.ToString();

            // Assert
            result.Should().Be(string.Empty);
        }

        /// <summary>
        /// Tests the <see cref="HighlightedText.ToString"/> method for the scenario where the Text property contains only whitespace.
        /// The expected outcome is that the returned string matches the whitespace value.
        /// </summary>
        [Fact]
        public void ToString_WithWhiteSpaceText_ReturnsWhiteSpace()
        {
            // Arrange
            string whitespaceText = "   ";
            var highlightedText = new HighlightedText
            {
                Text = whitespaceText,
                Style = "Underline"
            };

            // Act
            var result = highlightedText.ToString();

            // Assert
            result.Should().Be(whitespaceText);
        }

        /// <summary>
        /// Tests the <see cref="HighlightedText.ToString"/> method to verify its behavior when the Text property is null.
        /// The expected outcome is that ToString returns null.
        /// </summary>
        [Fact]
        public void ToString_WithNullText_ReturnsNull()
        {
            // Arrange
            // Although Text is a non-nullable reference type by convention,
            // the auto-property will allow null if not explicitly initialized.
            var highlightedText = new HighlightedText
            {
                Text = null,
                Style = "Regular"
            };

            // Act
            var result = highlightedText.ToString();

            // Assert
            result.Should().BeNull();
        }
    }
}
