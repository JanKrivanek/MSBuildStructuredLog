using System.Collections.Generic;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Xunit;

namespace Microsoft.Build.Logging.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ProjectImportsCollectorExtensions"/> class.
    /// </summary>
    public class ProjectImportsCollectorExtensionsTests
    {
        /// <summary>
        /// Tests that when a TaskStartedEventArgs is passed, the TaskFile is added to the collector.
        /// </summary>
        [Fact]
        public void IncludeSourceFiles_WithTaskStartedEventArgs_AddsTaskFile()
        {
            // Arrange
            var expectedFile = "taskFile.cs";
            var collector = new FakeProjectImportsCollector();
            // TaskStartedEventArgs constructor: (string message, string helpKeyword, string projectFile, string taskFile, int eventTimestamp)
            var taskStartedEvent = new TaskStartedEventArgs("message", "help", "projFile.csproj", expectedFile, 0);

            // Act
            collector.IncludeSourceFiles(taskStartedEvent);

            // Assert
            Assert.Single(collector.FilesAdded);
            Assert.Equal(expectedFile, collector.FilesAdded[0]);
        }

        /// <summary>
        /// Tests that when a TargetStartedEventArgs is passed, the TargetFile is added to the collector.
        /// </summary>
        [Fact]
        public void IncludeSourceFiles_WithTargetStartedEventArgs_AddsTargetFile()
        {
            // Arrange
            var expectedFile = "targetFile.targets";
            var collector = new FakeProjectImportsCollector();
            // TargetStartedEventArgs constructor: (string message, string helpKeyword, string targetName, string projectFile, string targetFile, int eventTimestamp)
            var targetStartedEvent = new TargetStartedEventArgs("message", "help", "targetName", "projFile.csproj", expectedFile, 0);

            // Act
            collector.IncludeSourceFiles(targetStartedEvent);

            // Assert
            Assert.Single(collector.FilesAdded);
            Assert.Equal(expectedFile, collector.FilesAdded[0]);
        }

        /// <summary>
        /// Tests that when a ProjectStartedEventArgs is passed, the ProjectFile is added to the collector.
        /// </summary>
        [Fact]
        public void IncludeSourceFiles_WithProjectStartedEventArgs_AddsProjectFile()
        {
            // Arrange
            var expectedFile = "projectFile.csproj";
            var collector = new FakeProjectImportsCollector();
            // ProjectStartedEventArgs constructor: (string message, string helpKeyword, string projectFile, string targetNames, int eventTimestamp)
            var projectStartedEvent = new ProjectStartedEventArgs("message", "help", expectedFile, "target1;target2", 0);

            // Act
            collector.IncludeSourceFiles(projectStartedEvent);

            // Assert
            Assert.Single(collector.FilesAdded);
            Assert.Equal(expectedFile, collector.FilesAdded[0]);
        }

        /// <summary>
        /// Tests that when a BuildEventArgs that does not match any known event type is passed,
        /// no file is added to the collector.
        /// </summary>
        [Fact]
        public void IncludeSourceFiles_WithNonMatchingEvent_DoesNotAddFile()
        {
            // Arrange
            var collector = new FakeProjectImportsCollector();
            // BuildEventArgs constructor: (string message, string helpKeyword, string senderName)
            var genericEvent = new BuildEventArgs("message", "help", "sender");

            // Act
            collector.IncludeSourceFiles(genericEvent);

            // Assert
            Assert.Empty(collector.FilesAdded);
        }

        /// <summary>
        /// A fake implementation of ProjectImportsCollector to capture calls to AddFile.
        /// </summary>
        private class FakeProjectImportsCollector
        {
            /// <summary>
            /// Gets the collection of files added.
            /// </summary>
            public List<string> FilesAdded { get; } = new List<string>();

            /// <summary>
            /// Simulates adding a file by recording it in a list.
            /// </summary>
            /// <param name="file">The file to add.</param>
            public void AddFile(string file)
            {
                FilesAdded.Add(file);
            }
        }
    }
}
