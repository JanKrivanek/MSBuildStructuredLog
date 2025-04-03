using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ReaderSettings"/> class.
    /// </summary>
    public class ReaderSettingsTests
    {
        /// <summary>
        /// Tests that the static <see cref="ReaderSettings.Default"/> property returns a non-null instance.
        /// Also validates that the default UnknownDataBehavior is set to Warning.
        /// </summary>
        [Fact]
        public void Default_WhenAccessed_ReturnsInstanceWithUnknownDataBehaviorWarning()
        {
            // Act
            ReaderSettings defaultSettings = ReaderSettings.Default;

            // Assert
            defaultSettings.Should().NotBeNull("Default should always return a non-null instance");
            defaultSettings.UnknownDataBehavior.Should().Be(UnknownDataBehavior.Warning, 
                "the default UnknownDataBehavior should be Warning");
        }

        /// <summary>
        /// Tests that the UnknownDataBehavior property can be set to a new value and returns the updated value.
        /// </summary>
        [Fact]
        public void UnknownDataBehavior_SetValue_UpdatesPropertyCorrectly()
        {
            // Arrange
            var settings = new ReaderSettings();
            UnknownDataBehavior newValue = UnknownDataBehavior.Warning; // Currently only Warning is known, so reusing it for demonstration.
            
            // Act
            settings.UnknownDataBehavior = newValue;

            // Assert
            settings.UnknownDataBehavior.Should().Be(newValue, "the UnknownDataBehavior property should return the value it was set to");
        }

        /// <summary>
        /// Tests that modifying the UnknownDataBehavior property on the static Default instance reflects the change.
        /// This test verifies that Default returns a mutable instance.
        /// </summary>
        [Fact]
        public void Default_ModifyUnknownDataBehavior_ReflectsUpdatedValue()
        {
            // Arrange
            ReaderSettings defaultSettings = ReaderSettings.Default;
            // Saving the original value to restore later if needed.
            UnknownDataBehavior originalValue = defaultSettings.UnknownDataBehavior;
            UnknownDataBehavior newValue = UnknownDataBehavior.Warning; // Using Warning as the only known value.

            // Act
            defaultSettings.UnknownDataBehavior = newValue;

            // Assert
            defaultSettings.UnknownDataBehavior.Should().Be(newValue, "modifying the UnknownDataBehavior property on the Default instance should reflect the updated value");

            // Clean up: Reset to original value if needed
            defaultSettings.UnknownDataBehavior = originalValue;
        }
    }
}
