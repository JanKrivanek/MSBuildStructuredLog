using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Microsoft.Build.Logging.StructuredLogger.ResolveAssemblyReferenceAnalyzer"/> class.
    /// </summary>
    public class ResolveAssemblyReferenceAnalyzerTests
    {
        private readonly ResolveAssemblyReferenceAnalyzer _analyzer;

        public ResolveAssemblyReferenceAnalyzerTests()
        {
            _analyzer = new ResolveAssemblyReferenceAnalyzer();
        }

        /// <summary>
        /// Tests the AnalyzeResolveAssemblyReference method for a happy path scenario.
        /// This test creates a fake task node with Parameters and Results folders, simulating a resolved reference.
        /// Expected outcome is that the UsedLocations and UnusedLocations are updated correctly,
        /// TotalRARDuration is incremented, and the corresponding UsedLocations node is added to the task.
        /// </summary>
        [Fact]
        public void AnalyzeResolveAssemblyReference_HappyPath_UpdatesLocations()
        {
            // Arrange
            var duration = TimeSpan.FromSeconds(10);
            var fakeTask = new FakeTask
            {
                Duration = duration,
                Name = "RARTask"
            };

            // set Build parent with StringTable
            var build = new FakeBuild
            {
                Name = "Build",
                StringTable = new FakeStringCache()
            };
            fakeTask.Parent = build;

            // Create Parameters folder with SearchPaths NamedNode containing two search paths.
            var parametersFolder = new FakeFolder { Name = FakeStrings.Parameters };
            var searchPathsNode = new FakeNamedNode { Name = FakeStrings.SearchPaths };
            // Two search path items: one that will be used and one that will be unused.
            var searchPathItem1 = new FakeItem { Text = @"C:\SearchPath1" };
            var searchPathItem2 = new FakeItem { Text = @"C:\SearchPath2" };
            searchPathsNode.Children.Add(searchPathItem1);
            searchPathsNode.Children.Add(searchPathItem2);
            parametersFolder.Children.Add(searchPathsNode);

            // Also add a dummy Assemblies parameter for completeness (not used in this test).
            var assembliesParameter = new FakeParameter { Name = FakeStrings.Assemblies };
            parametersFolder.Children.Add(assembliesParameter);

            fakeTask.Children.Add(parametersFolder);

            // Create Results folder with one Parameter child that simulates a reference.
            var resultsFolder = new FakeFolder { Name = FakeStrings.Results };
            var parameterNode = new FakeParameter { Name = "SomeReference" };

            // Add a resolved file path item.
            var resolvedFilePathItem = new FakeItem { Text = "Resolved file path is \"C:\\Resolved.dll\"" };
            parameterNode.Children.Add(resolvedFilePathItem);

            // Add a found at location item that matches one of the search paths.
            var foundAtItem = new FakeItem { Text = "Reference found at search path location \"C:\\SearchPath1\"" };
            parameterNode.Children.Add(foundAtItem);

            resultsFolder.Children.Add(parameterNode);
            fakeTask.Children.Add(resultsFolder);

            // Act
            _analyzer.AnalyzeResolveAssemblyReference(fakeTask);

            // Assert
            // TotalRARDuration should be increased.
            Assert.Equal(duration, _analyzer.TotalRARDuration);

            // The used location should contain "C:\SearchPath1".
            Assert.Contains(@"C:\SearchPath1", _analyzer.UsedLocations);
            // The unused location should contain "C:\SearchPath2".
            Assert.Contains(@"C:\SearchPath2", _analyzer.UnusedLocations);

            // Verify that the UsedLocations node was added to the fake task.
            var usedLocationsFolder = fakeTask.GetOrCreateNodeWithName<FakeFolder>(FakeStrings.UsedLocations);
            Assert.Contains(usedLocationsFolder.Children.OfType<FakeItem>(), item => item.Text == @"C:\SearchPath1");

            // Verify that the UnusedLocations node was added to the fake task.
            var unusedLocationsFolder = fakeTask.GetOrCreateNodeWithName<FakeFolder>(FakeStrings.UnusedLocations);
            Assert.Contains(unusedLocationsFolder.Children.OfType<FakeItem>(), item => item.Text == @"C:\SearchPath2");
        }

        /// <summary>
        /// Tests the AnalyzeResolveAssemblyReference method when the resolved file path equals the search location.
        /// In this case, the location should be skipped and not added to the UsedLocations.
        /// </summary>
        [Fact]
        public void AnalyzeResolveAssemblyReference_ResolvedFileEqualsLocation_SkipsUsedLocation()
        {
            // Arrange
            var fakeTask = new FakeTask
            {
                Duration = TimeSpan.FromSeconds(5),
                Name = "RARTask"
            };
            fakeTask.Parent = new FakeBuild { Name = "Build", StringTable = new FakeStringCache() };

            // Create Parameters folder with SearchPaths.
            var parametersFolder = new FakeFolder { Name = FakeStrings.Parameters };
            var searchPathsNode = new FakeNamedNode { Name = FakeStrings.SearchPaths };
            var searchPathItem = new FakeItem { Text = @"C:\SamePath" };
            searchPathsNode.Children.Add(searchPathItem);
            parametersFolder.Children.Add(searchPathsNode);
            fakeTask.Children.Add(parametersFolder);

            // Create Results folder with Parameter where resolved file path equals the found location.
            var resultsFolder = new FakeFolder { Name = FakeStrings.Results };
            var parameterNode = new FakeParameter { Name = "SomeReference" };
            var resolvedItem = new FakeItem { Text = "Resolved file path is \"C:\\SamePath\"" };
            parameterNode.Children.Add(resolvedItem);
            var foundAtItem = new FakeItem { Text = "Reference found at search path location \"C:\\SamePath\"" };
            parameterNode.Children.Add(foundAtItem);
            resultsFolder.Children.Add(parameterNode);
            fakeTask.Children.Add(resultsFolder);

            // Act
            _analyzer.AnalyzeResolveAssemblyReference(fakeTask);

            // Assert
            // Since the resolved file path equals the search location, it should not be added.
            Assert.Empty(_analyzer.UsedLocations);
            // However, the searchPaths loop will add the unmatched search path to UnusedLocations.
            Assert.Contains(@"C:\SamePath", _analyzer.UnusedLocations);

            // Verify that the UsedLocations node in fakeTask does not contain the search path.
            var usedLocationsFolder = fakeTask.GetOrCreateNodeWithName<FakeFolder>(FakeStrings.UsedLocations);
            Assert.DoesNotContain(usedLocationsFolder.Children.OfType<FakeItem>(), item => item.Text == @"C:\SamePath");

            // Verify that the UnusedLocations node in fakeTask contains the search path.
            var unusedLocationsFolder = fakeTask.GetOrCreateNodeWithName<FakeFolder>(FakeStrings.UnusedLocations);
            Assert.Contains(unusedLocationsFolder.Children.OfType<FakeItem>(), item => item.Text == @"C:\SamePath");
        }

        /// <summary>
        /// Tests the AnalyzeResolveAssemblyReference method for a dependency that is not CopyLocal due to metadata.
        /// This test simulates a dependency parameter with the required children and an Assemblies parameter containing a source item.
        /// Expected outcome is that metadata is transferred to the required-by item and a message is appended.
        /// </summary>
        [Fact]
        public void AnalyzeResolveAssemblyReference_DependencyNotCopyLocal_AddsMetadataAndMessage()
        {
            // Arrange
            var fakeTask = new FakeTask
            {
                Duration = TimeSpan.FromSeconds(7),
                Name = "RARTask"
            };
            fakeTask.Parent = new FakeBuild { Name = "Build", StringTable = new FakeStringCache() };

            // Create Parameters folder with Assemblies and SearchPaths for completeness.
            var parametersFolder = new FakeFolder { Name = FakeStrings.Parameters };

            // Assemblies parameter with a source item "Ref1" having a Metadata "Private" set to "false".
            var assembliesParameter = new FakeParameter { Name = FakeStrings.Assemblies };
            var sourceItem = new FakeItem { Text = "Ref1" };
            var privateMetadata = new FakeMetadata { Name = "Private", Value = "false" };
            sourceItem.Children.Add(privateMetadata);
            assembliesParameter.Children.Add(sourceItem);
            parametersFolder.Children.Add(assembliesParameter);

            // Add a dummy SearchPaths node.
            var searchPathsNode = new FakeNamedNode { Name = FakeStrings.SearchPaths };
            parametersFolder.Children.Add(searchPathsNode);

            fakeTask.Children.Add(parametersFolder);

            // Create Results folder with a dependency parameter.
            var resultsFolder = new FakeFolder { Name = FakeStrings.Results };
            var dependencyParameter = new FakeParameter { Name = "Dependency SomeAssembly" };

            // Add a required by item.
            var requiredByItem = new FakeItem { Text = "Required by \"Ref1\"" };
            dependencyParameter.Children.Add(requiredByItem);

            // Add the not copy local message.
            var notCopyLocalMessage = new FakeItem
            {
                Text = @"This reference is not ""CopyLocal"" because at least one source item had ""Private"" set to ""false"" and no source items had ""Private"" set to ""true""."
            };
            dependencyParameter.Children.Add(notCopyLocalMessage);

            resultsFolder.Children.Add(dependencyParameter);
            fakeTask.Children.Add(resultsFolder);

            // Act
            _analyzer.AnalyzeResolveAssemblyReference(fakeTask);

            // Assert
            // The requiredByItem should have gained a Metadata child with Name "Private" and Value "false".
            var addedMetadata = requiredByItem.Children.OfType<FakeMetadata>().FirstOrDefault();
            Assert.NotNull(addedMetadata);
            Assert.Equal("Private", addedMetadata.Name);
            Assert.Equal("false", addedMetadata.Value);

            // The notCopyLocalMessage item should have a Message child appended.
            var messageChild = notCopyLocalMessage.Children.OfType<FakeMessage>().FirstOrDefault();
            Assert.NotNull(messageChild);
            // Expected message text: "<sourceItem> has Private set to false"
            // sourceItem.ToString() returns its Text.
            Assert.Equal("Ref1 has Private set to false", messageChild.Text);
        }

        /// <summary>
        /// Tests the AppendFinalReport method to ensure that final assembly search paths locations are appended to the build node.
        /// This test sets pre-defined used and unused locations and verifies that they are reported in sorted order.
        /// </summary>
        [Fact]
        public void AppendFinalReport_AppendsLocationsToBuildReport()
        {
            // Arrange
            var build = new FakeBuild { Name = "Build", StringTable = new FakeStringCache() };

            // Setup used and unused locations in the analyzer.
            _analyzer.UsedLocations.Add("C:\\UsedPathB");
            _analyzer.UsedLocations.Add("C:\\UsedPathA"); // to test sorting

            _analyzer.UnusedLocations.Add("C:\\UnusedPath");

            // Act
            _analyzer.AppendFinalReport(build);

            // Assert
            // Verify UsedAssemblySearchPathsLocations node is created and its children are sorted.
            var usedAssemblyNode = build.GetOrCreateNodeWithName<FakeFolder>(FakeStrings.UsedAssemblySearchPathsLocations);
            var usedItems = usedAssemblyNode.Children.OfType<FakeItem>().Select(i => i.Text).ToList();
            Assert.Equal(new List<string> { "C:\\UsedPathA", "C:\\UsedPathB" }, usedItems);

            // Verify UnusedAssemblySearchPathsLocations node is created and its children are sorted.
            var unusedAssemblyNode = build.GetOrCreateNodeWithName<FakeFolder>(FakeStrings.UnusedAssemblySearchPathsLocations);
            var unusedItems = unusedAssemblyNode.Children.OfType<FakeItem>().Select(i => i.Text).ToList();
            Assert.Equal(new List<string> { "C:\\UnusedPath" }, unusedItems);
        }
    }

    #region Fake Classes for Testing

    // Base fake node simulating a tree structure.
    public class FakeNode
    {
        public string Name { get; set; }
        public FakeNode Parent { get; set; }
        public List<FakeNode> Children { get; } = new List<FakeNode>();

        /// <summary>
        /// Finds the first child of a specific type matching the provided predicate.
        /// </summary>
        public T FindChild<T>(Func<T, bool> predicate) where T : FakeNode
        {
            return Children.OfType<T>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Gets or creates a child node with the specified name.
        /// </summary>
        public T GetOrCreateNodeWithName<T>(string name) where T : FakeNode, new()
        {
            var node = Children.OfType<T>().FirstOrDefault(n => n.Name == name);
            if (node == null)
            {
                node = new T { Name = name };
                AddChild(node);
            }
            return node;
        }

        /// <summary>
        /// Adds a child node and sets its parent.
        /// </summary>
        public virtual void AddChild(FakeNode child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public override string ToString()
        {
            if (this is FakeItem fi && fi.Text != null)
            {
                return fi.Text;
            }
            return Name;
        }
    }

    public class FakeTask : FakeNode
    {
        public TimeSpan Duration { get; set; }
    }

    public class FakeBuild : FakeNode
    {
        public FakeStringCache StringTable { get; set; }
    }

    public class FakeFolder : FakeNode
    {
        public void SortChildren()
        {
            // Dummy implementation for sort; in real use, sorting would be applied.
            Children.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        }
    }

    public class FakeNamedNode : FakeNode
    {
    }

    public class FakeParameter : FakeNode
    {
    }

    public class FakeItem : FakeNode
    {
        public string Text { get; set; }
    }

    public class FakeMetadata : FakeNode
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class FakeMessage : FakeNode
    {
        public string Text { get; set; }
    }

    public class FakeStringCache
    {
        public string Intern(string s)
        {
            return s;
        }
    }

    public static class FakeStrings
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

    #endregion
}
