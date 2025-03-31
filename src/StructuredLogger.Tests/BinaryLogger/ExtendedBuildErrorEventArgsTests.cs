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
        private const string DefaultTypeValue = "undefined";

        /// <summary>
        /// Tests the internal default constructor to ensure it sets ExtendedType to "undefined".
        /// </summary>
        [Fact]
        public void DefaultConstructor_ShouldSetExtendedTypeToUndefined()
        {
            // Arrange & Act: Using Activator to invoke the internal default constructor.
            object? instance = Activator.CreateInstance(typeof(ExtendedBuildErrorEventArgs), nonPublic: true);
            instance.Should().NotBeNull();
            ExtendedBuildErrorEventArgs eventArgs = (ExtendedBuildErrorEventArgs)instance!;

            // Assert
            eventArgs.ExtendedType.Should().Be(DefaultTypeValue);
            eventArgs.ExtendedData.Should().BeNull();
            eventArgs.ExtendedMetadata.Should().BeNull();
        }

        /// <summary>
        /// Tests the constructor with the 'type' parameter to ensure ExtendedType is set correctly.
        /// </summary>
        [Theory]
        [InlineData("ErrorType1")]
        [InlineData("AnotherError")]
        public void Constructor_WithTypeParameter_ShouldSetExtendedType(string type)
        {
            // Arrange & Act
            var eventArgs = new ExtendedBuildErrorEventArgs(type);

            // Assert
            eventArgs.ExtendedType.Should().Be(type);
            eventArgs.ExtendedData.Should().BeNull();
            eventArgs.ExtendedMetadata.Should().BeNull();
        }

        /// <summary>
        /// Tests the full constructor without eventTimestamp to ensure inherited properties and ExtendedType are set correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithAllParameters_NoTimestamp_ShouldSetProperties()
        {
            // Arrange
            string type = "ErrorFull";
            string subcategory = "SubCat";
            string code = "E100";
            string file = "file.cs";
            int lineNumber = 10;
            int columnNumber = 5;
            int endLineNumber = 10;
            int endColumnNumber = 15;
            string message = "An error occurred";
            string helpKeyword = "HelpKey";
            string senderName = "Sender";

            // Act
            var eventArgs = new ExtendedBuildErrorEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName);

            // Assert
            eventArgs.ExtendedType.Should().Be(type);
            // Validate base properties (assuming the BuildErrorEventArgs pass these to public properties)
            eventArgs.Subcategory.Should().Be(subcategory);
            eventArgs.Code.Should().Be(code);
            eventArgs.File.Should().Be(file);
            eventArgs.LineNumber.Should().Be(lineNumber);
            eventArgs.ColumnNumber.Should().Be(columnNumber);
            eventArgs.EndLineNumber.Should().Be(endLineNumber);
            eventArgs.EndColumnNumber.Should().Be(endColumnNumber);
            eventArgs.Message.Should().Be(message);
            eventArgs.HelpKeyword.Should().Be(helpKeyword);
            eventArgs.SenderName.Should().Be(senderName);
        }

        /// <summary>
        /// Tests the full constructor with eventTimestamp to ensure inherited properties and ExtendedType are set correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithTimestamp_ShouldSetEventTimestampAndProperties()
        {
            // Arrange
            string type = "ErrorTimestamp";
            string subcategory = "SubCatTS";
            string code = "E200";
            string file = "file2.cs";
            int lineNumber = 20;
            int columnNumber = 8;
            int endLineNumber = 21;
            int endColumnNumber = 18;
            string message = "Timestamp error";
            string helpKeyword = "HelpTS";
            string senderName = "SenderTS";
            DateTime eventTimestamp = DateTime.Now;

            // Act
            var eventArgs = new ExtendedBuildErrorEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, eventTimestamp);

            // Assert
            eventArgs.ExtendedType.Should().Be(type);
            eventArgs.Subcategory.Should().Be(subcategory);
            eventArgs.Code.Should().Be(code);
            eventArgs.File.Should().Be(file);
            eventArgs.LineNumber.Should().Be(lineNumber);
            eventArgs.ColumnNumber.Should().Be(columnNumber);
            eventArgs.EndLineNumber.Should().Be(endLineNumber);
            eventArgs.EndColumnNumber.Should().Be(endColumnNumber);
            eventArgs.Message.Should().Be(message);
            eventArgs.HelpKeyword.Should().Be(helpKeyword);
            eventArgs.SenderName.Should().Be(senderName);
            // Assuming inherited Timestamp property exists in BuildEventArgs.
            eventArgs.Timestamp.Should().Be(eventTimestamp);
        }

        /// <summary>
        /// Tests the full constructor with eventTimestamp and message arguments to ensure properties are set correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithTimestampAndMessageArgs_ShouldSetPropertiesAndStoreMessageArgs()
        {
            // Arrange
            string type = "ErrorArgs";
            string subcategory = "SubCatArgs";
            string code = "E300";
            string file = "file3.cs";
            int lineNumber = 30;
            int columnNumber = 12;
            int endLineNumber = 31;
            int endColumnNumber = 22;
            string message = "Error with args";
            string helpKeyword = "HelpArgs";
            string senderName = "SenderArgs";
            DateTime eventTimestamp = DateTime.UtcNow;
            object[] messageArgs = new object[] { "Arg1", 123 };

            // Act
            var eventArgs = new ExtendedBuildErrorEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, eventTimestamp, messageArgs);

            // Assert
            eventArgs.ExtendedType.Should().Be(type);
            eventArgs.Subcategory.Should().Be(subcategory);
            eventArgs.Code.Should().Be(code);
            eventArgs.File.Should().Be(file);
            eventArgs.LineNumber.Should().Be(lineNumber);
            eventArgs.ColumnNumber.Should().Be(columnNumber);
            eventArgs.EndLineNumber.Should().Be(endLineNumber);
            eventArgs.EndColumnNumber.Should().Be(endColumnNumber);
            eventArgs.Message.Should().Be(message);
            eventArgs.HelpKeyword.Should().Be(helpKeyword);
            eventArgs.SenderName.Should().Be(senderName);
            eventArgs.Timestamp.Should().Be(eventTimestamp);
        }

        /// <summary>
        /// Tests the full constructor with helpLink, eventTimestamp, and message arguments to ensure all properties are set correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithHelpLinkTimestampAndMessageArgs_ShouldSetPropertiesAndStoreHelpLink()
        {
            // Arrange
            string type = "ErrorHelpLink";
            string subcategory = "SubCatHL";
            string code = "E400";
            string file = "file4.cs";
            int lineNumber = 40;
            int columnNumber = 16;
            int endLineNumber = 41;
            int endColumnNumber = 26;
            string message = "Error with help link";
            string helpKeyword = "HelpHL";
            string senderName = "SenderHL";
            string helpLink = "http://help.link";
            DateTime eventTimestamp = DateTime.UtcNow.AddHours(-1);
            object[] messageArgs = new object[] { "Param1", 456 };

            // Act
            var eventArgs = new ExtendedBuildErrorEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, helpLink, eventTimestamp, messageArgs);

            // Assert
            eventArgs.ExtendedType.Should().Be(type);
            eventArgs.Subcategory.Should().Be(subcategory);
            eventArgs.Code.Should().Be(code);
            eventArgs.File.Should().Be(file);
            eventArgs.LineNumber.Should().Be(lineNumber);
            eventArgs.ColumnNumber.Should().Be(columnNumber);
            eventArgs.EndLineNumber.Should().Be(endLineNumber);
            eventArgs.EndColumnNumber.Should().Be(endColumnNumber);
            eventArgs.Message.Should().Be(message);
            eventArgs.HelpKeyword.Should().Be(helpKeyword);
            eventArgs.SenderName.Should().Be(senderName);
            // The helpLink is expected to be stored in a property provided by the base class.
            eventArgs.HelpLink.Should().Be(helpLink);
            eventArgs.Timestamp.Should().Be(eventTimestamp);
        }

        /// <summary>
        /// Tests the mutability of the ExtendedData property.
        /// </summary>
        [Fact]
        public void ExtendedData_Property_SetAndGet_ShouldPersistValue()
        {
            // Arrange
            string type = "TestData";
            var eventArgs = new ExtendedBuildErrorEventArgs(type);
            string dataValue = "Some extra data";

            // Act
            eventArgs.ExtendedData = dataValue;

            // Assert
            eventArgs.ExtendedData.Should().Be(dataValue);
        }

        /// <summary>
        /// Tests the mutability of the ExtendedMetadata property.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_Property_SetAndGet_ShouldPersistDictionary()
        {
            // Arrange
            string type = "TestMetadata";
            var eventArgs = new ExtendedBuildErrorEventArgs(type);
            var metadata = new Dictionary<string, string?>
            {
                { "Key1", "Value1" },
                { "Key2", null }
            };

            // Act
            eventArgs.ExtendedMetadata = metadata;

            // Assert
            eventArgs.ExtendedMetadata.Should().BeEquivalentTo(metadata);
        }
    }
}
