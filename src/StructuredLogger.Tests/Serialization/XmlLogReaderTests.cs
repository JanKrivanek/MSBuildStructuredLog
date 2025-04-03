// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.IO;
// using System.Text;
// using System.Xml;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "XmlLogReader"/> class.
//     /// </summary>
//     public class XmlLogReaderTests
//     {
//         /// <summary>
//         /// Creates a temporary file with the specified content.
//         /// </summary>
//         /// <param name = "content">The file content.</param>
//         /// <returns>The temporary file path.</returns>
//         private static string CreateTempFile(string content)
//         {
//             string tempPath = Path.GetTempFileName();
//             File.WriteAllText(tempPath, content, Encoding.UTF8);
//             return tempPath;
//         }
// 
//         /// <summary>
//         /// Deletes the temporary file if it exists.
//         /// </summary>
//         /// <param name = "filePath">The file path.</param>
//         private static void DeleteTempFile(string filePath)
//         {
//             if (File.Exists(filePath))
//             {
//                 File.Delete(filePath);
//             }
//         }
// //  // [Error] (54-31)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (57-23)CS1061 'Build' does not contain a definition for 'LogFilePath' and no accessible extension method 'LogFilePath' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests the <see cref = "XmlLogReader.ReadFromXml(string)"/> static method with a valid XML file.
// //         /// Expected outcome: Returns a Build instance with the LogFilePath property set.
// //         /// </summary>
// //         [Fact]
// //         public void ReadFromXml_String_ValidFile_ReturnsBuildWithLogFilePathSet()
// //         {
// //             // Arrange
// //             string xmlContent = "<Build></Build>";
// //             string tempFile = CreateTempFile(xmlContent);
// //             try
// //             {
// //                 // Act
// //                 Build build = XmlLogReader.ReadFromXml(tempFile);
// //                 // Assert
// //                 build.Should().NotBeNull("because a valid Build XML should return a Build instance");
// //                 build.LogFilePath.Should().Be(tempFile, "because the LogFilePath should be set to the input file path");
// //             }
// //             finally
// //             {
// //                 DeleteTempFile(tempFile);
// //             }
// //         }
// //  // [Error] (75-27)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (78-19)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the <see cref = "XmlLogReader.ReadFromXml(string)"/> static method with an invalid (non-existent) file.
//         /// Expected outcome: Returns a Build instance with Succeeded property false indicating an error.
//         /// </summary>
//         [Fact]
//         public void ReadFromXml_String_InvalidFile_ReturnsBuildWithFailure()
//         {
//             // Arrange
//             string invalidFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xml");
//             // Act
//             Build build = XmlLogReader.ReadFromXml(invalidFilePath);
//             // Assert
//             build.Should().NotBeNull("because even on error a Build instance is returned");
//             build.Succeeded.Should().BeFalse("because an exception during file reading should set Succeeded to false");
//         }
// //  // [Error] (92-27)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build'
// //         /// <summary>
// //         /// Tests the <see cref = "XmlLogReader.ReadFromXml(System.IO.Stream)"/> static method with a valid XML stream.
// //         /// Expected outcome: Returns a Build instance.
// //         /// </summary>
// //         [Fact]
// //         public void ReadFromXml_Stream_ValidStream_ReturnsBuild()
// //         {
// //             // Arrange
// //             string xmlContent = "<Build></Build>";
// //             using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent));
// //             // Act
// //             Build build = XmlLogReader.ReadFromXml(stream);
// //             // Assert
// //             build.Should().NotBeNull("because a valid XML stream should return a Build instance");
// //         }
// //  // [Error] (108-27)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (111-19)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the <see cref = "XmlLogReader.ReadFromXml(System.IO.Stream)"/> static method with an invalid XML stream.
//         /// Expected outcome: Returns a Build instance with Succeeded property false indicating an error.
//         /// </summary>
//         [Fact]
//         public void ReadFromXml_Stream_InvalidXml_ReturnsBuildWithFailure()
//         {
//             // Arrange
//             string invalidXml = "<Build>"; // Incomplete XML (missing closing tag)
//             using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(invalidXml));
//             // Act
//             Build build = XmlLogReader.ReadFromXml(stream);
//             // Assert
//             build.Should().NotBeNull("because even invalid XML should return a Build instance");
//             build.Succeeded.Should().BeFalse("because an XML parsing error should result in Succeeded being false");
//         }
// //  // [Error] (128-31)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (131-23)CS1061 'Build' does not contain a definition for 'LogFilePath' and no accessible extension method 'LogFilePath' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests the instance method <see cref = "XmlLogReader.Read(string)"/> with a valid XML file.
// //         /// Expected outcome: Returns a Build instance with LogFilePath property set.
// //         /// </summary>
// //         [Fact]
// //         public void Read_String_ValidFile_ReturnsBuildWithLogFilePathSet()
// //         {
// //             // Arrange
// //             string xmlContent = "<Build></Build>";
// //             string tempFile = CreateTempFile(xmlContent);
// //             XmlLogReader reader = new XmlLogReader();
// //             try
// //             {
// //                 // Act
// //                 Build build = reader.Read(tempFile);
// //                 // Assert
// //                 build.Should().NotBeNull("because a valid Build XML should return a Build instance");
// //                 build.LogFilePath.Should().Be(tempFile, "because the LogFilePath should be set to the input file path");
// //             }
// //             finally
// //             {
// //                 DeleteTempFile(tempFile);
// //             }
// //         }
// //  // [Error] (150-27)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (153-19)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the instance method <see cref = "XmlLogReader.Read(string)"/> with an invalid (non-existent) file.
//         /// Expected outcome: Returns a Build instance with Succeeded property false indicating an error.
//         /// </summary>
//         [Fact]
//         public void Read_String_InvalidFile_ReturnsBuildWithFailure()
//         {
//             // Arrange
//             string invalidFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xml");
//             XmlLogReader reader = new XmlLogReader();
//             // Act
//             Build build = reader.Read(invalidFilePath);
//             // Assert
//             build.Should().NotBeNull("because even on error a Build instance is returned");
//             build.Succeeded.Should().BeFalse("because an exception during file reading should set Succeeded to false");
//         }
// //  // [Error] (168-27)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build'
// //         /// <summary>
// //         /// Tests the instance method <see cref = "XmlLogReader.Read(System.IO.Stream)"/> with a valid XML stream.
// //         /// Expected outcome: Returns a Build instance.
// //         /// </summary>
// //         [Fact]
// //         public void Read_Stream_ValidStream_ReturnsBuild()
// //         {
// //             // Arrange
// //             string xmlContent = "<Build></Build>";
// //             using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent));
// //             XmlLogReader reader = new XmlLogReader();
// //             // Act
// //             Build build = reader.Read(stream);
// //             // Assert
// //             build.Should().NotBeNull("because a valid XML stream should return a Build instance");
// //         }
// //  // [Error] (185-27)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' // [Error] (188-19)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests the instance method <see cref = "XmlLogReader.Read(System.IO.Stream)"/> with an invalid XML stream.
//         /// Expected outcome: Returns a Build instance with Succeeded property false indicating an error.
//         /// </summary>
//         [Fact]
//         public void Read_Stream_InvalidXml_ReturnsBuildWithFailure()
//         {
//             // Arrange
//             string invalidXml = "<Build>"; // Incomplete XML causes an XML parsing error.
//             using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(invalidXml));
//             XmlLogReader reader = new XmlLogReader();
//             // Act
//             Build build = reader.Read(stream);
//             // Assert
//             build.Should().NotBeNull("because even invalid XML should return a Build instance");
//             build.Succeeded.Should().BeFalse("because an XML parsing error should result in Succeeded being false");
//         }
//     }
// }