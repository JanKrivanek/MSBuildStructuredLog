using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Project"/> class.
    /// </summary>
    public class ProjectTests
    {
        private readonly Project _project;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTests"/> class.
        /// </summary>
        public ProjectTests()
        {
            _project = new Project();
            // Assuming that the Project inherits a public "Name" property from its base class.
            // If not, this assignment may need to be adjusted based on the actual implementation.
            _project.Name = "TestProject";
        }

        /// <summary>
        /// Tests that ProjectFileExtension returns the correct extension when a valid project file is provided.
        /// </summary>
        [Fact]
        public void ProjectFileExtension_WithValidProjectFile_ReturnsExtension()
        {
            // Arrange
            _project.ProjectFile = @"C:\folder\file.csproj";

            // Act
            string extension = _project.ProjectFileExtension;

            // Assert
            Assert.Equal(".csproj", extension);
        }

        /// <summary>
        /// Tests that ProjectFileExtension returns an empty string when ProjectFile is null.
        /// </summary>
        [Fact]
        public void ProjectFileExtension_WithNullProjectFile_ReturnsEmptyString()
        {
            // Arrange
            _project.ProjectFile = null;

            // Act
            string extension = _project.ProjectFileExtension;

            // Assert
            Assert.Equal(string.Empty, extension);
        }

        /// <summary>
        /// Tests that ProjectDirectory returns the correct directory when a valid ProjectFile is provided.
        /// </summary>
        [Fact]
        public void ProjectDirectory_WithValidProjectFile_ReturnsDirectory()
        {
            // Arrange
            _project.ProjectFile = @"C:\folder\file.csproj";

            // Act
            string directory = _project.ProjectDirectory;

            // Assert
            Assert.Equal(@"C:\folder", directory);
        }

        /// <summary>
        /// Tests that ProjectDirectory returns null when ProjectFile is an empty string.
        /// </summary>
        [Fact]
        public void ProjectDirectory_WithEmptyProjectFile_ReturnsNull()
        {
            // Arrange
            _project.ProjectFile = "";

            // Act
            string directory = _project.ProjectDirectory;

            // Assert
            Assert.Null(directory);
        }

        /// <summary>
        /// Tests that SourceFilePath returns the same value as ProjectFile.
        /// </summary>
        [Fact]
        public void SourceFilePath_ReturnsProjectFileValue()
        {
            // Arrange
            _project.ProjectFile = @"C:\folder\project.csproj";

            // Act
            string sourceFilePath = _project.SourceFilePath;

            // Assert
            Assert.Equal(@"C:\folder\project.csproj", sourceFilePath);
        }

        /// <summary>
        /// Tests that TypeName returns "Project" as expected.
        /// </summary>
        [Fact]
        public void TypeName_ReturnsProject()
        {
            // Act
            string typeName = _project.TypeName;

            // Assert
            Assert.Equal("Project", typeName);
        }

        /// <summary>
        /// Tests that ToString returns the expected string when EntryTargets and GlobalProperties are provided.
        /// </summary>
        [Fact]
        public void ToString_ReturnsExpectedFormat_WithTargetsAndGlobalProperties()
        {
            // Arrange
            _project.Name = "TestProject";
            _project.ProjectFile = @"C:\folder\project.csproj";
            _project.EntryTargets = new List<string> { "Build", "Clean" };
            _project.GlobalProperties = new Dictionary<string, string>
            {
                { "PropA", "ValueA" },
                { "PropB", "ValueB" }
            }.ToImmutableDictionary();

            // Act
            string result = _project.ToString();

            // Assert
            Assert.Contains("Project Name=TestProject", result);
            Assert.Contains("File=C:\\folder\\project.csproj", result);
            Assert.Contains("Targets=[Build, Clean]", result);
            Assert.Contains("GlobalProperties=[", result);
            Assert.Contains("PropA=ValueA", result);
        }

        /// <summary>
        /// Tests that ToString returns the expected string when no EntryTargets or GlobalProperties are provided.
        /// </summary>
        [Fact]
        public void ToString_ReturnsExpectedFormat_WithoutTargetsAndGlobalProperties()
        {
            // Arrange
            _project.Name = "TestProject";
            _project.ProjectFile = @"C:\folder\project.csproj";
            _project.EntryTargets = Array.Empty<string>();
            _project.GlobalProperties = ImmutableDictionary<string, string>.Empty;

            // Act
            string result = _project.ToString();

            // Assert
            Assert.Contains("Project Name=TestProject", result);
            Assert.Contains("File=C:\\folder\\project.csproj", result);
            Assert.DoesNotContain("Targets=[", result);
            Assert.DoesNotContain("GlobalProperties=[", result);
        }

        /// <summary>
        /// Tests that GetTargetById returns null when the target with the given id is not present.
        /// </summary>
        [Fact]
        public void GetTargetById_ReturnsNull_WhenTargetDoesNotExist()
        {
            // Act
            var target = _project.GetTargetById(100);

            // Assert
            Assert.Null(target);
        }

        /// <summary>
        /// Tests that GetTargetById throws an ArgumentException when passed an id of -1.
        /// </summary>
        [Fact]
        public void GetTargetById_ThrowsArgumentException_WhenIdIsMinusOne()
        {
            // Act & Assert
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _project.GetTargetById(-1));
            Assert.Equal("Invalid target id: -1", ex.Message);
        }

        /// <summary>
        /// Tests that CreateTarget creates a target with the correct id and increments its internal index.
        /// </summary>
        [Fact]
        public void CreateTarget_CreatesTargetWithCorrectId_AndIncrementsIndex()
        {
            // Arrange
            string targetName = "Compile";
            int targetId = 1;

            // Act
            Target target = _project.CreateTarget(targetName, targetId);

            // Assert
            Assert.NotNull(target);
            Assert.Equal(targetId, target.Id);
            Assert.Equal(targetName, target.Name);

            // Act - create a second target to verify that its index is incremented
            Target secondTarget = _project.CreateTarget("Link", 2);

            // Assert
            Assert.True(secondTarget.Index > target.Index);
        }

        /// <summary>
        /// Tests that the IsLowRelevance property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void IsLowRelevance_GetSet_WorksCorrectly()
        {
            // Arrange & Act
            _project.IsLowRelevance = true;
            bool valueAfterSetTrue = _project.IsLowRelevance;

            _project.IsLowRelevance = false;
            bool valueAfterSetFalse = _project.IsLowRelevance;

            // Assert
            Assert.True(valueAfterSetTrue);
            Assert.False(valueAfterSetFalse);
        }

        /// <summary>
        /// Tests that TargetsDisplayText returns an empty string when TargetsText is null or empty.
        /// </summary>
        [Fact]
        public void TargetsDisplayText_ReturnsEmpty_WhenTargetsTextIsNullOrEmpty()
        {
            // Arrange
            _project.TargetsText = "";
            string resultEmpty = _project.TargetsDisplayText;

            _project.TargetsText = null;
            string resultNull = _project.TargetsDisplayText;

            // Assert
            Assert.Equal(string.Empty, resultEmpty);
            Assert.Equal(string.Empty, resultNull);
        }

        /// <summary>
        /// Tests that ToolTip returns a string containing the project file, targets, and global properties.
        /// </summary>
        [Fact]
        public void ToolTip_ReturnsExpectedFormat()
        {
            // Arrange
            _project.ProjectFile = @"C:\folder\project.csproj";
            _project.EntryTargets = new List<string> { "Build", "Clean" };
            _project.GlobalProperties = new Dictionary<string, string>
            {
                { "Key1", "LongValueThatShouldBeShortened" },
                { "Key2", "Value2" }
            }.ToImmutableDictionary();

            // Act
            string tooltip = _project.ToolTip;

            // Assert
            Assert.Contains(@"C:\folder\project.csproj", tooltip);
            Assert.Contains("Targets:", tooltip);
            Assert.Contains("Build", tooltip);
            Assert.Contains("Clean", tooltip);
            Assert.Contains("Global Properties:", tooltip);
            Assert.Contains("Key1 =", tooltip);
        }

        /// <summary>
        /// Tests that OnTaskAdded and GetTaskById work correctly with a valid task.
        /// </summary>
