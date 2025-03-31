using FluentAssertions;
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
        private readonly string _propertyName;
        private readonly string _propertyValue;
        private readonly string _propertySource;
        private readonly string _file;
        private readonly int _line;
        private readonly int _column;
        private readonly string _message;
        private readonly string _helpKeyword;
        private readonly string _senderName;
        //private readonly MessageImportance _importance;

        /// <summary>
        /// Initializes test data for ExtendedPropertyInitialValueSetEventArgs tests.
        /// </summary>
        public ExtendedPropertyInitialValueSetEventArgsTests()
        {
            _propertyName = "TestProperty";
            _propertyValue = "TestValue";
            _propertySource = "TestSource";
            _file = "TestFile.cs";
            _line = 10;
            _column = 20;
            _message = "This is a test message.";
            _helpKeyword = "Help001";
            _senderName = "TestSender";
            //_importance = MessageImportance.High;
        }

        /// <summary>
        /// Tests that the ExtendedPropertyInitialValueSetEventArgs constructor assigns all properties correctly when all parameters are provided.
        /// Arrange: Prepare valid test data for all parameters.
        /// Act: Create an instance using the constructor with all parameters.
        /// Assert: Verify that the instance properties and inherited values match the provided inputs.
        /// </summary>
        [Fact]
        public void Constructor_WithAllParameters_AssignsPropertiesCorrectly()
        {
            // Act
            var eventArgs = new ExtendedPropertyInitialValueSetEventArgs(
                _propertyName,
                _propertyValue,
                _propertySource,
                _file,
                _line,
                _column,
                _message,
                _helpKeyword,
                _senderName,
                MessageImportance.High);

            // Assert
            eventArgs.PropertyName.Should().Be(_propertyName);
            eventArgs.PropertyValue.Should().Be(_propertyValue);
            eventArgs.PropertySource.Should().Be(_propertySource);
            eventArgs.File.Should().Be(_file);
            eventArgs.LineNumber.Should().Be(_line);
            eventArgs.ColumnNumber.Should().Be(_column);
            eventArgs.Message.Should().Be(_message);
            eventArgs.HelpKeyword.Should().Be(_helpKeyword);
            eventArgs.SenderName.Should().Be(_senderName);
            eventArgs.Importance.Should().Be(MessageImportance.High);
        }

        /// <summary>
        /// Tests that the ExtendedPropertyInitialValueSetEventArgs constructor assigns default values for optional parameters when they are omitted.
        /// Arrange: Prepare valid test data without optional parameters.
        /// Act: Create an instance using the constructor without specifying helpKeyword, senderName, and importance.
        /// Assert: Verify that helpKeyword and senderName are null and importance is set to MessageImportance.Low.
        /// </summary>
        [Fact]
        public void Constructor_WithoutOptionalParameters_SetsDefaultValues()
        {
            // Arrange
            var defaultImportance = MessageImportance.Low;

            // Act
            var eventArgs = new ExtendedPropertyInitialValueSetEventArgs(
                _propertyName,
                _propertyValue,
                _propertySource,
                _file,
                _line,
                _column,
                _message);

            // Assert
            eventArgs.PropertyName.Should().Be(_propertyName);
            eventArgs.PropertyValue.Should().Be(_propertyValue);
            eventArgs.PropertySource.Should().Be(_propertySource);
            eventArgs.File.Should().Be(_file);
            eventArgs.LineNumber.Should().Be(_line);
            eventArgs.ColumnNumber.Should().Be(_column);
            eventArgs.Message.Should().Be(_message);
            eventArgs.HelpKeyword.Should().BeNull();
            eventArgs.SenderName.Should().BeNull();
            eventArgs.Importance.Should().Be(defaultImportance);
        }

        /// <summary>
        /// Tests that the property setters correctly allow modification of the properties after construction.
        /// Arrange: Instantiate the class with initial test values.
        /// Act: Change the values using the property setters.
        /// Assert: Confirm that the properties reflect the new values.
        /// </summary>
        [Fact]
        public void PropertySetters_ModifyPropertiesSuccessfully()
        {
            // Arrange
            var eventArgs = new ExtendedPropertyInitialValueSetEventArgs(
                _propertyName,
                _propertyValue,
                _propertySource,
                _file,
                _line,
                _column,
                _message,
                _helpKeyword,
                _senderName,
                MessageImportance.High);

            var newPropertyName = "NewProperty";
            var newPropertyValue = "NewValue";
            var newPropertySource = "NewSource";

            // Act
            eventArgs.PropertyName = newPropertyName;
            eventArgs.PropertyValue = newPropertyValue;
            eventArgs.PropertySource = newPropertySource;

            // Assert
            eventArgs.PropertyName.Should().Be(newPropertyName);
            eventArgs.PropertyValue.Should().Be(newPropertyValue);
            eventArgs.PropertySource.Should().Be(newPropertySource);
        }

        /// <summary>
        /// Tests the behavior of the constructor when empty string values are provided.
        /// Arrange: Prepare empty strings and zero values for file, message, line, and column.
        /// Act: Create an instance using empty strings and zero values.
        /// Assert: Verify that properties are set to empty strings or zero as appropriate, and optional parameters are null/default.
        /// </summary>
        [Fact]
        public void Constructor_WithEmptyStrings_AssignsEmptyValues()
        {
            // Arrange
            var empty = string.Empty;
            var localLine = 0;
            var localColumn = 0;
            var localMessage = string.Empty;

            // Act
            var eventArgs = new ExtendedPropertyInitialValueSetEventArgs(
                empty,
                empty,
                empty,
                empty,
                localLine,
                localColumn,
                localMessage);

            // Assert
            eventArgs.PropertyName.Should().Be(empty);
            eventArgs.PropertyValue.Should().Be(empty);
            eventArgs.PropertySource.Should().Be(empty);
            eventArgs.File.Should().Be(empty);
            eventArgs.LineNumber.Should().Be(localLine);
            eventArgs.ColumnNumber.Should().Be(localColumn);
            eventArgs.Message.Should().Be(localMessage);
            eventArgs.HelpKeyword.Should().BeNull();
            eventArgs.SenderName.Should().BeNull();
            eventArgs.Importance.Should().Be(MessageImportance.Low);
        }
    }
}
