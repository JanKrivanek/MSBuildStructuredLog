using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedBuildErrorEventArgs"/> class.
    /// </summary>
    public class ExtendedBuildErrorEventArgsTests
    {
        /// <summary>
        /// Tests the internal default constructor to ensure it sets ExtendedType to "undefined" and other extended properties to null.
        /// </summary>
        [Fact]
        public void DefaultConstructor_ShouldSetExtendedTypeToUndefined()
        {
            // Act
            var instance = new ExtendedBuildErrorEventArgs();

            // Assert
            instance.ExtendedType.Should().Be("undefined");
            instance.ExtendedMetadata.Should().BeNull();
            instance.ExtendedData.Should().BeNull();
        }

        /// <summary>
        /// Tests the constructor that accepts a type string to ensure it initializes ExtendedType correctly for various inputs.
        /// </summary>
        /// <param name="type">The value to set for the ExtendedType property.</param>
        [Theory]
        [InlineData("ErrorType")]
        [InlineData("")]
        [InlineData("!@#$%^&*()")]
        public void Constructor_WithType_ShouldSetExtendedType(string type)
        {
            // Act
            var instance = new ExtendedBuildErrorEventArgs(type);

            // Assert
            instance.ExtendedType.Should().Be(type);
        }

        /// <summary>
        /// Tests the constructor with all event data (without a timestamp) to ensure it initializes ExtendedType correctly.
        /// </summary>
        [Fact]
        public void Constructor_AllEventDataWithoutTimestamp_ShouldSetExtendedType()
        {
            // Arrange
            var type = "ErrorType";
            string? subcategory = "Subcategory";
            string? code = "ERR001";
            string? file = "file.cs";
            int lineNumber = 10;
            int columnNumber = 5;
            int endLineNumber = 10;
            int endColumnNumber = 15;
            string? message = "An error occurred.";
            string? helpKeyword = "HelpKeyword";
            string? senderName = "Sender";

            // Act
            var instance = new ExtendedBuildErrorEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName);

            // Assert
            instance.ExtendedType.Should().Be(type);
        }

        /// <summary>
        /// Tests the constructor with all event data and a timestamp to ensure it initializes ExtendedType correctly.
        /// </summary>
        [Fact]
        public void Constructor_AllEventDataWithTimestamp_ShouldSetExtendedType()
        {
            // Arrange
            var type = "ErrorType";
            string? subcategory = "SubcategoryTimestamp";
            string? code = "ERR002";
            string? file = "file2.cs";
            int lineNumber = int.MinValue;
            int columnNumber = 0;
            int endLineNumber = int.MaxValue;
            int endColumnNumber = 0;
            string? message = "Error with timestamp.";
            string? helpKeyword = "HelpKeywordTimestamp";
            string? senderName = "SenderTimestamp";
            var eventTimestamp = DateTime.UtcNow;

            // Act
            var instance = new ExtendedBuildErrorEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, eventTimestamp);

            // Assert
            instance.ExtendedType.Should().Be(type);
        }

        /// <summary>
        /// Tests the constructor with all event data, a timestamp, and message arguments to ensure it initializes ExtendedType correctly.
        /// </summary>
        [Fact]
        public void Constructor_AllEventDataWithTimestampAndMessageArgs_ShouldSetExtendedType()
        {
            // Arrange
            var type = "ErrorType";
            string? subcategory = "SubcategoryArgs";
            string? code = "ERR003";
            string? file = "file3.cs";
            int lineNumber = 1;
            int columnNumber = 1;
            int endLineNumber = 2;
            int endColumnNumber = 2;
            string? message = "Error with message arguments.";
            string? helpKeyword = "HelpKeywordArgs";
            string? senderName = "SenderArgs";
            var eventTimestamp = DateTime.Now;
            object[] messageArgs = { 1, "arg", null };

            // Act
            var instance = new ExtendedBuildErrorEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, eventTimestamp, messageArgs);

            // Assert
            instance.ExtendedType.Should().Be(type);
        }

        /// <summary>
        /// Tests the constructor with all event data, a timestamp, a helpLink, and message arguments to ensure it initializes ExtendedType correctly.
        /// </summary>
        [Fact]
        public void Constructor_AllEventDataWithTimestampHelpLinkAndMessageArgs_ShouldSetExtendedType()
        {
            // Arrange
            var type = "ErrorType";
            string? subcategory = "SubcategoryHelpLink";
            string? code = "ERR004";
            string? file = "file4.cs";
            int lineNumber = 100;
            int columnNumber = 50;
            int endLineNumber = 150;
            int endColumnNumber = 75;
            string? message = "Error with help link.";
            string? helpKeyword = "HelpKeywordLink";
            string? senderName = "SenderLink";
            string? helpLink = "http://help.link";
            var eventTimestamp = DateTime.Now;
            object[] messageArgs = { "arg1", "arg2" };

            // Act
            var instance = new ExtendedBuildErrorEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, helpLink, eventTimestamp, messageArgs);

            // Assert
            instance.ExtendedType.Should().Be(type);
        }

        /// <summary>
        /// Tests setting and getting the ExtendedType property to ensure property roundtrip behavior.
        /// </summary>
        [Fact]
        public void ExtendedTypeProperty_SetAndGet_ShouldReturnSameValue()
        {
            // Arrange
            var initialType = "InitialType";
            var instance = new ExtendedBuildErrorEventArgs(initialType);
            var newType = "NewType";

            // Act
            instance.ExtendedType = newType;

            // Assert
            instance.ExtendedType.Should().Be(newType);
        }

        /// <summary>
        /// Tests setting and getting the ExtendedMetadata property to ensure property roundtrip behavior.
        /// </summary>
        [Fact]
        public void ExtendedMetadataProperty_SetAndGet_ShouldReturnSameInstance()
        {
            // Arrange
            var instance = new ExtendedBuildErrorEventArgs("TestType");
            IDictionary<string, string> metadata = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

            // Act
            instance.ExtendedMetadata = metadata;

            // Assert
            instance.ExtendedMetadata.Should().BeSameAs(metadata);
        }

        /// <summary>
        /// Tests setting and getting the ExtendedData property to ensure property roundtrip behavior.
        /// </summary>
        [Fact]
        public void ExtendedDataProperty_SetAndGet_ShouldReturnSameValue()
        {
            // Arrange
            var instance = new ExtendedBuildErrorEventArgs("TestType");
            var data = "Some extended data";

            // Act
            instance.ExtendedData = data;

            // Assert
            instance.ExtendedData.Should().Be(data);
        }
    }
}
