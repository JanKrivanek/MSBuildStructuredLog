using Microsoft.Build.Framework;
using StructuredLogger.BinaryLogger;
using System;
using Xunit;

namespace StructuredLogger.BinaryLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedPropertyReassignmentEventArgs"/> class.
    /// </summary>
    public class ExtendedPropertyReassignmentEventArgsTests
    {
        private readonly string _propertyName;
        private readonly string _previousValue;
        private readonly string _newValue;
        private readonly string _file;
        private readonly int _line;
        private readonly int _column;
        private readonly string _message;
        private readonly string _helpKeyword;
        private readonly string _senderName;
        private readonly MessageImportance _importance;

        public ExtendedPropertyReassignmentEventArgsTests()
        {
            _propertyName = "TestProperty";
            _previousValue = "OldValue";
            _newValue = "NewValue";
            _file = "testfile.txt";
            _line = 42;
            _column = 10;
            _message = "Property value was reassigned.";
            _helpKeyword = "HelpKeyword";
            _senderName = "UnitTestSender";
            _importance = MessageImportance.High;
        }

        /// <summary>
        /// Tests that the constructor correctly assigns all properties when provided with valid parameters.
        /// </summary>
        [Fact]
        public void Constructor_WithValidParameters_InitializesPropertiesCorrectly()
        {
            // Arrange & Act
            var eventArgs = new ExtendedPropertyReassignmentEventArgs(
                propertyName: _propertyName,
                previousValue: _previousValue,
                newValue: _newValue,
                file: _file,
                line: _line,
                column: _column,
                message: _message,
                helpKeyword: _helpKeyword,
                senderName: _senderName,
                importance: _importance);

            // Assert extended properties
            Assert.Equal(_propertyName, eventArgs.PropertyName);
            Assert.Equal(_previousValue, eventArgs.PreviousValue);
            Assert.Equal(_newValue, eventArgs.NewValue);

            // Assert base class properties inherited from BuildMessageEventArgs
            Assert.Equal(_file, eventArgs.File);
            Assert.Equal(_line, eventArgs.LineNumber);
            Assert.Equal(_column, eventArgs.ColumnNumber);
            Assert.Equal(_message, eventArgs.Message);
            Assert.Equal(_helpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(_senderName, eventArgs.SenderName);
            Assert.Equal(_importance, eventArgs.Importance);
        }

        /// <summary>
        /// Tests that the public properties can be updated with new values after initialization.
        /// </summary>
        [Fact]
        public void PropertySetters_WhenModified_ReflectUpdatedValues()
        {
            // Arrange
            var eventArgs = new ExtendedPropertyReassignmentEventArgs(
                propertyName: _propertyName,
                previousValue: _previousValue,
                newValue: _newValue,
                file: _file,
                line: _line,
                column: _column,
                message: _message,
                helpKeyword: _helpKeyword,
                senderName: _senderName,
                importance: _importance);

            var updatedPropertyName = "UpdatedProperty";
            var updatedPreviousValue = "UpdatedOldValue";
            var updatedNewValue = "UpdatedNewValue";

            // Act
            eventArgs.PropertyName = updatedPropertyName;
            eventArgs.PreviousValue = updatedPreviousValue;
            eventArgs.NewValue = updatedNewValue;

            // Assert
            Assert.Equal(updatedPropertyName, eventArgs.PropertyName);
            Assert.Equal(updatedPreviousValue, eventArgs.PreviousValue);
            Assert.Equal(updatedNewValue, eventArgs.NewValue);
        }

        /// <summary>
        /// Tests that the constructor correctly handles null values for optional parameters helpKeyword and senderName.
        /// </summary>
        [Fact]
        public void Constructor_WithNullOptionalParameters_InitializesOptionalValuesAsNull()
        {
            // Arrange
            string nullHelpKeyword = null;
            string nullSenderName = null;

            // Act
            var eventArgs = new ExtendedPropertyReassignmentEventArgs(
                propertyName: _propertyName,
                previousValue: _previousValue,
                newValue: _newValue,
                file: _file,
                line: 0,
                column: 0,
                message: _message,
                helpKeyword: nullHelpKeyword,
                senderName: nullSenderName,
                importance: MessageImportance.Low);

            // Assert extended properties
            Assert.Equal(_propertyName, eventArgs.PropertyName);
            Assert.Equal(_previousValue, eventArgs.PreviousValue);
            Assert.Equal(_newValue, eventArgs.NewValue);

            // Assert base class properties for optional parameters
            Assert.Equal(nullHelpKeyword, eventArgs.HelpKeyword);
            Assert.Equal(nullSenderName, eventArgs.SenderName);
        }
    }
}
