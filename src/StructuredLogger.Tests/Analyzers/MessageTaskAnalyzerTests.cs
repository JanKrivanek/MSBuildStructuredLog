using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="MessageTaskAnalyzer"/> class.
    /// </summary>
    public class MessageTaskAnalyzerTests
    {
        /// <summary>
        /// Tests that when a valid message with a non-null ShortenedText is present in the task's children,
        /// the task's Name is updated accordingly.
        /// </summary>
        [Fact]
        public void Analyze_WithValidMessage_SetsTaskName()
        {
            // Arrange
            var expectedText = "Sample";
            var task = new Task
            {
                Name = null,
                Children = new List<object>
                {
                    new Message { ShortenedText = expectedText }
                }
            };

            // Act
            MessageTaskAnalyzer.Analyze(task);

            // Assert
            Assert.Equal("Message: " + expectedText, task.Name);
        }

        /// <summary>
        /// Tests that when there is no Message object in the task's children,
        /// the task's Name remains unchanged.
        /// </summary>
        [Fact]
        public void Analyze_WithNoMessage_DoesNotChangeTaskName()
        {
            // Arrange
            var initialName = "InitialName";
            var task = new Task
            {
                Name = initialName,
                Children = new List<object>
                {
                    "Some string",
                    123
                }
            };

            // Act
            MessageTaskAnalyzer.Analyze(task);

            // Assert
            Assert.Equal(initialName, task.Name);
        }

        /// <summary>
        /// Tests that when a Message object with a null ShortenedText is present,
        /// the task's Name remains unchanged.
        /// </summary>
        [Fact]
        public void Analyze_WithMessageHavingNullShortenedText_DoesNotChangeTaskName()
        {
            // Arrange
            var task = new Task
            {
                Name = null,
                Children = new List<object>
                {
                    new Message { ShortenedText = null }
                }
            };

            // Act
            MessageTaskAnalyzer.Analyze(task);

            // Assert
            Assert.Null(task.Name);
        }

        /// <summary>
        /// Tests that when multiple Message objects are present, the first one with a non-null ShortenedText is used to update the task's Name.
        /// </summary>
        [Fact]
        public void Analyze_WithMultipleMessages_FirstValidMessageSetsTaskName()
        {
            // Arrange
            var task = new Task
            {
                Name = null,
                Children = new List<object>
                {
                    new Message { ShortenedText = "First" },
                    new Message { ShortenedText = "Second" }
                }
            };

            // Act
            MessageTaskAnalyzer.Analyze(task);

            // Assert
            Assert.Equal("Message: First", task.Name);
        }

        /// <summary>
        /// Tests that when the first Message object in the task's children has a null ShortenedText,
        /// even if a subsequent Message has a non-null ShortenedText, the task's Name remains unchanged.
        /// </summary>
        [Fact]
        public void Analyze_WithMultipleMessages_FirstMessageNullShortenedText_DoesNotUpdateTaskName()
        {
            // Arrange
            var task = new Task
            {
                Name = null,
                Children = new List<object>
                {
                    new Message { ShortenedText = null },
                    new Message { ShortenedText = "Found" }
                }
            };

            // Act
            MessageTaskAnalyzer.Analyze(task);

            // Assert
            Assert.Null(task.Name);
        }

        /// <summary>
        /// Tests that when a null task is provided to the Analyze method,
        /// a NullReferenceException is thrown.
        /// </summary>
        [Fact]
        public void Analyze_WithNullTask_ThrowsNullReferenceException()
        {
            // Arrange
            Task task = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => MessageTaskAnalyzer.Analyze(task));
        }
    }
}

// The following stub classes are provided to ensure the test code is self-contained.
// In a real scenario, these classes are expected to be part of the production code.
namespace Microsoft.Build.Logging.StructuredLogger
{
    /// <summary>
    /// Represents a task that has a Name and a collection of children elements.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of children for the task.
        /// </summary>
        public IList<object> Children { get; set; } = new List<object>();
    }

    /// <summary>
    /// Represents a message with shortened text.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the shortened text of the message.
        /// </summary>
        public string ShortenedText { get; set; }
    }
}
