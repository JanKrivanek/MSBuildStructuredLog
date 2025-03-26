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
        private readonly byte[] sampleData = new byte[] { 1, 2, 3, 4, 5, 6, 7 };

        #region Constructor Tests

        /// <summary>
        /// Tests that the constructor throws ArgumentNullException when a null stream is passed.
        /// </summary>
        [Fact]
        public void Constructor_NullStream_ThrowsArgumentNullException()
        {
            // Arrange
            Stream nullStream = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AsyncBufferedReadStream(nullStream));
            Assert.Throws<ArgumentNullException>(() => new AsyncBufferedReadStream(nullStream, 1024));
        }

        /// <summary>
        /// Tests that the constructor throws ArgumentOutOfRangeException when bufferSize is less than or equal to zero.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Constructor_InvalidBufferSize_ThrowsArgumentOutOfRangeException(int invalidBufferSize)
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new AsyncBufferedReadStream(memoryStream, invalidBufferSize));
        }
        #endregion

        #region Read Method Tests

        /// <summary>
        /// Tests that Read throws ArgumentNullException when the provided array is null.
        /// </summary>
        [Fact]
        public void Read_NullArray_ThrowsArgumentNullException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            byte[] nullArray = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => asyncStream.Read(nullArray, 0, 1));
        }

        /// <summary>
        /// Tests that Read throws ArgumentOutOfRangeException when offset is negative.
        /// </summary>
        [Fact]
        public void Read_NegativeOffset_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[10];

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => asyncStream.Read(buffer, -1, 5));
        }

        /// <summary>
        /// Tests that Read throws ArgumentOutOfRangeException when count is negative.
        /// </summary>
        [Fact]
        public void Read_NegativeCount_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[10];

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => asyncStream.Read(buffer, 0, -1));
        }

        /// <summary>
        /// Tests that Read throws ArgumentException when the provided array does not have enough space.
        /// </summary>
        [Fact]
        public void Read_ArrayTooSmallForCount_ThrowsArgumentException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[5];

            // Act & Assert
            Assert.Throws<ArgumentException>(() => asyncStream.Read(buffer, 2, 4));
        }

        /// <summary>
        /// Tests that Read returns zero when the count provided is zero.
        /// </summary>
        [Fact]
        public void Read_ZeroCount_ReturnsZero()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[10];

            // Act
            int bytesRead = asyncStream.Read(buffer, 0, 0);

            // Assert
            Assert.Equal(0, bytesRead);
        }

        /// <summary>
        /// Tests that Read reads the data correctly across multiple internal buffer refills.
        /// This test uses a reduced buffer size to force multiple buffer refills.
        /// </summary>
        [Fact]
        public void Read_ReadsDataCorrectly_AcrossMultipleRefills()
        {
            // Arrange
            byte[] testData = new byte[] { 10, 20, 30, 40, 50, 60, 70 };
            using var memoryStream = new MemoryStream(testData);
            int smallBufferSize = 3;
            using var asyncStream = new AsyncBufferedReadStream(memoryStream, smallBufferSize);
            byte[] buffer = new byte[testData.Length];

            // Act
            int totalRead = asyncStream.Read(buffer, 0, testData.Length);

            // Assert
            Assert.Equal(testData.Length, totalRead);
            Assert.Equal(testData, buffer);
        }
        #endregion

        #region ReadByte Method Tests

        /// <summary>
        /// Tests that ReadByte returns -1 when the stream is empty.
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
            Assert.Equal(-1, result);
        }

        /// <summary>
        /// Tests that ReadByte returns bytes sequentially.
        /// </summary>
        [Fact]
        public void ReadByte_ReadsBytesInSequence()
        {
            // Arrange
            byte[] testData = new byte[] { 100, 101, 102 };
            using var memoryStream = new MemoryStream(testData);
            // Setting a small buffer size to trigger prefetch logic.
            using var asyncStream = new AsyncBufferedReadStream(memoryStream, bufferSize: 2);

            // Act & Assert
            int firstByte = asyncStream.ReadByte();
            Assert.Equal(100, firstByte);

            int secondByte = asyncStream.ReadByte();
            Assert.Equal(101, secondByte);

            int thirdByte = asyncStream.ReadByte();
            Assert.Equal(102, thirdByte);

            int endByte = asyncStream.ReadByte();
            Assert.Equal(-1, endByte);
        }
        #endregion

        #region Property Tests

        /// <summary>
        /// Tests that CanRead, CanWrite, and CanSeek properties reflect the underlying stream's capabilities.
        /// </summary>
        [Fact]
        public void Properties_ReturnUnderlyingStreamProperties()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);

            // Act & Assert
            Assert.Equal(memoryStream.CanRead, asyncStream.CanRead);
            Assert.Equal(memoryStream.CanWrite, asyncStream.CanWrite);
            Assert.Equal(memoryStream.CanSeek, asyncStream.CanSeek);
        }

        /// <summary>
        /// Tests that Length property returns the length of the underlying stream.
        /// </summary>
        [Fact]
        public void Length_ReturnsUnderlyingStreamLength()
        {
            // Arrange
            byte[] testData = new byte[50];
            using var memoryStream = new MemoryStream(testData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);

            // Act & Assert
            Assert.Equal(memoryStream.Length, asyncStream.Length);
        }

        /// <summary>
        /// Tests that Position property computes the correct logical position after reading some bytes.
        /// The expected position is calculated as: underlying stream's current position + internal readPosition - internal readLength.
        /// </summary>
        [Fact]
        public void Position_ReturnsCorrectValue_AfterReadingSomeBytes()
        {
            // Arrange
            byte[] testData = new byte[] { 1, 2, 3, 4, 5 };
            int smallBufferSize = 3;
            using var memoryStream = new MemoryStream(testData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream, smallBufferSize);

            // Act: Read two bytes; this will force an initial buffer fill.
            int first = asyncStream.ReadByte(); // read 1
            int second = asyncStream.ReadByte(); // read 2

            /* 
             After the first refill:
             - The internal buffer is filled with 'smallBufferSize' bytes.
             - memoryStream.Position becomes 3.
             - readLength equals 3 and readPosition becomes 2 after these two reads.
             So, expected logical Position = 3 + 2 - 3 = 2.
            */
            long expectedPosition = 2;

            // Assert
            Assert.Equal(expectedPosition, asyncStream.Position);
        }
        #endregion

        #region NotImplemented Method Tests

        /// <summary>
        /// Tests that Flush method throws NotImplementedException.
        /// </summary>
        [Fact]
        public void Flush_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => asyncStream.Flush());
        }

        /// <summary>
        /// Tests that Seek method throws NotImplementedException.
        /// </summary>
        [Fact]
        public void Seek_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            long offset = 0;

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => asyncStream.Seek(offset, SeekOrigin.Begin));
        }

        /// <summary>
        /// Tests that SetLength method throws NotImplementedException.
        /// </summary>
        [Fact]
        public void SetLength_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            long newLength = 100;

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => asyncStream.SetLength(newLength));
        }

        /// <summary>
        /// Tests that Write method throws NotImplementedException.
        /// </summary>
        [Fact]
        public void Write_ThrowsNotImplementedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(sampleData);
            using var asyncStream = new AsyncBufferedReadStream(memoryStream);
            byte[] buffer = new byte[5];

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => asyncStream.Write(buffer, 0, buffer.Length));
        }
        #endregion
    }
}
