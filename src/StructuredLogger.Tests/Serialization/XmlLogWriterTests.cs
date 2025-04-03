// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.IO;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="XmlLogWriter"/> class.
//     /// </summary>
//     public class XmlLogWriterTests
//     {
// //         /// <summary> // [Error] (28-41)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         /// Tests the <see cref="XmlLogWriter.WriteToXml(Build, string)"/> method to ensure it creates a file with XML content when provided with a valid Build instance and log file path.
// //         /// </summary>
// //         [Fact]
// //         public void WriteToXml_ValidBuildAndLogFile_CreatesXmlFileWithXmlContent()
// //         {
// //             // Arrange
// //             // TODO: Replace the following dummy Build instance with a proper initialized Build object if needed.
// //             Build build = new Build();
// //             string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xml");
// // 
// //             try
// //             {
// //                 // Act
// //                 XmlLogWriter.WriteToXml(build, tempFile);
// // 
// //                 // Assert
// //                 File.Exists(tempFile).Should().BeTrue("because the log file should be created when writing XML");
// //                 string fileContent = File.ReadAllText(tempFile);
// //                 fileContent.Should().NotBeNullOrWhiteSpace("because the file should contain XML content");
// //                 fileContent.Should().StartWith("<?xml", "because an XML document should start with an XML declaration");
// //             }
// //             finally
// //             {
// //                 if (File.Exists(tempFile))
// //                 {
// //                     File.Delete(tempFile);
// //                 }
// //             }
// //         }
// //  // [Error] (60-30)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         /// <summary>
//         /// Tests the <see cref="XmlLogWriter.Write(Build, string)"/> method to ensure it creates a file with XML content when provided with a valid Build instance and log file path.
//         /// </summary>
//         [Fact]
//         public void Write_ValidBuildAndLogFile_CreatesXmlFileWithXmlContent()
//         {
//             // Arrange
//             // TODO: Replace the following dummy Build instance with a proper initialized Build object if needed.
//             Build build = new Build();
//             string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xml");
//             XmlLogWriter writer = new XmlLogWriter();
// 
//             try
//             {
//                 // Act
//                 writer.Write(build, tempFile);
// 
//                 // Assert
//                 File.Exists(tempFile).Should().BeTrue("because the log file should be created when writing XML");
//                 string fileContent = File.ReadAllText(tempFile);
//                 fileContent.Should().NotBeNullOrWhiteSpace("because the file should contain XML content");
//                 fileContent.Should().StartWith("<?xml", "because an XML document should start with an XML declaration");
//             }
//             finally
//             {
//                 if (File.Exists(tempFile))
//                 {
//                     File.Delete(tempFile);
//                 }
//             }
//         }
// //  // [Error] (88-56)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         /// <summary>
// //         /// Tests the <see cref="XmlLogWriter.WriteToXml(Build, string)"/> method to ensure it throws an exception when provided with an invalid (empty) log file path.
// //         /// </summary>
// //         [Fact]
// //         public void WriteToXml_InvalidLogFilePath_ThrowsException()
// //         {
// //             // Arrange
// //             Build build = new Build();
// //             string invalidLogFile = string.Empty;
// // 
// //             // Act
// //             Action act = () => XmlLogWriter.WriteToXml(build, invalidLogFile);
// // 
// //             // Assert
// //             act.Should().Throw<ArgumentException>("because an empty string is not a valid file path");
// //         }
// //  // [Error] (106-45)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         /// <summary>
//         /// Tests the <see cref="XmlLogWriter.Write(Build, string)"/> method to ensure it throws an exception when provided with an invalid (empty) log file path.
//         /// </summary>
//         [Fact]
//         public void Write_InvalidLogFilePath_ThrowsException()
//         {
//             // Arrange
//             Build build = new Build();
//             string invalidLogFile = string.Empty;
//             XmlLogWriter writer = new XmlLogWriter();
// 
//             // Act
//             Action act = () => writer.Write(build, invalidLogFile);
// 
//             // Assert
//             act.Should().Throw<ArgumentException>("because an empty string is not a valid file path");
//         }
// //  // [Error] (123-56)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         /// <summary>
// //         /// Tests the <see cref="XmlLogWriter.WriteToXml(Build, string)"/> method to ensure it throws a NullReferenceException when provided with a null Build instance.
// //         /// </summary>
// //         [Fact]
// //         public void WriteToXml_NullBuild_ThrowsNullReferenceException()
// //         {
// //             // Arrange
// //             Build nullBuild = null!;
// //             string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xml");
// // 
// //             // Act
// //             Action act = () => XmlLogWriter.WriteToXml(nullBuild, tempFile);
// // 
// //             // Assert
// //             act.Should().Throw<NullReferenceException>("because a null Build object should lead to a null reference during XML node writing");
// //         }
// //  // [Error] (141-45)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         /// <summary>
//         /// Tests the <see cref="XmlLogWriter.Write(Build, string)"/> method to ensure it throws a NullReferenceException when provided with a null Build instance.
//         /// </summary>
//         [Fact]
//         public void Write_NullBuild_ThrowsNullReferenceException()
//         {
//             // Arrange
//             Build nullBuild = null!;
//             string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xml");
//             XmlLogWriter writer = new XmlLogWriter();
// 
//             // Act
//             Action act = () => writer.Write(nullBuild, tempFile);
// 
//             // Assert
//             act.Should().Throw<NullReferenceException>("because a null Build object should lead to a null reference during XML node writing");
//         }
//     }
// }