// using FluentAssertions;
// using Moq;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.Build.Logging.StructuredLogger;
// using StructuredLogViewer;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ProjectReferenceGraph"/> class.
//     /// </summary>
//     public class ProjectReferenceGraphTests
//     {
// //         // Note: The following tests assume the existence of constructors and settable properties // [Error] (31-58)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         // for types such as Build, Folder, ProjectEvaluation, etc.
// //         // TODO: Replace the dummy implementations with actual valid instances as per your project setup.
// // 
// //         /// <summary>
// //         /// Tests that the constructor throws a <see cref="NullReferenceException"/> when provided with a null Build.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_NullBuild_ThrowsException()
// //         {
// //             // Arrange
// //             Build nullBuild = null;
// // 
// //             // Act
// //             Action act = () => new ProjectReferenceGraph(nullBuild);
// // 
// //             // Assert
// //             act.Should().Throw<NullReferenceException>();
// //         }
// //  // [Error] (49-58)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         /// <summary>
//         /// Tests that the constructor does not throw when provided with a Build that has no evaluations.
//         /// This test uses a dummy Build instance with an empty EvaluationFolder.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithEmptyBuild_DoesNotThrow()
//         {
//             // Arrange
//             // TODO: Create a valid Build instance with an EvaluationFolder that has empty Children.
//             Build dummyBuild = CreateDummyBuildWithoutEvaluations();
// 
//             // Act
//             Action act = () => new ProjectReferenceGraph(dummyBuild);
// 
//             // Assert
//             act.Should().NotThrow();
//         }
// //  // [Error] (63-51)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build' // [Error] (64-31)CS7036 There is no argument given that corresponds to the required parameter 'query' of 'NodeQueryMatcher.NodeQueryMatcher(string)' // [Error] (67-17)CS0272 The property or indexer 'NodeQueryMatcher.TypeKeyword' cannot be used in this context because the set accessor is inaccessible
// //         /// <summary>
// //         /// Tests that TryGetResults returns false when the matcher TypeKeyword is neither "project" nor "projectreference".
// //         /// </summary>
// //         [Fact]
// //         public void TryGetResults_WithNonMatchingTypeKeyword_ReturnsFalse()
// //         {
// //             // Arrange
// //             Build dummyBuild = CreateDummyBuildWithoutEvaluations();
// //             var graph = new ProjectReferenceGraph(dummyBuild);
// //             var matcher = new NodeQueryMatcher
// //             {
// //                 Height = -1,
// //                 TypeKeyword = "nonmatching"
// //             };
// //             var resultSet = new List<SearchResult>();
// // 
// //             // Act
// //             bool result = graph.TryGetResults(matcher, resultSet, 10);
// // 
// //             // Assert
// //             result.Should().BeFalse();
// //             resultSet.Should().BeEmpty();
// //         }
// //  // [Error] (87-51)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build' // [Error] (88-31)CS7036 There is no argument given that corresponds to the required parameter 'query' of 'NodeQueryMatcher.NodeQueryMatcher(string)' // [Error] (92-17)CS0272 The property or indexer 'NodeQueryMatcher.TypeKeyword' cannot be used in this context because the set accessor is inaccessible
//         /// <summary>
//         /// Tests that TryGetResults processes a project height search correctly when there are no projects.
//         /// </summary>
//         [Fact]
//         public void TryGetResults_ProjectHeightSearchWithEmptyReferences_ReturnsTrueAndEmptyResultSet()
//         {
//             // Arrange
//             Build dummyBuild = CreateDummyBuildWithoutEvaluations();
//             var graph = new ProjectReferenceGraph(dummyBuild);
//             var matcher = new NodeQueryMatcher
//             {
//                 // Height is set to a value (non -1) to indicate a project height search.
//                 Height = 5,
//                 TypeKeyword = "project"
//             };
//             var resultSet = new List<SearchResult>();
// 
//             // Act
//             bool result = graph.TryGetResults(matcher, resultSet, 10);
// 
//             // Assert
//             result.Should().BeTrue();
//             resultSet.Should().BeEmpty();
//         }
// //  // [Error] (112-51)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build' // [Error] (113-31)CS7036 There is no argument given that corresponds to the required parameter 'query' of 'NodeQueryMatcher.NodeQueryMatcher(string)' // [Error] (116-17)CS0272 The property or indexer 'NodeQueryMatcher.TypeKeyword' cannot be used in this context because the set accessor is inaccessible // [Error] (117-17)CS0200 Property or indexer 'NodeQueryMatcher.ProjectMatchers' cannot be assigned to -- it is read only
// //         /// <summary>
// //         /// Tests that TryGetResults returns false and adds a note when a projectreference search is requested with an incorrect number of project matchers.
// //         /// </summary>
// //         [Fact]
// //         public void TryGetResults_ProjectReferenceWithInvalidProjectMatchers_ReturnsFalseAndAddsNote()
// //         {
// //             // Arrange
// //             Build dummyBuild = CreateDummyBuildWithoutEvaluations();
// //             var graph = new ProjectReferenceGraph(dummyBuild);
// //             var matcher = new NodeQueryMatcher
// //             {
// //                 Height = -1,
// //                 TypeKeyword = "projectreference",
// //                 ProjectMatchers = new List<NodeQueryMatcher>() // Invalid: expecting exactly one matcher.
// //             };
// //             var resultSet = new List<SearchResult>();
// // 
// //             // Act
// //             bool result = graph.TryGetResults(matcher, resultSet, 10);
// // 
// //             // Assert
// //             result.Should().BeFalse();
// //             resultSet.Should().HaveCount(1);
// //             resultSet[0].Node.Should().NotBeNull();
// //             // TODO: Optionally, validate the note text content in resultSet[0].Node.
// //         }
// //  // [Error] (141-51)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build' // [Error] (144-38)CS7036 There is no argument given that corresponds to the required parameter 'query' of 'NodeQueryMatcher.NodeQueryMatcher(string)' // [Error] (147-17)CS0272 The property or indexer 'NodeQueryMatcher.TypeKeyword' cannot be used in this context because the set accessor is inaccessible // [Error] (150-17)CS0272 The property or indexer 'NodeQueryMatcher.Terms' cannot be used in this context because the set accessor is inaccessible // [Error] (153-31)CS7036 There is no argument given that corresponds to the required parameter 'query' of 'NodeQueryMatcher.NodeQueryMatcher(string)' // [Error] (156-17)CS0272 The property or indexer 'NodeQueryMatcher.TypeKeyword' cannot be used in this context because the set accessor is inaccessible // [Error] (157-17)CS0200 Property or indexer 'NodeQueryMatcher.ProjectMatchers' cannot be assigned to -- it is read only
//         /// <summary>
//         /// Tests that TryGetResults returns true for a valid projectreference search.
//         /// Note: This test requires a valid Build instance with at least one ProjectEvaluation containing a ProjectReference.
//         /// </summary>
//         [Fact]
//         public void TryGetResults_ProjectReferenceWithValidProjectMatcher_ReturnsTrue()
//         {
//             // Arrange
//             // TODO: Create a valid Build instance with at least one ProjectEvaluation containing a ProjectReference.
//             Build dummyBuild = CreateDummyBuildWithProjectEvaluation();
//             var graph = new ProjectReferenceGraph(dummyBuild);
// 
//             // Create a dummy project matcher.
//             var projectMatcher = new NodeQueryMatcher
//             {
//                 Height = -1,
//                 TypeKeyword = "project",
//                 // Terms collection used in matching; adjust as necessary for your implementation.
//                 // For testing purposes, assume that a non-empty term will match a project.
//                 Terms = new List<string> { "dummy" }
//             };
// 
//             var matcher = new NodeQueryMatcher
//             {
//                 Height = -1,
//                 TypeKeyword = "projectreference",
//                 ProjectMatchers = new List<NodeQueryMatcher> { projectMatcher }
//             };
// 
//             var resultSet = new List<SearchResult>();
// 
//             // Act
//             bool result = graph.TryGetResults(matcher, resultSet, 10);
// 
//             // Assert
//             result.Should().BeTrue();
//             resultSet.Should().NotBeNull();
//             // TODO: Add additional assertions based on the expected structure of the search results.
//         }
// 
//         /// <summary>
//         /// Creates a dummy Build instance with an empty EvaluationFolder.
//         /// </summary>
//         /// <returns>A dummy Build instance.</returns>
//         private Build CreateDummyBuildWithoutEvaluations()
//         {
//             // TODO: Replace with an actual instance construction.
//             // The following is a minimal dummy implementation and may need adjustments based on your actual Build class.
//             var dummyBuild = new Build();
//             // It is assumed that dummyBuild.EvaluationFolder exists and its Children collection is empty.
//             return dummyBuild;
//         }
// 
//         /// <summary>
//         /// Creates a dummy Build instance with a ProjectEvaluation that contains a ProjectReference.
//         /// </summary>
//         /// <returns>A dummy Build instance.</returns>
//         private Build CreateDummyBuildWithProjectEvaluation()
//         {
//             // TODO: Replace with an actual instance construction that mimics a Build with an EvaluationFolder
//             // containing at least one ProjectEvaluation. The ProjectEvaluation should have an Items Folder
//             // with an AddItem child "ProjectReference", and at least one Item with its Text property set.
//             var dummyBuild = new Build();
//             // Setup of EvaluationFolder, Children, ProjectEvaluation, etc., should be added here.
//             return dummyBuild;
//         }
//     }
// }