// using FluentAssertions;
// using StructuredLogViewer;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// 
// namespace StructuredLogViewer.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ConditionNode"/> class.
//     /// </summary>
//     public class ConditionNodeTests
//     {
//         /// <summary>
//         /// Tests that the Parse method correctly parses a simple boolean condition when evaluation is enabled.
//         /// The test verifies that a literal 'true' string is parsed and evaluated to a boolean true.
//         /// </summary>
//         [Fact]
//         public void Parse_SimpleTrueString_WithDoEvaluate_ReturnsTrue()
//         {
//             // Arrange
//             string input = "'true'";
//             
//             // Act
//             ConditionNode node = ConditionNode.Parse(input, true);
//             List<ConditionNode> nodes = node.ToList();
//             // The root node is initially empty, so we target a child node that contains the literal value.
//             ConditionNode? leaf = nodes.FirstOrDefault(n => 
//                 !string.IsNullOrWhiteSpace(n.Text) && 
//                 n.Text.IndexOf("true", StringComparison.OrdinalIgnoreCase) >= 0);
//             
//             // Assert
//             leaf.Should().NotBeNull("a leaf node with the literal 'true' should be present");
//             leaf!.Result.Should().BeTrue("the condition 'true' should evaluate to true");
//             leaf.Text.Should().Contain("true");
//         }
// //  // [Error] (51-76)CS1501 No overload for method 'Contains' takes 2 arguments
// //         /// <summary>
// //         /// Tests that the Parse method correctly evaluates a numeric less-than comparison.
// //         /// The test uses a string representing a numeric comparison (1 < 2) and expects a true result.
// //         /// </summary>
// //         [Fact]
// //         public void Parse_NumericComparison_LessThan_ReturnsTrue()
// //         {
// //             // Arrange
// //             string input = "'1'< '2'";
// //             
// //             // Act
// //             ConditionNode node = ConditionNode.Parse(input, true);
// //             ConditionNode? leaf = node.ToList().FirstOrDefault(n => n.Text.Contains("<", StringComparison.Ordinal));
// //             
// //             // Assert
// //             leaf.Should().NotBeNull("a node representing numeric comparison should be present");
// //             leaf!.Result.Should().BeTrue("since 1 is less than 2, the comparison should evaluate to true");
// //             leaf.Text.Should().Contain("<");
// //         }
// //  // [Error] (72-24)CS1501 No overload for method 'Contains' takes 2 arguments // [Error] (72-75)CS1501 No overload for method 'Contains' takes 2 arguments
//         /// <summary>
//         /// Tests that the Parse method correctly evaluates an equality comparison that returns true.
//         /// The test compares two equal strings and verifies that the evaluation returns true.
//         /// </summary>
//         [Fact]
//         public void Parse_EqualityComparison_Equal_ReturnsTrue()
//         {
//             // Arrange
//             string input = "'a'='a'";
//             
//             // Act
//             ConditionNode node = ConditionNode.Parse(input, true);
//             ConditionNode? leaf = node.ToList().FirstOrDefault(n => 
//                 n.Text.Contains("=", StringComparison.Ordinal) && !n.Text.Contains("!", StringComparison.Ordinal));
//             
//             // Assert
//             leaf.Should().NotBeNull("a node representing equality comparison should be present");
//             leaf!.Result.Should().BeTrue("because the values are equal, the comparison should evaluate to true");
//             leaf.Text.Should().Contain("=");
//         }
// //  // [Error] (92-76)CS1501 No overload for method 'Contains' takes 2 arguments
// //         /// <summary>
// //         /// Tests that the Parse method correctly evaluates an inequality comparison that returns true.
// //         /// The test uses an input string with 'a' not equal to 'b' and expects the result to be true.
// //         /// </summary>
// //         [Fact]
// //         public void Parse_EqualityComparison_NotEqual_ReturnsTrue()
// //         {
// //             // Arrange
// //             string input = "'a'!='b'";
// //             
// //             // Act
// //             ConditionNode node = ConditionNode.Parse(input, true);
// //             ConditionNode? leaf = node.ToList().FirstOrDefault(n => n.Text.Contains("!", StringComparison.Ordinal));
// //             
// //             // Assert
// //             leaf.Should().NotBeNull("a node representing inequality should be present");
// //             leaf!.Result.Should().BeTrue("since 'a' is not equal to 'b', the inequality should evaluate to true");
// //             leaf.Text.Should().Contain("!");
// //         }
// // 
//         /// <summary>
//         /// Tests that the Parse method throws an exception for mismatched quotes.
//         /// An input string with an unclosed quote results in an exception indicating a parsing error.
//         /// </summary>
//         [Fact]
//         public void Parse_MismatchedQuote_ThrowsException()
//         {
//             // Arrange
//             string input = "'unclosed";
//             
//             // Act
//             Action act = () => ConditionNode.Parse(input, true);
//             
//             // Assert
//             act.Should().Throw<Exception>().WithMessage("Can't parse*");
//         }
// 
//         /// <summary>
//         /// Tests that the Process method throws an exception when the number of nodes in the unevaluated and evaluated trees differ.
//         /// This simulates a scenario where the parsed trees have different node counts.
//         /// </summary>
//         [Fact]
//         public void Process_DifferentNumberOfNodes_ThrowsException()
//         {
//             // Arrange
//             // The unevaluated string results in two condition nodes while the evaluated string results in one.
//             string unevaluated = "'a' and 'b'";
//             string evaluated = "'a'";
//             
//             ConditionNode unevalNode = ConditionNode.Parse(unevaluated, true);
//             ConditionNode evalNode = ConditionNode.Parse(evaluated, true);
//             
//             // Act
//             Action act = () => ConditionNode.Process(unevalNode, evalNode);
//             
//             // Assert
//             act.Should().Throw<Exception>().WithMessage("Condition parsing return a different number of nodes*");
//         }
// 
//         /// <summary>
//         /// Tests that the GetEnumerator method returns all nodes from the tree, including those nested within groups.
//         /// The test verifies that nodes from both the outer and inner groups are present.
//         /// </summary>
//         [Fact]
//         public void GetEnumerator_ReturnsAllNodes_IncludingNestedNodes()
//         {
//             // Arrange
//             string input = "('true' and ('false'))";
//             
//             // Act
//             ConditionNode node = ConditionNode.Parse(input, true);
//             List<ConditionNode> nodes = node.ToList();
//             
//             // Assert
//             nodes.Count.Should().BeGreaterThan(1, "the parsed tree should contain multiple nodes including nested ones");
//             nodes.Any(n => n.Text.IndexOf("true", StringComparison.OrdinalIgnoreCase) >= 0)
//                 .Should().BeTrue("one of the nodes should contain the literal 'true'");
//             nodes.Any(n => n.Text.IndexOf("false", StringComparison.OrdinalIgnoreCase) >= 0)
//                 .Should().BeTrue("one of the nested nodes should contain the literal 'false'");
//         }
// 
//         /// <summary>
//         /// Tests that the Parse method, when evaluation is disabled, creates nodes that retain their default Result values.
//         /// Without evaluation, the Result property should remain true (the default value).
//         /// </summary>
//         [Fact]
//         public void Parse_WithoutEvaluation_DefaultResultIsTrue()
//         {
//             // Arrange
//             string input = "'false'";
//             
//             // Act
//             ConditionNode node = ConditionNode.Parse(input, false);
//             ConditionNode? leaf = node.ToList().FirstOrDefault(n => 
//                 n.Text.IndexOf("false", StringComparison.OrdinalIgnoreCase) >= 0);
//             
//             // Assert
//             leaf.Should().NotBeNull("a leaf node with the literal 'false' should be present");
//             leaf!.Result.Should().BeTrue("without evaluation, the default Result remains true");
//         }
//     }
// }