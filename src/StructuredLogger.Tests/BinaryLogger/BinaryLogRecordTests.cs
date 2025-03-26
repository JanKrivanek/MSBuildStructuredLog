using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Record"/> class.
    /// </summary>
    public class RecordTests
    {
        private readonly BinaryLogRecordKind _testKind;
        private readonly byte[] _testBytes;
        private readonly BuildEventArgs _testArgs;
        private readonly long _testStart;
        private readonly long _testLength;

        public RecordTests()
        {
            // Initialize test data. Assume BinaryLogRecordKind is an enum; use a specific value cast if necessary.
            _testKind = (BinaryLogRecordKind)1;
            _testBytes = new byte[] { 1, 2, 3 };
            _testArgs = null; // For simplicity, using null as BuildEventArgs; in a real test scenario, a proper instance could be provided.
            _testStart = 100;
            _testLength = 200;
        }

        /// <summary>
        /// Tests that the Record instance correctly holds assigned values in its public fields.
        /// This test creates a Record, sets its fields, and verifies the values are retained.
        /// </summary>
        [Fact]
        public void Record_Initialization_AssignsValuesCorrectly()
        {
            // Arrange
            var record = new Record();

            // Act
            record.Kind = _testKind;
            record.Bytes = _testBytes;
            record.Args = _testArgs;
            record.Start = _testStart;
            record.Length = _testLength;

            // Assert
            Assert.Equal(_testKind, record.Kind);
            Assert.Equal(_testBytes, record.Bytes);
            Assert.Equal(_testArgs, record.Args);
            Assert.Equal(_testStart, record.Start);
            Assert.Equal(_testLength, record.Length);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="RecordInfo"/> record struct.
    /// </summary>
    public class RecordInfoTests
    {
        private readonly BinaryLogRecordKind _testKind;
        private readonly long _testStart;
        private readonly long _testLength;

        public RecordInfoTests()
        {
            _testKind = (BinaryLogRecordKind)1;
            _testStart = 100;
            _testLength = 200;
        }

        /// <summary>
        /// Tests that the RecordInfo properties return the values provided during initialization.
        /// The test verifies that the record struct's properties are correctly set.
        /// </summary>
        [Fact]
        public void RecordInfo_Properties_ReturnCorrectValues()
        {
            // Act
            var recordInfo = new RecordInfo(_testKind, _testStart, _testLength);

            // Assert
            Assert.Equal(_testKind, recordInfo.Kind);
            Assert.Equal(_testStart, recordInfo.Start);
            Assert.Equal(_testLength, recordInfo.Length);
        }

        /// <summary>
        /// Tests that two RecordInfo instances with the same values are considered equal.
        /// This verifies the auto-generated equality members of the record struct.
        /// </summary>
        [Fact]
        public void RecordInfo_Equals_SameValues_ReturnsTrue()
        {
            // Arrange
            var recordInfo1 = new RecordInfo(_testKind, _testStart, _testLength);
            var recordInfo2 = new RecordInfo(_testKind, _testStart, _testLength);

            // Act & Assert
            Assert.Equal(recordInfo1, recordInfo2);
            Assert.True(recordInfo1.Equals(recordInfo2));
        }

        /// <summary>
        /// Tests that two RecordInfo instances with different values are not equal.
        /// This validates that the equality comparison distinguishes different state.
        /// </summary>
        [Fact]
        public void RecordInfo_Equals_DifferentValues_ReturnsFalse()
        {
            // Arrange
            var recordInfo1 = new RecordInfo(_testKind, _testStart, _testLength);
            var recordInfo2 = new RecordInfo((BinaryLogRecordKind)2, _testStart + 1, _testLength + 1);

            // Act & Assert
            Assert.NotEqual(recordInfo1, recordInfo2);
            Assert.False(recordInfo1.Equals(recordInfo2));
        }

        /// <summary>
        /// Tests that GetHashCode returns the same value for two RecordInfo instances with identical state.
        /// This ensures consistency of the auto-generated hash code method.
        /// </summary>
        [Fact]
        public void RecordInfo_GetHashCode_SameValues_ReturnSameHashCode()
        {
            // Arrange
            var recordInfo1 = new RecordInfo(_testKind, _testStart, _testLength);
            var recordInfo2 = new RecordInfo(_testKind, _testStart, _testLength);

            // Act
            int hash1 = recordInfo1.GetHashCode();
            int hash2 = recordInfo2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        /// <summary>
        /// Tests that the ToString method of RecordInfo returns a non-empty string.
        /// This verifies that the auto-generated ToString method produces output.
        /// </summary>
        [Fact]
        public void RecordInfo_ToString_ReturnsNonEmptyString()
        {
            // Arrange
            var recordInfo = new RecordInfo(_testKind, _testStart, _testLength);

            // Act
            string toStringResult = recordInfo.ToString();

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(toStringResult));
        }
    }
}
