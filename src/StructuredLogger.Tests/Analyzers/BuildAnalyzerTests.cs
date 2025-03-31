// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Microsoft.Build.Logging.StructuredLogger.UnitTests;
// using Moq;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "BuildAnalyzer"/> class.
//     /// </summary>
//     public class BuildAnalyzerTests
//     {
// //         /// <summary> // [Error] (26-19)CS1061 'FakeBuild' does not contain a definition for 'IsAnalyzed' and no accessible extension method 'IsAnalyzed' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?) // [Error] (38-19)CS1061 'FakeBuild' does not contain a definition for 'EvaluationFolder' and no accessible extension method 'EvaluationFolder' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?) // [Error] (40-19)CS1061 'FakeBuild' does not contain a definition for 'EnvironmentFolder' and no accessible extension method 'EnvironmentFolder' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?) // [Error] (45-40)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build' // [Error] (47-19)CS1061 'FakeBuild' does not contain a definition for 'IsAnalyzed' and no accessible extension method 'IsAnalyzed' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?) // [Error] (49-19)CS1061 'FakeBuild' does not contain a definition for 'Statistics' and no accessible extension method 'Statistics' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests that AnalyzeBuild analyzes an unanalyzed build, performs evaluation and environment analysis,
// //         /// and marks the build as analyzed.
// //         /// </summary>
// //         [Fact]
// //         public void AnalyzeBuild_WhenNotAnalyzed_ShouldAnalyzeAndMarkAsAnalyzed()
// //         {
// //             // Arrange
// //             var build = new FakeBuild();
// //             build.IsAnalyzed.Should().BeFalse("build initially should not be analyzed");
// //             // Set EvaluationFolder with a child ProjectEvaluation to trigger AnalyzeEvaluation branch.
// //             var evalFolder = new Folder
// //             {
// //                 Name = "Evaluation"
// //             };
// //             var projEval = new ProjectEvaluation
// //             {
// //                 Name = "ProjectEvaluation",
// //                 Duration = TimeSpan.FromMilliseconds(500)
// //             };
// //             evalFolder.Children.Add(projEval);
// //             build.EvaluationFolder = evalFolder;
// //             // Set EnvironmentFolder to trigger AnalyzeEnvironment branch.
// //             build.EnvironmentFolder = new Folder
// //             {
// //                 Name = "Environment"
// //             };
// //             // Act
// //             BuildAnalyzer.AnalyzeBuild(build);
// //             // Assert
// //             build.IsAnalyzed.Should().BeTrue("after analysis the build should be marked as analyzed");
// //             // In this minimal analysis, Statistics.TimedNodeCount is set to the analyzer's index value.
// //             build.Statistics.TimedNodeCount.Should().BeGreaterOrEqualTo(0, "TimedNodeCount must be a non-negative integer");
// //             // Verify that RelativeDuration has been set in the ProjectEvaluation child.
// //             projEval.RelativeDuration.Should().BeGreaterThan(0, "RelativeDuration should be calculated for project evaluations");
// //         }
// //  // [Error] (62-19)CS1061 'FakeBuild' does not contain a definition for 'IsAnalyzed' and no accessible extension method 'IsAnalyzed' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?) // [Error] (65-19)CS1061 'FakeBuild' does not contain a definition for 'Children' and no accessible extension method 'Children' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?) // [Error] (67-40)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build' // [Error] (71-19)CS1061 'FakeBuild' does not contain a definition for 'Statistics' and no accessible extension method 'Statistics' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that when the build is already analyzed, calling AnalyzeBuild seals and recalculates indices.
//         /// </summary>
//         [Fact]
//         public void AnalyzeBuild_WhenAlreadyAnalyzed_ShouldSealIndices()
//         {
//             // Arrange
//             var build = new FakeBuild();
//             build.IsAnalyzed = true;
//             // Add a TimedNode child to the build's Children collection.
//             var timedNode = new TimedNode();
//             build.Children.Add(timedNode);
//             // Act
//             BuildAnalyzer.AnalyzeBuild(build);
//             // Assert
//             // SealAndCalculateIndices should assign indices to all TimedNode children.
//             timedNode.Index.Should().Be(0, "the first TimedNode in the traversal should have index 0");
//             build.Statistics.TimedNodeCount.Should().Be(1, "TimedNodeCount should reflect the visited timed nodes count");
//         }
// //  // [Error] (105-19)CS1061 'FakeBuild' does not contain a definition for 'Children' and no accessible extension method 'Children' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?) // [Error] (112-19)CS1061 'FakeBuild' does not contain a definition for 'Children' and no accessible extension method 'Children' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?) // [Error] (114-52)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build' // [Error] (114-90)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         /// <summary>
// //         /// Tests that GetProjectsSortedTopologically returns projects in a topologically sorted order.
// //         /// </summary>
// //         [Fact]
// //         public void GetProjectsSortedTopologically_WhenCalled_ReturnsTopologicallySortedProjects()
// //         {
// //             // Arrange
// //             var build = new FakeBuild();
// //             // Create projects with nested structure.
// //             // Parent project
// //             var project1 = new Project
// //             {
// //                 ProjectFile = "A.csproj",
// //                 EvaluationId = 1
// //             };
// //             // Child project
// //             var project2 = new Project
// //             {
// //                 ProjectFile = "B.csproj",
// //                 EvaluationId = 1
// //             };
// //             // Grandchild project
// //             var project3 = new Project
// //             {
// //                 ProjectFile = "C.csproj",
// //                 EvaluationId = 1
// //             };
// //             // Establish nesting
// //             project1.Children.Add(project2);
// //             project2.Children.Add(project3);
// //             // Add projects to build children collection
// //             build.Children.Add(project1);
// //             // Also add an independent project.
// //             var independentProject = new Project
// //             {
// //                 ProjectFile = "D.csproj",
// //                 EvaluationId = 1
// //             };
// //             build.Children.Add(independentProject);
// //             // Act
// //             var sortedProjects = new BuildAnalyzer(build).GetProjectsSortedTopologically(build).ToList();
// //             // Assert
// //             // Expect that each project appears exactly once.
// //             sortedProjects.Should().HaveCount(4, "all projects should be returned without duplicates");
// //             // The parent projects should come after their nested children (postorder traversal from Visit method)
// //             // Based on the implementation, the deepest nested project is added first.
// //             sortedProjects[0].ProjectFile.Should().Be("C.csproj", "the deepest project should appear first");
// //             sortedProjects[1].ProjectFile.Should().Be("B.csproj", "the child project should appear after its nested project");
// //             sortedProjects[2].ProjectFile.Should().Be("A.csproj", "the parent project should appear after its children");
// //             sortedProjects[3].ProjectFile.Should().Be("D.csproj", "the independent project should be added separately");
// //         }
// //     }
// //  // [Error] (130-18)CS0263 Partial declarations of 'FakeBuild' must not specify different base classes
// // #region Fake Build and Related Classes
// //     // Minimal fake implementations required for testing BuildAnalyzer.
// //     // These classes mimic the minimal interface of production classes.
// //     public class FakeBuild : Build
// //     {
// //         public FakeBuild()
// //         {
// //         // Initialize base properties if necessary.
// //         }
// //     }
// // 
//     // Minimal implementations for Build and related classes.
//     // In a real test environment, these types would be defined in the production code.
//     public class Build
//     {
//         public bool IsAnalyzed { get; set; }
//         public Folder EvaluationFolder { get; set; }
//         public Folder EnvironmentFolder { get; set; }
//         public string LogFilePath { get; set; }
//         public Statistics Statistics { get; } = new Statistics();
//         public StringTable StringTable { get; } = new StringTable();
//         public List<object> SearchExtensions { get; } = new List<object>();
//         public List<TreeNode> Children { get; } = new List<TreeNode>();
// 
//         public void VisitAllChildren<T>(Action<T> action)
//             where T : TreeNode
//         {
//             void Recurse(TreeNode node)
//             {
//                 if (node is T t)
//                 {
//                     action(t);
//                 }
// 
//                 foreach (var child in node.Children)
//                 {
//                     Recurse(child);
//                 }
//             }
// 
//             foreach (var child in Children)
//             {
//                 Recurse(child);
//             }
//         }
// 
//         public void AddChild(TreeNode child) => Children.Add(child);
//         public void AddChildAtBeginning(TreeNode child) => Children.Insert(0, child);
//         public T GetOrCreateNodeWithName<T>(string name)
//             where T : NamedNode, new()
//         {
//             var existing = Children.OfType<T>().FirstOrDefault(n => n.Name == name);
//             if (existing != null)
//             {
//                 return existing;
//             }
// 
//             var node = new T
//             {
//                 Name = name
//             };
//             Children.Add(node);
//             return node;
//         }
// //  // [Error] (197-17)CS0117 'ProjectEvaluation' does not contain a definition for 'TargetFramework' // [Error] (198-17)CS0117 'ProjectEvaluation' does not contain a definition for 'Platform' // [Error] (199-17)CS0117 'ProjectEvaluation' does not contain a definition for 'Configuration'
// //         public ProjectEvaluation FindEvaluation(int id)
// //         {
// //             // Simplified implementation for testing.
// //             return id == 1 ? new ProjectEvaluation
// //             {
// //                 TargetFramework = "net5.0",
// //                 Platform = "AnyCPU",
// //                 Configuration = "Debug"
// //             }
// // 
// //             : null;
// //         }
// // 
//         public void RegisterTask(Task task)
//         {
//         // No-op for testing.
//         }
//     }
// 
//     public class Statistics
//     {
//         public int TimedNodeCount { get; set; }
//     }
// 
//     public class StringTable
//     {
//         public string Intern(string text) => text;
//     }
// 
//     public class TreeNode
//     {
//         public List<TreeNode> Children { get; } = new List<TreeNode>();
//         public bool HasChildren => Children.Any();
//     }
// 
//     public class TimedNode : TreeNode
//     {
//         public int Index { get; set; }
//         public TimeSpan Duration { get; set; } = TimeSpan.Zero;
//     }
// 
//     public class NamedNode : TreeNode
//     {
//         public string Name { get; set; } = string.Empty;
// 
//         public void SortChildren()
//         {
//         // No-op for testing.
//         }
//     }
// 
//     public class Folder : NamedNode
//     {
//     }
// 
//     public class ProjectEvaluation : NamedNode
//     {
//         public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(1);
//         public double RelativeDuration { get; set; }
//     }
// 
//     public class Project : TimedNode
//     {
//         public string ProjectFile { get; set; } = string.Empty;
//         public int EvaluationId { get; set; }
//         public string TargetFramework { get; set; }
//         public string Platform { get; set; }
//         public string Configuration { get; set; }
//         public bool IsLowRelevance { get; set; }
//         public Folder EntryTargets { get; set; }
//     }
// 
//     public class Task : TimedNode
//     {
//         public string Name { get; set; } = string.Empty;
//         public string FromAssembly { get; set; }
//         public Task Parent { get; set; }
//     }
// 
//     public class Target : TimedNode
//     {
//         public string Name { get; set; } = string.Empty;
//         public bool Skipped { get; set; }
//         public string DependsOnTargets { get; set; }
//         public bool IsLowRelevance { get; set; }
//     }
// 
//     public class Message : TreeNode
//     {
//         public bool IsLowRelevance { get; set; }
//     }
// 
//     public class Property : TreeNode
//     {
//         public string Name { get; set; } = string.Empty;
//         public string Value { get; set; } = string.Empty;
//     }
// 
//     public class BuildError : TreeNode
//     {
//         public string Text { get; set; } = string.Empty;
//     }
// 
//     public class Item : TreeNode
//     {
//         public string Text { get; set; } = string.Empty;
//     }
// 
//     public class Note : TreeNode
//     {
//         public string Text { get; set; } = string.Empty;
//     }
// 
//     public class SearchableItem : TreeNode
//     {
//         public string Text { get; set; } = string.Empty;
//         public string SearchText { get; set; } = string.Empty;
// 
//         public void AddChild(SearchableItem item) => Children.Add(item);
//     }
// 
//     public static class TextUtilities
//     {
//         public static string DisplayDuration(TimeSpan duration) => duration.TotalMilliseconds + "ms";
//     }
// 
//     public static class Strings
//     {
//         public static string PropertyReassignmentFolder { get; } = "PropertyReassignmentFolder";
//         public static string Duration { get; } = "Duration";
//         public static string Assembly { get; } = "Assembly";
//         public static string Messages { get; } = "Messages";
//         public static string EntryTargets { get; } = "EntryTargets";
//     }
// 
//     public class DoubleWritesAnalyzer
//     {
//         public void AnalyzeTask(Task task)
//         {
//         }
// 
//         public void AppendDoubleWritesFolder(Build build)
//         {
//         }
//     }
// 
//     public class ResolveAssemblyReferenceAnalyzer
//     {
//         public void AnalyzeResolveAssemblyReference(Task task)
//         {
//         }
// 
//         public void AppendFinalReport(Build build)
//         {
//         }
//     }
// 
//     public class CppAnalyzer
//     {
//         public void AnalyzeEnvironment(NamedNode folder)
//         {
//         }
// 
//         public void AppendCppAnalyzer(Build build)
//         {
//         }
// 
//         public void AnalyzeTask(Task cppTask)
//         {
//         }
// 
//         public class CppTask : Task
//         {
//         }
//     }
// 
//     public class CscTaskAnalyzer
//     {
//         public static (Folder analyzerReport, Folder generatorReport) Analyze(Task task) => (null, null);
//         public static void CreateMergedReport(Folder folder, Folder[] reports)
//         {
//         }
//     }
// 
//     public class MessageTaskAnalyzer
//     {
//         public static void Analyze(Task task)
//         {
//         }
//     }
// #endregion
// }