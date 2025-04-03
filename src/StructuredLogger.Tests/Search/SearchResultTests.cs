// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Moq;
// using StructuredLogViewer;
// using System;
// using System.Collections.Generic;
// using Xunit;
// 
// namespace StructuredLogViewer.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="SearchResult"/> class.
//     /// </summary>
//     public class SearchResultTests
//     {
//         /// <summary>
//         /// A fake TimedNode implementation for testing the SearchResult constructor with temporal properties.
//         /// </summary>
//         private class FakeTimedNode : TimedNode
//         {
//             // Hide base Duration, StartTime and EndTime with settable properties for testing purposes.
//             public new TimeSpan Duration { get; set; }
//             public new DateTime StartTime { get; set; }
//             public new DateTime EndTime { get; set; }
// 
//             public override string ToString()
//             {
//                 return "FakeTimedNode";
//             }
//         }
// 
//         /// <summary>
//         /// A dummy BaseNode implementation for testing non-timed node scenarios.
//         /// </summary>
//         private class DummyBaseNode : BaseNode
//         {
//             public override string ToString()
//             {
//                 return "DummyBaseNode";
//             }
//         }
// 
//         /// <summary>
//         /// Tests that the default constructor initializes properties to their default values.
//         /// Expected outcome: Node is null, time properties are default or zero, and collections are empty.
//         /// </summary>
//         [Fact]
//         public void DefaultConstructor_WhenCalled_InitializesDefaultValues()
//         {
//             // Arrange & Act
//             var searchResult = new SearchResult();
//             
//             // Assert
//             searchResult.Node.Should().BeNull();
//             searchResult.FieldsToDisplay.Should().BeNull();
//             searchResult.RootFolder.Should().BeNull();
//             searchResult.AssociatedFileCopy.Should().BeNull();
//             searchResult.Duration.Should().Be(TimeSpan.Zero);
//             searchResult.StartTime.Should().Be(default);
//             searchResult.EndTime.Should().Be(default);
//             searchResult.WordsInFields.Should().BeEmpty();
//             searchResult.MatchedByType.Should().BeFalse();
//         }
// 
//         /// <summary>
//         /// Tests that the parameterized constructor with a non-timed node does not set temporal properties.
//         /// Expected outcome: Duration, StartTime, and EndTime remain at their default values.
//         /// </summary>
//         [Fact]
//         public void ParameterizedConstructor_WithNonTimedNode_DoesNotSetTimeProperties()
//         {
//             // Arrange
//             var dummyNode = new DummyBaseNode();
// 
//             // Act
//             var searchResult = new SearchResult(dummyNode, includeDuration: true, includeStart: true, includeEnd: true);
// 
//             // Assert
//             searchResult.Node.Should().BeSameAs(dummyNode);
//             searchResult.Duration.Should().Be(TimeSpan.Zero);
//             searchResult.StartTime.Should().Be(default);
//             searchResult.EndTime.Should().Be(default);
//         }
// 
//         /// <summary>
//         /// Tests that the parameterized constructor with a timed node sets temporal properties when flags are true.
//         /// Expected outcome: Duration, StartTime, and EndTime are copied from the provided timed node.
//         /// </summary>
//         [Fact]
//         public void ParameterizedConstructor_WithTimedNode_IncludesSpecifiedTimeProperties()
//         {
//             // Arrange
//             var fakeTimedNode = new FakeTimedNode
//             {
//                 Duration = TimeSpan.FromMinutes(5),
//                 StartTime = DateTime.Now.AddMinutes(-10),
//                 EndTime = DateTime.Now
//             };
// 
//             // Act
//             var searchResult = new SearchResult(fakeTimedNode, includeDuration: true, includeStart: true, includeEnd: true);
// 
//             // Assert
//             searchResult.Node.Should().BeSameAs(fakeTimedNode);
//             searchResult.Duration.Should().Be(fakeTimedNode.Duration);
//             searchResult.StartTime.Should().Be(fakeTimedNode.StartTime);
//             searchResult.EndTime.Should().Be(fakeTimedNode.EndTime);
//         }
// 
//         /// <summary>
//         /// Tests that the parameterized constructor with a timed node does not set temporal properties when flags are false.
//         /// Expected outcome: Temporal properties remain at their default values.
//         /// </summary>
//         [Fact]
//         public void ParameterizedConstructor_WithTimedNode_ExcludesTimePropertiesWhenFlagsFalse()
//         {
//             // Arrange
//             var fakeTimedNode = new FakeTimedNode
//             {
//                 Duration = TimeSpan.FromMinutes(10),
//                 StartTime = DateTime.Now.AddMinutes(-20),
//                 EndTime = DateTime.Now
//             };
// 
//             // Act
//             var searchResult = new SearchResult(fakeTimedNode, includeDuration: false, includeStart: false, includeEnd: false);
// 
//             // Assert
//             searchResult.Node.Should().BeSameAs(fakeTimedNode);
//             searchResult.Duration.Should().Be(TimeSpan.Zero);
//             searchResult.StartTime.Should().Be(default);
//             searchResult.EndTime.Should().Be(default);
//         }
// 
//         /// <summary>
//         /// Tests that the AddMatch method appends the match when addAtBeginning is false.
//         /// Expected outcome: The new match appears at the end of the WordsInFields list.
//         /// </summary>
//         [Fact]
//         public void AddMatch_WhenAddAtBeginningFalse_AppendsMatch()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
//             int initialCount = searchResult.WordsInFields.Count;
// 
//             // Act
//             searchResult.AddMatch("Field1", "Word1", addAtBeginning: false);
// 
//             // Assert
//             searchResult.WordsInFields.Count.Should().Be(initialCount + 1);
//             searchResult.WordsInFields[initialCount].field.Should().Be("Field1");
//             searchResult.WordsInFields[initialCount].match.Should().Be("Word1");
//         }
// 
//         /// <summary>
//         /// Tests that the AddMatch method inserts the match at the beginning when addAtBeginning is true.
//         /// Expected outcome: The new match appears as the first element in the WordsInFields list.
//         /// </summary>
//         [Fact]
//         public void AddMatch_WhenAddAtBeginningTrue_InsertsMatchAtBeginning()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
//             // Pre-populate with an existing match.
//             searchResult.AddMatch("ExistingField", "ExistingWord");
//             int initialCount = searchResult.WordsInFields.Count;
// 
//             // Act
//             searchResult.AddMatch("NewField", "NewWord", addAtBeginning: true);
// 
//             // Assert
//             searchResult.WordsInFields.Count.Should().Be(initialCount + 1);
//             searchResult.WordsInFields[0].field.Should().Be("NewField");
//             searchResult.WordsInFields[0].match.Should().Be("NewWord");
//         }
// 
//         /// <summary>
//         /// Tests that the AddMatchByNodeType method sets the MatchedByType property to true.
//         /// Expected outcome: MatchedByType becomes true after the method is invoked.
//         /// </summary>
//         [Fact]
//         public void AddMatchByNodeType_WhenCalled_SetsMatchedByTypeToTrue()
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
// 
//         /// <summary>
//         /// Tests that the ToString method returns the string representation of the Node when Node is not null.
//         /// Expected outcome: ToString returns the result of Node.ToString().
//         /// </summary>
//         [Fact]
//         public void ToString_WhenNodeIsNotNull_ReturnsNodeToString()
//         {
//             // Arrange
//             var dummyNode = new DummyBaseNode();
//             var searchResult = new SearchResult(dummyNode);
// 
//             // Act
//             string result = searchResult.ToString();
// 
//             // Assert
//             result.Should().Be("DummyBaseNode");
//         }
// 
//         /// <summary>
//         /// Tests that the ToString method returns null when Node is null.
//         /// Expected outcome: ToString returns null.
//         /// </summary>
//         [Fact]
//         public void ToString_WhenNodeIsNull_ReturnsNull()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
// 
//             // Act
//             string result = searchResult.ToString();
// 
//             // Assert
//             result.Should().BeNull();
//         }
// 
//         /// <summary>
//         /// Tests setting and getting the FieldsToDisplay property using various string values.
//         /// Expected outcome: The property returns exactly what was set.
//         /// </summary>
//         [Fact]
//         public void FieldsToDisplayProperty_WhenSet_ReturnsSameValue()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
//             var fields = new List<string> { string.Empty, "A", new string('b', 500), "Special!@#$%^&*()" };
// 
//             // Act
//             searchResult.FieldsToDisplay = fields;
// 
//             // Assert
//             searchResult.FieldsToDisplay.Should().BeEquivalentTo(fields);
//         }
// 
//         /// <summary>
//         /// Tests setting and getting the RootFolder property with various path formats.
//         /// Expected outcome: The property returns exactly what was set.
//         /// </summary>
//         [Theory]
//         [InlineData("")]
//         [InlineData("C:\\Folder\\SubFolder")]
//         [InlineData("/usr/local/bin")]
//         [InlineData("\\\\NetworkPath\\SharedFolder")]
//         public void RootFolderProperty_WhenSet_ReturnsSameValue(string path)
//         {
//             // Arrange
//             var searchResult = new SearchResult();
// 
//             // Act
//             searchResult.RootFolder = path;
// 
//             // Assert
//             searchResult.RootFolder.Should().Be(path);
//         }
// 
//         /// <summary>
//         /// Tests setting and retrieving the Duration, StartTime, and EndTime properties.
//         /// Expected outcome: The properties reflect the set values accurately.
//         /// </summary>
//         [Fact]
//         public void TimeProperties_WhenSet_ReturnCorrectValues()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
//             var duration = TimeSpan.FromSeconds(30);
//             var startTime = DateTime.UtcNow;
//             var endTime = DateTime.UtcNow.AddSeconds(30);
// 
//             // Act
//             searchResult.Duration = duration;
//             searchResult.StartTime = startTime;
//             searchResult.EndTime = endTime;
// 
//             // Assert
//             searchResult.Duration.Should().Be(duration);
//             searchResult.StartTime.Should().Be(startTime);
//             searchResult.EndTime.Should().Be(endTime);
//         }
// 
//         /// <summary>
//         /// Tests setting and getting the AssociatedFileCopy property.
//         /// Expected outcome: The property returns the same instance that was set.
//         /// </summary>
//         [Fact]
//         public void AssociatedFileCopyProperty_WhenSet_ReturnsSameInstance()
//         {
//             // Arrange
//             var searchResult = new SearchResult();
//             var fileCopyInfo = new FileCopyInfo();
// 
//             // Act
//             searchResult.AssociatedFileCopy = fileCopyInfo;
// 
//             // Assert
//             searchResult.AssociatedFileCopy.Should().BeSameAs(fileCopyInfo);
//         }
//     }
// }