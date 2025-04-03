// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.IO.Compression;
// using System.Linq;
// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using Moq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="BinLogReader"/> class.
//     /// </summary>
//     public class BinLogReaderTests
//     {
//         /// <summary>
//         /// Tests the <see cref="BinLogReader.Replay(string)"/> method with an invalid file path.
//         /// Expected to throw a <see cref="FileNotFoundException"/>.
//         /// </summary>
//         [Fact]
//         public void Replay_StringInvalidPath_ThrowsFileNotFoundException()
//         {
//             // Arrange
//             var binLogReader = new BinLogReader();
//             string invalidPath = "non_existent_file.binlog";
// 
//             // Act
//             Action act = () => binLogReader.Replay(invalidPath);
// 
//             // Assert
//             act.Should().Throw<FileNotFoundException>();
//         }
// //  // [Error] (62-66)CS1503 Argument 2: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.ProgressStub' to 'Microsoft.Build.Logging.StructuredLogger.Progress'
// //         /// <summary>
// //         /// Tests the <see cref="BinLogReader.Replay(string, Progress)"/> method with a valid but minimal binary log file.
// //         /// This is a partial test due to the complexity of creating a complete valid binary log.
// //         /// </summary>
// //         [Fact]
// //         public void Replay_StringWithProgress_ValidFile_DoesNotThrow()
// //         {
// //             // Arrange
// //             string tempFile = Path.GetTempFileName();
// //             try
// //             {
// //                 // Create a minimal compressed file with dummy data.
// //                 using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
// //                 using (var gzip = new GZipStream(fs, CompressionMode.Compress))
// //                 {
// //                     var writer = new BinaryWriter(gzip);
// //                     // Write a minimal file format version integer.
// //                     writer.Write(1);
// //                     // No additional data written so that reader.Read() returns null quickly.
// //                 }
// // 
// //                 var progress = new ProgressStub();
// //                 var binLogReader = new BinLogReader();
// // 
// //                 // Act
// //                 Action act = () => binLogReader.Replay(tempFile, progress);
// // 
// //                 // Assert
// //                 act.Should().NotThrow();
// //             }
// //             finally
// //             {
// //                 if (File.Exists(tempFile))
// //                 {
// //                     File.Delete(tempFile);
// //                 }
// //             }
// //         }
// // 
//         /// <summary>
//         /// Tests the <see cref="BinLogReader.Replay(Stream)"/> method using a stream containing minimal valid gzip data.
//         /// This is a partial test due to the complexity of creating a complete valid binary log.
//         /// </summary>
//         [Fact]
//         public void Replay_Stream_ValidStream_DoesNotThrow()
//         {
//             // Arrange
//             byte[] gzipData;
//             using (var ms = new MemoryStream())
//             {
//                 using (var gzip = new GZipStream(ms, CompressionMode.Compress, leaveOpen: true))
//                 {
//                     var writer = new BinaryWriter(gzip);
//                     writer.Write(1); // minimal valid file format version.
//                 }
//                 gzipData = ms.ToArray();
//             }
// 
//             using var stream = new MemoryStream(gzipData);
//             var binLogReader = new BinLogReader();
// 
//             // Act
//             Action act = () => binLogReader.Replay(stream);
// 
//             // Assert
//             act.Should().NotThrow();
//         }
// //  // [Error] (129-52)CS1503 Argument 1: cannot convert from 'System.IO.MemoryStream' to 'string' // [Error] (129-60)CS1503 Argument 2: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.ProgressStub' to 'Microsoft.Build.Logging.StructuredLogger.Progress'
// //         /// <summary>
// //         /// Tests the <see cref="BinLogReader.Replay(Stream, Progress)"/> method using a stream containing minimal valid gzip data and a progress reporter.
// //         /// This is a partial test due to the complexity of creating a complete valid binary log.
// //         /// </summary>
// //         [Fact]
// //         public void Replay_StreamWithProgress_ValidStream_DoesNotThrow()
// //         {
// //             // Arrange
// //             byte[] gzipData;
// //             using (var ms = new MemoryStream())
// //             {
// //                 using (var gzip = new GZipStream(ms, CompressionMode.Compress, leaveOpen: true))
// //                 {
// //                     var writer = new BinaryWriter(gzip);
// //                     writer.Write(1); // minimal valid file format version.
// //                 }
// //                 gzipData = ms.ToArray();
// //             }
// //             
// //             using var stream = new MemoryStream(gzipData);
// //             var progress = new ProgressStub();
// //             var binLogReader = new BinLogReader();
// // 
// //             // Act
// //             Action act = () => binLogReader.Replay(stream, progress);
// // 
// //             // Assert
// //             act.Should().NotThrow();
// //         }
// // 
//         /// <summary>
//         /// Tests the <see cref="BinLogReader.ReadRecords(string)"/> method with an invalid file path.
//         /// Expected to throw a <see cref="FileNotFoundException"/>.
//         /// </summary>
//         [Fact]
//         public void ReadRecords_StringInvalidPath_ThrowsFileNotFoundException()
//         {
//             // Arrange
//             var binLogReader = new BinLogReader();
//             string invalidPath = "non_existent_file.binlog";
// 
//             // Act
//             Action act = () =>
//             {
//                 var records = binLogReader.ReadRecords(invalidPath);
//                 // Enumerate to force execution.
//                 foreach (var record in records) { }
//             };
// 
//             // Assert
//             act.Should().Throw<FileNotFoundException>();
//         }
// 
//         /// <summary>
//         /// Tests the <see cref="BinLogReader.ReadRecords(byte[])"/> method with an empty byte array.
//         /// Expected to throw an exception due to inability to decompress empty input.
//         /// </summary>
//         [Fact]
//         public void ReadRecords_EmptyByteArray_ThrowsException()
//         {
//             // Arrange
//             var binLogReader = new BinLogReader();
//             byte[] emptyBytes = Array.Empty<byte>();
// 
//             // Act
//             Action act = () =>
//             {
//                 var records = binLogReader.ReadRecords(emptyBytes);
//                 foreach (var record in records) { }
//             };
// 
//             // Assert
//             act.Should().Throw<InvalidDataException>();
//         }
// 
//         /// <summary>
//         /// Tests the <see cref="BinLogReader.ReadRecords(Stream)"/> method with a stream containing minimal valid gzip data.
//         /// This is a partial test due to the complexity of creating a complete valid binary log.
//         /// </summary>
//         [Fact]
//         public void ReadRecords_Stream_ValidStream_DoesNotThrow()
//         {
//             // Arrange
//             byte[] gzipData;
//             using (var ms = new MemoryStream())
//             {
//                 using(var gzip = new GZipStream(ms, CompressionMode.Compress, leaveOpen: true))
//                 {
//                     var writer = new BinaryWriter(gzip);
//                     writer.Write(1);
//                 }
//                 gzipData = ms.ToArray();
//             }
//             using var stream = new MemoryStream(gzipData);
//             var binLogReader = new BinLogReader();
// 
//             // Act
//             Action act = () =>
//             {
//                 var records = binLogReader.ReadRecords(stream);
//                 foreach(var r in records) { }
//             };
// 
//             // Assert
//             act.Should().NotThrow();
//         }
// 
//         /// <summary>
//         /// Tests the <see cref="BinLogReader.ChunkBinlog(string)"/> method with an invalid file path.
//         /// Expected to throw a <see cref="FileNotFoundException"/>.
//         /// </summary>
//         [Fact]
//         public void ChunkBinlog_InvalidPath_ThrowsFileNotFoundException()
//         {
//             // Arrange
//             var binLogReader = new BinLogReader();
//             string invalidPath = "non_existent_file.binlog";
// 
//             // Act
//             Action act = () =>
//             {
//                 var records = binLogReader.ChunkBinlog(invalidPath);
//                 foreach(var r in records) { }
//             };
// 
//             // Assert
//             act.Should().Throw<FileNotFoundException>();
//         }
// 
//         /// <summary>
//         /// Tests the <see cref="BinLogReader.ChunkBinlog(string)"/> method with a valid file.
//         /// This is a partial test due to the complexity of creating a complete valid binary log.
//         /// </summary>
//         [Fact]
//         public void ChunkBinlog_ValidFile_DoesNotThrow()
//         {
//             // Arrange
//             string tempFile = Path.GetTempFileName();
//             try
//             {
//                 using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
//                 using (var gzip = new GZipStream(fs, CompressionMode.Compress))
//                 {
//                     var writer = new BinaryWriter(gzip);
//                     writer.Write(1);
//                 }
// 
//                 var binLogReader = new BinLogReader();
// 
//                 // Act
//                 Action act = () =>
//                 {
//                     var records = binLogReader.ChunkBinlog(tempFile);
//                     foreach(var r in records) { }
//                 };
// 
//                 // Assert
//                 act.Should().NotThrow();
//             }
//             finally
//             {
//                 if (File.Exists(tempFile))
//                 {
//                     File.Delete(tempFile);
//                 }
//             }
//         }
// 
//         /// <summary>
//         /// Tests the <see cref="BinLogReader.ReadRecordsFromDecompressedStream(Stream)"/> method using a stream with minimal valid gzip data.
//         /// This is a partial test due to the complexity of creating a complete valid binary log.
//         /// </summary>
//         [Fact]
//         public void ReadRecordsFromDecompressedStream_ValidStream_DoesNotThrow()
//         {
//             // Arrange
//             byte[] gzipData;
//             using(var ms = new MemoryStream())
//             {
//                 using(var gzip = new GZipStream(ms, CompressionMode.Compress, leaveOpen: true))
//                 {
//                     var writer = new BinaryWriter(gzip);
//                     writer.Write(1);
//                 }
//                 gzipData = ms.ToArray();
//             }
//             using var stream = new MemoryStream(gzipData);
//             var binLogReader = new BinLogReader();
// 
//             // Act
//             Action act = () =>
//             {
//                 var records = binLogReader.ReadRecordsFromDecompressedStream(stream);
//                 foreach(var r in records) { }
//             };
// 
//             // Assert
//             act.Should().NotThrow();
//         }
// 
//         /// <summary>
//         /// Tests the <see cref="BinLogReader.ReadRecordsFromDecompressedStream(Stream, bool)"/> method using a stream with minimal valid gzip data.
//         /// This is a partial test due to the complexity of creating a complete valid binary log.
//         /// </summary>
//         [Fact]
//         public void ReadRecordsFromDecompressedStream_WithAuxiliaryFlag_ValidStream_DoesNotThrow()
//         {
//             // Arrange
//             byte[] gzipData;
//             using(var ms = new MemoryStream())
//             {
//                 using(var gzip = new GZipStream(ms, CompressionMode.Compress, leaveOpen: true))
//                 {
//                     var writer = new BinaryWriter(gzip);
//                     writer.Write(1);
//                 }
//                 gzipData = ms.ToArray();
//             }
//             using var stream = new MemoryStream(gzipData);
//             var binLogReader = new BinLogReader();
// 
//             // Act
//             Action act = () =>
//             {
//                 var records = binLogReader.ReadRecordsFromDecompressedStream(stream, includeAuxiliaryRecords: true);
//                 foreach(var r in records) { }
//             };
// 
//             // Assert
//             act.Should().NotThrow();
//         }
// //  // [Error] (345-90)CS1503 Argument 3: cannot convert from 'string' to 'System.DateTime'
// //         /// <summary>
// //         /// Tests the static <see cref="BinLogReader.ToBinaryLogRecordKind(BuildEventArgs)"/> method with a BuildStartedEventArgs instance.
// //         /// Expected to return <see cref="BinaryLogRecordKind.BuildStarted"/>.
// //         /// </summary>
// //         [Fact]
// //         public void ToBinaryLogRecordKind_WithBuildStartedEventArgs_ReturnsBuildStarted()
// //         {
// //             // Arrange
// //             var buildStartedArgs = new BuildStartedEventArgs("Build started", "Message", "HelpKeyword", "Sender");
// // 
// //             // Act
// //             var result = BinLogReader.ToBinaryLogRecordKind(buildStartedArgs);
// // 
// //             // Assert
// //             result.Should().Be(BinaryLogRecordKind.BuildStarted);
// //         }
// //  // [Error] (362-93)CS1503 Argument 3: cannot convert from 'string' to 'bool' // [Error] (362-108)CS1503 Argument 4: cannot convert from 'string' to 'System.DateTime'
//         /// <summary>
//         /// Tests the static <see cref="BinLogReader.ToBinaryLogRecordKind(BuildEventArgs)"/> method with a BuildFinishedEventArgs instance.
//         /// Expected to return <see cref="BinaryLogRecordKind.BuildFinished"/>.
//         /// </summary>
//         [Fact]
//         public void ToBinaryLogRecordKind_WithBuildFinishedEventArgs_ReturnsBuildFinished()
//         {
//             // Arrange
//             var buildFinishedArgs = new BuildFinishedEventArgs("Build finished", "Message", "HelpKeyword", "Sender", true);
// 
//             // Act
//             var result = BinLogReader.ToBinaryLogRecordKind(buildFinishedArgs);
// 
//             // Assert
//             result.Should().Be(BinaryLogRecordKind.BuildFinished);
//         }
// 
//         // Additional tests for other BuildEventArgs types can be added here following the above pattern.
//     }
// 
//     /// <summary>
//     /// A stub implementation of the Progress class to capture progress updates.
//     /// </summary>
//     public class ProgressStub : Progress<ProgressUpdate>
//     {
//         public List<ProgressUpdate> Updates { get; } = new List<ProgressUpdate>();
// //  // [Error] (381-30)CS0115 'ProgressStub.Report(ProgressUpdate)': no suitable method found to override
// //         public override void Report(ProgressUpdate value)
// //         {
// //             Updates.Add(value);
// //         }
// //     }
// }