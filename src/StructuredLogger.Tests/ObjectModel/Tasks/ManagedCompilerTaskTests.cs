using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ManagedCompilerTask"/> class.
    /// </summary>
    public class ManagedCompilerTaskTests
    {
        /// <summary>
        /// Tests the CompilationWrites property when no children are present. Expected outcome is null.
        /// This test verifies the happy path where the task does not have any children (i.e. HasChildren is false).
        /// </summary>
        [Fact]
        public void CompilationWrites_WhenNoChildren_ReturnsNull()
        {
            // Arrange
            // A new instance of ManagedCompilerTask is created. By default, it is assumed that it has no children.
            var task = new ManagedCompilerTask();

            // Act
            var result = task.CompilationWrites;

            // Assert
            result.Should().BeNull("because the task does not have children, so CompilationWrites should return null.");
        }

        // NOTE:
        // The CompilationWrites getter depends on the HasChildren property and the static method CompilationWrites.TryParse.
        // Both HasChildren and CompilationWrites.TryParse cannot be set or mocked as per provided metadata.
        // To enable complete testing for scenarios where HasChildren is true (including both when TryParse returns null 
        // and when it returns a valid CompilationWrites value), consider refactoring the code to allow injecting 
        // the dependencies or overriding the HasChildren behavior in a derived test class.
    }
}
