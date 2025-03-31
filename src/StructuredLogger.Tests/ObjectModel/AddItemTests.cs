using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Reflection;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="AddItem"/> class.
    /// </summary>
    public class AddItemTests
    {
        /// <summary>
        /// Tests that the default constructor initializes DisableChildrenCache to true.
        /// This test uses reflection to check the value of the DisableChildrenCache property,
        /// which is set in the constructor of AddItem.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_InitializesDisableChildrenCacheToTrue()
        {
            // Arrange & Act
            var addItem = new AddItem();
            
            // Assert
            // Try to get the property using public binding.
            PropertyInfo? propertyInfo = addItem.GetType().GetProperty("DisableChildrenCache", BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo != null)
            {
                bool value = (bool)propertyInfo.GetValue(addItem)!;
                value.Should().BeTrue("DisableChildrenCache should be initialized to true in the constructor.");
            }
            else
            {
                // If not found publicly, check non-public property.
                propertyInfo = addItem.GetType().GetProperty("DisableChildrenCache", BindingFlags.Instance | BindingFlags.NonPublic);
                propertyInfo.Should().NotBeNull("DisableChildrenCache property should exist.");
                bool value = (bool)propertyInfo!.GetValue(addItem)!;
                value.Should().BeTrue("DisableChildrenCache should be initialized to true in the constructor.");
            }
        }

        /// <summary>
        /// Tests that the TypeName property returns "AddItem".
        /// This confirms that the property returns the correct class name.
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsAddItem()
        {
            // Arrange
            var addItem = new AddItem();

            // Act
            string typeName = addItem.TypeName;

            // Assert
            typeName.Should().Be("AddItem", "TypeName should return the name of the class i.e., 'AddItem'.");
        }

        /// <summary>
        /// Tests that the LineNumber property gets and sets values correctly, including null.
        /// This ensures that the property correctly handles assignment and retrieval of its value.
        /// </summary>
        [Fact]
        public void LineNumber_GetSet_WorksAsExpected()
        {
            // Arrange
            var addItem = new AddItem();

            // Act & Assert
            addItem.LineNumber.Should().BeNull("LineNumber should be null by default.");

            addItem.LineNumber = 15;
            addItem.LineNumber.Should().Be(15, "LineNumber should retain the set integer value.");

            addItem.LineNumber = null;
            addItem.LineNumber.Should().BeNull("LineNumber should be able to be set back to null.");
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="TaskParameterItem"/> class.
    /// </summary>
    public class TaskParameterItemTests
    {
        /// <summary>
        /// Tests that the default constructor of TaskParameterItem correctly inherits properties from AddItem.
        /// This includes the TypeName and the initialization of DisableChildrenCache.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_InheritsAddItemProperties()
        {
            // Arrange & Act
            var taskParameterItem = new TaskParameterItem();

            // Assert
            taskParameterItem.TypeName.Should().Be("AddItem", "TaskParameterItem inherits TypeName from AddItem.");
            taskParameterItem.LineNumber.Should().BeNull("LineNumber should be null by default.");

            // Verify DisableChildrenCache using reflection.
            PropertyInfo? propertyInfo = taskParameterItem.GetType().GetProperty("DisableChildrenCache", BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo != null)
            {
                bool value = (bool)propertyInfo.GetValue(taskParameterItem)!;
                value.Should().BeTrue("DisableChildrenCache should be initialized to true in the constructor.");
            }
            else
            {
                propertyInfo = taskParameterItem.GetType().GetProperty("DisableChildrenCache", BindingFlags.Instance | BindingFlags.NonPublic);
                propertyInfo.Should().NotBeNull("DisableChildrenCache property should exist.");
                bool value = (bool)propertyInfo!.GetValue(taskParameterItem)!;
                value.Should().BeTrue("DisableChildrenCache should be initialized to true in the constructor.");
            }
        }

        /// <summary>
        /// Tests that the ParameterName property gets and sets values correctly.
        /// This verifies that a value assigned to ParameterName is correctly retained.
        /// </summary>
        [Fact]
        public void ParameterName_GetSet_WorksAsExpected()
        {
            // Arrange
            var taskParameterItem = new TaskParameterItem();
            const string expectedParameterName = "TestParameter";

            // Act
            taskParameterItem.ParameterName = expectedParameterName;

            // Assert
            taskParameterItem.ParameterName.Should().Be(expectedParameterName, "ParameterName should retain its set value.");
        }
    }
}
