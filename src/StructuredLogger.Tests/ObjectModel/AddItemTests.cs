using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="AddItem"/> class.
    /// </summary>
    public class AddItemTests
    {
        private readonly AddItem _addItem;

        public AddItemTests()
        {
            _addItem = new AddItem();
        }

        /// <summary>
        /// Tests that the constructor initializes the <see cref="AddItem.TypeName"/> property correctly.
        /// Expected outcome is that the TypeName property returns "AddItem".
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_InitializesTypeNameCorrectly()
        {
            // Act
            string typeName = _addItem.TypeName;

            // Assert
            Assert.Equal("AddItem", typeName);
        }

        /// <summary>
        /// Tests that the default value of the <see cref="AddItem.LineNumber"/> property is null.
        /// Expected outcome is that a newly constructed instance has a null LineNumber.
        /// </summary>
        [Fact]
        public void LineNumber_DefaultValue_IsNull()
        {
            // Act
            int? lineNumber = _addItem.LineNumber;

            // Assert
            Assert.Null(lineNumber);
        }

        /// <summary>
        /// Tests that the <see cref="AddItem.LineNumber"/> property correctly stores and retrieves an integer value.
        /// Expected outcome is the same value is returned after setting it.
        /// </summary>
        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        public void LineNumber_SetValue_ReturnsSameValue(int expectedLineNumber)
        {
            // Arrange
            _addItem.LineNumber = expectedLineNumber;

            // Act
            int? actualLineNumber = _addItem.LineNumber;

            // Assert
            Assert.Equal(expectedLineNumber, actualLineNumber);
        }

        /// <summary>
        /// Tests that the <see cref="AddItem.LineNumber"/> property can be reset to null after being set.
        /// Expected outcome is that after setting to null, the property returns null.
        /// </summary>
        [Fact]
        public void LineNumber_SetToNull_ReturnsNull()
        {
            // Arrange
            _addItem.LineNumber = 10;

            // Act
            _addItem.LineNumber = null;

            // Assert
            Assert.Null(_addItem.LineNumber);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="TaskParameterItem"/> class.
    /// </summary>
    public class TaskParameterItemTests
    {
        private readonly TaskParameterItem _taskParameterItem;

        public TaskParameterItemTests()
        {
            _taskParameterItem = new TaskParameterItem();
        }

        /// <summary>
        /// Tests that the inherited <see cref="AddItem.TypeName"/> property in <see cref="TaskParameterItem"/> returns "AddItem".
        /// Expected outcome is the TypeName property remains "AddItem" even in the derived type.
        /// </summary>
        [Fact]
        public void TypeName_InheritedProperty_ReturnsAddItem()
        {
            // Act
            string typeName = _taskParameterItem.TypeName;

            // Assert
            Assert.Equal("AddItem", typeName);
        }

        /// <summary>
        /// Tests that the <see cref="TaskParameterItem.ParameterName"/> property correctly stores and retrieves a string value.
        /// Expected outcome is that after setting a valid string, the same string is returned.
        /// </summary>
        [Theory]
        [InlineData("Parameter1")]
        [InlineData("")]
        [InlineData(null)]
        public void ParameterName_SetAndGet_WorksAsExpected(string expectedParameterName)
        {
            // Act
            _taskParameterItem.ParameterName = expectedParameterName;
            string actualParameterName = _taskParameterItem.ParameterName;

            // Assert
            Assert.Equal(expectedParameterName, actualParameterName);
        }
    }
}
