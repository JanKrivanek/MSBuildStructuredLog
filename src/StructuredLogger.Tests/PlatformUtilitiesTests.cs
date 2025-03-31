using FluentAssertions;
using System;
using System.Reflection;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="PlatformUtilities"/> class.
    /// </summary>
    public class PlatformUtilitiesTests
    {
        private readonly bool _isBrowser;
        private readonly bool _isWasi;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformUtilitiesTests"/> class.
        /// Uses reflection to extract the private static fields from the <see cref="PlatformUtilities"/> class.
        /// </summary>
        public PlatformUtilitiesTests()
        {
            // Retrieve the private static fields _isBrowser and _isWasi via reflection.
            FieldInfo? browserField = typeof(PlatformUtilities).GetField("_isBrowser", BindingFlags.NonPublic | BindingFlags.Static);
            FieldInfo? wasiField = typeof(PlatformUtilities).GetField("_isWasi", BindingFlags.NonPublic | BindingFlags.Static);

            if (browserField == null) throw new InvalidOperationException("Could not find _isBrowser field.");
            if (wasiField == null) throw new InvalidOperationException("Could not find _isWasi field.");

            _isBrowser = (bool)browserField.GetValue(null)!;
            _isWasi = (bool)wasiField.GetValue(null)!;
        }

        /// <summary>
        /// Tests the <see cref="PlatformUtilities.HasThreads"/> property to ensure it returns the expected value.
        /// The expected value is computed as !(_isBrowser && _isWasi) i.e. both _isBrowser and _isWasi must be false.
        /// </summary>
        [Fact]
        public void HasThreads_WhenCalled_ReturnsExpectedValue()
        {
            // Arrange
            bool expected = !_isBrowser && !_isWasi;

            // Act
            bool actual = PlatformUtilities.HasThreads;

            // Assert
            actual.Should().Be(expected, "HasThreads should be true only when both _isBrowser and _isWasi are false.");
        }

        /// <summary>
        /// Tests the <see cref="PlatformUtilities.HasTempStorage"/> property to ensure it returns the expected value.
        /// The expected value is computed as !_isWasi.
        /// </summary>
        [Fact]
        public void HasTempStorage_WhenCalled_ReturnsExpectedValue()
        {
            // Arrange
            bool expected = !_isWasi;

            // Act
            bool actual = PlatformUtilities.HasTempStorage;

            // Assert
            actual.Should().Be(expected, "HasTempStorage should be true if _isWasi is false.");
        }

        /// <summary>
        /// Tests the <see cref="PlatformUtilities.HasColor"/> property to ensure it returns the expected value.
        /// The expected value is computed as !_isWasi.
        /// </summary>
        [Fact]
        public void HasColor_WhenCalled_ReturnsExpectedValue()
        {
            // Arrange
            bool expected = !_isWasi;

            // Act
            bool actual = PlatformUtilities.HasColor;

            // Assert
            actual.Should().Be(expected, "HasColor should be true if _isWasi is false.");
        }
    }
}
