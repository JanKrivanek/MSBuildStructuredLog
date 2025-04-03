using DotUtils.StreamUtils;
using FluentAssertions;
using System;
using Xunit;

namespace DotUtils.StreamUtils.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="CleanupScope"/> struct.
    /// </summary>
    public class CleanupScopeTests
    {
        /// <summary>
        /// Tests the <see cref="CleanupScope.Dispose"/> method when provided with a valid action.
        /// The test verifies that the Dispose method executes the provided action.
        /// </summary>
        [Fact]
        public void Dispose_WhenCalledWithValidAction_ExecutesAction()
        {
            // Arrange
            bool actionExecuted = false;
            Action validAction = () => actionExecuted = true;
            var cleanupScope = new CleanupScope(validAction);

            // Act
            cleanupScope.Dispose();

            // Assert
            actionExecuted.Should().BeTrue("because the action supplied should have been executed by Dispose");
        }

        /// <summary>
        /// Tests the <see cref="CleanupScope.Dispose"/> method when constructed with a null action.
        /// The test verifies that Dispose throws a NullReferenceException when the internal action is null.
        /// </summary>
        [Fact]
        public void Dispose_WhenCalledWithNullAction_ThrowsNullReferenceException()
        {
            // Arrange
            // Using null! to bypass the non-nullable warning since the behavior under test is passing a null action.
            var cleanupScope = new CleanupScope(null!);

            // Act
            Action act = () => cleanupScope.Dispose();

            // Assert
            act.Should().Throw<NullReferenceException>("because calling Dispose on a null action should result in a NullReferenceException");
        }
    }
}
