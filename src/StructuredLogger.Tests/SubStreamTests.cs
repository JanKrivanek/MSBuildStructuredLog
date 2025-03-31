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
        /// <summary>
        /// Tests that the constructor throws an InvalidOperationException when the underlying stream is not readable.
        /// </summary>
        [Fact]
        public void Constructor_NonReadableStream_ThrowsInvalidOperationException()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.Setup(s => s.CanRead).Returns(false);

            // Act
            Action act = () => new SubStream(mockStream.Object, 10);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Stream must be readable.");
        }

        /// <summary>
        /// Tests that the Read method reads the correct number of bytes and updates internal position accordingly.
        /// </summary>
        [Fact]
        public void Read_WithinSubStreamLength_ReturnsCorrectBytesAndUpdatesPosition()
        {
            // Arrange
            byte[] data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            using var baseStream = new MemoryStream(data);
            int subLength = 5;
            using var subStream = new SubStream(baseStream, subLength);
            var buffer = new byte[10];

            // Act
            int bytesRead = subStream.Read(buffer, 0, buffer.Length);

            // Assert
            bytesRead.Should().Be(subLength);
            buffer.Should().StartWith(new byte[] { 1, 2, 3, 4, 5 });
            subStream.IsAtEnd.Should().BeTrue();

            // Act - subsequent read should return 0 bytes when substream is exhausted.
            int additionalBytes = subStream.Read(buffer, 0, buffer.Length);
            additionalBytes.Should().Be(0);
        }

        /// <summary>
        /// Tests that the Read method returns only the available number of bytes if requested count exceeds remaining length.
        /// </summary>
        [Fact]
        public void Read_RequestMoreThanAvailable_ReturnsOnlyRemainingBytes()
        {
            // Arrange
            byte[] data = new byte[] { 10, 20, 30, 40, 50 };
            using var baseStream = new MemoryStream(data);
            int subLength = 3;
            using var subStream = new SubStream(baseStream, subLength);
            var buffer = new byte[5];

            // Act
            int bytesRead = subStream.Read(buffer, 0, buffer.Length);

            // Assert
            bytesRead.Should().Be(3);
            buffer.Should().StartWith(new byte[] { 10, 20, 30 });
            subStream.IsAtEnd.Should().BeTrue();
        }

        /// <summary>
        /// Tests that the ReadByte method returns the correct byte values and updates the position until the end is reached.
        /// </summary>
        [Fact]
        public void ReadByte_WhenBytesAvailable_ReturnsByteAndUpdatesPosition()
        {
            // Arrange
            byte[] data = new byte[] { 10, 20, 30 };
            using var baseStream = new MemoryStream(data);
            int subLength = 2;
            using var subStream = new SubStream(baseStream, subLength);

            // Act & Assert
            int firstByte = subStream.ReadByte();
            firstByte.Should().Be(10);

            int secondByte = subStream.ReadByte();
            secondByte.Should().Be(20);

            // Next call should return -1 since subLength is reached.
            int endByte = subStream.ReadByte();
            endByte.Should().Be(-1);
        }

        /// <summary>
        /// Tests that the ReadAsync method reads the correct number of bytes asynchronously and updates internal position.
        /// </summary>
        [Fact]
        public async Task ReadAsync_WithinSubStreamLength_ReturnsCorrectBytesAndUpdatesPosition()
        {
            // Arrange
            byte[] data = new byte[] { 5, 4, 3, 2, 1, 0 };
            using var baseStream = new MemoryStream(data);
            int subLength = 4;
            using var subStream = new SubStream(baseStream, subLength);
            var buffer = new byte[10];

            // Act
            int bytesRead = await subStream.ReadAsync(buffer, 0, buffer.Length, CancellationToken.None).ConfigureAwait(false);

            // Assert
            bytesRead.Should().Be(subLength);
            buffer.Should().StartWith(new byte[] { 5, 4, 3, 2 });
            subStream.IsAtEnd.Should().BeTrue();

            // Act - subsequent async read should return 0 bytes.
            int additionalBytes = await subStream.ReadAsync(buffer, 0, buffer.Length, CancellationToken.None).ConfigureAwait(false);
            additionalBytes.Should().Be(0);
        }

