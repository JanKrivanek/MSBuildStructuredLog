using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="SearchableItem"/> class.
    /// </summary>
    public class SearchableItemTests
    {
        private readonly SearchableItem _searchableItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchableItemTests"/> class.
        /// </summary>
        public SearchableItemTests()
        {
            _searchableItem = new SearchableItem();
        }

        /// <summary>
        /// Tests that when SearchText is not explicitly set, the getter returns the base Text value.
        /// This test arranges by setting the inherited Text property, 
        /// then verifies that the SearchText property returns the same value.
        /// </summary>
        [Fact]
        public void SearchText_Get_WhenNotSet_ReturnsBaseText()
        {
            // Arrange
            const string baseText = "Default Base Text";
            _searchableItem.Text = baseText;

            // Act
            string result = _searchableItem.SearchText;

            // Assert
            result.Should().Be(baseText, "because when SearchText is not explicitly set, it should fall back to the base Text property value");
        }

        /// <summary>
        /// Tests that after setting SearchText explicitly, the getter returns the assigned value,
        /// even if the base Text property is also set.
        /// </summary>
        [Fact]
        public void SearchText_Get_AfterBeingSet_ReturnsAssignedValue()
        {
            // Arrange
            const string baseText = "Default Base Text";
            const string customSearchText = "Custom Search Text";
            _searchableItem.Text = baseText;
            _searchableItem.SearchText = customSearchText;

            // Act
            string result = _searchableItem.SearchText;

            // Assert
            result.Should().Be(customSearchText, "because when SearchText is explicitly set, it should return the custom value regardless of the base Text property");
        }

        /// <summary>
        /// Tests that setting SearchText to an empty string results in an empty string being returned.
        /// This covers the edge case for empty string input.
        /// </summary>
        [Fact]
        public void SearchText_Set_ToEmptyString_ReturnsEmptyString()
        {
            // Arrange
            const string emptyString = "";
            _searchableItem.SearchText = emptyString;

            // Act
            string result = _searchableItem.SearchText;

            // Assert
            result.Should().Be(emptyString, "because setting SearchText to an empty string should return an empty string");
        }

        /// <summary>
        /// Tests that setting SearchText to a very long string returns the same long string.
        /// This covers the boundary condition for excessive length input.
        /// </summary>
        [Fact]
        public void SearchText_Set_ToLongString_ReturnsLongString()
        {
            // Arrange
            string longString = new string('x', 10000); // construct a 10,000-character long string
            _searchableItem.SearchText = longString;

            // Act
            string result = _searchableItem.SearchText;

            // Assert
            result.Should().Be(longString, "because setting SearchText to a long string should preserve the full value");
        }

        /// <summary>
        /// Tests that setting SearchText with special characters returns the same string with special characters.
        /// This covers edge cases with unusual characters.
        /// </summary>
        [Fact]
        public void SearchText_Set_WithSpecialCharacters_ReturnsSameString()
        {
            // Arrange
            string specialCharString = "!@#$%^&*()_+-=[]{}|;':,./<>?";
            _searchableItem.SearchText = specialCharString;

            // Act
            string result = _searchableItem.SearchText;

            // Assert
            result.Should().Be(specialCharString, "because setting SearchText with special characters should return the exact same string");
        }
    }
}
