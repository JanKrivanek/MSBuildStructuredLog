using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotUtils.StreamUtils;
using Moq;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TransparentReadStream"/> class.
    /// </summary>
    public class TransparentReadStreamTests
    {
        /// <summary>
        /// A fake stream used for testing that allows controlling behavior and tracking method calls.
        /// </summary>
        private class FakeStream : Stream
        {
            private readonly MemoryStream _innerStream;
            public int FlushCallCount { get; private set; }
            public int FlushAsyncCallCount { get; private set; }
            public bool IsClosed { get; private set; }

            public FakeStream(byte[] data = null)
            {
                _innerStream = data != null ? new MemoryStream(data) : new MemoryStream();
            }

            public override bool CanRead => _innerStream.CanRead;
            public override bool CanSeek => _innerStream.CanSeek;
            public override bool CanWrite => _innerStream.CanWrite;
            public override long Length => _innerStream.Length;
            public override long Position 
            { 
                get => _innerStream.Position; 
                set => _innerStream.Position = value; 
            }

            public override void Flush()
            {
                FlushCallCount++;
                _innerStream.Flush();
            }

            public override Task FlushAsync(CancellationToken cancellationToken)
            {
                FlushAsyncCallCount++;
                return _innerStream.FlushAsync(cancellationToken);
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _innerStream.Read(buffer, offset, count);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return _innerStream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                _innerStream.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                _innerStream.Write(buffer, offset, count);
            }

            public override int ReadByte()
            {
                return _innerStream.ReadByte();
            }

#if NET
            public override int Read(Span<byte> buffer)
            {
                int count = _innerStream.Read(buffer);
                return count;
            }

            public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
            {
                int count = await _innerStream.ReadAsync(buffer, cancellationToken);
                return count;
            }
#endif

            protected override void Dispose(bool disposing)
            {
                IsClosed = true;
                base.Dispose(disposing);
                _innerStream.Dispose();
            }
        }

        /// <summary>
        /// A non-seekable stream that wraps an inner stream, overriding CanSeek.
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
            public override long Length => _innerStream.Length;
            public override long Position 
            { 
                get => _innerStream.Position; 
                set => _innerStream.Position = value; 
            }

            public override void Flush() => _innerStream.Flush();

            public override int Read(byte[] buffer, int offset, int count) => _innerStream.Read(buffer, offset, count);

            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException("Seek is not supported.");

            public override void SetLength(long value) => _innerStream.SetLength(value);

            public override void Write(byte[] buffer, int offset, int count) => _innerStream.Write(buffer, offset, count);

            public override int ReadByte() => _innerStream.ReadByte();

#if NET
            public override int Read(Span<byte> buffer) => _innerStream.Read(buffer);

            public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
            {
                return await _innerStream.ReadAsync(buffer, cancellationToken);
            }
#endif

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                _innerStream.Dispose();
            }
        }

        /// <summary>
        /// A non-readable stream that throws on read operations.
        /// </summary>
        private class NonReadableStream : Stream
        {
            public override bool CanRead => false;
            public override bool CanSeek => false;
            public override bool CanWrite => false;
            public override long Length => throw new NotSupportedException();
            public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
            public override void Flush() => throw new NotSupportedException();
            public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
            public override void SetLength(long value) => throw new NotSupportedException();
            public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        }

        /// <summary>
        /// Tests that EnsureSeekableStream returns the same stream if the input stream is already seekable.
        /// </summary>
        [Fact]
        public void EnsureSeekableStream_StreamIsSeekable_ReturnsSameInstance()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
            
            // Act
            Stream result = TransparentReadStream.EnsureSeekableStream(memoryStream);
            
            // Assert
            Assert.Same(memoryStream, result);
        }

        /// <summary>
        /// Tests that EnsureSeekableStream wraps a non-seekable stream in a TransparentReadStream.
        /// </summary>
        [Fact]
        public void EnsureSeekableStream_StreamIsNonSeekable_ReturnsTransparentReadStreamWrapper()
        {
            // Arrange
            var innerStream = new MemoryStream(new byte[] { 4, 5, 6 });
            using var nonSeekable = new NonSeekableStream(innerStream);
            
            // Act
            Stream result = TransparentReadStream.EnsureSeekableStream(nonSeekable);
            
            // Assert
            Assert.IsType<TransparentReadStream>(result);
        }

        /// <summary>
        /// Tests that EnsureSeekableStream throws an InvalidOperationException when the stream is not readable.
        /// </summary>
        [Fact]
        public void EnsureSeekableStream_StreamIsNonReadable_ThrowsInvalidOperationException()
        {
            // Arrange
            using var nonReadable = new NonReadableStream();
            
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => TransparentReadStream.EnsureSeekableStream(nonReadable));
        }

        /// <summary>
        /// Tests that EnsureTransparentReadStream returns the same instance when the input stream is already a TransparentReadStream.
        /// </summary>
        [Fact]
        public void EnsureTransparentReadStream_InputIsTransparentReadStream_ReturnsSameInstance()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[] { 7, 8, 9 });
            TransparentReadStream transparentStream = new TransparentReadStream(memoryStream);
            
            // Act
            TransparentReadStream result = TransparentReadStream.EnsureTransparentReadStream(transparentStream);
            
            // Assert
            Assert.Same(transparentStream, result);
        }

        /// <summary>
        /// Tests that EnsureTransparentReadStream wraps a readable stream in a TransparentReadStream if it is not already one.
        /// </summary>
        [Fact]
        public void EnsureTransparentReadStream_StreamIsReadable_ReturnsNewTransparentReadStream()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[] { 10, 11, 12 });
            
            // Act
            TransparentReadStream result = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            
            // Assert
            Assert.IsType<TransparentReadStream>(result);
            Assert.NotSame(memoryStream, result);
        }

        /// <summary>
        /// Tests that EnsureTransparentReadStream throws an InvalidOperationException when the stream is not readable.
        /// </summary>
        [Fact]
        public void EnsureTransparentReadStream_StreamIsNonReadable_ThrowsInvalidOperationException()
        {
            // Arrange
            using var nonReadable = new NonReadableStream();
            
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => TransparentReadStream.EnsureTransparentReadStream(nonReadable));
        }

        /// <summary>
        /// Tests the BytesCountAllowedToRead setter and the BytesCountAllowedToReadRemaining getter, ensuring the allowed read count is properly computed.
        /// </summary>
        [Fact]
        public void BytesCountAllowedToRead_PropertyBehavior_WorksAsExpected()
        {
            // Arrange
            byte[] data = new byte[100];
            using var memoryStream = new MemoryStream(data);
            TransparentReadStream trStream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);

            // Initially, no restriction so remaining should be 0
            Assert.Equal(0, trStream.BytesCountAllowedToReadRemaining);
            
            // Act: set allowed bytes count to 10.
            int allowedBytes = 10;
            trStream.BytesCountAllowedToRead = allowedBytes;
            
            // Read 4 bytes from the stream.
            byte[] buffer = new byte[10];
            int bytesRead = trStream.Read(buffer, 0, 4);
            
            // Assert: remaining allowed bytes should be 6.
            Assert.Equal(4, bytesRead);
            Assert.Equal(6, trStream.BytesCountAllowedToReadRemaining);
        }

        /// <summary>
        /// Tests that Flush calls the underlying stream's Flush method.
        /// </summary>
        [Fact]
        public void Flush_CallsUnderlyingStreamFlush()
        {
            // Arrange
            var fakeStream = new FakeStream(new byte[10]);
            TransparentReadStream trStream = TransparentReadStream.EnsureTransparentReadStream(fakeStream);
            
            // Act
            trStream.Flush();
            
            // Assert
            Assert.Equal(1, fakeStream.FlushCallCount);
        }

        /// <summary>
        /// Tests that FlushAsync calls the underlying stream's FlushAsync method.
        /// </summary>
        [Fact]
        public async Task FlushAsync_CallsUnderlyingStreamFlushAsync()
        {
            // Arrange
            var fakeStream = new FakeStream(new byte[10]);
            TransparentReadStream trStream = TransparentReadStream.EnsureTransparentReadStream(fakeStream);
            
            // Act
            await trStream.FlushAsync(CancellationToken.None);
            
            // Assert
            Assert.Equal(1, fakeStream.FlushAsyncCallCount);
        }

        /// <summary>
        /// Tests that Read returns the correct number of bytes, respecting the allowed byte limit if set.
        /// </summary>
        [Fact]
        public void Read_Method_ReadsCorrectCountWithLimit()
        {
            // Arrange
            byte[] data = new byte[20];
            for (int i = 0; i < 20; i++) data[i] = (byte)i;
            using var memoryStream = new MemoryStream(data);
            TransparentReadStream trStream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            
            // Set allowed bytes count to 10
            trStream.BytesCountAllowedToRead = 10;
            byte[] buffer = new byte[15];
            
            // Act: attempt to read 15 bytes, but allowed limit is 10.
            int bytesRead = trStream.Read(buffer, 0, 15);
            
            // Assert
            Assert.Equal(10, bytesRead);
            // Verify that the bytes read are the first 10 bytes of the data.
            for (int i = 0; i < bytesRead; i++)
            {
                Assert.Equal(i, buffer[i]);
            }
        }

        /// <summary>
        /// Tests that ReadByte returns a valid byte within the allowed range and -1 when the limit is exceeded.
        /// </summary>
        [Fact]
        public void ReadByte_ReturnsByteWithinLimit_AndMinusOneAfterLimit()
        {
            // Arrange
            byte[] data = new byte[] { 100, 101 };
            using var memoryStream = new MemoryStream(data);
            TransparentReadStream trStream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);

            // Set allowed bytes to 1.
            trStream.BytesCountAllowedToRead = 1;
            
            // Act & Assert
            int firstByte = trStream.ReadByte();
            Assert.Equal(100, firstByte);
            // Next call should return -1 because limit reached.
            int secondByte = trStream.ReadByte();
            Assert.Equal(-1, secondByte);
        }

        /// <summary>
        /// Tests that ReadAsync returns the correct number of bytes, taking the allowed byte limit into account.
        /// </summary>
        [Fact]
        public async Task ReadAsync_Method_ReadsCorrectCountWithLimit()
        {
            // Arrange
            byte[] data = new byte[30];
            for (int i = 0; i < 30; i++) data[i] = (byte)(i + 1);
            using var memoryStream = new MemoryStream(data);
            TransparentReadStream trStream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            
            // Set allowed bytes count to 15.
            trStream.BytesCountAllowedToRead = 15;
            byte[] buffer = new byte[20];
            
            // Act: attempt to read 20 bytes asynchronously.
            int bytesRead = await trStream.ReadAsync(buffer, 0, 20, CancellationToken.None);
            
            // Assert
            Assert.Equal(15, bytesRead);
            for (int i = 0; i < bytesRead; i++)
            {
                Assert.Equal(i + 1, buffer[i]);
            }
        }

