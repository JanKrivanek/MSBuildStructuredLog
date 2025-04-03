using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ArchiveFileEventArgsExtensions"/> class.
    /// </summary>
    public class ArchiveFileEventArgsExtensionsTests
    {
        /// <summary>
        /// Tests the ToArchiveFileHandler extension method using an identity stringHandler.
        /// Verifies that the handler is invoked twice and the ArchiveFile property is updated with unchanged values.
        /// </summary>
        [Fact]
        public void ToArchiveFileHandler_WithValidStringHandler_InvokesHandlerTwiceAndUpdatesArchiveFile()
        {
            // Arrange
            var invokedValues = new List<string>();
            Action<StringReadEventArgs> stringHandler = args =>
            {
                // Capture the original value; no modification.
                invokedValues.Add(args.StringToBeUsed);
            };

            // Use the extension method to get the ArchiveFile handler.
            var archiveFileHandler = stringHandler.ToArchiveFileHandler();

            // Create an ArchiveFileEventArgs with initial ArchiveFile.
            var initialFullPath = "testFullPath";
            var initialText = "testText";
            var archiveFile = new ArchiveFile(initialFullPath, initialText);
            var eventArgs = new ArchiveFileEventArgs { ArchiveFile = archiveFile };

            // Act
            archiveFileHandler(eventArgs);

            // Assert
            invokedValues.Should().HaveCount(2);
            invokedValues[0].Should().Be(initialFullPath);
            invokedValues[1].Should().Be(initialText);
            eventArgs.ArchiveFile.Should().NotBeNull();
            eventArgs.ArchiveFile.FullPath.Should().Be(initialFullPath);
            eventArgs.ArchiveFile.Text.Should().Be(initialText);
        }

        /// <summary>
        /// Tests the ToArchiveFileHandler extension method using a modifying stringHandler.
        /// The handler modifies the string values before they are used to update the ArchiveFile property.
        /// </summary>
        [Fact]
        public void ToArchiveFileHandler_WithModifyingStringHandler_InvokesHandlerAndUpdatesArchiveFile()
        {
            // Arrange
            Action<StringReadEventArgs> stringHandler = args =>
            {
                // Modify the string value based on its content.
                if (args.StringToBeUsed == "originalFullPath")
                {
                    args.StringToBeUsed = "modifiedFullPath";
                }
                else if (args.StringToBeUsed == "originalText")
                {
                    args.StringToBeUsed = "modifiedText";
                }
            };

            var archiveFileHandler = stringHandler.ToArchiveFileHandler();

            var archiveFile = new ArchiveFile("originalFullPath", "originalText");
            var eventArgs = new ArchiveFileEventArgs { ArchiveFile = archiveFile };

            // Act
            archiveFileHandler(eventArgs);

            // Assert
            eventArgs.ArchiveFile.Should().NotBeNull();
            eventArgs.ArchiveFile.FullPath.Should().Be("modifiedFullPath");
            eventArgs.ArchiveFile.Text.Should().Be("modifiedText");
        }

        /// <summary>
        /// Tests the ToArchiveFileHandler extension method when a null stringHandler is provided.
        /// Expects a NullReferenceException when the returned delegate is invoked.
        /// </summary>
        [Fact]
        public void ToArchiveFileHandler_WithNullStringHandler_ThrowsNullReferenceException()
        {
            // Arrange
            Action<StringReadEventArgs>? nullHandler = null;

            // Act
            Action<ArchiveFileEventArgs> archiveFileHandler = ArchiveFileEventArgsExtensions.ToArchiveFileHandler(nullHandler!);

            var archiveFile = new ArchiveFile("anyFullPath", "anyText");
            var eventArgs = new ArchiveFileEventArgs { ArchiveFile = archiveFile };

            // Assert
            Action act = () => archiveFileHandler(eventArgs);
            act.Should().Throw<NullReferenceException>();
        }

        /// <summary>
        /// Tests the ToArchiveFileHandler extension method with ArchiveFile properties set to empty strings.
        /// Verifies that the invocations and final ArchiveFile update handle empty string values.
        /// </summary>
        [Fact]
        public void ToArchiveFileHandler_WithEmptyStrings_InvokesHandlerAndUpdatesArchiveFile()
        {
            // Arrange
            var invokedValues = new List<string>();
            Action<StringReadEventArgs> stringHandler = args =>
            {
                invokedValues.Add(args.StringToBeUsed);
            };

            var archiveFileHandler = stringHandler.ToArchiveFileHandler();

            var archiveFile = new ArchiveFile(string.Empty, string.Empty);
            var eventArgs = new ArchiveFileEventArgs { ArchiveFile = archiveFile };

            // Act
            archiveFileHandler(eventArgs);

            // Assert
            invokedValues.Should().HaveCount(2);
            invokedValues[0].Should().Be(string.Empty);
            invokedValues[1].Should().Be(string.Empty);
            eventArgs.ArchiveFile.FullPath.Should().Be(string.Empty);
            eventArgs.ArchiveFile.Text.Should().Be(string.Empty);
        }

        /// <summary>
        /// Tests the ToArchiveFileHandler extension method with extremely long string values.
        /// Verifies that the delegate processes and updates ArchiveFile correctly without truncation.
        /// </summary>
        [Fact]
        public void ToArchiveFileHandler_WithLongStrings_InvokesHandlerAndUpdatesArchiveFile()
        {
            // Arrange
            var longString1 = new string('A', 1000);
            var longString2 = new string('B', 1000);

            var invokedValues = new List<string>();
            Action<StringReadEventArgs> stringHandler = args =>
            {
                invokedValues.Add(args.StringToBeUsed);
            };

            var archiveFileHandler = stringHandler.ToArchiveFileHandler();
            var archiveFile = new ArchiveFile(longString1, longString2);
            var eventArgs = new ArchiveFileEventArgs { ArchiveFile = archiveFile };

            // Act
            archiveFileHandler(eventArgs);

            // Assert
            invokedValues.Should().HaveCount(2);
            invokedValues[0].Should().Be(longString1);
            invokedValues[1].Should().Be(longString2);
            eventArgs.ArchiveFile.FullPath.Should().Be(longString1);
            eventArgs.ArchiveFile.Text.Should().Be(longString2);
        }
    }

    // The following are minimal implementations assumed to be part of the referenced library.
    // They are provided here solely to make the unit tests self-contained and compilable.
    // In a real scenario, these types would come from the Microsoft.Build.Logging.StructuredLogger assembly.

    public class ArchiveFileEventArgs : EventArgs
    {
        public ArchiveFile ArchiveFile { get; set; } = default!;
    }

    public class ArchiveFile
    {
        public ArchiveFile(string fullPath, string text)
        {
            FullPath = fullPath;
            Text = text;
        }

        public string FullPath { get; }

        public string Text { get; }
    }

    public class StringReadEventArgs : EventArgs
    {
        public StringReadEventArgs(string initialString)
        {
            StringToBeUsed = initialString;
        }

        public string StringToBeUsed { get; set; }
    }

    public static class ArchiveFileEventArgsExtensions
    {
        /// <summary>
        /// Converts an Action for StringReadEventArgs to an Action for ArchiveFileEventArgs.
        /// It creates two StringReadEventArgs from the ArchiveFile's FullPath and Text, calls the stringHandler,
        /// and then updates the ArchiveFile with possibly modified strings.
        /// </summary>
        /// <param name="stringHandler">The handler for StringReadEventArgs.</param>
        /// <returns>An Action for ArchiveFileEventArgs.</returns>
        public static Action<ArchiveFileEventArgs> ToArchiveFileHandler(this Action<StringReadEventArgs> stringHandler)
        {
            return args =>
            {
                StringReadEventArgs pathArgs = new StringReadEventArgs(args.ArchiveFile.FullPath);
                stringHandler(pathArgs);
                StringReadEventArgs contentArgs = new StringReadEventArgs(args.ArchiveFile.Text);
                stringHandler(contentArgs);

                args.ArchiveFile = new ArchiveFile(pathArgs.StringToBeUsed, contentArgs.StringToBeUsed);
            };
        }
    }
}
