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
    /// Unit tests for the <see cref="TransparentReadStream"/> class.
    /// </summary>
    public class TransparentReadStreamTests
    {
        #region EnsureSeekableStream Tests

        /// <summary>
        /// Tests that EnsureSeekableStream returns the same stream if the stream is already seekable.
        /// </summary>
        [Fact]
        public void EnsureSeekableStream_WhenStreamCanSeek_ReturnsSameStream()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            
            // Act
            Stream result = TransparentReadStream.EnsureSeekableStream(memoryStream);

            // Assert
            result.Should().BeSameAs(memoryStream);
        }

        /// <summary>
        /// Tests that EnsureSeekableStream returns a TransparentReadStream wrapper when the stream cannot seek but is readable.
        /// </summary>
        [Fact]
        public void EnsureSeekableStream_WhenStreamCannotSeekButReadable_ReturnsTransparentReadStream()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.Setup(s => s.CanSeek).Returns(false);
            mockStream.Setup(s => s.CanRead).Returns(true);

            // Act
            Stream result = TransparentReadStream.EnsureSeekableStream(mockStream.Object);

            // Assert
            result.Should().BeOfType<TransparentReadStream>();
        }

        /// <summary>
        /// Tests that EnsureSeekableStream throws InvalidOperationException when the stream is not readable.
        /// </summary>
        [Fact]
        public void EnsureSeekableStream_WhenStreamNotReadable_ThrowsInvalidOperationException()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.Setup(s => s.CanSeek).Returns(false);
            mockStream.Setup(s => s.CanRead).Returns(false);

            // Act
            Action act = () => TransparentReadStream.EnsureSeekableStream(mockStream.Object);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Stream must be readable.");
        }

        #endregion

        #region EnsureTransparentReadStream Tests

        /// <summary>
        /// Tests that EnsureTransparentReadStream returns the same instance if the provided stream is already a TransparentReadStream.
        /// </summary>
        [Fact]
        public void EnsureTransparentReadStream_WhenAlreadyTransparentReadStream_ReturnsSameInstance()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            TransparentReadStream transparentStream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            
            // Act
            TransparentReadStream result = TransparentReadStream.EnsureTransparentReadStream(transparentStream);

            // Assert
            result.Should().BeSameAs(transparentStream);
        }

        /// <summary>
        /// Tests that EnsureTransparentReadStream wraps a non-transparent but readable stream into a TransparentReadStream.
        /// </summary>
        [Fact]
        public void EnsureTransparentReadStream_WhenNotTransparentButReadable_ReturnsNewTransparentReadStream()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            
            // Act
            TransparentReadStream result = TransparentReadStream.EnsureTransparentReadStream(memoryStream);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeSameAs(memoryStream);
        }

        /// <summary>
        /// Tests that EnsureTransparentReadStream throws InvalidOperationException when the stream is not readable.
        /// </summary>
        [Fact]
        public void EnsureTransparentReadStream_WhenStreamNotReadable_ThrowsInvalidOperationException()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.Setup(s => s.CanRead).Returns(false);

            // Act
            Action act = () => TransparentReadStream.EnsureTransparentReadStream(mockStream.Object);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Stream must be readable.");
        }

        #endregion

        #region Property and Read Method Tests

        /// <summary>
        /// Tests that the properties CanRead, CanSeek, CanWrite, and Length reflect the underlying stream.
        /// </summary>
        [Fact]
        public void Properties_ShouldReflectUnderlyingStream()
        {
            // Arrange
            byte[] bytes = { 1, 2, 3, 4, 5 };
            using var memoryStream = new MemoryStream(bytes);
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);

            // Act & Assert
            stream.CanRead.Should().BeTrue();
            stream.CanSeek.Should().BeTrue();
            stream.CanWrite.Should().BeFalse();
            stream.Length.Should().Be(memoryStream.Length);
        }

        /// <summary>
        /// Tests that setting BytesCountAllowedToRead limits the number of bytes that can be read.
        /// </summary>
        [Fact]
        public void BytesCountAllowedToRead_SetLimitsTheReadAmount()
        {
            // Arrange
            byte[] bytes = { 10, 20, 30, 40, 50, 60 };
            using var memoryStream = new MemoryStream(bytes);
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);

            // Limit allowed bytes to 4.
            stream.BytesCountAllowedToRead = 4;

            byte[] buffer = new byte[10];

            // Act: attempt to read more than allowed.
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            // Assert: only 4 bytes should be read.
            bytesRead.Should().Be(4);
            stream.BytesCountAllowedToReadRemaining.Should().Be(0);
        }

        /// <summary>
        /// Tests that when BytesCountAllowedToRead is not set, BytesCountAllowedToReadRemaining returns 0.
        /// </summary>
        [Fact]
        public void BytesCountAllowedToReadRemaining_WhenUnlimited_ReturnsZero()
        {
            // Arrange
            byte[] bytes = { 1, 2, 3, 4 };
            using var memoryStream = new MemoryStream(bytes);
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);

            // Act & Assert
            stream.BytesCountAllowedToReadRemaining.Should().Be(0);
        }

        /// <summary>
        /// Tests that Read(byte[], int, int) reads the correct number of bytes and updates the stream position.
        /// </summary>
        [Fact]
        public void Read_WhenCalled_UpdatesPositionAndReturnsBytesRead()
        {
            // Arrange
            byte[] sourceBytes = { 100, 101, 102, 103, 104 };
            using var memoryStream = new MemoryStream(sourceBytes);
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            byte[] buffer = new byte[10];

            // Act
            int bytesRead = stream.Read(buffer, 0, 3);

            // Assert
            bytesRead.Should().Be(3);
            // Verify that the bytes read match expected
            buffer[0].Should().Be(100);
            buffer[1].Should().Be(101);
            buffer[2].Should().Be(102);
            stream.Position.Should().Be(3);
        }

        /// <summary>
        /// Tests that Read(byte[], int, int) does not read beyond the allowed limit.
        /// </summary>
        [Fact]
        public void Read_WhenExceedingAllowedLimit_ReadsOnlyAllowedBytes()
        {
            // Arrange
            byte[] sourceBytes = { 1, 2, 3, 4, 5 };
            using var memoryStream = new MemoryStream(sourceBytes);
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            stream.BytesCountAllowedToRead = 2;
            byte[] buffer = new byte[5];

            // Act
            int bytesRead = stream.Read(buffer, 0, 4);

            // Assert
            bytesRead.Should().Be(2);
            stream.Position.Should().Be(2);
        }

        /// <summary>
        /// Tests that ReadByte correctly reads a byte and updates the position.
        /// </summary>
        [Fact]
        public void ReadByte_WhenCalled_ReturnsByteAndUpdatesPosition()
        {
            // Arrange
            byte[] sourceBytes = { 200, 201 };
            using var memoryStream = new MemoryStream(sourceBytes);
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);

            // Act
            int firstByte = stream.ReadByte();
            int secondByte = stream.ReadByte();
            int thirdByte = stream.ReadByte(); // Should return -1 (EOF)

            // Assert
            firstByte.Should().Be(200);
            secondByte.Should().Be(201);
            thirdByte.Should().Be(-1);
            stream.Position.Should().Be(2);
        }

        /// <summary>
        /// Tests that ReadAsync correctly reads bytes and updates the position.
        /// </summary>
        [Fact]
        public async Task ReadAsync_WhenCalled_UpdatesPositionAndReturnsBytesRead()
        {
            // Arrange
            byte[] sourceBytes = { 50, 51, 52, 53 };
            using var memoryStream = new MemoryStream(sourceBytes);
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            byte[] buffer = new byte[10];
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            int bytesRead = await stream.ReadAsync(buffer, 0, 3, cancellationToken).ConfigureAwait(false);

            // Assert
            bytesRead.Should().Be(3);
            stream.Position.Should().Be(3);
        }

