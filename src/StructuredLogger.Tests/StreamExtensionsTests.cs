using System;
using System.Buffers;
using System.IO;
using System.Text;
using DotUtils.StreamUtils;
using FluentAssertions;
using Moq;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Contains unit tests for the <see cref="StreamExtensions"/> class.
    /// </summary>
    public class StreamExtensionsTests
    {
        /// <summary>
        /// Tests that the ReadAtLeast method returns the minimum number of bytes when the stream contains enough data.
        /// </summary>
        [Fact]
        public void ReadAtLeast_WhenEnoughBytesInStream_ReturnsMinimumBytes()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("abcdef");
            using MemoryStream ms = new MemoryStream(data);
            byte[] buffer = new byte[10];
            int offset = 2;
            int minimumBytes = 4;
            // Act
            int bytesRead = ms.ReadAtLeast(buffer, offset, minimumBytes, throwOnEndOfStream: true);
            // Assert
            bytesRead.Should().Be(minimumBytes);
            // Also ensure that the buffer contains the expected data at the correct offset.
            buffer.AsSpan(offset, minimumBytes).SequenceEqual(data.AsSpan(0, minimumBytes)).Should().BeTrue();
        }

        /// <summary>
        /// Tests that ReadAtLeast returns the available bytes when the stream does not have enough data and throwOnEndOfStream is false.
        /// </summary>
        [Fact]
        public void ReadAtLeast_WhenNotEnoughBytesWithoutThrow_ReturnsAvailableBytes()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("xyz");
            using MemoryStream ms = new MemoryStream(data);
            byte[] buffer = new byte[10];
            int offset = 0;
            int minimumBytes = 5;
            // Act
            int bytesRead = ms.ReadAtLeast(buffer, offset, minimumBytes, throwOnEndOfStream: false);
            // Assert
            bytesRead.Should().Be(data.Length);
            buffer.AsSpan(0, data.Length).SequenceEqual(data).Should().BeTrue();
        }

        /// <summary>
        /// Tests that ReadAtLeast throws an InvalidDataException when the stream does not have enough data and throwOnEndOfStream is true.
        /// </summary>
        [Fact]
        public void ReadAtLeast_WhenNotEnoughBytesWithThrow_ThrowsInvalidDataException()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("1234");
            using MemoryStream ms = new MemoryStream(data);
            byte[] buffer = new byte[10];
            int offset = 0;
            int minimumBytes = 5;
            // Act
            Action act = () => ms.ReadAtLeast(buffer, offset, minimumBytes, throwOnEndOfStream: true);
            // Assert
            act.Should().Throw<InvalidDataException>()
                .WithMessage("Unexpected end of stream.");
        }

        /// <summary>
        /// Tests that SkipBytes() correctly skips all bytes in the stream.
        /// </summary>
        [Fact]
        public void SkipBytes_WithoutParameters_SkipsEntireStream()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("Hello World");
            using MemoryStream ms = new MemoryStream(data);
            long totalLength = ms.Length;
            // Act
            long skipped = ms.SkipBytes();
            // Assert
            skipped.Should().Be(totalLength);
            ms.Position.Should().Be(totalLength);
        }

        /// <summary>
        /// Tests that SkipBytes(long bytesCount) correctly skips the specified number of bytes when available.
        /// </summary>
        [Fact]
        public void SkipBytes_WithSpecifiedBytes_SkipsExactCount()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("TestStreamData");
            using MemoryStream ms = new MemoryStream(data);
            int skipCount = 5;
            // Act
            long skipped = ms.SkipBytes(skipCount);
            // Assert
            skipped.Should().Be(skipCount);
            ms.Position.Should().Be(skipCount);
        }

        /// <summary>
        /// Tests that SkipBytes returns the available bytes when skipping more bytes than available and throwOnEndOfStream is false.
        /// </summary>
        [Fact]
        public void SkipBytes_WhenNotEnoughBytesWithoutThrow_ReturnsAvailableBytes()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("Short");
            using MemoryStream ms = new MemoryStream(data);
            int skipCount = 10;
            // Act
            long skipped = ms.SkipBytes(skipCount, throwOnEndOfStream: false);
            // Assert
            skipped.Should().Be(data.Length);
            ms.Position.Should().Be(data.Length);
        }

        /// <summary>
        /// Tests that SkipBytes with provided buffer works correctly.
        /// </summary>
        [Fact]
        public void SkipBytes_WithProvidedBuffer_WorksCorrectly()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("BufferTestData");
            using MemoryStream ms = new MemoryStream(data);
            byte[] customBuffer = new byte[4096];
            int skipCount = 6;
            // Act
            int skipped = ms.SkipBytes(skipCount, throwOnEndOfStream: true, buffer: customBuffer);
            // Assert
            skipped.Should().Be(skipCount);
            ms.Position.Should().Be(skipCount);
        }

        /// <summary>
        /// Tests that SkipBytes throws an ArgumentOutOfRangeException when a negative bytesCount is specified.
        /// </summary>
        [Fact]
        public void SkipBytes_WithNegativeBytes_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("Data");
            using MemoryStream ms = new MemoryStream(data);
            // Act
            Action act = () => ms.SkipBytes(-5, throwOnEndOfStream: true);
            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*-5*");
        }

        /// <summary>
        /// Tests that SkipBytes throws an ArgumentOutOfRangeException when bytesCount exceeds int.MaxValue.
        /// </summary>
        [Fact]
        public void SkipBytes_WithBytesCountExceedingIntMaxValue_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("Data");
            using MemoryStream ms = new MemoryStream(data);
            long tooLargeValue = (long)int.MaxValue + 1;
            // Act
            Action act = () => ms.SkipBytes(tooLargeValue, throwOnEndOfStream: true);
            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage($"*{tooLargeValue}*");
        }

        /// <summary>
        /// Tests that ReadToEnd reads the entire stream and returns the expected byte array.
        /// </summary>
        [Fact]
        public void ReadToEnd_ReadsEntireStream_ReturnsAllBytes()
        {
            // Arrange
            byte[] originalData = Encoding.ASCII.GetBytes("CompleteStreamData");
            using MemoryStream ms = new MemoryStream(originalData);
            // Act
            byte[] result = ms.ReadToEnd();
            // Assert
            result.Should().BeEquivalentTo(originalData);
        }

        /// <summary>
        /// Tests that TryGetLength returns true and the correct length for seekable streams.
        /// </summary>
        [Fact]
        public void TryGetLength_ForSeekableStream_ReturnsTrueAndCorrectLength()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("1234567890");
            using MemoryStream ms = new MemoryStream(data);
            // Act
            bool result = ms.TryGetLength(out long length);
            // Assert
            result.Should().BeTrue();
            length.Should().Be(data.Length);
        }

        /// <summary>
        /// Tests that TryGetLength returns false for non-seekable streams.
        /// </summary>
        [Fact]
        public void TryGetLength_ForNonSeekableStream_ReturnsFalse()
        {
            // Arrange
            using NonSeekableStream nonSeekable = new NonSeekableStream(Encoding.ASCII.GetBytes("NonSeekable"));
            // Act
            bool result = nonSeekable.TryGetLength(out long length);
            // Assert
            result.Should().BeFalse();
            length.Should().Be(0);
        }

        /// <summary>
        /// Tests that ToReadableSeekableStream returns a stream that is seekable when given a seekable stream.
        /// </summary>
        [Fact]
        public void ToReadableSeekableStream_ForSeekableStream_ReturnsSeekableStream()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes("SeekableData"));
            // Act
            Stream result = ms.ToReadableSeekableStream();
            // Assert
            result.Should().NotBeNull();
            result.CanSeek.Should().BeTrue();
        }

        /// <summary>
        /// Tests that ToReadableSeekableStream returns a seekable stream when given a non-seekable stream.
        /// </summary>
        [Fact]
        public void ToReadableSeekableStream_ForNonSeekableStream_ReturnsSeekableStream()
        {
            // Arrange
            using NonSeekableStream nonSeekable = new NonSeekableStream(Encoding.ASCII.GetBytes("NonSeekableData"));
            // Act
            Stream result = nonSeekable.ToReadableSeekableStream();
            // Assert
            result.Should().NotBeNull();
            result.CanSeek.Should().BeTrue();
        }

        /// <summary>
        /// Tests that Slice creates a stream limited to the specified length.
        /// </summary>
        [Fact]
        public void Slice_CreatesStreamLimitedToSpecifiedLength()
        {
            // Arrange
            byte[] data = Encoding.ASCII.GetBytes("1234567890");
            using MemoryStream ms = new MemoryStream(data);
            int sliceLength = 5;
            // Act
            using Stream sliced = ms.Slice(sliceLength);
            byte[] buffer = new byte[10];
            int totalRead = sliced.Read(buffer, 0, buffer.Length);
            // Assert
            totalRead.Should().Be(sliceLength);
            // Verify that reading beyond the slice returns 0 bytes.
            int additionalRead = sliced.Read(buffer, 0, buffer.Length);
            additionalRead.Should().Be(0);
            // Verify the data read matches the expected segment.
            buffer.AsSpan(0, sliceLength).SequenceEqual(data.AsSpan(0, sliceLength)).Should().BeTrue();
        }

        /// <summary>
        /// Tests that Concat correctly concatenates two streams.
        /// </summary>
        [Fact]
        public void Concat_ConcatenatesTwoStreams()
        {
            // Arrange
            byte[] firstData = Encoding.ASCII.GetBytes("Hello");
            byte[] secondData = Encoding.ASCII.GetBytes("World");
            using MemoryStream firstStream = new MemoryStream(firstData);
            using MemoryStream secondStream = new MemoryStream(secondData);
            // Act
            using Stream concatenatedStream = firstStream.Concat(secondStream);
            byte[] result = concatenatedStream.ReadToEnd();
            // Assert
            byte[] expected = new byte[firstData.Length + secondData.Length];
            Array.Copy(firstData, expected, firstData.Length);
            Array.Copy(secondData, 0, expected, firstData.Length, secondData.Length);
            result.Should().BeEquivalentTo(expected);
        }

        /// <summary>
        /// A private non-seekable stream implementation used for testing purposes.
        /// </summary>
        private class NonSeekableStream : MemoryStream
        {
            public NonSeekableStream(byte[] buffer)
                : base(buffer)
            {
            }

            public override bool CanSeek => false;
        }
    }
}
