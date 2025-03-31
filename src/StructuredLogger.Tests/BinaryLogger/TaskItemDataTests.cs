using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
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
        /// Tests that the default constructor initializes the Metadata property to a non-null dictionary.
        /// </summary>
        [Fact]
        public void Constructor_Default_CreatesNonNullMetadata()
        {
            // Arrange & Act
            var taskItemData = new TaskItemData();

            // Assert
            taskItemData.Metadata.Should().NotBeNull();
            taskItemData.Metadata.Should().BeEmpty();
        }

        /// <summary>
        /// Tests that the parameterized constructor correctly assigns provided itemSpec and metadata.
        /// </summary>
        [Fact]
        public void Constructor_WithMetadata_CreatesInstanceWithProvidedMetadata()
        {
            // Arrange
            string expectedItemSpec = "TestItem";
            var expectedMetadata = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

            // Act
            var taskItemData = new TaskItemData(expectedItemSpec, expectedMetadata);

            // Assert
            taskItemData.ItemSpec.Should().Be(expectedItemSpec);
            taskItemData.Metadata.Should().BeEquivalentTo(expectedMetadata);
        }

        /// <summary>
        /// Tests that the parameterized constructor assigns an empty metadata dictionary when null is provided.
        /// </summary>
        [Fact]
        public void Constructor_WithNullMetadata_AssignsEmptyMetadata()
        {
            // Arrange
            string expectedItemSpec = "TestItem";
            
            // Act
            var taskItemData = new TaskItemData(expectedItemSpec, null);

            // Assert
            taskItemData.ItemSpec.Should().Be(expectedItemSpec);
            taskItemData.Metadata.Should().NotBeNull();
            taskItemData.Metadata.Should().BeEmpty();
        }

        /// <summary>
        /// Tests that the MetadataCount property returns the correct count of metadata entries.
        /// </summary>
        [Fact]
        public void MetadataCount_ReturnsCorrectCount()
        {
            // Arrange
            var metadata = new Dictionary<string, string>
            {
                { "Key", "Value" }
            };
            var taskItemData = new TaskItemData("TestItem", metadata);

            // Act
            int count = taskItemData.MetadataCount;

            // Assert
            count.Should().Be(1);
        }

        /// <summary>
        /// Tests that the MetadataNames property returns the correct keys from the metadata.
        /// </summary>
        [Fact]
        public void MetadataNames_ReturnsCorrectKeys()
        {
            // Arrange
            var metadata = new Dictionary<string, string>
            {
                { "KeyA", "ValueA" },
                { "KeyB", "ValueB" }
            };
            var taskItemData = new TaskItemData("TestItem", metadata);

            // Act
            ICollection keys = taskItemData.MetadataNames;

            // Assert
            keys.Should().BeEquivalentTo(metadata.Keys);
        }

        /// <summary>
        /// Tests that CloneCustomMetadata returns the same metadata dictionary instance.
        /// </summary>
        [Fact]
        public void CloneCustomMetadata_ReturnsSameMetadataInstance()
        {
            // Arrange
            var metadata = new Dictionary<string, string>
            {
                { "Alpha", "Beta" }
            };
            var taskItemData = new TaskItemData("TestItem", metadata);

            // Act
            IDictionary clonedMetadata = taskItemData.CloneCustomMetadata();

            // Assert
            clonedMetadata.Should().BeSameAs(taskItemData.Metadata);
        }

        /// <summary>
        /// Tests that CopyMetadataTo throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void CopyMetadataTo_ThrowsNotImplementedException()
        {
            // Arrange
            var taskItemData = new TaskItemData("TestItem", null);
            var destinationMock = new Mock<ITaskItem>();

            // Act
            Action act = () => taskItemData.CopyMetadataTo(destinationMock.Object);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that GetMetadata returns the correct value for an existing metadata key.
        /// </summary>
        [Fact]
        public void GetMetadata_WithExistingKey_ReturnsValue()
        {
            // Arrange
            string key = "MyKey";
            string value = "MyValue";
            var metadata = new Dictionary<string, string>
            {
                { key, value }
            };
            var taskItemData = new TaskItemData("TestItem", metadata);

            // Act
            string result = taskItemData.GetMetadata(key);

            // Assert
            result.Should().Be(value);
        }

        /// <summary>
        /// Tests that GetMetadata returns null for a non-existing metadata key.
        /// </summary>
        [Fact]
        public void GetMetadata_WithNonExistingKey_ReturnsNull()
        {
            // Arrange
            var metadata = new Dictionary<string, string>
            {
                { "ExistKey", "ExistValue" }
            };
            var taskItemData = new TaskItemData("TestItem", metadata);
            string nonExistingKey = "NonExistKey";

            // Act
            string result = taskItemData.GetMetadata(nonExistingKey);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Tests that RemoveMetadata throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void RemoveMetadata_ThrowsNotImplementedException()
        {
            // Arrange
            var taskItemData = new TaskItemData("TestItem", null);

            // Act
            Action act = () => taskItemData.RemoveMetadata("AnyKey");

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that SetMetadata throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void SetMetadata_ThrowsNotImplementedException()
        {
            // Arrange
            var taskItemData = new TaskItemData("TestItem", null);

            // Act
            Action act = () => taskItemData.SetMetadata("AnyKey", "AnyValue");

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that ToString returns the ItemSpec when there is no metadata.
        /// </summary>
        [Fact]
        public void ToString_WithNoMetadata_ReturnsItemSpec()
        {
            // Arrange
            string itemSpec = "TestItemSpec";
            var taskItemData = new TaskItemData(itemSpec, new Dictionary<string, string>());

            // Act
            string result = taskItemData.ToString();

            // Assert
            result.Should().Be(itemSpec);
        }

        /// <summary>
        /// Tests that ToString returns a formatted string including metadata when metadata exists.
        /// </summary>
        [Fact]
        public void ToString_WithMetadata_ReturnsFormattedString()
        {
            // Arrange
            string itemSpec = "TestItemSpec";
            var metadata = new Dictionary<string, string>
            {
                { "Meta1", "Value1" },
                { "Meta2", "Value2" }
            };
            var taskItemData = new TaskItemData(itemSpec, metadata);

            // Act
            string result = taskItemData.ToString();

            // Assert
            // Expected result starts with itemSpec followed by each metadata entry formatted appropriately.
            var expectedLines = new List<string>
            {
                itemSpec,
                $"    Meta1=Value1",
                $"    Meta2=Value2"
            };
            
            // Because the order of metadata entries is not guaranteed, we check for all expected lines (except the first line order must be maintained).
            string[] resultLines = result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            resultLines[0].Should().Be(itemSpec);
            resultLines.Should().Contain($"    Meta1=Value1");
            resultLines.Should().Contain($"    Meta2=Value2");
            resultLines.Length.Should().Be(3);
        }
    }
}
