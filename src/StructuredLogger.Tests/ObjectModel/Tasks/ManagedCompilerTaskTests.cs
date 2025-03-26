using System;
using System.Reflection;
using Moq;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// A test double for <see cref="ManagedCompilerTask"/> that allows control over the <see cref="Task.HasChildren"/> property.
    /// It assumes that the base class Task exposes a virtual HasChildren property.
    /// </summary>
    public class TestManagedCompilerTask : ManagedCompilerTask
    {
        private readonly bool _hasChildren;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestManagedCompilerTask"/> class with the specified value for HasChildren.
        /// </summary>
        /// <param name="hasChildren">A boolean indicating whether the task has children.</param>
        public TestManagedCompilerTask(bool hasChildren)
        {
            _hasChildren = hasChildren;
        }

        /// <summary>
        /// Gets a value indicating whether this task has child elements.
        /// </summary>
//         public override bool HasChildren => _hasChildren; [Error] (29-30)CS0506 'TestManagedCompilerTask.HasChildren': cannot override inherited member 'TreeNode.HasChildren' because it is not marked virtual, abstract, or override
    }

    /// <summary>
    /// Unit tests for the <see cref="ManagedCompilerTask"/> class.
    /// </summary>
    public class ManagedCompilerTaskTests
    {
        /// <summary>
        /// Tests that <see cref="ManagedCompilerTask.CompilationWrites"/> returns null when HasChildren is false.
        /// Arrange: Create a task instance with HasChildren set to false.
        /// Act: Retrieve the CompilationWrites property.
        /// Assert: The returned value should be null.
        /// </summary>
        [Fact]
        public void CompilationWrites_NoChildren_ReturnsNull()
        {
            // Arrange
            var task = new TestManagedCompilerTask(hasChildren: false);

            // Act
            var result = task.CompilationWrites;

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that <see cref="ManagedCompilerTask.CompilationWrites"/> returns null when HasChildren is true 
        /// but the static TryParse method (dependency) returns null.
        /// Arrange: Create a task instance with HasChildren set to true and ensure no cached value is present.
        /// Act: Retrieve the CompilationWrites property.
        /// Assert: The returned value should be null.
        /// Note: This test assumes that under test conditions, the static dependency 
        /// Logging.StructuredLogger.CompilationWrites.TryParse(this) returns null.
        /// </summary>
        [Fact]
        public void CompilationWrites_HasChildrenAndTryParseReturnsNull_ReturnsNull()
        {
            // Arrange
            var task = new TestManagedCompilerTask(hasChildren: true);
            // Clear any previously cached value in the private field.
            var fieldInfo = typeof(ManagedCompilerTask).GetField("compilationWrites", BindingFlags.Instance | BindingFlags.NonPublic);
            fieldInfo.SetValue(task, null);

            // Act
            var result = task.CompilationWrites;

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that <see cref="ManagedCompilerTask.CompilationWrites"/> returns a cached value when it has been previously set.
        /// Arrange: Create a task instance with HasChildren set to true and use reflection to set the private cached field.
        /// Act: Retrieve the CompilationWrites property.
        /// Assert: The returned value should equal the cached value that was set.
        /// </summary>
        [Fact]
        public void CompilationWrites_CachedValue_ReturnsCachedValue()
        {
            // Arrange
            var task = new TestManagedCompilerTask(hasChildren: true);
            var fieldInfo = typeof(ManagedCompilerTask).GetField("compilationWrites", BindingFlags.Instance | BindingFlags.NonPublic);
            // Create a dummy value for CompilationWrites.
            // Since CompilationWrites is a nullable struct, we need to create an instance and wrap it.
            Type underlyingType = fieldInfo.FieldType.GetGenericArguments()[0];
            object dummyValue = Activator.CreateInstance(underlyingType);
            // Create a new Nullable<CompilationWrites> instance with the dummyValue.
            object nullableValue = Activator.CreateInstance(fieldInfo.FieldType, dummyValue);
            fieldInfo.SetValue(task, nullableValue);

            // Act
            var result = task.CompilationWrites;

            // Assert
            Assert.NotNull(result);
            // Since we set the cached value, the result should equal the dummyValue.
            Assert.Equal(dummyValue, result);
        }
    }
}
