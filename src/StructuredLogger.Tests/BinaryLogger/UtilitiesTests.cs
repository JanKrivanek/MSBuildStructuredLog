// // using System; // [Error] (4-7)CS0138 A 'using namespace' directive can only be applied to namespaces; 'Hashtable' is a type not a namespace. Consider a 'using static' directive instead
// // using System.Collections;
// // using System.Collections.Generic;
// // using System.Collections.Hashtable;
// // using System.IO;
// using System.Linq;
// using FluentAssertions;
// using Moq;
// using Xunit;
// 
// namespace Microsoft.Build.BackEnd.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Microsoft.Build.BackEnd.ItemGroupLoggingHelper"/> class.
//     /// </summary>
//     public class ItemGroupLoggingHelperTests
//     {
// //         /// <summary> // [Error] (26-82)CS0117 'TaskParameterMessageKind' does not contain a definition for 'Low'
// //         /// Tests that CreateTaskParameterEventArgs sets BuildEventContext, LineNumber and ColumnNumber correctly.
// //         /// </summary>
// //         [Fact]
// //         public void CreateTaskParameterEventArgs_HappyPath_SetsPropertiesCorrectly()
// //         {
// //             // Arrange
// //             var buildEventContext = new Microsoft.Build.Framework.BuildEventContext(10, 20, 30, 40, 50, 60, 70);
// //             var messageKind = Microsoft.Build.Framework.TaskParameterMessageKind.Low;
// //             string parameterName = "param";
// //             string propertyName = "prop";
// //             string itemType = "item";
// //             IList items = new ArrayList { "item1", "item2" };
// //             bool logItemMetadata = true;
// //             DateTime timestamp = DateTime.UtcNow;
// //             int line = 100;
// //             int column = 200;
// // 
// //             // Act
// //             var result = Microsoft.Build.BackEnd.ItemGroupLoggingHelper.CreateTaskParameterEventArgs(
// //                 buildEventContext,
// //                 messageKind,
// //                 parameterName,
// //                 propertyName,
// //                 itemType,
// //                 items,
// //                 logItemMetadata,
// //                 timestamp,
// //                 line,
// //                 column);
// // 
// //             // Assert
// //             result.Should().NotBeNull();
// //             result.BuildEventContext.Should().Be(buildEventContext);
// //             result.LineNumber.Should().Be(line);
// //             result.ColumnNumber.Should().Be(column);
// //         }
// //     }
// }
// 
// namespace Microsoft.Build.Internal.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Microsoft.Build.Internal.Utilities"/> class.
//     /// </summary>
//     public class UtilitiesTests
//     {
//         #region EnumerateProperties Tests
// 
//         /// <summary>
//         /// Tests that EnumerateProperties does nothing when the properties parameter is null.
//         /// </summary>
//         [Fact]
//         public void EnumerateProperties_NullProperties_NoCallbackInvoked()
//         {
//             // Arrange
//             bool callbackInvoked = false;
//             void Callback(KeyValuePair<string, string> kvp) => callbackInvoked = true;
//             
//             // Act
//             Microsoft.Build.Internal.Utilities.EnumerateProperties(null, Callback);
// 
//             // Assert
//             callbackInvoked.Should().BeFalse();
//         }
// 
//         /// <summary>
//         /// Tests that EnumerateProperties invokes callback for valid DictionaryEntry and KeyValuePair elements.
//         /// </summary>
//         [Fact]
//         public void EnumerateProperties_ValidEntries_CallbackInvokedForValidItems()
//         {
//             // Arrange
//             var list = new ArrayList();
//             // Valid DictionaryEntry with non-empty key
//             list.Add(new DictionaryEntry("key1", "value1"));
//             // DictionaryEntry with empty key should be ignored.
//             list.Add(new DictionaryEntry(string.Empty, "ignored"));
//             // Valid KeyValuePair
//             var kvp = new KeyValuePair<string, string>("key2", "value2");
//             list.Add(kvp);
//             var collected = new List<KeyValuePair<string, string>>();
// 
//             void Callback(KeyValuePair<string, string> pair)
//             {
//                 collected.Add(pair);
//             }
// 
//             // Act
//             Microsoft.Build.Internal.Utilities.EnumerateProperties(list, Callback);
// 
//             // Assert
//             collected.Should().HaveCount(2);
//             collected.Should().Contain(new KeyValuePair<string, string>("key1", "value1"));
//             collected.Should().Contain(new KeyValuePair<string, string>("key2", "value2"));
//         }
// 
//         #endregion
// 
//         #region EnumerateItems Tests
// 
//         /// <summary>
//         /// Tests that EnumerateItems invokes callback for DictionaryEntry items with a non-empty key.
//         /// </summary>
//         [Fact]
//         public void EnumerateItems_ValidEntries_CallbackInvokedForValidItems()
//         {
//             // Arrange
//             var list = new ArrayList();
//             // Valid DictionaryEntry with non-empty key
//             list.Add(new DictionaryEntry("itemType1", "itemValue1"));
//             // DictionaryEntry with null key should be skipped.
//             list.Add(new DictionaryEntry(null, "itemValue2"));
//             // DictionaryEntry with empty key should be skipped.
//             list.Add(new DictionaryEntry(string.Empty, "itemValue3"));
// 
//             var collected = new List<DictionaryEntry>();
// 
//             void Callback(DictionaryEntry entry)
//             {
//                 collected.Add(entry);
//             }
// 
//             // Act
//             Microsoft.Build.Internal.Utilities.EnumerateItems(list, Callback);
// 
//             // Assert
//             collected.Should().HaveCount(1);
//             collected[0].Key.Should().Be("itemType1");
//             collected[0].Value.Should().Be("itemValue1");
//         }
// 
//         #endregion
// 
//         #region EnumerateMetadata Tests
// 
//         /// <summary>
//         /// Tests that EnumerateMetadata returns the cloned metadata when the IDictionary supports IEnumerable of KeyValuePair.
//         /// </summary>
//         [Fact]
//         public void EnumerateMetadata_DictionaryBranch_ReturnsGenericEnumerable()
//         {
//             // Arrange
//             var metadata = new Dictionary<string, string>
//             {
//                 { "meta1", "value1" },
//                 { "meta2", "value2" }
//             };
//             var taskItemMock = new Mock<Microsoft.Build.Framework.ITaskItem>();
//             taskItemMock.Setup(t => t.CloneCustomMetadata()).Returns((IDictionary)metadata);
//             // Act
//             var result = taskItemMock.Object.EnumerateMetadata();
//             // Assert
//             result.Should().BeEquivalentTo(metadata);
//         }
// 
//         /// <summary>
//         /// Tests that EnumerateMetadata falls back to non-generic IDictionary when necessary and handles exceptions in GetMetadata.
//         /// </summary>
//         [Fact]
//         public void EnumerateMetadata_NonGenericBranch_HandlesExceptions()
//         {
//             // Arrange
//             // Use a Hashtable which is non-generic.
//             var metadata = new Hashtable
//             {
//                 { "meta1", "value1" },
//                 { "metaError", "dummy" }
//             };
// 
//             var taskItemMock = new Mock<Microsoft.Build.Framework.ITaskItem>();
//             taskItemMock.Setup(t => t.CloneCustomMetadata()).Returns(metadata);
//             // Setup GetMetadata to throw exception for key "metaError" and return normal value otherwise.
//             taskItemMock.Setup(t => t.GetMetadata("meta1")).Returns("value1");
//             taskItemMock.Setup(t => t.GetMetadata("metaError")).Throws(new Exception("metadata error"));
// 
//             // Act
//             var result = taskItemMock.Object.EnumerateMetadata().ToList();
// 
//             // Assert
//             result.Should().HaveCount(2);
//             result.Should().Contain(pair => pair.Key == "meta1" && pair.Value == "value1");
//             result.Should().Contain(pair => pair.Key == "metaError" && pair.Value == "metadata error");
//         }
// 
//         #endregion
// 
//         #region EqualTo Tests
// 
//         /// <summary>
//         /// Tests that EqualTo returns true when both BuildEventContext references are the same.
//         /// </summary>
//         [Fact]
//         public void EqualTo_SameInstance_ReturnsTrue()
//         {
//             // Arrange
//             var context = new Microsoft.Build.Framework.BuildEventContext(1, 2, 3, 4, 5, 6, 7);
//             
//             // Act
//             bool result = context.EqualTo(context);
// 
//             // Assert
//             result.Should().BeTrue();
//         }
// 
//         /// <summary>
//         /// Tests that EqualTo returns true when BuildEventContext properties are equal.
//         /// </summary>
//         [Fact]
//         public void EqualTo_EqualProperties_ReturnsTrue()
//         {
//             // Arrange
//             var context1 = new Microsoft.Build.Framework.BuildEventContext(10, 20, 30, 40, 50, 60, 70);
//             var context2 = new Microsoft.Build.Framework.BuildEventContext(10, 20, 30, 40, 50, 60, 70);
// 
//             // Act
//             bool result = context1.EqualTo(context2);
// 
//             // Assert
//             result.Should().BeTrue();
//         }
// 
//         /// <summary>
//         /// Tests that EqualTo returns false when one of the BuildEventContext parameters is null.
//         /// </summary>
//         [Fact]
//         public void EqualTo_OneNull_ReturnsFalse()
//         {
//             // Arrange
//             var context = new Microsoft.Build.Framework.BuildEventContext(10, 20, 30, 40, 50, 60, 70);
// 
//             // Act
//             bool result = context.EqualTo(null);
// 
//             // Assert
//             result.Should().BeFalse();
//         }
// 
//         /// <summary>
//         /// Tests that EqualTo returns false when BuildEventContext properties differ.
//         /// </summary>
//         [Fact]
//         public void EqualTo_DifferentProperties_ReturnsFalse()
//         {
//             // Arrange
//             var context1 = new Microsoft.Build.Framework.BuildEventContext(10, 20, 30, 40, 50, 60, 70);
//             var context2 = new Microsoft.Build.Framework.BuildEventContext(11, 20, 30, 40, 50, 60, 70);
// 
//             // Act
//             bool result = context1.EqualTo(context2);
// 
//             // Assert
//             result.Should().BeFalse();
//         }
// 
//         #endregion
//     }
// }
// 
// namespace Microsoft.Build.Shared.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the extension methods on BinaryWriter in <see cref="Microsoft.Build.Shared.BinaryWriterExtensions"/>.
//     /// </summary>
//     public class BinaryWriterExtensionsTests
//     {
//         #region WriteOptionalString Tests
// 
//         /// <summary>
//         /// Tests that WriteOptionalString writes a zero byte when the string is null.
//         /// </summary>
//         [Fact]
//         public void WriteOptionalString_NullValue_WritesZeroByte()
//         {
//             // Arrange
//             using var ms = new MemoryStream();
//             using var writer = new BinaryWriter(ms);
// 
//             // Act
//             writer.WriteOptionalString(null);
//             ms.Position = 0;
//             using var reader = new BinaryReader(ms);
//             byte indicator = reader.ReadByte();
// 
//             // Assert
//             indicator.Should().Be(0);
//         }
// 
//         /// <summary>
//         /// Tests that WriteOptionalString writes an indicator byte 1 and then the string when value is non-null.
//         /// </summary>
//         [Fact]
//         public void WriteOptionalString_NonNullValue_WritesIndicatorAndString()
//         {
//             // Arrange
//             const string testString = "Hello";
//             using var ms = new MemoryStream();
//             using var writer = new BinaryWriter(ms);
// 
//             // Act
//             writer.WriteOptionalString(testString);
//             ms.Position = 0;
//             using var reader = new BinaryReader(ms);
//             byte indicator = reader.ReadByte();
//             string result = reader.ReadString();
// 
//             // Assert
//             indicator.Should().Be(1);
//             result.Should().Be(testString);
//         }
// 
//         #endregion
// 
//         #region WriteTimestamp Tests
// 
//         /// <summary>
//         /// Tests that WriteTimestamp writes the correct ticks and DateTimeKind.
//         /// </summary>
//         [Fact]
//         public void WriteTimestamp_WritesTicksAndKind()
//         {
//             // Arrange
//             DateTime timestamp = new DateTime(2022, 1, 1, 12, 0, 0, DateTimeKind.Local);
//             using var ms = new MemoryStream();
//             using var writer = new BinaryWriter(ms);
// 
//             // Act
//             writer.WriteTimestamp(timestamp);
//             ms.Position = 0;
//             using var reader = new BinaryReader(ms);
//             long ticks = reader.ReadInt64();
//             int kind = reader.ReadInt32();
// 
//             // Assert
//             ticks.Should().Be(timestamp.Ticks);
//             kind.Should().Be((int)timestamp.Kind);
//         }
// 
//         #endregion
// 
//         #region Write7BitEncodedInt Tests
// 
//         /// <summary>
//         /// Tests that Write7BitEncodedInt writes the integer using 7-bit encoding.
//         /// </summary>
//         [Fact]
//         public void Write7BitEncodedInt_WritesCorrectEncoding()
//         {
//             // Arrange
//             int value = 123456;
//             using var ms = new MemoryStream();
//             using var writer = new BinaryWriter(ms);
// 
//             // Act
//             writer.Write7BitEncodedInt(value);
//             ms.Position = 0;
//             // Use the framework's Read7BitEncodedInt extension method from our BinaryReaderExtensions.
//             int result = Microsoft.Build.Shared.BinaryReaderExtensions.Read7BitEncodedInt(new BinaryReader(ms));
// 
//             // Assert
//             result.Should().Be(value);
//         }
// 
//         #endregion
// 
//         #region WriteOptionalBuildEventContext and WriteBuildEventContext Tests
// 
//         /// <summary>
//         /// Tests that WriteOptionalBuildEventContext writes zero indicator when context is null.
//         /// </summary>
//         [Fact]
//         public void WriteOptionalBuildEventContext_NullContext_WritesZero()
//         {
//             // Arrange
//             using var ms = new MemoryStream();
//             using var writer = new BinaryWriter(ms);
// 
//             // Act
//             writer.WriteOptionalBuildEventContext(null);
//             ms.Position = 0;
//             byte indicator = new BinaryReader(ms).ReadByte();
// 
//             // Assert
//             indicator.Should().Be(0);
//         }
// 
//         /// <summary>
//         /// Tests that WriteOptionalBuildEventContext writes one indicator and then the context data when context is provided.
//         /// </summary>
//         [Fact]
//         public void WriteOptionalBuildEventContext_NonNull_WritesOneAndContext()
//         {
//             // Arrange
//             var context = new Microsoft.Build.Framework.BuildEventContext(5, 6, 7, 8, 9, 10, 11);
//             using var ms = new MemoryStream();
//             using var writer = new BinaryWriter(ms);
// 
//             // Act
//             writer.WriteOptionalBuildEventContext(context);
//             ms.Position = 0;
//             using var reader = new BinaryReader(ms);
//             byte indicator = reader.ReadByte();
//             var readContext = reader.ReadBuildEventContext();
// 
//             // Assert
//             indicator.Should().Be(1);
//             readContext.TaskId.Should().Be(context.TaskId);
//             readContext.TargetId.Should().Be(context.TargetId);
//             readContext.ProjectContextId.Should().Be(context.ProjectContextId);
//             readContext.ProjectInstanceId.Should().Be(context.ProjectInstanceId);
//             readContext.NodeId.Should().Be(context.NodeId);
//             readContext.EvaluationId.Should().Be(context.EvaluationId);
//             readContext.SubmissionId.Should().Be(context.SubmissionId);
//         }
// 
//         /// <summary>
//         /// Tests that WriteBuildEventContext writes the context properties in the correct order.
//         /// </summary>
//         [Fact]
//         public void WriteBuildEventContext_WritesCorrectProperties()
//         {
//             // Arrange
//             var context = new Microsoft.Build.Framework.BuildEventContext(100, 200, 300, 400, 500, 600, 700);
//             using var ms = new MemoryStream();
//             using var writer = new BinaryWriter(ms);
// 
//             // Act
//             writer.WriteBuildEventContext(context);
//             ms.Position = 0;
//             using var reader = new BinaryReader(ms);
//             int nodeId = reader.ReadInt32();
//             int projectContextId = reader.ReadInt32();
//             int targetId = reader.ReadInt32();
//             int taskId = reader.ReadInt32();
//             int submissionId = reader.ReadInt32();
//             int projectInstanceId = reader.ReadInt32();
//             int evaluationId = reader.ReadInt32();
// 
//             // Assert
//             nodeId.Should().Be(context.NodeId);
//             projectContextId.Should().Be(context.ProjectContextId);
//             targetId.Should().Be(context.TargetId);
//             taskId.Should().Be(context.TaskId);
//             submissionId.Should().Be(context.SubmissionId);
//             projectInstanceId.Should().Be(context.ProjectInstanceId);
//             evaluationId.Should().Be(context.EvaluationId);
//         }
// 
//         #endregion
//     }
// 
//     /// <summary>
//     /// Unit tests for the extension methods on BinaryReader in <see cref="Microsoft.Build.Shared.BinaryReaderExtensions"/>.
//     /// </summary>
//     public class BinaryReaderExtensionsTests
//     {
//         #region ReadOptionalString Tests
// 
//         /// <summary>
//         /// Tests that ReadOptionalString returns null when the indicator byte is zero.
//         /// </summary>
//         [Fact]
//         public void ReadOptionalString_ZeroIndicator_ReturnsNull()
//         {
//             // Arrange
//             using var ms = new MemoryStream(new byte[] { 0 });
//             using var reader = new BinaryReader(ms);
// 
//             // Act
//             string result = reader.ReadOptionalString();
// 
//             // Assert
//             result.Should().BeNull();
//         }
// 
//         /// <summary>
//         /// Tests that ReadOptionalString returns the string when the indicator byte is one.
//         /// </summary>
//         [Fact]
//         public void ReadOptionalString_OneIndicator_ReturnsString()
//         {
//             // Arrange
//             const string expected = "TestString";
//             using var ms = new MemoryStream();
//             using (var writer = new BinaryWriter(ms))
//             {
//                 writer.Write((byte)1);
//                 writer.Write(expected);
//             }
//             ms.Position = 0;
//             using var reader = new BinaryReader(ms);
// 
//             // Act
//             string result = reader.ReadOptionalString();
// 
//             // Assert
//             result.Should().Be(expected);
//         }
// 
//         #endregion
// 
//         #region Read7BitEncodedInt Tests
// 
//         /// <summary>
//         /// Tests that Read7BitEncodedInt returns the correct integer value when the stream is correctly encoded.
//         /// </summary>
//         [Fact]
//         public void Read7BitEncodedInt_ValidEncoding_ReturnsCorrectValue()
//         {
//             // Arrange
//             int value = 98765;
//             using var ms = new MemoryStream();
//             using (var writer = new BinaryWriter(ms))
//             {
//                 writer.Write7BitEncodedInt(value);
//             }
//             ms.Position = 0;
//             using var reader = new BinaryReader(ms);
// 
//             // Act
//             int result = reader.Read7BitEncodedInt();
// 
//             // Assert
//             result.Should().Be(value);
//         }
// 
//         /// <summary>
//         /// Tests that Read7BitEncodedInt throws a FormatException if the 7-bit encoded integer is malformed.
//         /// </summary>
//         [Fact]
//         public void Read7BitEncodedInt_CorruptedStream_ThrowsFormatException()
//         {
//             // Arrange
//             // Create a stream with 5 bytes with the continuation bit set, causing an overflow.
//             byte[] corrupted = { 0x80, 0x80, 0x80, 0x80, 0x80 };
//             using var ms = new MemoryStream(corrupted);
//             using var reader = new BinaryReader(ms);
// 
//             // Act
//             Action action = () => reader.Read7BitEncodedInt();
// 
//             // Assert
//             action.Should().Throw<FormatException>();
//         }
// 
//         #endregion
// 
//         #region ReadTimestamp Tests
// 
//         /// <summary>
//         /// Tests that ReadTimestamp returns the correct DateTime based on ticks and kind.
//         /// </summary>
//         [Fact]
//         public void ReadTimestamp_ReturnsCorrectTimestamp()
//         {
//             // Arrange
//             DateTime expected = new DateTime(2021, 5, 15, 8, 30, 0, DateTimeKind.Utc);
//             using var ms = new MemoryStream();
//             using (var writer = new BinaryWriter(ms))
//             {
//                 writer.WriteTimestamp(expected);
//             }
//             ms.Position = 0;
//             using var reader = new BinaryReader(ms);
// 
//             // Act
//             DateTime result = reader.ReadTimestamp();
// 
//             // Assert
//             result.Should().Be(expected);
//         }
// 
//         #endregion
// 
//         #region ReadOptionalBuildEventContext and ReadBuildEventContext Tests
// 
//         /// <summary>
//         /// Tests that ReadOptionalBuildEventContext returns null when the indicator byte is zero.
//         /// </summary>
//         [Fact]
//         public void ReadOptionalBuildEventContext_ZeroIndicator_ReturnsNull()
//         {
//             // Arrange
//             using var ms = new MemoryStream(new byte[] { 0 });
//             using var reader = new BinaryReader(ms);
// 
//             // Act
//             var result = reader.ReadOptionalBuildEventContext();
// 
//             // Assert
//             result.Should().BeNull();
//         }
// 
//         /// <summary>
//         /// Tests that ReadOptionalBuildEventContext returns a valid BuildEventContext when the indicator byte is one.
//         /// </summary>
//         [Fact]
//         public void ReadOptionalBuildEventContext_OneIndicator_ReturnsContext()
//         {
//             // Arrange
//             var context = new Microsoft.Build.Framework.BuildEventContext(3, 4, 5, 6, 7, 8, 9);
//             using var ms = new MemoryStream();
//             using (var writer = new BinaryWriter(ms))
//             {
//                 writer.WriteOptionalBuildEventContext(context);
//             }
//             ms.Position = 0;
//             using var reader = new BinaryReader(ms);
// 
//             // Act
//             var result = reader.ReadOptionalBuildEventContext();
// 
//             // Assert
//             result.Should().NotBeNull();
//             result.TaskId.Should().Be(context.TaskId);
//             result.TargetId.Should().Be(context.TargetId);
//             result.ProjectContextId.Should().Be(context.ProjectContextId);
//             result.ProjectInstanceId.Should().Be(context.ProjectInstanceId);
//             result.NodeId.Should().Be(context.NodeId);
//             result.EvaluationId.Should().Be(context.EvaluationId);
//             result.SubmissionId.Should().Be(context.SubmissionId);
//         }
// 
//         /// <summary>
//         /// Tests that ReadBuildEventContext returns the correct BuildEventContext when read from the stream.
//         /// </summary>
//         [Fact]
//         public void ReadBuildEventContext_ReturnsCorrectContext()
//         {
//             // Arrange
//             var context = new Microsoft.Build.Framework.BuildEventContext(11, 22, 33, 44, 55, 66, 77);
//             using var ms = new MemoryStream();
//             using (var writer = new BinaryWriter(ms))
//             {
//                 writer.WriteBuildEventContext(context);
//             }
//             ms.Position = 0;
//             using var reader = new BinaryReader(ms);
// 
//             // Act
//             var result = reader.ReadBuildEventContext();
// 
//             // Assert
//             result.TaskId.Should().Be(context.TaskId);
//             result.TargetId.Should().Be(context.TargetId);
//             result.ProjectContextId.Should().Be(context.ProjectContextId);
//             result.ProjectInstanceId.Should().Be(context.ProjectInstanceId);
//             result.NodeId.Should().Be(context.NodeId);
//             result.EvaluationId.Should().Be(context.EvaluationId);
//             result.SubmissionId.Should().Be(context.SubmissionId);
//         }
// 
//         #endregion
//     }
// }