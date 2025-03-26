using System;
using System.Collections.Generic;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildStatistics"/> class.
    /// </summary>
    public class BuildStatisticsTests
    {
        private readonly BuildStatistics _buildStatistics;

        public BuildStatisticsTests()
        {
            _buildStatistics = new BuildStatistics();
        }

        /// <summary>
        /// A dummy task class to simulate a task with a Name property.
        /// </summary>
        private class TestTask
        {
            public string Name { get; set; }
        }

        #region Tests for Add method

        /// <summary>
        /// Tests that the Add method creates a new list when the key does not exist and adds the value.
        /// </summary>
        [Fact]
        public void Add_NewKey_CreatesListAndAddsValue()
        {
            // Arrange
            string key = "Task1";
            string value = "Message1";
            var dictionary = new Dictionary<string, List<string>>();

            // Act
            _buildStatistics.Add(key, value, dictionary);

            // Assert
            Assert.True(dictionary.ContainsKey(key));
            Assert.Single(dictionary[key]);
            Assert.Equal(value, dictionary[key][0]);
        }

        /// <summary>
        /// Tests that the Add method appends a value to an existing list when the key already exists.
        /// </summary>
        [Fact]
        public void Add_ExistingKey_AppendsValueToList()
        {
            // Arrange
            string key = "Task1";
            string initialValue = "Message1";
            string additionalValue = "Message2";
            var dictionary = new Dictionary<string, List<string>>
            {
                { key, new List<string> { initialValue } }
            };

            // Act
            _buildStatistics.Add(key, additionalValue, dictionary);

            // Assert
            Assert.True(dictionary.ContainsKey(key));
            Assert.Equal(2, dictionary[key].Count);
            Assert.Contains(initialValue, dictionary[key]);
            Assert.Contains(additionalValue, dictionary[key]);
        }

        /// <summary>
        /// Tests that the Add method throws an ArgumentNullException when a null key is provided.
        /// </summary>
        [Fact]
        public void Add_NullKey_ThrowsArgumentNullException()
        {
            // Arrange
            string key = null;
            string value = "Message";
            var dictionary = new Dictionary<string, List<string>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _buildStatistics.Add(key, value, dictionary));
        }

        /// <summary>
        /// Tests that the Add method adds a null value to the list if a null value is provided.
        /// </summary>
        [Fact]
        public void Add_NullValue_AddsNullToList()
        {
            // Arrange
            string key = "Task1";
            string value = null;
            var dictionary = new Dictionary<string, List<string>>();

            // Act
            _buildStatistics.Add(key, value, dictionary);

            // Assert
            Assert.True(dictionary.ContainsKey(key));
            Assert.Single(dictionary[key]);
            Assert.Null(dictionary[key][0]);
        }

        /// <summary>
        /// Tests that the Add method throws a NullReferenceException when the dictionary is null.
        /// </summary>
        [Fact]
        public void Add_NullDictionary_ThrowsNullReferenceException()
        {
            // Arrange
            string key = "Task1";
            string value = "Message";
            Dictionary<string, List<string>> dictionary = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _buildStatistics.Add(key, value, dictionary));
        }

        #endregion

        #region Tests for ReportTaskParameterMessage method

        /// <summary>
        /// Tests that ReportTaskParameterMessage adds a message to TaskParameterMessagesByTask for a new task.
        /// </summary>
        [Fact]
        public void ReportTaskParameterMessage_NewTask_AddsMessage()
        {
            // Arrange
            var task = new TestTask { Name = "TaskA" };
            string message = "Parameter message 1";

            // Act
            _buildStatistics.ReportTaskParameterMessage(ConvertToTask(task), message);

            // Assert
            Assert.True(_buildStatistics.TaskParameterMessagesByTask.ContainsKey(task.Name));
            Assert.Single(_buildStatistics.TaskParameterMessagesByTask[task.Name]);
            Assert.Equal(message, _buildStatistics.TaskParameterMessagesByTask[task.Name][0]);
        }

        /// <summary>
        /// Tests that ReportTaskParameterMessage appends a message to the existing list when the task already exists.
        /// </summary>
        [Fact]
        public void ReportTaskParameterMessage_ExistingTask_AppendsMessage()
        {
            // Arrange
            var task = new TestTask { Name = "TaskA" };
            string firstMessage = "Parameter message 1";
            string secondMessage = "Parameter message 2";

            // Act
            _buildStatistics.ReportTaskParameterMessage(ConvertToTask(task), firstMessage);
            _buildStatistics.ReportTaskParameterMessage(ConvertToTask(task), secondMessage);

            // Assert
            Assert.True(_buildStatistics.TaskParameterMessagesByTask.ContainsKey(task.Name));
            Assert.Equal(2, _buildStatistics.TaskParameterMessagesByTask[task.Name].Count);
            Assert.Contains(firstMessage, _buildStatistics.TaskParameterMessagesByTask[task.Name]);
            Assert.Contains(secondMessage, _buildStatistics.TaskParameterMessagesByTask[task.Name]);
        }

        #endregion

        #region Tests for ReportOutputItemMessage method

        /// <summary>
        /// Tests that ReportOutputItemMessage adds a message to OutputItemMessagesByTask for a new task.
        /// </summary>
        [Fact]
        public void ReportOutputItemMessage_NewTask_AddsMessage()
        {
            // Arrange
            var task = new TestTask { Name = "TaskB" };
            string message = "Output item message 1";

            // Act
            _buildStatistics.ReportOutputItemMessage(ConvertToTask(task), message);

            // Assert
            Assert.True(_buildStatistics.OutputItemMessagesByTask.ContainsKey(task.Name));
            Assert.Single(_buildStatistics.OutputItemMessagesByTask[task.Name]);
            Assert.Equal(message, _buildStatistics.OutputItemMessagesByTask[task.Name][0]);
        }

        /// <summary>
        /// Tests that ReportOutputItemMessage appends a message to the existing list when the task already exists.
        /// </summary>
        [Fact]
        public void ReportOutputItemMessage_ExistingTask_AppendsMessage()
        {
            // Arrange
            var task = new TestTask { Name = "TaskB" };
            string firstMessage = "Output item message 1";
            string secondMessage = "Output item message 2";

            // Act
            _buildStatistics.ReportOutputItemMessage(ConvertToTask(task), firstMessage);
            _buildStatistics.ReportOutputItemMessage(ConvertToTask(task), secondMessage);

            // Assert
            Assert.True(_buildStatistics.OutputItemMessagesByTask.ContainsKey(task.Name));
            Assert.Equal(2, _buildStatistics.OutputItemMessagesByTask[task.Name].Count);
            Assert.Contains(firstMessage, _buildStatistics.OutputItemMessagesByTask[task.Name]);
            Assert.Contains(secondMessage, _buildStatistics.OutputItemMessagesByTask[task.Name]);
        }

        #endregion

        /// <summary>
        /// Helper method to convert a TestTask to a Task-like object expected by BuildStatistics.
        /// Since the BuildStatistics.Report... methods expect a parameter of type Task with a Name property,
        /// this method creates a dynamic object with a Name property by leveraging an anonymous type and reflection.
        /// </summary>
        /// <param name="testTask">The TestTask instance.</param>
        /// <returns>An object with a Name property.</returns>
        private dynamic ConvertToTask(TestTask testTask)
        {
            // Since the BuildStatistics class expects a parameter of type "Task" with a "Name" property,
            // but we do not have access to the actual Task implementation, we use a simple anonymous object.
            // The BuildStatistics methods only use the Name property, so this is sufficient.
            return new { Name = testTask.Name };
        }
    }
}
