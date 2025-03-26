using System;
using System.Collections.Generic;
using StructuredLogger.BinaryLogger;
using Xunit;

namespace StructuredLogger.BinaryLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildCheckTracingEventArgs"/> class.
    /// </summary>
    public class BuildCheckTracingEventArgsTests
    {
        private readonly Dictionary<string, TimeSpan> _sampleTracingData;

        public BuildCheckTracingEventArgsTests()
        {
            _sampleTracingData = new Dictionary<string, TimeSpan>
            {
                { "Step1", TimeSpan.FromSeconds(1) },
                { "Step2", TimeSpan.FromSeconds(2) }
            };
        }

        /// <summary>
        /// Tests that the BuildCheckTracingEventArgs constructor correctly sets the TracingData property when provided a valid dictionary.
        /// </summary>
        [Fact]
        public void Constructor_WithValidDictionary_SetsTracingData()
        {
            // Arrange
            Dictionary<string, TimeSpan> expectedData = new Dictionary<string, TimeSpan>(_sampleTracingData);

            // Act
            var instance = new BuildCheckTracingEventArgs(expectedData);

            // Assert
            Assert.Equal(expectedData, instance.TracingData);
        }

        /// <summary>
        /// Tests that the BuildCheckTracingEventArgs constructor correctly sets the TracingData property when provided null.
        /// </summary>
        [Fact]
        public void Constructor_WithNullDictionary_SetsTracingDataToNull()
        {
            // Arrange
            Dictionary<string, TimeSpan> expectedData = null;

            // Act
            var instance = new BuildCheckTracingEventArgs(expectedData);

            // Assert
            Assert.Null(instance.TracingData);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="BuildCheckAcquisitionEventArgs"/> class.
    /// </summary>
    public class BuildCheckAcquisitionEventArgsTests
    {
        private readonly string _sampleAcquisitionPath;
        private readonly string _sampleProjectPath;

        public BuildCheckAcquisitionEventArgsTests()
        {
            _sampleAcquisitionPath = @"C:\Acquisition\Path";
            _sampleProjectPath = @"C:\Project\Path";
        }

        /// <summary>
        /// Tests that the BuildCheckAcquisitionEventArgs constructor correctly sets AcquisitionPath and ProjectPath properties when provided valid string values.
        /// </summary>
        [Fact]
        public void Constructor_WithValidStrings_SetsProperties()
        {
            // Arrange
            string expectedAcquisitionPath = _sampleAcquisitionPath;
            string expectedProjectPath = _sampleProjectPath;

            // Act
            var instance = new BuildCheckAcquisitionEventArgs(expectedAcquisitionPath, expectedProjectPath);

            // Assert
            Assert.Equal(expectedAcquisitionPath, instance.AcquisitionPath);
            Assert.Equal(expectedProjectPath, instance.ProjectPath);
        }

        /// <summary>
        /// Tests that the BuildCheckAcquisitionEventArgs constructor correctly sets properties to null when null values are provided.
        /// </summary>
        [Fact]
        public void Constructor_WithNullStrings_SetsPropertiesToNull()
        {
            // Arrange
            string expectedAcquisitionPath = null;
            string expectedProjectPath = null;

            // Act
            var instance = new BuildCheckAcquisitionEventArgs(expectedAcquisitionPath, expectedProjectPath);

            // Assert
            Assert.Null(instance.AcquisitionPath);
            Assert.Null(instance.ProjectPath);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="BuildCheckResultMessage"/> class.
    /// </summary>
    public class BuildCheckResultMessageTests
    {
        private readonly string _sampleMessage;

        public BuildCheckResultMessageTests()
        {
            _sampleMessage = "Test message";
        }

        /// <summary>
        /// Tests that the BuildCheckResultMessage constructor correctly sets RawMessage property when provided a non-empty string.
        /// </summary>
//         [Fact] [Error] (133-52)CS0122 'BuildEventArgs.RawMessage' is inaccessible due to its protection level
//         public void Constructor_WithValidMessage_SetsRawMessage()
//         {
//             // Arrange
//             string expectedMessage = _sampleMessage;
// 
//             // Act
//             var instance = new BuildCheckResultMessage(expectedMessage);
// 
//             // Assert
//             Assert.Equal(expectedMessage, instance.RawMessage);
//         }

        /// <summary>
        /// Tests that the BuildCheckResultMessage constructor correctly sets RawMessage property when provided an empty string.
        /// </summary>
//         [Fact] [Error] (149-52)CS0122 'BuildEventArgs.RawMessage' is inaccessible due to its protection level
//         public void Constructor_WithEmptyMessage_SetsRawMessageAsEmpty()
//         {
//             // Arrange
//             string expectedMessage = string.Empty;
// 
//             // Act
//             var instance = new BuildCheckResultMessage(expectedMessage);
// 
//             // Assert
//             Assert.Equal(expectedMessage, instance.RawMessage);
//         }

        /// <summary>
        /// Tests that the BuildCheckResultMessage constructor correctly sets RawMessage property to null when null is provided.
        /// </summary>
//         [Fact] [Error] (165-34)CS0122 'BuildEventArgs.RawMessage' is inaccessible due to its protection level
//         public void Constructor_WithNullMessage_SetsRawMessageToNull()
//         {
//             // Arrange
//             string expectedMessage = null;
// 
//             // Act
//             var instance = new BuildCheckResultMessage(expectedMessage);
// 
//             // Assert
//             Assert.Null(instance.RawMessage);
//         }
    }
}
