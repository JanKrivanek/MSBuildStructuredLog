using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotUtils.StreamUtils;
using Moq;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Provides a non-readable stream for testing purposes.
    /// </summary>
    internal class UnreadableStream : Stream
    {
        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => 0;
        public override long Position { get => 0; set { } }
        public override void Flush() => throw new NotSupportedException();
        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }

    /// <summary>
    /// Unit tests for the <see cref="ConcatenatedReadStream"/> class.
    /// </summary>
    public class ConcatenatedReadStreamTests
    {
        private readonly byte[] _sampleData1 = { 1, 2, 3 };
        private readonly byte[] _sampleData2 = { 4, 5 };
        private readonly byte[] _sampleData3 = { 6, 7, 8, 9 };

        /// <summary>
        /// Tests that the constructor using IEnumerable throws an exception when any stream is not readable.
        /// </summary>
        [Fact]
        public void Ctor_Enumerable_WithNonReadableStream_ThrowsArgumentException()
        {
            // Arrange
            IEnumerable<Stream> streams = new List<Stream>
            {
                new MemoryStream(_sampleData1),
                new UnreadableStream()
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ConcatenatedReadStream(streams));
        }

        /// <summary>
        /// Tests that the constructor using params throws an exception when any stream is not readable.
        /// </summary>
        [Fact]
        public void Ctor_Params_WithNonReadableStream_ThrowsArgumentException()
        {
            // Arrange
            Stream stream1 = new MemoryStream(_sampleData1);
            Stream stream2 = new UnreadableStream();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ConcatenatedReadStream(stream1, stream2));
        }

        /// <summary>
        /// Tests that nested ConcatenatedReadStream instances are flattened correctly by reading the concatenated result.
        /// </summary>
        [Fact]
        public void Ctor_NestedConcatenatedReadStream_FlattensStreamsCorrectly()
        {
            // Arrange
            MemoryStream ms1 = new MemoryStream(_sampleData1);
            MemoryStream ms2 = new MemoryStream(_sampleData2);
            ConcatenatedReadStream innerStream = new ConcatenatedReadStream(ms1, ms2);
            MemoryStream ms3 = new MemoryStream(_sampleData3);

            // Act
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(innerStream, ms3))
            {
                byte[] allBytes = new byte[_sampleData1.Length + _sampleData2.Length + _sampleData3.Length];
                int bytesRead = concatenatedStream.Read(allBytes, 0, allBytes.Length);

                // Assert
                Assert.Equal(allBytes.Length, bytesRead);
                byte[] expected = _sampleData1.Concat(_sampleData2).Concat(_sampleData3).ToArray();
                Assert.Equal(expected, allBytes);
            }
        }

        /// <summary>
        /// Tests that Flush always throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void Flush_AnyCall_ThrowsNotSupportedException()
        {
            // Arrange
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(new MemoryStream(_sampleData1)))
            {
                // Act & Assert
                Assert.Throws<NotSupportedException>(() => concatenatedStream.Flush());
            }
        }

        /// <summary>
        /// Tests the Read method to ensure it correctly reads bytes from multiple streams.
        /// </summary>
        [Fact]
        public void Read_HappyPath_ReadsFromMultipleStreams()
        {
            // Arrange
            MemoryStream ms1 = new MemoryStream(_sampleData1);
            MemoryStream ms2 = new MemoryStream(_sampleData2);
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(ms1, ms2))
            {
                byte[] buffer = new byte[10];

                // Act
                int bytesRead = concatenatedStream.Read(buffer, 0, buffer.Length);

                // Assert
                byte[] expected = _sampleData1.Concat(_sampleData2).ToArray();
                Assert.Equal(expected.Length, bytesRead);
                Assert.Equal(expected, buffer.Take(bytesRead).ToArray());
            }
        }

        /// <summary>
        /// Tests that calling Read with a count of zero returns 0 without altering the stream.
        /// </summary>
        [Fact]
        public void Read_WithZeroCount_ReturnsZero()
        {
            // Arrange
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(new MemoryStream(_sampleData1)))
            {
                byte[] buffer = new byte[10];

                // Act
                int bytesRead = concatenatedStream.Read(buffer, 0, 0);

                // Assert
                Assert.Equal(0, bytesRead);
            }
        }

        /// <summary>
        /// Tests that ReadByte returns the next byte correctly and advances the position.
        /// </summary>
        [Fact]
        public void ReadByte_HappyPath_ReturnsByte()
        {
            // Arrange
            MemoryStream ms = new MemoryStream(new byte[] { 42 });
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(ms))
            {
                // Act
                int value = concatenatedStream.ReadByte();

                // Assert
                Assert.Equal(42, value);
            }
        }

        /// <summary>
        /// Tests that ReadByte returns -1 when no more bytes are available.
        /// </summary>
        [Fact]
        public void ReadByte_WhenEmpty_ReturnsNegativeOne()
        {
            // Arrange
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(new MemoryStream(Array.Empty<byte>())))
            {
                // Act
                int value = concatenatedStream.ReadByte();

                // Assert
                Assert.Equal(-1, value);
            }
        }

        /// <summary>
        /// Tests the ReadAsync method to ensure it asynchronously reads and concatenates bytes from multiple streams.
        /// </summary>
        [Fact]
        public async Task ReadAsync_HappyPath_ReturnsConcatenatedBytes()
        {
            // Arrange
            MemoryStream ms1 = new MemoryStream(_sampleData1);
            MemoryStream ms2 = new MemoryStream(_sampleData2);
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(ms1, ms2))
            {
                byte[] buffer = new byte[10];

                // Act
                int bytesRead = await concatenatedStream.ReadAsync(buffer, 0, buffer.Length, CancellationToken.None);

                // Assert
                byte[] expected = _sampleData1.Concat(_sampleData2).ToArray();
                Assert.Equal(expected.Length, bytesRead);
                Assert.Equal(expected, buffer.Take(bytesRead).ToArray());
            }
        }

        /// <summary>
        /// Tests that calling ReadAsync with a count of zero returns 0 without altering the stream.
        /// </summary>
        [Fact]
        public async Task ReadAsync_WithZeroCount_ReturnsZero()
        {
            // Arrange
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(new MemoryStream(_sampleData1)))
            {
                byte[] buffer = new byte[10];

                // Act
                int bytesRead = await concatenatedStream.ReadAsync(buffer, 0, 0, CancellationToken.None);

                // Assert
                Assert.Equal(0, bytesRead);
            }
        }

