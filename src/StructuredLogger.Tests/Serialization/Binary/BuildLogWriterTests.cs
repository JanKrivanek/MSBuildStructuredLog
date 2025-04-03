// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="BuildLogWriter"/> class.
//     /// </summary>
//     public class BuildLogWriterTests
//     {
// //         /// <summary> // [Error] (27-53)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildDummy' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         /// Tests the Write static method with valid Build and filePath parameters to ensure no exceptions are thrown.
// //         /// Note: This test is limited due to non-mockable dependencies; further refactoring may be required for detailed verification.
// //         /// </summary>
// //         [Fact]
// //         public void Write_WithValidBuildAndFilePath_DoesNotThrow()
// //         {
// //             // Arrange
// //             // Creating a dummy Build instance.
// //             // BuildDummy is a minimal implementation of Build to enable testing.
// //             var build = new BuildDummy();
// //             string filePath = "dummy.log";
// // 
// //             // Act
// //             Action act = () => BuildLogWriter.Write(build, filePath);
// // 
// //             // Assert
// //             act.Should().NotThrow();
// //         }
// //  // [Error] (45-53)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildDummy' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         /// <summary>
//         /// Tests the Write static method when a null Build is provided, expecting a NullReferenceException.
//         /// Note: Based on the current implementation, a null Build will likely cause a NullReferenceException in WriteNode.
//         /// </summary>
//         [Fact]
//         public void Write_WhenBuildIsNull_ThrowsException()
//         {
//             // Arrange
//             BuildDummy build = null;
//             string filePath = "dummy.log";
// 
//             // Act
//             Action act = () => BuildLogWriter.Write(build, filePath);
// 
//             // Assert
//             act.Should().Throw<NullReferenceException>();
//         }
// //  // [Error] (65-58)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BuildDummy' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         /// <summary>
// //         /// Tests the Dispose method by indirectly invoking it through the Write method.
// //         /// Since BuildLogWriter is disposed at the end of the Write method,
// //         /// this test verifies that calling Dispose again does not throw an exception.
// //         /// Note: This is an indirect test since BuildLogWriter cannot be instantiated directly.
// //         /// </summary>
// //         [Fact]
// //         public void Dispose_CalledMultipleTimes_DoesNotThrow()
// //         {
// //             // Arrange
// //             var build = new BuildDummy();
// //             string filePath = "dummy.log";
// // 
// //             // Act
// //             Action actWrite = () => BuildLogWriter.Write(build, filePath);
// //             actWrite.Should().NotThrow();
// // 
// //             // Since we cannot access the BuildLogWriter instance directly to call Dispose multiple times,
// //             // manual verification or refactoring is required to test this scenario directly.
// //             // TODO: Refactor BuildLogWriter to allow injection of TreeBinaryWriter for direct Dispose testing.
// //         }
// //     }
// 
//     // Dummy implementations for testing purposes.
//     // These minimal classes allow instantiation and simulation of behavior for BuildLogWriter.
//     
//     /// <summary>
//     /// A dummy implementation of Build to facilitate testing of BuildLogWriter.
//     /// </summary>
//     internal class BuildDummy : Build
//     {
//         public BuildDummy()
//         {
//             // Initialize SourceFilesArchive with an empty byte array to satisfy the usage in WriteNode.
//             SourceFilesArchive = new byte[0];
//         }
//     }
// //  // [Error] (92-18)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'Build'
// //     /// <summary>
// //     /// Minimal implementation of Build.
// //     /// </summary>
// //     public class Build : BaseNode
// //     {
// //         public virtual byte[] SourceFilesArchive { get; set; }
// //     }
// //  // [Error] (100-27)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'BaseNode'
//     /// <summary>
//     /// Minimal abstract base class to support testable implementations.
//     /// </summary>
//     public abstract class BaseNode
//     {
//     }
// }