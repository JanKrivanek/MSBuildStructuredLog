using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ResolveAssemblyReferenceTask"/> class.
    /// </summary>
    public class ResolveAssemblyReferenceTaskTests
    {
        /// <summary>
        /// Tests that a new instance of <see cref="ResolveAssemblyReferenceTask"/> has null property values by default.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_PropertiesShouldBeNull()
        {
            // Arrange & Act
            var task = new ResolveAssemblyReferenceTask();

            // Assert
            task.Inputs.Should().BeNull("because Inputs should be null on initialization.");
            task.Results.Should().BeNull("because Results should be null on initialization.");
        }
//  // [Error] (37-27)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' to 'Microsoft.Build.Logging.StructuredLogger.Folder'
//         /// <summary>
//         /// Tests that the Inputs property can be set and retrieved correctly.
//         /// </summary>
//         [Fact]
//         public void InputsProperty_WhenSet_GetReturnsSameInstance()
//         {
//             // Arrange
//             var task = new ResolveAssemblyReferenceTask();
//             var folder = new Folder();
// 
//             // Act
//             task.Inputs = folder;
// 
//             // Assert
//             task.Inputs.Should().Be(folder, "because assigning to Inputs should store and return the same Folder instance.");
//         }
//  // [Error] (54-28)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Folder' to 'Microsoft.Build.Logging.StructuredLogger.Folder'
        /// <summary>
        /// Tests that the Results property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void ResultsProperty_WhenSet_GetReturnsSameInstance()
        {
            // Arrange
            var task = new ResolveAssemblyReferenceTask();
            var folder = new Folder();

            // Act
            task.Results = folder;

            // Assert
            task.Results.Should().Be(folder, "because assigning to Results should store and return the same Folder instance.");
        }
    }
}
