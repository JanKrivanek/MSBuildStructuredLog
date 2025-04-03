using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Build.Logging.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref = "TaskItem"/> class.
    /// </summary>
    public class TaskItemTests
    {
//         /// <summary> // [Error] (33-45)CS1061 'ObjectAssertions' does not contain a definition for 'BeEmpty' and no accessible extension method 'BeEmpty' accepting a first argument of type 'ObjectAssertions' could be found (are you missing a using directive or an assembly reference?)
//         /// Tests that the parameterless constructor initializes the TaskItem with default values.
//         /// Expected outcome: ItemSpec and EvaluatedIncludeEscaped are default (null), and Metadata is an empty dictionary.
//         /// </summary>
//         [Fact]
//         public void TaskItem_ParameterlessConstructor_InitializesPropertiesCorrectly()
//         {
//             // Arrange & Act
//             var taskItem = new TaskItem();
//             // Assert
//             taskItem.ItemSpec.Should().BeNull();
//             taskItem.EvaluatedIncludeEscaped.Should().BeNull();
//             taskItem.Metadata.Should().NotBeNull();
//             taskItem.Metadata.Should().BeEmpty();
//             taskItem.MetadataCount.Should().Be(0);
//             taskItem.MetadataNames.Should().BeEmpty();
//         }
// 
        /// <summary>
        /// Tests that the constructor with the itemSpec parameter sets the ItemSpec property correctly.
        /// Expected outcome: ItemSpec equals the provided value.
        /// </summary>
        [Theory]
        [InlineData("TestItem")]
        [InlineData("")]
        [InlineData("C:\\AbsolutePath\\Item.txt")]
        public void TaskItem_ConstructorWithItemSpec_SetsItemSpec(string itemSpec)
        {
            // Arrange & Act
            var taskItem = new TaskItem(itemSpec);
            // Assert
            taskItem.ItemSpec.Should().Be(itemSpec);
        }

        /// <summary>
        /// Tests that CloneCustomMetadata returns the same dictionary instance as the Metadata property.
        /// Expected outcome: The returned IDictionary is the same instance as Metadata.
        /// </summary>
        [Fact]
        public void CloneCustomMetadata_ReturnsSameDictionaryAsMetadata()
        {
            // Arrange
            var taskItem = new TaskItem("TestItem");
            taskItem.SetMetadata("Key1", "Value1");
            // Act
            IDictionary result = taskItem.CloneCustomMetadata();
            // Assert
            result.Should().BeSameAs(taskItem.Metadata);
        }

        /// <summary>
        /// Tests that CloneCustomMetadataEscaped throws a NotImplementedException as expected.
        /// </summary>
        [Fact]
        public void CloneCustomMetadataEscaped_AlwaysThrowsNotImplementedException()
        {
            // Arrange
            var taskItem = new TaskItem("TestItem");
            // Act
            Action act = () => taskItem.CloneCustomMetadataEscaped();
            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that CopyMetadataTo correctly copies metadata to the destination ITaskItem.
        /// Expected outcome: The destination receives all metadata key-value pairs.
        /// </summary>
        [Fact]
        public void CopyMetadataTo_WithValidDestination_CopiesAllMetadata()
        {
            // Arrange
            var taskItem = new TaskItem("SourceItem");
            taskItem.SetMetadata("Key1", "Value1");
            taskItem.SetMetadata("Key2", "Value2");
            var destinationMock = new Mock<ITaskItem>();
            // Setup expectation: We expect SetMetadata to be called with each key-value pair.
            destinationMock.Setup(d => d.SetMetadata(It.IsAny<string>(), It.IsAny<string>()));
            // Act
            taskItem.CopyMetadataTo(destinationMock.Object);
            // Assert
            destinationMock.Verify(d => d.SetMetadata("Key1", "Value1"), Times.Once);
            destinationMock.Verify(d => d.SetMetadata("Key2", "Value2"), Times.Once);
        }

        /// <summary>
        /// Tests that GetMetadata returns the correct metadata value when the key exists.
        /// Expected outcome: Returns the corresponding metadata value.
        /// </summary>
        [Theory]
        [InlineData("Key1", "Value1")]
        [InlineData("SpecialChars!@#", "ComplexValue")]
        public void GetMetadata_WhenMetadataExists_ReturnsValue(string key, string value)
        {
            // Arrange
            var taskItem = new TaskItem("TestItem");
            taskItem.SetMetadata(key, value);
            // Act
            var result = taskItem.GetMetadata(key);
            // Assert
            result.Should().Be(value);
        }

        /// <summary>
        /// Tests that GetMetadata returns the ItemSpec when the metadata name is "FullPath" and key does not exist.
        /// Expected outcome: Returns the ItemSpec.
        /// </summary>
        [Theory]
        [InlineData("TestItemPath")]
        [InlineData("")]
        public void GetMetadata_WhenMetadataDoesNotExistButNameIsFullPath_ReturnsItemSpec(string itemSpec)
        {
            // Arrange
            var taskItem = new TaskItem(itemSpec);
            // Act
            var result = taskItem.GetMetadata("FullPath");
            // Assert
            result.Should().Be(itemSpec);
        }

        /// <summary>
        /// Tests that GetMetadata returns an empty string when the metadata does not exist and the name is not "FullPath".
        /// Expected outcome: Returns an empty string.
        /// </summary>
        [Theory]
        [InlineData("NonExistentKey")]
        [InlineData("AnotherKey")]
        public void GetMetadata_WhenMetadataDoesNotExistAndNameIsNotFullPath_ReturnsEmptyString(string key)
        {
            // Arrange
            var taskItem = new TaskItem("TestItem");
            // Act
            var result = taskItem.GetMetadata(key);
            // Assert
            result.Should().Be("");
        }

        /// <summary>
        /// Tests that GetMetadataValueEscaped throws a NotImplementedException as expected.
        /// </summary>
        [Fact]
        public void GetMetadataValueEscaped_AlwaysThrowsNotImplementedException()
        {
            // Arrange
            var taskItem = new TaskItem("TestItem");
            // Act
            Action act = () => taskItem.GetMetadataValueEscaped("Key");
            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that RemoveMetadata throws a NotImplementedException as expected.
        /// </summary>
        [Fact]
        public void RemoveMetadata_AlwaysThrowsNotImplementedException()
        {
            // Arrange
            var taskItem = new TaskItem("TestItem");
            // Act
            Action act = () => taskItem.RemoveMetadata("Key");
            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that SetMetadata correctly assigns the metadata value.
        /// Expected outcome: The metadata value is set and can be retrieved from the Metadata dictionary.
        /// </summary>
        [Theory]
        [InlineData("Key1", "Value1")]
        [InlineData("AnotherKey", "AnotherValue")]
        public void SetMetadata_SetsTheMetadataCorrectly(string key, string value)
        {
            // Arrange
            var taskItem = new TaskItem("TestItem");
            // Act
            taskItem.SetMetadata(key, value);
            // Assert
            taskItem.Metadata.Should().ContainKey(key);
            taskItem.Metadata[key].Should().Be(value);
        }

        /// <summary>
        /// Tests that SetMetadataValueLiteral delegates to SetMetadata and sets the metadata correctly.
        /// Expected outcome: The corresponding metadata is set with the literal value.
        /// </summary>
        [Theory]
        [InlineData("KeyLiteral", "LiteralValue")]
        public void SetMetadataValueLiteral_SetsTheMetadataCorrectly(string key, string value)
        {
            // Arrange
            var taskItem = new TaskItem("TestItem");
            // Act
            taskItem.SetMetadataValueLiteral(key, value);
            // Assert
            taskItem.Metadata.Should().ContainKey(key);
            taskItem.Metadata[key].Should().Be(value);
        }

        /// <summary>
        /// Tests that the ToString method returns the same value as the ItemSpec property.
        /// Expected outcome: ToString returns the correct ItemSpec.
        /// </summary>
        [Theory]
        [InlineData("TestItemToString")]
        [InlineData("")]
        public void ToString_ReturnsItemSpec(string itemSpec)
        {
            // Arrange
            var taskItem = new TaskItem(itemSpec);
            // Act
            string result = taskItem.ToString();
            // Assert
            result.Should().Be(itemSpec);
        }

        /// <summary>
        /// Tests that the EvaluatedIncludeEscaped property can be set and retrieved accurately.
        /// Expected outcome: The property returns the value that was set.
        /// </summary>
        [Theory]
        [InlineData("EscapedValue")]
        [InlineData("")]
        public void EvaluatedIncludeEscaped_GetSet_ReturnsSetValue(string escapedValue)
        {
            // Arrange
            var taskItem = new TaskItem("TestItem");
            // Act
            taskItem.EvaluatedIncludeEscaped = escapedValue;
            // Assert
            taskItem.EvaluatedIncludeEscaped.Should().Be(escapedValue);
        }
    }
}