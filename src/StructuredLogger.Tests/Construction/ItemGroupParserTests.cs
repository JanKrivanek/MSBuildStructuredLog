// using FluentAssertions;
// using Moq;
// using System;
// using System.Collections.Generic;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="ItemGroupParser"/> class.
//     /// </summary>
//     public class ItemGroupParserTests
//     {
//         private readonly StringCache _stringCache;
// 
//         public ItemGroupParserTests()
//         {
//             // Assuming StringCache has a public parameterless constructor.
//             _stringCache = new StringCache();
//         }
// //  // [Error] (36-88)CS1503 Argument 3: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.StringCache' to 'Microsoft.Build.Logging.StructuredLogger.StringCache'
// //         /// <summary>
// //         /// Tests the <see cref="ItemGroupParser.ParsePropertyOrItemList(string, string, StringCache, bool)"/> method
// //         /// for a message that does not contain line breaks and when isOutputItem is false.
// //         /// Expected to return a Property node with correctly interned Name and Value.
// //         /// </summary>
// //         [Theory]
// //         [InlineData("Output Item(s):key=value", "Output Item(s):")]
// //         public void ParsePropertyOrItemList_NoLineBreak_IsOutputItemFalse_ReturnsProperty(string message, string prefix)
// //         {
// //             // Arrange
// //             bool isOutputItem = false;
// //             
// //             // Act
// //             BaseNode result = ItemGroupParser.ParsePropertyOrItemList(message, prefix, _stringCache, isOutputItem);
// // 
// //             // Assert
// //             result.Should().BeOfType<Property>("because a message without line breaks should be parsed as a Property when isOutputItem is false");
// //             var property = result as Property;
// //             property.Name.Should().Be(_stringCache.Intern("key"));
// //             property.Value.Should().Be(_stringCache.Intern("value"));
// //         }
// //  // [Error] (58-88)CS1503 Argument 3: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.StringCache' to 'Microsoft.Build.Logging.StructuredLogger.StringCache' // [Error] (62-27)CS0039 Cannot convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' to 'Microsoft.Build.Logging.StructuredLogger.AddItem' via a reference conversion, boxing conversion, unboxing conversion, wrapping conversion, or null type conversion // [Error] (67-29)CS0039 Cannot convert type 'Microsoft.Build.Logging.StructuredLogger.BaseNode' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Item' via a reference conversion, boxing conversion, unboxing conversion, wrapping conversion, or null type conversion
//         /// <summary>
//         /// Tests the <see cref="ItemGroupParser.ParsePropertyOrItemList(string, string, StringCache, bool)"/> method
//         /// for a message that does not contain line breaks and when isOutputItem is true.
//         /// Expected to return an AddItem node with a child Item node whose Text is correctly interned.
//         /// </summary>
//         [Theory]
//         [InlineData("Output Item(s):key=value", "Output Item(s):")]
//         public void ParsePropertyOrItemList_NoLineBreak_IsOutputItemTrue_ReturnsAddItemWithChildItem(string message, string prefix)
//         {
//             // Arrange
//             bool isOutputItem = true;
//             
//             // Act
//             BaseNode result = ItemGroupParser.ParsePropertyOrItemList(message, prefix, _stringCache, isOutputItem);
// 
//             // Assert
//             result.Should().BeOfType<AddItem>("because a message without line breaks should be parsed as an AddItem when isOutputItem is true");
//             var addItem = result as AddItem;
//             addItem.Name.Should().Be(_stringCache.Intern("key"));
// 
//             // Verify that at least one child Item node exists and that its Text property is correctly assigned.
//             addItem.Children.Should().NotBeEmpty("because a non-line-break message with output items should add a child Item");
//             var childItem = addItem.Children[0] as Item;
//             childItem.Should().NotBeNull();
//             childItem.Text.Should().Be(_stringCache.Intern("value"));
//         }
// //  // [Error] (88-88)CS1503 Argument 3: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.StringCache' to 'Microsoft.Build.Logging.StructuredLogger.StringCache' // [Error] (93-29)CS0039 Cannot convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' to 'Microsoft.Build.Logging.StructuredLogger.Parameter' via a reference conversion, boxing conversion, unboxing conversion, wrapping conversion, or null type conversion
// //         /// <summary>
// //         /// Tests the <see cref="ItemGroupParser.ParsePropertyOrItemList(string, string, StringCache, bool)"/> method
// //         /// when the message contains line breaks to simulate a multi-line value.
// //         /// This test targets the branch where the first line span length is greater than the prefix length.
// //         /// </summary>
// //         [Fact]
// //         public void ParsePropertyOrItemList_WithLineBreak_MultiLineValue_ReturnsParameterizedNode()
// //         {
// //             // Arrange
// //             // This message simulates a multi-line input.
// //             // Note: The actual behavior depends on the implementation of CollectLineSpans and TextUtilities.
// //             // Adjust the input as necessary to match the expected behavior.
// //             string prefix = "Output Item(s): ";
// //             string message = prefix + "key=value\ncontinued line of value";
// //             
// //             // Act
// //             BaseNode result = ItemGroupParser.ParsePropertyOrItemList(message, prefix, _stringCache, false);
// // 
// //             // Assert
// //             // When a multi-line message is parsed, a Parameter node is expected.
// //             result.Should().BeOfType<Parameter>("because a multi-line input should be parsed as a Parameter node");
// //             var parameter = result as Parameter;
// //             parameter.Name.Should().Be(_stringCache.Intern("key"));
// //             parameter.Children.Should().NotBeEmpty("because a multi-line input should add child items for each continuation line");
// //             
// //             // Additional assertions might be added once the exact behavior of multi-line parsing is defined.
// //         }
// //  // [Error] (116-69)CS1503 Argument 3: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.StringCache' to 'Microsoft.Build.Logging.StructuredLogger.StringCache'
//         /// <summary>
//         /// Tests the <see cref="ItemGroupParser.ParseThereWasAConflict(TreeNode, string, StringCache)"/> method
//         /// to ensure it properly parses a conflict message and adds child nodes to the parent.
//         /// </summary>
//         [Fact]
//         public void ParseThereWasAConflict_ValidMessage_AddsChildNodesToParent()
//         {
//             // Arrange
//             // Create a dummy parent TreeNode. Here we use Parameter as a concrete implementation of TreeNode.
//             var parent = new Parameter();
//             
//             // Simulate a message with multiple lines and varying leading spaces.
//             // The message content is contrived based on the parsing logic in ParseThereWasAConflict.
//             string message = "RootLine\n    FourSpaceLine=\n        EightSpaceLine=SomeValue\n          SixteenSpaceLine";
//             
//             // Act
//             ItemGroupParser.ParseThereWasAConflict(parent, message, _stringCache);
// 
//             // Assert
//             // Since the exact tree structure depends on the implementation details of
//             // CollectLineSpans and the parsing logic, we check that the parent's children collection has been modified.
//             parent.Children.Should().NotBeEmpty("because parsing a conflict message should add child nodes to the parent");
//         }
//     }
// }