// using FluentAssertions;
// using Moq;
// using Microsoft.Build.Logging.StructuredLogger;
// using StructuredLogViewer;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using Xunit;
// 
// namespace StructuredLogViewer.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="PropertiesAndItemsSearch"/> class.
//     /// </summary>
//     public class PropertiesAndItemsSearchTests
//     {
//         private readonly PropertiesAndItemsSearch _searcher;
// 
//         public PropertiesAndItemsSearchTests()
//         {
//             _searcher = new PropertiesAndItemsSearch();
//         }
// 
//         /// <summary>
//         /// Tests the Search method with a context that does not contain any properties or items folders.
//         /// Expects to return an empty result set.
//         /// </summary>
//         [Fact]
//         public void Search_WhenNoPropertiesOrItemsInContext_ReturnsEmptyResults()
//         {
//             // Arrange
//             // TODO: Initialize a valid TimedNode instance without properties, items, and reassignments.
//             // The fake implementation below provides a minimal TimedNode with no children.
//             TimedNode context = CreateEmptyTimedNode();
//             string searchText = "dummy";
//             int maxResults = 10;
//             bool markResultsInTree = false;
//             CancellationToken cancellationToken = CancellationToken.None;
// 
//             // Act
//             IEnumerable<SearchResult> results = _searcher.Search(context, searchText, maxResults, markResultsInTree, cancellationToken);
// 
//             // Assert
//             results.Should().BeEmpty("because the context does not contain any matching properties or items nodes.");
//         }
// 
//         /// <summary>
//         /// Tests the Search method when the cancellation token is cancelled.
//         /// Expects to throw an OperationCanceledException.
//         /// </summary>
//         [Fact]
//         public void Search_WhenCancellationIsRequested_ThrowsOperationCanceledException()
//         {
//             // Arrange
//             // TODO: Initialize a valid TimedNode instance as needed.
//             TimedNode context = CreateEmptyTimedNode();
//             string searchText = "dummy";
//             int maxResults = 10;
//             bool markResultsInTree = false;
//             using CancellationTokenSource cts = new CancellationTokenSource();
//             cts.Cancel();
//             CancellationToken cancellationToken = cts.Token;
// 
//             // Act
//             Action act = () => _searcher.Search(context, searchText, maxResults, markResultsInTree, cancellationToken);
// 
//             // Assert
//             // Depending on how SearchResult and Search.FindNodes handle cancellation,
//             // this expectation might need adjustment.
//             act.Should().Throw<OperationCanceledException>("because the cancellation token was cancelled.");
//         }
// 
//         /// <summary>
//         /// Tests the Search method with a context simulating a Project node with an associated Build and Evaluation.
//         /// Expects to return a combined set of search results that includes execution search results.
//         /// </summary>
//         [Fact]
//         public void Search_WhenContextIsProjectWithEvaluation_ReturnsCombinedResults()
//         {
//             // Arrange
//             // TODO: Create a valid TimedNode instance that simulates a Project node with
//             // an associated Build root and a matching ProjectEvaluation.
//             TimedNode context = CreateProjectTimedNode();
//             string searchText = "test";
//             int maxResults = 20;
//             bool markResultsInTree = true;
//             CancellationToken cancellationToken = CancellationToken.None;
// 
//             // Act
//             IEnumerable<SearchResult> results = _searcher.Search(context, searchText, maxResults, markResultsInTree, cancellationToken);
// 
//             // Assert
//             // Since the actual search logic depends on the internal tree structure,
//             // further assertions should be added based on the expected nodes.
//             results.Should().NotBeNull("because even if no matching nodes, an empty collection is expected.");
//             // Additional assertions based on the properties of the returned SearchResult may be added here.
//         }
// 
//         // Helper methods to create fake TimedNode instances for testing.
//         // Note: These minimal fake implementations are provided solely for testing purposes.
//         // In a real-world scenario, consider using actual instances if available.
// 
//         private TimedNode CreateEmptyTimedNode()
//         {
//             return new FakeTimedNode();
//         }
// 
//         private TimedNode CreateProjectTimedNode()
//         {
//             return new FakeProjectTimedNode();
//         }
// 
//         // Fake implementations for testing.
// 
//         private class FakeTimedNode : TimedNode
//         {
//             private readonly List<BaseNode> _children = new List<BaseNode>();
// //  // [Error] (120-38)CS0506 'PropertiesAndItemsSearchTests.FakeTimedNode.GetRoot()': cannot override inherited member 'BaseNode.GetRoot()' because it is not marked virtual, abstract, or override
// //             public override BaseNode GetRoot()
// //             {
// //                 // For an empty context, return itself.
// //                 return this;
// //             }
// //  // [Error] (126-32)CS0508 'PropertiesAndItemsSearchTests.FakeTimedNode.FindChild<T>(string)': return type must be 'T' to match overridden member 'TreeNode.FindChild<T>(string)' // [Error] (126-32)CS0453 The type 'T' must be a non-nullable value type in order to use it as parameter 'T' in the generic type or method 'Nullable<T>'
//             public override T? FindChild<T>(string name)
//             {
//                 // Always return null to simulate no matching child.
//                 return null;
//             }
// //  // [Error] (132-45)CS0506 'PropertiesAndItemsSearchTests.FakeTimedNode.Children': cannot override inherited member 'TreeNode.Children' because it is not marked virtual, abstract, or override
// //             public override IList<BaseNode> Children => _children;
// //         }
// 
//         private class FakeProjectTimedNode : FakeTimedNode
//         {
//             // Simulated properties for a Project node.
//             public int EvaluationId { get; set; } = 1;
//             public int Index { get; set; } = 100;
//             private readonly List<BaseNode> _children = new List<BaseNode>();
// 
//             public override BaseNode GetRoot()
//             {
//                 // Return a FakeBuild instance to simulate the Build root.
//                 return new FakeBuild(this);
//             }
// 
//             public override IList<BaseNode> Children => _children;
//         }
// 
//         private class FakeBuild : Build
//         {
//             private readonly FakeProjectTimedNode _projectNode;
//             private readonly List<ProjectEvaluation> _evaluations;
//             private readonly StringCache _stringTable;
// 
//             public FakeBuild(FakeProjectTimedNode projectNode)
//             {
//                 _projectNode = projectNode;
//                 _evaluations = new List<ProjectEvaluation>
//                 {
//                     new FakeProjectEvaluation(projectNode.EvaluationId)
//                 };
//                 _stringTable = new FakeStringCache();
//             }
// //  // [Error] (167-38)CS0506 'PropertiesAndItemsSearchTests.FakeBuild.GetRoot()': cannot override inherited member 'BaseNode.GetRoot()' because it is not marked virtual, abstract, or override
// //             public override BaseNode GetRoot()
// //             {
// //                 return this;
// //             }
// // 
//             // Expose a fake FindEvaluation method.
//             public ProjectEvaluation? FindEvaluation(int evaluationId)
//             {
//                 return _evaluations.FirstOrDefault(e => (e as FakeProjectEvaluation)?.EvaluationId == evaluationId);
//             }
// //  // [Error] (178-41)CS0506 'PropertiesAndItemsSearchTests.FakeBuild.StringTable': cannot override inherited member 'Build.StringTable' because it is not marked virtual, abstract, or override
// //             public override StringCache StringTable => _stringTable;
// //         }
// 
//         private class FakeProjectEvaluation : ProjectEvaluation
//         {
//             public FakeProjectEvaluation(int evaluationId)
//             {
//                 EvaluationId = evaluationId;
//             }
// 
//             public int EvaluationId { get; }
// //  // [Error] (190-45)CS0506 'PropertiesAndItemsSearchTests.FakeProjectEvaluation.Children': cannot override inherited member 'TreeNode.Children' because it is not marked virtual, abstract, or override
// //             public override IList<BaseNode> Children => new List<BaseNode>();
// //         }
// 
//         private class FakeStringCache : StringCache
//         {
//             private readonly HashSet<string> _instances = new HashSet<string>();
// //  // [Error] (197-49)CS0506 'PropertiesAndItemsSearchTests.FakeStringCache.Instances': cannot override inherited member 'StringCache.Instances' because it is not marked virtual, abstract, or override
// //             public override IEnumerable<string> Instances => _instances;
// //  // [Error] (199-36)CS0506 'PropertiesAndItemsSearchTests.FakeStringCache.Intern(string)': cannot override inherited member 'StringCache.Intern(string)' because it is not marked virtual, abstract, or override
//             public override string Intern(string value)
//             {
//                 _instances.Add(value);
//                 return value;
//             }
//         }
//     }
// }