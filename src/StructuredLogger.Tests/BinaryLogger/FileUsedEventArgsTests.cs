using System;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="FileUsedEventArgs"/> class.
    /// </summary>
    public class FileUsedEventArgsTests
    {
        /// <summary>
        /// Tests that the parameterless constructor initializes FilePath as null.
        /// Arrange: Create an instance using the default constructor.
        /// Act: Retrieve the FilePath property.
        /// Assert: FilePath should be null.
        /// </summary>
        [Fact]
        public void Ctor_Default_ShouldHaveNullFilePath()
        {
            // Arrange & Act
            var eventArgs = new FileUsedEventArgs();
            
            // Assert
            Assert.Null(eventArgs.FilePath);
        }
        
        /// <summary>
        /// Tests that the constructor with a response file path sets the FilePath property correctly.
        /// Arrange: Define a valid file path.
        /// Act: Create an instance using the parameterized constructor.
        /// Assert: FilePath should equal the given file path.
        /// </summary>
        [Theory]
        [InlineData("C:\\temp\\response.txt")]
        [InlineData("response.txt")]
        public void Ctor_WithValidResponseFilePath_ShouldSetFilePath(string responseFilePath)
        {
            // Arrange & Act
            var eventArgs = new FileUsedEventArgs(responseFilePath);
            
            // Assert
            Assert.Equal(responseFilePath, eventArgs.FilePath);
        }
        
        /// <summary>
        /// Tests that the constructor with an empty string sets the FilePath property to an empty string.
        /// Arrange: Define an empty string.
        /// Act: Create an instance using the parameterized constructor.
        /// Assert: FilePath should be an empty string.
        /// </summary>
        [Fact]
        public void Ctor_WithEmptyResponseFilePath_ShouldSetFilePathToEmptyString()
        {
            // Arrange
            string emptyPath = string.Empty;
            
            // Act
            var eventArgs = new FileUsedEventArgs(emptyPath);
            
            // Assert
            Assert.Equal(emptyPath, eventArgs.FilePath);
        }
        
        /// <summary>
        /// Tests that the FilePath property can be set and retrieved correctly after initialization.
        /// Arrange: Create an instance with an initial file path.
        /// Act: Update the FilePath property to a new value.
        /// Assert: The FilePath property should reflect the updated value.
        /// </summary>
        [Theory]
        [InlineData("initial.txt", "updated.txt")]
        [InlineData("C:\\file1.txt", "C:\\file2.txt")]
        [InlineData("", "noreply.txt")]
        public void FilePathProperty_SetAndGet_ShouldReflectChanges(string initialPath, string updatedPath)
        {
            // Arrange
            var eventArgs = new FileUsedEventArgs(initialPath);
            
            // Act
            eventArgs.FilePath = updatedPath;
            
            // Assert
            Assert.Equal(updatedPath, eventArgs.FilePath);
        }
    }
}
