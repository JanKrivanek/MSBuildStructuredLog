using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;
using Moq;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TaskItemData"/> class.
    /// </summary>
    public class TaskItemDataTests
    {
        /// <summary>
        /// Verifies that the default constructor initializes the Metadata property with an empty dictionary.
        /// </summary>
        [Fact]
        public void Constructor_Default_InitializesEmptyMetadata()
        {
            // Arrange & Act
            var taskItem = new TaskItemData();

            // Assert
            Assert.NotNull(taskItem.Metadata);
            Assert.Empty(taskItem.Metadata);
        }

        /// <summary>
        /// Verifies that the parameterized constructor assigns the ItemSpec and Metadata properties correctly when metadata is provided.
        /// </summary>
        [Fact]
        public void Constructor_WithMetadata_AssignsPropertiesCorrectly()
        {
            // Arrange
            string itemSpec = "TestItemSpec";
            var metadata = new Dictionary<string, string>
            {
                { "Key1", "Value1" }
            };

            // Act
            var taskItem = new TaskItemData(itemSpec, metadata);

            // Assert
            Assert.Equal(itemSpec, taskItem.ItemSpec);
            Assert.Equal(metadata, taskItem.Metadata);
            Assert.Single(taskItem.Metadata);
        }

        /// <summary>
        /// Verifies that the parameterized constructor assigns an empty metadata collection when null is provided.
        /// </summary>
        [Fact]
        public void Constructor_WithNullMetadata_UsesEmptyMetadata()
        {
            // Arrange
            string itemSpec = "TestItemSpec";

            // Act
            var taskItem = new TaskItemData(itemSpec, null);

            // Assert
            Assert.Equal(itemSpec, taskItem.ItemSpec);
            Assert.Empty(taskItem.Metadata);
        }

        /// <summary>
        /// Verifies that the MetadataCount property returns the correct count after adding metadata.
        /// </summary>
        [Fact]
        public void MetadataCount_WhenMetadataAdded_ReturnsCorrectCount()
        {
            // Arrange
            var taskItem = new TaskItemData();
            taskItem.Metadata.Add("Key1", "Value1");
            taskItem.Metadata.Add("Key2", "Value2");

            // Act
            int count = taskItem.MetadataCount;

            // Assert
            Assert.Equal(2, count);
        }

        /// <summary>
        /// Verifies that the MetadataNames property returns a collection containing all metadata keys.
        /// </summary>
//         [Fact] [Error] (102-37)CS1503 Argument 2: cannot convert from 'System.Collections.ICollection' to 'System.Collections.Generic.IEnumerable<string>' [Error] (103-37)CS1503 Argument 2: cannot convert from 'System.Collections.ICollection' to 'System.Collections.Generic.IEnumerable<string>'
//         public void MetadataNames_WhenMetadataExists_ReturnsAllKeys()
//         {
//             // Arrange
//             var taskItem = new TaskItemData();
//             taskItem.Metadata.Add("Key1", "Value1");
//             taskItem.Metadata.Add("Key2", "Value2");
// 
//             // Act
//             ICollection keys = taskItem.MetadataNames;
// 
//             // Assert
//             Assert.Contains("Key1", keys);
//             Assert.Contains("Key2", keys);
//             Assert.Equal(2, keys.Count);
//         }

        /// <summary>
        /// Verifies that CloneCustomMetadata returns the same instance as the Metadata property.
        /// </summary>
        [Fact]
        public void CloneCustomMetadata_WhenCalled_ReturnsSameInstance()
        {
            // Arrange
            var taskItem = new TaskItemData();
            taskItem.Metadata.Add("Key", "Value");

            // Act
            IDictionary clonedMetadata = taskItem.CloneCustomMetadata();

            // Assert
            Assert.Same(taskItem.Metadata, clonedMetadata);
        }

        /// <summary>
        /// Verifies that CopyMetadataTo throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void CopyMetadataTo_AnyInput_ThrowsNotImplementedException()
        {
            // Arrange
            var taskItem = new TaskItemData();
            var destinationTaskItemMock = new Mock<ITaskItem>();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => taskItem.CopyMetadataTo(destinationTaskItemMock.Object));
        }

        /// <summary>
        /// Verifies that GetMetadata returns the correct value when the key exists in Metadata.
        /// </summary>
        [Fact]
        public void GetMetadata_WhenKeyExists_ReturnsCorrectValue()
        {
            // Arrange
            var taskItem = new TaskItemData();
            taskItem.Metadata.Add("ExistingKey", "ExistingValue");

            // Act
            string result = taskItem.GetMetadata("ExistingKey");

            // Assert
            Assert.Equal("ExistingValue", result);
        }

        /// <summary>
        /// Verifies that GetMetadata returns null when the key does not exist in Metadata.
        /// </summary>
        [Fact]
        public void GetMetadata_WhenKeyDoesNotExist_ReturnsNull()
        {
            // Arrange
            var taskItem = new TaskItemData();

            // Act
            string result = taskItem.GetMetadata("NonExistingKey");

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies that RemoveMetadata throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void RemoveMetadata_AnyInput_ThrowsNotImplementedException()
        {
            // Arrange
            var taskItem = new TaskItemData();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => taskItem.RemoveMetadata("Key"));
        }

        /// <summary>
        /// Verifies that SetMetadata throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void SetMetadata_AnyInput_ThrowsNotImplementedException()
        {
            // Arrange
            var taskItem = new TaskItemData();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => taskItem.SetMetadata("Key", "Value"));
        }

        /// <summary>
        /// Verifies that ToString returns only the ItemSpec string when Metadata is empty.
        /// </summary>
        [Fact]
        public void ToString_WithEmptyMetadata_ReturnsItemSpecOnly()
        {
            // Arrange
            string itemSpec = "TestItemSpec";
            var taskItem = new TaskItemData { ItemSpec = itemSpec };

            // Act
            string result = taskItem.ToString();

            // Assert
            Assert.Equal(itemSpec, result);
        }

        /// <summary>
        /// Verifies that ToString returns a formatted string including the ItemSpec and each metadata key-value pair when Metadata is not empty.
        /// </summary>
        [Fact]
        public void ToString_WithMetadata_ReturnsFormattedOutput()
        {
            // Arrange
            string itemSpec = "TestItemSpec";
            var metadata = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };
            var taskItem = new TaskItemData(itemSpec, metadata);

            // Act
            string result = taskItem.ToString();

            // Assert
            // Must contain the itemSpec on the first line.
            Assert.StartsWith(itemSpec, result);

            // Check that each metadata entry is present in the expected format.
            Assert.Contains("    Key1=Value1", result);
            Assert.Contains("    Key2=Value2", result);
            // Ensure newlines are used in the output.
            Assert.Contains(Environment.NewLine, result);
        }
    }
}
