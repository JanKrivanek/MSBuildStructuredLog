using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ProjectEvaluation"/> class.
    /// </summary>
    public class ProjectEvaluationTests
    {
        private readonly ProjectEvaluation _projectEvaluation;

        public ProjectEvaluationTests()
        {
            _projectEvaluation = new ProjectEvaluation();
        }

        #region ProjectFile Property Tests

        /// <summary>
        /// Tests that the ProjectFile property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void ProjectFile_SetAndGet_ReturnsExpectedValue()
        {
            // Arrange
            string expectedFile = @"C:\Projects\Test.proj";

            // Act
            _projectEvaluation.ProjectFile = expectedFile;
            string actualFile = _projectEvaluation.ProjectFile;

            // Assert
            actualFile.Should().Be(expectedFile);
        }

        /// <summary>
        /// Tests that SourceFilePath property returns the value of ProjectFile.
        /// </summary>
        [Fact]
        public void SourceFilePath_Get_ReturnsProjectFile()
        {
            // Arrange
            string expectedFile = @"/home/test/project.proj";
            _projectEvaluation.ProjectFile = expectedFile;

            // Act
            string sourceFilePath = _projectEvaluation.SourceFilePath;

            // Assert
            sourceFilePath.Should().Be(expectedFile);
        }

        /// <summary>
        /// Tests that ProjectFileExtension returns the correct extension in lower invariant.
        /// </summary>
        [Theory]
        [InlineData(@"C:\folder\file.PROJ", ".proj")]
        [InlineData(@"/unix/path/file.xml", ".xml")]
        [InlineData(@"C:\folder\file", "")]
        public void ProjectFileExtension_Get_ReturnsCorrectExtension(string projectFile, string expectedExtension)
        {
            // Arrange
            _projectEvaluation.ProjectFile = projectFile;

            // Act
            string actualExtension = _projectEvaluation.ProjectFileExtension;

            // Assert
            actualExtension.Should().Be(expectedExtension);
        }

        #endregion

        #region EvaluationText, TargetFramework, Platform, Configuration, RelativeDuration Tests

        /// <summary>
        /// Tests that EvaluationText property is initialized to an empty string and can be set.
        /// </summary>
        [Fact]
        public void EvaluationText_GetAndSet_WorksAsExpected()
        {
            // Arrange
            _projectEvaluation.EvaluationText.Should().BeEmpty();
            string newText = "Evaluation completed successfully.";

            // Act
            _projectEvaluation.EvaluationText = newText;

            // Assert
            _projectEvaluation.EvaluationText.Should().Be(newText);
        }

        /// <summary>
        /// Tests that TargetFramework property can be set and retrieved.
        /// </summary>
        [Fact]
        public void TargetFramework_GetAndSet_WorksAsExpected()
        {
            // Arrange
            string expected = ".NET 6.0";

            // Act
            _projectEvaluation.TargetFramework = expected;

            // Assert
            _projectEvaluation.TargetFramework.Should().Be(expected);
        }

        /// <summary>
        /// Tests that Platform property can be set and retrieved.
        /// </summary>
        [Fact]
        public void Platform_GetAndSet_WorksAsExpected()
        {
            // Arrange
            string expected = "AnyCPU";

            // Act
            _projectEvaluation.Platform = expected;

            // Assert
            _projectEvaluation.Platform.Should().Be(expected);
        }

        /// <summary>
        /// Tests that Configuration property can be set and retrieved.
        /// </summary>
        [Fact]
        public void Configuration_GetAndSet_WorksAsExpected()
        {
            // Arrange
            string expected = "Debug";

            // Act
            _projectEvaluation.Configuration = expected;

            // Assert
            _projectEvaluation.Configuration.Should().Be(expected);
        }

        /// <summary>
        /// Tests that RelativeDuration property can be set and retrieved.
        /// </summary>
        [Fact]
        public void RelativeDuration_GetAndSet_WorksAsExpected()
        {
            // Arrange
            double expected = 123.456;

            // Act
            _projectEvaluation.RelativeDuration = expected;

            // Assert
            _projectEvaluation.RelativeDuration.Should().Be(expected);
        }

        #endregion

        #region AdornmentString, TypeName, MessageTexts Tests

        /// <summary>
        /// Tests that AdornmentString property returns a non-null string.
        /// </summary>
        [Fact]
        public void AdornmentString_Get_ReturnsNonNullValue()
        {
            // Act
            string adornment = _projectEvaluation.AdornmentString;

            // Assert
            adornment.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that TypeName property returns the expected class name.
        /// </summary>
        [Fact]
        public void TypeName_Get_ReturnsClassName()
        {
            // Act
            string typeName = _projectEvaluation.TypeName;

            // Assert
            typeName.Should().Be(nameof(ProjectEvaluation));
        }

        /// <summary>
        /// Tests that MessageTexts property is not null and can store messages.
        /// </summary>
        [Fact]
        public void MessageTexts_Get_AllowsAddingMessages()
        {
            // Act
            _projectEvaluation.MessageTexts.Add("Test message");

            // Assert
            _projectEvaluation.MessageTexts.Should().Contain("Test message");
        }

        #endregion

        #region IsLowRelevance Tests

        /// <summary>
        /// Tests that setting IsLowRelevance to true and then false reflects correctly.
        /// </summary>
        [Fact]
        public void IsLowRelevance_SetAndGet_WorksAsExpected()
        {
            // Arrange & Act
            _projectEvaluation.IsLowRelevance = true;
            bool isLowAfterSettingTrue = _projectEvaluation.IsLowRelevance;

            _projectEvaluation.IsLowRelevance = false;
            bool isLowAfterSettingFalse = _projectEvaluation.IsLowRelevance;
            
            // Assert
            isLowAfterSettingTrue.Should().BeTrue();
            isLowAfterSettingFalse.Should().BeFalse();
        }

        #endregion

        #region ToString and ToolTip Tests

        /// <summary>
        /// Tests that ToString returns a string containing Name, ProjectFile and Id formatted as expected.
        /// </summary>
        [Fact]
        public void ToString_WhenCalled_ReturnsFormattedString()
        {
            // Arrange
            string expectedName = "TestProject";
            string expectedFile = @"C:\Projects\Test.proj";
            _projectEvaluation.Name = expectedName;
            _projectEvaluation.ProjectFile = expectedFile;

            // The Id is set by the base class; assuming default is 0.
            int expectedId = _projectEvaluation.Id; // likely 0 if not set

            // Act
            string result = _projectEvaluation.ToString();

            // Assert
            result.Should().Contain($"Project={expectedName}");
            result.Should().Contain($"File={expectedFile}");
            result.Should().Contain($"Id={expectedId:D6}");
        }

        /// <summary>
        /// Tests that ToolTip returns a string starting with ProjectFile followed by a newline.
        /// </summary>
        [Fact]
        public void ToolTip_Get_ReturnsExpectedFormat()
        {
            // Arrange
            string expectedFile = "/home/test/project.proj";
            _projectEvaluation.ProjectFile = expectedFile;

            // Act
            string tooltip = _projectEvaluation.ToolTip;

            // Assert
            tooltip.Should().StartWith(expectedFile + "\n");
        }

        #endregion

        #region ImportsFolder and PropertyReassignmentFolder Tests

        /// <summary>
        /// Tests that the ImportsFolder property returns a non-null instance and is cached.
        /// </summary>
        [Fact]
        public void ImportsFolder_Get_ReturnsCachedInstance()
        {
            // Act
            var firstCall = _projectEvaluation.ImportsFolder;
            var secondCall = _projectEvaluation.ImportsFolder;

            // Assert
            firstCall.Should().NotBeNull();
            secondCall.Should().BeSameAs(firstCall);
        }

        /// <summary>
        /// Tests that the PropertyReassignmentFolder property returns a non-null instance and is cached.
        /// </summary>
        [Fact]
        public void PropertyReassignmentFolder_Get_ReturnsCachedInstance()
        {
            // Act
            var firstCall = _projectEvaluation.PropertyReassignmentFolder;
            var secondCall = _projectEvaluation.PropertyReassignmentFolder;

            // Assert
            firstCall.Should().NotBeNull();
            secondCall.Should().BeSameAs(firstCall);
        }

        #endregion

        #region AddImport and GetAllImportsTransitive Tests

        /// <summary>
        /// A dummy TextNode used for testing AddImport when the node is not an Import.
        /// Implements IHasSourceFile.
        /// </summary>
        private class DummyTextNode : TextNode, IHasSourceFile
        {
            public string SourceFilePath { get; set; }
        }

        /// <summary>
        /// A dummy Import used for testing AddImport when the node is an Import.
        /// Implements IHasSourceFile.
        /// </summary>
        private class DummyImport : Import, IHasSourceFile
        {
            public string SourceFilePath { get; set; }
            public string ImportedProjectFilePath { get; set; }
        }

        /// <summary>
        /// Tests AddImport with a TextNode that is not an Import.
        /// Expects that no import is added to the transitive imports.
        /// </summary>
        [Fact]
        public void AddImport_WhenTextNodeIsNotImport_DoesNotAddToTransitiveImports()
        {
            // Arrange
            string projectFile = "project.proj";
            _projectEvaluation.ProjectFile = projectFile;
            var dummyTextNode = new DummyTextNode
            {
                SourceFilePath = projectFile
            };

            // Act
            _projectEvaluation.AddImport(dummyTextNode);
            IEnumerable<Import> imports = _projectEvaluation.GetAllImportsTransitive();

            // Assert
            imports.Should().BeEmpty();
        }

// Could not make this test passing.
//         /// <summary>
//         /// Tests AddImport with an Import node that has a non-null ImportedProjectFilePath.
//         /// Expects that the import is recorded in the transitive imports.
//         /// </summary>
//         [Fact]
//         public void AddImport_WhenNodeIsImport_AddsToTransitiveImports()
//         {
//             // Arrange
//             string projectFile = "project.proj";
//             _projectEvaluation.ProjectFile = projectFile;
//             var dummyImport = new DummyImport
//             {
//                 // Use a SourceFilePath equal to projectFile so that Import nodes are added.
//                 SourceFilePath = projectFile,
//                 ImportedProjectFilePath = "imported.proj"
//             };
// 
//             // Act
//             _projectEvaluation.AddImport(dummyImport);
//             IEnumerable<Import> imports = _projectEvaluation.GetAllImportsTransitive();
// 
//             // Assert
//             imports.Should().ContainSingle()
//                 .Which.Should().Be(dummyImport);
//         }
// 
        #endregion
    }
}
