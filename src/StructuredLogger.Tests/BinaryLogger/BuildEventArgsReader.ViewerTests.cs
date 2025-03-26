using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        /// </summary>
        public BuildEventArgsReaderTests()
        {
            _reader = new BuildEventArgsReader();
        }

        /// <summary>
        /// Sets the private field "stringRecords" of the BuildEventArgsReader instance using reflection.
        /// </summary>
        /// <param name="records">The collection to set as the stringRecords value.</param>
        private void SetStringRecords(object records)
        {
            FieldInfo field = typeof(BuildEventArgsReader).GetField("stringRecords", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
            {
                throw new Exception("The field 'stringRecords' was not found on BuildEventArgsReader.");
            }
            field.SetValue(_reader, records);
        }

        /// <summary>
        /// Tests the GetStrings method when the underlying stringRecords contains a mix of strings and non-string objects.
        /// Expected outcome: Only string records are returned.
        /// </summary>
        [Fact]
        public void GetStrings_WithMixedTypeRecords_ReturnsOnlyStringValues()
        {
            // Arrange
            // Create a list with strings and non-string objects.
            var records = new ArrayList() { "Hello", 123, "World", null, new object() };
            SetStringRecords(records);

            // Act
            IEnumerable<string> result = _reader.GetStrings();

            // Assert
            string[] expected = new string[] { "Hello", "World" };
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the GetStrings method when the underlying stringRecords is not set.
        /// Expected outcome: A NullReferenceException is thrown.
        /// </summary>
        [Fact]
        public void GetStrings_WithNullStringRecords_ThrowsNullReferenceException()
        {
            // Arrange
            // Explicitly set the stringRecords field to null.
            SetStringRecords(null);

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _reader.GetStrings());
        }

        /// <summary>
        /// Tests the FormatResourceStringIgnoreCodeAndKeyword method overload with one argument.
        /// Expected outcome: The returned string is correctly formatted.
        /// </summary>
        [Fact]
        public void FormatResourceStringIgnoreCodeAndKeyword_OneArgument_ReturnsFormattedString()
        {
            // Arrange
            string resource = "Project \"{0}\" (default targets):";
            string arg0 = "MyProject";
            string expected = string.Format(resource, arg0);

            // Act
            string result = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arg0);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the FormatResourceStringIgnoreCodeAndKeyword method overload with two arguments.
        /// Expected outcome: The returned string is correctly formatted with two substitution values.
        /// </summary>
        [Fact]
        public void FormatResourceStringIgnoreCodeAndKeyword_TwoArguments_ReturnsFormattedString()
        {
            // Arrange
            string resource = "Project \"{0}\" ({1} target(s)):";
            string arg0 = "MyProject";
            string arg1 = "5";
            string expected = string.Format(resource, arg0, arg1);

            // Act
            string result = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arg0, arg1);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the FormatResourceStringIgnoreCodeAndKeyword method overload with three arguments.
        /// Expected outcome: The returned string is correctly formatted with three substitution values.
        /// </summary>
        [Fact]
        public void FormatResourceStringIgnoreCodeAndKeyword_ThreeArguments_ReturnsFormattedString()
        {
            // Arrange
            string resource = "Done building target \"{0}\" in project \"{1}\".";
            string arg0 = "Target1";
            string arg1 = "MyProject";
            string expected = string.Format(resource, arg0, arg1);

            // Act
            string result = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arg0, arg1);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the FormatResourceStringIgnoreCodeAndKeyword method overload with params array.
        /// Expected outcome: The returned string is correctly formatted with a variable number of substitution values.
        /// </summary>
        [Fact]
        public void FormatResourceStringIgnoreCodeAndKeyword_ParamsArray_ReturnsFormattedString()
        {
            // Arrange
            string resource = "Concat: {0}{1}{2}";
            string[] arguments = new string[] { "A", "B", "C" };
            string expected = string.Format(resource, arguments);

            // Act
            string result = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arguments);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
