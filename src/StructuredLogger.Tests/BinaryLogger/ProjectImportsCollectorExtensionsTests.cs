// using System;
// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging;
// using Moq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ProjectImportsCollectorExtensions"/> class.
//     /// </summary>
//     public class ProjectImportsCollectorExtensionsTests
//     {
//         private readonly Mock<ProjectImportsCollector> _mockCollector;
// 
//         public ProjectImportsCollectorExtensionsTests()
//         {
//             _mockCollector = new Mock<ProjectImportsCollector>();
//         }
// //  // [Error] (31-109)CS1503 Argument 5: cannot convert from 'int' to 'string' // [Error] (31-112)CS1503 Argument 6: cannot convert from 'int' to 'System.DateTime'
// //         /// <summary>
// //         /// Tests that IncludeSourceFiles correctly calls AddFile with the TaskFile from TaskStartedEventArgs.
// //         /// </summary>
// //         [Fact]
// //         public void IncludeSourceFiles_TaskStartedEventArgs_CallsAddFile()
// //         {
// //             // Arrange
// //             string expectedFile = "taskFile.cs";
// //             // Using TaskStartedEventArgs constructor: (message, helpKeyword, projectFile, taskFile, lineNumber, columnNumber)
// //             var taskEventArgs = new TaskStartedEventArgs("message", "help", "projectFile.cs", expectedFile, 0, 0);
// // 
// //             // Act
// //             ProjectImportsCollectorExtensions.IncludeSourceFiles(_mockCollector.Object, taskEventArgs);
// // 
// //             // Assert
// //             _mockCollector.Verify(collector => collector.AddFile(expectedFile), Times.Once,
// //                 "Expected AddFile to be called exactly once with the TaskFile from TaskStartedEventArgs.");
// //         }
// //  // [Error] (50-127)CS1503 Argument 6: cannot convert from 'int' to 'string' // [Error] (50-130)CS1503 Argument 7: cannot convert from 'int' to 'System.DateTime'
//         /// <summary>
//         /// Tests that IncludeSourceFiles correctly calls AddFile with the TargetFile from TargetStartedEventArgs.
//         /// </summary>
//         [Fact]
//         public void IncludeSourceFiles_TargetStartedEventArgs_CallsAddFile()
//         {
//             // Arrange
//             string expectedFile = "targetFile.cs";
//             // Using TargetStartedEventArgs constructor: (message, helpKeyword, targetFile, projectFile, targetName, lineNumber, columnNumber)
//             var targetEventArgs = new TargetStartedEventArgs("message", "help", expectedFile, "projectFile.cs", "targetName", 0, 0);
// 
//             // Act
//             ProjectImportsCollectorExtensions.IncludeSourceFiles(_mockCollector.Object, targetEventArgs);
// 
//             // Assert
//             _mockCollector.Verify(collector => collector.AddFile(expectedFile), Times.Once,
//                 "Expected AddFile to be called exactly once with the TargetFile from TargetStartedEventArgs.");
//         }
// //  // [Error] (69-40)CS1729 'ProjectStartedEventArgs' does not contain a constructor that takes 4 arguments
// //         /// <summary>
// //         /// Tests that IncludeSourceFiles correctly calls AddFile with the ProjectFile from ProjectStartedEventArgs.
// //         /// </summary>
// //         [Fact]
// //         public void IncludeSourceFiles_ProjectStartedEventArgs_CallsAddFile()
// //         {
// //             // Arrange
// //             string expectedFile = "projectFile.cs";
// //             // Using ProjectStartedEventArgs constructor. Assuming the constructor with DateTime parameter is available.
// //             var projectEventArgs = new ProjectStartedEventArgs("message", "help", expectedFile, DateTime.Now);
// // 
// //             // Act
// //             ProjectImportsCollectorExtensions.IncludeSourceFiles(_mockCollector.Object, projectEventArgs);
// // 
// //             // Assert
// //             _mockCollector.Verify(collector => collector.AddFile(expectedFile), Times.Once,
// //                 "Expected AddFile to be called exactly once with the ProjectFile from ProjectStartedEventArgs.");
// //         }
// // 
//         /// <summary>
//         /// Tests that IncludeSourceFiles does not call AddFile when the BuildEventArgs type is not supported.
//         /// </summary>
//         [Fact]
//         public void IncludeSourceFiles_UnhandledBuildEventArgs_DoesNotCallAddFile()
//         {
//             // Arrange
//             // Using BuildMessageEventArgs as an example of an unhandled BuildEventArgs type.
//             var unhandledEventArgs = new BuildMessageEventArgs("message", "help", "sender", MessageImportance.Low);
// 
//             // Act
//             ProjectImportsCollectorExtensions.IncludeSourceFiles(_mockCollector.Object, unhandledEventArgs);
// 
//             // Assert
//             _mockCollector.Verify(collector => collector.AddFile(It.IsAny<string>()), Times.Never,
//                 "Expected AddFile not to be called when an unsupported BuildEventArgs type is passed.");
//         }
//     }
// }