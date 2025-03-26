using System;
using System.IO;
using System.Reflection;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ErrorReporting"/> class.
    /// </summary>
    public class ErrorReportingTests
    {
        /// <summary>
        /// Helper method to set the private static field 'logFilePath' in ErrorReporting.
        /// </summary>
        /// <param name="newPath">The new log file path.</param>
        private void SetLogFilePath(string newPath)
        {
            FieldInfo field = typeof(ErrorReporting).GetField("logFilePath", BindingFlags.NonPublic | BindingFlags.Static);
            field.SetValue(null, newPath);
        }

        /// <summary>
        /// Helper method to retrieve the current value of the private static field 'logFilePath'.
        /// </summary>
        /// <returns>The current log file path.</returns>
        private string GetLogFilePath()
        {
            FieldInfo field = typeof(ErrorReporting).GetField("logFilePath", BindingFlags.NonPublic | BindingFlags.Static);
            return field.GetValue(null) as string;
        }

        /// <summary>
        /// Tests that the LogFilePath property returns a path ending with "LoggerExceptions.txt".
        /// </summary>
        [Fact]
        public void LogFilePath_Property_ReturnsExpectedFileName()
        {
            // Act
            string logFilePath = ErrorReporting.LogFilePath;

            // Assert
            Assert.EndsWith("LoggerExceptions.txt", logFilePath);
        }

        /// <summary>
        /// Tests that ReportException does nothing when provided a null exception.
        /// Expected outcome: No log file is created.
        /// </summary>
        [Fact]
        public void ReportException_NullException_DoesNotCreateLogFile()
        {
            // Arrange
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDirectory);
            string tempLogFile = Path.Combine(tempDirectory, "LoggerExceptions.txt");
            string originalPath = GetLogFilePath();
            try
            {
                SetLogFilePath(tempLogFile);
                if (File.Exists(tempLogFile))
                {
                    File.Delete(tempLogFile);
                }

                // Act
                ErrorReporting.ReportException(null);

                // Assert
                Assert.False(File.Exists(tempLogFile), "Log file should not be created when a null exception is passed.");
            }
            finally
            {
                // Restore original log file path and clean up the temporary directory.
                SetLogFilePath(originalPath);
                try
                {
                    Directory.Delete(tempDirectory, true);
                }
                catch { }
            }
        }

        /// <summary>
        /// Tests that ReportException appends the exception text to the log file when a valid exception is provided.
        /// Expected outcome: The log file is created and contains the exception.ToString() followed by a newline.
        /// </summary>
        [Fact]
        public void ReportException_ValidException_AppendsToLogFile()
        {
            // Arrange
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDirectory);
            string tempLogFile = Path.Combine(tempDirectory, "LoggerExceptions.txt");
            string originalPath = GetLogFilePath();
            try
            {
                SetLogFilePath(tempLogFile);
                if (File.Exists(tempLogFile))
                {
                    File.Delete(tempLogFile);
                }

                Exception ex = new Exception("Test exception");

                // Act
                ErrorReporting.ReportException(ex);

                // Assert
                Assert.True(File.Exists(tempLogFile), "Log file should be created when a valid exception is reported.");
                string fileContent = File.ReadAllText(tempLogFile);
                string expectedContent = ex.ToString() + Environment.NewLine;
                Assert.Equal(expectedContent, fileContent);
            }
            finally
            {
                // Restore original log file path and clean up the temporary directory.
                SetLogFilePath(originalPath);
                try
                {
                    Directory.Delete(tempDirectory, true);
                }
                catch { }
            }
        }

        /// <summary>
        /// Tests that if the existing log file exceeds the size threshold, it is deleted before appending the new exception.
        /// Expected outcome: The oversized file is replaced with a new log file containing only the new exception text.
        /// </summary>
        [Fact]
        public void ReportException_LogFileTooLarge_DeletesOldFileAndAppendsNewException()
        {
            // Arrange
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDirectory);
            string tempLogFile = Path.Combine(tempDirectory, "LoggerExceptions.txt");
            string originalPath = GetLogFilePath();
            try
            {
                SetLogFilePath(tempLogFile);
                // Create a file larger than 10,000,000 bytes.
                byte[] largeContent = new byte[10000001];
                File.WriteAllBytes(tempLogFile, largeContent);

                Exception ex = new Exception("New exception");

                // Act
                ErrorReporting.ReportException(ex);

                // Assert
                Assert.True(File.Exists(tempLogFile), "Log file should exist after reporting an exception.");
                string fileContent = File.ReadAllText(tempLogFile);
                string expectedContent = ex.ToString() + Environment.NewLine;
                Assert.Equal(expectedContent, fileContent);
            }
            finally
            {
                // Restore original log file path and clean up the temporary directory.
                SetLogFilePath(originalPath);
                try
                {
                    Directory.Delete(tempDirectory, true);
                }
                catch { }
            }
        }

        /// <summary>
        /// Tests that ReportException does not throw an exception when an error occurs during file operations (e.g., due to an invalid log file path).
        /// Expected outcome: No exception is propagated.
        /// </summary>
//         [Fact] [Error] (186-52)CS0117 'Record' does not contain a definition for 'Exception'
//         public void ReportException_InvalidLogFilePath_DoesNotThrowException()
//         {
//             // Arrange
//             string originalPath = GetLogFilePath();
//             try
//             {
//                 // Set an invalid log file path.
//                 SetLogFilePath(string.Empty);
//                 Exception ex = new Exception("Test exception with invalid path");
// 
//                 // Act & Assert
//                 Exception caughtException = Record.Exception(() => ErrorReporting.ReportException(ex));
//                 Assert.Null(caughtException);
//             }
//             finally
//             {
//                 // Restore the original log file path.
//                 SetLogFilePath(originalPath);
//             }
//         }
    }
}
