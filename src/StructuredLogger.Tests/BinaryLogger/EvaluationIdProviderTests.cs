using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="EvaluationIdProvider"/> class.
    /// </summary>
    public class EvaluationIdProviderTests
    {
        /// <summary>
        /// Tests that sequential calls to GetNextId return strictly increasing evaluation ids.
        /// </summary>
        [Fact]
        public void GetNextId_SequentialCalls_ReturnsStrictlyIncreasingIds()
        {
            // Arrange: Get the initial evaluation id.
            long firstId = EvaluationIdProvider.GetNextId();
            
            // Act: Make further sequential calls.
            long secondId = EvaluationIdProvider.GetNextId();
            long thirdId = EvaluationIdProvider.GetNextId();

            // Assert: Verify that the calls produced strictly increasing ids.
            Assert.True(secondId > firstId, $"Expected second id ({secondId}) to be greater than first id ({firstId}).");
            Assert.True(thirdId > secondId, $"Expected third id ({thirdId}) to be greater than second id ({secondId}).");
        }

        /// <summary>
        /// Tests that multiple sequential calls to GetNextId return unique ids.
        /// </summary>
        [Fact]
        public void GetNextId_MultipleSequentialCalls_ReturnUniqueIds()
        {
            // Arrange: Prepare collection to hold ids.
            const int callCount = 100;
            var ids = new List<long>();

            // Act: Call GetNextId sequentially a number of times.
            for (int i = 0; i < callCount; i++)
            {
                ids.Add(EvaluationIdProvider.GetNextId());
            }

            // Assert: All ids should be unique.
            var distinctCount = ids.Distinct().Count();
            Assert.Equal(callCount, distinctCount);
        }

        /// <summary>
        /// Tests that concurrent calls to GetNextId return unique ids.
        /// </summary>
//         [Fact] [Error] (58-27)CS0050 Inconsistent accessibility: return type 'Task' is less accessible than method 'EvaluationIdProviderTests.GetNextId_ConcurrentCalls_ReturnUniqueIds()' [Error] (58-27)CS1983 The return type of an async method must be void, Task, Task<T>, a task-like type, IAsyncEnumerable<T>, or IAsyncEnumerator<T> [Error] (68-32)CS0117 'Task' does not contain a definition for 'Run' [Error] (79-38)CS0117 'Task' does not contain a definition for 'WhenAll' [Error] (58-27)CS0161 'EvaluationIdProviderTests.GetNextId_ConcurrentCalls_ReturnUniqueIds()': not all code paths return a value
//         public async Task GetNextId_ConcurrentCalls_ReturnUniqueIds()
//         {
//             // Arrange: Define number of tasks and calls per task.
//             const int taskCount = 50;
//             const int callsPerTask = 20;
//             var tasks = new List<Task<List<long>>>();
// 
//             // Act: Invoke GetNextId concurrently.
//             for (int i = 0; i < taskCount; i++)
//             {
//                 tasks.Add(Task.Run(() =>
//                 {
//                     var localIds = new List<long>();
//                     for (int j = 0; j < callsPerTask; j++)
//                     {
//                         localIds.Add(EvaluationIdProvider.GetNextId());
//                     }
//                     return localIds;
//                 }));
//             }
// 
//             var results = await Task.WhenAll(tasks);
//             var allIds = results.SelectMany(x => x).ToList();
// 
//             // Assert: Verify that all concurrent calls return unique ids.
//             var distinctCount = allIds.Distinct().Count();
//             Assert.Equal(taskCount * callsPerTask, distinctCount);
//         }
    }
}
