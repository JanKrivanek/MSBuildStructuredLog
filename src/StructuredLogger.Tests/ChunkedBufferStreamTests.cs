using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotUtils.StreamUtils;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ChunkedBufferStream"/> class.
    /// </summary>
    public class ChunkedBufferStreamTests
    {
        private const int DefaultBufferSize = 4;

        #region Flush Tests

        /// <summary>
        /// Tests that Flush writes the current buffer content to the underlying stream.
        /// </summary>
        [Fact]
        public void Flush_WhenCalled_WritesBufferedDataToUnderlyingStream()
        {
            // Arrange
            byte[] inputData = { 1, 2 };
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);
            chunkedStream.Write(inputData, 0, inputData.Length);

            // Act
            chunkedStream.Flush();

            // Assert
            byte[] result = memoryStream.ToArray();
            Assert.Equal(inputData, result);
        }

        #endregion

        #region NotSupported Methods Tests

        /// <summary>
        /// Tests that calling Read always throws NotSupportedException.
        /// </summary>
        [Fact]
        public void Read_WhenCalled_ThrowsNotSupportedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);
            byte[] buffer = new byte[10];

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => chunkedStream.Read(buffer, 0, buffer.Length));
        }

        /// <summary>
        /// Tests that calling Seek always throws NotSupportedException.
        /// </summary>
        [Fact]
        public void Seek_WhenCalled_ThrowsNotSupportedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => chunkedStream.Seek(0, SeekOrigin.Begin));
        }

        /// <summary>
        /// Tests that calling SetLength always throws NotSupportedException.
        /// </summary>
        [Fact]
        public void SetLength_WhenCalled_ThrowsNotSupportedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => chunkedStream.SetLength(100));
        }

        #endregion

        #region Write(byte[], int, int) Tests

        /// <summary>
        /// Tests that Write writes data correctly to the buffer and triggers an automatic flush when the buffer is full.
        /// </summary>
        [Fact]
        public void Write_WithExactBufferMultiple_TriggersAutomaticFlush()
        {
            // Arrange
            byte[] inputData = { 10, 20, 30, 40, 50, 60 };
            using var memoryStream = new MemoryStream();
            // Buffer size is 4, so the first 4 bytes should flush automatically.
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);

            // Act
            chunkedStream.Write(inputData, 0, inputData.Length);

            // At this point, one flush should have occurred automatically for the first 4 bytes.
            byte[] intermediate = memoryStream.ToArray();
            // Assert intermediate flush: first 4 bytes flushed.
            Assert.Equal(new byte[] { 10, 20, 30, 40 }, intermediate);

            // Call Flush to flush remaining bytes.
            chunkedStream.Flush();
            byte[] finalResult = memoryStream.ToArray();

            // Expected: all 6 bytes
            Assert.Equal(new byte[] { 10, 20, 30, 40, 50, 60 }, finalResult);
        }

        /// <summary>
        /// Tests that Write does not automatically flush when the buffered data is less than the buffer capacity.
        /// </summary>
        [Fact]
        public void Write_WithPartialData_DoesNotFlushAutomatically()
        {
            // Arrange
            byte[] inputData = { 100, 101 };
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);

            // Act
            chunkedStream.Write(inputData, 0, inputData.Length);

            // Assert: underlying stream should still be empty
            Assert.Empty(memoryStream.ToArray());

            // After calling flush, underlying stream should contain the data.
            chunkedStream.Flush();
            Assert.Equal(inputData, memoryStream.ToArray());
        }

        /// <summary>
        /// Tests that Write with a non-zero offset and count writes the correct segment of the data.
        /// </summary>
        [Fact]
        public void Write_WithOffsetAndCount_WritesCorrectData()
        {
            // Arrange
            byte[] inputData = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int offset = 3;
            int count = 4; // expected segment: {3,4,5,6}
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);

            // Act
            chunkedStream.Write(inputData, offset, count);
            chunkedStream.Flush();

            // Assert
            byte[] expected = { 3, 4, 5, 6 };
            Assert.Equal(expected, memoryStream.ToArray());
        }

        #endregion

        #region WriteByte Tests

        /// <summary>
        /// Tests that WriteByte correctly writes a single byte and does not flush automatically when the buffer is not full.
        /// </summary>
        [Fact]
        public void WriteByte_WhenBufferNotFull_DoesNotFlushAutomatically()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            // Set buffer size to 3
            var chunkedStream = new ChunkedBufferStream(memoryStream, 3);

            // Act
            chunkedStream.WriteByte(99);

            // Assert: underlying stream should still be empty because buffer is not full.
            Assert.Empty(memoryStream.ToArray());

            // Flush and check content.
            chunkedStream.Flush();
            Assert.Equal(new byte[] { 99 }, memoryStream.ToArray());
        }

        /// <summary>
        /// Tests that WriteByte triggers an automatic flush when the buffer becomes full.
        /// </summary>
        [Fact]
        public void WriteByte_WhenBufferFull_TriggersAutomaticFlush()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            // Set buffer size to 1 so that every WriteByte call flushes when buffer is full.
            var chunkedStream = new ChunkedBufferStream(memoryStream, 1);

            // Act
            chunkedStream.WriteByte(111);
            // After writing one byte, the buffer should flush automatically.
            byte[] intermediate = memoryStream.ToArray();

            // Assert: first byte flushed
            Assert.Equal(new byte[] { 111 }, intermediate);

            // Write another byte and flush manually.
            chunkedStream.WriteByte(222);
            chunkedStream.Flush();
            byte[] finalResult = memoryStream.ToArray();

            // Expected overall stream: first flush plus second flush result.
            Assert.Equal(new byte[] { 111, 222 }, finalResult);
        }

        #endregion

        #region WriteAsync Tests

        /// <summary>
        /// Tests that WriteAsync writes data correctly and triggers automatic flushes when necessary.
        /// </summary>
        [Fact]
        public async Task WriteAsync_WithData_WritesAndFlushesAsExpected()
        {
            // Arrange
            byte[] inputData = { 5, 6, 7, 8, 9 };
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            await chunkedStream.WriteAsync(inputData, 0, inputData.Length, cancellationToken);

            // At this point, the first 4 bytes should have been flushed.
            byte[] intermediate = memoryStream.ToArray();
            Assert.Equal(new byte[] { 5, 6, 7, 8 }, intermediate);

            // Flush remaining
            await chunkedStream.FlushAsync(cancellationToken).ConfigureAwait(false);
            byte[] finalResult = memoryStream.ToArray();
            Assert.Equal(new byte[] { 5, 6, 7, 8, 9 }, finalResult);
        }

        #endregion

