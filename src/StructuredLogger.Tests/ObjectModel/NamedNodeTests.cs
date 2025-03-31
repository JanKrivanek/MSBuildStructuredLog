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

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedNodeTests"/> class.
        /// </summary>
        public NamedNodeTests()
        {
            _namedNode = new NamedNode();
        }

        /// <summary>
        /// Tests that the Name property can be set and retrieved correctly.
        /// </summary>
        /// <param name="testName">The name to set.</param>
        [Theory]
        [InlineData("TestName")]
        [InlineData("")]
        public void Name_SetAndGet_ReturnsSameValue(string testName)
        {
            // Arrange
            _namedNode.Name = testName;

            // Act
            string actualName = _namedNode.Name;

            // Assert
            actualName.Should().Be(testName, "Name getter should return the value that was set.");
        }
//  // [Error] (54-53)CS1061 'NamedNode' does not contain a definition for 'ShortenedName' and no accessible extension method 'ShortenedName' accepting a first argument of type 'NamedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (55-58)CS0117 'TextUtilities' does not contain a definition for 'ShortenValue'
//         /// <summary>
//         /// Tests that the ShortenedName property returns the shortened version of Name.
//         /// </summary>
//         /// <param name="testName">The name to set.</param>
//         [Theory]
//         [InlineData("ShortName")]
//         [InlineData("A name that is reasonably long and should be shortened")]
//         public void ShortenedName_WhenNameIsSet_ReturnsShortenedVersion(string testName)
//         {
//             // Arrange
//             _namedNode.Name = testName;
// 
//             // Act
//             string actualShortenedName = _namedNode.ShortenedName;
//             string expectedShortenedName = TextUtilities.ShortenValue(testName);
// 
//             // Assert
//             actualShortenedName.Should().Be(expectedShortenedName, "ShortenedName should reflect the shortened version of Name using TextUtilities.");
//         }
//  // [Error] (73-43)CS1061 'NamedNode' does not contain a definition for 'IsNameShortened' and no accessible extension method 'IsNameShortened' accepting a first argument of type 'NamedNode' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests that the IsNameShortened property returns false when the shortened name matches the original Name.
        /// </summary>
        [Fact]
        public void IsNameShortened_WhenNameIsNotShortened_ReturnsFalse()
        {
            // Arrange
            // Using a name that is assumed to not require shortening.
            string testName = "ShortName";
            _namedNode.Name = testName;

            // Act
            bool isShortened = _namedNode.IsNameShortened;

            // Assert
            isShortened.Should().BeFalse("IsNameShortened should be false when the shortened name is identical to the original Name.");
        }
//  // [Error] (91-43)CS1061 'NamedNode' does not contain a definition for 'IsNameShortened' and no accessible extension method 'IsNameShortened' accepting a first argument of type 'NamedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (92-55)CS0117 'TextUtilities' does not contain a definition for 'ShortenValue'
//         /// <summary>
//         /// Tests that the IsNameShortened property returns true when the shortened name differs from the original Name.
//         /// </summary>
//         [Fact]
//         public void IsNameShortened_WhenNameIsShortened_ReturnsTrue()
//         {
//             // Arrange
//             // Use a long name that is expected to be shortened by TextUtilities.
//             string testName = "This is a very long name that should be shortened by the utility function";
//             _namedNode.Name = testName;
// 
//             // Act
//             bool isShortened = _namedNode.IsNameShortened;
//             bool expected = testName != TextUtilities.ShortenValue(testName);
// 
//             // Assert
//             isShortened.Should().Be(expected, "IsNameShortened should reflect whether the shortened version differs from the original Name.");
//         }
//  // [Error] (105-42)CS1061 'NamedNode' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'NamedNode' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests that the TypeName property always returns the class name "NamedNode".
        /// </summary>
        [Fact]
        public void TypeName_Always_ReturnsNamedNode()
        {
            // Act
            string typeName = _namedNode.TypeName;

            // Assert
            typeName.Should().Be(nameof(NamedNode), "TypeName should always return the name of the class.");
        }
//  // [Error] (124-39)CS1061 'NamedNode' does not contain a definition for 'Title' and no accessible extension method 'Title' accepting a first argument of type 'NamedNode' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the Title property returns the same value as Name.
//         /// </summary>
//         /// <param name="testName">The name to set.</param>
//         [Theory]
//         [InlineData("Sample Title")]
//         [InlineData("")]
//         public void Title_WhenNameIsSet_ReturnsSameValueAsName(string testName)
//         {
//             // Arrange
//             _namedNode.Name = testName;
// 
//             // Act
//             string title = _namedNode.Title;
// 
//             // Assert
//             title.Should().Be(testName, "Title property should mirror the Name property.");
//         }
// 
        /// <summary>
        /// Tests that the ToString method returns the same value as Name.
        /// </summary>
        /// <param name="testName">The name to set.</param>
        [Theory]
        [InlineData("Example String")]
        [InlineData("Another Example")]
        public void ToString_WhenNameIsSet_ReturnsSameValueAsName(string testName)
        {
            // Arrange
            _namedNode.Name = testName;

            // Act
            string toStringResult = _namedNode.ToString();

            // Assert
            toStringResult.Should().Be(testName, "ToString should return the Name property value.");
        }
    }
}
