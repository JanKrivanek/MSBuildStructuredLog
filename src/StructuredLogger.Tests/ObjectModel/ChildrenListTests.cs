// using System;
// using System.Collections.Generic;
// using System.Collections.Specialized;
// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ChildrenList"/> class.
//     /// </summary>
//     public class ChildrenListTests
//     {
//         private readonly ChildrenList _childrenList;
// 
//         public ChildrenListTests()
//         {
//             _childrenList = new ChildrenList();
//         }
// 
//         /// <summary>
//         /// Test to ensure that RaiseCollectionChanged invokes the CollectionChanged event with Reset action.
//         /// </summary>
//         [Fact]
//         public void RaiseCollectionChanged_WhenInvoked_EventIsRaisedWithResetAction()
//         {
//             // Arrange
//             NotifyCollectionChangedEventArgs? receivedArgs = null;
//             _childrenList.CollectionChanged += (sender, args) => receivedArgs = args;
// 
//             // Act
//             _childrenList.RaiseCollectionChanged();
// 
//             // Assert
//             receivedArgs.Should().NotBeNull("because the event should be raised.");
//             receivedArgs!.Action.Should().Be(NotifyCollectionChangedAction.Reset, "because RaiseCollectionChanged triggers a Reset action.");
//         }
// //  // [Error] (48-31)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.ChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (49-31)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.ChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (52-40)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.ChildrenListTests.TestNode' cannot be used as type parameter 'T' in the generic type or method 'ChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.ChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'.
// //         /// <summary>
// //         /// Test to ensure that FindNode returns the correct node when a matching node exists.
// //         /// </summary>
// //         [Fact]
// //         public void FindNode_WhenMatchingNodeExists_ReturnsTheNode()
// //         {
// //             // Arrange
// //             var expectedNode = new TestNode("TestNode");
// //             _childrenList.Add(expectedNode);
// //             _childrenList.Add(new TestNode("OtherNode"));
// // 
// //             // Act
// //             var result = _childrenList.FindNode<TestNode>("TestNode");
// // 
// //             // Assert
// //             result.Should().Be(expectedNode, "because the node with matching title should be found.");
// //         }
// //  // [Error] (65-31)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.ChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (66-31)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.ChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (69-40)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.ChildrenListTests.TestNode' cannot be used as type parameter 'T' in the generic type or method 'ChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.ChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'.
//         /// <summary>
//         /// Test to ensure that FindNode returns default (null) when no matching node exists.
//         /// </summary>
//         [Fact]
//         public void FindNode_WhenNoMatchingNodeExists_ReturnsNull()
//         {
//             // Arrange
//             _childrenList.Add(new TestNode("Node1"));
//             _childrenList.Add(new TestNode("Node2"));
// 
//             // Act
//             var result = _childrenList.FindNode<TestNode>("NonExistent");
// 
//             // Assert
//             result.Should().BeNull("because no node with the given title exists.");
//         }
// 
// #if !NET8_0_OR_GREATER
//         /// <summary>
//         /// Test to ensure that EnsureCapacity sets the internal capacity of the list.
//         /// </summary>
//         [Fact]
//         public void EnsureCapacity_WhenCalled_SetsCapacityCorrectly()
//         {
//             // Arrange
//             int newCapacity = 50;
//             // Act
//             _childrenList.EnsureCapacity(newCapacity);
// 
//             // Assert
//             _childrenList.Capacity.Should().Be(newCapacity, "because EnsureCapacity should update the Capacity property.");
//         }
// // #endif // [Error] (95-23)CS0534 'ChildrenListTests.TestNode' does not implement inherited abstract member 'BaseNode.GetFullText()' // [Error] (99-17)CS0103 The name 'Title' does not exist in the current context
// // 
// //         /// <summary>
// //         /// A minimal test implementation of BaseNode used for unit testing purposes.
// //         /// </summary>
// //         private class TestNode : BaseNode
// //         {
// //             public TestNode(string title)
// //             {
// //                 Title = title;
// //             }
// //  // [Error] (102-50)CS0103 The name 'Title' does not exist in the current context
// //             public override string ToString() => Title;
// //         }
// //     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="CacheByNameChildrenList"/> class.
//     /// </summary>
//     public class CacheByNameChildrenListTests
//     {
//         private readonly CacheByNameChildrenList _cacheList;
// 
//         public CacheByNameChildrenListTests()
//         {
//             _cacheList = new CacheByNameChildrenList();
//         }
// //  // [Error] (126-28)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (129-40)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' cannot be used as type parameter 'T' in the generic type or method 'CacheByNameChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'. // [Error] (130-41)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' cannot be used as type parameter 'T' in the generic type or method 'CacheByNameChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'.
// //         /// <summary>
// //         /// Test to ensure that FindNode caches the found node and returns the same instance on subsequent calls.
// //         /// </summary>
// //         [Fact]
// //         public void FindNode_WhenNodeExists_ReturnsSameInstanceOnSubsequentCalls()
// //         {
// //             // Arrange
// //             var node = new TestNode("CachedNode");
// //             _cacheList.Add(node);
// // 
// //             // Act
// //             var firstCall = _cacheList.FindNode<TestNode>("CachedNode");
// //             var secondCall = _cacheList.FindNode<TestNode>("CachedNode");
// // 
// //             // Assert
// //             firstCall.Should().NotBeNull("because the node exists in the list.");
// //             secondCall.Should().BeSameAs(firstCall, "because the node should be cached and the same instance returned.");
// //         }
// //  // [Error] (145-28)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (148-37)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' cannot be used as type parameter 'T' in the generic type or method 'CacheByNameChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'.
//         /// <summary>
//         /// Test to ensure that FindNode is case-insensitive when searching by name.
//         /// </summary>
//         [Fact]
//         public void FindNode_WhenCalledWithDifferentCasing_ReturnsNode()
//         {
//             // Arrange
//             var node = new TestNode("CaseTest");
//             _cacheList.Add(node);
// 
//             // Act
//             var result = _cacheList.FindNode<TestNode>("casetest");
// 
//             // Assert
//             result.Should().NotBeNull("because title matching should be case-insensitive.");
//             result.Should().Be(node, "because the node should be matched regardless of case.");
//         }
// //  // [Error] (162-28)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (163-28)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (166-37)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' cannot be used as type parameter 'T' in the generic type or method 'CacheByNameChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'.
// //         /// <summary>
// //         /// Test to ensure that FindNode returns default (null) when no matching node exists.
// //         /// </summary>
// //         [Fact]
// //         public void FindNode_WhenNoMatchingNodeExists_ReturnsNull()
// //         {
// //             // Arrange
// //             _cacheList.Add(new TestNode("Node1"));
// //             _cacheList.Add(new TestNode("Node2"));
// // 
// //             // Act
// //             var result = _cacheList.FindNode<TestNode>("NonExistent");
// // 
// //             // Assert
// //             result.Should().BeNull("because no matching node was found.");
// //         }
// //  // [Error] (182-28)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNodeDerived' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (185-37)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' cannot be used as type parameter 'T' in the generic type or method 'CacheByNameChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.CacheByNameChildrenListTests.TestNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'.
//         /// <summary>
//         /// Test to ensure that FindNode returns null when node of the requested type does not exist,
//         /// even if a node with a matching title exists of a different type.
//         /// </summary>
//         [Fact]
//         public void FindNode_WhenNodeExistsButDifferentType_ReturnsNull()
//         {
//             // Arrange
//             // Add a node of type TestNodeDerived instead of TestNode.
//             var derivedNode = new TestNodeDerived("DerivedNode");
//             _cacheList.Add(derivedNode);
// 
//             // Act
//             var result = _cacheList.FindNode<TestNode>("DerivedNode");
// 
//             // Assert
//             result.Should().BeNull("because the node exists but is not of the requested type.");
//         }
// //  // [Error] (194-23)CS0534 'CacheByNameChildrenListTests.TestNode' does not implement inherited abstract member 'BaseNode.GetFullText()' // [Error] (198-17)CS0103 The name 'Title' does not exist in the current context
// //         /// <summary>
// //         /// A minimal test implementation of BaseNode used for unit testing purposes.
// //         /// </summary>
// //         private class TestNode : BaseNode
// //         {
// //             public TestNode(string title)
// //             {
// //                 Title = title;
// //             }
// //  // [Error] (201-50)CS0103 The name 'Title' does not exist in the current context
// //             public override string ToString() => Title;
// //         }
// // 
//         /// <summary>
//         /// A minimal derived test implementation of BaseNode used for negative type matching tests.
//         /// </summary>
//         private class TestNodeDerived : TestNode
//         {
//             public TestNodeDerived(string title) : base(title)
//             {
//             }
//         }
//     }
// }