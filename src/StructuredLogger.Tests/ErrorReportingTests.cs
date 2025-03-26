using System.IO;
using System.Reflection;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ErrorReporting"/> class.
    /// </summary>
    public class ErrorReportingTests : IDisposable
    {
        private readonly string _tempDir;
        private readonly string _testLogFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorReportingTests"/> class.
        /// Sets up a unique temporary directory and overrides the log file path used by ErrorReporting.
        /// </summary>
        public ErrorReportingTests()
        {
            // Create a unique temporary directory for testing.
            _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);
            _testLogFilePath = Path.Combine(_tempDir, "LoggerExceptions.txt");

            // Override the private static readonly field 'logFilePath' in ErrorReporting using reflection.
            var field = typeof(ErrorReporting).GetField("logFilePath", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, _testLogFilePath);
        }

        /// <summary>
        /// Cleans up the temporary testing directory.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (Directory.Exists(_tempDir))
                {
                    Directory.Delete(_tempDir, true);
                }
            }
            catch
            {
                // Ignore cleanup exceptions.
            }
        }

        /// <summary>
        /// Verifies that the LogFilePath property returns the expected file path.
        /// </summary>
        [Fact]
        public void LogFilePath_Always_ReturnsCorrectPath()
        {
            // Act
            string logPath = ErrorReporting.LogFilePath;

            // Assert: The returned log file path should match the overridden test log file path.
            Assert.Equal(_testLogFilePath, logPath);
        }

        /// <summary>
        /// Ensures that ReportException does nothing when a null exception is provided.
        /// </summary>
        [Fact]
        public void ReportException_NullException_DoesNothing()
        {
            // Arrange: Ensure the log file does not exist.
            if (File.Exists(_testLogFilePath))
            {
                File.Delete(_testLogFilePath);
            }

            // Act
            ErrorReporting.ReportException(null);

            // Assert: No log file should be created.
            Assert.False(File.Exists(_testLogFilePath));
        }

        /// <summary>
        /// Verifies that ReportException creates the log file and writes the exception details when no log file exists.
        /// </summary>
        [Fact]
        public void ReportException_ExceptionProvided_FileCreatedWithExceptionText()
        {
            // Arrange: Ensure the log file does not exist.
            if (File.Exists(_testLogFilePath))
            {
                File.Delete(_testLogFilePath);
            }
            Exception testException = new Exception("Test exception");

            // Act
            ErrorReporting.ReportException(testException);

            // Assert: The log file should exist with the exception message appended along with a new line.
            Assert.True(File.Exists(_testLogFilePath));
            string content = File.ReadAllText(_testLogFilePath);
            Assert.Contains("Test exception", content);
            Assert.Contains(Environment.NewLine, content);
        }

        /// <summary>
        /// Verifies that ReportException appends exception details to an existing small log file.
        /// </summary>
        [Fact]
        public void ReportException_SmallExistingFile_AppendsExceptionText()
        {
            // Arrange: Create a small log file with initial content.
            string initialContent = "Initial log content" + Environment.NewLine;
            File.WriteAllText(_testLogFilePath, initialContent);
            Exception testException = new Exception("Appended exception");

            // Act
            ErrorReporting.ReportException(testException);

            // Assert: The log file should contain both the initial content and the appended exception text.
            Assert.True(File.Exists(_testLogFilePath));
            string content = File.ReadAllText(_testLogFilePath);
            Assert.StartsWith(initialContent, content);
            Assert.Contains("Appended exception", content);
        }

        /// <summary>
        /// Verifies that when the existing log file exceeds the size limit, ReportException deletes it and writes only the new exception details.
        /// </summary>
        [Fact]
        public void ReportException_LargeExistingFile_DeletesAndCreatesNewFileWithExceptionText()
        {
            // Arrange: Create a log file exceeding 10,000,000 bytes.
            using (var stream = File.Create(_testLogFilePath))
            {
                stream.SetLength(10000001); // 10,000,001 bytes.
            }
            Exception testException = new Exception("Large file exception");

            // Act
            ErrorReporting.ReportException(testException);

            // Assert: The new log file should contain only the newly logged exception and be much smaller than the original size.
            Assert.True(File.Exists(_testLogFilePath));
            string content = File.ReadAllText(_testLogFilePath);
            Assert.Contains("Large file exception", content);
            Assert.True(content.Length < 1000, "Log file should only contain the freshly logged exception text.");
        }

        /// <summary>
        /// Ensures that ReportException handles exceptions during file operations gracefully without propagating them.
        /// Simulates a file system error by overriding the log file path with an invalid path.
        /// </summary>
        [Fact]
        public void ReportException_WhenFileOperationThrows_ExceptionIsCaught()
        {
            // Arrange: Override the log file path with an invalid path to simulate a file system failure.
            var invalidPath = "?:\\InvalidPath\\LoggerExceptions.txt";
            var field = typeof(ErrorReporting).GetField("logFilePath", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, invalidPath);
            Exception testException = new Exception("Exception during file operation");

            // Act: Call ReportException and capture any exception.
            var exception = Record.Exception(() => ErrorReporting.ReportException(testException));

            // Assert: ReportException should not propagate any exceptions.
            Assert.Null(exception);

            // Reset the log file path back to the valid test path.
            field.SetValue(null, _testLogFilePath);
        }
    }
}
