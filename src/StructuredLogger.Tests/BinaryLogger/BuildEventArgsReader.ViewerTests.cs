using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
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
        /// Uses reflection to create an instance of BuildEventArgsReader.
        /// </summary>
        public BuildEventArgsReaderTests()
        {
            // Create an instance via reflection, assuming a parameterless constructor exists (even if non-public).
            _reader = (BuildEventArgsReader)Activator.CreateInstance(typeof(BuildEventArgsReader), nonPublic: true);
        }

        /// <summary>
        /// Tests the GetStrings method when the internal stringRecords collection is empty.
        /// Expected outcome: An empty enumerable is returned.
        /// </summary>
        [Fact]
        public void GetStrings_WhenStringRecordsIsEmpty_ReturnsEmptyEnumerable()
        {
            // Arrange: Set the private field 'stringRecords' to an empty ArrayList.
            SetStringRecords(new ArrayList());

            // Act
            IEnumerable<string> result = _reader.GetStrings();

            // Assert
            result.Should().BeEmpty();
        }

        /// <summary>
        /// Tests the GetStrings method when the internal stringRecords collection contains only strings.
        /// Expected outcome: All strings in the collection are returned.
        /// </summary>
        [Fact]
        public void GetStrings_WhenStringRecordsContainsOnlyStrings_ReturnsAllStrings()
        {
            // Arrange: Set the private field 'stringRecords' to an ArrayList of strings.
            var stringList = new ArrayList { "Hello", "World" };
            SetStringRecords(stringList);

            // Act
            IEnumerable<string> result = _reader.GetStrings();

            // Assert
            result.Should().BeEquivalentTo(new List<string> { "Hello", "World" });
        }

        /// <summary>
        /// Tests the GetStrings method when the internal stringRecords collection contains mixed types.
        /// Expected outcome: Only string entries are returned.
        /// </summary>
        [Fact]
        public void GetStrings_WhenStringRecordsContainsMixedTypes_ReturnsOnlyStrings()
        {
            // Arrange: Set the private field 'stringRecords' to an ArrayList with mixed types.
            var mixedList = new ArrayList { "Hello", 123, "World", DateTime.Now };
            SetStringRecords(mixedList);
            var expectedStrings = mixedList.Cast<object>().OfType<string>().ToList();

            // Act
            IEnumerable<string> result = _reader.GetStrings();

            // Assert
            result.Should().BeEquivalentTo(expectedStrings);
        }

        /// <summary>
        /// Helper method to set the private field 'stringRecords' in BuildEventArgsReader for testing purposes.
        /// </summary>
        /// <param name="value">The value to assign to the stringRecords field.</param>
        private void SetStringRecords(object value)
        {
            FieldInfo field = typeof(BuildEventArgsReader).GetField("stringRecords", BindingFlags.Instance | BindingFlags.NonPublic);
            field.Should().NotBeNull("The private field 'stringRecords' must exist in BuildEventArgsReader.");
            field.SetValue(_reader, value);
        }

        /// <summary>
        /// Tests the one-argument overload of FormatResourceStringIgnoreCodeAndKeyword.
        /// Expected outcome: Returns a formatted string using a single argument.
        /// </summary>
        [Fact]
        public void FormatResourceStringIgnoreCodeAndKeyword_OneArgument_ReturnsFormattedString()
        {
            // Arrange
            string resource = "Hello {0}";
            string arg0 = "World";
            string expected = string.Format(resource, arg0);

            // Act
            string result = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arg0);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the two-argument overload of FormatResourceStringIgnoreCodeAndKeyword.
        /// Expected outcome: Returns a formatted string using two arguments.
        /// </summary>
        [Fact]
        public void FormatResourceStringIgnoreCodeAndKeyword_TwoArguments_ReturnsFormattedString()
        {
            // Arrange
            string resource = "{0} {1}";
            string arg0 = "Hello";
            string arg1 = "World";
            string expected = string.Format(resource, arg0, arg1);

            // Act
            string result = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arg0, arg1);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the three-argument overload of FormatResourceStringIgnoreCodeAndKeyword.
        /// Expected outcome: Returns a formatted string using three arguments.
        /// </summary>
        [Fact]
        public void FormatResourceStringIgnoreCodeAndKeyword_ThreeArguments_ReturnsFormattedString()
        {
            // Arrange
            string resource = "{0} {1} {2}";
            string arg0 = "One";
            string arg1 = "Two";
            string arg2 = "Three";
            string expected = string.Format(resource, arg0, arg1, arg2);

            // Act
            string result = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arg0, arg1, arg2);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the params overload of FormatResourceStringIgnoreCodeAndKeyword.
        /// Expected outcome: Returns a formatted string using the provided arguments array.
        /// </summary>
        [Fact]
        public void FormatResourceStringIgnoreCodeAndKeyword_ParamsOverload_ReturnsFormattedString()
        {
            // Arrange
            string resource = "{0} {1} {2} {3}";
            string[] args = new string[] { "This", "is", "a", "test" };
            string expected = string.Format(resource, args.Cast<object>().ToArray());

            // Act
            string result = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, args);

            // Assert
            result.Should().Be(expected);
        }
    }
}
