using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Microsoft.Build.Logging.StructuredLogger.UnitTests;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref = "NameValueNode"/> class.
    /// </summary>
    public class NameValueNodeTests
    {
        private readonly NameValueNode _node;
        /// <summary>
        /// Initializes a new instance of the <see cref = "NameValueNodeTests"/> class.
        /// </summary>
        public NameValueNodeTests()
        {
            _node = new NameValueNode();
        }
//  // [Error] (37-35)CS1061 'NameValueNode' does not contain a definition for 'NameAndEquals' and no accessible extension method 'NameAndEquals' accepting a first argument of type 'NameValueNode' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the NameAndEquals property returns the correct formatted string based on the Name property.
//         /// </summary>
//         /// <param name = "name">The name to set in the node.</param>
//         /// <param name = "expected">The expected formatted string.</param>
//         [Theory]
//         [InlineData("TestName", "TestName = ")]
//         [InlineData("", " = ")]
//         [InlineData("Special@#%", "Special@#% = ")]
//         public void NameAndEquals_WhenCalled_ReturnsNameAndEquals(string name, string expected)
//         {
//             // Arrange
//             _node.Name = name;
//             // Act
//             string actual = _node.NameAndEquals;
//             // Assert
//             actual.Should().Be(expected);
//         }
//  // [Error] (57-35)CS1061 'NameValueNode' does not contain a definition for 'ShortenedValue' and no accessible extension method 'ShortenedValue' accepting a first argument of type 'NameValueNode' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests that the ShortenedValue property returns a value equivalent to the output of TextUtilities.ShortenValue.
        /// </summary>
        /// <param name = "value">The value to set in the node.</param>
        /// <param name = "dummyExpected">
        /// Dummy expected parameter; if null, the result is compared with the computed shortened value.
        /// </param>
        [Theory]
        [InlineData("Short", null)] // Assuming short values are not altered.
        [InlineData("A very long value that should be shortened", null)]
        public void ShortenedValue_WhenCalled_ReturnsConsistentShortenedValue(string value, string? dummyExpected)
        {
            // Arrange
            _node.Value = value;
            // Act
            string actual = _node.ShortenedValue;
            string expectedComputed = global::Microsoft.Build.Logging.StructuredLogger.TextUtilities.ShortenValue(value);
            // Assert
            actual.Should().Be(expectedComputed);
        }
//  // [Error] (77-38)CS1061 'NameValueNode' does not contain a definition for 'ShortenedValue' and no accessible extension method 'ShortenedValue' accepting a first argument of type 'NameValueNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (80-38)CS1061 'NameValueNode' does not contain a definition for 'IsValueShortened' and no accessible extension method 'IsValueShortened' accepting a first argument of type 'NameValueNode' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the IsValueShortened property correctly indicates whether the Value has been shortened.
//         /// </summary>
//         /// <param name = "value">The value to test.</param>
//         /// <param name = "expectedFlag">
//         /// The expected flag is set based on whether the shortened value differs from the original.
//         /// </param>
//         [Theory]
//         [InlineData("Short", false)] // Expect no shortening.
//         [InlineData("A very long value that should be shortened", true)] // Expect shortening.
//         public void IsValueShortened_WhenCalled_ReturnsCorrectFlag(string value, bool expectedFlag)
//         {
//             // Arrange
//             _node.Value = value;
//             string shortened = _node.ShortenedValue;
//             bool computedFlag = value != shortened;
//             // Act
//             bool isShortened = _node.IsValueShortened;
//             // Assert
//             isShortened.Should().Be(computedFlag);
//             isShortened.Should().Be(expectedFlag, $"Because for value '{value}', expected IsValueShortened to be {expectedFlag}");
//         }
//  // [Error] (99-34)CS1061 'NameValueNode' does not contain a definition for 'Title' and no accessible extension method 'Title' accepting a first argument of type 'NameValueNode' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests that the Title property returns the same value as the Name property.
        /// </summary>
        /// <param name = "name">The name to set in the node.</param>
        [Theory]
        [InlineData("TitleTest")]
        [InlineData("")]
        [InlineData("123")]
        public void Title_WhenCalled_ReturnsName(string name)
        {
            // Arrange
            _node.Name = name;
            // Act
            string title = _node.Title;
            // Assert
            title.Should().Be(name);
        }
//  // [Error] (111-37)CS1061 'NameValueNode' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'NameValueNode' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the TypeName property returns the correct type name "NameValueNode".
//         /// </summary>
//         [Fact]
//         public void TypeName_WhenCalled_ReturnsNameValueNodeString()
//         {
//             // Act
//             string typeName = _node.TypeName;
//             // Assert
//             typeName.Should().Be(nameof(NameValueNode));
//         }
// 
        /// <summary>
        /// Tests that the ToString method returns a concatenated string in the format "Name = Value".
        /// </summary>
        /// <param name = "name">The name to set.</param>
        /// <param name = "value">The value to set.</param>
        /// <param name = "expected">The expected concatenated string.</param>
        [Theory]
        [InlineData("TestName", "TestValue", "TestName = TestValue")]
        [InlineData("", "", " = ")]
        [InlineData("Name", "", "Name = ")]
        [InlineData("", "Value", " = Value")]
        public void ToString_WhenCalled_ReturnsCorrectConcatenation(string name, string value, string expected)
        {
            // Arrange
            _node.Name = name;
            _node.Value = value;
            // Act
            string result = _node.ToString();
            // Assert
            result.Should().Be(expected);
        }
//  // [Error] (153-37)CS1061 'NameValueNode' does not contain a definition for 'GetFullText' and no accessible extension method 'GetFullText' accepting a first argument of type 'NameValueNode' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the GetFullText method returns the same value as the ToString method.
//         /// </summary>
//         /// <param name = "name">The name to set.</param>
//         /// <param name = "value">The value to set.</param>
//         /// <param name = "expected">The expected full text.</param>
//         [Theory]
//         [InlineData("LongName", "LongValue", "LongName = LongValue")]
//         public void GetFullText_WhenCalled_ReturnsSameAsToString(string name, string value, string expected)
//         {
//             // Arrange
//             _node.Name = name;
//             _node.Value = value;
//             string toStringResult = _node.ToString();
//             // Act
//             string fullText = _node.GetFullText();
//             // Assert
//             fullText.Should().Be(toStringResult);
//             fullText.Should().Be(expected);
//         }
//  // [Error] (170-34)CS1061 'NameValueNode' does not contain a definition for 'Title' and no accessible extension method 'Title' accepting a first argument of type 'NameValueNode' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests that setting the Name property updates the Title property accordingly.
        /// </summary>
        /// <param name = "name">The name to set.</param>
        [Theory]
        [InlineData("NameTest")]
        public void SettingName_WhenCalled_UpdatesTitle(string name)
        {
            // Arrange
            _node.Name = name;
            // Act
            string title = _node.Title;
            // Assert
            title.Should().Be(name);
        }
//  // [Error] (182-19)CS1061 'NameValueNode' does not contain a definition for 'IsVisible' and no accessible extension method 'IsVisible' accepting a first argument of type 'NameValueNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (184-36)CS1061 'NameValueNode' does not contain a definition for 'IsVisible' and no accessible extension method 'IsVisible' accepting a first argument of type 'NameValueNode' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the IsVisible property always returns true, even when set to false.
//         /// </summary>
//         [Fact]
//         public void IsVisible_WhenSetToFalse_ReturnsTrue()
//         {
//             // Arrange
//             _node.IsVisible = false;
//             // Act
//             bool isVisible = _node.IsVisible;
//             // Assert
//             isVisible.Should().BeTrue();
//         }
//  // [Error] (196-19)CS1061 'NameValueNode' does not contain a definition for 'IsExpanded' and no accessible extension method 'IsExpanded' accepting a first argument of type 'NameValueNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (198-37)CS1061 'NameValueNode' does not contain a definition for 'IsExpanded' and no accessible extension method 'IsExpanded' accepting a first argument of type 'NameValueNode' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests that the IsExpanded property always returns true, even when set to false.
        /// </summary>
        [Fact]
        public void IsExpanded_WhenSetToFalse_ReturnsTrue()
        {
            // Arrange
            _node.IsExpanded = false;
            // Act
            bool isExpanded = _node.IsExpanded;
            // Assert
            isExpanded.Should().BeTrue();
        }
    }
}