// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Moq;
// using System;
// using System.Collections.Generic;
// using System.Text;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="StringWriter"/> class.
//     /// </summary>
//     public class StringWriterTests
//     {
//         /// <summary>
//         /// Tests that GetString returns an empty string when the provided root node is null.
//         /// Expected Outcome: An empty string is returned.
//         /// </summary>
//         [Fact]
//         public void GetString_NullRootNode_ReturnsEmptyString()
//         {
//             // Arrange
//             Microsoft.Build.Logging.StructuredLogger.BaseNode rootNode = null!;
// 
//             // Act
//             string result = StringWriter.GetString(rootNode);
// 
//             // Assert
//             result.Should().BeEmpty("because a null root node should result in an empty string output");
//         }
// //  // [Error] (43-39)CS1061 'BaseNode' does not contain a definition for 'GetFullText' and no accessible extension method 'GetFullText' accepting a first argument of type 'BaseNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (46-52)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
// //         /// <summary>
// //         /// Tests that GetString returns the correct string for a simple BaseNode without children.
// //         /// Expected Outcome: The output contains the node's text followed by a new line.
// //         /// </summary>
// //         [Fact]
// //         public void GetString_SimpleBaseNode_ReturnsCorrectString()
// //         {
// //             // Arrange
// //             const string expectedText = "TestNode";
// //             var baseNodeMock = new Mock<BaseNode>();
// //             baseNodeMock.Setup(n => n.GetFullText()).Returns(expectedText);
// // 
// //             // Act
// //             string result = StringWriter.GetString(baseNodeMock.Object);
// // 
// //             // Assert
// //             string expectedOutput = expectedText + Environment.NewLine;
// //             result.Should().Be(expectedOutput, "because the simple base node's text should be output with a trailing new line");
// //         }
// // 
//         /// <summary>
//         /// Tests that GetString correctly processes a tree node with one child.
//         /// Expected Outcome: The parent's text is output, followed by an indented child's text on a new line.
//         /// </summary>
//         [Fact]
//         public void GetString_TreeNodeWithChild_ReturnsIndentedOutput()
//         {
//             // Arrange
//             const string parentText = "ParentNode";
//             const string childText = "ChildNode";
// 
//             var parentNode = new DummyTreeNode(parentText);
//             var childNode = new DummyBaseNode(childText);
//             parentNode.AddChild(childNode);
// 
//             // Act
//             string result = StringWriter.GetString(parentNode);
// 
//             // Assert
//             // Expected output: parent's text with no indent, then child's text with an indent of 4 spaces.
//             string expectedOutput = parentText + Environment.NewLine + "    " + childText + Environment.NewLine;
//             result.Should().Be(expectedOutput, "because the tree node output should include the parent's text and an indented child's text");
//         }
// 
//         /// <summary>
//         /// Tests that GetString respects the MaxStringLength constraint by preventing further node processing once the limit is exceeded.
//         /// Expected Outcome: Only the root node's text is output when the MaxStringLength is set artificially low.
//         /// </summary>
//         [Fact]
//         public void GetString_WhenMaxStringLengthExceeded_DoesNotProcessChildNodes()
//         {
//             // Arrange
//             // Backup the original MaxStringLength value.
//             int originalMaxLength = StringWriter.MaxStringLength;
//             try
//             {
//                 // Set a low maximum length to force early termination in processing child nodes.
//                 StringWriter.MaxStringLength = 10; // small value to trigger cutoff
// 
//                 const string parentText = "Parent"; // length 6
//                 const string childText = "Child";   // length 5
// 
//                 var parentNode = new DummyTreeNode(parentText);
//                 var childNode = new DummyBaseNode(childText);
//                 parentNode.AddChild(childNode);
// 
//                 // Act
//                 string result = StringWriter.GetString(parentNode);
// 
//                 // Assert
//                 // The parent's text appended with a new line will exceed the limit in subsequent recursive calls,
//                 // so the child node should not be processed.
//                 string expectedOutput = parentText + Environment.NewLine;
//                 result.Should().Be(expectedOutput, "because when MaxStringLength is exceeded, child nodes should not be processed");
//             }
//             finally
//             {
//                 // Restore the original maximum string length.
//                 StringWriter.MaxStringLength = originalMaxLength;
//             }
//         }
// 
//         #region Dummy Implementations for Testing
// 
//         /// <summary>
//         /// Provides a dummy concrete implementation of the BaseNode class for testing purposes.
//         /// </summary>
//         private class DummyBaseNode : BaseNode
//         {
//             private readonly string _text;
// 
//             /// <summary>
//             /// Initializes a new instance of the <see cref="DummyBaseNode"/> class.
//             /// </summary>
//             /// <param name="text">The text to be returned by GetFullText.</param>
//             public DummyBaseNode(string text)
//             {
//                 _text = text;
//             }
// //  // [Error] (137-36)CS0115 'StringWriterTests.DummyBaseNode.GetFullText()': no suitable method found to override
// //             /// <summary>
// //             /// Returns the full text representation of the node.
// //             /// </summary>
// //             /// <returns>The text provided at construction.</returns>
// //             public override string GetFullText()
// //             {
// //                 return _text;
// //             }
// //         }
// 
//         /// <summary>
//         /// Provides a dummy concrete implementation of the TreeNode class for testing purposes.
//         /// </summary>
//         private class DummyTreeNode : TreeNode
//         {
//             private readonly string _text;
//             private readonly List<BaseNode> _children;
// 
//             /// <summary>
//             /// Initializes a new instance of the <see cref="DummyTreeNode"/> class.
//             /// </summary>
//             /// <param name="text">The text to be returned by GetFullText.</param>
//             public DummyTreeNode(string text)
//             {
//                 _text = text;
//                 _children = new List<BaseNode>();
//             }
// 
//             /// <summary>
//             /// Adds a child node to this tree node.
//             /// </summary>
//             /// <param name="child">The child node to add.</param>
//             public void AddChild(BaseNode child)
//             {
//                 _children.Add(child);
//             }
// 
//             /// <summary>
//             /// Returns the full text representation of the node.
//             /// </summary>
//             /// <returns>The text provided at construction.</returns>
//             public override string GetFullText()
//             {
//                 return _text;
//             }
// //  // [Error] (182-34)CS0506 'StringWriterTests.DummyTreeNode.HasChildren': cannot override inherited member 'TreeNode.HasChildren' because it is not marked virtual, abstract, or override
// //             /// <summary>
// //             /// Gets a value indicating whether the tree node has child nodes.
// //             /// </summary>
// //             public override bool HasChildren => _children.Count > 0;
// //  // [Error] (187-45)CS0506 'StringWriterTests.DummyTreeNode.Children': cannot override inherited member 'TreeNode.Children' because it is not marked virtual, abstract, or override
//             /// <summary>
//             /// Gets the list of child nodes.
//             /// </summary>
//             public override IList<BaseNode> Children => _children;
//         }
// 
//         #endregion
//     }
// }