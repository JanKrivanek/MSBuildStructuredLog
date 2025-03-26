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
        private readonly ReaderErrorType _sampleErrorType;
        private readonly BinaryLogRecordKind _sampleRecordKind;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryLogReaderErrorEventArgsTests"/> class.
        /// </summary>
        public BinaryLogReaderErrorEventArgsTests()
        {
            // Use default enum values for testing.
            _sampleErrorType = default(ReaderErrorType);
            _sampleRecordKind = default(BinaryLogRecordKind);
        }

        /// <summary>
        /// Tests that GetFormattedMessage returns the expected message when the delegate returns a valid string.
        /// The test constructs a BinaryLogReaderErrorEventArgs with a delegate that returns a known string and verifies that 
        /// GetFormattedMessage yields that string and that the properties ErrorType and RecordKind are correctly set.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_WhenDelegateReturnsMessage_ReturnsExpectedMessage()
        {
            // Arrange
            const string expectedMessage = "Test error message";
            FormatErrorMessage formatErrorMessage = () => expectedMessage;
            var eventArgs = new BinaryLogReaderErrorEventArgs(_sampleErrorType, _sampleRecordKind, formatErrorMessage);

            // Act
            string actualMessage = eventArgs.GetFormattedMessage();

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
            Assert.Equal(_sampleErrorType, eventArgs.ErrorType);
            Assert.Equal(_sampleRecordKind, eventArgs.RecordKind);
        }

        /// <summary>
        /// Tests that GetFormattedMessage returns null when the delegate returns null.
        /// The test verifies that if the delegate returns null, GetFormattedMessage correctly propagates the null value.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_WhenDelegateReturnsNull_ReturnsNull()
        {
            // Arrange
            FormatErrorMessage formatErrorMessage = () => null;
            var eventArgs = new BinaryLogReaderErrorEventArgs(_sampleErrorType, _sampleRecordKind, formatErrorMessage);

            // Act
            string actualMessage = eventArgs.GetFormattedMessage();

            // Assert
            Assert.Null(actualMessage);
        }

        /// <summary>
        /// Tests that GetFormattedMessage propagates an exception thrown by the delegate.
        /// The test constructs a BinaryLogReaderErrorEventArgs with a delegate that throws an exception and verifies that 
        /// GetFormattedMessage throws the same exception.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_WhenDelegateThrowsException_PropagatesException()
        {
            // Arrange
            var expectedException = new InvalidOperationException("Delegate failure");
            FormatErrorMessage formatErrorMessage = () => throw expectedException;
            var eventArgs = new BinaryLogReaderErrorEventArgs(_sampleErrorType, _sampleRecordKind, formatErrorMessage);

            // Act & Assert
            var actualException = Assert.Throws<InvalidOperationException>(() => eventArgs.GetFormattedMessage());
            Assert.Equal(expectedException.Message, actualException.Message);
        }
    }
}
