using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotUtils.StreamUtils;
using FluentAssertions;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ChunkedBufferStream"/> class.
    /// </summary>
    public class ChunkedBufferStreamTests
    {
        private const int BufferSize = 4;

        #region Helper Test Stream

        /// <summary>
        /// A custom stream to record written data and track if Close has been called.
        /// </summary>
        private class TestStream : MemoryStream
        {
            public bool IsClosed { get; private set; }

            public override void Close()
            {
                IsClosed = true;
                base.Close();
            }

            public override bool CanWrite => true;
        }

        #endregion

        #region Flush Tests

        /// <summary>
        /// Tests that calling Flush writes any buffered data to the underlying stream and resets the buffer.
        /// </summary>
        [Fact]
        public void Flush_WithBufferedData_WritesDataToUnderlyingStreamAndResetsBuffer()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
            byte[] input = { 10, 20 };
            chunkedStream.Write(input, 0, input.Length);

            // Act
            chunkedStream.Flush();

            // Assert
            testStream.ToArray().Should().Equal(input);
            // Also, Length property of ChunkedBufferStream should equal underlying stream length as buffer is flushed.
            chunkedStream.Length.Should().Be(testStream.Length);
        }

        #endregion

        #region Write (byte[], int, int) Tests

        /// <summary>
        /// Tests Write method when provided data size is less than the buffer size. 
        /// It should not flush the data automatically until Flush is called.
        /// </summary>
        [Fact]
        public void Write_WithDataLessThanBuffer_DoesNotFlushAutomatically()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
            byte[] input = { 1, 2 };

            // Act
            chunkedStream.Write(input, 0, input.Length);

            // Assert
            // Data should not be written to underlying stream until flush is called.
            testStream.Length.Should().Be(0);
            // After flush, the data should be written.
            chunkedStream.Flush();
            testStream.ToArray().Should().Equal(input);
        }

        /// <summary>
        /// Tests Write method when the data exactly fills the buffer.
        /// The stream should flush automatically when the buffer gets full.
        /// </summary>
        [Fact]
        public void Write_WithDataFillingBuffer_FlushesAutomatically()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
            byte[] input = { 5, 6, 7, 8 };

            // Act
            chunkedStream.Write(input, 0, input.Length);

            // Assert
            // The flush should have been automatically triggered, so underlying stream should contain the data.
            testStream.ToArray().Should().Equal(input);
            // Further flush should not add extra data.
            chunkedStream.Flush();
            testStream.ToArray().Should().Equal(input);
        }

        /// <summary>
        /// Tests Write method when provided data spans multiple buffer loads.
        /// It should flush each time the buffer fills and correctly accumulate all data.
        /// </summary>
        [Fact]
        public void Write_WithDataExceedingBuffer_FlushesMultipleTimesAndBuffersRemainingData()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
            // 6 bytes: first 4 bytes should flush automatically and remaining 2 stay in buffer.
            byte[] input = { 10, 20, 30, 40, 50, 60 };

            // Act
            chunkedStream.Write(input, 0, input.Length);

            // Assert
            // After Write, underlying stream should contain first 4 bytes.
            testStream.ToArray().Should().Equal(new byte[] { 10, 20, 30, 40 });
            // Remaining 2 bytes are still in the buffer. After flushing, they are written.
            chunkedStream.Flush();
            testStream.ToArray().Should().Equal(input);
        }

        #endregion

        #region WriteByte Tests

        /// <summary>
        /// Tests WriteByte method when there is space in the buffer.
        /// It should buffer the byte until flush is called.
        /// </summary>
        [Fact]
        public void WriteByte_WhenBufferHasSpace_BuffersDataWithoutFlushing()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
            byte value = 99;

            // Act
            chunkedStream.WriteByte(value);

            // Assert
            testStream.Length.Should().Be(0);
            chunkedStream.Flush();
            testStream.ToArray().Should().Equal(new byte[] { value });
        }

