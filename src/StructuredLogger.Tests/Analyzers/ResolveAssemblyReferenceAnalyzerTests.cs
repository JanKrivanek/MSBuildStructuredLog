// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Microsoft.Build.Logging.StructuredLogger.UnitTests;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "ResolveAssemblyReferenceAnalyzer"/> class.
//     /// </summary>
//     public class ResolveAssemblyReferenceAnalyzerTests
//     {
//         private readonly ResolveAssemblyReferenceAnalyzer _analyzer;
//         public ResolveAssemblyReferenceAnalyzerTests()
//         {
//             _analyzer = new ResolveAssemblyReferenceAnalyzer();
//         }
// //  // [Error] (39-17)CS0117 'FakeBuild' does not contain a definition for 'Name' // [Error] (42-23)CS1061 'FakeBuild' does not contain a definition for 'AddChild' and no accessible extension method 'AddChild' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?) // [Error] (80-55)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeTask' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' // [Error] (82-23)CS1061 'ResolveAssemblyReferenceAnalyzer' does not contain a definition for 'TotalRARDuration' and no accessible extension method 'TotalRARDuration' accepting a first argument of type 'ResolveAssemblyReferenceAnalyzer' could be found (are you missing a using directive or an assembly reference?) // [Error] (83-23)CS1061 'ResolveAssemblyReferenceAnalyzer' does not contain a definition for 'UsedLocations' and no accessible extension method 'UsedLocations' accepting a first argument of type 'ResolveAssemblyReferenceAnalyzer' could be found (are you missing a using directive or an assembly reference?) // [Error] (88-23)CS1061 'ResolveAssemblyReferenceAnalyzer' does not contain a definition for 'UnusedLocations' and no accessible extension method 'UnusedLocations' accepting a first argument of type 'ResolveAssemblyReferenceAnalyzer' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that AnalyzeResolveAssemblyReference correctly accumulates Duration,
// //         /// adds used locations based on the found reference and updates the UsedLocations folder.
// //         /// </summary>
// //         [Fact]
// //         public void AnalyzeResolveAssemblyReference_NoResolvedFilePath_AddsUsedLocationFromFoundAt()
// //         {
// //             // Arrange
// //             var duration = TimeSpan.FromSeconds(5);
// //             var fakeTask = new FakeTask
// //             {
// //                 Duration = duration,
// //                 Name = "Task"
// //             };
// //             // Create a fake Build parent with a StringCache.
// //             var fakeBuild = new FakeBuild
// //             {
// //                 Name = "Build",
// //                 StringTable = new FakeStringCache()
// //             };
// //             fakeBuild.AddChild(fakeTask);
// //             // Create Parameters folder with a SearchPaths named node.
// //             var parametersFolder = new FakeFolder
// //             {
// //                 Name = Strings.Parameters
// //             };
// //             fakeTask.AddChild(parametersFolder);
// //             var searchPathsNode = new FakeNamedNode
// //             {
// //                 Name = Strings.SearchPaths
// //             };
// //             parametersFolder.AddChild(searchPathsNode);
// //             // Add a search path value as a FakeItem.
// //             var searchPathItem = new FakeItem
// //             {
// //                 Text = @"C:\TestPath"
// //             };
// //             searchPathsNode.AddChild(searchPathItem);
// //             // Create Results folder with a Parameter reference.
// //             var resultsFolder = new FakeFolder
// //             {
// //                 Name = Strings.Results
// //             };
// //             fakeTask.AddChild(resultsFolder);
// //             var referenceParameter = new FakeParameter
// //             {
// //                 Name = "SomeReference"
// //             };
// //             resultsFolder.AddChild(referenceParameter);
// //             // Do not add a resolved file path item so that resolvedFilePath remains null.
// //             // Add found at location item.
// //             var foundAtItem = new FakeItem
// //             {
// //                 // Form the text as required by the analyzer.
// //                 // "Reference found at search path location \"" + location + "\""
// //                 Text = $"Reference found at search path location \"{@"C:\TestPath"}\""};
// //             referenceParameter.AddChild(foundAtItem);
// //             // Act
// //             _analyzer.AnalyzeResolveAssemblyReference(fakeTask);
// //             // Assert
// //             _analyzer.TotalRARDuration.Should().Be(duration);
// //             _analyzer.UsedLocations.Should().Contain(@"C:\TestPath");
// //             // Verify that the UsedLocations folder in fakeTask has been created with the correct item.
// //             var usedLocationsFolder = fakeTask.GetOrCreateNodeWithName<FakeFolder>(Strings.UsedLocations);
// //             usedLocationsFolder.Children.Should().ContainSingle().Which.As<FakeItem>().Text.Should().Be(@"C:\TestPath");
// //             // The search path was used, so UnusedLocations should not contain it.
// //             _analyzer.UnusedLocations.Should().NotContain(@"C:\TestPath");
// //         }
// //  // [Error] (107-17)CS0117 'FakeBuild' does not contain a definition for 'Name' // [Error] (110-23)CS1061 'FakeBuild' does not contain a definition for 'AddChild' and no accessible extension method 'AddChild' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?) // [Error] (128-55)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeTask' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' // [Error] (130-23)CS1061 'ResolveAssemblyReferenceAnalyzer' does not contain a definition for 'TotalRARDuration' and no accessible extension method 'TotalRARDuration' accepting a first argument of type 'ResolveAssemblyReferenceAnalyzer' could be found (are you missing a using directive or an assembly reference?) // [Error] (131-23)CS1061 'ResolveAssemblyReferenceAnalyzer' does not contain a definition for 'UsedLocations' and no accessible extension method 'UsedLocations' accepting a first argument of type 'ResolveAssemblyReferenceAnalyzer' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that AnalyzeResolveAssemblyReference does not process search paths when Parameters folder is missing.
//         /// </summary>
//         [Fact]
//         public void AnalyzeResolveAssemblyReference_NoParametersFolder_DoesNotProcessSearchPaths()
//         {
//             // Arrange
//             var duration = TimeSpan.FromSeconds(3);
//             var fakeTask = new FakeTask
//             {
//                 Duration = duration,
//                 Name = "Task"
//             };
//             // Create a Build parent.
//             var fakeBuild = new FakeBuild
//             {
//                 Name = "Build",
//                 StringTable = new FakeStringCache()
//             };
//             fakeBuild.AddChild(fakeTask);
//             // Do not add Parameters folder.
//             // Create Results folder with a Parameter reference having found location.
//             var resultsFolder = new FakeFolder
//             {
//                 Name = Strings.Results
//             };
//             fakeTask.AddChild(resultsFolder);
//             var referenceParameter = new FakeParameter
//             {
//                 Name = "SomeReference"
//             };
//             resultsFolder.AddChild(referenceParameter);
//             var foundAtItem = new FakeItem
//             {
//                 Text = $"Reference found at search path location \"{@"D:\AnotherPath"}\""};
//             referenceParameter.AddChild(foundAtItem);
//             // Act
//             _analyzer.AnalyzeResolveAssemblyReference(fakeTask);
//             // Assert
//             _analyzer.TotalRARDuration.Should().Be(duration);
//             _analyzer.UsedLocations.Should().Contain(@"D:\AnotherPath");
//             // Since there is no searchPaths provided, no UsedLocations folder should be created.
//             fakeTask.Children.Any(n => n.Name == Strings.UsedLocations).Should().BeFalse();
//         }
// //  // [Error] (152-17)CS0117 'FakeBuild' does not contain a definition for 'Name' // [Error] (155-23)CS1061 'FakeBuild' does not contain a definition for 'AddChild' and no accessible extension method 'AddChild' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?) // [Error] (202-55)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeTask' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task'
// //         /// <summary>
// //         /// Tests the Dependency branch in AnalyzeResolveAssemblyReference where a reference item identifies
// //         /// a dependency not marked as CopyLocal. It validates that metadata from source items is appended.
// //         /// </summary>
// //         [Fact]
// //         public void AnalyzeResolveAssemblyReference_DependencyReference_AppendsMetadataFromSourceItem()
// //         {
// //             // Arrange
// //             var fakeTask = new FakeTask
// //             {
// //                 Duration = TimeSpan.FromSeconds(2),
// //                 Name = "Task"
// //             };
// //             // Create a Build parent with a StringCache.
// //             var fakeBuild = new FakeBuild
// //             {
// //                 Name = "Build",
// //                 StringTable = new FakeStringCache()
// //             };
// //             fakeBuild.AddChild(fakeTask);
// //             // Create Results folder with a Dependency reference.
// //             var resultsFolder = new FakeFolder
// //             {
// //                 Name = Strings.Results
// //             };
// //             fakeTask.AddChild(resultsFolder);
// //             var dependencyReference = new FakeParameter
// //             {
// //                 Name = "Dependency \"SomeDependency, Version=1.0.0\""
// //             };
// //             resultsFolder.AddChild(dependencyReference);
// //             // Add child items: one RequiredBy and one not copy local message.
// //             var requiredByItem = new FakeItem
// //             {
// //                 Text = "Required by \"SourceAssembly\""
// //             };
// //             dependencyReference.AddChild(requiredByItem);
// //             var notCopyLocalMessage = new FakeItem
// //             {
// //                 Text = @"This reference is not ""CopyLocal"" because at least one source item had ""Private"" set to ""false"" and no source items had ""Private"" set to ""true""."
// //             };
// //             dependencyReference.AddChild(notCopyLocalMessage);
// //             // Create Parameters folder with Assemblies parameter.
// //             var parametersFolder = new FakeFolder
// //             {
// //                 Name = Strings.Parameters
// //             };
// //             fakeTask.AddChild(parametersFolder);
// //             var assembliesParameter = new FakeParameter
// //             {
// //                 Name = Strings.Assemblies
// //             };
// //             parametersFolder.AddChild(assembliesParameter);
// //             // Add a source item for "SourceAssembly" with metadata.
// //             var sourceItem = new FakeItem
// //             {
// //                 Text = "SourceAssembly"
// //             };
// //             var privateMetadata = new FakeMetadata
// //             {
// //                 Name = "Private",
// //                 Value = "false"
// //             };
// //             sourceItem.AddChild(privateMetadata);
// //             assembliesParameter.AddChild(sourceItem);
// //             // Act
// //             _analyzer.AnalyzeResolveAssemblyReference(fakeTask);
// //             // Assert
// //             // Verify that the requiredByItem now has a child Metadata with Name "Private" and Value "false".
// //             requiredByItem.Children.OfType<FakeMetadata>().Should().ContainSingle(m => m.Name == "Private" && m.Value == "false");
// //             // Verify that the notCopyLocalMessage now has a child Message containing the appropriate text.
// //             var expectedMessagePart = "SourceAssembly has Private set to false";
// //             notCopyLocalMessage.Children.OfType<FakeMessage>().Should().Contain(m => m.Text.Contains(expectedMessagePart));
// //         }
// //  // [Error] (219-23)CS1061 'ResolveAssemblyReferenceAnalyzer' does not contain a definition for 'UsedLocations' and no accessible extension method 'UsedLocations' accepting a first argument of type 'ResolveAssemblyReferenceAnalyzer' could be found (are you missing a using directive or an assembly reference?) // [Error] (220-23)CS1061 'ResolveAssemblyReferenceAnalyzer' does not contain a definition for 'UnusedLocations' and no accessible extension method 'UnusedLocations' accepting a first argument of type 'ResolveAssemblyReferenceAnalyzer' could be found (are you missing a using directive or an assembly reference?) // [Error] (221-23)CS1061 'ResolveAssemblyReferenceAnalyzer' does not contain a definition for 'UsedLocations' and no accessible extension method 'UsedLocations' accepting a first argument of type 'ResolveAssemblyReferenceAnalyzer' could be found (are you missing a using directive or an assembly reference?) // [Error] (222-23)CS1061 'ResolveAssemblyReferenceAnalyzer' does not contain a definition for 'UsedLocations' and no accessible extension method 'UsedLocations' accepting a first argument of type 'ResolveAssemblyReferenceAnalyzer' could be found (are you missing a using directive or an assembly reference?) // [Error] (223-23)CS1061 'ResolveAssemblyReferenceAnalyzer' does not contain a definition for 'UnusedLocations' and no accessible extension method 'UnusedLocations' accepting a first argument of type 'ResolveAssemblyReferenceAnalyzer' could be found (are you missing a using directive or an assembly reference?) // [Error] (227-17)CS0117 'FakeBuild' does not contain a definition for 'Name' // [Error] (231-41)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeBuild' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (234-48)CS1061 'FakeBuild' does not contain a definition for 'GetOrCreateNodeWithName' and no accessible extension method 'GetOrCreateNodeWithName' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?) // [Error] (237-50)CS1061 'FakeBuild' does not contain a definition for 'GetOrCreateNodeWithName' and no accessible extension method 'GetOrCreateNodeWithName' accepting a first argument of type 'FakeBuild' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that AppendFinalReport appends final used and unused search path locations to the build node.
//         /// </summary>
//         [Fact]
//         public void AppendFinalReport_WithUsedAndUnusedLocations_AppendsReportToBuild()
//         {
//             // Arrange
//             // Set up analyzer with some used and unused locations.
//             _analyzer.UsedLocations.Clear();
//             _analyzer.UnusedLocations.Clear();
//             _analyzer.UsedLocations.Add(@"C:\Used1");
//             _analyzer.UsedLocations.Add(@"C:\Used2");
//             _analyzer.UnusedLocations.Add(@"D:\Unused");
//             // Create a fake Build node.
//             var fakeBuild = new FakeBuild
//             {
//                 Name = "Build",
//                 StringTable = new FakeStringCache()
//             };
//             // Act
//             _analyzer.AppendFinalReport(fakeBuild);
//             // Assert
//             // Verify that build has a folder for used assembly search paths locations with sorted items.
//             var usedAssemblyFolder = fakeBuild.GetOrCreateNodeWithName<FakeFolder>(Strings.UsedAssemblySearchPathsLocations);
//             usedAssemblyFolder.Children.OfType<FakeItem>().Select(i => i.Text).Should().BeInAscendingOrder().And.Contain(new[] { @"C:\Used1", @"C:\Used2" });
//             // Verify that build has a folder for unused assembly search paths locations with sorted items.
//             var unusedAssemblyFolder = fakeBuild.GetOrCreateNodeWithName<FakeFolder>(Strings.UnusedAssemblySearchPathsLocations);
//             unusedAssemblyFolder.Children.OfType<FakeItem>().Select(i => i.Text).Should().BeInAscendingOrder().And.Contain(@"D:\Unused");
//         }
//     }
// //  // [Error] (244-27)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'Strings'
// // #region FakeNode Hierarchy and Support Classes
// //     // Minimal implementation of the Strings helper as used in the production code.
// //     internal static class Strings
// //     {
// //         public const string Results = "Results";
// //         public const string Parameters = "Parameters";
// //         public const string SearchPaths = "SearchPaths";
// //         public const string Assemblies = "Assemblies";
// //         public const string UsedLocations = "UsedLocations";
// //         public const string UnusedLocations = "UnusedLocations";
// //         public const string UsedAssemblySearchPathsLocations = "UsedAssemblySearchPathsLocations";
// //         public const string UnusedAssemblySearchPathsLocations = "UnusedAssemblySearchPathsLocations";
// //     }
// // 
//     // Minimal fake implementation for StringCache.
//     internal class FakeStringCache
//     {
//         public string Intern(string message)
//         {
//             return message;
//         }
//     }
// 
//     // Base fake node for constructing the fake tree structure.
//     internal class FakeNode
//     {
//         public string Name { get; set; } = string.Empty;
//         public string Text { get; set; } = string.Empty;
//         public List<FakeNode> Children { get; } = new List<FakeNode>();
//         public FakeNode? Parent { get; set; }
// 
//         public virtual void AddChild(FakeNode child)
//         {
//             child.Parent = this;
//             Children.Add(child);
//         }
// 
//         public T? FindChild<T>(Func<FakeNode, bool> predicate)
//             where T : FakeNode
//         {
//             foreach (var child in Children)
//             {
//                 if (child is T t && predicate(child))
//                 {
//                     return t;
//                 }
//             }
// 
//             return null;
//         }
// 
//         public T GetOrCreateNodeWithName<T>(string name)
//             where T : FakeNode, new()
//         {
//             var existing = Children.FirstOrDefault(n => n.Name == name) as T;
//             if (existing == null)
//             {
//                 existing = new T
//                 {
//                     Name = name
//                 };
//                 AddChild(existing);
//             }
// 
//             return existing;
//         }
// 
//         public T? GetNearestParent<T>()
//             where T : FakeNode
//         {
//             var current = Parent;
//             while (current != null)
//             {
//                 if (current is T t)
//                     return t;
//                 current = current.Parent;
//             }
// 
//             return null;
//         }
// 
//         public override string ToString()
//         {
//             return !string.IsNullOrEmpty(Text) ? Text : Name;
//         }
//     }
// 
//     // Fake implementations for specific node types.
//     internal class FakeTask : FakeNode
//     {
//         public TimeSpan Duration { get; set; }
//     }
// //  // [Error] (335-20)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'FakeBuild'
// //     internal class FakeBuild : FakeNode
// //     {
// //         public FakeStringCache? StringTable { get; set; } // [Error] (337-33)CS0053 Inconsistent accessibility: property type 'FakeStringCache' is less accessible than property 'FakeBuild.StringTable'
// //     }
// // 
//     internal class FakeFolder : FakeNode
//     {
//         // Simulate SortChildren by sorting based on the Text property of child nodes.
//         public void SortChildren()
//         {
//             Children.Sort((a, b) => string.Compare(a.ToString(), b.ToString(), StringComparison.Ordinal));
//         }
//     }
// 
//     internal class FakeNamedNode : FakeNode
//     {
//     }
// 
//     internal class FakeParameter : FakeNode
//     {
//     }
// 
//     internal class FakeItem : FakeNode
//     {
//     }
// 
//     internal class FakeMetadata : FakeNode
//     {
//         public string Value { get; set; } = string.Empty;
//     }
// 
//     internal class FakeMessage : FakeNode
//     {
//     }
// #endregion
// }