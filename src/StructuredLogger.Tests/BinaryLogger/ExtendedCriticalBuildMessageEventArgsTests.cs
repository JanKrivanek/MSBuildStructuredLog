using System.Collections.Generic;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedCriticalBuildMessageEventArgs"/> class.
    /// </summary>
    public class ExtendedCriticalBuildMessageEventArgsTests
    {
        private readonly string _testType = "TestType";
        private readonly string? _testSubcategory = "TestSubcategory";
        private readonly string? _testCode = "TestCode";
        private readonly string? _testFile = "TestFile.log";
        private readonly int _testLineNumber = 10;
        private readonly int _testColumnNumber = 20;
        private readonly int _testEndLineNumber = 15;
        private readonly int _testEndColumnNumber = 25;
        private readonly string? _testMessage = "Test Message";
        private readonly string? _testHelpKeyword = "TestHelp";
        private readonly string? _testSenderName = "TestSender";
        private readonly DateTime _fixedTimestamp = new DateTime(2023, 10, 10, 12, 0, 0, DateTimeKind.Utc);
        private readonly object[] _testMessageArgs = new object[] { "arg1", 123 };

        /// <summary>
        /// Tests that the 11-parameter constructor properly sets ExtendedType when valid parameters are provided.
        /// </summary>
        [Fact]
        public void Constructor_11Params_ValidParameters_SetsExtendedType()
        {
            // Arrange & Act
            var eventArgs = new ExtendedCriticalBuildMessageEventArgs(
                _testType,
                _testSubcategory,
                _testCode,
                _testFile,
                _testLineNumber,
                _testColumnNumber,
                _testEndLineNumber,
                _testEndColumnNumber,
                _testMessage,
                _testHelpKeyword,
                _testSenderName);

            // Assert
            Assert.Equal(_testType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the 12-parameter constructor properly sets ExtendedType when valid parameters and a fixed timestamp are provided.
        /// </summary>
        [Fact]
        public void Constructor_12Params_ValidParameters_SetsExtendedType()
        {
            // Arrange & Act
            var eventArgs = new ExtendedCriticalBuildMessageEventArgs(
                _testType,
                _testSubcategory,
                _testCode,
                _testFile,
                _testLineNumber,
                _testColumnNumber,
                _testEndLineNumber,
                _testEndColumnNumber,
                _testMessage,
                _testHelpKeyword,
                _testSenderName,
                _fixedTimestamp);

            // Assert
            Assert.Equal(_testType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the 13-parameter constructor properly sets ExtendedType when valid parameters, including message arguments, are provided.
        /// </summary>
        [Fact]
        public void Constructor_13Params_ValidParameters_SetsExtendedType()
        {
            // Arrange & Act
            var eventArgs = new ExtendedCriticalBuildMessageEventArgs(
                _testType,
                _testSubcategory,
                _testCode,
                _testFile,
                _testLineNumber,
                _testColumnNumber,
                _testEndLineNumber,
                _testEndColumnNumber,
                _testMessage,
                _testHelpKeyword,
                _testSenderName,
                _fixedTimestamp,
                _testMessageArgs);

            // Assert
            Assert.Equal(_testType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the single-parameter constructor properly sets ExtendedType.
        /// </summary>
        [Fact]
        public void Constructor_StringOnly_SetsExtendedTypeProperly()
        {
            // Arrange
            string expectedType = "OnlyType";

            // Act
            var eventArgs = new ExtendedCriticalBuildMessageEventArgs(expectedType);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void Property_SetExtendedMetadata_SetsValue()
        {
            // Arrange
            var eventArgs = new ExtendedCriticalBuildMessageEventArgs(_testType);
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
        public void Property_SetExtendedData_SetsValue()
        {
            // Arrange
            var eventArgs = new ExtendedCriticalBuildMessageEventArgs(_testType);
            string expectedData = "Extended additional data";

            // Act
            eventArgs.ExtendedData = expectedData;

            // Assert
            Assert.Equal(expectedData, eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests that constructors correctly handle empty string values for type.
        /// </summary>
        [Fact]
        public void Constructor_EmptyType_SetsExtendedTypeToEmpty()
        {
            // Arrange
            string emptyType = string.Empty;

            // Act
            var eventArgs11 = new ExtendedCriticalBuildMessageEventArgs(
                emptyType,
                _testSubcategory,
                _testCode,
                _testFile,
                _testLineNumber,
                _testColumnNumber,
                _testEndLineNumber,
                _testEndColumnNumber,
                _testMessage,
                _testHelpKeyword,
                _testSenderName);

            var eventArgs12 = new ExtendedCriticalBuildMessageEventArgs(
                emptyType,
                _testSubcategory,
                _testCode,
                _testFile,
                _testLineNumber,
                _testColumnNumber,
                _testEndLineNumber,
                _testEndColumnNumber,
                _testMessage,
                _testHelpKeyword,
                _testSenderName,
                _fixedTimestamp);

            var eventArgs13 = new ExtendedCriticalBuildMessageEventArgs(
                emptyType,
                _testSubcategory,
                _testCode,
                _testFile,
                _testLineNumber,
                _testColumnNumber,
                _testEndLineNumber,
                _testEndColumnNumber,
                _testMessage,
                _testHelpKeyword,
                _testSenderName,
                _fixedTimestamp,
                _testMessageArgs);

            var eventArgsSingle = new ExtendedCriticalBuildMessageEventArgs(emptyType);

            // Assert
            Assert.Equal(emptyType, eventArgs11.ExtendedType);
            Assert.Equal(emptyType, eventArgs12.ExtendedType);
            Assert.Equal(emptyType, eventArgs13.ExtendedType);
            Assert.Equal(emptyType, eventArgsSingle.ExtendedType);
        }

        /// <summary>
        /// Tests that constructors correctly handle null values for nullable parameters.
        /// </summary>
        [Fact]
        public void Constructor_NullNullableParameters_DoesNotThrowAndSetsExtendedType()
        {
            // Arrange
            string typeValue = "NullTest";

            // Act
            var eventArgs11 = new ExtendedCriticalBuildMessageEventArgs(
                typeValue,
                null,
                null,
                null,
                0,
                0,
                0,
                0,
                null,
                null,
                null);

            var eventArgs12 = new ExtendedCriticalBuildMessageEventArgs(
                typeValue,
                null,
                null,
                null,
                0,
                0,
                0,
                0,
                null,
                null,
                null,
                _fixedTimestamp);

            var eventArgs13 = new ExtendedCriticalBuildMessageEventArgs(
                typeValue,
                null,
                null,
                null,
                0,
                0,
                0,
                0,
                null,
                null,
                null,
                _fixedTimestamp,
                null);

            // Assert
            Assert.Equal(typeValue, eventArgs11.ExtendedType);
            Assert.Equal(typeValue, eventArgs12.ExtendedType);
            Assert.Equal(typeValue, eventArgs13.ExtendedType);
        }
    }
}
