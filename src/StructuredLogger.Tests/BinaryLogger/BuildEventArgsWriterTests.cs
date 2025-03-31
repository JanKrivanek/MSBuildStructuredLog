using System;
using System.IO;
using System.Text;
using FluentAssertions;
using Moq;
using Microsoft.Build.Framework;
using Microsoft.Build.Framework.Profiler;
using Microsoft.Build.Logging.StructuredLogger;
using StructuredLogger.BinaryLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildEventArgsWriter"/> class.
    /// </summary>
    public class BuildEventArgsWriterTests
    {
        /// <summary>
        /// Tests that calling Write with a BuildMessageEventArgs writes data to the underlying stream.
        /// This verifies that the method produces output and sets up the binary log record correctly.
        /// </summary>
        [Fact]
        public void Write_BuildMessageEventArgs_ShouldWriteNonEmptyStream()
        {
            // Arrange
            using var outputStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(outputStream, Encoding.UTF8, leaveOpen: true);
            var writer = new BuildEventArgsWriter(binaryWriter);
            var timestamp = DateTime.UtcNow;
            var message = "Test Message";
            var buildMessageEventArgs = new BuildMessageEventArgs(message, helpKeyword: "Help", senderName: "TestSender", MessageImportance.High, timestamp)
            {
                BuildEventContext = new BuildEventContext(1, 2, 3, 4, 5, 6, 7)
            };

            // Act
            writer.Write(buildMessageEventArgs);
            binaryWriter.Flush();

            // Assert
            outputStream.Length.Should().BeGreaterThan(0);
        }

        /// <summary>
        /// Tests that WriteBlob writes the provided blob content directly to the underlying stream.
        /// This test verifies that the blob content appears within the final binary output.
        /// </summary>
        [Fact]
        public void WriteBlob_ValidStream_WritesContent()
        {
            // Arrange
            var blobContent = "Hello Blob";
            var blobBytes = Encoding.UTF8.GetBytes(blobContent);
            using var blobStream = new MemoryStream(blobBytes);
            using var outputStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(outputStream, Encoding.UTF8, leaveOpen: true);
            var writer = new BuildEventArgsWriter(binaryWriter);

            // Act
            writer.WriteBlob(BinaryLogRecordKind.String, blobStream);
            binaryWriter.Flush();

            // Assert
            outputStream.Length.Should().BeGreaterThan(blobBytes.Length);
            // Check that some part of the blob content appears in the final output.
            var outputBytes = outputStream.ToArray();
            Array.IndexOf(outputBytes, blobBytes[0]).Should().BeGreaterThanOrEqualTo(0);
        }

        /// <summary>
        /// Tests that WriteBlob throws an ArgumentOutOfRangeException when the provided stream's length exceeds int.MaxValue.
        /// This verifies boundary checking for excessively large streams.
        /// </summary>
        [Fact]
        public void WriteBlob_LargeStream_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var mockStream = new Mock<Stream>();
            mockStream.Setup(s => s.CanSeek).Returns(true);
            mockStream.Setup(s => s.Length).Returns((long)int.MaxValue + 1);
            // Setup CopyTo to do nothing.
            mockStream.Setup(s => s.CopyTo(It.IsAny<Stream>()));

            using var outputStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(outputStream, Encoding.UTF8, leaveOpen: true);
            var writer = new BuildEventArgsWriter(binaryWriter);

            // Act
            Action act = () => writer.WriteBlob(BinaryLogRecordKind.String, mockStream.Object);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        /// <summary>
        /// Tests that WriteStringRecord writes a string record correctly by writing the record kind and the string.
        /// This test verifies that the underlying stream contains the serialized record kind followed by the text.
        /// </summary>
        [Fact]
        public void WriteStringRecord_ValidString_WritesCorrectRecord()
        {
            // Arrange
            var testString = "Sample String";
            using var outputStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(outputStream, Encoding.UTF8, leaveOpen: true);
            var writer = new BuildEventArgsWriter(binaryWriter);

            // Act
            writer.WriteStringRecord(testString);
            binaryWriter.Flush();

            // Assert
            outputStream.Position = 0;
            int recordKind = Read7BitEncodedInt(outputStream);
            // Expect recordKind to equal (int)BinaryLogRecordKind.String
            recordKind.Should().Be((int)BinaryLogRecordKind.String);

            // Read the string written via BinaryWriter.Write(string)
            string readString = new BinaryReader(outputStream, Encoding.UTF8).ReadString();
            readString.Should().Be(testString);
        }

        /// <summary>
        /// Helper method to read a 7-bit encoded integer from the stream.
        /// </summary>
        /// <param name="stream">The stream from which to read the integer.</param>
        /// <returns>The decoded integer.</returns>
        private int Read7BitEncodedInt(Stream stream)
        {
            int count = 0;
            int shift = 0;
            while (true)
            {
                int b = stream.ReadByte();
                if (b == -1)
                {
                    throw new EndOfStreamException();
                }
                count |= (b & 0x7F) << shift;
                if ((b & 0x80) == 0)
                    break;
                shift += 7;
            }
            return count;
        }
    }
}
