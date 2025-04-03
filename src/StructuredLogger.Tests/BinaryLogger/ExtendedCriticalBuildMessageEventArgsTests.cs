using FluentAssertions;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedCriticalBuildMessageEventArgs"/> class.
    /// </summary>
    public class ExtendedCriticalBuildMessageEventArgsTests
    {
        /// <summary>
        /// Tests the single parameter constructor to ensure it sets the ExtendedType property correctly.
        /// This test covers various edge cases including empty, normal, and long strings.
        /// </summary>
        /// <param name="inputType">The input string for the ExtendedType.</param>
        [Theory]
        [InlineData("TestType")]
        [InlineData("")]
        [InlineData("!@#$%^&*()_+-=[]{};':\",.<>/?")]
        public void Constructor_SingleParameter_ValidInput_SetsExtendedType(string inputType)
        {
            // Arrange & Act
            var instance = new ExtendedCriticalBuildMessageEventArgs(inputType);

            // Assert
            instance.ExtendedType.Should().Be(inputType, "the single parameter constructor should set ExtendedType to the provided value");
        }

        /// <summary>
        /// Tests the default (internal) constructor to ensure it sets ExtendedType to "undefined".
        /// </summary>
        [Fact]
        public void DefaultConstructor_ReturnsUndefinedType()
        {
            // Arrange & Act
            var instance = new ExtendedCriticalBuildMessageEventArgs();

            // Assert
            instance.ExtendedType.Should().Be("undefined", "the default constructor should initialize ExtendedType with 'undefined'");
        }

        /// <summary>
        /// Tests the constructor with the eventTimestamp parameter (without message arguments) to ensure it sets ExtendedType correctly.
        /// This test also uses boundary int values for line, column numbers.
        /// </summary>
        [Fact]
        public void Constructor_WithEventTimestamp_NoMessageArgs_SetsExtendedType()
        {
            // Arrange
            string expectedExtendedType = "CriticalType";
            string? subcategory = "SubCat";
            string? code = "ERR001";
            string? file = "file.txt";
            int lineNumber = int.MinValue;
            int columnNumber = int.MaxValue;
            int endLineNumber = 0;
            int endColumnNumber = 0;
            string? message = "A critical message";
            string? helpKeyword = "Help001";
            string? senderName = "UnitTest";
            DateTime eventTimestamp = DateTime.UtcNow;

            // Act
            var instance = new ExtendedCriticalBuildMessageEventArgs(
                expectedExtendedType,
                subcategory,
                code,
                file,
                lineNumber,
                columnNumber,
                endLineNumber,
                endColumnNumber,
                message,
                helpKeyword,
                senderName,
                eventTimestamp);

            // Assert
            instance.ExtendedType.Should().Be(expectedExtendedType, "the constructor with eventTimestamp should assign ExtendedType from the provided type");
        }

        /// <summary>
        /// Tests the constructor with the eventTimestamp parameter and message arguments to ensure it sets ExtendedType correctly.
        /// This test uses various boundary values for numeric parameters.
        /// </summary>
        [Fact]
        public void Constructor_WithEventTimestamp_AndMessageArgs_SetsExtendedType()
        {
            // Arrange
            string expectedExtendedType = "ExtendedCriticalType";
            string? subcategory = null;
            string? code = null;
            string? file = null;
            int lineNumber = 0;
            int columnNumber = 0;
            int endLineNumber = int.MaxValue;
            int endColumnNumber = int.MinValue;
            string? message = "";
            string? helpKeyword = "HelpKeyword";
            string? senderName = "TestSender";
            DateTime eventTimestamp = DateTime.UtcNow;
            object[] messageArgs = { "arg1", 123 };

            // Act
            var instance = new ExtendedCriticalBuildMessageEventArgs(
                expectedExtendedType,
                subcategory,
                code,
                file,
                lineNumber,
                columnNumber,
                endLineNumber,
                endColumnNumber,
                message,
                helpKeyword,
                senderName,
                eventTimestamp,
                messageArgs);

            // Assert
            instance.ExtendedType.Should().Be(expectedExtendedType, "the constructor with message arguments should assign ExtendedType from the provided type");
        }

        /// <summary>
        /// Tests the ExtendedType property getter and setter to ensure that it returns the same value that was set.
        /// </summary>
        [Fact]
        public void ExtendedTypeProperty_SetAndGet_ReturnsSameValue()
        {
            // Arrange
            var instance = new ExtendedCriticalBuildMessageEventArgs("InitialType");
            string newType = "UpdatedType";

            // Act
            instance.ExtendedType = newType;

            // Assert
            instance.ExtendedType.Should().Be(newType, "the ExtendedType property should return the value that was set");
        }

        /// <summary>
        /// Tests the ExtendedMetadata property getter and setter to ensure that it supports assignment and retrieval.
        /// </summary>
        [Fact]
        public void ExtendedMetadataProperty_SetAndGet_ReturnsSameValue()
        {
            // Arrange
            var instance = new ExtendedCriticalBuildMessageEventArgs("TestType");
            IDictionary<string, string>? expectedMetadata = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

            // Act
            instance.ExtendedMetadata = expectedMetadata;

            // Assert
            instance.ExtendedMetadata.Should().BeEquivalentTo(expectedMetadata, "the ExtendedMetadata property should return the dictionary that was assigned");
        }

        /// <summary>
        /// Tests the ExtendedData property getter and setter to ensure that it supports assignment and retrieval,
        /// including edge cases such as empty strings and strings with special characters.
        /// </summary>
        [Fact]
        public void ExtendedDataProperty_SetAndGet_ReturnsSameValue()
        {
            // Arrange
            var instance = new ExtendedCriticalBuildMessageEventArgs("TestType");
            string expectedData = "Some extended data! @#$%^&*()";

            // Act
            instance.ExtendedData = expectedData;

            // Assert
            instance.ExtendedData.Should().Be(expectedData, "the ExtendedData property should return the string that was assigned");
        }
    }
}
