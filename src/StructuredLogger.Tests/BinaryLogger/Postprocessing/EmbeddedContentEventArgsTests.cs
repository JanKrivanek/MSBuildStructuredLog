using System.IO;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="EmbeddedContentEventArgs"/> class.
    /// </summary>
    public class EmbeddedContentEventArgsTests
    {
        /// <summary>
        /// Tests that the constructor assigns properties correctly when valid parameters are provided.
        /// </summary>
        [Fact]
        public void Constructor_WithValidParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            // A valid BinaryLogRecordKind value, using cast as a dummy value.
            var expectedContentKind = (BinaryLogRecordKind)1;
            using var expectedStream = new MemoryStream();

            // Act
            var eventArgs = new EmbeddedContentEventArgs(expectedContentKind, expectedStream);

            // Assert
            Assert.Equal(expectedContentKind, eventArgs.ContentKind);
            Assert.Equal(expectedStream, eventArgs.ContentStream);
        }

        /// <summary>
        /// Tests that the constructor assigns properties correctly even when a null stream is provided.
        /// </summary>
        [Fact]
        public void Constructor_WithNullContentStream_SetsPropertiesCorrectly()
        {
            // Arrange
            var expectedContentKind = (BinaryLogRecordKind)2;
            Stream expectedStream = null;

            // Act
            var eventArgs = new EmbeddedContentEventArgs(expectedContentKind, expectedStream);

            // Assert
            Assert.Equal(expectedContentKind, eventArgs.ContentKind);
            Assert.Null(eventArgs.ContentStream);
        }

        /// <summary>
        /// Tests that the constructor properly assigns an invalid enum value for the content kind.
        /// </summary>
        [Fact]
        public void Constructor_WithInvalidEnumValue_SetsPropertiesCorrectly()
        {
            // Arrange
            var invalidContentKind = (BinaryLogRecordKind)(-1);
            using var expectedStream = new MemoryStream();

            // Act
            var eventArgs = new EmbeddedContentEventArgs(invalidContentKind, expectedStream);

            // Assert
            Assert.Equal(invalidContentKind, eventArgs.ContentKind);
            Assert.Equal(expectedStream, eventArgs.ContentStream);
        }
    }
}
