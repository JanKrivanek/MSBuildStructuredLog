using Microsoft.Build.Logging;
using System;
using Xunit;

namespace Microsoft.Build.Logging.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Record"/> class.
    /// </summary>
    public class RecordTests
    {
        private readonly Record _record;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordTests"/> class.
        /// </summary>
        public RecordTests()
        {
            _record = new Record();
        }

        /// <summary>
        /// Tests that field assignment in the <see cref="Record"/> class correctly stores and retrieves values.
        /// This test sets each public field of a Record instance and verifies that they are equal to the assigned values.
        /// </summary>
        [Fact]
        public void FieldAssignment_WhenAssignedValues_ShouldReturnExpectedValues()
        {
            // Arrange
            var expectedKind = BinaryLogRecordKind.ProjectEvaluation; // using a plausible enum value.
            byte[] expectedBytes = new byte[] { 1, 2, 3 };
            BuildEventArgs expectedArgs = new BuildMessageEventArgs("Test message", "HelpKeyword", "Sender", MessageImportance.High);
            long expectedStart = 100;
            long expectedLength = 50;

            // Act
            _record.Kind = expectedKind;
            _record.Bytes = expectedBytes;
            _record.Args = expectedArgs;
            _record.Start = expectedStart;
            _record.Length = expectedLength;

            // Assert
            Assert.Equal(expectedKind, _record.Kind);
            Assert.Equal(expectedBytes, _record.Bytes);
            Assert.Equal(expectedArgs, _record.Args);
            Assert.Equal(expectedStart, _record.Start);
            Assert.Equal(expectedLength, _record.Length);
        }

        /// <summary>
        /// Tests that the <see cref="Record"/> class fields correctly handle null assignments.
        /// This test assigns null to the reference type fields and verifies that they are null.
        /// </summary>
        [Fact]
        public void FieldAssignment_WhenAssignedNullValues_ShouldReturnNullValues()
        {
            // Arrange
            byte[] expectedBytes = null;
            BuildEventArgs expectedArgs = null;

            // Act
            _record.Bytes = expectedBytes;
            _record.Args = expectedArgs;

            // Assert
            Assert.Null(_record.Bytes);
            Assert.Null(_record.Args);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="RecordInfo"/> record struct.
    /// </summary>
    public class RecordInfoTests
    {
        /// <summary>
        /// Tests that the <see cref="RecordInfo"/> constructor initializes properties correctly.
        /// This test creates a RecordInfo instance with specific values and verifies that its properties match the expected values.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalledWithValidValues_ShouldInitializeProperties()
        {
            // Arrange
            var expectedKind = BinaryLogRecordKind.Target; // using a plausible enum value.
            long expectedStart = 200;
            long expectedLength = 100;

            // Act
            var recordInfo = new RecordInfo(expectedKind, expectedStart, expectedLength);

            // Assert
            Assert.Equal(expectedKind, recordInfo.Kind);
            Assert.Equal(expectedStart, recordInfo.Start);
            Assert.Equal(expectedLength, recordInfo.Length);
        }

        /// <summary>
        /// Tests the equality of two <see cref="RecordInfo"/> instances with identical values.
        /// This verifies that the value equality semantics of the record struct work as expected.
        /// </summary>
        [Fact]
        public void Equals_TwoIdenticalInstances_ShouldBeEqual()
        {
            // Arrange
            var expectedKind = BinaryLogRecordKind.Error; // using a plausible enum value.
            long expectedStart = 0;
            long expectedLength = 0;
            var recordInfo1 = new RecordInfo(expectedKind, expectedStart, expectedLength);
            var recordInfo2 = new RecordInfo(expectedKind, expectedStart, expectedLength);

            // Act & Assert
            Assert.Equal(recordInfo1, recordInfo2);
        }
    }
}
