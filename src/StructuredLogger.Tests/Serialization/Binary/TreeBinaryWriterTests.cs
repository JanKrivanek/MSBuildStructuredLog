using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TreeBinaryWriter"/> class.
    /// </summary>
    public class TreeBinaryWriterTests : IDisposable
    {
        // Temporary file used for testing.
        private readonly string _tempFilePath;

        public TreeBinaryWriterTests()
        {
            _tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".bin");
        }

        /// <summary>
        /// Cleans up the temporary file.
        /// </summary>
        public void Dispose()
        {
            if (File.Exists(_tempFilePath))
            {
                try
                {
                    File.Delete(_tempFilePath);
                }
                catch
                {
                    // ignore exceptions during cleanup
                }
            }
        }

        /// <summary>
        /// Helper method to read and decompress the output file.
        /// It skips the first 3 version bytes, decompresses the remainder using GZipStream,
        /// and extracts the string table and the remaining tree nodes data.
        /// </summary>
        /// <param name="filePath">Path to the file produced by TreeBinaryWriter.</param>
        /// <param name="stringTableCount">Output count of strings in the string table.</param>
        /// <param name="stringTable">Output list of strings from the string table.</param>
        /// <param name="treeNodesData">Output raw bytes of the tree nodes section.</param>
        private void ReadDecompressedFile(string filePath, out int stringTableCount, out List<string> stringTable, out byte[] treeNodesData)
        {
            stringTable = new List<string>();
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Read version bytes (first 3 bytes)
                byte[] versionBytes = new byte[3];
                int bytesRead = fs.Read(versionBytes, 0, 3);
                if (bytesRead != 3)
                {
                    throw new InvalidOperationException("File does not contain expected version bytes.");
                }

                using (GZipStream gzip = new GZipStream(fs, CompressionMode.Decompress))
                using (BinaryReader reader = new BinaryReader(gzip))
                {
                    // Read string table: first an integer count then that many strings.
                    stringTableCount = reader.ReadInt32();
                    for (int i = 0; i < stringTableCount; i++)
                    {
                        stringTable.Add(reader.ReadString());
                    }

                    // Read the remaining bytes as tree nodes data.
                    List<byte> nodesData = new List<byte>();
                    byte[] buffer = reader.ReadBytes(1024);
                    while (buffer.Length > 0)
                    {
                        nodesData.AddRange(buffer);
                        buffer = reader.ReadBytes(1024);
                    }
                    treeNodesData = nodesData.ToArray();
                }
            }
        }

        /// <summary>
        /// Tests that WriteNode writes the correct string index to the tree nodes stream and registers the string in the string table.
        /// Expected outcome: the string table contains one entry ("Test") and the tree nodes stream contains the integer index 1.
        /// </summary>
        [Fact]
        public void WriteNode_WithValidName_WritesCorrectly()
        {
            // Arrange
            using (var writer = new TreeBinaryWriter(_tempFilePath))
            {
                // Act
                writer.WriteNode("Test");
            }
            // Assert
            ReadDecompressedFile(_tempFilePath, out int tableCount, out List<string> table, out byte[] nodesData);

            Assert.Equal(1, tableCount);
            Assert.Single(table);
            Assert.Equal("Test", table.First());

            using (MemoryStream ms = new MemoryStream(nodesData))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                int nodeIndex = reader.ReadInt32();
                Assert.Equal(1, nodeIndex);
                // Ensure no extra data exists.
                Assert.Equal(0, ms.Position);
                Assert.Equal(ms.Length, ms.Position);
            }
        }

        /// <summary>
        /// Tests that WriteAttributeValue and WriteEndAttributes work as expected.
        /// Expected outcome: the string table registers both the node name and the attribute value, and the tree nodes stream contains the node index,
        /// followed by the count of attributes and then the attribute's string index.
        /// </summary>
        [Fact]
        public void WriteAttributeValueAndEndAttributes_WithValidInputs_WritesCorrectly()
        {
            // Arrange
            using (var writer = new TreeBinaryWriter(_tempFilePath))
            {
                writer.WriteNode("Node");
                writer.WriteAttributeValue("attr");
                writer.WriteEndAttributes();
            }
            // Act & Assert
            ReadDecompressedFile(_tempFilePath, out int tableCount, out List<string> table, out byte[] nodesData);

            // Expect two strings: "Node" with index 1 and "attr" with index 2.
            Assert.Equal(2, tableCount);
            Assert.Equal("Node", table[0]);
            Assert.Equal("attr", table[1]);

            using (MemoryStream ms = new MemoryStream(nodesData))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                // Tree nodes stream should first contain the node index.
                int nodeIndex = reader.ReadInt32();
                Assert.Equal(1, nodeIndex);

                // WriteEndAttributes writes attribute count then each attribute's string index.
                int attributesCount = reader.ReadInt32();
                Assert.Equal(1, attributesCount);

                int attrIndex = reader.ReadInt32();
                Assert.Equal(2, attrIndex);

                // Ensure no extra data exists.
                Assert.Equal(ms.Length, ms.Position);
            }
        }

        /// <summary>
        /// Tests that WriteChildrenCount writes the correct integer value to the tree nodes stream.
        /// Expected outcome: with no strings registered, the string table is empty and the tree nodes stream contains the integer representing children count.
        /// </summary>
        [Fact]
        public void WriteChildrenCount_WithValidCount_WritesCorrectly()
        {
            // Arrange
            using (var writer = new TreeBinaryWriter(_tempFilePath))
            {
                writer.WriteChildrenCount(5);
            }
            // Act & Assert
            ReadDecompressedFile(_tempFilePath, out int tableCount, out List<string> table, out byte[] nodesData);

            Assert.Equal(0, tableCount);
            Assert.Empty(table);

            using (MemoryStream ms = new MemoryStream(nodesData))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                int childrenCount = reader.ReadInt32();
                Assert.Equal(5, childrenCount);
                Assert.Equal(ms.Length, ms.Position);
            }
        }

        /// <summary>
        /// Tests that WriteByteArray handles a null input by writing a zero length.
        /// Expected outcome: the string table remains empty and the tree nodes stream contains an integer zero for the byte array length.
        /// </summary>
        [Fact]
        public void WriteByteArray_NullInput_WritesZeroLength()
        {
            // Arrange
            using (var writer = new TreeBinaryWriter(_tempFilePath))
            {
                writer.WriteByteArray(null);
            }
            // Act & Assert
            ReadDecompressedFile(_tempFilePath, out int tableCount, out List<string> table, out byte[] nodesData);

            Assert.Equal(0, tableCount);
            Assert.Empty(table);

            using (MemoryStream ms = new MemoryStream(nodesData))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                int length = reader.ReadInt32();
                Assert.Equal(0, length);
                Assert.Equal(ms.Length, ms.Position);
            }
        }

        /// <summary>
        /// Tests that WriteByteArray handles an empty array by writing a zero length.
        /// Expected outcome: the string table remains empty and the tree nodes stream contains an integer zero for the byte array length.
        /// </summary>
        [Fact]
        public void WriteByteArray_EmptyArray_WritesZeroLength()
        {
            // Arrange
            using (var writer = new TreeBinaryWriter(_tempFilePath))
            {
                writer.WriteByteArray(new byte[0]);
            }
            // Act & Assert
            ReadDecompressedFile(_tempFilePath, out int tableCount, out List<string> table, out byte[] nodesData);

            Assert.Equal(0, tableCount);
            Assert.Empty(table);

            using (MemoryStream ms = new MemoryStream(nodesData))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                int length = reader.ReadInt32();
                Assert.Equal(0, length);
                Assert.Equal(ms.Length, ms.Position);
            }
        }

        /// <summary>
        /// Tests that WriteByteArray correctly writes non-empty byte arrays.
        /// Expected outcome: the string table remains empty and the tree nodes stream contains the byte array length followed by the actual bytes.
        /// </summary>
        [Fact]
        public void WriteByteArray_NonEmptyArray_WritesCorrectData()
        {
            // Arrange
            byte[] dataToWrite = new byte[] { 1, 2, 3 };
            using (var writer = new TreeBinaryWriter(_tempFilePath))
            {
                writer.WriteByteArray(dataToWrite);
            }
            // Act & Assert
            ReadDecompressedFile(_tempFilePath, out int tableCount, out List<string> table, out byte[] nodesData);

            Assert.Equal(0, tableCount);
            Assert.Empty(table);

            using (MemoryStream ms = new MemoryStream(nodesData))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                int length = reader.ReadInt32();
                Assert.Equal(3, length);

                byte[] readBytes = reader.ReadBytes(length);
                Assert.Equal(dataToWrite, readBytes);

                Assert.Equal(ms.Length, ms.Position);
            }
        }
    }
}
