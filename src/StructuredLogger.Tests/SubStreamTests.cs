using System;
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
        private readonly byte[] _testData = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        /// <summary>
        /// Tests that the constructor throws an InvalidOperationException
        /// when provided with a stream that is not readable.
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
        /// Tests that the constructor correctly initializes the SubStream with a valid stream.
        /// </summary>
        [Fact]
        public void Constructor_ValidStream_InitializesProperties()
        {
            // Arrange
            using var memoryStream = new MemoryStream(_testData);
            long subLength = 5;

            // Act
            var subStream = new SubStream(memoryStream, subLength);

            // Assert
            Assert.Equal(subLength, subStream.Length);
            // Position should be initially 0.
            Assert.Equal(0, subStream.Position);
        }

        /// <summary>
        /// Tests that the IsAtEnd property returns false when the position is less than the specified length.
        /// </summary>
        [Fact]
        public void IsAtEnd_ReturnsFalse_WhenPositionLessThanLength()
        {
            // Arrange
            using var memoryStream = new MemoryStream(_testData);
            long subLength = 5;
            var subStream = new SubStream(memoryStream, subLength);

            // Act & Assert
            Assert.False(subStream.IsAtEnd);
        }

        /// <summary>
        /// Tests that the Read method reads only up to the bounded length.
        /// </summary>
        [Fact]
        public void Read_ReadsUpToBound_ReturnsCorrectData()
        {
            // Arrange
            using var memoryStream = new MemoryStream(_testData);
            int bound = 4;
            var subStream = new SubStream(memoryStream, bound);
            byte[] buffer = new byte[10];

            // Act
            int bytesRead = subStream.Read(buffer, 0, buffer.Length);

            // Assert
            Assert.Equal(bound, bytesRead);
            for (int i = 0; i < bound; i++)
            {
                Assert.Equal(_testData[i], buffer[i]);
            }
            // After reading exactly the bound, IsAtEnd should be true.
            Assert.True(subStream.IsAtEnd);
        }

        /// <summary>
        /// Tests that the ReadByte method returns the correct byte and updates the position.
        /// Also verifies that it returns -1 when the end of the bounded stream is reached.
        /// </summary>
        [Fact]
        public void ReadByte_ReturnsByteAndUpdatesPosition_AndReturnsMinusOneAtEnd()
        {
            // Arrange
            byte[] data = new byte[] { 10, 20 };
            using var memoryStream = new MemoryStream(data);
            var subStream = new SubStream(memoryStream, data.Length);

            // Act & Assert
            int first = subStream.ReadByte();
            Assert.Equal(10, first);
            Assert.Equal(1, subStream.Position);

            int second = subStream.ReadByte();
            Assert.Equal(20, second);
            Assert.Equal(2, subStream.Position);

            int third = subStream.ReadByte();
            Assert.Equal(-1, third);
            Assert.Equal(2, subStream.Position);
        }

        /// <summary>
        /// Tests that the ReadAsync method reads only up to the bounded length asynchronously.
        /// </summary>
        [Fact]
        public async Task ReadAsync_ReadsUpToBound_ReturnsCorrectData()
        {
            // Arrange
            using var memoryStream = new MemoryStream(_testData);
            int bound = 6;
            var subStream = new SubStream(memoryStream, bound);
            byte[] buffer = new byte[10];

            // Act
            int bytesRead = await subStream.ReadAsync(buffer, 0, buffer.Length, CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.Equal(bound, bytesRead);
            for (int i = 0; i < bound; i++)
            {
                Assert.Equal(_testData[i], buffer[i]);
            }
            Assert.True(subStream.IsAtEnd);
        }

        /// <summary>
        /// Tests that the Flush method calls the underlying stream's Flush method.
        /// </summary>
        [Fact]
        public void Flush_CallsUnderlyingStreamFlush()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.Setup(s => s.CanRead).Returns(true);
            // Setup Flush as virtual
            mockStream.Setup(s => s.Flush());

            var subStream = new SubStream(mockStream.Object, 10);

            // Act
            subStream.Flush();

            // Assert
            mockStream.Verify(s => s.Flush(), Times.Once);
        }

        /// <summary>
        /// Tests that the FlushAsync method calls the underlying stream's FlushAsync method.
        /// </summary>
        [Fact]
        public async Task FlushAsync_CallsUnderlyingStreamFlushAsync()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.Setup(s => s.CanRead).Returns(true);
            mockStream.Setup(s => s.FlushAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var subStream = new SubStream(mockStream.Object, 10);

            // Act
            await subStream.FlushAsync(CancellationToken.None).ConfigureAwait(false);

            // Assert
            mockStream.Verify(s => s.FlushAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Tests that setting the Position property throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void PositionSetter_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(_testData);
            var subStream = new SubStream(memoryStream, 10);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => subStream.Position = 5);
        }

        /// <summary>
        /// Tests that the Seek method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Seek_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(_testData);
            var subStream = new SubStream(memoryStream, 10);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => subStream.Seek(0, SeekOrigin.Begin));
        }

        /// <summary>
        /// Tests that the SetLength method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void SetLength_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(_testData);
            var subStream = new SubStream(memoryStream, 10);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => subStream.SetLength(20));
        }

        /// <summary>
        /// Tests that the Write method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Write_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(_testData);
            var subStream = new SubStream(memoryStream, 10);
            byte[] buffer = new byte[5];

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => subStream.Write(buffer, 0, buffer.Length));
        }

#if NET
        /// <summary>
        /// Tests that the Read(Span<byte>) method reads data up to the bounded length.
        /// </summary>
        [Fact]
        public void Read_Span_ReadsUpToBound_ReturnsCorrectData()
        {
            // Arrange
            using var memoryStream = new MemoryStream(_testData);
            int bound = 7;
            var subStream = new SubStream(memoryStream, bound);
            Span<byte> buffer = new byte[10];

            // Act
            int bytesRead = subStream.Read(buffer);

            // Assert
            Assert.Equal(bound, bytesRead);
            for (int i = 0; i < bound; i++)
            {
                Assert.Equal(_testData[i], buffer[i]);
            }
            Assert.True(subStream.IsAtEnd);
        }

        /// <summary>
        /// Tests that the ReadAsync(Memory<byte>, CancellationToken) method reads data up to the bounded length asynchronously.
        /// </summary>
        [Fact]
        public async Task ReadAsync_Memory_ReadsUpToBound_ReturnsCorrectData()
        {
            // Arrange
            using var memoryStream = new MemoryStream(_testData);
            int bound = 8;
            var subStream = new SubStream(memoryStream, bound);
            Memory<byte> buffer = new byte[10];

            // Act
            int bytesRead = await subStream.ReadAsync(buffer, CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.Equal(bound, bytesRead);
            for (int i = 0; i < bound; i++)
            {
                Assert.Equal(_testData[i], buffer.Span[i]);
            }
            Assert.True(subStream.IsAtEnd);
        }
#endif
    }
}
