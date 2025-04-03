// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging;
// using System;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ProjectImportsCollectorExtensions"/> class.
//     /// </summary>
//     public class ProjectImportsCollectorExtensionsTests
//     {
//         // IMPORTANT:
//         // Due to the design limitations of ProjectImportsCollector and its non-virtual AddFile method,
//         // it is not possible to intercept or verify the call to AddFile.
//         // Thus, these tests verify that the IncludeSourceFiles extension method completes without throwing exceptions.
//         
//         /// <summary>
//         /// Tests that calling IncludeSourceFiles with a TaskStartedEventArgs does not throw an exception.
//         /// Expected behavior: The method attempts to add the file specified in TaskStartedEventArgs.TaskFile.
//         /// Note: Due to non-virtual AddFile, the file addition cannot be intercepted.
//         /// </summary>
//         [Fact]
//         public void IncludeSourceFiles_TaskStartedEventArgs_DoesNotThrow()
//         {
//             // Arrange
//             var collector = new DummyProjectImportsCollector();
//             var fakeEvent = new FakeTaskStartedEventArgs("C:\\fake\\taskfile.cs");
// 
//             // Act
//             Action act = () => collector.IncludeSourceFiles(fakeEvent);
// 
//             // Assert
//             act.Should().NotThrow("IncludeSourceFiles should process TaskStartedEventArgs without throwing an exception.");
//         }
// 
//         /// <summary>
//         /// Tests that calling IncludeSourceFiles with a TargetStartedEventArgs does not throw an exception.
//         /// Expected behavior: The method attempts to add the file specified in TargetStartedEventArgs.TargetFile.
//         /// Note: Due to non-virtual AddFile, the file addition cannot be intercepted.
//         /// </summary>
//         [Fact]
//         public void IncludeSourceFiles_TargetStartedEventArgs_DoesNotThrow()
//         {
//             // Arrange
//             var collector = new DummyProjectImportsCollector();
//             var fakeEvent = new FakeTargetStartedEventArgs("C:\\fake\\targetfile.targets");
// 
//             // Act
//             Action act = () => collector.IncludeSourceFiles(fakeEvent);
// 
//             // Assert
//             act.Should().NotThrow("IncludeSourceFiles should process TargetStartedEventArgs without throwing an exception.");
//         }
// 
//         /// <summary>
//         /// Tests that calling IncludeSourceFiles with a ProjectStartedEventArgs does not throw an exception.
//         /// Expected behavior: The method attempts to add the file specified in ProjectStartedEventArgs.ProjectFile.
//         /// Note: Due to non-virtual AddFile, the file addition cannot be intercepted.
//         /// </summary>
//         [Fact]
//         public void IncludeSourceFiles_ProjectStartedEventArgs_DoesNotThrow()
//         {
//             // Arrange
//             var collector = new DummyProjectImportsCollector();
//             var fakeEvent = new FakeProjectStartedEventArgs("C:\\fake\\projectfile.csproj");
// 
//             // Act
//             Action act = () => collector.IncludeSourceFiles(fakeEvent);
// 
//             // Assert
//             act.Should().NotThrow("IncludeSourceFiles should process ProjectStartedEventArgs without throwing an exception.");
//         }
// 
//         /// <summary>
//         /// Tests that calling IncludeSourceFiles with an unhandled BuildEventArgs type does not throw an exception.
//         /// Expected behavior: The method does nothing if the event type is not TaskStarted, TargetStarted or ProjectStarted.
//         /// </summary>
//         [Fact]
//         public void IncludeSourceFiles_WithUnhandledBuildEventArgs_DoesNotThrow()
//         {
//             // Arrange
//             var collector = new DummyProjectImportsCollector();
//             var fakeEvent = new FakeBuildEventArgs();
// 
//             // Act
//             Action act = () => collector.IncludeSourceFiles(fakeEvent);
// 
//             // Assert
//             act.Should().NotThrow("IncludeSourceFiles should gracefully handle unrecognized BuildEventArgs types without throwing an exception.");
//         }
//     }
// //  // [Error] (98-20)CS7036 There is no argument given that corresponds to the required parameter 'logFilePath' of 'ProjectImportsCollector.ProjectImportsCollector(string, bool, string, bool)'
// //     /// <summary>
// //     /// Dummy subclass of ProjectImportsCollector used for testing the extension method.
// //     /// </summary>
// //     internal class DummyProjectImportsCollector : ProjectImportsCollector
// //     {
// //         // No additional implementation is provided.
// //         // Due to the non-virtual nature of AddFile, it is not possible to intercept the file addition.
// //     }
// // 
//     /// <summary>
//     /// Fake BuildEventArgs to simulate an unhandled event type.
//     /// </summary>
//     internal class FakeBuildEventArgs : BuildEventArgs
//     {
//         // Minimal implementation for testing purposes.
//     }
// 
//     /// <summary>
//     /// Fake TaskStartedEventArgs used for testing.
//     /// </summary>
//     internal class FakeTaskStartedEventArgs : TaskStartedEventArgs
//     {
//         private readonly string _taskFile;
// 
//         public FakeTaskStartedEventArgs(string taskFile)
//         {
//             _taskFile = taskFile;
//         }
// 
//         // Hiding the base property. Note: This may not be used by the extension method if the base property is accessed.
//         public new string TaskFile => _taskFile;
//     }
// 
//     /// <summary>
//     /// Fake TargetStartedEventArgs used for testing.
//     /// </summary>
//     internal class FakeTargetStartedEventArgs : TargetStartedEventArgs
//     {
//         private readonly string _targetFile;
// 
//         public FakeTargetStartedEventArgs(string targetFile)
//         {
//             _targetFile = targetFile;
//         }
// 
//         public new string TargetFile => _targetFile;
//     }
// 
//     /// <summary>
//     /// Fake ProjectStartedEventArgs used for testing.
//     /// </summary>
//     internal class FakeProjectStartedEventArgs : ProjectStartedEventArgs
//     {
//         private readonly string? _projectFile;
// 
//         public FakeProjectStartedEventArgs(string? projectFile)
//         {
//             _projectFile = projectFile;
//         }
// 
//         public new string? ProjectFile => _projectFile;
//     }
// }