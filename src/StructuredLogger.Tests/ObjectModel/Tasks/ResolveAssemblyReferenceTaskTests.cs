// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ResolveAssemblyReferenceTask"/> class.
//     /// </summary>
//     public class ResolveAssemblyReferenceTaskTests
//     {
//         private readonly ResolveAssemblyReferenceTask _task;
// 
//         /// <summary>
//         /// Initializes a new instance of the <see cref="ResolveAssemblyReferenceTaskTests"/> class.
//         /// </summary>
//         public ResolveAssemblyReferenceTaskTests()
//         {
//             _task = new ResolveAssemblyReferenceTask();
//         }
// 
//         /// <summary>
//         /// Tests that the default value of the Inputs property is null before any assignment.
//         /// </summary>
//         [Fact]
//         public void Inputs_DefaultValue_ShouldBeNull()
//         {
//             // Act
//             var inputsValue = _task.Inputs;
// 
//             // Assert
//             inputsValue.Should().BeNull("because no value has been assigned to Inputs upon object initialization");
//         }
// //  // [Error] (45-28)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' to 'Microsoft.Build.Logging.StructuredLogger.Folder'
// //         /// <summary>
// //         /// Tests that setting the Inputs property returns the same value when retrieved.
// //         /// </summary>
// //         [Fact]
// //         public void Inputs_SetAndGet_WithValidFolder_ReturnsSameInstance()
// //         {
// //             // Arrange
// //             var expectedFolder = new Folder();
// // 
// //             // Act
// //             _task.Inputs = expectedFolder;
// //             var actualFolder = _task.Inputs;
// // 
// //             // Assert
// //             actualFolder.Should().BeSameAs(expectedFolder, "because the getter should return the exact instance that was set");
// //         }
// // 
//         /// <summary>
//         /// Tests that the default value of the Results property is null before any assignment.
//         /// </summary>
//         [Fact]
//         public void Results_DefaultValue_ShouldBeNull()
//         {
//             // Act
//             var resultsValue = _task.Results;
// 
//             // Assert
//             resultsValue.Should().BeNull("because no value has been assigned to Results upon object initialization");
//         }
// //  // [Error] (75-29)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' to 'Microsoft.Build.Logging.StructuredLogger.Folder'
// //         /// <summary>
// //         /// Tests that setting the Results property returns the same value when retrieved.
// //         /// </summary>
// //         [Fact]
// //         public void Results_SetAndGet_WithValidFolder_ReturnsSameInstance()
// //         {
// //             // Arrange
// //             var expectedFolder = new Folder();
// // 
// //             // Act
// //             _task.Results = expectedFolder;
// //             var actualFolder = _task.Results;
// // 
// //             // Assert
// //             actualFolder.Should().BeSameAs(expectedFolder, "because the getter should return the exact instance that was set");
// //         }
// //     }
// }