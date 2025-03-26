// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using Moq;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.IO.Compression;
// using System.Threading;
// using System.Threading.Tasks;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "Build"/> class.
//     /// </summary>
//     public class BuildTests
//     {
//         /// <summary>
//         /// Tests that setting the MSBuildVersion property with quotes is trimmed and parsed correctly.
//         /// </summary>
// //         [Fact] [Error] (26-29)CS7036 There is no argument given that corresponds to the required parameter 'tasks' of 'Build.Build(List<FakeTask>)' [Error] (28-19)CS1061 'Build' does not contain a definition for 'MSBuildVersion' and no accessible extension method 'MSBuildVersion' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (30-31)CS1061 'Build' does not contain a definition for 'IsMSBuildVersionAtLeast' and no accessible extension method 'IsMSBuildVersionAtLeast' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (31-32)CS1061 'Build' does not contain a definition for 'IsMSBuildVersionAtLeast' and no accessible extension method 'IsMSBuildVersionAtLeast' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         public void MSBuildVersion_SetWithQuotes_SetsVersionCorrectly()
// //         {
// //             // Arrange
// //             var build = new Build();
// //             string versionWithQuotes = "\"15.3.0\"";
// //             build.MSBuildVersion = versionWithQuotes;
// //             // Act & Assert
// //             Assert.True(build.IsMSBuildVersionAtLeast(15, 3), "Expected version to be at least 15.3");
// //             Assert.False(build.IsMSBuildVersionAtLeast(15, 4), "Expected version not to be at least 15.4");
// //         }
// 
//         /// <summary>
//         /// Tests that setting the MSBuildExecutablePath property trims surrounding quotes.
//         /// </summary>
// //         [Fact] [Error] (41-29)CS7036 There is no argument given that corresponds to the required parameter 'tasks' of 'Build.Build(List<FakeTask>)' [Error] (44-19)CS1061 'Build' does not contain a definition for 'MSBuildExecutablePath' and no accessible extension method 'MSBuildExecutablePath' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (46-52)CS1061 'Build' does not contain a definition for 'MSBuildExecutablePath' and no accessible extension method 'MSBuildExecutablePath' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         public void MSBuildExecutablePath_SetWithQuotes_TrimsQuotes()
// //         {
// //             // Arrange
// //             var build = new Build();
// //             string pathWithQuotes = "\"/usr/bin/msbuild\"";
// //             // Act
// //             build.MSBuildExecutablePath = pathWithQuotes;
// //             // Assert
// //             Assert.Equal("/usr/bin/msbuild", build.MSBuildExecutablePath);
// //         }
// 
//         /// <summary>
//         /// Tests that accessing the SourceFiles property triggers extraction from the SourceFilesArchive and then clears the archive.
//         /// </summary>
// //         [Fact] [Error] (56-29)CS7036 There is no argument given that corresponds to the required parameter 'tasks' of 'Build.Build(List<FakeTask>)' [Error] (58-19)CS1061 'Build' does not contain a definition for 'SourceFilesArchive' and no accessible extension method 'SourceFilesArchive' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (60-37)CS1061 'Build' does not contain a definition for 'SourceFiles' and no accessible extension method 'SourceFiles' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (64-31)CS1061 'Build' does not contain a definition for 'SourceFilesArchive' and no accessible extension method 'SourceFilesArchive' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         public void SourceFilesProperty_ReadsFromArchiveAndClearsArchive()
// //         {
// //             // Arrange
// //             var build = new Build();
// //             byte[] zipArchiveBytes = CreateZipArchive(new Dictionary<string, string> { { "file1.txt", "Content of file1" } });
// //             build.SourceFilesArchive = zipArchiveBytes;
// //             // Act
// //             var sourceFiles = build.SourceFiles;
// //             // Assert
// //             Assert.NotNull(sourceFiles);
// //             Assert.NotEmpty(sourceFiles);
// //             Assert.Null(build.SourceFilesArchive);
// //         }
// 
//         /// <summary>
//         /// Tests that ReadSourceFiles(Stream) returns archive files when provided with a valid zip stream.
//         /// </summary>
// //         [Fact] [Error] (77-32)CS0117 'Build' does not contain a definition for 'ReadSourceFiles'
// //         public void ReadSourceFiles_Stream_ValidZip_ReturnsArchiveFiles()
// //         {
// //             // Arrange
// //             byte[] zipArchiveBytes = CreateZipArchive(new Dictionary<string, string> { { "test.txt", "Test content" } });
// //             using var stream = new MemoryStream(zipArchiveBytes);
// //             // Act
// //             var result = Build.ReadSourceFiles(stream);
// //             // Assert
// //             Assert.NotNull(result);
// //             Assert.Single(result);
// //         }
// 
//         /// <summary>
//         /// Tests that ReadSourceFiles(Stream) returns an empty list when provided with an invalid (corrupt) zip stream.
//         /// </summary>
// //         [Fact] [Error] (101-32)CS0117 'Build' does not contain a definition for 'ReadSourceFiles'
// //         public void ReadSourceFiles_Stream_InvalidZip_ReturnsEmptyList()
// //         {
// //             // Arrange
// //             byte[] invalidData = new byte[]
// //             {
// //                 0,
// //                 1,
// //                 2,
// //                 3,
// //                 4,
// //                 5
// //             };
// //             using var stream = new MemoryStream(invalidData);
// //             // Act
// //             var result = Build.ReadSourceFiles(stream);
// //             // Assert
// //             Assert.NotNull(result);
// //             Assert.Empty(result);
// //         }
// 
//         /// <summary>
//         /// Tests that ReadSourceFiles(byte[]) returns archive files when provided with valid zip archive bytes.
//         /// </summary>
// //         [Fact] [Error] (116-32)CS0117 'Build' does not contain a definition for 'ReadSourceFiles'
// //         public void ReadSourceFiles_ByteArray_ValidZip_ReturnsArchiveFiles()
// //         {
// //             // Arrange
// //             byte[] zipArchiveBytes = CreateZipArchive(new Dictionary<string, string> { { "file2.txt", "File2 content" } });
// //             // Act
// //             var result = Build.ReadSourceFiles(zipArchiveBytes);
// //             // Assert
// //             Assert.NotNull(result);
// //             Assert.Single(result);
// //         }
// 
//         /// <summary>
//         /// Tests that ReadSourceFiles(string) returns archive files when provided with a valid zip file path.
//         /// </summary>
// //         [Fact] [Error] (135-36)CS0117 'Build' does not contain a definition for 'ReadSourceFiles'
// //         public void ReadSourceFiles_String_ValidZip_ReturnsArchiveFiles()
// //         {
// //             // Arrange
// //             byte[] zipArchiveBytes = CreateZipArchive(new Dictionary<string, string> { { "file3.txt", "File3 content" } });
// //             string tempFilePath = Path.GetTempFileName();
// //             File.WriteAllBytes(tempFilePath, zipArchiveBytes);
// //             try
// //             {
// //                 // Act
// //                 var result = Build.ReadSourceFiles(tempFilePath);
// //                 // Assert
// //                 Assert.NotNull(result);
// //                 Assert.Single(result);
// //             }
// //             finally
// //             {
// //                 File.Delete(tempFilePath);
// //             }
// //         }
// 
//         /// <summary>
//         /// Tests that when the IgnoreEmbeddedFiles property is set, entries containing the ignore substring are omitted.
//         /// </summary>
// //         [Fact] [Error] (154-19)CS0117 'Build' does not contain a definition for 'IgnoreEmbeddedFiles' [Error] (158-32)CS0117 'Build' does not contain a definition for 'ReadSourceFiles' [Error] (164-19)CS0117 'Build' does not contain a definition for 'IgnoreEmbeddedFiles'
// //         public void ReadSourceFiles_Stream_WithIgnoreEmbeddedFiles_IgnoresMatchingEntries()
// //         {
// //             // Arrange
// //             string ignoreToken = "ignore";
// //             Build.IgnoreEmbeddedFiles = ignoreToken;
// //             byte[] zipArchiveBytes = CreateZipArchive(new Dictionary<string, string> { { "ignore_this.txt", "Should be ignored" }, { "include_this.txt", "Should be included" } });
// //             using var stream = new MemoryStream(zipArchiveBytes);
// //             // Act
// //             var result = Build.ReadSourceFiles(stream);
// //             // Assert
// //             // Only one file that does not contain the ignore token in its name should be included.
// //             Assert.NotNull(result);
// //             Assert.Single(result);
// //             // Cleanup
// //             Build.IgnoreEmbeddedFiles = null;
// //         }
// 
//         /// <summary>
//         /// Tests that RegisterTask does not add a task when its FromAssembly property is not a string.
//         /// </summary>
// //         [Fact] [Error] (174-29)CS7036 There is no argument given that corresponds to the required parameter 'tasks' of 'Build.Build(List<FakeTask>)' [Error] (177-19)CS1061 'Build' does not contain a definition for 'RegisterTask' and no accessible extension method 'RegisterTask' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (179-32)CS1061 'Build' does not contain a definition for 'TaskAssemblies' and no accessible extension method 'TaskAssemblies' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         public void RegisterTask_TaskWithNullFromAssembly_DoesNotAddTask()
// //         {
// //             // Arrange
// //             var build = new Build();
// //             var fakeTask = new FakeTask(null, "TaskWithNullAssembly");
// //             // Act
// //             build.RegisterTask(fakeTask);
// //             // Assert
// //             Assert.Empty(build.TaskAssemblies);
// //         }
// 
//         /// <summary>
//         /// Tests that RegisterTask adds the task name under the correct assembly key.
//         /// </summary>
// //         [Fact] [Error] (189-29)CS7036 There is no argument given that corresponds to the required parameter 'tasks' of 'Build.Build(List<FakeTask>)' [Error] (192-19)CS1061 'Build' does not contain a definition for 'RegisterTask' and no accessible extension method 'RegisterTask' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (193-19)CS1061 'Build' does not contain a definition for 'RegisterTask' and no accessible extension method 'RegisterTask' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (195-33)CS1061 'Build' does not contain a definition for 'TaskAssemblies' and no accessible extension method 'TaskAssemblies' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (196-31)CS1061 'Build' does not contain a definition for 'TaskAssemblies' and no accessible extension method 'TaskAssemblies' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (197-48)CS1061 'Build' does not contain a definition for 'TaskAssemblies' and no accessible extension method 'TaskAssemblies' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (198-48)CS1061 'Build' does not contain a definition for 'TaskAssemblies' and no accessible extension method 'TaskAssemblies' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         public void RegisterTask_ValidTask_AddsTaskNameToAssembly()
// //         {
// //             // Arrange
// //             var build = new Build();
// //             var fakeTask = new FakeTask("assembly.dll", "TaskName1");
// //             // Act
// //             build.RegisterTask(fakeTask);
// //             build.RegisterTask(new FakeTask("assembly.dll", "TaskName2"));
// //             // Assert
// //             Assert.Single(build.TaskAssemblies);
// //             Assert.True(build.TaskAssemblies.ContainsKey("assembly.dll"));
// //             Assert.Contains("TaskName1", build.TaskAssemblies["assembly.dll"]);
// //             Assert.Contains("TaskName2", build.TaskAssemblies["assembly.dll"]);
// //         }
// 
//         /// <summary>
//         /// Tests that the TypeName property returns the correct class name.
//         /// </summary>
// //         [Fact] [Error] (208-29)CS7036 There is no argument given that corresponds to the required parameter 'tasks' of 'Build.Build(List<FakeTask>)' [Error] (210-34)CS1061 'Build' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         public void TypeName_ReturnsBuild()
// //         {
// //             // Arrange
// //             var build = new Build();
// //             // Act
// //             var typeName = build.TypeName;
// //             // Assert
// //             Assert.Equal("Build", typeName);
// //         }
// 
//         /// <summary>
//         /// Tests that the ToString method returns a string indicating build success and duration.
//         /// </summary>
// //         [Fact] [Error] (222-29)CS7036 There is no argument given that corresponds to the required parameter 'tasks' of 'Build.Build(List<FakeTask>)' [Error] (223-19)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         public void ToString_ReturnsCorrectFormat()
// //         {
// //             // Arrange
// //             var build = new Build();
// //             build.Succeeded = true;
// //             // Simulate a duration text; assuming base TimedNode provides DurationText.
// //             // Since we cannot set DurationText directly, we check for presence of "succeeded".
// //             // Act
// //             var str = build.ToString();
// //             // Assert
// //             Assert.Contains("Build succeeded", str);
// //             Assert.Contains("Duration:", str);
// //         }
// 
//         /// <summary>
//         /// Tests that FindDescendant returns the expected child node when a descendant exists.
//         /// </summary>
// //         [Fact] [Error] (240-29)CS7036 There is no argument given that corresponds to the required parameter 'tasks' of 'Build.Build(List<FakeTask>)' [Error] (242-36)CS1061 'Build' does not contain a definition for 'EvaluationFolder' and no accessible extension method 'EvaluationFolder' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (244-36)CS1061 'Build' does not contain a definition for 'FindDescendant' and no accessible extension method 'FindDescendant' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         public void FindDescendant_WhenChildExists_ReturnsChild()
// //         {
// //             // Arrange
// //             var build = new Build();
// //             // Force creation of an evaluation folder which is added as a child.
// //             var evalFolder = build.EvaluationFolder;
// //             // Act
// //             var descendant = build.FindDescendant(0);
// //             // Assert
// //             Assert.NotNull(descendant);
// //             Assert.Equal(evalFolder, descendant);
// //         }
// 
//         /// <summary>
//         /// Tests that FindDescendant returns null when the specified index is out of range.
//         /// </summary>
// //         [Fact] [Error] (257-29)CS7036 There is no argument given that corresponds to the required parameter 'tasks' of 'Build.Build(List<FakeTask>)' [Error] (259-23)CS1061 'Build' does not contain a definition for 'EvaluationFolder' and no accessible extension method 'EvaluationFolder' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (261-36)CS1061 'Build' does not contain a definition for 'FindDescendant' and no accessible extension method 'FindDescendant' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         public void FindDescendant_WhenIndexOutOfRange_ReturnsNull()
// //         {
// //             // Arrange
// //             var build = new Build();
// //             // Ensure at least one child exists.
// //             _ = build.EvaluationFolder;
// //             // Act
// //             var descendant = build.FindDescendant(100);
// //             // Assert
// //             Assert.Null(descendant);
// //         }
// 
//         /// <summary>
//         /// Tests that FindEvaluation returns null when no evaluation with the specified id exists.
//         /// </summary>
// //         [Fact] [Error] (273-29)CS7036 There is no argument given that corresponds to the required parameter 'tasks' of 'Build.Build(List<FakeTask>)' [Error] (275-36)CS1061 'Build' does not contain a definition for 'FindEvaluation' and no accessible extension method 'FindEvaluation' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         public void FindEvaluation_WhenNotFound_ReturnsNull()
// //         {
// //             // Arrange
// //             var build = new Build();
// //             // Act
// //             var evaluation = build.FindEvaluation(999);
// //             // Assert
// //             Assert.Null(evaluation);
// //         }
// 
//         /// <summary>
//         /// Tests that RunInBackground executes the provided action and WaitForBackgroundTasks waits for its completion.
//         /// </summary>
// //         [Fact] [Error] (287-29)CS7036 There is no argument given that corresponds to the required parameter 'tasks' of 'Build.Build(List<FakeTask>)' [Error] (296-19)CS1061 'Build' does not contain a definition for 'RunInBackground' and no accessible extension method 'RunInBackground' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (297-19)CS1061 'Build' does not contain a definition for 'WaitForBackgroundTasks' and no accessible extension method 'WaitForBackgroundTasks' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         public void RunInBackground_ActionExecuted()
// //         {
// //             // Arrange
// //             var build = new Build();
// //             bool actionExecuted = false;
// //             Action action = () =>
// //             {
// //                 // Simulate some work.
// //                 Thread.Sleep(50);
// //                 actionExecuted = true;
// //             };
// //             // Act
// //             build.RunInBackground(action);
// //             build.WaitForBackgroundTasks();
// //             // Assert
// //             Assert.True(actionExecuted, "The background action was not executed as expected.");
// //         }
// 
//         /// <summary>
//         /// Tests that WaitForBackgroundTasks handles the scenario when no background tasks have been scheduled.
//         /// </summary>
// //         [Fact] [Error] (309-29)CS7036 There is no argument given that corresponds to the required parameter 'tasks' of 'Build.Build(List<FakeTask>)' [Error] (311-36)CS0117 'Record' does not contain a definition for 'Exception'
// //         public void WaitForBackgroundTasks_NoTasks_DoesNotThrow()
// //         {
// //             // Arrange
// //             var build = new Build();
// //             // Act & Assert
// //             var exception = Record.Exception(() => build.WaitForBackgroundTasks());
// //             Assert.Null(exception);
// //         }
// 
//         /// <summary>
//         /// Creates a zip archive in memory with the specified entries.
//         /// </summary>
//         /// <param name = "entries">Dictionary mapping entry names to their string content.</param>
//         /// <returns>Byte array representing the zip archive.</returns>
// //         private static byte[] CreateZipArchive(Dictionary<string, string> entries) [Error] (323-38)CS1069 The type name 'ZipArchive' could not be found in the namespace 'System.IO.Compression'. This type has been forwarded to assembly 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' Consider adding a reference to that assembly. [Error] (323-60)CS0103 The name 'ZipArchiveMode' does not exist in the current context
// //         {
// //             using var memStream = new MemoryStream();
// //             using (var archive = new ZipArchive(memStream, ZipArchiveMode.Create, true))
// //             {
// //                 foreach (var kvp in entries)
// //                 {
// //                     var entry = archive.CreateEntry(kvp.Key);
// //                     using var entryStream = entry.Open();
// //                     using var writer = new StreamWriter(entryStream);
// //                     writer.Write(kvp.Value);
// //                 }
// //             }
// // 
// //             return memStream.ToArray();
// //         }
// 
//         /// <summary>
//         /// A fake implementation of the Task class used for testing RegisterTask.
//         /// Assumes that the real Task class has a public property FromAssembly and Name.
//         /// </summary>
//         private class FakeTask : Task
//         {
// //             public override string Name { get; set; } [Error] (343-36)CS0506 'BuildTests.FakeTask.Name': cannot override inherited member 'FakeNode.Name' because it is not marked virtual, abstract, or override
// //             public override object FromAssembly { get; set; } [Error] (344-36)CS0115 'BuildTests.FakeTask.FromAssembly': no suitable method found to override
// 
//             public FakeTask(object fromAssembly, string name)
//             {
//                 FromAssembly = fromAssembly;
//                 Name = name;
//             }
//         }
//     }
// }
