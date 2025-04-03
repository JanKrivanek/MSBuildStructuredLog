using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="FileCopyOperation"/> class.
    /// </summary>
    public class FileCopyOperationTests
    {
        /// <summary>
        /// Tests the <see cref="FileCopyOperation.ToString"/> method with various combinations of Source and Destination values.
        /// This test uses inline data to cover standard, empty, and special-character cases.
        /// Expected outcome is that the returned string is formatted as "Source ➔ Destination".
        /// </summary>
        /// <param name="source">The source file path.</param>
        /// <param name="destination">The destination file path.</param>
        /// <param name="expected">The expected formatted string.</param>
        [Theory]
        [InlineData("source1", "destination1", "source1 ➔ destination1")]
        [InlineData("", "", " ➔ ")]
        [InlineData("src !@#", "dest $%^", "src !@# ➔ dest $%^")]
        [InlineData("C:\\Temp\\file.txt", "/var/tmp/file.txt", "C:\\Temp\\file.txt ➔ /var/tmp/file.txt")]
        public void ToString_WithVariousInputs_ReturnsExpectedFormattedString(string source, string destination, string expected)
        {
            // Arrange
            var operation = new FileCopyOperation
            {
                Source = source,
                Destination = destination
            };

            // Act
            string result = operation.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the <see cref="FileCopyOperation.ToString"/> method when very long strings are provided for Source and Destination.
        /// This test verifies that even with excessively long strings, the formatting remains correct.
        /// </summary>
        [Fact]
        public void ToString_WithLongStrings_ReturnsCorrectlyFormattedString()
        {
            // Arrange
            string longSource = new string('A', 1000);
            string longDestination = new string('B', 1000);
            var operation = new FileCopyOperation
            {
                Source = longSource,
                Destination = longDestination
            };
            string expected = $"{longSource} ➔ {longDestination}";

            // Act
            string result = operation.ToString();

            // Assert
            result.Should().Be(expected);
        }
    }
}
