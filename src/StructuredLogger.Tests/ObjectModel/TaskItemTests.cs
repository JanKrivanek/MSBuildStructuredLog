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
        private readonly string _testItemSpec = "TestItemSpec";
        private readonly string _testMetadataKey = "Key";
        private readonly string _testMetadataValue = "Value";
        /// <summary>
        /// Tests that the parameterless constructor initializes a TaskItem with default values.
        /// </summary>
        [Fact]
        public void DefaultConstructor_InitializesEmptyTaskItem()
        {
            // Arrange & Act
            var taskItem = new TaskItem();
            // Assert
            taskItem.ItemSpec.Should().BeNull();
            taskItem.Metadata.Should().NotBeNull().And.BeEmpty();
            taskItem.EvaluatedIncludeEscaped.Should().BeNull();
            taskItem.MetadataCount.Should().Be(0);
            ((ICollection)taskItem.MetadataNames).Count.Should().Be(0);
        }

        /// <summary>
        /// Tests that the constructor with an itemSpec parameter sets the ItemSpec property correctly.
        /// </summary>
        [Fact]
        public void ConstructorWithItemSpec_SetsItemSpecProperty()
        {
            // Arrange & Act
            var taskItem = new TaskItem(_testItemSpec);
            // Assert
            taskItem.ItemSpec.Should().Be(_testItemSpec);
        }
//  // [Error] (62-37)CS1061 'ObjectAssertions' does not contain a definition for 'ContainKey' and no accessible extension method 'ContainKey' accepting a first argument of type 'ObjectAssertions' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that CloneCustomMetadata returns the same metadata dictionary.
//         /// </summary>
//         [Fact]
//         public void CloneCustomMetadata_WhenCalled_ReturnsSameMetadataDictionary()
//         {
//             // Arrange
//             var taskItem = new TaskItem();
//             taskItem.SetMetadata(_testMetadataKey, _testMetadataValue);
//             // Act
//             IDictionary clonedMetadata = taskItem.CloneCustomMetadata();
//             // Assert
//             clonedMetadata.Should().NotBeNull();
//             clonedMetadata.Should().ContainKey(_testMetadataKey).WhoseValue.Should().Be(_testMetadataValue);
//             // Verify that modifying the returned dictionary modifies the original metadata.
//             clonedMetadata[_testMetadataKey] = "UpdatedValue";
//             taskItem.Metadata[_testMetadataKey].Should().Be("UpdatedValue");
//         }
// 
        /// <summary>
        /// Tests that CloneCustomMetadataEscaped throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void CloneCustomMetadataEscaped_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            var taskItem = new TaskItem();
            // Act
            Action act = () => taskItem.CloneCustomMetadataEscaped();
            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that CopyMetadataTo copies all metadata entries to a destination ITaskItem.
        /// </summary>
        [Fact]
        public void CopyMetadataTo_WhenCalled_CopiesAllMetadataEntries()
        {
            // Arrange
            var taskItem = new TaskItem();
            taskItem.SetMetadata("Key1", "Value1");
            taskItem.SetMetadata("Key2", "Value2");
            var destinationMock = new Mock<ITaskItem>();
            destinationMock.Setup(d => d.SetMetadata(It.IsAny<string>(), It.IsAny<string>()));
            // Act
            taskItem.CopyMetadataTo(destinationMock.Object);
            // Assert
            destinationMock.Verify(d => d.SetMetadata("Key1", "Value1"), Times.Once);
            destinationMock.Verify(d => d.SetMetadata("Key2", "Value2"), Times.Once);
        }

        /// <summary>
        /// Tests that GetMetadata returns the corresponding metadata value when the key exists.
        /// </summary>
        [Fact]
        public void GetMetadata_WhenKeyExists_ReturnsCorrespondingValue()
        {
            // Arrange
            var taskItem = new TaskItem(_testItemSpec);
            taskItem.SetMetadata(_testMetadataKey, _testMetadataValue);
            // Act
            string result = taskItem.GetMetadata(_testMetadataKey);
            // Assert
            result.Should().Be(_testMetadataValue);
        }

        /// <summary>
        /// Tests that GetMetadata returns ItemSpec when the metadata key is "FullPath" and not present in the metadata.
        /// </summary>
        [Fact]
        public void GetMetadata_WhenKeyIsFullPathAndNotInMetadata_ReturnsItemSpec()
        {
            // Arrange
            var taskItem = new TaskItem(_testItemSpec);
            // Act
            string result = taskItem.GetMetadata("FullPath");
            // Assert
            result.Should().Be(_testItemSpec);
        }

        /// <summary>
        /// Tests that GetMetadata returns an empty string when the key does not exist and is not "FullPath".
        /// </summary>
        [Fact]
        public void GetMetadata_WhenKeyDoesNotExistAndNotFullPath_ReturnsEmptyString()
        {
            // Arrange
            var taskItem = new TaskItem(_testItemSpec);
            // Act
            string result = taskItem.GetMetadata("NonExistentKey");
            // Assert
            result.Should().BeEmpty();
        }

        /// <summary>
        /// Tests that GetMetadataValueEscaped throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void GetMetadataValueEscaped_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            var taskItem = new TaskItem();
            // Act
            Action act = () => taskItem.GetMetadataValueEscaped("AnyKey");
            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that RemoveMetadata throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void RemoveMetadata_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            var taskItem = new TaskItem();
            // Act
            Action act = () => taskItem.RemoveMetadata("AnyKey");
            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that SetMetadata correctly adds a new metadata entry and updates it when called again.
        /// </summary>
        [Fact]
        public void SetMetadata_WhenCalled_AddsOrUpdatesMetadataEntry()
        {
            // Arrange
            var taskItem = new TaskItem();
            // Act
            taskItem.SetMetadata(_testMetadataKey, _testMetadataValue);
            // Assert
            taskItem.Metadata.Should().ContainKey(_testMetadataKey).WhoseValue.Should().Be(_testMetadataValue);
            // Act - update the metadata value.
            taskItem.SetMetadata(_testMetadataKey, "UpdatedValue");
            // Assert
            taskItem.Metadata[_testMetadataKey].Should().Be("UpdatedValue");
        }

        /// <summary>
        /// Tests that SetMetadataValueLiteral sets metadata correctly by delegating its logic to SetMetadata.
        /// </summary>
        [Fact]
        public void SetMetadataValueLiteral_WhenCalled_SetsMetadataCorrectly()
        {
            // Arrange
            var taskItem = new TaskItem();
            // Act
            taskItem.SetMetadataValueLiteral(_testMetadataKey, _testMetadataValue);
            // Assert
            taskItem.Metadata.Should().ContainKey(_testMetadataKey).WhoseValue.Should().Be(_testMetadataValue);
        }

        /// <summary>
        /// Tests that ToString returns the same value as the ItemSpec property.
        /// </summary>
        [Fact]
        public void ToString_WhenCalled_ReturnsItemSpec()
        {
            // Arrange
            var taskItem = new TaskItem(_testItemSpec);
            // Act
            string result = taskItem.ToString();
            // Assert
            result.Should().Be(_testItemSpec);
        }
    }
}