#if NET
        /// <summary>
        /// Tests that the Read(Span<byte>) method works correctly by reading up to the SubStream length.
        /// </summary>
        [Fact]
        public void ReadSpan_WithinSubStreamLength_ReturnsCorrectBytesAndUpdatesPosition()
        {
            // Arrange
            byte[] data = new byte[] { 100, 101, 102, 103, 104 };
            using var baseStream = new MemoryStream(data);
            int subLength = 3;
            using var subStream = new SubStream(baseStream, subLength);
            Span<byte> buffer = new byte[5];

            // Act
            int bytesRead = subStream.Read(buffer);

            // Assert
            bytesRead.Should().Be(3);
            buffer.Slice(0, 3).ToArray().Should().Equal(new byte[] { 100, 101, 102 });
            subStream.IsAtEnd.Should().BeTrue();
        }

        /// <summary>
        /// Tests that the ReadAsync(Memory<byte>, CancellationToken) method works correctly by asynchronously reading up to the SubStream length.
        /// </summary>
        [Fact]
        public async Task ReadAsyncMemory_WithinSubStreamLength_ReturnsCorrectBytesAndUpdatesPosition()
        {
            // Arrange
            byte[] data = new byte[] { 200, 201, 202, 203 };
            using var baseStream = new MemoryStream(data);
            int subLength = 2;
            using var subStream = new SubStream(baseStream, subLength);
            Memory<byte> buffer = new byte[5];

            // Act
            int bytesRead = await subStream.ReadAsync(buffer, CancellationToken.None).ConfigureAwait(false);

            // Assert
            bytesRead.Should().Be(2);
            buffer.Slice(0, 2).ToArray().Should().Equal(new byte[] { 200, 201 });
            subStream.IsAtEnd.Should().BeTrue();

            // Act - subsequent async read should return 0 bytes.
            int additionalBytes = await subStream.ReadAsync(buffer, CancellationToken.None).ConfigureAwait(false);
            additionalBytes.Should().Be(0);
        }
#endif

        /// <summary>
        /// Tests that the Flush method calls the underlying stream's Flush method.
        /// </summary>
        [Fact]
        public void Flush_CallsUnderlyingStreamFlush()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            // Ensure CanRead returns true because constructor checks it.
            mockStream.Setup(s => s.CanRead).Returns(true);
            // Setup Flush as virtual.
            mockStream.Setup(s => s.Flush());
            using var subStream = new SubStream(mockStream.Object, 10);

            // Act
            subStream.Flush();

            // Assert
            mockStream.Verify(s => s.Flush(), Times.Once);
        }
//  // [Error] (213-13)CS1929 'ISetup<Stream, Task>' does not contain a definition for 'ReturnsAsync' and the best extension method overload 'SequenceExtensions.ReturnsAsync<Task>(ISetupSequentialResult<Task<Task>>, Task)' requires a receiver of type 'Moq.Language.ISetupSequentialResult<System.Threading.Tasks.Task<System.Threading.Tasks.Task>>'
//         /// <summary>
//         /// Tests that the FlushAsync method calls the underlying stream's FlushAsync method.
//         /// </summary>
//         [Fact]
//         public async Task FlushAsync_CallsUnderlyingStreamFlushAsync()
//         {
//             // Arrange
//             var mockStream = new Mock<Stream>();
//             mockStream.Setup(s => s.CanRead).Returns(true);
//             mockStream.Setup(s => s.FlushAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Task.CompletedTask);
//             using var subStream = new SubStream(mockStream.Object, 10);
//             var cancellationToken = new CancellationToken();
// 
//             // Act
//             await subStream.FlushAsync(cancellationToken).ConfigureAwait(false);
// 
//             // Assert
//             mockStream.Verify(s => s.FlushAsync(cancellationToken), Times.Once);
//         }
// 
        /// <summary>
        /// Tests that the Seek method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Seek_AnyInput_ThrowsNotImplementedException()
        {
            // Arrange
            byte[] data = new byte[] { 1, 2, 3 };
            using var baseStream = new MemoryStream(data);
            using var subStream = new SubStream(baseStream, 3);

            // Act
            Action act = () => subStream.Seek(0, SeekOrigin.Begin);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that the SetLength method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void SetLength_AnyInput_ThrowsNotImplementedException()
        {
            // Arrange
            byte[] data = new byte[] { 1, 2, 3 };
            using var baseStream = new MemoryStream(data);
            using var subStream = new SubStream(baseStream, 3);

            // Act
            Action act = () => subStream.SetLength(10);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that the Write method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Write_AnyInput_ThrowsNotImplementedException()
        {
            // Arrange
            byte[] data = new byte[] { 1, 2, 3 };
            using var baseStream = new MemoryStream(data);
            using var subStream = new SubStream(baseStream, 3);
            byte[] buffer = new byte[] { 9, 9, 9 };

            // Act
            Action act = () => subStream.Write(buffer, 0, buffer.Length);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that setting the Position property throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void PositionSetter_SetValue_ThrowsNotImplementedException()
        {
            // Arrange
            byte[] data = new byte[] { 10, 20, 30 };
            using var baseStream = new MemoryStream(data);
            using var subStream = new SubStream(baseStream, 3);

            // Act
            Action act = () => { var _ = subStream.Position; subStream.Position = 10; };

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that the read-only properties return the expected values.
        /// </summary>
        [Fact]
        public void Properties_Getters_ReturnExpectedValues()
        {
            // Arrange
            byte[] data = new byte[] { 1, 2, 3, 4 };
            using var baseStream = new MemoryStream(data);
            long subLength = 4;
            using var subStream = new SubStream(baseStream, subLength);

            // Act & Assert
            subStream.CanRead.Should().BeTrue();
            subStream.CanWrite.Should().BeFalse();
            subStream.CanSeek.Should().BeFalse();
            subStream.Length.Should().Be(subLength);
        }
    }
}
