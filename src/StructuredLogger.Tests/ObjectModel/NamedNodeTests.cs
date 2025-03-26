using System;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="NamedNode"/> class.
    /// </summary>
    public class NamedNodeTests
    {
        /// <summary>
        /// Tests that the <see cref="NamedNode.TypeName"/> property returns "NamedNode".
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsNamedNode()
        {
            // Arrange
            var node = new NamedNode { Name = "Test" };

            // Act
            string typeName = node.TypeName;

            // Assert
            Assert.Equal("NamedNode", typeName);
        }

        /// <summary>
        /// Tests that the <see cref="NamedNode.Title"/> property returns the same value as <see cref="NamedNode.Name"/>.
        /// </summary>
        [Fact]
        public void Title_WhenNameIsSet_ReturnsName()
        {
            // Arrange
            const string expectedName = "TestTitle";
            var node = new NamedNode { Name = expectedName };

            // Act
            string title = node.Title;

            // Assert
            Assert.Equal(expectedName, title);
        }

        /// <summary>
        /// Tests that the <see cref="NamedNode.ToString"/> method returns the same value as <see cref="NamedNode.Name"/>.
        /// </summary>
        [Fact]
        public void ToString_WhenNameIsSet_ReturnsName()
        {
            // Arrange
            const string expectedName = "TestToString";
            var node = new NamedNode { Name = expectedName };

            // Act
            string result = node.ToString();

            // Assert
            Assert.Equal(expectedName, result);
        }

        /// <summary>
        /// Tests that the <see cref="NamedNode.ShortenedName"/> property returns the value
        /// obtained from <see cref="TextUtilities.ShortenValue(string)"/> when a valid name is provided.
        /// </summary>
        /// <param name="name">The input name for testing shortening behavior.</param>
        [Theory]
        [InlineData("")]
        [InlineData("Short")]
        [InlineData("This is a very long name that might be shortened")]
        public void ShortenedName_WhenNameIsSet_ReturnsExpectedShortenedValue(string name)
        {
            // Arrange
            var node = new NamedNode { Name = name };
            string expectedShortened = TextUtilities.ShortenValue(name);

            // Act
            string actualShortened = node.ShortenedName;

            // Assert
            Assert.Equal(expectedShortened, actualShortened);
        }

        /// <summary>
        /// Tests that the <see cref="NamedNode.IsNameShortened"/> property correctly reflects
        /// whether the shortened value is different from the original name.
        /// </summary>
        /// <param name="name">The input name for testing the shortening indicator.</param>
        [Theory]
        [InlineData("Short")]
        [InlineData("This is a very long name that might be shortened")]
        public void IsNameShortened_WhenNameIsSet_ReturnsCorrectComparisonResult(string name)
        {
            // Arrange
            var node = new NamedNode { Name = name };
            bool expectedIsShortened = node.Name != TextUtilities.ShortenValue(node.Name);

            // Act
            bool actualIsShortened = node.IsNameShortened;

            // Assert
            Assert.Equal(expectedIsShortened, actualIsShortened);
        }

        /// <summary>
        /// Tests the behavior of the <see cref="NamedNode"/> properties when <see cref="NamedNode.Name"/> is set to null.
        /// Expected behavior is that <see cref="NamedNode.ShortenedName"/> returns null, <see cref="NamedNode.IsNameShortened"/> is false,
        /// and both <see cref="NamedNode.Title"/> and <see cref="NamedNode.ToString"/> return null.
        /// </summary>
        [Fact]
        public void Properties_WhenNameIsNull_HandleGracefully()
        {
            // Arrange
            var node = new NamedNode { Name = null };

            // Act
            string shortenedName = node.ShortenedName;
            bool isNameShortened = node.IsNameShortened;
            string title = node.Title;
            string toStringResult = node.ToString();

            // Assert
            Assert.Null(shortenedName);
            Assert.False(isNameShortened);
            Assert.Null(title);
            Assert.Null(toStringResult);
        }
    }
}
