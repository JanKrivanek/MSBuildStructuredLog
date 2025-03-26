// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="StreamChunkOverReadException"/> class.
    /// </summary>
    public class StreamChunkOverReadExceptionTests
    {
        /// <summary>
        /// Tests that the default constructor initializes the exception with the expected default properties.
        /// The default constructor should instantiate the exception with a non-null message and a null inner exception.
        /// </summary>
        [Fact]
        public void DefaultConstructor_InitializesWithExpectedProperties()
        {
            // Act
            var exception = new StreamChunkOverReadException();

            // Assert
            Assert.NotNull(exception);
            // When no message is provided, the Message property should be set by the base Exception class.
            Assert.False(string.IsNullOrEmpty(exception.Message));
            Assert.Null(exception.InnerException);
        }

        /// <summary>
        /// Tests that the constructor accepting a message parameter assigns the Message property correctly.
        /// The test covers cases with non-empty string, empty string, and null.
        /// </summary>
        /// <param name="testMessage">The message value to test.</param>
        [Theory]
        [InlineData("Test error message.")]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_WithMessage_AssignsMessage(string testMessage)
        {
            // Act
            var exception = new StreamChunkOverReadException(testMessage);

            // Assert
            if (testMessage == null)
            {
                // When null is provided, the Exception.Message property should still return a non-null, non-empty string.
                Assert.False(string.IsNullOrEmpty(exception.Message));
            }
            else
            {
                Assert.Equal(testMessage, exception.Message);
            }
            Assert.Null(exception.InnerException);
        }

        /// <summary>
        /// Tests that the constructor accepting both a message and an inner exception assigns both properties correctly.
        /// The test covers cases with non-empty string, empty string, and null message values.
        /// </summary>
        /// <param name="testMessage">The message value to test.</param>
        [Theory]
        [InlineData("Composite error message.")]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_WithMessageAndInnerException_AssignsProperties(string testMessage)
        {
            // Arrange
            var innerException = new Exception("Inner exception message.");

            // Act
            var exception = new StreamChunkOverReadException(testMessage, innerException);

            // Assert
            if (testMessage == null)
            {
                // When null is provided, Message property should be set by the base Exception class.
                Assert.False(string.IsNullOrEmpty(exception.Message));
            }
            else
            {
                Assert.Equal(testMessage, exception.Message);
            }
            Assert.Equal(innerException, exception.InnerException);
        }
    }
}
