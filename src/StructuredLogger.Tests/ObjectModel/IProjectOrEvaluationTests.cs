using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ProjectOrEvaluationHelper"/> class.
    /// </summary>
    public class ProjectOrEvaluationHelperTests
    {
        /// <summary>
        /// A fake implementation of <see cref="IProjectOrEvaluation"/> for testing purposes.
        /// </summary>
        private sealed class FakeProjectOrEvaluation : IProjectOrEvaluation
        {
            public string ProjectFile { get; set; } = string.Empty;
            public string TargetFramework { get; set; } = string.Empty;
            public string Platform { get; set; } = string.Empty;
            public string Configuration { get; set; } = string.Empty;
        }

        /// <summary>
        /// Tests that GetAdornmentString returns only the target framework when ShowConfigurationAndPlatform is false.
        /// </summary>
        [Fact]
        public void GetAdornmentString_ShowConfigurationFalse_ReturnsOnlyTargetFramework()
        {
            // Arrange
            ProjectOrEvaluationHelper.ClearCache();
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = false;
            var project = new FakeProjectOrEvaluation
            {
                TargetFramework = "net5.0",
                Configuration = "Debug",
                Platform = "AnyCPU",
                ProjectFile = "dummy.csproj"
            };

            // Act
            string result = project.GetAdornmentString();

            // Assert
            result.Should().Be("net5.0");
        }

        /// <summary>
        /// Tests that GetAdornmentString returns the concatenated adornment string including configuration and platform when ShowConfigurationAndPlatform is true.
        /// </summary>
        [Fact]
        public void GetAdornmentString_ShowConfigurationTrue_ReturnsFullAdornment()
        {
            // Arrange
            ProjectOrEvaluationHelper.ClearCache();
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = true;
            var project = new FakeProjectOrEvaluation
            {
                TargetFramework = "net6.0",
                Configuration = "Release",
                Platform = "x64",
                ProjectFile = "dummy.csproj"
            };

            // Act
            string result = project.GetAdornmentString();

            // Assert
            result.Should().Be("net6.0,Release,x64");
        }

        /// <summary>
        /// Tests that GetAdornmentString omits empty configuration and platform strings even when ShowConfigurationAndPlatform is true.
        /// </summary>
        [Fact]
        public void GetAdornmentString_EmptyConfigurationAndPlatform_ReturnsOnlyTargetFramework()
        {
            // Arrange
            ProjectOrEvaluationHelper.ClearCache();
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = true;
            var project = new FakeProjectOrEvaluation
            {
                TargetFramework = "net7.0",
                Configuration = string.Empty,
                Platform = string.Empty,
                ProjectFile = "dummy.csproj"
            };

            // Act
            string result = project.GetAdornmentString();

            // Assert
            result.Should().Be("net7.0");
        }

        /// <summary>
        /// Tests that GetAdornmentString returns an empty string when the TargetFramework is empty regardless of the flag.
        /// </summary>
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetAdornmentString_EmptyTargetFramework_ReturnsEmptyString(bool showConfigurationAndPlatform)
        {
            // Arrange
            ProjectOrEvaluationHelper.ClearCache();
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = showConfigurationAndPlatform;
            var project = new FakeProjectOrEvaluation
            {
                TargetFramework = string.Empty,
                Configuration = "AnyConfig",
                Platform = "AnyPlatform",
                ProjectFile = "dummy.csproj"
            };

            // Act
            string result = project.GetAdornmentString();

            // Assert
            result.Should().Be(string.Empty);
        }

        /// <summary>
        /// Tests that ClearCache properly clears the cached adornment strings. After clearing the cache, changes in ShowConfigurationAndPlatform are reflected.
        /// </summary>
        [Fact]
        public void ClearCache_AfterChangingFlag_CacheIsCleared()
        {
            // Arrange
            ProjectOrEvaluationHelper.ClearCache();
            // Initially, set flag to false.
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = false;
            var project = new FakeProjectOrEvaluation
            {
                TargetFramework = "netcoreapp3.1",
                Configuration = "Debug",
                Platform = "AnyCPU",
                ProjectFile = "dummy.csproj"
            };

            // Act
            string resultBeforeCacheClear = project.GetAdornmentString(); // should cache "netcoreapp3.1"
            
            // Change the flag to true.
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = true;
            // Without clearing cache, the result remains the cached value.
            string resultWithoutClearingCache = project.GetAdornmentString();
            
            // Clear the cache.
            ProjectOrEvaluationHelper.ClearCache();
            string resultAfterCacheClear = project.GetAdornmentString();

            // Assert
            resultBeforeCacheClear.Should().Be("netcoreapp3.1");
            resultWithoutClearingCache.Should().Be("netcoreapp3.1", "the cached value should not change when the flag is updated without clearing the cache");
            resultAfterCacheClear.Should().Be("netcoreapp3.1,Debug,AnyCPU", "after clearing the cache, the new adornment string should reflect the updated flag");
        }
    }
}
