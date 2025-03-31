// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using Microsoft.Build.Logging.StructuredLogger.UnitTests;
// using Moq;
// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.IO;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "Target"/> class.
//     /// </summary>
//     public class TargetTests
//     {
// //         /// <summary> // [Error] (29-20)CS1061 'Target' does not contain a definition for 'Id' and no accessible extension method 'Id' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests that the constructor sets the default Id to -1.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_Default_SetsIdToMinusOne()
// //         {
// //             // Arrange & Act
// //             var target = new Target();
// //             // Assert
// //             // Assuming Id is inherited from TimedNode and is publicly accessible.
// //             target.Id.Should().Be(-1, "the default constructor should set Id to -1");
// //         }
// //  // [Error] (43-17)CS0117 'Target' does not contain a definition for 'ParentTarget' // [Error] (44-17)CS0117 'Target' does not contain a definition for 'OriginalNode' // [Error] (45-17)CS0117 'Target' does not contain a definition for 'TargetBuiltReason' // [Error] (49-34)CS1061 'Target' does not contain a definition for 'ParentTargetTooltip' and no accessible extension method 'ParentTargetTooltip' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that ParentTargetTooltip returns an empty string when ParentTarget is null or empty.
//         /// </summary>
//         [Theory]
//         [InlineData(null)]
//         [InlineData("")]
//         public void ParentTargetTooltip_WhenParentTargetNullOrEmpty_ReturnsEmpty(string parentTarget)
//         {
//             // Arrange
//             var target = new Target
//             {
//                 ParentTarget = parentTarget,
//                 OriginalNode = null,
//                 TargetBuiltReason = TargetBuiltReason.AfterTargets, // value doesn't matter here
//                 Name = "SampleTarget"
//             };
//             // Act
//             var tooltip = target.ParentTargetTooltip;
//             // Assert
//             tooltip.Should().Be(string.Empty);
//         }
// //  // [Error] (63-17)CS0117 'Target' does not contain a definition for 'ParentTarget' // [Error] (64-17)CS0117 'Target' does not contain a definition for 'OriginalNode' // [Error] (65-17)CS0117 'Target' does not contain a definition for 'TargetBuiltReason' // [Error] (69-34)CS1061 'Target' does not contain a definition for 'ParentTargetTooltip' and no accessible extension method 'ParentTargetTooltip' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that ParentTargetTooltip returns the navigation tooltip when OriginalNode is not null.
// //         /// </summary>
// //         [Fact]
// //         public void ParentTargetTooltip_WhenOriginalNodeNotNull_ReturnsNavigationTooltip()
// //         {
// //             // Arrange
// //             var target = new Target
// //             {
// //                 ParentTarget = "Parent",
// //                 OriginalNode = new TimedNodeStub(),
// //                 TargetBuiltReason = TargetBuiltReason.BeforeTargets,
// //                 Name = "SampleTarget"
// //             };
// //             // Act
// //             var tooltip = target.ParentTargetTooltip;
// //             // Assert
// //             tooltip.Should().Be("Navigate to where the target was built originally");
// //         }
// //  // [Error] (83-17)CS0117 'Target' does not contain a definition for 'ParentTarget' // [Error] (84-17)CS0117 'Target' does not contain a definition for 'OriginalNode' // [Error] (85-17)CS0117 'Target' does not contain a definition for 'TargetBuiltReason' // [Error] (89-34)CS1061 'Target' does not contain a definition for 'ParentTargetTooltip' and no accessible extension method 'ParentTargetTooltip' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that ParentTargetTooltip returns a built reason message when OriginalNode is null and TargetBuiltReason is None.
//         /// </summary>
//         [Fact]
//         public void ParentTargetTooltip_WhenNoOriginalNodeAndReasonNone_ReturnsBuiltReasonMessage()
//         {
//             // Arrange
//             var target = new Target
//             {
//                 ParentTarget = "Parent",
//                 OriginalNode = null,
//                 TargetBuiltReason = TargetBuiltReason.None,
//                 Name = "SampleTarget"
//             };
//             // Act
//             var tooltip = target.ParentTargetTooltip;
//             // Assert
//             tooltip.Should().Be("SampleTarget was built because of Parent");
//         }
// //  // [Error] (103-17)CS0117 'Target' does not contain a definition for 'ParentTarget' // [Error] (104-17)CS0117 'Target' does not contain a definition for 'OriginalNode' // [Error] (105-17)CS0117 'Target' does not contain a definition for 'TargetBuiltReason' // [Error] (109-34)CS1061 'Target' does not contain a definition for 'ParentTargetTooltip' and no accessible extension method 'ParentTargetTooltip' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that ParentTargetTooltip returns the correct message for TargetBuiltReason.AfterTargets.
// //         /// </summary>
// //         [Fact]
// //         public void ParentTargetTooltip_WhenAfterTargets_ReturnsAfterTargetsMessage()
// //         {
// //             // Arrange
// //             var target = new Target
// //             {
// //                 ParentTarget = "Parent",
// //                 OriginalNode = null,
// //                 TargetBuiltReason = TargetBuiltReason.AfterTargets,
// //                 Name = "SampleTarget"
// //             };
// //             // Act
// //             var tooltip = target.ParentTargetTooltip;
// //             // Assert
// //             tooltip.Should().Be("Target 'SampleTarget' had AfterTargets='Parent' directly or indirectly");
// //         }
// //  // [Error] (123-17)CS0117 'Target' does not contain a definition for 'ParentTarget' // [Error] (124-17)CS0117 'Target' does not contain a definition for 'OriginalNode' // [Error] (125-17)CS0117 'Target' does not contain a definition for 'TargetBuiltReason' // [Error] (129-34)CS1061 'Target' does not contain a definition for 'ParentTargetTooltip' and no accessible extension method 'ParentTargetTooltip' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that ParentTargetTooltip returns the correct message for TargetBuiltReason.BeforeTargets.
//         /// </summary>
//         [Fact]
//         public void ParentTargetTooltip_WhenBeforeTargets_ReturnsBeforeTargetsMessage()
//         {
//             // Arrange
//             var target = new Target
//             {
//                 ParentTarget = "Parent",
//                 OriginalNode = null,
//                 TargetBuiltReason = TargetBuiltReason.BeforeTargets,
//                 Name = "SampleTarget"
//             };
//             // Act
//             var tooltip = target.ParentTargetTooltip;
//             // Assert
//             tooltip.Should().Be("Target 'SampleTarget' had BeforeTargets='Parent'");
//         }
// //  // [Error] (143-17)CS0117 'Target' does not contain a definition for 'ParentTarget' // [Error] (144-17)CS0117 'Target' does not contain a definition for 'OriginalNode' // [Error] (145-17)CS0117 'Target' does not contain a definition for 'TargetBuiltReason' // [Error] (149-34)CS1061 'Target' does not contain a definition for 'ParentTargetTooltip' and no accessible extension method 'ParentTargetTooltip' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that ParentTargetTooltip returns the correct message for TargetBuiltReason.DependsOn.
// //         /// </summary>
// //         [Fact]
// //         public void ParentTargetTooltip_WhenDependsOn_ReturnsDependsOnMessage()
// //         {
// //             // Arrange
// //             var target = new Target
// //             {
// //                 ParentTarget = "Parent",
// //                 OriginalNode = null,
// //                 TargetBuiltReason = TargetBuiltReason.DependsOn,
// //                 Name = "SampleTarget"
// //             };
// //             // Act
// //             var tooltip = target.ParentTargetTooltip;
// //             // Assert
// //             tooltip.Should().Be("Target 'Parent' had DependsOnTargets='SampleTarget'");
// //         }
// //  // [Error] (165-17)CS0117 'Target' does not contain a definition for 'ParentTarget' // [Error] (166-17)CS0117 'Target' does not contain a definition for 'OriginalNode' // [Error] (167-17)CS0117 'Target' does not contain a definition for 'TargetBuiltReason' // [Error] (170-31)CS1061 'Target' does not contain a definition for 'ParentTargetText' and no accessible extension method 'ParentTargetText' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that ParentTargetText returns an empty string when ParentTarget is null or empty.
//         /// </summary>
//         [Theory]
//         [InlineData(null)]
//         [InlineData("")]
//         public void ParentTargetText_WhenParentTargetNullOrEmpty_ReturnsEmpty(string parentTarget)
//         {
//             // Arrange
//             var target = new Target
//             {
//                 ParentTarget = parentTarget,
//                 OriginalNode = null,
//                 TargetBuiltReason = TargetBuiltReason.AfterTargets
//             };
//             // Act
//             var text = target.ParentTargetText;
//             // Assert
//             text.Should().Be(string.Empty);
//         }
// //  // [Error] (184-17)CS0117 'Target' does not contain a definition for 'ParentTarget' // [Error] (185-17)CS0117 'Target' does not contain a definition for 'OriginalNode' // [Error] (186-17)CS0117 'Target' does not contain a definition for 'TargetBuiltReason' // [Error] (189-31)CS1061 'Target' does not contain a definition for 'ParentTargetText' and no accessible extension method 'ParentTargetText' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that ParentTargetText returns ParentTarget directly when OriginalNode is not null.
// //         /// </summary>
// //         [Fact]
// //         public void ParentTargetText_WhenOriginalNodeNotNull_ReturnsParentTarget()
// //         {
// //             // Arrange
// //             var target = new Target
// //             {
// //                 ParentTarget = "Parent",
// //                 OriginalNode = new TimedNodeStub(),
// //                 TargetBuiltReason = TargetBuiltReason.BeforeTargets
// //             };
// //             // Act
// //             var text = target.ParentTargetText;
// //             // Assert
// //             text.Should().Be("Parent");
// //         }
// //  // [Error] (203-17)CS0117 'Target' does not contain a definition for 'ParentTarget' // [Error] (204-17)CS0117 'Target' does not contain a definition for 'OriginalNode' // [Error] (205-17)CS0117 'Target' does not contain a definition for 'TargetBuiltReason' // [Error] (208-31)CS1061 'Target' does not contain a definition for 'ParentTargetText' and no accessible extension method 'ParentTargetText' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that ParentTargetText returns the correct formatted text for TargetBuiltReason.AfterTargets.
//         /// </summary>
//         [Fact]
//         public void ParentTargetText_WhenAfterTargets_ReturnsTextWithUpArrow()
//         {
//             // Arrange
//             var target = new Target
//             {
//                 ParentTarget = "Parent",
//                 OriginalNode = null,
//                 TargetBuiltReason = TargetBuiltReason.AfterTargets
//             };
//             // Act
//             var text = target.ParentTargetText;
//             // Assert
//             text.Should().Be(" ↑ Parent");
//         }
// //  // [Error] (222-17)CS0117 'Target' does not contain a definition for 'ParentTarget' // [Error] (223-17)CS0117 'Target' does not contain a definition for 'OriginalNode' // [Error] (224-17)CS0117 'Target' does not contain a definition for 'TargetBuiltReason' // [Error] (227-31)CS1061 'Target' does not contain a definition for 'ParentTargetText' and no accessible extension method 'ParentTargetText' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that ParentTargetText returns the correct formatted text for TargetBuiltReason.BeforeTargets.
// //         /// </summary>
// //         [Fact]
// //         public void ParentTargetText_WhenBeforeTargets_ReturnsTextWithDownArrow()
// //         {
// //             // Arrange
// //             var target = new Target
// //             {
// //                 ParentTarget = "Parent",
// //                 OriginalNode = null,
// //                 TargetBuiltReason = TargetBuiltReason.BeforeTargets
// //             };
// //             // Act
// //             var text = target.ParentTargetText;
// //             // Assert
// //             text.Should().Be(" ↓ Parent");
// //         }
// //  // [Error] (241-17)CS0117 'Target' does not contain a definition for 'ParentTarget' // [Error] (242-17)CS0117 'Target' does not contain a definition for 'OriginalNode' // [Error] (243-17)CS0117 'Target' does not contain a definition for 'TargetBuiltReason' // [Error] (246-31)CS1061 'Target' does not contain a definition for 'ParentTargetText' and no accessible extension method 'ParentTargetText' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that ParentTargetText returns the correct formatted text for TargetBuiltReason.DependsOn.
//         /// </summary>
//         [Fact]
//         public void ParentTargetText_WhenDependsOn_ReturnsTextWithRightArrow()
//         {
//             // Arrange
//             var target = new Target
//             {
//                 ParentTarget = "Parent",
//                 OriginalNode = null,
//                 TargetBuiltReason = TargetBuiltReason.DependsOn
//             };
//             // Act
//             var text = target.ParentTargetText;
//             // Assert
//             text.Should().Be(" → Parent");
//         }
// //  // [Error] (260-17)CS0117 'Target' does not contain a definition for 'ParentTarget' // [Error] (261-17)CS0117 'Target' does not contain a definition for 'OriginalNode' // [Error] (262-17)CS0117 'Target' does not contain a definition for 'TargetBuiltReason' // [Error] (265-31)CS1061 'Target' does not contain a definition for 'ParentTargetText' and no accessible extension method 'ParentTargetText' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that ParentTargetText returns the default right arrow formatted text when TargetBuiltReason is not specified.
// //         /// </summary>
// //         [Fact]
// //         public void ParentTargetText_WhenUnknownReason_ReturnsTextWithDefaultRightArrow()
// //         {
// //             // Arrange
// //             var target = new Target
// //             {
// //                 ParentTarget = "Parent",
// //                 OriginalNode = null,
// //                 TargetBuiltReason = (TargetBuiltReason)999 // unknown reason
// //             };
// //             // Act
// //             var text = target.ParentTargetText;
// //             // Assert
// //             text.Should().Be(" → Parent");
// //         }
// //  // [Error] (281-26)CS1061 'Target' does not contain a definition for 'TryAddTarget' and no accessible extension method 'TryAddTarget' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that TryAddTarget adds the target as a child when its Parent is null.
//         /// </summary>
//         [Fact]
//         public void TryAddTarget_WhenTargetParentIsNull_AddsChild()
//         {
//             // Arrange
//             var parentTarget = new Target();
//             var childTarget = new Target();
//             // Ensure childTarget.Parent is null (assuming default is null)
//             // Act
//             parentTarget.TryAddTarget(childTarget);
//             // Assert
//             // Access the Children property inherited from TimedNode.
//             parentTarget.Children.Should().Contain(childTarget, "child should be added when its Parent is null");
//         }
// //  // [Error] (297-25)CS1061 'Target' does not contain a definition for 'Parent' and no accessible extension method 'Parent' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?) // [Error] (299-26)CS1061 'Target' does not contain a definition for 'TryAddTarget' and no accessible extension method 'TryAddTarget' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that TryAddTarget does not add the target as a child when its Parent is not null.
// //         /// </summary>
// //         [Fact]
// //         public void TryAddTarget_WhenTargetParentIsNotNull_DoesNotAddChild()
// //         {
// //             // Arrange
// //             var parentTarget = new Target();
// //             var childTarget = new Target();
// //             // Simulate that childTarget already has a parent.
// //             childTarget.Parent = new Target();
// //             // Act
// //             parentTarget.TryAddTarget(childTarget);
// //             // Assert
// //             parentTarget.Children.Should().NotContain(childTarget, "child should not be added when its Parent is not null");
// //         }
// //  // [Error] (316-17)CS0117 'TestTask' does not contain a definition for 'Id' // [Error] (320-40)CS1061 'Target' does not contain a definition for 'GetTaskById' and no accessible extension method 'GetTaskById' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that GetTaskById returns the correct task if it exists among the children.
//         /// </summary>
//         [Fact]
//         public void GetTaskById_WhenTaskExists_ReturnsTask()
//         {
//             // Arrange
//             var target = new Target();
//             int taskId = 42;
//             // Create a dummy task and add it to the Children collection.
//             var dummyTask = new TestTask
//             {
//                 Id = taskId
//             };
//             target.Children.Add(dummyTask);
//             // Act
//             var retrievedTask = target.GetTaskById(taskId);
//             // Assert
//             retrievedTask.Should().BeSameAs(dummyTask, "the task with the matching Id should be returned");
//         }
// //  // [Error] (335-40)CS1061 'Target' does not contain a definition for 'GetTaskById' and no accessible extension method 'GetTaskById' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that GetTaskById returns null when no task with the specified Id exists.
// //         /// </summary>
// //         [Fact]
// //         public void GetTaskById_WhenTaskDoesNotExist_ReturnsNull()
// //         {
// //             // Arrange
// //             var target = new Target();
// //             int taskId = 99;
// //             // Act
// //             var retrievedTask = target.GetTaskById(taskId);
// //             // Assert
// //             retrievedTask.Should().BeNull("if no task with the specified Id exists, null should be returned");
// //         }
// //  // [Error] (351-17)CS0117 'TestTask' does not contain a definition for 'Id' // [Error] (355-36)CS1061 'Target' does not contain a definition for 'GetTaskById' and no accessible extension method 'GetTaskById' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?) // [Error] (358-37)CS1061 'Target' does not contain a definition for 'GetTaskById' and no accessible extension method 'GetTaskById' accepting a first argument of type 'Target' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that GetTaskById caches the found task so that subsequent calls return the cached instance.
//         /// </summary>
//         [Fact]
//         public void GetTaskById_WhenCalledMultipleTimes_ReturnsCachedTask()
//         {
//             // Arrange
//             var target = new Target();
//             int taskId = 55;
//             var dummyTask = new TestTask
//             {
//                 Id = taskId
//             };
//             target.Children.Add(dummyTask);
//             // Act
//             var firstCall = target.GetTaskById(taskId);
//             // Remove the task from Children to ensure caching is used
//             target.Children.Remove(dummyTask);
//             var secondCall = target.GetTaskById(taskId);
//             // Assert
//             firstCall.Should().BeSameAs(dummyTask, "the first call should return the task from children");
//             secondCall.Should().BeSameAs(dummyTask, "the task should be returned from the cache on subsequent calls");
//         }
// 
//         /// <summary>
//         /// Tests that ToString returns the correctly formatted string.
//         /// </summary>
//         [Fact]
//         public void ToString_ReturnsFormattedString()
//         {
//             // Arrange
//             // Use a testable subclass to override GetNearestParent to return a dummy project.
//             var dummyProject = new DummyProject
//             {
//                 ProjectFile = "C:\\Projects\\Test.proj"
//             };
//             var testTarget = new TestableTarget(dummyProject)
//             {
//                 Name = "TestTarget"
//             };
//             // Act
//             var result = testTarget.ToString();
//             // Assert
//             var expected = $"Target Name=TestTarget Project={Path.GetFileName(dummyProject.ProjectFile)}";
//             result.Should().Be(expected);
//         }
// 
//         /// <summary>
//         /// Tests that the IsLowRelevance property getter and setter work as expected.
//         /// </summary>
//         [Fact]
//         public void IsLowRelevance_GetSet_WorksCorrectly()
//         {
//             // Arrange
//             var target = new Target();
//             // Act & Assert
//             // Initially, with no flag set and assuming IsSelected is false by default, it should be false.
//             target.IsLowRelevance.Should().BeFalse("by default, LowRelevance flag should not be set");
//             target.IsLowRelevance = true;
//             target.IsLowRelevance.Should().BeTrue("after setting LowRelevance to true, getter should return true");
//             target.IsLowRelevance = false;
//             target.IsLowRelevance.Should().BeFalse("after setting LowRelevance to false, getter should return false");
//         }
//     }
// 
//     /// <summary>
//     /// A stub class for TimedNode used for testing purposes.
//     /// </summary>
//     internal class TimedNodeStub : TimedNode
//     {
//     // Minimal stub implementation. No additional members are required.
//     }
// 
//     /// <summary>
//     /// A testable subclass of Target to override behavior for testing ToString.
//     /// </summary>
//     internal class TestableTarget : Target
//     {
//         private readonly DummyProject _dummyProject;
//         public TestableTarget(DummyProject dummyProject)
//         {
//             _dummyProject = dummyProject;
//         }
// //  // [Error] (424-30)CS0115 'TestableTarget.GetNearestParent<T>()': no suitable method found to override // [Error] (432-25)CS0117 'Target' does not contain a definition for 'GetNearestParent'
// //         protected override T GetNearestParent<T>()
// //         {
// //             // If T is DummyProject (or Project), return the dummy project.
// //             if (typeof(T).IsAssignableFrom(typeof(DummyProject)))
// //             {
// //                 return (T)(object)_dummyProject;
// //             }
// // 
// //             return base.GetNearestParent<T>();
// //         }
// //     }
// 
//     /// <summary>
//     /// A dummy implementation of a project with a ProjectFile property.
//     /// </summary>
//     internal class DummyProject
//     {
//         public string ProjectFile { get; set; } = string.Empty;
//     }
// 
//     /// <summary>
//     /// A test implementation of Task inherited from the production Task.
//     /// </summary>
//     internal class TestTask : Task
//     {
//     // We assume Task has a public parameterless constructor and an overridable Id property.
//     }
// }