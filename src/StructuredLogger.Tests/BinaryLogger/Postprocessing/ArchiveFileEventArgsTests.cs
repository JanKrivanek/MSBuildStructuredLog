// using System;
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
//         // A dummy implementation of ArchiveFile for testing purposes.
//         // This assumes that ArchiveFile is a class used in ArchiveFileEventArgs.
//         // If ArchiveFile is available from the project references, this dummy class can be removed.
// //         private class DummyArchiveFile : ArchiveFile [Error] (15-23)CS7036 There is no argument given that corresponds to the required parameter 'fullPath' of 'ArchiveFile.ArchiveFile(string, string)'
// //         {
// //         }
// 
//         /// <summary>
//         /// Tests that the constructor of ArchiveFileEventArgs correctly sets the ArchiveFile property
//         /// when provided with a valid ArchiveFile instance.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithValidArchiveFile_SetsArchiveFileProperty()
//         {
//             // Arrange
//             var expectedArchiveFile = new DummyArchiveFile();
// 
//             // Act
//             var eventArgs = new ArchiveFileEventArgs(expectedArchiveFile);
// 
//             // Assert
//             Assert.Equal(expectedArchiveFile, eventArgs.ArchiveFile);
//         }
// 
//         /// <summary>
//         /// Tests that the ArchiveFile property setter allows updating the property,
//         /// including assigning a null value.
//         /// </summary>
//         [Fact]
//         public void ArchiveFileProperty_SetToNull_AllowsNullAssignment()
//         {
//             // Arrange
//             var initialArchiveFile = new DummyArchiveFile();
//             var eventArgs = new ArchiveFileEventArgs(initialArchiveFile);
// 
//             // Act
//             eventArgs.ArchiveFile = null;
// 
//             // Assert
//             Assert.Null(eventArgs.ArchiveFile);
//         }
//     }
// }
