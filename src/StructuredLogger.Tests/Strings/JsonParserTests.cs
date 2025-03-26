using System;
using System.Collections.Generic;
using TinyJson;
using Xunit;

namespace TinyJson.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="JSONParser"/> class.
    /// </summary>
    public class JSONParserTests
    {
        /// <summary>
        /// A helper class used for testing object deserialization.
        /// </summary>
        private class TestObject
        {
            public int Value;
            public string Name { get; set; }
        }

        /// <summary>
        /// A helper enum used for testing enum deserialization.
        /// </summary>
        private enum TestEnum
        {
            Zero,
            One
        }

        /// <summary>
        /// Tests that the FromJson extension method correctly parses a JSON string with escaped characters.
        /// Input JSON contains an escaped newline character.
        /// Expected outcome is that the escape sequence is correctly interpreted.
        /// </summary>
        [Fact]
        public void FromJson_WithEscapedCharacters_ParsesCorrectly()
        {
            // Arrange
            string json = "\"Line\\nBreak\"";

            // Act
            var result = json.FromJson<string>();

            // Assert
            Assert.Equal("Line\nBreak", result);
        }

        /// <summary>
        /// Tests that the FromJson extension method correctly parses a valid JSON string.
        /// Expected outcome is the unescaped string returned.
        /// </summary>
        [Fact]
        public void FromJson_WithValidString_ReturnsUnescapedString()
        {
            // Arrange
            string json = "\"Hello World\"";

            // Act
            var result = json.FromJson<string>();

            // Assert
            Assert.Equal("Hello World", result);
        }

        /// <summary>
        /// Tests that the FromJson extension method correctly handles an empty JSON string.
        /// Expected outcome is an empty string.
        /// </summary>
        [Fact]
        public void FromJson_WithEmptyStringJson_ReturnsEmptyString()
        {
            // Arrange
            string json = "\"\"";

            // Act
            var result = json.FromJson<string>();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        /// <summary>
        /// Tests that the FromJson extension method correctly parses a JSON representing an integer.
        /// Expected outcome is the integer value.
        /// </summary>
        [Fact]
        public void FromJson_WithPrimitiveInt_ParsesCorrectly()
        {
            // Arrange
            string json = "123";

            // Act
            var result = json.FromJson<int>();

            // Assert
            Assert.Equal(123, result);
        }

        /// <summary>
        /// Tests that the FromJson extension method correctly parses a JSON representing a decimal number.
        /// Expected outcome is the decimal value.
        /// </summary>
        [Fact]
        public void FromJson_WithDecimal_ParsesCorrectly()
        {
            // Arrange
            string json = "123.45";

            // Act
            var result = json.FromJson<decimal>();

            // Assert
            Assert.Equal(123.45m, result);
        }

        /// <summary>
        /// Tests that the FromJson extension method returns null when the JSON token is 'null'.
        /// Expected outcome is a null object.
        /// </summary>
        [Fact]
        public void FromJson_WithNullJson_ReturnsNull()
        {
            // Arrange
            string json = "null";

            // Act
            var result = json.FromJson<object>();

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that the FromJson extension method correctly parses a JSON array into an integer array.
        /// Expected outcome is an array with the correct integer elements.
        /// </summary>
        [Fact]
        public void FromJson_WithArray_ReturnsIntArray()
        {
            // Arrange
            string json = "[1,2,3]";

            // Act
            var result = json.FromJson<int[]>();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(new int[] { 1, 2, 3 }, result);
        }

        /// <summary>
        /// Tests that the FromJson extension method correctly parses a JSON array into a List of integers.
        /// Expected outcome is a List with the correct integer elements.
        /// </summary>
        [Fact]
        public void FromJson_WithList_ReturnsListOfInt()
        {
            // Arrange
            string json = "[1,2,3]";

            // Act
            var result = json.FromJson<List<int>>();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(new List<int> { 1, 2, 3 }, result);
        }

        /// <summary>
        /// Tests that the FromJson extension method correctly parses a JSON object into a Dictionary.
        /// Expected outcome is a Dictionary with the correct key-value pair.
        /// </summary>
        [Fact]
        public void FromJson_WithDictionary_ReturnsDictionary()
        {
            // Arrange
            string json = "{\"Key\":123}";

            // Act
            var result = json.FromJson<Dictionary<string, object>>();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ContainsKey("Key"));
            Assert.Equal(123, result["Key"]);
        }

        /// <summary>
        /// Tests that the FromJson extension method correctly parses a JSON object into a custom object.
        /// Expected outcome is that the object's fields and properties are properly set.
        /// </summary>
        [Fact]
        public void FromJson_WithCustomObject_ReturnsCorrectObject()
        {
            // Arrange
            string json = "{\"Value\":10,\"Name\":\"Test\"}";

            // Act
            var result = json.FromJson<TestObject>();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Value);
            Assert.Equal("Test", result.Name);
        }

        /// <summary>
        /// Tests that the FromJson extension method throws a FormatException when provided with an
        /// invalid JSON representation for a primitive type.
        /// Expected outcome is a FormatException.
        /// </summary>
        [Fact]
        public void FromJson_WithInvalidPrimitiveJson_ThrowsFormatException()
        {
            // Arrange
            string json = "abc";

            // Act & Assert
            Assert.Throws<FormatException>(() => json.FromJson<int>());
        }

        /// <summary>
        /// Tests that the FromJson extension method correctly parses an empty JSON array.
        /// Expected outcome is an empty collection (array or list).
        /// </summary>
        [Fact]
        public void FromJson_WithEmptyArray_ReturnsEmptyArrayOrList()
        {
            // Arrange
            string jsonArray = "[]";

            // Act
            var arrayResult = jsonArray.FromJson<int[]>();
            var listResult = jsonArray.FromJson<List<int>>();

            // Assert
            Assert.NotNull(arrayResult);
            Assert.Empty(arrayResult);
            Assert.NotNull(listResult);
            Assert.Empty(listResult);
        }

        /// <summary>
        /// Tests that the FromJson extension method correctly parses a JSON string into an enum value.
        /// Expected outcome is the correct enum value.
        /// </summary>
        [Fact]
        public void FromJson_WithEnumString_ReturnsEnumValue()
        {
            // Arrange
            string json = "\"One\"";

            // Act
            var result = json.FromJson<TestEnum>();

            // Assert
            Assert.Equal(TestEnum.One, result);
        }

        /// <summary>
        /// Tests that the FromJson extension method returns the default enum value when an invalid enum string is provided.
        /// Expected outcome is the default enum, in this case TestEnum.Zero.
        /// </summary>
        [Fact]
        public void FromJson_WithInvalidEnumString_ReturnsDefaultEnum()
        {
            // Arrange
            string json = "\"Invalid\"";

            // Act
            var result = json.FromJson<TestEnum>();

            // Assert
            Assert.Equal(TestEnum.Zero, result);
        }

        /// <summary>
        /// Tests the internal ParseValue method to ensure it correctly parses a JSON string for a string type.
        /// Expected outcome is the unescaped string.
        /// </summary>
        [Fact]
        public void ParseValue_ForString_ReturnsUnescapedString()
        {
            // Arrange
            string inputJson = "\"Hello\\nWorld\"";

            // Act
            object result = JSONParser.ParseValue(typeof(string), inputJson);

            // Assert
            Assert.IsType<string>(result);
            Assert.Equal("Hello\nWorld", (string)result);
        }

        /// <summary>
        /// Tests the internal ParseValue method to ensure it correctly parses a JSON string for an integer type.
        /// Expected outcome is the correct integer value.
        /// </summary>
        [Fact]
        public void ParseValue_ForInt_ReturnsCorrectInteger()
        {
            // Arrange
            string inputJson = "456";

            // Act
            object result = JSONParser.ParseValue(typeof(int), inputJson);

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(456, (int)result);
        }

        /// <summary>
        /// Tests the internal ParseValue method for the handling of a JSON 'null' value.
        /// Expected outcome is a null result.
        /// </summary>
        [Fact]
        public void ParseValue_WithNullJson_ReturnsNull()
        {
            // Arrange
            string inputJson = "null";

            // Act
            object result = JSONParser.ParseValue(typeof(object), inputJson);

            // Assert
            Assert.Null(result);
        }
    }
}
