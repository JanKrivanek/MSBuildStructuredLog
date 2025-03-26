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
        /// Tests that Dispose method invokes the provided cleanup action.
        /// </summary>
        [Fact]
        public void Dispose_WhenCalled_InvokesDisposeAction()
        {
            // Arrange
            bool actionInvoked = false;
            Action action = () => actionInvoked = true;
            var cleanupScope = new CleanupScope(action);

            // Act
            cleanupScope.Dispose();

            // Assert
            Assert.True(actionInvoked, "Expected the dispose action to be invoked when Dispose is called.");
        }

        /// <summary>
        /// Tests that Dispose method throws a NullReferenceException when the cleanup action is null.
        /// </summary>
        [Fact]
        public void Dispose_WithNullAction_ThrowsNullReferenceException()
        {
            // Arrange
            var cleanupScope = new CleanupScope(null);

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => cleanupScope.Dispose());
        }

        /// <summary>
        /// Tests that Dispose method propagates exceptions thrown by the cleanup action.
        /// </summary>
        [Fact]
        public void Dispose_WhenActionThrowsException_PropagatesException()
        {
            // Arrange
            var expectedException = new InvalidOperationException("Test exception");
            Action throwingAction = () => throw expectedException;
            var cleanupScope = new CleanupScope(throwingAction);

            // Act & Assert
            var actualException = Assert.Throws<InvalidOperationException>(() => cleanupScope.Dispose());
            Assert.Equal(expectedException.Message, actualException.Message);
        }
    }
}
