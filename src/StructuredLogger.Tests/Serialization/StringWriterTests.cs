// using FluentAssertions;
// using Moq;
// using System;
// using System.Collections.Generic;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="StringWriter"/> class.
//     /// </summary>
//     public class StringWriterTests
//     {
// //         /// <summary> // [Error] (25-52)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.BaseNode [C:\src\binlog-viewer\src\StructuredLogger.Tests\Serialization\StringWriterTests.cs(111)]' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode [C:\src\binlog-viewer\src\StructuredLogger\ObjectModel\BaseNode.cs(7)]'
// //         /// Tests that GetString returns an empty string when the input node is null.
// //         /// </summary>
// //         [Fact]
// //         public void GetString_NullInput_ReturnsEmptyString()
// //         {
// //             // Arrange
// //             BaseNode input = null;
// //             
// //             // Act
// //             string result = StringWriter.GetString(input);
// // 
// //             // Assert
// //             result.Should().BeEmpty("because a null node should produce an empty string");
// //         }
// //  // [Error] (41-52)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.SimpleNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
//         /// <summary>
//         /// Tests that GetString returns the expected string for a simple node without children.
//         /// </summary>
//         [Fact]
//         public void GetString_SimpleNode_ReturnsTextWithNewLine()
//         {
//             // Arrange
//             var node = new SimpleNode("Test");
// 
//             // Act
//             string result = StringWriter.GetString(node);
// 
//             // Assert
//             result.Should().Be("Test" + Environment.NewLine, "because the simple node text should be output followed by a newline");
//         }
// //  // [Error] (54-28)CS1729 'TreeNode' does not contain a constructor that takes 1 arguments // [Error] (56-31)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.SimpleNode' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.TreeNode' // [Error] (59-52)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.TreeNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
// //         /// <summary>
// //         /// Tests that GetString correctly processes a TreeNode with a child node and applies proper indentation.
// //         /// </summary>
// //         [Fact]
// //         public void GetString_TreeNodeWithChild_ReturnsIndentedChild()
// //         {
// //             // Arrange
// //             var root = new TreeNode("Root");
// //             var child = new SimpleNode("Child");
// //             root.Children.Add(child);
// // 
// //             // Act
// //             string result = StringWriter.GetString(root);
// // 
// //             // Assert
// //             // Expected output:
// //             // "Root" + newline +
// //             // "    Child" + newline
// //             string expected = "Root" + Environment.NewLine + "    Child" + Environment.NewLine;
// //             result.Should().Be(expected, "because the root and its child should be printed with proper indentation");
// //         }
// //  // [Error] (85-32)CS1729 'TreeNode' does not contain a constructor that takes 1 arguments // [Error] (87-35)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.SimpleNode' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.TreeNode' // [Error] (90-56)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.TreeNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
//         /// <summary>
//         /// Tests that GetString stops processing further child nodes when the StringBuilder length exceeds MaxStringLength.
//         /// This is simulated by lowering the MaxStringLength threshold.
//         /// </summary>
//         [Fact]
//         public void GetString_WhenMaxStringLengthExceeded_SkipsChildProcessing()
//         {
//             // Arrange
//             int originalMax = StringWriter.MaxStringLength;
//             try
//             {
//                 // Set a very low maximum string length so that after writing the root,
//                 // the length exceeds the threshold and the child is not processed.
//                 StringWriter.MaxStringLength = 10;
//                 // Create a root node such that after appending its text and a newline, the length is > 10.
//                 // For example, "1234567890" is 10 characters; appending a newline makes it exceed.
//                 var root = new TreeNode("1234567890");
//                 // Add a child node which should not be written because the limit is exceeded.
//                 root.Children.Add(new SimpleNode("Child"));
// 
//                 // Act
//                 string result = StringWriter.GetString(root);
// 
//                 // Assert
//                 // Only the root node's text and newline should be present.
//                 result.Should().Be("1234567890" + Environment.NewLine, "because the maximum string length was exceeded, so child nodes were skipped");
//             }
//             finally
//             {
//                 // Restore the original maximum string length after the test.
//                 StringWriter.MaxStringLength = originalMax;
//             }
//         }
//     }
// }
// 
// namespace Microsoft.Build.Logging.StructuredLogger
// {
//     /// <summary>
//     /// Represents the abstract base class for nodes used in StringWriter.
//     /// This minimal implementation is provided for testing purposes.
//     /// </summary>
//     public abstract class BaseNode
//     {
//         /// <summary>
//         /// Gets the full text representation of the node.
//         /// </summary>
//         /// <returns>The text representing the node.</returns>
//         public abstract string GetFullText();
//     }
// 
//     /// <summary>
//     /// Represents a simple node with static text and no children.
//     /// This minimal implementation is provided for testing purposes.
//     /// </summary>
//     public class SimpleNode : BaseNode
//     {
//         private readonly string _text;
// 
//         /// <summary>
//         /// Initializes a new instance of the <see cref="SimpleNode"/> class with the specified text.
//         /// </summary>
//         /// <param name="text">The text of the node.</param>
//         public SimpleNode(string text)
//         {
//             _text = text;
//         }
// 
//         /// <summary>
//         /// Returns the full text representation of the node.
//         /// </summary>
//         /// <returns>The text of the node.</returns>
//         public override string GetFullText() => _text;
//     }
// //  // [Error] (148-18)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger' already contains a definition for 'TreeNode'
// //     /// <summary>
// //     /// Represents a tree node which may have child nodes.
// //     /// This minimal implementation is provided for testing purposes.
// //     /// </summary>
// //     public class TreeNode : BaseNode
// //     {
// //         private readonly string _text;
// // 
// //         /// <summary>
// //         /// Gets or sets the child nodes.
// //         /// </summary>
// //         public List<BaseNode> Children { get; set; } = new List<BaseNode>();
// //  // [Error] (160-36)CS0229 Ambiguity between 'TreeNode.Children' and 'TreeNode.Children' // [Error] (160-56)CS0229 Ambiguity between 'TreeNode.Children' and 'TreeNode.Children'
// //         /// <summary>
// //         /// Gets a value indicating whether the node has any children.
// //         /// </summary>
// //         public bool HasChildren => Children != null && Children.Count > 0;
// // 
// //         /// <summary>
// //         /// Initializes a new instance of the <see cref="TreeNode"/> class with the specified text.
// //         /// </summary>
// //         /// <param name="text">The text of the node.</param>
// //         public TreeNode(string text)
// //         {
// //             _text = text;
// //         }
// // 
// //         /// <summary>
// //         /// Returns the full text representation of the node.
// //         /// </summary>
// //         /// <returns>The text of the node.</returns>
// //         public override string GetFullText() => _text;
// //     }
// // }