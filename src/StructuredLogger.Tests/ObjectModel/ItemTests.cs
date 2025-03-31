// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="Item"/> class.
//     /// </summary>
//     public class ItemTests
//     {
//         private readonly Item _item;
// 
//         public ItemTests()
//         {
//             _item = new Item();
//         }
// //  // [Error] (31-18)CS1061 'Item' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Item' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the default constructor creates an instance with the expected TypeName.
// //         /// Note: The DisableChildrenCache property is set in the constructor but is not publicly accessible,
// //         /// so this test verifies the instance creation indirectly via the TypeName property.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WhenCalled_InstanceIsCreatedWithExpectedTypeName()
// //         {
// //             // Act
// //             var item = new Item();
// // 
// //             // Assert
// //             item.TypeName.Should().Be("Item");
// //         }
// // 
//         /// <summary>
//         /// Tests that the Text property returns the same value that was set.
//         /// The Text property is a wrapper over the Name property inherited from NamedNode.
//         /// </summary>
//         /// <param name="testText">The test input string for Text property.</param>
//         [Theory]
//         [InlineData("TestItem")]
//         [InlineData("")]
//         [InlineData("Another Item")]
//         public void TextProperty_SetAndGet_ReturnsExpectedValue(string testText)
//         {
//             // Arrange
//             _item.Text = testText;
// 
//             // Act
//             var result = _item.Text;
// 
//             // Assert
//             result.Should().Be(testText);
//         }
// //  // [Error] (62-34)CS1061 'Item' does not contain a definition for 'TypeName' and no accessible extension method 'TypeName' accepting a first argument of type 'Item' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the TypeName property returns "Item".
// //         /// </summary>
// //         [Fact]
// //         public void TypeNameProperty_WhenAccessed_ReturnsItem()
// //         {
// //             // Act
// //             var typeName = _item.TypeName;
// // 
// //             // Assert
// //             typeName.Should().Be("Item");
// //         }
// //     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="FileCopy"/> class.
//     /// </summary>
//     public class FileCopyTests
//     {
//         private readonly FileCopy _fileCopy;
// 
//         public FileCopyTests()
//         {
//             _fileCopy = new FileCopy();
//         }
// 
//         /// <summary>
//         /// Tests that the Kind property returns the same value that was set.
//         /// </summary>
//         /// <param name="testKind">The test input string for Kind property.</param>
//         [Theory]
//         [InlineData("Copy")]
//         [InlineData("")]
//         [InlineData("Move")]
//         public void KindProperty_SetAndGet_ReturnsExpectedValue(string testKind)
//         {
//             // Arrange
//             _fileCopy.Kind = testKind;
// 
//             // Act
//             var result = _fileCopy.Kind;
// 
//             // Assert
//             result.Should().Be(testKind);
//         }
// 
//         /// <summary>
//         /// Tests that the inherited Text property is properly set and retrieved in the FileCopy class.
//         /// </summary>
//         /// <param name="testText">The test input string for Text property.</param>
//         [Theory]
//         [InlineData("FileItem")]
//         [InlineData("Sample")]
//         public void TextProperty_InheritedSetAndGet_ReturnsExpectedValue(string testText)
//         {
//             // Arrange
//             _fileCopy.Text = testText;
// 
//             // Act
//             var result = _fileCopy.Text;
// 
//             // Assert
//             result.Should().Be(testText);
//         }
// 
//         /// <summary>
//         /// Tests that the TypeName property, inherited from the Item base class, returns "Item".
//         /// </summary>
//         [Fact]
//         public void TypeNameProperty_WhenAccessed_ReturnsItem()
//         {
//             // Act
//             var typeName = _fileCopy.TypeName;
// 
//             // Assert
//             typeName.Should().Be("Item");
//         }
//     }
// }