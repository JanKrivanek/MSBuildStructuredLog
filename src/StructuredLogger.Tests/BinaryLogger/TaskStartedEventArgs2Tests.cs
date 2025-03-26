using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TaskStartedEventArgs2"/> class.
    /// </summary>
    public class TaskStartedEventArgs2Tests
    {
        /// <summary>
        /// Tests that the constructor correctly sets the inherited base properties and initializes the new properties with their default values.
        /// </summary>
        [Fact]
        public void Constructor_WithValidParameters_SetsBasePropertiesAndDefaults()
        {
            // Arrange
            string expectedMessage = "Test message";
            string expectedHelpKeyword = "TestHelpKeyword";
            string expectedProjectFile = "project.csproj";
            string expectedTaskFile = "task.cs";
            string expectedTaskName = "TestTask";
            DateTime expectedEventTimestamp = DateTime.Now;

            // Act
            var eventArgs2 = new TaskStartedEventArgs2(
                expectedMessage,
                expectedHelpKeyword,
                expectedProjectFile,
                expectedTaskFile,
                expectedTaskName,
                expectedEventTimestamp);

            // Assert - Verify base properties are set as expected.
            Assert.Equal(expectedMessage, eventArgs2.Message);
            Assert.Equal(expectedHelpKeyword, eventArgs2.HelpKeyword);
            Assert.Equal(expectedProjectFile, eventArgs2.ProjectFile);
            Assert.Equal(expectedTaskFile, eventArgs2.TaskFile);
            Assert.Equal(expectedTaskName, eventArgs2.TaskName);
            Assert.Equal(expectedEventTimestamp, eventArgs2.EventTimestamp);

            // Assert - Verify that the new properties have their expected default values.
            Assert.Equal(0, eventArgs2.LineNumber);
            Assert.Equal(0, eventArgs2.ColumnNumber);
            Assert.Null(eventArgs2.TaskAssemblyLocation);
        }

        /// <summary>
        /// Tests that the <see cref="TaskStartedEventArgs2.LineNumber"/> property can be set and retrieved correctly.
        /// </summary>
        /// <param name="value">The value to set for the LineNumber property.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(123)]
        [InlineData(-10)]
        public void LineNumberProperty_SetAndGet_ReturnsSameValue(int value)
        {
            // Arrange
            var eventArgs2 = CreateDefaultTaskStartedEventArgs2();

            // Act
            eventArgs2.LineNumber = value;

            // Assert
            Assert.Equal(value, eventArgs2.LineNumber);
        }

        /// <summary>
        /// Tests that the <see cref="TaskStartedEventArgs2.ColumnNumber"/> property can be set and retrieved correctly.
        /// </summary>
        /// <param name="value">The value to set for the ColumnNumber property.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(456)]
        [InlineData(-5)]
        public void ColumnNumberProperty_SetAndGet_ReturnsSameValue(int value)
        {
            // Arrange
            var eventArgs2 = CreateDefaultTaskStartedEventArgs2();

            // Act
            eventArgs2.ColumnNumber = value;

            // Assert
            Assert.Equal(value, eventArgs2.ColumnNumber);
        }

        /// <summary>
        /// Tests that the <see cref="TaskStartedEventArgs2.TaskAssemblyLocation"/> property can be set and retrieved correctly.
        /// </summary>
        /// <param name="value">The string value to set for the TaskAssemblyLocation property.</param>
        [Theory]
        [InlineData("C:\\Path\\To\\Assembly.dll")]
        [InlineData("")]
        [InlineData(null)]
        public void TaskAssemblyLocationProperty_SetAndGet_ReturnsSameValue(string value)
        {
            // Arrange
            var eventArgs2 = CreateDefaultTaskStartedEventArgs2();

            // Act
            eventArgs2.TaskAssemblyLocation = value;

            // Assert
            Assert.Equal(value, eventArgs2.TaskAssemblyLocation);
        }

        /// <summary>
        /// Creates a default instance of <see cref="TaskStartedEventArgs2"/> for testing purposes.
        /// </summary>
        /// <returns>A new instance of <see cref="TaskStartedEventArgs2"/> initialized with default data.</returns>
        private static TaskStartedEventArgs2 CreateDefaultTaskStartedEventArgs2()
        {
            return new TaskStartedEventArgs2(
                "Default message",
                "DefaultHelpKeyword",
                "DefaultProject.csproj",
                "DefaultTaskFile.cs",
                "DefaultTaskName",
                DateTime.Now);
        }
    }
}
