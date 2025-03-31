using DotUtils.StreamUtils;
using FluentAssertions;
using Moq;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ChunkedBufferStream"/> class.
    /// </summary>
    public class ChunkedBufferStreamTests
    {
        private const int BufferSize = 4;

        /// <summary>
        /// Tests that Flush writes the pending buffered content to the underlying stream.
        /// </summary>
        [Fact]
        public void Flush_PendingData_WritesToUnderlyingStream()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);
            byte[] testData = { 1, 2 }; // less than BufferSize
            chunkedStream.Write(testData, 0, testData.Length);

            // Act
            chunkedStream.Flush();

            // Assert
            underlyingStream.ToArray().Should().BeEquivalentTo(testData);
        }

        /// <summary>
        /// Tests that Write writes data correctly and flushes automatically when buffer fills exactly.
        /// </summary>
        [Fact]
        public void Write_ExactBufferSizeData_FlushesAutomatically()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);
            byte[] testData = { 10, 20, 30, 40 }; // exactly BufferSize

            // Act
            chunkedStream.Write(testData, 0, testData.Length);

            // Assert
            // Since buffer filled, flush is called automatically so underlying stream should contain testData.
            underlyingStream.ToArray().Should().BeEquivalentTo(testData);
        }

        /// <summary>
        /// Tests that Write writes data in multiple cycles if input exceeds buffer capacity.
        /// </summary>
        [Fact]
        public void Write_MultipleCyclesData_WritesAllDataCorrectly()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);
            // Data length is not multiple of BufferSize.
            byte[] testData = { 1, 2, 3, 4, 5, 6, 7 };

            // Act
            chunkedStream.Write(testData, 0, testData.Length);
            // flush remaining partial data
            chunkedStream.Flush();

            // Assert
            underlyingStream.ToArray().Should().BeEquivalentTo(testData);
        }

        /// <summary>
        /// Tests that WriteByte writes a single byte correctly and flushes if the buffer becomes full.
        /// </summary>
        [Fact]
        public void WriteByte_BufferEdge_WritesAndFlushesAutomatically()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);

            // Fill the buffer with BufferSize - 1 bytes first.
            for (int i = 0; i < BufferSize - 1; i++)
            {
                chunkedStream.WriteByte((byte)(i + 1));
            }

            // Act
            // Writing one more byte should trigger Flush automatically.
            chunkedStream.WriteByte(99);

            // Assert
            underlyingStream.ToArray().Should().BeEquivalentTo(new byte[] { 1, 2, 3, 99 });
        }

        /// <summary>
        /// Tests that WriteAsync writes data correctly and flushes automatically when buffer fills.
        /// </summary>
        [Fact]
        public async Task WriteAsync_ExactBufferSizeData_FlushesAutomatically()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);
            byte[] testData = { 50, 60, 70, 80 };

            // Act
            await chunkedStream.WriteAsync(testData, 0, testData.Length, CancellationToken.None).ConfigureAwait(false);

            // Assert
            // Since buffer reached capacity, flush is triggered so underlying stream should be updated.
            underlyingStream.ToArray().Should().BeEquivalentTo(testData);
        }

        /// <summary>
        /// Tests that WriteAsync writes data in multiple cycles when input exceeds buffer capacity.
        /// </summary>
        [Fact]
        public async Task WriteAsync_MultipleCyclesData_WritesAllDataCorrectly()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);
            byte[] testData = { 9, 8, 7, 6, 5 }; // 5 > BufferSize -> one auto flush and one manual flush after writing.

            // Act
            await chunkedStream.WriteAsync(testData, 0, testData.Length, CancellationToken.None).ConfigureAwait(false);
            chunkedStream.Flush();

            // Assert
            underlyingStream.ToArray().Should().BeEquivalentTo(testData);
        }

#if NET
        /// <summary>
        /// Tests that Write(ReadOnlySpan) writes data correctly and flushes automatically when buffer is full.
        /// </summary>
        [Fact]
        public void Write_ReadOnlySpanData_WritesAndFlushesAutomatically()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);
            ReadOnlySpan<byte> testData = new byte[] { 11, 22, 33, 44 };

            // Act
            chunkedStream.Write(testData);

            // Assert
            underlyingStream.ToArray().Should().BeEquivalentTo(testData.ToArray());
        }

        /// <summary>
        /// Tests that WriteAsync(ReadOnlyMemory) writes data correctly in multiple cycles.
        /// </summary>
        [Fact]
        public async Task WriteAsync_ReadOnlyMemoryData_MultipleCycles_WritesAllDataCorrectly()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);
            ReadOnlyMemory<byte> testData = new byte[] { 100, 101, 102, 103, 104 };

            // Act
            await chunkedStream.WriteAsync(testData, CancellationToken.None).ConfigureAwait(false);
            chunkedStream.Flush();

            // Assert
            underlyingStream.ToArray().Should().BeEquivalentTo(testData.ToArray());
        }
