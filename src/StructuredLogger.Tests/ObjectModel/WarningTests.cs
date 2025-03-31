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
        /// Tests the <see cref="Warning.TypeName"/> property to ensure it returns "Warning".
        /// </summary>
        [Fact]
        public void TypeName_WhenAccessed_ReturnsNameOfWarning()
        {
            // Act
            string actualTypeName = _warning.TypeName;

            // Assert
            actualTypeName.Should().Be("Warning", "because the TypeName property uses nameof(Warning) to return the class name");
        }
    }
}
