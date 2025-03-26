using Microsoft.Build.Framework;
using StructuredLogger.BinaryLogger;
using Xunit;

namespace StructuredLogger.BinaryLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedPropertyInitialValueSetEventArgs"/> class.
    /// </summary>
    public class ExtendedPropertyInitialValueSetEventArgsTests
    {
        /// <summary>
        /// Tests that the constructor correctly sets all properties when valid non-null arguments are provided.
        /// This test verifies that both the properties defined in ExtendedPropertyInitialValueSetEventArgs and the base class properties are set as expected.
        /// </summary>
        [Fact]
        public void Constructor_WithValidArguments_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedPropertyName = "TestProperty";
            string expectedPropertyValue = "TestValue";
            string expectedPropertySource = "TestSource";
            string expectedFile = "testfile.txt";
            int expectedLine = 10;
            int expectedColumn = 20;
            string expectedMessage = "Test message";
            string expectedHelpKeyword = "HelpKeyword";
            string expectedSenderName = "SenderName";
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
            
            // Assert
            Assert.Equal(expectedPropertyName, eventArgs.PropertyName);
            Assert.Equal(expectedPropertyValue, eventArgs.PropertyValue);
            Assert.Equal(expectedPropertySource, eventArgs.PropertySource);
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Equal(expectedHelpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(expectedSenderName, eventArgs.SenderName);
            Assert.Equal(expectedImportance, eventArgs.Importance);
        }
        
        /// <summary>
        /// Tests that the constructor sets default values for optional parameters (helpKeyword and senderName) when they are not provided.
        /// This test verifies that omitting optional parameters results in null values and that the default importance is applied.
        /// </summary>
        [Fact]
        public void Constructor_WithoutOptionalParameters_SetsDefaultValues()
        {
            // Arrange
            string expectedPropertyName = "Prop";
            string expectedPropertyValue = "Val";
            string expectedPropertySource = "Source";
            string expectedFile = "file.csproj";
            int expectedLine = 0;
            int expectedColumn = 0;
            string expectedMessage = "Default message";
            MessageImportance expectedImportance = MessageImportance.Low; // default value
            
            // Act
            var eventArgs = new ExtendedPropertyInitialValueSetEventArgs(
                propertyName: expectedPropertyName,
                propertyValue: expectedPropertyValue,
                propertySource: expectedPropertySource,
                file: expectedFile,
                line: expectedLine,
                column: expectedColumn,
                message: expectedMessage);
            
            // Assert
            Assert.Equal(expectedPropertyName, eventArgs.PropertyName);
            Assert.Equal(expectedPropertyValue, eventArgs.PropertyValue);
            Assert.Equal(expectedPropertySource, eventArgs.PropertySource);
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Null(eventArgs.HelpKeyword);
            Assert.Null(eventArgs.SenderName);
            Assert.Equal(expectedImportance, eventArgs.Importance);
        }
        
        /// <summary>
        /// Tests that the constructor accepts null values for property name, property value, and property source without throwing exceptions.
        /// This test verifies that null values are handled gracefully and that other parameters are correctly set.
        /// </summary>
        [Fact]
        public void Constructor_WithNullProperties_AllowsNullValues()
        {
            // Arrange
            string expectedPropertyName = null;
            string expectedPropertyValue = null;
            string expectedPropertySource = null;
            string expectedFile = "file.log";
            int expectedLine = 5;
            int expectedColumn = 15;
            string expectedMessage = "Message with null property values";
            
            // Act
            var eventArgs = new ExtendedPropertyInitialValueSetEventArgs(
                propertyName: expectedPropertyName,
                propertyValue: expectedPropertyValue,
                propertySource: expectedPropertySource,
                file: expectedFile,
                line: expectedLine,
                column: expectedColumn,
                message: expectedMessage);
            
            // Assert
            Assert.Null(eventArgs.PropertyName);
            Assert.Null(eventArgs.PropertyValue);
            Assert.Null(eventArgs.PropertySource);
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Null(eventArgs.HelpKeyword);
            Assert.Null(eventArgs.SenderName);
            Assert.Equal(MessageImportance.Low, eventArgs.Importance);
        }
    }
}
