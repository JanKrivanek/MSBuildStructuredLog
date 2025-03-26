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
        /// Verifies that the Default property returns an instance of ReaderSettings 
        /// with the UnknownDataBehavior property set to Warning.
        /// </summary>
        [Fact]
        public void Default_WhenAccessed_ReturnsInstanceWithWarning()
        {
            // Act
            ReaderSettings defaultInstance = ReaderSettings.Default;

            // Assert
            Assert.NotNull(defaultInstance);
            Assert.Equal(UnknownDataBehavior.Warning, defaultInstance.UnknownDataBehavior);
        }

        /// <summary>
        /// Verifies that the UnknownDataBehavior property getter and setter behave as expected.
        /// </summary>
        [Fact]
        public void UnknownDataBehavior_SetterGetter_StoresAssignedValue()
        {
            // Arrange
            ReaderSettings settings = new ReaderSettings();

            // Act
            // Assuming UnknownDataBehavior enum has an alternative value 'Error'
            settings.UnknownDataBehavior = UnknownDataBehavior.Error;
            UnknownDataBehavior result = settings.UnknownDataBehavior;

            // Assert
            Assert.Equal(UnknownDataBehavior.Error, result);
        }
    }
}
