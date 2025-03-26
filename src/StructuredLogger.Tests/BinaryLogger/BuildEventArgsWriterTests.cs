using System;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Framework.Profiler;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildEventArgsWriter"/> class.
    /// </summary>
    public class BuildEventArgsWriterTests
    {
        private readonly MemoryStream _memoryStream;
        private readonly BinaryWriter _binaryWriter;
        private readonly BuildEventArgsWriter _writer;

        /// <summary>
        /// Initializes a new instance of the test class and creates a new BuildEventArgsWriter.
        /// </summary>
        public BuildEventArgsWriterTests()
        {
            _memoryStream = new MemoryStream();
            _binaryWriter = new BinaryWriter(_memoryStream, Encoding.UTF8, leaveOpen: true);
            _writer = new BuildEventArgsWriter(_binaryWriter);
        }

        /// <summary>
        /// Tests that calling Write with a BuildMessageEventArgs instance writes data to the underlying stream.
        /// Verifies that the output contains the provided message.
        /// </summary>
        [Fact]
        public void Write_WithBuildMessageEventArgs_WritesDataToStream()
        {
            // Arrange
            string testMessage = "Test Message";
            var timestamp = DateTime.UtcNow;
            var messageEventArgs = new BuildMessageEventArgs(testMessage, helpKeyword: null, senderName: "TestSender", MessageImportance.High, timestamp);

            // Act
            _writer.Write(messageEventArgs);
            _binaryWriter.Flush();
            byte[] output = _memoryStream.ToArray();
            string outputString = Encoding.UTF8.GetString(output);

            // Assert
            Assert.NotEmpty(output);
            Assert.Contains(testMessage, outputString);
        }

        /// <summary>
        /// Tests that WriteBlob writes a blob record to the stream when given a valid stream.
        /// Verifies that the blob data appears in the output.
        /// </summary>
        [Fact]
        public void WriteBlob_WithValidStream_WritesBlobData()
        {
            // Arrange
            string blobContent = "BlobData";
            byte[] blobBytes = Encoding.UTF8.GetBytes(blobContent);
            using MemoryStream blobStream = new MemoryStream(blobBytes);
            // Use a dummy record kind; for example, assume BuildStarted record kind.
            BinaryLogRecordKind recordKind = BinaryLogRecordKind.BuildStarted;

            // Act
            _writer.WriteBlob(recordKind, blobStream);
            _binaryWriter.Flush();
            byte[] output = _memoryStream.ToArray();
            string outputString = Encoding.UTF8.GetString(output);

            // Assert
            Assert.NotEmpty(output);
            Assert.Contains(blobContent, outputString);
        }

        /// <summary>
        /// Tests that WriteBlob throws an ArgumentOutOfRangeException when the provided stream's length exceeds int.MaxValue.
        /// </summary>
        [Fact]
        public void WriteBlob_WithOversizedStream_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.Setup(s => s.CanSeek).Returns(true);
            // Set Length property to int.MaxValue + 1
            mockStream.Setup(s => s.Length).Returns((long)int.MaxValue + 1);
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _writer.WriteBlob(BinaryLogRecordKind.BuildStarted, mockStream.Object));
        }

        /// <summary>
        /// Tests that WriteStringRecord writes a string record to the stream.
        /// Verifies that the output contains the provided string.
        /// </summary>
        [Fact]
        public void WriteStringRecord_WritesRecordContainingString()
        {
            // Arrange
            string testString = "Hello";
            
            // Act
            _writer.WriteStringRecord(testString);
            _binaryWriter.Flush();
            byte[] output = _memoryStream.ToArray();
            string outputString = Encoding.UTF8.GetString(output);

            // Assert
            Assert.NotEmpty(output);
            Assert.Contains(testString, outputString);
        }
    }
}
