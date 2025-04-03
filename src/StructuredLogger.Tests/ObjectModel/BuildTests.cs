// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using Microsoft.Build.Logging.StructuredLogger.UnitTests;
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
//         private readonly Build _build;
//         public BuildTests()
//         {
//             _build = new Build();
//         }
// //  // [Error] (40-20)CS1061 'Build' does not contain a definition for 'MSBuildVersion' and no accessible extension method 'MSBuildVersion' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (42-34)CS1061 'Build' does not contain a definition for 'IsMSBuildVersionAtLeast' and no accessible extension method 'IsMSBuildVersionAtLeast' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// // #region MSBuildVersion Tests
// //         /// <summary>
// //         /// Tests that setting a valid MSBuildVersion string properly parses the version and returns true for matching minimum version.
// //         /// </summary>
// //         /// <param name = "versionString">The MSBuildVersion string to set.</param>
// //         /// <param name = "expectedMajor">Expected major version.</param>
// //         /// <param name = "expectedMinor">Expected minor version.</param>
// //         [Theory]
// //         [InlineData("12.3", 12, 3)]
// //         [InlineData("\"15.0\"", 15, 0)]
// //         public void MSBuildVersion_SetValidVersion_ParsesCorrectly(string versionString, int expectedMajor, int expectedMinor)
// //         {
// //             // Arrange
// //             _build.MSBuildVersion = versionString;
// //             // Act
// //             bool result = _build.IsMSBuildVersionAtLeast(expectedMajor, expectedMinor);
// //             // Assert
// //             result.Should().BeTrue("because the MSBuildVersion was set to a valid version string and should meet the minimum version requirement");
// //         }
// //  // [Error] (57-20)CS1061 'Build' does not contain a definition for 'MSBuildVersion' and no accessible extension method 'MSBuildVersion' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (59-34)CS1061 'Build' does not contain a definition for 'IsMSBuildVersionAtLeast' and no accessible extension method 'IsMSBuildVersionAtLeast' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that setting an invalid MSBuildVersion string results in version remaining unparsed.
//         /// </summary>
//         /// <param name = "versionString">An invalid version string.</param>
//         [Theory]
//         [InlineData("invalid")]
//         [InlineData("1")]
//         public void MSBuildVersion_SetInvalidVersion_DoesNotParse(string versionString)
//         {
//             // Arrange
//             _build.MSBuildVersion = versionString;
//             // Act
//             bool result = _build.IsMSBuildVersionAtLeast(0, 0);
//             // Assert
//             result.Should().BeFalse("because an invalid MSBuildVersion string should not be parsed into a valid version");
//         }
// //  // [Error] (74-20)CS1061 'Build' does not contain a definition for 'MSBuildExecutablePath' and no accessible extension method 'MSBuildExecutablePath' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (76-36)CS1061 'Build' does not contain a definition for 'MSBuildExecutablePath' and no accessible extension method 'MSBuildExecutablePath' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// // #endregion
// // #region MSBuildExecutablePath Tests
// //         /// <summary>
// //         /// Tests that MSBuildExecutablePath trims quotes when being set.
// //         /// </summary>
// //         [Fact]
// //         public void MSBuildExecutablePath_SetWithQuotes_TrimsQuotes()
// //         {
// //             // Arrange
// //             string pathWithQuotes = "\"C:\\Program Files\\MSBuild\\msbuild.exe\"";
// //             _build.MSBuildExecutablePath = pathWithQuotes;
// //             // Act
// //             string result = _build.MSBuildExecutablePath;
// //             // Assert
// //             result.Should().Be("C:\\Program Files\\MSBuild\\msbuild.exe", "because the quotes around the path should be trimmed");
// //         }
// //  // [Error] (108-47)CS1061 'Build' does not contain a definition for 'SourceFiles' and no accessible extension method 'SourceFiles' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (110-48)CS1061 'Build' does not contain a definition for 'SourceFiles' and no accessible extension method 'SourceFiles' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// #endregion
// #region SourceFiles Tests
//         /// <summary>
//         /// Tests that the SourceFiles property reads source files from the SourceFilesArchive, clears the archive, and caches the result.
//         /// </summary>
//         [Fact]
//         public void SourceFiles_WhenArchiveProvided_ReadsAndClearsArchive()
//         {
//             // Arrange: Create an in-memory zip archive with a single entry.
//             byte[] zipBytes;
//             using (var ms = new MemoryStream())
//             {
//                 using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
//                 {
//                     var entry = archive.CreateEntry("file1.txt");
//                     using (var entryStream = entry.Open())
//                     using (var writer = new StreamWriter(entryStream))
//                     {
//                         writer.Write("content");
//                     }
//                 }
// 
//                 zipBytes = ms.ToArray();
//             }
// 
//             _build.SourceFilesArchive = zipBytes;
//             // Act: First access should read and cache the result.
//             var sourceFilesFirstCall = _build.SourceFiles;
//             var archiveAfterCall = _build.SourceFilesArchive;
//             var sourceFilesSecondCall = _build.SourceFiles;
//             // Assert
//             sourceFilesFirstCall.Should().NotBeNull("because the zip archive was provided and should be read");
//             sourceFilesFirstCall.Should().NotBeEmpty("because the in-memory zip archive contains an entry");
//             archiveAfterCall.Should().BeNull("because the archive bytes should be cleared after reading");
//             sourceFilesSecondCall.Should().BeSameAs(sourceFilesFirstCall, "because the result should be cached after the initial read");
//         }
// //  // [Error] (142-31)CS0117 'Build' does not contain a definition for 'ReadSourceFiles' // [Error] (156-45)CS0117 'Build' does not contain a definition for 'ReadSourceFiles'
// //         /// <summary>
// //         /// Tests the static ReadSourceFiles(byte[]) method with both a valid zip archive and a corrupt archive.
// //         /// </summary>
// //         [Fact]
// //         public void ReadSourceFiles_ByteArray_ValidAndInvalidArchive()
// //         {
// //             // Arrange: Create a valid zip archive byte array.
// //             byte[] validZip;
// //             using (var ms = new MemoryStream())
// //             {
// //                 using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
// //                 {
// //                     var entry = archive.CreateEntry("file2.txt");
// //                     using (var entryStream = entry.Open())
// //                     using (var writer = new StreamWriter(entryStream))
// //                     {
// //                         writer.Write("data");
// //                     }
// //                 }
// // 
// //                 validZip = ms.ToArray();
// //             }
// // 
// //             // Act
// //             var files = Build.ReadSourceFiles(validZip);
// //             // Assert
// //             files.Should().NotBeNull("because the zip archive is valid");
// //             files.Should().NotBeEmpty("because the valid zip archive contains at least one entry");
// //             // Arrange: Create invalid zip archive bytes.
// //             byte[] invalidZip = new byte[]
// //             {
// //                 0,
// //                 1,
// //                 2,
// //                 3,
// //                 4
// //             };
// //             // Act
// //             var filesFromInvalidZip = Build.ReadSourceFiles(invalidZip);
// //             // Assert
// //             filesFromInvalidZip.Should().BeEmpty("because a corrupt zip archive should be tolerated and return an empty list");
// //         }
// //  // [Error] (182-35)CS0117 'Build' does not contain a definition for 'ReadSourceFiles'
//         /// <summary>
//         /// Tests the static ReadSourceFiles(string) method by creating a temporary zip file and verifying it reads the archive entries.
//         /// </summary>
//         [Fact]
//         public void ReadSourceFiles_String_TemporaryZipFile_ReadsEntries()
//         {
//             // Arrange: Create a temporary zip file.
//             string tempFile = Path.GetTempFileName();
//             try
//             {
//                 using (var fs = new FileStream(tempFile, FileMode.Create))
//                 using (var archive = new ZipArchive(fs, ZipArchiveMode.Create))
//                 {
//                     var entry = archive.CreateEntry("tempFile.txt");
//                     using (var writer = new StreamWriter(entry.Open()))
//                     {
//                         writer.Write("temporary content");
//                     }
//                 }
// 
//                 // Act
//                 var files = Build.ReadSourceFiles(tempFile);
//                 // Assert
//                 files.Should().NotBeNull("because the temporary zip file is valid");
//                 files.Should().NotBeEmpty("because the temporary zip file contains an entry");
//             }
//             finally
//             {
//                 if (File.Exists(tempFile))
//                 {
//                     File.Delete(tempFile);
//                 }
//             }
//         }
// 
// #endregion
// #region RegisterTask Tests
//         /// <summary>
//         /// Tests the RegisterTask method to ensure that when a task with a non-null FromAssembly is provided, its name is registered.
//         /// </summary>
//         [Fact]
//         public void RegisterTask_WithValidFromAssembly_AddsTaskNameToTaskAssemblies()
//         {
//         // Arrange
//         // NOTE: Task is not easily instantiated with the desired property values.
//         // This test is partial. In a real test, a concrete instance or a derived test double should be created 
//         // such that Task.FromAssembly returns a non-null string (e.g., "assembly.dll").
//         // For demonstration purposes, please complete this test with a proper Task instance.
//         }
// //  // [Error] (220-38)CS1061 'Build' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// // #endregion
// // #region ToString and TypeName Tests
// //         /// <summary>
// //         /// Tests that the TypeName property returns the expected string.
// //         /// </summary>
// //         [Fact]
// //         public void TypeName_ReturnsExpectedValue()
// //         {
// //             // Act
// //             string typeName = _build.TypeName;
// //             // Assert
// //             typeName.Should().Be("Build", "because the TypeName property should return 'Build'");
// //         }
// //  // [Error] (232-20)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (237-20)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the ToString method to ensure it returns a string indicating the build success status and includes the duration text.
//         /// </summary>
//         [Fact]
//         public void ToString_ReturnsFormattedString()
//         {
//             // Arrange
//             _build.Succeeded = true;
//             string resultSucceeded = _build.ToString();
//             // Assert for succeeded build
//             resultSucceeded.Should().Contain("succeeded", "because Succeeded is true");
//             // Arrange for failed build
//             _build.Succeeded = false;
//             string resultFailed = _build.ToString();
//             // Assert for failed build
//             resultFailed.Should().Contain("failed", "because Succeeded is false");
//         }
// //  // [Error] (256-20)CS1061 'Build' does not contain a definition for 'AddChild' and no accessible extension method 'AddChild' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (258-32)CS1061 'Build' does not contain a definition for 'FindDescendant' and no accessible extension method 'FindDescendant' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// // #endregion
// // #region FindDescendant Tests
// //         /// <summary>
// //         /// Tests that FindDescendant returns the correct child node when a child is added.
// //         /// </summary>
// //         [Fact]
// //         public void FindDescendant_WithExistingIndex_ReturnsExpectedNode()
// //         {
// //             // Arrange
// //             var child = new TimedNode
// //             {
// //                 Name = "ChildNode"
// //             };
// //             _build.AddChild(child);
// //             // Act
// //             var found = _build.FindDescendant(0);
// //             // Assert
// //             found.Should().NotBeNull("because a child node was added and should be found");
// //             found.Should().Be(child, "because the first child node should be returned");
// //         }
// //  // [Error] (273-37)CS1061 'Build' does not contain a definition for 'FindEvaluation' and no accessible extension method 'FindEvaluation' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// #endregion
// #region FindEvaluation Tests
//         /// <summary>
//         /// Tests that FindEvaluation returns null if no evaluation with the specified id exists.
//         /// </summary>
//         [Fact]
//         public void FindEvaluation_WhenIdNotPresent_ReturnsNull()
//         {
//             // Act
//             var evaluation = _build.FindEvaluation(999);
//             // Assert
//             evaluation.Should().BeNull("because there is no evaluation matching the provided id");
//         }
// //  // [Error] (290-43)CS1061 'Build' does not contain a definition for 'EvaluationFolder' and no accessible extension method 'EvaluationFolder' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (293-42)CS1061 'Build' does not contain a definition for 'FindEvaluation' and no accessible extension method 'FindEvaluation' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that FindEvaluation returns the evaluation when it exists within the EvaluationFolder.
// //         /// </summary>
// //         [Fact]
// //         public void FindEvaluation_WhenEvaluationExists_ReturnsEvaluation()
// //         {
// //             // Arrange
// //             // Create a dummy ProjectEvaluation and set its Id.
// //             var projectEvaluation = new ProjectEvaluation();
// //             // Assuming Id property is publicly settable for testing purposes.
// //             projectEvaluation.Id = 100;
// //             // Get the EvaluationFolder which will create and cache the folder.
// //             var evaluationFolder = _build.EvaluationFolder;
// //             evaluationFolder.AddChild(projectEvaluation);
// //             // Act
// //             var foundEvaluation = _build.FindEvaluation(100);
// //             // Assert
// //             foundEvaluation.Should().NotBeNull("because an evaluation with id 100 was added");
// //             foundEvaluation.Should().Be(projectEvaluation, "because the found evaluation should match the one that was added");
// //         }
// //  // [Error] (314-20)CS1061 'Build' does not contain a definition for 'RunInBackground' and no accessible extension method 'RunInBackground' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (315-20)CS1061 'Build' does not contain a definition for 'WaitForBackgroundTasks' and no accessible extension method 'WaitForBackgroundTasks' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// #endregion
// #region Background Task Tests
//         /// <summary>
//         /// Tests the RunInBackground and WaitForBackgroundTasks methods to ensure that background actions are executed.
//         /// </summary>
//         [Fact]
//         public void RunInBackground_ExecutesActionAndWaitForBackgroundTasks_WorksCorrectly()
//         {
//             // Arrange
//             bool actionExecuted = false;
//             Action action = () =>
//             {
//                 actionExecuted = true;
//             };
//             // Act
//             _build.RunInBackground(action);
//             _build.WaitForBackgroundTasks();
//             // Assert
//             actionExecuted.Should().BeTrue("because the background action should have been executed after waiting for background tasks");
//         }
// #endregion
//     }
// }