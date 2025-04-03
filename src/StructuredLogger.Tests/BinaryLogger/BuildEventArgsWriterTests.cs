using System;
using System.IO;
using System.Text;
using FluentAssertions;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// A fake BuildMessageEventArgs implementation for testing purposes.
    /// </summary>
    internal class FakeBuildMessageEventArgs : BuildMessageEventArgs
    {
        public FakeBuildMessageEventArgs(string message)
            : base(message, helpKeyword: null, senderName: "TestSender", importance: MessageImportance.Normal, eventTimestamp: DateTime.UtcNow)
        {
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="BuildEventArgsWriter"/> class.
    /// </summary>
    public class BuildEventArgsWriterTests
    {
        /// <summary>
        /// Tests that calling Write with a valid BuildEventArgs instance writes binary data to the underlying stream.
        /// </summary>
        [Fact]
        public void Write_WithValidBuildEventArgs_WritesDataToStream()
        {
            // Arrange
            using var memoryStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, leaveOpen: true);
            var writer = new BuildEventArgsWriter(binaryWriter);
            var fakeEvent = new FakeBuildMessageEventArgs("Test message");

            // Act
            writer.Write(fakeEvent);
            binaryWriter.Flush();
            var outputBytes = memoryStream.ToArray();

            // Assert
            outputBytes.Should().NotBeNull();
            outputBytes.Length.Should().BeGreaterThan(0, "because writing a valid event should produce non-empty binary output");
        }

        /// <summary>
        /// Tests that WriteBlob writes the provided blob along with its header information to the underlying stream.
        /// </summary>
        [Fact]
        public void WriteBlob_WithValidBlob_WritesBlobToStream()
        {
            // Arrange
            using var outputStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(outputStream, Encoding.UTF8, leaveOpen: true);
            var writer = new BuildEventArgsWriter(binaryWriter);

            // Prepare a blob stream with known content.
            byte[] blobContent = Encoding.UTF8.GetBytes("Blob content for testing.");
            using var blobStream = new MemoryStream(blobContent);
            long initialLength = outputStream.Length;

            // Act
            writer.WriteBlob(BinaryLogRecordKind.String, blobStream);
            binaryWriter.Flush();
            long finalLength = outputStream.Length;
            var outputBytes = outputStream.ToArray();
            string outputText = Encoding.UTF8.GetString(outputBytes);

            // Assert
            finalLength.Should().BeGreaterThan(initialLength, "because writing a blob should increase the stream length");
            outputText.Should().Contain("Blob content for testing.", "because the blob content should be present in the output stream");
        }
    }
}
