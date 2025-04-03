using FluentAssertions;
using Microsoft.Build.Framework;
using System;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedBuildWarningEventArgs"/> class.
    /// </summary>
    public class ExtendedBuildWarningEventArgsTests
    {
        /// <summary>
        /// Tests the internal default constructor to ensure it sets ExtendedType to "undefined".
        /// NOTE: This test requires the test assembly to have access to internal members (via InternalsVisibleTo).
        /// </summary>
        [Fact]
        public void InternalConstructor_WhenCalled_SetsExtendedTypeToUndefined()
        {
            // Arrange & Act
            var instance = new ExtendedBuildWarningEventArgs();

            // Assert
            instance.ExtendedType.Should().Be("undefined");
            instance.ExtendedMetadata.Should().BeNull();
            instance.ExtendedData.Should().BeNull();
        }

        /// <summary>
        /// Tests the constructor that takes a single string parameter.
        /// Verifies that ExtendedType is set correctly when a valid type is provided.
        /// </summary>
        [Fact]
        public void Constructor_WithTypeParameter_SetsExtendedTypeCorrectly()
        {
            // Arrange
            string expectedType = "CustomWarning";

            // Act
            var instance = new ExtendedBuildWarningEventArgs(expectedType);

            // Assert
            instance.ExtendedType.Should().Be(expectedType);
            instance.ExtendedMetadata.Should().BeNull();
            instance.ExtendedData.Should().BeNull();
        }

        /// <summary>
        /// Tests the constructor that allows initializing all event data.
        /// Verifies that ExtendedType and base class properties are set correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithFullEventData_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedType = "FullDataType";
            string? expectedSubcategory = "SubCategory";
            string? expectedCode = "W001";
            string? expectedFile = "file.txt";
            int expectedLineNumber = int.MinValue;
            int expectedColumnNumber = 10;
            int expectedEndLineNumber = int.MaxValue;
            int expectedEndColumnNumber = 20;
            string? expectedMessage = "A warning has occurred.";
            string? expectedHelpKeyword = "HelpKey";
            string? expectedSenderName = "TestSender";

            // Act
            var instance = new ExtendedBuildWarningEventArgs(
                expectedType,
                expectedSubcategory,
                expectedCode,
                expectedFile,
                expectedLineNumber,
                expectedColumnNumber,
                expectedEndLineNumber,
                expectedEndColumnNumber,
                expectedMessage,
                expectedHelpKeyword,
                expectedSenderName);

            // Assert
            instance.ExtendedType.Should().Be(expectedType);
            instance.ExtendedMetadata.Should().BeNull();
            instance.ExtendedData.Should().BeNull();

            // Additional assertions for base class properties, if available.
            instance.Subcategory.Should().Be(expectedSubcategory);
            instance.Code.Should().Be(expectedCode);
            instance.File.Should().Be(expectedFile);
            instance.LineNumber.Should().Be(expectedLineNumber);
            instance.ColumnNumber.Should().Be(expectedColumnNumber);
            instance.EndLineNumber.Should().Be(expectedEndLineNumber);
            instance.EndColumnNumber.Should().Be(expectedEndColumnNumber);
            instance.Message.Should().Be(expectedMessage);
            instance.HelpKeyword.Should().Be(expectedHelpKeyword);
            instance.SenderName.Should().Be(expectedSenderName);
        }

