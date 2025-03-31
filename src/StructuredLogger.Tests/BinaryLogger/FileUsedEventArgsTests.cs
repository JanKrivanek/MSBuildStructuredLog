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
        /// </summary>
        [Fact]
        public void DefaultConstructor_ShouldInitializeWithNullFilePath()
        {
            // Arrange & Act
            var instance = new FileUsedEventArgs();

            // Assert
            instance.FilePath.Should().BeNull("because the default constructor does not set FilePath");
        }

        /// <summary>
        /// Tests that the parameterized constructor sets the FilePath property correctly when passed a valid file path.
        /// </summary>
        [Fact]
        public void ParameterizedConstructor_WithValidFilePath_ShouldSetFilePath()
        {
            // Arrange
            string expectedFilePath = "C:\\temp\\response.txt";

            // Act
            var instance = new FileUsedEventArgs(expectedFilePath);

            // Assert
            instance.FilePath.Should().Be(expectedFilePath, "because the constructor should assign the provided file path to FilePath");
        }

        /// <summary>
        /// Tests that the parameterized constructor correctly sets the FilePath property when passed an empty string.
        /// </summary>
        [Fact]
        public void ParameterizedConstructor_WithEmptyString_ShouldSetFilePathToEmpty()
        {
            // Arrange
            string expectedFilePath = string.Empty;

            // Act
            var instance = new FileUsedEventArgs(expectedFilePath);

            // Assert
            instance.FilePath.Should().Be(expectedFilePath, "because the constructor should assign even an empty string to FilePath");
        }

        /// <summary>
        /// Tests that the FilePath property can be set to a non-null value and retrieved correctly.
        /// </summary>
        [Fact]
        public void FilePathProperty_SetAndGet_ShouldPersistValue()
        {
            // Arrange
            var instance = new FileUsedEventArgs();
            string expectedFilePath = "C:\\data\\file.txt";

            // Act
            instance.FilePath = expectedFilePath;
            string? actualFilePath = instance.FilePath;

            // Assert
            actualFilePath.Should().Be(expectedFilePath, "because the property setter should correctly assign the value");
        }

        /// <summary>
        /// Tests that the FilePath property can be set to null and retrieved as null.
        /// </summary>
        [Fact]
        public void FilePathProperty_CanBeSetToNull_ShouldBeNull()
        {
            // Arrange
            var instance = new FileUsedEventArgs();
            instance.FilePath = "NonNullValue";

            // Act
            instance.FilePath = null;
            string? actualFilePath = instance.FilePath;

            // Assert
            actualFilePath.Should().BeNull("because the property accepts nullable values and should reflect null when set");
        }
    }
}
