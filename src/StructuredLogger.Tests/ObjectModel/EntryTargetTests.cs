using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="EntryTarget"/> class.
    /// </summary>
    public class EntryTargetTests
    {
        /// <summary>
        /// Tests that the TypeName property returns the expected class name.
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsExpectedTypeName()
        {
            // Arrange
            var entryTarget = new EntryTarget();

            // Act
            string typeName = entryTarget.TypeName;

            // Assert
            typeName.Should().Be(nameof(EntryTarget));
        }

        /// <summary>
        /// Tests that the Target property returns null when there is no parent project available.
        /// Expected behavior: without a parent project, the Target property should return null.
        /// </summary>
        [Fact]
        public void Target_WhenNoParentProject_ReturnsNull()
        {
            // Arrange
            var entryTarget = new EntryTarget();

            // Act
            var target = entryTarget.Target;

            // Assert
            target.Should().BeNull("because without a parent project, no target can be resolved");
        }

        /// <summary>
        /// Tests that the IsLowRelevance property returns true when there is no target available.
        /// Expected behavior: without an associated target, IsLowRelevance should default to true.
        /// </summary>
        [Fact]
        public void IsLowRelevance_WhenNoTarget_ReturnsTrue()
        {
            // Arrange
            var entryTarget = new EntryTarget();

            // Act
            bool isLowRelevance = entryTarget.IsLowRelevance;

            // Assert
            isLowRelevance.Should().BeTrue("because in absence of a target, the default relevance is low (true)");
        }

        /// <summary>
        /// Tests that the DurationText property returns null when there is no target available.
        /// Expected behavior: without an associated target, DurationText should be null.
        /// </summary>
        [Fact]
        public void DurationText_WhenNoTarget_ReturnsNull()
        {
            // Arrange
            var entryTarget = new EntryTarget();

            // Act
            var durationText = entryTarget.DurationText;

            // Assert
            durationText.Should().BeNull("because without an associated target there is no duration text");
        }

        // The following tests are partial and indicate areas where further integration tests could be developed 
        // if the surrounding infrastructure (such as Project and Target hierarchy) could be simulated.
        // Due to the inability to set or mock internal dependencies as documented, additional tests 
        // verifying the behavior when a parent project exists and returns a valid target cannot be generated 
        // without further modifications or test hooks in the production code.
    }
}
