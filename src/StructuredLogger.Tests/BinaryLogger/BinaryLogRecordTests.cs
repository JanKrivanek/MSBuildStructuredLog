// using FluentAssertions;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Record"/> class.
//     /// </summary>
//     public class RecordTests
//     {
// //         /// <summary> // [Error] (22-52)CS0117 'BinaryLogRecordKind' does not contain a definition for 'Core'
// //         /// Tests that the <see cref="Record"/> class stores field values correctly after assignment.
// //         /// </summary>
// //         [Fact]
// //         public void Record_FieldAssignment_ShouldStoreValuesCorrectly()
// //         {
// //             // Arrange
// //             var expectedKind = BinaryLogRecordKind.Core;
// //             var expectedBytes = new byte[] { 1, 2, 3 };
// //             BuildEventArgs expectedArgs = new BuildMessageEventArgs("Test", "TestSubcategory", "TestCode", MessageImportance.High);
// //             long expectedStart = 10;
// //             long expectedLength = 20;
// // 
// //             var record = new Record();
// // 
// //             // Act
// //             record.Kind = expectedKind;
// //             record.Bytes = expectedBytes;
// //             record.Args = expectedArgs;
// //             record.Start = expectedStart;
// //             record.Length = expectedLength;
// // 
// //             // Assert
// //             record.Kind.Should().Be(expectedKind, "the Kind field should store the assigned value");
// //             record.Bytes.Should().Equal(expectedBytes, "the Bytes field should store the assigned array");
// //             record.Args.Should().Be(expectedArgs, "the Args field should store the assigned instance");
// //             record.Start.Should().Be(expectedStart, "the Start field should store the assigned value");
// //             record.Length.Should().Be(expectedLength, "the Length field should store the assigned value");
// //         }
// //     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="RecordInfo"/> record struct.
//     /// </summary>
//     public class RecordInfoTests
//     {
// //         private readonly BinaryLogRecordKind _sampleKind = BinaryLogRecordKind.Core; // [Error] (51-80)CS0117 'BinaryLogRecordKind' does not contain a definition for 'Core'
// //         private readonly long _sampleStart = 100;
//         private readonly long _sampleLength = 200;
// 
//         /// <summary>
//         /// Tests that two RecordInfo instances with identical field values are equal.
//         /// </summary>
//         [Fact]
//         public void Equals_WhenInstancesAreIdentical_ShouldBeEqual()
//         {
//             // Arrange
//             var recordInfo1 = new RecordInfo(_sampleKind, _sampleStart, _sampleLength);
//             var recordInfo2 = new RecordInfo(_sampleKind, _sampleStart, _sampleLength);
// 
//             // Act & Assert
//             recordInfo1.Should().Be(recordInfo2, "record structs with identical values should be equal");
//             (recordInfo1 == recordInfo2).Should().BeTrue("the equality operator should return true for identical values");
//         }
// //  // [Error] (78-66)CS0117 'BinaryLogRecordKind' does not contain a definition for 'Target'
// //         /// <summary>
// //         /// Tests that two RecordInfo instances with different field values are not equal.
// //         /// </summary>
// //         [Fact]
// //         public void Equals_WhenInstancesDiffer_ShouldNotBeEqual()
// //         {
// //             // Arrange
// //             var recordInfo1 = new RecordInfo(_sampleKind, _sampleStart, _sampleLength);
// //             var recordInfo2 = new RecordInfo(BinaryLogRecordKind.Target, _sampleStart, _sampleLength);
// // 
// //             // Act & Assert
// //             recordInfo1.Should().NotBe(recordInfo2, "record structs with different values should not be equal");
// //             (recordInfo1 == recordInfo2).Should().BeFalse("the equality operator should return false for different values");
// //         }
// // 
//         /// <summary>
//         /// Tests the GetHashCode method to ensure consistent hash codes for the same instance.
//         /// </summary>
//         [Fact]
//         public void GetHashCode_SameInstance_ShouldReturnConsistentHashCode()
//         {
//             // Arrange
//             var recordInfo = new RecordInfo(_sampleKind, _sampleStart, _sampleLength);
// 
//             // Act
//             int hashCode1 = recordInfo.GetHashCode();
//             int hashCode2 = recordInfo.GetHashCode();
// 
//             // Assert
//             hashCode1.Should().Be(hashCode2, "the hash code should be consistent across multiple invocations for the same instance");
//         }
// 
//         /// <summary>
//         /// Tests that ToString returns a non-empty string representing the RecordInfo instance.
//         /// </summary>
//         [Fact]
//         public void ToString_ShouldReturnNonEmptyString()
//         {
//             // Arrange
//             var recordInfo = new RecordInfo(_sampleKind, _sampleStart, _sampleLength);
// 
//             // Act
//             string result = recordInfo.ToString();
// 
//             // Assert
//             result.Should().NotBeNullOrWhiteSpace("ToString should return a meaningful string representation of the record struct");
//         }
//     }
// }