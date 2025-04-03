using FluentAssertions;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="AsyncBufferedReadStream"/> class.
    /// </summary>
    public class AsyncBufferedReadStreamTests
    {
        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when a null stream is provided.
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
        /// Tests that the constructor throws an ArgumentOutOfRangeException when an invalid buffer size is provided.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_InvalidBufferSize_ThrowsArgumentOutOfRangeException(int invalidBufferSize)
        {
            // Arrange
            using var memoryStream = new MemoryStream();

            // Act
            Action act = () => new AsyncBufferedReadStream(memoryStream, invalidBufferSize);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*bufferSize*");
        }

        /// <summary>
        /// Tests that Read throws an ArgumentNullException when a null array is provided.
        /// </summary>
        [Fact]
        public void Read_NullArray_ThrowsArgumentNullException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);
            
            // Act
            Action act = () => bufferedStream.Read(null, 0, 1);
            
            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*array*");
        }

        /// <summary>
        /// Tests that Read throws an ArgumentOutOfRangeException when a negative offset is provided.
        /// </summary>
        [Fact]
        public void Read_NegativeOffset_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[5];
            
            // Act
            Action act = () => bufferedStream.Read(buffer, -1, 2);
            
            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*offset*");
        }

        /// <summary>
        /// Tests that Read throws an ArgumentOutOfRangeException when a negative count is provided.
        /// </summary>
        [Fact]
        public void Read_NegativeCount_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[5];
            
            // Act
            Action act = () => bufferedStream.Read(buffer, 0, -1);
            
            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*count*");
        }

        /// <summary>
        /// Tests that Read throws an ArgumentException when the buffer length is insufficient given the offset and count.
        /// </summary>
        [Fact]
        public void Read_ArrayTooShort_ThrowsArgumentException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[3];
            
            // Act
            Action act = () => bufferedStream.Read(buffer, 2, 2);
            
            // Assert
            act.Should().Throw<ArgumentException>();
        }

        /// <summary>
        /// Tests that Read returns zero when count is zero.
        /// </summary>
        [Fact]
        public void Read_ZeroCount_ReturnsZero()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[5];

            // Act
            int bytesRead = bufferedStream.Read(buffer, 0, 0);

            // Assert
            bytesRead.Should().Be(0);
        }

        /// <summary>
        /// Tests that Read successfully reads the expected data from the underlying stream.
        /// </summary>
        [Fact]
        public void Read_ValidRead_ReturnsCorrectData()
        {
            // Arrange
            byte[] sourceData = new byte[100];
            for (int i = 0; i < sourceData.Length; i++)
            {
                sourceData[i] = (byte)(i % 256);
            }
            using var memoryStream = new MemoryStream(sourceData);
            // Use a smaller buffer size to force multiple refills.
            var bufferedStream = new AsyncBufferedReadStream(memoryStream, 20);
            byte[] buffer = new byte[100];

            // Act
            int totalRead = bufferedStream.Read(buffer, 0, 100);

            // Assert
            totalRead.Should().Be(sourceData.Length);
            buffer.Should().Equal(sourceData);
        }

        /// <summary>
        /// Tests that ReadByte returns the correct byte when data is available.
        /// </summary>
        [Fact]
        public void ReadByte_WithData_ReturnsCorrectByte()
        {
            // Arrange
            byte[] sourceData = new byte[] { 10, 20, 30 };
            using var memoryStream = new MemoryStream(sourceData);
            var bufferedStream = new AsyncBufferedReadStream(memoryStream, 2);

            // Act
            int firstByte = bufferedStream.ReadByte();
            int secondByte = bufferedStream.ReadByte();
            int thirdByte = bufferedStream.ReadByte();

            // Assert
            firstByte.Should().Be(10);
            secondByte.Should().Be(20);
            thirdByte.Should().Be(30);
        }

        /// <summary>
        /// Tests that ReadByte returns -1 when the end of the stream is reached.
        /// </summary>
        [Fact]
        public void ReadByte_EndOfStream_ReturnsMinusOne()
        {
            // Arrange
            byte[] sourceData = new byte[0];
            using var memoryStream = new MemoryStream(sourceData);
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            int result = bufferedStream.ReadByte();

            // Assert
            result.Should().Be(-1);
        }

        /// <summary>
        /// Tests that the CanRead property returns the same value as the underlying stream's CanRead.
        /// </summary>
        [Fact]
        public void CanRead_ReturnsUnderlyingStreamCanRead()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            bool canRead = bufferedStream.CanRead;

            // Assert
            canRead.Should().Be(memoryStream.CanRead);
        }

        /// <summary>
        /// Tests that the CanWrite property returns the same value as the underlying stream's CanWrite.
        /// </summary>
        [Fact]
        public void CanWrite_ReturnsUnderlyingStreamCanWrite()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            bool canWrite = bufferedStream.CanWrite;

            // Assert
            canWrite.Should().Be(memoryStream.CanWrite);
        }

        /// <summary>
        /// Tests that the CanSeek property returns the same value as the underlying stream's CanSeek.
        /// </summary>
        [Fact]
        public void CanSeek_ReturnsUnderlyingStreamCanSeek()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            bool canSeek = bufferedStream.CanSeek;

            // Assert
            canSeek.Should().Be(memoryStream.CanSeek);
        }

        /// <summary>
        /// Tests that the Length property returns the same value as the underlying stream's Length.
        /// </summary>
        [Fact]
        public void Length_ReturnsUnderlyingStreamLength()
        {
            // Arrange
            byte[] data = new byte[50];
            using var memoryStream = new MemoryStream(data);
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            long length = bufferedStream.Length;

            // Assert
            length.Should().Be(memoryStream.Length);
        }

        /// <summary>
        /// Tests that the Position getter returns the correctly computed position.
        /// </summary>
        [Fact]
        public void Position_Get_ReturnsCorrectComputedPosition()
        {
            // Arrange
            byte[] sourceData = new byte[10];
            for (int i = 0; i < sourceData.Length; i++)
            {
                sourceData[i] = (byte)i;
            }
            using var memoryStream = new MemoryStream(sourceData);
            var bufferedStream = new AsyncBufferedReadStream(memoryStream, 4);
            byte[] buffer = new byte[4];
            // Read 4 bytes to trigger a buffer fill.
            int bytesRead = bufferedStream.Read(buffer, 0, 4);
            // Expected computed position = underlying stream position + readPosition - readLength.
            // After reading, readPosition should equal readLength, hence this equals memoryStream.Position.
            long expectedPosition = memoryStream.Position;

            // Act
            long actualPosition = bufferedStream.Position;

            // Assert
            actualPosition.Should().Be(expectedPosition);
        }

        /// <summary>
        /// Tests that Flush throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Flush_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            Action act = () => bufferedStream.Flush();

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that Seek throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Seek_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            Action act = () => bufferedStream.Seek(0, SeekOrigin.Begin);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that SetLength throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void SetLength_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            Action act = () => bufferedStream.SetLength(10);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that Write throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Write_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[5];

            // Act
            Action act = () => bufferedStream.Write(buffer, 0, 5);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that setting the Position property throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Position_Set_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            var bufferedStream = new AsyncBufferedReadStream(memoryStream);

            // Act
            Action act = () => bufferedStream.Position = 5;

            // Assert
            act.Should().Throw<NotImplementedException>();
        }
    }
}
