using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="StringsSet"/> class.
    /// Note: The tests depend on the external resource "Strings.json" used to populate the static ResourcesCollection.
    /// Since ResourcesCollection cannot be directly set or mocked, these tests are marked as skipped.
    /// To enable the tests, modify the production code to allow injection of a test-specific ResourcesCollection.
    /// </summary>
    public class StringsSetTests
    {
        /// <summary>
        /// Tests that the constructor sets the Culture property correctly when a valid culture is provided.
        /// This test assumes that the static ResourcesCollection has an entry for the provided culture.
        /// </summary>
        [Fact(Skip = "Requires manual setup of ResourcesCollection to inject test data.")]
        public void Constructor_ValidCulture_SetsCultureProperty()
        {
            // Arrange
            var testCulture = "en-US";
            // For testing purposes, assume ResourcesCollection contains the key "en-US" with a valid dictionary.
            // e.g., ResourcesCollection["en-US"] = new Dictionary<string, string> { { "Greeting", "Hello" } };

            // Act
            var stringsSet = new StringsSet(testCulture);

            // Assert
            stringsSet.Culture.Should().Be(testCulture, "the constructor should set the Culture property to the provided culture.");
        }

        /// <summary>
        /// Tests that the constructor throws a KeyNotFoundException when an invalid culture is provided.
        /// This test assumes that the static ResourcesCollection does not contain the provided invalid culture.
        /// </summary>
        [Fact(Skip = "Requires manual setup of ResourcesCollection to inject test data.")]
        public void Constructor_InvalidCulture_ThrowsKeyNotFoundException()
        {
            // Arrange
            var invalidCulture = "invalid-culture";
            // The ResourcesCollection is assumed to not contain an entry for "invalid-culture".

            // Act
            Action act = () => new StringsSet(invalidCulture);

            // Assert
            act.Should().Throw<KeyNotFoundException>("the constructor should throw when the provided culture is not found in the ResourcesCollection.");
        }

        /// <summary>
        /// Tests that GetString returns the expected value when the key exists in the resource set.
        /// This test assumes that the static ResourcesCollection has been set up with the test culture and key.
        /// </summary>
        [Fact(Skip = "Requires manual setup of ResourcesCollection to inject test data.")]
        public void GetString_ExistingKey_ReturnsExpectedValue()
        {
            // Arrange
            var testCulture = "en-US";
            var testKey = "Greeting";
            var expectedValue = "Hello";
            // Assume ResourcesCollection["en-US"] contains { "Greeting", "Hello" }.
            var stringsSet = new StringsSet(testCulture);

            // Act
            var result = stringsSet.GetString(testKey);

            // Assert
            result.Should().Be(expectedValue, "GetString should return the expected value when the key exists in the resource set.");
        }

        /// <summary>
        /// Tests that GetString returns an empty string when the key does not exist in the resource set.
        /// This test assumes that the static ResourcesCollection has been set up with the test culture and that the key is absent.
        /// </summary>
        [Fact(Skip = "Requires manual setup of ResourcesCollection to inject test data.")]
        public void GetString_NonExistingKey_ReturnsEmptyString()
        {
            // Arrange
            var testCulture = "en-US";
            var nonExistingKey = "NonExistingKey"; // This key is not present in the resource dictionary.
            var stringsSet = new StringsSet(testCulture);

            // Act
            var result = stringsSet.GetString(nonExistingKey);

            // Assert
            result.Should().Be(string.Empty, "GetString should return an empty string when the key does not exist in the resource set.");
        }
    }
}
