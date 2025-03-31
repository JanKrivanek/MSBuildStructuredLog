// using FluentAssertions;
// using Microsoft.Build.Framework.Profiler;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.IO;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref = "EvaluationProfileEntry"/> class.
//     /// </summary>
//     public class EvaluationProfileEntryTests
//     {
// //         /// <summary> // [Error] (25-17)CS0200 Property or indexer 'ProfiledLocation.InclusiveTime' cannot be assigned to -- it is read only // [Error] (26-17)CS0200 Property or indexer 'ProfiledLocation.NumberOfHits' cannot be assigned to -- it is read only
// //         /// Tests that the AddEntry method assigns the ProfiledLocation property.
// //         /// </summary>
// //         [Fact]
// //         public void AddEntry_ValidProfiledLocation_SetsProfiledLocation()
// //         {
// //             // Arrange
// //             var entry = new EvaluationProfileEntry();
// //             var profiledLocation = new ProfiledLocation
// //             {
// //                 InclusiveTime = TimeSpan.FromMilliseconds(150),
// //                 NumberOfHits = 3
// //             };
// //             // Act
// //             entry.AddEntry(profiledLocation);
// //             // Assert
// //             entry.ProfiledLocation.Should().Be(profiledLocation);
// //         }
// // 
//         /// <summary>
//         /// Tests that accessing NumberOfHits without setting ProfiledLocation throws a NullReferenceException.
//         /// </summary>
//         [Fact]
//         public void NumberOfHits_ProfiledLocationNotSet_ThrowsNullReferenceException()
//         {
//             // Arrange
//             var entry = new EvaluationProfileEntry();
//             // Act
//             Action act = () =>
//             {
//                 var _ = entry.NumberOfHits;
//             };
//             // Assert
//             act.Should().Throw<NullReferenceException>();
//         }
// //  // [Error] (61-17)CS0200 Property or indexer 'ProfiledLocation.InclusiveTime' cannot be assigned to -- it is read only // [Error] (62-17)CS0200 Property or indexer 'ProfiledLocation.NumberOfHits' cannot be assigned to -- it is read only
// //         /// <summary>
// //         /// Tests that NumberOfHits returns an empty string when NumberOfHits is 0.
// //         /// </summary>
// //         [Fact]
// //         public void NumberOfHits_WhenHitsAreZero_ReturnsEmptyString()
// //         {
// //             // Arrange
// //             var entry = new EvaluationProfileEntry();
// //             var profiledLocation = new ProfiledLocation
// //             {
// //                 InclusiveTime = TimeSpan.FromMilliseconds(100),
// //                 NumberOfHits = 0
// //             };
// //             entry.AddEntry(profiledLocation);
// //             // Act
// //             var result = entry.NumberOfHits;
// //             // Assert
// //             result.Should().Be(string.Empty);
// //         }
// //  // [Error] (83-17)CS0200 Property or indexer 'ProfiledLocation.InclusiveTime' cannot be assigned to -- it is read only // [Error] (84-17)CS0200 Property or indexer 'ProfiledLocation.NumberOfHits' cannot be assigned to -- it is read only
//         /// <summary>
//         /// Tests that NumberOfHits returns the correct string representation when NumberOfHits is greater than zero.
//         /// </summary>
//         [Theory]
//         [InlineData(1, "1")]
//         [InlineData(5, "5")]
//         public void NumberOfHits_WhenHitsArePositive_ReturnsHitsAsString(int hits, string expected)
//         {
//             // Arrange
//             var entry = new EvaluationProfileEntry();
//             var profiledLocation = new ProfiledLocation
//             {
//                 InclusiveTime = TimeSpan.FromMilliseconds(100),
//                 NumberOfHits = hits
//             };
//             entry.AddEntry(profiledLocation);
//             // Act
//             var result = entry.NumberOfHits;
//             // Assert
//             result.Should().Be(expected);
//         }
// //  // [Error] (103-17)CS0200 Property or indexer 'ProfiledLocation.InclusiveTime' cannot be assigned to -- it is read only // [Error] (104-17)CS0200 Property or indexer 'ProfiledLocation.NumberOfHits' cannot be assigned to -- it is read only
// //         /// <summary>
// //         /// Tests that DurationText returns an empty string when InclusiveTime is zero.
// //         /// </summary>
// //         [Fact]
// //         public void DurationText_WhenInclusiveTimeIsZero_ReturnsEmptyString()
// //         {
// //             // Arrange
// //             var entry = new EvaluationProfileEntry();
// //             var profiledLocation = new ProfiledLocation
// //             {
// //                 InclusiveTime = TimeSpan.Zero,
// //                 NumberOfHits = 2
// //             };
// //             entry.AddEntry(profiledLocation);
// //             // Act
// //             var durationText = entry.DurationText;
// //             // Assert
// //             durationText.Should().BeEmpty();
// //         }
// //  // [Error] (124-17)CS0200 Property or indexer 'ProfiledLocation.InclusiveTime' cannot be assigned to -- it is read only // [Error] (125-17)CS0200 Property or indexer 'ProfiledLocation.NumberOfHits' cannot be assigned to -- it is read only
//         /// <summary>
//         /// Tests that DurationText appends " (1 hit)" when there is exactly one hit.
//         /// </summary>
//         [Fact]
//         public void DurationText_WhenOneHit_AppendsOneHitText()
//         {
//             // Arrange
//             var entry = new EvaluationProfileEntry();
//             var testTime = TimeSpan.FromMilliseconds(200);
//             var profiledLocation = new ProfiledLocation
//             {
//                 InclusiveTime = testTime,
//                 NumberOfHits = 1
//             };
//             entry.AddEntry(profiledLocation);
//             // Act
//             var durationText = entry.DurationText;
//             // Assert
//             // Since the actual display format is determined by TextUtilities.DisplayDuration,
//             // we only check that the resulting string ends with " (1 hit)".
//             durationText.Should().EndWith(" (1 hit)");
//         }
// //  // [Error] (148-17)CS0200 Property or indexer 'ProfiledLocation.InclusiveTime' cannot be assigned to -- it is read only // [Error] (149-17)CS0200 Property or indexer 'ProfiledLocation.NumberOfHits' cannot be assigned to -- it is read only
// //         /// <summary>
// //         /// Tests that DurationText appends " (n hits)" when there are multiple hits.
// //         /// </summary>
// //         [Fact]
// //         public void DurationText_WhenMultipleHits_AppendsHitsText()
// //         {
// //             // Arrange
// //             var entry = new EvaluationProfileEntry();
// //             var testTime = TimeSpan.FromMilliseconds(250);
// //             int hits = 4;
// //             var profiledLocation = new ProfiledLocation
// //             {
// //                 InclusiveTime = testTime,
// //                 NumberOfHits = hits
// //             };
// //             entry.AddEntry(profiledLocation);
// //             // Act
// //             var durationText = entry.DurationText;
// //             // Assert
// //             durationText.Should().EndWith($" ({hits} hits)");
// //         }
// // 
//         /// <summary>
//         /// Tests that the FileName property returns the correct file name given a valid SourceFilePath.
//         /// </summary>
//         [Fact]
//         public void FileName_WithValidSourceFilePath_ReturnsFileName()
//         {
//             // Arrange
//             var filePath = @"C:\temp\example.txt";
//             var entry = new EvaluationProfileEntry
//             {
//                 SourceFilePath = filePath
//             };
//             // Act
//             var fileName = entry.FileName;
//             // Assert
//             fileName.Should().Be(Path.GetFileName(filePath));
//         }
// 
//         /// <summary>
//         /// Tests that Title returns EvaluationPassDescription when SourceFilePath is null.
//         /// </summary>
//         [Fact]
//         public void Title_WhenSourceFilePathIsNull_ReturnsEvaluationPassDescription()
//         {
//             // Arrange
//             var entry = new EvaluationProfileEntry
//             {
//                 SourceFilePath = null,
//                 EvaluationPassDescription = "PassDescription"
//             };
//             // Act
//             var title = entry.Title;
//             // Assert
//             title.Should().Be("PassDescription");
//         }
// 
//         /// <summary>
//         /// Tests that Title returns "FileName:LineNumber" when Kind is Element.
//         /// </summary>
//         [Fact]
//         public void Title_WhenKindIsElement_ReturnsFileNameAndLineNumber()
//         {
//             // Arrange
//             var filePath = @"C:\folder\file.cs";
//             int lineNumber = 42;
//             var entry = new EvaluationProfileEntry
//             {
//                 SourceFilePath = filePath,
//                 Kind = EvaluationLocationKind.Element,
//                 LineNumber = lineNumber
//             };
//             // Act
//             var title = entry.Title;
//             // Assert
//             title.Should().Be($"{Path.GetFileName(filePath)}:{lineNumber}");
//         }
// //  // [Error] (225-57)CS0117 'EvaluationLocationKind' does not contain a definition for 'Import'
// //         /// <summary>
// //         /// Tests that Title returns "FileName:LineNumber {Kind}" when Kind is not Element.
// //         /// </summary>
// //         [Fact]
// //         public void Title_WhenKindIsNotElement_ReturnsFileNameLineNumberAndKind()
// //         {
// //             // Arrange
// //             var filePath = @"C:\folder\file.cs";
// //             int lineNumber = 10;
// //             // Assuming EvaluationLocationKind has a value other than Element.
// //             var notElementKind = EvaluationLocationKind.Import; // Use a different enum value if available.
// //             var entry = new EvaluationProfileEntry
// //             {
// //                 SourceFilePath = filePath,
// //                 Kind = notElementKind,
// //                 LineNumber = lineNumber
// //             };
// //             // Act
// //             var title = entry.Title;
// //             // Assert
// //             title.Should().Be($"{Path.GetFileName(filePath)}:{lineNumber} {notElementKind}");
// //         }
// // 
//         /// <summary>
//         /// Tests that ShortenedElementDescription returns the original string when its length is below the maximum.
//         /// </summary>
//         [Fact]
//         public void ShortenedElementDescription_WhenShorterThanMaxChars_ReturnsOriginalString()
//         {
//             // Arrange
//             string description = "Short description";
//             var entry = new EvaluationProfileEntry
//             {
//                 ElementDescription = description
//             };
//             // Act
//             var shortened = entry.ShortenedElementDescription;
//             // Assert
//             shortened.Should().Be(description);
//         }
// 
//         /// <summary>
//         /// Tests that ShortenedElementDescription returns a shortened version when the description exceeds the maximum allowed length.
//         /// </summary>
//         [Fact]
//         public void ShortenedElementDescription_WhenLongerThanMaxChars_ReturnsShortenedVersion()
//         {
//             // Arrange
//             // Create a string longer than 80 characters.
//             string longDescription = new string ('A', 100);
//             var entry = new EvaluationProfileEntry
//             {
//                 ElementDescription = longDescription
//             };
//             // Act
//             var shortened = entry.ShortenedElementDescription;
//             // Assert
//             shortened.Length.Should().BeLessThanOrEqualTo(83); // 80 chars + "..."
//             shortened.Should().EndWith("...");
//         }
// 
//         /// <summary>
//         /// Tests that setting and getting the Value property works as expected.
//         /// </summary>
//         [Theory]
//         [InlineData(0.0)]
//         [InlineData(123.45)]
//         [InlineData(-99.99)]
//         public void Value_SetAndGet_ReturnsSameValue(double testValue)
//         {
//             // Arrange
//             var entry = new EvaluationProfileEntry();
//             // Act
//             entry.Value = testValue;
//             var value = entry.Value;
//             // Assert
//             value.Should().Be(testValue);
//         }
// 
//         /// <summary>
//         /// Tests that the TypeName property returns the name of the class.
//         /// </summary>
//         [Fact]
//         public void TypeName_ReturnsCorrectClassName()
//         {
//             // Arrange
//             var entry = new EvaluationProfileEntry();
//             // Act
//             var typeName = entry.TypeName;
//             // Assert
//             typeName.Should().Be(nameof(EvaluationProfileEntry));
//         }
// 
//         /// <summary>
//         /// Tests that ToString returns the same value as Title.
//         /// </summary>
//         [Fact]
//         public void ToString_ReturnsTitleValue()
//         {
//             // Arrange
//             var entry = new EvaluationProfileEntry
//             {
//                 SourceFilePath = @"C:\example\test.cs",
//                 Kind = EvaluationLocationKind.Element,
//                 LineNumber = 15
//             };
//             // Act
//             var toStringValue = entry.ToString();
//             var titleValue = entry.Title;
//             // Assert
//             toStringValue.Should().Be(titleValue);
//         }
//     }
// }