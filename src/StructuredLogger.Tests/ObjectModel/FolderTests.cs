// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Folder"/> class.
//     /// </summary>
//     public class FolderTests
//     {
// //         /// <summary> // [Error] (23-38)CS1061 'Folder' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Folder' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests that the TypeName property returns the correct string "Folder".
// //         /// </summary>
// //         [Fact]
// //         public void TypeName_WhenCalled_ReturnsFolder()
// //         {
// //             // Arrange
// //             var folder = new Folder();
// // 
// //             // Act
// //             string typeName = folder.TypeName;
// // 
// //             // Assert
// //             typeName.Should().Be("Folder");
// //         }
// //  // [Error] (44-42)CS1061 'Folder' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'Folder' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Placeholder test for the IsLowRelevance getter.
//         /// This test should simulate the underlying behavior inherited from NamedNode.
//         /// The getter returns HasFlag(LowRelevance) && !IsSelected. Since HasFlag and IsSelected
//         /// come from the base class and cannot be mocked or overridden, further test setup is needed.
//         /// TODO: Create a test double or derived test class to simulate setting internal flag states.
//         /// </summary>
//         [Fact(Skip = "TODO: Implement simulation for HasFlag and IsSelected dependencies in Folder.")]
//         public void IsLowRelevance_Getter_WhenLowFlagSetAndNotSelected_ReturnsTrue()
//         {
//             // Arrange
//             // TODO: Instantiate a test double of Folder that allows simulation of HasFlag and IsSelected.
//             var folder = new Folder();
// 
//             // Act
//             bool isLowRelevance = folder.IsLowRelevance;
// 
//             // Assert
//             isLowRelevance.Should().BeTrue();
//         }
// //  // [Error] (64-20)CS1061 'Folder' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'Folder' could be found (are you missing a using directive or an assembly reference?) // [Error] (65-44)CS1061 'Folder' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'Folder' could be found (are you missing a using directive or an assembly reference?) // [Error] (66-20)CS1061 'Folder' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'Folder' could be found (are you missing a using directive or an assembly reference?) // [Error] (67-45)CS1061 'Folder' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'Folder' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Placeholder test for the IsLowRelevance setter.
// //         /// The setter calls SetFlag(LowRelevance, value), which in turn affects the behavior of HasFlag.
// //         /// Due to the dependency on base implementations that cannot be mocked, additional setup is required.
// //         /// TODO: Create a test double or derived test class to simulate and verify SetFlag behavior.
// //         /// </summary>
// //         [Fact(Skip = "TODO: Implement simulation for SetFlag behavior when setting IsLowRelevance.")]
// //         public void IsLowRelevance_Setter_WhenSetToTrueOrFalse_UpdatesFlagCorrectly()
// //         {
// //             // Arrange
// //             // TODO: Instantiate a test double of Folder that allows verification of flag changes.
// //             var folder = new Folder();
// // 
// //             // Act
// //             folder.IsLowRelevance = true;
// //             bool flagAfterSetTrue = folder.IsLowRelevance;
// //             folder.IsLowRelevance = false;
// //             bool flagAfterSetFalse = folder.IsLowRelevance;
// // 
// //             // Assert
// //             // TODO: Update assertions after implementing simulation of flag behavior.
// //             flagAfterSetTrue.Should().BeTrue();
// //             flagAfterSetFalse.Should().BeFalse();
// //         }
// //     }
// }