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
        /// Tests that parsing a plain text condition without evaluation creates a child node with the expected text.
        /// This verifies that the Parse method concatenates the literal correctly and does not attempt evaluation.
        /// </summary>
        [Fact]
        public void Parse_WithPlainText_NoEvaluation_SetsTextAndDefaultResult()
        {
            // Arrange
            string input = "'test'";
            bool doEvaluate = false;

            // Act
            ConditionNode root = ConditionNode.Parse(input, doEvaluate);

            // Assert
            // The root node has an Operator set to AND and should have at least one child.
            Assert.NotNull(root);
            Assert.Equal(ConditionNode.ConditionOperator.AND, root.Operator);
            Assert.True(root.Children.Count >= 1);
            // Check that one of the child nodes contains the literal text.
            bool found = root.Children.Any(child => child.Text.Contains("test"));
            Assert.True(found);
            // As evaluation is not performed, the Result should remain true (default).
            Assert.True(root.Result);
        }

        /// <summary>
        /// Tests that parsing a condition with an equality comparison where both sides are equal returns true.
        /// Expected outcome is that evaluation returns true.
        /// </summary>
        [Fact]
        public void Parse_EvaluationEquality_WhenValuesAreEqual_ReturnsTrue()
        {
            // Arrange
            // Input format: 'hello'='hello'
            string input = "'hello'='hello'";
            bool doEvaluate = true;

            // Act
            ConditionNode root = ConditionNode.Parse(input, doEvaluate);

            // Assert
            // The evaluation should result in true.
            // Since the evaluation is computed in the leaf child nodes, verify that all leaf nodes have Result true.
            var allLeafResults = root.OfType<ConditionNode>().Select(n => n.Result);
            Assert.All(allLeafResults, result => Assert.True(result));
        }

        /// <summary>
        /// Tests that parsing a condition with a not-equal comparison where the values differ returns true.
        /// </summary>
        [Fact]
        public void Parse_EvaluationInequality_WhenValuesDiffer_ReturnsTrue()
        {
            // Arrange
            // Input format: 'hello'!='world'
            string input = "'hello'!='world'";
            bool doEvaluate = true;

            // Act
            ConditionNode root = ConditionNode.Parse(input, doEvaluate);

            // Assert
            // The condition 'hello' not equals 'world' should evaluate to true.
            var allLeafResults = root.OfType<ConditionNode>().Select(n => n.Result);
            Assert.All(allLeafResults, result => Assert.True(result));
        }

        /// <summary>
        /// Tests numeric comparison evaluation where the left value is less than the right value.
        /// Expected outcome is that the evaluation returns true.
        /// </summary>
        [Fact]
        public void Parse_EvaluationNumericComparison_LessThan_ReturnsTrue()
        {
            // Arrange
            // Input format: '2.5'<'3.5'
            string input = "'2.5'<'3.5'";
            bool doEvaluate = true;

            // Act
            ConditionNode root = ConditionNode.Parse(input, doEvaluate);

            // Assert
            // The numeric comparison 2.5 < 3.5 should yield true.
            var allLeafResults = root.OfType<ConditionNode>().Select(n => n.Result);
            Assert.All(allLeafResults, result => Assert.True(result));
        }

        /// <summary>
        /// Tests numeric comparison evaluation where the left value is not less than the right value.
        /// Expected outcome is that the evaluation returns false.
        /// </summary>
        [Fact]
        public void Parse_EvaluationNumericComparison_LessThan_ReturnsFalse()
        {
            // Arrange
            // Input format: '3.5'<'2.5' should evaluate to false.
            string input = "'3.5'<'2.5'";
            bool doEvaluate = true;

            // Act
            ConditionNode root = ConditionNode.Parse(input, doEvaluate);

            // Assert
            // The numeric comparison 3.5 < 2.5 should yield false.
            var allLeafResults = root.OfType<ConditionNode>().Select(n => n.Result);
            Assert.All(allLeafResults, result => Assert.False(result));
        }

        /// <summary>
        /// Tests version comparison evaluation where the left version is less than the right version.
        /// Expected outcome is that the evaluation returns true.
        /// </summary>
        [Fact]
        public void Parse_EvaluationVersionComparison_LessThan_ReturnsTrue()
        {
            // Arrange
            // Input format: '1.0.0.0'<'2.0.0.0'
            string input = "'1.0.0.0'<'2.0.0.0'";
            bool doEvaluate = true;

            // Act
            ConditionNode root = ConditionNode.Parse(input, doEvaluate);

            // Assert
            // The version comparison should evaluate to true.
            var allLeafResults = root.OfType<ConditionNode>().Select(n => n.Result);
            Assert.All(allLeafResults, result => Assert.True(result));
        }

        /// <summary>
        /// Tests the ParseAndProcess method to ensure that it combines unevaluated and evaluated nodes correctly by concatenating text with an arrow.
        /// </summary>
        [Fact]
        public void ParseAndProcess_CombinesTextAndResultCorrectly()
        {
            // Arrange
            // Create a simple condition string that can be parsed
            string unevaluated = "'value'";
            // For evaluated, use an expression that will be evaluated; using equality which should be true.
            string evaluated = "'value'='value'";

            // Act
            ConditionNode resultNode = ConditionNode.ParseAndProcess(unevaluated, evaluated);

            // Assert
            // Verify that for each corresponding node, the text is concatenated with "➄" arrow (Unicode character U+2794)
            foreach (var node in resultNode)
            {
                // Only non-empty texts are processed.
                if (!string.IsNullOrEmpty(node.Text))
                {
                    Assert.Contains("➄", node.Text);
                }
            }
        }

        /// <summary>
        /// Tests that the Process method throws an exception when the two provided condition trees have a different number of nodes.
        /// </summary>
        [Fact]
        public void Process_MismatchedNodeCount_ThrowsException()
        {
            // Arrange
            // Create an unevaluated node with one child.
            ConditionNode unevaluated = new ConditionNode
            {
                Operator = ConditionNode.ConditionOperator.AND,
            };
            unevaluated.Children.Add(new ConditionNode { Text = "Child1", Result = true });

            // Create an evaluated node with no children.
            ConditionNode evaluated = new ConditionNode
            {
                Operator = ConditionNode.ConditionOperator.AND,
            };

            // Act & Assert
            Exception ex = Assert.Throws<Exception>(() => ConditionNode.Process(unevaluated, evaluated));
            Assert.Contains("different number of nodes", ex.Message);
        }

        /// <summary>
        /// Tests that the GetEnumerator method returns a flattened sequence of nodes including the root and all of its children.
        /// </summary>
        [Fact]
        public void GetEnumerator_ReturnsProperSequence()
        {
            // Arrange
            // Manually construct a ConditionNode tree.
            ConditionNode root = new ConditionNode { Text = "Root", Result = true };
            var child1 = new ConditionNode { Text = "Child1", Result = true, Parent = root };
            var child2 = new ConditionNode { Text = "Child2", Result = false, Parent = root };
            var grandChild = new ConditionNode { Text = "GrandChild", Result = true, Parent = child1 };
            root.Children.Add(child1);
            root.Children.Add(child2);
            child1.Children.Add(grandChild);

            // Act
            List<ConditionNode> allNodes = root.ToList();

            // Assert
            // Expecting 4 nodes: root, child1, grandChild, and child2 (order depends on enumeration implementation).
            Assert.Equal(4, allNodes.Count);
            Assert.Contains(allNodes, n => n.Text == "Root");
            Assert.Contains(allNodes, n => n.Text == "Child1");
            Assert.Contains(allNodes, n => n.Text == "Child2");
            Assert.Contains(allNodes, n => n.Text == "GrandChild");
        }
    }
}
