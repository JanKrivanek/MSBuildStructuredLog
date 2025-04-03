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

        public CriticalBuildMessageTests()
        {
            _criticalBuildMessage = new CriticalBuildMessage();
        }

        /// <summary>
        /// Tests the TypeName property of CriticalBuildMessage to ensure it returns the expected type name.
        /// </summary>
        [Fact]
        public void TypeName_HappyPath_ReturnsCriticalBuildMessageString()
        {
            // Act
            string actualTypeName = _criticalBuildMessage.TypeName;

            // Assert
            actualTypeName.Should().Be("CriticalBuildMessage");
        }
    }
}
