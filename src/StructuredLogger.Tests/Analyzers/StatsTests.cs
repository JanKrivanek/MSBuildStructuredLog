using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BinlogStats"/> class.
    /// </summary>
    public class BinlogStatsTests
    {
        /// <summary>
        /// Tests the GetString method to ensure it returns a properly formatted string.
        /// </summary>
        [Fact]
        public void GetString_ValidInputs_ReturnsFormattedString()
        {
            // Arrange
            string name = "TestName";
            long total = 123456;
            int count = 10;
            int largest = 50;
            string expected = $"{name.PadRight(30, ' ')}\t\t\tTotal size: {total:N0}\t\t\tCount: {count:N0}\t\t\tLargest: {largest:N0}";

            // Act
            string result = BinlogStats.GetString(name, total, count, largest);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests the Calculate method when provided with a non-existing file path, expecting a FileNotFoundException.
        /// </summary>
        [Fact]
        public void Calculate_NonExistingFile_ThrowsFileNotFoundException()
        {
            // Arrange
            string nonExistingPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".binlog");

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => BinlogStats.Calculate(nonExistingPath));
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="BinlogStats.RecordsByType"/> class.
    /// </summary>
    public class RecordsByTypeTests
    {
        private readonly BinlogStats.RecordsByType _recordsByType;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordsByTypeTests"/> class.
        /// </summary>
        public RecordsByTypeTests()
        {
            _recordsByType = new BinlogStats.RecordsByType("TestType");
        }

        /// <summary>
        /// Tests the Add method with a null type, verifying that the record is added to the internal list.
        /// </summary>
        [Fact]
        public void Add_NullType_AddsRecordToList()
        {
            // Arrange
            long recordLength = 100;
            var fakeRecord = new FakeRecord(recordLength, null);
            int initialCount = _recordsByType.Count;

            // Act
            _recordsByType.Add(fakeRecord, null, null);

            // Assert
            Assert.Equal(initialCount + 1, _recordsByType.Count);
            Assert.Equal(recordLength, _recordsByType.TotalLength);
            Assert.Equal((int)recordLength, _recordsByType.Largest);
        }

        /// <summary>
        /// Tests the Seal method to ensure it sorts the records and sets the CategorizedRecords property.
        /// </summary>
        [Fact]
        public void Seal_WithRecords_SortsRecordsAndSetsCategorizedRecords()
        {
            // Arrange
            // Create multiple fake records with varying lengths.
            var record1 = new FakeRecord(50, new BuildMessageEventArgs("Message one", null, null));
            var record2 = new FakeRecord(100, new BuildMessageEventArgs("Longer message", null, null));
            var record3 = new FakeRecord(75, new BuildMessageEventArgs("Mid message", null, null));

            _recordsByType.Add(record1, null, null);
            _recordsByType.Add(record2, null, null);
            _recordsByType.Add(record3, null, null);

            // Act
            _recordsByType.Seal();

            // Assert
            Assert.NotNull(_recordsByType.CategorizedRecords);
            // Verify that the records in this bucket are sorted in descending order by length.
            List<Record> list = _recordsByType.Records.ToList();
            for (int i = 0; i < list.Count - 1; i++)
            {
                Assert.True(list[i].Length >= list[i + 1].Length);
            }
        }

        /// <summary>
        /// Tests the ToString method to ensure it returns the correctly formatted string representation.
        /// </summary>
        [Fact]
        public void ToString_ReturnsFormattedString()
        {
            // Arrange
            var recordsByType = new BinlogStats.RecordsByType("MyType");
            // Add a fake record to update internal properties.
            var fakeRecord = new FakeRecord(200, null);
            recordsByType.Add(fakeRecord, null, null);

            // Act
            string result = recordsByType.ToString();
            string expected = BinlogStats.GetString("MyType", recordsByType.TotalLength, recordsByType.Count, recordsByType.Largest);

            // Assert
            Assert.Equal(expected, result);
        }
    }

    /// <summary>
    /// A fake implementation of the Record class for unit testing purposes.
    /// </summary>
    public class FakeRecord : Record
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeRecord"/> class.
        /// </summary>
        /// <param name="length">The length of the record.</param>
        /// <param name="args">The BuildEventArgs associated with the record.</param>
        public FakeRecord(long length, BuildEventArgs args)
        {
            Length = length;
            Args = args;
        }

        /// <summary>
        /// Gets or sets the length of the record.
        /// </summary>
        public override long Length { get; set; }

        /// <summary>
        /// Gets or sets the BuildEventArgs associated with the record.
        /// </summary>
        public override BuildEventArgs Args { get; set; }
    }
}

namespace Microsoft.Build.Logging.StructuredLogger
{
    /// <summary>
    /// A minimal abstract representation of a Record for testing purposes.
    /// In production, Record is expected to have a more complete implementation.
    /// </summary>
    public abstract class Record
    {
        /// <summary>
        /// Gets or sets the length of the record.
        /// </summary>
        public abstract long Length { get; set; }

        /// <summary>
        /// Gets or sets the BuildEventArgs associated with the record.
        /// </summary>
        public abstract BuildEventArgs Args { get; set; }
    }
}
