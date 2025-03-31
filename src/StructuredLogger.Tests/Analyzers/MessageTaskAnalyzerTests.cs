// using FluentAssertions;
// using Moq;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="MessageTaskAnalyzer"/> class.
//     /// </summary>
//     public class MessageTaskAnalyzerTests
//     {
//         /// <summary>
//         /// Tests that calling Analyze with a null Task parameter throws a NullReferenceException.
//         /// </summary>
//         [Fact]
//         public void Analyze_NullTask_ThrowsNullReferenceException()
//         {
//             // Arrange
//             Action act = () => MessageTaskAnalyzer.Analyze(null!);
// 
//             // Act & Assert
//             act.Should().Throw<NullReferenceException>();
//         }
// //  // [Error] (36-18)CS0229 Ambiguity between 'Task.Name' and 'Task.Name' // [Error] (42-18)CS0229 Ambiguity between 'Task.Name' and 'Task.Name'
// //         /// <summary>
// //         /// Tests that when the Task has no children, Analyze does not modify the Task's Name.
// //         /// </summary>
// //         [Fact]
// //         public void Analyze_TaskWithNoChildren_DoesNotChangeName()
// //         {
// //             // Arrange
// //             var task = new Task();
// //             task.Name = "OriginalName";
// // 
// //             // Act
// //             MessageTaskAnalyzer.Analyze(task);
// // 
// //             // Assert
// //             task.Name.Should().Be("OriginalName");
// //         }
// //  // [Error] (57-18)CS0229 Ambiguity between 'Task.Name' and 'Task.Name' // [Error] (64-18)CS0229 Ambiguity between 'Task.Name' and 'Task.Name'
//         /// <summary>
//         /// Tests that when the Task has a Message child with a null ShortenedText, Analyze does not modify the Task's Name.
//         /// </summary>
//         [Fact]
//         public void Analyze_TaskWithMessageHavingNullShortenedText_DoesNotChangeName()
//         {
//             // Arrange
//             var message = new Message
//             {
//                 ShortenedText = null
//             };
//             var task = new Task();
//             task.Name = "OriginalName";
//             task.Children.Add(message);
// 
//             // Act
//             MessageTaskAnalyzer.Analyze(task);
// 
//             // Assert
//             task.Name.Should().Be("OriginalName");
//         }
// //  // [Error] (86-18)CS0229 Ambiguity between 'Task.Name' and 'Task.Name'
// //         /// <summary>
// //         /// Tests that when the Task has a Message child with a non-null ShortenedText, Analyze sets the Task's Name appropriately.
// //         /// </summary>
// //         [Fact]
// //         public void Analyze_TaskWithMessageHavingNonNullShortenedText_SetsMessageName()
// //         {
// //             // Arrange
// //             var expectedText = "TestMessage";
// //             var message = new Message
// //             {
// //                 ShortenedText = expectedText
// //             };
// //             var task = new Task();
// //             task.Children.Add(message);
// // 
// //             // Act
// //             MessageTaskAnalyzer.Analyze(task);
// // 
// //             // Assert
// //             task.Name.Should().Be("Message: " + expectedText);
// //         }
// //  // [Error] (98-17)CS0229 Ambiguity between 'Task.Name' and 'Task.Name'
//         /// <summary>
//         /// Tests that when the Task's Children property is null, Analyze throws a NullReferenceException.
//         /// </summary>
//         [Fact]
//         public void Analyze_TaskWithNullChildren_ThrowsNullReferenceException()
//         {
//             // Arrange
//             var task = new TaskWithNullChildren
//             {
//                 Name = "OriginalName"
//             };
// 
//             // Act
//             Action act = () => MessageTaskAnalyzer.Analyze(task);
// 
//             // Assert
//             act.Should().Throw<NullReferenceException>();
//         }
//     }
// //  // [Error] (115-18)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'Task'
// //     // Minimal implementations added solely for testing purposes because the definitions
// //     // for Task and Message are not provided in the snippet.
// // 
// //     /// <summary>
// //     /// Minimal implementation of a Task for testing.
// //     /// </summary>
// //     public class Task
// //     {
// //         public IList<object> Children { get; set; } = new List<object>();
// //         public string? Name { get; set; }
// //     }
// //  // [Error] (124-18)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'Message'
//     /// <summary>
//     /// Minimal implementation of a Message for testing.
//     /// </summary>
//     public class Message
//     {
//         public string? ShortenedText { get; set; }
//     }
// 
//     /// <summary>
//     /// A derived Task class simulating a scenario where the Children property is null.
//     /// </summary>
//     public class TaskWithNullChildren : Task
//     {
//         public TaskWithNullChildren()
//         {
//             Children = null!;
//         }
//     }
// }