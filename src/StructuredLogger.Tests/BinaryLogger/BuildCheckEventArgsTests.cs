// using FluentAssertions;
// using Microsoft.Build.Framework;
// using StructuredLogger.BinaryLogger;
// using System;
// using System.Collections.Generic;
// using Xunit;
// 
// namespace StructuredLogger.BinaryLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="BuildCheckTracingEventArgs"/> class.
//     /// </summary>
//     public class BuildCheckTracingEventArgsTests
//     {
//         private readonly Dictionary<string, TimeSpan> _sampleTracingData;
//         
//         public BuildCheckTracingEventArgsTests()
//         {
//             _sampleTracingData = new Dictionary<string, TimeSpan>
//             {
//                 { "Task1", TimeSpan.FromSeconds(10) },
//                 { "Task2", TimeSpan.FromSeconds(20) }
//             };
//         }
// 
//         /// <summary>
//         /// Tests that the BuildCheckTracingEventArgs constructor correctly assigns a valid tracing data dictionary.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithValidTracingData_SetsTracingDataCorrectly()
//         {
//             // Act
//             var eventArgs = new BuildCheckTracingEventArgs(_sampleTracingData);
// 
//             // Assert
//             eventArgs.TracingData.Should().BeEquivalentTo(_sampleTracingData, "the tracing data should be assigned from the constructor");
//         }
// 
//         /// <summary>
//         /// Tests that the BuildCheckTracingEventArgs constructor correctly assigns an empty dictionary.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithEmptyTracingData_SetsTracingDataToEmpty()
//         {
//             // Arrange
//             var emptyData = new Dictionary<string, TimeSpan>();
// 
//             // Act
//             var eventArgs = new BuildCheckTracingEventArgs(emptyData);
// 
//             // Assert
//             eventArgs.TracingData.Should().BeEmpty("an empty dictionary should be assigned correctly without errors");
//         }
// 
//         /// <summary>
//         /// Tests the behavior of the BuildCheckTracingEventArgs constructor when null is provided.
//         /// Note: Although non-nullable, this tests runtime behavior using null-forgiving operator.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithNullTracingData_AllowsNullAssignment()
//         {
//             // Arrange
//             Dictionary<string, TimeSpan>? nullData = null;
// 
//             // Act
//             var eventArgs = new BuildCheckTracingEventArgs(nullData!);
// 
//             // Assert
//             eventArgs.TracingData.Should().BeNull("if null is provided, the tracing data property should reflect the null value");
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="BuildCheckAcquisitionEventArgs"/> class.
//     /// </summary>
//     public class BuildCheckAcquisitionEventArgsTests
//     {
//         private readonly string _sampleAcquisitionPath;
//         private readonly string _sampleProjectPath;
// 
//         public BuildCheckAcquisitionEventArgsTests()
//         {
//             _sampleAcquisitionPath = "C:\\Sample\\Acquisition.path";
//             _sampleProjectPath = "C:\\Sample\\Project.path";
//         }
// 
//         /// <summary>
//         /// Tests that the BuildCheckAcquisitionEventArgs constructor correctly assigns the acquisition and project paths.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithValidPaths_SetsPropertiesCorrectly()
//         {
//             // Act
//             var eventArgs = new BuildCheckAcquisitionEventArgs(_sampleAcquisitionPath, _sampleProjectPath);
// 
//             // Assert
//             eventArgs.AcquisitionPath.Should().Be(_sampleAcquisitionPath, "the acquisition path should match the provided value");
//             eventArgs.ProjectPath.Should().Be(_sampleProjectPath, "the project path should match the provided value");
//         }
// 
//         /// <summary>
//         /// Tests that the BuildCheckAcquisitionEventArgs constructor correctly assigns empty string values.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithEmptyPaths_SetsPropertiesToEmpty()
//         {
//             // Arrange
//             string emptyPath = string.Empty;
// 
//             // Act
//             var eventArgs = new BuildCheckAcquisitionEventArgs(emptyPath, emptyPath);
// 
//             // Assert
//             eventArgs.AcquisitionPath.Should().Be(emptyPath, "the acquisition path should be empty when provided as empty string");
//             eventArgs.ProjectPath.Should().Be(emptyPath, "the project path should be empty when provided as empty string");
//         }
//     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="BuildCheckResultMessage"/> class.
//     /// </summary>
//     public class BuildCheckResultMessageTests
//     {
//         private readonly string _sampleMessage;
// 
//         public BuildCheckResultMessageTests()
//         {
//             _sampleMessage = "Test message payload";
//         }
// //  // [Error] (141-27)CS0122 'BuildEventArgs.RawMessage' is inaccessible due to its protection level
// //         /// <summary>
// //         /// Tests that the BuildCheckResultMessage constructor correctly assigns the provided message.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithValidMessage_SetsRawMessageCorrectly()
// //         {
// //             // Act
// //             var resultMessage = new BuildCheckResultMessage(_sampleMessage);
// // 
// //             // Assert
// //             resultMessage.RawMessage.Should().Be(_sampleMessage, "the raw message should match the provided message");
// //         }
// //  // [Error] (157-27)CS0122 'BuildEventArgs.RawMessage' is inaccessible due to its protection level
//         /// <summary>
//         /// Tests that the BuildCheckResultMessage constructor correctly handles an empty message.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithEmptyMessage_SetsRawMessageToEmpty()
//         {
//             // Arrange
//             var emptyMessage = string.Empty;
// 
//             // Act
//             var resultMessage = new BuildCheckResultMessage(emptyMessage);
// 
//             // Assert
//             resultMessage.RawMessage.Should().Be(emptyMessage, "an empty message should be assigned correctly");
//         }
// //  // [Error] (174-27)CS0122 'BuildEventArgs.RawMessage' is inaccessible due to its protection level
// //         /// <summary>
// //         /// Tests the behavior of the BuildCheckResultMessage constructor when null is provided.
// //         /// Note: Although message parameter is non-nullable, this test uses null-forgiving operator to test runtime behavior.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithNullMessage_AllowsNullAssignment()
// //         {
// //             // Arrange
// //             string? nullMessage = null;
// // 
// //             // Act
// //             var resultMessage = new BuildCheckResultMessage(nullMessage!);
// // 
// //             // Assert
// //             resultMessage.RawMessage.Should().BeNull("if null is provided, the raw message should reflect the null value");
// //         }
// //     }
// }