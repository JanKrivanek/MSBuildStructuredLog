using System;
using DotUtils.StreamUtils;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="CleanupScope"/> struct.
    /// </summary>
    public class CleanupScopeTests
    {
        /// <summary>
        /// Tests that the Dispose method correctly invokes the provided dispose action.
        /// </summary>
        [Fact]
        public void Dispose_WhenCalled_InvokesDisposeAction()
        {
            // Arrange
            bool actionInvoked = false;
            Action disposeAction = () => actionInvoked = true;
            var cleanupScope = new CleanupScope(disposeAction);

            // Act
            cleanupScope.Dispose();

            // Assert
            Assert.True(actionInvoked, "Dispose did not invoke the dispose action as expected.");
        }

        /// <summary>
        /// Tests that if CleanupScope is constructed with a null dispose action,
        /// calling Dispose throws a NullReferenceException.
        /// </summary>
        [Fact]
        public void Dispose_WithNullDisposeAction_ThrowsNullReferenceException()
        {
            // Arrange
            var cleanupScope = new CleanupScope(null);

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => cleanupScope.Dispose());
        }

        /// <summary>
        /// Tests that if the provided dispose action throws an exception,
        /// Dispose propagates that exception.
        /// </summary>
        [Fact]
        public void Dispose_WhenDisposeActionThrowsException_PropagatesException()
        {
            // Arrange
            string expectedMessage = "Dispose action exception.";
            Action throwingAction = () => throw new InvalidOperationException(expectedMessage);
            var cleanupScope = new CleanupScope(throwingAction);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => cleanupScope.Dispose());
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
