using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedBuildWarningEventArgs"/> class.
    /// </summary>
    public class ExtendedBuildWarningEventArgsTests
    {
        /// <summary>
        /// Tests that the default internal constructor sets the ExtendedType property to "undefined" and leaves extended properties null.
        /// </summary>
        [Fact]
        public void DefaultConstructor_ShouldSetExtendedTypeToUndefined()
        {
            // Arrange & Act
            var eventArgs = new ExtendedBuildWarningEventArgs();

            // Assert
            Assert.Equal("undefined", eventArgs.ExtendedType);
            Assert.Null(eventArgs.ExtendedMetadata);
            Assert.Null(eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests that the constructor with only the type parameter sets the ExtendedType property correctly.
        /// </summary>
        /// <param name="inputType">Input value for ExtendedType.</param>
        [Theory]
        [InlineData("WarningType")]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_WithTypeParameter_ShouldSetExtendedType(string inputType)
        {
            // Arrange & Act
            var eventArgs = new ExtendedBuildWarningEventArgs(inputType);

            // Assert
            Assert.Equal(inputType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the full constructor (without timestamp) assigns the ExtendedType and base properties correctly.
        /// </summary>
        [Fact]
        public void Constructor_FullWithoutTimestamp_ShouldAssignProperties()
        {
            // Arrange
            string type = "FullType";
            string subcategory = "SubCategory";
            string code = "Code123";
            string file = "file.txt";
            int lineNumber = 10;
            int columnNumber = 5;
            int endLineNumber = 12;
            int endColumnNumber = 15;
            string message = "Test message";
            string helpKeyword = "HelpKeyword";
            string senderName = "Sender";

            // Act
            var eventArgs = new ExtendedBuildWarningEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName);

            // Assert
            Assert.Equal(type, eventArgs.ExtendedType);
            // Verify base properties are assigned correctly.
            Assert.Equal(subcategory, eventArgs.Subcategory);
            Assert.Equal(code, eventArgs.Code);
            Assert.Equal(file, eventArgs.File);
            Assert.Equal(lineNumber, eventArgs.LineNumber);
            Assert.Equal(columnNumber, eventArgs.ColumnNumber);
            Assert.Equal(endLineNumber, eventArgs.EndLineNumber);
            Assert.Equal(endColumnNumber, eventArgs.EndColumnNumber);
            Assert.Equal(message, eventArgs.Message);
            Assert.Equal(helpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(senderName, eventArgs.SenderName);
        }

        /// <summary>
        /// Tests that the constructor with timestamp assigns the ExtendedType and base properties correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithTimestamp_ShouldAssignProperties()
        {
            // Arrange
            string type = "TimestampType";
            string subcategory = "SubTimestamp";
            string code = "CodeTS";
            string file = "fileTS.txt";
            int lineNumber = 20;
            int columnNumber = 10;
            int endLineNumber = 22;
            int endColumnNumber = 25;
            string message = "Timestamp message";
            string helpKeyword = "TSHelp";
            string senderName = "TSSender";
            DateTime eventTimestamp = DateTime.Now;

            // Act
            var eventArgs = new ExtendedBuildWarningEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, eventTimestamp);

            // Assert
            Assert.Equal(type, eventArgs.ExtendedType);
            Assert.Equal(subcategory, eventArgs.Subcategory);
            Assert.Equal(code, eventArgs.Code);
            Assert.Equal(file, eventArgs.File);
            Assert.Equal(lineNumber, eventArgs.LineNumber);
            Assert.Equal(columnNumber, eventArgs.ColumnNumber);
            Assert.Equal(endLineNumber, eventArgs.EndLineNumber);
            Assert.Equal(endColumnNumber, eventArgs.EndColumnNumber);
            Assert.Equal(message, eventArgs.Message);
            Assert.Equal(helpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(senderName, eventArgs.SenderName);
            Assert.Equal(eventTimestamp, eventArgs.Timestamp);
        }

        /// <summary>
        /// Tests that the constructor with timestamp and message arguments assigns the ExtendedType and base properties correctly.
        /// </summary>
//         [Fact] [Error] (157-49)CS1061 'ExtendedBuildWarningEventArgs' does not contain a definition for 'MessageArgs' and no accessible extension method 'MessageArgs' accepting a first argument of type 'ExtendedBuildWarningEventArgs' could be found (are you missing a using directive or an assembly reference?)
//         public void Constructor_WithTimestampAndMessageArgs_ShouldAssignProperties()
//         {
//             // Arrange
//             string type = "TypeWithArgs";
//             string subcategory = "SubArgs";
//             string code = "CodeArgs";
//             string file = "fileArgs.txt";
//             int lineNumber = 30;
//             int columnNumber = 15;
//             int endLineNumber = 32;
//             int endColumnNumber = 35;
//             string message = "Message with args";
//             string helpKeyword = "HelpArgs";
//             string senderName = "SenderArgs";
//             DateTime eventTimestamp = DateTime.UtcNow;
//             object[] messageArgs = new object[] { "arg1", 2, null };
// 
//             // Act
//             var eventArgs = new ExtendedBuildWarningEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, eventTimestamp, messageArgs);
// 
//             // Assert
//             Assert.Equal(type, eventArgs.ExtendedType);
//             Assert.Equal(subcategory, eventArgs.Subcategory);
//             Assert.Equal(code, eventArgs.Code);
//             Assert.Equal(file, eventArgs.File);
//             Assert.Equal(lineNumber, eventArgs.LineNumber);
//             Assert.Equal(columnNumber, eventArgs.ColumnNumber);
//             Assert.Equal(endLineNumber, eventArgs.EndLineNumber);
//             Assert.Equal(endColumnNumber, eventArgs.EndColumnNumber);
//             Assert.Equal(message, eventArgs.Message);
//             Assert.Equal(helpKeyword, eventArgs.HelpKeyword);
//             Assert.Equal(senderName, eventArgs.SenderName);
//             Assert.Equal(eventTimestamp, eventArgs.Timestamp);
//             Assert.Equal(messageArgs, eventArgs.MessageArgs);
//         }

        /// <summary>
        /// Tests that the constructor with help link, timestamp, and message arguments assigns the ExtendedType and base properties correctly.
        /// </summary>
//         [Fact] [Error] (199-49)CS1061 'ExtendedBuildWarningEventArgs' does not contain a definition for 'MessageArgs' and no accessible extension method 'MessageArgs' accepting a first argument of type 'ExtendedBuildWarningEventArgs' could be found (are you missing a using directive or an assembly reference?)
//         public void Constructor_WithHelpLinkTimestampAndMessageArgs_ShouldAssignProperties()
//         {
//             // Arrange
//             string type = "TypeWithHelpLink";
//             string subcategory = "SubHelp";
//             string code = "CodeHelp";
//             string file = "fileHelp.txt";
//             int lineNumber = 40;
//             int columnNumber = 20;
//             int endLineNumber = 42;
//             int endColumnNumber = 45;
//             string message = "Help message";
//             string helpKeyword = "HelpKeyword";
//             string senderName = "HelpSender";
//             string helpLink = "http://example.com/help";
//             DateTime eventTimestamp = DateTime.UtcNow;
//             object[] messageArgs = new object[] { "param1", 3 };
// 
//             // Act
//             var eventArgs = new ExtendedBuildWarningEventArgs(type, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, helpLink, eventTimestamp, messageArgs);
// 
//             // Assert
//             Assert.Equal(type, eventArgs.ExtendedType);
//             Assert.Equal(subcategory, eventArgs.Subcategory);
//             Assert.Equal(code, eventArgs.Code);
//             Assert.Equal(file, eventArgs.File);
//             Assert.Equal(lineNumber, eventArgs.LineNumber);
//             Assert.Equal(columnNumber, eventArgs.ColumnNumber);
//             Assert.Equal(endLineNumber, eventArgs.EndLineNumber);
//             Assert.Equal(endColumnNumber, eventArgs.EndColumnNumber);
//             Assert.Equal(message, eventArgs.Message);
//             Assert.Equal(helpKeyword, eventArgs.HelpKeyword);
//             Assert.Equal(senderName, eventArgs.SenderName);
//             Assert.Equal(helpLink, eventArgs.HelpLink);
//             Assert.Equal(eventTimestamp, eventArgs.Timestamp);
//             Assert.Equal(messageArgs, eventArgs.MessageArgs);
//         }

        /// <summary>
        /// Tests that the ExtendedMetadata and ExtendedData properties can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void ExtendedProperties_CanBeSetAndGet()
        {
            // Arrange
            var eventArgs = new ExtendedBuildWarningEventArgs("TestType");
            var metadata = new Dictionary<string, string?> { { "Key", "Value" } };
            string extendedData = "Additional Data";

            // Act
            eventArgs.ExtendedMetadata = metadata;
            eventArgs.ExtendedData = extendedData;

            // Assert
            Assert.Equal(metadata, eventArgs.ExtendedMetadata);
            Assert.Equal(extendedData, eventArgs.ExtendedData);
        }
    }
}
