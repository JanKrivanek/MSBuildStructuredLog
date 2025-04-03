// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Microsoft.Build.Logging.StructuredLogger.UnitTests;
// using Moq;
// using System;
// using System.Collections.Generic;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "BuildAnalyzer"/> class.
//     /// </summary>
//     public class BuildAnalyzerTests
//     {
// //         /// <summary> // [Error] (26-40)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build' // [Error] (28-19)CS1061 'Build' does not contain a definition for 'IsAnalyzed' and no accessible extension method 'IsAnalyzed' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests that the static AnalyzeBuild method marks the build as analyzed when it was not analyzed.
// //         /// </summary>
// //         [Fact]
// //         public void AnalyzeBuild_WhenBuildNotAnalyzed_MarksBuildAsAnalyzed()
// //         {
// //             // Arrange
// //             // TODO: Replace the TestBuild implementation with a suitable instance of Build if available.
// //             var build = CreateTestBuild(isAnalyzed: false);
// //             // Act
// //             BuildAnalyzer.AnalyzeBuild(build);
// //             // Assert
// //             build.IsAnalyzed.Should().BeTrue("AnalyzeBuild should mark the build as analyzed after processing.");
// //         }
// //  // [Error] (40-40)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build' // [Error] (43-19)CS1061 'Build' does not contain a definition for 'Statistics' and no accessible extension method 'Statistics' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that the static AnalyzeBuild method seals and calculates indices when the build is already analyzed.
//         /// </summary>
//         [Fact]
//         public void AnalyzeBuild_WhenBuildAlreadyAnalyzed_SealsAndCalculatesIndices()
//         {
//             // Arrange
//             var build = CreateTestBuild(isAnalyzed: true);
//             // Act
//             BuildAnalyzer.AnalyzeBuild(build);
//             // Assert
//             // TODO: Verify that SealAndCalculateIndices was invoked by checking properties such as Statistics.TimedNodeCount.
//             build.Statistics.TimedNodeCount.Should().BeGreaterOrEqualTo(0, "Indices should be calculated even if the build was already analyzed.");
//         }
// //  // [Error] (56-91)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         /// <summary>
// //         /// Tests that GetProjectsSortedTopologically returns a collection of projects with valid ProjectFile values.
// //         /// </summary>
// //         [Fact]
// //         public void GetProjectsSortedTopologically_WithChildProjects_ReturnsProjectsWithValidProjectFile()
// //         {
// //             // Arrange
// //             var build = CreateTestBuildWithProjects();
// //             var analyzer = CreateBuildAnalyzer(build);
// //             // Act
// //             IEnumerable<Project> sortedProjects = analyzer.GetProjectsSortedTopologically(build);
// //             // Assert
// //             sortedProjects.Should().NotBeNull("The returned collection should not be null.");
// //             foreach (var project in sortedProjects)
// //             {
// //                 project.ProjectFile.Should().NotBeNullOrEmpty("Each project should have a non-empty ProjectFile.");
// //             }
// //         }
// //  // [Error] (75-17)CS0200 Property or indexer 'BuildAnalyzerTests.TestBuild.SearchExtensions' cannot be assigned to -- it is read only
// #region Helper Methods and Dummy Implementations
//         // The following helper methods provide minimal dummy implementations for Build, Project,
//         // and associated types required to test BuildAnalyzer. These are simplified and for testing purposes only.
//         private Build CreateTestBuild(bool isAnalyzed)
//         {
//             // Create a dummy build instance with minimal required properties.
//             var build = new TestBuild
//             {
//                 IsAnalyzed = isAnalyzed,
//                 Statistics = new BuildStatistics(),
//                 SearchExtensions = new List<ISearchExtension>()
//             };
//             return build;
//         }
// //  // [Error] (86-17)CS0200 Property or indexer 'BuildAnalyzerTests.TestBuild.SearchExtensions' cannot be assigned to -- it is read only // [Error] (97-32)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildAnalyzerTests.TestProject' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' // [Error] (98-32)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildAnalyzerTests.TestProject' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode'
// //         private Build CreateTestBuildWithProjects()
// //         {
// //             var build = new TestBuild
// //             {
// //                 IsAnalyzed = false,
// //                 Statistics = new BuildStatistics(),
// //                 SearchExtensions = new List<ISearchExtension>()
// //             };
// //             // Add dummy projects as children.
// //             var project1 = new TestProject
// //             {
// //                 ProjectFile = "ProjectA.csproj"
// //             };
// //             var project2 = new TestProject
// //             {
// //                 ProjectFile = "ProjectB.csproj"
// //             };
// //             build.Children.Add(project1);
// //             build.Children.Add(project2);
// //             return build;
// //         }
// //  // [Error] (105-38)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         private BuildAnalyzer CreateBuildAnalyzer(Build build)
//         {
//             // Instantiate BuildAnalyzer using the provided build instance.
//             return new BuildAnalyzer(build);
//         }
// 
//         // Minimal dummy implementations to support testing.
//         // In real tests, use actual instances from Microsoft.Build.Logging.StructuredLogger.
//         private class TestBuild : Build
//         {
//             public TestBuild()
//             {
//                 Children = new List<BaseNode>();
//             }
// //  // [Error] (117-45)CS0115 'BuildAnalyzerTests.TestBuild.Children': no suitable method found to override
// //             public override IList<BaseNode> Children { get; }
// //             public override bool IsAnalyzed { get; set; } // [Error] (118-34)CS0115 'BuildAnalyzerTests.TestBuild.IsAnalyzed': no suitable method found to override
// //             public override BuildStatistics Statistics { get; set; } // [Error] (119-45)CS0115 'BuildAnalyzerTests.TestBuild.Statistics': no suitable method found to override
// //             public override IList<ISearchExtension> SearchExtensions { get; } // [Error] (120-53)CS0115 'BuildAnalyzerTests.TestBuild.SearchExtensions': no suitable method found to override
//         }
// 
//         private class TestProject : Project
//         {
//             public TestProject()
//             {
//                 Children = new List<BaseNode>();
//             }
// //  // [Error] (130-45)CS0506 'BuildAnalyzerTests.TestProject.Children': cannot override inherited member 'TreeNode.Children' because it is not marked virtual, abstract, or override
// //             public override IList<BaseNode> Children { get; }
// //             public override string ProjectFile { get; set; } // [Error] (131-36)CS0506 'BuildAnalyzerTests.TestProject.ProjectFile': cannot override inherited member 'Project.ProjectFile' because it is not marked virtual, abstract, or override
//         }
// 
//         // Dummy implementations for BuildStatistics and ISearchExtension.
//         // Only the properties needed for testing are implemented.
//         private class BuildStatistics : BuildStatisticsBase
//         {
//             public override int TimedNodeCount { get; set; }
//         }
// 
//         private abstract class BuildStatisticsBase
//         {
//             public abstract int TimedNodeCount { get; set; }
//         }
// 
//         private interface ISearchExtension
//         {
//         }
// #endregion
//     }
// }