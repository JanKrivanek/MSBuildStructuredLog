using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedBuildErrorEventArgs"/> class.
    /// </summary>
    public class ExtendedBuildErrorEventArgsTests
    {
        private const string DefaultTypeString = "undefined";
        private const string CustomType = "CustomErrorType";

        /// <summary>
        /// Tests that the internal parameterless constructor initializes the ExtendedType property to "undefined".
        /// </summary>
        [Fact]
        public void InternalConstructor_ShouldInitializeWithUndefined()
        {
            // Arrange
            // Get the internal parameterless constructor using reflection.
            ConstructorInfo? internalCtor = typeof(ExtendedBuildErrorEventArgs).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                binder: null,
                types: Type.EmptyTypes,
                modifiers: null);
            Assert.NotNull(internalCtor);

            // Act
            var instance = (ExtendedBuildErrorEventArgs)internalCtor.Invoke(null);

            // Assert
            Assert.Equal(DefaultTypeString, instance.ExtendedType);
            Assert.Null(instance.ExtendedData);
            Assert.Null(instance.ExtendedMetadata);
        }

        /// <summary>
        /// Tests that the constructor with only a type parameter correctly sets the ExtendedType property.
        /// </summary>
        [Fact]
        public void Constructor_WithTypeParameter_ShouldSetExtendedType()
        {
            // Arrange & Act
            var instance = new ExtendedBuildErrorEventArgs(CustomType);

            // Assert
            Assert.Equal(CustomType, instance.ExtendedType);
            Assert.Null(instance.ExtendedData);
            Assert.Null(instance.ExtendedMetadata);
        }

        /// <summary>
        /// Tests that the constructor with 11 parameters correctly sets the ExtendedType property.
        /// </summary>
        [Fact]
        public void Constructor_WithAllParameters_ShouldSetExtendedType()
        {
            // Arrange
            string subcategory = "sub";
            string code = "code";
            string file = "file.cs";
            int lineNumber = 10;
            int columnNumber = 20;
            int endLineNumber = 15;
            int endColumnNumber = 25;
            string message = "Error message";
            string helpKeyword = "help";
            string senderName = "sender";

            // Act
            var instance = new ExtendedBuildErrorEventArgs(CustomType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName);

            // Assert
            Assert.Equal(CustomType, instance.ExtendedType);
            Assert.Null(instance.ExtendedData);
            Assert.Null(instance.ExtendedMetadata);
        }

        /// <summary>
        /// Tests that the constructor with eventTimestamp (12 parameters) correctly sets the ExtendedType property.
        /// </summary>
        [Fact]
        public void Constructor_WithTimestamp_ShouldSetExtendedType()
        {
            // Arrange
            string subcategory = "subTimestamp";
            string code = "codeTimestamp";
            string file = "fileTimestamp.cs";
            int lineNumber = 11;
            int columnNumber = 21;
            int endLineNumber = 16;
            int endColumnNumber = 26;
            string message = "Timestamp error message";
            string helpKeyword = "helpTimestamp";
            string senderName = "senderTimestamp";
            DateTime eventTimestamp = DateTime.Now;

            // Act
            var instance = new ExtendedBuildErrorEventArgs(CustomType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, eventTimestamp);

            // Assert
            Assert.Equal(CustomType, instance.ExtendedType);
        }

        /// <summary>
        /// Tests that the constructor with eventTimestamp and messageArgs (13 parameters) correctly sets the ExtendedType property.
        /// </summary>
        [Fact]
        public void Constructor_WithTimestampAndMessageArgs_ShouldSetExtendedType()
        {
            // Arrange
            string subcategory = "subArgs";
            string code = "codeArgs";
            string file = "fileArgs.cs";
            int lineNumber = 12;
            int columnNumber = 22;
            int endLineNumber = 17;
            int endColumnNumber = 27;
            string message = "Message args error";
            string helpKeyword = "helpArgs";
            string senderName = "senderArgs";
            DateTime eventTimestamp = DateTime.UtcNow;
            object[] messageArgs = new object[] { "arg1", 2, null };

            // Act
            var instance = new ExtendedBuildErrorEventArgs(CustomType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, eventTimestamp, messageArgs);

            // Assert
            Assert.Equal(CustomType, instance.ExtendedType);
        }

        /// <summary>
        /// Tests that the constructor with helpLink, eventTimestamp and messageArgs (14 parameters) correctly sets the ExtendedType property.
        /// </summary>
        [Fact]
        public void Constructor_WithHelpLinkAndArgs_ShouldSetExtendedType()
        {
            // Arrange
            string subcategory = "subLink";
            string code = "codeLink";
            string file = "fileLink.cs";
            int lineNumber = 13;
            int columnNumber = 23;
            int endLineNumber = 18;
            int endColumnNumber = 28;
            string message = "HelpLink error message";
            string helpKeyword = "helpLink";
            string senderName = "senderLink";
            string helpLink = "http://helplink";
            DateTime eventTimestamp = DateTime.UtcNow;
            object[] messageArgs = new object[] { "argLink1", 3 };

            // Act
            var instance = new ExtendedBuildErrorEventArgs(CustomType, subcategory, code, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, helpKeyword, senderName, helpLink, eventTimestamp, messageArgs);

            // Assert
            Assert.Equal(CustomType, instance.ExtendedType);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void ExtendedMetadataProperty_CanBeSetAndRetrieved()
        {
            // Arrange
            var instance = new ExtendedBuildErrorEventArgs(CustomType);
            var metadata = new Dictionary<string, string?> { { "Key1", "Value1" }, { "Key2", null } };

            // Act
            instance.ExtendedMetadata = metadata;

            // Assert
            Assert.NotNull(instance.ExtendedMetadata);
            Assert.Equal(2, instance.ExtendedMetadata.Count);
            Assert.Equal("Value1", instance.ExtendedMetadata["Key1"]);
            Assert.Null(instance.ExtendedMetadata["Key2"]);
        }

        /// <summary>
        /// Tests that the ExtendedData property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void ExtendedDataProperty_CanBeSetAndRetrieved()
        {
            // Arrange
            var instance = new ExtendedBuildErrorEventArgs(CustomType);
            string testData = "Some extended data";

            // Act
            instance.ExtendedData = testData;

            // Assert
            Assert.Equal(testData, instance.ExtendedData);
        }

        /// <summary>
        /// Tests that the ExtendedType property setter allows changing its value.
        /// </summary>
        [Fact]
        public void ExtendedTypeProperty_Setter_AllowsValueChange()
        {
            // Arrange
            var instance = new ExtendedBuildErrorEventArgs(CustomType);
            string newType = "NewErrorType";

            // Act
            instance.ExtendedType = newType;

            // Assert
            Assert.Equal(newType, instance.ExtendedType);
        }
    }
}
