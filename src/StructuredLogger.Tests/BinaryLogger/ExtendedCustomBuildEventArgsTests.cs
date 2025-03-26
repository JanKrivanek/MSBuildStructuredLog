using System.Collections.Generic;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ExtendedCustomBuildEventArgs"/> class.
    /// </summary>
    public class ExtendedCustomBuildEventArgsTests
    {
        /// <summary>
        /// Tests the internal default constructor to ensure it sets ExtendedType to "undefined".
        /// </summary>
        [Fact]
        public void DefaultConstructor_ShouldSetExtendedTypeToUndefined()
        {
            // Arrange & Act
            var eventArgs = new ExtendedCustomBuildEventArgs();

            // Assert
            Assert.Equal("undefined", eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests the constructor with the type parameter to ensure it sets ExtendedType correctly.
        /// </summary>
        /// <param name="type">The type string to use for initialization.</param>
        [Theory]
        [InlineData("Build")]
        [InlineData("Custom")]
        public void Constructor_WithTypeParameter_ShouldSetExtendedType(string type)
        {
            // Arrange & Act
            var eventArgs = new ExtendedCustomBuildEventArgs(type);

            // Assert
            Assert.Equal(type, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests the constructor with type, message, helpKeyword, and senderName parameters.
        /// Ensures ExtendedType is set correctly and the base properties are forwarded.
        /// </summary>
        /// <param name="type">The type string to use for initialization.</param>
        /// <param name="message">A message string.</param>
        /// <param name="helpKeyword">A help keyword.</param>
        /// <param name="senderName">A sender name string.</param>
        [Theory]
        [InlineData("Build", "Test Message", "TestHelp", "TestSender")]
        [InlineData("Log", null, null, null)]
        public void Constructor_WithTypeMessageHelpSender_ShouldSetExtendedTypeAndBaseProperties(string type, string message, string helpKeyword, string senderName)
        {
            // Arrange & Act
            var eventArgs = new ExtendedCustomBuildEventArgs(type, message, helpKeyword, senderName);

            // Assert
            Assert.Equal(type, eventArgs.ExtendedType);

            // Assuming that the base class (CustomBuildEventArgs) exposes the following properties.
            // They are verified here if they exist.
            var baseEventArgs = eventArgs as CustomBuildEventArgs;
            Assert.NotNull(baseEventArgs);
            Assert.Equal(message, baseEventArgs.Message);
            Assert.Equal(helpKeyword, baseEventArgs.HelpKeyword);
            Assert.Equal(senderName, baseEventArgs.SenderName);
        }

        /// <summary>
        /// Tests the constructor with type, message, helpKeyword, senderName, and eventTimestamp parameters.
        /// Ensures ExtendedType is set correctly and the event timestamp is forwarded.
        /// </summary>
        /// <param name="type">The type string to use for initialization.</param>
        /// <param name="message">A message string.</param>
        /// <param name="helpKeyword">A help keyword.</param>
        /// <param name="senderName">A sender name string.</param>
        [Theory]
        [InlineData("Build", "Message", "Help", "Sender")]
        public void Constructor_WithTimestamp_ShouldSetExtendedTypeAndEventTimestamp(string type, string message, string helpKeyword, string senderName)
        {
            // Arrange
            DateTime timestamp = DateTime.Now;

            // Act
            var eventArgs = new ExtendedCustomBuildEventArgs(type, message, helpKeyword, senderName, timestamp);

            // Assert
            Assert.Equal(type, eventArgs.ExtendedType);
            var baseEventArgs = eventArgs as CustomBuildEventArgs;
            Assert.NotNull(baseEventArgs);
            Assert.Equal(timestamp, baseEventArgs.EventTimestamp);
        }

        /// <summary>
        /// Tests the constructor with type, message, helpKeyword, senderName, eventTimestamp, and messageArgs parameters.
        /// Ensures ExtendedType is set correctly and message arguments are accepted.
        /// </summary>
        /// <param name="type">The type string to use for initialization.</param>
        /// <param name="message">A message string.</param>
        /// <param name="helpKeyword">A help keyword.</param>
        /// <param name="senderName">A sender name string.</param>
        [Theory]
        [InlineData("Deploy", "Msg", "Help", "Sender")]
        public void Constructor_WithMessageArgs_ShouldSetExtendedTypeAndAcceptMessageArgs(string type, string message, string helpKeyword, string senderName)
        {
            // Arrange
            DateTime timestamp = DateTime.UtcNow;
            object[] messageArgs = new object[] { "arg1", 123, null };

            // Act
            var eventArgs = new ExtendedCustomBuildEventArgs(type, message, helpKeyword, senderName, timestamp, messageArgs);

            // Assert
            Assert.Equal(type, eventArgs.ExtendedType);
            var baseEventArgs = eventArgs as CustomBuildEventArgs;
            Assert.NotNull(baseEventArgs);
            Assert.Equal(timestamp, baseEventArgs.EventTimestamp);

            // If the base class exposes a property for MessageArgs, it can be asserted here.
            // For example:
            // Assert.Equal(messageArgs, baseEventArgs.MessageArgs);
        }

        /// <summary>
        /// Tests that the ExtendedType property getter and setter work correctly.
        /// </summary>
        /// <param name="value">The new value to set for ExtendedType.</param>
        [Theory]
        [InlineData("Initial")]
        [InlineData("Updated")]
        public void ExtendedType_Property_GetterSetter_WorksCorrectly(string value)
        {
            // Arrange
            var eventArgs = new ExtendedCustomBuildEventArgs("Initial");

            // Act
            eventArgs.ExtendedType = value;

            // Assert
            Assert.Equal(value, eventArgs.ExtendedType);
        }

        /// <summary>
        /// Tests that the ExtendedMetadata property getter and setter work correctly, including handling null values.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_Property_GetterSetter_WorksCorrectly()
        {
            // Arrange
            var eventArgs = new ExtendedCustomBuildEventArgs("Test");
            IDictionary<string, string?> metadata = new Dictionary<string, string?>
            {
                { "Key1", "Value1" },
                { "Key2", null }
            };

            // Act
            eventArgs.ExtendedMetadata = metadata;

            // Assert
            Assert.Equal(metadata, eventArgs.ExtendedMetadata);

            // Act - set to null
            eventArgs.ExtendedMetadata = null;

            // Assert
            Assert.Null(eventArgs.ExtendedMetadata);
        }

        /// <summary>
        /// Tests that the ExtendedData property getter and setter work correctly, including handling null values.
        /// </summary>
        [Fact]
        public void ExtendedData_Property_GetterSetter_WorksCorrectly()
        {
            // Arrange
            var eventArgs = new ExtendedCustomBuildEventArgs("Test");
            string testData = "Sample Data";

            // Act
            eventArgs.ExtendedData = testData;

            // Assert
            Assert.Equal(testData, eventArgs.ExtendedData);

            // Act - set to null
            eventArgs.ExtendedData = null;

            // Assert
            Assert.Null(eventArgs.ExtendedData);
        }
    }
}
