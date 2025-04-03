// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Microsoft.Build.Logging.StructuredLogger.UnitTests;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "MessageTaskAnalyzer"/> class.
//     /// </summary>
//     public class MessageTaskAnalyzerTests
//     {
// //         /// <summary> // [Error] (23-40)CS1061 'Task' does not contain a definition for 'Name' and no accessible extension method 'Name' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?) // [Error] (25-41)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task' // [Error] (27-18)CS1061 'Task' does not contain a definition for 'Name' and no accessible extension method 'Name' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests that when the task has no children, the Analyze method does not change the task name.
// //         /// </summary>
// //         [Fact]
// //         public void Analyze_WhenTaskHasNoChildren_TaskNameRemainsUnchanged()
// //         {
// //             // Arrange
// //             Task task = CreateTestTask();
// //             string originalName = task.Name;
// //             // Act
// //             MessageTaskAnalyzer.Analyze(task);
// //             // Assert
// //             task.Name.Should().Be(originalName, "because no Message child was present to update the task name");
// //         }
// //  // [Error] (39-40)CS1061 'Task' does not contain a definition for 'Name' and no accessible extension method 'Name' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?) // [Error] (43-18)CS1061 'Task' does not contain a definition for 'Children' and no accessible extension method 'Children' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?) // [Error] (45-41)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task' // [Error] (47-18)CS1061 'Task' does not contain a definition for 'Name' and no accessible extension method 'Name' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that when the task has a Message child with a null ShortenedText,
//         /// the Analyze method does not update the task name.
//         /// </summary>
//         [Fact]
//         public void Analyze_WhenTaskHasMessageChildWithNullShortenedText_TaskNameRemainsUnchanged()
//         {
//             // Arrange
//             Task task = CreateTestTask();
//             string originalName = task.Name;
//             // TODO: Since Microsoft.Build.Logging.StructuredLogger.Message.ShortenedText cannot be set directly,
//             // a test-specific implementation (TestMessage) is used to simulate a Message with null ShortenedText.
//             var message = new TestMessage(null);
//             task.Children.Add(message);
//             // Act
//             MessageTaskAnalyzer.Analyze(task);
//             // Assert
//             task.Name.Should().Be(originalName, "because the Message child's ShortenedText was null");
//         }
// //  // [Error] (63-18)CS1061 'Task' does not contain a definition for 'Children' and no accessible extension method 'Children' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?) // [Error] (65-41)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task' // [Error] (67-18)CS1061 'Task' does not contain a definition for 'Name' and no accessible extension method 'Name' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that when the task has a Message child with a non-null ShortenedText,
// //         /// the Analyze method updates the task name accordingly.
// //         /// </summary>
// //         [Fact]
// //         public void Analyze_WhenTaskHasMessageChildWithShortenedText_TaskNameIsUpdated()
// //         {
// //             // Arrange
// //             Task task = CreateTestTask();
// //             string expectedText = "Test Message";
// //             // TODO: Since Microsoft.Build.Logging.StructuredLogger.Message.ShortenedText cannot be set directly,
// //             // a test-specific implementation (TestMessage) is used to simulate a Message with a valid ShortenedText.
// //             var message = new TestMessage(expectedText);
// //             task.Children.Add(message);
// //             // Act
// //             MessageTaskAnalyzer.Analyze(task);
// //             // Assert
// //             task.Name.Should().Be("Message: " + expectedText, "because the Message child's ShortenedText was provided");
// //         }
// // 
//         /// <summary>
//         /// Helper method to create a testable instance of Task with a modifiable Children collection.
//         /// </summary>
//         /// <returns>A Task instance for testing purposes.</returns>
//         private Task CreateTestTask()
//         {
//             // Since the Task.Children property cannot be set directly in the original type,
//             // a test-specific subclass (TaskWrapper) is used to allow manipulation of the Children list.
//             return new TaskWrapper();
//         }
//     }
// 
//     // Test double for Task to allow modification of Children and Name.
//     internal class TaskWrapper : Task
//     {
//         private readonly IList<BaseNode> _children;
//         private string _name;
//         public TaskWrapper()
//         {
//             // Initialize the modifiable children list.
//             _children = new List<BaseNode>();
//             _name = string.Empty;
//         }
// //  // [Error] (97-41)CS0115 'TaskWrapper.Children': no suitable method found to override
// //         /// <summary>
// //         /// Gets the modifiable list of child nodes.
// //         /// </summary>
// //         public override IList<BaseNode> Children => _children;
// //         /// <summary> // [Error] (101-32)CS0115 'TaskWrapper.Name': no suitable method found to override
//         /// Gets or sets the task name.
//         /// </summary>
//         public override string Name { get => _name; set => _name = value; }
//     }
// 
//     // Test double for Message to allow specifying a ShortenedText value.
//     internal class TestMessage : Message
//     {
//         private readonly string? _shortenedText;
//         public TestMessage(string? shortenedText)
//         {
//             _shortenedText = shortenedText;
//         }
// //  // [Error] (116-33)CS0506 'TestMessage.ShortenedText': cannot override inherited member 'TextNode.ShortenedText' because it is not marked virtual, abstract, or override
// //         /// <summary>
// //         /// Gets the test-specific ShortenedText value.
// //         /// </summary>
// //         public override string? ShortenedText => _shortenedText;
// //     }
// }