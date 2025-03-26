using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotUtils.StreamUtils;
using Moq;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="SubStream"/> class.
    /// </summary>
    public class SubStreamTests
    {
        private readonly byte[] _sampleData = new byte[] { 1, 2, 3, 4, 5 };

        /// <summary>
        /// Tests that constructing a SubStream with a non-readable stream throws an InvalidOperationException.
        /// </summary>
        [Fact]
        public void Constructor_NonReadableStream_ThrowsInvalidOperationException()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.Setup(s => s.CanRead).Returns(false);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => new SubStream(mockStream.Object, 10));
        }

        /// <summary>
        /// Tests that the CanRead property returns true.
        /// </summary>
        [Fact]
        public void CanRead_ReturnsTrue()
        {
            // Arrange
            using var ms = new MemoryStream();
            var subStream = new SubStream(ms, 100);

            // Act & Assert
            Assert.True(subStream.CanRead);
        }

        /// <summary>
        /// Tests that the CanSeek property returns false.
        /// </summary>
        [Fact]
        public void CanSeek_ReturnsFalse()
        {
            // Arrange
            using var ms = new MemoryStream();
            var subStream = new SubStream(ms, 100);

            // Act & Assert
            Assert.False(subStream.CanSeek);
        }

        /// <summary>
        /// Tests that the CanWrite property returns false.
        /// </summary>
        [Fact]
        public void CanWrite_ReturnsFalse()
        {
            // Arrange
            using var ms = new MemoryStream();
            var subStream = new SubStream(ms, 100);

            // Act & Assert
            Assert.False(subStream.CanWrite);
        }

        /// <summary>
        /// Tests that the Length property returns the length provided in the constructor.
        /// </summary>
        [Fact]
        public void Length_ReturnsProvidedLength()
        {
            // Arrange
            const long expectedLength = 50;
            using var ms = new MemoryStream();
            var subStream = new SubStream(ms, expectedLength);

            // Act & Assert
            Assert.Equal(expectedLength, subStream.Length);
        }

        /// <summary>
        /// Tests that the Position getter returns 0 initially.
        /// </summary>
        [Fact]
        public void PositionGetter_ReturnsZeroInitially()
        {
            // Arrange
            using var ms = new MemoryStream();
            var subStream = new SubStream(ms, 100);

            // Act & Assert
            Assert.Equal(0, subStream.Position);
        }

        /// <summary>
        /// Tests that the Position setter throws NotImplementedException.
        /// </summary>
        [Fact]
        public void PositionSetter_ThrowsNotImplementedException()
        {
            // Arrange
            using var ms = new MemoryStream();
            var subStream = new SubStream(ms, 100);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => subStream.Position = 10);
        }

        /// <summary>
        /// Tests that Flush calls the underlying stream's Flush method.
        /// </summary>
        [Fact]
        public void Flush_CallsUnderlyingStreamFlush()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.Setup(s => s.CanRead).Returns(true);
            var subStream = new SubStream(mockStream.Object, 100);

            // Act
            subStream.Flush();

            // Assert
            mockStream.Verify(s => s.Flush(), Times.Once);
        }

        /// <summary>
        /// Tests that FlushAsync calls the underlying stream's FlushAsync method.
        /// </summary>
        [Fact]
        public async Task FlushAsync_CallsUnderlyingStreamFlushAsync()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.Setup(s => s.CanRead).Returns(true);
            mockStream.Setup(s => s.FlushAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var subStream = new SubStream(mockStream.Object, 100);
            var cancellationToken = new CancellationToken();

            // Act
            await subStream.FlushAsync(cancellationToken);

            // Assert
            mockStream.Verify(s => s.FlushAsync(cancellationToken), Times.Once);
        }

        /// <summary>
        /// Tests that the Read method reads only up to the specified boundary and updates the internal position.
        /// </summary>
        [Fact]
        public void Read_ReadsUpToBoundaryAndUpdatesPosition()
        {
            // Arrange
            byte[] data = _sampleData; // 5 bytes: 1,2,3,4,5
            using var ms = new MemoryStream(data);
            // Limit substream to 3 bytes.
            var subStream = new SubStream(ms, 3);
            var buffer = new byte[10];

            // Act
            int bytesRead = subStream.Read(buffer, 0, buffer.Length);

            // Assert
            Assert.Equal(3, bytesRead);
            Assert.Equal(1, buffer[0]);
            Assert.Equal(2, buffer[1]);
            Assert.Equal(3, buffer[2]);
            Assert.True(subStream.IsAtEnd);
        }

        /// <summary>
        /// Tests that the ReadByte method reads a single byte and updates the internal position accordingly.
        /// </summary>
        [Fact]
        public void ReadByte_ReturnsByteAndUpdatesPosition()
        {
            // Arrange
            byte[] data = _sampleData;
            using var ms = new MemoryStream(data);
            var subStream = new SubStream(ms, data.Length);

            // Act
            int firstByte = subStream.ReadByte();
            int secondByte = subStream.ReadByte();

            // Assert
            Assert.Equal(1, firstByte);
            Assert.Equal(2, secondByte);
        }

        /// <summary>
        /// Tests that the ReadByte method returns -1 when the substream has reached its end.
        /// </summary>
        [Fact]
        public void ReadByte_WhenAtEnd_ReturnsMinusOne()
        {
            // Arrange
            byte[] data = new byte[] { 9 };
            using var ms = new MemoryStream(data);
            var subStream = new SubStream(ms, 1);

            // Act
            int byteRead = subStream.ReadByte();
            int byteAfterEnd = subStream.ReadByte();

            // Assert
            Assert.Equal(9, byteRead);
            Assert.Equal(-1, byteAfterEnd);
        }

        /// <summary>
        /// Tests that the ReadAsync method reads only up to the specified boundary and correctly updates the internal position.
        /// </summary>
        [Fact]
        public async Task ReadAsync_ReadsUpToBoundaryAndUpdatesPosition()
        {
            // Arrange
            byte[] data = _sampleData; // 5 bytes: 1,2,3,4,5
            using var ms = new MemoryStream(data);
            // Limit substream to 4 bytes.
            var subStream = new SubStream(ms, 4);
            var buffer = new byte[10];
            var cancellationToken = new CancellationToken();

            // Act
            int bytesRead = await subStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

            // Assert
            Assert.Equal(4, bytesRead);
            Assert.Equal(1, buffer[0]);
            Assert.Equal(2, buffer[1]);
            Assert.Equal(3, buffer[2]);
            Assert.Equal(4, buffer[3]);
            Assert.True(subStream.IsAtEnd);
        }

