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
        /// <summary>
        /// Tests that GetFormattedMessage returns the expected string as provided by the delegate.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_WhenDelegateReturnsString_ReturnsExactString()
        {
            // Arrange
            const string expectedMessage = "Test error message";
            FormatErrorMessage formatErrorMessage = () => expectedMessage;
            var errorType = (ReaderErrorType)1;
            var recordKind = (BinaryLogRecordKind)2;
            var args = new BinaryLogReaderErrorEventArgs(errorType, recordKind, formatErrorMessage);

            // Act
            string actualMessage = args.GetFormattedMessage();

            // Assert
            actualMessage.Should().Be(expectedMessage);
        }

        /// <summary>
        /// Tests that the ErrorType and RecordKind properties return the values set via the constructor.
        /// </summary>
        [Fact]
        public void Properties_ReturnAssignedValues()
        {
            // Arrange
            const string dummyMessage = "Dummy";
            FormatErrorMessage formatErrorMessage = () => dummyMessage;
            var errorType = (ReaderErrorType)5;
            var recordKind = (BinaryLogRecordKind)10;
            var args = new BinaryLogReaderErrorEventArgs(errorType, recordKind, formatErrorMessage);

            // Act & Assert
            args.ErrorType.Should().Be(errorType);
            args.RecordKind.Should().Be(recordKind);
        }

        /// <summary>
        /// Tests that multiple calls to GetFormattedMessage result in the delegate being executed each time.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_MultipleCalls_DelegateExecutedEachTime()
        {
            // Arrange
            int callCount = 0;
            FormatErrorMessage formatErrorMessage = () =>
            {
                callCount++;
                return $"Call {callCount}";
            };
            var errorType = (ReaderErrorType)0;
            var recordKind = (BinaryLogRecordKind)0;
            var args = new BinaryLogReaderErrorEventArgs(errorType, recordKind, formatErrorMessage);

            // Act
            string firstCall = args.GetFormattedMessage();
            string secondCall = args.GetFormattedMessage();

            // Assert
            firstCall.Should().Be("Call 1");
            secondCall.Should().Be("Call 2");
        }

        /// <summary>
        /// Tests that GetFormattedMessage throws a NullReferenceException when the delegate is null.
        /// This test simulates an unexpected null assignment despite the non-nullable expectation.
        /// </summary>
        [Fact]
        public void GetFormattedMessage_WhenDelegateIsNull_ThrowsNullReferenceException()
        {
            // Arrange
            FormatErrorMessage formatErrorMessage = null!;
            var errorType = (ReaderErrorType)0;
            var recordKind = (BinaryLogRecordKind)0;
            var args = new BinaryLogReaderErrorEventArgs(errorType, recordKind, formatErrorMessage);

            // Act
            Action act = () => args.GetFormattedMessage();

            // Assert
            act.Should().Throw<NullReferenceException>();
        }
    }
}
