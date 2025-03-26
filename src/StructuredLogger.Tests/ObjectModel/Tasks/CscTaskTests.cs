using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="CscTask"/> class.
    /// </summary>
    public class CscTaskTests
    {
        /// <summary>
        /// Tests that an instance of <see cref="CscTask"/> is successfully created.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_ShouldInstantiateCscTask()
        {
            // Arrange & Act
            var cscTask = new CscTask();

            // Assert
            Assert.NotNull(cscTask);
            // Verify that the created instance is assignable to ManagedCompilerTask, its base type.
            Assert.IsAssignableFrom<ManagedCompilerTask>(cscTask);
        }
    }
}
