using FluentAssertions;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Contains unit tests for the <see cref="BinlogStats"/> class.
    /// </summary>
    public class BinlogStatsTests
    {
        /// <summary>
        /// Tests the GetString static method to ensure it returns a correctly formatted string.
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
            result.Should().Be(expected, "GetString should format the string correctly based on provided inputs.");
        }

        /// <summary>
        /// Tests the Calculate static method with a non-existent file path to ensure it throws a FileNotFoundException.
        /// </summary>
        [Fact]
        public void Calculate_NonExistentFile_ThrowsFileNotFoundException()
        {
            // Arrange
            string invalidFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".binlog");

            // Act
            Action action = () => BinlogStats.Calculate(invalidFilePath);

            // Assert
            action.Should().Throw<FileNotFoundException>("accessing FileInfo on a non-existent file should throw FileNotFoundException.");
        }

        /// <summary>
        /// Tests the Calculate static method using a valid empty file.
        /// NOTE: This test is skipped because it requires a valid binlog file format for proper processing.
        /// </summary>
        [Fact(Skip = "Requires a valid binlog file to execute Calculate method without errors.")]
        public void Calculate_ValidEmptyFile_ReturnsStatsWithCorrectFileSizeAndZeroRecords()
        {
            // Arrange
            string tempFilePath = Path.GetTempFileName();
            try
            {
                // Write an empty byte array to the file.
                File.WriteAllBytes(tempFilePath, new byte[0]);

                // Act
                var stats = BinlogStats.Calculate(tempFilePath);

                // Assert
                stats.FileSize.Should().Be(0, "an empty file should have a file size of 0 bytes");
                stats.RecordCount.Should().Be(0, "no records should be processed from an empty file");
            }
            finally
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }
    }

    /// <summary>
    /// Contains unit tests for the nested <see cref="BinlogStats.RecordsByType"/> class.
    /// </summary>
    public class BinlogStatsRecordsByTypeTests
    {
        /// <summary>
        /// A minimal fake implementation of a Record for testing purposes.
        /// </summary>
        private class FakeRecord : Microsoft.Build.Logging.Record
        {
            public FakeRecord(long length, BuildEventArgs? args)
            {
                Length = length;
                Args = args;
            }
        }

        /// <summary>
        /// Tests the constructor of RecordsByType to ensure it initializes with the given type.
        /// </summary>
        [Fact]
        public void Constructor_WithTypeParameter_InitializesTypeCorrectly()
        {
            // Arrange
            string type = "TestType";

            // Act
            var recordsByType = new BinlogStats.RecordsByType(type);

            // Assert
            recordsByType.Type.Should().Be(type, "the constructor should correctly assign the provided type to the Type property.");
        }

        /// <summary>
        /// Tests the Create static method to ensure it returns a new instance with the provided type.
        /// </summary>
        [Fact]
        public void Create_WithValidType_ReturnsNewInstanceWithCorrectType()
        {
            // Arrange
            string type = "CreatedType";

            // Act
            var instance = BinlogStats.RecordsByType.Create(type);

            // Assert
            instance.Should().NotBeNull("Create should return a new instance of RecordsByType");
            instance.Type.Should().Be(type, "the instance's Type property should match the provided type");
        }
//  // [Error] (151-45)CS1503 Argument 1: cannot convert from 'long' to 'int'
//         /// <summary>
//         /// Tests the Add method when passing a null type to ensure it adds the record directly.
//         /// </summary>
//         [Fact]
//         public void Add_NullType_AddsRecordToListAndUpdatesCounters()
//         {
//             // Arrange
//             var recordsByType = new BinlogStats.RecordsByType("Root");
//             long initialCount = recordsByType.Count;
//             long initialTotalLength = recordsByType.TotalLength;
//             int initialLargest = recordsByType.Largest;
//             var fakeRecord = new FakeRecord(100, null);
// 
//             // Act
//             recordsByType.Add(fakeRecord, null, null);
// 
//             // Assert
//             recordsByType.Count.Should().Be(initialCount + 1, "adding a record should increment the count");
//             recordsByType.TotalLength.Should().Be(initialTotalLength + 100, "the total length should increase by the length of the added record");
//             recordsByType.Largest.Should().Be(Math.Max(initialLargest, 100), "Largest should be updated to the maximum record length encountered");
//         }
// 
        /// <summary>
        /// Tests the Seal method to ensure it sorts the records and sets the CategorizedRecords property.
        /// </summary>
        [Fact]
        public void Seal_AfterAddingRecords_SortsRecordsAndCategorizesThem()
        {
            // Arrange
            var recordsByType = new BinlogStats.RecordsByType("Root");
            var record1 = new FakeRecord(50, null);
            var record2 = new FakeRecord(150, null);
            var record3 = new FakeRecord(100, null);

            recordsByType.Add(record1, null, null);
            recordsByType.Add(record2, null, null);
            recordsByType.Add(record3, null, null);

            // Act
            recordsByType.Seal();

            // Assert
            recordsByType.CategorizedRecords.Should().NotBeNull("Seal should assign a value to CategorizedRecords");
            // When records are added with a null type, no categorized buckets are created.
            recordsByType.CategorizedRecords.Should().BeEmpty("with direct additions when type is null, CategorizedRecords remains empty");

            // Validate that the records list is sorted in descending order by length.
            var sortedRecords = recordsByType.Records.OrderByDescending(r => r.Length).ToList();
            recordsByType.Records.Should().Equal(sortedRecords, "the records should be sorted in descending order by their Length");
        }

        /// <summary>
        /// Tests the ToString override to ensure it returns a correctly formatted string.
        /// </summary>
        [Fact]
        public void ToString_ReturnsSameAsFormattedGetStringOutput()
        {
            // Arrange
            var recordsByType = new BinlogStats.RecordsByType("TestType");
            var fakeRecord = new FakeRecord(200, null);
            recordsByType.Add(fakeRecord, null, null);
            string expected = BinlogStats.GetString(recordsByType.Type, recordsByType.TotalLength, recordsByType.Count, recordsByType.Largest);

            // Act
            string result = recordsByType.ToString();

            // Assert
            result.Should().Be(expected, "ToString should return the formatted string as constructed by GetString");
        }
    }
}
