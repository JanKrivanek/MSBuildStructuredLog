// using FluentAssertions;
// using FluentAssertions.Primitives;
// using StructuredLogViewer;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// 
// namespace StructuredLogViewer.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "ConditionNode"/> class.
//     /// </summary>
//     public class ConditionNodeTests
//     {
// //         /// <summary> // [Error] (31-74)CS1501 No overload for method 'Contains' takes 2 arguments
// //         /// Tests the Parse method with a simple literal input.
// //         /// Verifies that a node is returned with a child node containing the expected text.
// //         /// </summary>
// //         [Fact]
// //         public void Parse_SimpleLiteral_ReturnsNodeWithChildContainingText()
// //         {
// //             // Arrange
// //             string input = "'hello'";
// //             // Act
// //             ConditionNode result = ConditionNode.Parse(input);
// //             // Assert
// //             result.Should().NotBeNull("because a valid parse should return a ConditionNode instance");
// //             result.Children.Should().NotBeEmpty("because the literal value should be captured in a child node");
// //             // Check that at least one of the child nodes contains the literal "hello"
// //             bool containsHello = result.Children.Any(child => child.Text.Contains("hello", StringComparison.OrdinalIgnoreCase));
// //             containsHello.Should().BeTrue("because the parsed literal should include the word hello");
// //         }
// // 
//         /// <summary>
//         /// Tests the Parse method with an invalid input that has an unmatched quote.
//         /// Verifies that the method throws an exception with the expected message.
//         /// </summary>
//         [Fact]
//         public void Parse_InvalidQuote_ThrowsException()
//         {
//             // Arrange
//             string invalidInput = "'hello";
//             // Act
//             Action act = () => ConditionNode.Parse(invalidInput);
//             // Assert
//             act.Should().Throw<Exception>().WithMessage("Can't parse", "because an unmatched quote should trigger a parse failure");
//         }
// 
//         /// <summary>
//         /// Tests the ParseAndProcess method with valid unevaluated and evaluated inputs.
//         /// Verifies that the processed node has its child nodes' texts concatenated with the expected separator and results updated.
//         /// </summary>
//         [Fact]
//         public void ParseAndProcess_HappyPath_ReturnsProcessedNode()
//         {
//             // Arrange
//             // Use input with a group so that the literal value is properly parsed.
//             string unevaluated = "('value')";
//             string evaluated = "('processed')";
//             // Act
//             ConditionNode processedNode = ConditionNode.ParseAndProcess(unevaluated, evaluated);
//             // Assert
//             processedNode.Should().NotBeNull("because valid inputs should produce a processed ConditionNode");
//             processedNode.Children.Should().NotBeEmpty("because the parsed structure should have child nodes");
//             // Check that at least one child node's Text contains the Unicode arrow separator.
//             bool hasProcessedText = processedNode.Children.Any(child => child.Text.Contains(" \u2794 "));
//             hasProcessedText.Should().BeTrue("because processing should combine the texts of corresponding nodes");
//         }
// 
//         /// <summary>
//         /// Tests the Process method with two ConditionNode instances that have differing numbers of nodes.
//         /// Verifies that the method throws an exception indicating a mismatch in node counts.
//         /// </summary>
//         [Fact]
//         public void Process_NodeCountMismatch_ThrowsException()
//         {
//             // Arrange
//             // Create a simple unevaluated node without children.
//             ConditionNode unevaluatedNode = new ConditionNode
//             {
//                 Text = "node1"
//             };
//             // Create an evaluated node and add an extra child to force a mismatch.
//             ConditionNode evaluatedNode = new ConditionNode
//             {
//                 Text = "node2"
//             };
//             evaluatedNode.Children.Add(new ConditionNode { Text = "child" });
//             // Act
//             Action act = () => ConditionNode.Process(unevaluatedNode, evaluatedNode);
//             // Assert
//             act.Should().Throw<Exception>().WithMessage("*different number of nodes*", "because a mismatch in the enumerated nodes should trigger an exception");
//         }
// //  // [Error] (122-65)CS1061 'IEnumerator<ConditionNode>' does not contain a definition for 'ToList' and no accessible extension method 'ToList' accepting a first argument of type 'IEnumerator<ConditionNode>' could be found (are you missing a using directive or an assembly reference?) // [Error] (126-39)CS1061 'GenericCollectionAssertions<ConditionNode>' does not contain a definition for 'Be' and no accessible extension method 'Be' accepting a first argument of type 'GenericCollectionAssertions<ConditionNode>' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests the GetEnumerator method to ensure it returns all nodes in the tree recursively.
// //         /// Constructs a simple tree and verifies the total count and order of nodes returned.
// //         /// </summary>
// //         [Fact]
// //         public void GetEnumerator_ReturnsAllNodesRecursively()
// //         {
// //             // Arrange
// //             // Manually build a tree structure.
// //             ConditionNode root = new ConditionNode
// //             {
// //                 Text = "root"
// //             };
// //             ConditionNode child1 = new ConditionNode
// //             {
// //                 Text = "child1",
// //                 Parent = root
// //             };
// //             ConditionNode child2 = new ConditionNode
// //             {
// //                 Text = "child2",
// //                 Parent = root
// //             };
// //             root.Children.Add(child1);
// //             root.Children.Add(child2);
// //             // Act
// //             List<ConditionNode> allNodes = root.GetEnumerator().ToList();
// //             // Assert
// //             // Should include the root and its two children.
// //             allNodes.Count.Should().Be(3, "because the tree consists of one root node and two child nodes");
// //             allNodes.First().Should().Be(root, "because the enumeration should start with the root node");
// //             allNodes.Should().Contain(child1, "because child1 is part of the tree");
// //             allNodes.Should().Contain(child2, "because child2 is part of the tree");
// //         }
// //     }
// }