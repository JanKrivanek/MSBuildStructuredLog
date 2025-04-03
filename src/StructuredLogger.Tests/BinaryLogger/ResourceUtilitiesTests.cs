using FluentAssertions;
using Microsoft.Build.Shared;
using Xunit;

namespace Microsoft.Build.Shared.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ResourceUtilities"/> class.
    /// </summary>
    public class ResourceUtilitiesTests
    {
        /// <summary>
        /// Tests that FormatResourceString returns the provided message and sets out parameters correctly when using valid inputs.
        /// </summary>
        [Fact]
        public void FormatResourceString_WithValidInputs_ReturnsMessage_AndSetsOutParameters()
        {
            // Arrange
            string text = "Sample text";
            string filePath = "C:\\temp\\file.txt";
            string message = "Test Message";

            // Act
            string result = ResourceUtilities.FormatResourceString(out string errorCode, out string helpKeyword, text, filePath, message);

            // Assert
            result.Should().Be(message, "the method returns the message parameter as the result");
            errorCode.Should().Be("MSB0001", "the error code is hardcoded to 'MSB0001'");
            helpKeyword.Should().Be(string.Empty, "the help keyword is hardcoded to an empty string");
        }

        /// <summary>
        /// Tests that FormatResourceString correctly handles empty string inputs.
        /// </summary>
        [Fact]
        public void FormatResourceString_WithEmptyStrings_ReturnsEmptyMessage_AndSetsDefaults()
        {
            // Arrange
            string text = string.Empty;
            string filePath = string.Empty;
            string message = string.Empty;

            // Act
            string result = ResourceUtilities.FormatResourceString(out string errorCode, out string helpKeyword, text, filePath, message);

            // Assert
            result.Should().BeEmpty("the method returns the message parameter even if it is empty");
            errorCode.Should().Be("MSB0001", "the error code should remain 'MSB0001'");
            helpKeyword.Should().BeEmpty("the help keyword is expected to be empty");
        }

        /// <summary>
        /// Tests that FormatResourceStringStripCodeAndKeyword returns the provided message and sets out parameters correctly when using valid inputs.
        /// </summary>
        [Fact]
        public void FormatResourceStringStripCodeAndKeyword_WithValidInputs_ReturnsMessage_AndSetsOutParameters()
        {
            // Arrange
            string text = "Another sample";
            string filePath = "/usr/local/file.txt";
            string message = "Strip Test Message";

            // Act
            string result = ResourceUtilities.FormatResourceStringStripCodeAndKeyword(out string errorCode, out string helpKeyword, text, filePath, message);

            // Assert
            result.Should().Be(message, "the method returns the message parameter as the result");
            errorCode.Should().Be("MSB0001", "the error code is hardcoded to 'MSB0001'");
            helpKeyword.Should().Be(string.Empty, "the help keyword is hardcoded to an empty string");
        }

        /// <summary>
        /// Tests that FormatResourceStringStripCodeAndKeyword correctly handles edge-case inputs with empty strings.
        /// </summary>
        [Fact]
        public void FormatResourceStringStripCodeAndKeyword_WithEmptyStrings_ReturnsEmptyMessage_AndSetsDefaults()
        {
            // Arrange
            string text = string.Empty;
            string filePath = string.Empty;
            string message = string.Empty;

            // Act
            string result = ResourceUtilities.FormatResourceStringStripCodeAndKeyword(out string errorCode, out string helpKeyword, text, filePath, message);

            // Assert
            result.Should().BeEmpty("the method should return an empty message when provided with an empty message");
            errorCode.Should().Be("MSB0001", "the error code should be 'MSB0001' regardless of input");
            helpKeyword.Should().BeEmpty("the help keyword is expected to be an empty string");
        }

        /// <summary>
        /// Tests that the overload of FormatResourceString (with two string parameters) returns the first parameter.
        /// </summary>
        /// <param name="v1">First string parameter which should be returned.</param>
        /// <param name="v2">Second string parameter.</param>
        [Theory]
        [InlineData("First", "Second")]
        [InlineData("", "NonEmpty")]
        [InlineData("Longer string value", "Another value")]
        public void FormatResourceString_WithTwoStrings_ReturnsFirstParameter(string v1, string v2)
        {
            // Act
            string result = ResourceUtilities.FormatResourceString(v1, v2);

            // Assert
            result.Should().Be(v1, "the method returns the first string parameter regardless of the second parameter");
        }

        /// <summary>
        /// Tests that the overload of FormatResourceStringStripCodeAndKeyword (with two string parameters) returns the first parameter.
        /// </summary>
        /// <param name="v1">First string parameter which should be returned.</param>
        /// <param name="v2">Second string parameter.</param>
        [Theory]
        [InlineData("Alpha", "Beta")]
        [InlineData("", "Gamma")]
        [InlineData("EdgeCase", "")]
        public void FormatResourceStringStripCodeAndKeyword_WithTwoStrings_ReturnsFirstParameter(string v1, string v2)
        {
            // Act
            string result = ResourceUtilities.FormatResourceStringStripCodeAndKeyword(v1, v2);

            // Assert
            result.Should().Be(v1, "the method returns the first parameter regardless of the second parameter");
        }

        /// <summary>
        /// Tests that GetResourceString returns the same string that is provided.
        /// </summary>
        /// <param name="input">The input string to be returned.</param>
        [Theory]
        [InlineData("Resource Content")]
        [InlineData("")]
        [InlineData("Special characters !@#$%^&*()")]
        public void GetResourceString_WithVariousInputs_ReturnsSameString(string input)
        {
            // Act
            string result = ResourceUtilities.GetResourceString(input);

            // Assert
            result.Should().Be(input, "the method returns the input string without modification");
        }
    }
}
