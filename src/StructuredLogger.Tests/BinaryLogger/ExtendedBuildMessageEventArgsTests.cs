using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedBuildMessageEventArgs"/> class.
    /// </summary>
    public class ExtendedBuildMessageEventArgsTests
    {
        /// <summary>
        /// Tests that the internal default constructor sets ExtendedType to "undefined".
        /// </summary>
        [Fact]
        public void DefaultConstructor_Internal_ShouldSetExtendedTypeToUndefined()
        {
            // Arrange & Act
            var eventArgs = new ExtendedBuildMessageEventArgs();

            // Assert
            eventArgs.ExtendedType.Should().Be("undefined", "the default internal constructor should assign 'undefined' as the ExtendedType");
        }

        /// <summary>
        /// Tests that the constructor with only type parameter sets ExtendedType correctly.
        /// </summary>
        /// <param name="type">The custom extended type value.</param>
        [Theory]
        [InlineData("CustomType")]
        [InlineData("")]
        [InlineData("A very long extended type that exceeds normal length expectations........................................")]
        public void Constructor_WithType_ShouldSetExtendedType(string type)
        {
            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(type);

            // Assert
            eventArgs.ExtendedType.Should().Be(type, "the constructor should assign the provided type to ExtendedType");
        }

        /// <summary>
        /// Tests that the constructor with type, message, helpKeyword, senderName and importance sets ExtendedType correctly.
        /// Also tests edge cases with null and empty string parameters.
        /// </summary>
        [Theory]
        [InlineData("Type1", "Message", "Help", "Sender")]
        [InlineData("Type2", null, null, null)]
        [InlineData("", "", "", "")]
        public void Constructor_WithTypeMessageHelpSenderImportance_ShouldSetExtendedTypeAndPassParameters(string type, string? message, string? helpKeyword, string? senderName)
        {
            // Arrange
            var importance = MessageImportance.High;

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(type, message, helpKeyword, senderName, importance);

            // Assert
            eventArgs.ExtendedType.Should().Be(type, "the constructor should assign the provided type to ExtendedType");
        }

        /// <summary>
        /// Tests that the constructor with type, message, helpKeyword, senderName, importance and eventTimestamp sets ExtendedType correctly.
        /// </summary>
        [Theory]
        [InlineData("TypeWithTimestamp", "A message", "Keyword", "Sender")]
        [InlineData("TypeWithTimestamp", null, null, null)]
        public void Constructor_WithTypeMessageHelpSenderImportanceAndTimestamp_ShouldSetExtendedType(string type, string? message, string? helpKeyword, string? senderName)
        {
            // Arrange
            var importance = MessageImportance.Low;
            var eventTimestamp = DateTime.UtcNow;

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(type, message, helpKeyword, senderName, importance, eventTimestamp);

            // Assert
            eventArgs.ExtendedType.Should().Be(type, "the constructor should assign the provided type to ExtendedType");
        }

        /// <summary>
        /// Tests that the constructor with type, message, helpKeyword, senderName, importance, eventTimestamp and messageArgs sets ExtendedType correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithTypeMessageHelpSenderImportanceTimestampAndMessageArgs_ShouldSetExtendedType()
        {
            // Arrange
            string type = "TypeWithArgs";
            string? message = "Detailed Message";
            string? helpKeyword = "HelpKey";
            string? senderName = "SenderName";
            var importance = MessageImportance.Normal;
            var eventTimestamp = DateTime.Now;
            object[] messageArgs = new object[] { "arg1", 123, null };

            // Act
            var eventArgsInstance = new ExtendedBuildMessageEventArgs(type, message, helpKeyword, senderName, importance, eventTimestamp, messageArgs);

            // Assert
            eventArgsInstance.ExtendedType.Should().Be(type, "the constructor should assign the provided type to ExtendedType");
        }

        /// <summary>
        /// Tests that the constructor with file details (without timestamp) sets ExtendedType correctly.
        /// </summary>
        [Theory]
        [InlineData("FileType", "Subcat", "Code1", "C:\\temp\\file.txt", 1, 1, 1, 1, "Message", "Help", "Sender")]
        [InlineData("FileType", null, null, "/unix/path/file.txt", 0, 0, 0, 0, null, null, null)]
        public void Constructor_WithFileDetails_ShouldSetExtendedType(string type, string? subcategory, string? code, string? file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber,
            string? message, string? helpKeyword, string? senderName)
        {
            // Arrange
            var importance = MessageImportance.Low;

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, importance);

            // Assert
            eventArgs.ExtendedType.Should().Be(type, "the constructor should assign the provided type to ExtendedType");
        }

        /// <summary>
        /// Tests that the constructor with file details and timestamp sets ExtendedType correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithFileDetailsAndTimestamp_ShouldSetExtendedType()
        {
            // Arrange
            string type = "FileTypeWithTimestamp";
            string? subcategory = "Subcat";
            string? code = "Code2";
            string? file = "C:\\file.log";
            int lineNumber = 10, columnNumber = 5, endLineNumber = 10, endColumnNumber = 15;
            string? message = "Log Message";
            string? helpKeyword = "LogHelp";
            string? senderName = "Logger";
            var importance = MessageImportance.High;
            var eventTimestamp = DateTime.UtcNow;

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, importance, eventTimestamp);

            // Assert
            eventArgs.ExtendedType.Should().Be(type, "the constructor should assign the provided type to ExtendedType");
        }

        /// <summary>
        /// Tests that the constructor with file details, timestamp and message arguments sets ExtendedType correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithFileDetailsTimestampAndMessageArgs_ShouldSetExtendedType()
        {
            // Arrange
            string type = "FileTypeWithArgs";
            string? subcategory = "SubcatArgs";
            string? code = "CodeArgs";
            string? file = "/unix/file.log";
            int lineNumber = 0, columnNumber = 0, endLineNumber = 0, endColumnNumber = 0;
            string? message = null;
            string? helpKeyword = "";
            string? senderName = "UnitTest";
            var importance = MessageImportance.Normal;
            var eventTimestamp = DateTime.Now;
            object[] messageArgs = new object[] { "param1", 456 };

            // Act
            var eventArgsInstance = new ExtendedBuildMessageEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, importance, eventTimestamp, messageArgs);

            // Assert
            eventArgsInstance.ExtendedType.Should().Be(type, "the constructor should assign the provided type to ExtendedType");
        }

        /// <summary>
        /// Tests the setting and getting of the ExtendedMetadata property.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_Property_SetAndGet_ShouldWorkCorrectly()
        {
            // Arrange
            var eventArgs = new ExtendedBuildMessageEventArgs("TestType");
            var metadata = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

            // Act
            eventArgs.ExtendedMetadata = metadata;

            // Assert
            eventArgs.ExtendedMetadata.Should().BeEquivalentTo(metadata, "the ExtendedMetadata property should return the dictionary that was set");
        }

        /// <summary>
        /// Tests the setting and getting of the ExtendedData property.
        /// </summary>
        [Theory]
        [InlineData("Some extended data.")]
        [InlineData("")]
        [InlineData(null)]
        public void ExtendedData_Property_SetAndGet_ShouldWorkCorrectly(string? extendedData)
        {
            // Arrange
            var eventArgs = new ExtendedBuildMessageEventArgs("TestType");

            // Act
            eventArgs.ExtendedData = extendedData;

            // Assert
            eventArgs.ExtendedData.Should().Be(extendedData, "the ExtendedData property should return the value that was set");
        }
    }
}
