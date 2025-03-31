using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ErrorReporting"/> class.
    /// </summary>
    public class ErrorReportingTests
    {
        private readonly string _tempDirectory;

        public ErrorReportingTests()
        {
            // Create a unique temporary directory for the tests.
            _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
        }

        /// <summary>
        /// Sets the private static logFilePath field in ErrorReporting to a specified file path.
        /// </summary>
        /// <param name="filePath">The file path to set.</param>
        private void SetLogFilePath(string filePath)
        {
            FieldInfo? field = typeof(ErrorReporting).GetField("logFilePath", BindingFlags.NonPublic | BindingFlags.Static);
            field.Should().NotBeNull("The logFilePath field should exist in ErrorReporting.");
            field!.SetValue(null, filePath);
        }

        /// <summary>
        /// Gets a temporary log file path within the test's temporary directory.
        /// </summary>
        private string GetTempLogFilePath() => Path.Combine(_tempDirectory, "LoggerExceptions.txt");

        /// <summary>
        /// Tests that the LogFilePath property returns the expected file path.
        /// </summary>
        [Fact]
        public void LogFilePath_Property_ReturnsExpectedPath()
        {
            // Arrange
            string expectedPath = GetTempLogFilePath();
            SetLogFilePath(expectedPath);

            // Act
            string actualPath = ErrorReporting.LogFilePath;

            // Assert
            actualPath.Should().Be(expectedPath);
        }

        /// <summary>
        /// Tests that calling ReportException with a null exception does not create the log file.
        /// </summary>
        [Fact]
        public void ReportException_NullArgument_DoesNotCreateFile()
        {
            // Arrange
            string logFilePath = GetTempLogFilePath();
            SetLogFilePath(logFilePath);
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }

            // Act
            ErrorReporting.ReportException(null);

            // Assert
            File.Exists(logFilePath).Should().BeFalse("No file should be created when a null exception is passed.");
        }

        /// <summary>
        /// Tests that calling ReportException with a valid exception creates the log file and writes the exception details.
        /// </summary>
        [Fact]
        public void ReportException_ValidException_CreatesFileWithExceptionContent()
        {
            // Arrange
            string logFilePath = GetTempLogFilePath();
            SetLogFilePath(logFilePath);
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }
            Exception ex = new Exception("Test exception");

            // Act
            ErrorReporting.ReportException(ex);

            // Assert
            File.Exists(logFilePath).Should().BeTrue("The log file should be created when reporting a valid exception.");
            string content = File.ReadAllText(logFilePath);
            content.Should().Contain("Test exception", "the exception details should be written to the log file.");
        }

        /// <summary>
        /// Tests that when the existing log file is larger than 10MB, ReportException deletes it before appending new content.
        /// </summary>
        [Fact]
        public void ReportException_WhenFileTooLarge_DeletesFileAndWritesNewContent()
        {
            // Arrange
            string logFilePath = GetTempLogFilePath();
            SetLogFilePath(logFilePath);

            // Create a file that exceeds 10MB.
            byte[] largeContent = new byte[10_000_001]; // 10MB + 1 byte
            File.WriteAllBytes(logFilePath, largeContent);

            Exception ex = new Exception("New exception");

            // Act
            ErrorReporting.ReportException(ex);

            // Assert
            File.Exists(logFilePath).Should().BeTrue("The log file should exist after reporting an exception.");
            FileInfo info = new FileInfo(logFilePath);
            info.Length.Should().BeLessThan(largeContent.Length, "the old large file should have been deleted before writing the new exception.");
            string content = File.ReadAllText(logFilePath);
            content.Should().Contain("New exception", "the new exception details should be written to the log file.");
        }

        /// <summary>
        /// Tests that ReportException does not throw even if a file I/O exception occurs during logging.
        /// </summary>
        [Fact]
        public void ReportException_FileWriteThrowsException_DoesNotPropagateException()
        {
            // Arrange
            string logFilePath = GetTempLogFilePath();
            SetLogFilePath(logFilePath);
            // Ensure the directory exists.
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath) ?? string.Empty);

            // Create a file and mark it as read-only to simulate a file write failure.
            File.WriteAllText(logFilePath, "Initial content");
            File.SetAttributes(logFilePath, FileAttributes.ReadOnly);

            Exception ex = new Exception("Test exception for file write failure");

            // Act
            Action act = () => ErrorReporting.ReportException(ex);

            // Assert
            act.Should().NotThrow("ReportException should catch and suppress file IO exceptions.");

            // Cleanup: Remove the read-only attribute to allow deletion.
            File.SetAttributes(logFilePath, FileAttributes.Normal);
        }
    }
}
