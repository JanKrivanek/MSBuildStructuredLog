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
        /// Tests that the internal default constructor sets ExtendedType to "undefined".
        /// </summary>
        [Fact]
        public void InternalDefaultConstructor_SetsExtendedTypeToUndefined()
        {
            // Arrange & Act
            var instance = new ExtendedBuildWarningEventArgs_InternalAccessor();

            // Assert
            Assert.Equal("undefined", instance.GetExtendedType());
        }

        /// <summary>
        /// Tests that the constructor with a single type parameter sets the ExtendedType property correctly.
        /// </summary>
        /// <param name="type">The type string to assign.</param>
        [Theory]
        [InlineData("CustomType")]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_WithTypeParameter_SetsExtendedType(string type)
        {
            // Arrange & Act
            var instance = new ExtendedBuildWarningEventArgs(type);

            // Assert
            Assert.Equal(type, instance.ExtendedType);
        }

        /// <summary>
        /// Tests that the full constructor without event timestamp sets the ExtendedType and base properties correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedType = "TestType";
            string expectedSubcategory = "SubCat";
            string expectedCode = "Code123";
            string expectedFile = "file.cs";
            int expectedLineNumber = 10;
            int expectedColumnNumber = 20;
            int expectedEndLineNumber = 30;
            int expectedEndColumnNumber = 40;
            string expectedMessage = "Warning message";
            string expectedHelpKeyword = "HelpKey";
            string expectedSenderName = "Sender";

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
            Assert.Equal(expectedType, instance.ExtendedType);
            // Assuming the base properties are publicly accessible.
            Assert.Equal(expectedSubcategory, instance.Subcategory);
            Assert.Equal(expectedCode, instance.Code);
            Assert.Equal(expectedFile, instance.File);
            Assert.Equal(expectedLineNumber, instance.LineNumber);
            Assert.Equal(expectedColumnNumber, instance.ColumnNumber);
            Assert.Equal(expectedEndLineNumber, instance.EndLineNumber);
            Assert.Equal(expectedEndColumnNumber, instance.EndColumnNumber);
            Assert.Equal(expectedMessage, instance.Message);
            Assert.Equal(expectedHelpKeyword, instance.HelpKeyword);
            Assert.Equal(expectedSenderName, instance.SenderName);
        }

        /// <summary>
        /// Tests that the constructor including an event timestamp sets the ExtendedType and Timestamp property correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithEventTimestamp_SetsTimestampCorrectly()
        {
            // Arrange
            string expectedType = "TestType";
            string expectedSubcategory = "SubCat";
            string expectedCode = "Code123";
            string expectedFile = "file.cs";
            int expectedLineNumber = 1;
            int expectedColumnNumber = 2;
            int expectedEndLineNumber = 3;
            int expectedEndColumnNumber = 4;
            string expectedMessage = "Warning";
            string expectedHelpKeyword = "Help";
            string expectedSenderName = "Sender";
            DateTime expectedTimestamp = DateTime.Now;

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
                expectedTimestamp);

            // Assert
            Assert.Equal(expectedType, instance.ExtendedType);
            // Verify that the timestamp is set correctly. Assuming the base class exposes a 'Timestamp' property.
            Assert.Equal(expectedTimestamp, instance.Timestamp);
        }

        /// <summary>
        /// Tests that the constructor with event timestamp and message arguments handles null for messageArgs.
        /// </summary>
        [Fact]
        public void Constructor_WithEventTimestampAndMessageArgs_NullMessageArgs_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedType = "TestType";
            string expectedSubcategory = "SubCat";
            string expectedCode = "Code123";
            string expectedFile = "file.cs";
            int expectedLineNumber = 5;
            int expectedColumnNumber = 6;
            int expectedEndLineNumber = 7;
            int expectedEndColumnNumber = 8;
            string expectedMessage = "Test message";
            string expectedHelpKeyword = "Help";
            string expectedSenderName = "Sender";
            DateTime expectedTimestamp = DateTime.UtcNow;
            object[] messageArgs = null; // Explicitly passing null.

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
            Assert.Equal(expectedType, instance.ExtendedType);
            Assert.Equal(expectedTimestamp, instance.Timestamp);
            // There's no public MessageArgs property to verify, so successful construction is sufficient.
        }

        /// <summary>
        /// Tests that the constructor with helpLink and message arguments sets the ExtendedType, helpLink, and Timestamp properties correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithHelpLink_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedType = "TestType";
            string expectedSubcategory = "Cat";
            string expectedCode = "C123";
            string expectedFile = "file.cs";
            int expectedLineNumber = 100;
            int expectedColumnNumber = 200;
            int expectedEndLineNumber = 300;
            int expectedEndColumnNumber = 400;
            string expectedMessage = "A warning occurred";
            string expectedHelpKeyword = "Keyword";
            string expectedSenderName = "Sender";
            string expectedHelpLink = "http://help.link";
            DateTime expectedTimestamp = DateTime.Now;
            object[] expectedMessageArgs = new object[] { "arg1", 2 };

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
                expectedHelpLink,
                expectedTimestamp,
                expectedMessageArgs);

            // Assert
            Assert.Equal(expectedType, instance.ExtendedType);
            // Verify that the helpLink is set if the base class exposes the HelpLink property.
            Assert.Equal(expectedHelpLink, instance.HelpLink);
            Assert.Equal(expectedTimestamp, instance.Timestamp);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property can be set and retrieved.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_PropertySetAndGet_Works()
        {
            // Arrange
            var instance = new ExtendedBuildWarningEventArgs("TestType");
            var expectedMetadata = new Dictionary<string, string?>()
            {
                { "Key1", "Value1" },
                { "Key2", null }
            };

            // Act
            instance.ExtendedMetadata = expectedMetadata;

            // Assert
            Assert.Equal(expectedMetadata, instance.ExtendedMetadata);
        }

        /// <summary>
        /// Tests that the ExtendedData property can be set and retrieved.
        /// </summary>
        [Fact]
        public void ExtendedData_PropertySetAndGet_Works()
        {
            // Arrange
            var instance = new ExtendedBuildWarningEventArgs("TestType");
            string expectedData = "Some extended data";

            // Act
            instance.ExtendedData = expectedData;

            // Assert
            Assert.Equal(expectedData, instance.ExtendedData);
        }
    }

    /// <summary>
    /// Internal accessor class to facilitate testing of internal constructors of ExtendedBuildWarningEventArgs.
    /// </summary>
    internal class ExtendedBuildWarningEventArgs_InternalAccessor : ExtendedBuildWarningEventArgs
    {
        public ExtendedBuildWarningEventArgs_InternalAccessor() : base("undefined")
        {
        }

        public string GetExtendedType() => this.ExtendedType;
    }
}
