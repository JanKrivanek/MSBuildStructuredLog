using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System.Runtime.InteropServices;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="PlatformUtilities"/> class.
    /// </summary>
    public class PlatformUtilitiesTests
    {
        /// <summary>
        /// Tests the <see cref="PlatformUtilities.HasThreads"/> property to ensure it returns the correct value
        /// based on the underlying OS platform. The expected value is calculated as not being on a "BROWSER" or "WASI" platform.
        /// </summary>
        [Fact]
        public void HasThreads_WhenCalled_ReturnsExpectedValue()
        {
            // Arrange
            bool expected = !RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER"))
                             && !RuntimeInformation.IsOSPlatform(OSPlatform.Create("WASI"));

            // Act
            bool actual = PlatformUtilities.HasThreads;

            // Assert
            actual.Should().Be(expected, "because HasThreads is defined as !_isBrowser && !_isWasi");
        }

        /// <summary>
        /// Tests the <see cref="PlatformUtilities.HasTempStorage"/> property to ensure it returns the correct value
        /// based on the underlying OS platform. The expected value is calculated as not being on a "WASI" platform.
        /// </summary>
        [Fact]
        public void HasTempStorage_WhenCalled_ReturnsExpectedValue()
        {
            // Arrange
            bool expected = !RuntimeInformation.IsOSPlatform(OSPlatform.Create("WASI"));

            // Act
            bool actual = PlatformUtilities.HasTempStorage;

            // Assert
            actual.Should().Be(expected, "because HasTempStorage is defined as !_isWasi");
        }

        /// <summary>
        /// Tests the <see cref="PlatformUtilities.HasColor"/> property to ensure it returns the correct value
        /// based on the underlying OS platform. The expected value is calculated as not being on a "WASI" platform.
        /// </summary>
        [Fact]
        public void HasColor_WhenCalled_ReturnsExpectedValue()
        {
            // Arrange
            bool expected = !RuntimeInformation.IsOSPlatform(OSPlatform.Create("WASI"));

            // Act
            bool actual = PlatformUtilities.HasColor;

            // Assert
            actual.Should().Be(expected, "because HasColor is defined as !_isWasi");
        }
    }
}