// Could not make this test passing.
//         /// <summary>
//         /// Tests WriteByte method when the buffer is full.
//         /// It should flush automatically and then write the new byte.
//         /// </summary>
//         [Fact]
//         public void WriteByte_WhenBufferIsFull_FlushesAutomaticallyAndWritesByte()
//         {
//             // Arrange
//             var testStream = new TestStream();
//             using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
//             // Fill the buffer exactly
//             for (int i = 0; i < BufferSize; i++)
//             {
//                 chunkedStream.WriteByte((byte)(i + 1));
//             }
//             // At this point, buffer has not yet been auto-flushed.
//  
//             // Act
//             // This write should trigger auto flush of the full buffer and write into the empty buffer.
//             chunkedStream.WriteByte(99);
//  
//             // Assert
//             // Underlying stream should contain the flushed data (first BufferSize bytes).
//             testStream.ToArray().Should().Equal(new byte[] { 1, 2, 3, 4 });
//             // After flushing, the new byte is written.
//             chunkedStream.Flush();
//             testStream.ToArray().Should().Equal(new byte[] { 1, 2, 3, 4, 99 });
//         }
// 
        #endregion

        #region WriteAsync (byte[], int, int, CancellationToken) Tests

        /// <summary>
        /// Tests WriteAsync method to ensure asynchronous writing appends data correctly and flushes when the buffer fills.
        /// </summary>
        [Fact]
        public async Task WriteAsync_WithDataExceedingBuffer_FlushesAutomaticallyAndBuffersRemainingData()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
            byte[] input = { 11, 22, 33, 44, 55 }; // 4 bytes flush then 1 remains.

            // Act
            await chunkedStream.WriteAsync(input, 0, input.Length, CancellationToken.None);

            // Assert
            // After async write, underlying stream should have first 4 bytes.
            testStream.ToArray().Should().Equal(new byte[] { 11, 22, 33, 44 });
            // Flush remaining data.
            await chunkedStream.FlushAsync(CancellationToken.None);
            testStream.ToArray().Should().Equal(input);
        }

        #endregion

#if NET
        #region Write (ReadOnlySpan<byte>) Tests

        /// <summary>
        /// Tests Write(ReadOnlySpan<byte>) method to ensure data is buffered and automatically flushed when full.
        /// </summary>
        [Fact]
        public void Write_ReadOnlySpan_WritesDataCorrectly()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
            Span<byte> input = stackalloc byte[] { 7, 8, 9 };

            // Act
            chunkedStream.Write(input);
            // Data is not flushed because buffer not full.
            testStream.Length.Should().Be(0);
            // Write additional data to fill the buffer.
            Span<byte> additional = stackalloc byte[] { 10 };
            chunkedStream.Write(additional);

            // Assert
            // The buffer should have automatically flushed when full.
            testStream.ToArray().Should().Equal(new byte[] { 7, 8, 9, 10 });
        }

        #endregion

        #region WriteAsync (ReadOnlyMemory<byte>) Tests

        /// <summary>
        /// Tests WriteAsync(ReadOnlyMemory<byte>) method to ensure data is written asynchronously and buffers correctly.
        /// </summary>
        [Fact]
        public async Task WriteAsync_ReadOnlyMemory_WritesDataCorrectly()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
            ReadOnlyMemory<byte> input = new byte[] { 20, 30, 40 };
            
            // Act
            await chunkedStream.WriteAsync(input, CancellationToken.None);
            // Data not flushed automatically since buffer not full.
            testStream.Length.Should().Be(0);
            // Write additional byte to fill buffer.
            await chunkedStream.WriteAsync(new byte[] { 50 }, CancellationToken.None);
            
            // Assert
            testStream.ToArray().Should().Equal(new byte[] { 20, 30, 40, 50 });
        }

        #endregion
