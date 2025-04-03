using FluentAssertions;
using Microsoft.Build.Framework;
using System;
using System.Collections;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TaskParameterEventArgs2"/> class.
    /// </summary>
    public class TaskParameterEventArgs2Tests
    {
        /// <summary>
        /// Tests that the constructor correctly assigns the provided parameter values to the properties.
        /// Expected outcome: ParameterName and PropertyName are set as supplied and the new LineNumber and ColumnNumber are initialized to their default values (0).
        /// </summary>
        [Fact]
        public void Constructor_WithValidParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            TaskParameterMessageKind kind = (TaskParameterMessageKind)1;
            string parameterName = "ParameterTest";
            string propertyName = "PropertyTest";
            string itemType = "ItemTest";
            IList items = new ArrayList { "item1", "item2" };
            bool logItemMetadata = true;
            DateTime eventTimestamp = new DateTime(2023, 10, 1);

            // Act
            var evt = new TaskParameterEventArgs2(kind, parameterName, propertyName, itemType, items, logItemMetadata, eventTimestamp);

            // Assert
            evt.ParameterName.Should().Be(parameterName);
            evt.PropertyName.Should().Be(propertyName);
            evt.LineNumber.Should().Be(0);
            evt.ColumnNumber.Should().Be(0);
        }

        /// <summary>
        /// Tests that the ParameterName property can be set and retrieved correctly.
        /// Functional steps: Assign various string values (normal, empty, special characters) to ParameterName; verify that the getter returns the assigned value.
        /// Expected outcome: The getter should return the same string that was assigned.
        /// </summary>
        /// <param name="testValue">The string value to assign to ParameterName.</param>
        [Theory]
        [InlineData("TestParameter")]
        [InlineData("")]
        [InlineData("!@#$%^&*()")]
        public void ParameterName_SetAndGet_ReturnsAssignedValue(string testValue)
        {
            // Arrange
            var evt = CreateDefaultTaskParameterEventArgs2();

            // Act
            evt.ParameterName = testValue;

            // Assert
            evt.ParameterName.Should().Be(testValue);
        }

        /// <summary>
        /// Tests that the PropertyName property can be set and retrieved correctly.
        /// Functional steps: Assign various string values (normal, empty, special characters) to PropertyName; verify that the getter returns the assigned value.
        /// Expected outcome: The getter should return the same string that was assigned.
        /// </summary>
        /// <param name="testValue">The string value to assign to PropertyName.</param>
        [Theory]
        [InlineData("TestProperty")]
        [InlineData("")]
        [InlineData("!@#$%^&*()")]
        public void PropertyName_SetAndGet_ReturnsAssignedValue(string testValue)
        {
            // Arrange
            var evt = CreateDefaultTaskParameterEventArgs2();

            // Act
            evt.PropertyName = testValue;

            // Assert
            evt.PropertyName.Should().Be(testValue);
        }

        /// <summary>
        /// Tests that the LineNumber property can be set and retrieved correctly, including boundary values.
        /// Functional steps: Assign different integer values including 0, int.MinValue, and int.MaxValue; verify that the getter returns the same value.
        /// Expected outcome: The LineNumber property should return the value that was assigned.
        /// </summary>
        /// <param name="testValue">The integer value to assign to LineNumber.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void LineNumber_SetAndGet_ReturnsAssignedValue(int testValue)
        {
            // Arrange
            var evt = CreateDefaultTaskParameterEventArgs2();

            // Act
            evt.LineNumber = testValue;

            // Assert
            evt.LineNumber.Should().Be(testValue);
        }

        /// <summary>
        /// Tests that the ColumnNumber property can be set and retrieved correctly, including boundary values.
        /// Functional steps: Assign different integer values including 0, int.MinValue, and int.MaxValue; verify that the getter returns the same value.
        /// Expected outcome: The ColumnNumber property should return the value that was assigned.
        /// </summary>
        /// <param name="testValue">The integer value to assign to ColumnNumber.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void ColumnNumber_SetAndGet_ReturnsAssignedValue(int testValue)
        {
            // Arrange
            var evt = CreateDefaultTaskParameterEventArgs2();

            // Act
            evt.ColumnNumber = testValue;

            // Assert
            evt.ColumnNumber.Should().Be(testValue);
        }

        /// <summary>
        /// Helper method that creates a default instance of TaskParameterEventArgs2 with sample valid parameters.
        /// </summary>
        /// <returns>A default instance of TaskParameterEventArgs2.</returns>
        private static TaskParameterEventArgs2 CreateDefaultTaskParameterEventArgs2()
        {
            TaskParameterMessageKind kind = (TaskParameterMessageKind)1;
            string parameterName = "DefaultParameter";
            string propertyName = "DefaultProperty";
            string itemType = "DefaultItem";
            IList items = new ArrayList();
            bool logItemMetadata = false;
            DateTime eventTimestamp = DateTime.UtcNow;
            return new TaskParameterEventArgs2(kind, parameterName, propertyName, itemType, items, logItemMetadata, eventTimestamp);
        }
    }
}
