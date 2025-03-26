using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="FileUsedEventArgs"/> class.
    /// </summary>
    [TestClass]
    public class FileUsedEventArgsTests
    {
        /// <summary>
        /// Tests that the default constructor initializes the FilePath property to null.
        /// </summary>
        [TestMethod]
        public void DefaultConstructor_WhenCalled_FilePathIsNull()
        {
            // Arrange & Act
            FileUsedEventArgs eventArgs = new FileUsedEventArgs();

            // Assert
            Assert.IsNull(eventArgs.FilePath, "Expected FilePath to be null when FileUsedEventArgs is constructed using the default constructor.");
        }

        /// <summary>
        /// Tests that the parameterized constructor correctly sets the FilePath property when a non-null value is provided.
        /// </summary>
        [TestMethod]
        public void ParameterizedConstructor_WithValidResponseFilePath_SetsFilePath()
        {
            // Arrange
            string expectedPath = "C:\\Temp\\response.txt";

            // Act
            FileUsedEventArgs eventArgs = new FileUsedEventArgs(expectedPath);

            // Assert
            Assert.AreEqual(expectedPath, eventArgs.FilePath, "FilePath should match the value provided to the constructor.");
        }

        /// <summary>
        /// Tests that the parameterized constructor correctly handles a null responseFilePath.
        /// </summary>
        [TestMethod]
        public void ParameterizedConstructor_WithNullResponseFilePath_SetsFilePathToNull()
        {
            // Arrange
            string? expectedPath = null;

            // Act
            FileUsedEventArgs eventArgs = new FileUsedEventArgs(expectedPath);

            // Assert
            Assert.IsNull(eventArgs.FilePath, "FilePath should be null when a null value is passed to the constructor.");
        }

        /// <summary>
        /// Tests that the FilePath property can be set after object creation and correctly returns the set value.
        /// </summary>
        [TestMethod]
        public void FilePathProperty_WhenSet_ReturnsSameValue()
        {
            // Arrange
            FileUsedEventArgs eventArgs = new FileUsedEventArgs();
            string expectedValue = "C:\\NewPath\\file.txt";

            // Act
            eventArgs.FilePath = expectedValue;

            // Assert
            Assert.AreEqual(expectedValue, eventArgs.FilePath, "The FilePath property did not return the expected value after being set.");
        }
    }
}
