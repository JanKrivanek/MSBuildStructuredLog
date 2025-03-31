using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="CriticalBuildMessage"/> class.
    /// </summary>
    public class CriticalBuildMessageTests
    {
        private readonly CriticalBuildMessage _criticalBuildMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CriticalBuildMessageTests"/> class.
        /// </summary>
        public CriticalBuildMessageTests()
        {
            _criticalBuildMessage = new CriticalBuildMessage();
        }

        /// <summary>
        /// Tests that the TypeName property's getter returns the expected type name "CriticalBuildMessage".
        /// </summary>
        [Fact]
        public void TypeName_WhenAccessed_ReturnsCriticalBuildMessage()
        {
            // Act
            string typeName = _criticalBuildMessage.TypeName;

            // Assert
            typeName.Should().Be("CriticalBuildMessage");
        }
    }
}
