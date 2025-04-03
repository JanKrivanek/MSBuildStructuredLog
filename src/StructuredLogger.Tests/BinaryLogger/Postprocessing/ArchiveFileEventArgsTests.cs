// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
// //     /// <summary> // [Error] (22-38)CS7036 There is no argument given that corresponds to the required parameter 'fullPath' of 'ArchiveFile.ArchiveFile(string, string)' // [Error] (23-38)CS7036 There is no argument given that corresponds to the required parameter 'fullPath' of 'ArchiveFile.ArchiveFile(string, string)'
// //     /// Unit tests for the <see cref="ArchiveFileEventArgs"/> class.
// //     /// </summary>
// //     public class ArchiveFileEventArgsTests
// //     {
// //         private readonly ArchiveFile _dummyArchiveFile1;
// //         private readonly ArchiveFile _dummyArchiveFile2;
// // 
// //         /// <summary>
// //         /// Initializes a new instance of the <see cref="ArchiveFileEventArgsTests"/> class.
// //         /// </summary>
// //         public ArchiveFileEventArgsTests()
// //         {
// //             // Assuming ArchiveFile has an accessible default constructor.
// //             // If ArchiveFile requires specific initialization, update accordingly.
// //             _dummyArchiveFile1 = new ArchiveFile();
// //             _dummyArchiveFile2 = new ArchiveFile();
// //         }
// //  // [Error] (36-33)CS1729 'ArchiveFileEventArgs' does not contain a constructor that takes 1 arguments
// //         /// <summary>
// //         /// Tests the constructor of <see cref="ArchiveFileEventArgs"/> to ensure that the ArchiveFile property is correctly set.
// //         /// Arrange: Creates a dummy ArchiveFile.
// //         /// Act: Instantiates ArchiveFileEventArgs with the dummy ArchiveFile.
// //         /// Assert: Verifies that the ArchiveFile property equals the provided dummy ArchiveFile.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithValidArchiveFile_SetsArchiveFileProperty()
// //         {
// //             // Act
// //             var eventArgs = new ArchiveFileEventArgs(_dummyArchiveFile1);
// // 
// //             // Assert
// //             eventArgs.ArchiveFile.Should().Be(_dummyArchiveFile1, "because the constructor should assign the provided ArchiveFile to the ArchiveFile property.");
// //         }
// //  // [Error] (52-33)CS1729 'ArchiveFileEventArgs' does not contain a constructor that takes 1 arguments
// //         /// <summary>
// //         /// Tests the ArchiveFile property setter and getter.
// //         /// Arrange: Instantiates ArchiveFileEventArgs with an initial dummy ArchiveFile.
// //         /// Act: Updates the ArchiveFile property to a new dummy ArchiveFile.
// //         /// Assert: Verifies that the ArchiveFile property returns the updated dummy ArchiveFile.
// //         /// </summary>
// //         [Fact]
// //         public void ArchiveFile_PropertySetterAndGetter_ShouldReturnUpdatedValue()
// //         {
// //             // Arrange
// //             var eventArgs = new ArchiveFileEventArgs(_dummyArchiveFile1);
// // 
// //             // Act
// //             eventArgs.ArchiveFile = _dummyArchiveFile2;
// // 
// //             // Assert
// //             eventArgs.ArchiveFile.Should().Be(_dummyArchiveFile2, "because setting the ArchiveFile property should update its value accordingly.");
// //         }
// //     }
// // }