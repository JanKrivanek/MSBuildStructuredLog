using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Provides minimal stub implementations to support testing of ArchiveFileEventArgsExtensions.
    /// </summary>
    internal class ArchiveFile
    {
        public string FullPath { get; set; }
        public string Text { get; set; }

        public ArchiveFile(string fullPath, string text)
        {
            FullPath = fullPath;
            Text = text;
        }
    }

    /// <summary>
    /// Provides minimal stub implementation to support testing of ArchiveFileEventArgsExtensions.
    /// </summary>
    internal class ArchiveFileEventArgs : EventArgs
    {
        public ArchiveFile ArchiveFile { get; set; }

        public ArchiveFileEventArgs(ArchiveFile archiveFile)
        {
            ArchiveFile = archiveFile;
        }
    }

    /// <summary>
    /// Provides minimal stub implementation to support testing of ArchiveFileEventArgsExtensions.
    /// </summary>
    internal class StringReadEventArgs : EventArgs
    {
        public string Input { get; }
        public string StringToBeUsed { get; set; }

        public StringReadEventArgs(string input)
        {
            Input = input;
            StringToBeUsed = input;
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="ArchiveFileEventArgsExtensions"/> class.
    /// </summary>
    [TestClass]
    public class ArchiveFileEventArgsExtensionsTests
    {
        private readonly Func<string, string> _toUpperFunc;
        private readonly Func<string, string> _appendSuffixFunc;

        public ArchiveFileEventArgsExtensionsTests()
        {
            // A function that converts the input string to uppercase.
            _toUpperFunc = s => s?.ToUpper();
            // A function that appends "_processed" to the input string, even if null.
            _appendSuffixFunc = s => (s ?? "null") + "_processed";
        }

        /// <summary>
        /// Tests the ToArchiveFileHandler method with a stringHandler that converts the input to uppercase.
        /// Verifies that both the FullPath and Text of ArchiveFile are processed correctly.
        /// </summary>
        [TestMethod]
        public void ToArchiveFileHandler_HappyPath_ProcessesFullPathAndTextCorrectly()
        {
            // Arrange
            Action<StringReadEventArgs> stringHandler = args =>
            {
                args.StringToBeUsed = _toUpperFunc(args.Input);
            };

            var originalFullPath = "path/to/file.txt";
            var originalText = "file content";
            var archiveFile = new ArchiveFile(originalFullPath, originalText);
            var eventArgs = new ArchiveFileEventArgs(archiveFile);

            // Act
            var handler = stringHandler.ToArchiveFileHandler();
            handler(eventArgs);

            // Assert
            Assert.IsNotNull(eventArgs.ArchiveFile, "ArchiveFile should not be null after processing.");
            Assert.AreEqual(_toUpperFunc(originalFullPath), eventArgs.ArchiveFile.FullPath, "FullPath was not processed correctly.");
            Assert.AreEqual(_toUpperFunc(originalText), eventArgs.ArchiveFile.Text, "Text was not processed correctly.");
        }

        /// <summary>
        /// Tests that invoking ToArchiveFileHandler on a null stringHandler throws a NullReferenceException.
        /// </summary>
        [TestMethod]
        public void ToArchiveFileHandler_NullStringHandler_ThrowsNullReferenceException()
        {
            // Arrange
            Action<StringReadEventArgs> stringHandler = null;

            // Act & Assert
            Assert.ThrowsException<NullReferenceException>(() =>
            {
                // This should throw because extension method is invoked on a null instance.
                var handler = stringHandler.ToArchiveFileHandler();
            }, "Expected a NullReferenceException when calling ToArchiveFileHandler on a null delegate.");
        }

        /// <summary>
        /// Tests that passing an ArchiveFileEventArgs with a null ArchiveFile results in a NullReferenceException.
        /// </summary>
        [TestMethod]
        public void ToArchiveFileHandler_NullArchiveFile_ThrowsNullReferenceException()
        {
            // Arrange
            Action<StringReadEventArgs> stringHandler = args =>
            {
                // Simply pass through without modification.
                args.StringToBeUsed = args.Input;
            };

            var eventArgs = new ArchiveFileEventArgs(null);

            // Act
            var handler = stringHandler.ToArchiveFileHandler();

            // Assert
            Assert.ThrowsException<NullReferenceException>(() =>
            {
                handler(eventArgs);
            }, "Expected a NullReferenceException when processing an ArchiveFileEventArgs with a null ArchiveFile.");
        }

        /// <summary>
        /// Tests the ToArchiveFileHandler method when the ArchiveFile's FullPath is null.
        /// Verifies that the stringHandler processes the null value as per the defined function.
        /// </summary>
        [TestMethod]
        public void ToArchiveFileHandler_NullFullPath_ProcessesAsNullProcessed()
        {
            // Arrange
            Action<StringReadEventArgs> stringHandler = args =>
            {
                args.StringToBeUsed = _appendSuffixFunc(args.Input);
            };

            // FullPath is null, Text is valid.
            string originalText = "content";
            var archiveFile = new ArchiveFile(null, originalText);
            var eventArgs = new ArchiveFileEventArgs(archiveFile);

            // Act
            var handler = stringHandler.ToArchiveFileHandler();
            handler(eventArgs);

            // Assert
            Assert.IsNotNull(eventArgs.ArchiveFile, "ArchiveFile should not be null after processing even if FullPath was null.");
            Assert.AreEqual(_appendSuffixFunc(null), eventArgs.ArchiveFile.FullPath, "FullPath was not processed correctly when originally null.");
            Assert.AreEqual(_appendSuffixFunc(originalText), eventArgs.ArchiveFile.Text, "Text was not processed correctly.");
        }

        /// <summary>
        /// Tests the ToArchiveFileHandler method when the ArchiveFile's Text is null.
        /// Verifies that the stringHandler processes the null value as per the defined function.
        /// </summary>
        [TestMethod]
        public void ToArchiveFileHandler_NullText_ProcessesAsNullProcessed()
        {
            // Arrange
            Action<StringReadEventArgs> stringHandler = args =>
            {
                args.StringToBeUsed = _appendSuffixFunc(args.Input);
            };

            // Text is null, FullPath is valid.
            string originalFullPath = "some/path";
            var archiveFile = new ArchiveFile(originalFullPath, null);
            var eventArgs = new ArchiveFileEventArgs(archiveFile);

            // Act
            var handler = stringHandler.ToArchiveFileHandler();
            handler(eventArgs);

            // Assert
            Assert.IsNotNull(eventArgs.ArchiveFile, "ArchiveFile should not be null after processing even if Text was null.");
            Assert.AreEqual(_appendSuffixFunc(originalFullPath), eventArgs.ArchiveFile.FullPath, "FullPath was not processed correctly.");
            Assert.AreEqual(_appendSuffixFunc(null), eventArgs.ArchiveFile.Text, "Text was not processed correctly when originally null.");
        }
    }
}
