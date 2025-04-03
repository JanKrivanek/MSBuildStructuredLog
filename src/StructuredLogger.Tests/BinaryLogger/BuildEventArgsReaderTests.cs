// using System;
// using System.IO;
// using System.Text;
// using FluentAssertions;
// using Moq;
// using Xunit;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
// //     /// <summary> // [Error] (15-18)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'BuildEventArgsReaderTests'
// //     /// Unit tests for the <see cref="BuildEventArgsReader"/> class.
// //     /// </summary>
// //     public class BuildEventArgsReaderTests : IDisposable
// //     {
// //         private MemoryStream _memoryStream;
// //         private BinaryReader _binaryReader;
// // 
// //         public BuildEventArgsReaderTests()
// //         {
// //             // Initialize an empty memory stream and a binary reader around it.
// //             _memoryStream = new MemoryStream();
// //             _binaryReader = new BinaryReader(_memoryStream, Encoding.UTF8, leaveOpen: true);
// //         }
// // 
// //         public void Dispose()
// //         {
// //             _binaryReader.Dispose();
// //             _memoryStream.Dispose();
// //         }
// // 
// //         /// <summary>
// //         /// Tests that setting SkipUnknownEvents to true throws an InvalidOperationException
// //         /// when the file format version is below the forward compatibility minimal version.
// //         /// </summary>
// //         [Fact]
// //         public void SkipUnknownEvents_WhenFileFormatVersionIsLessThanRequired_ThrowsInvalidOperationException()
// //         {
// //             // Arrange
// //             int fileFormatVersion = 17; // Assume forward compatibility requires 18 or higher.
// //             // Reset stream position (no data needed for this test).
// //             _memoryStream.Position = 0;
// //             var reader = new BuildEventArgsReader(_binaryReader, fileFormatVersion);
// // 
// //             // Act
// //             Action act = () => reader.SkipUnknownEvents = true;
// // 
// //             // Assert
// //             act.Should().Throw<InvalidOperationException>()
// //                 .WithMessage($"Forward compatible reading is not supported for file format version {fileFormatVersion}*");
// //         }
// // 
// //         /// <summary>
// //         /// Tests that setting SkipUnknownEventParts to true throws an InvalidOperationException
// //         /// when the file format version is below the forward compatibility minimal version.
// //         /// </summary>
// //         [Fact]
// //         public void SkipUnknownEventParts_WhenFileFormatVersionIsLessThanRequired_ThrowsInvalidOperationException()
// //         {
// //             // Arrange
// //             int fileFormatVersion = 17; // Forward compatibility not supported below version 18.
// //             _memoryStream.Position = 0;
// //             var reader = new BuildEventArgsReader(_binaryReader, fileFormatVersion);
// // 
// //             // Act
// //             Action act = () => reader.SkipUnknownEventParts = true;
// // 
// //             // Assert
// //             act.Should().Throw<InvalidOperationException>()
// //                 .WithMessage($"Forward compatible reading is not supported for file format version {fileFormatVersion}*");
// //         }
// // 
// //         /// <summary>
// //         /// Tests that the Position property returns the current position of the underlying stream.
// //         /// </summary>
// //         [Fact]
// //         public void Position_PropertyReturnsCurrentStreamPosition()
// //         {
// //             // Arrange
// //             long initialPosition = _memoryStream.Position;
// //             var reader = new BuildEventArgsReader(_binaryReader, 18);
// // 
// //             // Act
// //             long currentPosition = reader.Position;
// // 
// //             // Assert
// //             currentPosition.Should().Be(initialPosition);
// //         }
// // 
// //         /// <summary>
// //         /// Tests that Dispose disposes the underlying binary reader when CloseInput is set to true.
// //         /// </summary>
// //         [Fact]
// //         public void Dispose_WithCloseInputTrue_DisposesUnderlyingBinaryReader()
// //         {
// //             // Arrange
// //             var reader = new BuildEventArgsReader(_binaryReader, 18)
// //             {
// //                 CloseInput = true
// //             };
// // 
// //             // Act
// //             reader.Dispose();
// // 
// //             // Assert: The underlying stream should be closed.
// //             Action act = () => _memoryStream.WriteByte(0);
// //             act.Should().Throw<ObjectDisposedException>();
// //         }
// // 
// //         /// <summary>
// //         /// Tests that Dispose does not dispose the underlying binary reader when CloseInput is set to false.
// //         /// </summary>
// //         [Fact]
// //         public void Dispose_WithCloseInputFalse_DoesNotDisposeUnderlyingBinaryReader()
// //         {
// //             // Arrange
// //             var reader = new BuildEventArgsReader(_binaryReader, 18)
// //             {
// //                 CloseInput = false
// //             };
// // 
// //             // Act
// //             reader.Dispose();
// // 
// //             // Assert: The underlying stream should still be writable.
// //             Action act = () => _memoryStream.WriteByte(0);
// //             act.Should().NotThrow();
// //         }
// // 
// //         /// <summary>
// //         /// Tests that ReadRaw returns a RawRecord with Stream.Null when the record kind indicates EndOfFile.
// //         /// Assumes that the EndOfFile enum value is represented as 0.
// //         /// </summary>
// //         [Fact]
// //         public void ReadRaw_WhenRecordKindIsEndOfFile_ReturnsRawRecordWithStreamNull()
// //         {
// //             // Arrange
// //             int fileFormatVersion = 18;
// //             // Write 4 bytes representing the record kind value for EndOfFile (assumed to be 0).
// //             byte[] endOfFileBytes = BitConverter.GetBytes(0);
// //             _memoryStream.Write(endOfFileBytes, 0, endOfFileBytes.Length);
// //             _memoryStream.Position = 0;
// //             var reader = new BuildEventArgsReader(_binaryReader, fileFormatVersion);
// // 
// //             // Act
// //             var rawRecord = reader.ReadRaw();
// // 
// //             // Assert
// //             rawRecord.RecordKind.Should().Be(0);
// //             rawRecord.Stream.Should().BeSameAs(Stream.Null);
// //         }
// // 
// //         /// <summary>
// //         /// Tests that Read returns null when the underlying data indicates EndOfFile.
// //         /// This test subscribes to the RecoverableReadError event to satisfy the reader's subscription check.
// //         /// </summary>
// //         [Fact]
// //         public void Read_WhenUnderlyingDataIsEndOfFile_ReturnsNull()
// //         {
// //             // Arrange
// //             int fileFormatVersion = 18;
// //             // Write 4 bytes for the record kind (assumed to be 0 indicating EndOfFile).
// //             byte[] endOfFileBytes = BitConverter.GetBytes(0);
// //             _memoryStream.Write(endOfFileBytes, 0, endOfFileBytes.Length);
// //             _memoryStream.Position = 0;
// //             var reader = new BuildEventArgsReader(_binaryReader, fileFormatVersion);
// //             // Subscribe to the RecoverableReadError event.
// //             reader.RecoverableReadError += (e) => { };
// // 
// //             // Act
// //             var result = reader.Read();
// // 
// //             // Assert
// //             result.Should().BeNull();
// //         }
// //     }
// // }