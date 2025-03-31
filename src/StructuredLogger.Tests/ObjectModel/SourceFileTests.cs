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
        /// Tests that the TypeName getter returns the expected value "SourceFile".
        /// </summary>
        [Fact]
        public void TypeName_Get_ReturnsSourceFile()
        {
            // Act
            string result = _sourceFile.TypeName;

            // Assert
            result.Should().Be("SourceFile", "because the TypeName property should return the name of the class");
        }

        /// <summary>
        /// Tests that the SourceFilePath property can be set and get properly with a valid file path.
        /// </summary>
        /// <param name="path">A valid file path string.</param>
        [Theory]
        [InlineData("C:\\Projects\\Example.cs")]
        [InlineData("example.cs")]
        [InlineData("")]
        public void SourceFilePath_SetAndGet_ReturnsSameValue(string path)
        {
            // Arrange
            _sourceFile.SourceFilePath = path;

            // Act
            string actualPath = _sourceFile.SourceFilePath;

            // Assert
            actualPath.Should().Be(path, "because the SourceFilePath property should return the value that was assigned");
        }
    }
}
