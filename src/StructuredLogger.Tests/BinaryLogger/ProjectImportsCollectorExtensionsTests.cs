using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.UnitTests
{
    /// <summary>
    /// Fake implementation of ProjectImportsCollector used for testing.
    /// </summary>
    internal class FakeProjectImportsCollector
    {
        public List<string> AddedFiles { get; } = new List<string>();

        /// <summary>
        /// Simulates adding a file.
        /// </summary>
        /// <param name="file">The file to add.</param>
        public void AddFile(string file)
        {
            AddedFiles.Add(file);
        }
    }

    /// <summary>
    /// Unit tests for <see cref="ProjectImportsCollectorExtensions"/>.
    /// </summary>
    public class ProjectImportsCollectorExtensionsTests
    {
        private readonly FakeProjectImportsCollector _fakeCollector;

        public ProjectImportsCollectorExtensionsTests()
        {
            _fakeCollector = new FakeProjectImportsCollector();
        }

        /// <summary>
        /// Tests that IncludeSourceFiles calls AddFile with TaskFile when a TaskStartedEventArgs is provided.
        /// </summary>
//         [Fact] [Error] (51-66)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.UnitTests.FakeProjectImportsCollector' to 'Microsoft.Build.Logging.ProjectImportsCollector'
//         public void IncludeSourceFiles_TaskStartedEventArgs_AddsTaskFile()
//         {
//             // Arrange
//             string expectedTaskFile = "task.csproj";
//             var mockTaskStartedEventArgs = new Mock<TaskStartedEventArgs>();
//             mockTaskStartedEventArgs.SetupGet(x => x.TaskFile).Returns(expectedTaskFile);
// 
//             // Act
//             ProjectImportsCollectorExtensions.IncludeSourceFiles(_fakeCollector, mockTaskStartedEventArgs.Object);
// 
//             // Assert
//             Assert.Single(_fakeCollector.AddedFiles);
//             Assert.Equal(expectedTaskFile, _fakeCollector.AddedFiles[0]);
//         }

        /// <summary>
        /// Tests that IncludeSourceFiles calls AddFile with TargetFile when a TargetStartedEventArgs is provided.
        /// </summary>
//         [Fact] [Error] (70-66)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.UnitTests.FakeProjectImportsCollector' to 'Microsoft.Build.Logging.ProjectImportsCollector'
//         public void IncludeSourceFiles_TargetStartedEventArgs_AddsTargetFile()
//         {
//             // Arrange
//             string expectedTargetFile = "target.csproj";
//             var mockTargetStartedEventArgs = new Mock<TargetStartedEventArgs>();
//             mockTargetStartedEventArgs.SetupGet(x => x.TargetFile).Returns(expectedTargetFile);
// 
//             // Act
//             ProjectImportsCollectorExtensions.IncludeSourceFiles(_fakeCollector, mockTargetStartedEventArgs.Object);
// 
//             // Assert
//             Assert.Single(_fakeCollector.AddedFiles);
//             Assert.Equal(expectedTargetFile, _fakeCollector.AddedFiles[0]);
//         }

        /// <summary>
        /// Tests that IncludeSourceFiles calls AddFile with ProjectFile when a ProjectStartedEventArgs is provided.
        /// </summary>
//         [Fact] [Error] (89-66)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.UnitTests.FakeProjectImportsCollector' to 'Microsoft.Build.Logging.ProjectImportsCollector'
//         public void IncludeSourceFiles_ProjectStartedEventArgs_AddsProjectFile()
//         {
//             // Arrange
//             string expectedProjectFile = "project.csproj";
//             var mockProjectStartedEventArgs = new Mock<ProjectStartedEventArgs>();
//             mockProjectStartedEventArgs.SetupGet(x => x.ProjectFile).Returns(expectedProjectFile);
// 
//             // Act
//             ProjectImportsCollectorExtensions.IncludeSourceFiles(_fakeCollector, mockProjectStartedEventArgs.Object);
// 
//             // Assert
//             Assert.Single(_fakeCollector.AddedFiles);
//             Assert.Equal(expectedProjectFile, _fakeCollector.AddedFiles[0]);
//         }

        /// <summary>
        /// Tests that IncludeSourceFiles does not call AddFile when an unknown BuildEventArgs is provided.
        /// </summary>
//         [Fact] [Error] (104-34)CS0144 Cannot create an instance of the abstract type or interface 'BuildEventArgs' [Error] (107-66)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.UnitTests.FakeProjectImportsCollector' to 'Microsoft.Build.Logging.ProjectImportsCollector'
//         public void IncludeSourceFiles_UnknownBuildEventArgs_DoesNotAddFile()
//         {
//             // Arrange
//             // Using a plain BuildEventArgs instance which is not one of the specific types.
//             var buildEventArgs = new BuildEventArgs();
// 
//             // Act
//             ProjectImportsCollectorExtensions.IncludeSourceFiles(_fakeCollector, buildEventArgs);
// 
//             // Assert
//             Assert.Empty(_fakeCollector.AddedFiles);
//         }

        /// <summary>
        /// Tests that IncludeSourceFiles does nothing when the event argument is null.
        /// </summary>
//         [Fact] [Error] (125-66)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.UnitTests.FakeProjectImportsCollector' to 'Microsoft.Build.Logging.ProjectImportsCollector'
//         public void IncludeSourceFiles_NullEvent_DoesNothing()
//         {
//             // Arrange
//             BuildEventArgs nullEvent = null;
// 
//             // Act
//             // Since the event argument is null, none of the if branches execute.
//             // Expect no exception and no file added.
//             ProjectImportsCollectorExtensions.IncludeSourceFiles(_fakeCollector, nullEvent);
// 
//             // Assert
//             Assert.Empty(_fakeCollector.AddedFiles);
//         }

        /// <summary>
        /// Tests that IncludeSourceFiles throws a NullReferenceException when the collector is null.
        /// </summary>
//         [Fact] [Error] (145-70)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.UnitTests.FakeProjectImportsCollector' to 'Microsoft.Build.Logging.ProjectImportsCollector'
//         public void IncludeSourceFiles_NullCollector_ThrowsNullReferenceException()
//         {
//             // Arrange
//             string expectedTaskFile = "task.csproj";
//             var mockTaskStartedEventArgs = new Mock<TaskStartedEventArgs>();
//             mockTaskStartedEventArgs.SetupGet(x => x.TaskFile).Returns(expectedTaskFile);
//             FakeProjectImportsCollector nullCollector = null;
// 
//             // Act & Assert
//             Assert.Throws<NullReferenceException>(() =>
//                 ProjectImportsCollectorExtensions.IncludeSourceFiles(nullCollector, mockTaskStartedEventArgs.Object));
//         }
    }
}
