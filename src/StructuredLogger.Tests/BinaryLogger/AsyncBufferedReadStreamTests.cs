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
        private const int DefaultBufferSize = 4096;

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when a null stream is provided.
        /// </summary>
        [Fact]
        public void Constructor_NullStream_ThrowsArgumentNullException()
        {
            // Arrange
            Stream nullStream = null;

            // Act & Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new AsyncBufferedReadStream(nullStream));
            Assert.Equal("stream", exception.ParamName);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentOutOfRangeException when a non-positive buffer size is provided.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public void Constructor_NonPositiveBufferSize_ThrowsArgumentOutOfRangeException(int invalidBufferSize)
        {
            // Arrange
            using MemoryStream ms = new MemoryStream();

            // Act & Assert
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => new AsyncBufferedReadStream(ms, invalidBufferSize));
            Assert.Equal("bufferSize", exception.ParamName);
        }

        /// <summary>
        /// Tests that the constructor creates an instance when valid parameters are provided.
        /// </summary>
        [Fact]
        public void Constructor_ValidParameters_CreatesInstanceSuccessfully()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream();

            // Act
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);
            
            // Assert
            Assert.True(streamWrapper.CanRead);
            Assert.Equal(ms.Length, streamWrapper.Length);
        }

        /// <summary>
        /// Tests that the Read method throws an ArgumentNullException when a null array is passed.
        /// </summary>
        [Fact]
        public void Read_NullArray_ThrowsArgumentNullException()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream(new byte[] { 1, 2, 3 });
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);

            // Act & Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => streamWrapper.Read(null, 0, 1));
            Assert.Equal("array", exception.ParamName);
        }

        /// <summary>
        /// Tests that the Read method throws an ArgumentOutOfRangeException when a negative offset is passed.
        /// </summary>
        [Fact]
        public void Read_NegativeOffset_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream(new byte[] { 1, 2, 3 });
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);
            byte[] buffer = new byte[3];

            // Act & Assert
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => streamWrapper.Read(buffer, -1, 1));
            Assert.Equal("offset", exception.ParamName);
        }

        /// <summary>
        /// Tests that the Read method throws an ArgumentOutOfRangeException when a negative count is passed.
        /// </summary>
        [Fact]
        public void Read_NegativeCount_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream(new byte[] { 1, 2, 3 });
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);
            byte[] buffer = new byte[3];

            // Act & Assert
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => streamWrapper.Read(buffer, 0, -1));
            Assert.Equal("count", exception.ParamName);
        }

        /// <summary>
        /// Tests that the Read method throws an ArgumentException when the buffer length minus offset is less than count.
        /// </summary>
        [Fact]
        public void Read_ArrayTooSmall_ThrowsArgumentException()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream(new byte[] { 1, 2, 3 });
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);
            byte[] buffer = new byte[2];

            // Act & Assert
            Assert.Throws<ArgumentException>(() => streamWrapper.Read(buffer, 0, 3));
        }

        /// <summary>
        /// Tests that the Read method returns zero when count is zero.
        /// </summary>
        [Fact]
        public void Read_ZeroCount_ReturnsZero()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream(new byte[] { 1, 2, 3 });
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);
            byte[] buffer = new byte[3];

            // Act
            int result = streamWrapper.Read(buffer, 0, 0);

            // Assert
            Assert.Equal(0, result);
        }

        /// <summary>
        /// Tests that the Read method reads the expected bytes from the underlying stream with a single refill.
        /// </summary>
        [Fact]
        public void Read_MemoryStream_ReturnsExpectedBytes()
        {
            // Arrange
            byte[] inputData = new byte[] { 10, 20, 30, 40, 50 };
            using MemoryStream ms = new MemoryStream(inputData);
            // Set a small buffer size to force refill.
            int customBufferSize = 3;
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms, customBufferSize);
            byte[] output = new byte[inputData.Length];

            // Act
            int totalRead = streamWrapper.Read(output, 0, output.Length);

            // Assert
            Assert.Equal(inputData.Length, totalRead);
            Assert.Equal(inputData, output);
        }

        /// <summary>
        /// Tests that the ReadByte method returns the next byte from the stream.
        /// </summary>
        [Fact]
        public void ReadByte_ReturnsNextByte()
        {
            // Arrange
            byte[] inputData = new byte[] { 99, 100 };
            using MemoryStream ms = new MemoryStream(inputData);
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);

            // Act
            int firstByte = streamWrapper.ReadByte();
            int secondByte = streamWrapper.ReadByte();

            // Assert
            Assert.Equal(99, firstByte);
            Assert.Equal(100, secondByte);
        }

        /// <summary>
        /// Tests that the ReadByte method returns -1 when no more bytes are available.
        /// </summary>
        [Fact]
        public void ReadByte_EndOfStream_ReturnsMinusOne()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream(new byte[0]);
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);

            // Act
            int result = streamWrapper.ReadByte();

            // Assert
            Assert.Equal(-1, result);
        }

        /// <summary>
        /// Tests that the property getters (CanRead, CanWrite, CanSeek, Length) reflect the underlying stream's properties.
        /// </summary>
        [Fact]
        public void Properties_ReflectUnderlyingStreamProperties()
        {
            // Arrange
            byte[] inputData = new byte[] { 1, 2, 3, 4 };
            using MemoryStream ms = new MemoryStream(inputData);
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);

            // Act & Assert
            Assert.True(streamWrapper.CanRead);
            Assert.Equal(ms.CanWrite, streamWrapper.CanWrite);
            Assert.Equal(ms.CanSeek, streamWrapper.CanSeek);
            Assert.Equal(ms.Length, streamWrapper.Length);
        }

        /// <summary>
        /// Tests that getting the Position property returns the correct computed value.
        /// </summary>
        [Fact]
        public void Position_Getter_ReturnsComputedPosition()
        {
            // Arrange
            byte[] inputData = new byte[] { 1, 2, 3, 4 };
            using MemoryStream ms = new MemoryStream(inputData);
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);
            
            // Act
            // Initially, no bytes have been read, thus readPosition and readLength are both 0.
            long position = streamWrapper.Position;

            // Assert
            Assert.Equal(ms.Position, position);
        }

        /// <summary>
        /// Tests that setting the Position property throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Position_Setter_ThrowsNotImplementedException()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream();
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => streamWrapper.Position = 10);
        }

        /// <summary>
        /// Tests that the Flush method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Flush_ThrowsNotImplementedException()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream();
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => streamWrapper.Flush());
        }

        /// <summary>
        /// Tests that the Seek method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Seek_ThrowsNotImplementedException()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream();
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => streamWrapper.Seek(0, SeekOrigin.Begin));
        }

        /// <summary>
        /// Tests that the SetLength method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void SetLength_ThrowsNotImplementedException()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream();
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => streamWrapper.SetLength(100));
        }

        /// <summary>
        /// Tests that the Write method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Write_ThrowsNotImplementedException()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream();
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);
            byte[] buffer = new byte[10];

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => streamWrapper.Write(buffer, 0, buffer.Length));
        }

        /// <summary>
        /// Tests that calling Dispose triggers Flush (which is not implemented) and subsequently throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Dispose_CallsFlushAndThrowsNotImplementedException()
        {
            // Arrange
            using MemoryStream ms = new MemoryStream();
            AsyncBufferedReadStream streamWrapper = new AsyncBufferedReadStream(ms);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => streamWrapper.Dispose());
        }
    }
}