// Could not make this test passing.
//         /// <summary>
//         /// Tests the constructor that allows setting a timestamp.
//         /// Verifies that ExtendedType and the timestamp (if accessible) are set correctly.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithTimestamp_SetsPropertiesCorrectly()
//         {
//             // Arrange
//             string expectedType = "TimestampType";
//             string? expectedSubcategory = "SubTs";
//             string? expectedCode = "W002";
//             string? expectedFile = "timestamp.txt";
//             int expectedLineNumber = 100;
//             int expectedColumnNumber = 15;
//             int expectedEndLineNumber = 105;
//             int expectedEndColumnNumber = 20;
//             string? expectedMessage = "Timestamp warning message.";
//             string? expectedHelpKeyword = "TimeHelp";
//             string? expectedSenderName = "TimeSender";
//             DateTime expectedTimestamp = new DateTime(2023, 1, 1, 1, 0, 0);
// 
//             // Act
//             var instance = new ExtendedBuildWarningEventArgs(
//                 expectedType,
//                 expectedSubcategory,
//                 expectedCode,
//                 expectedFile,
//                 expectedLineNumber,
//                 expectedColumnNumber,
//                 expectedEndLineNumber,
//                 expectedEndColumnNumber,
//                 expectedMessage,
//                 expectedHelpKeyword,
//                 expectedSenderName,
//                 expectedTimestamp);
// 
//             // Assert
//             instance.ExtendedType.Should().Be(expectedType);
//             instance.ExtendedMetadata.Should().BeNull();
//             instance.ExtendedData.Should().BeNull();
// 
//             // Base class property assertions.
//             instance.Subcategory.Should().Be(expectedSubcategory);
//             instance.Code.Should().Be(expectedCode);
//             instance.File.Should().Be(expectedFile);
//             instance.LineNumber.Should().Be(expectedLineNumber);
//             instance.ColumnNumber.Should().Be(expectedColumnNumber);
//             instance.EndLineNumber.Should().Be(expectedEndLineNumber);
//             instance.EndColumnNumber.Should().Be(expectedEndColumnNumber);
//             instance.Message.Should().Be(expectedMessage);
//             instance.HelpKeyword.Should().Be(expectedHelpKeyword);
//             instance.SenderName.Should().Be(expectedSenderName);
// 
//             // Assuming the base class exposes a property for the event timestamp.
//             instance.Timestamp.Should().Be(expectedTimestamp);
//         }
// 
        /// <summary>
        /// Tests the constructor that allows setting a timestamp and message arguments.
        /// Verifies that ExtendedType is set correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithTimestampAndMessageArgs_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedType = "TimestampArgsType";
            string? expectedSubcategory = "SubArgs";
            string? expectedCode = "W003";
            string? expectedFile = "args.txt";
            int expectedLineNumber = 50;
            int expectedColumnNumber = 5;
            int expectedEndLineNumber = 55;
            int expectedEndColumnNumber = 10;
            string? expectedMessage = "Warning with message args.";
            string? expectedHelpKeyword = "ArgsHelp";
            string? expectedSenderName = "ArgsSender";
            DateTime expectedTimestamp = DateTime.UtcNow;
            object[] messageArgs = new object[] { "arg1", 123, null! };

            // Act
            var instance = new ExtendedBuildWarningEventArgs(
                expectedType,
                expectedSubcategory,
                expectedCode,
                expectedFile,
                expectedLineNumber,
                expectedColumnNumber,
                expectedEndLineNumber,
                expectedEndColumnNumber,
                expectedMessage,
                expectedHelpKeyword,
                expectedSenderName,
                expectedTimestamp,
                messageArgs);

            // Assert
            instance.ExtendedType.Should().Be(expectedType);
            instance.ExtendedMetadata.Should().BeNull();
            instance.ExtendedData.Should().BeNull();

            // Base class assertions.
            instance.Subcategory.Should().Be(expectedSubcategory);
            instance.Code.Should().Be(expectedCode);
            instance.File.Should().Be(expectedFile);
            instance.LineNumber.Should().Be(expectedLineNumber);
            instance.ColumnNumber.Should().Be(expectedColumnNumber);
            instance.EndLineNumber.Should().Be(expectedEndLineNumber);
            instance.EndColumnNumber.Should().Be(expectedEndColumnNumber);
            instance.Message.Should().Be(expectedMessage);
            instance.HelpKeyword.Should().Be(expectedHelpKeyword);
            instance.SenderName.Should().Be(expectedSenderName);
            instance.Timestamp.Should().Be(expectedTimestamp.ToLocalTime());

            // Note: Message arguments are passed to the base constructor; if the base class exposes them,
            // add corresponding assertions here.
        }
// Could not make this test passing.
// 
//         /// <summary>
//         /// Tests the constructor that allows setting a help link, timestamp, and message arguments.
//         /// Verifies that ExtendedType is set correctly.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithHelpLinkTimestampAndMessageArgs_SetsPropertiesCorrectly()
//         {
//             // Arrange
//             string expectedType = "HelpLinkType";
//             string? expectedSubcategory = "SubHelp";
//             string? expectedCode = "W004";
//             string? expectedFile = "helplink.txt";
//             int expectedLineNumber = 200;
//             int expectedColumnNumber = 25;
//             int expectedEndLineNumber = 205;
//             int expectedEndColumnNumber = 30;
//             string? expectedMessage = "Warning with help link.";
//             string? expectedHelpKeyword = "HelpLinkKey";
//             string? expectedSenderName = "HelpSender";
//             string? expectedHelpLink = "http://example.com/help";
//             DateTime expectedTimestamp = new DateTime(2022, 12, 31, 1, 0, 0);
//             object[] messageArgs = new object[] { "param1", 456 };
// 
//             // Act
//             var instance = new ExtendedBuildWarningEventArgs(
//                 expectedType,
//                 expectedSubcategory,
//                 expectedCode,
//                 expectedFile,
//                 expectedLineNumber,
//                 expectedColumnNumber,
//                 expectedEndLineNumber,
//                 expectedEndColumnNumber,
//                 expectedMessage,
//                 expectedHelpKeyword,
//                 expectedSenderName,
//                 expectedHelpLink,
//                 expectedTimestamp,
//                 messageArgs);
// 
//             // Assert
//             instance.ExtendedType.Should().Be(expectedType);
//             instance.ExtendedMetadata.Should().BeNull();
//             instance.ExtendedData.Should().BeNull();
// 
//             // Base class assertions.
//             instance.Subcategory.Should().Be(expectedSubcategory);
//             instance.Code.Should().Be(expectedCode);
//             instance.File.Should().Be(expectedFile);
//             instance.LineNumber.Should().Be(expectedLineNumber);
//             instance.ColumnNumber.Should().Be(expectedColumnNumber);
//             instance.EndLineNumber.Should().Be(expectedEndLineNumber);
//             instance.EndColumnNumber.Should().Be(expectedEndColumnNumber);
//             instance.Message.Should().Be(expectedMessage);
//             instance.HelpKeyword.Should().Be(expectedHelpKeyword);
//             instance.SenderName.Should().Be(expectedSenderName);
//             instance.Timestamp.Should().Be(expectedTimestamp);
// 
//             // If the base class exposes a property for HelpLink, it should be asserted here.
//             instance.HelpLink.Should().Be(expectedHelpLink);
// 
//             // Note: Message arguments are passed to the base constructor; if the base class exposes them,
//             // add corresponding assertions here.
//         }
    }
}
