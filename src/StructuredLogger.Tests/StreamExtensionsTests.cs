using DotUtils.StreamUtils;
using FluentAssertions;
using Moq;
using System;
using System.Buffers;
using System.IO;
using System.Text;
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
        /// Tests that ReadAtLeast returns the exact minimum number of bytes when the stream supplies data in one read.
        /// </summary>
        [Fact]
        public void ReadAtLeast_HappyPath_ReadsExactMinimumBytes()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("HelloWorld");
            using MemoryStream stream = new MemoryStream(data);
            byte[] buffer = new byte[10];
            int offset = 0;
            int minimumBytes = 5;
            bool throwOnEndOfStream = true;

            // Act
            int bytesRead = stream.ReadAtLeast(buffer, offset, minimumBytes, throwOnEndOfStream);

            // Assert
            bytesRead.Should().Be(minimumBytes);
            // Verify that the read bytes match the expected data.
            buffer.AsSpan(0, minimumBytes).ToArray().Should().BeEquivalentTo(data.AsSpan(0, minimumBytes).ToArray());
        }

        /// <summary>
        /// Tests that ReadAtLeast successfully accumulates data from multiple partial reads.
        /// </summary>
        [Fact]
        public void ReadAtLeast_PartialReads_AccumulatesUntilMinimumReached()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("PartialReadTest");
            // Use a MemoryStream wrapped with PartialReadStream to simulate partial reads.
            using MemoryStream innerStream = new MemoryStream(data);
            using PartialReadStream stream = new PartialReadStream(innerStream, maxChunkSize: 3);
            byte[] buffer = new byte[data.Length];
            int offset = 0;
            int minimumBytes = 10;
            bool throwOnEndOfStream = true;

            // Act
            int bytesRead = stream.ReadAtLeast(buffer, offset, minimumBytes, throwOnEndOfStream);

            // Assert
            bytesRead.Should().Be(minimumBytes);
            buffer.AsSpan(0, bytesRead).ToArray().Should().BeEquivalentTo(data.AsSpan(0, minimumBytes).ToArray());
        }

        /// <summary>
        /// Tests that ReadAtLeast returns the total number of bytes read if the stream ends before reaching the minimum and throwOnEndOfStream is false.
        /// </summary>
        [Fact]
        public void ReadAtLeast_EndOfStreamWithoutThrow_ReturnsBytesRead()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("Short");
            using MemoryStream stream = new MemoryStream(data);
            byte[] buffer = new byte[10];
            int offset = 0;
            int minimumBytes = 10;
            bool throwOnEndOfStream = false;

            // Act
            int bytesRead = stream.ReadAtLeast(buffer, offset, minimumBytes, throwOnEndOfStream);

            // Assert
            bytesRead.Should().Be(data.Length);
        }

        /// <summary>
        /// Tests that ReadAtLeast throws an InvalidDataException when the stream ends before reaching the minimum and throwOnEndOfStream is true.
        /// </summary>
        [Fact]
        public void ReadAtLeast_EndOfStreamWithThrow_ThrowsInvalidDataException()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("Short");
            using MemoryStream stream = new MemoryStream(data);
            byte[] buffer = new byte[10];
            int offset = 0;
            int minimumBytes = 10;
            bool throwOnEndOfStream = true;

            // Act
            Action act = () => stream.ReadAtLeast(buffer, offset, minimumBytes, throwOnEndOfStream);

            // Assert
            act.Should().Throw<InvalidDataException>()
                .WithMessage("Unexpected end of stream.");
        }

        #endregion

        #region SkipBytes Tests

        /// <summary>
        /// Tests that SkipBytes with zero bytes to skip returns 0 and does not change stream position.
        /// </summary>
        [Fact]
        public void SkipBytes_ZeroBytes_ReturnsZeroAndNoAdvance()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("SampleData");
            using MemoryStream stream = new MemoryStream(data);
            long bytesToSkip = 0;
            bool throwOnEndOfStream = true;
            long originalPosition = stream.Position;

            // Act
            int skipped = stream.SkipBytes(bytesToSkip, throwOnEndOfStream);

            // Assert
            skipped.Should().Be(0);
            stream.Position.Should().Be(originalPosition);
        }

        /// <summary>
        /// Tests that SkipBytes skips the specified number of bytes when enough data is available.
        /// </summary>
        [Fact]
        public void SkipBytes_ValidBytes_SkipsCorrectly()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("1234567890");
            using MemoryStream stream = new MemoryStream(data);
            long bytesToSkip = 5;
            bool throwOnEndOfStream = true;

            // Act
            int skipped = stream.SkipBytes(bytesToSkip, throwOnEndOfStream);

            // Assert
            skipped.Should().Be(5);
            stream.Position.Should().Be(5);
        }

        /// <summary>
        /// Tests that SkipBytes throws an InvalidDataException when end of stream is reached before skipping required bytes and throwOnEndOfStream is true.
        /// </summary>
        [Fact]
        public void SkipBytes_EndOfStreamWithThrow_ThrowsInvalidDataException()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("Data");
            using MemoryStream stream = new MemoryStream(data);
            long bytesToSkip = 10;
            bool throwOnEndOfStream = true;

            // Act
            Action act = () => stream.SkipBytes(bytesToSkip, throwOnEndOfStream);

            // Assert
            act.Should().Throw<InvalidDataException>()
                .WithMessage("Unexpected end of stream.");
        }

        /// <summary>
        /// Tests that SkipBytes returns the number of bytes skipped when end of stream is reached and throwOnEndOfStream is false.
        /// </summary>
        [Fact]
        public void SkipBytes_EndOfStreamWithoutThrow_ReturnsBytesSkipped()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("Data");
            using MemoryStream stream = new MemoryStream(data);
            long bytesToSkip = 10;
            bool throwOnEndOfStream = false;

            // Act
            int skipped = stream.SkipBytes(bytesToSkip, throwOnEndOfStream);

            // Assert
            skipped.Should().Be(data.Length);
            stream.Position.Should().Be(data.Length);
        }
