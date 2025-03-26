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
        /// Tests that the internal default constructor sets ExtendedType to "undefined".
        /// </summary>
        [Fact]
        public void DefaultConstructor_ShouldSetExtendedTypeToUndefined()
        {
            // Arrange & Act
            var eventArgs = new ExtendedBuildMessageEventArgs();

            // Assert
            Assert.Equal("undefined", eventArgs.ExtendedType);
            Assert.Null(eventArgs.ExtendedMetadata);
            Assert.Null(eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests that the constructor accepting only type correctly sets the ExtendedType property.
        /// </summary>
        [Fact]
        public void Constructor_WithType_ShouldSetExtendedType()
        {
            // Arrange
            string expectedType = "CustomType";

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Null(eventArgs.ExtendedMetadata);
            Assert.Null(eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests that the constructor with message, helpKeyword, senderName, and importance correctly sets ExtendedType.
        /// </summary>
        [Fact]
        public void Constructor_WithMessageParameters_ShouldSetExtendedType()
        {
            // Arrange
            string expectedType = "Type1";
            string message = "Test message";
            string helpKeyword = "TestHelp";
            string senderName = "TestSender";
            MessageImportance importance = MessageImportance.High;

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, message, helpKeyword, senderName, importance);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Null(eventArgs.ExtendedMetadata);
            Assert.Null(eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests that the constructor with a timestamp correctly sets ExtendedType.
        /// </summary>
        [Fact]
        public void Constructor_WithTimestamp_ShouldSetExtendedType()
        {
            // Arrange
            string expectedType = "Type2";
            string message = "Test message";
            string helpKeyword = "TestHelp";
            string senderName = "TestSender";
            MessageImportance importance = MessageImportance.Normal;
            DateTime timestamp = DateTime.Now;

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, message, helpKeyword, senderName, importance, timestamp);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Null(eventArgs.ExtendedMetadata);
            Assert.Null(eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests that the constructor with a timestamp and message arguments correctly sets ExtendedType.
        /// </summary>
        [Fact]
        public void Constructor_WithTimestampAndMessageArgs_ShouldSetExtendedType()
        {
            // Arrange
            string expectedType = "Type3";
            string message = "Test message";
            string helpKeyword = "TestHelp";
            string senderName = "TestSender";
            MessageImportance importance = MessageImportance.High;
            DateTime timestamp = DateTime.Now;
            object[] messageArgs = { "arg1", 42 };

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, message, helpKeyword, senderName, importance, timestamp, messageArgs);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Null(eventArgs.ExtendedMetadata);
            Assert.Null(eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests that the constructor with detailed file and position information correctly sets ExtendedType.
        /// </summary>
        [Fact]
        public void Constructor_WithFileAndPositionParameters_ShouldSetExtendedType()
        {
            // Arrange
            string expectedType = "Type4";
            string subcategory = "SubCat";
            string code = "Code";
            string file = "file.cs";
            int lineNumber = 10;
            int columnNumber = 20;
            int endLineNumber = 30;
            int endColumnNumber = 40;
            string message = "Test message";
            string helpKeyword = "TestHelp";
            string senderName = "TestSender";
            MessageImportance importance = MessageImportance.Low;

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, importance);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the constructor with file parameters and timestamp correctly sets ExtendedType.
        /// </summary>
        [Fact]
        public void Constructor_WithFileParametersAndTimestamp_ShouldSetExtendedType()
        {
            // Arrange
            string expectedType = "Type5";
            string subcategory = "SubCat";
            string code = "Code";
            string file = "file.cs";
            int lineNumber = 10;
            int columnNumber = 20;
            int endLineNumber = 30;
            int endColumnNumber = 40;
            string message = "Test message";
            string helpKeyword = "TestHelp";
            string senderName = "TestSender";
            MessageImportance importance = MessageImportance.Normal;
            DateTime timestamp = DateTime.Now;

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, importance, timestamp);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the constructor with file parameters, timestamp, and message arguments correctly sets ExtendedType.
        /// </summary>
        [Fact]
        public void Constructor_WithFileParametersTimestampAndMessageArgs_ShouldSetExtendedType()
        {
            // Arrange
            string expectedType = "Type6";
            string subcategory = "SubCat";
            string code = "Code";
            string file = "file.cs";
            int lineNumber = 10;
            int columnNumber = 20;
            int endLineNumber = 30;
            int endColumnNumber = 40;
            string message = "Test message";
            string helpKeyword = "TestHelp";
            string senderName = "TestSender";
            MessageImportance importance = MessageImportance.High;
            DateTime timestamp = DateTime.Now;
            object[] messageArgs = { "arg1", "arg2" };

            // Act
            var eventArgs = new ExtendedBuildMessageEventArgs(expectedType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, importance, timestamp, messageArgs);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_Property_SetAndGet_ShouldPersistValue()
        {
            // Arrange
            var eventArgs = new ExtendedBuildMessageEventArgs("TypeForMetadata");
            var metadata = new Dictionary<string, string?> { { "Key1", "Value1" }, { "Key2", null } };

            // Act
            eventArgs.ExtendedMetadata = metadata;

            // Assert
            Assert.Equal(metadata, eventArgs.ExtendedMetadata);
        }

        /// <summary>
        /// Tests that the ExtendedData property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void ExtendedData_Property_SetAndGet_ShouldPersistValue()
        {
            // Arrange
            var eventArgs = new ExtendedBuildMessageEventArgs("TypeForData");
            string expectedData = "Some extended data";

            // Act
            eventArgs.ExtendedData = expectedData;

            // Assert
            Assert.Equal(expectedData, eventArgs.ExtendedData);
        }
    }
}
