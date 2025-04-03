using FluentAssertions;
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
        /// Tests that the constructor sets inherited and local properties correctly when default optional parameters are used.
        /// </summary>
        [Fact]
        public void Constructor_WithDefaultOptionals_SetsInheritedAndLocalPropertiesCorrectly()
        {
            // Arrange
            string expectedPropertyName = "TestProperty";
            string expectedPropertyValue = "SomeValue";
            string expectedPropertySource = "TestSource";
            string expectedFile = "C:\\temp\\file.txt";
            int expectedLine = 10;
            int expectedColumn = 20;
            string expectedMessage = "Test message";
            // Optional parameters not provided, so helpKeyword and senderName should be null,
            // and importance should be the default value MessageImportance.Low.
            MessageImportance expectedImportance = MessageImportance.Low;
            
            // Act
            var eventArgs = new ExtendedPropertyInitialValueSetEventArgs(
                propertyName: expectedPropertyName,
                propertyValue: expectedPropertyValue,
                propertySource: expectedPropertySource,
                file: expectedFile,
                line: expectedLine,
                column: expectedColumn,
                message: expectedMessage);

            // Assert - Local properties
            eventArgs.PropertyName.Should().Be(expectedPropertyName);
            eventArgs.PropertyValue.Should().Be(expectedPropertyValue);
            eventArgs.PropertySource.Should().Be(expectedPropertySource);
            // Assert - Inherited properties from BuildMessageEventArgs
            eventArgs.File.Should().Be(expectedFile);
            eventArgs.LineNumber.Should().Be(expectedLine);
            eventArgs.ColumnNumber.Should().Be(expectedColumn);
            eventArgs.Message.Should().Be(expectedMessage);
            eventArgs.HelpKeyword.Should().BeNull();
            eventArgs.SenderName.Should().BeNull();
            eventArgs.Importance.Should().Be(expectedImportance);
        }

        /// <summary>
        /// Tests that the constructor sets inherited and local properties correctly when all parameters are provided.
        /// </summary>
        [Fact]
        public void Constructor_WithAllParameters_SetsInheritedAndLocalPropertiesCorrectly()
        {
            // Arrange
            string expectedPropertyName = "NameProvided";
            string expectedPropertyValue = "ValueProvided";
            string expectedPropertySource = "SourceProvided";
            string expectedFile = "/usr/local/file.log";
            int expectedLine = 1;
            int expectedColumn = 2;
            string expectedMessage = "Detailed message";
            string expectedHelpKeyword = "HelpKey123";
            string expectedSenderName = "SenderXYZ";
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

            // Assert - Local properties
            eventArgs.PropertyName.Should().Be(expectedPropertyName);
            eventArgs.PropertyValue.Should().Be(expectedPropertyValue);
            eventArgs.PropertySource.Should().Be(expectedPropertySource);
            // Assert - Inherited properties from BuildMessageEventArgs
            eventArgs.File.Should().Be(expectedFile);
            eventArgs.LineNumber.Should().Be(expectedLine);
            eventArgs.ColumnNumber.Should().Be(expectedColumn);
            eventArgs.Message.Should().Be(expectedMessage);
            eventArgs.HelpKeyword.Should().Be(expectedHelpKeyword);
            eventArgs.SenderName.Should().Be(expectedSenderName);
            eventArgs.Importance.Should().Be(expectedImportance);
        }

        /// <summary>
        /// Tests that the constructor sets all properties correctly when boundary values are provided.
        /// </summary>
        [Fact]
        public void Constructor_WithBoundaryValues_SetsInheritedAndLocalPropertiesCorrectly()
        {
            // Arrange
            string expectedPropertyName = string.Empty;
            string expectedPropertyValue = string.Empty;
            string expectedPropertySource = string.Empty;
            string expectedFile = string.Empty;
            int expectedLine = int.MaxValue;
            int expectedColumn = int.MinValue;
            string expectedMessage = new string('x', 1000); // excessively long string
            // Using optional parameters as default.
            MessageImportance expectedImportance = MessageImportance.Low;
            
            // Act
            var eventArgs = new ExtendedPropertyInitialValueSetEventArgs(
                propertyName: expectedPropertyName,
                propertyValue: expectedPropertyValue,
                propertySource: expectedPropertySource,
                file: expectedFile,
                line: expectedLine,
                column: expectedColumn,
                message: expectedMessage);

            // Assert - Local properties
            eventArgs.PropertyName.Should().Be(expectedPropertyName);
            eventArgs.PropertyValue.Should().Be(expectedPropertyValue);
            eventArgs.PropertySource.Should().Be(expectedPropertySource);
            // Assert - Inherited properties from BuildMessageEventArgs
            eventArgs.File.Should().Be(expectedFile);
            eventArgs.LineNumber.Should().Be(expectedLine);
            eventArgs.ColumnNumber.Should().Be(expectedColumn);
            eventArgs.Message.Should().Be(expectedMessage);
            eventArgs.HelpKeyword.Should().BeNull();
            eventArgs.SenderName.Should().BeNull();
            eventArgs.Importance.Should().Be(expectedImportance);
        }

        /// <summary>
        /// Tests that property setters correctly update the object's properties after construction.
        /// </summary>
        [Fact]
        public void PropertySetters_ShouldUpdateProperties()
        {
            // Arrange
            var eventArgs = new ExtendedPropertyInitialValueSetEventArgs(
                propertyName: "InitialName",
                propertyValue: "InitialValue",
                propertySource: "InitialSource",
                file: "initialFile.log",
                line: 5,
                column: 5,
                message: "Initial message");

            // Act
            string newName = "UpdatedName";
            string newValue = "UpdatedValue";
            string newSource = "UpdatedSource";
            eventArgs.PropertyName = newName;
            eventArgs.PropertyValue = newValue;
            eventArgs.PropertySource = newSource;

            // Assert
            eventArgs.PropertyName.Should().Be(newName);
            eventArgs.PropertyValue.Should().Be(newValue);
            eventArgs.PropertySource.Should().Be(newSource);
        }
    }
}