#if NET
        /// <summary>
        /// Tests the Read method with Span<byte> to ensure it correctly reads bytes.
        /// </summary>
        [Fact]
        public void Read_Span_HappyPath_ReadsFromMultipleStreams()
        {
            // Arrange
            MemoryStream ms1 = new MemoryStream(_sampleData1);
            MemoryStream ms2 = new MemoryStream(_sampleData2);
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(ms1, ms2))
            {
                Span<byte> buffer = new byte[10];

                // Act
                int bytesRead = concatenatedStream.Read(buffer);

                // Assert
                byte[] expected = _sampleData1.Concat(_sampleData2).ToArray();
                Assert.Equal(expected.Length, bytesRead);
                Assert.Equal(expected, buffer.Slice(0, bytesRead).ToArray());
            }
        }

        /// <summary>
        /// Tests the ReadAsync method with Memory<byte> to ensure it asynchronously reads bytes.
        /// </summary>
        [Fact]
        public async Task ReadAsync_Memory_HappyPath_ReadsFromMultipleStreams()
        {
            // Arrange
            MemoryStream ms1 = new MemoryStream(_sampleData1);
            MemoryStream ms2 = new MemoryStream(_sampleData2);
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(ms1, ms2))
            {
                Memory<byte> buffer = new byte[10];

                // Act
                int bytesRead = await concatenatedStream.ReadAsync(buffer, CancellationToken.None);

                // Assert
                byte[] expected = _sampleData1.Concat(_sampleData2).ToArray();
                Assert.Equal(expected.Length, bytesRead);
                Assert.Equal(expected, buffer.Slice(0, bytesRead).ToArray());
            }
        }
#endif

        /// <summary>
        /// Tests that calling Seek always throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void Seek_AnyCall_ThrowsNotSupportedException()
        {
            // Arrange
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(new MemoryStream(_sampleData1)))
            {
                // Act & Assert
                Assert.Throws<NotSupportedException>(() => concatenatedStream.Seek(0, SeekOrigin.Begin));
            }
        }

        /// <summary>
        /// Tests that calling SetLength always throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void SetLength_AnyCall_ThrowsNotSupportedException()
        {
            // Arrange
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(new MemoryStream(_sampleData1)))
            {
                // Act & Assert
                Assert.Throws<NotSupportedException>(() => concatenatedStream.SetLength(100));
            }
        }

        /// <summary>
        /// Tests that calling Write always throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void Write_AnyCall_ThrowsNotSupportedException()
        {
            // Arrange
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(new MemoryStream(_sampleData1)))
            {
                byte[] buffer = new byte[10];

                // Act & Assert
                Assert.Throws<NotSupportedException>(() => concatenatedStream.Write(buffer, 0, buffer.Length));
            }
        }

        /// <summary>
        /// Tests that the properties return the correct values.
        /// </summary>
        [Fact]
        public void Properties_WhenInvoked_ReturnExpectedValues()
        {
            // Arrange
            MemoryStream ms = new MemoryStream(_sampleData1);
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(ms))
            {
                // Act & Assert
                Assert.True(concatenatedStream.CanRead);
                Assert.False(concatenatedStream.CanSeek);
                Assert.False(concatenatedStream.CanWrite);
            }
        }

        /// <summary>
        /// Tests that the Length property returns the sum of the lengths of the underlying streams.
        /// </summary>
        [Fact]
        public void Length_ReturnsSumOfUnderlyingStreamsLengths()
        {
            // Arrange
            MemoryStream ms1 = new MemoryStream(_sampleData1);
            MemoryStream ms2 = new MemoryStream(_sampleData2);
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(ms1, ms2))
            {
                // Act
                long expectedLength = ms1.Length + ms2.Length;
                long actualLength = concatenatedStream.Length;

                // Assert
                Assert.Equal(expectedLength, actualLength);
            }
        }

        /// <summary>
        /// Tests that setting the Position property throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void Position_Set_ThrowsNotSupportedException()
        {
            // Arrange
            using (ConcatenatedReadStream concatenatedStream = new ConcatenatedReadStream(new MemoryStream(_sampleData1)))
            {
                // Act & Assert
                Assert.Throws<NotSupportedException>(() => concatenatedStream.Position = 10);
            }
        }
    }
}
