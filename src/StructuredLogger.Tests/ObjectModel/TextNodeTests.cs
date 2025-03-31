using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TextNode"/> class.
    /// </summary>
    public class TextNodeTests
    {
        /// <summary>
        /// Verifies that the <see cref="TextNode.TypeName"/> property always returns "TextNode".
        /// </summary>
        [Fact]
        public void TypeName_Get_ReturnsTextNode()
        {
            // Arrange
            var textNode = new TextNode();

            // Act
            var typeName = textNode.TypeName;

            // Assert
            typeName.Should().Be("TextNode");
        }

        /// <summary>
        /// Verifies that the <see cref="TextNode.Title"/> property returns the value of Text when it is not null.
        /// </summary>
        [Fact]
        public void Title_WhenTextIsNotNull_ReturnsTextValue()
        {
            // Arrange
            var expectedText = "Sample Text";
            var textNode = new TextNode { Text = expectedText };

            // Act
            var title = textNode.Title;

            // Assert
            title.Should().Be(expectedText);
        }

        /// <summary>
        /// Verifies that the <see cref="TextNode.Title"/> property returns the <see cref="TextNode.TypeName"/> when Text is null.
        /// </summary>
        [Fact]
        public void Title_WhenTextIsNull_ReturnsTypeName()
        {
            // Arrange
            var textNode = new TextNode { Text = null };

            // Act
            var title = textNode.Title;

            // Assert
            title.Should().Be(textNode.TypeName);
        }

        /// <summary>
        /// Verifies that the <see cref="TextNode.ToString"/> method returns the same value as the <see cref="TextNode.Title"/> property.
        /// </summary>
        [Fact]
        public void ToString_Always_ReturnsTitleValue()
        {
            // Arrange
            var expectedText = "Another Sample Text";
            var textNode = new TextNode { Text = expectedText };

            // Act
            var toStringResult = textNode.ToString();

            // Assert
            toStringResult.Should().Be(textNode.Title);
        }

        /// <summary>
        /// Verifies that the <see cref="TextNode.ShortenedText"/> property returns the output of TextUtilities.ShortenValue.
        /// Since TextUtilities is external, we validate behavior using known inputs.
        /// </summary>
        [Fact]
        public void ShortenedText_WhenTextIsProvided_ReturnsShortenedValue()
        {
            // Arrange & Act - Scenario 1: A text assumed not long enough to be shortened.
            var shortText = "Hello";
            var textNodeShort = new TextNode { Text = shortText };
            var shortenedShort = textNodeShort.ShortenedText;

            // Assert
            // For a short text, we assume the utility returns the text unmodified.
            shortenedShort.Should().Be(shortText);

            // Arrange & Act - Scenario 2: A text expected to be shortened.
            var longText = "This is a very long text that is expected to be shortened by the utility method.";
            var textNodeLong = new TextNode { Text = longText };
            var shortenedLong = textNodeLong.ShortenedText;

            // Assert
            // For a long text, the shortened value should differ from the original.
            shortenedLong.Should().NotBe(longText);
            // Additionally, assuming the shortened text retains a part of the original value,
            // the original text should contain the shortened text once any ellipsis is removed.
            var truncated = shortenedLong.Replace("...", "");
            longText.Should().Contain(truncated);
        }

        /// <summary>
        /// Verifies that the <see cref="TextNode.IsTextShortened"/> property returns false when Text is null.
        /// </summary>
        [Fact]
        public void IsTextShortened_WhenTextIsNull_ReturnsFalse()
        {
            // Arrange
            var textNode = new TextNode { Text = null };

            // Act
            var isShortened = textNode.IsTextShortened;

            // Assert
            isShortened.Should().BeFalse();
        }

        /// <summary>
        /// Verifies that the <see cref="TextNode.IsTextShortened"/> property returns false when the text is not shortened.
        /// </summary>
        [Fact]
        public void IsTextShortened_WhenTextIsNotShortened_ReturnsFalse()
        {
            // Arrange
            var text = "Hello";
            var textNode = new TextNode { Text = text };

            // Act
            var isShortened = textNode.IsTextShortened;

            // Assert
            // When the utility does not shorten the text, the property should be false.
            isShortened.Should().BeFalse();
        }

        /// <summary>
        /// Verifies that the <see cref="TextNode.IsTextShortened"/> property returns true when the text is shortened.
        /// </summary>
        [Fact]
        public void IsTextShortened_WhenTextIsShortened_ReturnsTrue()
        {
            // Arrange
            // Provide a long text that is expected to be shortened by TextUtilities.
            var longText = "This is an exceptionally long text input that should trigger shortening.";
            var textNode = new TextNode { Text = longText };

            // Act
            var isShortened = textNode.IsTextShortened;
            var shortenedText = textNode.ShortenedText;

            // Assert
            // The shortened text should differ from the original.
            shortenedText.Should().NotBe(longText);
            isShortened.Should().BeTrue();
        }
    }
}