//  // [Error] (206-27)CS0266 Cannot implicitly convert type 'long' to 'int'. An explicit conversion exists (are you missing a cast?) // [Error] (209-33)CS1503 Argument 1: cannot convert from 'long' to 'int'
//         /// <summary>
//         /// Tests that SkipBytes overload without bytesCount parameter skips stream.Length bytes.
//         /// </summary>
//         [Fact]
//         public void SkipBytes_WithoutBytesCount_SkipsEntireStream()
//         {
//             // Arrange
//             byte[] data = Encoding.ASCII.GetBytes("CompleteStream");
//             using MemoryStream stream = new MemoryStream(data);
//             long expectedSkip = stream.Length;
// 
//             // Act
//             int skipped = stream.SkipBytes();
// 
//             // Assert
//             skipped.Should().Be(expectedSkip);
//             stream.Position.Should().Be(stream.Length);
//         }
// 
        /// <summary>
        /// Tests that SkipBytes throws ArgumentOutOfRangeException when a negative bytesCount is provided.
        /// </summary>
        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void SkipBytes_NegativeBytesCount_ThrowsArgumentOutOfRangeException(long negativeBytes)
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("TestData");
            using MemoryStream stream = new MemoryStream(data);
            bool throwOnEndOfStream = true;

            // Act
            Action act = () => stream.SkipBytes(negativeBytes, throwOnEndOfStream);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*Attempt to skip*");
        }

        /// <summary>
        /// Tests that SkipBytes overload with buffer parameter correctly skips bytes.
        /// </summary>
        [Fact]
        public void SkipBytes_WithBuffer_SkipsCorrectly()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("BufferSkipTestData");
            using MemoryStream stream = new MemoryStream(data);
            long bytesToSkip = 8;
            bool throwOnEndOfStream = true;
            byte[] buffer = new byte[4096];

            // Act
            int skipped = stream.SkipBytes(bytesToSkip, throwOnEndOfStream, buffer);

            // Assert
            skipped.Should().Be(8);
            stream.Position.Should().Be(8);
        }

        #endregion

        #region ReadToEnd Tests

        /// <summary>
        /// Tests that ReadToEnd returns the complete contents of the stream.
        /// </summary>
        [Fact]
        public void ReadToEnd_HappyPath_ReturnsAllBytes()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("ReadToEndTestData");
            using MemoryStream stream = new MemoryStream(data);

            // Act
            byte[] result = stream.ReadToEnd();

            // Assert
            result.Should().BeEquivalentTo(data);
        }

        #endregion

        #region TryGetLength Tests

        /// <summary>
        /// Tests that TryGetLength returns true and outputs correct length for a seekable stream.
        /// </summary>
        [Fact]
        public void TryGetLength_SeekableStream_ReturnsTrueAndCorrectLength()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("SeekableStreamData");
            using MemoryStream stream = new MemoryStream(data);

            // Act
            bool result = stream.TryGetLength(out long length);

            // Assert
            result.Should().BeTrue();
            length.Should().Be(stream.Length);
        }

        /// <summary>
        /// Tests that TryGetLength returns false and outputs zero for a non-seekable stream.
        /// </summary>
        [Fact]
        public void TryGetLength_NonSeekableStream_ReturnsFalseAndZeroLength()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("NonSeekableStreamData");
            using NonSeekableStream stream = new NonSeekableStream(new MemoryStream(data));

            // Act
            bool result = stream.TryGetLength(out long length);

            // Assert
            result.Should().BeFalse();
            length.Should().Be(0);
        }

        #endregion

        #region ToReadableSeekableStream Tests

        /// <summary>
        /// Tests that ToReadableSeekableStream returns a stream that is not null.
        /// </summary>
        [Fact]
        public void ToReadableSeekableStream_HappyPath_ReturnsNonNullStream()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("ReadableSeekable");
            using MemoryStream stream = new MemoryStream(data);

            // Act
            Stream result = stream.ToReadableSeekableStream();

            // Assert
            result.Should().NotBeNull();
        }

        #endregion

        #region Slice Tests

        /// <summary>
        /// Tests that Slice returns a stream instance representing a bounded view over the original stream.
        /// </summary>
        [Fact]
        public void Slice_HappyPath_ReturnsSubStream()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("SliceTestData");
            MemoryStream stream = new MemoryStream(data);
            long sliceLength = 5;

            // Act
            Stream result = stream.Slice(sliceLength);

            // Assert
            result.Should().NotBeNull();
            // Since SubStream is an internal implementation, we check the type name.
            result.GetType().Name.Should().Be("SubStream");
        }

        #endregion

        #region Concat Tests

        /// <summary>
        /// Tests that Concat returns a stream that concatenates two streams.
        /// </summary>
        [Fact]
        public void Concat_HappyPath_ReturnsConcatenatedReadStream()
        {
            // Arrange
            byte[] data1 = Encoding.ASCII.GetBytes("Hello");
            byte[] data2 = Encoding.ASCII.GetBytes("World");
            MemoryStream stream1 = new MemoryStream(data1);
            MemoryStream stream2 = new MemoryStream(data2);

            // Act
            Stream result = stream1.Concat(stream2);

            // Assert
            result.Should().NotBeNull();
            // Since ConcatenatedReadStream is an internal implementation, we check the type name.
            result.GetType().Name.Should().Be("ConcatenatedReadStream");
        }

        #endregion
    }

    /// <summary>
    /// A helper stream class to simulate partial reads.
    /// Wraps an inner stream and limits the maximum number of bytes returned per read.
    /// </summary>
    public class PartialReadStream : Stream
    {
        private readonly Stream _innerStream;
        private readonly int _maxChunkSize;

        public PartialReadStream(Stream innerStream, int maxChunkSize)
        {
            _innerStream = innerStream ?? throw new ArgumentNullException(nameof(innerStream));
            _maxChunkSize = maxChunkSize > 0 ? maxChunkSize : throw new ArgumentOutOfRangeException(nameof(maxChunkSize));
        }

        public override bool CanRead => _innerStream.CanRead;
        public override bool CanSeek => _innerStream.CanSeek;
        public override bool CanWrite => false;
        public override long Length => _innerStream.Length;

        public override long Position 
        { 
            get => _innerStream.Position; 
            set => _innerStream.Position = value; 
        }

        public override void Flush() => _innerStream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
        {
            int allowedCount = Math.Min(count, _maxChunkSize);
            return _innerStream.Read(buffer, offset, allowedCount);
        }

        public override long Seek(long offset, SeekOrigin origin) => _innerStream.Seek(offset, origin);

        public override void SetLength(long value) => _innerStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }

    /// <summary>
    /// A helper non-seekable stream wrapper for testing TryGetLength.
    /// </summary>
    public class NonSeekableStream : Stream
    {
        private readonly Stream _innerStream;

        public NonSeekableStream(Stream innerStream)
        {
            _innerStream = innerStream ?? throw new ArgumentNullException(nameof(innerStream));
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

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => _innerStream.Write(buffer, offset, count);
    }
}
