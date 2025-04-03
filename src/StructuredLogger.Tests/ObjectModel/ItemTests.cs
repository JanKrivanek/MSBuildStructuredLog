using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Item"/> class.
    /// </summary>
    public class ItemTests
    {
        private readonly Item _item;

        public ItemTests()
        {
            _item = new Item();
        }
//  // [Error] (31-18)CS1061 'Item' does not contain a definition for 'DisableChildrenCache' and no accessible extension method 'DisableChildrenCache' accepting a first argument of type 'Item' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the constructor initializes the Item with DisableChildrenCache set to true.
//         /// </summary>
//         [Fact]
//         public void Ctor_Always_SetsDisableChildrenCacheToTrue()
//         {
//             // Act
//             var item = new Item();
//             
//             // Assert
//             // Assuming that DisableChildrenCache is inherited and publicly accessible.
//             item.DisableChildrenCache.Should().BeTrue("the constructor should set DisableChildrenCache to true");
//         }
//  // [Error] (41-21)CS0182 An attribute argument must be a constant expression, typeof expression or array creation expression of an attribute parameter type
//         /// <summary>
//         /// Tests that the Text property getter returns the value set by the Text property setter.
//         /// </summary>
//         /// <param name="input">The string value to set for the Text property.</param>
//         [Theory]
//         [InlineData("Simple text")]
//         [InlineData("")]
//         [InlineData("A very very long text " + "x".PadLeft(1000, 'x'))]
//         [InlineData("Special characters !@#$%^&*()")]
//         public void Text_WhenSet_ReturnsExpectedValue(string input)
//         {
//             // Arrange
//             var item = new Item();
// 
//             // Act
//             item.Text = input;
//             string result = item.Text;
// 
//             // Assert
//             result.Should().Be(input, "the Text property should return the value that was set");
//         }
//  // [Error] (63-37)CS1061 'Item' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Item' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests that the TypeName property returns the expected type name.
        /// </summary>
        [Fact]
        public void TypeName_Always_ReturnsExpectedTypeName()
        {
            // Act
            string typeName = _item.TypeName;

            // Assert
            typeName.Should().Be(nameof(Item), "TypeName should return the name of the type, which is 'Item'");
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="FileCopy"/> class.
    /// </summary>
    public class FileCopyTests
    {
        private readonly FileCopy _fileCopy;

        public FileCopyTests()
        {
            _fileCopy = new FileCopy();
        }

        /// <summary>
        /// Tests that the FileCopy instance inherits the TypeName from its base Item class.
        /// </summary>
        [Fact]
        public void TypeName_Always_ReturnsExpectedTypeNameFromBase()
        {
            // Act
            string typeName = _fileCopy.TypeName;

            // Assert
            typeName.Should().Be(nameof(Item), "FileCopy inherits its TypeName from Item, which returns 'Item'");
        }
//  // [Error] (102-21)CS0182 An attribute argument must be a constant expression, typeof expression or array creation expression of an attribute parameter type
//         /// <summary>
//         /// Tests the Kind property getter and setter with various string values.
//         /// </summary>
//         /// <param name="kindValue">The string value to set for the Kind property.</param>
//         [Theory]
//         [InlineData("Copy")]
//         [InlineData("")]
//         [InlineData("A very long kind value " + "y".PadLeft(500, 'y'))]
//         [InlineData("Special_Kind-123!@#")]
//         public void Kind_WhenSet_ReturnsExpectedValue(string kindValue)
//         {
//             // Arrange
//             var fileCopy = new FileCopy();
// 
//             // Act
//             fileCopy.Kind = kindValue;
//             string result = fileCopy.Kind;
// 
//             // Assert
//             result.Should().Be(kindValue, "the Kind property should return the same value that was set");
//         }
// 
        /// <summary>
        /// Tests that the Text property (inherited from Item) works correctly in a FileCopy instance.
        /// </summary>
        /// <param name="textValue">The string value to set for the Text property.</param>
        [Theory]
        [InlineData("FileCopyText")]
        [InlineData("")]
        [InlineData("Text with special chars #$%^&*")]
        public void Text_WhenSetOnFileCopy_ReturnsExpectedValue(string textValue)
        {
            // Arrange
            var fileCopy = new FileCopy();

            // Act
            fileCopy.Text = textValue;
            string result = fileCopy.Text;

            // Assert
            result.Should().Be(textValue, "the inherited Text property should work correctly in FileCopy");
        }
    }
}
