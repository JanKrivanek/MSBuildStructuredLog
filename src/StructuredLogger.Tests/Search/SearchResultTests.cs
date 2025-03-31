// using FluentAssertions;
// using System;
// using StructuredLogViewer;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace StructuredLogViewer.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="SearchResult"/> class.
//     /// </summary>
//     public class SearchResultTests
//     {
// //         /// <summary> // [Error] (17-23)CS0534 'SearchResultTests.DummyBaseNode' does not implement inherited abstract member 'BaseNode.GetFullText()'
// //         /// Dummy implementation of BaseNode for testing purposes.
// //         /// </summary>
// //         private class DummyBaseNode : BaseNode
// //         {
// //             public override string ToString()
// //             {
// //                 return "DummyBaseNode";
// //             }
// //         }
// //  // [Error] (29-55)CS1721 Class 'SearchResultTests.DummyTimedNode' cannot have multiple base classes: 'SearchResultTests.DummyBaseNode' and 'TimedNode'
// //         /// <summary>
// //         /// Dummy implementation of TimedNode for testing timed properties.
// //         /// Inherits from DummyBaseNode and implements TimedNode interface members.
// //         /// </summary>
// //         private class DummyTimedNode : DummyBaseNode, TimedNode
// //         {
// //             public TimeSpan Duration { get; set; }
// //             public DateTime StartTime { get; set; }
// //             public DateTime EndTime { get; set; }
// // 
// //             public override string ToString()
// //             {
// //                 return "DummyTimedNode";
// //             }
// //         }
// // 
//         /// <summary>
//         /// Tests the parameterless constructor of SearchResult to verify default property values.
//         /// </summary>
//         [Fact]
//         public void Constructor_Parameterless_InitializesEmptyFields()
//         {
//             // Act
//             var searchResult = new SearchResult();
// 
//             // Assert
//             searchResult.Node.Should().BeNull();
//             searchResult.WordsInFields.Should().BeEmpty();
//             searchResult.FieldsToDisplay.Should().BeNull();
//             searchResult.MatchedByType.Should().BeFalse();
//             searchResult.Duration.Should().Be(TimeSpan.Zero);
//             searchResult.StartTime.Should().Be(default(DateTime));
//             searchResult.EndTime.Should().Be(default(DateTime));
//             searchResult.RootFolder.Should().BeNull();
//             searchResult.AssociatedFileCopy.Should().BeNull();
//         }
// //  // [Error] (72-49)CS1503 Argument 1: cannot convert from 'StructuredLogViewer.UnitTests.SearchResultTests.DummyBaseNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
// //         /// <summary>
// //         /// Tests the constructor of SearchResult with a non-timed BaseNode to ensure timed properties remain unset.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithNonTimedBaseNode_DoesNotSetTimedProperties()
// //         {
// //             // Arrange
// //             var dummyNode = new DummyBaseNode();
// // 
// //             // Act
// //             var searchResult = new SearchResult(dummyNode, includeDuration: true, includeStart: true, includeEnd: true);
// // 
// //             // Assert
// //             searchResult.Node.Should().Be(dummyNode);
// //             searchResult.Duration.Should().Be(TimeSpan.Zero);
// //             searchResult.StartTime.Should().Be(default(DateTime));
// //             searchResult.EndTime.Should().Be(default(DateTime));
// //         }
// //  // [Error] (103-49)CS1503 Argument 1: cannot convert from 'StructuredLogViewer.UnitTests.SearchResultTests.DummyTimedNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
//         /// <summary>
//         /// Tests the constructor of SearchResult with a TimedNode to verify that timed properties are set only when corresponding flags are true.
//         /// </summary>
//         /// <param name="includeDuration">Flag to indicate if Duration should be set.</param>
//         /// <param name="includeStart">Flag to indicate if StartTime should be set.</param>
//         /// <param name="includeEnd">Flag to indicate if EndTime should be set.</param>
//         [Theory]
//         [InlineData(true, true, true)]
//         [InlineData(true, false, false)]
//         [InlineData(false, true, false)]
//         [InlineData(false, false, true)]
//         public void Constructor_WithTimedNode_SetsTimedPropertiesBasedOnFlags(bool includeDuration, bool includeStart, bool includeEnd)
//         {
//             // Arrange
//             var dummyTimedNode = new DummyTimedNode
//             {
//                 Duration = TimeSpan.FromSeconds(10),
//                 StartTime = new DateTime(2023, 1, 1, 10, 0, 0),
//                 EndTime = new DateTime(2023, 1, 1, 10, 0, 10)
//             };
// 
//             // Act
//             var searchResult = new SearchResult(dummyTimedNode, includeDuration, includeStart, includeEnd);
// 
//             // Assert
//             searchResult.Node.Should().Be(dummyTimedNode);
//             searchResult.Duration.Should().Be(includeDuration ? dummyTimedNode.Duration : TimeSpan.Zero);
//             searchResult.StartTime.Should().Be(includeStart ? dummyTimedNode.StartTime : default(DateTime));
//             searchResult.EndTime.Should().Be(includeEnd ? dummyTimedNode.EndTime : default(DateTime));
//         }
// 
//         /// <summary>
//         /// Tests the AddMatch method when addAtBeginning is false to ensure matches are appended.
//         /// </summary>
//         [Fact]
//         public void AddMatch_WhenAddAtEnd_AppendsMatchToWordsInFields()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
// 
//             // Act
//             searchResult.AddMatch("Field1", "Match1", addAtBeginning: false);
//             searchResult.AddMatch("Field2", "Match2", addAtBeginning: false);
// 
//             // Assert
//             searchResult.WordsInFields.Should().HaveCount(2);
//             searchResult.WordsInFields[0].field.Should().Be("Field1");
//             searchResult.WordsInFields[0].match.Should().Be("Match1");
//             searchResult.WordsInFields[1].field.Should().Be("Field2");
//             searchResult.WordsInFields[1].match.Should().Be("Match2");
//         }
// 
//         /// <summary>
//         /// Tests the AddMatch method when addAtBeginning is true to ensure the match is inserted at the beginning.
//         /// </summary>
//         [Fact]
//         public void AddMatch_WhenAddAtBeginning_InsertsMatchAtBeginning()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
//             searchResult.AddMatch("Field1", "Match1", addAtBeginning: false);
// 
//             // Act
//             searchResult.AddMatch("Field2", "Match2", addAtBeginning: true);
// 
//             // Assert
//             searchResult.WordsInFields.Should().HaveCount(2);
//             searchResult.WordsInFields[0].field.Should().Be("Field2");
//             searchResult.WordsInFields[0].match.Should().Be("Match2");
//             searchResult.WordsInFields[1].field.Should().Be("Field1");
//             searchResult.WordsInFields[1].match.Should().Be("Match1");
//         }
// 
//         /// <summary>
//         /// Tests the AddMatchByNodeType method to verify that it sets MatchedByType to true.
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
//             searchResult.MatchedByType.Should().BeTrue();
//         }
// //  // [Error] (178-49)CS1503 Argument 1: cannot convert from 'StructuredLogViewer.UnitTests.SearchResultTests.DummyBaseNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
// //         /// <summary>
// //         /// Tests the ToString method when Node is not null to ensure it returns the Node's string representation.
// //         /// </summary>
// //         [Fact]
// //         public void ToString_WhenNodeIsNotNull_ReturnsNodeToString()
// //         {
// //             // Arrange
// //             var dummyNode = new DummyBaseNode();
// //             var searchResult = new SearchResult(dummyNode);
// // 
// //             // Act
// //             var result = searchResult.ToString();
// // 
// //             // Assert
// //             result.Should().Be(dummyNode.ToString());
// //         }
// // 
//         /// <summary>
//         /// Tests the ToString method when Node is null to ensure it returns null.
//         /// </summary>
//         [Fact]
//         public void ToString_WhenNodeIsNull_ReturnsNull()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
// 
//             // Act
//             var result = searchResult.ToString();
// 
//             // Assert
//             result.Should().BeNull();
//         }
//     }
// }