#if NET
        /// <summary>
        /// Tests that Read(Span<byte>) reads the correct number of bytes with respect to the allowed limit.
        /// </summary>
        [Fact]
        public void Read_Span_ReadsCorrectCountWithLimit()
        {
            // Arrange
            byte[] data = new byte[25];
            for (int i = 0; i < 25; i++) data[i] = (byte)(i + 5);
            using var memoryStream = new MemoryStream(data);
            TransparentReadStream trStream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            
            // Set allowed bytes count to 10.
            trStream.BytesCountAllowedToRead = 10;
            Span<byte> buffer = new byte[15];
            
            // Act: Read using Span.
            int bytesRead = trStream.Read(buffer);
            
            // Assert
            Assert.Equal(10, bytesRead);
            for (int i = 0; i < bytesRead; i++)
            {
                Assert.Equal(i + 5, buffer[i]);
            }
        }

        /// <summary>
        /// Tests that ReadAsync(Memory<byte>) reads the correct number of bytes with respect to the allowed limit.
        /// </summary>
        [Fact]
        public async Task ReadAsync_Memory_ReadsCorrectCountWithLimit()
        {
            // Arrange
            byte[] data = new byte[40];
            for (int i = 0; i < 40; i++) data[i] = (byte)(i + 2);
            using var memoryStream = new MemoryStream(data);
            TransparentReadStream trStream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            
            // Set allowed bytes count to 20.
            trStream.BytesCountAllowedToRead = 20;
            Memory<byte> buffer = new byte[25];
            
            // Act: Read asynchronously using Memory.
            int bytesRead = await trStream.ReadAsync(buffer, CancellationToken.None);
            
            // Assert
            Assert.Equal(20, bytesRead);
            for (int i = 0; i < bytesRead; i++)
            {
                Assert.Equal(i + 2, buffer.Span[i]);
            }
        }
