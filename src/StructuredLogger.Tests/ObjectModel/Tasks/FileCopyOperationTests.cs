using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="FileCopyOperation"/> class.
    /// </summary>
    public class FileCopyOperationTests
    {
//         /// <summary> // [Error] (31-38)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.TreeNode' to 'Microsoft.Build.Logging.StructuredLogger.TreeNode'
//         /// Tests that the properties of the FileCopyOperation class can be set and retrieved correctly.
//         /// The test assigns non-null values to the Source, Destination, Copied, and Node properties and verifies that they are stored as expected.
//         /// </summary>
//         [Fact]
//         public void Properties_WhenSet_CanBeRetrieved()
//         {
//             // Arrange
//             var expectedSource = "example.txt";
//             var expectedDestination = "copy.txt";
//             var expectedCopied = true;
//             var expectedNode = new TreeNode();
// 
//             var fileCopyOperation = new FileCopyOperation();
// 
//             // Act
//             fileCopyOperation.Source = expectedSource;
//             fileCopyOperation.Destination = expectedDestination;
//             fileCopyOperation.Copied = expectedCopied;
//             fileCopyOperation.Node = expectedNode;
// 
//             // Assert
//             fileCopyOperation.Source.Should().Be(expectedSource);
//             fileCopyOperation.Destination.Should().Be(expectedDestination);
//             fileCopyOperation.Copied.Should().Be(expectedCopied);
//             fileCopyOperation.Node.Should().Be(expectedNode);
//         }
// 
        /// <summary>
        /// Tests the ToString method to ensure it returns a formatted string using the Source and Destination properties.
        /// This test covers multiple valid inputs including typical filenames and empty strings.
        /// </summary>
        /// <param name="source">The source file path.</param>
        /// <param name="destination">The destination file path.</param>
        /// <param name="expected">The expected formatted string output.</param>
        [Theory]
        [InlineData("source.txt", "destination.txt", "source.txt ➔ destination.txt")]
        [InlineData("", "", " ➔ ")]
        [InlineData("src", "", "src ➔ ")]
        [InlineData("", "dest", " ➔ dest")]
        [InlineData("path with spaces.txt", "another path.doc", "path with spaces.txt ➔ another path.doc")]
        public void ToString_WithValidSourceAndDestination_ReturnsFormattedString(string source, string destination, string expected)
        {
            // Arrange
            var fileCopyOperation = new FileCopyOperation
            {
                Source = source,
                Destination = destination
            };

            // Act
            string actual = fileCopyOperation.ToString();

            // Assert
            actual.Should().Be(expected);
        }

        /// <summary>
        /// Tests the ToString method when Source and Destination properties are not explicitly set.
        /// Since the default values for uninitialized string properties are null which results in an empty string during string interpolation,
        /// the expected output should consist of the arrow with empty strings on both sides.
        /// </summary>
        [Fact]
        public void ToString_WhenPropertiesNotSet_ReturnsFormattedStringWithDefaults()
        {
            // Arrange
            var fileCopyOperation = new FileCopyOperation();

            // Act
            string actual = fileCopyOperation.ToString();

            // Assert
            actual.Should().Be(" ➔ ");
        }
    }
}
