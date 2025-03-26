using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// A fake implementation of BaseNode to be used for testing purposes.
    /// Assumes that BaseNode has an abstract or virtual property "Title".
    /// </summary>
    public class FakeNode : BaseNode
    {
        public FakeNode(string title)
        {
            Title = title;
        }

        public override string Title { get; }
    }

    /// <summary>
    /// Unit tests for the <see cref="ChildrenList"/> class.
    /// </summary>
    public class ChildrenListTests
    {
        /// <summary>
        /// Tests that the FindNode method returns the expected node when a matching node exists.
        /// </summary>
        [Fact]
        public void FindNode_WhenMatchingNodeExists_ReturnsNode()
        {
            // Arrange
            var expectedTitle = "TestNode";
            var node = new FakeNode(expectedTitle);
            var childrenList = new ChildrenList(new List<BaseNode> { node });

            // Act
            var result = childrenList.FindNode<FakeNode>(expectedTitle);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTitle, result.Title);
        }

        /// <summary>
        /// Tests that the FindNode method returns default (null) when no matching node exists.
        /// </summary>
        [Fact]
        public void FindNode_WhenNoMatchingNodeExists_ReturnsDefault()
        {
            // Arrange
            var childrenList = new ChildrenList(new List<BaseNode>());

            // Act
            var result = childrenList.FindNode<FakeNode>("NonExistent");

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that the FindNode method correctly handles a node with a null title and searching for a null name.
        /// </summary>
        [Fact]
        public void FindNode_WhenNodeTitleIsNullAndSearchedWithNull_ReturnsNode()
        {
            // Arrange
            var node = new FakeNode(null);
            var childrenList = new ChildrenList(new List<BaseNode> { node });

            // Act
            var result = childrenList.FindNode<FakeNode>(null);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Title);
        }

        /// <summary>
        /// Tests that the RaiseCollectionChanged method invokes the CollectionChanged event.
        /// </summary>
        [Fact]
        public void RaiseCollectionChanged_WhenCalled_InvokesEvent()
        {
            // Arrange
            var childrenList = new ChildrenList();
            bool eventFired = false;
            childrenList.CollectionChanged += (sender, args) =>
            {
                eventFired = true;
                Assert.Equal(NotifyCollectionChangedAction.Reset, args.Action);
                Assert.Same(childrenList, sender);
            };

            // Act
            childrenList.RaiseCollectionChanged();

            // Assert
            Assert.True(eventFired);
        }

#if !NET8_0_OR_GREATER
        /// <summary>
        /// Tests that the EnsureCapacity method sets the capacity correctly.
        /// This test is only applicable when the method is available.
        /// </summary>
        [Fact]
        public void EnsureCapacity_WhenCalled_SetsCapacity()
        {
            // Arrange
            int expectedCapacity = 50;
            var childrenList = new ChildrenList();

            // Act
            childrenList.EnsureCapacity(expectedCapacity);

            // Assert
            Assert.True(childrenList.Capacity >= expectedCapacity);
        }
#endif
    }

    /// <summary>
    /// Unit tests for the <see cref="CacheByNameChildrenList"/> class.
    /// </summary>
    public class CacheByNameChildrenListTests
    {
        /// <summary>
        /// Tests that the overridden FindNode method returns the expected node and caches it.
        /// Subsequent calls with the same type and name return the same instance.
        /// </summary>
        [Fact]
        public void FindNode_WhenCalledMultipleTimes_ReturnsCachedInstance()
        {
            // Arrange
            var expectedTitle = "CachedNode";
            var node = new FakeNode(expectedTitle);
            var cacheList = new CacheByNameChildrenList(new List<BaseNode> { node });

            // Act
            var firstCall = cacheList.FindNode<FakeNode>(expectedTitle);
            var secondCall = cacheList.FindNode<FakeNode>(expectedTitle);

            // Assert
            Assert.NotNull(firstCall);
            Assert.Same(firstCall, secondCall);
        }

        /// <summary>
        /// Tests that the FindNode method returns default (null) when no matching node exists.
        /// </summary>
        [Fact]
        public void FindNode_WhenNoMatchingNodeExists_ReturnsDefault()
        {
            // Arrange
            var cacheList = new CacheByNameChildrenList(new List<BaseNode>());

            // Act
            var result = cacheList.FindNode<FakeNode>("NonExistent");

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that the FindNode method is case-insensitive when matching node titles.
        /// </summary>
        [Fact]
        public void FindNode_WhenCalledWithDifferentCase_ReturnsNode()
        {
            // Arrange
            var originalTitle = "CaseSensitiveNode";
            var node = new FakeNode(originalTitle);
            var cacheList = new CacheByNameChildrenList(new List<BaseNode> { node });

            // Act
            var result = cacheList.FindNode<FakeNode>(originalTitle.ToLower());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(originalTitle, result.Title);
        }

        /// <summary>
        /// Tests that the FindNode method handles nodes with null titles correctly.
        /// </summary>
        [Fact]
        public void FindNode_WhenNodeTitleIsNullAndSearchedWithNull_ReturnsNode()
        {
            // Arrange
            var node = new FakeNode(null);
            var cacheList = new CacheByNameChildrenList(new List<BaseNode> { node });

            // Act
            var result = cacheList.FindNode<FakeNode>(null);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Title);
        }
    }
}
