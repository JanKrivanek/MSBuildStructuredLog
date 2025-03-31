using FluentAssertions;
using Microsoft.Build.BackEnd;
using Microsoft.Build.Framework;
using Microsoft.Build.Internal;
using Microsoft.Build.Shared;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Microsoft.Build.BackEnd.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ItemGroupLoggingHelper"/> class.
    /// </summary>
    public class ItemGroupLoggingHelperTests
    {
        private readonly BuildEventContext _buildEventContext;
        private readonly DateTime _timestamp;
        private readonly IList _items;

        public ItemGroupLoggingHelperTests()
        {
            _buildEventContext = new BuildEventContext(1, 2, 3, 4, 5, 6, 7);
            _timestamp = DateTime.Now;
            _items = new List<string> { "item1", "item2" };
        }

        /// <summary>
        /// Tests that CreateTaskParameterEventArgs returns a valid TaskParameterEventArgs with correct property assignments.
        /// </summary>
        [Fact]
        public void CreateTaskParameterEventArgs_ValidInputs_ReturnsExpectedEventArgs()
        {
            // Arrange
            var messageKind = (TaskParameterMessageKind)0; // Assuming default enum value
            string parameterName = "param";
            string propertyName = "prop";
            string itemType = "type";
            bool logItemMetadata = true;
            int line = 10;
            int column = 20;

            // Act
            TaskParameterEventArgs result = ItemGroupLoggingHelper.CreateTaskParameterEventArgs(
                _buildEventContext,
                messageKind,
                parameterName,
                propertyName,
                itemType,
                _items,
                logItemMetadata,
                _timestamp,
                line,
                column);

            // Assert
            result.Should().NotBeNull();
            result.BuildEventContext.Should().Be(_buildEventContext);
            result.LineNumber.Should().Be(line);
            result.ColumnNumber.Should().Be(column);
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
        /// <summary>
        /// Tests EnumerateProperties with a null properties input.
        /// </summary>
        [Fact]
        public void EnumerateProperties_NullInput_CallbackNotInvoked()
        {
            // Arrange
            IEnumerable properties = null;
            var callbackInvoked = false;

            // Act
            Utilities.EnumerateProperties(properties, kvp => callbackInvoked = true);

            // Assert
            callbackInvoked.Should().BeFalse();
        }

        /// <summary>
        /// Tests EnumerateProperties with valid DictionaryEntry and KeyValuePair items.
        /// </summary>
        [Fact]
        public void EnumerateProperties_ValidItems_InvokesCallbackForValidEntries()
        {
            // Arrange
            var list = new ArrayList
            {
                new DictionaryEntry("key1", "value1"),
                new KeyValuePair<string, string>("key2", "value2"),
                "invalid", // should be ignored
                new DictionaryEntry("", "noKey"), // empty key, ignored
                new DictionaryEntry("key3", null) // null value converted to empty string
            };

            var results = new List<KeyValuePair<string, string>>();

            // Act
            Utilities.EnumerateProperties(list, kvp => results.Add(kvp));

            // Assert
            results.Should().HaveCount(3);
            results.Should().Contain(new KeyValuePair<string, string>("key1", "value1"));
            results.Should().Contain(new KeyValuePair<string, string>("key2", "value2"));
            results.Should().Contain(new KeyValuePair<string, string>("key3", string.Empty));
        }

        /// <summary>
        /// Tests EnumerateItems invoking callback only for items with non-empty string key.
        /// </summary>
        [Fact]
        public void EnumerateItems_InvalidAndValidEntries_CallbackInvokedOnlyForValidItems()
        {
            // Arrange
            var hashtable = new ArrayList
            {
                new DictionaryEntry("itemType1", "value1"),
                new DictionaryEntry(null, "value2"), // ignored
                new DictionaryEntry("", "value3"),  // ignored
                "nonDictionaryEntry"               // ignored
            };

            var results = new List<DictionaryEntry>();

            // Act
            Utilities.EnumerateItems(hashtable, de => results.Add(de));

            // Assert
            results.Should().HaveCount(1);
            results.First().Key.Should().Be("itemType1");
            results.First().Value.Should().Be("value1");
        }

        /// <summary>
        /// Tests EqualTo extension method for BuildEventContext when both contexts refer to the same instance.
        /// </summary>
        [Fact]
        public void EqualTo_SameReference_ReturnsTrue()
        {
            // Arrange
            var context = new BuildEventContext(10, 20, 30, 40, 50, 60, 70);

            // Act
            bool isEqual = context.EqualTo(context);

            // Assert
            isEqual.Should().BeTrue();
        }

        /// <summary>
        /// Tests EqualTo extension method when one of the contexts is null.
        /// </summary>
        [Fact]
        public void EqualTo_OneNull_ReturnsFalse()
        {
            // Arrange
            var context = new BuildEventContext(1, 2, 3, 4, 5, 6, 7);

            // Act
            bool result1 = context.EqualTo(null);
            bool result2 = ((BuildEventContext)null).EqualTo(context);

            // Assert
            result1.Should().BeFalse();
            result2.Should().BeFalse();
        }

        /// <summary>
        /// Tests EqualTo extension method for BuildEventContext comparing contexts with identical properties.
        /// </summary>
        [Fact]
        public void EqualTo_IdenticalProperties_ReturnsTrue()
        {
            // Arrange
            var context1 = new BuildEventContext(1, 2, 3, 4, 5, 6, 7);
            var context2 = new BuildEventContext(1, 2, 3, 4, 5, 6, 7);

            // Act
            bool isEqual = context1.EqualTo(context2);

            // Assert
            isEqual.Should().BeTrue();
        }

        /// <summary>
        /// Tests EqualTo extension method for BuildEventContext comparing contexts with different properties.
        /// </summary>
        [Fact]
        public void EqualTo_DifferentProperties_ReturnsFalse()
        {
            // Arrange
            var context1 = new BuildEventContext(1, 2, 3, 4, 5, 6, 7);
            var context2 = new BuildEventContext(10, 2, 3, 4, 5, 6, 7);

            // Act
            bool isEqual = context1.EqualTo(context2);

            // Assert
            isEqual.Should().BeFalse();
        }

        /// <summary>
        /// Tests EnumerateMetadata extension method when CloneCustomMetadata returns an IEnumerable of KeyValuePair.
        /// </summary>
        [Fact]
        public void EnumerateMetadata_EnumerableMetadata_ReturnsOriginalEnumerable()
        {
            // Arrange
            var expectedMetadata = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string,string>("meta1", "val1"),
                new KeyValuePair<string,string>("meta2", "val2")
            };
            var mockTaskItem = new Mock<ITaskItem>();
            // Return expectedMetadata as IDictionary via a Dictionary
            IDictionary dictionary = expectedMetadata.ToDictionary(kvp => (object)kvp.Key, kvp => (object)kvp.Value);
            mockTaskItem.Setup(m => m.CloneCustomMetadata()).Returns(dictionary);

            // Act
            var metadata = mockTaskItem.Object.EnumerateMetadata();

            // Assert
            metadata.Should().BeEquivalentTo(expectedMetadata);
        }

        /// <summary>
        /// Tests EnumerateMetadata extension method when CloneCustomMetadata returns a non generic IDictionary.
        /// Also tests handling of exceptions from GetMetadata.
        /// </summary>
        [Fact]
        public void EnumerateMetadata_NonGenericMetadata_ReturnsMetadataWithExceptionMessages()
        {
            // Arrange
            var hashtable = new Hashtable
            {
                { "meta1", "value1" },
                { "meta2", "value2" }
            };
            var mockTaskItem = new Mock<ITaskItem>();
            mockTaskItem.Setup(m => m.CloneCustomMetadata()).Returns(hashtable);
            // Setup GetMetadata so that meta1 returns normal value and meta2 throws exception.
            mockTaskItem.Setup(m => m.GetMetadata("meta1")).Returns("value1");
            mockTaskItem.Setup(m => m.GetMetadata("meta2")).Throws(new InvalidOperationException("error occurred"));

            // Act
            var metadata = mockTaskItem.Object.EnumerateMetadata().ToList();

            // Assert
            metadata.Should().HaveCount(2);
            metadata.Should().Contain(kvp => kvp.Key == "meta1" && kvp.Value == "value1");
            metadata.Should().Contain(kvp => kvp.Key == "meta2" && kvp.Value == "error occurred");
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
        /// <summary>
        /// Tests WriteOptionalString writes a null indicator when value is null.
        /// </summary>
        [Fact]
        public void WriteOptionalString_NullValue_WritesZeroByte()
        {
            // Arrange
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms);

            // Act
            writer.WriteOptionalString(null);
            ms.Position = 0;
            int indicator = ms.ReadByte();

            // Assert
            indicator.Should().Be(0);
        }

        /// <summary>
        /// Tests WriteOptionalString writes a non-null indicator followed by the string.
        /// </summary>
        [Fact]
        public void WriteOptionalString_NonNullValue_WritesIndicatorAndString()
        {
            // Arrange
            string testValue = "test";
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms);

            // Act
            writer.WriteOptionalString(testValue);
            ms.Position = 0;
            int indicator = ms.ReadByte();
            using var reader = new BinaryReader(ms);
            string readString = reader.ReadString();

            // Assert
            indicator.Should().Be(1);
            readString.Should().Be(testValue);
        }

        /// <summary>
        /// Tests WriteTimestamp writes timestamp ticks and kind.
        /// </summary>
        [Fact]
        public void WriteTimestamp_ValidDateTime_WritesTicksAndKind()
        {
            // Arrange
            DateTime testDate = new DateTime(2022, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms);

            // Act
            writer.WriteTimestamp(testDate);
            ms.Position = 0;
            long ticks = new BinaryReader(ms).ReadInt64();
            int kind = new BinaryReader(ms).ReadInt32();

            // Assert
            ticks.Should().Be(testDate.Ticks);
            kind.Should().Be((int)testDate.Kind);
        }

        /// <summary>
        /// Tests Write7BitEncodedInt writes the integer in the correct format.
        /// </summary>
        [Fact]
        public void Write7BitEncodedInt_ValidInteger_WritesEncodedInteger()
        {
            // Arrange
            int value = 300;
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms);

            // Act
            writer.Write7BitEncodedInt(value);
            ms.Position = 0;
            int decoded = BinaryReaderExtensions.Read7BitEncodedInt(new BinaryReader(ms));

            // Assert
            decoded.Should().Be(value);
        }

        /// <summary>
        /// Tests WriteOptionalBuildEventContext writes zero byte when context is null.
        /// </summary>
        [Fact]
        public void WriteOptionalBuildEventContext_NullContext_WritesZeroByte()
        {
            // Arrange
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms);

            // Act
            writer.WriteOptionalBuildEventContext(null);
            ms.Position = 0;
            int indicator = ms.ReadByte();

            // Assert
            indicator.Should().Be(0);
        }

        /// <summary>
        /// Tests WriteOptionalBuildEventContext writes context correctly when non-null.
        /// </summary>
        [Fact]
        public void WriteOptionalBuildEventContext_NonNullContext_WritesIndicatorAndContext()
        {
            // Arrange
            var context = new BuildEventContext(10, 20, 30, 40, 50, 60, 70);
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms);

            // Act
            writer.WriteOptionalBuildEventContext(context);
            ms.Position = 0;
            int indicator = ms.ReadByte();

            // Use BinaryReaderExtensions to read context.
            BuildEventContext readContext = BinaryReaderExtensions.ReadOptionalBuildEventContext(new BinaryReader(ms));

            // Assert
            indicator.Should().Be(1);
            readContext.Should().NotBeNull();
            readContext.EqualTo(context).Should().BeTrue();
        }

        /// <summary>
        /// Tests WriteBuildEventContext writes context fields correctly.
        /// </summary>
        [Fact]
        public void WriteBuildEventContext_WritesAllContextFields()
        {
            // Arrange
            var context = new BuildEventContext(11, 22, 33, 44, 55, 66, 77);
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms);

            // Act
            writer.WriteBuildEventContext(context);
            ms.Position = 0;
            using var reader = new BinaryReader(ms);
            int nodeId = reader.ReadInt32();
            int projectContextId = reader.ReadInt32();
            int targetId = reader.ReadInt32();
            int taskId = reader.ReadInt32();
            int submissionId = reader.ReadInt32();
            int projectInstanceId = reader.ReadInt32();
            int evaluationId = reader.ReadInt32();

            // Assert
            nodeId.Should().Be(context.NodeId);
            projectContextId.Should().Be(context.ProjectContextId);
            targetId.Should().Be(context.TargetId);
            taskId.Should().Be(context.TaskId);
            submissionId.Should().Be(context.SubmissionId);
            projectInstanceId.Should().Be(context.ProjectInstanceId);
            evaluationId.Should().Be(context.EvaluationId);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="BinaryReaderExtensions"/> class.
    /// </summary>
    public class BinaryReaderExtensionsTests
    {
        /// <summary>
        /// Tests ReadOptionalString returns null when the written byte indicator is zero.
        /// </summary>
        [Fact]
        public void ReadOptionalString_ZeroIndicator_ReturnsNull()
        {
            // Arrange
            using var ms = new MemoryStream();
            ms.WriteByte(0);
            ms.Position = 0;
            using var reader = new BinaryReader(ms);

            // Act
            string result = reader.ReadOptionalString();

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Tests ReadOptionalString returns the string when written with a non-null indicator.
        /// </summary>
        [Fact]
        public void ReadOptionalString_NonZeroIndicator_ReturnsString()
        {
            // Arrange
            string testValue = "readTest";
            using var ms = new MemoryStream();
            using (var writer = new BinaryWriter(ms, System.Text.Encoding.UTF8, true))
            {
                writer.Write((byte)1);
                writer.Write(testValue);
            }
            ms.Position = 0;
            using var reader = new BinaryReader(ms);

            // Act
            string result = reader.ReadOptionalString();

            // Assert
            result.Should().Be(testValue);
        }

        /// <summary>
        /// Tests Read7BitEncodedInt reads a valid encoded integer.
        /// </summary>
        [Fact]
        public void Read7BitEncodedInt_ValidEncoding_ReturnsInteger()
        {
            // Arrange
            int value = 123456;
            using var ms = new MemoryStream();
            using (var writer = new BinaryWriter(ms, System.Text.Encoding.UTF8, true))
            {
                writer.Write7BitEncodedInt(value);
            }
            ms.Position = 0;
            using var reader = new BinaryReader(ms);

            // Act
            int result = reader.Read7BitEncodedInt();

            // Assert
            result.Should().Be(value);
        }

        /// <summary>
        /// Tests ReadTimestamp correctly reads the timestamp ticks and kind.
        /// </summary>
        [Fact]
        public void ReadTimestamp_ValidTimestamp_ReturnsCorrectDateTime()
        {
            // Arrange
            DateTime testDate = new DateTime(2021, 12, 31, 23, 59, 59, DateTimeKind.Local);
            using var ms = new MemoryStream();
            using (var writer = new BinaryWriter(ms, System.Text.Encoding.UTF8, true))
            {
                writer.WriteTimestamp(testDate);
            }
            ms.Position = 0;
            using var reader = new BinaryReader(ms);

            // Act
            DateTime result = reader.ReadTimestamp();

            // Assert
            result.Should().Be(testDate);
        }

        /// <summary>
        /// Tests ReadOptionalBuildEventContext returns null when indicator is zero.
        /// </summary>
        [Fact]
        public void ReadOptionalBuildEventContext_ZeroIndicator_ReturnsNull()
        {
            // Arrange
            using var ms = new MemoryStream();
            ms.WriteByte(0);
            ms.Position = 0;
            using var reader = new BinaryReader(ms);

            // Act
            BuildEventContext result = reader.ReadOptionalBuildEventContext();

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Tests ReadOptionalBuildEventContext correctly reads a BuildEventContext when non-null.
        /// </summary>
        [Fact]
        public void ReadOptionalBuildEventContext_NonZeroIndicator_ReturnsContext()
        {
            // Arrange
            var originalContext = new BuildEventContext(5, 6, 7, 8, 9, 10, 11);
            using var ms = new MemoryStream();
            using (var writer = new BinaryWriter(ms, System.Text.Encoding.UTF8, true))
            {
                writer.WriteOptionalBuildEventContext(originalContext);
            }
            ms.Position = 0;
            using var reader = new BinaryReader(ms);

            // Act
            BuildEventContext result = reader.ReadOptionalBuildEventContext();

            // Assert
            result.Should().NotBeNull();
            result.EqualTo(originalContext).Should().BeTrue();
        }

        /// <summary>
        /// Tests ReadBuildEventContext correctly reads BuildEventContext fields.
        /// </summary>
        [Fact]
        public void ReadBuildEventContext_ValidData_ReturnsCorrectContext()
        {
            // Arrange
            var originalContext = new BuildEventContext(15, 25, 35, 45, 55, 65, 75);
            using var ms = new MemoryStream();
            using (var writer = new BinaryWriter(ms, System.Text.Encoding.UTF8, true))
            {
                writer.WriteBuildEventContext(originalContext);
            }
            ms.Position = 0;
            using var reader = new BinaryReader(ms);

            // Act
            BuildEventContext result = reader.ReadBuildEventContext();

            // Assert
            result.EqualTo(originalContext).Should().BeTrue();
        }
    }
}
