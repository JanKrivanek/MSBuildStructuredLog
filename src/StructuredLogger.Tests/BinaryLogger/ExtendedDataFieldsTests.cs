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
        private readonly ExtendedDataFields _extendedDataFields;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedDataFieldsTests"/> class.
        /// </summary>
        public ExtendedDataFieldsTests()
        {
            _extendedDataFields = new ExtendedDataFields();
        }

        /// <summary>
        /// Tests that a newly created instance of ExtendedDataFields has null property values by default.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_PropertiesAreInitiallyNull()
        {
            // Arrange
            var instance = new ExtendedDataFields();

            // Act & Assert
            Assert.Null(instance.ExtendedType);
            Assert.Null(instance.ExtendedMetadata);
            Assert.Null(instance.ExtendedData);
        }

        /// <summary>
        /// Tests that the ExtendedType property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void ExtendedType_SetValidString_ReturnsSameString()
        {
            // Arrange
            string expectedValue = "SampleType";

            // Act
            _extendedDataFields.ExtendedType = expectedValue;
            string actualValue = _extendedDataFields.ExtendedType;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        /// <summary>
        /// Tests that the ExtendedType property correctly handles a null assignment.
        /// </summary>
        [Fact]
        public void ExtendedType_SetToNull_ReturnsNull()
        {
            // Arrange
            _extendedDataFields.ExtendedType = "InitialValue";

            // Act
            _extendedDataFields.ExtendedType = null;
            string actualValue = _extendedDataFields.ExtendedType;

            // Assert
            Assert.Null(actualValue);
        }

        /// <summary>
        /// Tests that the ExtendedData property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void ExtendedData_SetValidString_ReturnsSameString()
        {
            // Arrange
            string expectedValue = "SampleData";

            // Act
            _extendedDataFields.ExtendedData = expectedValue;
            string actualValue = _extendedDataFields.ExtendedData;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        /// <summary>
        /// Tests that the ExtendedData property correctly handles a null assignment.
        /// </summary>
        [Fact]
        public void ExtendedData_SetToNull_ReturnsNull()
        {
            // Arrange
            _extendedDataFields.ExtendedData = "InitialData";

            // Act
            _extendedDataFields.ExtendedData = null;
            string actualValue = _extendedDataFields.ExtendedData;

            // Assert
            Assert.Null(actualValue);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property can be set with a non-null dictionary and returns the same dictionary.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_SetValidDictionary_ReturnsSameDictionary()
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
            Assert.NotNull(actualDictionary);
            Assert.Equal(expectedDictionary, actualDictionary);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property correctly handles a null assignment.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_SetToNull_ReturnsNull()
        {
            // Arrange
            _extendedDataFields.ExtendedMetadata = new Dictionary<string, string>
            {
                { "TestKey", "TestValue" }
            };

            // Act
            _extendedDataFields.ExtendedMetadata = null;
            IDictionary<string, string> actualDictionary = _extendedDataFields.ExtendedMetadata;

            // Assert
            Assert.Null(actualDictionary);
        }

        /// <summary>
        /// Tests that modifications to the dictionary retrieved via ExtendedMetadata are reflected in the stored object.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_ModifyDictionary_ReflectsChanges()
        {
            // Arrange
            var metadataDictionary = new Dictionary<string, string>();
            _extendedDataFields.ExtendedMetadata = metadataDictionary;

            // Act
            _extendedDataFields.ExtendedMetadata["NewKey"] = "NewValue";

            // Assert
            Assert.True(_extendedDataFields.ExtendedMetadata.ContainsKey("NewKey"));
            Assert.Equal("NewValue", _extendedDataFields.ExtendedMetadata["NewKey"]);
        }
    }
}
