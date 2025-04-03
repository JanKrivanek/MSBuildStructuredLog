using FluentAssertions;
using Microsoft.Build.Framework;
using System;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="FileUsedEventArgs"/> class.
    /// </summary>
    public class FileUsedEventArgsTests
    {
        /// <summary>
        /// Tests that the default constructor initializes the FilePath property to null.
        /// This test follows the Arrange-Act-Assert pattern:
        /// Arrange: No additional setup is required.
        /// Act: A new instance is created using the default constructor.
        /// Assert: The FilePath property should be null.
        /// </summary>
        [Fact]
        public void Ctor_Default_ShouldHaveNullFilePath()
        {
            // Act
            var eventArgs = new FileUsedEventArgs();

            // Assert
            eventArgs.FilePath.Should().BeNull("because the default constructor should not initialize FilePath");
        }

        /// <summary>
        /// Tests that the parameterized constructor sets the FilePath property correctly for various valid string inputs.
        /// This test verifies the happy path and several edge cases for string inputs.
        /// </summary>
        /// <param name="input">The response file path to be assigned to FilePath.</param>
        [Theory]
        [InlineData("C:\\temp\\response.txt")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("path/with/special!@#$%^&*()")]
        public void Ctor_WithResponseFilePath_ValidValue_SetsFilePath(string input)
        {
            // Act
            var eventArgs = new FileUsedEventArgs(input);

            // Assert
            eventArgs.FilePath.Should().Be(input, "because the constructor should assign the provided input to FilePath");
        }

        /// <summary>
        /// Tests that the parameterized constructor correctly handles an extremely long string input.
        /// This test covers the boundary condition of a very large string value.
        /// </summary>
        [Fact]
        public void Ctor_WithResponseFilePath_ExtremelyLongString_SetsFilePath()
        {
            // Arrange
            string longString = new string('a', 10000);

            // Act
            var eventArgs = new FileUsedEventArgs(longString);

            // Assert
            eventArgs.FilePath.Should().Be(longString, "because the constructor should handle extremely long strings correctly");
        }

        /// <summary>
        /// Tests that the FilePath property can be updated after construction.
        /// This test verifies that the setter and getter of the FilePath property work as expected.
        /// </summary>
        [Fact]
        public void FilePath_Property_SetAndGet_ReturnsSetValue()
        {
            // Arrange
            var eventArgs = new FileUsedEventArgs();
            string expectedValue = "UpdatedPath";

            // Act
            eventArgs.FilePath = expectedValue;

            // Assert
            eventArgs.FilePath.Should().Be(expectedValue, "because the FilePath property should store and return the value set to it");
        }
    }
}