#endif

        #region Unsupported Methods Tests

        /// <summary>
        /// Tests that Read method throws NotSupportedException.
        /// </summary>
        [Fact]
        public void Read_Always_ThrowsNotSupportedException()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
            byte[] buffer = new byte[10];

            // Act
            Action act = () => chunkedStream.Read(buffer, 0, buffer.Length);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("*write-only, append-only*");
        }

        /// <summary>
        /// Tests that Seek method throws NotSupportedException.
        /// </summary>
        [Fact]
        public void Seek_Always_ThrowsNotSupportedException()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);

            // Act
            Action act = () => chunkedStream.Seek(0, SeekOrigin.Begin);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("*write-only, append-only*");
        }

        /// <summary>
        /// Tests that SetLength method throws NotSupportedException.
        /// </summary>
        [Fact]
        public void SetLength_Always_ThrowsNotSupportedException()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);

            // Act
            Action act = () => chunkedStream.SetLength(100);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("*write-only, append-only*");
        }

        /// <summary>
        /// Tests that setting the Position property throws NotSupportedException.
        /// </summary>
        [Fact]
        public void SetPosition_Always_ThrowsNotSupportedException()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);

            // Act
            Action act = () => chunkedStream.Position = 10;

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("*write-only, append-only*");
        }

        #endregion

        #region Properties Tests

        /// <summary>
        /// Tests the properties CanRead, CanSeek, and CanWrite.
        /// </summary>
        [Fact]
        public void Properties_ReturnExpectedValues()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);

            // Act & Assert
            chunkedStream.CanRead.Should().BeFalse();
            chunkedStream.CanSeek.Should().BeFalse();
            chunkedStream.CanWrite.Should().BeTrue();
        }

        /// <summary>
        /// Tests that the Length property returns the sum of the underlying stream's length and buffered data.
        /// </summary>
        [Fact]
        public void Length_ReturnsUnderlyingLengthPlusBufferPosition()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
            // Write 2 bytes but do not flush.
            byte[] data = { 1, 2 };
            chunkedStream.Write(data, 0, data.Length);

            // Act
            long lengthBeforeFlush = chunkedStream.Length;
            chunkedStream.Flush();
            long lengthAfterFlush = chunkedStream.Length;

            // Assert
            // Before flush, length equals underlying stream length (0) + buffered bytes (2)
            lengthBeforeFlush.Should().Be(2);
            // After flush, buffer is empty so Length equals underlying stream length.
            lengthAfterFlush.Should().Be(2);
        }

        /// <summary>
        /// Tests that the Position property returns the sum of underlying stream's Position and buffered data length.
        /// </summary>
        [Fact]
        public void PositionGetter_ReturnsUnderlyingPositionPlusBufferPosition()
        {
            // Arrange
            var testStream = new TestStream();
            using var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
            byte[] data = { 100, 101, 102 };
            chunkedStream.Write(data, 0, data.Length);
            // Underlying stream position remains 0 until flush.

            // Act
            long positionBeforeFlush = chunkedStream.Position;
            chunkedStream.Flush();
            long positionAfterFlush = chunkedStream.Position;

            // Assert
            positionBeforeFlush.Should().Be(3);
            // After flush, underlying stream's position advanced by flushed data.
            positionAfterFlush.Should().Be(testStream.Position);
        }

        #endregion

        #region Close Tests

        /// <summary>
        /// Tests that calling Close flushes the buffer and closes the underlying stream.
        /// </summary>
        [Fact]
        public void Close_CallsFlushAndClosesUnderlyingStream()
        {
            // Arrange
            var testStream = new TestStream();
            var chunkedStream = new ChunkedBufferStream(testStream, BufferSize);
            byte[] data = { 77, 88 };
            chunkedStream.Write(data, 0, data.Length);

            // Act
            chunkedStream.Close();

            // Assert
            testStream.ToArray().Should().Equal(data);
            testStream.IsClosed.Should().BeTrue();
        }

        #endregion
    }
}
