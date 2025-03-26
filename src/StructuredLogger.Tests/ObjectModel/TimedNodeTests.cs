using System;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TimedNode"/> class.
    /// </summary>
    public class TimedNodeTests
    {
        private readonly TimedNode _timedNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedNodeTests"/> class.
        /// </summary>
        public TimedNodeTests()
        {
            _timedNode = new TimedNode();
        }

        /// <summary>
        /// Tests that the Duration property returns the correct difference when EndTime is after StartTime.
        /// </summary>
        [Fact]
        public void Duration_WhenEndTimeGreaterThanStartTime_ReturnsDifference()
        {
            // Arrange
            var startTime = new DateTime(2023, 10, 1, 8, 0, 0);
            var endTime = new DateTime(2023, 10, 1, 9, 30, 0);
            _timedNode.StartTime = startTime;
            _timedNode.EndTime = endTime;

            // Act
            var duration = _timedNode.Duration;

            // Assert
            Assert.Equal(endTime - startTime, duration);
        }

        /// <summary>
        /// Tests that the Duration property returns TimeSpan.Zero when EndTime equals StartTime.
        /// </summary>
        [Fact]
        public void Duration_WhenEndTimeEqualsStartTime_ReturnsZero()
        {
            // Arrange
            var dateTime = new DateTime(2023, 10, 1, 10, 0, 0);
            _timedNode.StartTime = dateTime;
            _timedNode.EndTime = dateTime;

            // Act
            var duration = _timedNode.Duration;

            // Assert
            Assert.Equal(TimeSpan.Zero, duration);
        }

        /// <summary>
        /// Tests that the Duration property returns TimeSpan.Zero when EndTime is before StartTime.
        /// </summary>
        [Fact]
        public void Duration_WhenEndTimeBeforeStartTime_ReturnsZero()
        {
            // Arrange
            var startTime = new DateTime(2023, 10, 1, 12, 0, 0);
            var endTime = new DateTime(2023, 10, 1, 11, 0, 0);
            _timedNode.StartTime = startTime;
            _timedNode.EndTime = endTime;

            // Act
            var duration = _timedNode.Duration;

            // Assert
            Assert.Equal(TimeSpan.Zero, duration);
        }

        /// <summary>
        /// Tests that the DurationText property returns a non-null string.
        /// Since DurationText depends on the external TextUtilities, this test ensures the property is not null.
        /// </summary>
        [Fact]
        public void DurationText_WhenCalled_ReturnsNonNullString()
        {
            // Arrange
            _timedNode.StartTime = DateTime.Now;
            _timedNode.EndTime = DateTime.Now.AddMinutes(15);

            // Act
            var durationText = _timedNode.DurationText;

            // Assert
            Assert.NotNull(durationText);
        }

        /// <summary>
        /// Tests that GetTimeAndDurationText returns a formatted string containing "Start:", "End:", and "Duration:".
        /// </summary>
        /// <param name="fullPrecision">Specifies whether full precision formatting is requested.</param>
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetTimeAndDurationText_WhenCalled_ReturnsFormattedString(bool fullPrecision)
        {
            // Arrange
            var startTime = new DateTime(2023, 10, 1, 7, 45, 0);
            var endTime = new DateTime(2023, 10, 1, 8, 30, 0);
            _timedNode.StartTime = startTime;
            _timedNode.EndTime = endTime;

            // Act
            var result = _timedNode.GetTimeAndDurationText(fullPrecision);

            // Assert
            Assert.Contains("Start:", result);
            Assert.Contains("End:", result);
            Assert.Contains("Duration:", result);
        }

        /// <summary>
        /// Tests that GetTimeAndDurationText substitutes "0" for an empty duration string.
        /// Since DurationText is computed via TextUtilities, this test uses a scenario 
        /// where StartTime equals EndTime resulting in a zero duration.
        /// </summary>
        [Fact]
        public void GetTimeAndDurationText_WhenDurationIsZero_ReturnsZeroDuration()
        {
            // Arrange
            var dateTime = DateTime.Now;
            _timedNode.StartTime = dateTime;
            _timedNode.EndTime = dateTime;

            // Act
            var result = _timedNode.GetTimeAndDurationText();

            // Assert
            Assert.Contains("Duration: 0", result);
        }

        /// <summary>
        /// Tests that the ToolTip property returns the same value as GetTimeAndDurationText.
        /// </summary>
        [Fact]
        public void ToolTip_WhenAccessed_ReturnsSameAsGetTimeAndDurationText()
        {
            // Arrange
            _timedNode.StartTime = new DateTime(2023, 10, 1, 6, 0, 0);
            _timedNode.EndTime = new DateTime(2023, 10, 1, 7, 0, 0);
            var expectedToolTip = _timedNode.GetTimeAndDurationText();

            // Act
            var actualToolTip = _timedNode.ToolTip;

            // Assert
            Assert.Equal(expectedToolTip, actualToolTip);
        }

        /// <summary>
        /// Tests that the TypeName property returns "TimedNode".
        /// </summary>
        [Fact]
        public void TypeName_WhenAccessed_ReturnsTimedNode()
        {
            // Act
            var typeName = _timedNode.TypeName;

            // Assert
            Assert.Equal("TimedNode", typeName);
        }

        /// <summary>
        /// Tests that the auto-properties Id, NodeId, and Index can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void Properties_SetAndGet_ReturnExpectedValues()
        {
            // Arrange
            int expectedId = 10;
            int expectedNodeId = 20;
            int expectedIndex = 30;
            _timedNode.Id = expectedId;
            _timedNode.NodeId = expectedNodeId;
            _timedNode.Index = expectedIndex;

            // Act & Assert
            Assert.Equal(expectedId, _timedNode.Id);
            Assert.Equal(expectedNodeId, _timedNode.NodeId);
            Assert.Equal(expectedIndex, _timedNode.Index);
        }
    }
}