#if NET
        #region Write(ReadOnlySpan<byte>) Tests

        /// <summary>
        /// Tests that Write(ReadOnlySpan<byte>) writes data correctly to the buffer and flushes automatically when full.
        /// </summary>
        [Fact]
        public void Write_ReadOnlySpan_WritesDataCorrectly()
        {
            // Arrange
            byte[] inputData = { 15, 16, 17, 18, 19 };
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);

            // Act
            // This overload accepts ReadOnlySpan<byte>
            chunkedStream.Write(inputData);

            // After writing, the first 4 bytes should have flushed automatically.
            byte[] intermediate = memoryStream.ToArray();
            Assert.Equal(new byte[] { 15, 16, 17, 18 }, intermediate);

            // Flush remaining buffered byte.
            chunkedStream.Flush();
            byte[] finalResult = memoryStream.ToArray();
            Assert.Equal(new byte[] { 15, 16, 17, 18, 19 }, finalResult);
        }

        #endregion

        #region WriteAsync(ReadOnlyMemory<byte>, CancellationToken) Tests

        /// <summary>
        /// Tests that WriteAsync(ReadOnlyMemory<byte>, CancellationToken) writes data correctly and flushes automatically when the buffer is full.
        /// </summary>
        [Fact]
        public async Task WriteAsync_ReadOnlyMemory_WritesDataCorrectly()
        {
            // Arrange
            byte[] inputData = { 25, 26, 27, 28, 29 };
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            await chunkedStream.WriteAsync((ReadOnlyMemory<byte>)inputData, cancellationToken).ConfigureAwait(false);

            // The first 4 bytes should have flushed automatically.
            byte[] intermediate = memoryStream.ToArray();
            Assert.Equal(new byte[] { 25, 26, 27, 28 }, intermediate);

            // Flush remaining buffered data.
            await chunkedStream.FlushAsync(cancellationToken).ConfigureAwait(false);
            byte[] finalResult = memoryStream.ToArray();
            Assert.Equal(new byte[] { 25, 26, 27, 28, 29 }, finalResult);
        }

        #endregion
