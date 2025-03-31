// using System;
// using System.IO;
// using System.Text;
// using FluentAssertions;
// using Moq;
// using Xunit;
// using Microsoft.Build.Logging.StructuredLogger;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
// //     /// <summary> // [Error] (14-18)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'BuildEventArgsReaderTests'
// //     /// Unit tests for the <see cref="BuildEventArgsReader"/> class.
// //     /// </summary>
// //     public class BuildEventArgsReaderTests
// //     {
// //         private const int ForwardCompatibilityMinimalVersion = 18; // Assuming 18 is the minimal version
// // 
// //         /// <summary>
// //         /// Tests that the constructor correctly initializes the BuildEventArgsReader instance.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_ValidParameters_InitializesProperties()
// //         {
// //             // Arrange
// //             var memoryStream = new MemoryStream();
// //             // Write dummy data (an EndOfFile record) to satisfy reading requirement
// //             int endOfFile = (int)BinaryLogRecordKind.EndOfFile;
// //             memoryStream.Write(BitConverter.GetBytes(endOfFile), 0, 4);
// //             memoryStream.Position = 0;
// //             var binaryReader = new BinaryReader(memoryStream);
// //             int fileFormatVersion = ForwardCompatibilityMinimalVersion;
// // 
// //             // Act
// //             var reader = new BuildEventArgsReader(binaryReader, fileFormatVersion);
// // 
// //             // Assert
// //             reader.FileFormatVersion.Should().Be(fileFormatVersion);
// //             reader.Position.Should().Be(0);
// //         }
// // 
// //         /// <summary>
// //         /// Tests that setting SkipUnknownEvents when fileFormatVersion is less than the minimal forward compatibility version throws an exception.
// //         /// </summary>
// //         [Fact]
// //         public void SkipUnknownEvents_FileFormatVersionLessThanMinimum_ThrowsInvalidOperationException()
// //         {
// //             // Arrange
// //             var memoryStream = new MemoryStream();
// //             var binaryReader = new BinaryReader(memoryStream);
// //             int fileFormatVersion = ForwardCompatibilityMinimalVersion - 1;
// //             var reader = new BuildEventArgsReader(binaryReader, fileFormatVersion);
// // 
// //             // Act
// //             Action act = () => reader.SkipUnknownEvents = true;
// // 
// //             // Assert
// //             act.Should().Throw<InvalidOperationException>()
// //                 .WithMessage($"Forward compatible reading is not supported for file format version {fileFormatVersion} (needs >= 18).");
// //         }
// // 
// //         /// <summary>
// //         /// Tests that setting SkipUnknownEventParts when fileFormatVersion is less than the minimal forward compatibility version throws an exception.
// //         /// </summary>
// //         [Fact]
// //         public void SkipUnknownEventParts_FileFormatVersionLessThanMinimum_ThrowsInvalidOperationException()
// //         {
// //             // Arrange
// //             var memoryStream = new MemoryStream();
// //             var binaryReader = new BinaryReader(memoryStream);
// //             int fileFormatVersion = ForwardCompatibilityMinimalVersion - 1;
// //             var reader = new BuildEventArgsReader(binaryReader, fileFormatVersion);
// // 
// //             // Act
// //             Action act = () => reader.SkipUnknownEventParts = true;
// // 
// //             // Assert
// //             act.Should().Throw<InvalidOperationException>()
// //                 .WithMessage($"Forward compatible reading is not supported for file format version {fileFormatVersion} (needs >= 18).");
// //         }
// // 
// //         /// <summary>
// //         /// Tests that the Read method returns null when the stream only contains an EndOfFile record.
// //         /// </summary>
// //         [Fact]
// //         public void Read_StreamContainsOnlyEndOfFile_ReturnsNull()
// //         {
// //             // Arrange
// //             var memoryStream = new MemoryStream();
// //             int endOfFile = (int)BinaryLogRecordKind.EndOfFile;
// //             memoryStream.Write(BitConverter.GetBytes(endOfFile), 0, 4);
// //             memoryStream.Position = 0;
// //             var binaryReader = new BinaryReader(memoryStream);
// //             int fileFormatVersion = ForwardCompatibilityMinimalVersion;
// //             var reader = new BuildEventArgsReader(binaryReader, fileFormatVersion);
// //             // Subscribe to error event to satisfy error-checking in Read()
// //             reader.RecoverableReadError += _ => { };
// // 
// //             // Act
// //             var result = reader.Read();
// // 
// //             // Assert
// //             result.Should().BeNull();
// //         }
// // 
// //         /// <summary>
// //         /// Tests that calling Read when SkipUnknownEvents is set but no RecoverableReadError handler is subscribed throws an exception.
// //         /// </summary>
// //         [Fact]
// //         public void Read_SkipUnknownEventsWithoutErrorHandler_ThrowsInvalidOperationException()
// //         {
// //             // Arrange
// //             var memoryStream = new MemoryStream();
// //             int endOfFile = (int)BinaryLogRecordKind.EndOfFile;
// //             memoryStream.Write(BitConverter.GetBytes(endOfFile), 0, 4);
// //             memoryStream.Position = 0;
// //             var binaryReader = new BinaryReader(memoryStream);
// //             int fileFormatVersion = ForwardCompatibilityMinimalVersion;
// //             var reader = new BuildEventArgsReader(binaryReader, fileFormatVersion);
// //             // Initially, do not subscribe to RecoverableReadError.
// //             // Setting SkipUnknownEvents will not throw immediately; the exception is thrown in Read() due to CheckErrorsSubscribed.
// //             reader.SkipUnknownEvents = true;
// // 
// //             // Act
// //             Action act = () => reader.Read();
// // 
// //             // Assert
// //             act.Should().Throw<InvalidOperationException>()
// //                 .WithMessage("*Binlog_MissingRecoverableErrorSubscribeError*");
// //         }
// // 
// //         /// <summary>
// //         /// Tests that Dispose disposes the underlying binary reader when CloseInput is set to true.
// //         /// </summary>
// //         [Fact]
// //         public void Dispose_CloseInputTrue_DisposesBinaryReader()
// //         {
// //             // Arrange
// //             var fakeStream = new FakeStream();
// //             var binaryReader = new BinaryReader(fakeStream);
// //             int fileFormatVersion = ForwardCompatibilityMinimalVersion;
// //             var reader = new BuildEventArgsReader(binaryReader, fileFormatVersion)
// //             {
// //                 CloseInput = true
// //             };
// // 
// //             // Act
// //             reader.Dispose();
// // 
// //             // Assert
// //             fakeStream.WasDisposed.Should().BeTrue();
// //         }
// // 
// //         /// <summary>
// //         /// Tests that the Position property returns the correct value of the underlying stream.
// //         /// </summary>
// //         [Fact]
// //         public void Position_ReturnsUnderlyingStreamPosition()
// //         {
// //             // Arrange
// //             var buffer = new byte[100];
// //             var memoryStream = new MemoryStream(buffer);
// //             memoryStream.Position = 42;
// //             var binaryReader = new BinaryReader(memoryStream);
// //             int fileFormatVersion = ForwardCompatibilityMinimalVersion;
// //             var reader = new BuildEventArgsReader(binaryReader, fileFormatVersion);
// // 
// //             // Act
// //             var position = reader.Position;
// // 
// //             // Assert
// //             position.Should().Be(42);
// //         }
// // 
// //         /// <summary>
// //         /// A fake stream to test disposal behavior.
// //         /// </summary>
// //         private class FakeStream : MemoryStream
// //         {
// //             public bool WasDisposed { get; private set; } = false;
// // 
// //             protected override void Dispose(bool disposing)
// //             {
// //                 WasDisposed = true;
// //                 base.Dispose(disposing);
// //             }
// //         }
// //     }
// // }