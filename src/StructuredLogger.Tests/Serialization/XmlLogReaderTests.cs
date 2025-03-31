using System;
using System.IO;
using System.Text;
using System.Xml;
using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="XmlLogReader"/> class.
    /// </summary>
    public class XmlLogReaderTests
    {
        /// <summary>
        /// Tests that the Read(Stream) method returns a valid Build object when provided with a minimal valid XML.
        /// </summary>
        [Fact]
        public void Read_StreamWithValidXml_ReturnsBuild()
        {
            // Arrange
            // Minimal valid XML that contains the Build element with some attributes.
            string xmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Build Succeeded=\"true\" IsAnalyzed=\"true\"></Build>";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent));
            var reader = new XmlLogReader();

            // Act
            var build = reader.Read(stream);

            // Assert
            build.Should().NotBeNull("because a valid XML should produce a Build instance");
            // LogFilePath is not set when reading from a stream.
            // In this minimal XML, the attributes might be parsed to false if conversion fails,
            // so we only assert that no exception occurred and a Build object is returned.
        }

        /// <summary>
        /// Tests that the Read(Stream) method catches exceptions when provided with an invalid XML and returns a Build with errors.
        /// </summary>
        [Fact]
        public void Read_StreamWithInvalidXml_ReturnsBuildWithErrors()
        {
            // Arrange
            string invalidXml = "This is not valid XML.";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(invalidXml));
            var reader = new XmlLogReader();

            // Act
            var build = reader.Read(stream);

            // Assert
            // According to the implementation, if an exception occurs, build.Succeeded is set to false and two Errors are added.
            build.Should().NotBeNull("because even with errors a Build instance is returned");
            build.Succeeded.Should().BeFalse("because an exception occurred during XML processing");
            // Assuming Build has a Children collection for errors added via AddChild.
            // We assume at least 2 errors are present.
            build.Children.Should().HaveCountGreaterOrEqualTo(2, "because two error messages should be added upon exception");
        }

        /// <summary>
        /// Tests that the static ReadFromXml(Stream) method returns a valid Build object when provided with a valid XML stream.
        /// </summary>
        [Fact]
        public void ReadFromXml_StreamWithValidXml_ReturnsBuild()
        {
            // Arrange
            string xmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Build Succeeded=\"true\" IsAnalyzed=\"true\"></Build>";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent));

            // Act
            var build = XmlLogReader.ReadFromXml(stream);

            // Assert
            build.Should().NotBeNull("a valid XML stream should produce a Build instance");
        }

        /// <summary>
        /// Tests that the instance Read(string) method correctly reads from a file and assigns the LogFilePath.
        /// </summary>
        [Fact]
        public void Read_FileWithValidXml_ReturnsBuildWithLogFilePathSet()
        {
            // Arrange
            string xmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Build Succeeded=\"true\" IsAnalyzed=\"true\"></Build>";
            string tempFilePath = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFilePath, xmlContent, Encoding.UTF8);
                var reader = new XmlLogReader();

                // Act
                var build = reader.Read(tempFilePath);

                // Assert
                build.Should().NotBeNull("because a valid XML file should produce a Build instance");
                build.LogFilePath.Should().Be(tempFilePath, "because the LogFilePath should be set to the source file path");
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }

        /// <summary>
        /// Tests that the static ReadFromXml(string) method reads from a file and returns a Build with the LogFilePath property set.
        /// </summary>
        [Fact]
        public void ReadFromXml_StringFilePathWithValidXml_ReturnsBuildWithLogFilePathSet()
        {
            // Arrange
            string xmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Build Succeeded=\"true\" IsAnalyzed=\"true\"></Build>";
            string tempFilePath = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFilePath, xmlContent, Encoding.UTF8);

                // Act
                var build = XmlLogReader.ReadFromXml(tempFilePath);

                // Assert
                build.Should().NotBeNull("because a valid XML file should produce a Build instance");
                build.LogFilePath.Should().Be(tempFilePath, "because the LogFilePath should be set to the source file path");
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }

        /// <summary>
        /// Tests that the Read(string) method throws a FileNotFoundException when provided with a non-existent file path.
        /// </summary>
        [Fact]
        public void Read_FilePathNonexistent_ThrowsFileNotFoundException()
        {
            // Arrange
            string nonexistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xml");
            var reader = new XmlLogReader();

            // Act
            Action act = () => reader.Read(nonexistentFilePath);

            // Assert
            act.Should().Throw<FileNotFoundException>("because the file does not exist");
        }
    }
}
