using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Provides unit tests for the ConcatenatedReadStream class.
    /// </summary>
    public class ConcatenatedReadStreamTests
    {
        /// <summary>
        /// A helper stream that is not readable.
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
        /// A helper MemoryStream that tracks if Dispose has been called.
        /// </summary>
        private class TrackingMemoryStream : MemoryStream
        {
            public bool IsDisposed { get; private set; } = false;

            protected override void Dispose(bool disposing)
            {
                IsDisposed = true;
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Tests that constructor throws an ArgumentException when any provided stream is not readable.
        /// </summary>
        [Fact]
        public void Constructor_WithNonReadableStream_ThrowsArgumentException()
        {
            // Arrange
            var nonReadable = new NonReadableStream();
            var readable = new MemoryStream(Encoding.UTF8.GetBytes("data"));
            IEnumerable<Stream> streams = new List<Stream> { readable, nonReadable };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new ConcatenatedReadStream(streams));
            Assert.Contains("All streams must be readable", exception.Message);
        }

        /// <summary>
        /// Tests that constructor with nested ConcatenatedReadStream flattens the streams.
        /// </summary>
        [Fact]
        public void Constructor_WithNestedConcatenatedStream_FlattensStreams()
        {
            // Arrange
            byte[] data1 = Encoding.UTF8.GetBytes("Hello, ");
            byte[] data2 = Encoding.UTF8.GetBytes("World!");
            var stream1 = new MemoryStream(data1);
            var stream2 = new MemoryStream(data2);
            var nested = new ConcatenatedReadStream(stream1, stream2);

            byte[] data3 = Encoding.UTF8.GetBytes(" Goodbye.");
            var stream3 = new MemoryStream(data3);

            // Act
            var concatenated = new ConcatenatedReadStream(nested, stream3);
            byte[] buffer = new byte[100];
            int bytesRead = concatenated.Read(buffer, 0, buffer.Length);
            string result = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Assert
            Assert.Equal("Hello, World! Goodbye.", result);
        }

        /// <summary>
        /// Tests that Flush() throws NotSupportedException.
        /// </summary>
        [Fact]
        public void Flush_ThrowsNotSupportedException()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("data"));
            var concatenated = new ConcatenatedReadStream(stream);

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => concatenated.Flush());
        }

        /// <summary>
        /// Tests that Read returns 0 when count is 0.
        /// </summary>
        [Fact]
        public void Read_WithZeroCount_ReturnsZero()
        {
            // Arrange
            var data = Encoding.UTF8.GetBytes("sample");
            var stream = new MemoryStream(data);
            var concatenated = new ConcatenatedReadStream(stream);
            byte[] buffer = new byte[10];

            // Act
            int bytesRead = concatenated.Read(buffer, 0, 0);

            // Assert
            Assert.Equal(0, bytesRead);
        }

        /// <summary>
        /// Tests that Read correctly reads data across multiple streams and disposes fully read streams.
        /// </summary>
        [Fact]
        public void Read_CorrectlyReadsDataAcrossStreams()
        {
            // Arrange
            byte[] data1 = Encoding.UTF8.GetBytes("Foo");
            byte[] data2 = Encoding.UTF8.GetBytes("Bar");
            var trackingStream1 = new TrackingMemoryStream();
            trackingStream1.Write(data1, 0, data1.Length);
            trackingStream1.Position = 0;

            var trackingStream2 = new TrackingMemoryStream();
            trackingStream2.Write(data2, 0, data2.Length);
            trackingStream2.Position = 0;

            var concatenated = new ConcatenatedReadStream(trackingStream1, trackingStream2);
            byte[] buffer = new byte[10];

            // Act
            int totalBytes = concatenated.Read(buffer, 0, buffer.Length);
            string result = Encoding.UTF8.GetString(buffer, 0, totalBytes);

            // Assert
            Assert.Equal("FooBar", result);
            // Ensure that both underlying streams are disposed after being fully read.
            Assert.True(trackingStream1.IsDisposed);
            Assert.True(trackingStream2.IsDisposed);
        }

        /// <summary>
        /// Tests that ReadByte reads successive bytes and returns the correct values.
        /// </summary>
        [Fact]
        public void ReadByte_ReadsNextByte_ReturnsByteValue()
        {
            // Arrange
            byte[] data = { 10, 20, 30 };
            var stream = new MemoryStream(data);
            var concatenated = new ConcatenatedReadStream(stream);

            // Act & Assert
            Assert.Equal(10, concatenated.ReadByte());
            Assert.Equal(20, concatenated.ReadByte());
            Assert.Equal(30, concatenated.ReadByte());
        }

        /// <summary>
        /// Tests that ReadByte returns -1 after all data has been read and disposes underlying streams.
        /// </summary>
        [Fact]
        public void ReadByte_ReturnsMinusOneAfterAllDataRead()
        {
            // Arrange
            var trackingStream = new TrackingMemoryStream();
            byte[] data = { 100 };
            trackingStream.Write(data, 0, data.Length);
            trackingStream.Position = 0;
            var concatenated = new ConcatenatedReadStream(trackingStream);

            // Act
            int firstByte = concatenated.ReadByte();
            int endByte = concatenated.ReadByte();

            // Assert
            Assert.Equal(100, firstByte);
            Assert.Equal(-1, endByte);
            Assert.True(trackingStream.IsDisposed);
        }

        /// <summary>
        /// Tests that ReadAsync returns 0 when count is 0.
        /// </summary>
        [Fact]
        public async Task ReadAsync_WithZeroCount_ReturnsZero()
        {
            // Arrange
            var data = Encoding.UTF8.GetBytes("async");
            var stream = new MemoryStream(data);
            var concatenated = new ConcatenatedReadStream(stream);
            byte[] buffer = new byte[10];

            // Act
            int bytesRead = await concatenated.ReadAsync(buffer, 0, 0, CancellationToken.None);

            // Assert
            Assert.Equal(0, bytesRead);
        }

        /// <summary>
        /// Tests that ReadAsync correctly reads data across streams asynchronously.
        /// </summary>
        [Fact]
        public async Task ReadAsync_ReadsAllDataAcrossStreams()
        {
            // Arrange
            byte[] data1 = Encoding.UTF8.GetBytes("Async");
            byte[] data2 = Encoding.UTF8.GetBytes("Read");
            var trackingStream1 = new TrackingMemoryStream();
            trackingStream1.Write(data1, 0, data1.Length);
            trackingStream1.Position = 0;

            var trackingStream2 = new TrackingMemoryStream();
            trackingStream2.Write(data2, 0, data2.Length);
            trackingStream2.Position = 0;

            var concatenated = new ConcatenatedReadStream(trackingStream1, trackingStream2);
            byte[] buffer = new byte[20];

            // Act
            int totalBytes = await concatenated.ReadAsync(buffer, 0, buffer.Length, CancellationToken.None);
            string result = Encoding.UTF8.GetString(buffer, 0, totalBytes);

            // Assert
            Assert.Equal("AsyncRead", result);
            Assert.True(trackingStream1.IsDisposed);
            Assert.True(trackingStream2.IsDisposed);
        }

