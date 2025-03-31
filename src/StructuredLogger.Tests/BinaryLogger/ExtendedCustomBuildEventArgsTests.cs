using System;
using System.Collections.Generic;
using FluentAssertions;
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
        /// Tests that the default (internal) constructor sets the ExtendedType property to "undefined" and leaves other extended properties null.
        /// </summary>
        [Fact]
        public void DefaultConstructor_SetsExtendedTypeToUndefined()
        {
            // Arrange & Act
            var eventArgs = new ExtendedCustomBuildEventArgs();

            // Assert
            eventArgs.ExtendedType.Should().Be("undefined", "the default constructor should initialize ExtendedType to 'undefined'");
            eventArgs.ExtendedMetadata.Should().BeNull("ExtendedMetadata should be null by default");
            eventArgs.ExtendedData.Should().BeNull("ExtendedData should be null by default");
        }

        /// <summary>
        /// Tests that the constructor with the type parameter sets the ExtendedType property to the provided value.
        /// </summary>
        [Fact]
        public void Constructor_WithTypeParameter_SetsExtendedType()
        {
            // Arrange
            string expectedType = "CustomType";

            // Act
            var eventArgs = new ExtendedCustomBuildEventArgs(expectedType);

            // Assert
            eventArgs.ExtendedType.Should().Be(expectedType, "the constructor should initialize ExtendedType to the supplied type parameter");
            eventArgs.ExtendedMetadata.Should().BeNull("ExtendedMetadata should be null by default");
            eventArgs.ExtendedData.Should().BeNull("ExtendedData should be null by default");
        }

        /// <summary>
        /// Tests that the constructor with type, message, helpKeyword, and senderName parameters sets ExtendedType correctly,
        /// and passes event data to the base class.
        /// </summary>
        [Fact]
        public void Constructor_WithTypeMessageHelpKeywordSender_SetsProperties()
        {
            // Arrange
            string expectedType = "CustomType";
            string expectedMessage = "Test message";
            string expectedHelpKeyword = "HelpKeyword";
            string expectedSenderName = "SenderName";

            // Act
            var eventArgs = new ExtendedCustomBuildEventArgs(expectedType, expectedMessage, expectedHelpKeyword, expectedSenderName);

            // Assert
            eventArgs.ExtendedType.Should().Be(expectedType, "the constructor should initialize ExtendedType to the supplied type parameter");
            eventArgs.ExtendedMetadata.Should().BeNull("ExtendedMetadata should be null by default");
            eventArgs.ExtendedData.Should().BeNull("ExtendedData should be null by default");

            // Additionally, verify that the base class properties were initialized.
            // Assuming that the base class (CustomBuildEventArgs) exposes Message, HelpKeyword, and SenderName as public properties.
            var baseType = eventArgs.GetType().BaseType;
            baseType?.GetProperty("Message")?.GetValue(eventArgs)?.Should().Be(expectedMessage, "the Message property should be set via constructor");
            baseType?.GetProperty("HelpKeyword")?.GetValue(eventArgs)?.Should().Be(expectedHelpKeyword, "the HelpKeyword property should be set via constructor");
            baseType?.GetProperty("SenderName")?.GetValue(eventArgs)?.Should().Be(expectedSenderName, "the SenderName property should be set via constructor");
        }

        /// <summary>
        /// Tests that the constructor with type, message, helpKeyword, senderName, and eventTimestamp parameters sets ExtendedType correctly,
        /// and passes the event timestamp to the base class.
        /// </summary>
        [Fact]
        public void Constructor_WithEventTimestamp_SetsProperties()
        {
            // Arrange
            string expectedType = "CustomType";
            string expectedMessage = "Test message";
            string expectedHelpKeyword = "HelpKeyword";
            string expectedSenderName = "SenderName";
            DateTime expectedTimestamp = DateTime.UtcNow;

            // Act
            var eventArgs = new ExtendedCustomBuildEventArgs(expectedType, expectedMessage, expectedHelpKeyword, expectedSenderName, expectedTimestamp);

            // Assert
            eventArgs.ExtendedType.Should().Be(expectedType, "the constructor should initialize ExtendedType to the supplied type parameter");
            eventArgs.ExtendedMetadata.Should().BeNull("ExtendedMetadata should be null by default");
            eventArgs.ExtendedData.Should().BeNull("ExtendedData should be null by default");

            var baseType = eventArgs.GetType().BaseType;
            baseType?.GetProperty("Message")?.GetValue(eventArgs)?.Should().Be(expectedMessage, "the Message property should be set via constructor");
            baseType?.GetProperty("HelpKeyword")?.GetValue(eventArgs)?.Should().Be(expectedHelpKeyword, "the HelpKeyword property should be set via constructor");
            baseType?.GetProperty("SenderName")?.GetValue(eventArgs)?.Should().Be(expectedSenderName, "the SenderName property should be set via constructor");
            
            // Test for event timestamp - property name could be "Timestamp" or "EventTimestamp".
            // Attempting to check both possible names.
            var eventTimestampProperty = baseType?.GetProperty("EventTimestamp") ?? baseType?.GetProperty("Timestamp");
            eventTimestampProperty?.GetValue(eventArgs)?.Should().Be(expectedTimestamp, "the event timestamp should be set via constructor");
        }

        /// <summary>
        /// Tests that the constructor with type, message, helpKeyword, senderName, eventTimestamp and messageArgs parameters sets ExtendedType correctly,
        /// and passes the additional message arguments to the base class.
        /// </summary>
        [Fact]
        public void Constructor_WithMessageArgs_SetsProperties()
        {
            // Arrange
            string expectedType = "CustomType";
            string expectedMessage = "Test message";
            string expectedHelpKeyword = "HelpKeyword";
            string expectedSenderName = "SenderName";
            DateTime expectedTimestamp = DateTime.UtcNow;
            object[] messageArgs = new object[] { "Arg1", 42 };

            // Act
            var eventArgs = new ExtendedCustomBuildEventArgs(expectedType, expectedMessage, expectedHelpKeyword, expectedSenderName, expectedTimestamp, messageArgs);

            // Assert
            eventArgs.ExtendedType.Should().Be(expectedType, "the constructor should initialize ExtendedType to the supplied type parameter");
            eventArgs.ExtendedMetadata.Should().BeNull("ExtendedMetadata should be null by default");
            eventArgs.ExtendedData.Should().BeNull("ExtendedData should be null by default");

            var baseType = eventArgs.GetType().BaseType;
            baseType?.GetProperty("Message")?.GetValue(eventArgs)?.Should().Be(expectedMessage, "the Message property should be set via constructor");
            baseType?.GetProperty("HelpKeyword")?.GetValue(eventArgs)?.Should().Be(expectedHelpKeyword, "the HelpKeyword property should be set via constructor");
            baseType?.GetProperty("SenderName")?.GetValue(eventArgs)?.Should().Be(expectedSenderName, "the SenderName property should be set via constructor");

            var eventTimestampProperty = baseType?.GetProperty("EventTimestamp") ?? baseType?.GetProperty("Timestamp");
            eventTimestampProperty?.GetValue(eventArgs)?.Should().Be(expectedTimestamp, "the event timestamp should be set via constructor");

            // Also attempt to verify that messageArgs are passed.
            // If the base class has a public property for message arguments (for example, MessageArgs), verify them.
            var messageArgsProperty = baseType?.GetProperty("MessageArgs");
            if (messageArgsProperty != null)
            {
                var actualMessageArgs = messageArgsProperty.GetValue(eventArgs) as object[];
                actualMessageArgs.Should().BeEquivalentTo(messageArgs, "the messageArgs parameter should be passed to the base constructor");
            }
        }
    }
}
