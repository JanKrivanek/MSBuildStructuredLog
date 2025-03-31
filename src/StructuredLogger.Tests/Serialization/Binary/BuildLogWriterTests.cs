// using FluentAssertions;
// using Moq;
// using System;
// using System.IO;
// using System.Reflection;
// using Xunit;
// using Microsoft.Build.Logging.StructuredLogger;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="BuildLogWriter"/> class.
//     /// </summary>
//     public class BuildLogWriterTests
//     {
//         private readonly string _tempFilePath;
// 
//         public BuildLogWriterTests()
//         {
//             // Create a temporary file path for file based tests.
//             _tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".binlog");
//         }
// //  // [Error] (44-53)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildLogWriterTests.DummyBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         /// <summary>
// //         /// Tests that the static Write method completes without throwing an exception
// //         /// when provided with a valid Build instance and a valid file path.
// //         /// This test sets up a dummy Build instance with minimal required properties.
// //         /// Expected outcome: the method executes successfully.
// //         /// </summary>
// //         [Fact]
// //         public void Write_ValidBuild_DoesNotThrow()
// //         {
// //             // Arrange
// //             // Assuming that Build is a subclass of BaseNode and has public parameterless constructor.
// //             // Also assuming that SourceFilesArchive is a public property of type byte[].
// //             var build = new DummyBuild
// //             {
// //                 SourceFilesArchive = new byte[] { 1, 2, 3 },
// //                 Succeeded = true,
// //                 IsAnalyzed = false
// //             };
// // 
// //             // Act
// //             Action act = () => BuildLogWriter.Write(build, _tempFilePath);
// // 
// //             // Assert
// //             act.Should().NotThrow();
// //             
// //             // Clean up temporary file if created.
// //             if(File.Exists(_tempFilePath))
// //             {
// //                 File.Delete(_tempFilePath);
// //             }
// //         }
// //  // [Error] (68-53)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildLogWriterTests.DummyBuild' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         /// <summary>
//         /// Tests that the static Write method throws a NullReferenceException
//         /// when provided with a null Build instance.
//         /// Expected outcome: the method throws an exception due to a null dereference.
//         /// </summary>
//         [Fact]
//         public void Write_NullBuild_ThrowsNullReferenceException()
//         {
//             // Arrange
//             DummyBuild build = null;
// 
//             // Act
//             Action act = () => BuildLogWriter.Write(build, _tempFilePath);
// 
//             // Assert
//             act.Should().Throw<NullReferenceException>();
//         }
// 
//         /// <summary>
//         /// Tests that the Dispose method properly disposes the underlying writer
//         /// and sets the writer field to null.
//         /// This verification is achieved by replacing the private writer field with a mock.
//         /// Expected outcome: the Dispose method of the writer is called and the internal reference is cleared.
//         /// </summary>
//         [Fact]
//         public void Dispose_WhenCalled_CallsWriterDisposeAndSetsWriterToNull()
//         {
//             // Arrange
//             // Create an instance of BuildLogWriter using its non-public constructor.
//             var instance = (BuildLogWriter)Activator.CreateInstance(
//                 typeof(BuildLogWriter),
//                 BindingFlags.Instance | BindingFlags.NonPublic,
//                 binder: null,
//                 args: new object[] { "dummyPath" },
//                 culture: null);
//             instance.Should().NotBeNull();
// 
//             // Create a mock for TreeBinaryWriter.
//             var writerMock = new Mock<TreeBinaryWriter>("dummyPath");
//             writerMock.Setup(w => w.Dispose()).Verifiable();
// 
//             // Use reflection to set the private field 'writer' to the mock.
//             var writerField = typeof(BuildLogWriter).GetField("writer", BindingFlags.Instance | BindingFlags.NonPublic);
//             writerField.Should().NotBeNull();
//             writerField.SetValue(instance, writerMock.Object);
// 
//             // Act
//             instance.Dispose();
// 
//             // Assert
//             writerMock.Verify(w => w.Dispose(), Times.Once, "Dispose should be called exactly once on the writer.");
//             var writerValue = writerField.GetValue(instance);
//             writerValue.Should().BeNull("The writer field should be set to null after disposing.");
//         }
// //  // [Error] (120-23)CS0534 'BuildLogWriterTests.DummyBuild' does not implement inherited abstract member 'BaseNode.GetFullText()'
// //         #region Dummy Classes for Testing
// // 
// //         // The following dummy classes are provided solely for the purpose of unit testing
// //         // the BuildLogWriter class. In the actual production code, these classes are assumed
// //         // to be fully implemented.
// //         
// //         /// <summary>
// //         /// Dummy implementation of Build that derives from BaseNode.
// //         /// </summary>
// //         private class DummyBuild : BaseNode
// //         {
// //             public byte[] SourceFilesArchive { get; set; }
// //             public bool Succeeded { get; set; }
// //             public bool IsAnalyzed { get; set; }
// //         }
// // 
//         #endregion
//     }
// }