using System;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Build.Framework;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TaskStartedEventArgs2"/> class.
    /// </summary>
    public class TaskStartedEventArgs2Tests
    {
        private const string DefaultMessage = "Test Message";
        private const string DefaultHelpKeyword = "TestHelp";
        private const string DefaultProjectFile = "Project.csproj";
        private const string DefaultTaskFile = "TaskFile.cs";
        private const string DefaultTaskName = "TestTask";
//  // [Error] (44-22)CS1061 'TaskStartedEventArgs2' does not contain a definition for 'EventTimestamp' and no accessible extension method 'EventTimestamp' accepting a first argument of type 'TaskStartedEventArgs2' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the constructor properly initializes the base class properties with valid parameters.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithValidParameters_SetsBasePropertiesCorrectly()
//         {
//             // Arrange
//             var message = "A sample message";
//             var helpKeyword = "SampleHelp";
//             var projectFile = "/path/to/project.csproj";
//             var taskFile = "/path/to/taskfile.cs";
//             var taskName = "SampleTask";
//             var eventTimestamp = DateTime.UtcNow;
// 
//             // Act
//             var instance = new TaskStartedEventArgs2(message, helpKeyword, projectFile, taskFile, taskName, eventTimestamp);
// 
//             // Assert
//             instance.Should().NotBeNull();
//             instance.Message.Should().Be(message);
//             instance.HelpKeyword.Should().Be(helpKeyword);
//             instance.ProjectFile.Should().Be(projectFile);
//             instance.TaskFile.Should().Be(taskFile);
//             instance.TaskName.Should().Be(taskName);
//             instance.EventTimestamp.Should().Be(eventTimestamp);
//         }
// 
        /// <summary>
        /// Tests that the new properties (LineNumber, ColumnNumber, and TaskAssemblyLocation) correctly store and return assigned values.
        /// </summary>
        [Fact]
        public void Properties_GetAndSet_ReturnsAssignedValues()
        {
            // Arrange
            var instance = new TaskStartedEventArgs2(DefaultMessage, DefaultHelpKeyword, DefaultProjectFile, DefaultTaskFile, DefaultTaskName, DateTime.UtcNow);
            int expectedLineNumber = 123;
            int expectedColumnNumber = 456;
            string expectedAssemblyLocation = "C:\\Assembly\\location.dll";

            // Act
            instance.LineNumber = expectedLineNumber;
            instance.ColumnNumber = expectedColumnNumber;
            instance.TaskAssemblyLocation = expectedAssemblyLocation;

            // Assert
            instance.LineNumber.Should().Be(expectedLineNumber);
            instance.ColumnNumber.Should().Be(expectedColumnNumber);
            instance.TaskAssemblyLocation.Should().Be(expectedAssemblyLocation);
        }

        /// <summary>
        /// Tests that setting TaskAssemblyLocation to an empty string results in retrieving an empty string.
        /// </summary>
        [Fact]
        public void TaskAssemblyLocation_WhenSetToEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var instance = new TaskStartedEventArgs2(DefaultMessage, DefaultHelpKeyword, DefaultProjectFile, DefaultTaskFile, DefaultTaskName, DateTime.UtcNow);
            var emptyString = string.Empty;

            // Act
            instance.TaskAssemblyLocation = emptyString;

            // Assert
            instance.TaskAssemblyLocation.Should().Be(emptyString);
        }

        /// <summary>
        /// Tests that setting TaskAssemblyLocation to a string containing special characters returns the assigned string.
        /// </summary>
        [Fact]
        public void TaskAssemblyLocation_WhenSetToSpecialCharacterString_ReturnsExpectedString()
        {
            // Arrange
            var instance = new TaskStartedEventArgs2(DefaultMessage, DefaultHelpKeyword, DefaultProjectFile, DefaultTaskFile, DefaultTaskName, DateTime.UtcNow);
            var specialString = "C:\\Path\\äöüß_*&^%$#@!";

            // Act
            instance.TaskAssemblyLocation = specialString;

            // Assert
            instance.TaskAssemblyLocation.Should().Be(specialString);
        }
//  // [Error] (123-22)CS1061 'TaskStartedEventArgs2' does not contain a definition for 'EventTimestamp' and no accessible extension method 'EventTimestamp' accepting a first argument of type 'TaskStartedEventArgs2' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the constructor correctly sets the EventTimestamp property for boundary DateTime values.
//         /// </summary>
//         /// <param name="eventTimestamp">The boundary DateTime value to test.</param>
//         [Theory]
//         [MemberData(nameof(GetBoundaryDateTimes))]
//         public void Constructor_WithBoundaryDateTime_SetsEventTimestampCorrectly(DateTime eventTimestamp)
//         {
//             // Arrange
//             var message = "Boundary Message";
//             var helpKeyword = "BoundaryHelp";
//             var projectFile = "/boundary/project.csproj";
//             var taskFile = "/boundary/task.cs";
//             var taskName = "BoundaryTask";
// 
//             // Act
//             var instance = new TaskStartedEventArgs2(message, helpKeyword, projectFile, taskFile, taskName, eventTimestamp);
// 
//             // Assert
//             instance.EventTimestamp.Should().Be(eventTimestamp);
//         }
// 
        /// <summary>
        /// Provides boundary DateTime values for testing the constructor.
        /// </summary>
        public static System.Collections.Generic.IEnumerable<object[]> GetBoundaryDateTimes()
        {
            yield return new object[] { DateTime.MinValue };
            yield return new object[] { DateTime.MaxValue };
        }
    }
}
