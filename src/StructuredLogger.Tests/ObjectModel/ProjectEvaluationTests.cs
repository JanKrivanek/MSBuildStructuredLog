// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Microsoft.Build.Logging.StructuredLogger.UnitTests;
// using Moq;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Windows.Forms;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "ProjectEvaluation"/> class.
//     /// </summary>
//     public class ProjectEvaluationTests
//     {
//         private readonly ProjectEvaluation _projectEvaluation;
//         public ProjectEvaluationTests()
//         {
//             _projectEvaluation = new ProjectEvaluation();
//         }
// //  // [Error] (37-32)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFile' and no accessible extension method 'ProjectFile' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (39-48)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFileExtension' and no accessible extension method 'ProjectFileExtension' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the ProjectFileExtension property returns the correct extension in lower invariant form when ProjectFile is set.
// //         /// </summary>
// //         /// <param name = "projectFile">The input project file path.</param>
// //         /// <param name = "expectedExtension">The expected file extension in lower case.</param>
// //         [Theory]
// //         [InlineData(@"C:\folder\project.csproj", ".csproj")]
// //         [InlineData(@"C:\folder\project.CSPROJ", ".csproj")]
// //         [InlineData(@"C:\folder\project.fsproj", ".fsproj")]
// //         public void ProjectFileExtension_WhenProjectFileIsSet_ReturnsCorrectExtension(string projectFile, string expectedExtension)
// //         {
// //             // Arrange
// //             _projectEvaluation.ProjectFile = projectFile;
// //             // Act
// //             var extension = _projectEvaluation.ProjectFileExtension;
// //             // Assert
// //             extension.Should().Be(expectedExtension);
// //         }
// //  // [Error] (51-32)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFile' and no accessible extension method 'ProjectFile' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (53-48)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFileExtension' and no accessible extension method 'ProjectFileExtension' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the ProjectFileExtension property returns an empty string when the ProjectFile is null.
//         /// </summary>
//         [Fact]
//         public void ProjectFileExtension_WhenProjectFileIsNull_ReturnsEmptyString()
//         {
//             // Arrange
//             _projectEvaluation.ProjectFile = null;
//             // Act
//             var extension = _projectEvaluation.ProjectFileExtension;
//             // Assert
//             extension.Should().BeEmpty();
//         }
// //  // [Error] (66-32)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFile' and no accessible extension method 'ProjectFile' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (68-49)CS1061 'ProjectEvaluation' does not contain a definition for 'SourceFilePath' and no accessible extension method 'SourceFilePath' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the SourceFilePath property returns the same value as ProjectFile.
// //         /// </summary>
// //         [Fact]
// //         public void SourceFilePath_ReturnsProjectFile()
// //         {
// //             // Arrange
// //             var projectFile = @"C:\folder\project.proj";
// //             _projectEvaluation.ProjectFile = projectFile;
// //             // Act
// //             var sourcePath = _projectEvaluation.SourceFilePath;
// //             // Assert
// //             sourcePath.Should().Be(projectFile);
// //         }
// //  // [Error] (81-32)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFile' and no accessible extension method 'ProjectFile' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the explicit interface property IPreprocessable.RootFilePath returns the ProjectFile.
//         /// </summary>
//         [Fact]
//         public void IPreprocessable_RootFilePath_ReturnsProjectFile()
//         {
//             // Arrange
//             var projectFile = @"C:\folder\project.proj";
//             _projectEvaluation.ProjectFile = projectFile;
//             var preprocessor = (IPreprocessable)_projectEvaluation;
//             // Act
//             var rootFilePath = preprocessor.RootFilePath;
//             // Assert
//             rootFilePath.Should().Be(projectFile);
//         }
// //  // [Error] (106-32)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFile' and no accessible extension method 'ProjectFile' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (110-83)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFile' and no accessible extension method 'ProjectFile' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the ToString method returns a string containing the project Name, ProjectFile and formatted Id.
// //         /// </summary>
// //         [Fact]
// //         public void ToString_ReturnsExpectedFormat()
// //         {
// //             // Arrange
// //             // Set Name and Id via reflection if needed.
// //             var baseType = _projectEvaluation.GetType().BaseType;
// //             var nameProp = baseType.GetProperty("Name");
// //             nameProp?.SetValue(_projectEvaluation, "TestProject");
// //             var idProp = baseType.GetProperty("Id");
// //             if (idProp != null)
// //             {
// //                 idProp.SetValue(_projectEvaluation, 123);
// //             }
// // 
// //             _projectEvaluation.ProjectFile = @"C:\folder\project.proj";
// //             // Act
// //             var result = _projectEvaluation.ToString();
// //             // Assert
// //             result.Should().Contain("TestProject").And.Contain(_projectEvaluation.ProjectFile).And.Contain("000123");
// //         }
// //  // [Error] (120-32)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFile' and no accessible extension method 'ProjectFile' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (122-46)CS1061 'ProjectEvaluation' does not contain a definition for 'ToolTip' and no accessible extension method 'ToolTip' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (124-59)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFile' and no accessible extension method 'ProjectFile' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the ToolTip property returns a string starting with the ProjectFile and includes time/duration information.
//         /// </summary>
//         [Fact]
//         public void ToolTip_ReturnsExpectedFormat()
//         {
//             // Arrange
//             _projectEvaluation.ProjectFile = @"C:\folder\project.proj";
//             // Act
//             var tooltip = _projectEvaluation.ToolTip;
//             // Assert
//             tooltip.Should().StartWith(_projectEvaluation.ProjectFile).And.Contain("\n");
//         }
// //  // [Error] (134-48)CS1061 'ProjectEvaluation' does not contain a definition for 'AdornmentString' and no accessible extension method 'AdornmentString' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the AdornmentString property returns a non-null value.
// //         /// </summary>
// //         [Fact]
// //         public void AdornmentString_ReturnsNonNullValue()
// //         {
// //             // Act
// //             var adornment = _projectEvaluation.AdornmentString;
// //             // Assert
// //             adornment.Should().NotBeNull();
// //         }
// //  // [Error] (146-50)CS1061 'ProjectEvaluation' does not contain a definition for 'ImportsFolder' and no accessible extension method 'ImportsFolder' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (147-51)CS1061 'ProjectEvaluation' does not contain a definition for 'ImportsFolder' and no accessible extension method 'ImportsFolder' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the ImportsFolder property creates the folder upon first access and returns the same instance on subsequent accesses.
//         /// </summary>
//         [Fact]
//         public void ImportsFolder_WhenAccessedMultipleTimes_ReturnsSameInstance()
//         {
//             // Act
//             var firstAccess = _projectEvaluation.ImportsFolder;
//             var secondAccess = _projectEvaluation.ImportsFolder;
//             // Assert
//             firstAccess.Should().NotBeNull();
//             secondAccess.Should().BeSameAs(firstAccess);
//             // Verify that the folder name is set correctly. Assumes Strings.Imports equals "Imports".
//             firstAccess.Name.Should().Be("Imports");
//         }
// //  // [Error] (162-50)CS1061 'ProjectEvaluation' does not contain a definition for 'PropertyReassignmentFolder' and no accessible extension method 'PropertyReassignmentFolder' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (163-51)CS1061 'ProjectEvaluation' does not contain a definition for 'PropertyReassignmentFolder' and no accessible extension method 'PropertyReassignmentFolder' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the PropertyReassignmentFolder property creates the folder upon first access and returns the same instance on subsequent accesses.
// //         /// </summary>
// //         [Fact]
// //         public void PropertyReassignmentFolder_WhenAccessedMultipleTimes_ReturnsSameInstance()
// //         {
// //             // Act
// //             var firstAccess = _projectEvaluation.PropertyReassignmentFolder;
// //             var secondAccess = _projectEvaluation.PropertyReassignmentFolder;
// //             // Assert
// //             firstAccess.Should().NotBeNull();
// //             secondAccess.Should().BeSameAs(firstAccess);
// //             // Verify that the folder name is set correctly. Assumes Strings.PropertyReassignmentFolder equals "PropertyReassignmentFolder".
// //             firstAccess.Name.Should().Be("PropertyReassignmentFolder");
// //         }
// //  // [Error] (178-32)CS1061 'ProjectEvaluation' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (179-49)CS1061 'ProjectEvaluation' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (180-32)CS1061 'ProjectEvaluation' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (181-50)CS1061 'ProjectEvaluation' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the IsLowRelevance property setter and getter work as expected.
//         /// </summary>
//         [Fact]
//         public void IsLowRelevance_SetAndGet_ReturnsExpectedValue()
//         {
//             // Arrange & Act
//             _projectEvaluation.IsLowRelevance = true;
//             var trueResult = _projectEvaluation.IsLowRelevance;
//             _projectEvaluation.IsLowRelevance = false;
//             var falseResult = _projectEvaluation.IsLowRelevance;
//             // Assert
//             trueResult.Should().BeTrue();
//             falseResult.Should().BeFalse();
//         }
// //  // [Error] (195-32)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFile' and no accessible extension method 'ProjectFile' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (198-32)CS1061 'ProjectEvaluation' does not contain a definition for 'AddImport' and no accessible extension method 'AddImport' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (200-32)CS1061 'ProjectEvaluation' does not contain a definition for 'GetAllImportsTransitive' and no accessible extension method 'GetAllImportsTransitive' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (201-48)CS1061 'ProjectEvaluation' does not contain a definition for 'ImportsFolder' and no accessible extension method 'ImportsFolder' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests the AddImport method with a TextNode whose SourceFilePath matches the ProjectFile.
// //         /// It should add the node to the ImportsFolder and not add it to the internal imports map.
// //         /// </summary>
// //         [Fact]
// //         public void AddImport_TextNodeWithSameSourceFile_AddsToImportsFolderOnly()
// //         {
// //             // Arrange
// //             _projectEvaluation.ProjectFile = "ProjectFilePath";
// //             var dummyTextNode = new DummyTextNode("ProjectFilePath");
// //             // Act
// //             _projectEvaluation.AddImport(dummyTextNode);
// //             // Assert
// //             _projectEvaluation.GetAllImportsTransitive().Should().BeEmpty();
// //             ((DummyTreeNode)_projectEvaluation.ImportsFolder).Children.Should().Contain(dummyTextNode);
// //         }
// //  // [Error] (212-32)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFile' and no accessible extension method 'ProjectFile' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (215-32)CS1061 'ProjectEvaluation' does not contain a definition for 'AddImport' and no accessible extension method 'AddImport' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (217-32)CS1061 'ProjectEvaluation' does not contain a definition for 'GetAllImportsTransitive' and no accessible extension method 'GetAllImportsTransitive' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (218-48)CS1061 'ProjectEvaluation' does not contain a definition for 'ImportsFolder' and no accessible extension method 'ImportsFolder' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the AddImport method with an Import whose SourceFilePath differs from the ProjectFile and has a non-null ImportedProjectFilePath.
//         /// It should store the import in the internal map and add it to the parent's children.
//         /// </summary>
//         [Fact]
//         public void AddImport_TextNodeDifferentSourceFile_WithImportedProjectFile_AddsToImportsMap()
//         {
//             // Arrange
//             _projectEvaluation.ProjectFile = "MainProject.proj";
//             var import = new DummyImport("DifferentPath.proj", "ImportedFile.proj");
//             // Act
//             _projectEvaluation.AddImport(import);
//             // Assert
//             _projectEvaluation.GetAllImportsTransitive().Should().ContainSingle().Which.Should().Be(import);
//             ((DummyTreeNode)_projectEvaluation.ImportsFolder).Children.Should().Contain(import);
//         }
// //  // [Error] (229-32)CS1061 'ProjectEvaluation' does not contain a definition for 'ProjectFile' and no accessible extension method 'ProjectFile' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (232-32)CS1061 'ProjectEvaluation' does not contain a definition for 'AddImport' and no accessible extension method 'AddImport' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (235-32)CS1061 'ProjectEvaluation' does not contain a definition for 'AddImport' and no accessible extension method 'AddImport' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?) // [Error] (237-32)CS1061 'ProjectEvaluation' does not contain a definition for 'GetAllImportsTransitive' and no accessible extension method 'GetAllImportsTransitive' accepting a first argument of type 'ProjectEvaluation' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests the AddImport method for a TextNode whose SourceFilePath differs from ProjectFile and already exists in the import map.
// //         /// It should add the node as a child to the found parent from the imports map.
// //         /// </summary>
// //         [Fact]
// //         public void AddImport_TextNodeDifferentSourceFile_ExistingImportInMap_AddsToExistingParentChildren()
// //         {
// //             // Arrange
// //             _projectEvaluation.ProjectFile = "MainProject.proj";
// //             var existingImport = new DummyImport("OtherPath.proj", "ImportedFile.proj");
// //             // Add first to populate imports map.
// //             _projectEvaluation.AddImport(existingImport);
// //             var anotherNode = new DummyTextNode("OtherPath.proj");
// //             // Act
// //             _projectEvaluation.AddImport(anotherNode);
// //             // Assert
// //             _projectEvaluation.GetAllImportsTransitive().Should().ContainSingle().Which.Should().Be(existingImport);
// //             ((DummyTreeNode)existingImport).Children.Should().Contain(anotherNode);
// //         }
// // 
//         // Dummy classes to support testing of AddImport and child management.
//         // These provide minimal implementations for TreeNode, TextNode and Import.
//         private class DummyTreeNode : TreeNode
//         {
//             public List<TreeNode> Children { get; } = new List<TreeNode>();
// //  // [Error] (247-34)CS0115 'ProjectEvaluationTests.DummyTreeNode.AddChild(TreeNode)': no suitable method found to override
// //             public override void AddChild(TreeNode child)
// //             {
// //                 Children.Add(child);
// //             }
// //         }
// //  // [Error] (253-54)CS0535 'ProjectEvaluationTests.DummyTextNode' does not implement interface member 'IHasSourceFile.SourceFilePath.set' // [Error] (260-22)CS1061 'ProjectEvaluationTests.DummyTextNode' does not contain a definition for 'Name' and no accessible extension method 'Name' accepting a first argument of type 'ProjectEvaluationTests.DummyTextNode' could be found (are you missing a using directive or an assembly reference?)
// //         private class DummyTextNode : DummyTreeNode, IHasSourceFile
// //         {
// //             public string SourceFilePath { get; }
// // 
// //             public DummyTextNode(string sourceFilePath)
// //             {
// //                 SourceFilePath = sourceFilePath;
// //                 this.Name = "DummyTextNode";
// //             }
// //         }
// //  // [Error] (271-22)CS1061 'ProjectEvaluationTests.DummyImport' does not contain a definition for 'Name' and no accessible extension method 'Name' accepting a first argument of type 'ProjectEvaluationTests.DummyImport' could be found (are you missing a using directive or an assembly reference?)
//         private class DummyImport : DummyTextNode
//         {
//             public string ImportedProjectFilePath { get; }
// 
//             public DummyImport(string sourceFilePath, string importedProjectFilePath) : base(sourceFilePath)
//             {
//                 ImportedProjectFilePath = importedProjectFilePath;
//                 this.Name = "DummyImport";
//             }
//         }
//     }
// }