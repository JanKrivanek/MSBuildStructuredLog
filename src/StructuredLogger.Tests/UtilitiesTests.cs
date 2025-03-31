// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Utilities"/> class.
//     /// </summary>
//     public class UtilitiesTests
//     {
//         /// <summary>
//         /// Tests that BinarySearch returns the correct index when the item is found.
//         /// </summary>
//         [Fact]
//         public void BinarySearch_ItemFound_ReturnsCorrectIndex()
//         {
//             // Arrange
//             var list = new List<int> { 1, 3, 5, 7, 9 };
//             int searchItem = 5;
// 
//             // Act
//             int index = list.BinarySearch(searchItem, x => x);
// 
//             // Assert
//             index.Should().Be(2);
//         }
// 
//         /// <summary>
//         /// Tests that BinarySearch returns the negative insertion point when the item is not found.
//         /// </summary>
//         [Fact]
//         public void BinarySearch_ItemNotFound_ReturnsNegativeInsertionPoint()
//         {
//             // Arrange
//             var list = new List<int> { 1, 3, 5, 7, 9 };
//             int searchItem = 6;
// 
//             // Act
//             int index = list.BinarySearch(searchItem, x => x);
// 
//             // Assert
//             // Expected insertion point index is 3, so BinarySearch should return ~3, which is -4.
//             index.Should().Be(-4);
//         }
// 
//         /// <summary>
//         /// Tests that BinarySearch on an empty list returns ~0 indicating the item would be inserted at position 0.
//         /// </summary>
//         [Fact]
//         public void BinarySearch_EmptyList_ReturnsNegativeZero()
//         {
//             // Arrange
//             var list = new List<int>();
//             int searchItem = 10;
// 
//             // Act
//             int index = list.BinarySearch(searchItem, x => x);
// 
//             // Assert
//             index.Should().Be(~0);
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="BatchBlockingCollection{T}"/> class.
//     /// </summary>
//     public class BatchBlockingCollectionTests
//     {
//         private readonly int _batchSize = 3;
// 
//         /// <summary>
//         /// Tests that the Count property correctly reflects the total number of items added.
//         /// </summary>
//         [Fact]
//         public void Add_Items_CountPropertyReflectsTotalItems()
//         {
//             // Arrange
//             var collection = new BatchBlockingCollection<int>(_batchSize);
//             // Add items equal to batch size, which should fill the current batch
//             collection.Add(1);
//             collection.Add(2);
//             collection.Add(3);
//             // At this point, currentBatch.Count should be 3 and queue is empty.
//             collection.Count.Should().Be(3);
// 
//             // Act
//             // Adding one more item should enqueue the full batch and start a new current batch.
//             collection.Add(4);
// 
//             // Assert
//             // Count = queue.Count * BatchSize (1 full batch of 3) + currentBatch count (1) = 4.
//             collection.Count.Should().Be(4);
//         }
// //  // [Error] (104-27)CS1983 The return type of an async method must be void, Task, Task<T>, a task-like type, IAsyncEnumerable<T>, or IAsyncEnumerator<T> // [Error] (104-27)CS0161 'BatchBlockingCollectionTests.CompleteAdding_ProcessesAllItems()': not all code paths return a value
// //         /// <summary>
// //         /// Tests that all items are processed exactly once when CompleteAdding is called.
// //         /// </summary>
// //         [Fact]
// //         public async Task CompleteAdding_ProcessesAllItems()
// //         {
// //             // Arrange
// //             var collection = new BatchBlockingCollection<int>(_batchSize);
// //             var processedItems = new List<int>();
// //             collection.ProcessItem += item => processedItems.Add(item);
// //             var itemsToAdd = Enumerable.Range(1, 7).ToList();
// //             itemsToAdd.ForEach(item => collection.Add(item));
// // 
// //             // Act
// //             collection.CompleteAdding();
// //             await collection.Completion.ConfigureAwait(false);
// // 
// //             // Assert
// //             processedItems.Should().BeEquivalentTo(itemsToAdd);
// //         }
// //     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="Rental{T}"/> class.
//     /// </summary>
//     public class RentalTests
//     {
//         /// <summary>
//         /// Tests that Get returns new objects when the pool is empty.
//         /// </summary>
//         [Fact]
//         public void Get_PoolEmpty_ReturnsNewObject()
//         {
//             // Arrange
//             var rental = new Rental<object>(() => new object());
// 
//             // Act
//             object obj1 = rental.Get();
//             object obj2 = rental.Get();
// 
//             // Assert
//             obj1.Should().NotBeNull();
//             obj2.Should().NotBeNull();
//             obj1.Should().NotBeSameAs(obj2);
//         }
// 
//         /// <summary>
//         /// Tests that an object returned to the pool via Return is provided again when Get is called.
//         /// </summary>
//         [Fact]
//         public void Return_ObjectReturned_ObjectRetrievedAgain()
//         {
//             // Arrange
//             var rental = new Rental<object>(() => new object());
//             object obj = rental.Get();
// 
//             // Act
//             rental.Return(obj);
//             object objFromPool = rental.Get();
// 
//             // Assert
//             objFromPool.Should().BeSameAs(obj);
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="ChunkedList{T}"/> class.
//     /// </summary>
//     public class ChunkedListTests
//     {
//         private readonly int _chunkSize = 3;
// 
//         /// <summary>
//         /// Tests that items are correctly added and organized into chunks and that the Count property reflects the total number of items.
//         /// </summary>
//         [Fact]
//         public void Add_Items_ItemsOrganizedIntoChunksAndCountIsCorrect()
//         {
//             // Arrange
//             var chunkedList = new ChunkedList<int>(_chunkSize);
//             int totalItems = 5;
// 
//             // Act
//             for (int i = 1; i <= totalItems; i++)
//             {
//                 chunkedList.Add(i);
//             }
// 
//             // Assert
//             chunkedList.Count.Should().Be(totalItems);
//             // Expecting 2 chunks: first chunk full (_chunkSize items) and second chunk with the remaining items.
//             chunkedList.Chunks.Count.Should().Be(2);
//             chunkedList.Chunks.First().Count.Should().Be(_chunkSize);
//             chunkedList.Chunks.Last().Count.Should().Be(totalItems - _chunkSize);
//         }
// 
//         /// <summary>
//         /// Tests that the ToString method returns the count in the expected format.
//         /// </summary>
//         [Fact]
//         public void ToString_ReturnsCorrectFormat()
//         {
//             // Arrange
//             var chunkedList = new ChunkedList<int>(_chunkSize);
//             chunkedList.Add(10);
//             chunkedList.Add(20);
// 
//             // Act
//             string result = chunkedList.ToString();
// 
//             // Assert
//             result.Should().Be("Count = 2");
//         }
//     }
// }