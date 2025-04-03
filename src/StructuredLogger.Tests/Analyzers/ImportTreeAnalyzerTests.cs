// using System;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using FluentAssertions;
// using Moq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// A fake implementation of <see cref="StringCache"/> for unit testing purposes.
//     /// This minimal implementation simply returns the input string.
//     /// </summary>
//     internal class FakeStringCache : StringCache
//     {
//         public string SoftIntern(string input)
//         {
//             return input;
//         }
// 
//         public string Intern(string input)
//         {
//             return input;
//         }
//     }
// //  // [Error] (38-48)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeStringCache' to 'Microsoft.Build.Logging.StructuredLogger.StringCache'
// //     /// <summary>
// //     /// Unit tests for the <see cref="ImportTreeAnalyzer"/> class.
// //     /// </summary>
// //     public class ImportTreeAnalyzerTests
// //     {
// //         private readonly FakeStringCache _stringCache;
// //         private readonly ImportTreeAnalyzer _analyzer;
// // 
// //         public ImportTreeAnalyzerTests()
// //         {
// //             _stringCache = new FakeStringCache();
// //             _analyzer = new ImportTreeAnalyzer(_stringCache);
// //         }
// // 
// //         /// <summary>
// //         /// Tests the TryGetImportOrNoImport(ProjectImportedEventArgs) method for the happy path where
// //         /// the event arguments represent a valid project import.
// //         /// Expected behavior: Returns an instance of Import with correctly parsed properties.
// //         /// Note: This test is partial due to unmockable static dependencies (Reflector and Strings).
// //         /// </summary>
// //         [Fact(Skip = "Pending implementation due to unmockable static dependencies in Reflector and Strings.")]
// //         public void TryGetImportOrNoImport_WithProjectImportedEventArgs_MessageProjectImported_ReturnsImport()
// //         {
// //             // Arrange
// //             // TODO: Create a valid ProjectImportedEventArgs instance where:
// //             // Reflector.GetMessage(args) returns Strings.ProjectImported and
// //             // Reflector.GetArguments(args) returns new object[] { "importedProjectPath", "containingProjectPath", "10", "20" }.
// //             ProjectImportedEventArgs args = null!; 
// // 
// //             // Act
// //             TextNode result = _analyzer.TryGetImportOrNoImport(args);
// // 
// //             // Assert
// //             result.Should().BeOfType<Import>("because a valid project import should return an Import instance");
// //             var importResult = result as Import;
// //             importResult.Should().NotBeNull();
// //             // TODO: Assert that importResult contains the expected values.
// //         }
// // 
// //         /// <summary>
// //         /// Tests the TryGetImportOrNoImport(ProjectImportedEventArgs) method for the case where
// //         /// the event arguments indicate a skipped project import due to a false condition.
// //         /// Expected behavior: Returns an instance of NoImport with appropriate details.
// //         /// Note: This test is partial due to unmockable static dependencies (Reflector and Strings).
// //         /// </summary>
// //         [Fact(Skip = "Pending implementation due to unmockable static dependencies in Reflector and Strings.")]
// //         public void TryGetImportOrNoImport_WithProjectImportedEventArgs_MessageProjectImportSkippedFalseCondition_ReturnsNoImport()
// //         {
// //             // Arrange
// //             // TODO: Create a valid ProjectImportedEventArgs instance where:
// //             // Reflector.GetMessage(args) returns Strings.ProjectImportSkippedFalseCondition and
// //             // Reflector.GetArguments(args) returns new object[] { "projectPath", "containingProjectPath", "15", "25", "someCondition", "evaluatedValue" }.
// //             ProjectImportedEventArgs args = null!; 
// // 
// //             // Act
// //             TextNode result = _analyzer.TryGetImportOrNoImport(args);
// // 
// //             // Assert
// //             result.Should().BeOfType<NoImport>("because a skipped project import should return a NoImport instance");
// //             var noImportResult = result as NoImport;
// //             noImportResult.Should().NotBeNull();
// //             // TODO: Assert that noImportResult contains the expected details.
// //         }
// // 
// //         /// <summary>
// //         /// Tests the TryGetImportOrNoImport(ProjectImportedEventArgs) method for the case where
// //         /// the event arguments indicate a skipped project import due to an unresolved SDK.
// //         /// Expected behavior: Returns an instance of NoImport with details indicating the SDK resolution failure.
// //         /// Note: This test is partial due to unmockable static dependencies (Reflector and Strings).
// //         /// </summary>
// //         [Fact(Skip = "Pending implementation due to unmockable static dependencies in Reflector and Strings.")]
// //         public void TryGetImportOrNoImport_WithProjectImportedEventArgs_MessageCouldNotResolveSdk_ReturnsNoImport()
// //         {
// //             // Arrange
// //             // TODO: Create a valid ProjectImportedEventArgs instance where:
// //             // Reflector.GetMessage(args) returns Strings.CouldNotResolveSdk and
// //             // Reflector.GetArguments(args) returns new object[] { "sdkName" }.
// //             ProjectImportedEventArgs args = null!; 
// // 
// //             // Act
// //             TextNode result = _analyzer.TryGetImportOrNoImport(args);
// // 
// //             // Assert
// //             result.Should().BeOfType<NoImport>("because an unresolved SDK scenario should return a NoImport instance");
// //             var noImportResult = result as NoImport;
// //             noImportResult.Should().NotBeNull();
// //             // TODO: Assert that noImportResult contains the expected details.
// //         }
// //  // [Error] (127-90)CS1503 Argument 2: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeStringCache' to 'Microsoft.Build.Logging.StructuredLogger.StringCache'
// //         /// <summary>
// //         /// Tests the static TryGetImportOrNoImport(string, StringCache) method when the provided text does not match any expected pattern.
// //         /// Expected behavior: Returns null.
// //         /// </summary>
// //         [Fact]
// //         public void TryGetImportOrNoImport_WithNonMatchingText_ReturnsNull()
// //         {
// //             // Arrange
// //             string nonMatchingText = "This text does not match any expected import pattern.";
// // 
// //             // Act
// //             TextNode result = ImportTreeAnalyzer.TryGetImportOrNoImport(nonMatchingText, _stringCache);
// // 
// //             // Assert
// //             result.Should().BeNull("because non matching text should result in a null return value");
// //         }
// //  // [Error] (146-87)CS1503 Argument 2: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeStringCache' to 'Microsoft.Build.Logging.StructuredLogger.StringCache'
// //         /// <summary>
// //         /// Tests the static TryGetImportOrNoImport(string, StringCache) method when the provided text matches the expected project import pattern.
// //         /// Expected behavior: Returns an instance of Import with the properties correctly parsed from the text.
// //         /// Note: This test is partial. A valid test input string that conforms to Strings.ProjectImportedRegex is required.
// //         /// </summary>
// //         [Fact(Skip = "Pending implementation: supply a valid matching text based on the actual regex pattern.")]
// //         public void TryGetImportOrNoImport_WithMatchingText_ReturnsExpectedTextNode()
// //         {
// //             // Arrange
// //             // TODO: Supply a test string that matches the pattern defined in Strings.ProjectImportedRegex.
// //             string matchingText = "TODO: supply valid matching text";
// // 
// //             // Act
// //             TextNode result = ImportTreeAnalyzer.TryGetImportOrNoImport(matchingText, _stringCache);
// // 
// //             // Assert
// //             result.Should().BeOfType<Import>("because a matching text should be parsed into an Import instance");
// //             var importResult = result as Import;
// //             importResult.Should().NotBeNull();
// //             // TODO: Assert that importResult has the expected property values extracted from matchingText.
// //         }
// //     }
// // }