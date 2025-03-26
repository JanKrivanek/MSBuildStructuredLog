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
        /// Tests that ToString returns the value of the Text property when it is set to a non-null value.
        /// </summary>
        /// <param name="expectedText">The value to assign to the Text property.</param>
        [Theory]
        [InlineData("Hello")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Long text value for testing purposes.")]
        public void ToString_WhenTextIsSet_ReturnsSameText(string expectedText)
        {
            // Arrange
            var highlightedText = new HighlightedText { Text = expectedText };

            // Act
            string actualResult = highlightedText.ToString();

            // Assert
            Assert.Equal(expectedText, actualResult);
        }

        /// <summary>
        /// Tests that ToString returns null when the Text property is not set.
        /// </summary>
        [Fact]
        public void ToString_WhenTextNotSet_ReturnsNull()
        {
            // Arrange
            var highlightedText = new HighlightedText();

            // Act
            string actualResult = highlightedText.ToString();

            // Assert
            Assert.Null(actualResult);
        }

        /// <summary>
        /// Tests the getter and setter of the Text property with various inputs.
        /// </summary>
        /// <param name="value">The test value to assign to the Text property.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Sample text")]
        public void TextProperty_SetAndGet_ReturnsSameValue(string value)
        {
            // Arrange
            var highlightedText = new HighlightedText();

            // Act
            highlightedText.Text = value;
            string actualValue = highlightedText.Text;

            // Assert
            Assert.Equal(value, actualValue);
        }

        /// <summary>
        /// Tests the getter and setter of the Style property with various inputs.
        /// </summary>
        /// <param name="value">The test value to assign to the Style property.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Bold")]
        [InlineData("Italic")]
        public void StyleProperty_SetAndGet_ReturnsSameValue(string value)
        {
            // Arrange
            var highlightedText = new HighlightedText();

            // Act
            highlightedText.Style = value;
            string actualValue = highlightedText.Style;

            // Assert
            Assert.Equal(value, actualValue);
        }
    }
}
