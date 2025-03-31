// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Note"/> class.
//     /// </summary>
//     public class NoteTests
//     {
//         private readonly Note _note;
//         private readonly TestableNote _testableNote;
// 
//         /// <summary>
//         /// Initializes a new instance of the <see cref="NoteTests"/> class.
//         /// </summary>
//         public NoteTests()
//         {
//             _note = new Note();
//             _testableNote = new TestableNote();
//         }
// //  // [Error] (31-37)CS1061 'Note' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Note' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the public <see cref="Note.TypeName"/> property returns the string "Note".
// //         /// </summary>
// //         [Fact]
// //         public void TypeName_WhenAccessed_ShouldReturnNote()
// //         {
// //             // Act
// //             string typeName = _note.TypeName;
// // 
// //             // Assert
// //             typeName.Should().Be("Note");
// //         }
// // 
//         /// <summary>
//         /// Tests that the protected <see cref="Note.IsSelectable"/> property returns false.
//         /// </summary>
//         [Fact]
//         public void IsSelectable_WhenAccessedThroughDerivedClass_ShouldReturnFalse()
//         {
//             // Act
//             bool isSelectable = _testableNote.GetIsSelectable();
// 
//             // Assert
//             isSelectable.Should().BeFalse();
//         }
// 
//         /// <summary>
//         /// A testable subclass of <see cref="Note"/> that exposes the protected <see cref="Note.IsSelectable"/> property.
//         /// </summary>
//         private class TestableNote : Note
//         {
// //             /// <summary> // [Error] (59-51)CS0117 'Note' does not contain a definition for 'IsSelectable'
// //             /// Exposes the protected <see cref="Note.IsSelectable"/> property.
// //             /// </summary>
// //             /// <returns>A boolean indicating whether the note is selectable.</returns>
// //             public bool GetIsSelectable() => base.IsSelectable;
// //         }
//     }
// }