// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Moq;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ResolveAssemblyReferenceAnalyzer"/> class.
//     /// </summary>
//     public class ResolveAssemblyReferenceAnalyzerTests
//     {
//         private readonly ResolveAssemblyReferenceAnalyzer _analyzer;
// 
//         public ResolveAssemblyReferenceAnalyzerTests()
//         {
//             _analyzer = new ResolveAssemblyReferenceAnalyzer();
//         }
// //  // [Error] (39-55)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.TestTask' to 'Microsoft.Build.Logging.StructuredLogger.Task'
// //         /// <summary>
// //         /// Tests that AnalyzeResolveAssemblyReference updates TotalRARDuration using the Task.Duration.
// //         /// This partial test uses a minimal test double for Task.
// //         /// </summary>
// //         [Fact]
// //         public void AnalyzeResolveAssemblyReference_ValidTask_UpdatesTotalRARDuration()
// //         {
// //             // Arrange
// //             // TODO: Enhance the test double to simulate additional child node behaviors as needed.
// //             var expectedDuration = TimeSpan.FromSeconds(5);
// //             var testTask = new TestTask
// //             {
// //                 Duration = expectedDuration
// //             };
// // 
// //             // Act
// //             _analyzer.AnalyzeResolveAssemblyReference(testTask);
// // 
// //             // Assert
// //             _analyzer.TotalRARDuration.Should().Be(expectedDuration, "because TotalRARDuration should be incremented by Task.Duration");
// //         }
// //  // [Error] (55-74)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.TestTask' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         /// <summary>
//         /// Tests that AnalyzeResolveAssemblyReference throws a NullReferenceException when passed a null Task.
//         /// </summary>
//         [Fact]
//         public void AnalyzeResolveAssemblyReference_NullTask_ThrowsNullReferenceException()
//         {
//             // Arrange
//             TestTask? nullTask = null;
// 
//             // Act
//             Action act = () => _analyzer.AnalyzeResolveAssemblyReference(nullTask!);
// 
//             // Assert
//             act.Should().Throw<NullReferenceException>("because passing a null Task should result in a NullReferenceException");
//         }
// //  // [Error] (76-41)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.TestBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         /// <summary>
// //         /// Tests that AppendFinalReport appends used and unused search path locations to the Build report.
// //         /// This partial test uses a minimal test double for Build.
// //         /// </summary>
// //         [Fact]
// //         public void AppendFinalReport_ValidBuild_AppendsSearchPathLocations()
// //         {
// //             // Arrange
// //             // TODO: Enhance the test double for Build to simulate full node behavior if needed.
// //             var testBuild = new TestBuild();
// //             // Pre-populate UsedLocations and UnusedLocations for the test.
// //             _analyzer.UsedLocations.Add("Location1");
// //             _analyzer.UnusedLocations.Add("Location2");
// // 
// //             // Act
// //             _analyzer.AppendFinalReport(testBuild);
// // 
// //             // Assert
// //             testBuild.UsedAssemblySearchPathsLocationsNode.Should().NotBeNull("because a node for used locations should be created");
// //             testBuild.UnusedAssemblySearchPathsLocationsNode.Should().NotBeNull("because a node for unused locations should be created");
// // 
// //             testBuild.UsedAssemblySearchPathsLocationsNode!.Children
// //                 .Select(n => (n as Item)?.Text)
// //                 .Should().Contain("Location1", "because used location 'Location1' should be added to the report");
// // 
// //             testBuild.UnusedAssemblySearchPathsLocationsNode!.Children
// //                 .Select(n => (n as Item)?.Text)
// //                 .Should().Contain("Location2", "because unused location 'Location2' should be added to the report");
// //         }
// //  // [Error] (101-60)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.TestBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         /// <summary>
//         /// Tests that AppendFinalReport throws a NullReferenceException when passed a null Build.
//         /// </summary>
//         [Fact]
//         public void AppendFinalReport_NullBuild_ThrowsNullReferenceException()
//         {
//             // Arrange
//             TestBuild? nullBuild = null;
// 
//             // Act
//             Action act = () => _analyzer.AppendFinalReport(nullBuild!);
// 
//             // Assert
//             act.Should().Throw<NullReferenceException>("because calling AppendFinalReport with null Build should throw an exception");
//         }
//     }
// 
//     // ---------------------------------------------------------------------------
//     // Minimal test doubles for required types.
//     // These implementations are provided as partial fakes to support testing of ResolveAssemblyReferenceAnalyzer.
//     // In a comprehensive test suite, they should be replaced with more complete implementations.
//     // ---------------------------------------------------------------------------
// 
//     /// <summary>
//     /// Minimal test double for Microsoft.Build.Logging.StructuredLogger.Task.
//     /// </summary>
//     internal class TestTask : Task
//     {
//         public override TimeSpan Duration { get; set; }
// //  // [Error] (122-28)CS0453 The type 'T' must be a non-nullable value type in order to use it as parameter 'T' in the generic type or method 'Nullable<T>' // [Error] (127-24)CS0266 Cannot implicitly convert type 'T' to 'T?'. An explicit conversion exists (are you missing a cast?)
// //         // Minimal implementation for GetNearestParent<T>
// //         public override T? GetNearestParent<T>(Predicate<T> predicate)
// //         {
// //             if (typeof(T) == typeof(Build))
// //             {
// //                 var build = new TestBuild { StringTable = new StringCache() } as T;
// //                 return build;
// //             }
// //             return default;
// //         }
// //  // [Error] (133-28)CS0508 'TestTask.FindChild<T>(Predicate<T>)': return type must be 'T' to match overridden member 'Task.FindChild<T>(Predicate<T>)' // [Error] (133-28)CS0453 The type 'T' must be a non-nullable value type in order to use it as parameter 'T' in the generic type or method 'Nullable<T>'
// //         // Minimal implementation for FindChild<T>
// //         public override T? FindChild<T>(Predicate<T> predicate)
// //         {
// //             // For testing, return null. Extend this method if additional behavior is needed.
// //             return default;
// //         }
// // 
//         // Minimal override for GetOrCreateNodeWithName<T>
//         public override T GetOrCreateNodeWithName<T>(string name)
//         {
//             if (typeof(T) == typeof(Folder))
//             {
//                 return new TestFolder(name) as T;
//             }
//             return base.GetOrCreateNodeWithName<T>(name);
//         }
//     }
// 
//     /// <summary>
//     /// Minimal test double for Microsoft.Build.Logging.StructuredLogger.Build.
//     /// </summary>
//     internal class TestBuild : Build
//     {
//         public Folder? UsedAssemblySearchPathsLocationsNode { get; set; }
//         public Folder? UnusedAssemblySearchPathsLocationsNode { get; set; }
// 
//         public override Folder GetOrCreateNodeWithName<Folder>(string name)
//         {
//             if (name == Strings.UsedAssemblySearchPathsLocations)
//             {
//                 UsedAssemblySearchPathsLocationsNode = new TestFolder(name);
//                 return UsedAssemblySearchPathsLocationsNode as Folder;
//             }
//             if (name == Strings.UnusedAssemblySearchPathsLocations)
//             {
//                 UnusedAssemblySearchPathsLocationsNode = new TestFolder(name);
//                 return UnusedAssemblySearchPathsLocationsNode as Folder;
//             }
//             return base.GetOrCreateNodeWithName<Folder>(name);
//         }
//     }
// 
//     /// <summary>
//     /// Minimal test double for Microsoft.Build.Logging.StructuredLogger.Folder.
//     /// </summary>
//     internal class TestFolder : Folder
//     {
//         public TestFolder(string name)
//         {
//             Name = name;
//             Children = new List<BaseNode>();
//         }
//     }
// 
//     // Minimal placeholder for StringCache.
//     internal class StringCache
//     {
//         public string Intern(string text) => text;
//     }
// 
//     // Minimal placeholder implementations for the base types and nodes.
//     internal class Task : BaseNode
//     {
//         public virtual TimeSpan Duration { get; set; } = TimeSpan.Zero;
// //  // [Error] (197-28)CS0508 'Task.GetNearestParent<T>(Predicate<T>)': return type must be 'T' to match overridden member 'BaseNode.GetNearestParent<T>(Predicate<T>)' // [Error] (197-28)CS0453 The type 'T' must be a non-nullable value type in order to use it as parameter 'T' in the generic type or method 'Nullable<T>'
// //         public override T? GetNearestParent<T>(Predicate<T> predicate)
// //         {
// //             return default;
// //         }
// // 
//         public virtual T? FindChild<T>(Predicate<T> predicate)
//             where T : BaseNode
//         {
//             return default;
//         }
// 
//         public virtual T GetOrCreateNodeWithName<T>(string name)
//             where T : BaseNode, new()
//         {
//             return new T();
//         }
//     }
// 
//     internal class Build : BaseNode
//     {
// //         public StringCache? StringTable { get; set; } // [Error] (217-29)CS0053 Inconsistent accessibility: property type 'StringCache' is less accessible than property 'Build.StringTable'
// //         public override T GetOrCreateNodeWithName<T>(string name) // [Error] (219-23)CS0460 Constraints for override and explicit interface implementation methods are inherited from the base method, so they cannot be specified directly, except for either a 'class', or a 'struct' constraint.
//             where T : BaseNode, new()
//         {
//             return new T();
//         }
//     }
// 
//     internal class Folder : BaseNode
//     {
//         public string Name { get; set; } = string.Empty;
//         public List<BaseNode> Children { get; set; } = new List<BaseNode>();
//     }
// 
//     internal class Item : BaseNode
//     {
//         public string Text { get; set; } = string.Empty;
//     }
// 
//     internal class BaseNode
//     {
//         public virtual T? GetNearestParent<T>(Predicate<T> predicate)
//             where T : class
//         {
//             return default;
//         }
// 
//         public virtual T? FindChild<T>(Predicate<T> predicate)
//             where T : BaseNode
//         {
//             return default;
//         }
// 
//         public virtual T GetOrCreateNodeWithName<T>(string name)
//             where T : BaseNode, new()
//         {
//             return new T();
//         }
//     }
// 
//     internal static class Strings
//     {
//         public const string Results = "Results";
//         public const string Parameters = "Parameters";
//         public const string SearchPaths = "SearchPaths";
//         public const string Assemblies = "Assemblies";
//         public const string UsedLocations = "UsedLocations";
//         public const string UnusedLocations = "UnusedLocations";
//         public const string UsedAssemblySearchPathsLocations = "UsedAssemblySearchPathsLocations";
//         public const string UnusedAssemblySearchPathsLocations = "UnusedAssemblySearchPathsLocations";
//     }
// }