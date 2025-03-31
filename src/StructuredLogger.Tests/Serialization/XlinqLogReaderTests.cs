using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="XlinqLogReader"/> class.
    /// </summary>
    public class XlinqLogReaderTests
    {
        /// <summary>
        /// Tests that ReadFromXml returns a valid Build object when provided with a valid XML file and that status update is called.
        /// </summary>
        [Fact]
        public void ReadFromXml_ValidXml_FileExists_StatusUpdatesCalledAndReturnsBuildWithSucceededTrue()
        {
            // Arrange
            // Create a temporary XML file with a valid <Build> element.
            // Note: This test assumes that Serialization.CreateNode("Build") returns a Build object
            // that can be interpreted to have its Succeeded property set via the "Succeeded" attribute.
            string tempFilePath = Path.GetTempFileName();
            try
            {
                // Write minimal valid XML.
                // We supply Succeeded="true" and IsAnalyzed="false" to see if these values are processed.
                // The actual processing of attributes is delegated to Serialization.GetBoolean.
                // We assume that "true" will be parsed to true.
                var xmlContent = "<Build Succeeded=\"true\" IsAnalyzed=\"false\"></Build>";
                File.WriteAllText(tempFilePath, xmlContent);

                var statusMessages = new List<string>();
                void StatusUpdate(string message)
                {
                    statusMessages.Add(message);
                }

                // Act
                var build = XlinqLogReader.ReadFromXml(tempFilePath, StatusUpdate);

                // Assert
                build.Should().NotBeNull("because a valid XML file should result in a valid Build object");
                // Since valid XML was provided, Succeeded should be true.
                // In case the processing uses the attribute "true", we expect a true Boolean.
                // (Assuming Serialization.GetBoolean works as expected.)
                build.Succeeded.Should().BeTrue("because the Succeeded attribute was 'true' in the XML input");
                statusMessages.Should().ContainMatch("*Loading*")
                    .And.ContainMatch("*Populating tree*");
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
        /// Tests that ReadFromXml returns a Build object with error information when the XML file does not exist.
        /// </summary>
        [Fact]
        public void ReadFromXml_FileNotFound_ReturnsBuildWithErrorAndSucceededFalse()
        {
            // Arrange
            string nonExistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xml");
            var statusMessages = new List<string>();
            void StatusUpdate(string message)
            {
                statusMessages.Add(message);
            }

            // Act
            var build = XlinqLogReader.ReadFromXml(nonExistentFilePath, StatusUpdate);

            // Assert
            build.Should().NotBeNull("because even on exception a Build object is returned");
            build.Succeeded.Should().BeFalse("because a missing file should cause the build to fail");
            // Expect that the status update for 'Loading' was attempted.
            statusMessages.Should().Contain(message => message.Contains("Loading"));
        }

        /// <summary>
        /// Tests that ReadFromXml returns a Build object with error information when the XML file has invalid content.
        /// </summary>
        [Fact]
        public void ReadFromXml_InvalidXml_ReturnsBuildWithErrorAndSucceededFalse()
        {
            // Arrange
            string tempFilePath = Path.GetTempFileName();
            try
            {
                // Write invalid XML content.
                var invalidXml = "<Build Succeeded=\"true\" IsAnalyzed=\"false\">"; // Missing closing tag.
                File.WriteAllText(tempFilePath, invalidXml);

                var statusMessages = new List<string>();
                void StatusUpdate(string message)
                {
                    statusMessages.Add(message);
                }

                // Act
                var build = XlinqLogReader.ReadFromXml(tempFilePath, StatusUpdate);

                // Assert
                build.Should().NotBeNull("because even if an exception occurs, a Build object is returned");
                build.Succeeded.Should().BeFalse("because invalid XML should result in a failed build state");
                statusMessages.Should().Contain(message => message.Contains("Loading"))
                    .And.Contain(message => message.Contains("Populating tree"));
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }
    }
}
