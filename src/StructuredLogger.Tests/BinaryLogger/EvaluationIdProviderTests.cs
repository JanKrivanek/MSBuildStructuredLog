using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private readonly FieldInfo _assignedIdField;
        private readonly long _processId;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationIdProviderTests"/> class and resets the static state.
        /// </summary>
        public EvaluationIdProviderTests()
        {
            // Obtain the private static field _sAssignedId via reflection.
            _assignedIdField = typeof(EvaluationIdProvider).GetField("_sAssignedId", BindingFlags.Static | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException("Field _sAssignedId not found.");
            ResetAssignedId();

            // Retrieve the ProcessId private field (readonly) via reflection.
            FieldInfo processIdField = typeof(EvaluationIdProvider).GetField("ProcessId", BindingFlags.Static | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException("Field ProcessId not found.");
            _processId = (long)processIdField.GetValue(null);
        }

        /// <summary>
        /// Resets the _sAssignedId field to its initial value of -1.
        /// </summary>
        private void ResetAssignedId()
        {
            _assignedIdField.SetValue(null, (long)(-1));
        }

        /// <summary>
        /// Tests that multiple sequential calls to GetNextId return unique, monotonically increasing ids.
        /// </summary>
        [Fact]
        public void GetNextId_WhenCalledSequentially_ReturnsUniqueAndMonotonicIds()
        {
            // Arrange
            const int callCount = 5;
            var ids = new List<long>();

            // Act
            for (int i = 0; i < callCount; i++)
            {
                ids.Add(EvaluationIdProvider.GetNextId());
            }

            // Assert
            Assert.Equal(callCount, ids.Count);
            // Ensure all ids are unique.
            Assert.Equal(ids.Count, ids.Distinct().Count());

            // Ensure the ids are monotonically increasing.
            for (int i = 1; i < ids.Count; i++)
            {
                Assert.True(ids[i] > ids[i - 1], $"Id at index {i} ({ids[i]}) is not greater than previous id ({ids[i - 1]}).");
            }
        }

        /// <summary>
        /// Tests that concurrent calls to GetNextId return unique ids, ensuring thread safety.
        /// </summary>
        [Fact]
        public async Task GetNextId_WhenCalledConcurrently_ReturnsUniqueIds()
        {
            // Arrange
            const int taskCount = 50;
            const int callsPerTask = 20;
            var results = new ConcurrentBag<long>();

            // Act
            var tasks = Enumerable.Range(0, taskCount)
                .Select(_ => Task.Run(() =>
                {
                    for (int i = 0; i < callsPerTask; i++)
                    {
                        results.Add(EvaluationIdProvider.GetNextId());
                    }
                })).ToArray();

            await Task.WhenAll(tasks);

            // Assert
            int totalCalls = taskCount * callsPerTask;
            Assert.Equal(totalCalls, results.Count);
            Assert.Equal(totalCalls, results.Distinct().Count());
        }

        /// <summary>
        /// Tests that when the evaluation id generation encounters arithmetic overflow, an OverflowException is thrown.
        /// </summary>
        [Fact]
        public void GetNextId_WhenArithmeticOverflowOccurs_ThrowsOverflowException()
        {
            // Arrange
            // Set the _sAssignedId field to a value such that the next call to GetNextId causes an overflow.
            // Calculation: nextId = _sAssignedId + 1. We want: (nextId + ProcessId) equals long.MaxValue, so that
            // computing ((long.MaxValue * (long.MaxValue + 1)) / 2) overflows.
            _assignedIdField.SetValue(null, long.MaxValue - _processId - 1);

            // Act & Assert
            Assert.Throws<OverflowException>(() => EvaluationIdProvider.GetNextId());
        }
    }
}
