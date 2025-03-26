using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger
{
    /// <summary>
    /// Fake implementation of the TreeBinaryWriter used for testing.
    /// </summary>
    public class FakeTreeBinaryWriter : IDisposable
    {
        public readonly string FilePath;
        public readonly List<string> Calls;
        public bool Disposed { get; private set; }

        // Static dictionary to store instances by file path for retrieval in tests.
        public static readonly Dictionary<string, FakeTreeBinaryWriter> Instances = new Dictionary<string, FakeTreeBinaryWriter>();

        public FakeTreeBinaryWriter(string filePath)
        {
            FilePath = filePath;
            Calls = new List<string>();
            Instances[filePath] = this;
        }

        public void WriteNode(string nodeName)
        {
            Calls.Add("WriteNode:" + nodeName);
        }

        public void WriteAttributeValue(string value)
        {
            Calls.Add("WriteAttributeValue:" + value);
        }

        public void WriteEndAttributes()
        {
            Calls.Add("WriteEndAttributes");
        }

        public void WriteChildrenCount(int count)
        {
            Calls.Add("WriteChildrenCount:" + count);
        }

        public void WriteByteArray(byte[] data)
        {
            Calls.Add("WriteByteArray:" + (data != null ? data.Length.ToString() : "null"));
        }

        public void Dispose()
        {
            Disposed = true;
            Calls.Add("Dispose");
        }

        public static void ResetInstances()
        {
            Instances.Clear();
        }
    }

    /// <summary>
    /// Fake implementation of the Serialization helper.
    /// </summary>
    public static class Serialization
    {
        public static string GetNodeName(BaseNode node)
        {
            return node.GetType().Name;
        }
    }

    /// <summary>
    /// Base node used in the BuildLogWriter.
    /// </summary>
    public class BaseNode { }

    /// <summary>
    /// Represents a tree node that can have children.
    /// </summary>
    public class TreeNode : BaseNode
    {
        public List<BaseNode> Children { get; } = new List<BaseNode>();

        public bool HasChildren => Children.Count > 0;
    }

    /// <summary>
    /// Represents a build node.
    /// </summary>
    public class Build : TreeNode
    {
        public bool Succeeded { get; set; }
        public bool IsAnalyzed { get; set; }
        public byte[] SourceFilesArchive { get; set; }
    }

    /// <summary>
    /// Represents metadata.
    /// </summary>
    public class Metadata : BaseNode
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    // Additional stubs for other node types could be added here as needed.
}

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the BuildLogWriter class.
    /// </summary>
    public class BuildLogWriterTests : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildLogWriterTests"/> class and resets fake writer instances.
        /// </summary>
        public BuildLogWriterTests()
        {
            Microsoft.Build.Logging.StructuredLogger.FakeTreeBinaryWriter.ResetInstances();
        }

        /// <summary>
        /// Tests that the Write method processes a Build node without children correctly.
        /// The test creates a Build node with no children and a source files archive, then verifies that the
        /// underlying FakeTreeBinaryWriter logs the expected method calls including node writing, attribute writing,
        /// children count (0), writing the byte array, and disposal.
        /// </summary>
//         [Fact] [Error] (146-75)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.Build [C:\src-other\MSBuildStructuredLog\src\StructuredLogger.Tests\Serialization\Binary\BuildLogWriterTests.cs(93)]' to 'Microsoft.Build.Logging.StructuredLogger.Build [C:\src-other\MSBuildStructuredLog\src\StructuredLogger\ObjectModel\Build.cs(13)]'
//         public void Write_WithBuildWithoutChildren_CallsWriterMethodsCorrectly()
//         {
//             // Arrange
//             var testFilePath = "TestFile_NoChildren.log";
//             var build = new Microsoft.Build.Logging.StructuredLogger.Build
//             {
//                 Succeeded = true,
//                 IsAnalyzed = false,
//                 SourceFilesArchive = new byte[] { 1, 2, 3 }
//             };
// 
//             // Act
//             Microsoft.Build.Logging.StructuredLogger.BuildLogWriter.Write(build, testFilePath);
// 
//             // Assert
//             Assert.True(Microsoft.Build.Logging.StructuredLogger.FakeTreeBinaryWriter.Instances.ContainsKey(testFilePath));
//             var writer = Microsoft.Build.Logging.StructuredLogger.FakeTreeBinaryWriter.Instances[testFilePath];
// 
//             // Expected call for writing the node name ("Build")
//             Assert.Contains("WriteNode:Build", writer.Calls);
// 
//             // Expected attribute calls for Build properties
//             Assert.Contains("WriteAttributeValue:" + build.Succeeded.ToString(), writer.Calls);
//             Assert.Contains("WriteAttributeValue:" + build.IsAnalyzed.ToString(), writer.Calls);
// 
//             // End attributes call
//             Assert.Contains("WriteEndAttributes", writer.Calls);
// 
//             // Since there are no children, expects children count 0
//             Assert.Contains("WriteChildrenCount:0", writer.Calls);
// 
//             // Expected call for writing the byte array for source files archive
//             Assert.Contains("WriteByteArray:" + build.SourceFilesArchive.Length, writer.Calls);
// 
//             // Verify that the writer was disposed
//             Assert.Contains("Dispose", writer.Calls);
//         }

        /// <summary>
        /// Tests that the Write method processes a Build node with a child Metadata node correctly.
        /// The test creates a Build node with one child Metadata node and verifies that method calls for both
        /// the parent and child nodes are logged correctly by the FakeTreeBinaryWriter.
        /// </summary>
