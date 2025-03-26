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
        /// <summary>
        /// A minimal dummy implementation for Task that provides a Name property.
        /// This is used for testing the BuildStatistics Report methods.
        /// </summary>
        private class DummyTask
        {
            public string Name { get; set; }

            public DummyTask(string name)
            {
                Name = name;
            }
        }

        /// <summary>
        /// Tests the Add method for the scenario where the key does not yet exist in the dictionary.
        /// It verifies that a new list is created and the value is added.
        /// </summary>
        [Fact]
        public void Add_KeyDoesNotExist_AddsNewBucketWithValue()
        {
            // Arrange
            var statistics = new BuildStatistics();
            var dictionary = new Dictionary<string, List<string>>();
            string key = "TestTask";
            string value = "Test message";

            // Act
            statistics.Add(key, value, dictionary);

            // Assert
            Assert.True(dictionary.ContainsKey(key), "Dictionary should contain the new key after adding.");
            Assert.Single(dictionary[key]);
            Assert.Equal(value, dictionary[key][0]);
        }

        /// <summary>
        /// Tests the Add method for the scenario where the key already exists in the dictionary.
        /// It verifies that the value is appended to the existing list.
        /// </summary>
        [Fact]
        public void Add_KeyExists_AppendsValueToExistingBucket()
        {
            // Arrange
            var statistics = new BuildStatistics();
            var dictionary = new Dictionary<string, List<string>>();
            string key = "TestTask";
            string firstValue = "First message";
            string secondValue = "Second message";
            dictionary[key] = new List<string> { firstValue };

            // Act
            statistics.Add(key, secondValue, dictionary);

            // Assert
            Assert.True(dictionary.ContainsKey(key), "Dictionary should contain the key.");
            Assert.Equal(2, dictionary[key].Count);
            Assert.Contains(firstValue, dictionary[key]);
            Assert.Contains(secondValue, dictionary[key]);
        }

        /// <summary>
        /// Tests the Add method when a null dictionary is passed.
        /// It is expected to throw a NullReferenceException.
        /// </summary>
        [Fact]
        public void Add_NullDictionary_ThrowsNullReferenceException()
        {
            // Arrange
            var statistics = new BuildStatistics();
            string key = "TestTask";
            string value = "Test message";
            Dictionary<string, List<string>> nullDictionary = null!;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => statistics.Add(key, value, nullDictionary));
        }

        /// <summary>
        /// Tests the Add method when a null key is passed.
        /// The underlying dictionary should throw an ArgumentNullException.
        /// </summary>
        [Fact]
        public void Add_NullKey_ThrowsArgumentNullException()
        {
            // Arrange
            var statistics = new BuildStatistics();
            var dictionary = new Dictionary<string, List<string>>();
            string? key = null;
            string value = "Test message";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => statistics.Add(key!, value, dictionary));
        }

        /// <summary>
        /// Tests the ReportTaskParameterMessage method to ensure it correctly adds the message under the task's name in TaskParameterMessagesByTask.
        /// </summary>
        [Fact]
        public void ReportTaskParameterMessage_ValidTask_AddsMessageToTaskParameterMessagesByTask()
        {
            // Arrange
            var statistics = new BuildStatistics();
            var taskName = "DummyTask1";
            var dummyTask = new DummyTask(taskName);
            string message = "Parameter message";

            // Act
            statistics.ReportTaskParameterMessage(new DummyTaskAdapter(dummyTask), message);

            // Assert
            Assert.True(statistics.TaskParameterMessagesByTask.ContainsKey(taskName), "TaskParameterMessagesByTask should contain the task name.");
            Assert.Single(statistics.TaskParameterMessagesByTask[taskName]);
            Assert.Equal(message, statistics.TaskParameterMessagesByTask[taskName][0]);
        }

        /// <summary>
        /// Tests the ReportOutputItemMessage method to ensure it correctly adds the message under the task's name in OutputItemMessagesByTask.
        /// </summary>
        [Fact]
        public void ReportOutputItemMessage_ValidTask_AddsMessageToOutputItemMessagesByTask()
        {
            // Arrange
            var statistics = new BuildStatistics();
            var taskName = "DummyTask2";
            var dummyTask = new DummyTask(taskName);
            string message = "Output item message";

            // Act
            statistics.ReportOutputItemMessage(new DummyTaskAdapter(dummyTask), message);

            // Assert
            Assert.True(statistics.OutputItemMessagesByTask.ContainsKey(taskName), "OutputItemMessagesByTask should contain the task name.");
            Assert.Single(statistics.OutputItemMessagesByTask[taskName]);
            Assert.Equal(message, statistics.OutputItemMessagesByTask[taskName][0]);
        }

        /// <summary>
        /// Tests the TimedNodeCount property to ensure that the getter and setter work as expected.
        /// </summary>
        [Fact]
        public void TimedNodeCount_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            var statistics = new BuildStatistics();
            int expectedValue = 42;

            // Act
            statistics.TimedNodeCount = expectedValue;
            int actualValue = statistics.TimedNodeCount;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        /// <summary>
        /// Tests the behavior of ReportTaskParameterMessage and ReportOutputItemMessage when the message is null or empty.
        /// It verifies that the methods correctly add the null or empty messages.
        /// </summary>
        [Fact]
        public void ReportMethods_NullOrEmptyMessage_AddsMessageCorrectly()
        {
            // Arrange
            var statistics = new BuildStatistics();
            var taskName = "TestTask";
            var dummyTask = new DummyTask(taskName);
            string? nullMessage = null;
            string emptyMessage = string.Empty;

            // Act
            statistics.ReportTaskParameterMessage(new DummyTaskAdapter(dummyTask), nullMessage);
            statistics.ReportOutputItemMessage(new DummyTaskAdapter(dummyTask), emptyMessage);

            // Assert
            Assert.True(statistics.TaskParameterMessagesByTask.ContainsKey(taskName), "TaskParameterMessagesByTask should contain the task name.");
            Assert.Single(statistics.TaskParameterMessagesByTask[taskName]);
            Assert.Null(statistics.TaskParameterMessagesByTask[taskName][0]);

            Assert.True(statistics.OutputItemMessagesByTask.ContainsKey(taskName), "OutputItemMessagesByTask should contain the task name.");
            Assert.Single(statistics.OutputItemMessagesByTask[taskName]);
            Assert.Equal(emptyMessage, statistics.OutputItemMessagesByTask[taskName][0]);
        }

        /// <summary>
        /// An adapter class to wrap DummyTask into an instance that has a public Name property,
        /// matching the expected Task parameter in BuildStatistics methods.
        /// </summary>
        private class DummyTaskAdapter : global::Microsoft.Build.Logging.StructuredLogger.TaskBase
        {
            private readonly DummyTask _dummyTask;

            public DummyTaskAdapter(DummyTask dummyTask)
            {
                _dummyTask = dummyTask;
            }

            public override string Name => _dummyTask.Name;
        }
    }

    /// <summary>
    /// A minimal abstract base class to simulate the Task type expected by BuildStatistics.
    /// This allows the creation of a derived type that exposes the Name property.
    /// </summary>
    public abstract class TaskBase
    {
        public abstract string Name { get; }
    }
}
