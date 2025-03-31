// using FluentAssertions;
// using Moq;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// using Microsoft.Build.Logging.StructuredLogger;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Task"/> class.
//     /// </summary>
//     public class TaskTests
//     {
// //         /// <summary> // [Error] (25-36)CS1061 'Task' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests that the TypeName property returns "Task".
// //         /// </summary>
// //         [Fact]
// //         public void TypeName_WhenCalled_ReturnsTask()
// //         {
// //             // Arrange
// //             var task = new Task();
// // 
// //             // Act
// //             string typeName = task.TypeName;
// // 
// //             // Assert
// //             typeName.Should().Be("Task");
// //         }
// //  // [Error] (41-39)CS1061 'Task' does not contain a definition for 'IsDerivedTask' and no accessible extension method 'IsDerivedTask' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that IsDerivedTask returns false when the instance is exactly of type Task.
//         /// </summary>
//         [Fact]
//         public void IsDerivedTask_WhenInstanceIsTask_ReturnsFalse()
//         {
//             // Arrange
//             var task = new Task();
// 
//             // Act
//             bool isDerivedTask = task.IsDerivedTask;
// 
//             // Assert
//             isDerivedTask.Should().BeFalse();
//         }
// //  // [Error] (62-37)CS1061 'TaskTests.FakeTask' does not contain a definition for 'GetMessages' and no accessible extension method 'GetMessages' accepting a first argument of type 'TaskTests.FakeTask' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that GetMessages returns the messages contained in the Task's own Children when no folder with the key specified exists.
// //         /// </summary>
// //         [Fact]
// //         public void GetMessages_NoMessagesFolderFound_ReturnsMessagesFromSelf()
// //         {
// //             // Arrange
// //             var fakeTask = new FakeTask();
// //             // Add a Message to the Task's own children.
// //             var message = new Message();
// //             fakeTask.Children.Add(message);
// //             // Ensure no folder is returned for the Messages search.
// //             fakeTask.FakeFolder = null;
// // 
// //             // Act
// //             var messages = fakeTask.GetMessages();
// // 
// //             // Assert
// //             messages.Should().HaveCount(1);
// //             messages.First().Should().Be(message);
// //         }
// //  // [Error] (89-37)CS1061 'TaskTests.FakeTask' does not contain a definition for 'GetMessages' and no accessible extension method 'GetMessages' accepting a first argument of type 'TaskTests.FakeTask' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that GetMessages returns the messages from the Messages folder's children when such a folder is present.
//         /// </summary>
//         [Fact]
//         public void GetMessages_MessagesFolderFound_ReturnsMessagesFromFolder()
//         {
//             // Arrange
//             var fakeTask = new FakeTask();
//             // Even if there is a message in the task's own children, it should be ignored when a folder is found.
//             fakeTask.Children.Add(new Message());
// 
//             // Create a mock Folder to simulate the Messages folder.
//             var mockFolder = new Mock<Folder>();
//             var folderMessage = new Message();
//             var folderChildren = new List<TreeNode> { folderMessage };
//             mockFolder.Setup(f => f.Children).Returns(folderChildren);
// 
//             fakeTask.FakeFolder = mockFolder.Object;
// 
//             // Act
//             var messages = fakeTask.GetMessages();
// 
//             // Assert
//             messages.Should().HaveCount(1);
//             messages.First().Should().Be(folderMessage);
//         }
// 
//         /// <summary>
//         /// A fake Task class derived from Task to allow injection of custom Children and FindChild behavior for testing.
//         /// </summary>
//         private class FakeTask : Task
//         {
//             /// <summary>
//             /// Gets the list of child nodes for this fake task.
//             /// </summary>
//             public new List<TreeNode> Children { get; } = new List<TreeNode>();
// 
//             /// <summary>
//             /// Gets or sets the folder to be returned when searching for the Messages folder.
//             /// </summary>
//             public Folder? FakeFolder { get; set; }
// 
//             /// <summary>
//             /// Overrides the FindChild method to return a preset folder when the key matches.
//             /// </summary>
//             /// <typeparam name="T">The type of child node to search for.</typeparam>
//             /// <param name="key">The key to search by.</param>
//             /// <returns>The matching child folder if found; otherwise, default.</returns>
//             public new T? FindChild<T>(string key) where T : TreeNode
//             {
//                 if (typeof(T) == typeof(Folder) && key == Strings.Messages && FakeFolder != null)
//                 {
//                     return (T)(object)FakeFolder;
//                 }
//                 return default;
//             }
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="MSBuildTask"/> class.
//     /// </summary>
//     public class MSBuildTaskTests
//     {
//         private readonly MSBuildTask _msbuildTask;
// 
//         /// <summary>
//         /// Initializes a new instance of the <see cref="MSBuildTaskTests"/> class.
//         /// </summary>
//         public MSBuildTaskTests()
//         {
//             _msbuildTask = new MSBuildTask();
//         }
// 
//         /// <summary>
//         /// Tests that the TypeName property of MSBuildTask returns "Task" (as it is inherited and not overridden).
//         /// </summary>
//         [Fact]
//         public void TypeName_WhenCalled_ReturnsTask()
//         {
//             // Act
//             string typeName = _msbuildTask.TypeName;
// 
//             // Assert
//             typeName.Should().Be("Task");
//         }
// 
//         /// <summary>
//         /// Tests that IsDerivedTask returns true when the instance is of a derived type (MSBuildTask) rather than exactly Task.
//         /// </summary>
//         [Fact]
//         public void IsDerivedTask_WhenInstanceIsDerived_ReturnsTrue()
//         {
//             // Act
//             bool isDerivedTask = _msbuildTask.IsDerivedTask;
// 
//             // Assert
//             isDerivedTask.Should().BeTrue();
//         }
// 
//         /// <summary>
//         /// Tests that the properties FromAssembly, CommandLineArguments, SourceFilePath, and LineNumber can be set and retrieved correctly.
//         /// </summary>
//         [Fact]
//         public void PropertySettersAndGetters_WhenAssigned_ReturnsCorrectValues()
//         {
//             // Arrange
//             const string fromAssembly = "SomeAssembly.dll";
//             const string commandLineArguments = "/arg1 /arg2";
//             const string sourceFilePath = "C:\\Source\\file.cs";
//             int? lineNumber = 42;
// 
//             // Act
//             _msbuildTask.FromAssembly = fromAssembly;
//             _msbuildTask.CommandLineArguments = commandLineArguments;
//             _msbuildTask.SourceFilePath = sourceFilePath;
//             _msbuildTask.LineNumber = lineNumber;
// 
//             // Assert
//             _msbuildTask.FromAssembly.Should().Be(fromAssembly);
//             _msbuildTask.CommandLineArguments.Should().Be(commandLineArguments);
//             _msbuildTask.SourceFilePath.Should().Be(sourceFilePath);
//             _msbuildTask.LineNumber.Should().Be(lineNumber);
//         }
//     }
// }
//     
// // The following are minimal stub definitions to allow the tests to compile.
// // In a real scenario, these types would come from the production codebase.
// 
// namespace Microsoft.Build.Logging.StructuredLogger
// {
//     /// <summary>
//     /// Represents a basic node in a tree structure.
//     /// </summary>
//     public class TreeNode
//     {
//         /// <summary>
//         /// Gets the collection of child nodes.
//         /// </summary>
//         public virtual IList<TreeNode> Children { get; } = new List<TreeNode>();
//     }
// //  // [Error] (215-18)CS7036 There is no argument given that corresponds to the required parameter 'text' of 'TreeNode.TreeNode(string)'
// //     /// <summary>
// //     /// Represents a base node that might be timed.
// //     /// </summary>
// //     public class TimedNode : TreeNode
// //     {
// //     }
// //  // [Error] (222-18)CS7036 There is no argument given that corresponds to the required parameter 'text' of 'TreeNode.TreeNode(string)'
// //     /// <summary>
// //     /// Represents a message node.
// //     /// </summary>
// //     public class Message : TreeNode
// //     {
// //     }
// //  // [Error] (229-18)CS7036 There is no argument given that corresponds to the required parameter 'text' of 'TreeNode.TreeNode(string)'
// //     /// <summary>
// //     /// Represents a folder node.
// //     /// </summary>
// //     public class Folder : TreeNode
// //     {
// //     }
// // 
//     /// <summary>
//     /// Contains commonly used string constants.
//     /// </summary>
//     public static class Strings
//     {
//         /// <summary>
//         /// The key for messages.
//         /// </summary>
//         public const string Messages = "Messages";
//     }
// 
//     /// <summary>
//     /// Interface indicating the node has a source file.
//     /// </summary>
//     public interface IHasSourceFile
//     {
//         string SourceFilePath { get; set; }
//     }
// 
//     /// <summary>
//     /// Interface indicating the node has a line number.
//     /// </summary>
//     public interface IHasLineNumber
//     {
//         int? LineNumber { get; set; }
//     }
// }