#if NET
        /// <summary>
        /// Tests that the Read(Span<byte>) method reads only up to the specified boundary and correctly updates the internal position.
        /// </summary>
        [Fact]
        public void Read_Span_ReadsUpToBoundaryAndUpdatesPosition()
        {
            // Arrange
            byte[] data = _sampleData;
            using var ms = new MemoryStream(data);
            // Limit substream to 2 bytes.
            var subStream = new SubStream(ms, 2);
            Span<byte> buffer = new byte[5];

            // Act
            int bytesRead = subStream.Read(buffer);

            // Assert
            Assert.Equal(2, bytesRead);
            Assert.Equal(1, buffer[0]);
            Assert.Equal(2, buffer[1]);
            Assert.True(subStream.IsAtEnd);
        }

        /// <summary>
        /// Tests that the ReadAsync(Memory<byte>) method reads only up to the specified boundary and correctly updates the internal position.
        /// </summary>
        [Fact]
        public async Task ReadAsync_Memory_ReadsUpToBoundaryAndUpdatesPosition()
        {
            // Arrange
            byte[] data = _sampleData;
            using var ms = new MemoryStream(data);
            // Limit substream to 3 bytes.
            var subStream = new SubStream(ms, 3);
            Memory<byte> buffer = new byte[10];
            var cancellationToken = new CancellationToken();

            // Act
            int bytesRead = await subStream.ReadAsync(buffer, cancellationToken);

            // Assert
            Assert.Equal(3, bytesRead);
            Assert.Equal(1, buffer.Span[0]);
            Assert.Equal(2, buffer.Span[1]);
            Assert.Equal(3, buffer.Span[2]);
            Assert.True(subStream.IsAtEnd);
        }
#endif

        /// <summary>
        /// Tests that calling Seek throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Seek_ThrowsNotImplementedException()
        {
            // Arrange
            using var ms = new MemoryStream();
            var subStream = new SubStream(ms, 100);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => subStream.Seek(0, SeekOrigin.Begin));
        }

        /// <summary>
        /// Tests that calling SetLength throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void SetLength_ThrowsNotImplementedException()
        {
            // Arrange
            using var ms = new MemoryStream();
            var subStream = new SubStream(ms, 100);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => subStream.SetLength(50));
        }

        /// <summary>
        /// Tests that calling Write throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Write_ThrowsNotImplementedException()
        {
            // Arrange
            using var ms = new MemoryStream();
            var subStream = new SubStream(ms, 100);
            byte[] buffer = new byte[10];

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => subStream.Write(buffer, 0, buffer.Length));
        }
    }
}
