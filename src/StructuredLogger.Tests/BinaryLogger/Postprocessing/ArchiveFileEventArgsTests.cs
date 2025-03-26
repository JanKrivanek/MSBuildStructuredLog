using Moq;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ArchiveFileEventArgs"/> class.
    /// </summary>
    public class ArchiveFileEventArgsTests
    {
        /// <summary>
        /// Tests that the constructor assigns the ArchiveFile property correctly when a non-null ArchiveFile is provided.
        /// </summary>
        [Fact]
        public void Constructor_WhenArchiveFileIsNonNull_AssignsValue()
        {
            // Arrange
            var dummyArchiveFile = new Mock<ArchiveFile>().Object;
            
            // Act
            var eventArgs = new ArchiveFileEventArgs(dummyArchiveFile);
            
            // Assert
            Assert.Same(dummyArchiveFile, eventArgs.ArchiveFile);
        }

        /// <summary>
        /// Tests that the constructor assigns the ArchiveFile property correctly when a null ArchiveFile is provided.
        /// </summary>
        [Fact]
        public void Constructor_WhenArchiveFileIsNull_AssignsNullValue()
        {
            // Arrange
            ArchiveFile dummyArchiveFile = null;
            
            // Act
            var eventArgs = new ArchiveFileEventArgs(dummyArchiveFile);
            
            // Assert
            Assert.Null(eventArgs.ArchiveFile);
        }

        /// <summary>
        /// Tests that the ArchiveFile property setter updates the ArchiveFile property as expected.
        /// </summary>
        [Fact]
        public void ArchiveFile_Setter_UpdatesValue()
        {
            // Arrange
            var initialArchiveFile = new Mock<ArchiveFile>().Object;
            var newArchiveFile = new Mock<ArchiveFile>().Object;
            var eventArgs = new ArchiveFileEventArgs(initialArchiveFile);
            
            // Act
            eventArgs.ArchiveFile = newArchiveFile;
            
            // Assert
            Assert.Same(newArchiveFile, eventArgs.ArchiveFile);
        }
    }
}
