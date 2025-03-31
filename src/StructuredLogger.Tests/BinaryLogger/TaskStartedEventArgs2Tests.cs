using System;
using FluentAssertions;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TaskStartedEventArgs2"/> class.
    /// </summary>
    public class TaskStartedEventArgs2Tests
    {
        private readonly string _message;
        private readonly string _helpKeyword;
        private readonly string _projectFile;
        private readonly string _taskFile;
        private readonly string _taskName;
        private readonly DateTime _eventTimestamp;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskStartedEventArgs2Tests"/> class with predefined test data.
        /// </summary>
        public TaskStartedEventArgs2Tests()
        {
            _message = "Test Message";
            _helpKeyword = "TestHelpKeyword";
            _projectFile = "TestProject.csproj";
            _taskFile = "TestTaskFile.task";
            _taskName = "TestTask";
            _eventTimestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// Tests that the constructor of TaskStartedEventArgs2 correctly initializes the inherited base properties.
        /// This includes validating that the properties Message, HelpKeyword, ProjectFile, TaskFile, TaskName, and Timestamp
        /// are set to the values passed to the constructor.
        /// </summary>
        [Fact]
        public void Constructor_WithValidParameters_ShouldInitializeBaseProperties()
        {
            // Arrange & Act
            var taskEvent = new TaskStartedEventArgs2(_message, _helpKeyword, _projectFile, _taskFile, _taskName, _eventTimestamp);

            // Assert
            taskEvent.Should().NotBeNull("the object should be created successfully");
            taskEvent.Message.Should().Be(_message, "the Message property should be initialized with the constructor parameter");
            taskEvent.HelpKeyword.Should().Be(_helpKeyword, "the HelpKeyword property should be initialized with the constructor parameter");
            taskEvent.ProjectFile.Should().Be(_projectFile, "the ProjectFile property should be initialized with the constructor parameter");
            taskEvent.TaskFile.Should().Be(_taskFile, "the TaskFile property should be initialized with the constructor parameter");
            taskEvent.TaskName.Should().Be(_taskName, "the TaskName property should be initialized with the constructor parameter");
            taskEvent.Timestamp.Should().Be(_eventTimestamp, "the Timestamp property should be initialized with the constructor parameter");
        }

        /// <summary>
        /// Tests that the new LineNumber property can be assigned and returns the assigned value.
        /// </summary>
        /// <param name="lineNumber">An integer value representing the line number, including edge cases.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(-1)]
        public void LineNumber_SetValidValue_ReturnsExpectedValue(int lineNumber)
        {
            // Arrange
            var taskEvent = new TaskStartedEventArgs2(_message, _helpKeyword, _projectFile, _taskFile, _taskName, _eventTimestamp);

            // Act
            taskEvent.LineNumber = lineNumber;

            // Assert
            taskEvent.LineNumber.Should().Be(lineNumber, "the LineNumber property should return the value that was set");
        }

        /// <summary>
        /// Tests that the new ColumnNumber property can be assigned and returns the assigned value.
        /// </summary>
        /// <param name="columnNumber">An integer value representing the column number, including edge cases.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(50)]
        [InlineData(-10)]
        public void ColumnNumber_SetValidValue_ReturnsExpectedValue(int columnNumber)
        {
            // Arrange
            var taskEvent = new TaskStartedEventArgs2(_message, _helpKeyword, _projectFile, _taskFile, _taskName, _eventTimestamp);

            // Act
            taskEvent.ColumnNumber = columnNumber;

            // Assert
            taskEvent.ColumnNumber.Should().Be(columnNumber, "the ColumnNumber property should return the value that was set");
        }

        /// <summary>
        /// Tests that the TaskAssemblyLocation property can be assigned and returns the assigned string value.
        /// </summary>
        /// <param name="assemblyLocation">A string representing the location of the task assembly, including empty string scenarios.</param>
        [Theory]
        [InlineData("Assembly.dll")]
        [InlineData("")]
        public void TaskAssemblyLocation_SetValidValue_ReturnsExpectedValue(string assemblyLocation)
        {
            // Arrange
            var taskEvent = new TaskStartedEventArgs2(_message, _helpKeyword, _projectFile, _taskFile, _taskName, _eventTimestamp);

            // Act
            taskEvent.TaskAssemblyLocation = assemblyLocation;

            // Assert
            taskEvent.TaskAssemblyLocation.Should().Be(assemblyLocation, "the TaskAssemblyLocation property should return the value that was set");
        }
    }
}
