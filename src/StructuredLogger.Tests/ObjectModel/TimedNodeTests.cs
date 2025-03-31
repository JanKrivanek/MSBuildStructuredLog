// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using StructuredLogViewer;
// using System;
// using System.Diagnostics;
// using System.Windows.Forms;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "TimedNode"/> class.
//     /// </summary>
//     public class TimedNodeTests
//     {
// //         /// <summary> // [Error] (27-23)CS1061 'TimedNode' does not contain a definition for 'StartTime' and no accessible extension method 'StartTime' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (28-23)CS1061 'TimedNode' does not contain a definition for 'EndTime' and no accessible extension method 'EndTime' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests that the Duration property returns the correct difference when EndTime is after StartTime.
// //         /// </summary>
// //         [Fact]
// //         public void Duration_EndTimeAfterStartTime_ReturnsCorrectDifference()
// //         {
// //             // Arrange
// //             var timedNode = new TimedNode();
// //             var startTime = DateTime.Now;
// //             var endTime = startTime.AddHours(1);
// //             timedNode.StartTime = startTime;
// //             timedNode.EndTime = endTime;
// //             // Act
// //             TimeSpan duration = timedNode.Duration;
// //             // Assert
// //             duration.Should().Be(endTime - startTime);
// //         }
// //  // [Error] (45-23)CS1061 'TimedNode' does not contain a definition for 'StartTime' and no accessible extension method 'StartTime' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (46-23)CS1061 'TimedNode' does not contain a definition for 'EndTime' and no accessible extension method 'EndTime' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the Duration property returns TimeSpan.Zero when EndTime is before StartTime.
//         /// </summary>
//         [Fact]
//         public void Duration_EndTimeBeforeStartTime_ReturnsZero()
//         {
//             // Arrange
//             var timedNode = new TimedNode();
//             var endTime = DateTime.Now;
//             var startTime = endTime.AddHours(1);
//             timedNode.StartTime = startTime;
//             timedNode.EndTime = endTime;
//             // Act
//             TimeSpan duration = timedNode.Duration;
//             // Assert
//             duration.Should().Be(TimeSpan.Zero);
//         }
// //  // [Error] (65-23)CS1061 'TimedNode' does not contain a definition for 'Id' and no accessible extension method 'Id' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (66-23)CS1061 'TimedNode' does not contain a definition for 'NodeId' and no accessible extension method 'NodeId' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (69-23)CS1061 'TimedNode' does not contain a definition for 'Id' and no accessible extension method 'Id' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (70-23)CS1061 'TimedNode' does not contain a definition for 'NodeId' and no accessible extension method 'NodeId' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the auto-properties Id, NodeId, and Index can be correctly set and retrieved.
// //         /// </summary>
// //         [Fact]
// //         public void Properties_SetAndGet_ReturnsAssignedValues()
// //         {
// //             // Arrange
// //             var timedNode = new TimedNode();
// //             int expectedId = 42;
// //             int expectedNodeId = 7;
// //             int expectedIndex = 3;
// //             // Act
// //             timedNode.Id = expectedId;
// //             timedNode.NodeId = expectedNodeId;
// //             timedNode.Index = expectedIndex;
// //             // Assert
// //             timedNode.Id.Should().Be(expectedId);
// //             timedNode.NodeId.Should().Be(expectedNodeId);
// //             timedNode.Index.Should().Be(expectedIndex);
// //         }
// //  // [Error] (83-41)CS1061 'TimedNode' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the TypeName property returns "TimedNode".
//         /// </summary>
//         [Fact]
//         public void TypeName_ReturnsTimedNode()
//         {
//             // Arrange
//             var timedNode = new TimedNode();
//             // Act
//             string typeName = timedNode.TypeName;
//             // Assert
//             typeName.Should().Be("TimedNode");
//         }
// //  // [Error] (102-23)CS1061 'TimedNode' does not contain a definition for 'StartTime' and no accessible extension method 'StartTime' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (103-23)CS1061 'TimedNode' does not contain a definition for 'EndTime' and no accessible extension method 'EndTime' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (105-39)CS1061 'TimedNode' does not contain a definition for 'GetTimeAndDurationText' and no accessible extension method 'GetTimeAndDurationText' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that GetTimeAndDurationText returns a string containing Start, End, and Duration information.
// //         /// This test validates both default and full precision text generation.
// //         /// </summary>
// //         /// <param name = "fullPrecision">Determines if the full precision format is used.</param>
// //         [Theory]
// //         [InlineData(false)]
// //         [InlineData(true)]
// //         public void GetTimeAndDurationText_VariousPrecision_IncludesTimeAndDurationText(bool fullPrecision)
// //         {
// //             // Arrange
// //             var timedNode = new TimedNode();
// //             var startTime = new DateTime(2023, 1, 1, 8, 0, 0);
// //             var endTime = new DateTime(2023, 1, 1, 10, 30, 0);
// //             timedNode.StartTime = startTime;
// //             timedNode.EndTime = endTime;
// //             // Act
// //             string result = timedNode.GetTimeAndDurationText(fullPrecision);
// //             // Assert
// //             result.Should().Contain("Start:");
// //             result.Should().Contain("End:");
// //             result.Should().Contain("Duration:");
// //         }
// //  // [Error] (123-23)CS1061 'TimedNode' does not contain a definition for 'StartTime' and no accessible extension method 'StartTime' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (124-23)CS1061 'TimedNode' does not contain a definition for 'EndTime' and no accessible extension method 'EndTime' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (126-39)CS1061 'TimedNode' does not contain a definition for 'GetTimeAndDurationText' and no accessible extension method 'GetTimeAndDurationText' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that GetTimeAndDurationText replaces an empty DurationText with "0".
//         /// This simulates the scenario where TextUtilities.DisplayDuration returns an empty string.
//         /// </summary>
//         [Fact]
//         public void GetTimeAndDurationText_EmptyDurationText_ReplacesWithZero()
//         {
//             // Arrange
//             var timedNode = new TimedNode();
//             // Set identical StartTime and EndTime so Duration is zero, assuming TextUtilities.DisplayDuration may return an empty string.
//             var time = DateTime.Now;
//             timedNode.StartTime = time;
//             timedNode.EndTime = time;
//             // Act
//             string result = timedNode.GetTimeAndDurationText();
//             // Assert
//             result.Should().Contain("Duration: 0");
//         }
// //  // [Error] (140-23)CS1061 'TimedNode' does not contain a definition for 'StartTime' and no accessible extension method 'StartTime' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (141-23)CS1061 'TimedNode' does not contain a definition for 'EndTime' and no accessible extension method 'EndTime' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (143-40)CS1061 'TimedNode' does not contain a definition for 'ToolTip' and no accessible extension method 'ToolTip' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (144-52)CS1061 'TimedNode' does not contain a definition for 'GetTimeAndDurationText' and no accessible extension method 'GetTimeAndDurationText' accepting a first argument of type 'TimedNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the ToolTip property returns the same value as GetTimeAndDurationText.
// //         /// </summary>
// //         [Fact]
// //         public void ToolTip_EqualsGetTimeAndDurationText()
// //         {
// //             // Arrange
// //             var timedNode = new TimedNode();
// //             var time = new DateTime(2023, 5, 5, 12, 0, 0);
// //             timedNode.StartTime = time;
// //             timedNode.EndTime = time.AddMinutes(15);
// //             // Act
// //             string toolTip = timedNode.ToolTip;
// //             string timeAndDurationText = timedNode.GetTimeAndDurationText();
// //             // Assert
// //             toolTip.Should().Be(timeAndDurationText);
// //         }
// //     }
// }