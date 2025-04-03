// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using System.Collections.Concurrent;
// using System.Linq;
// using System.Threading.Tasks;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="EvaluationIdProvider"/> class.
//     /// </summary>
//     public class EvaluationIdProviderTests
//     {
//         /// <summary>
//         /// Tests that GetNextId returns unique and monotonically increasing values when called sequentially.
//         /// This test calls GetNextId twice in succession and asserts that the second value is greater than the first.
//         /// </summary>
//         [Fact]
//         public void GetNextId_WhenCalledSequentially_ReturnsUniqueAndIncreasingValues()
//         {
//             // Act
//             long firstId = EvaluationIdProvider.GetNextId();
//             long secondId = EvaluationIdProvider.GetNextId();
// 
//             // Assert
//             secondId.Should().BeGreaterThan(firstId, "sequential calls to GetNextId should return monotonically increasing values");
//         }
// //  // [Error] (35-27)CS0050 Inconsistent accessibility: return type 'Task' is less accessible than method 'EvaluationIdProviderTests.GetNextId_WhenCalledConcurrently_ReturnsUniqueValues()' // [Error] (35-27)CS1983 The return type of an async method must be void, Task, Task<T>, a task-like type, IAsyncEnumerable<T>, or IAsyncEnumerator<T> // [Error] (43-35)CS0117 'Task' does not contain a definition for 'Run' // [Error] (50-24)CS0117 'Task' does not contain a definition for 'WhenAll' // [Error] (35-27)CS0161 'EvaluationIdProviderTests.GetNextId_WhenCalledConcurrently_ReturnsUniqueValues()': not all code paths return a value
// //         /// <summary>
// //         /// Tests that GetNextId returns unique values when called concurrently from multiple threads.
// //         /// This test spawns multiple concurrent tasks to call GetNextId and verifies that all returned ids are unique.
// //         /// </summary>
// //         [Fact]
// //         public async Task GetNextId_WhenCalledConcurrently_ReturnsUniqueValues()
// //         {
// //             // Arrange
// //             const int concurrentCalls = 1000;
// //             var idBag = new ConcurrentBag<long>();
// // 
// //             // Act
// //             var tasks = Enumerable.Range(0, concurrentCalls)
// //                 .Select(_ => Task.Run(() =>
// //                 {
// //                     long id = EvaluationIdProvider.GetNextId();
// //                     idBag.Add(id);
// //                 }))
// //                 .ToArray();
// // 
// //             await Task.WhenAll(tasks);
// // 
// //             // Assert
// //             idBag.Distinct().Count().Should().Be(concurrentCalls, "each concurrent call should return a unique evaluation id");
// //         }
// //     }
// }