#if NET
        /// <summary>
        /// Tests that Read(Span<byte>) correctly reads data using Span overload.
        /// </summary>
        [Fact]
        public void ReadSpan_ReadsDataAcrossStreams()
        {
            // Arrange
            byte[] data1 = Encoding.UTF8.GetBytes("Span");
            byte[] data2 = Encoding.UTF8.GetBytes("Test");
            var trackingStream1 = new TrackingMemoryStream();
            trackingStream1.Write(data1, 0, data1.Length);
            trackingStream1.Position = 0;

            var trackingStream2 = new TrackingMemoryStream();
            trackingStream2.Write(data2, 0, data2.Length);
            trackingStream2.Position = 0;

            var concatenated = new ConcatenatedReadStream(trackingStream1, trackingStream2);
            Span<byte> buffer = new byte[20];

            // Act
            int totalBytes = concatenated.Read(buffer);
            string result = Encoding.UTF8.GetString(buffer.Slice(0, totalBytes));

            // Assert
            Assert.Equal("SpanTest", result);
            Assert.True(trackingStream1.IsDisposed);
            Assert.True(trackingStream2.IsDisposed);
        }

        /// <summary>
        /// Tests that ReadAsync(Memory<byte>, CancellationToken) correctly reads data across streams asynchronously.
        /// </summary>
        [Fact]
        public async Task ReadAsyncMemory_ReadsDataAcrossStreams()
        {
            // Arrange
            byte[] data1 = Encoding.UTF8.GetBytes("Memory");
            byte[] data2 = Encoding.UTF8.GetBytes("Async");
            var trackingStream1 = new TrackingMemoryStream();
            trackingStream1.Write(data1, 0, data1.Length);
            trackingStream1.Position = 0;

            var trackingStream2 = new TrackingMemoryStream();
            trackingStream2.Write(data2, 0, data2.Length);
            trackingStream2.Position = 0;

            var concatenated = new ConcatenatedReadStream(trackingStream1, trackingStream2);
            Memory<byte> buffer = new byte[20];

            // Act
            int totalBytes = await concatenated.ReadAsync(buffer, CancellationToken.None);
            string result = Encoding.UTF8.GetString(buffer.Slice(0, totalBytes).ToArray());

            // Assert
            Assert.Equal("MemoryAsync", result);
            Assert.True(trackingStream1.IsDisposed);
            Assert.True(trackingStream2.IsDisposed);
        }
