using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotUtils.StreamUtils;
using FluentAssertions;
using Moq;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ConcatenatedReadStream"/> class.
    /// </summary>
    public class ConcatenatedReadStreamTests
    {
        /// <summary>
        /// Tests that the constructor correctly creates an instance from valid streams.
        /// </summary>
        [Fact]
        public void Constructor_ValidStreams_CreatesInstance()
        {
            // Arrange
            var stream1 = new MemoryStream(new byte[] { 1, 2, 3 });
            var stream2 = new MemoryStream(new byte[] { 4, 5 });
            IEnumerable<Stream> streams = new[] { stream1, stream2 };

            // Act
            using var concatenatedStream = new ConcatenatedReadStream(streams);

            // Assert
            concatenatedStream.CanRead.Should().BeTrue();
            concatenatedStream.CanSeek.Should().BeFalse();
            concatenatedStream.CanWrite.Should().BeFalse();
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentException when any provided stream is not readable.
        /// </summary>
        [Fact]
        public void Constructor_StreamNotReadable_ThrowsArgumentException()
        {
            // Arrange
            var nonReadableMock = new Mock<Stream>();
            nonReadableMock.SetupGet(s => s.CanRead).Returns(false);

            // Act
            Action act = () => new ConcatenatedReadStream(nonReadableMock.Object);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("All streams must be readable*")
                .And.ParamName.Should().Be("streams");
        }

        /// <summary>
        /// Tests that the Read method correctly reads and concatenates data from multiple streams sequentially.
        /// </summary>
        [Fact]
        public void Read_WhenCalled_ReadsFromMultipleStreamsSequentially()
        {
            // Arrange
            byte[] data1 = { 1, 2, 3 };
            byte[] data2 = { 4, 5 };
            using var stream1 = new MemoryStream(data1);
            using var stream2 = new MemoryStream(data2);
            using var concatenatedStream = new ConcatenatedReadStream(stream1, stream2);
            var buffer = new byte[5];

            // Act
            int totalBytesRead = concatenatedStream.Read(buffer, 0, buffer.Length);

            // Assert
            totalBytesRead.Should().Be(5);
            buffer.Should().Equal(new byte[] { 1, 2, 3, 4, 5 });
        }

        /// <summary>
        /// Tests that the Read method returns 0 when no more data is available.
        /// </summary>
        [Fact]
        public void Read_WhenNoDataAvailable_ReturnsZero()
        {
            // Arrange
            using var emptyStream = new MemoryStream(Array.Empty<byte>());
            using var concatenatedStream = new ConcatenatedReadStream(emptyStream);
            var buffer = new byte[10];

            // Act
            int firstRead = concatenatedStream.Read(buffer, 0, buffer.Length);
            int secondRead = concatenatedStream.Read(buffer, 0, buffer.Length);

            // Assert
            firstRead.Should().Be(0);
            secondRead.Should().Be(0);
        }

        /// <summary>
        /// Tests that the ReadByte method correctly reads bytes across streams and returns -1 at the end.
        /// </summary>
        [Fact]
        public void ReadByte_WhenCalled_ReadsByteAcrossStreams()
        {
            // Arrange
            using var stream1 = new MemoryStream(new byte[] { 10 });
            using var stream2 = new MemoryStream(new byte[] { 20 });
            using var concatenatedStream = new ConcatenatedReadStream(stream1, stream2);

            // Act & Assert
            concatenatedStream.ReadByte().Should().Be(10);
            concatenatedStream.ReadByte().Should().Be(20);
            concatenatedStream.ReadByte().Should().Be(-1);
        }

        /// <summary>
        /// Tests that the asynchronous ReadAsync method correctly reads and concatenates data from multiple streams.
        /// </summary>
        [Fact]
        public async Task ReadAsync_WhenCalled_ReadsDataFromMultipleStreamsSequentially()
        {
            // Arrange
            byte[] data1 = { 1, 2, 3 };
            byte[] data2 = { 4, 5 };
            using var stream1 = new MemoryStream(data1);
            using var stream2 = new MemoryStream(data2);
            using var concatenatedStream = new ConcatenatedReadStream(stream1, stream2);
            var buffer = new byte[5];

            // Act
            int totalBytesRead = await concatenatedStream.ReadAsync(buffer, 0, buffer.Length, CancellationToken.None).ConfigureAwait(false);

            // Assert
            totalBytesRead.Should().Be(5);
            buffer.Should().Equal(new byte[] { 1, 2, 3, 4, 5 });
        }

#if NET
        /// <summary>
        /// Tests that the Span-based Read method correctly reads data from multiple streams.
        /// </summary>
        [Fact]
        public void Read_Span_WhenCalled_ReadsDataFromMultipleStreams()
        {
            // Arrange
            byte[] data1 = { 100, 101 };
            byte[] data2 = { 102, 103, 104 };
            using var stream1 = new MemoryStream(data1);
            using var stream2 = new MemoryStream(data2);
            using var concatenatedStream = new ConcatenatedReadStream(stream1, stream2);
            byte[] buffer = new byte[5];
            var span = buffer.AsSpan();

            // Act
            int totalBytesRead = concatenatedStream.Read(span);

            // Assert
            totalBytesRead.Should().Be(5);
            buffer.Should().Equal(new byte[] { 100, 101, 102, 103, 104 });
        }

        /// <summary>
        /// Tests that the Memory-based asynchronous ReadAsync method correctly reads data from multiple streams.
        /// </summary>
        [Fact]
        public async Task ReadAsyncMemory_WhenCalled_ReadsDataFromMultipleStreams()
        {
            // Arrange
            byte[] data1 = { 50, 51 };
            byte[] data2 = { 52, 53, 54 };
            using var stream1 = new MemoryStream(data1);
            using var stream2 = new MemoryStream(data2);
            using var concatenatedStream = new ConcatenatedReadStream(stream1, stream2);
            byte[] buffer = new byte[5];
            Memory<byte> memory = buffer;

            // Act
            int totalBytesRead = await concatenatedStream.ReadAsync(memory, CancellationToken.None).ConfigureAwait(false);

            // Assert
            totalBytesRead.Should().Be(5);
            buffer.Should().Equal(new byte[] { 50, 51, 52, 53, 54 });
        }
#endif

        /// <summary>
        /// Tests that the Flush method throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void Flush_WhenCalled_ThrowsNotSupportedException()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });
            using var concatenatedStream = new ConcatenatedReadStream(stream);

            // Act
            Action act = () => concatenatedStream.Flush();

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("ConcatenatedReadStream is forward-only read-only");
        }

        /// <summary>
        /// Tests that the Seek method throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void Seek_WhenCalled_ThrowsNotSupportedException()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });
            using var concatenatedStream = new ConcatenatedReadStream(stream);

            // Act
            Action act = () => concatenatedStream.Seek(0, SeekOrigin.Begin);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("ConcatenatedReadStream is forward-only read-only");
        }

        /// <summary>
        /// Tests that the SetLength method throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void SetLength_WhenCalled_ThrowsNotSupportedException()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });
            using var concatenatedStream = new ConcatenatedReadStream(stream);

            // Act
            Action act = () => concatenatedStream.SetLength(10);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("ConcatenatedReadStream is forward-only read-only");
        }

        /// <summary>
        /// Tests that the Write method throws a NotSupportedException.
        /// </summary>
        [Fact]
        public void Write_WhenCalled_ThrowsNotSupportedException()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });
            using var concatenatedStream = new ConcatenatedReadStream(stream);
            byte[] buffer = { 1, 2, 3 };

            // Act
            Action act = () => concatenatedStream.Write(buffer, 0, buffer.Length);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("ConcatenatedReadStream is forward-only read-only");
        }

        /// <summary>
        /// Tests that getting and setting the Position property behave as expected.
        /// Getting should return the number of bytes read, and setting should throw a NotSupportedException.
        /// </summary>
        [Fact]
        public void Position_PropertyBehavior_WorksAsExpected()
        {
            // Arrange
            byte[] data = { 1, 2, 3 };
            using var stream = new MemoryStream(data);
            using var concatenatedStream = new ConcatenatedReadStream(stream);
            var buffer = new byte[2];

            // Act
            int bytesRead = concatenatedStream.Read(buffer, 0, buffer.Length);
            long positionAfterRead = concatenatedStream.Position;

            // Assert
            bytesRead.Should().Be(2);
            positionAfterRead.Should().Be(2);

            // Act & Assert for setter
            Action act = () => concatenatedStream.Position = 10;
            act.Should().Throw<NotSupportedException>()
                .WithMessage("ConcatenatedReadStream is forward-only read-only");
        }

        /// <summary>
        /// Tests that the Length property returns the sum of the lengths of the underlying streams.
        /// </summary>
        [Fact]
        public void Length_WhenCalled_ReturnsSumOfUnderlyingStreamsLength()
        {
            // Arrange
            byte[] data1 = new byte[3]; // length 3
            byte[] data2 = new byte[4]; // length 4
            using var stream1 = new MemoryStream(data1);
            using var stream2 = new MemoryStream(data2);
            using var concatenatedStream = new ConcatenatedReadStream(stream1, stream2);

            // Act
            long length = concatenatedStream.Length;

            // Assert
            length.Should().Be(7);
        }

        /// <summary>
        /// Tests that when a nested ConcatenatedReadStream is provided, its inner streams are unwrapped correctly.
        /// </summary>
        [Fact]
        public void Constructor_NestedConcatenatedReadStreams_UnwrapsInnerStreams()
        {
            // Arrange
            byte[] dataNested1 = { 1 };
            byte[] dataNested2 = { 2 };
            using var nestedStream = new ConcatenatedReadStream(new MemoryStream(dataNested1), new MemoryStream(dataNested2));
            byte[] dataParent = { 3 };
            using var parentStream = new MemoryStream(dataParent);

            // Act
            using var concatenatedStream = new ConcatenatedReadStream(parentStream, nestedStream);
            var buffer = new byte[3];
            int bytesRead = concatenatedStream.Read(buffer, 0, buffer.Length);

            // Assert
            bytesRead.Should().Be(3);
            buffer.Should().Equal(new byte[] { 3, 1, 2 });
        }

        /// <summary>
        /// Tests that the asynchronous ReadAsync method throws an OperationCanceledException when cancellation is requested.
        /// </summary>
        [Fact]
        public async Task ReadAsync_WhenCancellationRequested_ThrowsOperationCanceledException()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            var mockStream = new Mock<Stream>();
            mockStream.SetupGet(s => s.CanRead).Returns(true);
            // Setup ReadAsync to respect cancellation.
            mockStream.Setup(s => s.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns<byte[], int, int, CancellationToken>((buffer, offset, count, token) =>
                {
                    return Task.FromCanceled<int>(token);
                });

            using var concatenatedStream = new ConcatenatedReadStream(mockStream.Object);
            var buffer = new byte[5];

            // Act
            Func<Task> act = async () => await concatenatedStream.ReadAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            await act.Should().ThrowAsync<OperationCanceledException>();
        }

#if NET
        /// <summary>
        /// Tests that the asynchronous ReadAsync using Memory overload throws an OperationCanceledException when cancellation is requested.
        /// </summary>
        [Fact]
        public async Task ReadAsyncMemory_WhenCancellationRequested_ThrowsOperationCanceledException()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            var mockStream = new Mock<Stream>();
            mockStream.SetupGet(s => s.CanRead).Returns(true);
            // Setup ReadAsync(Memory<byte>, CancellationToken) to respect cancellation.
            mockStream.Setup(s => s.ReadAsync(It.IsAny<Memory<byte>>(), It.IsAny<CancellationToken>()))
                .Returns<Memory<byte>, CancellationToken>((memory, token) => Task.FromCanceled<int>(token));

            using var concatenatedStream = new ConcatenatedReadStream(mockStream.Object);
            byte[] buffer = new byte[5];
            Memory<byte> mem = buffer;

            // Act
            Func<Task> act = async () => await concatenatedStream.ReadAsync(mem, cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            await act.Should().ThrowAsync<OperationCanceledException>();
        }
#endif
    }
}
