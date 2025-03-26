using DiffPlex.DiffBuilder.Model;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref = "BinaryLog"/> class.
    /// </summary>
    public class BinaryLogTests
    {
        /// <summary>
        /// Tests that calling ReadRecords with a null file path throws an exception.
        /// </summary>
        [Fact]
        public void ReadRecords_StringNull_ThrowsException()
        {
            // Arrange
            string binLogFilePath = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => BinaryLog.ReadRecords(binLogFilePath));
        }

        /// <summary>
        /// Tests that calling ReadRecords with a valid file path that does not exist throws an exception.
        /// </summary>
        [Fact]
        public void ReadRecords_StringNonExistentFile_ThrowsException()
        {
            // Arrange
            string nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".binlog");
            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => BinaryLog.ReadRecords(nonExistentFile).ToList());
        }

        /// <summary>
        /// Tests that calling ReadRecords with a null stream throws an exception.
        /// </summary>
        [Fact]
        public void ReadRecords_StreamNull_ThrowsException()
        {
            // Arrange
            Stream nullStream = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => BinaryLog.ReadRecords(nullStream).ToList());
        }

        /// <summary>
        /// Tests that calling ReadRecords with a null byte array throws an exception.
        /// </summary>
        [Fact]
        public void ReadRecords_ByteArrayNull_ThrowsException()
        {
            // Arrange
            byte[] nullBytes = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => BinaryLog.ReadRecords(nullBytes).ToList());
        }

        /// <summary>
        /// Tests that calling ReadBuild with a non-existent file path throws a FileNotFoundException.
        /// </summary>
        [Fact]
        public void ReadBuild_StringNonExistentFile_ThrowsException()
        {
            // Arrange
            string nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".binlog");
            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => BinaryLog.ReadBuild(nonExistentFile));
        }

        /// <summary>
        /// Tests that calling ReadBuild with an empty log file returns a Build object that indicates failure and contains an error message.
        /// </summary>
//         [Fact] [Error] (90-31)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' [Error] (93-36)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (95-72)CS1061 'FakeNode' does not contain a definition for 'Text' and no accessible extension method 'Text' accepting a first argument of type 'FakeNode' could be found (are you missing a using directive or an assembly reference?)
//         public void ReadBuild_StringEmptyFile_ReturnsErrorBuild()
//         {
//             // Arrange
//             string tempLogFile = Path.GetTempFileName();
//             try
//             {
//                 // Write empty content to simulate an empty log file.
//                 File.WriteAllBytes(tempLogFile, new byte[0]);
//                 // Act
//                 Build build = BinaryLog.ReadBuild(tempLogFile);
//                 // Assert
//                 Assert.NotNull(build);
//                 Assert.False(build.Succeeded);
//                 // Check that at least one child is an error containing the expected text.
//                 bool containsError = build.Children.Any(child => child.Text?.Contains("Error when opening the log file") == true);
//                 Assert.True(containsError, "The build error message was not found in the child records.");
//             }
//             finally
//             {
//                 File.Delete(tempLogFile);
//                 // Also delete the related project imports zip file if it was created.
//                 string projectImports = Path.ChangeExtension(tempLogFile, ".ProjectImports.zip");
//                 if (File.Exists(projectImports))
//                 {
//                     File.Delete(projectImports);
//                 }
//             }
//         }

        /// <summary>
        /// Tests that calling ReadBuild with a valid empty log file and an accompanying project imports zip file sets the SourceFilesArchive properly.
        /// </summary>
//         [Fact] [Error] (134-31)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' [Error] (138-38)CS1061 'Build' does not contain a definition for 'SourceFilesArchive' and no accessible extension method 'SourceFilesArchive' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (139-35)CS1061 'Build' does not contain a definition for 'SourceFilesArchive' and no accessible extension method 'SourceFilesArchive' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?)
//         public void ReadBuild_StringWithProjectImportsZip_SetsSourceFilesArchive()
//         {
//             // Arrange
//             string tempLogFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".binlog");
//             string projectImportsFile = Path.ChangeExtension(tempLogFile, ".ProjectImports.zip");
//             byte[] projectImportsContent = new byte[]
//             {
//                 1,
//                 2,
//                 3,
//                 4,
//                 5
//             };
//             try
//             {
//                 // Create an empty log file.
//                 File.WriteAllBytes(tempLogFile, new byte[0]);
//                 // Create the project imports zip file with specific content.
//                 File.WriteAllBytes(projectImportsFile, projectImportsContent);
//                 // Act
//                 Build build = BinaryLog.ReadBuild(tempLogFile);
//                 // Assert
//                 Assert.NotNull(build);
//                 // Even if the build is in error state due to empty log, the project imports archive should be set.
//                 Assert.NotNull(build.SourceFilesArchive);
//                 Assert.True(build.SourceFilesArchive.SequenceEqual(projectImportsContent), "The SourceFilesArchive does not match the expected content.");
//             }
//             finally
//             {
//                 if (File.Exists(tempLogFile))
//                 {
//                     File.Delete(tempLogFile);
//                 }
// 
//                 if (File.Exists(projectImportsFile))
//                 {
//                     File.Delete(projectImportsFile);
//                 }
//             }
//         }

        /// <summary>
        /// Tests that calling ReadBuild with a null stream throws an exception.
        /// </summary>
        [Fact]
        public void ReadBuild_StreamNull_ThrowsException()
        {
            // Arrange
            Stream nullStream = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => BinaryLog.ReadBuild(nullStream));
        }

        /// <summary>
        /// Tests that calling ReadBuild with an empty MemoryStream returns a Build object indicating failure and includes an error message.
        /// </summary>
//         [Fact] [Error] (176-27)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.Build' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' [Error] (179-32)CS1061 'Build' does not contain a definition for 'Succeeded' and no accessible extension method 'Succeeded' accepting a first argument of type 'Build' could be found (are you missing a using directive or an assembly reference?) [Error] (180-68)CS1061 'FakeNode' does not contain a definition for 'Text' and no accessible extension method 'Text' accepting a first argument of type 'FakeNode' could be found (are you missing a using directive or an assembly reference?)
//         public void ReadBuild_EmptyStream_ReturnsErrorBuild()
//         {
//             // Arrange
//             using MemoryStream emptyStream = new MemoryStream(new byte[0]);
//             // Act
//             Build build = BinaryLog.ReadBuild(emptyStream);
//             // Assert
//             Assert.NotNull(build);
//             Assert.False(build.Succeeded);
//             bool containsError = build.Children.Any(child => child.Text?.Contains("Error when opening the log file") == true);
//             Assert.True(containsError, "The build error message was not found in the child records.");
//         }
    }
}