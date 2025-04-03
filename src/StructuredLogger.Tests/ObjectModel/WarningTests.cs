using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Warning"/> class.
    /// </summary>
    public class WarningTests
    {
        private readonly Warning _warning;

        /// <summary>
        /// Initializes a new instance of the <see cref="WarningTests"/> class.
        /// </summary>
        public WarningTests()
        {
            _warning = new Warning();
        }

        /// <summary>
        /// Tests that the TypeName property returns the expected value.
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsExpectedValue()
        {
            // Act
            string actualTypeName = _warning.TypeName;

            // Assert
            actualTypeName.Should().Be(nameof(Warning));
        }
    }
}
