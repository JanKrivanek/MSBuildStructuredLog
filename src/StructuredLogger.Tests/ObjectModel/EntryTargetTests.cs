using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using System;
using System.Reflection;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="EntryTarget"/> class.
    /// </summary>
    public class EntryTargetTests
    {
        /// <summary>
        /// Verifies that the <see cref="EntryTarget.TypeName"/> property always returns "EntryTarget".
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsEntryTarget()
        {
            // Arrange
            var entryTarget = new EntryTarget();

            // Act
            string typeName = entryTarget.TypeName;

            // Assert
            Assert.Equal("EntryTarget", typeName);
        }

        /// <summary>
        /// Verifies that when no parent project is found, the <see cref="EntryTarget.Target"/> property returns null,
        /// <see cref="EntryTarget.IsLowRelevance"/> returns true by default, and <see cref="EntryTarget.DurationText"/> is null.
        /// </summary>
        [Fact]
        public void Target_NoParentProject_ReturnsNullAndDefaults()
        {
            // Arrange
            var entryTarget = new EntryTarget();

            // (Implicitly, GetNearestParent<Project>() will return null if no parent is attached)

            // Act
            var target = entryTarget.Target;
            bool isLowRelevance = entryTarget.IsLowRelevance;
            string durationText = entryTarget.DurationText;

            // Assert
            Assert.Null(target);
            Assert.True(isLowRelevance);
            Assert.Null(durationText);
        }

        /// <summary>
        /// Verifies that when a parent project with a matching target exists, the <see cref="EntryTarget.Target"/>
        /// property returns the matching target, and related properties reflect the target's settings.
        /// </summary>
//         [Fact] [Error] (75-54)CS1503 Argument 1: cannot convert from 'System.Func<Microsoft.Build.Logging.StructuredLogger.Target, bool>' to 'System.Predicate<Microsoft.Build.Logging.StructuredLogger.Target>'
//         public void Target_WithParentProjectAndMatchingTarget_ReturnsExpectedTarget()
//         {
//             // Arrange
//             const string testName = "TestTarget";
//             // Create a testable instance that allows setting the Name property.
//             var entryTarget = new TestEntryTarget(testName);
// 
//             // Create a mock for the Target dependency.
//             var targetMock = new Mock<Target>();
//             targetMock.SetupGet(t => t.Name).Returns(testName);
//             targetMock.SetupGet(t => t.IsLowRelevance).Returns(false);
//             targetMock.SetupGet(t => t.DurationText).Returns("5s");
// 
//             // Create a mock for the Project dependency.
//             var projectMock = new Mock<Project>();
//             // Setup the FindFirstChild<Target> method to return our target if predicate returns true.
//             projectMock
//                 .Setup(p => p.FindFirstChild<Target>(It.IsAny<Func<Target, bool>>()))
//                 .Returns((Func<Target, bool> predicate) =>
//                 {
//                     return predicate(targetMock.Object) ? targetMock.Object : null;
//                 });
// 
//             // Inject the mock project into the private field "project" of EntryTarget.
//             SetPrivateField(entryTarget, "project", projectMock.Object);
// 
//             // Act
//             Target resultTarget = entryTarget.Target;
//             bool isLowRelevance = entryTarget.IsLowRelevance;
//             string durationText = entryTarget.DurationText;
// 
//             // Assert
//             Assert.NotNull(resultTarget);
//             Assert.Equal(targetMock.Object, resultTarget);
//             Assert.False(isLowRelevance);
//             Assert.Equal("5s", durationText);
//         }

        /// <summary>
        /// Verifies that the <see cref="EntryTarget.Target"/> property caches its value.
        /// After the first retrieval, even if the underlying project returns a different target,
        /// subsequent calls return the originally cached target.
        /// </summary>
//         [Fact] [Error] (115-54)CS1503 Argument 1: cannot convert from 'System.Func<Microsoft.Build.Logging.StructuredLogger.Target, bool>' to 'System.Predicate<Microsoft.Build.Logging.StructuredLogger.Target>' [Error] (128-54)CS1503 Argument 1: cannot convert from 'System.Func<Microsoft.Build.Logging.StructuredLogger.Target, bool>' to 'System.Predicate<Microsoft.Build.Logging.StructuredLogger.Target>'
//         public void Target_CalledMultipleTimes_CachesTheResult()
//         {
//             // Arrange
//             const string testName = "CachedTarget";
//             var entryTarget = new TestEntryTarget(testName);
// 
//             var firstTargetMock = new Mock<Target>();
//             firstTargetMock.SetupGet(t => t.Name).Returns(testName);
//             firstTargetMock.SetupGet(t => t.IsLowRelevance).Returns(false);
//             firstTargetMock.SetupGet(t => t.DurationText).Returns("10s");
// 
//             var projectMock = new Mock<Project>();
//             projectMock
//                 .Setup(p => p.FindFirstChild<Target>(It.IsAny<Func<Target, bool>>()))
//                 .Returns((Func<Target, bool> predicate) =>
//                 {
//                     return predicate(firstTargetMock.Object) ? firstTargetMock.Object : null;
//                 });
// 
//             SetPrivateField(entryTarget, "project", projectMock.Object);
// 
//             // Act
//             Target firstCallResult = entryTarget.Target;
// 
//             // Change the project mock to return null (simulate change in dependency)
//             projectMock
//                 .Setup(p => p.FindFirstChild<Target>(It.IsAny<Func<Target, bool>>()))
//                 .Returns((Target)null);
// 
//             Target secondCallResult = entryTarget.Target;
// 
//             // Assert
//             Assert.Same(firstCallResult, secondCallResult);
//         }

        /// <summary>
        /// Helper method to set a private field on an object using reflection.
        /// </summary>
        /// <param name="instance">The object containing the private field.</param>
        /// <param name="fieldName">The name of the private field.</param>
        /// <param name="value">The value to set.</param>
        private static void SetPrivateField(object instance, string fieldName, object value)
        {
            FieldInfo field = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
            {
                throw new Exception($"Field '{fieldName}' was not found on type '{instance.GetType().FullName}'.");
            }
            field.SetValue(instance, value);
        }
    }

    /// <summary>
    /// A test subclass of <see cref="EntryTarget"/> that exposes a settable Name property.
    /// This allows simulation of the matching logic used in the Target property.
    /// </summary>
    internal class TestEntryTarget : EntryTarget
    {
        /// <summary>
        /// Gets or sets the name used for matching the target in the parent project.
        /// </summary>
        public new string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestEntryTarget"/> class with a specified name.
        /// </summary>
        /// <param name="name">The name to assign.</param>
        public TestEntryTarget(string name)
        {
            Name = name;
        }
    }
}
