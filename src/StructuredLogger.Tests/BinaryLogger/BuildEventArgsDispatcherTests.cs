using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="EventArgsDispatcher"/> class.
    /// </summary>
    public class EventArgsDispatcherTests
    {
        private readonly EventArgsDispatcher _dispatcher;

        public EventArgsDispatcherTests()
        {
            _dispatcher = new EventArgsDispatcher();
        }

        #region Helper Classes

        // A dummy build event args class that does not match any specific type.
        private class DummyBuildEventArgs : BuildEventArgs
        {
            public DummyBuildEventArgs() : base(null, null, null)
            {
            }
        }

        #endregion

        #region BuildMessageEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with BuildMessageEventArgs triggers MessageRaised and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenBuildMessageEventArgsProvided_InvokesMessageRaisedAndAnyEventRaised()
        {
            // Arrange
            bool messageRaisedInvoked = false;
            bool anyEventRaisedInvoked = false;
            BuildMessageEventArgs capturedMessageArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.MessageRaised += (sender, args) =>
            {
                messageRaisedInvoked = true;
                capturedMessageArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var messageEvent = new BuildMessageEventArgs("Test message", null, "TestCategory", MessageImportance.Low);

            // Act
            _dispatcher.Dispatch(messageEvent);

            // Assert
            Assert.True(messageRaisedInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(messageEvent, capturedMessageArgs);
            Assert.Equal(messageEvent, capturedAnyArgs);
        }

        #endregion

        #region TaskStartedEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with TaskStartedEventArgs triggers TaskStarted and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenTaskStartedEventArgsProvided_InvokesTaskStartedAndAnyEventRaised()
        {
            // Arrange
            bool taskStartedInvoked = false;
            bool anyEventRaisedInvoked = false;
            TaskStartedEventArgs capturedTaskStartedArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.TaskStarted += (sender, args) =>
            {
                taskStartedInvoked = true;
                capturedTaskStartedArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var taskStartedEvent = new TaskStartedEventArgs("TestTask", "Task started", null, "Sender");

            // Act
            _dispatcher.Dispatch(taskStartedEvent);

            // Assert
            Assert.True(taskStartedInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(taskStartedEvent, capturedTaskStartedArgs);
            Assert.Equal(taskStartedEvent, capturedAnyArgs);
        }

        #endregion

        #region TaskFinishedEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with TaskFinishedEventArgs triggers TaskFinished and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenTaskFinishedEventArgsProvided_InvokesTaskFinishedAndAnyEventRaised()
        {
            // Arrange
            bool taskFinishedInvoked = false;
            bool anyEventRaisedInvoked = false;
            TaskFinishedEventArgs capturedTaskFinishedArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.TaskFinished += (sender, args) =>
            {
                taskFinishedInvoked = true;
                capturedTaskFinishedArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var taskFinishedEvent = new TaskFinishedEventArgs("TestTask", "Task finished", null, "Sender", true);

            // Act
            _dispatcher.Dispatch(taskFinishedEvent);

            // Assert
            Assert.True(taskFinishedInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(taskFinishedEvent, capturedTaskFinishedArgs);
            Assert.Equal(taskFinishedEvent, capturedAnyArgs);
        }

        #endregion

        #region TargetStartedEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with TargetStartedEventArgs triggers TargetStarted and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenTargetStartedEventArgsProvided_InvokesTargetStartedAndAnyEventRaised()
        {
            // Arrange
            bool targetStartedInvoked = false;
            bool anyEventRaisedInvoked = false;
            TargetStartedEventArgs capturedTargetStartedArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.TargetStarted += (sender, args) =>
            {
                targetStartedInvoked = true;
                capturedTargetStartedArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var targetStartedEvent = new TargetStartedEventArgs("TestTarget", "Target started", null, "Sender");

            // Act
            _dispatcher.Dispatch(targetStartedEvent);

            // Assert
            Assert.True(targetStartedInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(targetStartedEvent, capturedTargetStartedArgs);
            Assert.Equal(targetStartedEvent, capturedAnyArgs);
        }

        #endregion

        #region TargetFinishedEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with TargetFinishedEventArgs triggers TargetFinished and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenTargetFinishedEventArgsProvided_InvokesTargetFinishedAndAnyEventRaised()
        {
            // Arrange
            bool targetFinishedInvoked = false;
            bool anyEventRaisedInvoked = false;
            TargetFinishedEventArgs capturedTargetFinishedArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.TargetFinished += (sender, args) =>
            {
                targetFinishedInvoked = true;
                capturedTargetFinishedArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var targetFinishedEvent = new TargetFinishedEventArgs("TestTarget", "Target finished", null, "Sender", true);

            // Act
            _dispatcher.Dispatch(targetFinishedEvent);

            // Assert
            Assert.True(targetFinishedInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(targetFinishedEvent, capturedTargetFinishedArgs);
            Assert.Equal(targetFinishedEvent, capturedAnyArgs);
        }

        #endregion

        #region ProjectStartedEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with ProjectStartedEventArgs triggers ProjectStarted and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenProjectStartedEventArgsProvided_InvokesProjectStartedAndAnyEventRaised()
        {
            // Arrange
            bool projectStartedInvoked = false;
            bool anyEventRaisedInvoked = false;
            ProjectStartedEventArgs capturedProjectStartedArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.ProjectStarted += (sender, args) =>
            {
                projectStartedInvoked = true;
                capturedProjectStartedArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var projectStartedEvent = new ProjectStartedEventArgs("TestProject", "Project started", null, "Sender", null);

            // Act
            _dispatcher.Dispatch(projectStartedEvent);

            // Assert
            Assert.True(projectStartedInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(projectStartedEvent, capturedProjectStartedArgs);
            Assert.Equal(projectStartedEvent, capturedAnyArgs);
        }

        #endregion

        #region ProjectFinishedEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with ProjectFinishedEventArgs triggers ProjectFinished and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenProjectFinishedEventArgsProvided_InvokesProjectFinishedAndAnyEventRaised()
        {
            // Arrange
            bool projectFinishedInvoked = false;
            bool anyEventRaisedInvoked = false;
            ProjectFinishedEventArgs capturedProjectFinishedArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.ProjectFinished += (sender, args) =>
            {
                projectFinishedInvoked = true;
                capturedProjectFinishedArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var projectFinishedEvent = new ProjectFinishedEventArgs("TestProject", "Project finished", null, "Sender", null, true);

            // Act
            _dispatcher.Dispatch(projectFinishedEvent);

            // Assert
            Assert.True(projectFinishedInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(projectFinishedEvent, capturedProjectFinishedArgs);
            Assert.Equal(projectFinishedEvent, capturedAnyArgs);
        }

        #endregion

        #region BuildStartedEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with BuildStartedEventArgs triggers BuildStarted and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenBuildStartedEventArgsProvided_InvokesBuildStartedAndAnyEventRaised()
        {
            // Arrange
            bool buildStartedInvoked = false;
            bool anyEventRaisedInvoked = false;
            BuildStartedEventArgs capturedBuildStartedArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.BuildStarted += (sender, args) =>
            {
                buildStartedInvoked = true;
                capturedBuildStartedArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var buildStartedEvent = new BuildStartedEventArgs("Build started", "Sender");

            // Act
            _dispatcher.Dispatch(buildStartedEvent);

            // Assert
            Assert.True(buildStartedInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(buildStartedEvent, capturedBuildStartedArgs);
            Assert.Equal(buildStartedEvent, capturedAnyArgs);
        }

        #endregion

        #region BuildFinishedEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with BuildFinishedEventArgs triggers BuildFinished and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenBuildFinishedEventArgsProvided_InvokesBuildFinishedAndAnyEventRaised()
        {
            // Arrange
            bool buildFinishedInvoked = false;
            bool anyEventRaisedInvoked = false;
            BuildFinishedEventArgs capturedBuildFinishedArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.BuildFinished += (sender, args) =>
            {
                buildFinishedInvoked = true;
                capturedBuildFinishedArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var buildFinishedEvent = new BuildFinishedEventArgs("Build finished", "Sender", true);

            // Act
            _dispatcher.Dispatch(buildFinishedEvent);

            // Assert
            Assert.True(buildFinishedInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(buildFinishedEvent, capturedBuildFinishedArgs);
            Assert.Equal(buildFinishedEvent, capturedAnyArgs);
        }

        #endregion

        #region CustomBuildEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with CustomBuildEventArgs triggers CustomEventRaised and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenCustomBuildEventArgsProvided_InvokesCustomEventRaisedAndAnyEventRaised()
        {
            // Arrange
            bool customEventInvoked = false;
            bool anyEventRaisedInvoked = false;
            CustomBuildEventArgs capturedCustomArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.CustomEventRaised += (sender, args) =>
            {
                customEventInvoked = true;
                capturedCustomArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var customEvent = new CustomBuildEventArgs("Custom event", null, "Sender");

            // Act
            _dispatcher.Dispatch(customEvent);

            // Assert
            Assert.True(customEventInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(customEvent, capturedCustomArgs);
            Assert.Equal(customEvent, capturedAnyArgs);
        }

        #endregion

        #region BuildStatusEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with BuildStatusEventArgs triggers StatusEventRaised and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenBuildStatusEventArgsProvided_InvokesStatusEventRaisedAndAnyEventRaised()
        {
            // Arrange
            bool statusEventInvoked = false;
            bool anyEventRaisedInvoked = false;
            BuildStatusEventArgs capturedStatusArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.StatusEventRaised += (sender, args) =>
            {
                statusEventInvoked = true;
                capturedStatusArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var statusEvent = new BuildStatusEventArgs("Status event", null, "Sender");

            // Act
            _dispatcher.Dispatch(statusEvent);

            // Assert
            Assert.True(statusEventInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(statusEvent, capturedStatusArgs);
            Assert.Equal(statusEvent, capturedAnyArgs);
        }

        #endregion

        #region BuildWarningEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with BuildWarningEventArgs triggers WarningRaised and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenBuildWarningEventArgsProvided_InvokesWarningRaisedAndAnyEventRaised()
        {
            // Arrange
            bool warningInvoked = false;
            bool anyEventRaisedInvoked = false;
            BuildWarningEventArgs capturedWarningArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.WarningRaised += (sender, args) =>
            {
                warningInvoked = true;
                capturedWarningArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var warningEvent = new BuildWarningEventArgs("Test warning", "Code", "File", 0, 0, 0, 0, "Warning message", "Sender");

            // Act
            _dispatcher.Dispatch(warningEvent);

            // Assert
            Assert.True(warningInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(warningEvent, capturedWarningArgs);
            Assert.Equal(warningEvent, capturedAnyArgs);
        }

        #endregion

        #region BuildErrorEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with BuildErrorEventArgs triggers ErrorRaised and AnyEventRaised events.
        /// </summary>
        [Fact]
        public void Dispatch_WhenBuildErrorEventArgsProvided_InvokesErrorRaisedAndAnyEventRaised()
        {
            // Arrange
            bool errorInvoked = false;
            bool anyEventRaisedInvoked = false;
            BuildErrorEventArgs capturedErrorArgs = null;
            BuildEventArgs capturedAnyArgs = null;

            _dispatcher.ErrorRaised += (sender, args) =>
            {
                errorInvoked = true;
                capturedErrorArgs = args;
            };

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var errorEvent = new BuildErrorEventArgs("Subcategory", "Code", "File", 1, 1, 1, 1, "Error message", "HelpKeyword", "Sender");

            // Act
            _dispatcher.Dispatch(errorEvent);

            // Assert
            Assert.True(errorInvoked);
            Assert.True(anyEventRaisedInvoked);
            Assert.Equal(errorEvent, capturedErrorArgs);
            Assert.Equal(errorEvent, capturedAnyArgs);
        }

        #endregion

        #region Dummy EventArgs Tests

        /// <summary>
        /// Tests that Dispatch with an unrecognized BuildEventArgs type only triggers AnyEventRaised.
        /// </summary>
        [Fact]
        public void Dispatch_WhenUnrecognizedBuildEventArgsProvided_InvokesOnlyAnyEventRaised()
        {
            // Arrange
            bool anyEventRaisedInvoked = false;
            BuildEventArgs capturedAnyArgs = null;
            bool customEventInvoked = false;

            // Subscribe to all events that could be mistakenly triggered.
            _dispatcher.MessageRaised += (sender, args) => customEventInvoked = true;
            _dispatcher.TaskStarted += (sender, args) => customEventInvoked = true;
            _dispatcher.TaskFinished += (sender, args) => customEventInvoked = true;
            _dispatcher.TargetStarted += (sender, args) => customEventInvoked = true;
            _dispatcher.TargetFinished += (sender, args) => customEventInvoked = true;
            _dispatcher.ProjectStarted += (sender, args) => customEventInvoked = true;
            _dispatcher.ProjectFinished += (sender, args) => customEventInvoked = true;
            _dispatcher.BuildStarted += (sender, args) => customEventInvoked = true;
            _dispatcher.BuildFinished += (sender, args) => customEventInvoked = true;
            _dispatcher.CustomEventRaised += (sender, args) => customEventInvoked = true;
            _dispatcher.StatusEventRaised += (sender, args) => customEventInvoked = true;
            _dispatcher.WarningRaised += (sender, args) => customEventInvoked = true;
            _dispatcher.ErrorRaised += (sender, args) => customEventInvoked = true;

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            var dummyEvent = new DummyBuildEventArgs();

            // Act
            _dispatcher.Dispatch(dummyEvent);

            // Assert
            Assert.True(anyEventRaisedInvoked);
            Assert.False(customEventInvoked);
            Assert.Equal(dummyEvent, capturedAnyArgs);
        }

        #endregion

        #region Null BuildEventArgs Tests

        /// <summary>
        /// Tests that Dispatch with a null BuildEventArgs only triggers AnyEventRaised if subscribed and passes null.
        /// </summary>
        [Fact]
        public void Dispatch_WhenNullBuildEventArgsProvided_InvokesAnyEventRaisedWithNull()
        {
            // Arrange
            bool anyEventRaisedInvoked = false;
            BuildEventArgs capturedAnyArgs = new BuildMessageEventArgs("dummy", null, "dummy", MessageImportance.Low); // initial non-null value

            _dispatcher.AnyEventRaised += (sender, args) =>
            {
                anyEventRaisedInvoked = true;
                capturedAnyArgs = args;
            };

            // Act
            _dispatcher.Dispatch(null);

            // Assert
            Assert.True(anyEventRaisedInvoked);
            Assert.Null(capturedAnyArgs);
        }

        #endregion
    }
}
