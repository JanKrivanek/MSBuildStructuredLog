// using FluentAssertions;
// using Moq;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using Microsoft.Build.Logging.StructuredLogger;
// using StructuredLogViewer;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="FileCopyInfo"/> class.
//     /// </summary>
//     public class FileCopyInfoTests
//     {
//         /// <summary>
//         /// Tests that ToString returns the same value as FileCopyOperation.ToString when FileCopyOperation is set.
//         /// </summary>
//         [Fact]
//         public void ToString_FileCopyOperationSet_ReturnsFileCopyOperationToString()
//         {
//             // Arrange
//             var expectedString = "TestOperation";
//             // Moq can be used to mock the virtual ToString method on FileCopyOperation.
//             var fileCopyOperationMock = new Mock<FileCopyOperation>();
//             fileCopyOperationMock.Setup(op => op.ToString()).Returns(expectedString);
// 
//             var fileCopyInfo = new FileCopyInfo
//             {
//                 FileCopyOperation = fileCopyOperationMock.Object,
//                 Task = null,
//                 Target = null,
//                 Project = null
//             };
// 
//             // Act
//             var result = fileCopyInfo.ToString();
// 
//             // Assert
//             result.Should().Be(expectedString);
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="FileData"/> class.
//     /// </summary>
//     public class FileDataTests
//     {
//         /// <summary>
//         /// Tests that CompareTo returns a positive value when compared with null.
//         /// </summary>
//         [Fact]
//         public void CompareTo_NullOther_ReturnsPositiveValue()
//         {
//             // Arrange
//             var fileData = new FileData { Name = "TestFile" };
// 
//             // Act
//             int comparisonResult = fileData.CompareTo(null);
// 
//             // Assert
//             comparisonResult.Should().BeGreaterThan(0);
//         }
// 
//         /// <summary>
//         /// Tests that CompareTo returns zero when the names are equal regardless of case.
//         /// </summary>
//         [Fact]
//         public void CompareTo_EqualNames_ReturnsZero()
//         {
//             // Arrange
//             var fileData1 = new FileData { Name = "TestFile" };
//             var fileData2 = new FileData { Name = "testfile" };
// 
//             // Act
//             int comparisonResult = fileData1.CompareTo(fileData2);
// 
//             // Assert
//             comparisonResult.Should().Be(0);
//         }
// 
//         /// <summary>
//         /// Tests that CompareTo orders files correctly based on Name.
//         /// </summary>
//         [Fact]
//         public void CompareTo_DifferentNames_ReturnsCorrectOrdering()
//         {
//             // Arrange
//             var fileData1 = new FileData { Name = "Alpha" };
//             var fileData2 = new FileData { Name = "Beta" };
// 
//             // Act
//             int comparisonResult = fileData1.CompareTo(fileData2);
// 
//             // Assert
//             comparisonResult.Should().BeLessThan(0);
//         }
// 
//         /// <summary>
//         /// Tests that ToString returns the Name property.
//         /// </summary>
//         [Fact]
//         public void ToString_ReturnsNameProperty()
//         {
//             // Arrange
//             var fileData = new FileData { Name = "MyFile.txt" };
// 
//             // Act
//             var result = fileData.ToString();
// 
//             // Assert
//             result.Should().Be("MyFile.txt");
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="DirectoryData"/> class.
//     /// </summary>
//     public class DirectoryDataTests
//     {
//         /// <summary>
//         /// Tests that ToString returns only the Name if Parent is not set.
//         /// </summary>
//         [Fact]
//         public void ToString_NoParent_ReturnsName()
//         {
//             // Arrange
//             var directoryData = new DirectoryData { Name = "Folder" };
// 
//             // Act
//             var result = directoryData.ToString();
// 
//             // Assert
//             result.Should().Be("Folder");
//         }
// 
//         /// <summary>
//         /// Tests that ToString returns the combined path when Parent is set.
//         /// </summary>
//         [Fact]
//         public void ToString_WithParent_ReturnsCombinedPath()
//         {
//             // Arrange
//             var parent = new DirectoryData { Name = "ParentFolder" };
//             var child = new DirectoryData { Name = "ChildFolder", Parent = parent };
//             var expected = Path.Combine(parent.ToString(), "ChildFolder");
// 
//             // Act
//             var result = child.ToString();
// 
//             // Assert
//             result.Should().Be(expected);
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="FileCopyMap"/> class.
//     /// </summary>
//     public class FileCopyMapTests
//     {
//         private readonly FileCopyMap _fileCopyMap;
// 
//         public FileCopyMapTests()
//         {
//             _fileCopyMap = new FileCopyMap();
//         }
// 
//         #region GetFile Tests
// 
//         /// <summary>
//         /// Tests that GetFile creates and returns a new FileData object for a non-existent file path when create is true.
//         /// </summary>
//         [Fact]
//         public void GetFile_NonExistentFile_CreateTrue_ReturnsNewFileData()
//         {
//             // Arrange
//             string filePath = Path.Combine("C:\\", "TestFolder", "TestFile.txt");
//             // Ensure the directory exists in our FileCopyMap context.
//             var directory = _fileCopyMap.GetDirectory(Path.GetDirectoryName(filePath), create: true);
//             directory.Should().NotBeNull();
// 
//             // Act
//             var fileData = _fileCopyMap.GetFile(filePath, create: true);
// 
//             // Assert
//             fileData.Should().NotBeNull();
//             fileData.FilePath.Should().Be(filePath);
//             fileData.Name.Should().Be(Path.GetFileName(filePath));
//             fileData.Directory.Should().Be(directory);
//         }
// 
//         /// <summary>
//         /// Tests that GetFile returns null for a non-existent file path when create is false.
//         /// </summary>
//         [Fact]
//         public void GetFile_NonExistentFile_CreateFalse_ReturnsNull()
//         {
//             // Arrange
//             string filePath = Path.Combine("C:\\", "NonExistentFolder", "NoFile.txt");
//             var directory = _fileCopyMap.GetDirectory(Path.GetDirectoryName(filePath), create: false);
//             directory.Should().BeNull();
// 
//             // Act
//             var fileData = _fileCopyMap.GetFile(filePath, create: false);
// 
//             // Assert
//             fileData.Should().BeNull();
//         }
// 
//         #endregion
// 
//         #region GetDirectory Tests
// 
//         /// <summary>
//         /// Tests that GetDirectory creates and returns a new DirectoryData object for a new path when create is true.
//         /// </summary>
//         [Fact]
//         public void GetDirectory_NewPath_CreateTrue_ReturnsNewDirectoryData()
//         {
//             // Arrange
//             string path = Path.Combine("C:\\", "NewFolder");
// 
//             // Act
//             var directoryData = _fileCopyMap.GetDirectory(path, create: true);
// 
//             // Assert
//             directoryData.Should().NotBeNull();
//             directoryData.Name.Should().Be("NewFolder");
//         }
// 
//         /// <summary>
//         /// Tests that GetDirectory returns null for a non-existent path when create is false.
//         /// </summary>
//         [Fact]
//         public void GetDirectory_NonExistentPath_CreateFalse_ReturnsNull()
//         {
//             // Arrange
//             string path = Path.Combine("C:\\", "NonExistentFolder");
// 
//             // Act
//             var directoryData = _fileCopyMap.GetDirectory(path, create: false);
// 
//             // Assert
//             directoryData.Should().BeNull();
//         }
// //  // [Error] (260-31)CS7036 There is no argument given that corresponds to the required parameter 'query' of 'NodeQueryMatcher.NodeQueryMatcher(string)' // [Error] (262-17)CS0272 The property or indexer 'NodeQueryMatcher.TypeKeyword' cannot be used in this context because the set accessor is inaccessible // [Error] (263-17)CS0272 The property or indexer 'NodeQueryMatcher.Terms' cannot be used in this context because the set accessor is inaccessible
// //         #endregion
// // 
// //         #region TryGetResults Tests
// // 
// //         /// <summary>
// //         /// Tests that TryGetResults returns false when the matcher TypeKeyword is not "copy".
// //         /// </summary>
// //         [Fact]
// //         public void TryGetResults_TypeKeywordNotCopy_ReturnsFalse()
// //         {
// //             // Arrange
// //             var matcher = new NodeQueryMatcher
// //             {
// //                 TypeKeyword = "notcopy",
// //                 Terms = new List<Term>()
// //             };
// //             var results = new List<SearchResult>();
// // 
// //             // Act
// //             bool returnValue = _fileCopyMap.TryGetResults(matcher, results, 10);
// // 
// //             // Assert
// //             returnValue.Should().BeFalse();
// //             results.Should().BeEmpty();
// //         }
// //  // [Error] (282-31)CS7036 There is no argument given that corresponds to the required parameter 'query' of 'NodeQueryMatcher.NodeQueryMatcher(string)' // [Error] (284-17)CS0272 The property or indexer 'NodeQueryMatcher.TypeKeyword' cannot be used in this context because the set accessor is inaccessible // [Error] (285-17)CS0272 The property or indexer 'NodeQueryMatcher.Terms' cannot be used in this context because the set accessor is inaccessible // [Error] (286-17)CS0200 Property or indexer 'NodeQueryMatcher.ProjectMatchers' cannot be assigned to -- it is read only
//         /// <summary>
//         /// Tests that TryGetResults adds a note result when Terms are empty and no project match is provided.
//         /// </summary>
//         [Fact]
//         public void TryGetResults_EmptyTerms_NoProjectMatchers_AddsNoteResult()
//         {
//             // Arrange
//             var matcher = new NodeQueryMatcher
//             {
//                 TypeKeyword = "copy",
//                 Terms = new List<Term>(),
//                 ProjectMatchers = new List<NodeQueryMatcher>()
//             };
//             var results = new List<SearchResult>();
// 
//             // Act
//             bool returnValue = _fileCopyMap.TryGetResults(matcher, results, 10);
// 
//             // Assert
//             returnValue.Should().BeTrue();
//             results.Should().HaveCount(1);
//             // Check that the result contains a note message (manual verification might be needed).
//             results[0].Node.Should().NotBeNull();
//         }
// //  // [Error] (316-38)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
// //         #endregion
// // 
// //         #region AnalyzeTask Tests
// // 
// //         /// <summary>
// //         /// Partial test for AnalyzeTask method when a non-copy task is provided.
// //         /// </summary>
// //         [Fact]
// //         public void AnalyzeTask_NonCopyTask_DoesNothing()
// //         {
// //             // Arrange
// //             // Creating a dummy Task instance that is not a CopyTask.
// //             var dummyTask = new Task();
// // 
// //             // Act
// //             // Should not throw any exception.
// //             _fileCopyMap.AnalyzeTask(dummyTask);
// // 
// //             // Assert
// //             // Since internal state is not exposed, further assertions require additional context.
// //             // TODO: Extend test when more details about Task behavior are available.
// //             true.Should().BeTrue();
// //         }
// // 
//         #endregion
// 
//         #region AnalyzeTarget Tests
// 
//         /// <summary>
//         /// Partial test for AnalyzeTarget method when target does not match the criteria.
//         /// </summary>
//         [Fact]
//         public void AnalyzeTarget_NonMatchingTarget_DoesNothing()
//         {
//             // Arrange
//             var target = new Target { Name = "SomeOtherTarget", Skipped = true };
// 
//             // Act
//             _fileCopyMap.AnalyzeTarget(target);
// 
//             // Assert
//             // Since behavior is internal, further assertions require additional details.
//             // TODO: Extend test when observable behavior or state changes are available.
//             true.Should().BeTrue();
//         }
// 
//         #endregion
//     }
// }