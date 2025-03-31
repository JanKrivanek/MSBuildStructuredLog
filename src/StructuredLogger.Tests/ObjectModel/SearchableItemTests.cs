using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="SearchableItem"/> class.
    /// </summary>
    public class SearchableItemTests
    {
        /// <summary>
        /// Tests that the default value of SearchText returns the value of the base Text property 
        /// when SearchText has not been explicitly set.
        /// </summary>
        [Fact]
        public void SearchText_Get_WhenNotSet_ReturnsBaseText()
        {
            // Arrange
            var searchableItem = new SearchableItem();
            // Assuming that the base class 'Item' has a public settable property 'Text'.
            const string baseText = "BaseValue";
            searchableItem.Text = baseText;

            // Act
            string actualSearchText = searchableItem.SearchText;

            // Assert
            actualSearchText.Should().Be(baseText, "when SearchText is not explicitly set, it should fall back to the base Text property value.");
        }

        /// <summary>
        /// Tests that setting SearchText to a non-null value returns that exact value.
        /// </summary>
        [Fact]
        public void SearchText_Get_WhenSetToNonNull_ReturnsOverriddenValue()
        {
            // Arrange
            var searchableItem = new SearchableItem();
            // Set a base text to ensure the fallback is different.
            searchableItem.Text = "BaseValue";
            const string overriddenText = "OverriddenValue";

            // Act
            searchableItem.SearchText = overriddenText;
            string actualSearchText = searchableItem.SearchText;

            // Assert
            actualSearchText.Should().Be(overriddenText, "because setting SearchText should override the base Text property value.");
        }

        /// <summary>
        /// Tests that setting SearchText to null causes the getter to return the base Text property.
        /// </summary>
        [Fact]
        public void SearchText_Get_AfterSettingNull_ReturnsBaseText()
        {
            // Arrange
            var searchableItem = new SearchableItem();
            const string baseText = "BaseValue";
            searchableItem.Text = baseText;
            searchableItem.SearchText = "TemporaryValue";

            // Act
            searchableItem.SearchText = null;
            string actualSearchText = searchableItem.SearchText;

            // Assert
            actualSearchText.Should().Be(baseText, "because resetting SearchText to null should make the getter return the base Text property value.");
        }

        /// <summary>
        /// Tests that setting SearchText to an empty string returns the empty string, even if base Text has a different value.
        /// </summary>
        [Fact]
        public void SearchText_Get_WhenSetToEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var searchableItem = new SearchableItem();
            searchableItem.Text = "BaseValue";
            const string emptyValue = "";

            // Act
            searchableItem.SearchText = emptyValue;
            string actualSearchText = searchableItem.SearchText;

            // Assert
            actualSearchText.Should().Be(emptyValue, "because setting SearchText to an empty string should override the base Text value.");
        }

        /// <summary>
        /// Tests that when neither the base Text property nor SearchText is set, the getter returns null.
        /// </summary>
        [Fact]
        public void SearchText_Get_WhenNeitherSet_ReturnsNull()
        {
            // Arrange
            var searchableItem = new SearchableItem();
            // Neither Text nor SearchText is explicitly set, assuming default is null.

            // Act
            string actualSearchText = searchableItem.SearchText;

            // Assert
            actualSearchText.Should().BeNull("because when both the search text and base text are null, the getter should return null.");
        }
    }
}
