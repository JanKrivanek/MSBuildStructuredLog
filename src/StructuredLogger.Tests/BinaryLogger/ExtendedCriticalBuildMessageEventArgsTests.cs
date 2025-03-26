using System;
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
        private const string DefaultSubcategory = "TestSubcategory";
        private const string DefaultCode = "TestCode";
        private const string DefaultFile = "testfile.txt";
        private const int DefaultLineNumber = 10;
        private const int DefaultColumnNumber = 5;
        private const int DefaultEndLineNumber = 15;
        private const int DefaultEndColumnNumber = 20;
        private const string DefaultMessage = "Test message";
        private const string DefaultHelpKeyword = "TestHelp";
        private const string DefaultSenderName = "TestSender";

        /// <summary>
        /// Tests the constructor with required parameters (without explicit timestamp) to ensure it correctly sets ExtendedType and default properties.
        /// </summary>
        [Fact]
        public void Constructor_WithRequiredParameters_SetsExtendedTypeAndDefaults()
        {
            // Arrange
            var expectedType = "CustomType";

            // Act
            var eventArgs = new ExtendedCriticalBuildMessageEventArgs(
                expectedType,
                DefaultSubcategory,
                DefaultCode,
                DefaultFile,
                DefaultLineNumber,
                DefaultColumnNumber,
                DefaultEndLineNumber,
                DefaultEndColumnNumber,
                DefaultMessage,
                DefaultHelpKeyword,
                DefaultSenderName);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Null(eventArgs.ExtendedMetadata);
            Assert.Null(eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests the constructor with explicit timestamp to ensure it correctly sets ExtendedType.
        /// </summary>
        [Fact]
        public void Constructor_WithTimestamp_SetsExtendedType()
        {
            // Arrange
            var expectedType = "TimestampType";
            var expectedTimestamp = DateTime.UtcNow;

            // Act
            var eventArgs = new ExtendedCriticalBuildMessageEventArgs(
                expectedType,
                DefaultSubcategory,
                DefaultCode,
                DefaultFile,
                DefaultLineNumber,
                DefaultColumnNumber,
                DefaultEndLineNumber,
                DefaultEndColumnNumber,
                DefaultMessage,
                DefaultHelpKeyword,
                DefaultSenderName,
                expectedTimestamp);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Null(eventArgs.ExtendedMetadata);
            Assert.Null(eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests the constructor with message arguments to ensure it correctly sets ExtendedType.
        /// </summary>
        [Fact]
        public void Constructor_WithMessageArgs_SetsExtendedType()
        {
            // Arrange
            var expectedType = "ArgsType";
            object[] messageArgs = new object[] { "arg1", 2, null };

            // Act
            var eventArgs = new ExtendedCriticalBuildMessageEventArgs(
                expectedType,
                DefaultSubcategory,
                DefaultCode,
                DefaultFile,
                DefaultLineNumber,
                DefaultColumnNumber,
                DefaultEndLineNumber,
                DefaultEndColumnNumber,
                DefaultMessage,
                DefaultHelpKeyword,
                DefaultSenderName,
                DateTime.UtcNow,
                messageArgs);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Null(eventArgs.ExtendedMetadata);
            Assert.Null(eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests the single parameter constructor to ensure it correctly sets ExtendedType.
        /// </summary>
        [Fact]
        public void SingleParameterConstructor_SetsExtendedType()
        {
            // Arrange
            var expectedType = "SingleType";

            // Act
            var eventArgs = new ExtendedCriticalBuildMessageEventArgs(expectedType);

            // Assert
            Assert.Equal(expectedType, eventArgs.ExtendedType);
            Assert.Null(eventArgs.ExtendedMetadata);
            Assert.Null(eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests setting and retrieving ExtendedMetadata and ExtendedData properties.
        /// </summary>
        [Fact]
        public void SetExtendedProperties_PropertiesAreSetAndRetrieved()
        {
            // Arrange
            var eventArgs = new ExtendedCriticalBuildMessageEventArgs("PropertyTest");
            var metadata = new Dictionary<string, string?>
            {
                { "Key1", "Value1" },
                { "Key2", null }
            };
            var extendedData = "Some extended data";

            // Act
            eventArgs.ExtendedMetadata = metadata;
            eventArgs.ExtendedData = extendedData;

            // Assert
            Assert.Equal(metadata, eventArgs.ExtendedMetadata);
            Assert.Equal(extendedData, eventArgs.ExtendedData);
        }

        /// <summary>
        /// Tests the constructors with empty string and null parameters for optional fields to ensure ExtendedType is set correctly.
        /// </summary>
        [Fact]
        public void Constructors_WithEmptyOrNullParameters_HandleGracefully()
        {
            // Arrange
            var expectedType = "";
            string? nullSubcategory = null;
            string? nullCode = null;
            string? nullFile = null;
            string? nullMessage = null;
            string? nullHelpKeyword = null;
            string? nullSenderName = null;
            DateTime timestamp = DateTime.UtcNow;
            object[]? emptyMessageArgs = null;

            // Act
            var eventArgs1 = new ExtendedCriticalBuildMessageEventArgs(
                expectedType,
                nullSubcategory,
                nullCode,
                nullFile,
                0,
                0,
                0,
                0,
                nullMessage,
                nullHelpKeyword,
                nullSenderName);

            var eventArgs2 = new ExtendedCriticalBuildMessageEventArgs(
                expectedType,
                nullSubcategory,
                nullCode,
                nullFile,
                0,
                0,
                0,
                0,
                nullMessage,
                nullHelpKeyword,
                nullSenderName,
                timestamp);

            var eventArgs3 = new ExtendedCriticalBuildMessageEventArgs(
                expectedType,
                nullSubcategory,
                nullCode,
                nullFile,
                0,
                0,
                0,
                0,
                nullMessage,
                nullHelpKeyword,
                nullSenderName,
                timestamp,
                emptyMessageArgs);

            // Assert
            Assert.Equal(expectedType, eventArgs1.ExtendedType);
            Assert.Equal(expectedType, eventArgs2.ExtendedType);
            Assert.Equal(expectedType, eventArgs3.ExtendedType);
        }
    }
}