#endif

        /// <summary>
        /// Tests that Seek throws NotSupportedException.
        /// </summary>
        [Fact]
        public void Seek_ThrowsNotSupportedException()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("data"));
            var concatenated = new ConcatenatedReadStream(stream);

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => concatenated.Seek(0, SeekOrigin.Begin));
        }

        /// <summary>
        /// Tests that SetLength throws NotSupportedException.
        /// </summary>
        [Fact]
        public void SetLength_ThrowsNotSupportedException()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("data"));
            var concatenated = new ConcatenatedReadStream(stream);

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => concatenated.SetLength(100));
        }

        /// <summary>
        /// Tests that Write throws NotSupportedException.
        /// </summary>
        [Fact]
        public void Write_ThrowsNotSupportedException()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("data"));
            var concatenated = new ConcatenatedReadStream(stream);
            byte[] buffer = Encoding.UTF8.GetBytes("test");

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => concatenated.Write(buffer, 0, buffer.Length));
        }

        /// <summary>
        /// Tests that setting the Position property throws NotSupportedException.
        /// </summary>
        [Fact]
        public void Position_Set_ThrowsNotSupportedException()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("data"));
            var concatenated = new ConcatenatedReadStream(stream);

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => concatenated.Position = 10);
        }

        /// <summary>
        /// Tests the properties CanRead, CanSeek, and CanWrite.
        /// </summary>
        [Fact]
        public void Properties_ReturnCorrectValues()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("data"));
            var concatenated = new ConcatenatedReadStream(stream);

            // Act & Assert
            Assert.True(concatenated.CanRead);
            Assert.False(concatenated.CanSeek);
            Assert.False(concatenated.CanWrite);
        }

        /// <summary>
        /// Tests that the Length property returns the sum of underlying streams' lengths.
        /// </summary>
        [Fact]
        public void Length_ReturnsSumOfUnderlyingStreamsLengths()
        {
            // Arrange
            byte[] data1 = Encoding.UTF8.GetBytes("1234"); // length 4
            byte[] data2 = Encoding.UTF8.GetBytes("56789"); // length 5
            var stream1 = new MemoryStream(data1);
            var stream2 = new MemoryStream(data2);
            var concatenated = new ConcatenatedReadStream(stream1, stream2);

            // Act
            long length = concatenated.Length;

            // Assert
            Assert.Equal(9, length);
        }

        /// <summary>
        /// Tests that the Position property returns the correct read position after reading data.
        /// </summary>
        [Fact]
        public void Position_Get_ReturnsCorrectValueAfterRead()
        {
            // Arrange
            byte[] data1 = Encoding.UTF8.GetBytes("AB");
            byte[] data2 = Encoding.UTF8.GetBytes("CD");
            var stream1 = new MemoryStream(data1);
            var stream2 = new MemoryStream(data2);
            var concatenated = new ConcatenatedReadStream(stream1, stream2);
            byte[] buffer = new byte[3];

            // Act
            int bytesRead = concatenated.Read(buffer, 0, buffer.Length);
            long positionAfterRead = concatenated.Position;

            // Assert
            Assert.Equal(3, bytesRead);
            Assert.Equal(3, positionAfterRead);
        }
    }
}
