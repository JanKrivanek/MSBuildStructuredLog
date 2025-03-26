using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Utilities"/> class.
    /// </summary>
    public class UtilitiesTests
    {
        /// <summary>
        /// Tests that BinarySearch returns the correct index when the item exists in the sorted list.
        /// </summary>
        [Fact]
        public void BinarySearch_ItemExists_ReturnsCorrectIndex()
        {
            // Arrange
            var list = new List<int> { 1, 3, 5, 7, 9 };
            int target = 5;

            // Act
            int index = list.BinarySearch(target, x => x);

            // Assert
            Assert.Equal(2, index);
        }

        /// <summary>
        /// Tests that BinarySearch returns the complement of the insertion point when the item does not exist.
        /// </summary>
        [Theory]
        [InlineData(0, ~0)]
        [InlineData(2, ~1)]
        public void BinarySearch_ItemDoesNotExist_ReturnsComplementOfInsertionPoint(int target, int expected)
        {
            // Arrange
            var list = new List<int> { 1, 3, 5, 7, 9 };

            // Act
            int index = list.BinarySearch(target, x => x);

            // Assert
            Assert.Equal(expected, index);
        }

        /// <summary>
        /// Tests BinarySearch on an empty list returns the complement of zero.
        /// </summary>
        [Fact]
        public void BinarySearch_EmptyList_ReturnsComplementZero()
        {
            // Arrange
            var list = new List<int>();
            int target = 10;

            // Act
            int index = list.BinarySearch(target, x => x);

            // Assert
            Assert.Equal(~0, index);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="BatchBlockingCollection{T}"/> class.
    /// </summary>
    public class BatchBlockingCollectionTests
    {
        private readonly int _smallBatchSize = 3;

        /// <summary>
        /// Tests that the Count property reflects items added before batch capacity is reached.
        /// </summary>
        [Fact]
        public void Count_ItemsLessThanBatchSize_ReturnsCorrectCount()
        {
            // Arrange
            var collection = new BatchBlockingCollection<int>(_smallBatchSize, 0);
            collection.Add(10);
            collection.Add(20);

            // Act
            int count = collection.Count;

            // Assert
            Assert.Equal(2, count);
        }

        /// <summary>
        /// Tests that Add correctly rolls over to a new batch when current batch is full.
        /// </summary>
        [Fact]
        public void Add_WhenBatchIsFull_CreatesNewBatchAndAddsItem()
        {
            // Arrange
            var collection = new BatchBlockingCollection<int>(_smallBatchSize, 0);
            // Fill first batch (batch size = 3)
            collection.Add(1);
            collection.Add(2);
            collection.Add(3);
            // Add one more item which should create a new batch
            collection.Add(4);

            // Act
            int count = collection.Count;

            // Assert
            // First batch in queue has 3 items, current batch has 1 item.
            Assert.Equal(4, count);
        }

        /// <summary>
        /// Tests that CompleteAdding processes all batches and invokes the ProcessItem event for each item.
        /// </summary>
//         [Fact] [Error] (122-27)CS0050 Inconsistent accessibility: return type 'Task' is less accessible than method 'BatchBlockingCollectionTests.CompleteAdding_ProcessesAllItems_InvokesProcessItemForEachItem()' [Error] (122-27)CS1983 The return type of an async method must be void, Task, Task<T>, a task-like type, IAsyncEnumerable<T>, or IAsyncEnumerator<T> [Error] (122-27)CS0161 'BatchBlockingCollectionTests.CompleteAdding_ProcessesAllItems_InvokesProcessItemForEachItem()': not all code paths return a value
//         public async Task CompleteAdding_ProcessesAllItems_InvokesProcessItemForEachItem()
//         {
//             // Arrange
//             var processedItems = new List<int>();
//             var collection = new BatchBlockingCollection<int>(batchSize: 2, boundedCapacity: 0);
//             collection.ProcessItem += item => processedItems.Add(item);
// 
//             // Add several items
//             collection.Add(100);
//             collection.Add(200);
//             collection.Add(300);
//             collection.Add(400);
// 
//             // Act
//             collection.CompleteAdding();
//             await collection.Completion;
// 
//             // Assert
//             // Expect ProcessItem to be invoked for 4 items.
//             Assert.Equal(4, processedItems.Count);
//             Assert.Contains(100, processedItems);
//             Assert.Contains(200, processedItems);
//             Assert.Contains(300, processedItems);
//             Assert.Contains(400, processedItems);
//         }
    }

    /// <summary>
    /// Unit tests for the <see cref="Rental{T}"/> class.
    /// </summary>
    public class RentalTests
    {
        /// <summary>
        /// Tests that Get returns a new instance when the rental queue is empty.
        /// </summary>
        [Fact]
        public void Get_EmptyQueue_ReturnsNewInstance()
        {
            // Arrange
            int factoryInvocationCount = 0;
            var rental = new Rental<object>(() =>
            {
                factoryInvocationCount++;
                return new object();
            });

            // Act
            var instance = rental.Get();

            // Assert
            Assert.NotNull(instance);
            Assert.Equal(1, factoryInvocationCount);
        }

        /// <summary>
        /// Tests that Return causes the next call to Get to return the same instance.
        /// </summary>
        [Fact]
        public void Get_AfterReturn_ReturnsSameInstance()
        {
            // Arrange
            var rental = new Rental<string>(() => Guid.NewGuid().ToString());
            string instance1 = rental.Get();
            rental.Return(instance1);

            // Act
            string instance2 = rental.Get();

            // Assert
            Assert.Equal(instance1, instance2);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="ChunkedList{T}"/> class.
    /// </summary>
    public class ChunkedListTests
    {
        /// <summary>
        /// Tests that adding items increases the Count property correctly.
        /// </summary>
        [Fact]
        public void Add_Items_IncreasesCountCorrectly()
        {
            // Arrange
            var chunkedList = new ChunkedList<int>(chunkSize: 2);

            // Act
            chunkedList.Add(10);
            chunkedList.Add(20);
            chunkedList.Add(30);

            // Assert
            Assert.Equal(3, chunkedList.Count);
        }

        /// <summary>
        /// Tests that items are organized into multiple chunks when added items exceed the chunk size.
        /// </summary>
        [Fact]
        public void Add_ItemsExceedingChunkSize_CreatesMultipleChunks()
        {
            // Arrange
            var chunkSize = 2;
            var chunkedList = new ChunkedList<int>(chunkSize);

            // Act
            chunkedList.Add(1);
            chunkedList.Add(2);
            chunkedList.Add(3);
            chunkedList.Add(4);
            chunkedList.Add(5);

            // Assert
            Assert.True(chunkedList.Chunks.Count >= 3); // At least 3 chunks expected for 5 items with chunkSize = 2.
            int totalCount = chunkedList.Chunks.Sum(chunk => chunk.Count);
            Assert.Equal(5, totalCount);
        }

        /// <summary>
        /// Tests that the ToString method returns a string in the expected format.
        /// </summary>
        [Fact]
        public void ToString_ReturnsCountString()
        {
            // Arrange
            var chunkedList = new ChunkedList<string>(chunkSize: 3);
            chunkedList.Add("a");
            chunkedList.Add("b");

            // Act
            var result = chunkedList.ToString();

            // Assert
            Assert.Equal("Count = 2", result);
        }
    }
}
