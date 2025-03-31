// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text.RegularExpressions;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="CopyTask"/> class.
//     /// </summary>
//     public class CopyTaskTests
//     {
//         /// <summary>
//         /// Tests that the FileCopyOperations property returns an empty collection when there are no children.
//         /// </summary>
//         [Fact]
//         public void FileCopyOperations_NoChildren_ReturnsEmptyCollection()
//         {
//             // Arrange
//             var messages = new List<FakeMessage>
//             {
//                 // Even if messages exist, HasChildren is false so they will not be processed.
//                 new FakeMessage("source->destination")
//             };
//             var task = new TestableCopyTask(messages, hasChildren: false);
// 
//             // Act
//             var operations = task.FileCopyOperations;
// 
//             // Assert
//             operations.Should().BeEmpty("because there are no children to process");
//         }
// 
//         /// <summary>
//         /// Tests that the FileCopyOperations property returns an empty collection when messages do not match any expected pattern.
//         /// </summary>
//         [Fact]
//         public void FileCopyOperations_MessagesDoNotMatch_ReturnsEmptyCollection()
//         {
//             // Arrange
//             var messages = new List<FakeMessage>
//             {
//                 new FakeMessage("this is an unrelated message"),
//                 new FakeMessage("another irrelevant text")
//             };
//             var task = new TestableCopyTask(messages, hasChildren: true);
// 
//             // Act
//             var operations = task.FileCopyOperations;
// 
//             // Assert
//             operations.Should().BeEmpty("because none of the messages match the expected file copying patterns");
//         }
// 
//         /// <summary>
//         /// Tests that the ParseCopyingFileFrom protected static method correctly extracts file copy information when the match is successful.
//         /// </summary>
//         [Fact]
//         public void ParseCopyingFileFrom_ValidMatch_ReturnsProperFileCopyOperation()
//         {
//             // Arrange
//             // Create a regex pattern that simulates the expected format: "source->destination"
//             string input = "source->destination";
//             string pattern = @"^(?<From>.+?)->(?<To>.+)$";
//             var regex = new Regex(pattern);
//             var match = regex.Match(input);
//             match.Success.Should().BeTrue("the test regex should match the input");
//             match.Groups.Count.Should().BeGreaterThan(2, "there should be at least 3 groups (the full match, 'From', and 'To')");
// 
//             // Act
//             var operationCopied = TestableCopyTask.ExposeParseCopyingFileFrom(match);
//             var operationNotCopied = TestableCopyTask.ExposeParseCopyingFileFrom(match, copied: false);
// 
//             // Assert
//             operationCopied.Source.Should().Be("source", "the source should be extracted from the 'From' group");
//             operationCopied.Destination.Should().Be("destination", "the destination should be extracted from the 'To' group");
//             operationCopied.Copied.Should().BeTrue("the default value indicates the file was copied");
// 
//             operationNotCopied.Copied.Should().BeFalse("the method parameter indicates the file was not copied");
//         }
// 
//         /// <summary>
//         /// Tests that the FileCopyOperations property processes a message that matches the expected file copying pattern.
//         /// This test overrides the regex patterns in a controlled environment.
//         /// </summary>
//         [Fact]
//         public void FileCopyOperations_MessageMatchesPattern_ReturnsFileCopyOperation()
//         {
//             // Arrange
//             // For this test, we simulate that all three regexes behave the same.
//             // We assume that a message with text "source->destination" will match.
//             // NOTE: This test depends on the production Strings regex values.
//             // If the production regexes do not match this simple pattern, the test might need adjustment.
//             var testMessage = new FakeMessage("source->destination");
//             var messages = new List<FakeMessage> { testMessage };
//             var task = new TestableCopyTask(messages, hasChildren: true);
// 
//             // Act
//             var operations = task.FileCopyOperations.ToList();
// 
//             // Assert
//             operations.Should().ContainSingle("because one message matches the expected pattern");
//             var operation = operations.First();
//             operation.Source.Should().Be("source", "the source should be parsed from the message text");
//             operation.Destination.Should().Be("destination", "the destination should be parsed from the message text");
//             operation.Copied.Should().BeTrue("the default behavior indicates a copied operation");
//             operation.Node.Should().Be(testMessage, "the operation node should reference the originating message");
//         }
// 
//         #region Testable Helper Classes
// 
//         /// <summary>
//         /// A fake message class to simulate messages processed by CopyTask.
//         /// </summary>
//         private class FakeMessage
//         {
//             public string Text { get; }
// 
//             public FakeMessage(string text)
//             {
//                 Text = text;
//             }
//         }
// 
//         /// <summary>
//         /// A testable subclass of CopyTask to expose protected members and allow controlled testing.
//         /// </summary>
//         private class TestableCopyTask : CopyTask
//         {
//             private readonly IEnumerable<FakeMessage> _fakeMessages;
//             private readonly bool _hasChildren;
// 
//             public TestableCopyTask(IEnumerable<FakeMessage> fakeMessages, bool hasChildren)
//             {
//                 _fakeMessages = fakeMessages;
//                 _hasChildren = hasChildren;
//             }
// //  // [Error] (143-37)CS0506 'CopyTaskTests.TestableCopyTask.HasChildren': cannot override inherited member 'TreeNode.HasChildren' because it is not marked virtual, abstract, or override
// //             // Simulate the HasChildren property.
// //             protected override bool HasChildren => _hasChildren;
// //  // [Error] (146-52)CS0506 'CopyTaskTests.TestableCopyTask.GetMessages()': cannot override inherited member 'Task.GetMessages()' because it is not marked virtual, abstract, or override
//             // Override GetMessages to return a controlled list of fake messages.
//             protected override IEnumerable<object> GetMessages()
//             {
//                 // Returning as object here; the base method uses dynamic typing (accessing .Text property)
//                 return _fakeMessages.Cast<object>();
//             }
// 
//             /// <summary>
//             /// Exposes the protected static ParseCopyingFileFrom method for testing.
//             /// </summary>
//             /// <param name="match">The regex match to parse.</param>
//             /// <param name="copied">Indicates whether the file was copied.</param>
//             /// <returns>A new FileCopyOperation instance.</returns>
//             public static FileCopyOperation ExposeParseCopyingFileFrom(Match match, bool copied = true)
//             {
//                 return ParseCopyingFileFrom(match, copied);
//             }
//         }
// 
//         #endregion
//     }
// }