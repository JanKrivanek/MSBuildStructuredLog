using FluentAssertions;
using Microsoft.Build.Logging;
using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Build.Logging.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedDataFields"/> class.
    /// </summary>
    public class ExtendedDataFieldsTests
    {
        private readonly ExtendedDataFields _extendedDataFields;

        public ExtendedDataFieldsTests()
        {
            _extendedDataFields = new ExtendedDataFields();
        }

        /// <summary>
        /// Tests that the ExtendedType property can be set and retrieved correctly with a typical non-empty string value.
        /// </summary>
        [Fact]
        public void ExtendedType_SetToNonEmptyString_ReturnsSameString()
        {
            // Arrange
            string expectedValue = "TestType";

            // Act
            _extendedDataFields.ExtendedType = expectedValue;
            string actualValue = _extendedDataFields.ExtendedType;

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests that the ExtendedType property can be set to an empty string and retrieved correctly.
        /// </summary>
        [Fact]
        public void ExtendedType_SetToEmptyString_ReturnsEmptyString()
        {
            // Arrange
            string expectedValue = string.Empty;

            // Act
            _extendedDataFields.ExtendedType = expectedValue;
            string actualValue = _extendedDataFields.ExtendedType;

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests that the ExtendedData property can be set and retrieved correctly with a typical non-empty string value.
        /// </summary>
        [Fact]
        public void ExtendedData_SetToNonEmptyString_ReturnsSameString()
        {
            // Arrange
            string expectedValue = "Some extended data";

            // Act
            _extendedDataFields.ExtendedData = expectedValue;
            string actualValue = _extendedDataFields.ExtendedData;

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests that the ExtendedData property can be set to an empty string and retrieved correctly.
        /// </summary>
        [Fact]
        public void ExtendedData_SetToEmptyString_ReturnsEmptyString()
        {
            // Arrange
            string expectedValue = string.Empty;

            // Act
            _extendedDataFields.ExtendedData = expectedValue;
            string actualValue = _extendedDataFields.ExtendedData;

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property can be set and retrieved correctly when assigned a populated dictionary.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_SetToPopulatedDictionary_ReturnsSameDictionary()
        {
            // Arrange
            var expectedDictionary = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

            // Act
            _extendedDataFields.ExtendedMetadata = expectedDictionary;
            IDictionary<string, string> actualDictionary = _extendedDataFields.ExtendedMetadata;

            // Assert
            actualDictionary.Should().BeEquivalentTo(expectedDictionary);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property can be set and retrieved correctly when assigned an empty dictionary.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_SetToEmptyDictionary_ReturnsEmptyDictionary()
        {
            // Arrange
            var expectedDictionary = new Dictionary<string, string>();

            // Act
            _extendedDataFields.ExtendedMetadata = expectedDictionary;
            IDictionary<string, string> actualDictionary = _extendedDataFields.ExtendedMetadata;

            // Assert
            actualDictionary.Should().BeEmpty();
        }

        /// <summary>
        /// Tests the default state of a newly instantiated ExtendedDataFields object
        /// to ensure that its properties have their default values.
        /// </summary>
        [Fact]
        public void Constructor_DefaultInstance_PropertiesAreNullByDefault()
        {
            // Arrange
            var instance = new ExtendedDataFields();

            // Act & Assert
            instance.ExtendedType.Should().BeNull();
            instance.ExtendedData.Should().BeNull();
            instance.ExtendedMetadata.Should().BeNull();
        }
    }
}
