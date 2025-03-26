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
        /// Tests the FormatResourceString(out string, out string, string, string, string) method with valid inputs.
        /// Verifies that it returns the message parameter and sets the errorCode to "MSB0001" and helpKeyword to an empty string.
        /// </summary>
        /// <param name="text">The resource text.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="message">The message to be returned.</param>
        [Theory]
        [InlineData("SampleText", "C:\\path\\file.txt", "Test Message")]
        [InlineData("", "", "")]
        [InlineData(null, null, null)]
        public void FormatResourceString_HappyPath_ReturnsMessageAndSetsOutputs(string text, string filePath, string message)
        {
            // Act
            string result = ResourceUtilities.FormatResourceString(out string errorCode, out string helpKeyword, text, filePath, message);

            // Assert
            Assert.Equal("MSB0001", errorCode);
            Assert.Equal(string.Empty, helpKeyword);
            Assert.Equal(message, result);
        }

        /// <summary>
        /// Tests the FormatResourceStringStripCodeAndKeyword(out string, out string, string, string, string) method with valid inputs.
        /// Verifies that it returns the message parameter and sets the errorCode to "MSB0001" and helpKeyword to an empty string.
        /// </summary>
        /// <param name="text">The resource text.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="message">The message to be returned.</param>
        [Theory]
        [InlineData("Manifest", "D:\\files\\resource.txt", "Another Test Message")]
        [InlineData("", "", "")]
        [InlineData(null, null, null)]
        public void FormatResourceStringStripCodeAndKeyword_HappyPath_ReturnsMessageAndSetsOutputs(string text, string filePath, string message)
        {
            // Act
            string result = ResourceUtilities.FormatResourceStringStripCodeAndKeyword(out string errorCode, out string helpKeyword, text, filePath, message);

            // Assert
            Assert.Equal("MSB0001", errorCode);
            Assert.Equal(string.Empty, helpKeyword);
            Assert.Equal(message, result);
        }

        /// <summary>
        /// Tests the FormatResourceString(string, string) method ensuring it returns the first parameter.
        /// Covers normal inputs along with edge cases where the input might be empty or null.
        /// </summary>
        /// <param name="v1">The first string value.</param>
        /// <param name="v2">The second string value.</param>
        [Theory]
        [InlineData("FirstValue", "SecondValue")]
        [InlineData("", "AnyValue")]
        [InlineData(null, "AnyValue")]
        public void FormatResourceString_TwoParameters_ReturnsFirstParameter(string v1, string v2)
        {
            // Act
            string result = ResourceUtilities.FormatResourceString(v1, v2);

            // Assert
            Assert.Equal(v1, result);
        }

        /// <summary>
        /// Tests the FormatResourceStringStripCodeAndKeyword(string, string) method ensuring it returns the first parameter.
        /// Covers normal inputs along with edge cases where the input might be empty or null.
        /// </summary>
        /// <param name="v1">The first string value.</param>
        /// <param name="v2">The second string value.</param>
        [Theory]
        [InlineData("FirstValue", "SecondValue")]
        [InlineData("", "AnotherValue")]
        [InlineData(null, "AnotherValue")]
        public void FormatResourceStringStripCodeAndKeyword_TwoParameters_ReturnsFirstParameter(string v1, string v2)
        {
            // Act
            string result = ResourceUtilities.FormatResourceStringStripCodeAndKeyword(v1, v2);

            // Assert
            Assert.Equal(v1, result);
        }

        /// <summary>
        /// Tests the GetResourceString method to ensure it returns the input string unmodified.
        /// This covers both standard inputs and edge cases such as empty and null values.
        /// </summary>
        /// <param name="input">The input string which may be a normal string, empty, or null.</param>
        [Theory]
        [InlineData("Resource")]
        [InlineData("")]
        [InlineData(null)]
        public void GetResourceString_HappyPath_ReturnsInput(string input)
        {
            // Act
            string result = ResourceUtilities.GetResourceString(input);

            // Assert
            Assert.Equal(input, result);
        }
    }
}
