using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="RobocopyTask"/> class.
    /// </summary>
    public class RobocopyTaskTests
    {
        /// <summary>
        /// A testable subclass of <see cref="RobocopyTask"/> that exposes the protected GetFileCopyOperations method.
        /// </summary>
        private class TestableRobocopyTask : RobocopyTask
        {
            /// <summary>
            /// Exposes the protected GetFileCopyOperations method as public for testing.
            /// </summary>
            /// <returns>The enumerable collection of file copy operations.</returns>
            public IEnumerable<FileCopyOperation> InvokeGetFileCopyOperations()
            {
                return base.GetFileCopyOperations();
            }
        }

        /// <summary>
        /// Tests the GetFileCopyOperations method to ensure it returns an empty collection when there are no children.
        /// This test assumes that, by default, the HasChildren property evaluates to false.
        /// </summary>
        [Fact]
        public void GetFileCopyOperations_NoChildren_ReturnsEmptyCollection()
        {
            // Arrange
            var task = new TestableRobocopyTask();

            // Act
            IEnumerable<FileCopyOperation> operations = task.InvokeGetFileCopyOperations();

            // Assert
            operations.Should().BeEmpty("because when there are no children, no file copy operations should be returned.");
        }

        // TODO: To test GetFileCopyOperations when there are children and messages,
        // it is necessary to simulate the presence of child messages that match the various regex patterns.
        // However, the HasChildren property and GetMessages method are not settable/mocked with the current design.
        // To enable these tests, consider refactoring the code to allow injection of messages or overriding of related members.
        // Once refactored, add tests for:
        // - Messages that match Strings.RobocopyFileCopiedRegex resulting in a copied file operation.
        // - Messages that match Strings.RobocopyFileSkippedRegex, Strings.RobocopyFileSkippedAsDuplicateRegex, and Strings.RobocopyFileFailedRegex
        //   resulting in non-copied file operations.
    }
}
