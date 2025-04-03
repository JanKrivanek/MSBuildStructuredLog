using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Note"/> class.
    /// </summary>
    public class NoteTests
    {
        private readonly Note _note;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteTests"/> class.
        /// </summary>
        public NoteTests()
        {
            // Use a testable subclass to expose protected members if needed.
            _note = new TestableNote();
        }

        /// <summary>
        /// Tests that the TypeName property returns the expected value "Note".
        /// </summary>
        [Fact]
        public void TypeName_Get_ReturnsCorrectTypeName()
        {
            // Act
            string typeName = _note.TypeName;

            // Assert
            typeName.Should().Be("Note");
        }

        /// <summary>
        /// Tests that the protected IsSelectable property returns false.
        /// </summary>
        [Fact]
        public void IsSelectable_Get_ReturnsFalse()
        {
            // Arrange
            var testableNote = new TestableNote();

            // Act
            bool isSelectable = testableNote.GetIsSelectable();

            // Assert
            isSelectable.Should().BeFalse();
        }

        /// <summary>
        /// A testable subclass of <see cref="Note"/> to expose the protected IsSelectable property.
        /// </summary>
        private class TestableNote : Note
        {
            /// <summary>
            /// Exposes the protected IsSelectable property.
            /// </summary>
            /// <returns>The value of IsSelectable.</returns>
            public bool GetIsSelectable() => base.IsSelectable;
        }
    }
}
