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
        private readonly BuildSubmissionStartedEventArgs _args;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildSubmissionStartedEventArgsTests"/> class with default property values.
        /// </summary>
        public BuildSubmissionStartedEventArgsTests()
        {
            _args = new BuildSubmissionStartedEventArgs
            {
                GlobalProperties = new Dictionary<string, string?>(),
                EntryProjectsFullPath = new List<string>(),
                TargetNames = new List<string>(),
                Flags = BuildRequestDataFlags.None,
                SubmissionId = 0
            };
        }

        /// <summary>
        /// Verifies that the GlobalProperties property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void GlobalProperties_SetAndGet_ReturnsSameValue()
        {
            // Arrange
            var testProperties = new Dictionary<string, string?> { { "Key1", "Value1" } };

            // Act
            _args.GlobalProperties = testProperties;

            // Assert
            Assert.Equal(testProperties, _args.GlobalProperties);
        }

        /// <summary>
        /// Verifies that the GlobalProperties property can handle a null assignment.
        /// </summary>
        [Fact]
        public void GlobalProperties_SetToNull_ReturnsNull()
        {
            // Act
            _args.GlobalProperties = null;

            // Assert
            Assert.Null(_args.GlobalProperties);
        }

        /// <summary>
        /// Verifies that the EntryProjectsFullPath property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void EntryProjectsFullPath_SetAndGet_ReturnsSameValue()
        {
            // Arrange
            var testPaths = new List<string> { "C:\\Project\\project.csproj" };

            // Act
            _args.EntryProjectsFullPath = testPaths;

            // Assert
            Assert.Equal(testPaths, _args.EntryProjectsFullPath);
        }

        /// <summary>
        /// Verifies that the EntryProjectsFullPath property can handle a null assignment.
        /// </summary>
        [Fact]
        public void EntryProjectsFullPath_SetToNull_ReturnsNull()
        {
            // Act
            _args.EntryProjectsFullPath = null;

            // Assert
            Assert.Null(_args.EntryProjectsFullPath);
        }

        /// <summary>
        /// Verifies that the TargetNames property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void TargetNames_SetAndGet_ReturnsSameValue()
        {
            // Arrange
            var testTargets = new List<string> { "Build", "Clean" };

            // Act
            _args.TargetNames = testTargets;

            // Assert
            Assert.Equal(testTargets, _args.TargetNames);
        }

        /// <summary>
        /// Verifies that the TargetNames property can handle a null assignment.
        /// </summary>
        [Fact]
        public void TargetNames_SetToNull_ReturnsNull()
        {
            // Act
            _args.TargetNames = null;

            // Assert
            Assert.Null(_args.TargetNames);
        }

        /// <summary>
        /// Verifies that the Flags property can be set to different enum values.
        /// </summary>
        /// <param name="flag">The flag value to test.</param>
        [Theory]
        [InlineData(BuildRequestDataFlags.None)]
        [InlineData(BuildRequestDataFlags.ReplaceExistingProjectInstance)]
        [InlineData(BuildRequestDataFlags.ProvideProjectStateAfterBuild)]
        public void Flags_SetAndGet_ReturnsSameValue(BuildRequestDataFlags flag)
        {
            // Act
            _args.Flags = flag;

            // Assert
            Assert.Equal(flag, _args.Flags);
        }

        /// <summary>
        /// Verifies that the SubmissionId property can be set and retrieved correctly.
        /// </summary>
        /// <param name="submissionId">The submission id value to test.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(123)]
        [InlineData(-1)]
        public void SubmissionId_SetAndGet_ReturnsSameValue(int submissionId)
        {
            // Act
            _args.SubmissionId = submissionId;

            // Assert
            Assert.Equal(submissionId, _args.SubmissionId);
        }

        /// <summary>
        /// Verifies that all properties can be set simultaneously and retrieved correctly.
        /// </summary>
        [Fact]
        public void AllProperties_SetAndGet_ReturnsSameValues()
        {
            // Arrange
            var expectedGlobalProperties = new Dictionary<string, string?> { { "Prop", "Test" } };
            var expectedEntryProjects = new List<string> { @"C:\Projects\Test.csproj" };
            var expectedTargetNames = new List<string> { "Build" };
            var expectedFlags = BuildRequestDataFlags.ClearCachesAfterBuild;
            var expectedSubmissionId = 42;

            // Act
            _args.GlobalProperties = expectedGlobalProperties;
            _args.EntryProjectsFullPath = expectedEntryProjects;
            _args.TargetNames = expectedTargetNames;
            _args.Flags = expectedFlags;
            _args.SubmissionId = expectedSubmissionId;

            // Assert
            Assert.Equal(expectedGlobalProperties, _args.GlobalProperties);
            Assert.Equal(expectedEntryProjects, _args.EntryProjectsFullPath);
            Assert.Equal(expectedTargetNames, _args.TargetNames);
            Assert.Equal(expectedFlags, _args.Flags);
            Assert.Equal(expectedSubmissionId, _args.SubmissionId);
        }
    }
}
