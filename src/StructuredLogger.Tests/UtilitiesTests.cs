// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the Utilities class.
//     /// </summary>
//     public class UtilitiesTests
//     {
//         /// <summary>
//         /// Tests BinarySearch extension method when the source list is empty.
//         /// Expected to return bitwise complement of zero.
//         /// </summary>
//         [Fact]
//         public void BinarySearch_EmptyList_ReturnsNegativeInsertionIndex()
//         {
//             // Arrange
//             IList<int> list = new List<int>();
//             int searchItem = 5;
// 
//             // Act
//             int result = list.BinarySearch(searchItem, x => x);
// 
//             // Assert
//             result.Should().Be(~0, "an empty list should return the bitwise complement of 0 as the insertion index");
//         }
// 
//         /// <summary>
//         /// Tests BinarySearch extension method when the item is present in the list.
//         /// Expected to return the index of the found item.
//         /// </summary>
//         [Theory]
//         [InlineData(new int[] { 1, 3, 5, 7 }, 5, 2)]
//         [InlineData(new int[] { -10, 0, 10, 20 }, -10, 0)]
//         [InlineData(new int[] { Int32.MinValue, 0, Int32.MaxValue }, Int32.MaxValue, 2)]
//         public void BinarySearch_ItemPresent_ReturnsCorrectIndex(int[] array, int searchItem, int expectedIndex)
//         {
//             // Arrange
//             IList<int> list = array.ToList();
// 
//             // Act
//             int result = list.BinarySearch(searchItem, x => x);
// 
//             // Assert
//             result.Should().Be(expectedIndex, "the search item exists in the list at the expected index");
//         }
// 
//         /// <summary>
//         /// Tests BinarySearch extension method when the item is not present in the list.
//         /// Expected to return the bitwise complement of the index where the item should be inserted.
//         /// </summary>
//         [Theory]
//         [InlineData(new int[] { 1, 3, 5, 7 }, 4, ~2)]
//         [InlineData(new int[] { 2, 4, 6, 8 }, 5, ~2)]
//         [InlineData(new int[] { -20, -10, 0, 10 }, -15, ~1)]
//         public void BinarySearch_ItemNotPresent_ReturnsNegativeInsertionIndex(int[] array, int searchItem, int expectedResult)
//         {
//             // Arrange
//             IList<int> list = array.ToList();
// 
//             // Act
//             int result = list.BinarySearch(searchItem, x => x);
// 
//             // Assert
//             result.Should().Be(expectedResult, "when the search item is not found, the method should return the bitwise complement of the insertion index");
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the BatchBlockingCollection class.
//     /// </summary>
//     public class BatchBlockingCollectionTests
//     {
//         private const int DefaultBatchSize = 8192;
//         private readonly BatchBlockingCollection<int> _collection;
// 
//         /// <summary>
//         /// Initializes a new instance of the BatchBlockingCollectionTests class.
//         /// </summary>
//         public BatchBlockingCollectionTests()
//         {
//             _collection = new BatchBlockingCollection<int>();
//         }
// 
//         /// <summary>
//         /// Tests the Add method when adding items fewer than the batch size.
//         /// Expected the Count property to reflect the added items.
//         /// </summary>
//         [Fact]
//         public void Add_WhenAddingFewerThanBatchSize_CountMatchesNumberOfItems()
//         {
//             // Arrange
//             int itemsToAdd = 5;
//             for (int i = 0; i < itemsToAdd; i++)
//             {
//                 _collection.Add(i);
//             }
// 
//             // Act
//             int count = _collection.Count;
// 
//             // Assert
//             count.Should().Be(itemsToAdd, "when fewer items than batch capacity are added, the Count should equal the number of items added");
//         }
// 
//         /// <summary>
//         /// Tests the Add method when adding items exceeding the current batch capacity.
//         /// Expected the Count property to correctly accumulate items across batches.
//         /// </summary>
//         [Fact]
//         public void Add_WhenExceedingBatchSize_CountAccumulatesAcrossBatches()
//         {
//             // Arrange
//             int itemsToAdd = DefaultBatchSize + 1; // This forces creation of a new batch.
//             for (int i = 0; i < itemsToAdd; i++)
//             {
//                 _collection.Add(i);
//             }
// 
//             // Act
//             int count = _collection.Count;
// 
//             // Assert
//             count.Should().Be(itemsToAdd, "the Count should correctly aggregate items added across batches");
//         }
// //  // [Error] (139-27)CS0050 Inconsistent accessibility: return type 'Task' is less accessible than method 'BatchBlockingCollectionTests.CompleteAdding_WhenCalled_ProcessesAllItems()' // [Error] (139-27)CS1983 The return type of an async method must be void, Task, Task<T>, a task-like type, IAsyncEnumerable<T>, or IAsyncEnumerator<T> // [Error] (139-27)CS0161 'BatchBlockingCollectionTests.CompleteAdding_WhenCalled_ProcessesAllItems()': not all code paths return a value
// //         /// <summary>
// //         /// Tests the CompleteAdding method and ensures that the ProcessItem event processes all items.
// //         /// Expected all added items to be processed upon task completion.
// //         /// </summary>
// //         [Fact]
// //         public async Task CompleteAdding_WhenCalled_ProcessesAllItems()
// //         {
// //             // Arrange
// //             List<int> processedItems = new List<int>();
// //             using var semaphore = new SemaphoreSlim(1, 1);
// //             _collection.ProcessItem += item =>
// //             {
// //                 semaphore.Wait();
// //                 try
// //                 {
// //                     processedItems.Add(item);
// //                 }
// //                 finally
// //                 {
// //                     semaphore.Release();
// //                 }
// //             };
// // 
// //             int totalItems = 100;
// //             for (int i = 0; i < totalItems; i++)
// //             {
// //                 _collection.Add(i);
// //             }
// // 
// //             // Act
// //             _collection.CompleteAdding();
// //             await _collection.Completion.ConfigureAwait(false);
// // 
// //             // Assert
// //             processedItems.Should().HaveCount(totalItems, "all added items should be processed after completion");
// //             processedItems.OrderBy(x => x).Should().Equal(Enumerable.Range(0, totalItems), "the processed items should match the order of added items");
// //         }
// //     }
// 
//     /// <summary>
//     /// Unit tests for the Rental class.
//     /// </summary>
//     public class RentalTests
//     {
//         /// <summary>
//         /// Tests that the Get method invokes the factory when no items have been returned.
//         /// Expected to retrieve a new item from the factory.
//         /// </summary>
//         [Fact]
//         public void Get_WhenNoItemReturned_InvokesFactory()
//         {
//             // Arrange
//             int factoryInvocationCount = 0;
//             var rental = new Rental<int>(() =>
//             {
//                 factoryInvocationCount++;
//                 return -1;
//             });
// 
//             // Act
//             int result = rental.Get();
// 
//             // Assert
//             result.Should().Be(-1, "the factory should return -1");
//             factoryInvocationCount.Should().Be(1, "the factory must be called exactly once when the queue is empty");
//         }
// 
//         /// <summary>
//         /// Tests that an item returned via Return is retrieved by the subsequent Get call.
//         /// Expected to return the same item that was returned.
//         /// </summary>
//         [Fact]
//         public void Return_AfterItemReturned_GetReturnsTheReturnedItem()
//         {
//             // Arrange
//             var rental = new Rental<int>(() => -1);
//             int returnedItem = 42;
//             rental.Return(returnedItem);
// 
//             // Act
//             int result = rental.Get();
// 
//             // Assert
//             result.Should().Be(returnedItem, "the returned item should be dequeued by the next Get call");
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the ChunkedList class.
//     /// </summary>
//     public class ChunkedListTests
//     {
//         /// <summary>
//         /// Tests the Add method and Count property for a ChunkedList with a custom small chunk size.
//         /// Expected to update chunk allocations and count correctly.
//         /// </summary>
//         [Fact]
//         public void Add_WhenItemsAdded_CountAndChunksAreUpdatedCorrectly()
//         {
//             // Arrange
//             int customChunkSize = 3;
//             var chunkedList = new ChunkedList<int>(customChunkSize);
//             int itemsToAdd = 5; // This should create 2 chunks: one full chunk and one partial.
//             for (int i = 0; i < itemsToAdd; i++)
//             {
//                 chunkedList.Add(i);
//             }
// 
//             // Act
//             int totalCount = chunkedList.Count;
//             var chunks = chunkedList.Chunks;
// 
//             // Assert
//             totalCount.Should().Be(itemsToAdd, "the Count property should equal the total number of added items");
//             chunks.Should().HaveCount(2, "adding 5 items with a chunk size of 3 should allocate 2 chunks");
//             chunks.First().Should().HaveCount(customChunkSize, "the first chunk should be full");
//             chunks.Last().Should().HaveCount(itemsToAdd - customChunkSize, "the last chunk should contain the remainder of the items");
//         }
// 
//         /// <summary>
//         /// Tests the ToString method override.
//         /// Expected to return a string indicating the current count.
//         /// </summary>
//         [Fact]
//         public void ToString_WhenCalled_ReturnsCorrectCountInformation()
//         {
//             // Arrange
//             var chunkedList = new ChunkedList<string>(10);
//             chunkedList.Add("test");
// 
//             // Act
//             string result = chunkedList.ToString();
// 
//             // Assert
//             result.Should().Be($"Count = {chunkedList.Count}", "the ToString method should display the count in the expected format");
//         }
//     }
// }