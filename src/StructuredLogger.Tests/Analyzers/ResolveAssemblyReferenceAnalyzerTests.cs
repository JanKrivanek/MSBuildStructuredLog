using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Fake base node class to simulate the tree structure required by ResolveAssemblyReferenceAnalyzer.
    /// </summary>
    internal class FakeNode
    {
        public string Name { get; set; }
        public List<FakeNode> Children { get; } = new List<FakeNode>();
        public FakeNode Parent { get; set; }

        public FakeNode(string name = null)
        {
            Name = name;
        }

        public virtual string ToStringOverride()
        {
            return Name;
        }

//         public override string ToString() [Error] (32-69)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text' [Error] (34-29)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text'
//         {
//             // If the node has a Text property (as in FakeItem), use that.
//             if (this is FakeItem item && !string.IsNullOrEmpty(item.Text))
//             {
//                 return item.Text;
//             }
//             return Name;
//         }
    }

    /// <summary>
    /// Extension methods for FakeNode to simulate the tree lookup functions.
    /// </summary>
    internal static class FakeNodeExtensions
    {
        /// <summary>
        /// Simulates the GetNearestParent method.
        /// </summary>
        public static T GetNearestParent<T>(this FakeNode node) where T : FakeNode
        {
            var parent = node.Parent;
            while (parent != null)
            {
                if (parent is T t)
                {
                    return t;
                }
                parent = parent.Parent;
            }
            return null;
        }

        /// <summary>
        /// Simulates the FindChild method for immediate children.
        /// </summary>
        public static T FindChild<T>(this FakeNode node, Func<T, bool> predicate) where T : FakeNode
        {
            foreach (var child in node.Children.OfType<T>())
            {
                if (predicate(child))
                {
                    return child;
                }
            }
            return null;
        }

        /// <summary>
        /// Simulates the GetOrCreateNodeWithName method.
        /// </summary>
        public static T GetOrCreateNodeWithName<T>(this FakeNode node, string name) where T : FakeNode, new()
        {
            var found = node.Children.FirstOrDefault(n => n.Name == name) as T;
            if (found == null)
            {
                found = new T() { Name = name, Parent = node };
                node.Children.Add(found);
            }
            return found;
        }
    }

    /// <summary>
    /// Fake implementation of Task node.
    /// </summary>
//     internal class FakeTask : FakeNode [Error] (95-20)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'FakeTask'
//     {
//         public TimeSpan Duration { get; set; }
// 
//         public FakeTask(string name = "Task") : base(name)
//         {
//         }
//     }

    /// <summary>
    /// Fake implementation of Build node.
    /// </summary>
//     internal class FakeBuild : FakeNode [Error] (107-20)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'FakeBuild'
//     {
//         public string StringTable { get; set; }
// 
//         public FakeBuild(string name = "Build") : base(name)
//         {
//         }
//     }

    /// <summary>
    /// Fake implementation of Folder node.
    /// </summary>
//     internal class FakeFolder : FakeNode [Error] (119-20)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'FakeFolder'
//     {
//         public FakeFolder(string name) : base(name)
//         {
//         }
// 
//         public void SortChildren()
//         {
//             Children.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
//         }
//     }

    /// <summary>
    /// Fake implementation of Parameter node.
    /// </summary>
    internal class FakeParameter : FakeNode
    {
        public FakeParameter(string name) : base(name)
        {
        }
    }

    /// <summary>
    /// Fake implementation of NamedNode.
    /// </summary>
    internal class FakeNamedNode : FakeNode
    {
        public FakeNamedNode(string name) : base(name)
        {
        }
    }

    /// <summary>
    /// Fake implementation of Item.
    /// </summary>
//     internal class FakeItem : FakeNode [Error] (154-20)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'FakeItem' [Error] (159-13)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text'
//     {
//         public string Text { get; set; }
//         public FakeItem(string text)
//         {
//             Text = text;
//             Name = text;
//         }
//         public override string ToString() [Error] (164-20)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text'
//         {
//             return Text;
//         }
//     }

    /// <summary>
    /// Fake implementation of Metadata.
    /// </summary>
    internal class FakeMetadata : FakeNode
    {
        public string Value { get; set; }
        public FakeMetadata(string name, string value)
            : base(name)
        {
            Value = value;
        }
        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }

    /// <summary>
    /// Fake implementation of Message.
    /// </summary>
