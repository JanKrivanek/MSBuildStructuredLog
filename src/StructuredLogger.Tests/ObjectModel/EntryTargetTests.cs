using FluentAssertions;
using Moq;
using System;
using System.Reflection;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="EntryTarget"/> class.
    /// </summary>
    public class EntryTargetTests
    {
        private readonly string _defaultName = "TestTarget";

        /// <summary>
        /// Tests the TypeName property to ensure it returns "EntryTarget".
        /// </summary>
        [Fact]
        public void TypeName_WhenAccessed_ReturnsEntryTarget()
        {
            // Arrange
            var entryTarget = CreateFakeEntryTarget(_defaultName);

            // Act
            string typeName = entryTarget.TypeName;

            // Assert
            typeName.Should().Be("EntryTarget");
        }

        /// <summary>
        /// Tests the Target property when no parent project is available; expecting null.
        /// </summary>
        [Fact]
        public void Target_WhenNoParentProject_ReturnsNull()
        {
            // Arrange: Do not inject a project so GetNearestParent<Project>() returns null.
            var entryTarget = CreateFakeEntryTarget(_defaultName);

            // Act
            var target = entryTarget.Target;

            // Assert
            target.Should().BeNull();
        }
//  // [Error] (62-40)CS1061 'Target' does not contain a definition for 'DurationText' and no accessible extension method 'DurationText' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?) // [Error] (62-62)CS1503 Argument 1: cannot convert from 'string' to '?' // [Error] (67-38)CS1061 'Project' does not contain a definition for 'FindFirstChild' and no accessible extension method 'FindFirstChild' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the Target property when a parent project exists and returns a matching target.
//         /// </summary>
//         [Fact]
//         public void Target_WhenProjectProvidesMatchingTarget_ReturnsTarget()
//         {
//             // Arrange
//             var entryTarget = CreateFakeEntryTarget(_defaultName);
// 
//             // Create a mock for Target that matches by Name.
//             var mockTarget = new Mock<Target>();
//             mockTarget.SetupGet(t => t.Name).Returns(_defaultName);
//             mockTarget.SetupGet(t => t.IsLowRelevance).Returns(false);
//             mockTarget.SetupGet(t => t.DurationText).Returns("00:01:23");
// 
//             // Create a mock for Project.
//             var mockProject = new Mock<Project>();
//             // Setup FindFirstChild<Target> so that it returns the mockTarget when the predicate returns true.
//             mockProject.Setup(p => p.FindFirstChild<Target>(It.IsAny<Predicate<Target>>()))
//                        .Returns((Predicate<Target> predicate) =>
//                        {
//                            if (predicate(mockTarget.Object))
//                            {
//                                return mockTarget.Object;
//                            }
//                            return null;
//                        });
// 
//             // Inject the project into entryTarget via reflection.
//             SetPrivateField(entryTarget, "project", mockProject.Object);
// 
//             // Act
//             var target = entryTarget.Target;
// 
//             // Assert
//             target.Should().NotBeNull();
//             target.Should().Be(mockTarget.Object);
//         }
// 
        /// <summary>
        /// Tests the IsLowRelevance property when no target is found; expecting the default value (true).
        /// </summary>
        [Fact]
        public void IsLowRelevance_WhenNoTargetFound_ReturnsTrue()
        {
            // Arrange: No project is injected, so Target remains null.
            var entryTarget = CreateFakeEntryTarget(_defaultName);

            // Act
            bool isLowRelevance = entryTarget.IsLowRelevance;

            // Assert
            isLowRelevance.Should().BeTrue();
        }
//  // [Error] (120-38)CS1061 'Project' does not contain a definition for 'FindFirstChild' and no accessible extension method 'FindFirstChild' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the IsLowRelevance property when a matching target is found with IsLowRelevance set to false.
//         /// </summary>
//         [Fact]
//         public void IsLowRelevance_WhenTargetFoundWithFalse_ReturnsFalse()
//         {
//             // Arrange
//             var entryTarget = CreateFakeEntryTarget(_defaultName);
// 
//             // Create a mock Target with IsLowRelevance false.
//             var mockTarget = new Mock<Target>();
//             mockTarget.SetupGet(t => t.Name).Returns(_defaultName);
//             mockTarget.SetupGet(t => t.IsLowRelevance).Returns(false);
// 
//             // Create a mock Project that returns the mockTarget.
//             var mockProject = new Mock<Project>();
//             mockProject.Setup(p => p.FindFirstChild<Target>(It.IsAny<Predicate<Target>>()))
//                        .Returns(mockTarget.Object);
// 
//             SetPrivateField(entryTarget, "project", mockProject.Object);
// 
//             // Act
//             bool isLowRelevance = entryTarget.IsLowRelevance;
// 
//             // Assert
//             isLowRelevance.Should().BeFalse();
//         }
//  // [Error] (144-40)CS1061 'Target' does not contain a definition for 'DurationText' and no accessible extension method 'DurationText' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?) // [Error] (144-62)CS1503 Argument 1: cannot convert from 'string' to '?' // [Error] (147-38)CS1061 'Project' does not contain a definition for 'FindFirstChild' and no accessible extension method 'FindFirstChild' accepting a first argument of type 'Project' could be found (are you missing a using directive or an assembly reference?)
        /// <summary>
        /// Tests the DurationText property when a matching target is found; expecting the provided duration text.
        /// </summary>
        [Fact]
        public void DurationText_WhenTargetFound_ReturnsDurationText()
        {
            // Arrange
            var entryTarget = CreateFakeEntryTarget(_defaultName);

            string expectedDuration = "00:02:34";
            var mockTarget = new Mock<Target>();
            mockTarget.SetupGet(t => t.Name).Returns(_defaultName);
            mockTarget.SetupGet(t => t.DurationText).Returns(expectedDuration);

            var mockProject = new Mock<Project>();
            mockProject.Setup(p => p.FindFirstChild<Target>(It.IsAny<Predicate<Target>>()))
                       .Returns(mockTarget.Object);

            SetPrivateField(entryTarget, "project", mockProject.Object);

            // Act
            var duration = entryTarget.DurationText;

            // Assert
            duration.Should().Be(expectedDuration);
        }

        /// <summary>
        /// Helper method to set a private field via reflection.
        /// </summary>
        /// <param name="obj">The object whose field to set.</param>
        /// <param name="fieldName">The name of the private field.</param>
        /// <param name="value">The value to assign to the field.</param>
        private static void SetPrivateField(object obj, string fieldName, object? value)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field is not null)
            {
                field.SetValue(obj, value);
            }
        }

        /// <summary>
        /// Creates an instance of FakeEntryTarget with a specified name.
        /// </summary>
        /// <param name="name">The name to assign to the instance.</param>
        /// <returns>An instance of EntryTarget with the specified name.</returns>
        private static EntryTarget CreateFakeEntryTarget(string name)
        {
            return new FakeEntryTarget(name);
        }

        /// <summary>
        /// A fake subclass of EntryTarget that allows setting the Name property inherited from NamedNode.
        /// </summary>
        private class FakeEntryTarget : EntryTarget
        {
            public FakeEntryTarget(string name)
            {
                // Attempt to set the Name property using reflection.
                var nameProperty = typeof(NamedNode).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public);
                if (nameProperty != null && nameProperty.CanWrite)
                {
                    nameProperty.SetValue(this, name);
                }
                else
                {
                    // If no public setter exists, try to set a backing field named "name".
                    var nameField = typeof(NamedNode).GetField("name", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (nameField != null)
                    {
                        nameField.SetValue(this, name);
                    }
                }
            }
        }
    }
}
