using Microsoft.Build.Framework;
using StructuredLogger.BinaryLogger;
using System;
using Xunit;

namespace StructuredLogger.BinaryLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedPropertyInitialValueSetEventArgs"/> class.
    /// </summary>
    public class ExtendedPropertyInitialValueSetEventArgsTests
    {
        /// <summary>
        /// Tests that the constructor assigns all the provided properties correctly when valid non-null parameters are passed.
        /// This test arranges specific strings and integer values, creates an instance, and asserts that both the derived properties 
        /// and the base properties from BuildMessageEventArgs are set as expected.
        /// </summary>
        [Fact]
        public void Constructor_WithValidParameters_SetsAllPropertiesCorrectly()
        {
            // Arrange
            string expectedPropertyName = "TestProperty";
            string expectedPropertyValue = "TestValue";
            string expectedPropertySource = "TestSource";
            string expectedFile = "TestFile.cs";
            int expectedLine = 42;
            int expectedColumn = 10;
            string expectedMessage = "Test Message";
            string expectedHelpKeyword = "TestHelp";
            string expectedSenderName = "TestSender";
            MessageImportance expectedImportance = MessageImportance.High;

            // Act
            var eventArgs = new ExtendedPropertyInitialValueSetEventArgs(
                propertyName: expectedPropertyName,
                propertyValue: expectedPropertyValue,
                propertySource: expectedPropertySource,
                file: expectedFile,
                line: expectedLine,
                column: expectedColumn,
                message: expectedMessage,
                helpKeyword: expectedHelpKeyword,
                senderName: expectedSenderName,
                importance: expectedImportance);

            // Assert - Derived properties
            Assert.Equal(expectedPropertyName, eventArgs.PropertyName);
            Assert.Equal(expectedPropertyValue, eventArgs.PropertyValue);
            Assert.Equal(expectedPropertySource, eventArgs.PropertySource);

            // Assert - Base class properties initialized via the constructor
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Equal(expectedHelpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(expectedSenderName, eventArgs.SenderName);
            Assert.Equal(expectedImportance, eventArgs.Importance);
        }

        /// <summary>
        /// Tests that the constructor properly assigns null values for propertyName, propertyValue, and propertySource 
        /// when nulls are passed, ensuring that the class accepts null parameters without throwing exceptions.
        /// </summary>
        [Fact]
        public void Constructor_WithNullParameters_PropertiesAssignNull()
        {
            // Arrange
            string expectedPropertyName = null;
            string expectedPropertyValue = null;
            string expectedPropertySource = null;
            string expectedFile = "FileWithNullProperties.cs";
            int expectedLine = 0;
            int expectedColumn = 0;
            string expectedMessage = "Message with null properties";
            string expectedHelpKeyword = null;
            string expectedSenderName = null;
            MessageImportance expectedImportance = MessageImportance.Low;

            // Act
            var eventArgs = new ExtendedPropertyInitialValueSetEventArgs(
                propertyName: expectedPropertyName,
                propertyValue: expectedPropertyValue,
                propertySource: expectedPropertySource,
                file: expectedFile,
                line: expectedLine,
                column: expectedColumn,
                message: expectedMessage,
                helpKeyword: expectedHelpKeyword,
                senderName: expectedSenderName,
                importance: expectedImportance);

            // Assert - Derived properties
            Assert.Null(eventArgs.PropertyName);
            Assert.Null(eventArgs.PropertyValue);
            Assert.Null(eventArgs.PropertySource);

            // Assert - Base class properties
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Null(eventArgs.HelpKeyword);
            Assert.Null(eventArgs.SenderName);
            Assert.Equal(expectedImportance, eventArgs.Importance);
        }

        /// <summary>
        /// Tests that the public properties of the ExtendedPropertyInitialValueSetEventArgs class are mutable.
        /// This test first constructs an instance with initial values, then changes the properties, and verifies that the updated
        /// values are returned by the property getters.
        /// </summary>
        [Fact]
        public void PropertySetters_CanModifyProperties()
        {
            // Arrange
            var eventArgs = new ExtendedPropertyInitialValueSetEventArgs(
                propertyName: "OriginalName",
                propertyValue: "OriginalValue",
                propertySource: "OriginalSource",
                file: "File.cs",
                line: 1,
                column: 1,
                message: "Initial Message");

            // Act
            string newPropertyName = "UpdatedName";
            string newPropertyValue = "UpdatedValue";
            string newPropertySource = "UpdatedSource";

            eventArgs.PropertyName = newPropertyName;
            eventArgs.PropertyValue = newPropertyValue;
            eventArgs.PropertySource = newPropertySource;

            // Assert
            Assert.Equal(newPropertyName, eventArgs.PropertyName);
            Assert.Equal(newPropertyValue, eventArgs.PropertyValue);
            Assert.Equal(newPropertySource, eventArgs.PropertySource);
        }
    }
}
