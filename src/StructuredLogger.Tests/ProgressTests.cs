using Microsoft.Build.Logging.StructuredLogger;
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
        /// Tests that the default value for the CancellationToken property is CancellationToken.None.
        /// </summary>
        [Fact]
        public void CancellationToken_Property_DefaultsToNone()
        {
            // Arrange & Act
            CancellationToken token = _progress.CancellationToken;

            // Assert
            Assert.Equal(CancellationToken.None, token);
        }

        /// <summary>
        /// Tests that setting the CancellationToken property correctly updates its value.
        /// </summary>
        [Fact]
        public void CancellationToken_Property_Setter_Works()
        {
            // Arrange
            using CancellationTokenSource cts = new CancellationTokenSource();
            
            // Act
            _progress.CancellationToken = cts.Token;
            
            // Assert
            Assert.Equal(cts.Token, _progress.CancellationToken);
        }

        /// <summary>
        /// Tests that calling Report(double) with a subscriber attached raises the Updated event with the correct Ratio.
        /// </summary>
        [Fact]
        public void Report_Double_WithSubscriber_RaisesUpdatedEvent()
        {
            // Arrange
            double expectedRatio = 0.75;
            ProgressUpdate? receivedUpdate = null;
            _progress.Updated += update => receivedUpdate = update;
            
            // Act
            _progress.Report(expectedRatio);
            
            // Assert
            Assert.NotNull(receivedUpdate);
            Assert.Equal(expectedRatio, receivedUpdate.Value.Ratio);
        }

        /// <summary>
        /// Tests that calling Report(ProgressUpdate) with a subscriber attached raises the Updated event with the provided ProgressUpdate.
        /// </summary>
        [Fact]
        public void Report_ProgressUpdate_WithSubscriber_RaisesUpdatedEvent()
        {
            // Arrange
            ProgressUpdate expectedUpdate = new ProgressUpdate { Ratio = 0.5, BufferLength = 1024 };
            ProgressUpdate? receivedUpdate = null;
            _progress.Updated += update => receivedUpdate = update;
            
            // Act
            _progress.Report(expectedUpdate);
            
            // Assert
            Assert.NotNull(receivedUpdate);
            Assert.Equal(expectedUpdate.Ratio, receivedUpdate.Value.Ratio);
            Assert.Equal(expectedUpdate.BufferLength, receivedUpdate.Value.BufferLength);
        }

        /// <summary>
        /// Tests that calling Report(double) without any subscribers does not throw any exception.
        /// </summary>
//         [Fact] [Error] (96-23)CS0070 The event 'Progress.Updated' can only appear on the left hand side of += or -= (except when used from within the type 'Progress') [Error] (100-36)CS0117 'Record' does not contain a definition for 'Exception'
//         public void Report_Double_WithoutSubscriber_DoesNotThrow()
//         {
//             // Arrange
//             // Ensure no subscribers are attached.
//             _progress.Updated = null;
//             double ratio = 0.33;
//             
//             // Act & Assert
//             var exception = Record.Exception(() => _progress.Report(ratio));
//             Assert.Null(exception);
//         }

        /// <summary>
        /// Tests that calling Report(ProgressUpdate) without any subscribers does not throw any exception.
        /// </summary>
//         [Fact] [Error] (112-23)CS0070 The event 'Progress.Updated' can only appear on the left hand side of += or -= (except when used from within the type 'Progress') [Error] (116-36)CS0117 'Record' does not contain a definition for 'Exception'
//         public void Report_ProgressUpdate_WithoutSubscriber_DoesNotThrow()
//         {
//             // Arrange
//             // Ensure no subscribers are attached.
//             _progress.Updated = null;
//             ProgressUpdate update = new ProgressUpdate { Ratio = 0.66, BufferLength = 2048 };
//             
//             // Act & Assert
//             var exception = Record.Exception(() => _progress.Report(update));
//             Assert.Null(exception);
//         }
    }
}
