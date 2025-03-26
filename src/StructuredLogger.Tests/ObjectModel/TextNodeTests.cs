using System;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TextNode"/> class.
    /// </summary>
    public class TextNodeTests
    {
        /// <summary>
        /// Tests that the <see cref="TextNode.Title"/> property returns the TypeName when Text is null.
        /// </summary>
        [Fact]
        public void Title_WithNullText_ReturnsTypeName()
        {
            // Arrange
            var textNode = new TextNode { Text = null };

            // Act
            string title = textNode.Title;
            string expectedTitle = textNode.TypeName; // "TextNode"

            // Assert
            Assert.Equal(expectedTitle, title);
        }

        /// <summary>
        /// Tests that the <see cref="TextNode.Title"/> property returns the Text when it is not null.
        /// </summary>
        [Fact]
        public void Title_WithNonNullText_ReturnsText()
        {
            // Arrange
            string sampleText = "Sample text";
            var textNode = new TextNode { Text = sampleText };

            // Act
            string title = textNode.Title;

            // Assert
            Assert.Equal(sampleText, title);
        }

        /// <summary>
        /// Tests that the <see cref="TextNode.TypeName"/> property always returns "TextNode".
        /// </summary>
        [Fact]
        public void TypeName_Always_ReturnsTextNode()
        {
            // Arrange
            var textNode = new TextNode();

            // Act
            string typeName = textNode.TypeName;

            // Assert
            Assert.Equal("TextNode", typeName);
        }

        /// <summary>
        /// Tests that the <see cref="TextNode.ShortenedText"/> property returns the value 
        /// computed by TextUtilities.ShortenValue for a null Text.
        /// </summary>
        [Fact]
        public void ShortenedText_WithNullText_ReturnsExpected()
        {
            // Arrange
            var textNode = new TextNode { Text = null };
            string expectedShortenedText = TextUtilities.ShortenValue(null);

            // Act
            string actualShortenedText = textNode.ShortenedText;

            // Assert
            Assert.Equal(expectedShortenedText, actualShortenedText);
        }

        /// <summary>
        /// Tests that the <see cref="TextNode.ShortenedText"/> property returns the value 
        /// computed by TextUtilities.ShortenValue for a non-null Text.
        /// </summary>
        [Fact]
        public void ShortenedText_WithNonNullText_ReturnsExpected()
        {
            // Arrange
            string sampleText = "Example text for shortening";
            var textNode = new TextNode { Text = sampleText };
            string expectedShortenedText = TextUtilities.ShortenValue(sampleText);

            // Act
            string actualShortenedText = textNode.ShortenedText;

            // Assert
            Assert.Equal(expectedShortenedText, actualShortenedText);
        }

        /// <summary>
        /// Tests that the <see cref="TextNode.IsTextShortened"/> property returns false when Text is null.
        /// </summary>
        [Fact]
        public void IsTextShortened_WithNullText_ReturnsFalse()
        {
            // Arrange
            var textNode = new TextNode { Text = null };

            // Act
            bool isShortened = textNode.IsTextShortened;

            // Assert
            Assert.False(isShortened);
        }

        /// <summary>
        /// Tests that the <see cref="TextNode.IsTextShortened"/> property returns the correct boolean 
        /// based on the comparison between Text length and TextUtilities.GetShortenLength when Text is not null.
        /// </summary>
        [Fact]
        public void IsTextShortened_WithNonNullText_ReturnsExpected()
        {
            // Arrange
            string sampleText = "Sample text for checking length";
            var textNode = new TextNode { Text = sampleText };
            bool expectedIsShortened = sampleText.Length != TextUtilities.GetShortenLength(sampleText);

            // Act
            bool actualIsShortened = textNode.IsTextShortened;

            // Assert
            Assert.Equal(expectedIsShortened, actualIsShortened);
        }

        /// <summary>
        /// Tests that the overridden <see cref="TextNode.ToString"/> method returns the same value as the Title property.
        /// </summary>
        [Fact]
        public void ToString_Always_ReturnsTitle()
        {
            // Arrange
            string sampleText = "ToString test text";
            var textNode = new TextNode { Text = sampleText };

            // Act
            string title = textNode.Title;
            string toStringResult = textNode.ToString();

            // Assert
            Assert.Equal(title, toStringResult);
        }
    }
}
