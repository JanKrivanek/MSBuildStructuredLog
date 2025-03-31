using FluentAssertions;
using Moq;
using System;
using System.Reflection;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ManagedCompilerTask"/> class.
    /// </summary>
    public class ManagedCompilerTaskTests
    {
        /// <summary>
        /// Tests that the CompilationWrites property returns null when the task has no children.
        /// This test arranges a partial mock of ManagedCompilerTask with HasChildren set to false, then verifies that
        /// the CompilationWrites getter returns null without attempting the parse.
        /// </summary>
        [Fact]
        public void CompilationWrites_Getter_NoChildren_ReturnsNull()
        {
            // Arrange
            var mockTask = new Mock<ManagedCompilerTask> { CallBase = true };
            // Setup HasChildren to return false
            mockTask.Setup(t => t.HasChildren).Returns(false);
            ManagedCompilerTask taskInstance = mockTask.Object;

            // Act
            var result = taskInstance.CompilationWrites;

            // Assert
            result.Should().BeNull("because the task has no children, so CompilationWrites should be null");
        }

        /// <summary>
        /// Tests that the CompilationWrites property returns the cached CompilationWrites value when available.
        /// This test arranges a partial mock of ManagedCompilerTask with HasChildren set to true and manually sets the
        /// private field 'compilationWrites' to a dummy value. The getter should return the cached value without calling the parser.
        /// </summary>
        [Fact]
        public void CompilationWrites_Getter_WithCachedValue_ReturnsCachedCompilationWrites()
        {
            // Arrange
            var mockTask = new Mock<ManagedCompilerTask> { CallBase = true };
            // Setup HasChildren to return true so that the getter proceeds past the initial check.
            mockTask.Setup(t => t.HasChildren).Returns(true);
            ManagedCompilerTask taskInstance = mockTask.Object;
            
            // Create a dummy instance of CompilationWrites.
            // Since CompilationWrites is a struct, we can use Activator.CreateInstance to create its default value.
            var dummyCompilationWrites = (CompilationWrites)Activator.CreateInstance(typeof(CompilationWrites));
            
            // Set the private field 'compilationWrites' to a non-null value using reflection.
            FieldInfo? fieldInfo = typeof(ManagedCompilerTask).GetField("compilationWrites", BindingFlags.Instance | BindingFlags.NonPublic);
            fieldInfo.Should().NotBeNull("the field 'compilationWrites' must exist");
            fieldInfo.SetValue(taskInstance, (CompilationWrites?)dummyCompilationWrites);

            // Act
            var result = taskInstance.CompilationWrites;

            // Assert
            result.Should().Be(dummyCompilationWrites, "because the cached CompilationWrites value was set and should be returned");
        }
    }
}
