using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ResolveAssemblyReferenceTask"/> class.
    /// </summary>
    public class ResolveAssemblyReferenceTaskTests
    {
        private readonly ResolveAssemblyReferenceTask _task;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolveAssemblyReferenceTaskTests"/> class.
        /// </summary>
        public ResolveAssemblyReferenceTaskTests()
        {
            _task = new ResolveAssemblyReferenceTask();
        }

        /// <summary>
        /// Tests that a new instance of <see cref="ResolveAssemblyReferenceTask"/> has null default values for Inputs and Results properties.
        /// </summary>
        [Fact]
        public void Ctor_InitialProperties_AreNull()
        {
            // Act & Assert
            Assert.Null(_task.Inputs);
            Assert.Null(_task.Results);
        }

        /// <summary>
        /// Tests that setting the Inputs property to a valid Folder instance returns the expected value.
        /// </summary>
//         [Fact] [Error] (42-28)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' to 'Microsoft.Build.Logging.StructuredLogger.Folder' [Error] (43-35)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Folder' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder'
//         public void InputsProperty_SetAndGet_ReturnsExpectedValue()
//         {
//             // Arrange
//             Folder expectedFolder = new Folder();
// 
//             // Act
//             _task.Inputs = expectedFolder;
//             Folder actualFolder = _task.Inputs;
// 
//             // Assert
//             Assert.Equal(expectedFolder, actualFolder);
//         }

        /// <summary>
        /// Tests that setting the Results property to a valid Folder instance returns the expected value.
        /// </summary>
//         [Fact] [Error] (59-29)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' to 'Microsoft.Build.Logging.StructuredLogger.Folder' [Error] (60-35)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Folder' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder'
//         public void ResultsProperty_SetAndGet_ReturnsExpectedValue()
//         {
//             // Arrange
//             Folder expectedFolder = new Folder();
// 
//             // Act
//             _task.Results = expectedFolder;
//             Folder actualFolder = _task.Results;
// 
//             // Assert
//             Assert.Equal(expectedFolder, actualFolder);
//         }

        /// <summary>
        /// Tests that setting the Inputs property to null is properly handled.
        /// </summary>
//         [Fact] [Error] (73-28)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' to 'Microsoft.Build.Logging.StructuredLogger.Folder'
//         public void InputsProperty_SetToNull_ShouldBeNull()
//         {
//             // Arrange
//             _task.Inputs = new Folder();
// 
//             // Act
//             _task.Inputs = null;
// 
//             // Assert
//             Assert.Null(_task.Inputs);
//         }

        /// <summary>
        /// Tests that setting the Results property to null is properly handled.
        /// </summary>
//         [Fact] [Error] (89-29)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' to 'Microsoft.Build.Logging.StructuredLogger.Folder'
//         public void ResultsProperty_SetToNull_ShouldBeNull()
//         {
//             // Arrange
//             _task.Results = new Folder();
// 
//             // Act
//             _task.Results = null;
// 
//             // Assert
//             Assert.Null(_task.Results);
//         }
    }
}
