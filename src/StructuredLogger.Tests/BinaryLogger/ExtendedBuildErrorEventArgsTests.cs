using System.Collections.Generic;
using Microsoft.Build.Framework;
using Moq;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedBuildErrorEventArgs"/> class.
    /// </summary>
    public class ExtendedBuildErrorEventArgsTests
    {
        /// <summary>
        /// Tests that the constructor with a single type parameter correctly sets the ExtendedType property.
        /// </summary>
        [Fact]
        public void Constructor_WithTypeParameter_SetsExtendedType()
        {
            // Arrange
            string expectedType = "CustomError";

            // Act
            var eventArgs = new ExtendedBuildErrorEventArgs(expectedType);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the internal parameterless constructor initializes ExtendedType to "undefined".
        /// </summary>
        [Fact]
        public void InternalConstructor_Default_SetsExtendedTypeToUndefined()
        {
            // Arrange & Act
            var eventArgs = (ExtendedBuildErrorEventArgs)Activator.CreateInstance(typeof(ExtendedBuildErrorEventArgs), nonPublic: true);

            // Assert
            Assert.Equal("undefined", eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the full constructor without timestamp sets the ExtendedType property and passes parameters to the base.
        /// </summary>
        [Fact]
        public void Constructor_FullParameters_SetsExtendedType()
        {
            // Arrange
            string expectedType = "FullError";
            string? subcategory = "SubCat";
            string? code = "ERR001";
            string? file = "file.cs";
            int lineNumber = 10;
            int columnNumber = 5;
            int endLineNumber = 10;
            int endColumnNumber = 20;
            string? message = "An error occurred.";
            string? helpKeyword = "HELP001";
            string? senderName = "TestSender";

            // Act
            var eventArgs = new ExtendedBuildErrorEventArgs(expectedType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the constructor with a timestamp correctly sets the ExtendedType property.
        /// </summary>
        [Fact]
        public void Constructor_WithTimestamp_SetsExtendedType()
        {
            // Arrange
            string expectedType = "TimestampError";
            string? subcategory = "SubCat";
            string? code = "ERR002";
            string? file = "file2.cs";
            int lineNumber = 20;
            int columnNumber = 10;
            int endLineNumber = 20;
            int endColumnNumber = 30;
            string? message = "Timestamp error occurred.";
            string? helpKeyword = "HELP002";
            string? senderName = "TestSenderTimestamp";
            DateTime timestamp = DateTime.UtcNow;

            // Act
            var eventArgs = new ExtendedBuildErrorEventArgs(expectedType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, timestamp);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the constructor with timestamp and message arguments correctly sets the ExtendedType property.
        /// </summary>
        [Fact]
        public void Constructor_WithTimestampAndMessageArgs_SetsExtendedType()
        {
            // Arrange
            string expectedType = "TimestampArgsError";
            string? subcategory = "SubCat";
            string? code = "ERR003";
            string? file = "file3.cs";
            int lineNumber = 30;
            int columnNumber = 15;
            int endLineNumber = 30;
            int endColumnNumber = 35;
            string? message = "Error with args: {0}";
            string? helpKeyword = "HELP003";
            string? senderName = "TestSenderArgs";
            DateTime timestamp = DateTime.UtcNow;
            object[] messageArgs = new object[] { "Arg1" };

            // Act
            var eventArgs = new ExtendedBuildErrorEventArgs(expectedType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, timestamp, messageArgs);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the constructor with helpLink, timestamp, and message arguments correctly sets the ExtendedType property.
        /// </summary>
        [Fact]
        public void Constructor_WithHelpLinkTimestampAndMessageArgs_SetsExtendedType()
        {
            // Arrange
            string expectedType = "HelpLinkError";
            string? subcategory = "SubCat";
            string? code = "ERR004";
            string? file = "file4.cs";
            int lineNumber = 40;
            int columnNumber = 20;
            int endLineNumber = 40;
            int endColumnNumber = 45;
            string? message = "Error with help link and args: {0}";
            string? helpKeyword = "HELP004";
            string? senderName = "TestSenderHelpLink";
            string? helpLink = "http://help.link";
            DateTime timestamp = DateTime.UtcNow;
            object[] messageArgs = new object[] { "ArgX" };

            // Act
            var eventArgs = new ExtendedBuildErrorEventArgs(expectedType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, helpLink, timestamp, messageArgs);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_Property_SetAndGet_WorksCorrectly()
        {
            // Arrange
            var eventArgs = new ExtendedBuildErrorEventArgs("MetadataTest");
            var metadata = new Dictionary<string, string?>()
            {
                { "Key1", "Value1" },
                { "Key2", null }
            };

            // Act
            eventArgs.ExtendedMetadata = metadata;

            // Assert
            Assert.Equal(metadata, eventArgs.ExtendedMetadata);
        }

        /// <summary>
        /// Tests that the ExtendedData property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void ExtendedData_Property_SetAndGet_WorksCorrectly()
        {
            // Arrange
            var eventArgs = new ExtendedBuildErrorEventArgs("DataTest");
            string expectedData = "Extended error data";

            // Act
            eventArgs.ExtendedData = expectedData;

            // Assert
            Assert.Equal(expectedData, eventArgs.ExtendedData);
        }
    }
}
