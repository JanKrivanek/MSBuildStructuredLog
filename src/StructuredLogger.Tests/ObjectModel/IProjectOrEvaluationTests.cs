using System;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// A dummy implementation of the IProjectOrEvaluation interface for unit testing.
    /// </summary>
    public class DummyProjectOrEvaluation : IProjectOrEvaluation
    {
        public string ProjectFile { get; set; }
        public string TargetFramework { get; set; }
        public string Platform { get; set; }
        public string Configuration { get; set; }
    }

    /// <summary>
    /// Unit tests for the <see cref="ProjectOrEvaluationHelper"/> class.
    /// </summary>
    public class ProjectOrEvaluationHelperTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectOrEvaluationHelperTests"/> class.
        /// Resets static state before each test.
        /// </summary>
        public ProjectOrEvaluationHelperTests()
        {
            ProjectOrEvaluationHelper.ClearCache();
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = false;
        }

        /// <summary>
        /// Tests that GetAdornmentString returns only the TargetFramework when ShowConfigurationAndPlatform is false.
        /// Expected outcome: adornment string equals the TargetFramework value.
        /// </summary>
        [Fact]
        public void GetAdornmentString_ConfigurationOff_ReturnsOnlyTargetFramework()
        {
            // Arrange
            var project = new DummyProjectOrEvaluation
            {
                TargetFramework = "net5.0",
                Configuration = "Debug",
                Platform = "x86"
            };
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = false;

            // Act
            string result = ProjectOrEvaluationHelper.GetAdornmentString(project);

            // Assert
            Assert.Equal("net5.0", result);
        }

        /// <summary>
        /// Tests that GetAdornmentString returns all parts (TargetFramework, Configuration, and Platform) when ShowConfigurationAndPlatform is true.
        /// Expected outcome: adornment string is a comma-separated concatenation of non-empty values.
        /// </summary>
        [Fact]
        public void GetAdornmentString_ConfigurationOn_ReturnsAllParts()
        {
            // Arrange
            var project = new DummyProjectOrEvaluation
            {
                TargetFramework = "netcoreapp3.1",
                Configuration = "Release",
                Platform = "AnyCPU"
            };
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = true;

            // Act
            string result = ProjectOrEvaluationHelper.GetAdornmentString(project);

            // Assert
            Assert.Equal("netcoreapp3.1,Release,AnyCPU", result);
        }

        /// <summary>
        /// Tests that GetAdornmentString includes only non-empty properties.
        /// Covers various combinations of empty and non-empty property values.
        /// </summary>
        [Theory]
        [InlineData("", false, "", "", "")]
        [InlineData("", true, "Debug", "", "Debug")]
        [InlineData("", true, "", "x86", "x86")]
        [InlineData("net6.0", true, "", "", "net6.0")]
        [InlineData("net6.0", true, "Release", "", "net6.0,Release")]
        [InlineData("net6.0", true, "", "Arm", "net6.0,Arm")]
        public void GetAdornmentString_MissingValues_ReturnsOnlyNonEmpty(
            string targetFramework,
            bool showConfigAndPlatform,
            string configuration,
            string platform,
            string expected)
        {
            // Arrange
            var project = new DummyProjectOrEvaluation
            {
                TargetFramework = targetFramework,
                Configuration = configuration,
                Platform = platform
            };
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = showConfigAndPlatform;

            // Act
            string result = ProjectOrEvaluationHelper.GetAdornmentString(project);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that repeated calls to GetAdornmentString with the same project instance return the cached value.
        /// Expected outcome: the same result is returned on subsequent calls.
        /// </summary>
        [Fact]
        public void GetAdornmentString_CacheBehavior_ReturnsConsistentResult()
        {
            // Arrange
            var project = new DummyProjectOrEvaluation
            {
                TargetFramework = "net5.0",
                Configuration = "Debug",
                Platform = "x64"
            };
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = true;

            // Act
            string firstCall = ProjectOrEvaluationHelper.GetAdornmentString(project);
            string secondCall = ProjectOrEvaluationHelper.GetAdornmentString(project);

            // Assert
            Assert.Equal(firstCall, secondCall);
        }

        /// <summary>
        /// Tests that ClearCache clears the cached adornment string and forces recalculation when state changes.
        /// Expected outcome: after clearing the cache and changing ShowConfigurationAndPlatform,
        /// the computed adornment string reflects the new configuration.
        /// </summary>
        [Fact]
        public void ClearCache_RecomputesAdornmentStringAfterStateChange()
        {
            // Arrange
            var project = new DummyProjectOrEvaluation
            {
                TargetFramework = "net5.0",
                Configuration = "Debug",
                Platform = "x86"
            };
            // Initially, configuration is off so only TargetFramework is used.
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = false;
            string initialResult = ProjectOrEvaluationHelper.GetAdornmentString(project);
            Assert.Equal("net5.0", initialResult);

            // Act
            ProjectOrEvaluationHelper.ClearCache();
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = true;
            string updatedResult = ProjectOrEvaluationHelper.GetAdornmentString(project);

            // Assert
            Assert.Equal("net5.0,Debug,x86", updatedResult);
        }

        /// <summary>
        /// Tests that calling GetAdornmentString with a null project throws a NullReferenceException.
        /// Expected outcome: a NullReferenceException is thrown.
        /// </summary>
        [Fact]
        public void GetAdornmentString_NullProject_ThrowsNullReferenceException()
        {
            // Arrange
            DummyProjectOrEvaluation project = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => ProjectOrEvaluationHelper.GetAdornmentString(project));
        }
    }
}
