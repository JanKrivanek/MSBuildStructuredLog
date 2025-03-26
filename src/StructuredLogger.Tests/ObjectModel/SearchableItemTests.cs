using System;
using Microsoft.Build.Logging.StructuredLogger;
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
        /// Tests that the constructor of SearchableItem successfully initializes an instance.
        /// </summary>
        [Fact]
        public void Constructor_Always_InitializesObject()
        {
            // Act
            var item = new SearchableItem();

            // Assert
            Assert.NotNull(item);
        }

        /// <summary>
        /// Tests that the SearchText getter returns the base Text value when no explicit search text is set.
        /// </summary>
        [Fact]
        public void SearchText_GetWithoutExplicitSet_ReturnsBaseText()
        {
            // Arrange
            string baseText = "Base text";
            _searchableItem.Text = baseText; // Assuming 'Text' is defined in the base class 'Item'

            // Act
            string result = _searchableItem.SearchText;

            // Assert
            Assert.Equal(baseText, result);
        }

        /// <summary>
        /// Tests that the SearchText getter returns the explicitly set search text even when the base Text property differs.
        /// </summary>
        [Fact]
        public void SearchText_GetAfterExplicitSet_ReturnsExplicitValue()
        {
            // Arrange
            string baseText = "Base text";
            string explicitSearchText = "Explicit search text";
            _searchableItem.Text = baseText;
            _searchableItem.SearchText = explicitSearchText;

            // Act
            string result = _searchableItem.SearchText;

            // Assert
            Assert.Equal(explicitSearchText, result);
        }

        /// <summary>
        /// Tests that setting the SearchText property to null causes the getter to revert to returning the base Text value.
        /// </summary>
        [Fact]
        public void SearchText_SetToNull_ReturnsBaseText()
        {
            // Arrange
            string baseText = "Base text";
            _searchableItem.Text = baseText;
            _searchableItem.SearchText = "Some explicit text";

            // Act
            _searchableItem.SearchText = null;
            string result = _searchableItem.SearchText;

            // Assert
            Assert.Equal(baseText, result);
        }

        /// <summary>
        /// Tests that when both the explicit search text and base Text are null, the SearchText getter returns null.
        /// </summary>
        [Fact]
        public void SearchText_WhenBothExplicitAndBaseAreNull_ReturnsNull()
        {
            // Arrange
            _searchableItem.Text = null;
            _searchableItem.SearchText = null;

            // Act
            string result = _searchableItem.SearchText;

            // Assert
            Assert.Null(result);
        }
    }
}
