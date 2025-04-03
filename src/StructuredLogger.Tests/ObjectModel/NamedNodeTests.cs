using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="NamedNode"/> class.
    /// </summary>
    public class NamedNodeTests
    {
        private readonly NamedNode _namedNode;

        public NamedNodeTests()
        {
            _namedNode = new NamedNode();
        }

        /// <summary>
        /// Tests that the Name property can be set and returned correctly.
        /// </summary>
        /// <param name="testName">The test string to assign as Name.</param>
        [Theory]
        [InlineData("TestName")]
        [InlineData("")]
        [InlineData("A very long name that might be shortened")]
        [InlineData("特殊字符")]
        public void NameProperty_SetAndGet_ReturnsSameValue(string testName)
        {
            // Arrange
            _namedNode.Name = testName;

            // Act
            string actualName = _namedNode.Name;

            // Assert
            actualName.Should().Be(testName, "because the Name property should return the value that was set");
        }

        /// <summary>
        /// Tests that ShortenedName returns the value computed by TextUtilities.ShortenValue,
        /// and that IsNameShortened returns true if shortening has occurred.
        /// </summary>
        /// <param name="testName">The test string to assign as Name.</param>
        [Theory]
        [InlineData("TestName")]
        [InlineData("")]
        [InlineData("A very long name that might be shortened")]
        [InlineData("特殊字符")]
        public void ShortenedNameProperty_WhenNameIsSet_ReturnsExpectedShortenedValue(string testName)
        {
            // Arrange
            _namedNode.Name = testName;
            var expectedShortenedValue = global::Microsoft.Build.Logging.StructuredLogger.TextUtilities.ShortenValue(testName);

            // Act
            string actualShortenedName = _namedNode.ShortenedName;
            bool isNameShortened = _namedNode.IsNameShortened;

            // Assert
            actualShortenedName.Should().Be(expectedShortenedValue, "because ShortenedName computes its value using TextUtilities.ShortenValue");
            isNameShortened.Should().Be(testName != expectedShortenedValue, "because IsNameShortened should indicate if the Name was altered by shortening");
        }

        /// <summary>
        /// Tests that the TypeName property returns the expected type name.
        /// </summary>
        [Fact]
        public void TypeNameProperty_ReturnsNamedNodeString()
        {
            // Act
            string actualTypeName = _namedNode.TypeName;

            // Assert
            actualTypeName.Should().Be("NamedNode", "because TypeName returns the name of the class via nameof(NamedNode)");
        }

        /// <summary>
        /// Tests that the Title property returns the same value as the Name property.
        /// </summary>
        /// <param name="testName">The test string to assign as Name.</param>
        [Theory]
        [InlineData("TestName")]
        [InlineData("AnotherName")]
        [InlineData("特殊字符")]
        public void TitleProperty_WhenNameIsSet_ReturnsSameValueAsName(string testName)
        {
            // Arrange
            _namedNode.Name = testName;

            // Act
            string actualTitle = _namedNode.Title;

            // Assert
            actualTitle.Should().Be(testName, "because Title returns the Name property");
        }

        /// <summary>
        /// Tests that the ToString method returns the same value as the Name property.
        /// </summary>
        /// <param name="testName">The test string to assign as Name.</param>
        [Theory]
        [InlineData("TestName")]
        [InlineData("ExampleName")]
        [InlineData("特殊字符")]
        public void ToString_WhenNameIsSet_ReturnsSameValueAsName(string testName)
        {
            // Arrange
            _namedNode.Name = testName;

            // Act
            string actualToString = _namedNode.ToString();

            // Assert
            actualToString.Should().Be(testName, "because ToString is overridden to return the Name property");
        }
    }
}
