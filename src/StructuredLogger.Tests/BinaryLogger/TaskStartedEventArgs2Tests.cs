using Microsoft.Build.Framework;
using System;
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
        /// Initializes common test data.
        /// </summary>
        public TaskStartedEventArgs2Tests()
        {
            _message = "Test message";
            _helpKeyword = "TestHelpKeyword";
            _projectFile = "TestProject.csproj";
            _taskFile = "TestTask.dll";
            _taskName = "TestTask";
            _eventTimestamp = DateTime.Now;
        }

        /// <summary>
        /// Verifies that the constructor successfully creates an instance with valid parameters.
        /// </summary>
        [Fact]
        public void Constructor_WithValidParameters_CreatesInstance()
        {
            // Arrange & Act
            var instance = new TaskStartedEventArgs2(_message, _helpKeyword, _projectFile, _taskFile, _taskName, _eventTimestamp);

            // Assert
            Assert.NotNull(instance);
        }

        /// <summary>
        /// Verifies that the constructor does not throw an exception when null parameters are provided.
        /// </summary>
        [Fact]
        public void Constructor_WithNullParameters_DoesNotThrow()
        {
            // Arrange
            string nullMessage = null;
            string nullHelpKeyword = null;
            string nullProjectFile = null;
            string nullTaskFile = null;
            string nullTaskName = null;

            // Act
            var exception = Record.Exception(() => 
                new TaskStartedEventArgs2(nullMessage, nullHelpKeyword, nullProjectFile, nullTaskFile, nullTaskName, _eventTimestamp));

            // Assert
            Assert.Null(exception);
        }

        /// <summary>
        /// Verifies that the LineNumber property can be set and retrieved correctly.
        /// </summary>
        /// <param name="value">An integer value representing the line number.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void LineNumber_SetAndGet_ReturnsSetValue(int value)
        {
            // Arrange
            var instance = new TaskStartedEventArgs2(_message, _helpKeyword, _projectFile, _taskFile, _taskName, _eventTimestamp);

            // Act
            instance.LineNumber = value;

            // Assert
            Assert.Equal(value, instance.LineNumber);
        }

        /// <summary>
        /// Verifies that the ColumnNumber property can be set and retrieved correctly.
        /// </summary>
        /// <param name="value">An integer value representing the column number.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void ColumnNumber_SetAndGet_ReturnsSetValue(int value)
        {
            // Arrange
            var instance = new TaskStartedEventArgs2(_message, _helpKeyword, _projectFile, _taskFile, _taskName, _eventTimestamp);

            // Act
            instance.ColumnNumber = value;

            // Assert
            Assert.Equal(value, instance.ColumnNumber);
        }

        /// <summary>
        /// Verifies that the TaskAssemblyLocation property can be set and retrieved correctly.
        /// </summary>
        /// <param name="value">A string value representing the task assembly location.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("C:\\MyTask.dll")]
        public void TaskAssemblyLocation_SetAndGet_ReturnsSetValue(string value)
        {
            // Arrange
            var instance = new TaskStartedEventArgs2(_message, _helpKeyword, _projectFile, _taskFile, _taskName, _eventTimestamp);

            // Act
            instance.TaskAssemblyLocation = value;

            // Assert
            Assert.Equal(value, instance.TaskAssemblyLocation);
        }
    }
}
