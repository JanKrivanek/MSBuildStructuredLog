using FluentAssertions;
using System;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TimedNode"/> class.
    /// </summary>
    public class TimedNodeTests
    {
        private readonly TimedNode _timedNode;

        public TimedNodeTests()
        {
            _timedNode = new TimedNode();
        }

        /// <summary>
        /// Tests that the Duration property returns the correct time difference when EndTime is after StartTime.
        /// </summary>
        [Fact]
        public void Duration_WhenEndTimeIsAfterStartTime_ReturnsCorrectDifference()
        {
            // Arrange
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(2);
            _timedNode.StartTime = start;
            _timedNode.EndTime = end;
            
            // Act
            TimeSpan duration = _timedNode.Duration;
            
            // Assert
            duration.Should().Be(end - start);
        }

        /// <summary>
        /// Tests that the Duration property returns TimeSpan.Zero when EndTime is before StartTime.
        /// </summary>
        [Fact]
        public void Duration_WhenEndTimeIsBeforeStartTime_ReturnsZero()
        {
            // Arrange
            DateTime start = DateTime.Now;
            DateTime end = start.AddHours(-2);
            _timedNode.StartTime = start;
            _timedNode.EndTime = end;
            
            // Act
            TimeSpan duration = _timedNode.Duration;
            
            // Assert
            duration.Should().Be(TimeSpan.Zero);
        }

        /// <summary>
        /// Tests that the TypeName property returns "TimedNode".
        /// </summary>
        [Fact]
        public void TypeName_ReturnsTimedNode()
        {
            // Act
            string typeName = _timedNode.TypeName;
            
            // Assert
            typeName.Should().Be(nameof(TimedNode));
        }

        /// <summary>
        /// Tests that the DurationText property returns a non-empty string as provided by TextUtilities.
        /// </summary>
        [Fact]
        public void DurationText_WhenCalled_ReturnsNonEmptyString()
        {
            // Arrange
            DateTime start = new DateTime(2023, 1, 1, 0, 0, 0);
            DateTime end = start.AddMinutes(30);
            _timedNode.StartTime = start;
            _timedNode.EndTime = end;
            
            // Act
            string durationText = _timedNode.DurationText;
            
            // Assert
            durationText.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests that GetTimeAndDurationText returns a formatted string containing start time, end time, and duration with default precision.
        /// </summary>
        [Fact]
        public void GetTimeAndDurationText_DefaultPrecision_ReturnsFormattedString()
        {
            // Arrange
            DateTime start = new DateTime(2023, 1, 1, 8, 0, 0);
            DateTime end = start.AddHours(1);
            _timedNode.StartTime = start;
            _timedNode.EndTime = end;
            
            // Act
            string result = _timedNode.GetTimeAndDurationText();
            
            // Assert
            result.Should().Contain("Start:")
                  .And.Contain("End:")
                  .And.Contain("Duration:");
        }

        /// <summary>
        /// Tests that GetTimeAndDurationText returns a formatted string containing start time, end time, and duration with full precision.
        /// </summary>
        [Fact]
        public void GetTimeAndDurationText_FullPrecision_ReturnsFormattedString()
        {
            // Arrange
            DateTime start = new DateTime(2023, 1, 1, 8, 0, 0);
            DateTime end = start.AddSeconds(90);
            _timedNode.StartTime = start;
            _timedNode.EndTime = end;
            
            // Act
            string result = _timedNode.GetTimeAndDurationText(fullPrecision: true);
            
            // Assert
            result.Should().Contain("Start:")
                  .And.Contain("End:")
                  .And.Contain("Duration:");
        }

        /// <summary>
        /// Tests that the ToolTip property returns the same formatted string as GetTimeAndDurationText.
        /// </summary>
        [Fact]
        public void ToolTip_ReturnsSameValueAsGetTimeAndDurationText()
        {
            // Arrange
            DateTime start = new DateTime(2023, 1, 1, 12, 0, 0);
            DateTime end = start.AddMinutes(15);
            _timedNode.StartTime = start;
            _timedNode.EndTime = end;
            string expectedToolTip = _timedNode.GetTimeAndDurationText();
            
            // Act
            string toolTip = _timedNode.ToolTip;
            
            // Assert
            toolTip.Should().Be(expectedToolTip);
        }

        /// <summary>
        /// Tests that the auto-properties Id, NodeId, and Index can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void AutoProperties_SetAndGet_ReturnsAssignedValues()
        {
            // Arrange
            int expectedId = 123;
            int expectedNodeId = 456;
            int expectedIndex = int.MaxValue;
            
            // Act
            _timedNode.Id = expectedId;
            _timedNode.NodeId = expectedNodeId;
            _timedNode.Index = expectedIndex;
            
            // Assert
            _timedNode.Id.Should().Be(expectedId);
            _timedNode.NodeId.Should().Be(expectedNodeId);
            _timedNode.Index.Should().Be(expectedIndex);
        }
    }
}
