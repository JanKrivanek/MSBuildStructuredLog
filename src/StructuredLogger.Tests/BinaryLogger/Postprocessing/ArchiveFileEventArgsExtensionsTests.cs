// using FluentAssertions;
// using System;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ArchiveFileEventArgsExtensions"/> class.
//     /// </summary>
//     public class ArchiveFileEventArgsExtensionsTests
//     {
// //         /// <summary> // [Error] (26-33)CS7036 There is no argument given that corresponds to the required parameter 'archiveFile' of 'ArchiveFileEventArgs.ArchiveFileEventArgs(ArchiveFile)'
// //         /// Tests that the ToArchiveFileHandler extension method updates the ArchiveFile properties
// //         /// using a string handler that appends a modifier to the original strings.
// //         /// </summary>
// //         [Fact]
// //         public void ToArchiveFileHandler_HappyPath_UpdatesArchiveFile()
// //         {
// //             // Arrange
// //             // Create a string handler that appends "-modified" to the input string.
// //             Action<StringReadEventArgs> handler = args => args.StringToBeUsed = args.StringToBeUsed + "-modified";
// // 
// //             // Initial ArchiveFile with known FullPath and Text.
// //             var initialArchiveFile = new ArchiveFile("file.txt", "content");
// //             var eventArgs = new ArchiveFileEventArgs { ArchiveFile = initialArchiveFile };
// // 
// //             // Act
// //             var archiveFileHandler = handler.ToArchiveFileHandler();
// //             archiveFileHandler(eventArgs);
// // 
// //             // Assert
// //             eventArgs.ArchiveFile.Should().NotBeNull();
// //             eventArgs.ArchiveFile.FullPath.Should().Be("file.txt-modified");
// //             eventArgs.ArchiveFile.Text.Should().Be("content-modified");
// //         }
// //  // [Error] (50-33)CS7036 There is no argument given that corresponds to the required parameter 'archiveFile' of 'ArchiveFileEventArgs.ArchiveFileEventArgs(ArchiveFile)'
//         /// <summary>
//         /// Tests that the ToArchiveFileHandler extension method leaves the ArchiveFile strings unchanged
//         /// when the provided string handler does not modify the input.
//         /// </summary>
//         [Fact]
//         public void ToArchiveFileHandler_HandlerDoesNotModify_LeavesDataUnchanged()
//         {
//             // Arrange
//             // String handler that performs no modifications.
//             Action<StringReadEventArgs> handler = args => { /* No modification */ };
// 
//             var initialArchiveFile = new ArchiveFile("pathOriginal", "textOriginal");
//             var eventArgs = new ArchiveFileEventArgs { ArchiveFile = initialArchiveFile };
// 
//             // Act
//             var archiveFileHandler = handler.ToArchiveFileHandler();
//             archiveFileHandler(eventArgs);
// 
//             // Assert
//             eventArgs.ArchiveFile.Should().NotBeNull();
//             eventArgs.ArchiveFile.FullPath.Should().Be("pathOriginal");
//             eventArgs.ArchiveFile.Text.Should().Be("textOriginal");
//         }
// //  // [Error] (73-26)CS1061 'StringReadEventArgs' does not contain a definition for 'InitialValue' and no accessible extension method 'InitialValue' accepting a first argument of type 'StringReadEventArgs' could be found (are you missing a using directive or an assembly reference?) // [Error] (77-31)CS1061 'StringReadEventArgs' does not contain a definition for 'InitialValue' and no accessible extension method 'InitialValue' accepting a first argument of type 'StringReadEventArgs' could be found (are you missing a using directive or an assembly reference?) // [Error] (84-33)CS7036 There is no argument given that corresponds to the required parameter 'archiveFile' of 'ArchiveFileEventArgs.ArchiveFileEventArgs(ArchiveFile)'
// //         /// <summary>
// //         /// Tests that the ToArchiveFileHandler extension method correctly applies different modifications
// //         /// for the file path and the file content when the string handler distinguishes between them.
// //         /// </summary>
// //         [Fact]
// //         public void ToArchiveFileHandler_HandlerModifiesSeparately_UpdatesArchiveFileAccordingly()
// //         {
// //             // Arrange
// //             // String handler that checks the initial value and modifies accordingly.
// //             Action<StringReadEventArgs> handler = args =>
// //             {
// //                 if (args.InitialValue == "originalPath")
// //                 {
// //                     args.StringToBeUsed = "modifiedPath";
// //                 }
// //                 else if (args.InitialValue == "originalContent")
// //                 {
// //                     args.StringToBeUsed = "modifiedContent";
// //                 }
// //             };
// // 
// //             var initialArchiveFile = new ArchiveFile("originalPath", "originalContent");
// //             var eventArgs = new ArchiveFileEventArgs { ArchiveFile = initialArchiveFile };
// // 
// //             // Act
// //             var archiveFileHandler = handler.ToArchiveFileHandler();
// //             archiveFileHandler(eventArgs);
// // 
// //             // Assert
// //             eventArgs.ArchiveFile.Should().NotBeNull();
// //             eventArgs.ArchiveFile.FullPath.Should().Be("modifiedPath");
// //             eventArgs.ArchiveFile.Text.Should().Be("modifiedContent");
// //         }
// //  // [Error] (105-33)CS7036 There is no argument given that corresponds to the required parameter 'archiveFile' of 'ArchiveFileEventArgs.ArchiveFileEventArgs(ArchiveFile)'
//         /// <summary>
//         /// Tests that invoking the returned delegate from ToArchiveFileHandler with a null ArchiveFile
//         /// in the ArchiveFileEventArgs throws a NullReferenceException.
//         /// </summary>
//         [Fact]
//         public void ToArchiveFileHandler_WithNullArchiveFile_ThrowsNullReferenceException()
//         {
//             // Arrange
//             Action<StringReadEventArgs> handler = args => args.StringToBeUsed = args.StringToBeUsed + "-modified";
//             var eventArgs = new ArchiveFileEventArgs { ArchiveFile = null };
// 
//             // Act
//             var archiveFileHandler = handler.ToArchiveFileHandler();
//             Action act = () => archiveFileHandler(eventArgs);
// 
//             // Assert
//             act.Should().Throw<NullReferenceException>();
//         }
// //  // [Error] (128-33)CS7036 There is no argument given that corresponds to the required parameter 'archiveFile' of 'ArchiveFileEventArgs.ArchiveFileEventArgs(ArchiveFile)'
// //         /// <summary>
// //         /// Tests that using a null string handler and then invoking the returned delegate
// //         /// results in a NullReferenceException.
// //         /// </summary>
// //         [Fact]
// //         public void ToArchiveFileHandler_NullStringHandler_ThrowsNullReferenceExceptionWhenInvoked()
// //         {
// //             // Arrange
// //             Action<StringReadEventArgs> nullHandler = null;
// //             // Since the extension method does not validate the null handler,
// //             // it will return a delegate that throws when invoked.
// //             var archiveFileHandler = nullHandler.ToArchiveFileHandler();
// //             var initialArchiveFile = new ArchiveFile("anyPath", "anyContent");
// //             var eventArgs = new ArchiveFileEventArgs { ArchiveFile = initialArchiveFile };
// // 
// //             // Act
// //             Action act = () => archiveFileHandler(eventArgs);
// // 
// //             // Assert
// //             act.Should().Throw<NullReferenceException>();
// //         }
// //     }
// }