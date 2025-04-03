// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Microsoft.Build.Logging.StructuredLogger.UnitTests;
// using StructuredLogViewer;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// A test concrete implementation of BaseNode for testing purposes.
//     /// </summary>
//     public class TestNode : BaseNode
//     {
//         private readonly string _toStringValue;
//         public TestNode(string toStringValue = "TestNode")
//         {
//             _toStringValue = toStringValue;
//         }
// 
//         public override string ToString() => _toStringValue;
// //         // Expose ResetSearchResultStatus for testing if needed. // [Error] (25-59)CS0117 'BaseNode' does not contain a definition for 'ResetSearchResultStatus'
// //         public new void ResetSearchResultStatus() => base.ResetSearchResultStatus();
// //     }
// 
//     /// <summary>
//     /// A fake implementation of TreeNode for testing EnumerateSiblingsCycle.
//     /// </summary>
//     public class FakeTreeNode : TreeNode
//     {
//         private readonly List<BaseNode> _children = new List<BaseNode>();
// //         public override IList<BaseNode> Children => _children; // [Error] (34-41)CS0506 'FakeTreeNode.Children': cannot override inherited member 'TreeNode.Children' because it is not marked virtual, abstract, or override
// //  // [Error] (36-29)CS0115 'FakeTreeNode.FindChildIndex(BaseNode)': no suitable method found to override
//         public override int FindChildIndex(BaseNode node)
//         {
//             return _children.IndexOf(node);
//         }
// //  // [Error] (43-18)CS1061 'BaseNode' does not contain a definition for 'Parent' and no accessible extension method 'Parent' accepting a first argument of type 'BaseNode' could be found (are you missing a using directive or an assembly reference?)
// //         public void AddChild(BaseNode node)
// //         {
// //             node.Parent = this;
// //             _children.Add(node);
// //         }
// //     }
// //  // [Error] (56-22)CS0117 'BaseNode' does not contain a definition for 'ClearSelectedNode'
// //     /// <summary>
// //     /// Unit tests for the <see cref = "BaseNode"/> class.
// //     /// </summary>
// //     public class BaseNodeTests
// //     {
// //         public BaseNodeTests()
// //         {
// //             // Clear static selected node before each test.
// //             BaseNode.ClearSelectedNode();
// //         }
// //  // [Error] (69-18)CS1061 'TestNode' does not contain a definition for 'Parent' and no accessible extension method 'Parent' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (71-18)CS1061 'TestNode' does not contain a definition for 'Parent' and no accessible extension method 'Parent' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests the Parent property getter and setter.
// //         /// </summary>
// //         [Fact]
// //         public void Parent_SetAndGet_ReturnsCorrectValue()
// //         {
// //             // Arrange
// //             var node = new TestNode();
// //             var parentNode = new FakeTreeNode();
// //             // Act
// //             node.Parent = parentNode;
// //             // Assert
// //             node.Parent.Should().Be(parentNode);
// //         }
// //  // [Error] (83-33)CS1061 'TestNode' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the TypeName property returns the default name.
// //         /// </summary>
// //         [Fact]
// //         public void TypeName_Default_ReturnsBaseNode()
// //         {
// //             // Arrange
// //             var node = new TestNode();
// //             // Act
// //             var typeName = node.TypeName;
// //             // Assert
// //             typeName.Should().Be("BaseNode");
// //         }
// //  // [Error] (98-30)CS1061 'TestNode' does not contain a definition for 'Title' and no accessible extension method 'Title' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the Title property returns the value from ToString.
// //         /// </summary>
// //         [Fact]
// //         public void Title_Default_ReturnsToStringValue()
// //         {
// //             // Arrange
// //             var expected = "MyCustomNode";
// //             var node = new TestNode(expected);
// //             // Act
// //             var title = node.Title;
// //             // Assert
// //             title.Should().Be(expected);
// //         }
// //  // [Error] (113-33)CS1061 'TestNode' does not contain a definition for 'GetFullText' and no accessible extension method 'GetFullText' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that GetFullText returns text with normalized newline.
// //         /// </summary>
// //         [Fact]
// //         public void GetFullText_NormalizesNewlines()
// //         {
// //             // Arrange
// //             var input = "Line1\nLine2";
// //             var node = new TestNode(input);
// //             // Act
// //             var fullText = node.GetFullText();
// //             // Assert
// //             fullText.Should().Be(input.Replace("\n", Environment.NewLine));
// //         }
// //  // [Error] (126-18)CS1061 'TestNode' does not contain a definition for 'IsSelected' and no accessible extension method 'IsSelected' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (127-18)CS1061 'TestNode' does not contain a definition for 'IsSelected' and no accessible extension method 'IsSelected' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (129-22)CS0117 'BaseNode' does not contain a definition for 'ClearSelectedNode' // [Error] (131-18)CS1061 'TestNode' does not contain a definition for 'IsSelected' and no accessible extension method 'IsSelected' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests ClearSelectedNode resets selection.
// //         /// </summary>
// //         [Fact]
// //         public void ClearSelectedNode_ResetsSelection()
// //         {
// //             // Arrange
// //             var node = new TestNode();
// //             node.IsSelected = true;
// //             node.IsSelected.Should().BeTrue("because the node was selected");
// //             // Act
// //             BaseNode.ClearSelectedNode();
// //             // Assert
// //             node.IsSelected.Should().BeFalse("because ClearSelectedNode should clear selection");
// //         }
// //  // [Error] (144-19)CS1061 'TestNode' does not contain a definition for 'IsSelected' and no accessible extension method 'IsSelected' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (146-19)CS1061 'TestNode' does not contain a definition for 'IsSelected' and no accessible extension method 'IsSelected' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (147-19)CS1061 'TestNode' does not contain a definition for 'IsSelected' and no accessible extension method 'IsSelected' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (149-19)CS1061 'TestNode' does not contain a definition for 'IsSelected' and no accessible extension method 'IsSelected' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (151-19)CS1061 'TestNode' does not contain a definition for 'IsSelected' and no accessible extension method 'IsSelected' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests setting IsSelected property updates the selection correctly.
// //         /// </summary>
// //         [Fact]
// //         public void IsSelected_SetTrue_UpdatesSelection()
// //         {
// //             // Arrange
// //             var node1 = new TestNode("Node1");
// //             var node2 = new TestNode("Node2");
// //             // Act
// //             node1.IsSelected = true;
// //             // Assert
// //             node1.IsSelected.Should().BeTrue();
// //             node2.IsSelected.Should().BeFalse();
// //             // Act
// //             node1.IsSelected = false;
// //             // Assert
// //             node1.IsSelected.Should().BeFalse();
// //         }
// //  // [Error] (163-18)CS1061 'TestNode' does not contain a definition for 'IsSearchResult' and no accessible extension method 'IsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (164-18)CS1061 'TestNode' does not contain a definition for 'ContainsSearchResult' and no accessible extension method 'ContainsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (166-18)CS1061 'TestNode' does not contain a definition for 'IsSearchResult' and no accessible extension method 'IsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (167-18)CS1061 'TestNode' does not contain a definition for 'ContainsSearchResult' and no accessible extension method 'ContainsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (169-18)CS1061 'TestNode' does not contain a definition for 'IsSearchResult' and no accessible extension method 'IsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (170-18)CS1061 'TestNode' does not contain a definition for 'ContainsSearchResult' and no accessible extension method 'ContainsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (172-18)CS1061 'TestNode' does not contain a definition for 'IsSearchResult' and no accessible extension method 'IsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (173-18)CS1061 'TestNode' does not contain a definition for 'ContainsSearchResult' and no accessible extension method 'ContainsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests setting and getting IsSearchResult and ContainsSearchResult properties.
// //         /// </summary>
// //         [Fact]
// //         public void SearchResultProperties_SetAndGet_ReturnsCorrectValues()
// //         {
// //             // Arrange
// //             var node = new TestNode();
// //             // Act
// //             node.IsSearchResult = true;
// //             node.ContainsSearchResult = true;
// //             // Assert
// //             node.IsSearchResult.Should().BeTrue();
// //             node.ContainsSearchResult.Should().BeTrue();
// //             // Act
// //             node.IsSearchResult = false;
// //             node.ContainsSearchResult = false;
// //             // Assert
// //             node.IsSearchResult.Should().BeFalse();
// //             node.ContainsSearchResult.Should().BeFalse();
// //         }
// //  // [Error] (184-18)CS1061 'TestNode' does not contain a definition for 'IsSearchResult' and no accessible extension method 'IsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (185-18)CS1061 'TestNode' does not contain a definition for 'ContainsSearchResult' and no accessible extension method 'ContainsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (187-18)CS1061 'TestNode' does not contain a definition for 'IsSearchResult' and no accessible extension method 'IsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (188-18)CS1061 'TestNode' does not contain a definition for 'ContainsSearchResult' and no accessible extension method 'ContainsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (192-18)CS1061 'TestNode' does not contain a definition for 'IsSearchResult' and no accessible extension method 'IsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?) // [Error] (193-18)CS1061 'TestNode' does not contain a definition for 'ContainsSearchResult' and no accessible extension method 'ContainsSearchResult' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests ResetSearchResultStatus clears search result flags if they are set.
// //         /// </summary>
// //         [Fact]
// //         public void ResetSearchResultStatus_WhenFlagsSet_ClearsFlags()
// //         {
// //             // Arrange
// //             var node = new TestNode();
// //             node.IsSearchResult = true;
// //             node.ContainsSearchResult = true;
// //             // Pre-Assert
// //             node.IsSearchResult.Should().BeTrue();
// //             node.ContainsSearchResult.Should().BeTrue();
// //             // Act
// //             node.ResetSearchResultStatus();
// //             // Assert
// //             node.IsSearchResult.Should().BeFalse();
// //             node.ContainsSearchResult.Should().BeFalse();
// //         }
// //  // [Error] (206-17)CS0117 'TestNode' does not contain a definition for 'Parent' // [Error] (210-17)CS0117 'TestNode' does not contain a definition for 'Parent' // [Error] (213-37)CS1061 'TestNode' does not contain a definition for 'GetRoot' and no accessible extension method 'GetRoot' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests GetRoot returns the top-most parent in the tree.
// //         /// </summary>
// //         [Fact]
// //         public void GetRoot_WithParentChain_ReturnsRootNode()
// //         {
// //             // Arrange
// //             var root = new TestNode("Root");
// //             var child = new TestNode("Child")
// //             {
// //                 Parent = root
// //             };
// //             var grandChild = new TestNode("GrandChild")
// //             {
// //                 Parent = child
// //             };
// //             // Act
// //             var result = grandChild.GetRoot();
// //             // Assert
// //             result.Should().Be(root);
// //         }
// //  // [Error] (228-17)CS0117 'TestNode' does not contain a definition for 'Parent' // [Error] (232-17)CS0117 'TestNode' does not contain a definition for 'Parent' // [Error] (235-36)CS1061 'TestNode' does not contain a definition for 'GetParentChainExcludingThis' and no accessible extension method 'GetParentChainExcludingThis' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests GetParentChainExcludingThis returns the correct chain.
// //         /// </summary>
// //         [Fact]
// //         public void GetParentChainExcludingThis_WithParentChain_ReturnsCorrectOrder()
// //         {
// //             // Arrange
// //             var root = new TestNode("Root");
// //             var child = new TestNode("Child")
// //             {
// //                 Parent = root
// //             };
// //             var grandChild = new TestNode("GrandChild")
// //             {
// //                 Parent = child
// //             };
// //             // Act
// //             var chain = grandChild.GetParentChainExcludingThis().ToList();
// //             // Assert
// //             chain.Should().HaveCount(2);
// //             chain[0].Should().Be(root);
// //             chain[1].Should().Be(child);
// //         }
// //  // [Error] (252-17)CS0117 'TestNode' does not contain a definition for 'Parent' // [Error] (256-17)CS0117 'TestNode' does not contain a definition for 'Parent' // [Error] (259-36)CS1061 'TestNode' does not contain a definition for 'GetParentChainIncludingThis' and no accessible extension method 'GetParentChainIncludingThis' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests GetParentChainIncludingThis returns the correct chain.
// //         /// </summary>
// //         [Fact]
// //         public void GetParentChainIncludingThis_WithParentChain_ReturnsCorrectOrder()
// //         {
// //             // Arrange
// //             var root = new TestNode("Root");
// //             var child = new TestNode("Child")
// //             {
// //                 Parent = root
// //             };
// //             var grandChild = new TestNode("GrandChild")
// //             {
// //                 Parent = child
// //             };
// //             // Act
// //             var chain = grandChild.GetParentChainIncludingThis().ToList();
// //             // Assert
// //             chain.Should().HaveCount(2);
// //             // Expected order: child, grandChild
// //             chain[0].Should().Be(child);
// //             chain[1].Should().Be(grandChild);
// //         }
// //  // [Error] (277-17)CS0117 'TestNode' does not contain a definition for 'Parent' // [Error] (281-17)CS0117 'TestNode' does not contain a definition for 'Parent' // [Error] (284-38)CS7036 There is no argument given that corresponds to the required parameter 'predicate' of 'BaseNode.GetNearestParent<T>(Predicate<T>)'
// //         /// <summary>
// //         /// Tests GetNearestParent returns the nearest parent of the given type.
// //         /// </summary>
// //         [Fact]
// //         public void GetNearestParent_WhenParentExists_ReturnsNearestMatchingParent()
// //         {
// //             // Arrange
// //             var root = new TestNode("Root");
// //             var child = new TestNode("Child")
// //             {
// //                 Parent = root
// //             };
// //             var grandChild = new TestNode("GrandChild")
// //             {
// //                 Parent = child
// //             };
// //             // Act
// //             var nearest = grandChild.GetNearestParent<TestNode>();
// //             // Assert
// //             nearest.Should().Be(child);
// //         }
// //  // [Error] (299-17)CS0117 'TestNode' does not contain a definition for 'Parent' // [Error] (303-17)CS0117 'TestNode' does not contain a definition for 'Parent'
// //         /// <summary>
// //         /// Tests GetNearestParent with predicate returns the correct parent when predicate is satisfied.
// //         /// </summary>
// //         [Fact]
// //         public void GetNearestParent_WithPredicate_ReturnsMatchingParent()
// //         {
// //             // Arrange
// //             var root = new TestNode("Root");
// //             var child = new TestNode("Child")
// //             {
// //                 Parent = root
// //             };
// //             var grandChild = new TestNode("GrandChild")
// //             {
// //                 Parent = child
// //             };
// //             // Act
// //             var nearest = grandChild.GetNearestParent<TestNode>(t => t.ToString() == "Root");
// //             // Assert
// //             nearest.Should().Be(root);
// //         }
// //  // [Error] (321-17)CS0117 'TestNode' does not contain a definition for 'Parent'
// //         /// <summary>
// //         /// Tests GetNearestParent returns null when no matching parent exists.
// //         /// </summary>
// //         [Fact]
// //         public void GetNearestParent_WhenNoMatchingParentExists_ReturnsNull()
// //         {
// //             // Arrange
// //             var root = new TestNode("Root");
// //             var child = new TestNode("Child")
// //             {
// //                 Parent = root
// //             };
// //             // Act
// //             var result = child.GetNearestParent<TestNode>(t => t.ToString() == "NonExistent");
// //             // Assert
// //             result.Should().BeNull();
// //         }
// //  // [Error] (338-31)CS1061 'TestNode' does not contain a definition for 'GetNearestParentOrSelf' and no accessible extension method 'GetNearestParentOrSelf' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests GetNearestParentOrSelf returns the current node if it matches the type.
// //         /// </summary>
// //         [Fact]
// //         public void GetNearestParentOrSelf_WhenSelfMatches_ReturnsSelf()
// //         {
// //             // Arrange
// //             var node = new TestNode("Self");
// //             // Act
// //             var result = node.GetNearestParentOrSelf<TestNode>();
// //             // Assert
// //             result.Should().Be(node);
// //         }
// //  // [Error] (354-17)CS0117 'TestNode' does not contain a definition for 'Parent' // [Error] (357-32)CS1061 'TestNode' does not contain a definition for 'GetNearestParentOrSelf' and no accessible extension method 'GetNearestParentOrSelf' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests GetNearestParentOrSelf returns the nearest parent when self does not match.
// //         /// </summary>
// //         [Fact]
// //         public void GetNearestParentOrSelf_WhenSelfDoesNotMatch_ReturnsParent()
// //         {
// //             // Arrange
// //             // Since BaseNode is the base type, calling GetNearestParentOrSelf<BaseNode>() on a TestNode will return itself.
// //             var root = new TestNode("Root");
// //             var child = new TestNode("Child")
// //             {
// //                 Parent = root
// //             };
// //             // Act
// //             var result = child.GetNearestParentOrSelf<BaseNode>();
// //             // Assert
// //             result.Should().Be(child);
// //         }
// //  // [Error] (377-40)CS1061 'TestNode' does not contain a definition for 'EnumerateSiblingsCycle' and no accessible extension method 'EnumerateSiblingsCycle' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests EnumerateSiblingsCycle returns the correct cycle of siblings.
// //         /// </summary>
// //         [Fact]
// //         public void EnumerateSiblingsCycle_WithParent_ReturnsCycleOfSiblings()
// //         {
// //             // Arrange
// //             var parent = new FakeTreeNode();
// //             var child1 = new TestNode("Child1");
// //             var child2 = new TestNode("Child2");
// //             var child3 = new TestNode("Child3");
// //             parent.AddChild(child1);
// //             parent.AddChild(child2);
// //             parent.AddChild(child3);
// //             // Act
// //             var siblingsCycle = child2.EnumerateSiblingsCycle().ToList();
// //             // Assert
// //             // Expected order starting from child2: child2, child3, child1
// //             siblingsCycle.Should().HaveCount(3);
// //             siblingsCycle[0].Should().Be(child2);
// //             siblingsCycle[1].Should().Be(child3);
// //             siblingsCycle[2].Should().Be(child1);
// //         }
// //  // [Error] (395-38)CS1061 'TestNode' does not contain a definition for 'EnumerateSiblingsCycle' and no accessible extension method 'EnumerateSiblingsCycle' accepting a first argument of type 'TestNode' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests EnumerateSiblingsCycle when node has no parent returns only itself.
// //         /// </summary>
// //         [Fact]
// //         public void EnumerateSiblingsCycle_NoParent_ReturnsSelf()
// //         {
// //             // Arrange
// //             var node = new TestNode("Orphan");
// //             // Act
// //             var siblingsCycle = node.EnumerateSiblingsCycle().ToList();
// //             // Assert
// //             siblingsCycle.Should().HaveCount(1);
// //             siblingsCycle[0].Should().Be(node);
// //         }
// //     }
// // }