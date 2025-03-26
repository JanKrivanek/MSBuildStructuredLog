using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections;
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
        /// Tests that when the XML file does not exist, the ReadFromXml method returns a Build with error information.
        /// </summary>
        [Fact]
        public void ReadFromXml_FileNotFound_ReturnsBuildWithErrors()
        {
            // Arrange
            string nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xml");
            var statusUpdates = new List<string>();
            Action<string> statusUpdate = msg => statusUpdates.Add(msg);

            // Act
            var build = XlinqLogReader.ReadFromXml(nonExistentFile, statusUpdate);

            // Assert
            // Check that the Succeeded property is false.
            var succeededProperty = build.GetType().GetProperty("Succeeded");
            Assert.NotNull(succeededProperty);
            bool succeeded = (bool)succeededProperty.GetValue(build);
            Assert.False(succeeded);

            // Check that two errors have been added.
            var childrenProperty = build.GetType().GetProperty("Children");
            Assert.NotNull(childrenProperty);
            var children = childrenProperty.GetValue(build) as IEnumerable;
            int errorCount = 0;
            var errorTexts = new List<string>();
            foreach (var child in children)
            {
                errorCount++;
                var textProp = child.GetType().GetProperty("Text");
                if (textProp != null)
                {
                    errorTexts.Add(textProp.GetValue(child) as string);
                }
            }
            Assert.Equal(2, errorCount);
            Assert.Contains(errorTexts, error => error != null && error.Contains("Error when opening file:"));
        }

        /// <summary>
        /// Tests that when the XML file contains invalid XML content, the ReadFromXml method returns a Build with error information.
        /// </summary>
        [Fact]
        public void ReadFromXml_InvalidXml_ReturnsBuildWithErrors()
        {
            // Arrange
            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xml");
            File.WriteAllText(tempFile, "This is not valid XML content.");
            var statusUpdates = new List<string>();
            Action<string> statusUpdate = msg => statusUpdates.Add(msg);

            try
            {
                // Act
                var build = XlinqLogReader.ReadFromXml(tempFile, statusUpdate);

                // Assert
                var succeededProperty = build.GetType().GetProperty("Succeeded");
                Assert.NotNull(succeededProperty);
                bool succeeded = (bool)succeededProperty.GetValue(build);
                Assert.False(succeeded);

                var childrenProperty = build.GetType().GetProperty("Children");
                Assert.NotNull(childrenProperty);
                var children = childrenProperty.GetValue(build) as IEnumerable;
                int errorCount = 0;
                var errorTexts = new List<string>();
                foreach (var child in children)
                {
                    errorCount++;
                    var textProp = child.GetType().GetProperty("Text");
                    if (textProp != null)
                    {
                        errorTexts.Add(textProp.GetValue(child) as string);
                    }
                }
                Assert.Equal(2, errorCount);
                Assert.Contains(errorTexts, error => error != null && error.Contains("Error when opening file:"));
            }
            finally
            {
                // Cleanup
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
    }
}
