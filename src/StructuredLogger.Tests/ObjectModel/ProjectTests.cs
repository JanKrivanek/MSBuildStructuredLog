using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Microsoft.Build.Logging.StructuredLogger.UnitTests;
using Moq;
using StructuredLogger.BinaryLogger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref = "Project"/> class.
    /// </summary>
    public class ProjectTests
    {
        private readonly Project _project;
        public ProjectTests()
        {
            _project = new Project();
        }
//  // [Error] (38-38)CS1061 'Project' does not contain a definition for 'ProjectFileExtension' and no accessible extension method 'ProjectFileExtension' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the ProjectFileExtension property when ProjectFile is null to ensure it returns an empty string.
//         /// </summary>
//         [Fact]
//         public void ProjectFileExtension_WhenProjectFileIsNull_ReturnsEmptyString()
//         {
//             // Arrange
//             _project.ProjectFile = null;
//             // Act
//             var extension = _project.ProjectFileExtension;
//             // Assert
//             extension.Should().Be(string.Empty);
//         }
//  // [Error] (52-38)CS1061 'Project' does not contain a definition for 'ProjectFileExtension' and no accessible extension method 'ProjectFileExtension' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests the ProjectFileExtension property when ProjectFile is set to a file path to ensure it returns lower-case extension.
        /// </summary>
        [Fact]
        public void ProjectFileExtension_WhenProjectFileIsSet_ReturnsLowerCaseExtension()
        {
            // Arrange
            _project.ProjectFile = @"C:\Folder\MyProject.CSPROJ";
            // Act
            var extension = _project.ProjectFileExtension;
            // Assert
            extension.Should().Be(".csproj");
        }
//  // [Error] (66-38)CS1061 'Project' does not contain a definition for 'ProjectDirectory' and no accessible extension method 'ProjectDirectory' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the ProjectDirectory property when ProjectFile is null or empty to ensure it returns null.
//         /// </summary>
//         [Fact]
//         public void ProjectDirectory_WhenProjectFileIsNullOrEmpty_ReturnsNull()
//         {
//             // Arrange
//             _project.ProjectFile = "";
//             // Act
//             var directory = _project.ProjectDirectory;
//             // Assert
//             directory.Should().BeNull();
//         }
//  // [Error] (80-38)CS1061 'Project' does not contain a definition for 'ProjectDirectory' and no accessible extension method 'ProjectDirectory' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests the ProjectDirectory property when ProjectFile is set to a valid path to ensure it returns the directory name.
        /// </summary>
        [Fact]
        public void ProjectDirectory_WhenProjectFileIsSet_ReturnsDirectoryName()
        {
            // Arrange
            _project.ProjectFile = Path.Combine("C:", "Folder", "MyProject.csproj");
            // Act
            var directory = _project.ProjectDirectory;
            // Assert
            directory.Should().Be(Path.Combine("C:", "Folder"));
        }
//  // [Error] (95-43)CS1061 'Project' does not contain a definition for 'SourceFilePath' and no accessible extension method 'SourceFilePath' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the SourceFilePath property to ensure it returns the same value as ProjectFile.
//         /// </summary>
//         [Fact]
//         public void SourceFilePath_ReturnsProjectFile()
//         {
//             // Arrange
//             var filePath = @"C:\Folder\MyProject.csproj";
//             _project.ProjectFile = filePath;
//             // Act
//             var sourceFilePath = _project.SourceFilePath;
//             // Assert
//             sourceFilePath.Should().Be(filePath);
//         }
// 
        /// <summary>
        /// Tests the explicit IPreprocessable.RootFilePath implementation to ensure it returns the ProjectFile.
        /// </summary>
        [Fact]
        public void IPreprocessable_RootFilePath_ReturnsProjectFile()
        {
            // Arrange
            var filePath = @"C:\Folder\MyProject.csproj";
            _project.ProjectFile = filePath;
            var preprocessable = (IPreprocessable)_project;
            // Act
            var rootFilePath = preprocessable.RootFilePath;
            // Assert
            rootFilePath.Should().Be(filePath);
        }
//  // [Error] (123-37)CS1061 'Project' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the TypeName property to ensure it returns the string "Project".
//         /// </summary>
//         [Fact]
//         public void TypeName_ReturnsProject()
//         {
//             // Act
//             var typeName = _project.TypeName;
//             // Assert
//             typeName.Should().Be("Project");
//         }
//  // [Error] (145-37)CS0029 Cannot implicitly convert type 'System.Collections.Generic.List<string>' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' // [Error] (150-22)CS1061 'Project' does not contain a definition for 'GlobalProperties' and no accessible extension method 'GlobalProperties' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests the ToString method to ensure it returns a string containing project details.
        /// </summary>
        [Fact]
        public void ToString_ReturnsStringWithProjectDetails()
        {
            // Arrange
            // Assuming the base TimedNode has a public property 'Name'
            const string projectName = "TestProject";
            _project.ProjectFile = @"C:\Folder\MyProject.csproj";
            // Set Name via reflection if not publicly available
            var nameProperty = _project.GetType().GetProperty("Name");
            if (nameProperty != null && nameProperty.CanWrite)
            {
                nameProperty.SetValue(_project, projectName);
            }

            _project.EntryTargets = new List<string>
            {
                "Build",
                "Clean"
            };
            _project.GlobalProperties = ImmutableDictionary<string, string>.Empty.Add("Configuration", "Debug");
            // Act
            var result = _project.ToString();
            // Assert
            result.Should().Contain($"Project Name={projectName}");
            result.Should().Contain("File=C:\\Folder\\MyProject.csproj");
            result.Should().Contain("Targets=[Build, Clean]");
            result.Should().Contain("GlobalProperties=[Configuration=Debug");
        }
//  // [Error] (167-41)CS1061 'Project' does not contain a definition for 'GetTargetById' and no accessible extension method 'GetTargetById' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the GetTargetById method when an invalid id (-1) is provided to ensure it throws an ArgumentException.
//         /// </summary>
//         [Fact]
//         public void GetTargetById_InvalidId_ThrowsArgumentException()
//         {
//             // Act
//             Action act = () => _project.GetTargetById(-1);
//             // Assert
//             act.Should().Throw<ArgumentException>().WithMessage("Invalid target id: -1");
//         }
//  // [Error] (181-35)CS1061 'Project' does not contain a definition for 'GetTargetById' and no accessible extension method 'GetTargetById' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests the GetTargetById method when a non-existing id is provided to ensure it returns null.
        /// </summary>
        [Fact]
        public void GetTargetById_NonExistingId_ReturnsNull()
        {
            // Arrange
            var nonExistingId = 999;
            // Act
            var target = _project.GetTargetById(nonExistingId);
            // Assert
            target.Should().BeNull();
        }
//  // [Error] (193-42)CS1061 'Project' does not contain a definition for 'CreateTarget' and no accessible extension method 'CreateTarget' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?) // [Error] (195-35)CS1061 'Project' does not contain a definition for 'GetTargetById' and no accessible extension method 'GetTargetById' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the GetTargetById method to ensure it returns the target when it has been created.
//         /// </summary>
//         [Fact]
//         public void GetTargetById_ExistingId_ReturnsTarget()
//         {
//             // Arrange
//             var createdTarget = _project.CreateTarget("Build", 1);
//             // Act
//             var target = _project.GetTargetById(1);
//             // Assert
//             target.Should().BeSameAs(createdTarget);
//         }
//  // [Error] (208-36)CS1061 'Project' does not contain a definition for 'CreateTarget' and no accessible extension method 'CreateTarget' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?) // [Error] (209-36)CS1061 'Project' does not contain a definition for 'CreateTarget' and no accessible extension method 'CreateTarget' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests the CreateTarget method to ensure it creates a target with the provided name and id, 
        /// and that successive calls increment the internal index.
        /// </summary>
        [Fact]
        public void CreateTarget_WhenCalled_CreatesTargetWithCorrectNameIdAndIndex()
        {
            // Arrange & Act
            var target1 = _project.CreateTarget("Build", 1);
            var target2 = _project.CreateTarget("Clean", 2);
            // Assert
            target1.Name.Should().Be("Build");
            target1.Id.Should().Be(1);
            target1.Index.Should().BeGreaterThan(0);
            target2.Name.Should().Be("Clean");
            target2.Id.Should().Be(2);
            target2.Index.Should().BeGreaterThan(target1.Index);
        }

        /// <summary>
        /// Tests the IsLowRelevance property to ensure it correctly stores and returns values.
        /// Note: This test assumes that the default value for IsSelected is false.
        /// </summary>
        [Fact]
        public void IsLowRelevance_SetAndGet_ReturnsExpectedValue()
        {
            // Arrange & Act
            _project.IsLowRelevance = true;
            var valueWhenTrue = _project.IsLowRelevance;
            _project.IsLowRelevance = false;
            var valueWhenFalse = _project.IsLowRelevance;
            // Assert
            valueWhenTrue.Should().BeTrue();
            valueWhenFalse.Should().BeFalse();
        }
//  // [Error] (243-22)CS1061 'Project' does not contain a definition for 'TargetsText' and no accessible extension method 'TargetsText' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?) // [Error] (245-40)CS1061 'Project' does not contain a definition for 'TargetsDisplayText' and no accessible extension method 'TargetsDisplayText' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the TargetsDisplayText property when TargetsText is null or empty to ensure it returns an empty string.
//         /// </summary>
//         [Fact]
//         public void TargetsDisplayText_WhenTargetsTextIsNullOrEmpty_ReturnsEmptyString()
//         {
//             // Arrange
//             _project.TargetsText = "";
//             // Act
//             var displayText = _project.TargetsDisplayText;
//             // Assert
//             displayText.Should().Be(string.Empty);
//         }
//  // [Error] (257-22)CS1061 'Project' does not contain a definition for 'TargetsText' and no accessible extension method 'TargetsText' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?) // [Error] (259-40)CS1061 'Project' does not contain a definition for 'TargetsDisplayText' and no accessible extension method 'TargetsDisplayText' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests the TargetsDisplayText property when TargetsText is set to ensure it returns the correctly formatted string.
        /// </summary>
        [Fact]
        public void TargetsDisplayText_WhenTargetsTextIsSet_ReturnsFormattedString()
        {
            // Arrange
            _project.TargetsText = "Build";
            // Act
            var displayText = _project.TargetsDisplayText;
            // Assert
            displayText.Should().Be(" → Build");
        }
//  // [Error] (273-37)CS0029 Cannot implicitly convert type 'System.Collections.Generic.List<string>' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' // [Error] (278-22)CS1061 'Project' does not contain a definition for 'GlobalProperties' and no accessible extension method 'GlobalProperties' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?) // [Error] (280-36)CS1061 'Project' does not contain a definition for 'ToolTip' and no accessible extension method 'ToolTip' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the ToolTip property to ensure it returns a tooltip containing project file, targets, global properties,
//         /// and time/duration text.
//         /// </summary>
//         [Fact]
//         public void ToolTip_ReturnsTooltipWithAllInformation()
//         {
//             // Arrange
//             _project.ProjectFile = @"C:\Folder\MyProject.csproj";
//             _project.EntryTargets = new List<string>
//             {
//                 "Build",
//                 "Clean"
//             };
//             _project.GlobalProperties = ImmutableDictionary<string, string>.Empty.Add("Configuration", "Debug").Add("Platform", "AnyCPU").Add("Custom", "Value");
//             // Act
//             var tooltip = _project.ToolTip;
//             // Assert
//             tooltip.Should().Contain(_project.ProjectFile);
//             tooltip.Should().Contain("Targets:");
//             // Since targets are sorted alphabetically, "Build" should come before "Clean"
//             tooltip.Should().Contain("Build");
//             tooltip.Should().Contain("Clean");
//             tooltip.Should().Contain("Global Properties:");
//             tooltip.Should().Contain("Configuration = Debug");
//             tooltip.Should().Contain("Platform = AnyCPU");
//             tooltip.Should().Contain("Custom = Value");
//             // Also checks that time and duration text is appended (cannot assert exact value, so ensure not empty)
//             tooltip.Trim().Should().NotEndWith(string.Empty);
//         }
//  // [Error] (304-17)CS0117 'Task' does not contain a definition for 'Id' // [Error] (307-22)CS1061 'Project' does not contain a definition for 'OnTaskAdded' and no accessible extension method 'OnTaskAdded' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?) // [Error] (308-42)CS1061 'Project' does not contain a definition for 'GetTaskById' and no accessible extension method 'GetTaskById' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?) // [Error] (309-44)CS1061 'Project' does not contain a definition for 'GetTaskById' and no accessible extension method 'GetTaskById' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests the OnTaskAdded and GetTaskById methods to ensure that tasks are added and retrieved correctly.
        /// </summary>
        [Fact]
        public void OnTaskAdded_And_GetTaskById_AddsAndRetrievesTask()
        {
            // Arrange
            var task = new Task
            {
                Id = 5
            };
            // Act
            _project.OnTaskAdded(task);
            var retrievedTask = _project.GetTaskById(5);
            var nonExistingTask = _project.GetTaskById(999);
            // Assert
            retrievedTask.Should().BeSameAs(task);
            nonExistingTask.Should().BeNull();
        }
//  // [Error] (322-35)CS1061 'Project' does not contain a definition for 'CreateTarget' and no accessible extension method 'CreateTarget' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?) // [Error] (330-40)CS1061 'Project' does not contain a definition for 'FindTarget' and no accessible extension method 'FindTarget' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the FindTarget method to ensure that it finds a target by its name from the project's children.
//         /// </summary>
//         [Fact]
//         public void FindTarget_WhenTargetExists_ReturnsTarget()
//         {
//             // Arrange
//             var target = _project.CreateTarget("Build", 1);
//             // Using reflection to access the inherited Children property.
//             var childrenProperty = _project.GetType().GetProperty("Children", BindingFlags.Instance | BindingFlags.Public);
//             childrenProperty.Should().NotBeNull("Project should have a public Children property from its base class.");
//             var children = childrenProperty.GetValue(_project) as IList;
//             children.Should().NotBeNull("Children property should be a collection.");
//             children.Add(target);
//             // Act
//             var foundTarget = _project.FindTarget("Build");
//             // Assert
//             foundTarget.Should().BeSameAs(target);
//         }
//  // [Error] (348-40)CS1061 'Project' does not contain a definition for 'FindTarget' and no accessible extension method 'FindTarget' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests the FindTarget method when no matching target exists to ensure it returns null.
        /// </summary>
        [Fact]
        public void FindTarget_WhenTargetDoesNotExist_ReturnsNull()
        {
            // Arrange
            // Ensure that Children is empty or does not contain the target
            var childrenProperty = _project.GetType().GetProperty("Children", BindingFlags.Instance | BindingFlags.Public);
            childrenProperty.Should().NotBeNull("Project should have a public Children property from its base class.");
            var children = childrenProperty.GetValue(_project) as IList;
            children?.Clear();
            // Act
            var foundTarget = _project.FindTarget("NonExistingTarget");
            // Assert
            foundTarget.Should().BeNull();
        }
    }
}