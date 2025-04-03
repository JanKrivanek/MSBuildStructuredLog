using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.IO;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ErrorReporting"/> class.
    /// </summary>
    public class ErrorReportingTests : IDisposable
    {
        private readonly string _logFilePath;

        public ErrorReportingTests()
        {
            // Retrieve the log file path from the ErrorReporting class.
            _logFilePath = ErrorReporting.LogFilePath;
            CleanupLogFile();
        }

        /// <summary>
        /// Cleans up the log file if it exists.
        /// </summary>
        private void CleanupLogFile()
        {
            try
            {
                if (File.Exists(_logFilePath))
                {
                    File.Delete(_logFilePath);
                }
            }
            catch
            {
                // Swallow cleanup exceptions.
            }
        }

        /// <summary>
        /// Disposes resources by cleaning up the log file created during tests.
        /// </summary>
        public void Dispose()
        {
            CleanupLogFile();
        }

        /// <summary>
        /// Tests that calling ReportException with a null exception does not create the log file.
        /// Expected outcome: No file is created.
        /// </summary>
        [Fact]
        public void ReportException_NullInput_DoesNotCreateFile()
        {
            // Arrange
            // Ensure the log file does not exist.
            if (File.Exists(_logFilePath))
            {
                File.Delete(_logFilePath);
            }

            // Act
            ErrorReporting.ReportException(null);

            // Assert
            File.Exists(_logFilePath).Should().BeFalse("because no file should be created when reporting a null exception");
        }

        /// <summary>
        /// Tests that ReportException appends the exception information to the log file.
        /// Expected outcome: The log file is created and contains the exception details.
        /// </summary>
        [Fact]
        public void ReportException_HappyPath_AppendsExceptionToLog()
        {
            // Arrange
            var testException = new Exception("Test exception");
            if (File.Exists(_logFilePath))
            {
                File.Delete(_logFilePath);
            }

            // Act
            ErrorReporting.ReportException(testException);

            // Assert
            File.Exists(_logFilePath).Should().BeTrue("because the log file should be created when a valid exception is reported");
            string content = File.ReadAllText(_logFilePath);
            content.Should().Contain(testException.ToString(), "because the exception details should be appended to the log file");
            content.Should().EndWith(Environment.NewLine, "because a newline is appended after the exception details");
        }

        /// <summary>
        /// Tests that ReportException deletes the log file if its size exceeds 10,000,000 bytes and then appends the new exception.
        /// Expected outcome: The old log file is deleted and a new one is created with the exception details.
        /// </summary>
        [Fact]
        public void ReportException_FileTooBig_DeletesOldLogAndAppendsNewException()
        {
            // Arrange
            // Create a dummy log file with content larger than 10,000,000 bytes.
            string dummyContent = new string('a', 10000001);
            Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath) ?? string.Empty);
            File.WriteAllText(_logFilePath, dummyContent);
            new FileInfo(_logFilePath).Length.Should().BeGreaterThan(10000000, "because we wrote more than 10,000,000 characters");

            var testException = new Exception("New test exception");

            // Act
            ErrorReporting.ReportException(testException);

            // Assert
            File.Exists(_logFilePath).Should().BeTrue("because the log file should be re-created even if the old one was too big");
            string content = File.ReadAllText(_logFilePath);
            content.Should().Contain(testException.ToString(), "because the new exception details should be appended after deleting the oversized file");
            content.Should().EndWith(Environment.NewLine, "because a newline is appended after the exception details");
        }

        /// <summary>
        /// Tests that ReportException does not throw an exception even when a file operation fails.
        /// This is simulated by locking the log file to force a failure in appending text.
        /// Expected outcome: No exception is propagated.
        /// </summary>
        [Fact]
        public void ReportException_FileLocked_DoesNotThrow()
        {
            // Arrange
            Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath) ?? string.Empty);
            // Create and lock the file.
            using (var stream = new FileStream(_logFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                var testException = new Exception("Locked file exception test");

                // Act
                Action act = () => ErrorReporting.ReportException(testException);

                // Assert
                act.Should().NotThrow("because ReportException should catch and swallow any exceptions thrown during file operations");
            }
        }
    }
}
