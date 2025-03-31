// using FluentAssertions;
// using System;
// using System.Collections.Generic;
// using Xunit;
// using Microsoft.Build.Logging.StructuredLogger;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="BuildStatistics"/> class.
//     /// </summary>
//     public class BuildStatisticsTests
//     {
//         private readonly BuildStatistics _buildStatistics;
// 
//         public BuildStatisticsTests()
//         {
//             _buildStatistics = new BuildStatistics();
//         }
// 
//         /// <summary>
//         /// A simple dummy task class to simulate a task with a Name property.
//         /// </summary>
//         private class TestTask
//         {
//             public string Name { get; set; }
// 
//             public TestTask(string name)
//             {
//                 Name = name;
//             }
//         }
// //  // [Error] (46-57)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildStatisticsTests.TestTask' to 'Microsoft.Build.Logging.StructuredLogger.Task' // [Error] (47-57)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildStatisticsTests.TestTask' to 'Microsoft.Build.Logging.StructuredLogger.Task'
// //         /// <summary>
// //         /// Tests that ReportTaskParameterMessage correctly adds a message to the TaskParameterMessagesByTask dictionary when given a valid task and message.
// //         /// </summary>
// //         [Fact]
// //         public void ReportTaskParameterMessage_WithValidInput_UpdatesTaskParameterMessagesDictionary()
// //         {
// //             // Arrange
// //             var testTask = new TestTask("TaskA");
// //             string message1 = "Parameter message 1";
// //             string message2 = "Parameter message 2";
// // 
// //             // Act
// //             _buildStatistics.ReportTaskParameterMessage(testTask, message1);
// //             _buildStatistics.ReportTaskParameterMessage(testTask, message2);
// // 
// //             // Assert
// //             _buildStatistics.TaskParameterMessagesByTask.Should().ContainKey(testTask.Name);
// //             _buildStatistics.TaskParameterMessagesByTask[testTask.Name].Should().ContainInOrder(new List<string> { message1, message2 });
// //         }
// //  // [Error] (65-76)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildStatisticsTests.TestTask' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         /// <summary>
//         /// Tests that ReportTaskParameterMessage throws a NullReferenceException when passed a null task.
//         /// </summary>
//         [Fact]
//         public void ReportTaskParameterMessage_NullTask_ThrowsNullReferenceException()
//         {
//             // Arrange
//             TestTask? nullTask = null;
//             string message = "Some Message";
// 
//             // Act
//             Action act = () => _buildStatistics.ReportTaskParameterMessage(nullTask!, message);
// 
//             // Assert
//             act.Should().Throw<NullReferenceException>();
//         }
// //  // [Error] (83-54)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildStatisticsTests.TestTask' to 'Microsoft.Build.Logging.StructuredLogger.Task' // [Error] (84-54)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildStatisticsTests.TestTask' to 'Microsoft.Build.Logging.StructuredLogger.Task'
// //         /// <summary>
// //         /// Tests that ReportOutputItemMessage correctly adds a message to the OutputItemMessagesByTask dictionary when given a valid task and message.
// //         /// </summary>
// //         [Fact]
// //         public void ReportOutputItemMessage_WithValidInput_UpdatesOutputItemMessagesDictionary()
// //         {
// //             // Arrange
// //             var testTask = new TestTask("TaskB");
// //             string message1 = "Output item message 1";
// //             string message2 = "Output item message 2";
// // 
// //             // Act
// //             _buildStatistics.ReportOutputItemMessage(testTask, message1);
// //             _buildStatistics.ReportOutputItemMessage(testTask, message2);
// // 
// //             // Assert
// //             _buildStatistics.OutputItemMessagesByTask.Should().ContainKey(testTask.Name);
// //             _buildStatistics.OutputItemMessagesByTask[testTask.Name].Should().ContainInOrder(new List<string> { message1, message2 });
// //         }
// //  // [Error] (102-73)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildStatisticsTests.TestTask' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         /// <summary>
//         /// Tests that ReportOutputItemMessage throws a NullReferenceException when passed a null task.
//         /// </summary>
//         [Fact]
//         public void ReportOutputItemMessage_NullTask_ThrowsNullReferenceException()
//         {
//             // Arrange
//             TestTask? nullTask = null;
//             string message = "Some Output Message";
// 
//             // Act
//             Action act = () => _buildStatistics.ReportOutputItemMessage(nullTask!, message);
// 
//             // Assert
//             act.Should().Throw<NullReferenceException>();
//         }
// 
//         /// <summary>
//         /// Tests that Add method creates a new bucket for a key that does not exist in the dictionary and adds the value.
//         /// </summary>
//         [Fact]
//         public void Add_WhenKeyDoesNotExist_AddsNewBucket()
//         {
//             // Arrange
//             string key = "NewKey";
//             string value = "NewValue";
//             var dictionary = new Dictionary<string, List<string>>();
// 
//             // Act
//             _buildStatistics.Add(key, value, dictionary);
// 
//             // Assert
//             dictionary.Should().ContainKey(key);
//             dictionary[key].Should().ContainSingle().Which.Should().Be(value);
//         }
// 
//         /// <summary>
//         /// Tests that Add method appends the value to the existing bucket if the key already exists in the dictionary.
//         /// </summary>
//         [Fact]
//         public void Add_WhenKeyExists_AppendsToExistingBucket()
//         {
//             // Arrange
//             string key = "ExistingKey";
//             string initialValue = "InitialValue";
//             string additionalValue = "AdditionalValue";
//             var dictionary = new Dictionary<string, List<string>>
//             {
//                 { key, new List<string> { initialValue } }
//             };
// 
//             // Act
//             _buildStatistics.Add(key, additionalValue, dictionary);
// 
//             // Assert
//             dictionary.Should().ContainKey(key);
//             dictionary[key].Should().HaveCount(2);
//             dictionary[key].Should().ContainInOrder(initialValue, additionalValue);
//         }
// 
//         /// <summary>
//         /// Tests that the TimedNodeCount property can be set and retrieved correctly.
//         /// </summary>
//         [Fact]
//         public void TimedNodeCount_SetAndGet_ReturnsSameValue()
//         {
//             // Arrange
//             int testValue = 42;
// 
//             // Act
//             _buildStatistics.TimedNodeCount = testValue;
//             int actualValue = _buildStatistics.TimedNodeCount;
// 
//             // Assert
//             actualValue.Should().Be(testValue);
//         }
//     }
// }