// using FluentAssertions;
// using Microsoft.Build.Logging;
// using Microsoft.Build.Logging.StructuredLogger;
// using Microsoft.Build.Logging.StructuredLogger.UnitTests;
// using Moq;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "Task"/> class.
//     /// </summary>
//     public class TaskTests
//     {
//         private readonly Task _task;
//         public TaskTests()
//         {
//             _task = new Task();
//         }
// //  // [Error] (31-37)CS1061 'Task' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the TypeName property returns "Task".
// //         /// </summary>
// //         [Fact]
// //         public void TypeName_WhenCalled_ReturnsTask()
// //         {
// //             // Act
// //             string typeName = _task.TypeName;
// //             // Assert
// //             typeName.Should().Be("Task");
// //         }
// //  // [Error] (43-36)CS1061 'Task' does not contain a definition for 'IsDerivedTask' and no accessible extension method 'IsDerivedTask' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the IsDerivedTask property returns false when the instance is of type Task.
//         /// </summary>
//         [Fact]
//         public void IsDerivedTask_WhenInstanceIsTask_ReturnsFalse()
//         {
//             // Act
//             bool isDerived = _task.IsDerivedTask;
//             // Assert
//             isDerived.Should().BeFalse();
//         }
// //  // [Error] (57-19)CS1061 'Task' does not contain a definition for 'FromAssembly' and no accessible extension method 'FromAssembly' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?) // [Error] (59-35)CS1061 'Task' does not contain a definition for 'FromAssembly' and no accessible extension method 'FromAssembly' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the FromAssembly property can be set and retrieved correctly.
// //         /// </summary>
// //         [Theory]
// //         [InlineData("")]
// //         [InlineData("MyAssembly")]
// //         public void FromAssembly_SetAndGet_ReturnsExpectedValue(string assemblyValue)
// //         {
// //             // Arrange
// //             _task.FromAssembly = assemblyValue;
// //             // Act
// //             string result = _task.FromAssembly;
// //             // Assert
// //             result.Should().Be(assemblyValue);
// //         }
// //  // [Error] (74-19)CS1061 'Task' does not contain a definition for 'CommandLineArguments' and no accessible extension method 'CommandLineArguments' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?) // [Error] (76-35)CS1061 'Task' does not contain a definition for 'CommandLineArguments' and no accessible extension method 'CommandLineArguments' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the CommandLineArguments property can be set and retrieved correctly.
//         /// </summary>
//         [Theory]
//         [InlineData("")]
//         [InlineData("arg1 arg2")]
//         [InlineData("--flag \"value with spaces\"")]
//         public void CommandLineArguments_SetAndGet_ReturnsExpectedValue(string arguments)
//         {
//             // Arrange
//             _task.CommandLineArguments = arguments;
//             // Act
//             string result = _task.CommandLineArguments;
//             // Assert
//             result.Should().Be(arguments);
//         }
// //  // [Error] (92-19)CS1061 'Task' does not contain a definition for 'SourceFilePath' and no accessible extension method 'SourceFilePath' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?) // [Error] (94-35)CS1061 'Task' does not contain a definition for 'SourceFilePath' and no accessible extension method 'SourceFilePath' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the SourceFilePath property can be set and retrieved correctly.
// //         /// </summary>
// //         [Theory]
// //         [InlineData("C:\\temp\\file.txt")]
// //         [InlineData("/usr/local/file.txt")]
// //         [InlineData("relative/path/file.txt")]
// //         [InlineData("invalid:\\path?*")]
// //         public void SourceFilePath_SetAndGet_ReturnsExpectedValue(string path)
// //         {
// //             // Arrange
// //             _task.SourceFilePath = path;
// //             // Act
// //             string result = _task.SourceFilePath;
// //             // Assert
// //             result.Should().Be(path);
// //         }
// //  // [Error] (111-19)CS1061 'Task' does not contain a definition for 'LineNumber' and no accessible extension method 'LineNumber' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?) // [Error] (113-33)CS1061 'Task' does not contain a definition for 'LineNumber' and no accessible extension method 'LineNumber' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the LineNumber property can be set and retrieved correctly.
//         /// </summary>
//         [Theory]
//         [InlineData(null)]
//         [InlineData(0)]
//         [InlineData(1)]
//         [InlineData(int.MaxValue)]
//         [InlineData(int.MinValue)]
//         public void LineNumber_SetAndGet_ReturnsExpectedValue(int? lineNumber)
//         {
//             // Arrange
//             _task.LineNumber = lineNumber;
//             // Act
//             int? result = _task.LineNumber;
//             // Assert
//             result.Should().Be(lineNumber);
//         }
// //  // [Error] (131-34)CS1061 'Task' does not contain a definition for 'GetMessages' and no accessible extension method 'GetMessages' accepting a first argument of type 'Task' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Partial test for the GetMessages method.
// //         /// Note: Due to the complexity in setting up the underlying TreeNode.Children collection and the FindChild behavior,
// //         /// further configuration is required to fully test GetMessages. This test serves as a scaffold.
// //         /// </summary>
// //         [Fact(Skip = "TODO: Setup proper test conditions for GetMessages including configuring the Children collection and FindChild behavior.")]
// //         public void GetMessages_WhenCalled_ReturnsExpectedMessages()
// //         {
// //             // Arrange
// //             // TODO: Create a test-specific subclass or set up the internal Children collection of the Task instance
// //             // with Message instances, and an optional Folder node that holds Message instances.
// //             // This may require exposing or constructing a test double for the TreeNode/TimedNode behavior.
// //             // Act
// //             var messages = _task.GetMessages();
// //             // Assert
// //             // TODO: Validate that the returned messages collection contains the expected Message instances.
// //             messages.Should().NotBeNull();
// //         }
// //     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref = "MSBuildTask"/> class.
//     /// </summary>
//     public class MSBuildTaskTests
//     {
//         private readonly MSBuildTask _msBuildTask;
//         public MSBuildTaskTests()
//         {
//             _msBuildTask = new MSBuildTask();
//         }
// 
//         /// <summary>
//         /// Tests that the IsDerivedTask property returns true when the instance is derived from Task.
//         /// </summary>
//         [Fact]
//         public void IsDerivedTask_WhenInstanceIsDerived_ReturnsTrue()
//         {
//             // Act
//             bool isDerived = _msBuildTask.IsDerivedTask;
//             // Assert
//             isDerived.Should().BeTrue();
//         }
//     }
// }