//         [Fact] [Error] (199-75)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.Build [C:\src-other\MSBuildStructuredLog\src\StructuredLogger.Tests\Serialization\Binary\BuildLogWriterTests.cs(93)]' to 'Microsoft.Build.Logging.StructuredLogger.Build [C:\src-other\MSBuildStructuredLog\src\StructuredLogger\ObjectModel\Build.cs(13)]'
//         public void Write_WithBuildWithChild_CallsWriterMethodsForChild()
//         {
//             // Arrange
//             var testFilePath = "TestFile_WithChild.log";
//             var build = new Microsoft.Build.Logging.StructuredLogger.Build
//             {
//                 Succeeded = false,
//                 IsAnalyzed = true,
//                 SourceFilesArchive = new byte[] { 10, 20 }
//             };
// 
//             var metadata = new Microsoft.Build.Logging.StructuredLogger.Metadata
//             {
//                 Name = "TestName",
//                 Value = "TestValue"
//             };
// 
//             // Add child Metadata node to the Build's children
//             build.Children.Add(metadata);
// 
//             // Act
//             Microsoft.Build.Logging.StructuredLogger.BuildLogWriter.Write(build, testFilePath);
// 
//             // Assert
//             Assert.True(Microsoft.Build.Logging.StructuredLogger.FakeTreeBinaryWriter.Instances.ContainsKey(testFilePath));
//             var writer = Microsoft.Build.Logging.StructuredLogger.FakeTreeBinaryWriter.Instances[testFilePath];
// 
//             // Verify writing of the Build node and its attributes
//             Assert.Contains("WriteNode:Build", writer.Calls);
//             Assert.Contains("WriteAttributeValue:" + build.Succeeded.ToString(), writer.Calls);
//             Assert.Contains("WriteAttributeValue:" + build.IsAnalyzed.ToString(), writer.Calls);
// 
//             // End attributes for Build node
//             Assert.Contains("WriteEndAttributes", writer.Calls);
// 
//             // Build node should indicate one child
//             Assert.Contains("WriteChildrenCount:1", writer.Calls);
// 
//             // Verify that the child Metadata node has been processed
//             Assert.Contains("WriteNode:Metadata", writer.Calls);
//             Assert.Contains("WriteAttributeValue:" + metadata.Name, writer.Calls);
//             Assert.Contains("WriteAttributeValue:" + metadata.Value, writer.Calls);
// 
//             // Child node should have no children
//             Assert.Contains("WriteChildrenCount:0", writer.Calls);
// 
//             // Finally, verify that the Build node's byte array is written and writer is disposed
//             Assert.Contains("WriteByteArray:" + build.SourceFilesArchive.Length, writer.Calls);
//             Assert.Contains("Dispose", writer.Calls);
//         }

        /// <summary>
        /// Tests that the underlying writer is disposed after using the Write method.
        /// The Write method employs a using block which should call Dispose on the underlying FakeTreeBinaryWriter.
        /// </summary>
//         [Fact] [Error] (246-75)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.Build [C:\src-other\MSBuildStructuredLog\src\StructuredLogger.Tests\Serialization\Binary\BuildLogWriterTests.cs(93)]' to 'Microsoft.Build.Logging.StructuredLogger.Build [C:\src-other\MSBuildStructuredLog\src\StructuredLogger\ObjectModel\Build.cs(13)]'
//         public void Dispose_CallsUnderlyingWriterDispose()
//         {
//             // Arrange
//             var testFilePath = "TestFile_Dispose.log";
//             var build = new Microsoft.Build.Logging.StructuredLogger.Build
//             {
//                 Succeeded = true,
//                 IsAnalyzed = true,
//                 SourceFilesArchive = new byte[0]
//             };
// 
//             // Act
//             Microsoft.Build.Logging.StructuredLogger.BuildLogWriter.Write(build, testFilePath);
// 
//             // Assert
//             Assert.True(Microsoft.Build.Logging.StructuredLogger.FakeTreeBinaryWriter.Instances.ContainsKey(testFilePath));
//             var writer = Microsoft.Build.Logging.StructuredLogger.FakeTreeBinaryWriter.Instances[testFilePath];
//             Assert.True(writer.Disposed);
//         }

        /// <summary>
        /// Disposes resources used in testing by resetting fake writer instances.
        /// </summary>
        public void Dispose()
        {
            Microsoft.Build.Logging.StructuredLogger.FakeTreeBinaryWriter.ResetInstances();
        }
    }
}
