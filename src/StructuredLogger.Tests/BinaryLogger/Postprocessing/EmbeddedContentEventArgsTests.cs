// using System;
// using System.IO;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="EmbeddedContentEventArgs"/> class.
//     /// </summary>
//     public class EmbeddedContentEventArgsTests
//     {
// //         private readonly BinaryLogRecordKind _sampleContentKind = BinaryLogRecordKind.Unknown; // Assuming Unknown is a valid enum value. [Error] (13-87)CS0117 'BinaryLogRecordKind' does not contain a definition for 'Unknown'
//         
//         /// <summary>
//         /// Tests that the constructor properly assigns the provided valid stream and content kind to the respective properties.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithValidStream_SetsPropertiesCorrectly()
//         {
//             // Arrange
//             using var memoryStream = new MemoryStream();
//             BinaryLogRecordKind expectedContentKind = _sampleContentKind;
// 
//             // Act
//             var eventArgs = new EmbeddedContentEventArgs(expectedContentKind, memoryStream);
// 
//             // Assert
//             Assert.Equal(expectedContentKind, eventArgs.ContentKind);
//             Assert.Equal(memoryStream, eventArgs.ContentStream);
//         }
// 
//         /// <summary>
//         /// Tests that the constructor accepts a null stream and still sets the ContentKind while leaving ContentStream as null.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithNullStream_SetsPropertiesCorrectly()
//         {
//             // Arrange
//             Stream nullStream = null;
//             BinaryLogRecordKind expectedContentKind = _sampleContentKind;
// 
//             // Act
//             var eventArgs = new EmbeddedContentEventArgs(expectedContentKind, nullStream);
// 
//             // Assert
//             Assert.Equal(expectedContentKind, eventArgs.ContentKind);
//             Assert.Null(eventArgs.ContentStream);
//         }
//     }
// }
