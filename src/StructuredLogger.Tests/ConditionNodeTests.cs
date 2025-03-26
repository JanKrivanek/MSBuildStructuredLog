using System;
using System.Collections.Generic;
using System.Linq;
using StructuredLogViewer;
using Xunit;

namespace StructuredLogViewer.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ConditionNode"/> class.
    /// </summary>
    public class ConditionNodeTests
    {
        /// <summary>
        /// Tests the Parse method with an empty string input.
        /// Expects a root node with default properties and no children.
        /// </summary>
        [Fact]
        public void Parse_EmptyInput_ReturnsRootNodeWithoutChildren()
        {
            // Arrange
            string input = "";

            // Act
            ConditionNode root = ConditionNode.Parse(input);

            // Assert
            Assert.NotNull(root);
            Assert.Equal(ConditionNode.ConditionOperator.AND, root.Operator);
            Assert.True(root.Result);
            Assert.Empty(root.Children);
        }

        /// <summary>
        /// Tests the Parse method with a boolean literal input when evaluation is enabled.
        /// Expects that a child node is created with the text containing the literal and the evaluation yields true.
        /// </summary>
        [Fact]
        public void Parse_BooleanLiteralDoEvaluate_ReturnsChildWithCorrectResult()
        {
            // Arrange
            string input = "'true'";
            bool doEvaluate = true;

            // Act
            ConditionNode root = ConditionNode.Parse(input, doEvaluate);
            List<ConditionNode> allNodes = root.ToList();

            // Assert
            // The enumerator yields the root as the first node; the subsequent nodes are parsed children.
            Assert.True(allNodes.Count >= 2);
            ConditionNode child = allNodes[1];
            Assert.Contains("true", child.Text, StringComparison.OrdinalIgnoreCase);
            Assert.True(child.Result);
        }

        /// <summary>
        /// Tests the Parse method with a numeric equality comparison string.
        /// Expects that comparing '1' equal to '1' evaluates to true.
        /// </summary>
        [Fact]
        public void Parse_NumericComparison_ReturnsCorrectResult()
        {
            // Arrange
            string input = "'1'='1'";
            bool doEvaluate = true;

            // Act
            ConditionNode root = ConditionNode.Parse(input, doEvaluate);
            List<ConditionNode> nodes = root.ToList();
            ConditionNode child = nodes.Skip(1).FirstOrDefault();

            // Assert
            Assert.NotNull(child);
            Assert.True(child.Result);
            Assert.Contains("1", child.Text);
        }

        /// <summary>
        /// Tests the Parse method with a NOT boolean literal.
        /// Expects that negating 'true' yields a result of false.
        /// </summary>
        [Fact]
        public void Parse_NotBooleanLiteral_ReturnsCorrectNegatedResult()
        {
            // Arrange
            string input = "!'true'";
            bool doEvaluate = true;

            // Act
            ConditionNode root = ConditionNode.Parse(input, doEvaluate);
            List<ConditionNode> nodes = root.ToList();
            ConditionNode child = nodes.Skip(1).FirstOrDefault();

            // Assert
            Assert.NotNull(child);
            Assert.False(child.Result);
            Assert.Contains("true", child.Text, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Tests the Process method when unevaluated and evaluated node trees have the same node count.
        /// Expects that the texts are merged using the arrow delimiter and that the evaluated results are propagated.
        /// </summary>
        [Fact]
        public void Process_SameNumberOfNodes_MergesTextAndSetsResult()
        {
            // Arrange
            // Use Parse to generate node trees.
            string unevalInput = "'hello'";
            string evalInput = "'world'";
            ConditionNode unevalNode = ConditionNode.Parse(unevalInput, false);
            ConditionNode evalNode = ConditionNode.Parse(evalInput, true);

            // Act
            ConditionNode processed = ConditionNode.Process(unevalNode, evalNode);
            List<ConditionNode> processedNodes = processed.ToList();
            List<ConditionNode> evalNodes = evalNode.ToList();

            // Assert
            Assert.Equal(processedNodes.Count, evalNodes.Count);
            foreach (var (pNode, eNode) in processedNodes.Zip(evalNodes, Tuple.Create))
            {
                if (!string.IsNullOrWhiteSpace(pNode.Text))
                {
                    // The merged text should contain the arrow and the evaluated text.
                    Assert.Contains("\u2794", pNode.Text);
                    Assert.Contains(eNode.Text, pNode.Text);
                }
                Assert.Equal(eNode.Result, pNode.Result);
            }
        }

        /// <summary>
        /// Tests the Process method when the node trees have a mismatched count.
        /// Expects that an exception is thrown indicating a difference in node counts.
        /// </summary>
        [Fact]
        public void Process_NodeCountMismatch_ThrowsException()
        {
            // Arrange
            // Manually create a mismatched node structure.
            ConditionNode unevalNode = new ConditionNode { Text = "A", Result = true };
            unevalNode.Children.Add(new ConditionNode { Text = "Child1", Result = true });

            ConditionNode evalNode = new ConditionNode { Text = "B", Result = false };
            // evalNode has no children, so enumeration counts will differ.

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() => ConditionNode.Process(unevalNode, evalNode));
            Assert.Contains("different number of nodes", ex.Message);
        }

        /// <summary>
        /// Tests the GetEnumerator method to ensure that it returns the node itself and all recursive children.
        /// Expects that the enumeration order is the root followed by its children in depth-first order.
        /// </summary>
        [Fact]
        public void GetEnumerator_ReturnsSelfAndRecursiveChildren()
        {
            // Arrange
            ConditionNode root = new ConditionNode { Text = "Root", Result = true };
            ConditionNode child1 = new ConditionNode { Text = "Child1", Result = true, Parent = root };
            ConditionNode child2 = new ConditionNode { Text = "Child2", Result = false, Parent = root };
            root.Children.Add(child1);
            root.Children.Add(child2);
            ConditionNode grandChild = new ConditionNode { Text = "GrandChild", Result = true, Parent = child1 };
            child1.Children.Add(grandChild);

            // Act
            List<ConditionNode> nodes = root.ToList();

            // Assert
            // Expected order: Root, Child1, GrandChild, Child2.
            Assert.Equal(4, nodes.Count);
            Assert.Equal("Root", nodes[0].Text);
            Assert.Equal("Child1", nodes[1].Text);
            Assert.Equal("GrandChild", nodes[2].Text);
            Assert.Equal("Child2", nodes[3].Text);
        }

        /// <summary>
        /// Tests the ParseAndProcess method to ensure that two condition strings are parsed, processed, and merged correctly.
        /// Expects merged texts containing inputs from both parsed trees and the evaluated result is properly assigned.
        /// </summary>
        [Fact]
        public void ParseAndProcess_MergesNodesCorrectly()
        {
            // Arrange
            string unevalInput = "'foo'";
            string evalInput = "'bar'";

            // Act
            ConditionNode processedNode = ConditionNode.ParseAndProcess(unevalInput, evalInput);
            List<ConditionNode> processedNodes = processedNode.ToList();

            // Assert
            // The root node is yielded first; its child should contain merged text with an arrow delimiter.
            Assert.True(processedNodes.Count >= 2);
            ConditionNode child = processedNodes[1];
            Assert.Contains("\u2794", child.Text);
            Assert.Contains("foo", child.Text);
            Assert.Contains("bar", child.Text);
            // For the evaluated node, if evaluation of 'bar' fails to parse as bool, it returns true by default.
            Assert.True(child.Result);
        }
    }
}
