using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BinaryLogReaderErrorEventArgs"/> class.
    /// </summary>
    public class BinaryLogReaderErrorEventArgsTests
    {
        /// <summary>
        /// Tests that the GetFormattedMessage method returns the expected non-empty error message.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_WhenDelegateReturnsNonEmptyString_ReturnsExpectedMessage()
        {
            // Arrange
            const string expectedMessage = "An error occurred during reading.";
            FormatErrorMessage formatDelegate = () => expectedMessage;
            // For testing purposes, we use dummy values for ReaderErrorType and BinaryLogRecordKind.
            // Assuming that these types are enums, we can cast integer values.
            var errorType = (ReaderErrorType)1;
            var recordKind = (BinaryLogRecordKind)2;
            var eventArgs = new BinaryLogReaderErrorEventArgs(errorType, recordKind, formatDelegate);

            // Act
            string actualMessage = eventArgs.GetFormattedMessage();

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        /// <summary>
        /// Tests that the GetFormattedMessage method returns an empty string when the delegate returns an empty string.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_WhenDelegateReturnsEmptyString_ReturnsEmptyString()
        {
            // Arrange
            const string expectedMessage = "";
            FormatErrorMessage formatDelegate = () => expectedMessage;
            var errorType = (ReaderErrorType)0;
            var recordKind = (BinaryLogRecordKind)0;
            var eventArgs = new BinaryLogReaderErrorEventArgs(errorType, recordKind, formatDelegate);

            // Act
            string actualMessage = eventArgs.GetFormattedMessage();

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        /// <summary>
        /// Tests that the GetFormattedMessage method returns null when the delegate returns null.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_WhenDelegateReturnsNull_ReturnsNull()
        {
            // Arrange
            string expectedMessage = null;
            FormatErrorMessage formatDelegate = () => expectedMessage;
            var errorType = (ReaderErrorType)5;
            var recordKind = (BinaryLogRecordKind)10;
            var eventArgs = new BinaryLogReaderErrorEventArgs(errorType, recordKind, formatDelegate);

            // Act
            string actualMessage = eventArgs.GetFormattedMessage();

            // Assert
            Assert.Null(actualMessage);
        }

        /// <summary>
        /// Tests that the properties ErrorType and RecordKind are set correctly by the constructor.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_SetsPropertiesCorrectly()
        {
            // Arrange
            var expectedErrorType = (ReaderErrorType)3;
            var expectedRecordKind = (BinaryLogRecordKind)7;
            FormatErrorMessage formatDelegate = () => "Test message";
            var eventArgs = new BinaryLogReaderErrorEventArgs(expectedErrorType, expectedRecordKind, formatDelegate);

            // Act
            var actualErrorType = eventArgs.ErrorType;
            var actualRecordKind = eventArgs.RecordKind;

            // Assert
            Assert.Equal(expectedErrorType, actualErrorType);
            Assert.Equal(expectedRecordKind, actualRecordKind);
        }
    }
}
