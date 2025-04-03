using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotUtils.StreamUtils;
using FluentAssertions;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ConcatenatedReadStream"/> class.
    /// </summary>
    public class ConcatenatedReadStreamTests
    {
        /// <summary>
        /// A dummy non-readable stream used for testing.
        /// </summary>
        private class NonReadableStream : Stream
        {
            public override bool CanRead => false;
            public override bool CanSeek => false;
            public override bool CanWrite => false;
            public override long Length => 0;
            public override long Position { get => 0; set { } }
            public override void Flush() { }
            public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
            public override void SetLength(long value) => throw new NotSupportedException();
            public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        }

        /// <summary>
        /// Creates a MemoryStream initialized with the given data.
        /// </summary>
        /// <param name="data">The data to initialize the stream with.</param>
        /// <returns>A readable MemoryStream.</returns>
        private static MemoryStream CreateMemoryStream(byte[] data)
        {
            return new MemoryStream(data, writable: false);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentException when any provided stream is not readable.
        /// </summary>
        [Fact]
        public void Constructor_NonReadableStream_ThrowsArgumentException()
        {
            // Arrange
            var nonReadable = new NonReadableStream();
            var readable = CreateMemoryStream(new byte[] { 1, 2, 3 });

            // Act
            Action action = () => new ConcatenatedReadStream(new Stream[] { readable, nonReadable });

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("All streams must be readable*")
                .And.ParamName.Should().Be("streams");
        }

        /// <summary>
        /// Tests that the constructor flattens nested ConcatenatedReadStream instances.
        /// </summary>
        [Fact]
        public void Constructor_NestedConcatenatedReadStream_FlattensStreams()
        {
            // Arrange
            var stream1 = CreateMemoryStream(new byte[] { 1, 2 });
            var stream2 = CreateMemoryStream(new byte[] { 3, 4 });
            var nested = new ConcatenatedReadStream(new[] { stream1, stream2 });

            var stream3 = CreateMemoryStream(new byte[] { 5, 6 });
            
            // Act
            // Pass nested stream and another stream
            var concatenated = new ConcatenatedReadStream(nested, stream3);
            byte[] buffer = new byte[6];
            int bytesRead = concatenated.Read(buffer, 0, buffer.Length);

            // Assert
            bytesRead.Should().Be(6);
            buffer.Should().Equal(new byte[] { 1, 2, 3, 4, 5, 6 });
        }

        /// <summary>
        /// Tests that Flush() always throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void Flush_Always_ThrowsNotSupportedException()
        {
            // Arrange
            var stream = CreateMemoryStream(new byte[] { 1, 2, 3 });
            var concatenated = new ConcatenatedReadStream(stream);

            // Act
            Action act = () => concatenated.Flush();

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("ConcatenatedReadStream is forward-only read-only");
        }

        /// <summary>
        /// Tests the Read(byte[], int, int) method for happy path, reading across multiple streams.
        /// </summary>
        [Fact]
        public void Read_WithMultipleStreams_ReturnsConcatenatedData()
        {
            // Arrange
            var data1 = new byte[] { 10, 20 };
            var data2 = new byte[] { 30, 40, 50 };
            var stream1 = CreateMemoryStream(data1);
            var stream2 = CreateMemoryStream(data2);
            var concatenated = new ConcatenatedReadStream(new[] { stream1, stream2 });
            byte[] buffer = new byte[10];

            // Act
            int bytesRead = concatenated.Read(buffer, 0, buffer.Length);

            // Assert
            bytesRead.Should().Be(5);
            buffer.Take(5).Should().Equal(new byte[] { 10, 20, 30, 40, 50 });
        }

        /// <summary>
        /// Tests the Read(byte[], int, int) method when count is zero.
        /// </summary>
        [Fact]
        public void Read_ZeroCount_ReturnsZero()
        {
            // Arrange
            var stream = CreateMemoryStream(new byte[] { 1, 2, 3 });
            var concatenated = new ConcatenatedReadStream(stream);
            byte[] buffer = new byte[10];

            // Act
            int bytesRead = concatenated.Read(buffer, 0, 0);

            // Assert
            bytesRead.Should().Be(0);
        }

        /// <summary>
        /// Tests the ReadByte() method to ensure it returns bytes in order and -1 when finished.
        /// </summary>
        [Fact]
        public void ReadByte_WithMultipleStreams_ReturnsConcatenatedDataAndThenMinusOne()
        {
            // Arrange
            var data1 = new byte[] { 100 };
            var data2 = new byte[] { 200 };
            var stream1 = CreateMemoryStream(data1);
            var stream2 = CreateMemoryStream(data2);
            var concatenated = new ConcatenatedReadStream(new[] { stream1, stream2 });

            // Act & Assert
            concatenated.ReadByte().Should().Be(100);
            concatenated.ReadByte().Should().Be(200);
            concatenated.ReadByte().Should().Be(-1);
        }

        /// <summary>
        /// Tests the asynchronous ReadAsync(byte[], int, int, CancellationToken) method for happy path.
        /// </summary>
        [Fact]
        public async Task ReadAsync_WithMultipleStreams_ReturnsConcatenatedData()
        {
            // Arrange
            var data1 = new byte[] { 5, 6 };
            var data2 = new byte[] { 7, 8, 9 };
            var stream1 = CreateMemoryStream(data1);
            var stream2 = CreateMemoryStream(data2);
            var concatenated = new ConcatenatedReadStream(new[] { stream1, stream2 });
            byte[] buffer = new byte[10];
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            int bytesRead = await concatenated.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);

            // Assert
            bytesRead.Should().Be(5);
            buffer.Take(5).Should().Equal(new byte[] { 5, 6, 7, 8, 9 });
        }

#if NET
        /// <summary>
        /// Tests the Read(Span<byte>) method for happy path.
        /// </summary>
        [Fact]
        public void ReadSpan_WithMultipleStreams_ReturnsConcatenatedData()
        {
            // Arrange
            var data1 = new byte[] { 11, 12 };
            var data2 = new byte[] { 13, 14, 15, 16 };
            var stream1 = CreateMemoryStream(data1);
            var stream2 = CreateMemoryStream(data2);
            var concatenated = new ConcatenatedReadStream(new[] { stream1, stream2 });
            Span<byte> buffer = new byte[10];

            // Act
            int bytesRead = concatenated.Read(buffer);

            // Assert
            bytesRead.Should().Be(6);
            buffer.Slice(0, 6).ToArray().Should().Equal(new byte[] { 11, 12, 13, 14, 15, 16 });
        }

        /// <summary>
        /// Tests the asynchronous ReadAsync(Memory<byte>, CancellationToken) method for happy path.
        /// </summary>
        [Fact]
        public async Task ReadAsyncMemory_WithMultipleStreams_ReturnsConcatenatedData()
        {
            // Arrange
            var data1 = new byte[] { 21, 22, 23 };
            var data2 = new byte[] { 24, 25 };
            var stream1 = CreateMemoryStream(data1);
            var stream2 = CreateMemoryStream(data2);
            var concatenated = new ConcatenatedReadStream(new[] { stream1, stream2 });
            Memory<byte> buffer = new byte[10];
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            int bytesRead = await concatenated.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);

            // Assert
            bytesRead.Should().Be(5);
            buffer.Slice(0, 5).ToArray().Should().Equal(new byte[] { 21, 22, 23, 24, 25 });
        }