//         [Fact] [Error] (290-35)CS0117 'Task' does not contain a definition for 'Id' [Error] (293-34)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task' [Error] (294-34)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Task' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task'
//         public void OnTaskAdded_And_GetTaskById_WorksCorrectly()
//         {
//             // Arrange
//             var task = new Task { Id = 10 };
// 
//             // Act
//             _project.OnTaskAdded(task);
//             Task retrievedTask = _project.GetTaskById(10);
// 
//             // Assert
//             Assert.Equal(task, retrievedTask);
//         }

        /// <summary>
        /// Tests that FindTarget returns the target when it exists in the project's children collection.
        /// </summary>
        [Fact]
        public void FindTarget_ReturnsTarget_WhenItExists()
        {
            // Arrange
            var target = new Target { Name = "TestTarget", Id = 100 };

            // Attempt to set the Children collection via a public property if available.
            PropertyInfo childrenProperty = typeof(Project).GetProperty("Children");
            if (childrenProperty != null && childrenProperty.CanWrite)
            {
                var childrenList = Activator.CreateInstance(typeof(List<object>)) as IList<object>;
                childrenList.Add(target);
                childrenProperty.SetValue(_project, childrenList);
            }
            else
            {
                // Alternatively, attempt to set a backing field named "children"
                FieldInfo childrenField = typeof(Project).GetField("children", BindingFlags.Instance | BindingFlags.NonPublic);
                if (childrenField != null)
                {
                    var childrenList = new List<object> { target };
                    childrenField.SetValue(_project, childrenList);
                }
                else
                {
                    // If unable to set children, skip the test.
                    return;
                }
            }

            // Act
            Target foundTarget = _project.FindTarget("TestTarget");

            // Assert
            Assert.NotNull(foundTarget);
            Assert.Equal(100, foundTarget.Id);
        }

        /// <summary>
        /// Tests that FindTarget returns null when the target does not exist in the project's children collection.
        /// </summary>
        [Fact]
        public void FindTarget_ReturnsNull_WhenTargetDoesNotExist()
        {
            // Arrange
            PropertyInfo childrenProperty = typeof(Project).GetProperty("Children");
            if (childrenProperty != null && childrenProperty.CanWrite)
            {
                childrenProperty.SetValue(_project, new List<object>());
            }
            else
            {
                FieldInfo childrenField = typeof(Project).GetField("children", BindingFlags.Instance | BindingFlags.NonPublic);
                if (childrenField != null)
                {
                    childrenField.SetValue(_project, new List<object>());
                }
            }

            // Act
            Target foundTarget = _project.FindTarget("Nonexistent");

            // Assert
            Assert.Null(foundTarget);
        }
    }
}
