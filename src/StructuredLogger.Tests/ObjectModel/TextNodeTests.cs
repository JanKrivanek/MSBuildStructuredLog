using FluentAssertions;
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
        /// Tests that the default TypeName property always returns "TextNode".
        /// </summary>
        [Fact]
        public void TypeName_Always_ReturnsTextNode()
        {
            // Arrange
            var textNode = new TextNode();

            // Act
            var typeName = textNode.TypeName;

            // Assert
            typeName.Should().Be("TextNode");
        }

        /// <summary>
        /// Tests that the Title property returns the Text value when it is non-null.
        /// </summary>
        [Fact]
        public void Title_WithNonNullText_ReturnsOriginalText()
        {
            // Arrange
            var expectedText = "Example Text";
            var textNode = new TextNode { Text = expectedText };

            // Act
            var title = textNode.Title;

            // Assert
            title.Should().Be(expectedText);
        }

        /// <summary>
        /// Tests that the Title property returns TypeName when Text is null.
        /// </summary>
        [Fact]
        public void Title_WithNullText_ReturnsTypeName()
        {
            // Arrange
            var textNode = new TextNode { Text = null };

            // Act
            var title = textNode.Title;

            // Assert
            title.Should().Be(textNode.TypeName);
        }

        /// <summary>
        /// Tests that the ToString method returns the same value as the Title property.
        /// </summary>
        [Fact]
        public void ToString_ReturnsTitle()
        {
            // Arrange
            var testText = "Sample ToString";
            var textNode = new TextNode { Text = testText };

            // Act
            var toStringResult = textNode.ToString();

            // Assert
            toStringResult.Should().Be(textNode.Title);
        }

        /// <summary>
        /// Tests that the ShortenedText property returns the expected shortened value when Text is non-null.
        /// </summary>
        /// <param name="inputText">The input text to be shortened.</param>
        [Theory]
        [InlineData("A short text")]
        [InlineData("This is a longer text that might be shortened")]
        [InlineData("")]
        public void ShortenedText_WithNonNullText_ReturnsExpectedShortenedValue(string inputText)
        {
            // Arrange
            var textNode = new TextNode { Text = inputText };
            var expectedValue = TextUtilities.ShortenValue(inputText);

            // Act
            var shortenedText = textNode.ShortenedText;

            // Assert
            shortenedText.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests that the ShortenedText property returns the expected value when Text is null.
        /// </summary>
        [Fact]
        public void ShortenedText_WithNullText_ReturnsExpectedShortenedValue()
        {
            // Arrange
            var textNode = new TextNode { Text = null };
            var expectedValue = TextUtilities.ShortenValue(null);

            // Act
            var shortenedText = textNode.ShortenedText;

            // Assert
            shortenedText.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests that the IsTextShortened property returns false when Text is null.
        /// </summary>
        [Fact]
        public void IsTextShortened_WithNullText_ReturnsFalse()
        {
            // Arrange
            var textNode = new TextNode { Text = null };

            // Act
            var isShortened = textNode.IsTextShortened;

            // Assert
            isShortened.Should().BeFalse();
        }

        /// <summary>
        /// Tests that the IsTextShortened property returns the correct boolean value based on the Text and its shorten length.
        /// </summary>
        /// <param name="inputText">The input text to evaluate.</param>
        [Theory]
        [InlineData("Normal Text")]
        [InlineData("")]
        [InlineData("A")]
        public void IsTextShortened_WithNonNullText_ReturnsCorrectResult(string inputText)
        {
            // Arrange
            var textNode = new TextNode { Text = inputText };
            bool expected = inputText != null && inputText.Length != TextUtilities.GetShortenLength(inputText);

            // Act
            var isShortened = textNode.IsTextShortened;

            // Assert
            isShortened.Should().Be(expected);
        }

        /// <summary>
        /// Tests the behavior of properties when using an extremely long string for Text.
        /// </summary>
        [Fact]
        public void Properties_WithExtremelyLongText_HandleBoundaryConditions()
        {
            // Arrange
            var longText = new string('a', 10_000); // Boundary condition: very long string.
            var textNode = new TextNode { Text = longText };
            var expectedShortened = TextUtilities.ShortenValue(longText);
            bool expectedIsShortened = longText.Length != TextUtilities.GetShortenLength(longText);

            // Act
            var title = textNode.Title;
            var shortenedText = textNode.ShortenedText;
            var isShortened = textNode.IsTextShortened;

            // Assert
            title.Should().Be(longText);
            shortenedText.Should().Be(expectedShortened);
            isShortened.Should().Be(expectedIsShortened);
        }
    }
}
