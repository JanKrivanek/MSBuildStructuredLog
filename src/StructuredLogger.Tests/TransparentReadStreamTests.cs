// using System;
// using System.IO;
// using System.Threading;
// using System.Threading.Tasks;
// using DotUtils.StreamUtils;
// using FluentAssertions;
// using Moq;
// using Xunit;
// 
// namespace DotUtils.StreamUtils.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="TransparentReadStream"/> class.
//     /// </summary>
//     public class TransparentReadStreamTests
//     {
//         // Helper method to create a non-seekable, readable stream using Moq.
//         private static Stream CreateNonSeekableReadableStream()
//         {
//             var mockStream = new Mock<Stream>(MockBehavior.Strict);
//             mockStream.SetupGet(s => s.CanSeek).Returns(false);
//             mockStream.SetupGet(s => s.CanRead).Returns(true);
//             // Setup Read to return 0 bytes read (end-of-stream) by default.
//             mockStream.Setup(s => s.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
//                       .Returns(0);
//             // Setup ReadByte to return -1 by default.
//             mockStream.Setup(s => s.ReadByte()).Returns(-1);
//             // Setup Flush and FlushAsync to do nothing.
//             mockStream.Setup(s => s.Flush());
//             mockStream.Setup(s => s.FlushAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
//             // Setup Length property (though not used) - can return 0.
//             mockStream.SetupGet(s => s.Length).Returns(0);
//             // Setup Seek (should not be called since CanSeek is false).
//             mockStream.Setup(s => s.Seek(It.IsAny<long>(), It.IsAny<SeekOrigin>()))
//                       .Throws(new NotSupportedException());
//             // Setup Close to do nothing.
//             mockStream.Setup(s => s.Close());
//             return mockStream.Object;
//         }
// 
//         /// <summary>
//         /// Tests that EnsureSeekableStream returns the same stream if the stream is already seekable.
//         /// </summary>
//         [Fact]
//         public void EnsureSeekableStream_StreamIsSeekable_ReturnsSameStream()
//         {
//             // Arrange
//             using var memoryStream = new MemoryStream();
// 
//             // Act
//             Stream result = TransparentReadStream.EnsureSeekableStream(memoryStream);
// 
//             // Assert
//             result.Should().BeSameAs(memoryStream);
//         }
// 
//         /// <summary>
//         /// Tests that EnsureSeekableStream wraps a non-seekable stream in a TransparentReadStream.
//         /// </summary>
//         [Fact]
//         public void EnsureSeekableStream_StreamIsNotSeekable_ReturnsTransparentReadStream()
//         {
//             // Arrange
//             Stream nonSeekableStream = CreateNonSeekableReadableStream();
// 
//             // Act
//             Stream result = TransparentReadStream.EnsureSeekableStream(nonSeekableStream);
// 
//             // Assert
//             result.Should().BeOfType<TransparentReadStream>();
//         }
// 
//         /// <summary>
//         /// Tests that EnsureSeekableStream throws an InvalidOperationException when the stream is not readable.
//         /// </summary>
//         [Fact]
//         public void EnsureSeekableStream_StreamNotReadable_ThrowsInvalidOperationException()
//         {
//             // Arrange
//             var mockStream = new Mock<Stream>(MockBehavior.Strict);
//             mockStream.SetupGet(s => s.CanSeek).Returns(false);
//             mockStream.SetupGet(s => s.CanRead).Returns(false);
//             Stream nonReadableStream = mockStream.Object;
// 
//             // Act
//             Action act = () => TransparentReadStream.EnsureSeekableStream(nonReadableStream);
// 
//             // Assert
//             act.Should().Throw<InvalidOperationException>()
//                 .WithMessage("Stream must be readable.");
//         }
// //  // [Error] (101-59)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
// //         /// <summary>
// //         /// Tests that EnsureTransparentReadStream returns the same instance when the stream is already a TransparentReadStream.
// //         /// </summary>
// //         [Fact]
// //         public void EnsureTransparentReadStream_AlreadyTransparent_ReturnsSameInstance()
// //         {
// //             // Arrange
// //             using var innerStream = new MemoryStream();
// //             TransparentReadStream transparentStream = new TransparentReadStream(innerStream);
// // 
// //             // Act
// //             TransparentReadStream result = TransparentReadStream.EnsureTransparentReadStream(transparentStream);
// // 
// //             // Assert
// //             result.Should().BeSameAs(transparentStream);
// //         }
// // 
//         /// <summary>
//         /// Tests that EnsureTransparentReadStream wraps a non-transparent but readable stream in a TransparentReadStream.
//         /// </summary>
//         [Fact]
//         public void EnsureTransparentReadStream_NotTransparentButReadable_ReturnsTransparentReadStream()
//         {
//             // Arrange
//             using var memoryStream = new MemoryStream();
// 
//             // Act
//             TransparentReadStream result = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
// 
//             // Assert
//             result.Should().NotBeSameAs(memoryStream);
//             result.Should().BeOfType<TransparentReadStream>();
//         }
// 
//         /// <summary>
//         /// Tests that EnsureTransparentReadStream throws an InvalidOperationException when the stream is not readable.
//         /// </summary>
//         [Fact]
//         public void EnsureTransparentReadStream_StreamNotReadable_ThrowsInvalidOperationException()
//         {
//             // Arrange
//             var mockStream = new Mock<Stream>(MockBehavior.Strict);
//             mockStream.SetupGet(s => s.CanRead).Returns(false);
//             Stream nonReadableStream = mockStream.Object;
// 
//             // Act
//             Action act = () => TransparentReadStream.EnsureTransparentReadStream(nonReadableStream);
// 
//             // Assert
//             act.Should().Throw<InvalidOperationException>()
//                 .WithMessage("Stream must be readable.");
//         }
// //  // [Error] (156-41)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
// //         /// <summary>
// //         /// Tests the BytesCountAllowedToRead setter and BytesCountAllowedToReadRemaining property.
// //         /// Sets a read limit, reads some bytes, and verifies the remaining allowed bytes.
// //         /// </summary>
// //         [Fact]
// //         public void BytesCountAllowedToRead_SetAllowedAndRead_UpdatesRemainingCount()
// //         {
// //             // Arrange
// //             byte[] data = { 10, 20, 30, 40, 50 };
// //             using var memoryStream = new MemoryStream(data);
// //             var transparentStream = new TransparentReadStream(memoryStream);
// //             
// //             // Set allowed read count to 3 bytes.
// //             transparentStream.BytesCountAllowedToRead = 3;
// //             transparentStream.BytesCountAllowedToReadRemaining.Should().Be(3);
// // 
// //             byte[] buffer = new byte[2];
// // 
// //             // Act: Read 2 bytes.
// //             int bytesRead = transparentStream.Read(buffer, 0, buffer.Length);
// // 
// //             // Assert
// //             bytesRead.Should().Be(2);
// //             transparentStream.BytesCountAllowedToReadRemaining.Should().Be(1);
// // 
// //             // Act: Attempt to read 2 more bytes, but only 1 allowed.
// //             bytesRead = transparentStream.Read(buffer, 0, buffer.Length);
// // 
// //             // Assert
// //             bytesRead.Should().Be(1);
// //             transparentStream.BytesCountAllowedToReadRemaining.Should().Be(0);
// //         }
// //  // [Error] (188-41)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
//         /// <summary>
//         /// Tests that Read method reads at most the allowed number of bytes when a limit is set.
//         /// </summary>
//         [Fact]
//         public void Read_WithAllowedLimit_ReadsOnlyAllowedBytes()
//         {
//             // Arrange
//             byte[] data = { 1, 2, 3, 4, 5 };
//             using var memoryStream = new MemoryStream(data);
//             var transparentStream = new TransparentReadStream(memoryStream);
//             // Set allowed read count to 3 bytes.
//             transparentStream.BytesCountAllowedToRead = 3;
//             byte[] buffer = new byte[5];
// 
//             // Act
//             int totalRead = transparentStream.Read(buffer, 0, buffer.Length);
// 
//             // Assert
//             totalRead.Should().Be(3);
//             buffer[0].Should().Be(1);
//             buffer[1].Should().Be(2);
//             buffer[2].Should().Be(3);
//         }
// //  // [Error] (212-41)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
// //         /// <summary>
// //         /// Tests that ReadByte returns the next byte if allowed, and -1 if the allowed limit is reached.
// //         /// </summary>
// //         [Fact]
// //         public void ReadByte_WithAllowedLimit_ReadsOnlyAllowedByte()
// //         {
// //             // Arrange
// //             byte[] data = { 100, 101 };
// //             using var memoryStream = new MemoryStream(data);
// //             var transparentStream = new TransparentReadStream(memoryStream);
// //             // Set allowed read count to 1 byte.
// //             transparentStream.BytesCountAllowedToRead = 1;
// // 
// //             // Act
// //             int firstByte = transparentStream.ReadByte();
// //             int secondByte = transparentStream.ReadByte();
// // 
// //             // Assert
// //             firstByte.Should().Be(100);
// //             secondByte.Should().Be(-1);
// //         }
// //  // [Error] (234-41)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
//         /// <summary>
//         /// Tests that ReadAsync reads asynchronously and respects the allowed bytes limit.
//         /// </summary>
//         [Fact]
//         public async Task ReadAsync_WithAllowedLimit_ReadsOnlyAllowedBytes()
//         {
//             // Arrange
//             byte[] data = { 10, 20, 30, 40 };
//             using var memoryStream = new MemoryStream(data);
//             var transparentStream = new TransparentReadStream(memoryStream);
//             transparentStream.BytesCountAllowedToRead = 2;
//             byte[] buffer = new byte[4];
// 
//             // Act
//             int bytesRead = await transparentStream.ReadAsync(buffer, 0, buffer.Length, CancellationToken.None);
// 
//             // Assert
//             bytesRead.Should().Be(2);
//             buffer[0].Should().Be(10);
//             buffer[1].Should().Be(20);
//         }
// //  // [Error] (302-41)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
// // #if NET
// //         /// <summary>
// //         /// Tests that Read(Span<byte>) reads synchronously and respects the allowed bytes limit.
// //         /// </summary>
// //         [Fact]
// //         public void ReadSpan_WithAllowedLimit_ReadsOnlyAllowedBytes()
// //         {
// //             // Arrange
// //             byte[] data = { 5, 6, 7, 8 };
// //             using var memoryStream = new MemoryStream(data);
// //             var transparentStream = new TransparentReadStream(memoryStream);
// //             transparentStream.BytesCountAllowedToRead = 3;
// //             Span<byte> buffer = new byte[4];
// // 
// //             // Act
// //             int bytesRead = transparentStream.Read(buffer);
// // 
// //             // Assert
// //             bytesRead.Should().Be(3);
// //             buffer[0].Should().Be(5);
// //             buffer[1].Should().Be(6);
// //             buffer[2].Should().Be(7);
// //         }
// // 
// //         /// <summary>
// //         /// Tests that ReadAsync with Memory<byte> reads asynchronously and respects the allowed bytes limit.
// //         /// </summary>
// //         [Fact]
// //         public async Task ReadAsyncMemory_WithAllowedLimit_ReadsOnlyAllowedBytes()
// //         {
// //             // Arrange
// //             byte[] data = { 11, 22, 33, 44 };
// //             using var memoryStream = new MemoryStream(data);
// //             var transparentStream = new TransparentReadStream(memoryStream);
// //             transparentStream.BytesCountAllowedToRead = 2;
// //             Memory<byte> buffer = new byte[4];
// // 
// //             // Act
// //             int bytesRead = await transparentStream.ReadAsync(buffer, CancellationToken.None);
// // 
// //             // Assert
// //             bytesRead.Should().Be(2);
// //             buffer.Span[0].Should().Be(11);
// //             buffer.Span[1].Should().Be(22);
// //         }
// // #endif
// // 
// //         /// <summary>
// //         /// Tests that Seek throws a NotSupportedException when the origin is not SeekOrigin.Current.
// //         /// </summary>
// //         [Fact]
// //         public void Seek_NonCurrentOrigin_ThrowsNotSupportedException()
// //         {
// //             // Arrange
// //             using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
// //             var transparentStream = new TransparentReadStream(memoryStream);
// // 
// //             // Act
// //             Action act = () => transparentStream.Seek(1, SeekOrigin.Begin);
// // 
// //             // Assert
// //             act.Should().Throw<NotSupportedException>()
// //                .WithMessage("Only seeking from SeekOrigin.Current is supported.");
// //         }
// //  // [Error] (321-41)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
//         /// <summary>
//         /// Tests that Seek with SeekOrigin.Current skips the specified number of bytes.
//         /// </summary>
//         [Fact]
//         public void Seek_Current_SkipsBytesAndUpdatesPosition()
//         {
//             // Arrange
//             byte[] data = { 9, 8, 7, 6, 5 };
//             using var memoryStream = new MemoryStream(data);
//             var transparentStream = new TransparentReadStream(memoryStream);
// 
//             // Act: Skip 2 bytes.
//             long newPosition = transparentStream.Seek(2, SeekOrigin.Current);
// 
//             // Assert: Position should be 2.
//             newPosition.Should().Be(2);
// 
//             // Next read should return the 3rd byte of the underlying stream.
//             int nextByte = transparentStream.ReadByte();
//             nextByte.Should().Be(7);
//         }
// //  // [Error] (342-41)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
// //         /// <summary>
// //         /// Tests that SetLength always throws a NotSupportedException.
// //         /// </summary>
// //         [Fact]
// //         public void SetLength_Always_ThrowsNotSupportedException()
// //         {
// //             // Arrange
// //             using var memoryStream = new MemoryStream();
// //             var transparentStream = new TransparentReadStream(memoryStream);
// // 
// //             // Act
// //             Action act = () => transparentStream.SetLength(100);
// // 
// //             // Assert
// //             act.Should().Throw<NotSupportedException>()
// //                 .WithMessage("Expanding stream is not supported.");
// //         }
// //  // [Error] (360-41)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
//         /// <summary>
//         /// Tests that Write always throws a NotSupportedException.
//         /// </summary>
//         [Fact]
//         public void Write_Always_ThrowsNotSupportedException()
//         {
//             // Arrange
//             using var memoryStream = new MemoryStream();
//             var transparentStream = new TransparentReadStream(memoryStream);
//             byte[] buffer = { 1, 2, 3 };
// 
//             // Act
//             Action act = () => transparentStream.Write(buffer, 0, buffer.Length);
// 
//             // Assert
//             act.Should().Throw<NotSupportedException>()
//                 .WithMessage("Writing is not supported.");
//         }
// //  // [Error] (379-41)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
// //         /// <summary>
// //         /// Tests that Flush calls the underlying stream's Flush without throwing exceptions.
// //         /// </summary>
// //         [Fact]
// //         public void Flush_CallsUnderlyingStreamFlush()
// //         {
// //             // Arrange
// //             using var memoryStream = new MemoryStream();
// //             var transparentStream = new TransparentReadStream(memoryStream);
// // 
// //             // Act
// //             Action act = () => transparentStream.Flush();
// // 
// //             // Assert
// //             act.Should().NotThrow();
// //         }
// //  // [Error] (396-41)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
//         /// <summary>
//         /// Tests that FlushAsync calls the underlying stream's FlushAsync without throwing exceptions.
//         /// </summary>
//         [Fact]
//         public async Task FlushAsync_CallsUnderlyingStreamFlushAsync()
//         {
//             // Arrange
//             using var memoryStream = new MemoryStream();
//             var transparentStream = new TransparentReadStream(memoryStream);
// 
//             // Act
//             Func<Task> act = async () => await transparentStream.FlushAsync(CancellationToken.None);
// 
//             // Assert
//             await act.Should().NotThrowAsync();
//         }
// //  // [Error] (414-41)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
// //         /// <summary>
// //         /// Tests that the Length property returns the underlying stream's length.
// //         /// </summary>
// //         [Fact]
// //         public void Length_ReturnsUnderlyingStreamLength()
// //         {
// //             // Arrange
// //             byte[] data = { 1, 2, 3, 4 };
// //             using var memoryStream = new MemoryStream(data);
// //             var transparentStream = new TransparentReadStream(memoryStream);
// // 
// //             // Act
// //             long length = transparentStream.Length;
// // 
// //             // Assert
// //             length.Should().Be(memoryStream.Length);
// //         }
// //  // [Error] (438-41)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
//         /// <summary>
//         /// Tests that Close calls the underlying stream's Close method.
//         /// </summary>
//         [Fact]
//         public void Close_CallsUnderlyingStreamClose()
//         {
//             // Arrange
//             var wasClosed = false;
//             var mockStream = new Mock<Stream>(MockBehavior.Strict);
//             mockStream.SetupGet(s => s.CanRead).Returns(true);
//             mockStream.SetupGet(s => s.CanSeek).Returns(true);
//             mockStream.SetupGet(s => s.Length).Returns(10);
//             mockStream.Setup(s => s.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>())).Returns(0);
//             mockStream.Setup(s => s.Close()).Callback(() => wasClosed = true);
//             Stream stream = mockStream.Object;
//             var transparentStream = new TransparentReadStream(stream);
// 
//             // Act
//             transparentStream.Close();
// 
//             // Assert
//             wasClosed.Should().BeTrue();
//         }
//     }
// }