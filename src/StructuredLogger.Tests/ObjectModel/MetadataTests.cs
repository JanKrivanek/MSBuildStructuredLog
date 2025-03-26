using Microsoft.Build.Logging.StructuredLogger;
using System;
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
        /// Initializes a new instance of the <see cref="MetadataTests"/> class and sets up the test instance.
        /// </summary>
        public MetadataTests()
        {
            // Arrange: Create an instance of Metadata.
            _metadata = new Metadata();
        }

        /// <summary>
        /// Tests that the TypeName property returns "Metadata" as expected.
        /// Functional steps:
        /// 1. Access the TypeName property of the Metadata instance.
        /// 2. Verify that the returned string equals "Metadata".
        /// Expected outcome: The TypeName property returns "Metadata".
        /// </summary>
//         [Fact] [Error] (34-41)CS1061 'Metadata' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Metadata' could be found (are you missing a using directive or an assembly reference?)
//         public void TypeName_WhenAccessed_ReturnsMetadataString()
//         {
//             // Act: Retrieve the type name from the property.
//             string typeName = _metadata.TypeName;
// 
//             // Assert: Validate the retrieved value matches the expected output.
//             Assert.Equal("Metadata", typeName);
//         }
    }
}
