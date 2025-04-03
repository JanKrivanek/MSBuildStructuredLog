// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Microsoft.Build.Logging.StructuredLogger.UnitTests;
// using Moq;
// using StructuredLogViewer;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// 
// namespace StructuredLogViewer.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "ResultTree"/> class.
//     /// </summary>
//     public class ResultTreeTests
//     {
// //         /// <summary> // [Error] (28-13)CS0104 'Folder' is an ambiguous reference between 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' and 'Microsoft.Build.Logging.StructuredLogger.Folder'
// //         /// Tests BuildResultTree when the resultsObject is not a valid collection.
// //         /// Expects an empty Folder with no children.
// //         /// </summary>
// //         [Fact]
// //         public void BuildResultTree_InvalidResultsObject_ReturnsEmptyFolder()
// //         {
// //             // Arrange
// //             object invalidResults = 123; // not a valid ICollection<SearchResult>
// //             // Act
// //             Folder result = ResultTree.BuildResultTree(invalidResults);
// //             // Assert
// //             result.Children.Should().BeEmpty("because the provided results object is not a collection of SearchResult");
// //         }
// //  // [Error] (45-13)CS0104 'Folder' is an ambiguous reference between 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' and 'Microsoft.Build.Logging.StructuredLogger.Folder'
//         /// <summary>
//         /// Tests BuildResultTree with an empty results collection and addDuration enabled.
//         /// Expects a Folder with a Note child reflecting the status message.
//         /// </summary>
//         [Fact]
//         public void BuildResultTree_EmptyResults_AddDurationEnabled_ReturnsFolderWithNote()
//         {
//             // Arrange
//             var emptyResults = new List<SearchResult>();
//             TimeSpan elapsed = TimeSpan.Zero;
//             TimeSpan precalculationDuration = TimeSpan.Zero;
//             // Act
//             Folder result = ResultTree.BuildResultTree(emptyResults, elapsed, precalculationDuration, addDuration: true, addWhenNoResults: null);
//             // Assert
//             result.Children.Should().HaveCount(1, "because addDuration adds a Note even if the results collection is empty");
//             var note = result.Children[0] as Note;
//             note.Should().NotBeNull("because the added node should be a Note instance when addDuration is true");
//             note.Text.Should().Contain("0 result", "because the status should reflect zero results");
//         }
// //  // [Error] (64-18)CS0104 'BaseNode' is an ambiguous reference between 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' and 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (69-13)CS0104 'Folder' is an ambiguous reference between 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' and 'Microsoft.Build.Logging.StructuredLogger.Folder' // [Error] (69-141)CS1503 Argument 5: cannot convert from 'System.Func<Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode>' to 'System.Func<Microsoft.Build.Logging.StructuredLogger.BaseNode>'
// //         /// <summary>
// //         /// Tests BuildResultTree with an empty results collection, addDuration disabled and a custom node provided via addWhenNoResults.
// //         /// Expects a Folder with the custom node added.
// //         /// </summary>
// //         [Fact]
// //         public void BuildResultTree_EmptyResults_AddDurationDisabledWithAddWhenNoResults_ReturnsFolderWithCustomNode()
// //         {
// //             // Arrange
// //             var emptyResults = new List<SearchResult>();
// //             TimeSpan elapsed = TimeSpan.Zero;
// //             TimeSpan precalculationDuration = TimeSpan.Zero;
// //             Func<BaseNode> addWhenNoResults = () => new Note
// //             {
// //                 Text = "No Results Available"
// //             };
// //             // Act
// //             Folder result = ResultTree.BuildResultTree(emptyResults, elapsed, precalculationDuration, addDuration: false, addWhenNoResults: addWhenNoResults);
// //             // Assert
// //             result.Children.Should().HaveCount(1, "because addWhenNoResults is used to add a node when no children exist");
// //             var customNode = result.Children[0];
// //             customNode.Text.Should().Be("No Results Available", "because the custom node should carry the provided text");
// //         }
// //  // [Error] (90-17)CS0200 Property or indexer 'SearchResult.Node' cannot be assigned to -- it is read only // [Error] (99-13)CS0104 'Folder' is an ambiguous reference between 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' and 'Microsoft.Build.Logging.StructuredLogger.Folder'
//         /// <summary>
//         /// Tests BuildResultTree with a results collection containing one SearchResult with a non-zero Duration.
//         /// Expects the Folder to include a Note for status, a Message for total duration, and a ProxyNode wrapping the result.
//         /// </summary>
//         [Fact]
//         public void BuildResultTree_ResultsWithDuration_ReturnsFolderWithMessageAndProxyNode()
//         {
//             // Arrange
//             // Create a dummy SearchResult with non-zero Duration and a Node having a non-null Parent.
//             var searchResult = new SearchResult
//             {
//                 Duration = TimeSpan.FromSeconds(10),
//                 StartTime = DateTime.MinValue,
//                 EndTime = DateTime.MinValue,
//                 Node = CreateDummyNodeWithParent("DummyNode")
//             };
//             var results = new List<SearchResult>
//             {
//                 searchResult
//             };
//             TimeSpan elapsed = TimeSpan.FromSeconds(5);
//             TimeSpan precalculationDuration = TimeSpan.Zero;
//             // Act
//             Folder result = ResultTree.BuildResultTree(results, elapsed, precalculationDuration, addDuration: true, addWhenNoResults: null);
//             // Assert
//             // Expect a Note (from addDuration), a Message (for total duration) and a node for the search result.
//             result.Children.Should().HaveCount(3, "because includeDuration branch adds both a Message and a ProxyNode in addition to the status Note");
//             // Validate the Note (first child)
//             var note = result.Children[0] as Note;
//             note.Should().NotBeNull("because the first child should be a Note");
//             note.Text.Should().Contain("1 result", "because the status should show one result");
//             // Validate the Message (second child)
//             var message = result.Children[1] as Message;
//             message.Should().NotBeNull("because a Message should be added when any result has a non-zero Duration");
//             message.Text.Should().StartWith("Total duration:", "because the Message should display the total duration");
//             // Validate the ProxyNode (third child)
//             var proxy = result.Children[2];
//             proxy.Should().BeOfType<ProxyNode>("because the search result should be wrapped in a ProxyNode");
//             var proxyNode = proxy as ProxyNode;
//             proxyNode.SearchResult.Should().Be(searchResult, "because the ProxyNode is expected to reference the original SearchResult");
//         }
// //  // [Error] (131-17)CS0200 Property or indexer 'SearchResult.Node' cannot be assigned to -- it is read only // [Error] (140-13)CS0104 'Folder' is an ambiguous reference between 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' and 'Microsoft.Build.Logging.StructuredLogger.Folder'
// //         /// <summary>
// //         /// Tests BuildResultTree with a results collection containing one SearchResult with a null Node.
// //         /// Expects the Folder to include a Note and a newly created ProxyNode wrapping the result.
// //         /// </summary>
// //         [Fact]
// //         public void BuildResultTree_ResultWithNullNode_ReturnsFolderWithProxyNode()
// //         {
// //             // Arrange
// //             var searchResult = new SearchResult
// //             {
// //                 Duration = TimeSpan.Zero,
// //                 StartTime = DateTime.MinValue,
// //                 EndTime = DateTime.MinValue,
// //                 Node = null
// //             };
// //             var results = new List<SearchResult>
// //             {
// //                 searchResult
// //             };
// //             TimeSpan elapsed = TimeSpan.Zero;
// //             TimeSpan precalculationDuration = TimeSpan.Zero;
// //             // Act
// //             Folder result = ResultTree.BuildResultTree(results, elapsed, precalculationDuration, addDuration: true, addWhenNoResults: null);
// //             // Assert
// //             // Expect a Note from addDuration and a ProxyNode wrapping the search result.
// //             result.Children.Should().HaveCount(2, "because a status Note is added then the result is processed into a ProxyNode");
// //             var proxy = result.Children[1];
// //             proxy.Should().BeOfType<ProxyNode>("because when SearchResult.Node is null, a new ProxyNode is created");
// //             var proxyNode = proxy as ProxyNode;
// //             proxyNode.SearchResult.Should().Be(searchResult, "because the ProxyNode should reference the provided SearchResult");
// //         }
// //  // [Error] (156-17)CS0104 'BaseNode' is an ambiguous reference between 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' and 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (159-36)CS0104 'Folder' is an ambiguous reference between 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' and 'Microsoft.Build.Logging.StructuredLogger.Folder'
// //         /// <summary>
// //         /// Helper method to create a dummy BaseNode with a non-null Parent.
// //         /// It creates a parent Folder and uses GetOrCreateNodeWithText to simulate a child node with Parent set.
// //         /// </summary>
// //         /// <param name = "text">The text to assign to the node.</param>
// //         /// <returns>A BaseNode with a non-null Parent.</returns>
// //         private BaseNode CreateDummyNodeWithParent(string text)
// //         {
// //             // Create a parent Folder.
// //             var parentFolder = new Folder();
// //             // Use GetOrCreateNodeWithText to ensure the returned node has its Parent set.
// //             var childNode = parentFolder.GetOrCreateNodeWithText<ProxyNode>(text);
// //             return childNode;
// //         }
// //     }
// }