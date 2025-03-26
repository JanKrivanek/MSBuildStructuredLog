// using Microsoft.Build.Logging.StructuredLogger;
// using Moq;
// using StructuredLogViewer;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text.RegularExpressions;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Provides a fake message used for simulating log messages.
//     /// </summary>
// //     internal class FakeMessage [Error] (15-20)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'FakeMessage'
// //     {
// //         public string Text { get; set; }
// //     }
// 
//     /// <summary>
//     /// A testable implementation of CopyTask to allow overriding of dependencies such as HasChildren and GetMessages.
//     /// </summary>
// //     internal class FakeCopyTask : CopyTask [Error] (23-20)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger.UnitTests' already contains a definition for 'FakeCopyTask'
// //     {
// //         private readonly bool _hasChildren;
// //         private readonly IEnumerable<FakeMessage> _messages;
// //         /// <summary>
// //         /// Initializes a new instance of the <see cref = "FakeCopyTask"/> class.
// //         /// </summary>
// //         /// <param name = "hasChildren">Indicates whether the task should simulate having children.</param>
// //         /// <param name = "messages">A collection of fake messages to be returned by GetMessages.</param>
// //         public FakeCopyTask(bool hasChildren, IEnumerable<FakeMessage> messages = null)
// //         {
// //             _hasChildren = hasChildren;
// //             _messages = messages;
// //         }
// // 
// //         /// <summary>
// //         /// Overrides the HasChildren property.
// //         /// </summary>
// //         protected override bool HasChildren => _hasChildren;
// // 
// //         /// <summary>
// //         /// Overrides the GetMessages method to provide fake messages.
// //         /// </summary>
// //         /// <returns>A collection of fake messages cast to dynamic objects.</returns>
// //         protected override IEnumerable<object> GetMessages()
// //         {
// //             // If no messages provided, return an empty list.
// //             if (_messages == null)
// //             {
// //                 return Enumerable.Empty<object>();
// //             }
// // 
// //             return _messages;
// //         }
// //     }
// 
//     /// <summary>
//     /// A testable subclass to expose the protected static ParseCopyingFileFrom method.
//     /// </summary>
//     internal class TestableCopyTask : CopyTask
//     {
//         /// <summary>
//         /// Exposes the protected static ParseCopyingFileFrom method.
//         /// </summary>
//         /// <param name = "match">The regex match to parse.</param>
//         /// <param name = "copied">Indicates if the operation was a copy.</param>
//         /// <returns>A FileCopyOperation instance with parsed values.</returns>
// //         public static FileCopyOperation InvokeParseCopyingFileFrom(Match match, bool copied = true) [Error] (70-68)CS0104 'Match' is an ambiguous reference between 'Moq.Match' and 'System.Text.RegularExpressions.Match' [Error] (72-20)CS0103 The name 'ParseCopyingFileFrom' does not exist in the current context
// //         {
// //             return ParseCopyingFileFrom(match, copied);
// //         }
//     }
// 
//     /// <summary>
//     /// Contains unit tests for the <see cref = "CopyTask"/> class.
//     /// </summary>
//     public class CopyTaskTests
//     {
//         /// <summary>
//         /// Tests that FileCopyOperations property returns an empty collection when HasChildren is false.
//         /// </summary>
//         [Fact]
//         public void FileCopyOperations_HasNoChildren_ReturnsEmptyCollection()
//         {
//             // Arrange
//             var fakeTask = new FakeCopyTask(hasChildren: false);
//             // Act
//             var operations = fakeTask.FileCopyOperations;
//             // Assert
//             Assert.Empty(operations);
//         }
// 
//         /// <summary>
//         /// Tests that GetFileCopyOperations returns an empty collection when HasChildren is true but there are no matching messages.
//         /// </summary>
// //         [Fact] [Error] (104-21)CS7036 There is no argument given that corresponds to the required parameter 'text' of 'FakeMessage.FakeMessage(string)' [Error] (106-21)CS0229 Ambiguity between 'FakeMessage.Text' and 'FakeMessage.Text'
// //         public void GetFileCopyOperations_NoMatchingMessages_ReturnsEmptyCollection()
// //         {
// //             // Arrange
// //             var messages = new List<FakeMessage>
// //             {
// //                 new FakeMessage
// //                 {
// //                     Text = "This is a non matching message"
// //                 }
// //             };
// //             var fakeTask = new FakeCopyTask(hasChildren: true, messages: messages);
// //             // Act
// //             var operations = fakeTask.FileCopyOperations;
// //             // Assert
// //             Assert.Empty(operations);
// //         }
// 
//         /// <summary>
//         /// Tests that GetFileCopyOperations successfully parses messages that match expected patterns.
//         /// The test supplies messages for all three branches:
//         /// - "Copying file from" text which implies a normal copy.
//         /// - "Did not copy file from" text which implies a non-copied operation.
//         /// - "Creating hard link" text which implies a hard link creation.
//         /// Each operation should have the Source and Destination correctly parsed and its Node set to the originating message.
//         /// </summary>
// //         [Fact] [Error] (130-32)CS7036 There is no argument given that corresponds to the required parameter 'text' of 'FakeMessage.FakeMessage(string)' [Error] (132-17)CS0229 Ambiguity between 'FakeMessage.Text' and 'FakeMessage.Text' [Error] (134-32)CS7036 There is no argument given that corresponds to the required parameter 'text' of 'FakeMessage.FakeMessage(string)' [Error] (136-17)CS0229 Ambiguity between 'FakeMessage.Text' and 'FakeMessage.Text' [Error] (138-32)CS7036 There is no argument given that corresponds to the required parameter 'text' of 'FakeMessage.FakeMessage(string)' [Error] (140-17)CS0229 Ambiguity between 'FakeMessage.Text' and 'FakeMessage.Text' [Error] (159-40)CS1061 'FakeCopyOperation' does not contain a definition for 'Node' and no accessible extension method 'Node' accepting a first argument of type 'FakeCopyOperation' could be found (are you missing a using directive or an assembly reference?) [Error] (165-40)CS1061 'FakeCopyOperation' does not contain a definition for 'Node' and no accessible extension method 'Node' accepting a first argument of type 'FakeCopyOperation' could be found (are you missing a using directive or an assembly reference?) [Error] (171-40)CS1061 'FakeCopyOperation' does not contain a definition for 'Node' and no accessible extension method 'Node' accepting a first argument of type 'FakeCopyOperation' could be found (are you missing a using directive or an assembly reference?)
// //         public void GetFileCopyOperations_MatchingMessages_ReturnsCorrectOperations()
// //         {
// //             // Arrange
// //             // Using a pattern assumed to be matched by the corresponding regex in Strings.
// //             // The real regex patterns are defined in Strings, but for test purposes we assume they match these sample texts.
// //             var message1 = new FakeMessage
// //             {
// //                 Text = "Copying file from source.txt to dest.txt"
// //             };
// //             var message2 = new FakeMessage
// //             {
// //                 Text = "Did not copy file from source.txt to dest.txt"
// //             };
// //             var message3 = new FakeMessage
// //             {
// //                 Text = "Creating hard link from source.txt to dest.txt"
// //             };
// //             var messages = new List<FakeMessage>
// //             {
// //                 message1,
// //                 message2,
// //                 message3
// //             };
// //             var fakeTask = new FakeCopyTask(hasChildren: true, messages: messages);
// //             // Act
// //             var operations = fakeTask.FileCopyOperations.ToList();
// //             // Assert
// //             // Expecting three operations, one for each matching branch.
// //             Assert.Equal(3, operations.Count);
// //             // Validate first operation: expected to be a copy operation.
// //             var op1 = operations[0];
// //             Assert.Equal("source.txt", op1.Source);
// //             Assert.Equal("dest.txt", op1.Destination);
// //             Assert.True(op1.Copied);
// //             Assert.Equal(message1, op1.Node);
// //             // Validate second operation: expected to be a non-copied operation.
// //             var op2 = operations[1];
// //             Assert.Equal("source.txt", op2.Source);
// //             Assert.Equal("dest.txt", op2.Destination);
// //             Assert.False(op2.Copied);
// //             Assert.Equal(message2, op2.Node);
// //             // Validate third operation: expected to be a copy operation (hard link creation branch).
// //             var op3 = operations[2];
// //             Assert.Equal("source.txt", op3.Source);
// //             Assert.Equal("dest.txt", op3.Destination);
// //             Assert.True(op3.Copied);
// //             Assert.Equal(message3, op3.Node);
// //         }
// 
//         /// <summary>
//         /// Tests that the protected static method ParseCopyingFileFrom properly parses a regex match into a FileCopyOperation.
//         /// This verifies that the Source, Destination, and Copied properties are set as expected.
//         /// </summary>
// //         [Fact] [Error] (189-79)CS1503 Argument 1: cannot convert from 'System.Text.RegularExpressions.Match' to 'Moq.Match' [Error] (190-82)CS1503 Argument 1: cannot convert from 'System.Text.RegularExpressions.Match' to 'Moq.Match'
// //         public void ParseCopyingFileFrom_ValidMatch_ReturnsCorrectFileCopyOperation()
// //         {
// //             // Arrange
// //             // Create a regex that mimics the expected pattern with named groups "From" and "To".
// //             var pattern = @"^(?<From>source\.txt);(?<To>dest\.txt)$";
// //             var regex = new Regex(pattern);
// //             var input = "source.txt;dest.txt";
// //             var match = regex.Match(input);
// //             Assert.True(match.Success, "The regex should successfully match the input.");
// //             // Act: invoke the protected static method through the testable subclass.
// //             var operationCopied = TestableCopyTask.InvokeParseCopyingFileFrom(match, true);
// //             var operationNotCopied = TestableCopyTask.InvokeParseCopyingFileFrom(match, false);
// //             // Assert
// //             Assert.Equal("source.txt", operationCopied.Source);
// //             Assert.Equal("dest.txt", operationCopied.Destination);
// //             Assert.True(operationCopied.Copied);
// //             Assert.Equal("source.txt", operationNotCopied.Source);
// //             Assert.Equal("dest.txt", operationNotCopied.Destination);
// //             Assert.False(operationNotCopied.Copied);
// //         }
//     }
// }
