// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.Collections.Generic;
// using System.Collections.Specialized;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// A dummy node class derived from BaseNode for testing purposes.
//     /// </summary>
//     public class DummyNode : BaseNode
//     {
//         /// <summary>
//         /// Initializes a new instance of the <see cref="DummyNode"/> class with the specified title.
//         /// </summary>
//         /// <param name="title">The title of the node.</param>
//         public DummyNode(string title)
//         {
//             Title = title;
//         }
// //  // [Error] (27-32)CS0115 'DummyNode.Title': no suitable method found to override
// //         /// <summary>
// //         /// Gets the title of the node.
// //         /// </summary>
// //         public override string Title { get; }
// //     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="ChildrenList"/> class.
//     /// </summary>
//     public class ChildrenListTests
//     {
//         private readonly ChildrenList _childrenList;
// 
//         /// <summary>
//         /// Initializes a new instance of the <see cref="ChildrenListTests"/> class.
//         /// </summary>
//         public ChildrenListTests()
//         {
//             _childrenList = new ChildrenList();
//         }
// 
//         /// <summary>
//         /// Tests that the default constructor initializes an empty list.
//         /// </summary>
//         [Fact]
//         public void Constructor_Default_InitializesEmptyList()
//         {
//             // Arrange & Act
//             var childrenList = new ChildrenList();
// 
//             // Assert
//             childrenList.Should().BeEmpty("a new list should have no elements");
//         }
// 
//         /// <summary>
//         /// Tests that the constructor with capacity initializes an empty list with the specified capacity.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithCapacity_InitializesEmptyListWithSpecifiedCapacity()
//         {
//             // Arrange
//             int capacity = 10;
// 
//             // Act
//             var childrenList = new ChildrenList(capacity);
// 
//             // Assert
//             childrenList.Capacity.Should().BeGreaterOrEqualTo(capacity, "the capacity should be at least as specified");
//             childrenList.Should().BeEmpty("no elements should be added by this constructor");
//         }
// //  // [Error] (89-49)CS1503 Argument 1: cannot convert from 'System.Collections.Generic.List<Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode>' to 'int'
// //         /// <summary>
// //         /// Tests that the constructor with initial children adds elements correctly.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithChildren_AddsElementsToList()
// //         {
// //             // Arrange
// //             var nodes = new List<BaseNode>
// //             {
// //                 new DummyNode("First"),
// //                 new DummyNode("Second")
// //             };
// // 
// //             // Act
// //             var childrenList = new ChildrenList(nodes);
// // 
// //             // Assert
// //             childrenList.Should().HaveCount(2, "the list should contain the provided children");
// //             childrenList[0].Title.Should().Be("First");
// //             childrenList[1].Title.Should().Be("Second");
// //         }
// // 
//         /// <summary>
//         /// Tests that RaiseCollectionChanged invokes the subscribed event with reset action.
//         /// </summary>
//         [Fact]
//         public void RaiseCollectionChanged_WhenEventSubscribed_EventRaisedWithResetAction()
//         {
//             // Arrange
//             var eventRaised = false;
//             NotifyCollectionChangedEventArgs? receivedArgs = null;
//             _childrenList.CollectionChanged += (sender, args) =>
//             {
//                 eventRaised = true;
//                 receivedArgs = args;
//             };
// 
//             // Act
//             _childrenList.RaiseCollectionChanged();
// 
//             // Assert
//             eventRaised.Should().BeTrue("the event should have been raised when invoking RaiseCollectionChanged");
//             receivedArgs.Should().NotBeNull("event args should not be null");
//             receivedArgs!.Action.Should().Be(NotifyCollectionChangedAction.Reset, "the action should be Reset as per implementation");
//         }
// //  // [Error] (128-40)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' cannot be used as type parameter 'T' in the generic type or method 'ChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'.
// //         /// <summary>
// //         /// Tests that FindNode returns null when the list is empty.
// //         /// </summary>
// //         [Fact]
// //         public void FindNode_WhenListIsEmpty_ReturnsNull()
// //         {
// //             // Act
// //             var result = _childrenList.FindNode<DummyNode>("AnyName");
// // 
// //             // Assert
// //             result.Should().BeNull("no node exists in an empty list");
// //         }
// //  // [Error] (143-31)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (144-31)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (147-40)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' cannot be used as type parameter 'T' in the generic type or method 'ChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'.
//         /// <summary>
//         /// Tests that FindNode returns the first matching node based on the provided name.
//         /// </summary>
//         [Fact]
//         public void FindNode_WhenMatchingNodeExists_ReturnsCorrectNode()
//         {
//             // Arrange
//             var node1 = new DummyNode("First");
//             var node2 = new DummyNode("Second");
//             _childrenList.Add(node1);
//             _childrenList.Add(node2);
// 
//             // Act
//             var result = _childrenList.FindNode<DummyNode>("Second");
// 
//             // Assert
//             result.Should().NotBeNull("a matching node exists");
//             result.Should().Be(node2, "FindNode should return the first node that matches the name");
//         }
// //  // [Error] (162-31)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (165-40)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' cannot be used as type parameter 'T' in the generic type or method 'ChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'.
// //         /// <summary>
// //         /// Tests that FindNode returns null when no node matches the given name.
// //         /// </summary>
// //         [Fact]
// //         public void FindNode_WhenNoMatchingNodeExists_ReturnsNull()
// //         {
// //             // Arrange
// //             var node = new DummyNode("Existing");
// //             _childrenList.Add(node);
// // 
// //             // Act
// //             var result = _childrenList.FindNode<DummyNode>("NonExisting");
// // 
// //             // Assert
// //             result.Should().BeNull("no node matches the given name");
// //         }
// // 
// #if !NET8_0_OR_GREATER
//         /// <summary>
//         /// Tests that EnsureCapacity sets the Capacity property correctly.
//         /// </summary>
//         [Fact]
//         public void EnsureCapacity_WhenCalled_SetsCapacityCorrectly()
//         {
//             // Arrange
//             int newCapacity = 50;
// 
//             // Act
//             _childrenList.EnsureCapacity(newCapacity);
// 
//             // Assert
//             _childrenList.Capacity.Should().BeGreaterOrEqualTo(newCapacity, "EnsureCapacity should set the capacity to the provided value");
//         }
// #endif
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="CacheByNameChildrenList"/> class.
//     /// </summary>
//     public class CacheByNameChildrenListTests
//     {
//         private readonly CacheByNameChildrenList _cacheByNameChildrenList;
// 
//         /// <summary>
//         /// Initializes a new instance of the <see cref="CacheByNameChildrenListTests"/> class.
//         /// </summary>
//         public CacheByNameChildrenListTests()
//         {
//             _cacheByNameChildrenList = new CacheByNameChildrenList();
//         }
// 
//         /// <summary>
//         /// Tests that the default constructor initializes an empty list.
//         /// </summary>
//         [Fact]
//         public void Constructor_Default_InitializesEmptyList()
//         {
//             // Arrange & Act
//             var list = new CacheByNameChildrenList();
// 
//             // Assert
//             list.Should().BeEmpty("a new CacheByNameChildrenList should have no elements");
//         }
// 
//         /// <summary>
//         /// Tests that the constructor with capacity initializes an empty list with the specified capacity.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithCapacity_InitializesEmptyListWithSpecifiedCapacity()
//         {
//             // Arrange
//             int capacity = 20;
// 
//             // Act
//             var list = new CacheByNameChildrenList(capacity);
// 
//             // Assert
//             list.Capacity.Should().BeGreaterOrEqualTo(capacity, "the list capacity should be at least as the specified capacity");
//             list.Should().BeEmpty("no elements should be added by the constructor");
//         }
// //  // [Error] (249-52)CS1503 Argument 1: cannot convert from 'System.Collections.Generic.List<Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode>' to 'int'
// //         /// <summary>
// //         /// Tests that the constructor with initial children adds elements correctly.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithChildren_AddsElementsToList()
// //         {
// //             // Arrange
// //             var nodes = new List<BaseNode>
// //             {
// //                 new DummyNode("Alpha"),
// //                 new DummyNode("Beta")
// //             };
// // 
// //             // Act
// //             var list = new CacheByNameChildrenList(nodes);
// // 
// //             // Assert
// //             list.Should().HaveCount(2, "the list should contain the provided children");
// //             list[0].Title.Should().Be("Alpha");
// //             list[1].Title.Should().Be("Beta");
// //         }
// //  // [Error] (264-51)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' cannot be used as type parameter 'T' in the generic type or method 'CacheByNameChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'.
//         /// <summary>
//         /// Tests that the overridden FindNode returns null when the list is empty.
//         /// </summary>
//         [Fact]
//         public void FindNode_WhenListIsEmpty_ReturnsNull()
//         {
//             // Act
//             var result = _cacheByNameChildrenList.FindNode<DummyNode>("AnyName");
// 
//             // Assert
//             result.Should().BeNull("no node exists in an empty list");
//         }
// //  // [Error] (278-42)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (281-54)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' cannot be used as type parameter 'T' in the generic type or method 'CacheByNameChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'. // [Error] (282-55)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' cannot be used as type parameter 'T' in the generic type or method 'CacheByNameChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'.
// //         /// <summary>
// //         /// Tests that the overridden FindNode returns the correct node and caches it.
// //         /// </summary>
// //         [Fact]
// //         public void FindNode_WhenMatchingNodeExists_ReturnsAndCachesNode()
// //         {
// //             // Arrange
// //             var node1 = new DummyNode("CacheTest");
// //             _cacheByNameChildrenList.Add(node1);
// // 
// //             // Act
// //             var firstCall = _cacheByNameChildrenList.FindNode<DummyNode>("CacheTest");
// //             var secondCall = _cacheByNameChildrenList.FindNode<DummyNode>("CacheTest");
// // 
// //             // Assert
// //             firstCall.Should().NotBeNull("a matching node exists");
// //             firstCall.Should().Be(node1, "the returned node should be the one added to the list");
// //             secondCall.Should().BeSameAs(firstCall, "the second call should retrieve the node from the cache");
// //         }
// //  // [Error] (298-42)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (301-51)CS0311 The type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' cannot be used as type parameter 'T' in the generic type or method 'CacheByNameChildrenList.FindNode<T>(string)'. There is no implicit reference conversion from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.DummyNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'.
//         /// <summary>
//         /// Tests that the overridden FindNode returns null when no matching node exists.
//         /// </summary>
//         [Fact]
//         public void FindNode_WhenNoMatchingNodeExists_ReturnsNull()
//         {
//             // Arrange
//             var node = new DummyNode("ExistingNode");
//             _cacheByNameChildrenList.Add(node);
// 
//             // Act
//             var result = _cacheByNameChildrenList.FindNode<DummyNode>("NonExistent");
// 
//             // Assert
//             result.Should().BeNull("no node matches the specified name");
//         }
//     }
// }