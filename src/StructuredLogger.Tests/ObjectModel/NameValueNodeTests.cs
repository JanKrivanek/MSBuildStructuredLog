using System;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="NameValueNode"/> class.
    /// </summary>
    public class NameValueNodeTests
    {
        /// <summary>
        /// Tests that the NameAndEquals property returns the correct concatenation of Name and " = ".
        /// </summary>
        [Fact]
        public void NameAndEquals_WithName_ReturnsNamePlusEquals()
        {
            // Arrange
            var node = new NameValueNode
            {
                Name = "TestName"
            };

            // Act
            var result = node.NameAndEquals;

            // Assert
            Assert.Equal("TestName = ", result);
        }

        /// <summary>
        /// Tests that the TypeName property always returns "NameValueNode".
        /// </summary>
        [Fact]
        public void TypeName_Always_ReturnsNameValueNode()
        {
            // Arrange
            var node = new NameValueNode();

            // Act
            var result = node.TypeName;

            // Assert
            Assert.Equal("NameValueNode", result);
        }

        /// <summary>
        /// Tests that the Title property returns the value of the Name property.
        /// </summary>
        [Fact]
        public void Title_WithName_ReturnsNameProperty()
        {
            // Arrange
            var node = new NameValueNode
            {
                Name = "TitleTest"
            };

            // Act
            var result = node.Title;

            // Assert
            Assert.Equal("TitleTest", result);
        }

        /// <summary>
        /// Tests that the ToString method returns the concatenation of Name, " = ", and Value.
        /// </summary>
        [Fact]
        public void ToString_WithNameAndValue_ReturnsConcatenatedString()
        {
            // Arrange
            var node = new NameValueNode
            {
                Name = "N",
                Value = "V"
            };

            // Act
            var result = node.ToString();

            // Assert
            Assert.Equal("N = V", result);
        }

        /// <summary>
        /// Tests that the GetFullText method returns the same string as the ToString method.
        /// </summary>
        [Fact]
        public void GetFullText_ReturnsSameAsToString()
        {
            // Arrange
            var node = new NameValueNode
            {
                Name = "Name",
                Value = "Value"
            };

            // Act
            var toStringResult = node.ToString();
            var fullTextResult = node.GetFullText();

            // Assert
            Assert.Equal(toStringResult, fullTextResult);
        }

        /// <summary>
        /// Tests that the IsVisible property always returns true.
        /// </summary>
        [Fact]
        public void IsVisible_Always_ReturnsTrue()
        {
            // Arrange
            var node = new NameValueNode();

            // Act
            var isVisible = node.IsVisible;

            // Assert
            Assert.True(isVisible);
        }

        /// <summary>
        /// Tests that the IsExpanded property always returns true.
        /// </summary>
        [Fact]
        public void IsExpanded_Always_ReturnsTrue()
        {
            // Arrange
            var node = new NameValueNode();

            // Act
            var isExpanded = node.IsExpanded;

            // Assert
            Assert.True(isExpanded);
        }

        /// <summary>
        /// Tests that the ShortenedValue property returns the expected shortened version of the Value.
        /// This ensures that the property delegates to TextUtilities.ShortenValue correctly.
        /// </summary>
        [Fact]
        public void ShortenedValue_WithValue_ReturnsShortenedVersion()
        {
            // Arrange
            const string originalValue = "SomeLongValueThatCouldBeShortened";
            var node = new NameValueNode
            {
                Value = originalValue
            };

            // Act
            var shortened = node.ShortenedValue;

            // Assert
            var expected = TextUtilities.ShortenValue(originalValue);
            Assert.Equal(expected, shortened);
        }

        /// <summary>
        /// Tests that the IsValueShortened property returns false when the Value is short enough not to be altered.
        /// </summary>
        [Fact]
        public void IsValueShortened_WhenValueIsShort_ReturnsFalse()
        {
            // Arrange
            const string originalValue = "Short";
            var node = new NameValueNode
            {
                Value = originalValue
            };

            // Act
            var isShortened = node.IsValueShortened;

            // Assert
            // Assuming for short values, TextUtilities.ShortenValue returns the same string.
            Assert.False(isShortened);
        }

        /// <summary>
        /// Tests that the IsValueShortened property behaves correctly when the Value is long.
        /// It checks if the shortened value differs from the original, and if so, IsValueShortened returns true.
        /// </summary>
        [Fact]
        public void IsValueShortened_WhenValueIsLong_ReturnsTrueIfShortened()
        {
            // Arrange
            const string originalValue = "This is a very long value that should be shortened by the utility method.";
            var node = new NameValueNode
            {
                Value = originalValue
            };

            // Act
            var shortened = node.ShortenedValue;

            // Assert
            if (originalValue != shortened)
            {
                Assert.True(node.IsValueShortened);
            }
            else
            {
                Assert.False(node.IsValueShortened);
            }
        }

        /// <summary>
        /// Tests the behavior of the class when both Name and Value properties are set to null.
        /// It verifies that string concatenation handles null values appropriately.
        /// </summary>
        [Fact]
        public void NullValues_NullNameAndValue_PropertiesBehaveAsExpected()
        {
            // Arrange
            var node = new NameValueNode
            {
                Name = null,
                Value = null
            };

            // Act
            var nameAndEquals = node.NameAndEquals;
            var toStringResult = node.ToString();
            var fullTextResult = node.GetFullText();

            // Assert
            // In C#, concatenating null with strings treats null as an empty string.
            Assert.Equal(" = ", nameAndEquals);
            Assert.Equal(" = ", toStringResult);
            Assert.Equal(" = ", fullTextResult);

            // Additionally, verify that accessing ShortenedValue and IsValueShortened doesn't cause exceptions.
            var shortened = node.ShortenedValue;
            Assert.False(node.IsValueShortened);
        }
    }
}
