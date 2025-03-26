using StructuredLogger.BinaryLogger;
using Xunit;

namespace StructuredLogger.BinaryLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedPropertyReassignmentEventArgs"/> class.
    /// </summary>
    public class ExtendedPropertyReassignmentEventArgsTests
    {
        /// <summary>
        /// Tests that the constructor sets properties correctly when all parameters are provided.
        /// </summary>
        [Fact]
        public void Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedPropertyName = "TestProperty";
            string expectedPreviousValue = "OldValue";
            string expectedNewValue = "NewValue";
            string expectedFile = "file.txt";
            int expectedLine = 10;
            int expectedColumn = 5;
            string expectedMessage = "Property reassignment occurred.";
            string expectedHelpKeyword = "HelpKey";
            string expectedSenderName = "Sender";
            var expectedImportance = MessageImportance.High;
            
            // Act
            var eventArgs = new ExtendedPropertyReassignmentEventArgs(
                expectedPropertyName,
                expectedPreviousValue,
                expectedNewValue,
                expectedFile,
                expectedLine,
                expectedColumn,
                expectedMessage,
                expectedHelpKeyword,
                expectedSenderName,
                expectedImportance);
            
            // Assert
            Assert.Equal(expectedPropertyName, eventArgs.PropertyName);
            Assert.Equal(expectedPreviousValue, eventArgs.PreviousValue);
            Assert.Equal(expectedNewValue, eventArgs.NewValue);
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Equal(expectedHelpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(expectedSenderName, eventArgs.SenderName);
            Assert.Equal(expectedImportance, eventArgs.Importance);
        }
        
        /// <summary>
        /// Tests that the constructor sets properties correctly when optional parameters are omitted.
        /// </summary>
        [Fact]
        public void Constructor_WithNullOptionalParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedPropertyName = "TestProperty";
            string expectedPreviousValue = "OldValue";
            string expectedNewValue = "NewValue";
            string expectedFile = "file.txt";
            int expectedLine = 0;
            int expectedColumn = 0;
            string expectedMessage = "Test message.";
            
            // Act
            var eventArgs = new ExtendedPropertyReassignmentEventArgs(
                expectedPropertyName,
                expectedPreviousValue,
                expectedNewValue,
                expectedFile,
                expectedLine,
                expectedColumn,
                expectedMessage);
            
            // Assert
            Assert.Equal(expectedPropertyName, eventArgs.PropertyName);
            Assert.Equal(expectedPreviousValue, eventArgs.PreviousValue);
            Assert.Equal(expectedNewValue, eventArgs.NewValue);
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
            Assert.Equal(expectedMessage, eventArgs.Message);
            Assert.Null(eventArgs.HelpKeyword);
            Assert.Null(eventArgs.SenderName);
            Assert.Equal(MessageImportance.Low, eventArgs.Importance);
        }
        
        /// <summary>
        /// Tests that the properties' setters and getters function correctly.
        /// </summary>
        [Fact]
        public void Properties_SetterGetter_WorksCorrectly()
        {
            // Arrange
            var eventArgs = new ExtendedPropertyReassignmentEventArgs(
                "InitialProperty",
                "InitialOldValue",
                "InitialNewValue",
                "initial.txt",
                1,
                1,
                "Initial message");
            
            // Act
            eventArgs.PropertyName = "UpdatedProperty";
            eventArgs.PreviousValue = "UpdatedOldValue";
            eventArgs.NewValue = "UpdatedNewValue";
            
            // Assert
            Assert.Equal("UpdatedProperty", eventArgs.PropertyName);
            Assert.Equal("UpdatedOldValue", eventArgs.PreviousValue);
            Assert.Equal("UpdatedNewValue", eventArgs.NewValue);
        }
        
        /// <summary>
        /// Tests that the constructor accepts null values for parameters without throwing exceptions.
        /// </summary>
        [Fact]
        public void Constructor_WithNullParameters_DoesNotThrow()
        {
            // Arrange
            string expectedFile = null;
            int expectedLine = -1;
            int expectedColumn = -1;
            string expectedMessage = null;
            
            // Act & Assert
            var exception = Record.Exception(() => new ExtendedPropertyReassignmentEventArgs(
                null,
                null,
                null,
                expectedFile,
                expectedLine,
                expectedColumn,
                expectedMessage));
            Assert.Null(exception);
        }
    }
}
