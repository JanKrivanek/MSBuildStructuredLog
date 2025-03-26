using Microsoft.Build.Shared;
using Moq;
using System;
using Xunit;

namespace Microsoft.Build.Shared.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ResourceUtilities"/> class.
    /// </summary>
    public class ResourceUtilitiesTests
    {
        /// <summary>
        /// Tests the FormatResourceString(out string, out string, string, string, string) method with valid non-null inputs.
        /// Expected outcome: The method returns the provided message, errorCode is "MSB0001", and helpKeyword is an empty string.
        /// </summary>
        [Theory]
        [InlineData("text", "file.txt", "Test message")]
        [InlineData("", "", "")]
        public void FormatResourceString_WithValidInput_ReturnsMessageAndSetsOutParams(string text, string filePath, string message)
        {
            // Arrange
            string expectedErrorCode = "MSB0001";
            string expectedHelpKeyword = "";
            
            // Act
            string result = ResourceUtilities.FormatResourceString(out string errorCode, out string helpKeyword, text, filePath, message);
            
            // Assert
            Assert.Equal(expectedErrorCode, errorCode);
            Assert.Equal(expectedHelpKeyword, helpKeyword);
            Assert.Equal(message, result);
        }

        /// <summary>
        /// Tests the FormatResourceString(out string, out string, string, string, string) method with null values for text and filePath.
        /// Expected outcome: The method returns the provided message (which can be null) and correctly sets out parameters.
        /// </summary>
        [Fact]
        public void FormatResourceString_WithNullTextAndFilePath_ReturnsMessageAndSetsOutParams()
        {
            // Arrange
            string message = "Null test message";
            string expectedErrorCode = "MSB0001";
            string expectedHelpKeyword = "";
            
            // Act
            string result = ResourceUtilities.FormatResourceString(out string errorCode, out string helpKeyword, null, null, message);
            
            // Assert
            Assert.Equal(expectedErrorCode, errorCode);
            Assert.Equal(expectedHelpKeyword, helpKeyword);
            Assert.Equal(message, result);
        }

        /// <summary>
        /// Tests the FormatResourceStringStripCodeAndKeyword(out string, out string, string, string, string) method with valid inputs.
        /// Expected outcome: The method returns the provided message, errorCode is "MSB0001", and helpKeyword is an empty string.
        /// </summary>
        [Theory]
        [InlineData("resource", "path", "Another message")]
        [InlineData("", "", "")]
        public void FormatResourceStringStripCodeAndKeyword_WithValidInput_ReturnsMessageAndSetsOutParams(string text, string filePath, string message)
        {
            // Arrange
            string expectedErrorCode = "MSB0001";
            string expectedHelpKeyword = "";
            
            // Act
            string result = ResourceUtilities.FormatResourceStringStripCodeAndKeyword(out string errorCode, out string helpKeyword, text, filePath, message);
            
            // Assert
            Assert.Equal(expectedErrorCode, errorCode);
            Assert.Equal(expectedHelpKeyword, helpKeyword);
            Assert.Equal(message, result);
        }

        /// <summary>
        /// Tests the FormatResourceStringStripCodeAndKeyword(out string, out string, string, string, string) method with null parameters for text and filePath.
        /// Expected outcome: The method returns the provided message (which can be null) and sets out parameters accordingly.
        /// </summary>
        [Fact]
        public void FormatResourceStringStripCodeAndKeyword_WithNullTextAndFilePath_ReturnsMessageAndSetsOutParams()
        {
            // Arrange
            string message = null;
            string expectedErrorCode = "MSB0001";
            string expectedHelpKeyword = "";
            
            // Act
            string result = ResourceUtilities.FormatResourceStringStripCodeAndKeyword(out string errorCode, out string helpKeyword, null, null, message);
            
            // Assert
            Assert.Equal(expectedErrorCode, errorCode);
            Assert.Equal(expectedHelpKeyword, helpKeyword);
            Assert.Null(result);
        }

        /// <summary>
        /// Tests the FormatResourceString(string, string) method with various non-null inputs.
        /// Expected outcome: The method returns the first string parameter.
        /// </summary>
        [Theory]
        [InlineData("First", "Second", "First")]
        [InlineData("", "Anything", "")]
        public void FormatResourceString_WithValidInputs_ReturnsFirstParameter(string v1, string v2, string expected)
        {
            // Act
            string result = ResourceUtilities.FormatResourceString(v1, v2);
            
            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the FormatResourceString(string, string) method when the first parameter is null.
        /// Expected outcome: The method returns null.
        /// </summary>
        [Fact]
        public void FormatResourceString_WithNullFirstParameter_ReturnsNull()
        {
            // Arrange
            string v1 = null;
            string v2 = "Non-null";
            
            // Act
            string result = ResourceUtilities.FormatResourceString(v1, v2);
            
            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests the FormatResourceStringStripCodeAndKeyword(string, string) method with normal inputs.
        /// Expected outcome: The method returns the first parameter.
        /// </summary>
        [Theory]
        [InlineData("Alpha", "Beta", "Alpha")]
        [InlineData("", "Something", "")]
        public void FormatResourceStringStripCodeAndKeyword_WithValidInputs_ReturnsFirstParameter(string v1, string v2, string expected)
        {
            // Act
            string result = ResourceUtilities.FormatResourceStringStripCodeAndKeyword(v1, v2);
            
            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the FormatResourceStringStripCodeAndKeyword(string, string) method when the first parameter is null.
        /// Expected outcome: The method returns null.
        /// </summary>
        [Fact]
        public void FormatResourceStringStripCodeAndKeyword_WithNullFirstParameter_ReturnsNull()
        {
            // Arrange
            string v1 = null;
            string v2 = "Not null";
            
            // Act
            string result = ResourceUtilities.FormatResourceStringStripCodeAndKeyword(v1, v2);
            
            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests the GetResourceString(string) method with various inputs.
        /// Expected outcome: The method returns the input string unchanged.
        /// </summary>
        [Theory]
        [InlineData("Resource Text")]
        [InlineData("")]
        public void GetResourceString_WithValidInput_ReturnsSameString(string input)
        {
            // Act
            string result = ResourceUtilities.GetResourceString(input);
            
            // Assert
            Assert.Equal(input, result);
        }

        /// <summary>
        /// Tests the GetResourceString(string) method when the input is null.
        /// Expected outcome: The method returns null.
        /// </summary>
        [Fact]
        public void GetResourceString_WithNullInput_ReturnsNull()
        {
            // Act
            string result = ResourceUtilities.GetResourceString(null);
            
            // Assert
            Assert.Null(result);
        }
    }
}
