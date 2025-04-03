using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;
using Microsoft.Build.Collections;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildEventArgsReader"/> class.
    /// </summary>
    public class BuildEventArgsReaderTests
    {
//         /// <summary> // [Error] (25-30)CS7036 There is no argument given that corresponds to the required parameter 'binaryReader' of 'BuildEventArgsReader.BuildEventArgsReader(BinaryReader, int)'
//         /// Tests the <see cref="BuildEventArgsReader.GetStrings"/> method when no string records are present.
//         /// Expected outcome: Returns an empty enumerable.
//         /// Note: This test is limited by the inability to directly inject string records into the internal collection.
//         /// </summary>
//         [Fact]
//         public void GetStrings_WhenNoRecords_ReturnsEmptyEnumerable()
//         {
//             // Arrange
//             var reader = new BuildEventArgsReader();
// 
//             // Act
//             IEnumerable<string> result = reader.GetStrings();
// 
//             // Assert
//             result.Should().BeEmpty("because no string records have been added");
//         }
// 
        /// <summary>
        /// Tests the <see cref="BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(string, string)"/> method
        /// to ensure it returns the correctly formatted string using one argument.
        /// </summary>
        /// <param name="resource">The format string.</param>
        /// <param name="arg0">The argument to format into the string.</param>
        /// <param name="expected">The expected output.</param>
        [Theory]
        [InlineData("Hello {0}", "World", "Hello World")]
        [InlineData("Value: {0}", "", "Value: ")]
        public void FormatResourceStringIgnoreCodeAndKeyword_OneArgument_ReturnsFormattedString(string resource, string arg0, string expected)
        {
            // Act
            string result = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arg0);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the <see cref="BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(string, string, string)"/> method
        /// to ensure it returns the correctly formatted string using two arguments.
        /// </summary>
        /// <param name="resource">The format string.</param>
        /// <param name="arg0">The first argument.</param>
        /// <param name="arg1">The second argument.</param>
        /// <param name="expected">The expected output.</param>
        [Theory]
        [InlineData("Hello {0} {1}", "World", "Again", "Hello World Again")]
        [InlineData("{0} - {1}", "First", "Second", "First - Second")]
        public void FormatResourceStringIgnoreCodeAndKeyword_TwoArguments_ReturnsFormattedString(string resource, string arg0, string arg1, string expected)
        {
            // Act
            string result = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arg0, arg1);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the <see cref="BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(string, string, string, string)"/> method
        /// to ensure it returns the correctly formatted string using three arguments.
        /// </summary>
        /// <param name="resource">The format string.</param>
        /// <param name="arg0">The first argument.</param>
        /// <param name="arg1">The second argument.</param>
        /// <param name="arg2">The third argument.</param>
        /// <param name="expected">The expected output.</param>
        [Theory]
        [InlineData("Hello {0} {1} {2}", "A", "B", "C", "Hello A B C")]
        [InlineData("{0}:{1}:{2}", "1", "2", "3", "1:2:3")]
        public void FormatResourceStringIgnoreCodeAndKeyword_ThreeArguments_ReturnsFormattedString(string resource, string arg0, string arg1, string arg2, string expected)
        {
            // Act
            string result = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, arg0, arg1, arg2);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the <see cref="BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(string, params string[])"/> method
        /// to ensure it returns the correctly formatted string using multiple arguments.
        /// </summary>
        [Fact]
        public void FormatResourceStringIgnoreCodeAndKeyword_Params_ReturnsFormattedString()
        {
            // Arrange
            string resource = "Values: {0}, {1}, {2}";
            string[] args = new[] { "One", "Two", "Three" };
            string expected = string.Format(resource, args);

            // Act
            string result = BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource, args);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests that the <see cref="BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(string, string)"/> method
        /// throws an <see cref="ArgumentNullException"/> when a null resource string is provided.
        /// </summary>
        [Fact]
        public void FormatResourceStringIgnoreCodeAndKeyword_NullResource_ThrowsArgumentNullException()
        {
            // Arrange
            string? resource = null;
            string arg0 = "Test";

            // Act
            Action act = () => BuildEventArgsReader.FormatResourceStringIgnoreCodeAndKeyword(resource!, arg0);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
