// using System;
// using System.Collections.Generic;
// using Microsoft.Build.Logging.StructuredLogger;
// using StructuredLogViewer;
// using Xunit;
// 
// namespace StructuredLogViewer.UnitTests
// {
//     /// <summary>
//     /// Provides a dummy implementation of BaseNode for testing purposes.
//     /// </summary>
//     public class DummyBaseNode : BaseNode
//     {
//         /// <summary>
//         /// Returns a string representation of this DummyBaseNode.
//         /// </summary>
//         /// <returns>A fixed string "DummyBaseNode".</returns>
//         public override string ToString() => "DummyBaseNode";
//     }
// 
//     /// <summary>
//     /// Provides a dummy implementation of TimedNode for testing purposes.
//     /// </summary>
// //     public class DummyTimedNode : TimedNode [Error] (34-13)CS0200 Property or indexer 'TimedNode.Duration' cannot be assigned to -- it is read only
// //     {
// //         /// <summary>
// //         /// Initializes a new instance of the <see cref="DummyTimedNode"/> class with specified timing values.
// //         /// </summary>
// //         /// <param name="duration">The duration to set.</param>
// //         /// <param name="startTime">The start time to set.</param>
// //         /// <param name="endTime">The end time to set.</param>
// //         public DummyTimedNode(TimeSpan duration, DateTime startTime, DateTime endTime)
// //         {
// //             Duration = duration;
// //             StartTime = startTime;
// //             EndTime = endTime;
// //         }
// // 
// //         /// <summary>
// //         /// Returns a string representation of this DummyTimedNode.
// //         /// </summary>
// //         /// <returns>A fixed string "DummyTimedNode".</returns>
// //         public override string ToString() => "DummyTimedNode";
// //     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="SearchResult"/> class.
//     /// </summary>
//     public class SearchResultTests
//     {
//         /// <summary>
//         /// Tests the default constructor of SearchResult to ensure it initializes properties appropriately.
//         /// Expected outcome: Node is null, WordsInFields is empty, timing and other properties have default values.
//         /// </summary>
//         [Fact]
//         public void DefaultConstructor_InitializesPropertiesCorrectly()
//         {
//             // Arrange & Act
//             var searchResult = new SearchResult();
// 
//             // Assert
//             Assert.Null(searchResult.Node);
//             Assert.Empty(searchResult.WordsInFields);
//             Assert.False(searchResult.MatchedByType);
//             Assert.Null(searchResult.FieldsToDisplay);
//             Assert.Equal(default(TimeSpan), searchResult.Duration);
//             Assert.Equal(default(DateTime), searchResult.StartTime);
//             Assert.Equal(default(DateTime), searchResult.EndTime);
//             Assert.Null(searchResult.RootFolder);
//             Assert.Null(searchResult.AssociatedFileCopy);
//         }
// 
//         /// <summary>
//         /// Tests the parameterized constructor with a non-timed BaseNode.
//         /// Expected outcome: Timing properties remain default since the provided node is not a TimedNode.
//         /// </summary>
// //         [Fact] [Error] (84-49)CS1503 Argument 1: cannot convert from 'StructuredLogViewer.UnitTests.DummyBaseNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' [Error] (87-26)CS1503 Argument 1: cannot convert from 'StructuredLogViewer.UnitTests.DummyBaseNode' to 'System.DateTime' [Error] (87-37)CS1503 Argument 2: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.BaseNode' to 'System.DateTime'
// //         public void ParameterizedConstructor_WithNonTimedNode_DoesNotSetTimingProperties()
// //         {
// //             // Arrange
// //             var dummyNode = new DummyBaseNode();
// // 
// //             // Act
// //             var searchResult = new SearchResult(dummyNode, includeDuration: true, includeStart: true, includeEnd: true);
// // 
// //             // Assert
// //             Assert.Equal(dummyNode, searchResult.Node);
// //             Assert.Equal(default(TimeSpan), searchResult.Duration);
// //             Assert.Equal(default(DateTime), searchResult.StartTime);
// //             Assert.Equal(default(DateTime), searchResult.EndTime);
// //         }
// 
//         /// <summary>
//         /// Tests the parameterized constructor with a timed node to verify timing properties are set conditionally.
//         /// Expected outcome: When flags are set true the corresponding timing properties are assigned; otherwise they remain default.
//         /// </summary>
//         [Fact]
//         public void ParameterizedConstructor_WithTimedNode_SetsTimingPropertiesBasedOnFlags()
//         {
//             // Arrange
//             var expectedDuration = TimeSpan.FromSeconds(30);
//             var expectedStartTime = new DateTime(2023, 1, 1, 10, 0, 0);
//             var expectedEndTime = new DateTime(2023, 1, 1, 10, 30, 0);
//             var timedNode = new DummyTimedNode(expectedDuration, expectedStartTime, expectedEndTime);
// 
//             // Act
//             var searchResultAll = new SearchResult(timedNode, includeDuration: true, includeStart: true, includeEnd: true);
//             var searchResultPartial = new SearchResult(timedNode, includeDuration: false, includeStart: true, includeEnd: false);
// 
//             // Assert for searchResultAll
//             Assert.Equal(expectedDuration, searchResultAll.Duration);
//             Assert.Equal(expectedStartTime, searchResultAll.StartTime);
//             Assert.Equal(expectedEndTime, searchResultAll.EndTime);
// 
//             // Assert for searchResultPartial
//             Assert.Equal(default(TimeSpan), searchResultPartial.Duration);
//             Assert.Equal(expectedStartTime, searchResultPartial.StartTime);
//             Assert.Equal(default(DateTime), searchResultPartial.EndTime);
//         }
// 
//         /// <summary>
//         /// Tests the AddMatch method when addAtBeginning is false.
//         /// Expected outcome: The match is added at the end of the WordsInFields list.
//         /// </summary>
//         [Fact]
//         public void AddMatch_WithAddAtBeginningFalse_AddsMatchAtEnd()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
//             var field1 = "FieldA";
//             var word1 = "MatchA";
//             var field2 = "FieldB";
//             var word2 = "MatchB";
// 
//             // Act
//             searchResult.AddMatch(field1, word1, addAtBeginning: false);
//             searchResult.AddMatch(field2, word2, addAtBeginning: false);
// 
//             // Assert
//             Assert.Equal(2, searchResult.WordsInFields.Count);
//             Assert.Equal((field1, word1), searchResult.WordsInFields[0]);
//             Assert.Equal((field2, word2), searchResult.WordsInFields[1]);
//         }
// 
//         /// <summary>
//         /// Tests the AddMatch method when addAtBeginning is true.
//         /// Expected outcome: The match is inserted at the beginning of the WordsInFields list.
//         /// </summary>
//         [Fact]
//         public void AddMatch_WithAddAtBeginningTrue_AddsMatchAtBeginning()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
//             var field1 = "FieldA";
//             var word1 = "MatchA";
//             var field2 = "FieldB";
//             var word2 = "MatchB";
// 
//             // Act
//             searchResult.AddMatch(field1, word1, addAtBeginning: false); // List: [(FieldA, MatchA)]
//             searchResult.AddMatch(field2, word2, addAtBeginning: true);  // List: [(FieldB, MatchB), (FieldA, MatchA)]
// 
//             // Assert
//             Assert.Equal(2, searchResult.WordsInFields.Count);
//             Assert.Equal((field2, word2), searchResult.WordsInFields[0]);
//             Assert.Equal((field1, word1), searchResult.WordsInFields[1]);
//         }
// 
//         /// <summary>
//         /// Tests the AddMatchByNodeType method to ensure it flags the SearchResult as matched by node type.
//         /// Expected outcome: MatchedByType property is set to true.
//         /// </summary>
//         [Fact]
//         public void AddMatchByNodeType_SetsMatchedByTypeToTrue()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
// 
//             // Act
//             searchResult.AddMatchByNodeType();
// 
//             // Assert
//             Assert.True(searchResult.MatchedByType);
//         }
// 
//         /// <summary>
//         /// Tests the ToString method when Node is non-null.
//         /// Expected outcome: Returns the string provided by the Node's ToString.
//         /// </summary>
// //         [Fact] [Error] (195-49)CS1503 Argument 1: cannot convert from 'StructuredLogViewer.UnitTests.DummyBaseNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
// //         public void ToString_WithNonNullNode_ReturnsNodeString()
// //         {
// //             // Arrange
// //             var dummyNode = new DummyBaseNode();
// //             var searchResult = new SearchResult(dummyNode);
// // 
// //             // Act
// //             var result = searchResult.ToString();
// // 
// //             // Assert
// //             Assert.Equal(dummyNode.ToString(), result);
// //         }
// 
//         /// <summary>
//         /// Tests the ToString method when Node is null.
//         /// Expected outcome: Returns null.
//         /// </summary>
//         [Fact]
//         public void ToString_WithNullNode_ReturnsNull()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
// 
//             // Act
//             var result = searchResult.ToString();
// 
//             // Assert
//             Assert.Null(result);
//         }
//     }
// }
