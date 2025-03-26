using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Build.BackEnd;
using Microsoft.Build.Framework;
using Microsoft.Build.Internal;
using Microsoft.Build.Shared;
using Moq;
using Xunit;

namespace Microsoft.Build.BackEnd.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ItemGroupLoggingHelper"/> class.
    /// </summary>
    public class ItemGroupLoggingHelperTests
    {
        private readonly BuildEventContext _buildEventContext;
        private readonly IList _items;

        public ItemGroupLoggingHelperTests()
        {
            // Initialize with some dummy test values.
            _buildEventContext = new BuildEventContext(10, 20, 30, 40, 50, 60, 70);
            _items = new List<object>();
        }

        /// <summary>
        /// Tests that CreateTaskParameterEventArgs correctly assigns the provided values.
        /// </summary>
        [Fact]
        public void CreateTaskParameterEventArgs_HappyPath_SetsPropertiesCorrectly()
        {
            // Arrange
            TaskParameterMessageKind messageKind = TaskParameterMessageKind.Warning;
            string parameterName = "parameterName";
            string propertyName = "propertyName";
            string itemType = "itemType";
            bool logItemMetadata = true;
            DateTime timestamp = DateTime.Now;
            int line = 100;
            int column = 200;

            // Act
            var result = ItemGroupLoggingHelper.CreateTaskParameterEventArgs(
                _buildEventContext,
                messageKind,
                parameterName,
                propertyName,
                itemType,
                _items,
                logItemMetadata,
                timestamp,
                line,
                column);

            // Assert
            // Using reflection to access properties: BuildEventContext, LineNumber, ColumnNumber.
            // Since TaskParameterEventArgs2 is internal and the type of result is not exposed, we use dynamic.
            dynamic dynamicResult = result;
            Assert.Equal(_buildEventContext, (BuildEventContext)dynamicResult.BuildEventContext);
            Assert.Equal(line, (int)dynamicResult.LineNumber);
            Assert.Equal(column, (int)dynamicResult.ColumnNumber);
        }
    }
}

