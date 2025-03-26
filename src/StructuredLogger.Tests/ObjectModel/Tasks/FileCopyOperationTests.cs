using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="FileCopyOperation"/> class.
    /// </summary>
    public class FileCopyOperationTests
    {
        /// <summary>
        /// Tests the <see cref="FileCopyOperation.ToString"/> method with typical non-null Source and Destination values to ensure the correct string is returned.
        /// </summary>
        /// <param name="source">The source file path.</param>
        /// <param name="destination">The destination file path.</param>
        /// <param name="expected">The expected formatted string.</param>
        [Theory]
        [InlineData("FileA.txt", "FileB.txt", "FileA.txt ➔ FileB.txt")]
        [InlineData("src\\FileA.txt", "dest\\FileA.txt", "src\\FileA.txt ➔ dest\\FileA.txt")]
        public void ToString_WithValidNonNullPaths_ReturnsFormattedString(string source, string destination, string expected)
        {
            // Arrange
            var fileCopyOperation = new FileCopyOperation
            {
                Source = source,
                Destination = destination
            };

            // Act
            string result = fileCopyOperation.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the <see cref="FileCopyOperation.ToString"/> method when the Source property is null to ensure graceful handling.
        /// </summary>
        /// <param name="source">The source file path (null).</param>
        /// <param name="destination">The destination file path.</param>
        /// <param name="expected">The expected formatted string with null Source handled as empty string.</param>
        [Theory]
        [InlineData(null, "FileB.txt", " ➔ FileB.txt")]
        public void ToString_WithNullSource_ReturnsFormattedString(string source, string destination, string expected)
        {
            // Arrange
            var fileCopyOperation = new FileCopyOperation
            {
                Source = source,
                Destination = destination
            };

            // Act
            string result = fileCopyOperation.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the <see cref="FileCopyOperation.ToString"/> method when the Destination property is null to ensure graceful handling.
        /// </summary>
        /// <param name="source">The source file path.</param>
        /// <param name="destination">The destination file path (null).</param>
        /// <param name="expected">The expected formatted string with null Destination handled as empty string.</param>
        [Theory]
        [InlineData("FileA.txt", null, "FileA.txt ➔ ")]
        public void ToString_WithNullDestination_ReturnsFormattedString(string source, string destination, string expected)
        {
            // Arrange
            var fileCopyOperation = new FileCopyOperation
            {
                Source = source,
                Destination = destination
            };

            // Act
            string result = fileCopyOperation.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the <see cref="FileCopyOperation.ToString"/> method when both Source and Destination are null to ensure graceful handling.
        /// </summary>
        [Fact]
        public void ToString_WithBothNull_ReturnsFormattedString()
        {
            // Arrange
            var fileCopyOperation = new FileCopyOperation
            {
                Source = null,
                Destination = null
            };

            // Act
            string result = fileCopyOperation.ToString();

            // Assert
            Assert.Equal(" ➔ ", result);
        }

        /// <summary>
        /// Tests the <see cref="FileCopyOperation.ToString"/> method when Source and Destination are empty strings to ensure the formatted string is correct.
        /// </summary>
        [Fact]
        public void ToString_WithEmptyStrings_ReturnsFormattedString()
        {
            // Arrange
            var fileCopyOperation = new FileCopyOperation
            {
                Source = string.Empty,
                Destination = string.Empty
            };

            // Act
            string result = fileCopyOperation.ToString();

            // Assert
            Assert.Equal(" ➔ ", result);
        }
    }
}
