using System.IO;
using System.Text;
using Microsoft.Build.Framework;
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
        private readonly MemoryStream _outputStream;
        private readonly BinaryWriter _binaryWriter;
        private readonly BuildEventArgsWriter _writer;

        public BuildEventArgsWriterTests()
        {
            _outputStream = new MemoryStream();
            _binaryWriter = new BinaryWriter(_outputStream, Encoding.UTF8, leaveOpen: true);
            _writer = new BuildEventArgsWriter(_binaryWriter);
        }

        /// <summary>
        /// Tests that the constructor initializes the writer without throwing exceptions.
        /// </summary>
        [Fact]
        public void Constructor_ShouldInitializeWithoutException()
        {
            // Arrange & Act
            // Instance is created in the constructor. No exception means success.

            // Assert
            Assert.NotNull(_writer);
        }

        /// <summary>
        /// Tests that the Write method properly writes a BuildMessageEventArgs record.
        /// The test verifies that data is written to the underlying stream.
        /// </summary>
        [Fact]
        public void Write_WithBuildMessageEventArgs_WritesData()
        {
            // Arrange
            string expectedMessage = "Test Message";
            string expectedHelpKeyword = "TestHelp";
            string expectedSenderName = "TestSender";
            DateTime now = DateTime.UtcNow;
            // Using the BuildMessageEventArgs constructor.
            var buildMessageEventArgs = new BuildMessageEventArgs(expectedMessage, expectedHelpKeyword, expectedSenderName, MessageImportance.Normal, now)
            {
                BuildEventContext = new BuildEventContext(1, 2, 3, 4, 5, 6, 7)
            };

            // Act
            _writer.Write(buildMessageEventArgs);
            _binaryWriter.Flush();

            // Assert
            Assert.True(_outputStream.Length > 0, "Expected non-empty output stream after writing event args.");
        }

        /// <summary>
        /// Tests that WriteStringRecord writes a string record with the correct record kind and string.
        /// </summary>
        [Fact]
        public void WriteStringRecord_WritesCorrectRecord()
        {
            // Arrange
            string testString = "TestString";
            _outputStream.SetLength(0);
            // Act
            _writer.WriteStringRecord(testString);
            _binaryWriter.Flush();
            _outputStream.Position = 0;

            // Read the record kind which is written as 7-bit encoded int.
            int recordKind = Read7BitEncodedInt(_outputStream);
            // Then read the string written by BinaryWriter.Write(string)
            string resultString = new BinaryReader(_outputStream, Encoding.UTF8, leaveOpen: true).ReadString();

            // Assert
            Assert.Equal((int)BinaryLogRecordKind.String, recordKind);
            Assert.Equal(testString, resultString);
        }

        /// <summary>
        /// Tests that WriteBlob writes the blob to the underlying stream correctly.
        /// It verifies the record kind, blob length, and blob content.
        /// </summary>
        [Fact]
        public void WriteBlob_WithValidStream_WritesBlob()
        {
            // Arrange
            byte[] blobContent = new byte[] { 1, 2, 3, 4, 5 };
            using (var blobStream = new MemoryStream(blobContent))
            {
                _outputStream.SetLength(0);

                // Act
                _writer.WriteBlob(BinaryLogRecordKind.BuildStarted, blobStream);
                _binaryWriter.Flush();
                _outputStream.Position = 0;

                // Read the record kind.
                int recordKind = Read7BitEncodedInt(_outputStream);
                // Read blob length.
                int length = Read7BitEncodedInt(_outputStream);
                // Read blob data.
                byte[] readBlob = new byte[length];
                int bytesRead = _outputStream.Read(readBlob, 0, length);

                // Assert
                Assert.Equal((int)BinaryLogRecordKind.BuildStarted, recordKind);
                Assert.Equal(blobContent.Length, length);
                Assert.Equal(blobContent.Length, bytesRead);
                Assert.Equal(blobContent, readBlob);
            }
        }

        /// <summary>
        /// Tests that WriteBlob throws an ArgumentOutOfRangeException when the provided stream's length exceeds int.MaxValue.
        /// </summary>
        [Fact]
        public void WriteBlob_WithTooLargeStream_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using (var largeStream = new FakeLargeStream())
            {
                // Act & Assert
                Assert.Throws<ArgumentOutOfRangeException>(() => _writer.WriteBlob(BinaryLogRecordKind.BuildFinished, largeStream));
            }
        }

        /// <summary>
        /// Helper method to read a 7-bit encoded integer from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The decoded integer.</returns>
        private int Read7BitEncodedInt(Stream stream)
        {
            int count = 0;
            int shift = 0;
            while (true)
            {
                int b = stream.ReadByte();
                if (b == -1)
                    throw new EndOfStreamException("Stream ended prematurely while attempting to read 7-bit encoded integer.");

                count |= (b & 0x7F) << shift;
                if ((b & 0x80) == 0)
                    break;
                shift += 7;
                if (shift > 35)
                    throw new FormatException("7-bit encoded integer is too large.");
            }
            return count;
        }

        /// <summary>
        /// A fake stream to simulate a stream that exceeds int.MaxValue in length.
        /// </summary>
        private class FakeLargeStream : Stream
        {
            public override bool CanRead => false;
            public override bool CanSeek => true;
            public override bool CanWrite => false;

            public override long Length => (long)int.MaxValue + 1;

            public override long Position { get => 0; set { } }

            public override void Flush() { }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return 0;
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
        }
    }
}
