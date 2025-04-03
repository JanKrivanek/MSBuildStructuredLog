// using System;
// using System.Collections.Generic;
// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
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
//         #region Tests for Add method
// 
//         /// <summary>
//         /// Tests the Add method when the specified key does not exist in the dictionary.
//         /// It verifies that a new list is created and the value is added.
//         /// </summary>
//         [Fact]
//         public void Add_WhenKeyDoesNotExist_CreatesNewListAndAddsValue()
//         {
//             // Arrange
//             var dictionary = new Dictionary<string, List<string>>();
//             string key = "TestKey";
//             string value = "TestValue";
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
//         /// Tests the Add method when the specified key already exists in the dictionary.
//         /// It verifies that the value is appended to the existing list.
//         /// </summary>
//         [Fact]
//         public void Add_WhenKeyExists_AppendsValueToExistingList()
//         {
//             // Arrange
//             var dictionary = new Dictionary<string, List<string>>
//             {
//                 { "ExistingKey", new List<string> { "InitialValue" } }
//             };
//             string key = "ExistingKey";
//             string newValue = "AppendedValue";
// 
//             // Act
//             _buildStatistics.Add(key, newValue, dictionary);
// 
//             // Assert
//             dictionary.Should().ContainKey(key);
//             dictionary[key].Should().HaveCount(2).And.Contain(newValue);
//         }
// 
//         /// <summary>
//         /// Tests the Add method using edge cases such as an empty key and a very long value.
//         /// </summary>
//         [Fact]
//         public void Add_WhenCalledWithEdgeCaseValues_AddsValueCorrectly()
//         {
//             // Arrange
//             var dictionary = new Dictionary<string, List<string>>();
//             string emptyKey = string.Empty;
//             string longValue = new string('x', 1000);
// 
//             // Act
//             _buildStatistics.Add(emptyKey, longValue, dictionary);
// 
//             // Assert
//             dictionary.Should().ContainKey(emptyKey);
//             dictionary[emptyKey].Should().ContainSingle().Which.Should().Be(longValue);
//         }
// //  // [Error] (99-35)CS0117 'Task' does not contain a definition for 'Name' // [Error] (103-57)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
// //         #endregion
// // 
// //         #region Tests for ReportTaskParameterMessage
// // 
// //         /// <summary>
// //         /// Tests the ReportTaskParameterMessage method with a standard task and message.
// //         /// It verifies that the message is recorded in the TaskParameterMessagesByTask dictionary.
// //         /// </summary>
// //         [Fact]
// //         public void ReportTaskParameterMessage_WhenCalled_AddsMessageToTaskParameterMessagesByTask()
// //         {
// //             // Arrange
// //             // NOTE: Replace the instantiation below with a valid instance of Microsoft.Build.Logging.StructuredLogger.Task
// //             // if the Task class does not have a public parameterless constructor.
// //             var task = new Task { Name = "TestTask" };
// //             string message = "Parameter message";
// // 
// //             // Act
// //             _buildStatistics.ReportTaskParameterMessage(task, message);
// // 
// //             // Assert
// //             _buildStatistics.TaskParameterMessagesByTask.Should().ContainKey("TestTask");
// //             _buildStatistics.TaskParameterMessagesByTask["TestTask"].Should().ContainSingle().Which.Should().Be(message);
// //         }
// //  // [Error] (118-35)CS0117 'Task' does not contain a definition for 'Name' // [Error] (122-57)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         /// <summary>
//         /// Tests the ReportTaskParameterMessage method using an empty message.
//         /// It verifies that an empty string message is recorded correctly.
//         /// </summary>
//         [Fact]
//         public void ReportTaskParameterMessage_WithEmptyMessage_AddsEmptyMessage()
//         {
//             // Arrange
//             var task = new Task { Name = "EmptyMessageTask" };
//             string message = string.Empty;
// 
//             // Act
//             _buildStatistics.ReportTaskParameterMessage(task, message);
// 
//             // Assert
//             _buildStatistics.TaskParameterMessagesByTask.Should().ContainKey("EmptyMessageTask");
//             _buildStatistics.TaskParameterMessagesByTask["EmptyMessageTask"].Should().ContainSingle().Which.Should().Be(message);
//         }
// //  // [Error] (143-35)CS0117 'Task' does not contain a definition for 'Name' // [Error] (147-54)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
// //         #endregion
// // 
// //         #region Tests for ReportOutputItemMessage
// // 
// //         /// <summary>
// //         /// Tests the ReportOutputItemMessage method with a standard task and message.
// //         /// It verifies that the message is recorded in the OutputItemMessagesByTask dictionary.
// //         /// </summary>
// //         [Fact]
// //         public void ReportOutputItemMessage_WhenCalled_AddsMessageToOutputItemMessagesByTask()
// //         {
// //             // Arrange
// //             // NOTE: Replace the instantiation below with a valid instance of Microsoft.Build.Logging.StructuredLogger.Task
// //             // if the Task class does not have a public parameterless constructor.
// //             var task = new Task { Name = "OutputTask" };
// //             string message = "Output item message";
// // 
// //             // Act
// //             _buildStatistics.ReportOutputItemMessage(task, message);
// // 
// //             // Assert
// //             _buildStatistics.OutputItemMessagesByTask.Should().ContainKey("OutputTask");
// //             _buildStatistics.OutputItemMessagesByTask["OutputTask"].Should().ContainSingle().Which.Should().Be(message);
// //         }
// //  // [Error] (162-35)CS0117 'Task' does not contain a definition for 'Name' // [Error] (166-54)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         /// <summary>
//         /// Tests the ReportOutputItemMessage method using a message with special characters.
//         /// It verifies that the method correctly records messages even with special characters.
//         /// </summary>
//         [Fact]
//         public void ReportOutputItemMessage_WithSpecialCharactersMessage_AddsMessageCorrectly()
//         {
//             // Arrange
//             var task = new Task { Name = "SpecialCharTask" };
//             string message = "!@#$%^&*()_+[]{};:,.<>/?";
// 
//             // Act
//             _buildStatistics.ReportOutputItemMessage(task, message);
// 
//             // Assert
//             _buildStatistics.OutputItemMessagesByTask.Should().ContainKey("SpecialCharTask");
//             _buildStatistics.OutputItemMessagesByTask["SpecialCharTask"].Should().ContainSingle().Which.Should().Be(message);
//         }
// 
//         #endregion
//     }
// }