using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="RemoveItem"/> class.
    /// </summary>
    public class RemoveItemTests
    {
        /// <summary>
        /// Tests that the constructor sets the DisableChildrenCache property to true.
        /// </summary>
        [Fact]
        public void Constructor_Always_SetsDisableChildrenCacheTrue()
        {
            // Arrange & Act
            var removeItem = new RemoveItem();

            // Assert
            removeItem.DisableChildrenCache.Should().BeTrue("the constructor should set DisableChildrenCache to true");
        }

        /// <summary>
        /// Tests that the TypeName property correctly returns the class name.
        /// </summary>
        [Fact]
        public void TypeName_Get_ReturnsRemoveItem()
        {
            // Arrange
            var removeItem = new RemoveItem();

            // Act
            string typeName = removeItem.TypeName;

            // Assert
            typeName.Should().Be("RemoveItem", "TypeName should reflect the name of the class");
        }

        /// <summary>
        /// Tests that the LineNumber property can be correctly get and set, including when setting to null.
        /// </summary>
        [Fact]
        public void LineNumber_Property_GetSet_WorksCorrectly()
        {
            // Arrange
            var removeItem = new RemoveItem();

            // Act & Assert: By default, LineNumber should be null.
            removeItem.LineNumber.Should().BeNull("LineNumber should be null by default");

            // Act: Set LineNumber to a specific value.
            int testValue = 42;
            removeItem.LineNumber = testValue;

            // Assert: Confirm that LineNumber returns the value set.
            removeItem.LineNumber.Should().Be(testValue, "LineNumber should return the value that was set");

            // Act: Reset LineNumber to null.
            removeItem.LineNumber = null;

            // Assert: Confirm that LineNumber is null.
            removeItem.LineNumber.Should().BeNull("LineNumber should allow being reset to null");
        }
    }
}
