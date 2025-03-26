using System;
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
        /// Tests that the constructor sets both OriginalString and StringToBeUsed to the provided non-null string.
        /// </summary>
        [Theory]
        [InlineData("Test")]
        [InlineData("Another test string")]
        public void Constructor_WithValidString_SetsPropertiesCorrectly(string input)
        {
            // Arrange & Act
            var args = new StringReadEventArgs(input);

            // Assert
            Assert.Equal(input, args.OriginalString);
            Assert.Equal(input, args.StringToBeUsed);
        }

        /// <summary>
        /// Tests that the constructor sets both OriginalString and StringToBeUsed to null when null is provided.
        /// </summary>
        [Fact]
        public void Constructor_WithNullValue_SetsPropertiesAsNull()
        {
            // Arrange
            string input = null;

            // Act
            var args = new StringReadEventArgs(input);

            // Assert
            Assert.Null(args.OriginalString);
            Assert.Null(args.StringToBeUsed);
        }

        /// <summary>
        /// Tests that modifying the StringToBeUsed property does not affect the OriginalString property.
        /// </summary>
        [Fact]
        public void SetStringToBeUsed_WhenChanged_OriginalStringRemainsUnchanged()
        {
            // Arrange
            var initialValue = "Initial Value";
            var newValue = "Modified Value";
            var args = new StringReadEventArgs(initialValue);

            // Act
            args.StringToBeUsed = newValue;

            // Assert
            Assert.Equal(initialValue, args.OriginalString);
            Assert.Equal(newValue, args.StringToBeUsed);
        }

        /// <summary>
        /// Tests that calling Reuse with a valid non-null value updates both OriginalString and StringToBeUsed.
        /// </summary>
        [Fact]
        public void Reuse_WithValidValue_UpdatesProperties()
        {
            // Arrange
            var initialValue = "Initial";
            var newValue = "Reused";
            var args = new StringReadEventArgs(initialValue);

            // Act
            args.Reuse(newValue);

            // Assert
            Assert.Equal(newValue, args.OriginalString);
            Assert.Equal(newValue, args.StringToBeUsed);
        }

        /// <summary>
        /// Tests that calling Reuse with a null value updates both OriginalString and StringToBeUsed to null.
        /// </summary>
        [Fact]
        public void Reuse_WithNullValue_UpdatesPropertiesToNull()
        {
            // Arrange
            var initialValue = "Initial";
            string newValue = null;
            var args = new StringReadEventArgs(initialValue);

            // Act
            args.Reuse(newValue);

            // Assert
            Assert.Null(args.OriginalString);
            Assert.Null(args.StringToBeUsed);
        }
    }
}
