// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Moq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger
// {
//     /// <summary>
//     /// Represents a Task with child elements and a Name property.
//     /// </summary>
//     public class Task
//     {
//         public List<object> Children { get; set; } = new List<object>();
//         public string Name { get; set; }
//     }
// 
//     /// <summary>
//     /// Represents a Message with a ShortenedText property.
//     /// </summary>
//     public class Message
//     {
//         public string ShortenedText { get; set; }
//     }
// 
//     /// <summary>
//     /// Provides analysis for tasks based on contained messages.
//     /// </summary>
//     public class MessageTaskAnalyzer
//     {
//         /// <summary>
//         /// Analyzes a Task to update its Name property based on a contained Message's ShortenedText.
//         /// </summary>
//         /// <param name="task">The task to analyze.</param>
// //         public static void Analyze(Task task) [Error] (37-32)CS0229 Ambiguity between 'Task.Children' and 'Task.Children'
// //         {
// //             var message = task.Children.OfType<Message>().FirstOrDefault();
// //             if (message?.ShortenedText != null)
// //             {
// //                 task.Name = "Message: " + message.ShortenedText;
// //             }
// //         }
//     }
// }
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Microsoft.Build.Logging.StructuredLogger.MessageTaskAnalyzer"/> class.
//     /// </summary>
//     public class MessageTaskAnalyzerTests
//     {
//         /// <summary>
//         /// Tests that Analyze sets the Task.Name when a child Message with a non-null ShortenedText is present.
//         /// </summary>
// //         [Fact] [Error] (62-18)CS0229 Ambiguity between 'Task.Children' and 'Task.Children'
// //         public void Analyze_WhenTaskHasMessageWithNonNullShortenedText_SetsTaskName()
// //         {
// //             // Arrange
// //             var expectedText = "Test Message";
// //             var task = new Microsoft.Build.Logging.StructuredLogger.Task();
// //             task.Children.Add(new Microsoft.Build.Logging.StructuredLogger.Message { ShortenedText = expectedText });
// //             task.Name = "Initial";
// // 
// //             // Act
// //             Microsoft.Build.Logging.StructuredLogger.MessageTaskAnalyzer.Analyze(task);
// // 
// //             // Assert
// //             Assert.Equal("Message: " + expectedText, task.Name);
// //         }
// 
//         /// <summary>
//         /// Tests that Analyze does not update Task.Name when the Task has no children.
//         /// </summary>
//         [Fact]
//         public void Analyze_WhenTaskHasNoChildren_LeavesTaskNameUnchanged()
//         {
//             // Arrange
//             var originalName = "OriginalName";
//             var task = new Microsoft.Build.Logging.StructuredLogger.Task { Name = originalName };
// 
//             // Act
//             Microsoft.Build.Logging.StructuredLogger.MessageTaskAnalyzer.Analyze(task);
// 
//             // Assert
//             Assert.Equal(originalName, task.Name);
//         }
// 
//         /// <summary>
//         /// Tests that Analyze does not update Task.Name when the child Message's ShortenedText is null.
//         /// </summary>
// //         [Fact] [Error] (98-18)CS0229 Ambiguity between 'Task.Children' and 'Task.Children'
// //         public void Analyze_WhenTaskHasMessageWithNullShortenedText_DoesNotChangeTaskName()
// //         {
// //             // Arrange
// //             var originalName = "OriginalName";
// //             var task = new Microsoft.Build.Logging.StructuredLogger.Task { Name = originalName };
// //             task.Children.Add(new Microsoft.Build.Logging.StructuredLogger.Message { ShortenedText = null });
// // 
// //             // Act
// //             Microsoft.Build.Logging.StructuredLogger.MessageTaskAnalyzer.Analyze(task);
// // 
// //             // Assert
// //             Assert.Equal(originalName, task.Name);
// //         }
// 
//         /// <summary>
//         /// Tests that Analyze uses the first Message in the Task.Children collection when multiple Message objects are present.
//         /// </summary>
// //         [Fact] [Error] (117-18)CS0229 Ambiguity between 'Task.Children' and 'Task.Children' [Error] (118-18)CS0229 Ambiguity between 'Task.Children' and 'Task.Children'
// //         public void Analyze_WhenTaskHasMultipleMessages_UsesFirstMessageOnly()
// //         {
// //             // Arrange
// //             var firstMessageText = "First";
// //             var secondMessageText = "Second";
// //             var task = new Microsoft.Build.Logging.StructuredLogger.Task { Name = "Initial" };
// //             task.Children.Add(new Microsoft.Build.Logging.StructuredLogger.Message { ShortenedText = firstMessageText });
// //             task.Children.Add(new Microsoft.Build.Logging.StructuredLogger.Message { ShortenedText = secondMessageText });
// // 
// //             // Act
// //             Microsoft.Build.Logging.StructuredLogger.MessageTaskAnalyzer.Analyze(task);
// // 
// //             // Assert
// //             Assert.Equal("Message: " + firstMessageText, task.Name);
// //         }
// 
//         /// <summary>
//         /// Tests that Analyze throws a NullReferenceException when a null Task is passed.
//         /// </summary>
//         [Fact]
//         public void Analyze_WhenTaskIsNull_ThrowsNullReferenceException()
//         {
//             // Arrange
//             Microsoft.Build.Logging.StructuredLogger.Task task = null;
// 
//             // Act & Assert
//             Assert.Throws<NullReferenceException>(() =>
//                 Microsoft.Build.Logging.StructuredLogger.MessageTaskAnalyzer.Analyze(task));
//         }
// 
//         /// <summary>
//         /// Tests that Analyze throws a NullReferenceException when Task.Children is null.
//         /// </summary>
// //         [Fact] [Error] (148-94)CS0229 Ambiguity between 'Task.Children' and 'Task.Children'
// //         public void Analyze_WhenTaskChildrenIsNull_ThrowsNullReferenceException()
// //         {
// //             // Arrange
// //             var task = new Microsoft.Build.Logging.StructuredLogger.Task { Name = "Initial", Children = null };
// // 
// //             // Act & Assert
// //             Assert.Throws<NullReferenceException>(() =>
// //                 Microsoft.Build.Logging.StructuredLogger.MessageTaskAnalyzer.Analyze(task));
// //         }
//     }
// }