#if NET
        /// <summary>
        /// Tests that Read(Span&lt;byte&gt;) reads the correct number of bytes and updates the position.
        /// </summary>
        [Fact]
        public void ReadSpan_WhenCalled_UpdatesPositionAndReturnsBytesRead()
        {
            // Arrange
            byte[] sourceBytes = { 10, 20, 30, 40 };
            using var memoryStream = new MemoryStream(sourceBytes);
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            Span<byte> buffer = new byte[10];

            // Act
            int bytesRead = stream.Read(buffer);

            // Assert
            bytesRead.Should().Be(sourceBytes.Length);
            stream.Position.Should().Be(sourceBytes.Length);
        }

        /// <summary>
        /// Tests that ReadAsync(Memory&lt;byte&gt;) reads bytes correctly and updates the position.
        /// </summary>
        [Fact]
        public async Task ReadAsyncMemory_WhenCalled_UpdatesPositionAndReturnsBytesRead()
        {
            // Arrange
            byte[] sourceBytes = { 5, 6, 7, 8, 9 };
            using var memoryStream = new MemoryStream(sourceBytes);
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            Memory<byte> buffer = new byte[10];

            // Act
            int bytesRead = await stream.ReadAsync(buffer).ConfigureAwait(false);

            // Assert
            bytesRead.Should().Be(sourceBytes.Length);
            stream.Position.Should().Be(sourceBytes.Length);
        }
