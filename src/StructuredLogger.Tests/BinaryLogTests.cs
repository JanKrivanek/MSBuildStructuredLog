// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "BinaryLog"/> class.
//     /// </summary>
//     public class BinaryLogTests
//     {
//         /// <summary>
//         /// Tests the ReadRecords(string) method with an invalid file path.
//         /// Expects a FileNotFoundException or similar exception due to the non-existent file.
//         /// </summary>
//         [Fact]
//         public void ReadRecords_String_InvalidPath_ThrowsFileNotFoundException()
//         {
//             // Arrange
//             string invalidPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".binlog");
//             // Act
//             Action action = () => BinaryLog.ReadRecords(invalidPath).ToList();
//             // Assert
//             action.Should().Throw<FileNotFoundException>("because the file does not exist at the provided path");
//         }
// 
//         /// <summary>
//         /// Tests the ReadRecords(Stream) method using an empty MemoryStream.
//         /// Expected behavior: returns an empty collection of records.
//         /// </summary>
//         [Fact]
//         public void ReadRecords_Stream_EmptyStream_ReturnsEmptyCollection()
//         {
//             // Arrange
//             using MemoryStream emptyStream = new MemoryStream(Array.Empty<byte>());
//             // Act
//             IEnumerable<Record> records = BinaryLog.ReadRecords(emptyStream);
//             // Assert
//             records.Should().BeEmpty("because an empty stream should yield no records");
//         }
// 
//         /// <summary>
//         /// Tests the ReadRecords(byte[]) method using an empty byte array.
//         /// Expected behavior: returns an empty collection of records.
//         /// </summary>
//         [Fact]
//         public void ReadRecords_Bytes_EmptyArray_ReturnsEmptyCollection()
//         {
//             // Arrange
//             byte[] emptyBytes = Array.Empty<byte>();
//             // Act
//             IEnumerable<Record> records = BinaryLog.ReadRecords(emptyBytes);
//             // Assert
//             records.Should().BeEmpty("because an empty byte array should yield no records");
//         }
// 
//         /// <summary>
//         /// Tests the ReadBuild(string) method with an invalid file path.
//         /// Expects a FileNotFoundException due to inability to open a non-existent file.
//         /// </summary>
//         [Fact]
//         public void ReadBuild_String_InvalidPath_ThrowsFileNotFoundException()
//         {
//             // Arrange
//             string invalidPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".binlog");
//             // Act
//             Action action = () => BinaryLog.ReadBuild(invalidPath);
//             // Assert
//             action.Should().Throw<FileNotFoundException>("because the build log file does not exist at the provided path");
//         }
// //  // [Error] (87-27)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (90-19)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests the ReadBuild(Stream, ...) method using an empty MemoryStream.
// //         /// Expected behavior: the method returns a Build instance marked as failed with error information.
// //         /// </summary>
// //         [Fact]
// //         public void ReadBuild_Stream_EmptyStream_ReturnsFailedBuild()
// //         {
// //             // Arrange
// //             using MemoryStream emptyStream = new MemoryStream(Array.Empty<byte>());
// //             // Act
// //             Build build = BinaryLog.ReadBuild(emptyStream);
// //             // Assert
// //             build.Should().NotBeNull("because a Build instance should be returned even if reading fails");
// //             build.Succeeded.Should().BeFalse("because reading an empty stream should result in a failed build");
// //         }
// //  // [Error] (107-27)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (110-19)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the ReadBuild(Stream, Progress, byte[], ReaderSettings) method overload using an empty MemoryStream.
//         /// Expected behavior: the method returns a Build instance marked as failed with error information.
//         /// </summary>
//         [Fact]
//         public void ReadBuild_Stream_WithProgressAndReaderSettings_EmptyStream_ReturnsFailedBuild()
//         {
//             // Arrange
//             using MemoryStream emptyStream = new MemoryStream(Array.Empty<byte>());
//             Progress progress = new Progress();
//             // Using the default ReaderSettings as provided by the class.
//             ReaderSettings readerSettings = ReaderSettings.Default;
//             byte[] projectImportsArchive = null;
//             // Act
//             Build build = BinaryLog.ReadBuild(emptyStream, progress, projectImportsArchive, readerSettings);
//             // Assert
//             build.Should().NotBeNull("because a Build instance should be returned even if reading fails");
//             build.Succeeded.Should().BeFalse("because reading an empty stream should result in a failed build");
//         }
//     /*
//          * NOTE: The remaining overloads of ReadBuild(string, Progress) and ReadBuild(string, ReaderSettings)
//          * internally call the ReadBuild(string, Progress, ReaderSettings) method. Integration/happy path testing
//          * for these methods requires a valid binary log file and corresponding project imports archive.
//          * Therefore, further tests for successful reading are left as a TODO for manual or integration testing.
//          */
//     }
// }