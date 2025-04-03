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
        /// Tests the ExtendedType property getter and setter with a typical string value.
        /// </summary>
        [Fact]
        public void ExtendedType_SetTypicalValue_GetReturnsSameValue()
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
        /// Tests the ExtendedType property getter and setter with an empty string.
        /// </summary>
        [Fact]
        public void ExtendedType_SetEmptyString_GetReturnsEmptyString()
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
        /// Tests the ExtendedType property getter and setter with a string containing special characters.
        /// </summary>
        [Fact]
        public void ExtendedType_SetSpecialCharacters_GetReturnsSameValue()
        {
            // Arrange
            string expectedValue = "!@#$%^&*()_+-=[]{}|;':,./<>?";

            // Act
            _extendedDataFields.ExtendedType = expectedValue;
            string actualValue = _extendedDataFields.ExtendedType;

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests the ExtendedType property getter and setter with a very long string.
        /// </summary>
        [Fact]
        public void ExtendedType_SetVeryLongString_GetReturnsSameValue()
        {
            // Arrange
            string expectedValue = new string('A', 10000);

            // Act
            _extendedDataFields.ExtendedType = expectedValue;
            string actualValue = _extendedDataFields.ExtendedType;

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests the ExtendedData property getter and setter with a typical string value.
        /// </summary>
        [Fact]
        public void ExtendedData_SetTypicalValue_GetReturnsSameValue()
        {
            // Arrange
            string expectedValue = "SampleData";

            // Act
            _extendedDataFields.ExtendedData = expectedValue;
            string actualValue = _extendedDataFields.ExtendedData;

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests the ExtendedData property getter and setter with an empty string.
        /// </summary>
        [Fact]
        public void ExtendedData_SetEmptyString_GetReturnsEmptyString()
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
        /// Tests the ExtendedData property getter and setter with a very long string.
        /// </summary>
        [Fact]
        public void ExtendedData_SetVeryLongString_GetReturnsSameValue()
        {
            // Arrange
            string expectedValue = new string('B', 10000);

            // Act
            _extendedDataFields.ExtendedData = expectedValue;
            string actualValue = _extendedDataFields.ExtendedData;

            // Assert
            actualValue.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests the ExtendedMetadata property getter and setter with a typical dictionary.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_SetTypicalDictionary_GetReturnsEquivalentDictionary()
        {
            // Arrange
            var expectedDictionary = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

            // Act
            _extendedDataFields.ExtendedMetadata = expectedDictionary;
            var actualDictionary = _extendedDataFields.ExtendedMetadata;

            // Assert
            actualDictionary.Should().BeEquivalentTo(expectedDictionary);
        }

        /// <summary>
        /// Tests the ExtendedMetadata property getter and setter with an empty dictionary.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_SetEmptyDictionary_GetReturnsEmptyDictionary()
        {
            // Arrange
            var expectedDictionary = new Dictionary<string, string>();

            // Act
            _extendedDataFields.ExtendedMetadata = expectedDictionary;
            var actualDictionary = _extendedDataFields.ExtendedMetadata;

            // Assert
            actualDictionary.Should().BeEmpty();
        }

        /// <summary>
        /// Tests the ExtendedMetadata property getter and setter with a dictionary containing special characters.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_SetDictionaryWithSpecialCharacters_GetReturnsEquivalentDictionary()
        {
            // Arrange
            var expectedDictionary = new Dictionary<string, string>
            {
                { "!@#$", "^&*()" },
                { "中文键", "值" }
            };

            // Act
            _extendedDataFields.ExtendedMetadata = expectedDictionary;
            var actualDictionary = _extendedDataFields.ExtendedMetadata;

            // Assert
            actualDictionary.Should().BeEquivalentTo(expectedDictionary);
        }

        /// <summary>
        /// Tests the ExtendedMetadata property getter and setter with a null value.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_SetNull_GetReturnsNull()
        {
            // Arrange
            IDictionary<string, string>? expectedDictionary = null;

            // Act
            _extendedDataFields.ExtendedMetadata = expectedDictionary;
            var actualDictionary = _extendedDataFields.ExtendedMetadata;

            // Assert
            actualDictionary.Should().BeNull();
        }
    }
}
