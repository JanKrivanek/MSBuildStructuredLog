// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.IO;
// using System.Xml;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="XmlLogWriter"/> class.
//     /// </summary>
//     public class XmlLogWriterTests
//     {
// //         // We assume that the Build class (which inherits from TreeNode) is available in the production assembly. // [Error] (28-17)CS0200 Property or indexer 'Build.Children' cannot be assigned to -- it is read only
// //         // For the purposes of these tests, we create minimal instances with only the properties used by XmlLogWriter.
// //         // Note: In a real test environment, the full object model would be used.
// //         
// //         /// <summary>
// //         /// A minimal stub for the Build class to facilitate testing.
// //         /// In production, the actual Build implementation is used.
// //         /// </summary>
// //         private class DummyBuild : Build
// //         {
// //             public DummyBuild()
// //             {
// //                 // Ensure there are no children.
// //                 this.Children = Array.Empty<BaseNode>();
// //             }
// //         }
// // 
//         /// <summary>
//         /// A minimal stub for the TreeNode aspects needed for testing.
//         /// </summary>
//         private abstract class DummyTreeNode : TreeNode
//         {
// //             // Expose Children for test purposes. // [Error] (38-40)CS0506 'XmlLogWriterTests.DummyTreeNode.Children': cannot override inherited member 'TreeNode.Children' because it is not marked virtual, abstract, or override
// //             public override BaseNode[] Children { get; set; } = Array.Empty<BaseNode>();
// //  // [Error] (40-34)CS0506 'XmlLogWriterTests.DummyTreeNode.HasChildren': cannot override inherited member 'TreeNode.HasChildren' because it is not marked virtual, abstract, or override
//             public override bool HasChildren => Children != null && Children.Length > 0;
//         }
// //  // [Error] (54-17)CS0117 'XmlLogWriterTests.DummyBuild' does not contain a definition for 'Succeeded'
// //         /// <summary>
// //         /// Creates a dummy Build instance with preset properties.
// //         /// </summary>
// //         /// <param name="succeeded">Indicates whether the build succeeded.</param>
// //         /// <param name="isAnalyzed">Indicates whether the build is analyzed.</param>
// //         /// <returns>A dummy Build instance.</returns>
// //         private static Build CreateDummyBuild(bool succeeded, bool isAnalyzed)
// //         {
// //             // Assuming that Build inherits from TreeNode and has Succeeded and IsAnalyzed properties.
// //             var dummyBuild = new DummyBuild
// //             {
// //                 Succeeded = succeeded,
// //                 IsAnalyzed = isAnalyzed
// //             };
// //             return dummyBuild;
// //         }
// //  // [Error] (74-41)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         /// <summary>
//         /// Tests the static WriteToXml method when provided with a valid Build instance and file path.
//         /// Expects that a well-formed XML file is created with the appropriate attributes.
//         /// </summary>
//         [Fact]
//         public void WriteToXml_ValidBuild_CreatesWellFormedXml()
//         {
//             // Arrange
//             Build build = CreateDummyBuild(true, true);
//             string tempFile = Path.GetTempFileName();
// 
//             try
//             {
//                 // Act
//                 XmlLogWriter.WriteToXml(build, tempFile);
// 
//                 // Assert
//                 File.Exists(tempFile).Should().BeTrue("The log file should be created by WriteToXml.");
// 
//                 string xmlContent = File.ReadAllText(tempFile);
//                 xmlContent.Should().NotBeNullOrEmpty("The XML content should not be empty.");
// 
//                 // Check for XML declaration.
//                 xmlContent.Should().Contain("<?xml", "the output should contain an XML declaration.");
// 
//                 // We assume that the Serialization.GetNodeName returns the type name "Build".
//                 // Also, the WriteAttributes for Build appends attributes for Succeeded and IsAnalyzed.
//                 xmlContent.Should().Contain("<Build", "the root element should be 'Build'.");
//                 xmlContent.Should().Contain("Succeeded=\"True\"", "the Succeeded attribute should be set to 'True'.");
//                 xmlContent.Should().Contain("IsAnalyzed=\"True\"", "the IsAnalyzed attribute should be set to 'True'.");
//             }
//             finally
//             {
//                 // Cleanup
//                 if (File.Exists(tempFile))
//                 {
//                     File.Delete(tempFile);
//                 }
//             }
//         }
// //  // [Error] (116-36)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         /// <summary>
// //         /// Tests the instance Write method when provided with a valid Build instance and file path.
// //         /// Expects that a well-formed XML file is created with the appropriate attributes.
// //         /// </summary>
// //         [Fact]
// //         public void Write_ValidBuild_CreatesWellFormedXml()
// //         {
// //             // Arrange
// //             Build build = CreateDummyBuild(false, false);
// //             string tempFile = Path.GetTempFileName();
// //             var xmlLogWriter = new XmlLogWriter();
// // 
// //             try
// //             {
// //                 // Act
// //                 xmlLogWriter.Write(build, tempFile);
// // 
// //                 // Assert
// //                 File.Exists(tempFile).Should().BeTrue("The log file should be created by Write method.");
// // 
// //                 string xmlContent = File.ReadAllText(tempFile);
// //                 xmlContent.Should().NotBeNullOrEmpty("The XML content should not be empty.");
// // 
// //                 // Check for XML declaration.
// //                 xmlContent.Should().Contain("<?xml", "the output should contain an XML declaration.");
// // 
// //                 // We assume that the Serialization.GetNodeName returns the type name "Build".
// //                 // Also, the WriteAttributes for Build appends attributes for Succeeded and IsAnalyzed.
// //                 xmlContent.Should().Contain("<Build", "the root element should be 'Build'.");
// //                 xmlContent.Should().Contain("Succeeded=\"False\"", "the Succeeded attribute should be set to 'False'.");
// //                 xmlContent.Should().Contain("IsAnalyzed=\"False\"", "the IsAnalyzed attribute should be set to 'False'.");
// //             }
// //             finally
// //             {
// //                 // Cleanup
// //                 if (File.Exists(tempFile))
// //                 {
// //                     File.Delete(tempFile);
// //                 }
// //             }
// //         }
// //  // [Error] (156-51)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         /// <summary>
//         /// Tests that the Write method throws an exception when provided with an invalid file path.
//         /// Expects an ArgumentException due to an empty file path.
//         /// </summary>
//         [Fact]
//         public void Write_InvalidFilePath_ThrowsException()
//         {
//             // Arrange
//             Build build = CreateDummyBuild(true, true);
//             string invalidFilePath = string.Empty; // Using an empty string as an invalid file path.
//             var xmlLogWriter = new XmlLogWriter();
// 
//             // Act
//             Action act = () => xmlLogWriter.Write(build, invalidFilePath);
// 
//             // Assert
//             act.Should().Throw<ArgumentException>("an empty file path should trigger an ArgumentException when opening the file stream");
//         }
//     }
// }