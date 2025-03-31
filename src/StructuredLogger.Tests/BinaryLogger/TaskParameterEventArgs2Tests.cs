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
        /// Tests that the constructor of TaskParameterEventArgs2 sets its properties correctly.
        /// Arrange: Create test values for all constructor parameters.
        /// Act: Construct a new instance of TaskParameterEventArgs2.
        /// Assert: Verify that ParameterName, PropertyName, and new properties have the expected default values.
        /// </summary>
        [Fact]
        public void Constructor_ValidInputs_SetsPropertiesCorrectly()
        {
            // Arrange
            TaskParameterMessageKind kind = (TaskParameterMessageKind)0;
            string parameterName = "TestParameter";
            string propertyName = "TestProperty";
            string itemType = "TestItemType";
            IList items = new ArrayList { "Item1", "Item2" };
            bool logItemMetadata = true;
            DateTime eventTimestamp = DateTime.Now;
        
            // Act
            var instance = new TaskParameterEventArgs2(kind, parameterName, propertyName, itemType, items, logItemMetadata, eventTimestamp);
        
            // Assert
            instance.ParameterName.Should().Be(parameterName);
            instance.PropertyName.Should().Be(propertyName);
            instance.LineNumber.Should().Be(0);
            instance.ColumnNumber.Should().Be(0);
        }
        
        /// <summary>
        /// Tests that the properties of TaskParameterEventArgs2 can be updated and retrieved correctly.
        /// Arrange: Create an instance and new values to set.
        /// Act: Set new values to ParameterName, PropertyName, LineNumber, and ColumnNumber.
        /// Assert: Verify that the updated properties return the new values.
        /// </summary>
        [Fact]
        public void PropertySet_Get_UpdatesPropertiesCorrectly()
        {
            // Arrange
            TaskParameterMessageKind kind = (TaskParameterMessageKind)0;
            string parameterName = "InitialParameter";
            string propertyName = "InitialProperty";
            string itemType = "InitialItemType";
            IList items = new ArrayList { "InitialItem" };
            bool logItemMetadata = false;
            DateTime eventTimestamp = DateTime.UtcNow;
            
            var instance = new TaskParameterEventArgs2(kind, parameterName, propertyName, itemType, items, logItemMetadata, eventTimestamp);
            
            string newParameterName = "UpdatedParameter";
            string newPropertyName = "UpdatedProperty";
            int newLineNumber = 42;
            int newColumnNumber = 84;
        
            // Act
            instance.ParameterName = newParameterName;
            instance.PropertyName = newPropertyName;
            instance.LineNumber = newLineNumber;
            instance.ColumnNumber = newColumnNumber;
        
            // Assert
            instance.ParameterName.Should().Be(newParameterName);
            instance.PropertyName.Should().Be(newPropertyName);
            instance.LineNumber.Should().Be(newLineNumber);
            instance.ColumnNumber.Should().Be(newColumnNumber);
        }
        
        /// <summary>
        /// Tests that the constructor accepts null values for string parameters.
        /// Arrange: Provide null for ParameterName and PropertyName.
        /// Act: Create an instance with null string values.
        /// Assert: Verify that ParameterName and PropertyName properties are null.
        /// </summary>
        [Fact]
        public void Constructor_WithNullStringParameters_AllowsNulls()
        {
            // Arrange
            TaskParameterMessageKind kind = (TaskParameterMessageKind)0;
            string parameterName = null!;
            string propertyName = null!;
            string itemType = "TestItemType";
            IList items = new ArrayList();
            bool logItemMetadata = false;
            DateTime eventTimestamp = DateTime.Now;
            
            // Act
            var instance = new TaskParameterEventArgs2(kind, parameterName, propertyName, itemType, items, logItemMetadata, eventTimestamp);
            
            // Assert
            instance.ParameterName.Should().BeNull();
            instance.PropertyName.Should().BeNull();
        }
    }
}