#endif

        /// <summary>
        /// Tests that Seek throws a NotSupportedException when an unsupported SeekOrigin is used.
        /// </summary>
        [Fact]
        public void Seek_InvalidOrigin_ThrowsNotSupportedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[10]);
            TransparentReadStream trStream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            
            // Act & Assert
            Assert.Throws<NotSupportedException>(() => trStream.Seek(5, SeekOrigin.Begin));
        }

        /// <summary>
        /// Tests that SetLength throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void SetLength_Always_ThrowsNotSupportedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[10]);
            TransparentReadStream trStream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            
            // Act & Assert
            Assert.Throws<NotSupportedException>(() => trStream.SetLength(20));
        }

        /// <summary>
        /// Tests that Write always throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void Write_Always_ThrowsNotSupportedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[10]);
            TransparentReadStream trStream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            byte[] buffer = new byte[5];
            
            // Act & Assert
            Assert.Throws<NotSupportedException>(() => trStream.Write(buffer, 0, buffer.Length));
        }

        /// <summary>
        /// Tests that Close calls the underlying stream's Close method.
        /// </summary>
        [Fact]
        public void Close_CallsUnderlyingStreamClose()
        {
            // Arrange
            var fakeStream = new FakeStream(new byte[10]);
            TransparentReadStream trStream = TransparentReadStream.EnsureTransparentReadStream(fakeStream);
            
            // Act
            trStream.Close();
            
            // Assert
            Assert.True(fakeStream.IsClosed);
        }
    }
}
