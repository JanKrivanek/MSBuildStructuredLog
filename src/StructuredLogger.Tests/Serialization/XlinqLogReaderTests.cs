// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Xml.Linq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "XlinqLogReader"/> class.
//     /// </summary>
//     public class XlinqLogReaderTests
//     {
// //         /// <summary> // [Error] (33-32)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build'
// //         /// Tests the ReadFromXml method with a valid XML file and no status update action.
// //         /// This test creates a temporary XML file representing a Build node and asserts that
// //         /// a Build object is returned from the method.
// //         /// </summary>
// //         [Fact]
// //         public void ReadFromXml_ValidXmlWithoutStatusUpdate_ReturnsBuild()
// //         {
// //             // Arrange
// //             string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xml");
// //             // Minimal XML for a Build node with attributes simulating a successful build.
// //             string xmlContent = @"<Build Succeeded=""true"" IsAnalyzed=""false""></Build>";
// //             File.WriteAllText(tempFilePath, xmlContent);
// //             try
// //             {
// //                 // Act
// //                 Build result = XlinqLogReader.ReadFromXml(tempFilePath, null);
// //                 // Assert
// //                 result.Should().NotBeNull("a Build object should be returned for valid XML");
// //                 result.Should().BeOfType<Build>("the returned object should be of type Build");
// //             }
// //             finally
// //             {
// //                 // Clean up temporary file.
// //                 if (File.Exists(tempFilePath))
// //                 {
// //                     File.Delete(tempFilePath);
// //                 }
// //             }
// //         }
// //  // [Error] (64-32)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build'
//         /// <summary>
//         /// Tests the ReadFromXml method with a valid XML file and a provided status update action.
//         /// This test verifies that the supplied action is called with the expected status messages.
//         /// </summary>
//         [Fact]
//         public void ReadFromXml_WithStatusUpdate_CallsStatusUpdateMessages()
//         {
//             // Arrange
//             string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xml");
//             string xmlContent = @"<Build Succeeded=""true"" IsAnalyzed=""false""></Build>";
//             File.WriteAllText(tempFilePath, xmlContent);
//             var statusMessages = new List<string>();
//             Action<string> statusUpdate = message => statusMessages.Add(message);
//             try
//             {
//                 // Act
//                 Build result = XlinqLogReader.ReadFromXml(tempFilePath, statusUpdate);
//                 // Assert
//                 result.Should().NotBeNull("a Build object should be returned for valid XML");
//                 statusMessages.Should().ContainSingle(msg => msg.StartsWith("Loading "), "a loading message should be provided");
//                 statusMessages.Should().Contain("Populating tree", "a populating tree message should be provided");
//             }
//             finally
//             {
//                 // Clean up temporary file.
//                 if (File.Exists(tempFilePath))
//                 {
//                     File.Delete(tempFilePath);
//                 }
//             }
//         }
// //  // [Error] (91-28)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (94-20)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) // [Error] (97-28)CS0039 Cannot convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.TreeNode' via a reference conversion, boxing conversion, unboxing conversion, wrapping conversion, or null type conversion
// //         /// <summary>
// //         /// Tests the ReadFromXml method with an invalid file path.
// //         /// This test verifies that when an exception occurs (due to a missing file),
// //         /// the method returns a Build object with Succeeded set to false and two error children added.
// //         /// </summary>
// //         [Fact]
// //         public void ReadFromXml_InvalidFilePath_ReturnsBuildWithErrors()
// //         {
// //             // Arrange
// //             string invalidFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xml");
// //             // Act
// //             Build result = XlinqLogReader.ReadFromXml(invalidFilePath, null);
// //             // Assert
// //             result.Should().NotBeNull("even if an exception occurs, a Build object should be returned");
// //             result.Succeeded.Should().BeFalse("the returned Build should indicate failure when an exception occurs");
// //             // Since errors are added as children to the build node, verify that two errors were added.
// //             // Build is expected to be a TreeNode type (or derived from it) able to hold children.
// //             var treeNode = result as TreeNode;
// //             treeNode.Should().NotBeNull("Build is expected to be a TreeNode type to hold child error nodes");
// //             treeNode.Children.Should().HaveCount(2, "two errors should be added in case of a file loading exception");
// //         }
// //     }
// }