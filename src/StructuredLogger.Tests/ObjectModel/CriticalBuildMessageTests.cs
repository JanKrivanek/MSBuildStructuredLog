using Microsoft.Build.Logging.StructuredLogger;
using System;
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
        /// Tests that the TypeName property returns "CriticalBuildMessage".
        /// </summary>
        [Fact]
        public void TypeName_GetValue_ReturnsCriticalBuildMessage()
        {
            // Act
            string actualTypeName = _criticalBuildMessage.TypeName;

            // Assert
            Assert.Equal("CriticalBuildMessage", actualTypeName);
        }
    }
}