namespace Microsoft.Build.Internal.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Utilities"/> class.
    /// </summary>
    public class UtilitiesTests
    {
        public UtilitiesTests()
        {
        }

        /// <summary>
        /// Tests that EnumerateProperties does not invoke the callback when properties is null.
        /// </summary>
        [Fact]
        public void EnumerateProperties_NullProperties_NoCallbackInvoked()
        {
            // Arrange
            IEnumerable properties = null;
            bool callbackInvoked = false;
            Action<KeyValuePair<string, string>> callback = kvp => callbackInvoked = true;

            // Act
            Utilities.EnumerateProperties(properties, callback);

            // Assert
            Assert.False(callbackInvoked);
        }

        /// <summary>
        /// Tests that EnumerateProperties correctly invokes the callback for valid dictionary entries and KeyValuePair.
        /// </summary>
        [Fact]
        public void EnumerateProperties_ValidProperties_CallbackInvokedForValidEntries()
        {
            // Arrange
            var list = new ArrayList();
            // Add a valid DictionaryEntry with non-empty key.
            list.Add(new DictionaryEntry("Key1", "Value1"));
            // Add an invalid item.
            list.Add("Invalid");
            // Add a valid KeyValuePair
            list.Add(new KeyValuePair<string, string>("Key2", "Value2"));

            var callbackResults = new List<KeyValuePair<string, string>>();
            Action<KeyValuePair<string, string>> callback = kvp => callbackResults.Add(kvp);

            // Act
            Utilities.EnumerateProperties(list, callback);

            // Assert
            Assert.Equal(2, callbackResults.Count);
            Assert.Contains(callbackResults, kvp => kvp.Key == "Key1" && kvp.Value == "Value1");
            Assert.Contains(callbackResults, kvp => kvp.Key == "Key2" && kvp.Value == "Value2");
        }

        /// <summary>
        /// Tests that EnumerateItems only invokes the callback for dictionary entries with non-null/empty keys.
        /// </summary>
        [Fact]
        public void EnumerateItems_ValidAndInvalidItems_CallbackInvokedOnlyForValidEntries()
        {
            // Arrange
            var list = new ArrayList();
            // Valid dictionary entry.
            list.Add(new DictionaryEntry("ItemType1", "ItemValue1"));
            // Invalid dictionary entry: empty key.
            list.Add(new DictionaryEntry("", "ItemValue2"));
            // Invalid type
            list.Add(123);

            var callbackResults = new List<DictionaryEntry>();
            Action<DictionaryEntry> callback = de => callbackResults.Add(de);

            // Act
            Utilities.EnumerateItems(list, callback);

            // Assert
            Assert.Single(callbackResults);
            Assert.Equal("ItemType1", callbackResults[0].Key);
            Assert.Equal("ItemValue1", callbackResults[0].Value);
        }

        /// <summary>
        /// Tests the EnumerateMetadata extension method when CloneCustomMetadata returns an IEnumerable of KeyValuePair.
        /// </summary>
        [Fact]
        public void EnumerateMetadata_CustomMetadataImplementsIEnumerable_ReturnsEnumeratedMetadata()
        {
            // Arrange
            var metadata = new Dictionary<string, string>
            {
                { "Meta1", "Value1" },
                { "Meta2", "Value2" }
            };

            var mockTaskItem = new Mock<ITaskItem>();
            // Setup CloneCustomMetadata to return a dictionary that implements IEnumerable<KeyValuePair<string, string>>
            mockTaskItem.Setup(t => t.CloneCustomMetadata()).Returns((IDictionary)metadata);

            // Act
            var result = mockTaskItem.Object.EnumerateMetadata().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, kvp => kvp.Key == "Meta1" && kvp.Value == "Value1");
            Assert.Contains(result, kvp => kvp.Key == "Meta2" && kvp.Value == "Value2");
        }

        /// <summary>
        /// Tests the EnumerateMetadata extension method when CloneCustomMetadata returns a non-generic IDictionary.
        /// Also tests fallback when GetMetadata works correctly.
        /// </summary>
        [Fact]
        public void EnumerateMetadata_NonGenericDictionary_ReturnsMetadataUsingFallback()
        {
            // Arrange
            var customMetadata = new Hashtable();
            customMetadata["Meta1"] = "Value1";
            customMetadata["Meta2"] = "Value2";

            var mockTaskItem = new Mock<ITaskItem>();
            mockTaskItem.Setup(t => t.CloneCustomMetadata()).Returns(customMetadata);
            // Setup GetMetadata to simply return the value stored in the hashtable.
            mockTaskItem.Setup(t => t.GetMetadata(It.IsAny<string>()))
                .Returns((string key) => customMetadata[key]?.ToString());

            // Act
            var result = mockTaskItem.Object.EnumerateMetadata().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, kvp => kvp.Key == "Meta1" && kvp.Value == "Value1");
            Assert.Contains(result, kvp => kvp.Key == "Meta2" && kvp.Value == "Value2");
        }

        /// <summary>
        /// Tests the EnumerateMetadata extension method fallback when GetMetadata throws an exception.
        /// </summary>
        [Fact]
        public void EnumerateMetadata_GetMetadataThrowsException_ReturnsExceptionMessageAsValue()
        {
            // Arrange
            var customMetadata = new Hashtable();
            customMetadata["Meta1"] = "Value1";
            customMetadata["MetaError"] = "AnyValue";

            var expectedErrorMessage = "Test exception";
            var mockTaskItem = new Mock<ITaskItem>();
            mockTaskItem.Setup(t => t.CloneCustomMetadata()).Returns(customMetadata);
            // For Meta1, return value normally.
            mockTaskItem.Setup(t => t.GetMetadata("Meta1")).Returns("Value1");
            // For MetaError, throw exception.
            mockTaskItem.Setup(t => t.GetMetadata("MetaError")).Throws(new Exception(expectedErrorMessage));

            // Act
            var result = mockTaskItem.Object.EnumerateMetadata().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, kvp => kvp.Key == "Meta1" && kvp.Value == "Value1");
            Assert.Contains(result, kvp => kvp.Key == "MetaError" && kvp.Value == expectedErrorMessage);
        }

        /// <summary>
        /// Tests the EqualTo extension method for BuildEventContext when both contexts are the same reference.
        /// </summary>
        [Fact]
        public void EqualTo_SameReference_ReturnsTrue()
        {
            // Arrange
            var context = new BuildEventContext(1, 2, 3, 4, 5, 6, 7);

            // Act
            bool result = context.EqualTo(context);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests the EqualTo extension method for BuildEventContext when contexts are equal in all properties.
        /// </summary>
        [Fact]
        public void EqualTo_EqualContexts_ReturnsTrue()
        {
            // Arrange
            var context1 = new BuildEventContext(1, 2, 3, 4, 5, 6, 7);
            var context2 = new BuildEventContext(1, 2, 3, 4, 5, 6, 7);

            // Act
            bool result = context1.EqualTo(context2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests the EqualTo extension method for BuildEventContext when contexts differ.
        /// </summary>
        [Fact]
        public void EqualTo_DifferentContexts_ReturnsFalse()
        {
            // Arrange
            var context1 = new BuildEventContext(1, 2, 3, 4, 5, 6, 7);
            var context2 = new BuildEventContext(10, 20, 30, 40, 50, 60, 70);

            // Act
            bool result = context1.EqualTo(context2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Tests the EqualTo extension method when one context is null.
        /// </summary>
        [Fact]
        public void EqualTo_OneContextNull_ReturnsFalse()
        {
            // Arrange
            BuildEventContext context1 = new BuildEventContext(1, 2, 3, 4, 5, 6, 7);
            BuildEventContext context2 = null;

            // Act
            bool result = context1.EqualTo(context2);

            // Assert
            Assert.False(result);
        }
    }
}

namespace Microsoft.Build.Shared.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BinaryWriterExtensions"/> class.
    /// </summary>
    public class BinaryWriterExtensionsTests
    {
        public BinaryWriterExtensionsTests()
        {
        }

        /// <summary>
        /// Tests the WriteOptionalString extension method by verifying that a null string writes a leading zero.
        /// </summary>
        [Fact]
        public void WriteOptionalString_NullValue_WritesZeroByte()
        {
            // Arrange
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            writer.WriteOptionalString(null);
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            byte flag = reader.ReadByte();

            // Assert
            Assert.Equal(0, flag);
        }

        /// <summary>
        /// Tests the WriteOptionalString extension method by verifying that a non-null string writes a leading one and the string correctly.
        /// </summary>
        [Fact]
        public void WriteOptionalString_NonNullValue_WritesOneByteAndString()
        {
            // Arrange
            string testString = "Test";
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            writer.WriteOptionalString(testString);
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            byte flag = reader.ReadByte();
            string result = reader.ReadString();

            // Assert
            Assert.Equal(1, flag);
            Assert.Equal(testString, result);
        }

        /// <summary>
        /// Tests the WriteTimestamp extension method by writing a DateTime value and reading back its components.
        /// </summary>
        [Fact]
        public void WriteTimestamp_WritesCorrectTicksAndKind()
        {
            // Arrange
            DateTime timestamp = new DateTime(2022, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            writer.WriteTimestamp(timestamp);
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            long ticks = reader.ReadInt64();
            int kindValue = reader.ReadInt32();
            DateTimeKind kind = (DateTimeKind)kindValue;

            // Assert
            Assert.Equal(timestamp.Ticks, ticks);
            Assert.Equal(timestamp.Kind, kind);
        }

        /// <summary>
        /// Tests the Write7BitEncodedInt extension method by writing an integer and verifying the encoding.
        /// </summary>
        [Fact]
        public void Write7BitEncodedInt_WritesEncodedIntegerCorrectly()
        {
            // Arrange
            int value = 123456;
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            writer.Write7BitEncodedInt(value);
            writer.Flush();
            ms.Position = 0;
            // Read the encoded int manually using the corresponding BinaryReaderExtensions method.
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            int decodedValue = reader.Read7BitEncodedInt();

            // Assert
            Assert.Equal(value, decodedValue);
        }

        /// <summary>
        /// Tests the WriteOptionalBuildEventContext extension by verifying that a null context writes a zero byte.
        /// </summary>
        [Fact]
        public void WriteOptionalBuildEventContext_NullContext_WritesZeroByte()
        {
            // Arrange
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            writer.WriteOptionalBuildEventContext(null);
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            byte flag = reader.ReadByte();

            // Assert
            Assert.Equal(0, flag);
        }

        /// <summary>
        /// Tests the WriteOptionalBuildEventContext extension by verifying that a non-null context is correctly written.
        /// </summary>
        [Fact]
        public void WriteOptionalBuildEventContext_NonNullContext_WritesContextData()
        {
            // Arrange
            var context = new BuildEventContext(5, 10, 15, 20, 25, 30, 35);
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            writer.WriteOptionalBuildEventContext(context);
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            byte flag = reader.ReadByte();

            // Assert
            Assert.Equal(1, flag);
            // Use the BinaryReaderExtensions to read the context back.
            var readContext = reader.ReadBuildEventContext();
            Assert.Equal(context.NodeId, readContext.NodeId);
            Assert.Equal(context.ProjectContextId, readContext.ProjectContextId);
            Assert.Equal(context.TargetId, readContext.TargetId);
            Assert.Equal(context.TaskId, readContext.TaskId);
            Assert.Equal(context.SubmissionId, readContext.SubmissionId);
            Assert.Equal(context.ProjectInstanceId, readContext.ProjectInstanceId);
            Assert.Equal(context.EvaluationId, readContext.EvaluationId);
        }

        /// <summary>
        /// Tests the WriteBuildEventContext extension by writing a context and reading it back to verify field order.
        /// </summary>
        [Fact]
        public void WriteBuildEventContext_WritesContextFieldsInOrder()
        {
            // Arrange
            var context = new BuildEventContext(11, 22, 33, 44, 55, 66, 77);
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            writer.WriteBuildEventContext(context);
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            int nodeId = reader.ReadInt32();
            int projectContextId = reader.ReadInt32();
            int targetId = reader.ReadInt32();
            int taskId = reader.ReadInt32();
            int submissionId = reader.ReadInt32();
            int projectInstanceId = reader.ReadInt32();
            int evaluationId = reader.ReadInt32();

            // Assert
            Assert.Equal(context.NodeId, nodeId);
            Assert.Equal(context.ProjectContextId, projectContextId);
            Assert.Equal(context.TargetId, targetId);
            Assert.Equal(context.TaskId, taskId);
            Assert.Equal(context.SubmissionId, submissionId);
            Assert.Equal(context.ProjectInstanceId, projectInstanceId);
            Assert.Equal(context.EvaluationId, evaluationId);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="BinaryReaderExtensions"/> class.
    /// </summary>
    public class BinaryReaderExtensionsTests
    {
        public BinaryReaderExtensionsTests()
        {
        }

        /// <summary>
        /// Tests the ReadOptionalString extension method when the value is null.
        /// </summary>
        [Fact]
        public void ReadOptionalString_NullValue_ReturnsNull()
        {
            // Arrange
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
            writer.Write((byte)0); // indicates null
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            string result = reader.ReadOptionalString();

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests the ReadOptionalString extension method when a valid non-null string is present.
        /// </summary>
        [Fact]
        public void ReadOptionalString_NonNullValue_ReturnsString()
        {
            // Arrange
            string expected = "Hello";
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
            writer.Write((byte)1);
            writer.Write(expected);
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            string result = reader.ReadOptionalString();

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that Read7BitEncodedInt correctly decodes the integer value.
        /// </summary>
        [Fact]
        public void Read7BitEncodedInt_WritesAndReadsInteger_CorrectValue()
        {
            // Arrange
            int expected = 98765;
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
            writer.Write7BitEncodedInt(expected);
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            int actual = reader.Read7BitEncodedInt();

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Tests the ReadTimestamp extension method by writing a DateTime value and reading it back.
        /// </summary>
        [Fact]
        public void ReadTimestamp_WritesAndReadsDateTime_CorrectValue()
        {
            // Arrange
            DateTime timestamp = new DateTime(2023, 6, 15, 8, 30, 0, DateTimeKind.Local);
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
            writer.WriteTimestamp(timestamp);
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            DateTime result = reader.ReadTimestamp();

            // Assert
            Assert.Equal(timestamp.Ticks, result.Ticks);
            Assert.Equal(timestamp.Kind, result.Kind);
        }

        /// <summary>
        /// Tests the ReadOptionalBuildEventContext extension method when a null context was written.
        /// </summary>
        [Fact]
        public void ReadOptionalBuildEventContext_NullContext_ReturnsNull()
        {
            // Arrange
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
            writer.Write((byte)0);
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            BuildEventContext result = reader.ReadOptionalBuildEventContext();

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests the ReadOptionalBuildEventContext extension method when a non-null context was written.
        /// </summary>
        [Fact]
        public void ReadOptionalBuildEventContext_NonNullContext_ReturnsContext()
        {
            // Arrange
            var context = new BuildEventContext(3, 6, 9, 12, 15, 18, 21);
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
            writer.WriteOptionalBuildEventContext(context);
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);
            // First byte has already been read in ReadOptionalBuildEventContext method.

            // Act
            BuildEventContext result = reader.ReadOptionalBuildEventContext();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(context.NodeId, result.NodeId);
            Assert.Equal(context.ProjectContextId, result.ProjectContextId);
            Assert.Equal(context.TargetId, result.TargetId);
            Assert.Equal(context.TaskId, result.TaskId);
            Assert.Equal(context.SubmissionId, result.SubmissionId);
            Assert.Equal(context.ProjectInstanceId, result.ProjectInstanceId);
            Assert.Equal(context.EvaluationId, result.EvaluationId);
        }

        /// <summary>
        /// Tests the ReadBuildEventContext extension method by writing a context and reading it back.
        /// </summary>
        [Fact]
        public void ReadBuildEventContext_WritesAndReadsContext_CorrectFieldOrder()
        {
            // Arrange
            var context = new BuildEventContext(7, 14, 21, 28, 35, 42, 49);
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
            writer.WriteBuildEventContext(context);
            writer.Flush();
            ms.Position = 0;
            using var reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true);

            // Act
            var readContext = reader.ReadBuildEventContext();

            // Assert
            Assert.Equal(context.NodeId, readContext.NodeId);
            Assert.Equal(context.ProjectContextId, readContext.ProjectContextId);
            Assert.Equal(context.TargetId, readContext.TargetId);
            Assert.Equal(context.TaskId, readContext.TaskId);
            Assert.Equal(context.SubmissionId, readContext.SubmissionId);
            Assert.Equal(context.ProjectInstanceId, readContext.ProjectInstanceId);
            Assert.Equal(context.EvaluationId, readContext.EvaluationId);
        }
    }
}
