using System;
using System.Collections.Generic;
using Microsoft.Build.Logging;
using Xunit;

namespace Microsoft.Build.Logging.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedDataFields"/> class.
    /// </summary>
    public class ExtendedDataFieldsTests
    {
        /// <summary>
        /// Tests that a new instance of ExtendedDataFields initializes all properties to null.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_PropertiesAreNull()
        {
            // Arrange & Act
            var dataFields = new ExtendedDataFields();

            // Assert
            Assert.Null(dataFields.ExtendedType);
            Assert.Null(dataFields.ExtendedData);
            Assert.Null(dataFields.ExtendedMetadata);
        }

        /// <summary>
        /// Tests that the ExtendedType property can be set and retrieved correctly.
        /// Covers cases with null, empty, and non-empty string values.
        /// </summary>
        /// <param name="testValue">Test value for the ExtendedType property.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("TestType")]
        public void ExtendedType_SetAndGet_ReturnsSameValue(string testValue)
        {
            // Arrange
            var dataFields = new ExtendedDataFields();

            // Act
            dataFields.ExtendedType = testValue;

            // Assert
            Assert.Equal(testValue, dataFields.ExtendedType);
        }

        /// <summary>
        /// Tests that the ExtendedData property can be set and retrieved correctly.
        /// Checks various scenarios including null, empty, whitespace, and typical string.
        /// </summary>
        /// <param name="testValue">Test value for the ExtendedData property.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Some extended data")]
        public void ExtendedData_SetAndGet_ReturnsSameValue(string testValue)
        {
            // Arrange
            var dataFields = new ExtendedDataFields();

            // Act
            dataFields.ExtendedData = testValue;

            // Assert
            Assert.Equal(testValue, dataFields.ExtendedData);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property correctly returns null when set to null.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_SetToNull_ReturnsNull()
        {
            // Arrange
            var dataFields = new ExtendedDataFields();

            // Act
            dataFields.ExtendedMetadata = null;

            // Assert
            Assert.Null(dataFields.ExtendedMetadata);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property can be set and retrieved correctly.
        /// Uses a dictionary with sample key-value pairs.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_SetAndGet_ReturnsSameDictionary()
        {
            // Arrange
            var dataFields = new ExtendedDataFields();
            var testDictionary = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

            // Act
            dataFields.ExtendedMetadata = testDictionary;

            // Assert
            Assert.Equal(testDictionary, dataFields.ExtendedMetadata);
            Assert.Equal("Value1", dataFields.ExtendedMetadata["Key1"]);
            Assert.Equal("Value2", dataFields.ExtendedMetadata["Key2"]);
        }
    }
}