#endif

        /// <summary>
        /// Tests that Read method throws NotSupportedException.
        /// </summary>
        [Fact]
        public void Read_AnyInput_ThrowsNotSupportedException()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);
            byte[] buffer = new byte[10];

            // Act
            Action act = () => chunkedStream.Read(buffer, 0, buffer.Length);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("GreedyBufferedStream is write-only, append-only");
        }

        /// <summary>
        /// Tests that Seek method throws NotSupportedException.
        /// </summary>
        [Fact]
        public void Seek_AnyInput_ThrowsNotSupportedException()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);

            // Act
            Action act = () => chunkedStream.Seek(0, SeekOrigin.Begin);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("GreedyBufferedStream is write-only, append-only");
        }

        /// <summary>
        /// Tests that SetLength method throws NotSupportedException.
        /// </summary>
        [Fact]
        public void SetLength_AnyInput_ThrowsNotSupportedException()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);

            // Act
            Action act = () => chunkedStream.SetLength(100);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("GreedyBufferedStream is write-only, append-only");
        }

        /// <summary>
        /// Tests that setting the Position property throws NotSupportedException.
        /// </summary>
        [Fact]
        public void PositionSetter_AnyValue_ThrowsNotSupportedException()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);

            // Act
            Action act = () => chunkedStream.Position = 10;

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("GreedyBufferedStream is write-only, append-only");
        }

        /// <summary>
        /// Tests that the Position getter returns the sum of the underlying stream's position and the buffered offset.
        /// </summary>
        [Fact]
        public void PositionGetter_ReturnsUnderlyingPositionPlusBufferOffset()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);
            // Write 2 bytes but do not flush.
            byte[] data = { 1, 2 };
            chunkedStream.Write(data, 0, data.Length);

            // Underlying stream position remains zero because flush hasn't been called.
            long expectedPosition = underlyingStream.Position + data.Length; // 0 + 2

            // Act
            long actualPosition = chunkedStream.Position;

            // Assert
            actualPosition.Should().Be(expectedPosition);
        }

        /// <summary>
        /// Tests that the Length property returns the underlying stream's length plus the buffered offset.
        /// </summary>
        [Fact]
        public void Length_ReturnsUnderlyingLengthPlusBufferOffset()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);
            // Write 3 bytes but do not flush.
            byte[] data = { 5, 6, 7 };
            chunkedStream.Write(data, 0, data.Length);

            // Underlying stream length is zero because flush hasn't been called.
            long expectedLength = underlyingStream.Length + data.Length; // 0 + 3

            // Act
            long actualLength = chunkedStream.Length;

            // Assert
            actualLength.Should().Be(expectedLength);
        }

        /// <summary>
        /// Tests that the capability properties return the expected values.
        /// </summary>
        [Fact]
        public void CapabilityProperties_ReturnExpectedValues()
        {
            // Arrange
            var underlyingStream = new MemoryStream();
            using var chunkedStream = new ChunkedBufferStream(underlyingStream, BufferSize);

            // Act & Assert
            chunkedStream.CanRead.Should().BeFalse();
            chunkedStream.CanSeek.Should().BeFalse();
            chunkedStream.CanWrite.Should().Be(underlyingStream.CanWrite);
        }

        /// <summary>
        /// Tests that Close flushes the remaining buffer data and closes the underlying stream.
        /// </summary>
        [Fact]
        public void Close_CallsFlushAndClosesUnderlyingStream()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            // Setup Write to simply record calls.
            mockStream.Setup(s => s.Write(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()));
            // Setup Close to be verifiable.
            mockStream.Setup(s => s.Close()).Verifiable();
            mockStream.SetupGet(s => s.CanWrite).Returns(true);
            // Use a small buffer so that flush will be called.
            using (var chunkedStream = new ChunkedBufferStream(mockStream.Object, BufferSize))
            {
                byte[] data = { 1, 2 };
                chunkedStream.Write(data, 0, data.Length);
                // Act
                chunkedStream.Close();
            }

            // Assert
            mockStream.Verify(s => s.Write(It.IsAny<byte[]>(), 0, 2), Times.Once);
            mockStream.Verify(s => s.Close(), Times.Once);
        }
    }
}
