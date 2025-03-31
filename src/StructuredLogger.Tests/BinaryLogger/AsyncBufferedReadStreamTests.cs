using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace System.IO.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="AsyncBufferedReadStream"/> class.
    /// </summary>
    public class AsyncBufferedReadStreamTests
    {
        private readonly byte[] sampleData = new byte[] { 1, 2, 3, 4, 5 };

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provided stream is null.
        /// </summary>
        [Fact]
        public void Constructor_NullStream_ThrowsArgumentNullException()
        {
            // Arrange
            Stream nullStream = null;

            // Act
            Action act = () => new AsyncBufferedReadStream(nullStream);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*stream*");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentOutOfRangeException when the bufferSize is less than or equal to zero.
        /// </summary>
        /// <param name="bufferSize">The invalid buffer size.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_NonPositiveBufferSize_ThrowsArgumentOutOfRangeException(int bufferSize)
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);

            // Act
            Action act = () => new AsyncBufferedReadStream(memoryStream, bufferSize);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*bufferSize*");
        }

        /// <summary>
        /// Tests that Read throws an ArgumentNullException when the array parameter is null.
        /// </summary>
        [Fact]
        public void Read_NullArray_ThrowsArgumentNullException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            byte[] nullArray = null;
            int offset = 0;
            int count = 1;

            // Act
            Action act = () => asyncStream.Read(nullArray, offset, count);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*array*");
        }

        /// <summary>
        /// Tests that Read throws an ArgumentOutOfRangeException when the offset is negative.
        /// </summary>
        [Fact]
        public void Read_NegativeOffset_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[5];
            int offset = -1;
            int count = 1;

            // Act
            Action act = () => asyncStream.Read(buffer, offset, count);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*offset*");
        }

        /// <summary>
        /// Tests that Read throws an ArgumentOutOfRangeException when the count is negative.
        /// </summary>
        [Fact]
        public void Read_NegativeCount_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[5];
            int offset = 0;
            int count = -1;

            // Act
            Action act = () => asyncStream.Read(buffer, offset, count);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*count*");
        }

        /// <summary>
        /// Tests that Read throws an ArgumentException when the provided array length minus offset is less than count.
        /// </summary>
        [Fact]
        public void Read_ArrayTooSmall_ThrowsArgumentException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[3];
            int offset = 2;
            int count = 2; // 3 - 2 < 2

            // Act
            Action act = () => asyncStream.Read(buffer, offset, count);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        /// <summary>
        /// Tests that Read returns 0 when the count parameter is 0.
        /// </summary>
        [Fact]
        public void Read_CountZero_ReturnsZero()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[5];
            int offset = 0;
            int count = 0;

            // Act
            int bytesRead = asyncStream.Read(buffer, offset, count);

            // Assert
            bytesRead.Should().Be(0);
        }

        /// <summary>
        /// Tests that Read returns the expected number of bytes and reads the correct data in a valid scenario.
        /// </summary>
        [Fact]
        public void Read_ValidData_ReturnsExpectedBytes()
        {
            // Arrange
            byte[] data = new byte[] { 10, 20, 30, 40, 50 };
            using var memoryStream = new MemoryStream(data);
            // Set buffer size smaller than data length to force multiple refills.
            using var asyncStream = new AsyncBufferedReadStream(memoryStream, 2);
            byte[] buffer = new byte[5];
            int offset = 0;
            int count = 5;

            // Act
            int bytesRead = asyncStream.Read(buffer, offset, count);

            // Assert
            bytesRead.Should().Be(data.Length);
            buffer.Should().Equal(data);
        }

        /// <summary>
        /// Tests that ReadByte returns -1 when called on an empty stream.
        /// </summary>
        [Fact]
        public void ReadByte_EndOfStream_ReturnsMinusOne()
        {
            // Arrange
            using var memoryStream = new MemoryStream(Array.Empty<byte>());
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            int result = asyncStream.ReadByte();

            // Assert
            result.Should().Be(-1);
        }

        /// <summary>
        /// Tests that ReadByte returns the expected byte when data is available.
        /// </summary>
        [Fact]
        public void ReadByte_ValidData_ReturnsExpectedByte()
        {
            // Arrange
            byte[] data = new byte[] { 100, 101 };
            using var memoryStream = new MemoryStream(data);
            // Set buffer size to 1 to force a refill after first byte.
            using var asyncStream = new AsyncBufferedReadStream(memoryStream, 1);

            // Act
            int firstByte = asyncStream.ReadByte();
            int secondByte = asyncStream.ReadByte();
            int thirdByte = asyncStream.ReadByte(); // end of stream

            // Assert
            firstByte.Should().Be(100);
            secondByte.Should().Be(101);
            thirdByte.Should().Be(-1);
        }

        /// <summary>
        /// Tests that the CanRead, CanWrite, and CanSeek properties correctly reflect the underlying stream's capabilities.
        /// </summary>
        [Fact]
        public void Properties_CanRead_CanWrite_CanSeek_ReturnExpectedValues()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);

            // Act & Assert
            asyncStream.CanRead.Should().BeTrue();
            asyncStream.CanWrite.Should().BeTrue();
            asyncStream.CanSeek.Should().BeTrue();
        }

        /// <summary>
        /// Tests that the Length property returns the underlying stream's length.
        /// </summary>
        [Fact]
        public void Length_ReturnsUnderlyingStreamLength()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            long length = asyncStream.Length;

            // Assert
            length.Should().Be(memoryStream.Length);
        }

        /// <summary>
        /// Tests that the Position getter returns the correct calculated position after performing read operations.
        /// </summary>
        [Fact]
        public void Position_Get_ReturnsCorrectValue()
        {
            // Arrange
            byte[] data = new byte[] { 1, 2, 3, 4 };
            // Use a small buffer size to force immediate refill.
            using var memoryStream = new MemoryStream(data);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream, 2);

            // Act
            // The first read will trigger a prefetch of 2 bytes.
            int firstByte = asyncStream.ReadByte();
            // At this point, internal readPosition == 1 and readLength == 2.
            // Underlying stream has advanced by 2 bytes due to prefetch.
            long calculatedPosition = asyncStream.Position;

            // Expected position = underlying stream position + readPosition - readLength
            // = 2 + 1 - 2 = 1.
            // Assert
            calculatedPosition.Should().Be(1);
        }

        /// <summary>
        /// Tests that Flush method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Flush_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            Action act = () => asyncStream.Flush();

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that Seek method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Seek_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            long offset = 0;
            SeekOrigin origin = SeekOrigin.Begin;

            // Act
            Action act = () => asyncStream.Seek(offset, origin);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that SetLength method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void SetLength_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            long newLength = 10;

            // Act
            Action act = () => asyncStream.SetLength(newLength);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that Write method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Write_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[3];
            int offset = 0;
            int count = 3;

            // Act
            Action act = () => asyncStream.Write(buffer, offset, count);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that disposing the AsyncBufferedReadStream disposes the underlying stream and sets internal fields to null.
        /// </summary>
        [Fact]
        public void Dispose_WhenCalled_SetsCanReadToFalse()
        {
            // Arrange
            var memoryStream = new MemoryStream(sampleData);
            var asyncStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            asyncStream.Dispose();

            // Assert
            // Since the underlying stream is set to null upon disposal, CanRead should return false.
            asyncStream.CanRead.Should().BeFalse();
        }
    }
}
