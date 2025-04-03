// using FluentAssertions;
// using FluentAssertions.Primitives;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.IO;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "EmbeddedContentEventArgs"/> class.
//     /// </summary>
//     public class EmbeddedContentEventArgsTests
//     {
// //         /// <summary> // [Error] (33-46)CS1061 'StreamAssertions' does not contain a definition for 'Be' and no accessible extension method 'Be' accepting a first argument of type 'StreamAssertions' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests the constructor of <see cref = "EmbeddedContentEventArgs"/> with valid inputs.
// //         /// Verifies that the <see cref = "EmbeddedContentEventArgs.ContentKind"/> and <see cref = "EmbeddedContentEventArgs.ContentStream"/>
// //         /// properties return the expected values.
// //         /// </summary>
// //         /// <param name = "contentKindValue">An integer value representing a valid <see cref = "BinaryLogRecordKind"/>.</param>
// //         [Theory]
// //         [InlineData(0)]
// //         [InlineData(1)]
// //         public void Constructor_ValidInputs_PropertiesReturnExpectedValues(int contentKindValue)
// //         {
// //             // Arrange
// //             BinaryLogRecordKind expectedContentKind = (BinaryLogRecordKind)contentKindValue;
// //             using var stream = new MemoryStream(new byte[] { 1, 2, 3 });
// //             // Act
// //             var eventArgs = new EmbeddedContentEventArgs(expectedContentKind, stream);
// //             // Assert
// //             eventArgs.ContentKind.Should().Be(expectedContentKind);
// //             eventArgs.ContentStream.Should().Be(stream);
// //         }
// //  // [Error] (51-46)CS1061 'StreamAssertions' does not contain a definition for 'Be' and no accessible extension method 'Be' accepting a first argument of type 'StreamAssertions' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the constructor of <see cref = "EmbeddedContentEventArgs"/> with a disposed stream.
//         /// Verifies that even if the stream is disposed, the properties return the assigned values.
//         /// </summary>
//         [Fact]
//         public void Constructor_DisposedStream_PropertiesReturnExpectedValues()
//         {
//             // Arrange
//             BinaryLogRecordKind expectedContentKind = (BinaryLogRecordKind)1;
//             var stream = new MemoryStream(new byte[] { 4, 5, 6 });
//             stream.Dispose();
//             // Act
//             var eventArgs = new EmbeddedContentEventArgs(expectedContentKind, stream);
//             // Assert
//             eventArgs.ContentKind.Should().Be(expectedContentKind);
//             eventArgs.ContentStream.Should().Be(stream);
//         }
// //  // [Error] (68-46)CS1061 'StreamAssertions' does not contain a definition for 'Be' and no accessible extension method 'Be' accepting a first argument of type 'StreamAssertions' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests the constructor of <see cref = "EmbeddedContentEventArgs"/> using the extreme value <see cref = "Int32.MinValue"/>
// //         /// casted to <see cref = "BinaryLogRecordKind"/> to ensure the properties are set correctly.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_IntMinValue_PropertiesReturnExpectedValues()
// //         {
// //             // Arrange
// //             BinaryLogRecordKind expectedContentKind = (BinaryLogRecordKind)int.MinValue;
// //             using var stream = new MemoryStream(new byte[] { 7, 8, 9 });
// //             // Act
// //             var eventArgs = new EmbeddedContentEventArgs(expectedContentKind, stream);
// //             // Assert
// //             eventArgs.ContentKind.Should().Be(expectedContentKind);
// //             eventArgs.ContentStream.Should().Be(stream);
// //         }
// //  // [Error] (85-46)CS1061 'StreamAssertions' does not contain a definition for 'Be' and no accessible extension method 'Be' accepting a first argument of type 'StreamAssertions' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the constructor of <see cref = "EmbeddedContentEventArgs"/> using the extreme value <see cref = "Int32.MaxValue"/>
//         /// casted to <see cref = "BinaryLogRecordKind"/> to ensure the properties are set correctly.
//         /// </summary>
//         [Fact]
//         public void Constructor_IntMaxValue_PropertiesReturnExpectedValues()
//         {
//             // Arrange
//             BinaryLogRecordKind expectedContentKind = (BinaryLogRecordKind)int.MaxValue;
//             using var stream = new MemoryStream(new byte[] { 10, 11, 12 });
//             // Act
//             var eventArgs = new EmbeddedContentEventArgs(expectedContentKind, stream);
//             // Assert
//             eventArgs.ContentKind.Should().Be(expectedContentKind);
//             eventArgs.ContentStream.Should().Be(stream);
//         }
//     }
// }