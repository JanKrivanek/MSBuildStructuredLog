using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DotUtils.StreamUtils;
using Moq;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="StreamExtensions"/> class.
    /// </summary>
    public class StreamExtensionsTests
    {
        #region ReadAtLeast Tests

        /// <summary>
        /// Tests that ReadAtLeast returns the exact number of minimum bytes when enough data is available.
        /// </summary>
        [Fact]
        public void ReadAtLeast_WithEnoughData_ReturnsMinimumBytes()
        {
            // Arrange
            byte[] streamData = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray();
            using MemoryStream stream = new MemoryStream(streamData);
            byte[] buffer = new byte[15];
            int offset = 5;
            int minimumBytes = 6; // read 6 bytes starting at buffer[5]

            // Act
            int bytesRead = stream.ReadAtLeast(buffer, offset, minimumBytes, throwOnEndOfStream: true);

            // Assert
            Assert.Equal(minimumBytes, bytesRead);
            for (int i = 0; i < minimumBytes; i++)
            {
                Assert.Equal(streamData[i], buffer[offset + i]);
            }
        }

        /// <summary>
        /// Tests that ReadAtLeast throws InvalidDataException when stream ends before reaching minimum bytes and throwOnEndOfStream is true.
        /// </summary>
        [Fact]
        public void ReadAtLeast_WhenStreamEndsAndThrowOnEndOfStreamTrue_ThrowsInvalidDataException()
        {
            // Arrange
            byte[] streamData = new byte[] { 1, 2 }; // only 2 bytes available
            using MemoryStream stream = new MemoryStream(streamData);
            byte[] buffer = new byte[5];
            int offset = 0;
            int minimumBytes = 3; // require 3 bytes

            // Act & Assert
            Assert.Throws<InvalidDataException>(() =>
                stream.ReadAtLeast(buffer, offset, minimumBytes, throwOnEndOfStream: true));
        }

        /// <summary>
        /// Tests that ReadAtLeast returns the partial bytes read when stream ends and throwOnEndOfStream is false.
        /// </summary>
        [Fact]
        public void ReadAtLeast_WhenStreamEndsAndThrowOnEndOfStreamFalse_ReturnsPartialCount()
        {
            // Arrange
            byte[] streamData = new byte[] { 10, 20 }; // only 2 bytes available
            using MemoryStream stream = new MemoryStream(streamData);
            byte[] buffer = new byte[5];
            int offset = 1;
            int minimumBytes = 3;

            // Act
            int bytesRead = stream.ReadAtLeast(buffer, offset, minimumBytes, throwOnEndOfStream: false);

            // Assert
            Assert.Equal(2, bytesRead);
            Assert.Equal(10, buffer[1]);
            Assert.Equal(20, buffer[2]);
        }

        #endregion

        #region SkipBytes Tests

        /// <summary>
        /// Tests that SkipBytes with bytesCount 0 returns 0 without reading any data.
        /// </summary>
        [Fact]
        public void SkipBytes_WithZeroBytes_ReturnsZero()
        {
            // Arrange
            byte[] data = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray();
            using MemoryStream stream = new MemoryStream(data);

            // Act
            int skipped = stream.SkipBytes(0, throwOnEndOfStream: true);

            // Assert
            Assert.Equal(0, skipped);
            Assert.Equal(0, stream.Position);
        }

        /// <summary>
        /// Tests that SkipBytes successfully skips the requested number of bytes when enough data is available.
        /// </summary>
        [Fact]
        public void SkipBytes_WithEnoughData_SkipsCorrectNumberOfBytes()
        {
            // Arrange
            byte[] data = Enumerable.Range(0, 100).Select(i => (byte)i).ToArray();
            using MemoryStream stream = new MemoryStream(data);
            int bytesToSkip = 50;

            // Act
            int skipped = stream.SkipBytes(bytesToSkip, throwOnEndOfStream: true);

            // Assert
            Assert.Equal(bytesToSkip, skipped);
            Assert.Equal(bytesToSkip, stream.Position);
        }

        /// <summary>
        /// Tests that SkipBytes throws InvalidDataException when not enough data is available and throwOnEndOfStream is true.
        /// </summary>
        [Fact]
        public void SkipBytes_WhenNotEnoughDataAndThrowOnEndOfStreamTrue_ThrowsInvalidDataException()
        {
            // Arrange
            byte[] data = Enumerable.Range(0, 30).Select(i => (byte)i).ToArray();
            using MemoryStream stream = new MemoryStream(data);
            int bytesToSkip = 40;

            // Act & Assert
            Assert.Throws<InvalidDataException>(() =>
                stream.SkipBytes(bytesToSkip, throwOnEndOfStream: true));
        }

        /// <summary>
        /// Tests that SkipBytes returns partial bytes skipped when not enough data is available and throwOnEndOfStream is false.
        /// </summary>
        [Fact]
        public void SkipBytes_WhenNotEnoughDataAndThrowOnEndOfStreamFalse_ReturnsPartialSkippedCount()
        {
            // Arrange
            byte[] data = Enumerable.Range(0, 30).Select(i => (byte)i).ToArray();
            using MemoryStream stream = new MemoryStream(data);
            int bytesToSkip = 40;

            // Act
            int skipped = stream.SkipBytes(bytesToSkip, throwOnEndOfStream: false);

            // Assert
            Assert.Equal(data.Length, skipped);
            Assert.Equal(data.Length, stream.Position);
        }

        /// <summary>
        /// Tests that SkipBytes with negative bytesCount throws ArgumentOutOfRangeException.
        /// </summary>
        [Fact]
        public void SkipBytes_WithNegativeBytesCount_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            byte[] data = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray();
            using MemoryStream stream = new MemoryStream(data);
            long invalidBytesCount = -1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                stream.SkipBytes(invalidBytesCount, throwOnEndOfStream: true));
        }

        /// <summary>
        /// Tests that SkipBytes with bytesCount greater than int.MaxValue throws ArgumentOutOfRangeException.
        /// </summary>
        [Fact]
        public void SkipBytes_WithBytesCountGreaterThanIntMaxValue_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            byte[] data = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray();
            using MemoryStream stream = new MemoryStream(data);
            long invalidBytesCount = (long)int.MaxValue + 1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                stream.SkipBytes(invalidBytesCount, throwOnEndOfStream: true));
        }

        /// <summary>
        /// Tests the SkipBytes() overload that uses stream.Length as bytesCount.
        /// </summary>
        [Fact]
        public void SkipBytes_WithoutParameters_SkipsEntireStream()
        {
            // Arrange
            byte[] data = Enumerable.Range(0, 20).Select(i => (byte)i).ToArray();
            using MemoryStream stream = new MemoryStream(data);

            // Act
            int skipped = stream.SkipBytes();

            // Assert
            Assert.Equal(data.Length, skipped);
            Assert.Equal(data.Length, stream.Position);
        }

        /// <summary>
        /// Tests the SkipBytes(long bytesCount) overload that uses throwOnEndOfStream default true.
        /// </summary>
        [Fact]
        public void SkipBytes_WithLongBytesCount_SkipsCorrectNumberOfBytes()
        {
            // Arrange
            byte[] data = Enumerable.Range(0, 60).Select(i => (byte)i).ToArray();
            using MemoryStream stream = new MemoryStream(data);
            long bytesToSkip = 30;

            // Act
            int skipped = stream.SkipBytes(bytesToSkip);

            // Assert
            Assert.Equal((int)bytesToSkip, skipped);
            Assert.Equal(bytesToSkip, stream.Position);
        }

        #endregion

        #region ReadToEnd Tests

        /// <summary>
        /// Tests that ReadToEnd returns the complete content of a seekable MemoryStream.
        /// </summary>
        [Fact]
        public void ReadToEnd_WithSeekableStream_ReturnsAllBytes()
        {
            // Arrange
            byte[] expectedContent = Enumerable.Range(0, 50).Select(i => (byte)i).ToArray();
            using MemoryStream stream = new MemoryStream(expectedContent);

            // Act
            byte[] result = stream.ReadToEnd();

            // Assert
            Assert.Equal(expectedContent, result);
        }

        /// <summary>
        /// Tests that ReadToEnd returns the complete content of a non-seekable stream.
        /// </summary>
        [Fact]
        public void ReadToEnd_WithNonSeekableStream_ReturnsAllBytes()
        {
            // Arrange
            byte[] expectedContent = Enumerable.Range(100, 30).Select(i => (byte)i).ToArray();
            using MemoryStream innerStream = new MemoryStream(expectedContent);
            using NonSeekableStream stream = new NonSeekableStream(innerStream);

            // Act
            byte[] result = stream.ReadToEnd();

            // Assert
            Assert.Equal(expectedContent, result);
        }

        #endregion

        #region TryGetLength Tests

        /// <summary>
        /// Tests that TryGetLength returns true and the correct length for a seekable stream.
        /// </summary>
        [Fact]
        public void TryGetLength_WithSeekableStream_ReturnsTrueAndLength()
        {
            // Arrange
            byte[] data = Enumerable.Range(0, 25).Select(i => (byte)i).ToArray();
            using MemoryStream stream = new MemoryStream(data);

            // Act
            bool result = stream.TryGetLength(out long length);

            // Assert
            Assert.True(result);
            Assert.Equal(data.Length, length);
        }

        /// <summary>
        /// Tests that TryGetLength returns false and length is set to 0 for a non-seekable stream.
        /// </summary>
        [Fact]
        public void TryGetLength_WithNonSeekableStream_ReturnsFalseAndZeroLength()
        {
            // Arrange
            byte[] data = Enumerable.Range(0, 25).Select(i => (byte)i).ToArray();
            using MemoryStream innerStream = new MemoryStream(data);
            using NonSeekableStream stream = new NonSeekableStream(innerStream);

            // Act
            bool result = stream.TryGetLength(out long length);

            // Assert
            Assert.False(result);
            Assert.Equal(0, length);
        }

        #endregion

        #region ToReadableSeekableStream Tests

        /// <summary>
        /// Tests that ToReadableSeekableStream returns the same instance for a stream that is already seekable.
        /// </summary>
        [Fact]
        public void ToReadableSeekableStream_WithSeekableStream_ReturnsSameInstance()
        {
            // Arrange
            using MemoryStream stream = new MemoryStream(new byte[] { 1, 2, 3 });

            // Act
            Stream result = stream.ToReadableSeekableStream();

            // Assert
            Assert.Same(stream, result);
        }

        /// <summary>
        /// Tests that ToReadableSeekableStream returns a seekable stream when given a non-seekable stream.
        /// </summary>
        [Fact]
        public void ToReadableSeekableStream_WithNonSeekableStream_ReturnsSeekableStream()
        {
            // Arrange
            using MemoryStream innerStream = new MemoryStream(new byte[] { 4, 5, 6 });
            using NonSeekableStream nonSeekable = new NonSeekableStream(innerStream);

            // Act
            Stream result = nonSeekable.ToReadableSeekableStream();

            // Assert
            Assert.True(result.CanSeek);
        }

        #endregion

        #region Slice Tests

        /// <summary>
        /// Tests that Slice creates a stream that only allows reading up to the specified length.
        /// </summary>
        [Fact]
        public void Slice_WithValidLength_ReturnsSubStreamWithRestrictedContent()
        {
            // Arrange
            byte[] data = Enumerable.Range(10, 20).Select(i => (byte)i).ToArray();
            using MemoryStream stream = new MemoryStream(data);
            int sliceLength = 10;

            // Act
            using Stream slice = stream.Slice(sliceLength);
            byte[] result = new byte[20];
            int readBytes = slice.Read(result, 0, result.Length);

            // Assert
            Assert.Equal(sliceLength, readBytes);
            Assert.Equal(data.Take(sliceLength), result.Take(readBytes));
            // Further reads should result in 0 bytes.
            Assert.Equal(0, slice.Read(result, 0, result.Length));
        }

        #endregion

        #region Concat Tests

        /// <summary>
        /// Tests that Concat returns a stream that concatenates the contents of two streams.
        /// </summary>
        [Fact]
        public void Concat_WithTwoStreams_ReturnsStreamWithConcatenatedContent()
        {
            // Arrange
            byte[] firstData = Enumerable.Range(1, 5).Select(i => (byte)i).ToArray();
            byte[] secondData = Enumerable.Range(101, 5).Select(i => (byte)i).ToArray();
            using MemoryStream firstStream = new MemoryStream(firstData);
            using MemoryStream secondStream = new MemoryStream(secondData);

            // Act
            using Stream concatStream = firstStream.Concat(secondStream);
            byte[] result = concatStream.ReadToEnd();

            // Assert
            byte[] expected = firstData.Concat(secondData).ToArray();
            Assert.Equal(expected, result);
        }

        #endregion

        #region Helper Classes

        /// <summary>
        /// A non-seekable stream wrapper used for testing purposes.
        /// </summary>
        private class NonSeekableStream : Stream
        {
            private readonly Stream _innerStream;

            public NonSeekableStream(Stream innerStream)
            {
                _innerStream = innerStream;
            }

            public override bool CanRead => _innerStream.CanRead;
            public override bool CanSeek => false;
            public override bool CanWrite => _innerStream.CanWrite;
            public override long Length => throw new NotSupportedException();
            public override long Position
            {
                get => _innerStream.Position;
                set => throw new NotSupportedException();
            }

            public override void Flush() => _innerStream.Flush();

            public override int Read(byte[] buffer, int offset, int count) => _innerStream.Read(buffer, offset, count);

            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

            public override void SetLength(long value) => _innerStream.SetLength(value);

            public override void Write(byte[] buffer, int offset, int count) => _innerStream.Write(buffer, offset, count);

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _innerStream.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}
