// using System; [Error] (497-1)CS1038 #endregion directive expected
// using System.IO;
// using System.Threading;
// using System.Threading.Tasks;
// using DotUtils.StreamUtils;
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
//         /// <summary>
//         /// A test stream that overrides functionality for verifying flush and close calls.
//         /// </summary>
//         private class TestMemoryStream : MemoryStream
//         {
//             public bool FlushCalled { get; private set; }
//             public bool FlushAsyncCalled { get; private set; }
//             public bool CloseCalled { get; private set; }
// 
//             public TestMemoryStream(byte[] buffer) : base(buffer)
//             {
//             }
// 
//             public override void Flush()
//             {
//                 FlushCalled = true;
//                 base.Flush();
//             }
// 
//             public override Task FlushAsync(CancellationToken cancellationToken)
//             {
//                 FlushAsyncCalled = true;
//                 return base.FlushAsync(cancellationToken);
//             }
// 
//             public override void Close()
//             {
//                 CloseCalled = true;
//                 base.Close();
//             }
//         }
// 
//         /// <summary>
//         /// A non-seekable stream for testing purposes.
//         /// </summary>
//         private class NonSeekableStream : MemoryStream
//         {
//             public override bool CanSeek => false;
//         }
// 
//         #region Static Methods Tests
// 
//         /// <summary>
//         /// Tests that EnsureSeekableStream returns the original stream if it is already seekable.
//         /// </summary>
//         [Fact]
//         public void EnsureSeekableStream_WhenStreamIsSeekable_ReturnsOriginalStream()
//         {
//             // Arrange
//             var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
// 
//             // Act
//             Stream result = TransparentReadStream.EnsureSeekableStream(memoryStream);
// 
//             // Assert
//             Assert.Same(memoryStream, result);
//         }
// 
//         /// <summary>
//         /// Tests that EnsureSeekableStream wraps a non-seekable but readable stream inside a TransparentReadStream.
//         /// </summary>
//         [Fact]
//         public void EnsureSeekableStream_WhenStreamIsNotSeekableButReadable_ReturnsTransparentReadStream()
//         {
//             // Arrange
//             var nonSeekable = new NonSeekableStream();
// 
//             // Act
//             Stream result = TransparentReadStream.EnsureSeekableStream(nonSeekable);
// 
//             // Assert
//             Assert.IsType<TransparentReadStream>(result);
//         }
// 
//         /// <summary>
//         /// Tests that EnsureSeekableStream throws an InvalidOperationException when the stream is not readable.
//         /// </summary>
//         [Fact]
//         public void EnsureSeekableStream_WhenStreamIsNotReadable_ThrowsInvalidOperationException()
//         {
//             // Arrange
//             var nonReadable = new NonReadableStream();
// 
//             // Act & Assert
//             Assert.Throws<InvalidOperationException>(() => TransparentReadStream.EnsureSeekableStream(nonReadable));
//         }
// 
//         /// <summary>
//         /// Tests that EnsureTransparentReadStream returns the same instance if the stream is already a TransparentReadStream.
//         /// </summary>
//         [Fact] [Error] (111-59)CS0122 'TransparentReadStream.TransparentReadStream(Stream)' is inaccessible due to its protection level
//         public void EnsureTransparentReadStream_WhenStreamIsTransparentReadStream_ReturnsSameInstance()
//         {
//             // Arrange
//             var underlying = new MemoryStream(new byte[] { 1, 2, 3 });
//             TransparentReadStream transparentStream = new TransparentReadStream(underlying);
//             // Act
//             TransparentReadStream result = TransparentReadStream.EnsureTransparentReadStream(transparentStream);
// 
//             // Assert
//             Assert.Same(transparentStream, result);
//         }
// 
//         /// <summary>
//         /// Tests that EnsureTransparentReadStream wraps a normal readable stream in a TransparentReadStream.
//         /// </summary>
//         [Fact]
//         public void EnsureTransparentReadStream_WhenStreamIsNotTransparentReadStream_ReturnsNewTransparentReadStream()
//         {
//             // Arrange
//             var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
// 
//             // Act
//             TransparentReadStream result = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
// 
//             // Assert
//             Assert.IsType<TransparentReadStream>(result);
//         }
// 
//         /// <summary>
//         /// Tests that EnsureTransparentReadStream throws an InvalidOperationException when the stream is not readable.
//         /// </summary>
//         [Fact]
//         public void EnsureTransparentReadStream_WhenStreamIsNotReadable_ThrowsInvalidOperationException()
//         {
//             // Arrange
//             var nonReadable = new NonReadableStream();
// 
//             // Act & Assert
//             Assert.Throws<InvalidOperationException>(() => TransparentReadStream.EnsureTransparentReadStream(nonReadable));
//         }
// 
//         #endregion
// 
//         #region Instance Method Tests
// 
//         /// <summary>
//         /// Tests that setting BytesCountAllowedToRead and reading bytes updates BytesCountAllowedToReadRemaining correctly.
//         /// </summary>
//         [Fact]
//         public void BytesCountAllowedToRead_SetAndRead_UpdatesRemainingBytesCorrectly()
//         {
//             // Arrange
//             byte[] data = new byte[10];
//             for (int i = 0; i < data.Length; i++)
//             {
//                 data[i] = (byte)i;
//             }
//             var memoryStream = new MemoryStream(data);
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
//             trs.BytesCountAllowedToRead = 5; // Allow only first 5 bytes
// 
//             byte[] buffer = new byte[3];
// 
//             // Act
//             int readCount1 = trs.Read(buffer, 0, buffer.Length);
// 
//             // Assert
//             Assert.Equal(3, readCount1);
//             Assert.Equal(2, trs.BytesCountAllowedToReadRemaining);
//         }
// 
//         /// <summary>
//         /// Tests that Read(byte[], int, int) reads fewer bytes when the allowed read limit is reached.
//         /// </summary>
//         [Fact]
//         public void Read_WhenCountExceedsAllowedLimit_ReadsOnlyAllowedBytes()
//         {
//             // Arrange
//             byte[] data = new byte[10];
//             for (int i = 0; i < 10; i++)
//             {
//                 data[i] = (byte)(i + 1);
//             }
//             var memoryStream = new MemoryStream(data);
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
//             trs.BytesCountAllowedToRead = 4; // allow only 4 bytes
// 
//             byte[] buffer = new byte[6];
// 
//             // Act
//             int readCount = trs.Read(buffer, 0, buffer.Length);
// 
//             // Assert
//             Assert.Equal(4, readCount);
//             // Verify that the data read is correct (first 4 bytes)
//             for (int i = 0; i < 4; i++)
//             {
//                 Assert.Equal((byte)(i + 1), buffer[i]);
//             }
//         }
// 
//         /// <summary>
//         /// Tests that ReadByte returns correct byte when within allowed limit.
//         /// </summary>
//         [Fact]
//         public void ReadByte_WhenWithinAllowedLimit_ReturnsByteAndAdvancesPosition()
//         {
//             // Arrange
//             byte[] data = new byte[] { 10, 20, 30 };
//             var memoryStream = new MemoryStream(data);
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
//             trs.BytesCountAllowedToRead = 2; // allow only two bytes
// 
//             // Act & Assert
//             int firstByte = trs.ReadByte();
//             Assert.Equal(10, firstByte);
//             int secondByte = trs.ReadByte();
//             Assert.Equal(20, secondByte);
//             // Third read should exceed allowed limit
//             int thirdByte = trs.ReadByte();
//             Assert.Equal(-1, thirdByte);
//         }
// 
//         /// <summary>
//         /// Tests that ReadAsync(byte[], int, int, CancellationToken) reads the correct number of bytes considering allowed limit.
//         /// </summary>
//         [Fact]
//         public async Task ReadAsync_WhenCalled_ReadsCorrectlyWithinAllowedLimit()
//         {
//             // Arrange
//             byte[] data = new byte[10];
//             for (int i = 0; i < data.Length; i++)
//             {
//                 data[i] = (byte)(i + 2);
//             }
//             var memoryStream = new MemoryStream(data);
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
//             trs.BytesCountAllowedToRead = 5;
// 
//             byte[] buffer = new byte[10];
// 
//             // Act
//             int readCount = await trs.ReadAsync(buffer, 0, buffer.Length, CancellationToken.None);
// 
//             // Assert
//             Assert.Equal(5, readCount);
//             for (int i = 0; i < readCount; i++)
//             {
//                 Assert.Equal((byte)(i + 2), buffer[i]);
//             }
//         }
// 
// #if NET
//         /// <summary>
//         /// Tests that Read(Span<byte>) reads the correct number of bytes considering the allowed limit.
//         /// </summary>
//         [Fact]
//         public void ReadSpan_WhenCalled_ReadsCorrectlyWithinAllowedLimit()
//         {
//             // Arrange
//             byte[] data = new byte[8];
//             for (int i = 0; i < data.Length; i++)
//             {
//                 data[i] = (byte)(i + 5);
//             }
//             var memoryStream = new MemoryStream(data);
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
//             trs.BytesCountAllowedToRead = 3;
//             Span<byte> buffer = new byte[5];
// 
//             // Act
//             int readCount = trs.Read(buffer);
// 
//             // Assert
//             Assert.Equal(3, readCount);
//             for (int i = 0; i < readCount; i++)
//             {
//                 Assert.Equal((byte)(i + 5), buffer[i]);
//             }
//         }
// 
//         /// <summary>
//         /// Tests that ReadAsync(Memory<byte>, CancellationToken) reads the correct number of bytes considering allowed limit.
//         /// </summary>
//         [Fact]
//         public async Task ReadAsyncMemory_WhenCalled_ReadsCorrectlyWithinAllowedLimit()
//         {
//             // Arrange
//             byte[] data = new byte[8];
//             for (int i = 0; i < data.Length; i++)
//             {
//                 data[i] = (byte)(i + 8);
//             }
//             var memoryStream = new MemoryStream(data);
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
//             trs.BytesCountAllowedToRead = 4;
//             Memory<byte> buffer = new byte[6];
// 
//             // Act
//             int readCount = await trs.ReadAsync(buffer, CancellationToken.None);
// 
//             // Assert
//             Assert.Equal(4, readCount);
//             for (int i = 0; i < readCount; i++)
//             {
//                 Assert.Equal((byte)(i + 8), buffer.Span[i]);
//             }
//         }
// #endif
// 
//         /// <summary>
//         /// Tests that Seek with SeekOrigin.Current advances the position correctly.
//         /// </summary>
//         [Fact]
//         public void Seek_WithSeekOriginCurrent_AdvancesPosition()
//         {
//             // Arrange
//             byte[] data = new byte[20];
//             for (int i = 0; i < data.Length; i++)
//             {
//                 data[i] = (byte)(i);
//             }
//             var memoryStream = new MemoryStream(data);
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
// 
//             // Act
//             long newPosition = trs.Seek(5, SeekOrigin.Current);
// 
//             // Assert
//             Assert.Equal(5, newPosition);
// 
//             // Further seek forward using Position setter
//             trs.Position = 10;
//             Assert.Equal(10, trs.Position);
//         }
// 
//         /// <summary>
//         /// Tests that Seek throws a NotSupportedException when using a non-supported SeekOrigin.
//         /// </summary>
//         [Fact]
//         public void Seek_WithNonCurrentOrigin_ThrowsNotSupportedException()
//         {
//             // Arrange
//             var memoryStream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
// 
//             // Act & Assert
//             Assert.Throws<NotSupportedException>(() => trs.Seek(2, SeekOrigin.Begin));
//         }
// 
//         /// <summary>
//         /// Tests that SetLength throws NotSupportedException.
//         /// </summary>
//         [Fact]
//         public void SetLength_Always_ThrowsNotSupportedException()
//         {
//             // Arrange
//             var memoryStream = new MemoryStream(new byte[] { 1, 2, 3 });
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
// 
//             // Act & Assert
//             Assert.Throws<NotSupportedException>(() => trs.SetLength(10));
//         }
// 
//         /// <summary>
//         /// Tests that Write always throws NotSupportedException.
//         /// </summary>
//         [Fact]
//         public void Write_Always_ThrowsNotSupportedException()
//         {
//             // Arrange
//             var memoryStream = new MemoryStream(new byte[] { 10, 20, 30 });
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
//             byte[] buffer = new byte[5];
// 
//             // Act & Assert
//             Assert.Throws<NotSupportedException>(() => trs.Write(buffer, 0, buffer.Length));
//         }
// 
//         /// <summary>
//         /// Tests that Flush calls the underlying stream's Flush method.
//         /// </summary>
//         [Fact]
//         public void Flush_InvokesUnderlyingStreamFlush()
//         {
//             // Arrange
//             byte[] data = new byte[5];
//             var testStream = new TestMemoryStream(data);
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(testStream);
// 
//             // Act
//             trs.Flush();
// 
//             // Assert
//             Assert.True(testStream.FlushCalled);
//         }
// 
//         /// <summary>
//         /// Tests that FlushAsync calls the underlying stream's FlushAsync method.
//         /// </summary>
//         [Fact]
//         public async Task FlushAsync_InvokesUnderlyingStreamFlushAsync()
//         {
//             // Arrange
//             byte[] data = new byte[5];
//             var testStream = new TestMemoryStream(data);
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(testStream);
// 
//             // Act
//             await trs.FlushAsync(CancellationToken.None);
// 
//             // Assert
//             Assert.True(testStream.FlushAsyncCalled);
//         }
// 
//         /// <summary>
//         /// Tests that setting the Position property forward works correctly.
//         /// </summary>
//         [Fact]
//         public void Position_SetForward_UpdatesPosition()
//         {
//             // Arrange
//             byte[] data = new byte[15];
//             for (int i = 0; i < data.Length; i++)
//             {
//                 data[i] = (byte)i;
//             }
//             var memoryStream = new MemoryStream(data);
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
// 
//             // Act
//             trs.Position = 5;
// 
//             // Assert
//             Assert.Equal(5, trs.Position);
//         }
// 
//         /// <summary>
//         /// Tests that setting the Position property to a value lower than the current position throws an exception.
//         /// </summary>
//         [Fact]
//         public void Position_SetBackward_ThrowsException()
//         {
//             // Arrange
//             byte[] data = new byte[10];
//             var memoryStream = new MemoryStream(data);
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(memoryStream);
//             // Advance forward
//             trs.Position = 5;
// 
//             // Act & Assert
//             Assert.ThrowsAny<Exception>(() => trs.Position = 3);
//         }
// 
//         /// <summary>
//         /// Tests that Close calls the underlying stream's Close method.
//         /// </summary>
//         [Fact]
//         public void Close_InvokesUnderlyingStreamClose()
//         {
//             // Arrange
//             byte[] data = new byte[5];
//             var testStream = new TestMemoryStream(data);
//             TransparentReadStream trs = TransparentReadStream.EnsureTransparentReadStream(testStream);
// 
//             // Act
//             trs.Close();
// 
//             // Assert
//             Assert.True(testStream.CloseCalled);
//         }
//     }
// 
//     /// <summary>
//     /// A non-readable stream used for testing exception scenarios.
//     /// </summary>
//     internal class NonReadableStream : Stream
//     {
//         public override bool CanRead => false;
//         public override bool CanSeek => false;
//         public override bool CanWrite => false;
//         public override long Length => 0;
//         public override long Position { get => 0; set => throw new NotSupportedException(); }
//         public override void Flush() => throw new NotSupportedException();
//         public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
//         public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
//         public override void SetLength(long value) => throw new NotSupportedException();
//         public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
//     }
// }
// 