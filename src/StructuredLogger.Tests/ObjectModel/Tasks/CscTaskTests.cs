using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="CscTask"/> class.
    /// </summary>
    public class CscTaskTests
    {
        /// <summary>
        /// Tests that the default constructor of <see cref="CscTask"/> creates a non-null instance.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_CreatesInstance()
        {
            // Act
            var instance = new CscTask();

            // Assert
            instance.Should().NotBeNull();
        }
    }
}
