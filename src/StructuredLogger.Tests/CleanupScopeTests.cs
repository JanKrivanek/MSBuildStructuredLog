using System;
using DotUtils.StreamUtils;
using FluentAssertions;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="CleanupScope"/> struct.
    /// </summary>
    public class CleanupScopeTests
    {
        /// <summary>
        /// Tests that calling Dispose on a CleanupScope instance invokes the provided dispose action.
        /// This test verifies the happy path where the delegate sets a flag upon invocation.
        /// </summary>
        [Fact]
        public void Dispose_WhenCalled_InvokesDisposeAction()
        {
            // Arrange
            bool disposeCalled = false;
            Action action = () => disposeCalled = true;
            var cleanupScope = new CleanupScope(action);

            // Act
            cleanupScope.Dispose();

            // Assert
            disposeCalled.Should().BeTrue("because the dispose action should have been executed when Dispose is called");
        }

        /// <summary>
        /// Tests that if the dispose action throws an exception, Dispose propagates the exception.
        /// This test verifies that no exception handling within Dispose masks the exception.
        /// </summary>
        [Fact]
        public void Dispose_WhenDisposeActionThrows_ExceptionIsPropagated()
        {
            // Arrange
            var expectedException = new InvalidOperationException("Test exception");
            Action action = () => throw expectedException;
            var cleanupScope = new CleanupScope(action);

            // Act
            Action act = () => cleanupScope.Dispose();

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Test exception");
        }

        /// <summary>
        /// Tests that when a null dispose action is provided, calling Dispose results in a NullReferenceException.
        /// This covers the edge case where the delegate is null.
        /// </summary>
        [Fact]
        public void Dispose_WhenDisposeActionIsNull_ThrowsNullReferenceException()
        {
            // Arrange
            var cleanupScope = new CleanupScope(null!);

            // Act
            Action act = () => cleanupScope.Dispose();

            // Assert
            act.Should().Throw<NullReferenceException>("because invoking a null delegate should result in a NullReferenceException");
        }
    }
}
