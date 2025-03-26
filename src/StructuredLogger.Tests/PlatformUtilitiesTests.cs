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
        private readonly FieldInfo _isBrowserField;
        private readonly FieldInfo _isWasiField;
        private readonly bool _originalIsBrowser;
        private readonly bool _originalIsWasi;

        public PlatformUtilitiesTests()
        {
            _isBrowserField = typeof(PlatformUtilities).GetField("_isBrowser", BindingFlags.NonPublic | BindingFlags.Static);
            _isWasiField = typeof(PlatformUtilities).GetField("_isWasi", BindingFlags.NonPublic | BindingFlags.Static);
            if (_isBrowserField == null || _isWasiField == null)
            {
                throw new Exception("Required fields not found via reflection.");
            }

            _originalIsBrowser = (bool)_isBrowserField.GetValue(null);
            _originalIsWasi = (bool)_isWasiField.GetValue(null);
        }

        /// <summary>
        /// Restores the original values of the private fields in PlatformUtilities.
        /// </summary>
        private void RestoreOriginalValues()
        {
            _isBrowserField.SetValue(null, _originalIsBrowser);
            _isWasiField.SetValue(null, _originalIsWasi);
        }

        /// <summary>
        /// Sets the private fields _isBrowser and _isWasi to the specified values.
        /// </summary>
        /// <param name="isBrowser">The value to set for _isBrowser.</param>
        /// <param name="isWasi">The value to set for _isWasi.</param>
        private void SetPlatformValues(bool isBrowser, bool isWasi)
        {
            _isBrowserField.SetValue(null, isBrowser);
            _isWasiField.SetValue(null, isWasi);
        }

        /// <summary>
        /// Tests PlatformUtilities properties in a normal environment scenario where neither BROWSER nor WASI are detected.
        /// Expected outcome: HasThreads, HasTempStorage, and HasColor are all true.
        /// </summary>
        [Fact]
        public void Properties_NormalEnvironment_ReturnExpectedValues()
        {
            // Arrange
            SetPlatformValues(isBrowser: false, isWasi: false);

            // Act
            bool hasThreads = PlatformUtilities.HasThreads;
            bool hasTempStorage = PlatformUtilities.HasTempStorage;
            bool hasColor = PlatformUtilities.HasColor;

            // Assert
            Assert.True(hasThreads, "Expected HasThreads to be true when _isBrowser and _isWasi are false.");
            Assert.True(hasTempStorage, "Expected HasTempStorage to be true when _isWasi is false.");
            Assert.True(hasColor, "Expected HasColor to be true when _isWasi is false.");

            // Cleanup
            RestoreOriginalValues();
        }

        /// <summary>
        /// Tests PlatformUtilities properties when WASI is detected.
        /// Expected outcome: HasThreads is false and both HasTempStorage and HasColor are false.
        /// </summary>
        [Fact]
        public void Properties_WhenWasiIsTrue_ReturnExpectedValues()
        {
            // Arrange
            SetPlatformValues(isBrowser: false, isWasi: true);

            // Act
            bool hasThreads = PlatformUtilities.HasThreads;
            bool hasTempStorage = PlatformUtilities.HasTempStorage;
            bool hasColor = PlatformUtilities.HasColor;

            // Assert
            Assert.False(hasThreads, "Expected HasThreads to be false when _isWasi is true.");
            Assert.False(hasTempStorage, "Expected HasTempStorage to be false when _isWasi is true.");
            Assert.False(hasColor, "Expected HasColor to be false when _isWasi is true.");

            // Cleanup
            RestoreOriginalValues();
        }

        /// <summary>
        /// Tests PlatformUtilities properties when BROWSER is detected but WASI is not.
        /// Expected outcome: HasThreads is false while HasTempStorage and HasColor remain true.
        /// </summary>
        [Fact]
        public void Properties_WhenBrowserIsTrue_ReturnExpectedValues()
        {
            // Arrange
            SetPlatformValues(isBrowser: true, isWasi: false);

            // Act
            bool hasThreads = PlatformUtilities.HasThreads;
            bool hasTempStorage = PlatformUtilities.HasTempStorage;
            bool hasColor = PlatformUtilities.HasColor;

            // Assert
            Assert.False(hasThreads, "Expected HasThreads to be false when _isBrowser is true.");
            Assert.True(hasTempStorage, "Expected HasTempStorage to be true when _isWasi is false.");
            Assert.True(hasColor, "Expected HasColor to be true when _isWasi is false.");

            // Cleanup
            RestoreOriginalValues();
        }

        /// <summary>
        /// Tests PlatformUtilities properties when both BROWSER and WASI are detected.
        /// Expected outcome: All properties report false.
        /// </summary>
        [Fact]
        public void Properties_WhenBrowserAndWasiAreTrue_ReturnExpectedValues()
        {
            // Arrange
            SetPlatformValues(isBrowser: true, isWasi: true);

            // Act
            bool hasThreads = PlatformUtilities.HasThreads;
            bool hasTempStorage = PlatformUtilities.HasTempStorage;
            bool hasColor = PlatformUtilities.HasColor;

            // Assert
            Assert.False(hasThreads, "Expected HasThreads to be false when both _isBrowser and _isWasi are true.");
            Assert.False(hasTempStorage, "Expected HasTempStorage to be false when _isWasi is true.");
            Assert.False(hasColor, "Expected HasColor to be false when _isWasi is true.");

            // Cleanup
            RestoreOriginalValues();
        }
    }
}
