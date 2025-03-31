using FluentAssertions;
using System;
using System.Collections.Generic;
using StructuredLogger.BinaryLogger;
using Xunit;

namespace StructuredLogger.BinaryLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildSubmissionStartedEventArgs"/> class.
    /// </summary>
    public class BuildSubmissionStartedEventArgsTests
    {
        private readonly BuildSubmissionStartedEventArgs _eventArgs;

        public BuildSubmissionStartedEventArgsTests()
        {
            // Arrange: Create an instance of the BuildSubmissionStartedEventArgs.
            _eventArgs = new BuildSubmissionStartedEventArgs();
        }

        /// <summary>
        /// Tests that the GlobalProperties property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void GlobalProperties_SetAndGet_ReturnsSameDictionary()
        {
            // Arrange
            var expectedProperties = new Dictionary<string, string?>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

            // Act
            _eventArgs.GlobalProperties = expectedProperties;
            IDictionary<string, string?> actualProperties = _eventArgs.GlobalProperties;

            // Assert
            actualProperties.Should().BeEquivalentTo(expectedProperties);
        }

        /// <summary>
        /// Tests that the EntryProjectsFullPath property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void EntryProjectsFullPath_SetAndGet_ReturnsSameEnumerable()
        {
            // Arrange
            var expectedPaths = new List<string> { "C:\\Project\\proj1.csproj", "C:\\Project\\proj2.csproj" };

            // Act
            _eventArgs.EntryProjectsFullPath = expectedPaths;
            IEnumerable<string> actualPaths = _eventArgs.EntryProjectsFullPath;

            // Assert
            actualPaths.Should().BeEquivalentTo(expectedPaths);
        }

        /// <summary>
        /// Tests that the TargetNames property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void TargetNames_SetAndGet_ReturnsSameEnumerable()
        {
            // Arrange
            var expectedTargetNames = new List<string> { "Build", "Clean", "Rebuild" };

            // Act
            _eventArgs.TargetNames = expectedTargetNames;
            IEnumerable<string> actualTargetNames = _eventArgs.TargetNames;

            // Assert
            actualTargetNames.Should().BeEquivalentTo(expectedTargetNames);
        }

        /// <summary>
        /// Tests that the Flags property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void Flags_SetAndGet_ReturnsSameFlagValue()
        {
            // Arrange
            BuildRequestDataFlags expectedFlags = BuildRequestDataFlags.ReplaceExistingProjectInstance | BuildRequestDataFlags.ProvideProjectStateAfterBuild;

            // Act
            _eventArgs.Flags = expectedFlags;
            BuildRequestDataFlags actualFlags = _eventArgs.Flags;

            // Assert
            actualFlags.Should().Be(expectedFlags);
        }

        /// <summary>
        /// Tests that the SubmissionId property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void SubmissionId_SetAndGet_ReturnsSameValue()
        {
            // Arrange
            int expectedSubmissionId = 42;

            // Act
            _eventArgs.SubmissionId = expectedSubmissionId;
            int actualSubmissionId = _eventArgs.SubmissionId;

            // Assert
            actualSubmissionId.Should().Be(expectedSubmissionId);
        }
    }
}
