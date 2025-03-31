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
        private readonly Metadata _metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataTests"/> class.
        /// </summary>
        public MetadataTests()
        {
            _metadata = new Metadata();
        }

        /// <summary>
        /// Tests that the <see cref="Metadata.TypeName"/> getter returns the correct type name.
        /// Arrange: Instantiates the Metadata class.
        /// Act: Retrieves the value of the TypeName property.
        /// Assert: Verifies that the returned value matches "Metadata".
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsMetadata()
        {
            // Act
            string actualTypeName = _metadata.TypeName;

            // Assert
            actualTypeName.Should().Be("Metadata", "because the TypeName property returns the name of the class.");
        }
    }
}
