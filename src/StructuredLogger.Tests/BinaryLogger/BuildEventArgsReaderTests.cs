using System;
using System.IO;
using System.Text;
using Moq;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;
using StructuredLogger.BinaryLogger; // for BinaryLogger and BinaryLogRecordKind

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildEventArgsReader"/> class.
    /// </summary>
//     public class BuildEventArgsReaderTests [Error] (14-18)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'BuildEventArgsReaderTests'
//     {
//         // We assume that BinaryLogger.ForwardCompatibilityMinimalVersion is 18.
//         private const int ForwardCompatibilityMinimalVersion = 18;
// 
//         /// <summary>
//         /// A helper MemoryStream subclass to track disposal.
//         /// </summary>
//         private class DisposableMemoryStream : MemoryStream
//         {
//             public bool IsDisposed { get; private set; } = false;
//             protected override void Dispose(bool disposing)
//             {
//                 base.Dispose(disposing);
//                 IsDisposed = true;
//             }
//         }
// 
//         /// <summary>
//         /// Creates a BuildEventArgsReader with the given stream and file format version.
//         /// </summary>
//         private BuildEventArgsReader CreateReader(Stream stream, int fileFormatVersion)
//         {
//             var binaryReader = new BinaryReader(stream, Encoding.UTF8);
//             return new BuildEventArgsReader(binaryReader, fileFormatVersion);
//         }
// 
//         /// <summary>
//         /// Tests that the constructor creates an instance when provided valid arguments.
//         /// </summary>
//         [Fact]
//         public void Constructor_ValidArguments_InstanceCreated()
//         {
//             // Arrange
//             using var stream = new MemoryStream();
//             
//             // Act
//             var reader = CreateReader(stream, ForwardCompatibilityMinimalVersion);
//             
//             // Assert
//             Assert.NotNull(reader);
//         }
// 
//         /// <summary>
//         /// Tests that the Position property returns the underlying stream's current position.
//         /// </summary>
//         [Fact]
//         public void Position_WhenCalled_ReturnsCorrectPosition()
//         {
//             // Arrange
//             byte[] data = Encoding.UTF8.GetBytes("Test Data");
//             using var stream = new MemoryStream(data);
//             var reader = CreateReader(stream, ForwardCompatibilityMinimalVersion);
//             
//             // Act
//             long positionBefore = reader.Position;
//             // Read some bytes manually from underlying stream
//             stream.ReadByte();
//             long positionAfter = reader.Position;
//             
//             // Assert
//             Assert.Equal(positionBefore + 1, positionAfter);
//         }
// 
//         /// <summary>
//         /// Tests that setting SkipUnknownEvents to true with a file format version below the minimal required version throws an exception.
//         /// </summary>
//         [Fact]
//         public void SkipUnknownEvents_VersionTooLow_ThrowsInvalidOperationException()
//         {
//             // Arrange
//             using var stream = new MemoryStream();
//             // using version lower than ForwardCompatibilityMinimalVersion (i.e. 17)
//             var reader = CreateReader(stream, ForwardCompatibilityMinimalVersion - 1);
//             
//             // Act & Assert
//             var exception = Assert.Throws<InvalidOperationException>(() =>
//             {
//                 reader.SkipUnknownEvents = true;
//             });
//             Assert.Contains("Forward compatible reading is not supported", exception.Message);
//         }
// 
//         /// <summary>
//         /// Tests that setting SkipUnknownEventParts to true with a file format version below the minimal required version throws an exception.
//         /// </summary>
//         [Fact]
//         public void SkipUnknownEventParts_VersionTooLow_ThrowsInvalidOperationException()
//         {
//             // Arrange
//             using var stream = new MemoryStream();
//             var reader = CreateReader(stream, ForwardCompatibilityMinimalVersion - 1);
//             
//             // Act & Assert
//             var exception = Assert.Throws<InvalidOperationException>(() =>
//             {
//                 reader.SkipUnknownEventParts = true;
//             });
//             Assert.Contains("Forward compatible reading is not supported", exception.Message);
//         }
// 
//         /// <summary>
//         /// Tests that Read returns null when the underlying stream signals EndOfFile.
//         /// In this test, we simulate an EndOfFile record by writing the corresponding integer value.
//         /// We assume that BinaryLogRecordKind.EndOfFile is represented by 0.
//         /// </summary>
//         [Fact]
//         public void Read_WhenEndOfFile_ReturnsNull()
//         {
//             // Arrange
//             using var memStream = new MemoryStream();
//             using (var writer = new BinaryWriter(memStream, Encoding.UTF8, leaveOpen: true))
//             {
//                 // Write an integer representing BinaryLogRecordKind.EndOfFile.
//                 writer.Write(0); // assuming 0 equals EndOfFile
//             }
//             memStream.Position = 0;
//             var reader = CreateReader(memStream, ForwardCompatibilityMinimalVersion);
//             // Ensure skip flags are false so CheckErrorsSubscribed does not throw.
//             
//             // Act
//             var result = reader.Read();
//             
//             // Assert
//             Assert.Null(result);
//         }
// 
//         /// <summary>
//         /// Tests that Dispose properly disposes the underlying BinaryReader when CloseInput is set to true.
//         /// </summary>
//         [Fact]
//         public void Dispose_WithCloseInputTrue_DisposesBinaryReader()
//         {
//             // Arrange
//             var disposableStream = new DisposableMemoryStream();
//             var binaryReader = new BinaryReader(disposableStream, Encoding.UTF8);
//             var reader = new BuildEventArgsReader(binaryReader, ForwardCompatibilityMinimalVersion);
//             reader.CloseInput = true;
//             
//             // Act
//             reader.Dispose();
//             
//             // Assert
//             Assert.True(disposableStream.IsDisposed);
//         }
// 
//         /// <summary>
//         /// Tests that the MinimumReaderVersion property can be get and set.
//         /// </summary>
//         [Fact]
//         public void MinimumReaderVersion_GetSet_ReturnsExpectedValue()
//         {
//             // Arrange
//             using var stream = new MemoryStream();
//             var reader = CreateReader(stream, ForwardCompatibilityMinimalVersion);
//             int expectedVersion = 25;
//             
//             // Act
//             reader.MinimumReaderVersion = expectedVersion;
//             int actualVersion = reader.MinimumReaderVersion;
//             
//             // Assert
//             Assert.Equal(expectedVersion, actualVersion);
//         }
// 
//         /// <summary>
//         /// Tests that Dispose does not close the underlying BinaryReader when CloseInput is false.
//         /// </summary>
//         [Fact]
//         public void Dispose_WithCloseInputFalse_DoesNotDisposeBinaryReader()
//         {
//             // Arrange
//             var disposableStream = new DisposableMemoryStream();
//             var binaryReader = new BinaryReader(disposableStream, Encoding.UTF8);
//             var reader = new BuildEventArgsReader(binaryReader, ForwardCompatibilityMinimalVersion);
//             reader.CloseInput = false;
//             
//             // Act
//             reader.Dispose();
//             
//             // Attempt to read from the underlying stream
//             long positionAfterDispose = 0;
//             try
//             {
//                 positionAfterDispose = disposableStream.Position;
//             }
//             catch (ObjectDisposedException)
//             {
//                 // if disposed, the test should fail
//                 Assert.True(false, "Underlying stream was disposed unexpectedly.");
//             }
//             
//             // Assert
//             Assert.Equal(0, positionAfterDispose);
//         }
//     }
}
