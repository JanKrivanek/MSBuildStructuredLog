using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="FscTask"/> class.
    /// </summary>
    public class FscTaskTests
    {
        /// <summary>
        /// Tests that constructing a <see cref="FscTask"/> instance creates a non-null object
        /// and that the instance is assignable to <see cref="ManagedCompilerTask"/>.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_CreatesInstance()
        {
            // Arrange & Act
            var instance = new FscTask();

            // Assert
            instance.Should().NotBeNull();
            instance.Should().BeAssignableTo<ManagedCompilerTask>();
        }
    }
}
