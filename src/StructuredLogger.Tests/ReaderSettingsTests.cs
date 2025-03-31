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
        /// Tests that the static Default property returns an instance with UnknownDataBehavior set to Warning.
        /// </summary>
        [Fact]
        public void DefaultProperty_ShouldReturnInstanceWithWarning()
        {
            // Act
            ReaderSettings defaultSettings = ReaderSettings.Default;

            // Assert
            defaultSettings.Should().NotBeNull("because the Default property should always return a non-null instance");
            defaultSettings.UnknownDataBehavior.Should().Be(UnknownDataBehavior.Warning, "because the default unknown data behavior is defined as Warning");
        }

        /// <summary>
        /// Tests that the UnknownDataBehavior property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void SetUnknownDataBehavior_PropertyShouldUpdateCorrectly()
        {
            // Arrange
            var settings = new ReaderSettings();

            // Determine an alternative enum value different from Warning.
            // Since the actual enum values for UnknownDataBehavior are not fully known,
            // we assume that adding 1 to the Warning value yields a valid alternative.
            UnknownDataBehavior alternativeValue = (UnknownDataBehavior)(((int)UnknownDataBehavior.Warning) + 1);

            // Act
            settings.UnknownDataBehavior = alternativeValue;

            // Assert
            settings.UnknownDataBehavior.Should().Be(alternativeValue, "because the property setter should correctly update the value");
        }

        /// <summary>
        /// Tests that the Default property always returns the same instance.
        /// </summary>
        [Fact]
        public void DefaultProperty_ShouldReturnSingletonInstance()
        {
            // Act
            ReaderSettings firstInstance = ReaderSettings.Default;
            ReaderSettings secondInstance = ReaderSettings.Default;

            // Assert
            firstInstance.Should().BeSameAs(secondInstance, "because the Default property is expected to return a singleton instance");
        }
    }
}
