using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref = "XmlLogReader"/> class.
    /// </summary>
    public class XmlLogReaderTests
    {
        /// <summary>
        /// Tests the static method ReadFromXml(string) with a valid XML file.
        /// The test creates a temporary file with minimal valid XML, reads it and asserts that the returned build has its LogFilePath 
        /// property set to the file path.
        /// </summary>
//         [Fact] [Error] (31-32)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' [Error] (34-51)CS1061 'Build' does not contain a definition for 'LogFilePath' and no accessible extension method 'LogFilePath' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
//         public void ReadFromXml_FilePath_ValidXml_ReturnsBuildWithLogFilePathSet()
//         {
//             // Arrange
//             string tempFilePath = Path.GetTempFileName();
//             try
//             {
//                 // Write minimal valid XML. The root element name is "Build".
//                 File.WriteAllText(tempFilePath, "<Build/>", Encoding.UTF8);
//                 // Act
//                 Build result = XmlLogReader.ReadFromXml(tempFilePath);
//                 // Assert
//                 Assert.NotNull(result);
//                 Assert.Equal(tempFilePath, result.LogFilePath);
//             }
//             finally
//             {
//                 if (File.Exists(tempFilePath))
//                 {
//                     File.Delete(tempFilePath);
//                 }
//             }
//         }

        /// <summary>
        /// Tests the static method ReadFromXml(string) when the file does not exist.
        /// Expectation: A FileNotFoundException is thrown when attempting to open a non-existent file.
        /// </summary>
        [Fact]
        public void ReadFromXml_FilePath_FileDoesNotExist_ThrowsFileNotFoundException()
        {
            // Arrange
            string nonExistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xml");
            // Act & Assert
            Assert.Throws<FileNotFoundException>(() =>
            {
                // This should throw because the file does not exist.
                XmlLogReader reader = new XmlLogReader();
                reader.Read(nonExistentFilePath);
            });
        }

        /// <summary>
        /// Tests the static method ReadFromXml(Stream) with a valid XML stream.
        /// The test passes a memory stream containing minimal valid XML and asserts that the returned build is not null 
        /// and that LogFilePath remains unset.
        /// </summary>
//         [Fact] [Error] (76-32)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' [Error] (80-36)CS1061 'Build' does not contain a definition for 'LogFilePath' and no accessible extension method 'LogFilePath' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
//         public void ReadFromXml_Stream_ValidXml_ReturnsBuild()
//         {
//             // Arrange
//             string xmlContent = "<Build/>";
//             using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent)))
//             {
//                 // Act
//                 Build result = XmlLogReader.ReadFromXml(stream);
//                 // Assert
//                 Assert.NotNull(result);
//                 // For stream based reading, LogFilePath is not set.
//                 Assert.Null(result.LogFilePath);
//             }
//         }

        /// <summary>
        /// Tests the static method ReadFromXml(Stream) with invalid XML content.
        /// The test passes a memory stream with invalid XML and asserts that the returned build indicates failure
        /// and contains error messages.
        /// </summary>
//         [Fact] [Error] (98-32)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' [Error] (102-37)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (106-106)CS8121 An expression of type 'FakeNode' cannot be handled by a pattern of type 'Error'.
//         public void ReadFromXml_Stream_InvalidXml_ReturnsBuildWithErrors()
//         {
//             // Arrange
//             // Provide content that is not valid XML.
//             string invalidXmlContent = "This is not valid XML";
//             using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(invalidXmlContent)))
//             {
//                 // Act
//                 Build result = XmlLogReader.ReadFromXml(stream);
//                 // Assert
//                 Assert.NotNull(result);
//                 // Since an exception is caught, Succeeded should be false.
//                 Assert.False(result.Succeeded);
//                 // Expecting error nodes to have been added.
//                 // Assuming Build has a Children property that holds added error nodes.
//                 // Verify that an error message indicating failure to open XML log file is present.
//                 bool hasExpectedError = result.Children != null && result.Children.Any(child => child is Error error && error.Text == "Error when opening XML log file.");
//                 Assert.True(hasExpectedError, "Expected error message was not found in the build errors.");
//             }
//         }

        /// <summary>
        /// Tests the static method ReadFromXml(Stream) to ensure that it wraps the instance Read(Stream) correctly.
        /// Uses a valid XML stream input to obtain a build and asserts that the build is returned successfully.
        /// </summary>
//         [Fact] [Error] (123-42)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' [Error] (128-44)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' [Error] (133-49)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (133-77)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
//         public void ReadFromXml_Stream_StaticMethod_ReturnsSameAsInstanceMethod()
//         {
//             // Arrange
//             string xmlContent = "<Build/>";
//             using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent)))
//             {
//                 // Act
//                 Build resultFromStatic = XmlLogReader.ReadFromXml(stream);
//                 // Directly call instance method
//                 XmlLogReader reader = new XmlLogReader();
//                 // Reset stream position for second read.
//                 stream.Position = 0;
//                 Build resultFromInstance = reader.Read(stream);
//                 // Assert
//                 Assert.NotNull(resultFromStatic);
//                 Assert.NotNull(resultFromInstance);
//                 // In a happy path with minimal XML, both should result in builds with default or similar properties.
//                 Assert.Equal(resultFromInstance.Succeeded, resultFromStatic.Succeeded);
//             }
//         }
    }
}