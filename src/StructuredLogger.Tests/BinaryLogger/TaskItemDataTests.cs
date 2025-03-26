using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TaskItemData"/> class.
    /// </summary>
    public class TaskItemDataTests
    {
        /// <summary>
        /// Tests that the default constructor initializes the Metadata property to an empty dictionary.
        /// </summary>
        [Fact]
        public void DefaultConstructor_InitializesMetadataToEmptyDictionary()
        {
            // Arrange & Act
            TaskItemData itemData = new TaskItemData();

            // Assert
            Assert.NotNull(itemData.Metadata);
            Assert.Empty(itemData.Metadata);
            Assert.Equal(0, itemData.MetadataCount);
            Assert.IsAssignableFrom<ICollection>(itemData.MetadataNames);
        }

        /// <summary>
        /// Tests that the parameterized constructor initializes properties correctly when a non-null metadata dictionary is provided.
        /// </summary>
        [Fact]
        public void ParameterizedConstructor_WithNonNullMetadata_InitializesPropertiesCorrectly()
        {
            // Arrange
            string expectedItemSpec = "TestItem";
            var metadata = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

            // Act
            TaskItemData itemData = new TaskItemData(expectedItemSpec, metadata);

            // Assert
            Assert.Equal(expectedItemSpec, itemData.ItemSpec);
            Assert.Same(metadata, itemData.Metadata);
            Assert.Equal(2, itemData.MetadataCount);
            Assert.Equal(metadata.Keys, itemData.MetadataNames);
        }

        /// <summary>
        /// Tests that the parameterized constructor uses an empty metadata collection when provided with a null metadata parameter.
        /// </summary>
        [Fact]
        public void ParameterizedConstructor_WithNullMetadata_UsesEmptyMetadata()
        {
            // Arrange
            string expectedItemSpec = "TestItem";

            // Act
            TaskItemData itemData = new TaskItemData(expectedItemSpec, null);

            // Assert
            Assert.Equal(expectedItemSpec, itemData.ItemSpec);
            Assert.NotNull(itemData.Metadata);
            Assert.Empty(itemData.Metadata);
            Assert.Equal(0, itemData.MetadataCount);
        }

        /// <summary>
        /// Tests that GetMetadata returns the corresponding metadata value when the key exists.
        /// </summary>
        [Fact]
        public void GetMetadata_WhenKeyExists_ReturnsCorrespondingValue()
        {
            // Arrange
            var itemData = new TaskItemData
            {
                ItemSpec = "ItemSpec"
            };
            itemData.Metadata["TestKey"] = "TestValue";

            // Act
            string result = itemData.GetMetadata("TestKey");

            // Assert
            Assert.Equal("TestValue", result);
        }

        /// <summary>
        /// Tests that GetMetadata returns null when the requested metadata key does not exist.
        /// </summary>
        [Fact]
        public void GetMetadata_WhenKeyDoesNotExist_ReturnsNull()
        {
            // Arrange
            var itemData = new TaskItemData
            {
                ItemSpec = "ItemSpec"
            };
            itemData.Metadata["ExistingKey"] = "Value";

            // Act
            string result = itemData.GetMetadata("NonExistingKey");

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that CloneCustomMetadata returns the same metadata collection instance.
        /// </summary>
        [Fact]
        public void CloneCustomMetadata_ReturnsSameInstanceAsMetadata()
        {
            // Arrange
            var itemData = new TaskItemData
            {
                ItemSpec = "ItemSpec"
            };
            itemData.Metadata["Key"] = "Value";

            // Act
            IDictionary clonedMetadata = itemData.CloneCustomMetadata();

            // Assert
            Assert.Same(itemData.Metadata, clonedMetadata);
        }

        /// <summary>
        /// Tests that ToString returns only the ItemSpec when no metadata is present.
        /// </summary>
        [Fact]
        public void ToString_WhenNoMetadata_ReturnsItemSpecOnly()
        {
            // Arrange
            string expectedItemSpec = "OnlyItemSpec";
            var itemData = new TaskItemData
            {
                ItemSpec = expectedItemSpec
            };

            // Act
            string result = itemData.ToString();

            // Assert
            Assert.Equal(expectedItemSpec, result);
        }

        /// <summary>
        /// Tests that ToString returns a formatted string containing ItemSpec and its metadata when metadata is present.
        /// </summary>
        [Fact]
        public void ToString_WhenMetadataExists_ReturnsFormattedString()
        {
            // Arrange
            string expectedItemSpec = "TestItem";
            var metadata = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };
            TaskItemData itemData = new TaskItemData(expectedItemSpec, metadata);

            // Act
            string result = itemData.ToString();

            // Assert
            // Expecting first line to be ItemSpec, then each metadata entry on new line with 4 spaces indent.
            StringBuilder expectedBuilder = new StringBuilder();
            expectedBuilder.AppendLine(expectedItemSpec);
            foreach (var kvp in metadata)
            {
                expectedBuilder.Append("    ");
                expectedBuilder.Append(kvp.Key);
                expectedBuilder.Append("=");
                expectedBuilder.AppendLine(kvp.Value);
            }
            Assert.Equal(expectedBuilder.ToString(), result);
        }

        /// <summary>
        /// Tests that CopyMetadataTo throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void CopyMetadataTo_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var itemData = new TaskItemData();
            ITaskItem dummyDestination = new DummyTaskItem();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => itemData.CopyMetadataTo(dummyDestination));
        }

        /// <summary>
        /// Tests that RemoveMetadata throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void RemoveMetadata_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var itemData = new TaskItemData();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => itemData.RemoveMetadata("SomeKey"));
        }

        /// <summary>
        /// Tests that SetMetadata throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void SetMetadata_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var itemData = new TaskItemData();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => itemData.SetMetadata("SomeKey", "SomeValue"));
        }

        /// <summary>
        /// A dummy implementation of ITaskItem for testing purposes.
        /// </summary>
        private class DummyTaskItem : ITaskItem
        {
            public string ItemSpec { get; set; }
            public IDictionary<string, string> Metadata { get; }
            public int MetadataCount => 0;
            public ICollection MetadataNames => new List<string>();

            public IDictionary CloneCustomMetadata()
            {
                return new Dictionary<string, string>();
            }

            public void CopyMetadataTo(ITaskItem destinationItem)
            {
                // No implementation
            }

            public string GetMetadata(string metadataName)
            {
                return null;
            }

            public void RemoveMetadata(string metadataName)
            {
                // No implementation
            }

            public void SetMetadata(string metadataName, string metadataValue)
            {
                // No implementation
            }

            public override string ToString()
            {
                return ItemSpec;
            }
        }
    }
}