//     internal class FakeMessage : FakeNode [Error] (193-13)CS0229 Ambiguity between 'FakeMessage.Text' and 'FakeMessage.Text'
//     {
//         public string Text { get; set; }
//         public FakeMessage(string text)
//         {
//             Text = text;
//             Name = "Message";
//         }
//         public override string ToString() [Error] (198-20)CS0229 Ambiguity between 'FakeMessage.Text' and 'FakeMessage.Text'
//         {
//             return Text;
//         }
//     }

    /// <summary>
    /// Fake implementation to simulate Strings constants.
    /// </summary>
    internal static class FakeStrings
    {
        public const string Results = "Results";
        public const string Parameters = "Parameters";
        public const string SearchPaths = "SearchPaths";
        public const string Assemblies = "Assemblies";
        public const string UsedLocations = "UsedLocations";
        public const string UnusedLocations = "UnusedLocations";
        public const string UsedAssemblySearchPathsLocations = "UsedAssemblySearchPathsLocations";
        public const string UnusedAssemblySearchPathsLocations = "UnusedAssemblySearchPathsLocations";
    }

    /// <summary>
    /// Derived analyzer that uses the fake classes instead of the actual nodes.
    /// This subclass is used solely for testing purposes to override creation of nodes.
    /// </summary>
    internal class TestableResolveAssemblyReferenceAnalyzer : ResolveAssemblyReferenceAnalyzer
    {
        // Expose internal state if needed
    }

    /// <summary>
    /// Unit tests for the <see cref="ResolveAssemblyReferenceAnalyzer"/> class.
    /// </summary>
    public class ResolveAssemblyReferenceAnalyzerTests
    {
        private readonly TestableResolveAssemblyReferenceAnalyzer _analyzer;

        public ResolveAssemblyReferenceAnalyzerTests()
        {
            _analyzer = new TestableResolveAssemblyReferenceAnalyzer();
        }

        /// <summary>
        /// Tests AnalyzeResolveAssemblyReference when both Results and Parameters folders are present.
        /// Validates that UsedLocations and UnusedLocations are updated correctly and duration is accumulated.
        /// </summary>
//         [Fact] [Error] (246-24)CS0144 Cannot create an instance of the abstract type or interface 'FakeTask' [Error] (263-43)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeNamedNode' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (278-40)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeParameter' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (281-55)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeTask' to 'Microsoft.Build.Logging.StructuredLogger.Task' [Error] (293-44)CS0310 'FakeFolder' must be a non-abstract type with a public parameterless constructor in order to use it as parameter 'T' in the generic type or method 'FakeNodeExtensions.GetOrCreateNodeWithName<T>(FakeNode, string)' [Error] (294-100)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text' [Error] (298-46)CS0310 'FakeFolder' must be a non-abstract type with a public parameterless constructor in order to use it as parameter 'T' in the generic type or method 'FakeNodeExtensions.GetOrCreateNodeWithName<T>(FakeNode, string)' [Error] (299-104)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text'
//         public void AnalyzeResolveAssemblyReference_ResultsAndParametersPresent_UpdatesAnalyzerAndNodes()
//         {
//             // Arrange
//             var task = new FakeTask();
//             task.Duration = TimeSpan.FromSeconds(1);
// 
//             // Create a fake Build parent with a simple string table.
//             var build = new FakeBuild { StringTable = "Interned" };
//             task.Parent = build;
// 
//             // Create Parameters folder with SearchPaths.
//             var parametersFolder = new FakeFolder(FakeStrings.Parameters) { Parent = task };
//             task.Children.Add(parametersFolder);
// 
//             var searchPathsNode = new FakeNamedNode(FakeStrings.SearchPaths) { Parent = parametersFolder };
//             // Add two search paths: one that will be used and one that will not.
//             var usedSearchPath = new FakeItem("C:\\MyResolution2");
//             var unusedSearchPath = new FakeItem("C:\\OtherPath");
//             searchPathsNode.Children.Add(usedSearchPath);
//             searchPathsNode.Children.Add(unusedSearchPath);
//             parametersFolder.Children.Add(searchPathsNode);
// 
//             // Create Results folder with one Parameter (reference) node.
//             var resultsFolder = new FakeFolder(FakeStrings.Results) { Parent = task };
//             task.Children.Add(resultsFolder);
// 
//             var referenceParameter = new FakeParameter("SomeReference") { Parent = resultsFolder };
//             // Add child: Resolved file path node.
//             var resolvedFilePathText = "Resolved file path is \"C:\\MyResolution\"";
//             var resolvedFilePathItem = new FakeItem(resolvedFilePathText);
//             referenceParameter.Children.Add(resolvedFilePathItem);
//             // Add child: Found at location node.
//             var foundAtText = "Reference found at search path location \"C:\\MyResolution2\"";
//             var foundAtItem = new FakeItem(foundAtText);
//             referenceParameter.Children.Add(foundAtItem);
//             resultsFolder.Children.Add(referenceParameter);
// 
//             // Act
//             _analyzer.AnalyzeResolveAssemblyReference(task);
// 
//             // Assert
//             // TotalRARDuration should be updated.
//             Assert.Equal(TimeSpan.FromSeconds(1), _analyzer.TotalRARDuration);
// 
//             // UsedLocations should contain the found location.
//             Assert.Contains("C:\\MyResolution2", _analyzer.UsedLocations);
//             // UnusedLocations should contain the unused search path.
//             Assert.Contains("C:\\OtherPath", _analyzer.UnusedLocations);
// 
//             // Verify that the task now has a UsedLocations folder with the used search path item.
//             var usedLocationsFolder = task.GetOrCreateNodeWithName<FakeFolder>(FakeStrings.UsedLocations);
//             var usedLocationsTexts = usedLocationsFolder.Children.OfType<FakeItem>().Select(i => i.Text);
//             Assert.Contains("C:\\MyResolution2", usedLocationsTexts);
// 
//             // Verify that the task now has an UnusedLocations folder with the unused search path item.
//             var unusedLocationsFolder = task.GetOrCreateNodeWithName<FakeFolder>(FakeStrings.UnusedLocations);
//             var unusedLocationsTexts = unusedLocationsFolder.Children.OfType<FakeItem>().Select(i => i.Text);
//             Assert.Contains("C:\\OtherPath", unusedLocationsTexts);
//         }

        /// <summary>
        /// Tests AnalyzeResolveAssemblyReference when Parameters folder is missing.
        /// Validates that only duration is updated and no search path processing occurs.
        /// </summary>
//         [Fact] [Error] (311-24)CS0144 Cannot create an instance of the abstract type or interface 'FakeTask' [Error] (323-55)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeTask' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         public void AnalyzeResolveAssemblyReference_NoParametersFolder_OnlyUpdatesDuration()
//         {
//             // Arrange
//             var task = new FakeTask();
//             task.Duration = TimeSpan.FromSeconds(2);
// 
//             // Create Build parent.
//             var build = new FakeBuild { StringTable = "Interned" };
//             task.Parent = build;
// 
//             // Only add Results folder.
//             var resultsFolder = new FakeFolder(FakeStrings.Results) { Parent = task };
//             task.Children.Add(resultsFolder);
// 
//             // Act
//             _analyzer.AnalyzeResolveAssemblyReference(task);
// 
//             // Assert
//             Assert.Equal(TimeSpan.FromSeconds(2), _analyzer.TotalRARDuration);
//             // Since no Parameters folder, searchPaths processing is skipped.
//             Assert.Empty(_analyzer.UsedLocations);
//             Assert.Empty(_analyzer.UnusedLocations);
//         }

        /// <summary>
        /// Tests AnalyzeResolveAssemblyReference dependency branch where the reference starts with "Dependency ".
        /// Validates that metadata from source items are propagated and a message is added.
        /// </summary>
//         [Fact] [Error] (340-24)CS0144 Cannot create an instance of the abstract type or interface 'FakeTask' [Error] (353-43)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeParameter' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (357-37)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeMetadata' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (374-40)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeParameter' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (377-55)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeTask' to 'Microsoft.Build.Logging.StructuredLogger.Task' [Error] (382-75)CS8121 An expression of type 'FakeItem' cannot be handled by a pattern of type 'FakeMetadata'. [Error] (386-75)CS8121 An expression of type 'FakeItem' cannot be handled by a pattern of type 'FakeMessage'. [Error] (386-98)CS0229 Ambiguity between 'FakeMessage.Text' and 'FakeMessage.Text'
//         public void AnalyzeResolveAssemblyReference_DependencyBranch_ProcessesMetadata()
//         {
//             // Arrange
//             var task = new FakeTask();
//             task.Duration = TimeSpan.FromSeconds(1);
// 
//             // Set Build parent with StringTable.
//             var build = new FakeBuild { StringTable = "Interned" };
//             task.Parent = build;
// 
//             // Create Parameters folder.
//             var parametersFolder = new FakeFolder(FakeStrings.Parameters) { Parent = task };
//             task.Children.Add(parametersFolder);
// 
//             // Create Assemblies parameter under Parameters.
//             var assembliesParameter = new FakeParameter(FakeStrings.Assemblies) { Parent = parametersFolder };
//             parametersFolder.Children.Add(assembliesParameter);
//             // Create a source item with text "SourceItem" and add a Metadata child.
//             var sourceItem = new FakeItem("SourceItem") { Parent = assembliesParameter };
//             var metadata = new FakeMetadata("Private", "false");
//             sourceItem.Children.Add(metadata);
//             assembliesParameter.Children.Add(sourceItem);
// 
//             // Create Results folder.
//             var resultsFolder = new FakeFolder(FakeStrings.Results) { Parent = task };
//             task.Children.Add(resultsFolder);
// 
//             // Create a dependency reference parameter.
//             var dependencyParameter = new FakeParameter("Dependency SomeDependency") { Parent = resultsFolder };
//             // Add child: Required by message.
//             var requiredByText = "Required by \"SourceItem\"";
//             var requiredByItem = new FakeItem(requiredByText) { Parent = dependencyParameter };
//             dependencyParameter.Children.Add(requiredByItem);
//             // Add child: not copy local message.
//             var notCopyLocalText = @"This reference is not ""CopyLocal"" because at least one source item had ""Private"" set to ""false"" and no source items had ""Private"" set to ""true"".";
//             var notCopyLocalItem = new FakeItem(notCopyLocalText) { Parent = dependencyParameter };
//             dependencyParameter.Children.Add(notCopyLocalItem);
//             resultsFolder.Children.Add(dependencyParameter);
// 
//             // Act
//             _analyzer.AnalyzeResolveAssemblyReference(task);
// 
//             // Assert
//             // The requiredBy item should have a Metadata child added.
//             Assert.Contains(dependencyParameter.Children, n =>
//                 n is FakeItem item && item.Children.Any(child => child is FakeMetadata meta && meta.Name == "Private"));
// 
//             // And the not copy local item should have a Message child added.
//             Assert.Contains(dependencyParameter.Children, n =>
//                 n is FakeItem item && item.Children.Any(child => child is FakeMessage msg && msg.Text.Contains("SourceItem has Private set to false")));
//         }

        /// <summary>
        /// Tests AppendFinalReport when UsedLocations and UnusedLocations have entries.
        /// Validates that the Build node is populated with final report folders containing sorted locations.
        /// </summary>
//         [Fact] [Error] (407-41)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build' [Error] (410-42)CS0310 'FakeFolder' must be a non-abstract type with a public parameterless constructor in order to use it as parameter 'T' in the generic type or method 'FakeBuild.GetOrCreateNodeWithName<T>(string)' [Error] (411-44)CS0310 'FakeFolder' must be a non-abstract type with a public parameterless constructor in order to use it as parameter 'T' in the generic type or method 'FakeBuild.GetOrCreateNodeWithName<T>(string)' [Error] (413-92)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text' [Error] (414-96)CS0229 Ambiguity between 'FakeItem.Text' and 'FakeItem.Text'
//         public void AppendFinalReport_WithLocations_CreatesReportNodes()
//         {
//             // Arrange
//             var build = new FakeBuild();
//             // Manually add some locations to the analyzer's sets.
//             _analyzer.UsedLocations.Clear();
//             _analyzer.UnusedLocations.Clear();
//             _analyzer.UsedLocations.Add("C:\\BPath");
//             _analyzer.UsedLocations.Add("C:\\APath");
//             _analyzer.UnusedLocations.Add("C:\\DPath");
//             _analyzer.UnusedLocations.Add("C:\\CPath");
// 
//             // Act
//             _analyzer.AppendFinalReport(build);
// 
//             // Assert
//             var usedReportFolder = build.GetOrCreateNodeWithName<FakeFolder>(FakeStrings.UsedAssemblySearchPathsLocations);
//             var unusedReportFolder = build.GetOrCreateNodeWithName<FakeFolder>(FakeStrings.UnusedAssemblySearchPathsLocations);
// 
//             var usedLocations = usedReportFolder.Children.OfType<FakeItem>().Select(i => i.Text).ToList();
//             var unusedLocations = unusedReportFolder.Children.OfType<FakeItem>().Select(i => i.Text).ToList();
// 
//             // Locations should be sorted.
//             Assert.Equal(new List<string> { "C:\\APath", "C:\\BPath" }, usedLocations);
//             Assert.Equal(new List<string> { "C:\\CPath", "C:\\DPath" }, unusedLocations);
//         }

        /// <summary>
        /// Tests AppendFinalReport when there are no entries in UsedLocations and UnusedLocations.
        /// Validates that no report nodes are added to the Build node.
        /// </summary>
//         [Fact] [Error] (434-41)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         public void AppendFinalReport_EmptyLocations_DoesNothing()
//         {
//             // Arrange
//             var build = new FakeBuild();
//             _analyzer.UsedLocations.Clear();
//             _analyzer.UnusedLocations.Clear();
// 
//             // Act
//             _analyzer.AppendFinalReport(build);
// 
//             // Assert
//             // Expect that the report nodes are not created as there are no locations.
//             var usedReportFolder = build.Children.FirstOrDefault(n => n.Name == FakeStrings.UsedAssemblySearchPathsLocations);
//             var unusedReportFolder = build.Children.FirstOrDefault(n => n.Name == FakeStrings.UnusedAssemblySearchPathsLocations);
//             Assert.Null(usedReportFolder);
//             Assert.Null(unusedReportFolder);
//         }
    }
}
