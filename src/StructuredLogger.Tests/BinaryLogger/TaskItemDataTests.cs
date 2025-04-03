using FluentAssertions;
using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TaskItemData"/> class.
    /// </summary>
    public class TaskItemDataTests
    {
        /// <summary>
        /// Tests that the parameterless constructor initializes Metadata as an empty dictionary and ItemSpec is default.
        /// </summary>
        [Fact]
        public void Ctor_WithoutParameters_InitializesEmptyMetadata()
        {
            // Arrange & Act
            TaskItemData taskItem = new TaskItemData();

            // Assert
            taskItem.Metadata.Should().NotBeNull();
            taskItem.Metadata.Should().BeEmpty();
            // ItemSpec is not initialized so it is null by default since it's auto-property.
            taskItem.ItemSpec.Should().BeNull();
        }

        /// <summary>
        /// Tests that the constructor with parameters sets the ItemSpec and Metadata correctly when metadata is provided.
        /// </summary>
        [Fact]
        public void Ctor_WithParameters_MetadataProvided_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedItemSpec = "TestItem";
            var expectedMetadata = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

            // Act
            TaskItemData taskItem = new TaskItemData(expectedItemSpec, expectedMetadata);

            // Assert
            taskItem.ItemSpec.Should().Be(expectedItemSpec);
            taskItem.Metadata.Should().BeEquivalentTo(expectedMetadata);
        }

        /// <summary>
        /// Tests that the constructor with parameters sets Metadata to an empty dictionary when a null reference is provided.
        /// </summary>
        [Fact]
        public void Ctor_WithParameters_MetadataNull_SetsMetadataToEmptyDictionary()
        {
            // Arrange
            string expectedItemSpec = "TestItem";

            // Act
            TaskItemData taskItem = new TaskItemData(expectedItemSpec, null);

            // Assert
            taskItem.ItemSpec.Should().Be(expectedItemSpec);
            taskItem.Metadata.Should().NotBeNull();
            taskItem.Metadata.Should().BeEmpty();
        }

        /// <summary>
        /// Tests that the MetadataCount property returns the count of metadata entries.
        /// </summary>
        [Fact]
        public void MetadataCount_WhenMetadataEntriesExist_ReturnsCorrectCount()
        {
            // Arrange
            var metadata = new Dictionary<string, string>
            {
                { "A", "Alpha" },
                { "B", "Beta" }
            };
            TaskItemData taskItem = new TaskItemData("Item", metadata);

            // Act
            int count = taskItem.MetadataCount;

            // Assert
            count.Should().Be(2);
        }

        /// <summary>
        /// Tests that the MetadataNames property returns a collection of the metadata keys.
        /// </summary>
        [Fact]
        public void MetadataNames_WhenMetadataExists_ReturnsKeysCollection()
        {
            // Arrange
            var metadata = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };
            TaskItemData taskItem = new TaskItemData("Item", metadata);

            // Act
            ICollection metadataNames = taskItem.MetadataNames;

            // Assert
            metadataNames.Should().NotBeNull();
            metadataNames.Should().BeEquivalentTo(metadata.Keys);
        }

        /// <summary>
        /// Tests that CloneCustomMetadata returns the original metadata dictionary, not a clone.
        /// </summary>
        [Fact]
        public void CloneCustomMetadata_Always_ReturnsSameMetadataReference()
        {
            // Arrange
            var metadata = new Dictionary<string, string>
            {
                { "Key", "Value" }
            };
            TaskItemData taskItem = new TaskItemData("Item", metadata);

            // Act
            IDictionary clonedMetadata = taskItem.CloneCustomMetadata();

            // Assert
            clonedMetadata.Should().BeSameAs(taskItem.Metadata);
        }

        /// <summary>
        /// Tests that CopyMetadataTo throws NotImplementedException.
        /// </summary>
        [Fact]
        public void CopyMetadataTo_Always_ThrowsNotImplementedException()
        {
            // Arrange
            TaskItemData taskItem = new TaskItemData();
            // Using a dummy instance of TaskItemData as destination.
            TaskItemData destinationItem = new TaskItemData();

            // Act
            Action action = () => taskItem.CopyMetadataTo(destinationItem);

            // Assert
            action.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that GetMetadata returns the value for an existing metadata key.
        /// </summary>
        [Theory]
        [InlineData("Key1", "Value1")]
        [InlineData("Key2", "Value2")]
        public void GetMetadata_WhenKeyExists_ReturnsCorrectValue(string key, string expectedValue)
        {
            // Arrange
            var metadata = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };
            TaskItemData taskItem = new TaskItemData("Item", metadata);

            // Act
            string result = taskItem.GetMetadata(key);

            // Assert
            result.Should().Be(expectedValue);
        }

        /// <summary>
        /// Tests that GetMetadata returns null for a metadata key that does not exist.
        /// </summary>
        [Fact]
        public void GetMetadata_WhenKeyDoesNotExist_ReturnsNull()
        {
            // Arrange
            var metadata = new Dictionary<string, string>
            {
                { "ExistingKey", "SomeValue" }
            };
            TaskItemData taskItem = new TaskItemData("Item", metadata);

            // Act
            string result = taskItem.GetMetadata("NonExistingKey");

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Tests that RemoveMetadata throws NotImplementedException.
        /// </summary>
        [Fact]
        public void RemoveMetadata_Always_ThrowsNotImplementedException()
        {
            // Arrange
            TaskItemData taskItem = new TaskItemData();

            // Act
            Action action = () => taskItem.RemoveMetadata("AnyKey");

            // Assert
            action.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that SetMetadata throws NotImplementedException.
        /// </summary>
        [Fact]
        public void SetMetadata_Always_ThrowsNotImplementedException()
        {
            // Arrange
            TaskItemData taskItem = new TaskItemData();

            // Act
            Action action = () => taskItem.SetMetadata("Key", "Value");

            // Assert
            action.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests ToString returns the ItemSpec when there is no metadata.
        /// </summary>
        [Fact]
        public void ToString_NoMetadata_ReturnsItemSpec()
        {
            // Arrange
            string expectedItemSpec = "TestItemSpec";
            TaskItemData taskItem = new TaskItemData
            {
                ItemSpec = expectedItemSpec
            };

            // Act
            string result = taskItem.ToString();

            // Assert
            result.Should().Be(expectedItemSpec);
        }

        /// <summary>
        /// Tests ToString returns correctly formatted string when metadata exists.
        /// </summary>
        [Fact]
        public void ToString_WithMetadata_ReturnsFormattedString()
        {
            // Arrange
            string itemSpec = "ItemSpecValue";
            var metadata = new Dictionary<string, string>
            {
                { "MetaKey1", "MetaValue1" },
                { "MetaKey2", "MetaValue2" }
            };
            TaskItemData taskItem = new TaskItemData(itemSpec, metadata);
            // Build expected string.
            // It first appends ItemSpec with a new line, then for each metadata, appends "    " + key + "=" + value with a new line.
            var expectedBuilder = new System.Text.StringBuilder();
            expectedBuilder.AppendLine(itemSpec);
            foreach (KeyValuePair<string, string> pair in metadata)
            {
                expectedBuilder.Append("    ");
                expectedBuilder.Append(pair.Key);
                expectedBuilder.Append("=");
                expectedBuilder.AppendLine(pair.Value);
            }
            string expected = expectedBuilder.ToString();

            // Act
            string result = taskItem.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests GetMetadata with edge case keys such as empty string and strings with special characters.
        /// </summary>
        [Theory]
        [InlineData("", "EmptyValue")]
        [InlineData("!@#$%^&*()", "SpecialValue")]
        public void GetMetadata_EdgeCaseKeys_ReturnsCorrectValue(string key, string expectedValue)
        {
            // Arrange
            var metadata = new Dictionary<string, string>
            {
                { key, expectedValue }
            };
            TaskItemData taskItem = new TaskItemData("Item", metadata);

            // Act
            string result = taskItem.GetMetadata(key);

            // Assert
            result.Should().Be(expectedValue);
        }
    }
}