#endif

        /// <summary>
        /// Tests that the Seek method always throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void Seek_Always_ThrowsNotSupportedException()
        {
            // Arrange
            var stream = CreateMemoryStream(new byte[] { 1, 2, 3 });
            var concatenated = new ConcatenatedReadStream(stream);

            // Act
            Action act = () => concatenated.Seek(0, SeekOrigin.Begin);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("ConcatenatedReadStream is forward-only read-only");
        }

        /// <summary>
        /// Tests that the SetLength method always throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void SetLength_Always_ThrowsNotSupportedException()
        {
            // Arrange
            var stream = CreateMemoryStream(new byte[] { 1, 2, 3 });
            var concatenated = new ConcatenatedReadStream(stream);

            // Act
            Action act = () => concatenated.SetLength(10);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("ConcatenatedReadStream is forward-only read-only");
        }

        /// <summary>
        /// Tests that the Write method always throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void Write_Always_ThrowsNotSupportedException()
        {
            // Arrange
            var stream = CreateMemoryStream(new byte[] { 1, 2, 3 });
            var concatenated = new ConcatenatedReadStream(stream);
            byte[] buffer = new byte[] { 1, 2, 3 };

            // Act
            Action act = () => concatenated.Write(buffer, 0, buffer.Length);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("ConcatenatedReadStream is forward-only read-only");
        }

        /// <summary>
        /// Tests the property values of the stream.
        /// </summary>
        [Fact]
        public void Properties_ReturnExpectedValues()
        {
            // Arrange
            var data1 = new byte[] { 1, 2, 3 };
            var data2 = new byte[] { 4, 5 };
            var stream1 = CreateMemoryStream(data1);
            var stream2 = CreateMemoryStream(data2);
            var concatenated = new ConcatenatedReadStream(new[] { stream1, stream2 });

            // Act
            bool canRead = concatenated.CanRead;
            bool canSeek = concatenated.CanSeek;
            bool canWrite = concatenated.CanWrite;
            long length = concatenated.Length;

            // Assert
            canRead.Should().BeTrue();
            canSeek.Should().BeFalse();
            canWrite.Should().BeFalse();
            length.Should().Be(stream1.Length + stream2.Length);
        }

        /// <summary>
        /// Tests that setting the Position property throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void PositionSetter_ThrowsNotSupportedException()
        {
            // Arrange
            var stream = CreateMemoryStream(new byte[] { 1, 2, 3 });
            var concatenated = new ConcatenatedReadStream(stream);

            // Act
            Action act = () => concatenated.Position = 10;

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("ConcatenatedReadStream is forward-only read-only");
        }

        /// <summary>
        /// Tests that reading beyond the available data returns only the available bytes.
        /// </summary>
        [Fact]
        public void Read_RequestMoreThanAvailable_ReturnsOnlyAvailableBytes()
        {
            // Arrange
            var data = new byte[] { 99, 100 };
            var stream = CreateMemoryStream(data);
            var concatenated = new ConcatenatedReadStream(stream);
            byte[] buffer = new byte[10];

            // Act
            int bytesRead = concatenated.Read(buffer, 0, buffer.Length);

            // Assert
            bytesRead.Should().Be(data.Length);
            buffer.Take(data.Length).Should().Equal(data);
        }
    }
}
