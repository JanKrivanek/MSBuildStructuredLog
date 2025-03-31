using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Progress"/> class.
    /// </summary>
    public class ProgressTests
    {
        private readonly Progress _progress;

        public ProgressTests()
        {
            _progress = new Progress();
        }

        /// <summary>
        /// Tests the Report(double) method to ensure it raises the Updated event with a ProgressUpdate 
        /// that has the correct Ratio and default BufferLength.
        /// </summary>
        [Fact]
        public void Report_DoubleParameter_InvokesUpdatedEventWithCorrectProgressUpdate()
        {
            // Arrange
            double expectedRatio = 0.75;
            ProgressUpdate? receivedUpdate = null;
            _progress.Updated += update => receivedUpdate = update;

            // Act
            _progress.Report(expectedRatio);

            // Assert
            receivedUpdate.Should().NotBeNull("the event should have been raised with a ProgressUpdate");
            receivedUpdate?.Ratio.Should().Be(expectedRatio, "the ratio in the progress update should match the provided value");
            receivedUpdate?.BufferLength.Should().Be(0, "BufferLength should be default (0) as it is not set");
        }

        /// <summary>
        /// Tests the Report(ProgressUpdate) method to ensure it raises the Updated event 
        /// with the provided ProgressUpdate instance.
        /// </summary>
        [Fact]
        public void Report_ProgressUpdateParameter_InvokesUpdatedEventWithProvidedProgressUpdate()
        {
            // Arrange
            ProgressUpdate expectedUpdate = new ProgressUpdate { Ratio = 0.5, BufferLength = 1024 };
            ProgressUpdate? receivedUpdate = null;
            _progress.Updated += update => receivedUpdate = update;

            // Act
            _progress.Report(expectedUpdate);

            // Assert
            receivedUpdate.Should().BeEquivalentTo(expectedUpdate, "the event should be raised with the same ProgressUpdate instance provided");
        }

        /// <summary>
        /// Tests the Report(ProgressUpdate) method when no subscribers are attached.
        /// It should complete without throwing any exception.
        /// </summary>
        [Fact]
        public void Report_WithoutSubscribers_DoesNotThrowException()
        {
            // Arrange
            // Unsubscribe all subscribers if any exist (for the sake of the test, create a new instance)
            var progressNoSubscriber = new Progress();
            ProgressUpdate update = new ProgressUpdate { Ratio = 1.0, BufferLength = 512 };

            // Act
            Action act = () => progressNoSubscriber.Report(update);

            // Assert
            act.Should().NotThrow("reporting progress with no subscribers should not throw any exception");
        }

        /// <summary>
        /// Tests the CancellationToken property to ensure it has a default value and can be set appropriately.
        /// </summary>
        [Fact]
        public void CancellationToken_SetAndGetProperty_WorksAsExpected()
        {
            // Arrange
            CancellationToken expectedToken = new CancellationTokenSource().Token;
            _progress.CancellationToken.Should().Be(CancellationToken.None, "the default CancellationToken should be CancellationToken.None");

            // Act
            _progress.CancellationToken = expectedToken;

            // Assert
            _progress.CancellationToken.Should().Be(expectedToken, "the CancellationToken property should return the value it was set to");
        }
    }
}
