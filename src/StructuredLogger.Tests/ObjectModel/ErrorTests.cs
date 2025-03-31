// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Error"/> class.
//     /// </summary>
//     public class ErrorTests
//     {
//         private readonly Error _error;
// 
//         public ErrorTests()
//         {
//             _error = new Error();
//         }
// 
//         /// <summary>
//         /// Tests that the TypeName property of the Error class returns "Error".
//         /// </summary>
//         [Fact]
//         public void TypeName_WhenCalled_ReturnsError()
//         {
//             // Act
//             string result = _error.TypeName;
// 
//             // Assert
//             result.Should().Be("Error", because: "the TypeName property for Error should return its own name");
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="BuildError"/> class.
//     /// </summary>
//     public class BuildErrorTests
//     {
//         private readonly BuildError _buildError;
// 
//         public BuildErrorTests()
//         {
//             _buildError = new BuildError();
//         }
// //  // [Error] (52-41)CS1061 'BuildError' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'BuildError' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the TypeName property of the BuildError class returns "Build Error".
// //         /// </summary>
// //         [Fact]
// //         public void TypeName_WhenCalled_ReturnsBuildError()
// //         {
// //             // Act
// //             string result = _buildError.TypeName;
// // 
// //             // Assert
// //             result.Should().Be("Build Error", because: "the TypeName property for BuildError should return its specific string");
// //         }
// //     }
// }