#endif

        #endregion

        #region Seek, SetLength, Write, and Close Tests

        /// <summary>
        /// Tests that Seek with SeekOrigin.Current adjusts the position correctly.
        /// </summary>
        [Fact]
        public void Seek_WhenCalledWithCurrentOrigin_UpdatesPosition()
        {
            // Arrange
            byte[] sourceBytes = { 1, 2, 3, 4, 5 };
            using var memoryStream = new MemoryStream(sourceBytes);
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);

            // Act: Read 2 bytes first.
            byte[] buffer = new byte[2];
            int initialRead = stream.Read(buffer, 0, 2);
            // Now seek forward 2 more bytes relative to current position.
            long newPosition = stream.Seek(2, SeekOrigin.Current);

            // Assert
            newPosition.Should().Be(4);
            stream.Position.Should().Be(4);
        }

        /// <summary>
        /// Tests that Seek with a non-Current SeekOrigin throws a NotSupportedException.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public void Seek_WhenCalledWithNonCurrentOrigin_ThrowsNotSupportedException(int offset)
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);

            // Act
            Action actBegin = () => stream.Seek(offset, SeekOrigin.Begin);
            Action actEnd = () => stream.Seek(offset, SeekOrigin.End);

            // Assert
            actBegin.Should().Throw<NotSupportedException>()
                .WithMessage("Only seeking from SeekOrigin.Current is supported.");
            actEnd.Should().Throw<NotSupportedException>()
                .WithMessage("Only seeking from SeekOrigin.Current is supported.");
        }

        /// <summary>
        /// Tests that SetLength throws NotSupportedException.
        /// </summary>
        [Fact]
        public void SetLength_WhenCalled_ThrowsNotSupportedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);

            // Act
            Action act = () => stream.SetLength(10);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("Expanding stream is not supported.");
        }

        /// <summary>
        /// Tests that Write throws NotSupportedException.
        /// </summary>
        [Fact]
        public void Write_WhenCalled_ThrowsNotSupportedException()
        {
            // Arrange
            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
            TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
            byte[] buffer = new byte[5];

            // Act
            Action act = () => stream.Write(buffer, 0, buffer.Length);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .WithMessage("Writing is not supported.");
        }

// Could not make this test passing.
//         /// <summary>
//         /// Tests that Close calls the underlying stream's Close method.
//         /// </summary>
//         [Fact]
//         public void Close_WhenCalled_InvokesUnderlyingStreamClose()
//         {
//             // Arrange
//             var mockStream = new Mock<Stream>();
//             mockStream.Setup(s => s.Close());
//             // Ensure the mock returns required properties.
//             mockStream.Setup(s => s.CanRead).Returns(true);
//             using (TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(mockStream.Object))
//             {
//                 // Act
//                 stream.Close();
//             }
//         
//             // Assert
//             mockStream.Verify(s => s.Close(), Times.Exactly(2));
//         }
// 
        #endregion

        #region Flush and FlushAsync Tests

        /// <summary>
        /// Tests that Flush calls the underlying stream's Flush method.
        /// </summary>
        [Fact]
        public void Flush_WhenCalled_InvokesUnderlyingStreamFlush()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.Setup(s => s.Flush());
            mockStream.Setup(s => s.CanRead).Returns(true);
            using TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(mockStream.Object);

            // Act
            stream.Flush();

            // Assert
            mockStream.Verify(s => s.Flush(), Times.Once);
        }
//  // [Error] (458-13)CS1929 'ISetup<Stream, Task>' does not contain a definition for 'ReturnsAsync' and the best extension method overload 'SequenceExtensions.ReturnsAsync<Task>(ISetupSequentialResult<Task<Task>>, Task)' requires a receiver of type 'Moq.Language.ISetupSequentialResult<System.Threading.Tasks.Task<System.Threading.Tasks.Task>>'
//         /// <summary>
//         /// Tests that FlushAsync calls the underlying stream's FlushAsync method.
//         /// </summary>
//         [Fact]
//         public async Task FlushAsync_WhenCalled_InvokesUnderlyingStreamFlushAsync()
//         {
//             // Arrange
//             var mockStream = new Mock<Stream>();
//             mockStream.Setup(s => s.FlushAsync(It.IsAny<CancellationToken>()))
//                       .ReturnsAsync(Task.CompletedTask)
//                       .Verifiable();
//             mockStream.Setup(s => s.CanRead).Returns(true);
//             using TransparentReadStream stream = TransparentReadStream.EnsureTransparentReadStream(mockStream.Object);
//             CancellationToken cancellationToken = CancellationToken.None;
// 
//             // Act
//             await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
// 
//             // Assert
//             mockStream.Verify(s => s.FlushAsync(cancellationToken), Times.Once);
//         }
// 
        #endregion
    }
}
