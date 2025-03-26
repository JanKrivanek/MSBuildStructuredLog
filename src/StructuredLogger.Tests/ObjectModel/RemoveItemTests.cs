using Microsoft.Build.Logging.StructuredLogger;
using System.Reflection;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="RemoveItem"/> class.
    /// </summary>
    public class RemoveItemTests
    {
        /// <summary>
        /// Tests the default constructor of <see cref="RemoveItem"/>.
        /// Verifies that upon instantiation, the DisableChildrenCache property (inherited from NamedNode) is set to true,
        /// the TypeName property returns "RemoveItem", and the LineNumber property is null.
        /// </summary>
        [Fact]
        public void Constructor_Always_InitializesPropertiesCorrectly()
        {
            // Arrange & Act
            var removeItem = new RemoveItem();

            // Assert: Verify that TypeName returns "RemoveItem"
            Assert.Equal("RemoveItem", removeItem.TypeName);

            // Assert: Verify that LineNumber is null by default
            Assert.Null(removeItem.LineNumber);

            // Assert: Verify that DisableChildrenCache is set to true via reflection.
            // This property is assumed to be defined in the base class NamedNode.
            PropertyInfo disableChildrenCacheProperty = removeItem.GetType().GetProperty("DisableChildrenCache", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Assert.NotNull(disableChildrenCacheProperty);

            object value = disableChildrenCacheProperty.GetValue(removeItem);
            Assert.True(value is bool flag && flag, "DisableChildrenCache should be true after construction.");
        }

        /// <summary>
        /// Tests that the <see cref="RemoveItem.TypeName"/> property returns its expected value.
        /// Expected outcome: "RemoveItem".
        /// </summary>
        [Fact]
        public void TypeName_Get_ReturnsRemoveItem()
        {
            // Arrange
            var removeItem = new RemoveItem();

            // Act
            string typeName = removeItem.TypeName;

            // Assert
            Assert.Equal("RemoveItem", typeName);
        }

        /// <summary>
        /// Tests the getter and setter functionality of the <see cref="RemoveItem.LineNumber"/> property.
        /// Verifies that a value can be assigned and retrieved, and that it can also be reset to null.
        /// </summary>
        [Fact]
        public void LineNumber_GetSet_WorksAsExpected()
        {
            // Arrange
            var removeItem = new RemoveItem();

            // Act & Assert: Verify the default value is null.
            Assert.Null(removeItem.LineNumber);

            // Act: Set LineNumber to a specific value.
            int expectedLineNumber = 100;
            removeItem.LineNumber = expectedLineNumber;

            // Assert: Retrieve the same value.
            Assert.Equal(expectedLineNumber, removeItem.LineNumber);

            // Act: Reset LineNumber to null.
            removeItem.LineNumber = null;

            // Assert: Verify that LineNumber is null.
            Assert.Null(removeItem.LineNumber);
        }
    }
}
