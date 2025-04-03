using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="SourceFile"/> class.
    /// </summary>
    public class SourceFileTests
    {
        private readonly SourceFile _sourceFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceFileTests"/> class.
        /// </summary>
        public SourceFileTests()
        {
            _sourceFile = new SourceFile();
        }

        /// <summary>
        /// Tests that the <see cref="SourceFile.TypeName"/> property returns the expected type name.
        /// </summary>
        [Fact]
        public void TypeName_WhenAccessed_ReturnsSourceFile()
        {
            // Act
            string typeName = _sourceFile.TypeName;

            // Assert
            typeName.Should().Be(nameof(SourceFile));
        }

        /// <summary>
        /// Tests that the <see cref="SourceFile.SourceFilePath"/> property can be set and retrieved correctly.
        /// </summary>
        /// <param name="path">The file path to set.</param>
        [Theory]
        [InlineData("C:\\Project\\file.cs")]                  // Windows absolute path
        [InlineData("/usr/local/project/file.cs")]            // Unix-style path
        [InlineData("relative/path/file.cs")]                 // Relative path
        [InlineData("")]                                      // Empty string edge case
        [InlineData("短路径")]                                // Short string with special characters
        [InlineData("ThisIsAReallyLongFilePath_XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")] // Excessively long string
        public void SourceFilePath_SetAndGet_ShouldReturnSameValue(string path)
        {
            // Arrange & Act
            _sourceFile.SourceFilePath = path;
            string actualPath = _sourceFile.SourceFilePath;

            // Assert
            actualPath.Should().Be(path);
        }
    }
}
