using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="NameValueNode"/> class.
    /// </summary>
    public class NameValueNodeTests
    {
        private readonly NameValueNode _node;

        public NameValueNodeTests()
        {
            _node = new NameValueNode();
        }

        /// <summary>
        /// Tests that the NameAndEquals property correctly returns the Name concatenated with " = ".
        /// </summary>
        [Fact]
        public void NameAndEquals_WhenNameIsSet_ReturnsNameWithEqualsSign()
        {
            // Arrange
            const string expectedName = "TestName";
            _node.Name = expectedName;
            _node.Value = "AnyValue";

            // Act
            string result = _node.NameAndEquals;

            // Assert
            result.Should().Be(expectedName + " = ");
        }

        /// <summary>
        /// Tests that the Title property returns the same string as the Name property.
        /// </summary>
        [Fact]
        public void Title_WhenNameIsSet_ReturnsSameValueAsName()
        {
            // Arrange
            const string testName = "SampleName";
            _node.Name = testName;

            // Act
            string title = _node.Title;

            // Assert
            title.Should().Be(testName);
        }

        /// <summary>
        /// Tests that the TypeName property returns the correct name of the class.
        /// </summary>
        [Fact]
        public void TypeName_ReturnsNameValueNode()
        {
            // Act
            string typeName = _node.TypeName;

            // Assert
            typeName.Should().Be(nameof(NameValueNode));
        }

        /// <summary>
        /// Tests that the ToString method returns a concatenated string in the form "Name = Value".
        /// </summary>
        [Fact]
        public void ToString_WhenCalled_ReturnsConcatenationOfNameAndValue()
        {
            // Arrange
            const string testName = "Key";
            const string testValue = "Value";
            _node.Name = testName;
            _node.Value = testValue;
            string expected = $"{testName} = {testValue}";

            // Act
            string result = _node.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that the GetFullText method returns the same result as the ToString method.
        /// </summary>
        [Fact]
        public void GetFullText_WhenCalled_ReturnsSameResultAsToString()
        {
            // Arrange
            const string testName = "PropName";
            const string testValue = "PropValue";
            _node.Name = testName;
            _node.Value = testValue;

            // Act
            string fullText = _node.GetFullText();
            string toStringResult = _node.ToString();

            // Assert
            fullText.Should().Be(toStringResult);
        }

        /// <summary>
        /// Tests that the IsVisible property always returns true, regardless of set operations.
        /// </summary>
        [Fact]
        public void IsVisible_PropertyAlways_ReturnsTrue()
        {
            // Act
            bool initialIsVisible = _node.IsVisible;
            _node.IsVisible = false; // Setter should have no effect.

            // Assert
            _node.IsVisible.Should().BeTrue();
            initialIsVisible.Should().BeTrue();
        }

        /// <summary>
        /// Tests that the IsExpanded property always returns true, regardless of set operations.
        /// </summary>
        [Fact]
        public void IsExpanded_PropertyAlways_ReturnsTrue()
        {
            // Act
            bool initialIsExpanded = _node.IsExpanded;
            _node.IsExpanded = false; // Setter should have no effect.

            // Assert
            _node.IsExpanded.Should().BeTrue();
            initialIsExpanded.Should().BeTrue();
        }

        /// <summary>
        /// Tests that when a short value is assigned, ShortenedValue returns the original value and IsValueShortened is false.
        /// </summary>
        [Fact]
        public void ShortenedValue_WhenValueIsShort_ReturnsOriginalAndIsValueShortenedFalse()
        {
            // Arrange
            const string shortValue = "Short";
            _node.Value = shortValue;

            // Act
            string shortenedValue = _node.ShortenedValue;
            bool isValueShortened = _node.IsValueShortened;

            // Assert
            shortenedValue.Should().Be(shortValue);
            isValueShortened.Should().BeFalse();
        }

        /// <summary>
        /// Tests that when a long value is assigned, ShortenedValue returns a shortened version and IsValueShortened is true.
        /// </summary>
        [Fact]
        public void ShortenedValue_WhenValueIsLong_ReturnsShortenedValueAndIsValueShortenedTrue()
        {
            // Arrange
            // Create a long string; assuming TextUtilities.ShortenValue shortens sufficiently long strings.
            string longValue = new string('a', 100);
            _node.Value = longValue;

            // Act
            string shortenedValue = _node.ShortenedValue;
            bool isValueShortened = _node.IsValueShortened;

            // Assert
            shortenedValue.Should().NotBe(longValue);
            isValueShortened.Should().BeTrue();
        }
    }
}
