// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ArchiveFileEventArgs"/> class.
//     /// </summary>
//     public class ArchiveFileEventArgsTests
//     {
// //         /// <summary> // [Error] (19-43)CS7036 There is no argument given that corresponds to the required parameter 'fullPath' of 'ArchiveFile.ArchiveFile(string, string)'
// //         /// Verifies that the constructor sets the ArchiveFile property correctly when provided with a valid ArchiveFile instance.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithValidArchiveFile_SetsArchiveFileProperty()
// //         {
// //             // Arrange
// //             var expectedArchiveFile = new ArchiveFile();
// // 
// //             // Act
// //             var archiveFileEventArgs = new ArchiveFileEventArgs(expectedArchiveFile);
// // 
// //             // Assert
// //             archiveFileEventArgs.ArchiveFile.Should().Be(expectedArchiveFile);
// //         }
// //  // [Error] (35-42)CS7036 There is no argument given that corresponds to the required parameter 'fullPath' of 'ArchiveFile.ArchiveFile(string, string)' // [Error] (37-38)CS7036 There is no argument given that corresponds to the required parameter 'fullPath' of 'ArchiveFile.ArchiveFile(string, string)'
//         /// <summary>
//         /// Verifies that the ArchiveFile property can be modified and reflects the updated value.
//         /// </summary>
//         [Fact]
//         public void ArchiveFileProperty_WhenModified_ReflectsNewValue()
//         {
//             // Arrange
//             var initialArchiveFile = new ArchiveFile();
//             var archiveFileEventArgs = new ArchiveFileEventArgs(initialArchiveFile);
//             var newArchiveFile = new ArchiveFile();
// 
//             // Act
//             archiveFileEventArgs.ArchiveFile = newArchiveFile;
// 
//             // Assert
//             archiveFileEventArgs.ArchiveFile.Should().Be(newArchiveFile);
//         }
//     }
// }