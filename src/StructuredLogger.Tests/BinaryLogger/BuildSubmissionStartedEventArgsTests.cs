using FluentAssertions;
using Moq;
using StructuredLogger.BinaryLogger;
using System;
using System.Collections.Generic;
using Xunit;

namespace StructuredLogger.BinaryLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildSubmissionStartedEventArgs"/> class.
    /// </summary>
    public class BuildSubmissionStartedEventArgsTests
    {
        private readonly BuildSubmissionStartedEventArgs _eventArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildSubmissionStartedEventArgsTests"/> class.
        /// </summary>
        public BuildSubmissionStartedEventArgsTests()
        {
            _eventArgs = new BuildSubmissionStartedEventArgs();
        }

        /// <summary>
        /// Tests that the constructor sets default property values.
        /// Expected: The auto properties have their default values.
        /// </summary>
        [Fact]
        public void Constructor_InitialState_PropertiesAreDefault()
        {
            // Act & Assert
            _eventArgs.GlobalProperties.Should().BeNull("GlobalProperties should be null by default.");
            _eventArgs.EntryProjectsFullPath.Should().BeNull("EntryProjectsFullPath should be null by default.");
            _eventArgs.TargetNames.Should().BeNull("TargetNames should be null by default.");
            _eventArgs.Flags.Should().Be(BuildRequestDataFlags.None, "Flags should default to None.");
            _eventArgs.SubmissionId.Should().Be(0, "SubmissionId should default to 0.");
        }

        /// <summary>
        /// Tests setting and getting GlobalProperties with a valid dictionary.
        /// Expected: The property returns the same dictionary that was assigned.
        /// </summary>
        [Fact]
        public void GlobalProperties_SetAndGet_ReturnsSameDictionary()
        {
            // Arrange
            var properties = new Dictionary<string, string>
            {
                { "Configuration", "Debug" },
                { "Platform", "AnyCPU" }
            };

            // Act
            _eventArgs.GlobalProperties = properties;

            // Assert
            _eventArgs.GlobalProperties.Should().BeEquivalentTo(properties, "GlobalProperties was not correctly set or retrieved.");
        }

        /// <summary>
        /// Tests setting GlobalProperties with an empty dictionary.
        /// Expected: The property returns an empty dictionary.
        /// </summary>
        [Fact]
        public void GlobalProperties_SetEmptyDictionary_ReturnsEmptyDictionary()
        {
            // Arrange
            var emptyProperties = new Dictionary<string, string>();

            // Act
            _eventArgs.GlobalProperties = emptyProperties;

            // Assert
            _eventArgs.GlobalProperties.Should().BeEmpty("GlobalProperties should be empty when an empty dictionary is assigned.");
        }

        /// <summary>
        /// Tests setting and getting EntryProjectsFullPath with a valid list of project paths.
        /// Expected: The property returns the same list that was assigned.
        /// </summary>
        [Fact]
        public void EntryProjectsFullPath_SetAndGet_ReturnsSameCollection()
        {
            // Arrange
            var paths = new List<string>
            {
                "C:\\Projects\\ProjectA.csproj",
                "/usr/local/projects/ProjectB.csproj",
                @"\\NetworkPath\Projects\ProjectC.csproj"
            };

            // Act
            _eventArgs.EntryProjectsFullPath = paths;

            // Assert
            _eventArgs.EntryProjectsFullPath.Should().BeEquivalentTo(paths, "EntryProjectsFullPath was not correctly set or retrieved.");
        }

        /// <summary>
        /// Tests setting and getting TargetNames with a valid list of target names.
        /// Expected: The property returns the same list that was assigned.
        /// </summary>
        [Fact]
        public void TargetNames_SetAndGet_ReturnsSameCollection()
        {
            // Arrange
            var targets = new List<string>
            {
                "Build",
                "Clean",
                "Rebuild"
            };

            // Act
            _eventArgs.TargetNames = targets;

            // Assert
            _eventArgs.TargetNames.Should().BeEquivalentTo(targets, "TargetNames was not correctly set or retrieved.");
        }

        /// <summary>
        /// Tests setting and getting Flags property with various enum values.
        /// Expected: The property returns the same value that was assigned.
        /// </summary>
        [Theory]
        [InlineData(BuildRequestDataFlags.None)]
        [InlineData(BuildRequestDataFlags.ReplaceExistingProjectInstance)]
        [InlineData(BuildRequestDataFlags.ProvideProjectStateAfterBuild)]
        [InlineData(BuildRequestDataFlags.IgnoreExistingProjectState)]
        [InlineData(BuildRequestDataFlags.ClearCachesAfterBuild)]
        [InlineData(BuildRequestDataFlags.SkipNonexistentTargets)]
        [InlineData(BuildRequestDataFlags.ProvideSubsetOfStateAfterBuild)]
        [InlineData(BuildRequestDataFlags.IgnoreMissingEmptyAndInvalidImports)]
        [InlineData(BuildRequestDataFlags.FailOnUnresolvedSdk)]
        public void Flags_SetAndGet_ReturnsSameEnumValue(BuildRequestDataFlags flag)
        {
            // Act
            _eventArgs.Flags = flag;

            // Assert
            _eventArgs.Flags.Should().Be(flag, "Flags property did not return the set enum value.");
        }

        /// <summary>
        /// Tests setting and getting SubmissionId property with a variety of integer values.
        /// Expected: The property returns the same value that was assigned.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void SubmissionId_SetAndGet_ReturnsSameValue(int submissionId)
        {
            // Act
            _eventArgs.SubmissionId = submissionId;

            // Assert
            _eventArgs.SubmissionId.Should().Be(submissionId, "SubmissionId property did not return the set integer value.");
        }
    }
}
