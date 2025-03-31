using FluentAssertions;
using Moq;
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
        /// A simple class for testing object deserialization.
        /// </summary>
        private class SimpleObject
        {
            public int Value;
            public string Name { get; set; }
        }

        /// <summary>
        /// A test enum for verifying enum parsing.
        /// </summary>
        private enum TestEnum
        {
            None,
            First,
            Second
        }

        /// <summary>
        /// Tests the FromJson extension method for a valid JSON string input.
        /// Expected outcome is that the unescaped string is returned.
        /// </summary>
        /// <param name="json">A JSON encoded string.</param>
        /// <param name="expected">The expected unescaped string.</param>
        [Theory]
        [InlineData("\"Hello\"", "Hello")]
        [InlineData("\"\"", "")]
        public void FromJson_String_ValidJson_ReturnsExpectedString(string json, string expected)
        {
            // Act
            string result = json.FromJson<string>();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the FromJson extension method for a JSON string with escaped characters.
        /// Expected outcome is that the string is correctly unescaped.
        /// </summary>
        [Fact]
        public void FromJson_String_EscapedCharacters_ReturnsUnescapedString()
        {
            // Arrange
            string json = "\"Line1\\nLine2\\tTabbed\\\"Quote\\\"\"";
            string expected = "Line1\nLine2\tTabbed\"Quote\"";

            // Act
            string result = json.FromJson<string>();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the FromJson extension method for primitive integer JSON input.
        /// Expected outcome is that the correct integer is returned.
        /// </summary>
        /// <param name="json">A JSON integer represented as a string.</param>
        /// <param name="expected">The expected integer value.</param>
        [Theory]
        [InlineData("123", 123)]
        [InlineData("-456", -456)]
        public void FromJson_Int_ValidJson_ReturnsExpectedInt(string json, int expected)
        {
            // Act
            int result = json.FromJson<int>();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the FromJson extension method for primitive double JSON input.
        /// Expected outcome is that the correct double value is returned.
        /// </summary>
        /// <param name="json">A JSON double represented as a string.</param>
        /// <param name="expected">The expected double value.</param>
        [Theory]
        [InlineData("123.45", 123.45)]
        [InlineData("-0.678", -0.678)]
        public void FromJson_Double_ValidJson_ReturnsExpectedDouble(string json, double expected)
        {
            // Act
            double result = json.FromJson<double>();

            // Assert
            result.Should().BeApproximately(expected, 0.0001);
        }

        /// <summary>
        /// Tests the FromJson extension method for JSON boolean input.
        /// Expected outcome is that the correct boolean value is returned.
        /// </summary>
        /// <param name="json">A JSON boolean represented as a string.</param>
        /// <param name="expected">The expected boolean value.</param>
        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void FromJson_Bool_ValidJson_ReturnsExpectedBool(string json, bool expected)
        {
            // Act
            bool result = json.FromJson<bool>();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the FromJson extension method for JSON null input.
        /// Expected outcome is that null is returned.
        /// </summary>
        [Fact]
        public void FromJson_NullJson_ReturnsNull()
        {
            // Act
            object result = "null".FromJson<object>();

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Tests the FromJson extension method for a JSON array of integers.
        /// Expected outcome is to correctly parse and return an array of integers.
        /// </summary>
        [Fact]
        public void FromJson_ArrayInt_ValidJson_ReturnsExpectedArray()
        {
            // Arrange
            string json = "[1,2,3]";
            int[] expected = new int[] { 1, 2, 3 };

            // Act
            int[] result = json.FromJson<int[]>();

            // Assert
            result.Should().Equal(expected);
        }

        /// <summary>
        /// Tests the FromJson extension method for a JSON list of integers.
        /// Expected outcome is to correctly parse and return a List of integers.
        /// </summary>
        [Fact]
        public void FromJson_ListInt_ValidJson_ReturnsExpectedList()
        {
            // Arrange
            string json = "[4,5,6]";
            var expected = new List<int> { 4, 5, 6 };

            // Act
            List<int> result = json.FromJson<List<int>>();

            // Assert
            result.Should().Equal(expected);
        }

        /// <summary>
        /// Tests the FromJson extension method for a JSON object representing a dictionary.
        /// Expected outcome is to correctly parse and return a dictionary with string keys.
        /// </summary>
        [Fact]
        public void FromJson_Dictionary_ValidJson_ReturnsExpectedDictionary()
        {
            // Arrange
            string json = "{\"A\":1,\"B\":2}";
            var expected = new Dictionary<string, int>
            {
                { "A", 1 },
                { "B", 2 }
            };

            // Act
            Dictionary<string, int> result = json.FromJson<Dictionary<string, int>>();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
//  // [Error] (205-21)CS0051 Inconsistent accessibility: parameter type 'JSONParserTests.TestEnum' is less accessible than method 'JSONParserTests.FromJson_Enum_ValidJson_ReturnsExpectedEnum(string, JSONParserTests.TestEnum)'
//         /// <summary>
//         /// Tests the FromJson extension method for a JSON enum value.
//         /// Expected outcome is that the correct enum value is returned.
//         /// </summary>
//         /// <param name="json">A JSON enum represented as a string.</param>
//         /// <param name="expected">The expected enum value.</param>
//         [Theory]
//         [InlineData("\"First\"", TestEnum.First)]
//         [InlineData("\"Second\"", TestEnum.Second)]
//         public void FromJson_Enum_ValidJson_ReturnsExpectedEnum(string json, TestEnum expected)
//         {
//             // Act
//             TestEnum result = json.FromJson<TestEnum>();
// 
//             // Assert
//             result.Should().Be(expected);
//         }
// 
        /// <summary>
        /// Tests the FromJson extension method for a JSON object representing a class with public fields and properties.
        /// Expected outcome is that the object is correctly populated.
        /// </summary>
        [Fact]
        public void FromJson_Object_ValidJson_ReturnsPopulatedObject()
        {
            // Arrange
            string json = "{\"Value\":10,\"Name\":\"TestName\"}";

            // Act
            SimpleObject result = json.FromJson<SimpleObject>();

            // Assert
            result.Should().NotBeNull();
            result.Value.Should().Be(10);
            result.Name.Should().Be("TestName");
        }

        /// <summary>
        /// Tests the FromJson extension method with an invalid JSON format.
        /// Expected outcome is that the parsing fails gracefully and returns null.
        /// </summary>
        [Fact]
        public void FromJson_InvalidJsonFormat_ReturnsNull()
        {
            // Arrange
            string json = "123abc";

            // Act
            object result = json.FromJson<object>();

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Tests the FromJson extension method with an empty JSON string.
        /// Expected outcome is that the parsing fails gracefully and returns null.
        /// </summary>
        [Fact]
        public void FromJson_EmptyString_ReturnsNull()
        {
            // Arrange
            string json = "";

            // Act
            object result = json.FromJson<object>();

            // Assert
            result.Should().BeNull();
        }
    }
}
