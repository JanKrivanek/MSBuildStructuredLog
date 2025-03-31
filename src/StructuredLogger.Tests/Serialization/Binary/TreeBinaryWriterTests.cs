using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.IO;
using System.IO.Compression;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="TreeBinaryWriter"/> class.
    /// </summary>
    public class TreeBinaryWriterTests
    {
        /// <summary>
        /// Tests that calling Dispose without writing any additional data results in a file
        /// that contains the version bytes and a gzip-compressed stream with an empty string table.
        /// </summary>
        [Fact]
        public void Dispose_WhenNoData_WritesVersionAndEmptyGzipContent()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            try
            {
                // Act
                using (var writer = new TreeBinaryWriter(tempFile))
                {
                    // No additional writes.
                }

                // Assert
                using (var fs = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    // Read first three version bytes.
                    byte[] versionBytes = new byte[3];
                    fs.Read(versionBytes, 0, 3);
                    versionBytes.Should().Equal(new byte[] { 1, 2, 48 });

                    // The remainder of the file is a GZip stream.
                    using (var gzip = new GZipStream(fs, CompressionMode.Decompress))
                    using (var br = new BinaryReader(gzip))
                    {
                        // The string table count should be 0.
                        int stringTableCount = br.ReadInt32();
                        stringTableCount.Should().Be(0);

                        // There should be no further data from the tree nodes stream.
                        gzip.ReadByte().Should().Be(-1);
                    }
                }
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        /// <summary>
        /// Tests that WriteNode correctly writes the node data and updates the string table.
        /// </summary>
        [Fact]
        public void WriteNode_WhenCalled_WritesNodeData()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            string nodeName = "Node1";
            try
            {
                // Act
                using (var writer = new TreeBinaryWriter(tempFile))
                {
                    writer.WriteNode(nodeName);
                }

                // Assert
                using (var fs = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    // Version bytes.
                    byte[] versionBytes = new byte[3];
                    fs.Read(versionBytes, 0, 3);
                    versionBytes.Should().Equal(new byte[] { 1, 2, 48 });

                    using (var gzip = new GZipStream(fs, CompressionMode.Decompress))
                    using (var br = new BinaryReader(gzip))
                    {
                        // Read string table.
                        int stringTableCount = br.ReadInt32();
                        stringTableCount.Should().Be(1);
                        string tableEntry = br.ReadString();
                        tableEntry.Should().Be(nodeName);

                        // Read the tree node stream content.
                        // WriteNode writes the string index for the node.
                        int nodeIndex = br.ReadInt32();
                        // Since nodeName is the first non-null string, its index should be 1.
                        nodeIndex.Should().Be(1);

                        // There should be no additional data.
                        gzip.ReadByte().Should().Be(-1);
                    }
                }
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        /// <summary>
        /// Tests that WriteEndAttributes writes a zero count when no attributes have been added.
        /// </summary>
        [Fact]
        public void WriteEndAttributes_WhenNoAttributes_WritesZeroCount()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            string nodeName = "Node1";
            try
            {
                // Act
                using (var writer = new TreeBinaryWriter(tempFile))
                {
                    writer.WriteNode(nodeName);
                    writer.WriteEndAttributes();
                }

                // Assert
                using (var fs = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    // Read version bytes.
                    byte[] versionBytes = new byte[3];
                    fs.Read(versionBytes, 0, 3);
                    versionBytes.Should().Equal(new byte[] { 1, 2, 48 });

                    using (var gzip = new GZipStream(fs, CompressionMode.Decompress))
                    using (var br = new BinaryReader(gzip))
                    {
                        // String table: should contain one entry ("Node1")
                        int stringTableCount = br.ReadInt32();
                        stringTableCount.Should().Be(1);
                        br.ReadString().Should().Be(nodeName);

                        // Tree node stream:
                        // WriteNode writes an int representing the node's string index.
                        int nodeIndex = br.ReadInt32();
                        nodeIndex.Should().Be(1);
                        // WriteEndAttributes writes the count of attributes which should be 0.
                        int attributesCount = br.ReadInt32();
                        attributesCount.Should().Be(0);

                        // No extra data.
                        gzip.ReadByte().Should().Be(-1);
                    }
                }
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        /// <summary>
        /// Tests that WriteAttributeValue accumulates values and WriteEndAttributes writes the correct attribute data.
        /// </summary>
        [Fact]
        public void WriteAttributeValue_WhenCalled_AccumulatesAttributes()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            string nodeName = "Node1";
            string attr1 = "att1";
            string attr2 = "att2";
            try
            {
                // Act
                using (var writer = new TreeBinaryWriter(tempFile))
                {
                    writer.WriteNode(nodeName);
                    writer.WriteAttributeValue(attr1);
                    writer.WriteAttributeValue(attr2);
                    writer.WriteEndAttributes();
                }

                // Assert
                using (var fs = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    // Read version bytes.
                    byte[] versionBytes = new byte[3];
                    fs.Read(versionBytes, 0, 3);
                    versionBytes.Should().Equal(new byte[] { 1, 2, 48 });

                    using (var gzip = new GZipStream(fs, CompressionMode.Decompress))
                    using (var br = new BinaryReader(gzip))
                    {
                        // String table should have three entries.
                        int stringTableCount = br.ReadInt32();
                        stringTableCount.Should().Be(3);
                        // The order is by value index: nodeName (index 1), attr1 (index 2), attr2 (index 3)
                        string nodeEntry = br.ReadString();
                        string attrEntry1 = br.ReadString();
                        string attrEntry2 = br.ReadString();
                        nodeEntry.Should().Be(nodeName);
                        attrEntry1.Should().Be(attr1);
                        attrEntry2.Should().Be(attr2);

                        // Tree node stream:
                        // WriteNode: writes index for nodeName.
                        int nodeIndex = br.ReadInt32();
                        nodeIndex.Should().Be(1);
                        // WriteEndAttributes: writes count of attributes.
                        int attributesCount = br.ReadInt32();
                        attributesCount.Should().Be(2);
                        // Then writes attribute indices.
                        int attrIndex1 = br.ReadInt32();
                        int attrIndex2 = br.ReadInt32();
                        // Since attr1 was added first then attr2, expect indices 2 and 3.
                        attrIndex1.Should().Be(2);
                        attrIndex2.Should().Be(3);

                        // No extra data.
                        gzip.ReadByte().Should().Be(-1);
                    }
                }
            }
            finally
            {
                if(File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        /// <summary>
        /// Tests that WriteChildrenCount writes the given integer into the tree node stream.
        /// </summary>
        [Fact]
        public void WriteChildrenCount_WhenCalled_WritesChildrenCount()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            int childrenCount = 5;
            try
            {
                // Act
                using (var writer = new TreeBinaryWriter(tempFile))
                {
                    writer.WriteChildrenCount(childrenCount);
                }

                // Assert
                using (var fs = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    // Version bytes.
                    byte[] versionBytes = new byte[3];
                    fs.Read(versionBytes, 0, 3);
                    versionBytes.Should().Equal(new byte[] { 1, 2, 48 });

                    using (var gzip = new GZipStream(fs, CompressionMode.Decompress))
                    using (var br = new BinaryReader(gzip))
                    {
                        // Since no node or attribute methods were called, string table should be empty.
                        int stringTableCount = br.ReadInt32();
                        stringTableCount.Should().Be(0);

                        // Then the tree node stream has the children count.
                        int readChildrenCount = br.ReadInt32();
                        readChildrenCount.Should().Be(childrenCount);

                        // No extra data.
                        gzip.ReadByte().Should().Be(-1);
                    }
                }
            }
            finally
            {
                if(File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        /// <summary>
        /// Tests that WriteByteArray with a null input writes a length of zero.
        /// </summary>
        [Fact]
        public void WriteByteArray_WhenCalledWithNull_WritesZeroLength()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            try
            {
                // Act
                using (var writer = new TreeBinaryWriter(tempFile))
                {
                    writer.WriteByteArray(null);
                }
                // Assert
                using (var fs = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    // Skip version bytes.
                    fs.Seek(3, SeekOrigin.Begin);
                    using (var gzip = new GZipStream(fs, CompressionMode.Decompress))
                    using (var br = new BinaryReader(gzip))
                    {
                        // String table is empty.
                        int stringTableCount = br.ReadInt32();
                        stringTableCount.Should().Be(0);

                        // Tree node stream: WriteByteArray writes length of the byte array.
                        int byteArrayLength = br.ReadInt32();
                        byteArrayLength.Should().Be(0);

                        gzip.ReadByte().Should().Be(-1);
                    }
                }
            }
            finally
            {
                if(File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        /// <summary>
        /// Tests that WriteByteArray with an empty array writes a length of zero.
        /// </summary>
        [Fact]
        public void WriteByteArray_WhenCalledWithEmptyArray_WritesZeroLength()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            byte[] emptyArray = new byte[0];
            try
            {
                // Act
                using (var writer = new TreeBinaryWriter(tempFile))
                {
                    writer.WriteByteArray(emptyArray);
                }
                // Assert
                using (var fs = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    fs.Seek(3, SeekOrigin.Begin);
                    using (var gzip = new GZipStream(fs, CompressionMode.Decompress))
                    using (var br = new BinaryReader(gzip))
                    {
                        int stringTableCount = br.ReadInt32();
                        stringTableCount.Should().Be(0);

                        int byteArrayLength = br.ReadInt32();
                        byteArrayLength.Should().Be(0);

                        gzip.ReadByte().Should().Be(-1);
                    }
                }
            }
            finally
            {
                if(File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        /// <summary>
        /// Tests that WriteByteArray with a non-empty array writes the length and the byte data correctly.
        /// </summary>
        [Fact]
        public void WriteByteArray_WhenCalledWithNonEmptyArray_WritesLengthAndBytes()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            byte[] data = new byte[] { 10, 20, 30 };
            try
            {
                // Act
                using (var writer = new TreeBinaryWriter(tempFile))
                {
                    writer.WriteByteArray(data);
                }

                // Assert
                using (var fs = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    fs.Seek(3, SeekOrigin.Begin);
                    using (var gzip = new GZipStream(fs, CompressionMode.Decompress))
                    using (var br = new BinaryReader(gzip))
                    {
                        int stringTableCount = br.ReadInt32();
                        stringTableCount.Should().Be(0);

                        int byteArrayLength = br.ReadInt32();
                        byteArrayLength.Should().Be(data.Length);

                        byte[] readData = br.ReadBytes(byteArrayLength);
                        readData.Should().Equal(data);

                        gzip.ReadByte().Should().Be(-1);
                    }
                }
            }
            finally
            {
                if(File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        /// <summary>
        /// Tests that calling WriteNode clears any previously accumulated attributes.
        /// </summary>
        [Fact]
        public void WriteNode_ClearsPreviousAttributes()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            string nodeName = "Node1";
            string attributeBefore = "att1";
            try
            {
                // Act
                using (var writer = new TreeBinaryWriter(tempFile))
                {
                    // Add an attribute value.
                    writer.WriteAttributeValue(attributeBefore);
                    // Call WriteNode which should clear the attributes.
                    writer.WriteNode(nodeName);
                    writer.WriteEndAttributes();
                }

                // Assert
                using (var fs = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    fs.Seek(3, SeekOrigin.Begin);
                    using (var gzip = new GZipStream(fs, CompressionMode.Decompress))
                    using (var br = new BinaryReader(gzip))
                    {
                        // Only the node name should be in the string table, as the previous attribute should have been cleared.
                        int stringTableCount = br.ReadInt32();
                        stringTableCount.Should().Be(1);
                        string tableEntry = br.ReadString();
                        tableEntry.Should().Be(nodeName);

                        // Tree node stream: WriteNode writes index for nodeName.
                        int nodeIndex = br.ReadInt32();
                        nodeIndex.Should().Be(1);

                        // WriteEndAttributes should write 0 attributes.
                        int attributesCount = br.ReadInt32();
                        attributesCount.Should().Be(0);

                        gzip.ReadByte().Should().Be(-1);
                    }
                }
            }
            finally
            {
                if(File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        /// <summary>
        /// Tests that calling Dispose multiple times does not throw an exception.
        /// </summary>
        [Fact]
        public void Dispose_CalledMultipleTimes_DoesNotThrow()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            TreeBinaryWriter writer = null;
            try
            {
                writer = new TreeBinaryWriter(tempFile);
                // Act & Assert
                // First dispose.
                Action firstDispose = () => writer.Dispose();
                firstDispose.Should().NotThrow();

                // Second dispose should not throw.
                Action secondDispose = () => writer.Dispose();
                secondDispose.Should().NotThrow();
            }
            finally
            {
                writer?.Dispose();
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
    }
}
