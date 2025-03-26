using System;
using System.Collections.Generic;
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
        /// Tests that the default (internal) constructor sets ExtendedType to "undefined".
        /// </summary>
        [Fact]
        public void DefaultConstructor_WhenCalled_SetsExtendedTypeToUndefined()
        {
            // Arrange & Act
            var eventArgs = new ExtendedBuildMessageEventArgs();

            // Assert
            Assert.Equal("undefined", eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the single parameter constructor sets ExtendedType as provided.
        /// </summary>
        [Fact]
        public void Constructor_WithTypeParameter_SetsExtendedTypeCorrectly()
        {
            // Arrange
            string expectedType = "CustomType";

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the constructor with message details copies provided properties correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithMessageInfo_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedType = "InfoType";
            string expectedMessage = "Test Message";
            string expectedHelpKeyword = "HelpKey";
            string expectedSenderName = "Sender";
            MessageImportance expectedImportance = MessageImportance.Low;

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, expectedMessage, expectedHelpKeyword, expectedSenderName, expectedImportance);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Equal(expectedHelpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(expectedSenderName, eventArgs.SenderName);
            Assert.Equal(expectedImportance, eventArgs.Importance);
        }

        /// <summary>
        /// Tests that the constructor with message info and timestamp sets properties correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithMessageInfoAndTimestamp_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedType = "TimestampType";
            string expectedMessage = "Timestamp Message";
            string expectedHelpKeyword = "TimestampHelp";
            string expectedSenderName = "TimestampSender";
            MessageImportance expectedImportance = MessageImportance.High;
            DateTime expectedTimestamp = new DateTime(2023, 1, 1, 12, 0, 0);

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, expectedMessage, expectedHelpKeyword, expectedSenderName, expectedImportance, expectedTimestamp);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Equal(expectedHelpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(expectedSenderName, eventArgs.SenderName);
            Assert.Equal(expectedImportance, eventArgs.Importance);
            Assert.Equal(expectedTimestamp, eventArgs.Timestamp);
        }

        /// <summary>
        /// Tests that the constructor with message info, timestamp, and message arguments sets ExtendedType and does not throw with provided message arguments.
        /// </summary>
        [Fact]
        public void Constructor_WithMessageInfoTimestampAndMessageArgs_SetsExtendedTypeCorrectly()
        {
            // Arrange
            string expectedType = "ArgsType";
            string expectedMessage = "Args Message";
            string expectedHelpKeyword = "ArgsHelp";
            string expectedSenderName = "ArgsSender";
            MessageImportance expectedImportance = MessageImportance.Normal;
            DateTime expectedTimestamp = DateTime.Now;
            object[] messageArgs = new object[] { "arg1", 2 };

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, expectedMessage, expectedHelpKeyword, expectedSenderName, expectedImportance, expectedTimestamp, messageArgs);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Equal(expectedHelpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(expectedSenderName, eventArgs.SenderName);
            Assert.Equal(expectedImportance, eventArgs.Importance);
            Assert.Equal(expectedTimestamp, eventArgs.Timestamp);
        }

        /// <summary>
        /// Tests that the full constructor (without timestamp) with detailed event information sets all properties correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithFullDetails_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedType = "FullType";
            string expectedSubcategory = "SubCat";
            string expectedCode = "Code123";
            string expectedFile = "File.cs";
            int expectedLineNumber = 10;
            int expectedColumnNumber = 5;
            int expectedEndLineNumber = 10;
            int expectedEndColumnNumber = 15;
            string expectedMessage = "Full message";
            string expectedHelpKeyword = "FullHelp";
            string expectedSenderName = "FullSender";
            MessageImportance expectedImportance = MessageImportance.Low;

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, expectedSubcategory, expectedCode, expectedFile,
                expectedLineNumber, expectedColumnNumber, expectedEndLineNumber, expectedEndColumnNumber,
                expectedMessage, expectedHelpKeyword, expectedSenderName, expectedImportance);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Equal(expectedSubcategory, eventArgs.Subcategory);
            Assert.Equal(expectedCode, eventArgs.Code);
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLineNumber, eventArgs.LineNumber);
            Assert.Equal(expectedColumnNumber, eventArgs.ColumnNumber);
            Assert.Equal(expectedEndLineNumber, eventArgs.EndLineNumber);
            Assert.Equal(expectedEndColumnNumber, eventArgs.EndColumnNumber);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Equal(expectedHelpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(expectedSenderName, eventArgs.SenderName);
            Assert.Equal(expectedImportance, eventArgs.Importance);
        }

        /// <summary>
        /// Tests that the full constructor with detailed event information and timestamp sets properties correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithFullDetailsAndTimestamp_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedType = "FullTimestampType";
            string expectedSubcategory = "SubCatTS";
            string expectedCode = "CodeTS";
            string expectedFile = "FileTS.cs";
            int expectedLineNumber = 20;
            int expectedColumnNumber = 10;
            int expectedEndLineNumber = 20;
            int expectedEndColumnNumber = 30;
            string expectedMessage = "Timestamp full message";
            string expectedHelpKeyword = "TSHelp";
            string expectedSenderName = "TSSender";
            MessageImportance expectedImportance = MessageImportance.High;
            DateTime expectedTimestamp = new DateTime(2023, 6, 15, 9, 30, 0);

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, expectedSubcategory, expectedCode, expectedFile,
                expectedLineNumber, expectedColumnNumber, expectedEndLineNumber, expectedEndColumnNumber,
                expectedMessage, expectedHelpKeyword, expectedSenderName, expectedImportance, expectedTimestamp);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Equal(expectedSubcategory, eventArgs.Subcategory);
            Assert.Equal(expectedCode, eventArgs.Code);
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLineNumber, eventArgs.LineNumber);
            Assert.Equal(expectedColumnNumber, eventArgs.ColumnNumber);
            Assert.Equal(expectedEndLineNumber, eventArgs.EndLineNumber);
            Assert.Equal(expectedEndColumnNumber, eventArgs.EndColumnNumber);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Equal(expectedHelpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(expectedSenderName, eventArgs.SenderName);
            Assert.Equal(expectedImportance, eventArgs.Importance);
            Assert.Equal(expectedTimestamp, eventArgs.Timestamp);
        }

        /// <summary>
        /// Tests that the full constructor with detailed event information, timestamp, and message arguments sets properties correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithFullDetailsTimestampAndMessageArgs_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedType = "FullArgsType";
            string expectedSubcategory = "SubCatArgs";
            string expectedCode = "CodeArgs";
            string expectedFile = "FileArgs.cs";
            int expectedLineNumber = 30;
            int expectedColumnNumber = 15;
            int expectedEndLineNumber = 30;
            int expectedEndColumnNumber = 25;
            string expectedMessage = "Full args message";
            string expectedHelpKeyword = "ArgsHelp";
            string expectedSenderName = "ArgsSender";
            MessageImportance expectedImportance = MessageImportance.Normal;
            DateTime expectedTimestamp = new DateTime(2022, 12, 31, 23, 59, 59);
            object[] messageArgs = new object[] { "param1", 42 };

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, expectedSubcategory, expectedCode, expectedFile,
                expectedLineNumber, expectedColumnNumber, expectedEndLineNumber, expectedEndColumnNumber,
                expectedMessage, expectedHelpKeyword, expectedSenderName, expectedImportance, expectedTimestamp, messageArgs);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Equal(expectedSubcategory, eventArgs.Subcategory);
            Assert.Equal(expectedCode, eventArgs.Code);
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLineNumber, eventArgs.LineNumber);
            Assert.Equal(expectedColumnNumber, eventArgs.ColumnNumber);
            Assert.Equal(expectedEndLineNumber, eventArgs.EndLineNumber);
            Assert.Equal(expectedEndColumnNumber, eventArgs.EndColumnNumber);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Equal(expectedHelpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(expectedSenderName, eventArgs.SenderName);
            Assert.Equal(expectedImportance, eventArgs.Importance);
            Assert.Equal(expectedTimestamp, eventArgs.Timestamp);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property can be set and retrieved.
        /// </summary>
        [Fact]
        public void ExtendedMetadataProperty_WhenSet_CanBeRetrieved()
        {
            // Arrange
            var eventArgs = new ExtendedBuildMessageEventArgs("MetadataType");
            var metadata = new Dictionary<string, string?>()
            {
                {"Key1", "Value1" },
                {"Key2", null }
            };

            // Act
            eventArgs.ExtendedMetadata = metadata;

            // Assert
            Assert.Equal(metadata, eventArgs.ExtendedMetadata);
        }

        /// <summary>
        /// Tests that the ExtendedData property can be set and retrieved.
        /// </summary>
        [Fact]
        public void ExtendedDataProperty_WhenSet_CanBeRetrieved()
        {
            // Arrange
            var eventArgs = new ExtendedBuildMessageEventArgs("DataType");
            string expectedData = "Some extended data";

            // Act
            eventArgs.ExtendedData = expectedData;

            // Assert
            Assert.Equal(expectedData, eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests the constructors with null optional parameters to ensure they handle null values gracefully.
        /// </summary>
        [Fact]
        public void Constructors_WithNullOptionalParameters_HandleNullValues()
        {
            // Arrange
            string expectedType = "NullTest";
            MessageImportance expectedImportance = MessageImportance.Low;
            DateTime now = DateTime.UtcNow;

            // Act
            var eventArgs1 = new ExtendedBuildMessageEventArgs(expectedType, null, null, null, expectedImportance);
            var eventArgs2 = new ExtendedBuildMessageEventArgs(expectedType, null, null, null, expectedImportance, now);
            var eventArgs3 = new ExtendedBuildMessageEventArgs(expectedType, null, null, null, expectedImportance, now, null);
            var eventArgs4 = new ExtendedBuildMessageEventArgs(expectedType, null, null, null, 0, 0, 0, 0, null, null, null, expectedImportance);
            var eventArgs5 = new ExtendedBuildMessageEventArgs(expectedType, null, null, null, 0, 0, 0, 0, null, null, null, expectedImportance, now);
            var eventArgs6 = new ExtendedBuildMessageEventArgs(expectedType, null, null, null, 0, 0, 0, 0, null, null, null, expectedImportance, now, null);

            // Assert
            Assert.Equal(expectedType, eventArgs1.ExtendedType);
            Assert.Null(eventArgs1.Message);
            Assert.Null(eventArgs1.HelpKeyword);
            Assert.Null(eventArgs1.SenderName);

            Assert.Equal(expectedType, eventArgs2.ExtendedType);
            Assert.Null(eventArgs2.Message);
            Assert.Null(eventArgs2.HelpKeyword);
            Assert.Null(eventArgs2.SenderName);
            Assert.Equal(now, eventArgs2.Timestamp);

            Assert.Equal(expectedType, eventArgs3.ExtendedType);
            Assert.Null(eventArgs3.Message);
            Assert.Null(eventArgs3.HelpKeyword);
            Assert.Null(eventArgs3.SenderName);
            Assert.Equal(now, eventArgs3.Timestamp);

            Assert.Equal(expectedType, eventArgs4.ExtendedType);
            Assert.Null(eventArgs4.Message);
            Assert.Null(eventArgs4.HelpKeyword);
            Assert.Null(eventArgs4.SenderName);
            Assert.Equal(0, eventArgs4.LineNumber);
            Assert.Equal(0, eventArgs4.ColumnNumber);
            Assert.Equal(0, eventArgs4.EndLineNumber);
            Assert.Equal(0, eventArgs4.EndColumnNumber);

            Assert.Equal(expectedType, eventArgs5.ExtendedType);
            Assert.Null(eventArgs5.Message);
            Assert.Null(eventArgs5.HelpKeyword);
            Assert.Null(eventArgs5.SenderName);
            Assert.Equal(now, eventArgs5.Timestamp);

            Assert.Equal(expectedType, eventArgs6.ExtendedType);
            Assert.Null(eventArgs6.Message);
            Assert.Null(eventArgs6.HelpKeyword);
            Assert.Null(eventArgs6.SenderName);
            Assert.Equal(now, eventArgs6.Timestamp);
        }
    }
}
