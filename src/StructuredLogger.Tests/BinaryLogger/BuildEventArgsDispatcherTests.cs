// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="EventArgsDispatcher"/> class.
//     /// </summary>
//     public class EventArgsDispatcherTests
//     {
//         private readonly EventArgsDispatcher _dispatcher;
// 
//         public EventArgsDispatcherTests()
//         {
//             _dispatcher = new EventArgsDispatcher();
//         }
// 
//         /// <summary>
//         /// Tests that Dispatch correctly raises BuildMessageEventArgs and the generic AnyEventRaised event.
//         /// </summary>
//         [Fact]
//         public void Dispatch_BuildMessageEventArgs_RaisesMessageAndAnyEvent()
//         {
//             // Arrange
//             bool messageRaised = false;
//             bool anyRaised = false;
//             BuildMessageEventArgs? capturedMessageEvent = null;
//             BuildEventArgs? capturedAnyEvent = null;
//             var buildEvent = new BuildMessageEventArgs("TestMessage", "TestHelp", "TestSubcategory", MessageImportance.High);
// 
//             _dispatcher.MessageRaised += (sender, e) =>
//             {
//                 messageRaised = true;
//                 capturedMessageEvent = e;
//             };
//             _dispatcher.AnyEventRaised += (sender, e) =>
//             {
//                 anyRaised = true;
//                 capturedAnyEvent = e;
//             };
// 
//             // Act
//             _dispatcher.Dispatch(buildEvent);
// 
//             // Assert
//             messageRaised.Should().BeTrue("BuildMessageEventArgs should trigger MessageRaised event.");
//             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
//             capturedMessageEvent.Should().Be(buildEvent, "the event argument should match the dispatched BuildMessageEventArgs.");
//             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
//         }
// //  // [Error] (66-104)CS1503 Argument 5: cannot convert from 'int' to 'string'
// //         /// <summary>
// //         /// Tests that Dispatch correctly raises TaskStartedEventArgs and the generic AnyEventRaised event.
// //         /// </summary>
// //         [Fact]
// //         public void Dispatch_TaskStartedEventArgs_RaisesTaskStartedAndAnyEvent()
// //         {
// //             // Arrange
// //             bool taskStartedRaised = false;
// //             bool anyRaised = false;
// //             TaskStartedEventArgs? capturedTaskStartedEvent = null;
// //             BuildEventArgs? capturedAnyEvent = null;
// //             var buildEvent = new TaskStartedEventArgs("task", "projectFile", "targetName", "taskFile", 1);
// // 
// //             _dispatcher.TaskStarted += (sender, e) =>
// //             {
// //                 taskStartedRaised = true;
// //                 capturedTaskStartedEvent = e;
// //             };
// //             _dispatcher.AnyEventRaised += (sender, e) =>
// //             {
// //                 anyRaised = true;
// //                 capturedAnyEvent = e;
// //             };
// // 
// //             // Act
// //             _dispatcher.Dispatch(buildEvent);
// // 
// //             // Assert
// //             taskStartedRaised.Should().BeTrue("TaskStartedEventArgs should trigger TaskStarted event.");
// //             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
// //             capturedTaskStartedEvent.Should().Be(buildEvent, "the event argument should match the dispatched TaskStartedEventArgs.");
// //             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
// //         }
// //  // [Error] (100-105)CS1503 Argument 5: cannot convert from 'int' to 'string'
//         /// <summary>
//         /// Tests that Dispatch correctly raises TaskFinishedEventArgs and the generic AnyEventRaised event.
//         /// </summary>
//         [Fact]
//         public void Dispatch_TaskFinishedEventArgs_RaisesTaskFinishedAndAnyEvent()
//         {
//             // Arrange
//             bool taskFinishedRaised = false;
//             bool anyRaised = false;
//             TaskFinishedEventArgs? capturedTaskFinishedEvent = null;
//             BuildEventArgs? capturedAnyEvent = null;
//             var buildEvent = new TaskFinishedEventArgs("task", "projectFile", "targetName", "taskFile", 1, true);
// 
//             _dispatcher.TaskFinished += (sender, e) =>
//             {
//                 taskFinishedRaised = true;
//                 capturedTaskFinishedEvent = e;
//             };
//             _dispatcher.AnyEventRaised += (sender, e) =>
//             {
//                 anyRaised = true;
//                 capturedAnyEvent = e;
//             };
// 
//             // Act
//             _dispatcher.Dispatch(buildEvent);
// 
//             // Assert
//             taskFinishedRaised.Should().BeTrue("TaskFinishedEventArgs should trigger TaskFinished event.");
//             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
//             capturedTaskFinishedEvent.Should().Be(buildEvent, "the event argument should match the dispatched TaskFinishedEventArgs.");
//             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
//         }
// //  // [Error] (134-34)CS1729 'TargetStartedEventArgs' does not contain a constructor that takes 4 arguments
// //         /// <summary>
// //         /// Tests that Dispatch correctly raises TargetStartedEventArgs and the generic AnyEventRaised event.
// //         /// </summary>
// //         [Fact]
// //         public void Dispatch_TargetStartedEventArgs_RaisesTargetStartedAndAnyEvent()
// //         {
// //             // Arrange
// //             bool targetStartedRaised = false;
// //             bool anyRaised = false;
// //             TargetStartedEventArgs? capturedTargetStartedEvent = null;
// //             BuildEventArgs? capturedAnyEvent = null;
// //             var buildEvent = new TargetStartedEventArgs("target", "projectFile", "targetFile", 1);
// // 
// //             _dispatcher.TargetStarted += (sender, e) =>
// //             {
// //                 targetStartedRaised = true;
// //                 capturedTargetStartedEvent = e;
// //             };
// //             _dispatcher.AnyEventRaised += (sender, e) =>
// //             {
// //                 anyRaised = true;
// //                 capturedAnyEvent = e;
// //             };
// // 
// //             // Act
// //             _dispatcher.Dispatch(buildEvent);
// // 
// //             // Assert
// //             targetStartedRaised.Should().BeTrue("TargetStartedEventArgs should trigger TargetStarted event.");
// //             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
// //             capturedTargetStartedEvent.Should().Be(buildEvent, "the event argument should match the dispatched TargetStartedEventArgs.");
// //             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
// //         }
// //  // [Error] (168-34)CS1729 'TargetFinishedEventArgs' does not contain a constructor that takes 5 arguments
//         /// <summary>
//         /// Tests that Dispatch correctly raises TargetFinishedEventArgs and the generic AnyEventRaised event.
//         /// </summary>
//         [Fact]
//         public void Dispatch_TargetFinishedEventArgs_RaisesTargetFinishedAndAnyEvent()
//         {
//             // Arrange
//             bool targetFinishedRaised = false;
//             bool anyRaised = false;
//             TargetFinishedEventArgs? capturedTargetFinishedEvent = null;
//             BuildEventArgs? capturedAnyEvent = null;
//             var buildEvent = new TargetFinishedEventArgs("target", "projectFile", "targetFile", 1, true);
// 
//             _dispatcher.TargetFinished += (sender, e) =>
//             {
//                 targetFinishedRaised = true;
//                 capturedTargetFinishedEvent = e;
//             };
//             _dispatcher.AnyEventRaised += (sender, e) =>
//             {
//                 anyRaised = true;
//                 capturedAnyEvent = e;
//             };
// 
//             // Act
//             _dispatcher.Dispatch(buildEvent);
// 
//             // Assert
//             targetFinishedRaised.Should().BeTrue("TargetFinishedEventArgs should trigger TargetFinished event.");
//             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
//             capturedTargetFinishedEvent.Should().Be(buildEvent, "the event argument should match the dispatched TargetFinishedEventArgs.");
//             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
//         }
// //  // [Error] (202-34)CS1729 'ProjectStartedEventArgs' does not contain a constructor that takes 4 arguments
// //         /// <summary>
// //         /// Tests that Dispatch correctly raises ProjectStartedEventArgs and the generic AnyEventRaised event.
// //         /// </summary>
// //         [Fact]
// //         public void Dispatch_ProjectStartedEventArgs_RaisesProjectStartedAndAnyEvent()
// //         {
// //             // Arrange
// //             bool projectStartedRaised = false;
// //             bool anyRaised = false;
// //             ProjectStartedEventArgs? capturedProjectStartedEvent = null;
// //             BuildEventArgs? capturedAnyEvent = null;
// //             var buildEvent = new ProjectStartedEventArgs("project", "targetNames", "projectFile", "toolsVersion");
// // 
// //             _dispatcher.ProjectStarted += (sender, e) =>
// //             {
// //                 projectStartedRaised = true;
// //                 capturedProjectStartedEvent = e;
// //             };
// //             _dispatcher.AnyEventRaised += (sender, e) =>
// //             {
// //                 anyRaised = true;
// //                 capturedAnyEvent = e;
// //             };
// // 
// //             // Act
// //             _dispatcher.Dispatch(buildEvent);
// // 
// //             // Assert
// //             projectStartedRaised.Should().BeTrue("ProjectStartedEventArgs should trigger ProjectStarted event.");
// //             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
// //             capturedProjectStartedEvent.Should().Be(buildEvent, "the event argument should match the dispatched ProjectStartedEventArgs.");
// //             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
// //         }
// //  // [Error] (236-100)CS1503 Argument 4: cannot convert from 'string' to 'bool' // [Error] (236-116)CS1503 Argument 5: cannot convert from 'bool' to 'System.DateTime'
//         /// <summary>
//         /// Tests that Dispatch correctly raises ProjectFinishedEventArgs and the generic AnyEventRaised event.
//         /// </summary>
//         [Fact]
//         public void Dispatch_ProjectFinishedEventArgs_RaisesProjectFinishedAndAnyEvent()
//         {
//             // Arrange
//             bool projectFinishedRaised = false;
//             bool anyRaised = false;
//             ProjectFinishedEventArgs? capturedProjectFinishedEvent = null;
//             BuildEventArgs? capturedAnyEvent = null;
//             var buildEvent = new ProjectFinishedEventArgs("project", "projectFile", "targetNames", "toolsVersion", true);
// 
//             _dispatcher.ProjectFinished += (sender, e) =>
//             {
//                 projectFinishedRaised = true;
//                 capturedProjectFinishedEvent = e;
//             };
//             _dispatcher.AnyEventRaised += (sender, e) =>
//             {
//                 anyRaised = true;
//                 capturedAnyEvent = e;
//             };
// 
//             // Act
//             _dispatcher.Dispatch(buildEvent);
// 
//             // Assert
//             projectFinishedRaised.Should().BeTrue("ProjectFinishedEventArgs should trigger ProjectFinished event.");
//             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
//             capturedProjectFinishedEvent.Should().Be(buildEvent, "the event argument should match the dispatched ProjectFinishedEventArgs.");
//             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
//         }
// 
//         /// <summary>
//         /// Tests that Dispatch correctly raises BuildStartedEventArgs and the generic AnyEventRaised event.
//         /// </summary>
//         [Fact]
//         public void Dispatch_BuildStartedEventArgs_RaisesBuildStartedAndAnyEvent()
//         {
//             // Arrange
//             bool buildStartedRaised = false;
//             bool anyRaised = false;
//             BuildStartedEventArgs? capturedBuildStartedEvent = null;
//             BuildEventArgs? capturedAnyEvent = null;
//             var buildEvent = new BuildStartedEventArgs("TestMessage", "TestHelp");
// 
//             _dispatcher.BuildStarted += (sender, e) =>
//             {
//                 buildStartedRaised = true;
//                 capturedBuildStartedEvent = e;
//             };
//             _dispatcher.AnyEventRaised += (sender, e) =>
//             {
//                 anyRaised = true;
//                 capturedAnyEvent = e;
//             };
// 
//             // Act
//             _dispatcher.Dispatch(buildEvent);
// 
//             // Assert
//             buildStartedRaised.Should().BeTrue("BuildStartedEventArgs should trigger BuildStarted event.");
//             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
//             capturedBuildStartedEvent.Should().Be(buildEvent, "the event argument should match the dispatched BuildStartedEventArgs.");
//             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
//         }
// 
//         /// <summary>
//         /// Tests that Dispatch correctly raises BuildFinishedEventArgs and the generic AnyEventRaised event.
//         /// </summary>
//         [Fact]
//         public void Dispatch_BuildFinishedEventArgs_RaisesBuildFinishedAndAnyEvent()
//         {
//             // Arrange
//             bool buildFinishedRaised = false;
//             bool anyRaised = false;
//             BuildFinishedEventArgs? capturedBuildFinishedEvent = null;
//             BuildEventArgs? capturedAnyEvent = null;
//             var buildEvent = new BuildFinishedEventArgs("TestMessage", "TestHelp", true);
// 
//             _dispatcher.BuildFinished += (sender, e) =>
//             {
//                 buildFinishedRaised = true;
//                 capturedBuildFinishedEvent = e;
//             };
//             _dispatcher.AnyEventRaised += (sender, e) =>
//             {
//                 anyRaised = true;
//                 capturedAnyEvent = e;
//             };
// 
//             // Act
//             _dispatcher.Dispatch(buildEvent);
// 
//             // Assert
//             buildFinishedRaised.Should().BeTrue("BuildFinishedEventArgs should trigger BuildFinished event.");
//             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
//             capturedBuildFinishedEvent.Should().Be(buildEvent, "the event argument should match the dispatched BuildFinishedEventArgs.");
//             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
//         }
// //  // [Error] (338-30)CS0144 Cannot create an instance of the abstract type or interface 'CustomBuildEventArgs'
// //         /// <summary>
// //         /// Tests that Dispatch correctly raises CustomBuildEventArgs and the generic AnyEventRaised event.
// //         /// </summary>
// //         [Fact]
// //         public void Dispatch_CustomBuildEventArgs_RaisesCustomEventAndAnyEvent()
// //         {
// //             // Arrange
// //             bool customEventRaised = false;
// //             bool anyRaised = false;
// //             CustomBuildEventArgs? capturedCustomEvent = null;
// //             BuildEventArgs? capturedAnyEvent = null;
// //             var buildEvent = new CustomBuildEventArgs("TestMessage", "TestHelp");
// // 
// //             _dispatcher.CustomEventRaised += (sender, e) =>
// //             {
// //                 customEventRaised = true;
// //                 capturedCustomEvent = e;
// //             };
// //             _dispatcher.AnyEventRaised += (sender, e) =>
// //             {
// //                 anyRaised = true;
// //                 capturedAnyEvent = e;
// //             };
// // 
// //             // Act
// //             _dispatcher.Dispatch(buildEvent);
// // 
// //             // Assert
// //             customEventRaised.Should().BeTrue("CustomBuildEventArgs should trigger CustomEventRaised event.");
// //             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
// //             capturedCustomEvent.Should().Be(buildEvent, "the event argument should match the dispatched CustomBuildEventArgs.");
// //             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
// //         }
// //  // [Error] (373-30)CS0144 Cannot create an instance of the abstract type or interface 'BuildStatusEventArgs'
//         /// <summary>
//         /// Tests that Dispatch correctly raises BuildStatusEventArgs and the generic AnyEventRaised event.
//         /// </summary>
//         [Fact]
//         public void Dispatch_BuildStatusEventArgs_RaisesStatusEventAndAnyEvent()
//         {
//             // Arrange
//             bool statusEventRaised = false;
//             bool anyRaised = false;
//             BuildStatusEventArgs? capturedStatusEvent = null;
//             BuildEventArgs? capturedAnyEvent = null;
//             // Constructing BuildStatusEventArgs using its constructor.
//             var buildEvent = new BuildStatusEventArgs("TestMessage", "TestHelp", "TestCode", "TestFile", 1);
// 
//             _dispatcher.StatusEventRaised += (sender, e) =>
//             {
//                 statusEventRaised = true;
//                 capturedStatusEvent = e;
//             };
//             _dispatcher.AnyEventRaised += (sender, e) =>
//             {
//                 anyRaised = true;
//                 capturedAnyEvent = e;
//             };
// 
//             // Act
//             _dispatcher.Dispatch(buildEvent);
// 
//             // Assert
//             statusEventRaised.Should().BeTrue("BuildStatusEventArgs should trigger StatusEventRaised event.");
//             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
//             capturedStatusEvent.Should().Be(buildEvent, "the event argument should match the dispatched BuildStatusEventArgs.");
//             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
//         }
// 
//         /// <summary>
//         /// Tests that Dispatch correctly raises BuildWarningEventArgs and the generic AnyEventRaised event.
//         /// </summary>
//         [Fact]
//         public void Dispatch_BuildWarningEventArgs_RaisesWarningEventAndAnyEvent()
//         {
//             // Arrange
//             bool warningRaised = false;
//             bool anyRaised = false;
//             BuildWarningEventArgs? capturedWarningEvent = null;
//             BuildEventArgs? capturedAnyEvent = null;
//             var buildEvent = new BuildWarningEventArgs("Subcategory", "Code", "File", 1, 1, 1, 1, "Warning Message", "HelpKeyword", "SenderName");
// 
//             _dispatcher.WarningRaised += (sender, e) =>
//             {
//                 warningRaised = true;
//                 capturedWarningEvent = e;
//             };
//             _dispatcher.AnyEventRaised += (sender, e) =>
//             {
//                 anyRaised = true;
//                 capturedAnyEvent = e;
//             };
// 
//             // Act
//             _dispatcher.Dispatch(buildEvent);
// 
//             // Assert
//             warningRaised.Should().BeTrue("BuildWarningEventArgs should trigger WarningRaised event.");
//             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
//             capturedWarningEvent.Should().Be(buildEvent, "the event argument should match the dispatched BuildWarningEventArgs.");
//             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
//         }
// 
//         /// <summary>
//         /// Tests that Dispatch correctly raises BuildErrorEventArgs and the generic AnyEventRaised event.
//         /// </summary>
//         [Fact]
//         public void Dispatch_BuildErrorEventArgs_RaisesErrorEventAndAnyEvent()
//         {
//             // Arrange
//             bool errorRaised = false;
//             bool anyRaised = false;
//             BuildErrorEventArgs? capturedErrorEvent = null;
//             BuildEventArgs? capturedAnyEvent = null;
//             var buildEvent = new BuildErrorEventArgs("Subcategory", "Code", "File", 1, 1, 1, 1, "Error Message", "HelpKeyword", "SenderName");
// 
//             _dispatcher.ErrorRaised += (sender, e) =>
//             {
//                 errorRaised = true;
//                 capturedErrorEvent = e;
//             };
//             _dispatcher.AnyEventRaised += (sender, e) =>
//             {
//                 anyRaised = true;
//                 capturedAnyEvent = e;
//             };
// 
//             // Act
//             _dispatcher.Dispatch(buildEvent);
// 
//             // Assert
//             errorRaised.Should().BeTrue("BuildErrorEventArgs should trigger ErrorRaised event.");
//             anyRaised.Should().BeTrue("AnyEventRaised should be triggered regardless of event type.");
//             capturedErrorEvent.Should().Be(buildEvent, "the event argument should match the dispatched BuildErrorEventArgs.");
//             capturedAnyEvent.Should().Be(buildEvent, "the generic event should have been raised with the correct argument.");
//         }
// 
//         /// <summary>
//         /// Tests that Dispatch with an unhandled BuildEventArgs subtype raises only the generic AnyEventRaised event.
//         /// </summary>
//         [Fact]
//         public void Dispatch_UnhandledEventType_RaisesOnlyAnyEvent()
//         {
//             // Arrange
//             bool anyRaised = false;
//             BuildEventArgs? capturedAnyEvent = null;
//             var buildEvent = new UnhandledBuildEventArgs("Unhandled Message", "Help");
// 
//             _dispatcher.AnyEventRaised += (sender, e) =>
//             {
//                 anyRaised = true;
//                 capturedAnyEvent = e;
//             };
// 
//             // Act
//             _dispatcher.Dispatch(buildEvent);
// 
//             // Assert
//             anyRaised.Should().BeTrue("AnyEventRaised should be triggered even if the event type is unhandled.");
//             capturedAnyEvent.Should().Be(buildEvent, "the generic event should receive the unhandled BuildEventArgs.");
//         }
// 
//         /// <summary>
//         /// Tests that HasStructuredEventsSubscribers property returns false when no events are subscribed.
//         /// </summary>
//         [Fact]
//         public void HasStructuredEventsSubscribers_ReturnsFalse_WhenNoEventsSubscribed()
//         {
//             // Arrange
//             // A new dispatcher instance that has no events subscribed.
//             var dispatcher = new EventArgsDispatcher();
// 
//             // Act
//             bool result = GetHasStructuredEventsSubscribers(dispatcher);
// 
//             // Assert
//             result.Should().BeFalse("No event handlers have been subscribed, so HasStructuredEventsSubscribers should be false.");
//         }
// 
//         /// <summary>
//         /// Tests that HasStructuredEventsSubscribers property returns true when at least one event is subscribed.
//         /// </summary>
//         [Fact]
//         public void HasStructuredEventsSubscribers_ReturnsTrue_WhenEventSubscribed()
//         {
//             // Arrange
//             // Subscribe to one of the events.
//             _dispatcher.AnyEventRaised += (sender, e) => { };
// 
//             // Act
//             bool result = GetHasStructuredEventsSubscribers(_dispatcher);
// 
//             // Assert
//             result.Should().BeTrue("At least one event is subscribed, so HasStructuredEventsSubscribers should be true.");
//         }
// 
//         /// <summary>
//         /// Tests that Dispatch with a null BuildEventArgs raises AnyEventRaised with a null argument without throwing.
//         /// </summary>
//         [Fact]
//         public void Dispatch_NullBuildEvent_RaisesAnyEventWithNullArgument()
//         {
//             // Arrange
//             bool anyRaised = false;
//             BuildEventArgs? capturedAnyEvent = null;
//             _dispatcher.AnyEventRaised += (sender, e) =>
//             {
//                 anyRaised = true;
//                 capturedAnyEvent = e;
//             };
// 
//             // Act
//             _dispatcher.Dispatch(null!);
// 
//             // Assert
//             anyRaised.Should().BeTrue("AnyEventRaised should be triggered even when build event is null.");
//             capturedAnyEvent.Should().BeNull("The event argument should be null as passed.");
//         }
// 
//         /// <summary>
//         /// Helper method to access the internal HasStructuredEventsSubscribers property via reflection.
//         /// </summary>
//         /// <param name="dispatcher">The EventArgsDispatcher instance.</param>
//         /// <returns>The value of HasStructuredEventsSubscribers property.</returns>
//         private static bool GetHasStructuredEventsSubscribers(EventArgsDispatcher dispatcher)
//         {
//             var property = typeof(EventArgsDispatcher).GetProperty("HasStructuredEventsSubscribers", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
//             if (property == null)
//             {
//                 throw new InvalidOperationException("Could not find the internal property HasStructuredEventsSubscribers.");
//             }
//             return (bool)property.GetValue(dispatcher)!;
//         }
// 
//         /// <summary>
//         /// A custom BuildEventArgs type that is unhandled by Dispatch.
//         /// </summary>
//         private class UnhandledBuildEventArgs : BuildEventArgs
//         {
//             public UnhandledBuildEventArgs(string message, string helpKeyword)
//                 : base(message, helpKeyword, null)
//             {
//             }
//         }
//     }
// }