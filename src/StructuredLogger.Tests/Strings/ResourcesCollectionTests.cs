using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="StringsSet"/> class.
    /// </summary>
    public class StringsSetTests
    {
        private readonly string _validCulture = "en-US";
        private readonly Dictionary<string, Dictionary<string, string>> _testResourcesCollection;

        public StringsSetTests()
        {
            _testResourcesCollection = new Dictionary<string, Dictionary<string, string>>
            {
                { _validCulture, new Dictionary<string, string>
                    {
                        { "TestKey", "TestValue" },
                        { "AnotherKey", "AnotherValue" }
                    }
                }
            };
            SetStaticResourcesCollection(_testResourcesCollection);
        }

        /// <summary>
        /// Resets the internal static ResourcesCollection field of <see cref="StringsSet"/> using reflection.
        /// </summary>
        /// <param name="resources">The test resources dictionary to inject.</param>
        private static void SetStaticResourcesCollection(Dictionary<string, Dictionary<string, string>>? resources)
        {
            var field = typeof(StringsSet).GetField("resourcesCollection", BindingFlags.NonPublic | BindingFlags.Static);
            field.Should().NotBeNull("The static field 'resourcesCollection' should exist.");
            field!.SetValue(null, resources);
        }

        /// <summary>
        /// Tests that the constructor initializes a StringsSet instance correctly when provided with a valid culture key.
        /// It verifies that the Culture property is set and GetString returns the expected string for an existing key.
        /// </summary>
        [Fact]
        public void Constructor_ValidCulture_CreatesInstanceAndSetsCurrentSet()
        {
            // Arrange
            // _testResourcesCollection already contains a valid culture mapping (_validCulture)

            // Act
            var stringsSet = new StringsSet(_validCulture);

            // Assert
            stringsSet.Culture.Should().Be(_validCulture, "the Culture property should be set to the provided culture");
            string result = stringsSet.GetString("TestKey");
            result.Should().Be("TestValue", "GetString should return the corresponding value for an existing key");
        }

        /// <summary>
        /// Tests that the constructor throws a KeyNotFoundException when provided with an invalid culture key.
        /// This ensures that accessing the ResourcesCollection with a non-existent culture triggers an exception.
        /// </summary>
        [Fact]
        public void Constructor_InvalidCulture_ThrowsKeyNotFoundException()
        {
            // Arrange
            string invalidCulture = "fr-FR";

            // Act
            Action act = () => new StringsSet(invalidCulture);

            // Assert
            act.Should().Throw<KeyNotFoundException>("because the provided culture key does not exist in the resources collection");
        }

        /// <summary>
        /// Tests that GetString returns the correct string when a valid key exists in the current set.
        /// </summary>
        [Fact]
        public void GetString_ExistingKey_ReturnsExpectedValue()
        {
            // Arrange
            var stringsSet = new StringsSet(_validCulture);
            string key = "AnotherKey";
            string expectedValue = "AnotherValue";

            // Act
            string actualValue = stringsSet.GetString(key);

            // Assert
            actualValue.Should().Be(expectedValue, "because the key exists in the resource set and should return its corresponding value");
        }

        /// <summary>
        /// Tests that GetString returns an empty string when a non-existing key is requested.
        /// </summary>
        [Fact]
        public void GetString_NonExistingKey_ReturnsEmptyString()
        {
            // Arrange
            var stringsSet = new StringsSet(_validCulture);
            string nonExistingKey = "NonExistingKey";

            // Act
            string actualValue = stringsSet.GetString(nonExistingKey);

            // Assert
            actualValue.Should().BeEmpty("because the key does not exist and the method should return an empty string");
        }
    }
}
