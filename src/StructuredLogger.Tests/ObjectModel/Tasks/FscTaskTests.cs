using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="FscTask"/> class.
    /// </summary>
    public class FscTaskTests
    {
        /// <summary>
        /// Tests that an instance of <see cref="FscTask"/> can be created successfully.
        /// This test follows the Arrange-Act-Assert pattern by arranging necessary context, acting via instantiation, and asserting that the instance is not null.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_ReturnsNonNullInstance()
        {
            // Act
            var instance = new FscTask();

            // Assert
            instance.Should().NotBeNull("an instance of FscTask should be created successfully");
        }
    }
}
