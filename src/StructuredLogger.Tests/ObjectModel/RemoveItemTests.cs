using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="RemoveItem"/> class.
    /// </summary>
    public class RemoveItemTests
    {
        private readonly RemoveItem _removeItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveItemTests"/> class.
        /// </summary>
        public RemoveItemTests()
        {
            _removeItem = new RemoveItem();
        }

        /// <summary>
        /// Tests that the constructor sets the DisableChildrenCache property to true.
        /// </summary>
        [Fact]
        public void Constructor_Always_SetsDisableChildrenCacheToTrue()
        {
            // Arrange & Act are already done in the constructor.

            // Assert
            // Assuming DisableChildrenCache is publicly accessible via the base type.
            _removeItem.DisableChildrenCache.Should().BeTrue("the constructor should set DisableChildrenCache to true");
        }

        /// <summary>
        /// Tests that the TypeName property getter returns the expected type name.
        /// </summary>
        [Fact]
        public void TypeName_Getter_ReturnsExpectedTypeName()
        {
            // Arrange
            string expectedTypeName = nameof(RemoveItem);

            // Act
            string actualTypeName = _removeItem.TypeName;

            // Assert
            actualTypeName.Should().Be(expectedTypeName, "TypeName should return the name of the RemoveItem class");
        }

        /// <summary>
        /// Tests that the LineNumber property getter and setter work correctly when set to a valid integer.
        /// </summary>
        /// <param name="lineNumber">The integer value to assign to LineNumber.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void LineNumber_SetWithInteger_GetterReturnsSameValue(int lineNumber)
        {
            // Arrange
            // Act
            _removeItem.LineNumber = lineNumber;
            int? actualLineNumber = _removeItem.LineNumber;

            // Assert
            actualLineNumber.Should().Be(lineNumber, "LineNumber should return the value that was set");
        }

        /// <summary>
        /// Tests that the LineNumber property getter and setter work correctly when set to null.
        /// </summary>
        [Fact]
        public void LineNumber_SetToNull_GetterReturnsNull()
        {
            // Arrange
            // Act
            _removeItem.LineNumber = null;
            int? actualLineNumber = _removeItem.LineNumber;

            // Assert
            actualLineNumber.Should().BeNull("LineNumber should be null when set to null");
        }
    }
}
