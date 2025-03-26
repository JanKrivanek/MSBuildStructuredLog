using System;
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
        private readonly TestableNote _testableNote;

        /// <summary>
        /// Initializes new instances of <see cref="NoteTests"/>.
        /// </summary>
        public NoteTests()
        {
            _note = new Note();
            _testableNote = new TestableNote();
        }

        /// <summary>
        /// Tests that the <see cref="Note.TypeName"/> property returns the expected type name "Note".
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsCorrectTypeName()
        {
            // Act
            string result = _note.TypeName;

            // Assert
            Assert.Equal("Note", result);
        }

        /// <summary>
        /// Tests that the protected <see cref="Note.IsSelectable"/> property returns false.
        /// This is achieved by using a testable subclass that exposes the protected member.
        /// </summary>
        [Fact]
        public void IsSelectable_WhenAccessedThroughDerivedClass_ReturnsFalse()
        {
            // Act
            bool result = _testableNote.PublicIsSelectable;

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// A testable subclass of <see cref="Note"/> to expose protected members for testing.
        /// </summary>
        private class TestableNote : Note
        {
            /// <summary>
            /// Gets the value of the protected <see cref="Note.IsSelectable"/> property.
            /// </summary>
            public bool PublicIsSelectable => base.IsSelectable;
        }
    }
}
