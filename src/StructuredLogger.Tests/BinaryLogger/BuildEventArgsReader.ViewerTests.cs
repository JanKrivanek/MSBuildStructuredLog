using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Build.Collections;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildEventArgsReader"/> class.
    /// </summary>
    public class BuildEventArgsReaderTests
    {
        private readonly BuildEventArgsReader _reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildEventArgsReaderTests"/> class.
        /// Uses reflection to instantiate the internal BuildEventArgsReader.
        /// </summary>
        public BuildEventArgsReaderTests()
        {
            // Assumes BuildEventArgsReader has a non-public default constructor.
            _reader = (BuildEventArgsReader)Activator.CreateInstance(typeof(BuildEventArgsReader), nonPublic: true);
        }

        /// <summary>
        /// Tests the GetStrings method to ensure it returns only string records.
        /// This test sets the private "stringRecords" field via reflection with a mixed list of objects.
        /// Expected outcome is that only string entries are returned by GetStrings.
        /// </summary>
        [Fact]
        public void GetStrings_WithMixedRecords_ReturnsOnlyStrings()
        {
            // Arrange
            FieldInfo fieldInfo = typeof(BuildEventArgsReader).GetField("stringRecords", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(fieldInfo);

            var mixedList = new List<object> { "First", 42, "Second", null, new object() };
            fieldInfo.SetValue(_reader, mixedList);

            // Act
            IEnumerable<string> result = _reader.GetStrings();

            // Assert
            List<string> resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            Assert.Contains("First", resultList);
            Assert.Contains("Second", resultList);
        }

        /// <summary>
        /// Tests the FormatResourceStringIgnoreCodeAndKeyword overload that accepts one argument.
        /// Verifies that the formatted string is returned correctly.
        /// </summary>
        /// <param name="resource">The string format resource.</param>
        /// <param name="arg0">The argument to format into the resource string.</param>
        /// <param name="expected">The expected output after formatting.</param>
        [Theory]
        [InlineData("Hello, {0}!", "World", "Hello, World!")]
        [InlineData("Item: {0}", "", "Item: ")]
        public void FormatResourceStringIgnoreCodeAndKeyword_OneArg_ReturnsFormattedString(string resource, string arg0, string expected)
        {
            // Act
            string actual = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arg0);

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the FormatResourceStringIgnoreCodeAndKeyword overload that accepts two arguments.
        /// Verifies that the formatted string is returned correctly.
        /// </summary>
        /// <param name="resource">The string format resource.</param>
        /// <param name="arg0">The first argument to format into the resource string.</param>
        /// <param name="arg1">The second argument to format into the resource string.</param>
        /// <param name="expected">The expected output after formatting.</param>
        [Theory]
        [InlineData("Coordinates: ({0}, {1})", "10", "20", "Coordinates: (10, 20)")]
        [InlineData("X: {0}, Y: {1}", "A", "B", "X: A, Y: B")]
        public void FormatResourceStringIgnoreCodeAndKeyword_TwoArgs_ReturnsFormattedString(string resource, string arg0, string arg1, string expected)
        {
            // Act
            string actual = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arg0, arg1);

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the FormatResourceStringIgnoreCodeAndKeyword overload that accepts three arguments.
        /// Verifies that the formatted string is returned correctly.
        /// </summary>
        /// <param name="resource">The string format resource.</param>
        /// <param name="arg0">The first argument to format into the resource string.</param>
        /// <param name="arg1">The second argument to format into the resource string.</param>
        /// <param name="arg2">The third argument to format into the resource string.</param>
        /// <param name="expected">The expected output after formatting.</param>
        [Theory]
        [InlineData("Triple: {0} - {1} - {2}", "One", "Two", "Three", "Triple: One - Two - Three")]
        [InlineData("{0} {1} {2}", "A", "B", "C", "A B C")]
        public void FormatResourceStringIgnoreCodeAndKeyword_ThreeArgs_ReturnsFormattedString(string resource, string arg0, string arg1, string arg2, string expected)
        {
            // Act
            string actual = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arg0, arg1, arg2);

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the FormatResourceStringIgnoreCodeAndKeyword overload that accepts a params array of arguments.
        /// Verifies that the formatted string is returned correctly.
        /// </summary>
        /// <param name="resource">The string format resource.</param>
        /// <param name="arg0">The first argument to format into the resource string.</param>
        /// <param name="arg1">The second argument to format into the resource string.</param>
        /// <param name="expected">The expected output after formatting.</param>
        [Theory]
        [InlineData("Greeting: {0} {1}", "Good", "Morning", "Greeting: Good Morning")]
        [InlineData("Sum: {0} + {1} = {2}", "2", "3", "5", "Sum: 2 + 3 = 5")]
        public void FormatResourceStringIgnoreCodeAndKeyword_Params_ReturnsFormattedString(string resource, string arg0, string arg1, string expected)
        {
            // Arrange
            string[] args = new string[] { arg0, arg1 };

            // Act
            string actual = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, args);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
