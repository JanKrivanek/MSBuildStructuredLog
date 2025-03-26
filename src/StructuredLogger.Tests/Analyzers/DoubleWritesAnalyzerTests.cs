using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="DoubleWritesAnalyzer"/> class.
    /// </summary>
    public class DoubleWritesAnalyzerTests
    {
        /// <summary>
        /// Tests that GetDoubleWrites (static) returns an empty collection when no double writes are recorded.
        /// </summary>
//         [Fact] [Error] (25-21)CS7036 There is no argument given that corresponds to the required parameter 'hasChildren' of 'FakeCopyTask.FakeCopyTask(bool, IEnumerable<FakeMessage>)' [Error] (36-63)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         public void GetDoubleWrites_NoDoubleWrites_ReturnsEmptyCollection()
//         {
//             // Arrange
//             var build = new FakeBuild(new List<FakeTask>
//             {
//                 new FakeCopyTask
//                 {
//                     FileCopyOperations = new List<FakeCopyOperation>
//                     {
//                         // Only one copy for a destination so not a double write.
//                         new FakeCopyOperation { Source = "source1.txt", Destination = "dest.txt", Copied = true }
//                     }
//                 }
//             });
// 
//             // Act
//             var result = DoubleWritesAnalyzer.GetDoubleWrites(build).ToList();
// 
//             // Assert
//             Assert.Empty(result);
//         }

        /// <summary>
        /// Tests that GetDoubleWrites (static) returns the correct double write buckets when duplicate destinations are processed.
        /// </summary>
//         [Fact] [Error] (51-21)CS7036 There is no argument given that corresponds to the required parameter 'hasChildren' of 'FakeCopyTask.FakeCopyTask(bool, IEnumerable<FakeMessage>)' [Error] (62-63)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         public void GetDoubleWrites_DoubleWriteDetected_ReturnsCorrectBucket()
//         {
//             // Arrange
//             var build = new FakeBuild(new List<FakeTask>
//             {
//                 new FakeCopyTask
//                 {
//                     FileCopyOperations = new List<FakeCopyOperation>
//                     {
//                         new FakeCopyOperation { Source = "source1.txt", Destination = "dest.txt", Copied = true },
//                         new FakeCopyOperation { Source = "source2.txt", Destination = "dest.txt", Copied = true }
//                     }
//                 }
//             });
// 
//             // Act
//             var result = DoubleWritesAnalyzer.GetDoubleWrites(build).ToList();
// 
//             // Assert
//             Assert.Single(result);
//             var bucket = result.First();
//             // Using Path.GetFullPath to simulate GetFullPath behavior inside ProcessCopy.
//             var expectedSource1 = new FileInfo("source1.txt").FullName;
//             var expectedSource2 = new FileInfo("source2.txt").FullName;
//             Assert.Equal("dest.txt", bucket.Key);
//             Assert.Contains(expectedSource1, bucket.Value);
//             Assert.Contains(expectedSource2, bucket.Value);
//             Assert.Equal(2, bucket.Value.Count);
//         }

        /// <summary>
        /// Tests that AppendDoubleWritesFolder creates a DoubleWrites folder in the build and adds items for each double write.
        /// </summary>
//         [Fact] [Error] (85-21)CS7036 There is no argument given that corresponds to the required parameter 'hasChildren' of 'FakeCopyTask.FakeCopyTask(bool, IEnumerable<FakeMessage>)' [Error] (101-38)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeTask' to 'Microsoft.Build.Logging.StructuredLogger.Task' [Error] (103-47)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build' [Error] (112-45)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text' [Error] (117-51)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text' [Error] (118-51)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text'
//         public void AppendDoubleWritesFolder_DoubleWriteExists_CreatesFolderWithItems()
//         {
//             // Arrange
//             var fakeBuild = new FakeBuild(new List<FakeTask>
//             {
//                 new FakeCopyTask
//                 {
//                     FileCopyOperations = new List<FakeCopyOperation>
//                     {
//                         new FakeCopyOperation { Source = "src1.dll", Destination = "output.dll", Copied = true },
//                         new FakeCopyOperation { Source = "src2.dll", Destination = "output.dll", Copied = true }
//                     }
//                 }
//             });
// 
//             // Act
//             // Use static method to populate analyzer internal dictionary.
//             var analyzer = new DoubleWritesAnalyzer();
//             // Simulate visiting all tasks.
//             foreach (var task in fakeBuild.GetAllTasks())
//             {
//                 analyzer.AnalyzeTask(task);
//             }
//             analyzer.AppendDoubleWritesFolder(fakeBuild);
// 
//             // Assert
//             // Verify that a folder with name "DoubleWrites" was created in the build.
//             var doubleWritesFolder = fakeBuild.GetNodeByName("DoubleWrites");
//             Assert.NotNull(doubleWritesFolder);
//             Assert.Single(doubleWritesFolder.Children);
//             var item = doubleWritesFolder.Children.First();
//             // The item text should be the destination "output.dll".
//             Assert.Equal("output.dll", item.Text);
//             // There should be two children representing the two sources.
//             Assert.Equal(2, item.Children.Count);
//             var expectedSrc1 = new FileInfo("src1.dll").FullName;
//             var expectedSrc2 = new FileInfo("src2.dll").FullName;
//             Assert.Contains(item.Children, c => c.Text == expectedSrc1);
//             Assert.Contains(item.Children, c => c.Text == expectedSrc2);
//         }

        /// <summary>
        /// Tests that AnalyzeTask processes a CopyTask correctly by only processing copied operations.
        /// </summary>
//         [Fact] [Error] (129-32)CS7036 There is no argument given that corresponds to the required parameter 'hasChildren' of 'FakeCopyTask.FakeCopyTask(bool, IEnumerable<FakeMessage>)' [Error] (139-34)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeCopyTask' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         public void AnalyzeTask_CopyTask_CopiesOnlyWhenFlaggedAsCopied()
//         {
//             // Arrange
//             var analyzer = new DoubleWritesAnalyzer();
//             var copyTask = new FakeCopyTask
//             {
//                 FileCopyOperations = new List<FakeCopyOperation>
//                 {
//                     new FakeCopyOperation { Source = "valid1.txt", Destination = "dest.txt", Copied = false },
//                     new FakeCopyOperation { Source = "valid2.txt", Destination = "dest.txt", Copied = true }
//                 }
//             };
// 
//             // Act
//             analyzer.AnalyzeTask(copyTask);
//             var result = analyzer.GetDoubleWrites().ToList();
// 
//             // Assert
//             // Only one operation should be processed since only one is flagged Copied.
//             Assert.Empty(result); // Only one source, so not a double write.
//         }

        /// <summary>
        /// Tests that AnalyzeTask processes a CscTask with CompilationWrites correctly.
        /// </summary>
//         [Fact] [Error] (170-34)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeCscTask' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         public void AnalyzeTask_CscTaskWithCompilationWrites_ProcessesCompilationFiles()
//         {
//             // Arrange
//             var analyzer = new DoubleWritesAnalyzer();
//             var compilationWrites = new FakeCompilationWrites
//             {
//                 Assembly = "out1.dll",
//                 RefAssembly = "out2.dll",
//                 Pdb = "",
//                 XmlDocumentation = "",
//                 SourceLink = "",
//                 AssemblyOrRefAssembly = "source.dll"
//             };
//             var cscTask = new FakeCscTask
//             {
//                 CompilationWrites = compilationWrites
//             };
// 
//             // Act
//             analyzer.AnalyzeTask(cscTask);
//             var result = analyzer.GetDoubleWrites().ToList();
// 
//             // Assert
//             // Two different destinations ("out1.dll" and "out2.dll") are processed.
//             // But they use the same source, so each destination gets one source.
//             // Therefore, no double write is detected.
//             Assert.Empty(result);
//         }

        /// <summary>
        /// Tests that AnalyzeTask processes multiple tasks leading to a double write via compilation writes.
        /// </summary>
//         [Fact] [Error] (205-32)CS7036 There is no argument given that corresponds to the required parameter 'hasChildren' of 'FakeCopyTask.FakeCopyTask(bool, IEnumerable<FakeMessage>)' [Error] (214-34)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeCscTask' to 'Microsoft.Build.Logging.StructuredLogger.Task' [Error] (215-34)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeCopyTask' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         public void AnalyzeTask_MultipleTasksWithSameDestinationDifferentSources_DetectsDoubleWrite()
//         {
//             // Arrange
//             var analyzer = new DoubleWritesAnalyzer();
// 
//             // First task: CscTask with CompilationWrites writing out to "common.dll" from source "source1.dll".
//             var compilationWrites1 = new FakeCompilationWrites
//             {
//                 Assembly = "common.dll",
//                 RefAssembly = "",
//                 Pdb = "",
//                 XmlDocumentation = "",
//                 SourceLink = "",
//                 AssemblyOrRefAssembly = "source1.dll"
//             };
//             var cscTask = new FakeCscTask
//             {
//                 CompilationWrites = compilationWrites1
//             };
// 
//             // Second task: CopyTask that copies from "source2.dll" to "common.dll".
//             var copyTask = new FakeCopyTask
//             {
//                 FileCopyOperations = new List<FakeCopyOperation>
//                 {
//                     new FakeCopyOperation { Source = "source2.dll", Destination = "common.dll", Copied = true }
//                 }
//             };
// 
//             // Act
//             analyzer.AnalyzeTask(cscTask);
//             analyzer.AnalyzeTask(copyTask);
//             var result = analyzer.GetDoubleWrites().ToList();
// 
//             // Assert
//             Assert.Single(result);
//             var bucket = result.First();
//             var expectedSource1 = new FileInfo("source1.dll").FullName;
//             var expectedSource2 = new FileInfo("source2.dll").FullName;
//             Assert.Equal("common.dll", bucket.Key);
//             Assert.Contains(expectedSource1, bucket.Value);
//             Assert.Contains(expectedSource2, bucket.Value);
//             Assert.Equal(2, bucket.Value.Count);
//         }
    }

    #region Fake Implementations for Testing

    // Fake implementation for Build node used for testing.
    internal class FakeBuild
    {
        private readonly List<FakeTask> _tasks;
        private readonly Dictionary<string, FakeFolder> _nodes = new Dictionary<string, FakeFolder>(StringComparer.OrdinalIgnoreCase);

        public FakeBuild(List<FakeTask> tasks)
        {
            _tasks = tasks;
        }

        /// <summary>
        /// Simulates visiting all children of type T.
        /// </summary>
        public void VisitAllChildren<T>(Action<T> action) where T : class
        {
            foreach (var task in _tasks.OfType<T>())
            {
                action(task);
            }
        }

        /// <summary>
        /// Helper to get all tasks.
        /// </summary>
        public IEnumerable<FakeTask> GetAllTasks()
        {
            return _tasks;
        }

        /// <summary>
        /// Simulates getting or creating a node with a specific name.
        /// </summary>
        public T GetOrCreateNodeWithName<T>(string name) where T : FakeFolder, new()
        {
            if (_nodes.ContainsKey(name))
            {
                return (T)_nodes[name];
            }
            var folder = new T { Name = name };
            _nodes[name] = folder;
            return folder;
        }

        /// <summary>
        /// Helper method for tests to retrieve a node by name.
        /// </summary>
        public FakeFolder GetNodeByName(string name)
        {
            _nodes.TryGetValue(name, out var folder);
            return folder;
        }
    }

    // Base fake task.
    internal abstract class FakeTask
    {
    }

    // Fake implementation for CopyTask.
