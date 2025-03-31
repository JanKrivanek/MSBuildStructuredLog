using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.IO;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="EmbeddedContentEventArgs"/> class.
    /// </summary>
    public class EmbeddedContentEventArgsTests
    {
        private readonly MemoryStream _dummyStream;
        private readonly BinaryLogRecordKind _dummyContentKind;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedContentEventArgsTests"/> class with common test data.
        /// </summary>
        public EmbeddedContentEventArgsTests()
        {
            _dummyStream = new MemoryStream();
            _dummyContentKind = default(BinaryLogRecordKind);
        }

        /// <summary>
        /// Tests the constructor to ensure that properties are correctly assigned when valid parameters are provided.
        /// Expected outcome: The ContentKind and ContentStream properties should match the provided values.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalledWithValidParameters_ShouldSetProperties()
        {
            // Arrange
            BinaryLogRecordKind expectedContentKind = _dummyContentKind;
            Stream expectedStream = _dummyStream;

            // Act
            var eventArgs = new EmbeddedContentEventArgs(expectedContentKind, expectedStream);

            // Assert
            eventArgs.ContentKind.Should().Be(expectedContentKind);
            eventArgs.ContentStream.Should().BeSameAs(expectedStream);
        }

        /// <summary>
        /// Tests the constructor behavior when a null stream is provided.
        /// Expected outcome: The ContentStream property should be null and ContentKind should be set to the provided value.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalledWithNullStream_ShouldSetContentStreamToNull()
        {
            // Arrange
            BinaryLogRecordKind expectedContentKind = _dummyContentKind;
            Stream expectedStream = null;

            // Act
            var eventArgs = new EmbeddedContentEventArgs(expectedContentKind, expectedStream);

            // Assert
            eventArgs.ContentKind.Should().Be(expectedContentKind);
            eventArgs.ContentStream.Should().BeNull();
        }
    }
}
