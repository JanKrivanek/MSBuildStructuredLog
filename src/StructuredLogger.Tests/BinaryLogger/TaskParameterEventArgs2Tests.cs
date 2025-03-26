using System;
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
        /// Tests that the constructor correctly assigns the ParameterName and PropertyName properties.
        /// Also verifies that the LineNumber and ColumnNumber default to zero.
        /// </summary>
//         [Fact] [Error] (21-70)CS0117 'TaskParameterMessageKind' does not contain a definition for 'Error'
//         public void Constructor_WithValidInputs_SetsPropertiesCorrectly()
//         {
//             // Arrange
//             TaskParameterMessageKind kind = TaskParameterMessageKind.Error;
//             string testParameterName = "TestParameter";
//             string testPropertyName = "TestProperty";
//             string testItemType = "TestType";
//             IList testItems = new ArrayList();
//             bool logItemMetadata = true;
//             DateTime eventTimestamp = DateTime.Now;
// 
//             // Act
//             var eventArgs = new TaskParameterEventArgs2(kind, testParameterName, testPropertyName, testItemType, testItems, logItemMetadata, eventTimestamp);
// 
//             // Assert
//             Assert.Equal(testParameterName, eventArgs.ParameterName);
//             Assert.Equal(testPropertyName, eventArgs.PropertyName);
//             // Verify that the new properties have their default values.
//             Assert.Equal(0, eventArgs.LineNumber);
//             Assert.Equal(0, eventArgs.ColumnNumber);
//         }

        /// <summary>
        /// Tests that the constructor accepts null values for ParameterName and PropertyName without throwing exceptions.
        /// </summary>
//         [Fact] [Error] (47-70)CS0117 'TaskParameterMessageKind' does not contain a definition for 'Warning'
//         public void Constructor_WithNullNames_AllowsNullValues()
//         {
//             // Arrange
//             TaskParameterMessageKind kind = TaskParameterMessageKind.Warning;
//             string testParameterName = null;
//             string testPropertyName = null;
//             string testItemType = "TestType";
//             IList testItems = new ArrayList();
//             bool logItemMetadata = false;
//             DateTime eventTimestamp = DateTime.UtcNow;
// 
//             // Act
//             var eventArgs = new TaskParameterEventArgs2(kind, testParameterName, testPropertyName, testItemType, testItems, logItemMetadata, eventTimestamp);
// 
//             // Assert
//             Assert.Null(eventArgs.ParameterName);
//             Assert.Null(eventArgs.PropertyName);
//         }

        /// <summary>
        /// Tests that the LineNumber and ColumnNumber properties can be set and retrieved correctly.
        /// </summary>
//         [Fact] [Error] (70-70)CS0117 'TaskParameterMessageKind' does not contain a definition for 'Info'
//         public void LineNumberAndColumnNumber_SetValues_ReturnsExpectedValues()
//         {
//             // Arrange
//             TaskParameterMessageKind kind = TaskParameterMessageKind.Info;
//             string testParameterName = "Param";
//             string testPropertyName = "Prop";
//             string testItemType = "Type";
//             IList testItems = new ArrayList();
//             bool logItemMetadata = true;
//             DateTime eventTimestamp = DateTime.Now;
//             var eventArgs = new TaskParameterEventArgs2(kind, testParameterName, testPropertyName, testItemType, testItems, logItemMetadata, eventTimestamp);
//             int expectedLineNumber = 42;
//             int expectedColumnNumber = 84;
// 
//             // Act
//             eventArgs.LineNumber = expectedLineNumber;
//             eventArgs.ColumnNumber = expectedColumnNumber;
// 
//             // Assert
//             Assert.Equal(expectedLineNumber, eventArgs.LineNumber);
//             Assert.Equal(expectedColumnNumber, eventArgs.ColumnNumber);
//         }

        /// <summary>
        /// Tests that the ParameterName and PropertyName properties are mutable.
        /// This verifies that setting a new value is correctly reflected by the getters.
        /// </summary>
//         [Fact] [Error] (98-70)CS0117 'TaskParameterMessageKind' does not contain a definition for 'Info'
//         public void ParameterAndPropertyNames_SetNewValues_ReturnsUpdatedValues()
//         {
//             // Arrange
//             TaskParameterMessageKind kind = TaskParameterMessageKind.Info;
//             string initialParameterName = "InitialParam";
//             string initialPropertyName = "InitialProp";
//             string testItemType = "Type";
//             IList testItems = new ArrayList();
//             bool logItemMetadata = false;
//             DateTime eventTimestamp = DateTime.Now;
//             var eventArgs = new TaskParameterEventArgs2(kind, initialParameterName, initialPropertyName, testItemType, testItems, logItemMetadata, eventTimestamp);
//             string updatedParameterName = "UpdatedParam";
//             string updatedPropertyName = "UpdatedProp";
// 
//             // Act
//             eventArgs.ParameterName = updatedParameterName;
//             eventArgs.PropertyName = updatedPropertyName;
// 
//             // Assert
//             Assert.Equal(updatedParameterName, eventArgs.ParameterName);
//             Assert.Equal(updatedPropertyName, eventArgs.PropertyName);
//         }
    }
}