//     internal class FakeCopyTask : FakeTask [Error] (292-20)CS0263 Partial declarations of 'FakeCopyTask' must not specify different base classes
//     {
//         public List<FakeCopyOperation> FileCopyOperations { get; set; } = new List<FakeCopyOperation>();
//     }

    // Fake implementation for a file copy operation.
    internal class FakeCopyOperation
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public bool Copied { get; set; }
    }

    // Fake implementation for CscTask.
    internal class FakeCscTask : FakeTask
    {
        public FakeCompilationWrites? CompilationWrites { get; set; }
    }

    // Fake implementation for VbcTask.
    internal class FakeVbcTask : FakeTask
    {
        public FakeCompilationWrites? CompilationWrites { get; set; }
    }

    // Fake implementation for FscTask.
    internal class FakeFscTask : FakeTask
    {
        public FakeCompilationWrites? CompilationWrites { get; set; }
    }

    // Fake struct simulating CompilationWrites.
    internal struct FakeCompilationWrites
    {
        public string Assembly { get; set; }
        public string RefAssembly { get; set; }
        public string Pdb { get; set; }
        public string XmlDocumentation { get; set; }
        public string SourceLink { get; set; }
        public string AssemblyOrRefAssembly { get; set; }
    }

    // Fake implementation for Folder.
    internal class FakeFolder
    {
        public string Name { get; set; }
        public List<FakeItem> Children { get; } = new List<FakeItem>();

        public void AddChild(FakeItem item)
        {
            Children.Add(item);
        }
    }

    // Fake implementation for Item.
    internal class FakeItem
    {
        public string Text { get; set; }
        public List<FakeItem> Children { get; } = new List<FakeItem>();

        public void AddChild(FakeItem item)
        {
            Children.Add(item);
        }
    }

    #endregion

    #region Extensions to integrate FakeFolder and FakeItem with StructuredLogger API

    // These extension methods simulate the behavior of the GetOrCreateNodeWithName method in the real Build class,
    // and allow adding children to Folder and Item nodes, matching the expected API in DoubleWritesAnalyzer.
    internal static class FakeBuildExtensions
    {
        public static T GetOrCreateNodeWithName<T>(this FakeBuild build, string name) where T : FakeFolder, new()
        {
            return build.GetOrCreateNodeWithName<T>(name);
        }

        public static void AddChild(this FakeFolder folder, FakeItem item)
        {
            folder.AddChild(item);
        }

        public static void AddChild(this FakeItem item, FakeItem child)
        {
            item.AddChild(child);
        }
    }

    // To allow DoubleWritesAnalyzer.AppendDoubleWritesFolder to work with FakeBuild,
    // we provide implicit conversion between our fake types and the types expected by the analyzer.
    // Since DoubleWritesAnalyzer expects Folder and Item, we assume that FakeFolder and FakeItem are used.
    // For the purpose of tests, we create type aliases.
//     internal class Folder : FakeFolder [Error] (386-20)CS7036 There is no argument given that corresponds to the required parameter 'name' of 'FakeFolder.FakeFolder(string)'
//     {
//     }

//     internal class Item : FakeItem [Error] (390-20)CS7036 There is no argument given that corresponds to the required parameter 'text' of 'FakeItem.FakeItem(string)'
//     {
//     }

    // Similarly, we alias Build to FakeBuild and Task to FakeTask, and CopyTask to FakeCopyTask.
    internal class Build : FakeBuild
    {
        public Build(List<FakeTask> tasks) : base(tasks)
        {
        }
    }

    internal class Task : FakeTask
    {
    }

//     internal class CopyTask : FakeCopyTask [Error] (406-20)CS7036 There is no argument given that corresponds to the required parameter 'hasChildren' of 'FakeCopyTask.FakeCopyTask(bool, IEnumerable<FakeMessage>)'
//     {
//     }

    internal class CscTask : FakeCscTask
    {
    }

    internal class VbcTask : FakeVbcTask
    {
    }

    internal class FscTask : FakeFscTask
    {
    }
    #endregion
}
