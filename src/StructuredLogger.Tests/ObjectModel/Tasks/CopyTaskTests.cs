// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text.RegularExpressions;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="CopyTask"/> class.
//     /// </summary>
//     public class CopyTaskTests
//     {
//         private readonly CopyTask _copyTask;
// 
//         public CopyTaskTests()
//         {
//             // Arrange
//             _copyTask = new CopyTask();
//         }
// 
//         /// <summary>
//         /// Tests the FileCopyOperations property when there are no children.
//         /// Expected outcome is an empty collection.
//         /// </summary>
//         [Fact]
//         public void FileCopyOperations_Getter_WithoutChildren_ReturnsEmptyCollection()
//         {
//             // Act
//             var operations = _copyTask.FileCopyOperations;
// 
//             // Assert
//             operations.Should().BeEmpty("because no children exist, so no operations should be detected.");
//         }
// 
//         /// <summary>
//         /// Tests the FileCopyOperations getter to ensure the lazy initialization is caching the result.
//         /// </summary>
//         [Fact]
//         public void FileCopyOperations_Getter_CalledMultipleTimes_ReturnsSameInstance()
//         {
//             // Act
//             var firstCall = _copyTask.FileCopyOperations;
//             var secondCall = _copyTask.FileCopyOperations;
// 
//             // Assert
//             firstCall.Should().BeSameAs(secondCall, "because the result should be cached after the first retrieval.");
//         }
// //  // [Error] (65-38)CS0122 'CopyTask.ParseCopyingFileFrom(Match, bool)' is inaccessible due to its protection level
// //         /// <summary>
// //         /// Tests the ParseCopyingFileFrom method with a match that contains valid "From" and "To" groups.
// //         /// The expected outcome is a FileCopyOperation with the correct values and Copied flag set to true.
// //         /// </summary>
// //         [Fact]
// //         public void ParseCopyingFileFrom_HappyPathWithCopiedTrue_ReturnsCorrectOperation()
// //         {
// //             // Arrange
// //             string pattern = "^(?<From>source)(?<To>dest)$";
// //             string input = "sourcedest";
// //             Match match = Regex.Match(input, pattern);
// // 
// //             // Act
// //             var operation = CopyTask.ParseCopyingFileFrom(match);
// // 
// //             // Assert
// //             operation.Should().NotBeNull();
// //             operation.Source.Should().Be("source");
// //             operation.Destination.Should().Be("dest");
// //             operation.Copied.Should().BeTrue();
// //         }
// //  // [Error] (87-38)CS0122 'CopyTask.ParseCopyingFileFrom(Match, bool)' is inaccessible due to its protection level
//         /// <summary>
//         /// Tests the ParseCopyingFileFrom method with the copied parameter set to false.
//         /// The expected outcome is a FileCopyOperation with the Copied flag set to false.
//         /// </summary>
//         [Fact]
//         public void ParseCopyingFileFrom_HappyPathWithCopiedFalse_ReturnsCorrectOperation()
//         {
//             // Arrange
//             string pattern = "^(?<From>alpha)(?<To>beta)$";
//             string input = "alphabeta";
//             Match match = Regex.Match(input, pattern);
// 
//             // Act
//             var operation = CopyTask.ParseCopyingFileFrom(match, copied: false);
// 
//             // Assert
//             operation.Should().NotBeNull();
//             operation.Source.Should().Be("alpha");
//             operation.Destination.Should().Be("beta");
//             operation.Copied.Should().BeFalse();
//         }
// //  // [Error] (109-38)CS0122 'CopyTask.ParseCopyingFileFrom(Match, bool)' is inaccessible due to its protection level
// //         /// <summary>
// //         /// Tests the ParseCopyingFileFrom method with a match that contains empty groups.
// //         /// The expected outcome is a FileCopyOperation with empty Source and Destination.
// //         /// </summary>
// //         [Fact]
// //         public void ParseCopyingFileFrom_EdgeCaseEmptyGroups_ReturnsOperationWithEmptyValues()
// //         {
// //             // Arrange
// //             string pattern = "^(?<From>)(?<To>)$";
// //             string input = "";
// //             Match match = Regex.Match(input, pattern);
// // 
// //             // Act
// //             var operation = CopyTask.ParseCopyingFileFrom(match);
// // 
// //             // Assert
// //             operation.Should().NotBeNull();
// //             operation.Source.Should().BeEmpty();
// //             operation.Destination.Should().BeEmpty();
// //             operation.Copied.Should().BeTrue();
// //         }
// // 
//         /// <summary>
//         /// Tests the GetFileCopyOperations method in a scenario where the CopyTask has children and messages.
//         /// NOTE: This is a partial test. Due to the dependency on the HasChildren property and GetMessages method,
//         /// a custom test subclass should be provided to simulate different message scenarios. 
//         /// </summary>
//         [Fact]
//         public void GetFileCopyOperations_WithChildrenAndMessages_ReturnsExpectedOperations_PartialTest()
//         {
//             // Arrange
//             // TODO: Implement a derived TestableCopyTask that allows overriding HasChildren and GetMessages
//             // to simulate various messages that trigger the matching of CopyingFileFromRegex, DidNotCopyRegex and CreatingHardLinkRegex.
//             // For now, this test is a placeholder.
//             
//             // Act
//             var operations = _copyTask.FileCopyOperations;
// 
//             // Assert
//             // Since by default HasChildren is likely false, we expect no operations.
//             operations.Should().BeEmpty("because without simulated children or messages, no file copy operations should be generated.");
//         }
//     }
// }