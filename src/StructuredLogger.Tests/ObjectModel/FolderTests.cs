// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Microsoft.Build.Logging.StructuredLogger.UnitTests;
// using System;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "Folder"/> class.
//     /// </summary>
//     public class FolderTests
//     {
//         private readonly TestFolder _folder;
//         public FolderTests()
//         {
//             _folder = new TestFolder();
//         }
// //  // [Error] (27-39)CS1061 'FolderTests.TestFolder' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'FolderTests.TestFolder' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Verifies that the <see cref = "Folder.TypeName"/> property returns the correct type name.
// //         /// </summary>
// //         [Fact]
// //         public void TypeName_Get_ReturnsFolder()
// //         {
// //             // Act
// //             string typeName = _folder.TypeName;
// //             // Assert
// //             typeName.Should().Be("Folder");
// //         }
// //  // [Error] (41-21)CS1061 'FolderTests.TestFolder' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'FolderTests.TestFolder' could be found (are you missing a using directive or an assembly reference?) // [Error] (44-35)CS1061 'FolderTests.TestFolder' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'FolderTests.TestFolder' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Verifies that setting <see cref = "Folder.IsLowRelevance"/> to true calls SetFlag and the getter returns true when not selected.
//         /// </summary>
//         [Fact]
//         public void IsLowRelevance_SetTrueAndNotSelected_FlagIsTrueAndGetterReturnsTrue()
//         {
//             // Arrange
//             _folder.IsSelectedValue = false;
//             // Act
//             _folder.IsLowRelevance = true;
//             // Assert
//             _folder.FlagValue.Should().BeTrue("because setting IsLowRelevance to true should set the corresponding flag");
//             bool result = _folder.IsLowRelevance;
//             result.Should().BeTrue("because the flag is set and IsSelected is false");
//         }
// //  // [Error] (59-21)CS1061 'FolderTests.TestFolder' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'FolderTests.TestFolder' could be found (are you missing a using directive or an assembly reference?) // [Error] (62-35)CS1061 'FolderTests.TestFolder' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'FolderTests.TestFolder' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Verifies that setting <see cref = "Folder.IsLowRelevance"/> to false calls SetFlag and the getter returns false.
// //         /// </summary>
// //         [Fact]
// //         public void IsLowRelevance_SetFalse_FlagIsFalseAndGetterReturnsFalse()
// //         {
// //             // Arrange
// //             // Simulate that the flag was initially true.
// //             _folder.FlagValue = true;
// //             _folder.IsSelectedValue = false;
// //             // Act
// //             _folder.IsLowRelevance = false;
// //             // Assert
// //             _folder.FlagValue.Should().BeFalse("because setting IsLowRelevance to false should clear the corresponding flag");
// //             bool result = _folder.IsLowRelevance;
// //             result.Should().BeFalse("because the flag is false regardless of IsSelected value");
// //         }
// //  // [Error] (76-35)CS1061 'FolderTests.TestFolder' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'FolderTests.TestFolder' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Verifies that the <see cref = "Folder.IsLowRelevance"/> getter returns false when IsSelected is true, even if the flag is set.
//         /// </summary>
//         [Fact]
//         public void IsLowRelevance_Get_FlagSetButSelected_ReturnsFalse()
//         {
//             // Arrange
//             _folder.FlagValue = true;
//             _folder.IsSelectedValue = true;
//             // Act
//             bool result = _folder.IsLowRelevance;
//             // Assert
//             result.Should().BeFalse("because even if the LowRelevance flag is set, the node is selected so IsLowRelevance must be false");
//         }
// //  // [Error] (94-35)CS1061 'FolderTests.TestFolder' does not contain a definition for 'IsLowRelevance' and no accessible extension method 'IsLowRelevance' accepting a first argument of type 'FolderTests.TestFolder' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Verifies that the <see cref = "Folder.IsLowRelevance"/> getter returns false when the flag is not set, regardless of IsSelected state.
// //         /// </summary>
// //         /// <param name = "isSelected">The IsSelected state.</param>
// //         [Theory]
// //         [InlineData(false)]
// //         [InlineData(true)]
// //         public void IsLowRelevance_Get_FlagNotSet_ReturnsFalse(bool isSelected)
// //         {
// //             // Arrange
// //             _folder.FlagValue = false;
// //             _folder.IsSelectedValue = isSelected;
// //             // Act
// //             bool result = _folder.IsLowRelevance;
// //             // Assert
// //             result.Should().BeFalse("because when the LowRelevance flag is not set, IsLowRelevance should always be false");
// //         }
// // 
//         // Test double to simulate behavior inherited from NamedNode.
//         private class TestFolder : Folder
//         {
//             // Represents the simulated flag state for NodeFlags.LowRelevance.
//             public bool FlagValue { get; set; }
//             // Represents the simulated IsSelected property.
//             public bool IsSelectedValue { get; set; }
// //  // [Error] (112-34)CS0115 'FolderTests.TestFolder.HasFlag(NodeFlags)': no suitable method found to override
// //             /// <summary>
// //             /// Overrides the HasFlag method to return the simulated flag state.
// //             /// </summary>
// //             /// <param name = "flag">The node flag to check.</param>
// //             /// <returns>True if the flag is set; otherwise, false.</returns>
// //             public override bool HasFlag(NodeFlags flag)
// //             {
// //                 if (flag == NodeFlags.LowRelevance)
// //                 {
// //                     return FlagValue;
// //                 }
// // 
// //                 return false;
// //             }
// //  // [Error] (127-34)CS0115 'FolderTests.TestFolder.SetFlag(NodeFlags, bool)': no suitable method found to override
//             /// <summary>
//             /// Overrides the SetFlag method to set the simulated flag state.
//             /// </summary>
//             /// <param name = "flag">The node flag to set.</param>
//             /// <param name = "value">The value to set for the flag.</param>
//             public override void SetFlag(NodeFlags flag, bool value)
//             {
//                 if (flag == NodeFlags.LowRelevance)
//                 {
//                     FlagValue = value;
//                 }
//             }
// //  // [Error] (138-34)CS0115 'FolderTests.TestFolder.IsSelected': no suitable method found to override
// //             /// <summary>
// //             /// Overrides the IsSelected property to return the simulated selected state.
// //             /// </summary>
// //             public override bool IsSelected => IsSelectedValue;
// //         }
//     }
// }