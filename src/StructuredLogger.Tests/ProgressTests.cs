// using System;
// using System.Threading;
// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Progress"/> class.
//     /// </summary>
//     public class ProgressTests
//     {
//         private readonly Progress _progress;
// 
//         /// <summary>
//         /// Initializes a new instance of the <see cref="ProgressTests"/> class.
//         /// </summary>
//         public ProgressTests()
//         {
//             _progress = new Progress();
//         }
// 
//         /// <summary>
//         /// Tests that the CancellationToken property returns CancellationToken.None by default.
//         /// </summary>
//         [Fact]
//         public void CancellationToken_DefaultValue_ReturnsCancellationTokenNone()
//         {
//             // Act
//             CancellationToken token = _progress.CancellationToken;
// 
//             // Assert
//             token.Should().Be(CancellationToken.None);
//         }
// 
//         /// <summary>
//         /// Tests that the CancellationToken property can be set and retrieved correctly.
//         /// </summary>
//         [Fact]
//         public void CancellationToken_SetValue_ReturnsSetValue()
//         {
//             // Arrange
//             var expectedToken = new CancellationToken(true); // a canceled token
// 
//             // Act
//             _progress.CancellationToken = expectedToken;
//             CancellationToken actualToken = _progress.CancellationToken;
// 
//             // Assert
//             actualToken.Should().Be(expectedToken);
//         }
// 
//         /// <summary>
//         /// Tests the Report(double) method to ensure it invokes the Updated event with the correct ProgressUpdate.
//         /// Includes edge cases such as extreme double values and NaN.
//         /// </summary>
//         /// <param name="ratio">The ratio value to report.</param>
//         [Theory]
//         [InlineData(0.0)]
//         [InlineData(1.0)]
//         [InlineData(-1.0)]
//         [InlineData(double.MaxValue)]
//         [InlineData(double.MinValue)]
//         [InlineData(double.NaN)]
//         public void Report_Double_WithSubscriber_InvokesUpdatedWithCorrectRatio(double ratio)
//         {
//             // Arrange
//             ProgressUpdate? receivedUpdate = null;
//             _progress.Updated += update => receivedUpdate = update;
// 
//             // Act
//             _progress.Report(ratio);
// 
//             // Assert
//             receivedUpdate.Should().NotBeNull("because the Updated event should have been invoked with a progress update");
//             if (double.IsNaN(ratio))
//             {
//                 double.IsNaN(receivedUpdate?.Ratio ?? 0).Should().BeTrue("because the reported ratio is NaN");
//             }
//             else
//             {
//                 receivedUpdate?.Ratio.Should().Be(ratio, "because the reported ratio should match the input value");
//             }
//         }
// //  // [Error] (99-23)CS0070 The event 'Progress.Updated' can only appear on the left hand side of += or -= (except when used from within the type 'Progress')
// //         /// <summary>
// //         /// Tests the Report(double) method when no subscribers are attached to the Updated event,
// //         /// verifying that no exception is thrown regardless of the ratio value.
// //         /// </summary>
// //         /// <param name="ratio">The ratio value to report.</param>
// //         [Theory]
// //         [InlineData(0.0)]
// //         [InlineData(1.0)]
// //         public void Report_Double_NoSubscriber_DoesNotThrow(double ratio)
// //         {
// //             // Arrange
// //             // Detach any event subscribers.
// //             _progress.Updated = null;
// // 
// //             // Act
// //             Action act = () => _progress.Report(ratio);
// // 
// //             // Assert
// //             act.Should().NotThrow("because reporting progress with no subscribers should not throw an exception");
// //         }
// // 
//         /// <summary>
//         /// Tests the Report(ProgressUpdate) method to ensure it invokes the Updated event with the provided ProgressUpdate.
//         /// Covers various input scenarios including extreme values.
//         /// </summary>
//         /// <param name="ratio">The ratio value for the progress update.</param>
//         /// <param name="bufferLength">The buffer length for the progress update.</param>
//         [Theory]
//         [InlineData(0.5, 10)]
//         [InlineData(1.0, 0)]
//         [InlineData(-0.5, -10)]
//         [InlineData(double.MaxValue, int.MaxValue)]
//         [InlineData(double.MinValue, int.MinValue)]
//         public void Report_ProgressUpdate_WithSubscriber_InvokesUpdatedEvent(double ratio, int bufferLength)
//         {
//             // Arrange
//             var inputUpdate = new ProgressUpdate { Ratio = ratio, BufferLength = bufferLength };
//             ProgressUpdate? receivedUpdate = null;
//             _progress.Updated += update => receivedUpdate = update;
// 
//             // Act
//             _progress.Report(inputUpdate);
// 
//             // Assert
//             receivedUpdate.Should().NotBeNull("because the Updated event should have been invoked with the provided update");
//             if (double.IsNaN(ratio))
//             {
//                 double.IsNaN(receivedUpdate?.Ratio ?? 0).Should().BeTrue("because the reported ratio is NaN");
//             }
//             else
//             {
//                 receivedUpdate?.Ratio.Should().Be(ratio, "because the reported ratio should match the input update");
//             }
//             receivedUpdate?.BufferLength.Should().Be(bufferLength, "because the reported buffer length should match the input update");
//         }
// //  // [Error] (151-23)CS0070 The event 'Progress.Updated' can only appear on the left hand side of += or -= (except when used from within the type 'Progress')
// //         /// <summary>
// //         /// Tests the Report(ProgressUpdate) method when no subscribers are attached to the Updated event,
// //         /// ensuring that no exception is thrown.
// //         /// </summary>
// //         [Fact]
// //         public void Report_ProgressUpdate_NoSubscriber_DoesNotThrow()
// //         {
// //             // Arrange
// //             _progress.Updated = null;
// //             var update = new ProgressUpdate { Ratio = 0.75, BufferLength = 100 };
// // 
// //             // Act
// //             Action act = () => _progress.Report(update);
// // 
// //             // Assert
// //             act.Should().NotThrow("because reporting progress with no subscribers should not throw an exception");
// //         }
// //     }
// }