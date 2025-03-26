using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="StringReadEventArgs"/> class.
    /// </summary>
    public class StringReadEventArgsTests
    {
        /// <summary>
        /// Tests that the constructor correctly initializes the properties with a non-null string.
        /// </summary>
        [Fact]
        public void Constructor_WithNonNullString_InitializesPropertiesCorrectly()
        {
            // Arrange
            string input = "TestString";

            // Act
            var eventArgs = new StringReadEventArgs(input);

            // Assert
            Assert.Equal(input, eventArgs.OriginalString);
            Assert.Equal(input, eventArgs.StringToBeUsed);
        }

        /// <summary>
        /// Tests that the constructor correctly initializes the properties when a null string is provided.
        /// </summary>
        [Fact]
        public void Constructor_WithNullString_InitializesPropertiesAsNull()
        {
            // Arrange
            string input = null;

            // Act
            var eventArgs = new StringReadEventArgs(input);

            // Assert
            Assert.Null(eventArgs.OriginalString);
            Assert.Null(eventArgs.StringToBeUsed);
        }

        /// <summary>
        /// Tests that the internal Reuse method updates the properties with a new non-null value.
        /// </summary>
        [Fact]
        public void Reuse_WithNonNullString_UpdatesPropertiesCorrectly()
        {
            // Arrange
            string initial = "InitialValue";
            string updatedValue = "UpdatedValue";
            var eventArgs = new StringReadEventArgs(initial);

            // Act
            eventArgs.Reuse(updatedValue);

            // Assert
            Assert.Equal(updatedValue, eventArgs.OriginalString);
            Assert.Equal(updatedValue, eventArgs.StringToBeUsed);
        }

        /// <summary>
        /// Tests that the internal Reuse method updates the properties to null when passed a null value.
        /// </summary>
        [Fact]
        public void Reuse_WithNull_UpdatesPropertiesToNull()
        {
            // Arrange
            string initial = "InitialValue";
            string updatedValue = null;
            var eventArgs = new StringReadEventArgs(initial);

            // Act
            eventArgs.Reuse(updatedValue);

            // Assert
            Assert.Null(eventArgs.OriginalString);
            Assert.Null(eventArgs.StringToBeUsed);
        }

        /// <summary>
        /// Tests that the internal Reuse method updates the properties to an empty string when passed an empty string.
        /// </summary>
        [Fact]
        public void Reuse_WithEmptyString_UpdatesPropertiesToEmpty()
        {
            // Arrange
            string initial = "InitialValue";
            string updatedValue = string.Empty;
            var eventArgs = new StringReadEventArgs(initial);

            // Act
            eventArgs.Reuse(updatedValue);

            // Assert
            Assert.Equal(updatedValue, eventArgs.OriginalString);
            Assert.Equal(updatedValue, eventArgs.StringToBeUsed);
        }
    }
}
