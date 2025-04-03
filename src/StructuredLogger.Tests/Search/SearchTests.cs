// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using FluentAssertions;
// using Moq;
// using StructuredLogViewer;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace StructuredLogViewer.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Search"/> class.
//     /// </summary>
//     public class SearchTests
//     {
//         private const int DefaultMaxResults = Search.DefaultMaxResults;
// 
//         /// <summary>
//         /// Tests the FindNodes method when no root nodes are provided.
//         /// Expects an empty set of search results.
//         /// </summary>
//         [Fact]
//         public void FindNodes_WhenNoRootsProvided_ReturnsEmptySearchResults()
//         {
//             // Arrange
//             IEnumerable<TreeNode> roots = Enumerable.Empty<TreeNode>();
//             IEnumerable<string> strings = new List<string> { "sample" };
//             int maxResults = DefaultMaxResults;
//             bool markResultsInTree = false;
//             var search = new Search(roots, strings, maxResults, markResultsInTree);
//             var cancellationToken = CancellationToken.None;
//             string query = "test";
// 
//             // Act
//             var results = search.FindNodes(query, cancellationToken);
// 
//             // Assert
//             results.Should().BeEmpty("because there are no root nodes to search");
//         }
// 
//         /// <summary>
//         /// Tests the FindNodes method when the cancellation token is already signaled.
//         /// Expects an empty set of search results due to early cancellation.
//         /// </summary>
//         [Fact]
//         public void FindNodes_WhenCancellationTokenIsCanceled_ReturnsEmptySearchResults()
//         {
//             // Arrange
//             // Creating a dummy TreeNode instance.
//             var dummyTreeNode = new DummyTreeNode();
//             IEnumerable<TreeNode> roots = new List<TreeNode> { dummyTreeNode };
//             IEnumerable<string> strings = new List<string> { "sample" };
//             int maxResults = DefaultMaxResults;
//             bool markResultsInTree = false;
//             var search = new Search(roots, strings, maxResults, markResultsInTree);
// 
//             using CancellationTokenSource cts = new CancellationTokenSource();
//             cts.Cancel();
//             string query = "test";
// 
//             // Act
//             var results = search.FindNodes(query, cts.Token);
// 
//             // Assert
//             results.Should().BeEmpty("because the cancellation token was cancelled before processing started");
//         }
// 
//         /// <summary>
//         /// Partial test for the FindNodes method when a Build node with search extensions provides results.
//         /// TODO: Create a concrete Build instance with a proper SearchExtensions collection
//         /// and a mock ISearchExtension that returns true when TryGetResults is invoked.
//         /// </summary>
//         [Fact(Skip = "Manual implementation needed to setup Build with SearchExtensions.")]
//         public void FindNodes_WhenSearchExtensionReturnsResults_ReturnsEarlyWithProvidedResults()
//         {
//             // Arrange
//             // TODO: Create a Build instance (or a derived test double) with a SearchExtensions collection.
//             // Use Moq to setup a mock ISearchExtension that returns true when TryGetResults is invoked,
//             // and provides a predefined list of SearchResult.
//             throw new NotImplementedException("This test requires a concrete Build instance with a configured SearchExtensions collection.");
//         }
// 
//         /// <summary>
//         /// Partial test for the ClearSearchResults method when markResultsInTree is false.
//         /// Expects that no visit to nodes occurs.
//         /// TODO: Provide a concrete Build instance or a test double to validate that VisitAllChildren is not invoked.
//         /// </summary>
//         [Fact(Skip = "Manual implementation needed to create a testable Build instance for ClearSearchResults.")]
//         public void ClearSearchResults_WhenMarkResultsInTreeFalse_DoesNotVisitNodes()
//         {
//             // Arrange
//             // TODO: Create a Build instance and configure it so that if VisitAllChildren is called, the test will fail.
//             Build build = null; // Replace with an actual Build instance when available.
//             bool markResultsInTree = false;
// 
//             // Act
//             Search.ClearSearchResults(build, markResultsInTree);
// 
//             // Assert
//             // TODO: Verify that VisitAllChildren was not called.
//             throw new NotImplementedException("This test requires a concrete Build instance with behavior verification for VisitAllChildren.");
//         }
// 
//         /// <summary>
//         /// Partial test for the ClearSearchResults method when markResultsInTree is true.
//         /// Expects that VisitAllChildren is invoked to reset search result statuses on all nodes.
//         /// TODO: Provide a concrete Build instance or a test double to validate the invocation.
//         /// </summary>
//         [Fact(Skip = "Manual implementation needed to create a testable Build instance for ClearSearchResults with markResultsInTree true.")]
//         public void ClearSearchResults_WhenMarkResultsInTreeTrue_InvokesResetOnNodes()
//         {
//             // Arrange
//             // TODO: Create a Build instance with identifiable child nodes that record calls to ResetSearchResultStatus.
//             Build build = null; // Replace with an actual Build instance when available.
//             bool markResultsInTree = true;
// 
//             // Act
//             Search.ClearSearchResults(build, markResultsInTree);
// 
//             // Assert
//             // TODO: Assert that for every node in build, ResetSearchResultStatus was called.
//             throw new NotImplementedException("This test requires a concrete Build instance with verifiable behavior for VisitAllChildren.");
//         }
//     }
// 
//     /// <summary>
//     /// A minimal dummy implementation of TreeNode for testing purposes.
//     /// Since TreeNode from StructuredLogger may not provide a public constructor,
//     /// this dummy class is created solely for testing the cancellation scenario in Search.FindNodes.
//     /// </summary>
//     internal class DummyTreeNode : TreeNode
//     {
//         // Since we cannot determine the internal implementation details of TreeNode,
//         // we assume a parameterless constructor is available for testing.
//         public DummyTreeNode()
//         {
//         }
// //  // [Error] (142-30)CS0506 'DummyTreeNode.HasChildren': cannot override inherited member 'TreeNode.HasChildren' because it is not marked virtual, abstract, or override
// //         // Override HasChildren to return false to prevent traversal.
// //         public override bool HasChildren => false;
// //  // [Error] (145-41)CS0506 'DummyTreeNode.Children': cannot override inherited member 'TreeNode.Children' because it is not marked virtual, abstract, or override
//         // Provide a minimal implementation for the Children property.
//         public override IList<BaseNode> Children => new List<BaseNode>();
// //  // [Error] (148-30)CS0506 'DummyTreeNode.ResetSearchResultStatus()': cannot override inherited member 'BaseNode.ResetSearchResultStatus()' because it is not marked virtual, abstract, or override
// //         // Override ResetSearchResultStatus to allow invocation without side effects.
// //         public override void ResetSearchResultStatus()
// //         {
// //             // No implementation needed for dummy.
// //         }
// //     }
// }