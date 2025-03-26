using Moq;
using System;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Item"/> class.
    /// </summary>
    public class ItemTests
    {
        /// <summary>
        /// Tests that the default constructor initializes the Item
        /// and the TypeName property returns "Item".
        /// </summary>
//         [Fact] [Error] (24-39)CS1061 'Item' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Item' could be found (are you missing a using directive or an assembly reference?)
//         public void Constructor_WhenCalled_SetsTypeNameToItem()
//         {
//             // Arrange & Act
//             var item = new Item();
// 
//             // Assert
//             Assert.Equal("Item", item.TypeName);
//         }

        /// <summary>
        /// Tests the Text property getter and setter with various inputs.
        /// Verifies that the underlying Name property (accessed via Text) correctly reflects the assigned value.
        /// </summary>
        /// <param name="testValue">The test value to assign to the Text property.</param>
//         [Theory] [Error] (42-18)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text' [Error] (43-31)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text'
//         [InlineData(null)]
//         [InlineData("")]
//         [InlineData("TestName")]
//         public void TextProperty_WhenSet_ReturnsExpectedValue(string testValue)
//         {
//             // Arrange
//             var item = new Item();
// 
//             // Act
//             item.Text = testValue;
//             var result = item.Text;
// 
//             // Assert
//             Assert.Equal(testValue, result);
//         }
    }

    /// <summary>
    /// Unit tests for the <see cref="FileCopy"/> class.
    /// </summary>
    public class FileCopyTests
    {
        /// <summary>
        /// Tests that FileCopy inherits the behavior of Item.
        /// Specifically, verifies that the TypeName property returns "Item" despite being a FileCopy instance.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_InheritsTypeNameFromItem()
        {
            // Arrange & Act
            var fileCopy = new FileCopy();

            // Assert
            Assert.Equal("Item", fileCopy.TypeName);
        }

        /// <summary>
        /// Tests that the Kind property getter and setter works as expected.
        /// It verifies that the default value is null and that setting a value returns the expected result.
        /// </summary>
        [Fact]
        public void KindProperty_DefaultValue_IsNull()
        {
            // Arrange
            var fileCopy = new FileCopy();

            // Act
            var kindValue = fileCopy.Kind;

            // Assert
            Assert.Null(kindValue);
        }

        /// <summary>
        /// Tests the Kind property getter and setter with various inputs.
        /// Ensures that the Kind property correctly returns the assigned value.
        /// </summary>
        /// <param name="testValue">The test value to assign to the Kind property.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("CopyOperation")]
        public void KindProperty_WhenSet_ReturnsExpectedValue(string testValue)
        {
            // Arrange
            var fileCopy = new FileCopy();

            // Act
            fileCopy.Kind = testValue;
            var result = fileCopy.Kind;

            // Assert
            Assert.Equal(testValue, result);
        }

        /// <summary>
        /// Tests that the inherited Text property from Item functions correctly in FileCopy.
        /// Ensures that setting and getting the Text property works as expected.
        /// </summary>
        /// <param name="testValue">The test value to assign to the Text property.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("FileCopyItem")]
        public void TextProperty_WhenSet_ReturnsExpectedValue(string testValue)
        {
            // Arrange
            var fileCopy = new FileCopy();

            // Act
            fileCopy.Text = testValue;
            var result = fileCopy.Text;

            // Assert
            Assert.Equal(testValue, result);
        }
    }
}
