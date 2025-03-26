using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="StringsSet"/> class.
    /// </summary>
    public class StringsSetTests
    {
        private readonly Dictionary<string, Dictionary<string, string>> _testResourcesCollection;
        private const string ValidCulture = "en-US";
        private const string InvalidCulture = "fr-FR";
        private const string TestKey = "TestKey";
        private const string TestValue = "TestValue";

        /// <summary>
        /// Initializes a new instance of the <see cref="StringsSetTests"/> class and sets up a test resources collection.
        /// </summary>
        public StringsSetTests()
        {
            // Arrange a dummy resource collection.
            _testResourcesCollection = new Dictionary<string, Dictionary<string, string>>
            {
                { ValidCulture, new Dictionary<string, string> { { TestKey, TestValue } } }
            };

            // Set the private static field "resourcesCollection" to the test collection.
            var field = typeof(StringsSet).GetField("resourcesCollection", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, _testResourcesCollection);
        }

        /// <summary>
        /// Tests that the constructor sets the Culture property correctly and initializes the current set from the resource collection.
        /// </summary>
        [Fact]
        public void Constructor_ValidCulture_InitializesCurrentSetAndSetsCultureProperty()
        {
            // Act
            var stringsSet = new StringsSet(ValidCulture);

            // Assert
            Assert.Equal(ValidCulture, stringsSet.Culture);
            string result = stringsSet.GetString(TestKey);
            Assert.Equal(TestValue, result);
        }

        /// <summary>
        /// Tests that GetString returns the correct value when the key exists in the resource set.
        /// </summary>
        [Fact]
        public void GetString_ExistingKey_ReturnsCorrectValue()
        {
            // Arrange
            var stringsSet = new StringsSet(ValidCulture);

            // Act
            string actual = stringsSet.GetString(TestKey);

            // Assert
            Assert.Equal(TestValue, actual);
        }

        /// <summary>
        /// Tests that GetString returns an empty string when the key does not exist in the resource set.
        /// </summary>
        [Fact]
        public void GetString_NonExistingKey_ReturnsEmptyString()
        {
            // Arrange
            var stringsSet = new StringsSet(ValidCulture);
            const string nonExistingKey = "NonExistingKey";

            // Act
            string actual = stringsSet.GetString(nonExistingKey);

            // Assert
            Assert.Equal(string.Empty, actual);
        }

        /// <summary>
        /// Tests that the constructor throws a KeyNotFoundException when an invalid culture is provided and not present in the resource collection.
        /// </summary>
        [Fact]
        public void Constructor_InvalidCulture_ThrowsKeyNotFoundException()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => new StringsSet(InvalidCulture));
        }

        /// <summary>
        /// Tests that the ResourcesCollection property returns the same instance that was set, ensuring lazy initialization doesn't override an already set value.
        /// </summary>
        [Fact]
        public void ResourcesCollection_WhenAlreadyInitialized_ReturnsSameInstance()
        {
            // Act
            var resourcesCollection = StringsSet.ResourcesCollection;

            // Assert
            Assert.Same(_testResourcesCollection, resourcesCollection);
        }
    }
}
