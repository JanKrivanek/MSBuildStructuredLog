using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="StringReadEventArgs"/> class.
    /// </summary>
    public class StringReadEventArgsTests
    {
        private readonly string _initialString;
        private readonly string _newString;

        public StringReadEventArgsTests()
        {
            _initialString = "Initial test string";
            _newString = "New test string";
        }

        /// <summary>
        /// Tests that the constructor sets both OriginalString and StringToBeUsed properties to the provided string.
        /// </summary>
        [Fact]
        public void Constructor_ValidString_PropertiesAreInitialized()
        {
            // Arrange & Act
            var eventArgs = new StringReadEventArgs(_initialString);

            // Assert
            eventArgs.OriginalString.Should().Be(_initialString, "the constructor should initialize OriginalString with the provided value");
            eventArgs.StringToBeUsed.Should().Be(_initialString, "the constructor should initialize StringToBeUsed with the provided value");
        }

        /// <summary>
        /// Tests that the constructor works correctly when an empty string is provided.
        /// </summary>
        [Fact]
        public void Constructor_EmptyString_PropertiesAreInitialized()
        {
            // Arrange
            string emptyString = string.Empty;

            // Act
            var eventArgs = new StringReadEventArgs(emptyString);

            // Assert
            eventArgs.OriginalString.Should().Be(emptyString, "the constructor should initialize OriginalString with an empty string when provided");
            eventArgs.StringToBeUsed.Should().Be(emptyString, "the constructor should initialize StringToBeUsed with an empty string when provided");
        }

        /// <summary>
        /// Tests that the internal Reuse method correctly updates both OriginalString and StringToBeUsed properties with the new value.
        /// </summary>
        [Fact]
        public void Reuse_ValidNewValue_PropertiesAreUpdated()
        {
            // Arrange
            var eventArgs = new StringReadEventArgs(_initialString);
            
            // Act
            eventArgs.Reuse(_newString);

            // Assert
            eventArgs.OriginalString.Should().Be(_newString, "the Reuse method should update OriginalString with the new value");
            eventArgs.StringToBeUsed.Should().Be(_newString, "the Reuse method should update StringToBeUsed with the new value");
        }

        /// <summary>
        /// Tests that the Reuse method correctly updates properties when an empty string is provided.
        /// </summary>
        [Fact]
        public void Reuse_EmptyString_PropertiesAreUpdatedToEmpty()
        {
            // Arrange
            var eventArgs = new StringReadEventArgs(_initialString);
            string emptyString = string.Empty;

            // Act
            eventArgs.Reuse(emptyString);

            // Assert
            eventArgs.OriginalString.Should().Be(emptyString, "the Reuse method should update OriginalString to an empty string");
            eventArgs.StringToBeUsed.Should().Be(emptyString, "the Reuse method should update StringToBeUsed to an empty string");
        }
    }
}
