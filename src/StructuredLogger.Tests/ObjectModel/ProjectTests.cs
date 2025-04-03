using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Project"/> class.
    /// </summary>
    public class ProjectTests
    {
        /// <summary>
        /// Tests the ProjectFileExtension property when ProjectFile is null.
        /// Expected to return an empty string.
        /// </summary>
        [Fact]
        public void ProjectFileExtension_WhenProjectFileIsNull_ReturnsEmptyString()
        {
            // Arrange
            var project = new Project
            {
                ProjectFile = null
            };

            // Act
            string extension = project.ProjectFileExtension;

            // Assert
            extension.Should().Be(string.Empty);
        }

        /// <summary>
        /// Tests the ProjectFileExtension property with a valid file path.
        /// Expected to return the file extension in lower case.
        /// </summary>
        [Fact]
        public void ProjectFileExtension_WhenProjectFileIsSet_ReturnsLowerCaseExtension()
        {
            // Arrange
            var project = new Project
            {
                ProjectFile = @"C:\Projects\MyProject.CSPROJ"
            };
            string expectedExtension = Path.GetExtension(project.ProjectFile).ToLowerInvariant();

            // Act
            string extension = project.ProjectFileExtension;

            // Assert
            extension.Should().Be(expectedExtension);
        }

        /// <summary>
        /// Tests the ProjectDirectory property with a valid ProjectFile.
        /// Expected to return the directory name of the file.
        /// </summary>
        [Fact]
        public void ProjectDirectory_WhenProjectFileIsSet_ReturnsDirectoryName()
        {
            // Arrange
            var project = new Project
            {
                ProjectFile = @"C:\Projects\MyProject.csproj"
            };
            string expectedDirectory = Path.GetDirectoryName(project.ProjectFile);

            // Act
            string directory = project.ProjectDirectory;

            // Assert
            directory.Should().Be(expectedDirectory);
        }

        /// <summary>
        /// Tests the ProjectDirectory property when ProjectFile is empty.
        /// Expected to return null.
        /// </summary>
        [Fact]
        public void ProjectDirectory_WhenProjectFileIsEmpty_ReturnsNull()
        {
            // Arrange
            var project = new Project
            {
                ProjectFile = string.Empty
            };

            // Act
            string directory = project.ProjectDirectory;

            // Assert
            directory.Should().BeNull();
        }

        /// <summary>
        /// Tests the SourceFilePath property to ensure it returns the same value as ProjectFile.
        /// </summary>
        [Fact]
        public void SourceFilePath_ShouldMatchProjectFile()
        {
            // Arrange
            var project = new Project
            {
                ProjectFile = @"C:\Projects\MyProject.csproj"
            };

            // Act
            string sourceFilePath = project.SourceFilePath;

            // Assert
            sourceFilePath.Should().Be(project.ProjectFile);
        }

        /// <summary>
        /// Tests the TargetsDisplayText property when TargetsText is null or empty.
        /// Expected to return an empty string.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void TargetsDisplayText_WhenTargetsTextIsNullOrEmpty_ReturnsEmptyString(string targetsText)
        {
            // Arrange
            var project = new Project
            {
                TargetsText = targetsText
            };

            // Act
            string displayText = project.TargetsDisplayText;

            // Assert
            displayText.Should().Be(string.Empty);
        }

        /// <summary>
        /// Tests the TargetsDisplayText property when TargetsText has a value.
        /// Expected to return the formatted string with an arrow and a space.
        /// </summary>
        [Fact]
        public void TargetsDisplayText_WhenTargetsTextIsSet_ReturnsFormattedString()
        {
            // Arrange
            var project = new Project
            {
                TargetsText = "Build;Clean"
            };
            string expected = $" → {project.TargetsText}";

            // Act
            string displayText = project.TargetsDisplayText;

            // Assert
            displayText.Should().Be(expected);
        }

        /// <summary>
        /// Tests the ToString method for proper construction of the string representation.
        /// It verifies inclusion of project name, file, entry targets, and global properties.
        /// </summary>
        [Fact]
        public void ToString_WhenCalled_ReturnsCorrectStringRepresentation()
        {
            // Arrange
            var project = new Project
            {
                ProjectFile = @"C:\Projects\TestProject.csproj",
                EntryTargets = new List<string> { "Build", "Clean" },
                GlobalProperties = new Dictionary<string, string> { { "Key1", "Value1" } }.ToImmutableDictionary()
            };

            // Set Name property inherited from NamedNode; assuming it is settable.
            project.Name = "TestProject";

            // Act
            string result = project.ToString();

            // Assert
            result.Should().Contain("Project Name=TestProject");
            result.Should().Contain("File=C:\\Projects\\TestProject.csproj");
            result.Should().Contain("Targets=[");
            result.Should().Contain("Build");
            result.Should().Contain("Clean");
            result.Should().Contain("GlobalProperties=[");
            result.Should().Contain("Key1=Value1");
        }

        /// <summary>
        /// Tests GetTargetById when an invalid id (-1) is provided.
        /// Expected to throw an ArgumentException.
        /// </summary>
        [Fact]
        public void GetTargetById_WhenIdIsMinusOne_ThrowsArgumentException()
        {
            // Arrange
            var project = new Project();

            // Act
            Action act = () => project.GetTargetById(-1);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Invalid target id: -1");
        }

        /// <summary>
        /// Tests GetTargetById when a valid id that does not exist is provided.
        /// Expected to return null.
        /// </summary>
        [Fact]
        public void GetTargetById_WhenTargetDoesNotExist_ReturnsNull()
        {
            // Arrange
            var project = new Project();

            // Act
            var target = project.GetTargetById(100);

            // Assert
            target.Should().BeNull();
        }

        /// <summary>
        /// Tests CreateTarget to verify that a target is created with the given name and id,
        /// and that it can be retrieved using GetTargetById.
        /// </summary>
        [Fact]
        public void CreateTarget_WhenCalled_ReturnsTargetWithSpecifiedNameAndId()
        {
            // Arrange
            var project = new Project();
            string targetName = "MyTarget";
            int targetId = 42;

            // Act
            var createdTarget = project.CreateTarget(targetName, targetId);
            var retrievedTarget = project.GetTargetById(targetId);

            // Assert
            createdTarget.Should().NotBeNull();
            createdTarget.Name.Should().Be(targetName);
            createdTarget.Id.Should().Be(targetId);
            retrievedTarget.Should().Be(createdTarget);
        }

        /// <summary>
        /// Tests the IsLowRelevance property setter and getter.
        /// Verifies that setting the property updates its value accordingly.
        /// </summary>
        [Fact]
        public void IsLowRelevance_SetValue_UpdatesProperty()
        {
            // Arrange
            var project = new Project();

            // Act
            project.IsLowRelevance = true;
            bool valueAfterSettingTrue = project.IsLowRelevance;

            project.IsLowRelevance = false;
            bool valueAfterSettingFalse = project.IsLowRelevance;

            // Assert
            // Assuming default IsSelected is false, setting flag to true should return true.
            valueAfterSettingTrue.Should().BeTrue();
            valueAfterSettingFalse.Should().BeFalse();
        }

        /// <summary>
        /// Tests the ToolTip property to ensure it includes the ProjectFile,
        /// lists entry targets, and displays global properties with proper formatting.
        /// </summary>
        [Fact]
        public void ToolTip_WhenCalled_ReturnsFormattedToolTip()
        {
            // Arrange
            var project = new Project
            {
                ProjectFile = @"C:\Projects\TestProject.csproj",
                EntryTargets = new List<string> { "Build", "Clean" },
                GlobalProperties = new Dictionary<string, string>
                {
                    { "Prop1", "Value1" },
                    { "Prop2", "Value2" },
                    { "Prop3", "Value3" },
                    { "Prop4", "Value4" },
                    { "Prop5", "Value5" },
                    { "Prop6", "Value6" }
                }.ToImmutableDictionary()
            };

            // Act
            string toolTip = project.ToolTip;

            // Assert
            toolTip.Should().Contain(project.ProjectFile);
            toolTip.Should().Contain("Targets:");
            toolTip.Should().Contain("Build");
            toolTip.Should().Contain("Clean");
            toolTip.Should().Contain("Global Properties:");
            // Only 5 properties should be printed, then ellipsis.
            toolTip.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                   .Count(line => line.Contains(" = ")).Should().Be(5);
            toolTip.Should().Contain("...");
        }
//  // [Error] (320-35)CS0117 'Task' does not contain a definition for 'Id' // [Error] (323-33)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         /// <summary>
//         /// Tests OnTaskAdded and GetTaskById methods.
//         /// Verifies that a task added via OnTaskAdded can be retrieved by its id.
//         /// </summary>
//         [Fact]
//         public void OnTaskAdded_ThenGetTaskById_ReturnsAddedTask()
//         {
//             // Arrange
//             var project = new Project();
//             var task = new Task { Id = 101 };
// 
//             // Act
//             project.OnTaskAdded(task);
//             var retrievedTask = project.GetTaskById(101);
// 
//             // Assert
//             retrievedTask.Should().Be(task);
//         }
// 
        /// <summary>
        /// Tests GetTaskById when the task does not exist.
        /// Expected to return null.
        /// </summary>
        [Fact]
        public void GetTaskById_WhenTaskDoesNotExist_ReturnsNull()
        {
            // Arrange
            var project = new Project();

            // Act
            var task = project.GetTaskById(999);

            // Assert
            task.Should().BeNull();
        }

        /// <summary>
        /// Tests the FindTarget method to ensure it returns the target with the specified name from Children.
        /// </summary>
        [Fact]
        public void FindTarget_WhenTargetExistsInChildren_ReturnsTarget()
        {
            // Arrange
            var project = new Project();
            var target = project.CreateTarget("TestTarget", 1);
            // Assuming that project.Children is publicly accessible and modifiable.
            // Add the created target to the Children collection.
            project.Children.Add(target);

            // Act
            var foundTarget = project.FindTarget("TestTarget");

            // Assert
            foundTarget.Should().Be(target);
        }

        /// <summary>
        /// Tests the FindTarget method when no matching target exists in Children.
        /// Expected to return null.
        /// </summary>
        [Fact]
        public void FindTarget_WhenTargetDoesNotExistInChildren_ReturnsNull()
        {
            // Arrange
            var project = new Project();
            // Ensuring Children has no target with the specified name.
            project.Children.Clear();

            // Act
            var foundTarget = project.FindTarget("NonExistentTarget");

            // Assert
            foundTarget.Should().BeNull();
        }

        /// <summary>
        /// Tests the AdornmentString property to ensure that it returns a non-null string.
        /// Note: Further detailed tests may be required depending on the implementation of GetAdornmentString.
        /// </summary>
        [Fact]
        public void AdornmentString_Get_ReturnsNonNullString()
        {
            // Arrange
            var project = new Project();

            // Act
            string adornment = project.AdornmentString;

            // Assert
            adornment.Should().NotBeNull();
        }
    }
}