#endif

        #region Properties Tests

        /// <summary>
        /// Tests that the CanRead property always returns false.
        /// </summary>
        [Fact]
        public void CanReadProperty_ReturnsFalse()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);

            // Act & Assert
            Assert.False(chunkedStream.CanRead);
        }

        /// <summary>
        /// Tests that the CanSeek property always returns false.
        /// </summary>
        [Fact]
        public void CanSeekProperty_ReturnsFalse()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);

            // Act & Assert
            Assert.False(chunkedStream.CanSeek);
        }

        /// <summary>
        /// Tests that the CanWrite property returns the underlying stream's CanWrite value.
        /// </summary>
        [Fact]
        public void CanWriteProperty_ReturnsUnderlyingStreamCapability()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);

            // Act & Assert
            Assert.Equal(memoryStream.CanWrite, chunkedStream.CanWrite);
        }

        /// <summary>
        /// Tests that the Length property returns the sum of the underlying stream's length and buffered data length.
        /// </summary>
        [Fact]
        public void LengthProperty_ReturnsUnderlyingLengthPlusBufferedDataLength()
        {
            // Arrange
            byte[] inputData = { 1, 2, 3 };
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);

            // Act: Before flush, underlying stream length is 0 and buffered length is 3.
            chunkedStream.Write(inputData, 0, inputData.Length);
            long lengthBeforeFlush = chunkedStream.Length;

            // Flush to make the underlying stream length increase.
            chunkedStream.Flush();
            long lengthAfterFlush = chunkedStream.Length;

            // Assert
            Assert.Equal(3, lengthBeforeFlush);
            Assert.Equal(3, lengthAfterFlush);
        }

        /// <summary>
        /// Tests that the Position getter returns the sum of the underlying stream's position and buffered data length.
        /// </summary>
        [Fact]
        public void PositionProperty_Get_ReturnsUnderlyingPositionPlusBufferedDataLength()
        {
            // Arrange
            byte[] data1 = { 10, 20, 30 };
            byte[] data2 = { 40, 50 };
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);

            // Write first part and flush.
            chunkedStream.Write(data1, 0, data1.Length);
            chunkedStream.Flush(); // underlying.Position now 3, buffered 0.
            Assert.Equal(3, chunkedStream.Position);

            // Write second part without flush.
            chunkedStream.Write(data2, 0, data2.Length); // buffered 2 bytes.
            long expectedPosition = memoryStream.Position + 2;
            Assert.Equal(expectedPosition, chunkedStream.Position);
        }

        /// <summary>
        /// Tests that attempting to set the Position property throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void PositionProperty_Set_ThrowsNotSupportedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var chunkedStream = new ChunkedBufferStream(memoryStream, DefaultBufferSize);

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => chunkedStream.Position = 10);
        }

        #endregion

        #region Close Tests

        /// <summary>
        /// Tests that Close flushes the buffered data and closes the underlying stream.
        /// </summary>
        [Fact]
        public void Close_CallsFlushAndClosesUnderlyingStream()
        {
            // Arrange
            byte[] data = { 77, 88, 99 };
            using var testStream = new TestMemoryStream();
            var chunkedStream = new ChunkedBufferStream(testStream, DefaultBufferSize);
            chunkedStream.Write(data, 0, data.Length);

            // Act
            chunkedStream.Close();

            // Assert: after Close, the underlying stream should have been flushed and closed.
            Assert.True(testStream.IsClosed, "Underlying stream was not closed.");
            // The flush should have written the buffered data.
            Assert.Equal(data, testStream.ToArray());
        }

        #endregion

        /// <summary>
        /// A custom MemoryStream that tracks whether it has been closed.
        /// </summary>
        private class TestMemoryStream : MemoryStream
        {
            public bool IsClosed { get; private set; } = false;

            public override void Close()
            {
                IsClosed = true;
                base.Close();
            }

            protected override void Dispose(bool disposing)
            {
                IsClosed = true;
                base.Dispose(disposing);
            }
        }
    }
}
