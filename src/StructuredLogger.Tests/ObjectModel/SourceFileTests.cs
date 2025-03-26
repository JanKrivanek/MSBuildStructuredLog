using System;
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
        /// Tests that setting the <see cref="SourceFile.SourceFilePath"/> property with various inputs returns the expected value.
        /// This test covers setting valid file paths, an empty string, and a null value.
        /// Expected outcome is that the property getter returns exactly what was set.
        /// </summary>
        /// <param name="testPath">The file path value to set.</param>
        [Theory]
        [InlineData("C:\\Test\\File.cs")]
        [InlineData("")]
        [InlineData(null)]
        public void SourceFilePath_WhenSet_ReturnsSameValue(string testPath)
        {
            // Arrange
            _sourceFile.SourceFilePath = testPath;

            // Act
            string? result = _sourceFile.SourceFilePath;

            // Assert
            Assert.Equal(testPath, result);
        }

        /// <summary>
        /// Tests that the <see cref="SourceFile.TypeName"/> property correctly returns the name "SourceFile".
        /// This property is an override that should always return the hard-coded type name.
        /// </summary>
        [Fact]
        public void TypeName_WhenAccessed_ReturnsSourceFile()
        {
            // Act
            string result = _sourceFile.TypeName;

            // Assert
            Assert.Equal("SourceFile", result);
        }
    }
}
