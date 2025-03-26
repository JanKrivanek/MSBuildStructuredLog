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

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildSubmissionStartedEventArgsTests"/> class.
        /// </summary>
        public BuildSubmissionStartedEventArgsTests()
        {
            _eventArgs = new BuildSubmissionStartedEventArgs();
        }

        /// <summary>
        /// Tests that the constructor of BuildSubmissionStartedEventArgs initializes base properties correctly.
        /// Expected outcome: The Message property is an empty string and the Timestamp is recent.
        /// </summary>
        [Fact]
        public void Constructor_DefaultValues_AreSet()
        {
            // Assert
            Assert.Equal(string.Empty, _eventArgs.Message);
            // Validate that the Timestamp is within a reasonable range (e.g., within 5 seconds of current UTC time)
            Assert.True((DateTime.UtcNow - _eventArgs.Timestamp).TotalSeconds < 5, "Timestamp is not recent.");
        }

        /// <summary>
        /// Tests that the GlobalProperties property correctly stores and retrieves values.
        /// Expected outcome: The set dictionary is retrieved without modification.
        /// </summary>
        [Fact]
        public void GlobalProperties_SetAndGet_ReturnsExpectedValue()
        {
            // Arrange
            var expectedProperties = new Dictionary<string, string?>
            {
                { "Configuration", "Debug" },
                { "Platform", null }
            };

            // Act
            _eventArgs.GlobalProperties = expectedProperties;

            // Assert
            Assert.Equal(expectedProperties, _eventArgs.GlobalProperties);
        }

        /// <summary>
        /// Tests that the EntryProjectsFullPath property correctly stores and retrieves values.
        /// Expected outcome: The set list of project paths is retrieved without modification.
        /// </summary>
        [Fact]
        public void EntryProjectsFullPath_SetAndGet_ReturnsExpectedValue()
        {
            // Arrange
            var expectedPaths = new List<string> { "C:\\Project\\proj1.csproj", "C:\\Project\\proj2.csproj" };

            // Act
            _eventArgs.EntryProjectsFullPath = expectedPaths;

            // Assert
            Assert.Equal(expectedPaths, _eventArgs.EntryProjectsFullPath);
        }

        /// <summary>
        /// Tests that the TargetNames property correctly stores and retrieves values.
        /// Expected outcome: The set list of target names is retrieved without modification.
        /// </summary>
        [Fact]
        public void TargetNames_SetAndGet_ReturnsExpectedValue()
        {
            // Arrange
            var expectedTargets = new List<string> { "Build", "Clean" };

            // Act
            _eventArgs.TargetNames = expectedTargets;

            // Assert
            Assert.Equal(expectedTargets, _eventArgs.TargetNames);
        }

        /// <summary>
        /// Tests that the Flags property correctly stores and retrieves values.
        /// Expected outcome: The set combination of flags is retrieved as expected.
        /// </summary>
        [Fact]
        public void Flags_SetAndGet_ReturnsExpectedValue()
        {
            // Arrange
            var expectedFlags = BuildRequestDataFlags.ProvideProjectStateAfterBuild | BuildRequestDataFlags.ClearCachesAfterBuild;

            // Act
            _eventArgs.Flags = expectedFlags;

            // Assert
            Assert.Equal(expectedFlags, _eventArgs.Flags);
        }

        /// <summary>
        /// Tests that the SubmissionId property correctly stores and retrieves values.
        /// Expected outcome: The set submission identifier is retrieved without modification.
        /// </summary>
        [Fact]
        public void SubmissionId_SetAndGet_ReturnsExpectedValue()
        {
            // Arrange
            int expectedId = 100;

            // Act
            _eventArgs.SubmissionId = expectedId;

            // Assert
            Assert.Equal(expectedId, _eventArgs.SubmissionId);
        }
    }
}
