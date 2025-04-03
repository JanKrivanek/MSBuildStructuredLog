// using System;
// using System.Collections.Generic;
// using System.IO;
// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using Moq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="CompilerInvocation"/> class.
//     /// </summary>
//     public class CompilerInvocationTests
//     {
//         /// <summary>
//         /// Tests that the ProjectDirectory property returns an empty string when ProjectFilePath is null.
//         /// </summary>
//         [Fact]
//         public void ProjectDirectory_WhenProjectFilePathIsNull_ReturnsEmptyString()
//         {
//             // Arrange
//             var invocation = new CompilerInvocation
//             {
//                 ProjectFilePath = null,
//                 Language = "C#",
//                 CommandLineArguments = "/optimize"
//             };
// 
//             // Act
//             string result = invocation.ProjectDirectory;
// 
//             // Assert
//             result.Should().Be(string.Empty);
//         }
// 
//         /// <summary>
//         /// Tests that the ProjectDirectory property returns the correct directory name when ProjectFilePath is set.
//         /// </summary>
//         [Fact]
//         public void ProjectDirectory_WhenProjectFilePathIsSet_ReturnsDirectoryName()
//         {
//             // Arrange
//             string fakePath = Path.Combine("C:", "Projects", "MyProject", "project.csproj");
//             var invocation = new CompilerInvocation
//             {
//                 ProjectFilePath = fakePath,
//                 Language = "C#",
//                 CommandLineArguments = "/optimize"
//             };
// 
//             // Act
//             string result = invocation.ProjectDirectory;
// 
//             // Assert
//             result.Should().Be(Path.GetDirectoryName(fakePath));
//         }
// 
//         /// <summary>
//         /// Tests that the overridden ToString method returns the expected formatted string.
//         /// </summary>
//         [Fact]
//         public void ToString_WhenCalled_ReturnsFormattedString()
//         {
//             // Arrange
//             var invocation = new CompilerInvocation
//             {
//                 ProjectFilePath = "project.csproj",
//                 Language = "C#",
//                 CommandLineArguments = "/debug+ /optimize"
//             };
// 
//             // Act
//             string result = invocation.ToString();
// 
//             // Assert
//             result.Should().Be("project.csproj (C#): /debug+ /optimize");
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="CompilerInvocationsReader"/> class.
//     /// </summary>
//     public class CompilerInvocationsReaderTests
//     {
//         private readonly CompilerInvocationsReader _reader;
// 
//         public CompilerInvocationsReaderTests()
//         {
//             _reader = new CompilerInvocationsReader();
//         }
// 
//         /// <summary>
//         /// Tests that the Read(string) method throws a FileNotFoundException when the provided file path does not exist.
//         /// </summary>
//         [Fact]
//         public void Read_StringFilePath_FileDoesNotExist_ThrowsFileNotFoundException()
//         {
//             // Arrange
//             string nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".binlog");
// 
//             // Act
//             Action act = () => _reader.Read(nonExistentFile);
// 
//             // Assert
//             act.Should().Throw<FileNotFoundException>().WithMessage(nonExistentFile);
//         }
// 
//         /// <summary>
//         /// Integration test placeholder for Read(string) method when the file extension is ".buildlog".
//         /// This requires an actual build log file and integration with Serialization.Read.
//         /// </summary>
//         [Fact(Skip = "Integration test required to simulate .buildlog file reading and Serialization.Read call.")]
//         public void Read_StringFilePath_WithBuildLogExtension_CallsReadBuildLogFormat()
//         {
//             // TODO: Implement integration test for .buildlog file format.
//         }
// 
//         /// <summary>
//         /// Partial test for Read(Build) method.
//         /// Cannot fully test without a concrete Build instance with Task children.
//         /// </summary>
//         [Fact(Skip = "Requires a concrete Build instance with task children. Implement integration test.")]
//         public void Read_Build_ReturnsCompilerInvocations()
//         {
//             // TODO: Create a valid Build instance with appropriate Task children.
//             // Act
//             // var result = _reader.Read(build);
//             
//             // Assert
//             // result.Should().NotBeNull();
//         }
// 
//         /// <summary>
//         /// Tests that TryGetInvocationFromRecord returns null when the BuildEventArgs has a negative target id.
//         /// </summary>
//         [Fact]
//         public void TryGetInvocationFromRecord_WhenTargetIdIsNegative_ReturnsNull()
//         {
//             // Arrange
//             var args = new CustomBuildEventArgs(null);
//             var taskMap = new Dictionary<(int, int), CompilerInvocation>();
// 
//             // Act
//             var result = _reader.TryGetInvocationFromRecord(args, taskMap);
// 
//             // Assert
//             result.Should().BeNull();
//         }
// 
//         /// <summary>
//         /// Tests that when a TargetStartedEventArgs for "CoreCompile" is provided, an invocation is added to the dictionary and null is returned.
//         /// </summary>
//         [Fact]
//         public void TryGetInvocationFromRecord_WhenTargetStartedEventArgsForCoreCompile_AddsInvocationAndReturnsNull()
//         {
//             // Arrange
//             var buildEventContext = new CustomBuildEventContext(1, 1);
//             var targetStarted = new CustomTargetStartedEventArgs("CoreCompile", "project.csproj", buildEventContext);
//             var taskMap = new Dictionary<(int, int), CompilerInvocation>();
// 
//             // Act
//             var result = _reader.TryGetInvocationFromRecord(targetStarted, taskMap);
// 
//             // Assert
//             result.Should().BeNull();
//             taskMap.Should().ContainKey((1, 1));
//         }
// 
//         /// <summary>
//         /// Tests that when a TaskCommandLineEventArgs with a valid command line is provided, the previously stored invocation is updated and returned.
//         /// </summary>
//         [Fact]
//         public void TryGetInvocationFromRecord_WhenTaskCommandLineEventArgsWithValidCommandLine_ReturnsUpdatedInvocation()
//         {
//             // Arrange
//             var buildEventContext = new CustomBuildEventContext(2, 2);
//             // Simulate CoreCompile event to create the invocation entry.
//             var targetStarted = new CustomTargetStartedEventArgs("CoreCompile", "project.csproj", buildEventContext);
//             var taskMap = new Dictionary<(int, int), CompilerInvocation>();
//             _reader.TryGetInvocationFromRecord(targetStarted, taskMap);
//             taskMap.ContainsKey((2, 2)).Should().BeTrue();
// 
//             // Prepare TaskCommandLineEventArgs with a command line that includes the compiler executable.
//             string originalCommandLine = "csc.exe /optimize /warn:4";
//             var taskCommandLine = new CustomTaskCommandLineEventArgs("Csc", originalCommandLine, buildEventContext);
// 
//             // Act
//             var result = _reader.TryGetInvocationFromRecord(taskCommandLine, taskMap);
// 
//             // Assert
//             result.Should().NotBeNull();
//             result.Language.Should().Be(CompilerInvocation.CSharp);
//             // Calculate expected trimmed command line.
//             int index = originalCommandLine.IndexOf("csc.exe ", StringComparison.OrdinalIgnoreCase);
//             string expectedCommandLine = (index >= 0) ? originalCommandLine.Substring(index + 8) : originalCommandLine;
//             result.CommandLineArguments.Should().Be(expectedCommandLine);
//             taskMap.Should().NotContainKey((2, 2));
//         }
// 
//         /// <summary>
//         /// Tests that GetCommandLineFromEventArgs returns null when the language cannot be determined from the task name.
//         /// </summary>
//         [Fact]
//         public void GetCommandLineFromEventArgs_WhenLanguageIsNull_ReturnsNull()
//         {
//             // Arrange
//             var buildEventContext = new CustomBuildEventContext(3, 3);
//             var taskCommandLine = new CustomTaskCommandLineEventArgs("UnknownTask", "any command line", buildEventContext);
// 
//             // Act
//             string commandLine = _reader.GetCommandLineFromEventArgs(taskCommandLine, out string language);
// 
//             // Assert
//             commandLine.Should().BeNull();
//             language.Should().BeNull();
//         }
// 
//         /// <summary>
//         /// Tests that GetCommandLineFromEventArgs correctly trims the compiler executable from the command line and returns the appropriate language.
//         /// </summary>
//         [Fact]
//         public void GetCommandLineFromEventArgs_WhenValidTask_ReturnsTrimmedCommandLineAndLanguage()
//         {
//             // Arrange
//             var buildEventContext = new CustomBuildEventContext(4, 4);
//             string originalCommandLine = "csc.exe /debug+ /optimize";
//             var taskCommandLine = new CustomTaskCommandLineEventArgs("Csc", originalCommandLine, buildEventContext);
// 
//             // Act
//             string commandLine = _reader.GetCommandLineFromEventArgs(taskCommandLine, out string language);
// 
//             // Assert
//             language.Should().Be(CompilerInvocation.CSharp);
//             int index = originalCommandLine.IndexOf("csc.exe ", StringComparison.OrdinalIgnoreCase);
//             string expectedCommandLine = (index >= 0) ? originalCommandLine.Substring(index + 8) : originalCommandLine;
//             commandLine.Should().Be(expectedCommandLine);
//         }
// 
//         /// <summary>
//         /// Partial test for TryGetInvocationFromTask method.
//         /// Cannot fully test without a concrete Task object with proper Parent setup.
//         /// </summary>
//         [Fact(Skip = "Requires concrete Task instance with a CoreCompile parent Target. Implement integration test.")]
//         public void TryGetInvocationFromTask_WhenCalled_ReturnsCompilerInvocation()
//         {
//             // TODO: Create a valid Task instance with required properties and Parent set to a Target named "CoreCompile".
//             // Act
//             // var result = _reader.TryGetInvocationFromTask(task);
//             // Assert
//             // result.Should().NotBeNull();
//         }
// 
//         /// <summary>
//         /// Tests that TrimCompilerExeFromCommandLine removes the compiler executable substring for various scenarios.
//         /// </summary>
//         [Theory]
//         [InlineData("csc.exe /a /b", "C#", " /a /b")]
//         [InlineData("csc.dll /a /b", "C#", " /a /b")]
//         [InlineData("csc.exe\" /a /b", "C#", " /a /b")]
//         [InlineData("csc.dll\" /a /b", "C#", " /a /b")]
//         [InlineData("other /a /b", "C#", "other /a /b")]
//         [InlineData("vbc.exe /option", "Visual Basic", " /option")]
//         public void TrimCompilerExeFromCommandLine_ReturnsTrimmedCommandLine(string input, string language, string expected)
//         {
//             // Act
//             string result = CompilerInvocationsReader.TrimCompilerExeFromCommandLine(input, language);
// 
//             // Assert
//             result.Should().Be(expected);
//         }
// 
//         /// <summary>
//         /// Tests that GetLanguageFromTaskName returns the correct language based on known task names.
//         /// </summary>
//         [Theory]
//         [InlineData("Csc", CompilerInvocation.CSharp)]
//         [InlineData("Vbc", CompilerInvocation.VisualBasic)]
//         [InlineData("Fsc", CompilerInvocation.FSharp)]
//         [InlineData("Tsc", CompilerInvocation.TypeScript)]
//         [InlineData("Unknown", null)]
//         public void GetLanguageFromTaskName_ReturnsExpectedLanguage(string taskName, string expectedLanguage)
//         {
//             // Act
//             string result = CompilerInvocationsReader.GetLanguageFromTaskName(taskName);
// 
//             // Assert
//             result.Should().Be(expectedLanguage);
//         }
//     }
// //  // [Error] (304-15)CS1729 'BuildEventContext' does not contain a constructor that takes 1 arguments
// //     #region Custom Test Classes for BuildEventArgs
// // 
// //     /// <summary>
// //     /// Custom implementation of BuildEventContext for testing purposes.
// //     /// </summary>
// //     public class CustomBuildEventContext : BuildEventContext
// //     {
// //         private readonly int _targetId;
// //         private readonly int _projectInstanceId;
// // 
// //         public CustomBuildEventContext(int targetId, int projectInstanceId)
// //             : base(0)
// //         {
// //             _targetId = targetId;
// //             _projectInstanceId = projectInstanceId;
// //         }
// //  // [Error] (310-29)CS0506 'CustomBuildEventContext.TargetId': cannot override inherited member 'BuildEventContext.TargetId' because it is not marked virtual, abstract, or override
// //         public override int TargetId => _targetId;
// //         public override int ProjectInstanceId => _projectInstanceId; // [Error] (311-29)CS0506 'CustomBuildEventContext.ProjectInstanceId': cannot override inherited member 'BuildEventContext.ProjectInstanceId' because it is not marked virtual, abstract, or override
//     }
// //  // [Error] (320-15)CS1729 'TargetStartedEventArgs' does not contain a constructor that takes 4 arguments
// //     /// <summary>
// //     /// Custom implementation of TargetStartedEventArgs for testing purposes.
// //     /// </summary>
// //     public class CustomTargetStartedEventArgs : TargetStartedEventArgs
// //     {
// //         public CustomTargetStartedEventArgs(string targetName, string projectFile, BuildEventContext buildEventContext)
// //             : base(targetName, projectFile, null, buildEventContext)
// //         {
// //         }
// //     }
// //  // [Error] (331-43)CS1503 Argument 3: cannot convert from '<null>' to 'Microsoft.Build.Framework.MessageImportance' // [Error] (331-49)CS1503 Argument 4: cannot convert from 'Microsoft.Build.Framework.BuildEventContext' to 'System.DateTime'
//     /// <summary>
//     /// Custom implementation of TaskCommandLineEventArgs for testing purposes.
//     /// </summary>
//     public class CustomTaskCommandLineEventArgs : TaskCommandLineEventArgs
//     {
//         public CustomTaskCommandLineEventArgs(string taskName, string commandLine, BuildEventContext buildEventContext)
//             : base(taskName, commandLine, null, buildEventContext)
//         {
//         }
//     }
// //  // [Error] (342-20)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Framework.BuildEventContext' to 'string?'
// //     /// <summary>
// //     /// Custom implementation of BuildEventArgs for testing purposes.
// //     /// </summary>
// //     public class CustomBuildEventArgs : BuildEventArgs
// //     {
// //         public CustomBuildEventArgs(BuildEventContext? buildEventContext)
// //             : base(buildEventContext, string.Empty, string.Empty)
// //         {
// //         }
// //     }
// // 
//     #endregion
// }