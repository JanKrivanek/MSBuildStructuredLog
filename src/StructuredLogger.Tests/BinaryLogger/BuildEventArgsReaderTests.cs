using System.IO;
using System.Text;
using Moq;
using Xunit;
using DotUtils.StreamUtils;
using Microsoft.Build.Logging.StructuredLogger;
using StructuredLogger.BinaryLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildEventArgsReader"/> class.
    /// </summary>
    public class BuildEventArgsReaderTests
    {
        // For these tests we assume that the minimum forward compatible version is 18 as indicated by error messages in the source.
        private const int MinimalForwardCompatibleVersion = 18;

        // We use 99 as our test value for BinaryLogRecordKind.EndOfFile in our constructed binary stream.
        private const int EndOfFileRecordKindValue = 99;
        
        private readonly byte[] _endOfFileRecordBytes;
        
        public BuildEventArgsReaderTests()
        {
            // Prepare a byte array that represents a record kind of EndOfFile.
            // The Write7BitEncodedInt algorithm is used to encode integers.
            _endOfFileRecordBytes = Get7BitEncodedIntBytes(EndOfFileRecordKindValue);
        }
        
        private byte[] Get7BitEncodedIntBytes(int value)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                int v = value;
                while (v >= 0x80)
                {
                    bw.Write((byte)(v | 0x80));
                    v >>= 7;
                }
                bw.Write((byte)v);
                return ms.ToArray();
            }
        }
        
        /// <summary>
        /// Tests that the constructor properly sets the file format version and initializes internal state.
        /// </summary>
        [Fact]
        public void Constructor_ValidParameters_StateSet()
        {
            // Arrange
            using var ms = new MemoryStream(new byte[10]);
            using var br = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            int fileFormatVersion = MinimalForwardCompatibleVersion;
            
            // Act
            var reader = new BuildEventArgsReader(br, fileFormatVersion);
            
            // Assert
            Assert.Equal(fileFormatVersion, reader.FileFormatVersion);
        }
        
        /// <summary>
        /// Tests that the Position property returns the current position of the underlying stream.
        /// </summary>
        [Fact]
        public void Position_WhenStreamPositionChanged_ReturnsUpdatedPosition()
        {
            // Arrange
            byte[] data = new byte[20];
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            var reader = new BuildEventArgsReader(br, MinimalForwardCompatibleVersion);
            
            // Act
            long initialPosition = reader.Position;
            ms.Seek(10, SeekOrigin.Begin);
            long updatedPosition = reader.Position;
            
            // Assert
            Assert.Equal(0, initialPosition);
            Assert.Equal(10, updatedPosition);
        }
        
        /// <summary>
        /// Tests that setting SkipUnknownEvents to true on a reader with file format version below the minimum throws an InvalidOperationException.
        /// </summary>
        [Fact]
        public void SkipUnknownEvents_WhenFileFormatVersionBelowMinimum_ThrowsInvalidOperationException()
        {
            // Arrange
            using var ms = new MemoryStream(new byte[10]);
            using var br = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            int fileFormatVersion = MinimalForwardCompatibleVersion - 1;
            var reader = new BuildEventArgsReader(br, fileFormatVersion);
            
            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => reader.SkipUnknownEvents = true);
            Assert.Contains("needs >= 18", ex.Message);
        }
        
        /// <summary>
        /// Tests that setting SkipUnknownEventParts to true on a reader with file format version below the minimum throws an InvalidOperationException.
        /// </summary>
        [Fact]
        public void SkipUnknownEventParts_WhenFileFormatVersionBelowMinimum_ThrowsInvalidOperationException()
        {
            // Arrange
            using var ms = new MemoryStream(new byte[10]);
            using var br = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            int fileFormatVersion = MinimalForwardCompatibleVersion - 1;
            var reader = new BuildEventArgsReader(br, fileFormatVersion);
            
            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => reader.SkipUnknownEventParts = true);
            Assert.Contains("needs >= 18", ex.Message);
        }
        
        /// <summary>
        /// Tests that Dispose disposes the internal string storage, and, when CloseInput is true, disposes the underlying BinaryReader.
        /// </summary>
        [Fact]
        public void Dispose_WhenCloseInputTrue_DisposesBinaryReader()
        {
            // Arrange
            var ms = new MemoryStream(new byte[10]);
            var br = new BinaryReader(ms, Encoding.UTF8, leaveOpen: false);
            var reader = new BuildEventArgsReader(br, MinimalForwardCompatibleVersion)
            {
                CloseInput = true
            };
            
            // Act
            reader.Dispose();
            
            // Assert
            Assert.Throws<ObjectDisposedException>(() => ms.ReadByte());
        }
        
        /// <summary>
        /// Tests that ReadRaw returns a RawRecord with an EndOfFile record kind and a null stream when the record kind indicates end of file.
        /// </summary>
        [Fact]
        public void ReadRaw_WhenEndOfFileRecord_ReturnsRawRecordWithNullStream()
        {
            // Arrange
            // Create a memory stream that contains the EndOfFile record kind encoded as a 7-bit encoded integer.
            using var ms = new MemoryStream(_endOfFileRecordBytes);
            using var br = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            var reader = new BuildEventArgsReader(br, MinimalForwardCompatibleVersion);
            
            // Act
            var rawRecord = reader.ReadRaw();
            
            // Assert
            Assert.Equal((BinaryLogRecordKind)EndOfFileRecordKindValue, rawRecord.RecordKind);
            Assert.Same(Stream.Null, rawRecord.Stream);
        }
        
        /// <summary>
        /// Tests that Read throws an InvalidOperationException when SkipUnknownEvents is enabled but no RecoverableReadError subscriber is set.
        /// </summary>
        [Fact]
        public void Read_WithoutRecoverableReadErrorSubscriber_ThrowsInvalidOperationException()
        {
            // Arrange
            // Prepare a stream that returns an EndOfFile record to trigger Read() logic.
            using var ms = new MemoryStream(_endOfFileRecordBytes);
            using var br = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            var reader = new BuildEventArgsReader(br, MinimalForwardCompatibleVersion);
            // Enable SkipUnknownEvents so that CheckErrorsSubscribed() is triggered.
            reader.SkipUnknownEvents = true;
            
            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => reader.Read());
            Assert.Contains("Binlog_MissingRecoverableErrorSubscribeError", ex.Message);
        }
    }
}
