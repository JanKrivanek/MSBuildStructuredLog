using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotUtils.StreamUtils;
using FluentAssertions;
using Moq;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="SubStream"/> class.
    /// </summary>
    public class SubStreamTests
    {
        private const int TestSubStreamLength = 5;

        /// <summary>
        /// Tests that the constructor throws an InvalidOperationException when the underlying stream is not readable.
        /// </summary>
        [Fact]
        public void Constructor_WithNonReadableStream_ThrowsInvalidOperationException()
        {
            // Arrange
            var nonReadableStreamMock = new Mock<Stream>();
            nonReadableStreamMock.Setup(s => s.CanRead).Returns(false);

            // Act
            Action act = () => new SubStream(nonReadableStreamMock.Object, TestSubStreamLength);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Stream must be readable.");
        }

        /// <summary>
        /// Tests that the properties CanRead, CanSeek, CanWrite and Length return the expected values.
        /// </summary>
        [Fact]
        public void Properties_ShouldReturnExpectedValues()
        {
            // Arrange
            using var underlyingStream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
            var subStream = new SubStream(underlyingStream, TestSubStreamLength);

            // Act & Assert
            subStream.CanRead.Should().BeTrue();
            subStream.CanSeek.Should().BeFalse();
            subStream.CanWrite.Should().BeFalse();
            subStream.Length.Should().Be(TestSubStreamLength);
        }

        /// <summary>
        /// Tests that setting the Position property throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void PositionSetter_WhenSet_ThrowsNotImplementedException()
        {
            // Arrange
            using var underlyingStream = new MemoryStream(new byte[] { 1, 2, 3 });
            var subStream = new SubStream(underlyingStream, TestSubStreamLength);

            // Act
            Action act = () => subStream.Position = 2;

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that Flush() calls the underlying stream's Flush() method.
        /// </summary>
        [Fact]
        public void Flush_WhenCalled_InvokesUnderlyingStreamFlush()
        {
            // Arrange
            var underlyingStreamMock = new Mock<Stream>();
            underlyingStreamMock.Setup(s => s.CanRead).Returns(true);
            var subStream = new SubStream(underlyingStreamMock.Object, TestSubStreamLength);

            // Act
            subStream.Flush();

            // Assert
            underlyingStreamMock.Verify(s => s.Flush(), Times.Once);
        }

        /// <summary>
        /// Tests that FlushAsync() calls the underlying stream's FlushAsync() method.
        /// </summary>
        [Fact]
        public async Task FlushAsync_WhenCalled_InvokesUnderlyingStreamFlushAsync()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var underlyingStreamMock = new Mock<Stream>();
            underlyingStreamMock.Setup(s => s.CanRead).Returns(true);
            underlyingStreamMock.Setup(s => s.FlushAsync(cancellationToken)).Returns(Task.CompletedTask);
            var subStream = new SubStream(underlyingStreamMock.Object, TestSubStreamLength);

            // Act
            await subStream.FlushAsync(cancellationToken);

            // Assert
            underlyingStreamMock.Verify(s => s.FlushAsync(cancellationToken), Times.Once);
        }

        /// <summary>
        /// Tests that Read(byte[], int, int) reads only up to the specified substream length.
        /// </summary>
        [Fact]
        public void Read_WithCountExceedingSubStreamLength_ReadsOnlyRemainingBytes()
        {
            // Arrange
            byte[] data = { 10, 20, 30, 40, 50, 60, 70 };
            using var underlyingStream = new MemoryStream(data);
            // Limit substream length to 3 bytes
            var subStream = new SubStream(underlyingStream, 3);
            byte[] buffer = new byte[10];

            // Act
            int bytesRead = subStream.Read(buffer, 0, buffer.Length);

            // Assert
            bytesRead.Should().Be(3);
            buffer[0].Should().Be(10);
            buffer[1].Should().Be(20);
            buffer[2].Should().Be(30);
        }

        /// <summary>
        /// Tests that Read(byte[], int, int) returns 0 when no more data can be read from the substream.
        /// </summary>
        [Fact]
        public void Read_WhenAtEndOfSubStream_ReturnsZero()
        {
            // Arrange
            byte[] data = { 1, 2, 3 };
            using var underlyingStream = new MemoryStream(data);
            var subStream = new SubStream(underlyingStream, data.Length);
            byte[] buffer = new byte[10];

            // Act - read all available data
            int firstRead = subStream.Read(buffer, 0, data.Length);
            int secondRead = subStream.Read(buffer, 0, buffer.Length);

            // Assert
            firstRead.Should().Be(3);
            secondRead.Should().Be(0);
        }

        /// <summary>
        /// Tests that ReadByte() returns the correct byte values and -1 when the end of the substream is reached.
        /// </summary>
        [Fact]
        public void ReadByte_WhenCalled_ReturnsExpectedValues()
        {
            // Arrange
            byte[] data = { 100, 101, 102 };
            using var underlyingStream = new MemoryStream(data);
            var subStream = new SubStream(underlyingStream, data.Length);

            // Act & Assert
            subStream.ReadByte().Should().Be(100);
            subStream.ReadByte().Should().Be(101);
            subStream.ReadByte().Should().Be(102);
            subStream.ReadByte().Should().Be(-1);
        }

        /// <summary>
        /// Tests that ReadAsync(byte[], int, int, CancellationToken) reads only up to the available substream length.
        /// </summary>
        [Fact]
        public async Task ReadAsync_WithCountExceedingSubStreamLength_ReadsOnlyRemainingBytes()
        {
            // Arrange
            byte[] data = { 5, 15, 25, 35, 45, 55 };
            using var underlyingStream = new MemoryStream(data);
            // Limit substream length to 4 bytes
            var subStream = new SubStream(underlyingStream, 4);
            byte[] buffer = new byte[10];
            var cancellationToken = CancellationToken.None;

            // Act
            int bytesRead = await subStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

            // Assert
            bytesRead.Should().Be(4);
            buffer[0].Should().Be(5);
            buffer[1].Should().Be(15);
            buffer[2].Should().Be(25);
            buffer[3].Should().Be(35);
        }

        /// <summary>
        /// Tests that Seek(long, SeekOrigin) throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Seek_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            using var underlyingStream = new MemoryStream(new byte[] { 1, 2, 3 });
            var subStream = new SubStream(underlyingStream, TestSubStreamLength);

            // Act
            Action act = () => subStream.Seek(0, SeekOrigin.Begin);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that SetLength(long) throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void SetLength_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            using var underlyingStream = new MemoryStream(new byte[] { 1, 2, 3 });
            var subStream = new SubStream(underlyingStream, TestSubStreamLength);

            // Act
            Action act = () => subStream.SetLength(10);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that Write(byte[], int, int) throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Write_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            using var underlyingStream = new MemoryStream(new byte[] { 1, 2, 3 });
            var subStream = new SubStream(underlyingStream, TestSubStreamLength);
            byte[] buffer = { 1, 2, 3 };

            // Act
            Action act = () => subStream.Write(buffer, 0, buffer.Length);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

#if NET
        /// <summary>
        /// Tests that Read(Span{byte}) reads only up to the remaining available bytes in the substream.
        /// </summary>
        [Fact]
        public void ReadSpan_WithBufferLongerThanRemaining_ReadsOnlyRemainingBytes()
        {
            // Arrange
            byte[] data = { 200, 201, 202, 203, 204 };
            using var underlyingStream = new MemoryStream(data);
            // Limit substream length to 3 bytes
            var subStream = new SubStream(underlyingStream, 3);
            Span<byte> buffer = new byte[10];

            // Act
            int bytesRead = subStream.Read(buffer);

            // Assert
            bytesRead.Should().Be(3);
            buffer[0].Should().Be(200);
            buffer[1].Should().Be(201);
            buffer[2].Should().Be(202);
        }

        /// <summary>
        /// Tests that ReadAsync(Memory{byte}, CancellationToken) asynchronously reads only up to the remaining available bytes.
        /// </summary>
        [Fact]
        public async Task ReadAsyncMemory_WithBufferLongerThanRemaining_ReadsOnlyRemainingBytes()
        {
            // Arrange
            byte[] data = { 250, 251, 252, 253 };
            using var underlyingStream = new MemoryStream(data);
            // Limit substream length to 2 bytes
            var subStream = new SubStream(underlyingStream, 2);
            Memory<byte> buffer = new byte[10];
            var cancellationToken = CancellationToken.None;

            // Act
            int bytesRead = await subStream.ReadAsync(buffer, cancellationToken);

            // Assert
            bytesRead.Should().Be(2);
            buffer.Span[0].Should().Be(250);
            buffer.Span[1].Should().Be(251);
        }
#endif
    }
}
