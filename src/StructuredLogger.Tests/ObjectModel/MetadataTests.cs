using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Metadata"/> class.
    /// </summary>
    public class MetadataTests
    {
        /// <summary>
        /// Tests the <see cref="Metadata.TypeName"/> property to ensure it returns the expected type name.
        /// The test instantiates a <see cref="Metadata"/> object, accesses the TypeName property,
        /// and asserts that it equals the string "Metadata".
        /// </summary>
        [Fact]
        public void TypeName_WhenAccessed_ReturnsCorrectTypeName()
        {
            // Arrange
            var metadata = new Metadata();

            // Act
            string result = metadata.TypeName;

            // Assert
            result.Should().Be("Metadata");
        }
    }
}
