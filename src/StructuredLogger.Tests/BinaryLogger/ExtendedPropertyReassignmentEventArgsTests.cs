using FluentAssertions;
using Microsoft.Build.Framework;
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
        /// Tests the constructor with valid non-empty parameters to ensure that the properties are set correctly.
        /// </summary>
        [Fact]
        public void Constructor_HappyPath_SetsPropertiesCorrectly()
        {
            // Arrange
            string propertyName = "TestProperty";
            string previousValue = "OldValue";
            string newValue = "NewValue";
            string file = "C:\\temp\\file.txt";
            int line = 10;
            int column = 20;
            string message = "Property was reassigned";
            string helpKeyword = "HelpKeyword";
            string senderName = "TestSender";
            MessageImportance importance = MessageImportance.Low;

            // Act
            var instance = new ExtendedPropertyReassignmentEventArgs(propertyName, previousValue, newValue, file, line, column, message, helpKeyword, senderName, importance);

            // Assert
            instance.PropertyName.Should().Be(propertyName);
            instance.PreviousValue.Should().Be(previousValue);
            instance.NewValue.Should().Be(newValue);
            instance.File.Should().Be(file);
            instance.LineNumber.Should().Be(line);
            instance.ColumnNumber.Should().Be(column);
            instance.Message.Should().Be(message);
            instance.HelpKeyword.Should().Be(helpKeyword);
            instance.SenderName.Should().Be(senderName);
            instance.Importance.Should().Be(importance);
        }

        /// <summary>
        /// Tests the constructor with empty string parameters to verify that empty values are assigned as provided.
        /// </summary>
        [Fact]
        public void Constructor_WithEmptyStrings_SetsPropertiesCorrectly()
        {
            // Arrange
            string propertyName = string.Empty;
            string previousValue = string.Empty;
            string newValue = string.Empty;
            string file = string.Empty;
            int line = 0;
            int column = 0;
            string message = string.Empty;
            string helpKeyword = string.Empty;
            string senderName = string.Empty;
            MessageImportance importance = MessageImportance.Low;

            // Act
            var instance = new ExtendedPropertyReassignmentEventArgs(propertyName, previousValue, newValue, file, line, column, message, helpKeyword, senderName, importance);

            // Assert
            instance.PropertyName.Should().Be(propertyName);
            instance.PreviousValue.Should().Be(previousValue);
            instance.NewValue.Should().Be(newValue);
            instance.File.Should().Be(file);
            instance.LineNumber.Should().Be(line);
            instance.ColumnNumber.Should().Be(column);
            instance.Message.Should().Be(message);
            instance.HelpKeyword.Should().Be(helpKeyword);
            instance.SenderName.Should().Be(senderName);
            instance.Importance.Should().Be(importance);
        }

        /// <summary>
        /// Tests the constructor with extreme numerical values to ensure that the base properties are assigned without throwing exceptions.
        /// </summary>
        [Fact]
        public void Constructor_WithExtremeNumbers_DoesNotThrowAndSetsBaseProperties()
        {
            // Arrange
            string propertyName = "ExtremeProperty";
            string previousValue = "MinValue";
            string newValue = "MaxValue";
            string file = "/var/log/file.log";
            int line = int.MaxValue;
            int column = int.MinValue;
            string message = "Extreme numerical values";
            // Optional parameters passed as null.
            string helpKeyword = null;
            string senderName = null;
            MessageImportance importance = MessageImportance.Low;

            // Act
            ExtendedPropertyReassignmentEventArgs instance = null;
            var act = () => instance = new ExtendedPropertyReassignmentEventArgs(propertyName, previousValue, newValue, file, line, column, message, helpKeyword, senderName, importance);

            // Assert
            act.Should().NotThrow();
            instance.Should().NotBeNull();
            instance.PropertyName.Should().Be(propertyName);
            instance.PreviousValue.Should().Be(previousValue);
            instance.NewValue.Should().Be(newValue);
            instance.File.Should().Be(file);
            instance.LineNumber.Should().Be(line);
            instance.ColumnNumber.Should().Be(column);
            instance.Message.Should().Be(message);
            instance.HelpKeyword.Should().BeNull();
            instance.SenderName.Should().BeNull();
            instance.Importance.Should().Be(importance);
        }

        /// <summary>
        /// Tests the constructor with null optional parameters (helpKeyword and senderName) to verify that they are handled correctly.
        /// </summary>
        [Fact]
        public void Constructor_WithNullOptionalParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            string propertyName = "OptionalTestProperty";
            string previousValue = "Previous";
            string newValue = "New";
            string file = "file.log";
            int line = 5;
            int column = 5;
            string message = "Testing null optionals";
            // Optional parameters are passed as null.
            MessageImportance importance = MessageImportance.Low;

            // Act
            var instance = new ExtendedPropertyReassignmentEventArgs(propertyName, previousValue, newValue, file, line, column, message, null, null, importance);

            // Assert
            instance.PropertyName.Should().Be(propertyName);
            instance.PreviousValue.Should().Be(previousValue);
            instance.NewValue.Should().Be(newValue);
            instance.File.Should().Be(file);
            instance.LineNumber.Should().Be(line);
            instance.ColumnNumber.Should().Be(column);
            instance.Message.Should().Be(message);
            instance.HelpKeyword.Should().BeNull();
            instance.SenderName.Should().BeNull();
            instance.Importance.Should().Be(importance);
        }
    }
}
