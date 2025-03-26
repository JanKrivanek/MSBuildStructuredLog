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
        /// Tests that the TypeName property returns "Warning".
        /// This verifies the correct override implementation for TypeName.
        /// </summary>
        [Fact]
        public void TypeName_Get_ReturnsWarning()
        {
            // Act
            var result = _warning.TypeName;

            // Assert
            Assert.Equal("Warning", result);
        }
    }
}
