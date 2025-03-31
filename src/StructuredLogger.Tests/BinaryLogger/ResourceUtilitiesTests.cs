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
        /// Tests the FormatResourceString method with valid inputs to ensure that it sets the output parameters correctly and returns the expected message.
        /// </summary>
        [Fact]
        public void FormatResourceString_WithValidInputs_ReturnsMessageAndSetsOutParameters()
        {
            // Arrange
            string text = "SampleText";
            string filePath = "C:\\dummy\\file.txt";
            string message = "TestMessage";

            // Act
            string result = ResourceUtilities.FormatResourceString(out string errorCode, out string helpKeyword, text, filePath, message);

            // Assert
            result.Should().Be(message);
            errorCode.Should().Be("MSB0001");
            helpKeyword.Should().BeEmpty();
        }

        /// <summary>
        /// Tests the FormatResourceString method with empty strings to ensure it handles edge cases.
        /// </summary>
        [Fact]
        public void FormatResourceString_WithEmptyStrings_ReturnsEmptyMessageAndSetsOutParameters()
        {
            // Arrange
            string text = string.Empty;
            string filePath = string.Empty;
            string message = string.Empty;

            // Act
            string result = ResourceUtilities.FormatResourceString(out string errorCode, out string helpKeyword, text, filePath, message);

            // Assert
            result.Should().BeEmpty();
            errorCode.Should().Be("MSB0001");
            helpKeyword.Should().BeEmpty();
        }

        /// <summary>
        /// Tests the FormatResourceStringStripCodeAndKeyword method with valid inputs to ensure that it sets the output parameters correctly and returns the expected message.
        /// </summary>
        [Fact]
        public void FormatResourceStringStripCodeAndKeyword_WithValidInputs_ReturnsMessageAndSetsOutParameters()
        {
            // Arrange
            string text = "AnotherText";
            string filePath = "C:\\path\\to\\file.txt";
            string message = "AnotherMessage";

            // Act
            string result = ResourceUtilities.FormatResourceStringStripCodeAndKeyword(out string errorCode, out string helpKeyword, text, filePath, message);

            // Assert
            result.Should().Be(message);
            errorCode.Should().Be("MSB0001");
            helpKeyword.Should().BeEmpty();
        }

        /// <summary>
        /// Tests the FormatResourceStringStripCodeAndKeyword method with empty strings to ensure it handles edge cases.
        /// </summary>
        [Fact]
        public void FormatResourceStringStripCodeAndKeyword_WithEmptyStrings_ReturnsEmptyMessageAndSetsOutParameters()
        {
            // Arrange
            string text = string.Empty;
            string filePath = string.Empty;
            string message = string.Empty;

            // Act
            string result = ResourceUtilities.FormatResourceStringStripCodeAndKeyword(out string errorCode, out string helpKeyword, text, filePath, message);

            // Assert
            result.Should().BeEmpty();
            errorCode.Should().Be("MSB0001");
            helpKeyword.Should().BeEmpty();
        }

        /// <summary>
        /// Tests the two-parameter FormatResourceString method to ensure that it returns the first argument.
        /// </summary>
        /// <param name="firstValue">The value to return.</param>
        /// <param name="secondValue">The value that is ignored.</param>
        [Theory]
        [InlineData("FirstValue", "SecondValue")]
        [InlineData("", "IgnoredValue")]
        [InlineData("LongStringExample", "AnotherValue")]
        public void FormatResourceString_TwoParameter_ReturnsFirstArgument(string firstValue, string secondValue)
        {
            // Act
            string result = ResourceUtilities.FormatResourceString(firstValue, secondValue);

            // Assert
            result.Should().Be(firstValue);
        }

        /// <summary>
        /// Tests the two-parameter FormatResourceStringStripCodeAndKeyword method to ensure that it returns the first argument.
        /// </summary>
        /// <param name="firstValue">The value to return.</param>
        /// <param name="secondValue">The value that is ignored.</param>
        [Theory]
        [InlineData("FirstValue", "SecondValue")]
        [InlineData("", "NonEmpty")]
        [InlineData("EdgeCaseString", "Ignored")]
        public void FormatResourceStringStripCodeAndKeyword_TwoParameter_ReturnsFirstArgument(string firstValue, string secondValue)
        {
            // Act
            string result = ResourceUtilities.FormatResourceStringStripCodeAndKeyword(firstValue, secondValue);

            // Assert
            result.Should().Be(firstValue);
        }

        /// <summary>
        /// Tests the GetResourceString method to ensure that it returns the provided input string.
        /// </summary>
        /// <param name="input">The resource string to be returned.</param>
        [Theory]
        [InlineData("ResourceStringValue")]
        [InlineData("")]
        [InlineData("AnotherResource")]
        public void GetResourceString_WhenCalled_ReturnsInputString(string input)
        {
            // Act
            string result = ResourceUtilities.GetResourceString(input);

            // Assert
            result.Should().Be(input);
        }
    }
}
