using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BinaryLogReaderErrorEventArgs"/> class.
    /// </summary>
    public class BinaryLogReaderErrorEventArgsTests
    {
        private readonly ReaderErrorType _defaultErrorType;
        private readonly BinaryLogRecordKind _defaultRecordKind;

        public BinaryLogReaderErrorEventArgsTests()
        {
            // Initialize default enum values for testing. Assuming 0 is a valid value.
            _defaultErrorType = (ReaderErrorType)0;
            _defaultRecordKind = (BinaryLogRecordKind)0;
        }

        /// <summary>
        /// Tests that GetFormattedMessage returns the expected formatted message.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_WhenDelegateReturnsMessage_ReturnsExpectedMessage()
        {
            // Arrange
            var expectedMessage = "Test error message";
            FormatErrorMessage formatErrorMessage = () => expectedMessage;
            var eventArgs = new BinaryLogReaderErrorEventArgs(_defaultErrorType, _defaultRecordKind, formatErrorMessage);

            // Act
            var actualMessage = eventArgs.GetFormattedMessage();

            // Assert
            actualMessage.Should().Be(expectedMessage, "the delegate should return the expected error message");
        }

        /// <summary>
        /// Tests that GetFormattedMessage returns an empty string when the delegate returns an empty string.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_WhenDelegateReturnsEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var expectedMessage = string.Empty;
            FormatErrorMessage formatErrorMessage = () => expectedMessage;
            var eventArgs = new BinaryLogReaderErrorEventArgs(_defaultErrorType, _defaultRecordKind, formatErrorMessage);

            // Act
            var actualMessage = eventArgs.GetFormattedMessage();

            // Assert
            actualMessage.Should().Be(expectedMessage, "the delegate returns an empty string");
        }

        /// <summary>
        /// Tests that GetFormattedMessage propagates exceptions thrown by the delegate.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_WhenDelegateThrowsException_ThrowsException()
        {
            // Arrange
            var exceptionMessage = "Delegate failure";
            FormatErrorMessage formatErrorMessage = () => throw new InvalidOperationException(exceptionMessage);
            var eventArgs = new BinaryLogReaderErrorEventArgs(_defaultErrorType, _defaultRecordKind, formatErrorMessage);

            // Act
            Action act = () => eventArgs.GetFormattedMessage();

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage(exceptionMessage);
        }

        /// <summary>
        /// Tests that GetFormattedMessage throws a NullReferenceException when the delegate is null.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_WhenDelegateIsNull_ThrowsNullReferenceException()
        {
            // Arrange
            FormatErrorMessage? formatErrorMessage = null;
            var eventArgs = new BinaryLogReaderErrorEventArgs(_defaultErrorType, _defaultRecordKind, formatErrorMessage!);

            // Act
            Action act = () => eventArgs.GetFormattedMessage();

            // Assert
            act.Should().Throw<NullReferenceException>("because calling a null delegate will throw a NullReferenceException");
        }

        /// <summary>
        /// Tests that the constructor sets the ErrorType and RecordKind properties correctly.
        /// </summary>
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var expectedErrorType = (ReaderErrorType)1;
            var expectedRecordKind = (BinaryLogRecordKind)2;
            var expectedMessage = "Constructor test message";
            FormatErrorMessage formatErrorMessage = () => expectedMessage;

            // Act
            var eventArgs = new BinaryLogReaderErrorEventArgs(expectedErrorType, expectedRecordKind, formatErrorMessage);

            // Assert
            eventArgs.ErrorType.Should().Be(expectedErrorType, "ErrorType should be set by the constructor parameter");
            eventArgs.RecordKind.Should().Be(expectedRecordKind, "RecordKind should be set by the constructor parameter");
            eventArgs.GetFormattedMessage().Should().Be(expectedMessage, "GetFormattedMessage should return the message from the delegate");
        }
    }
}
