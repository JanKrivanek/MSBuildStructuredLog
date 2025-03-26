using System.Collections;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TaskParameterEventArgs2"/> class.
    /// </summary>
    public class TaskParameterEventArgs2Tests
    {
        /// <summary>
        /// Tests that the constructor sets all properties correctly given valid input parameters.
        /// </summary>
        [Fact]
        public void Constructor_HappyPath_SetsPropertiesCorrectly()
        {
            // Arrange
            TaskParameterMessageKind kind = (TaskParameterMessageKind)1;
            string expectedParameterName = "TestParameter";
            string expectedPropertyName = "TestProperty";
            string itemType = "TestItemType";
            IList items = new ArrayList { "item1", "item2" };
            bool logItemMetadata = true;
            DateTime eventTimestamp = new DateTime(2023, 1, 1);

            // Act
            var instance = new TaskParameterEventArgs2(kind, expectedParameterName, expectedPropertyName, itemType, items, logItemMetadata, eventTimestamp);

            // Assert
            Assert.Equal(expectedParameterName, instance.ParameterName);
            Assert.Equal(expectedPropertyName, instance.PropertyName);
            // The new LineNumber and ColumnNumber properties should default to 0.
            Assert.Equal(0, instance.LineNumber);
            Assert.Equal(0, instance.ColumnNumber);
        }

        /// <summary>
        /// Tests that the constructor accepts a null IList for items without throwing an exception.
        /// </summary>
        [Fact]
        public void Constructor_NullItems_AllowsNullForItems()
        {
            // Arrange
            TaskParameterMessageKind kind = (TaskParameterMessageKind)0;
            string expectedParameterName = "TestParameter";
            string expectedPropertyName = "TestProperty";
            string itemType = "TestItemType";
            IList items = null;
            bool logItemMetadata = false;
            DateTime eventTimestamp = DateTime.UtcNow;

            // Act & Assert
            var exception = Record.Exception(() => new TaskParameterEventArgs2(kind, expectedParameterName, expectedPropertyName, itemType, items, logItemMetadata, eventTimestamp));
            Assert.Null(exception);
        }

        /// <summary>
        /// Tests that the ParameterName property getter and setter work correctly.
        /// </summary>
        [Fact]
        public void ParameterName_PropertySetterGetter_WorksCorrectly()
        {
            // Arrange
            TaskParameterMessageKind kind = (TaskParameterMessageKind)2;
            string initialParameterName = "InitialParameter";
            string propertyName = "PropertyName";
            string itemType = "ItemType";
            IList items = new ArrayList();
            bool logItemMetadata = false;
            DateTime eventTimestamp = DateTime.Now;

            var instance = new TaskParameterEventArgs2(kind, initialParameterName, propertyName, itemType, items, logItemMetadata, eventTimestamp);
            string newParameterName = "UpdatedParameter";

            // Act
            instance.ParameterName = newParameterName;

            // Assert
            Assert.Equal(newParameterName, instance.ParameterName);
        }

        /// <summary>
        /// Tests that the PropertyName property getter and setter work correctly.
        /// </summary>
        [Fact]
        public void PropertyName_PropertySetterGetter_WorksCorrectly()
        {
            // Arrange
            TaskParameterMessageKind kind = (TaskParameterMessageKind)3;
            string parameterName = "ParameterName";
            string initialPropertyName = "InitialProperty";
            string itemType = "ItemType";
            IList items = new ArrayList();
            bool logItemMetadata = true;
            DateTime eventTimestamp = DateTime.Now;

            var instance = new TaskParameterEventArgs2(kind, parameterName, initialPropertyName, itemType, items, logItemMetadata, eventTimestamp);
            string newPropertyName = "UpdatedProperty";

            // Act
            instance.PropertyName = newPropertyName;

            // Assert
            Assert.Equal(newPropertyName, instance.PropertyName);
        }

        /// <summary>
        /// Tests that the LineNumber and ColumnNumber properties default to 0 and can be updated correctly.
        /// </summary>
        [Fact]
        public void LineAndColumnNumber_DefaultAndSetter_WorksCorrectly()
        {
            // Arrange
            TaskParameterMessageKind kind = (TaskParameterMessageKind)4;
            string parameterName = "Parameter";
            string propertyName = "Property";
            string itemType = "ItemType";
            IList items = new ArrayList();
            bool logItemMetadata = false;
            DateTime eventTimestamp = DateTime.Now;

            var instance = new TaskParameterEventArgs2(kind, parameterName, propertyName, itemType, items, logItemMetadata, eventTimestamp);
            int expectedLineNumber = 42;
            int expectedColumnNumber = 24;

            // Act
            instance.LineNumber = expectedLineNumber;
            instance.ColumnNumber = expectedColumnNumber;

            // Assert
            Assert.Equal(expectedLineNumber, instance.LineNumber);
            Assert.Equal(expectedColumnNumber, instance.ColumnNumber);
        }
    }
}
