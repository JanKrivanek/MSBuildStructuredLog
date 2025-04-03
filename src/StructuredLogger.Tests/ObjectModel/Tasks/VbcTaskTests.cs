using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="VbcTask"/> class.
    /// </summary>
    public class VbcTaskTests
    {
        /// <summary>
        /// Tests that the default constructor creates an instance of <see cref="VbcTask"/> and that it is assignable to <see cref="ManagedCompilerTask"/>.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_InstantiatesVbcTask()
        {
            // Act
            var task = new VbcTask();

            // Assert
            task.Should().NotBeNull();
            task.Should().BeOfType<VbcTask>();
            task.Should().BeAssignableTo<ManagedCompilerTask>();
        }
    }
}
