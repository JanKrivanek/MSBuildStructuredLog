using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TaskItem"/> class.
    /// </summary>
    public class TaskItemTests
    {
        /// <summary>
        /// Tests that the default constructor of TaskItem initializes properties to their expected defaults.
        /// </summary>
        [Fact]
        public void DefaultConstructor_InitializesPropertiesCorrectly()
        {
            // Arrange & Act
            var taskItem = new TaskItem();

            // Assert
            Assert.Null(taskItem.ItemSpec);
            Assert.NotNull(taskItem.Metadata);
            Assert.Equal(0, taskItem.MetadataCount);
            Assert.Null(taskItem.EvaluatedIncludeEscaped);
            Assert.NotNull(taskItem.MetadataNames);
        }

        /// <summary>
        /// Tests that the constructor with an itemSpec parameter sets the ItemSpec property.
        /// </summary>
        [Fact]
        public void ConstructorWithItemSpec_SetsItemSpecProperty()
        {
            // Arrange
            string expectedItemSpec = "TestItemSpec";

            // Act
            var taskItem = new TaskItem(expectedItemSpec);

            // Assert
            Assert.Equal(expectedItemSpec, taskItem.ItemSpec);
        }

        /// <summary>
        /// Tests that CloneCustomMetadata returns a dictionary containing the same metadata entries.
        /// </summary>
        [Fact]
        public void CloneCustomMetadata_WhenCalled_ReturnsSameMetadataEntries()
        {
            // Arrange
            var taskItem = new TaskItem();
            taskItem.SetMetadata("Key1", "Value1");
            taskItem.SetMetadata("Key2", "Value2");

            // Act
            IDictionary clonedMetadata = taskItem.CloneCustomMetadata();

            // Assert
            Assert.NotNull(clonedMetadata);
            Assert.Equal(2, clonedMetadata.Count);
            Assert.Equal("Value1", clonedMetadata["Key1"]);
            Assert.Equal("Value2", clonedMetadata["Key2"]);
        }

        /// <summary>
        /// Tests that CloneCustomMetadataEscaped throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void CloneCustomMetadataEscaped_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            var taskItem = new TaskItem();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => taskItem.CloneCustomMetadataEscaped());
        }

        /// <summary>
        /// Tests that CopyMetadataTo correctly copies all metadata to the destination ITaskItem.
        /// </summary>
        [Fact]
        public void CopyMetadataTo_WhenCalled_CopiesAllMetadataToDestination()
        {
            // Arrange
            var taskItem = new TaskItem("TestItem");
            taskItem.SetMetadata("Key1", "Value1");
            taskItem.SetMetadata("Key2", "Value2");

            var mockDestination = new Mock<ITaskItem>();

            // Act
            taskItem.CopyMetadataTo(mockDestination.Object);

            // Assert
            mockDestination.Verify(m => m.SetMetadata("Key1", "Value1"), Times.Once);
            mockDestination.Verify(m => m.SetMetadata("Key2", "Value2"), Times.Once);
        }

        /// <summary>
        /// Tests that GetMetadata returns the metadata value when it exists.
        /// </summary>
        [Fact]
        public void GetMetadata_WhenMetadataExists_ReturnsMetadataValue()
        {
            // Arrange
            var taskItem = new TaskItem("TestItemSpec");
            taskItem.SetMetadata("CustomKey", "CustomValue");

            // Act
            string result = taskItem.GetMetadata("CustomKey");

            // Assert
            Assert.Equal("CustomValue", result);
        }

        /// <summary>
        /// Tests that GetMetadata returns the ItemSpec when the requested metadata is "FullPath" and does not exist in metadata.
        /// </summary>
        [Fact]
        public void GetMetadata_WhenMetadataDoesNotExistAndKeyIsFullPath_ReturnsItemSpec()
        {
            // Arrange
            string expectedItemSpec = "FullPathValue";
            var taskItem = new TaskItem(expectedItemSpec);

            // Act
            string result = taskItem.GetMetadata("FullPath");

            // Assert
            Assert.Equal(expectedItemSpec, result);
        }

        /// <summary>
        /// Tests that GetMetadata returns an empty string when the requested metadata does not exist and is not "FullPath".
        /// </summary>
        [Fact]
        public void GetMetadata_WhenMetadataDoesNotExistAndKeyIsNotFullPath_ReturnsEmptyString()
        {
            // Arrange
            var taskItem = new TaskItem("TestItemSpec");

            // Act
            string result = taskItem.GetMetadata("NonExistentKey");

            // Assert
            Assert.Equal(string.Empty, result);
        }

        /// <summary>
        /// Tests that GetMetadataValueEscaped throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void GetMetadataValueEscaped_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            var taskItem = new TaskItem();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => taskItem.GetMetadataValueEscaped("AnyKey"));
        }

        /// <summary>
        /// Tests that RemoveMetadata throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void RemoveMetadata_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            var taskItem = new TaskItem();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => taskItem.RemoveMetadata("AnyKey"));
        }

        /// <summary>
        /// Tests that SetMetadata correctly sets the metadata value.
        /// </summary>
        [Fact]
        public void SetMetadata_WhenCalled_SetsMetadataCorrectly()
        {
            // Arrange
            var taskItem = new TaskItem();

            // Act
            taskItem.SetMetadata("Key", "Value");

            // Assert
            Assert.Equal("Value", taskItem.Metadata["Key"]);
        }

        /// <summary>
        /// Tests that SetMetadataValueLiteral correctly sets the metadata by internally calling SetMetadata.
        /// </summary>
        [Fact]
        public void SetMetadataValueLiteral_WhenCalled_SetsMetadataCorrectly()
        {
            // Arrange
            var taskItem = new TaskItem();

            // Act
            taskItem.SetMetadataValueLiteral("LiteralKey", "LiteralValue");

            // Assert
            Assert.Equal("LiteralValue", taskItem.Metadata["LiteralKey"]);
        }

        /// <summary>
        /// Tests that ToString returns the ItemSpec of the TaskItem.
        /// </summary>
        [Fact]
        public void ToString_WhenCalled_ReturnsItemSpec()
        {
            // Arrange
            string expectedItemSpec = "TestItemSpec";
            var taskItem = new TaskItem(expectedItemSpec);

            // Act
            string result = taskItem.ToString();

            // Assert
            Assert.Equal(expectedItemSpec, result);
        }

        /// <summary>
        /// Tests that MetadataCount reflects the number of metadata entries after adding entries.
        /// </summary>
        [Fact]
        public void MetadataCount_WhenMetadataAdded_ReturnsCorrectCount()
        {
            // Arrange
            var taskItem = new TaskItem();
            taskItem.SetMetadata("Key1", "Value1");
            taskItem.SetMetadata("Key2", "Value2");

            // Act
            int count = taskItem.MetadataCount;

            // Assert
            Assert.Equal(2, count);
        }

        /// <summary>
        /// Tests that MetadataNames returns a collection containing all metadata keys.
        /// </summary>
        [Fact]
        public void MetadataNames_WhenMetadataAdded_ReturnsAllKeys()
        {
            // Arrange
            var taskItem = new TaskItem();
            taskItem.SetMetadata("Key1", "Value1");
            taskItem.SetMetadata("Key2", "Value2");

            // Act
            ICollection names = taskItem.MetadataNames;

            // Assert
            Assert.Contains("Key1", (IEnumerable<string>)names);
            Assert.Contains("Key2", (IEnumerable<string>)names);
        }
    }
}
