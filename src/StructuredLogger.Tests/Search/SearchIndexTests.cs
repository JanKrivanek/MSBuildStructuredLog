// using System;
// using System.Collections.Generic;
// using System.Threading;
// using FluentAssertions;
// using Moq;
// using Xunit;
// using Microsoft.Build.Logging.StructuredLogger;
// using StructuredLogViewer;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// NOTE: The tests for SearchIndex rely on a valid instance of Build.
//     /// Since Build and its related types (e.g. StringTable, SearchExtensions) cannot be easily instantiated or mocked,
//     /// please provide a proper test implementation or test double for Build when integrating these tests.
//     /// The tests below are partial examples and need a valid Build instance setup.
//     /// </summary>
//     public class SearchIndexTests
//     {
//         // TODO: Create a valid Build instance that supplies a sealed StringTable with Instances and SearchExtensions.
//         private readonly Build _validBuild = null!; // assign a valid Build instance here
// 
//         public SearchIndexTests()
//         {
//             // Arrange: Initialize _validBuild with a proper non-null instance.
//             // For example: _validBuild = new Build(...);
//             // Since Build cannot be mocked, this is a placeholder.
//         }
// //  // [Error] (45-47)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         /// <summary>
// //         /// Tests that constructing a SearchIndex with a valid Build instance sets the Strings and NodeCount properties.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithValidBuild_InitializesProperties()
// //         {
// //             // Arrange
// //             // Ensure _validBuild is set before running this test
// //             if (_validBuild is null)
// //             {
// //                 // Indicate that a valid Build instance must be provided to run this test.
// //                 throw new NotImplementedException("Provide a valid Build instance for testing SearchIndex.");
// //             }
// // 
// //             // Act
// //             var searchIndex = new SearchIndex(_validBuild);
// // 
// //             // Assert
// //             searchIndex.Strings.Should().NotBeNull();
// //             searchIndex.NodeCount.Should().BeGreaterOrEqualTo(0);
// //         }
// //  // [Error] (63-47)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
//         /// <summary>
//         /// Tests the FindNodes method for a simple query.
//         /// </summary>
//         [Fact]
//         public void FindNodes_SimpleQuery_ReturnsSearchResultsOrEmptyList()
//         {
//             // Arrange
//             if (_validBuild is null)
//             {
//                 throw new NotImplementedException("Provide a valid Build instance for testing SearchIndex.");
//             }
//             var searchIndex = new SearchIndex(_validBuild)
//             {
//                 MaxResults = 10,
//                 MarkResultsInTree = false
//             };
//             string query = "sample";
//             CancellationToken token = CancellationToken.None;
// 
//             // Act
//             IEnumerable<SearchResult> results = searchIndex.FindNodes(query, token);
// 
//             // Assert
//             results.Should().NotBeNull();
//             // Depending on the Build instance content, results may be empty or contain matches.
//             // This test simply asserts that a result enumerable is provided.
//         }
// //  // [Error] (91-64)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode' // [Error] (97-47)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Build' to 'Microsoft.Build.Logging.StructuredLogger.Build'
// //         /// <summary>
// //         /// Tests the IsMatch method with a dummy NodeEntry and matcher.
// //         /// Note: This test is only partial as the behavior depends on the node and matcher details.
// //         /// </summary>
// //         [Fact]
// //         public void IsMatch_WithDummyParameters_ReturnsExpectedResultOrNull()
// //         {
// //             // Arrange
// //             // Since NodeQueryMatcher and term processing require a valid node and string table,
// //             // this test is a placeholder. Create a dummy NodeEntry.
// //             var dummyNode = new Mock<BaseNode>().Object;
// //             var nodeEntry = new NodeEntry { Field1 = 1, Node = dummyNode };
// //             // Creating a minimal NodeQueryMatcher instance may require proper setup.
// //             // For the purpose of this test, we assume a dummy matcher with default property values.
// //             var matcher = new NodeQueryMatcher("dummy");
// //             Term[] terms = new Term[0];
// // 
// //             var searchIndex = new SearchIndex(_validBuild ?? throw new NotImplementedException("Provide a valid Build instance."));
// // 
// //             // Act
// //             SearchResult? result = searchIndex.IsMatch(matcher, nodeEntry, terms);
// // 
// //             // Assert
// //             // Since behavior is undefined with dummy input, we simply assert that the method returns either null or a SearchResult.
// //             (result == null || result is SearchResult).Should().BeTrue();
// //         }
// //     }
// //  // [Error] (120-24)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
// //     /// <summary>
// //     /// Unit tests for the <see cref="NodeEntry"/> class.
// //     /// </summary>
// //     public class NodeEntryTests
// //     {
// //         private readonly NodeEntry _nodeEntry;
// // 
// //         public NodeEntryTests()
// //         {
// //             _nodeEntry = new NodeEntry
// //             {
// //                 Field1 = 42,
// //                 Node = new Mock<BaseNode>().Object
// //             };
// //         }
// // 
// //         /// <summary>
// //         /// Tests that GetField returns Field1 when index is 0.
// //         /// </summary>
// //         [Fact]
// //         public void GetField_IndexZero_ReturnsField1()
// //         {
// //             // Act
// //             int result = _nodeEntry.GetField(0);
// // 
// //             // Assert
// //             result.Should().Be(_nodeEntry.Field1);
// //         }
// // 
// //         /// <summary>
// //         /// Tests that GetField returns 0 for any index greater than 0.
// //         /// </summary>
// //         [Fact]
// //         public void GetField_IndexGreaterThanZero_ReturnsZero()
// //         {
// //             // Act
// //             int result = _nodeEntry.GetField(1);
// // 
// //             // Assert
// //             result.Should().Be(0);
// //         }
// //     }
// //  // [Error] (164-24)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
//     /// <summary>
//     /// Unit tests for the <see cref="NodeEntry2"/> class.
//     /// </summary>
//     public class NodeEntry2Tests
//     {
//         private readonly NodeEntry2 _nodeEntry2;
// 
//         public NodeEntry2Tests()
//         {
//             _nodeEntry2 = new NodeEntry2
//             {
//                 Field1 = 10,
//                 Field2 = 20,
//                 Node = new Mock<BaseNode>().Object
//             };
//         }
// 
//         /// <summary>
//         /// Tests that GetField returns Field1 when index is 0.
//         /// </summary>
//         [Fact]
//         public void GetField_IndexZero_ReturnsField1()
//         {
//             // Act
//             int result = _nodeEntry2.GetField(0);
// 
//             // Assert
//             result.Should().Be(_nodeEntry2.Field1);
//         }
// 
//         /// <summary>
//         /// Tests that GetField returns Field2 when index is 1.
//         /// </summary>
//         [Fact]
//         public void GetField_IndexOne_ReturnsField2()
//         {
//             // Act
//             int result = _nodeEntry2.GetField(1);
// 
//             // Assert
//             result.Should().Be(_nodeEntry2.Field2);
//         }
// 
//         /// <summary>
//         /// Tests that GetField returns 0 for index 2.
//         /// </summary>
//         [Fact]
//         public void GetField_IndexGreaterThanOne_ReturnsZero()
//         {
//             // Act
//             int result = _nodeEntry2.GetField(2);
// 
//             // Assert
//             result.Should().Be(0);
//         }
//     }
// //  // [Error] (222-24)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
// //     /// <summary>
// //     /// Unit tests for the <see cref="NodeEntry3"/> class.
// //     /// </summary>
// //     public class NodeEntry3Tests
// //     {
// //         private readonly NodeEntry3 _nodeEntry3;
// // 
// //         public NodeEntry3Tests()
// //         {
// //             _nodeEntry3 = new NodeEntry3
// //             {
// //                 Field1 = 5,
// //                 Field2 = 15,
// //                 Field3 = 25,
// //                 Node = new Mock<BaseNode>().Object
// //             };
// //         }
// // 
// //         /// <summary>
// //         /// Tests that GetField returns Field3 when index is 2.
// //         /// </summary>
// //         [Fact]
// //         public void GetField_IndexTwo_ReturnsField3()
// //         {
// //             // Act
// //             int result = _nodeEntry3.GetField(2);
// // 
// //             // Assert
// //             result.Should().Be(_nodeEntry3.Field3);
// //         }
// // 
// //         /// <summary>
// //         /// Tests that GetField returns Field1 or Field2 for indices 0 and 1 respectively.
// //         /// </summary>
// //         [Fact]
// //         public void GetField_IndexZeroAndOne_ReturnsCorrespondingFields()
// //         {
// //             // Act
// //             int result0 = _nodeEntry3.GetField(0);
// //             int result1 = _nodeEntry3.GetField(1);
// //             int result3 = _nodeEntry3.GetField(3);
// // 
// //             // Assert
// //             result0.Should().Be(_nodeEntry3.Field1);
// //             result1.Should().Be(_nodeEntry3.Field2);
// //             result3.Should().Be(0);
// //         }
// //     }
// //  // [Error] (272-24)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
//     /// <summary>
//     /// Unit tests for the <see cref="NodeEntry4"/> class.
//     /// </summary>
//     public class NodeEntry4Tests
//     {
//         private readonly NodeEntry4 _nodeEntry4;
// 
//         public NodeEntry4Tests()
//         {
//             _nodeEntry4 = new NodeEntry4
//             {
//                 Field1 = 1,
//                 Field2 = 2,
//                 Field3 = 3,
//                 Field4 = 4,
//                 Node = new Mock<BaseNode>().Object
//             };
//         }
// 
//         /// <summary>
//         /// Tests that GetField returns Field4 when index is 3.
//         /// </summary>
//         [Fact]
//         public void GetField_IndexThree_ReturnsField4()
//         {
//             // Act
//             int result = _nodeEntry4.GetField(3);
// 
//             // Assert
//             result.Should().Be(_nodeEntry4.Field4);
//         }
// 
//         /// <summary>
//         /// Tests that GetField returns correct values for index 0 to 2.
//         /// </summary>
//         [Fact]
//         public void GetField_IndicesZeroToTwo_ReturnsCorrespondingFields()
//         {
//             // Act
//             int result0 = _nodeEntry4.GetField(0);
//             int result1 = _nodeEntry4.GetField(1);
//             int result2 = _nodeEntry4.GetField(2);
// 
//             // Assert
//             result0.Should().Be(_nodeEntry4.Field1);
//             result1.Should().Be(_nodeEntry4.Field2);
//             result2.Should().Be(_nodeEntry4.Field3);
//         }
//     }
// //  // [Error] (323-24)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
// //     /// <summary>
// //     /// Unit tests for the <see cref="NodeEntry5"/> class.
// //     /// </summary>
// //     public class NodeEntry5Tests
// //     {
// //         private readonly NodeEntry5 _nodeEntry5;
// // 
// //         public NodeEntry5Tests()
// //         {
// //             _nodeEntry5 = new NodeEntry5
// //             {
// //                 Field1 = 7,
// //                 Field2 = 8,
// //                 Field3 = 9,
// //                 Field4 = 10,
// //                 Field5 = 11,
// //                 Node = new Mock<BaseNode>().Object
// //             };
// //         }
// // 
// //         /// <summary>
// //         /// Tests that GetField returns Field5 when index is 4.
// //         /// </summary>
// //         [Fact]
// //         public void GetField_IndexFour_ReturnsField5()
// //         {
// //             // Act
// //             int result = _nodeEntry5.GetField(4);
// // 
// //             // Assert
// //             result.Should().Be(_nodeEntry5.Field5);
// //         }
// // 
// //         /// <summary>
// //         /// Tests that GetField returns correct values for indices 0 to 3.
// //         /// </summary>
// //         [Fact]
// //         public void GetField_IndicesZeroToThree_ReturnsCorrespondingFields()
// //         {
// //             // Act
// //             int result0 = _nodeEntry5.GetField(0);
// //             int result1 = _nodeEntry5.GetField(1);
// //             int result2 = _nodeEntry5.GetField(2);
// //             int result3 = _nodeEntry5.GetField(3);
// //             int result5 = _nodeEntry5.GetField(5);
// // 
// //             // Assert
// //             result0.Should().Be(_nodeEntry5.Field1);
// //             result1.Should().Be(_nodeEntry5.Field2);
// //             result2.Should().Be(_nodeEntry5.Field3);
// //             result3.Should().Be(_nodeEntry5.Field4);
// //             result5.Should().Be(0);
// //         }
// //     }
// //  // [Error] (379-24)CS0029 Cannot implicitly convert type 'Microsoft.Build.Logging.StructuredLogger.UnitTests.BaseNode' to 'Microsoft.Build.Logging.StructuredLogger.BaseNode'
//     /// <summary>
//     /// Unit tests for the <see cref="NodeEntry6"/> class.
//     /// </summary>
//     public class NodeEntry6Tests
//     {
//         private readonly NodeEntry6 _nodeEntry6;
// 
//         public NodeEntry6Tests()
//         {
//             _nodeEntry6 = new NodeEntry6
//             {
//                 Field1 = 100,
//                 Field2 = 200,
//                 Field3 = 300,
//                 Field4 = 400,
//                 Field5 = 500,
//                 Field6 = 600,
//                 Node = new Mock<BaseNode>().Object
//             };
//         }
// 
//         /// <summary>
//         /// Tests that GetField returns Field6 when index is 5.
//         /// </summary>
//         [Fact]
//         public void GetField_IndexFive_ReturnsField6()
//         {
//             // Act
//             int result = _nodeEntry6.GetField(5);
// 
//             // Assert
//             result.Should().Be(_nodeEntry6.Field6);
//         }
// 
//         /// <summary>
//         /// Tests that GetField returns correct values for indices 0 to 4.
//         /// </summary>
//         [Fact]
//         public void GetField_IndicesZeroToFour_ReturnsCorrespondingFields()
//         {
//             // Act
//             int result0 = _nodeEntry6.GetField(0);
//             int result1 = _nodeEntry6.GetField(1);
//             int result2 = _nodeEntry6.GetField(2);
//             int result3 = _nodeEntry6.GetField(3);
//             int result4 = _nodeEntry6.GetField(4);
//             int result6 = _nodeEntry6.GetField(6);
// 
//             // Assert
//             result0.Should().Be(_nodeEntry6.Field1);
//             result1.Should().Be(_nodeEntry6.Field2);
//             result2.Should().Be(_nodeEntry6.Field3);
//             result3.Should().Be(_nodeEntry6.Field4);
//             result4.Should().Be(_nodeEntry6.Field5);
//             result6.Should().Be(0);
//         }
//     }
// }