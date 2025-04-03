using FluentAssertions;
using Moq;
using System;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ProjectOrEvaluationHelper"/> class.
    /// </summary>
    public class ProjectOrEvaluationHelperTests
    {
        // Reset static fields before each test if needed.
        public ProjectOrEvaluationHelperTests()
        {
            // Ensure a known starting state.
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = false;
            ProjectOrEvaluationHelper.ClearCache();
        }

        /// <summary>
        /// Tests the GetAdornmentString extension method when ShowConfigurationAndPlatform is false.
        /// Expected to return only the TargetFramework.
        /// </summary>
        [Fact]
        public void GetAdornmentString_WhenShowConfigurationAndPlatformFalse_ReturnsOnlyTargetFramework()
        {
            // Arrange
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = false;
            var targetFramework = "net5.0";
            var mockProject = new Mock<IProjectOrEvaluation>();
            mockProject.SetupGet(p => p.TargetFramework).Returns(targetFramework);
            // Even if Configuration and Platform have values, they should be ignored.
            mockProject.SetupGet(p => p.Configuration).Returns("Debug");
            mockProject.SetupGet(p => p.Platform).Returns("x86");

            // Act
            string resultFirstCall = ProjectOrEvaluationHelper.GetAdornmentString(mockProject.Object);
            string resultSecondCall = ProjectOrEvaluationHelper.GetAdornmentString(mockProject.Object);

            // Assert
            resultFirstCall.Should().Be(targetFramework, "because only the TargetFramework should be used when ShowConfigurationAndPlatform is false");
            resultSecondCall.Should().Be(targetFramework, "because caching should return the same result on subsequent calls");
        }

        /// <summary>
        /// Tests the GetAdornmentString extension method when ShowConfigurationAndPlatform is true.
        /// Expected to return the concatenated TargetFramework, Configuration, and Platform.
        /// </summary>
        [Fact]
        public void GetAdornmentString_WhenShowConfigurationAndPlatformTrue_ReturnsAdornmentWithAllFields()
        {
            // Arrange
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = true;
            var targetFramework = "net5.0";
            var configuration = "Debug";
            var platform = "x86";
            var expectedAdornment = $"{targetFramework},{configuration},{platform}";

            var mockProject = new Mock<IProjectOrEvaluation>();
            mockProject.SetupGet(p => p.TargetFramework).Returns(targetFramework);
            mockProject.SetupGet(p => p.Configuration).Returns(configuration);
            mockProject.SetupGet(p => p.Platform).Returns(platform);

            // Act
            string result = ProjectOrEvaluationHelper.GetAdornmentString(mockProject.Object);

            // Assert
            result.Should().Be(expectedAdornment, "because all non-empty properties should be concatenated when ShowConfigurationAndPlatform is true");
        }

        /// <summary>
        /// Tests the GetAdornmentString extension method when ShowConfigurationAndPlatform is true,
        /// but Configuration and Platform are empty. Expected to return only the TargetFramework.
        /// </summary>
        [Fact]
        public void GetAdornmentString_WhenShowConfigurationAndPlatformTrueWithEmptyConfigurationAndPlatform_ReturnsOnlyTargetFramework()
        {
            // Arrange
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = true;
            var targetFramework = "net5.0";
            var expectedAdornment = targetFramework;

            var mockProject = new Mock<IProjectOrEvaluation>();
            mockProject.SetupGet(p => p.TargetFramework).Returns(targetFramework);
            mockProject.SetupGet(p => p.Configuration).Returns(string.Empty);
            mockProject.SetupGet(p => p.Platform).Returns(string.Empty);

            // Act
            string result = ProjectOrEvaluationHelper.GetAdornmentString(mockProject.Object);

            // Assert
            result.Should().Be(expectedAdornment, "because only the non-empty TargetFramework should be included when other properties are empty");
        }

        /// <summary>
        /// Tests the GetAdornmentString extension method when TargetFramework is empty and ShowConfigurationAndPlatform is false.
        /// Expected to return an empty string.
        /// </summary>
        [Fact]
        public void GetAdornmentString_WhenTargetFrameworkEmptyAndShowConfigurationAndPlatformFalse_ReturnsEmptyString()
        {
            // Arrange
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = false;
            var mockProject = new Mock<IProjectOrEvaluation>();
            mockProject.SetupGet(p => p.TargetFramework).Returns(string.Empty);
            // Even if these have values, they should be ignored.
            mockProject.SetupGet(p => p.Configuration).Returns("Debug");
            mockProject.SetupGet(p => p.Platform).Returns("x86");

            // Act
            string result = ProjectOrEvaluationHelper.GetAdornmentString(mockProject.Object);

            // Assert
            result.Should().Be(string.Empty, "because an empty TargetFramework should result in no adornment when ShowConfigurationAndPlatform is false");
        }

        /// <summary>
        /// Tests the GetAdornmentString extension method when TargetFramework is empty and ShowConfigurationAndPlatform is true.
        /// Expected to return the concatenation of Configuration and Platform.
        /// </summary>
        [Fact]
        public void GetAdornmentString_WhenTargetFrameworkEmptyAndShowConfigurationAndPlatformTrue_ReturnsRemainingFields()
        {
            // Arrange
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = true;
            var configuration = "Debug";
            var platform = "x86";
            var expectedAdornment = $"{configuration},{platform}";

            var mockProject = new Mock<IProjectOrEvaluation>();
            mockProject.SetupGet(p => p.TargetFramework).Returns(string.Empty);
            mockProject.SetupGet(p => p.Configuration).Returns(configuration);
            mockProject.SetupGet(p => p.Platform).Returns(platform);

            // Act
            string result = ProjectOrEvaluationHelper.GetAdornmentString(mockProject.Object);

            // Assert
            result.Should().Be(expectedAdornment, "because when TargetFramework is empty, only non-empty Configuration and Platform should be concatenated");
        }

        /// <summary>
        /// Tests the GetAdornmentString extension method when a null project is passed.
        /// Expected to throw a NullReferenceException.
        /// </summary>
        [Fact]
        public void GetAdornmentString_WhenProjectIsNull_ThrowsNullReferenceException()
        {
            // Arrange
            IProjectOrEvaluation? nullProject = null;

            // Act
            Action act = () => ProjectOrEvaluationHelper.GetAdornmentString(nullProject!);

            // Assert
            act.Should().Throw<NullReferenceException>("because passing null to an extension method that accesses instance members should throw");
        }

        /// <summary>
        /// Tests the ClearCache method to ensure that the cache is reset.
        /// After clearing the cache, a subsequent call to GetAdornmentString should recompute the adornment.
        /// </summary>
        [Fact]
        public void ClearCache_WhenCalled_ResetsCacheAndRecomputesAdornment()
        {
            // Arrange
            ProjectOrEvaluationHelper.ShowConfigurationAndPlatform = true;
            var targetFramework = "net5.0";
            var configuration = "Debug";
            var platform = "x86";
            var expectedAdornment = $"{targetFramework},{configuration},{platform}";

            // Using dynamic property values via local variables.
            string dynamicTarget = targetFramework;
            string dynamicConfiguration = configuration;
            string dynamicPlatform = platform;

            var mockProject = new Mock<IProjectOrEvaluation>();
            mockProject.SetupGet(p => p.TargetFramework).Returns(() => dynamicTarget);
            mockProject.SetupGet(p => p.Configuration).Returns(() => dynamicConfiguration);
            mockProject.SetupGet(p => p.Platform).Returns(() => dynamicPlatform);

            // Act - first call to cache the adornment.
            string cachedAdornment = ProjectOrEvaluationHelper.GetAdornmentString(mockProject.Object);
            // Change the dynamic properties
            dynamicTarget = "net6.0";
            dynamicConfiguration = "Release";
            dynamicPlatform = "x64";

            // Without clearing cache, a new adornment should be computed because key has changed.
            string newAdornment = ProjectOrEvaluationHelper.GetAdornmentString(mockProject.Object);

            // Clear the cache.
            ProjectOrEvaluationHelper.ClearCache();

            // After clearing the cache, the adornment is recomputed with the current property values.
            string recomputedAdornment = ProjectOrEvaluationHelper.GetAdornmentString(mockProject.Object);

            // Assert
            cachedAdornment.Should().Be(expectedAdornment, "because the initial values were used for the first call");
            newAdornment.Should().Be("net6.0,Release,x64", "because the key is computed from the updated property values and was not cached previously");
            recomputedAdornment.Should().Be("net6.0,Release,x64", "because clearing the cache forces recomputation of the adornment with current property values");
        }
    }
}
