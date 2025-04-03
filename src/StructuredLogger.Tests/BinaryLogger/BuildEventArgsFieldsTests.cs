using FluentAssertions;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using System;
using Xunit;

namespace Microsoft.Build.Logging.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildEventArgsFields"/> class.
    /// </summary>
    public class BuildEventArgsFieldsTests
    {
        private readonly BuildEventArgsFields _eventArgsFields;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildEventArgsFieldsTests"/> class.
        /// </summary>
        public BuildEventArgsFieldsTests()
        {
            _eventArgsFields = new BuildEventArgsFields();
        }

        /// <summary>
        /// Tests that the default value for the Importance property is MessageImportance.Low.
        /// </summary>
        [Fact]
        public void Importance_DefaultValue_ShouldBeLow()
        {
            // Arrange & Act done in the constructor

            // Assert
            _eventArgsFields.Importance.Should().Be(MessageImportance.Low);
        }

        /// <summary>
        /// Tests that the Flags property can be set and retrieved correctly with various enum values.
        /// </summary>
        /// <param name="flagsNumber">An integer value to cast to BuildEventArgsFieldFlags.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void Flags_SetAndGet_ShouldReturnSameValue(int flagsNumber)
        {
            // Arrange
            var expected = (BuildEventArgsFieldFlags)flagsNumber;
            var instance = new BuildEventArgsFields();

            // Act
            instance.Flags = expected;

            // Assert
            instance.Flags.Should().Be(expected);
        }
//  // [Error] (65-21)CS0182 An attribute argument must be a constant expression, typeof expression or array creation expression of an attribute parameter type
//         /// <summary>
//         /// Tests that the Message property can be set and retrieved correctly for various string inputs.
//         /// </summary>
//         /// <param name="testMessage">The test string value for Message.</param>
//         [Theory]
//         [InlineData("Test message")]
//         [InlineData("")]
//         [InlineData("Special characters !@#$%^&*()")]
//         [InlineData("A very long message " + "x".PadLeft(1000, 'x'))]
//         public void Message_SetAndGet_ShouldReturnSameValue(string testMessage)
//         {
//             // Arrange
//             var instance = new BuildEventArgsFields();
// 
//             // Act
//             instance.Message = testMessage;
// 
//             // Assert
//             instance.Message.Should().Be(testMessage);
//         }
// 
        /// <summary>
        /// Tests that the Arguments property can be set and retrieved correctly when assigned an empty array.
        /// </summary>
        [Fact]
        public void Arguments_SetAndGet_WithEmptyArray_ShouldReturnSameArray()
        {
            // Arrange
            var instance = new BuildEventArgsFields();
            object[] expected = Array.Empty<object>();

            // Act
            instance.Arguments = expected;

            // Assert
            instance.Arguments.Should().BeEquivalentTo(expected);
        }

        /// <summary>
        /// Tests that the Arguments property can be set and retrieved correctly when assigned a non-empty array.
        /// </summary>
        [Fact]
        public void Arguments_SetAndGet_WithNonEmptyArray_ShouldReturnSameArray()
        {
            // Arrange
            var instance = new BuildEventArgsFields();
            object[] expected = new object[] { 1, "two", 3.0 };

            // Act
            instance.Arguments = expected;

            // Assert
            instance.Arguments.Should().BeEquivalentTo(expected);
        }

        /// <summary>
        /// Tests that the BuildEventContext property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void BuildEventContext_SetAndGet_ShouldReturnSameInstance()
        {
            // Arrange
            var instance = new BuildEventArgsFields();
            // Assuming BuildEventContext constructor signature: BuildEventContext(int nodeId, int projectContextId, int taskId, int submissionId)
            var expected = new BuildEventContext(1, 2, 3, 4);

            // Act
            instance.BuildEventContext = expected;

            // Assert
            instance.BuildEventContext.Should().Be(expected);
        }

        /// <summary>
        /// Tests that the ThreadId property can be set and retrieved correctly for various integer values.
        /// </summary>
        /// <param name="threadId">The integer value for ThreadId.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void ThreadId_SetAndGet_ShouldReturnSameValue(int threadId)
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // Act
            instance.ThreadId = threadId;

            // Assert
            instance.ThreadId.Should().Be(threadId);
        }

        /// <summary>
        /// Tests that the HelpKeyword property can be set and retrieved correctly for various string inputs.
        /// </summary>
        /// <param name="keyword">The test string value for HelpKeyword.</param>
        [Theory]
        [InlineData("HelpKey")]
        [InlineData("")]
        [InlineData("Special_Keyword-123!")]
        public void HelpKeyword_SetAndGet_ShouldReturnSameValue(string keyword)
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // Act
            instance.HelpKeyword = keyword;

            // Assert
            instance.HelpKeyword.Should().Be(keyword);
        }

        /// <summary>
        /// Tests that the SenderName property can be set and retrieved correctly for various string inputs.
        /// </summary>
        /// <param name="senderName">The test string value for SenderName.</param>
        [Theory]
        [InlineData("Sender1")]
        [InlineData("")]
        [InlineData("Sender_Name_With_Special_Chars!@#")]
        public void SenderName_SetAndGet_ShouldReturnSameValue(string senderName)
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // Act
            instance.SenderName = senderName;

            // Assert
            instance.SenderName.Should().Be(senderName);
        }

        /// <summary>
        /// Tests that the Timestamp property can be set and retrieved correctly for boundary DateTime values.
        /// </summary>
        [Fact]
        public void Timestamp_SetAndGet_WithBoundaryValues_ShouldReturnSameValue()
        {
            // Arrange
            var instance = new BuildEventArgsFields();
            DateTime minTime = DateTime.MinValue;
            DateTime maxTime = DateTime.MaxValue;

            // Act & Assert for DateTime.MinValue
            instance.Timestamp = minTime;
            instance.Timestamp.Should().Be(minTime);

            // Act & Assert for DateTime.MaxValue
            instance.Timestamp = maxTime;
            instance.Timestamp.Should().Be(maxTime);
        }

        /// <summary>
        /// Tests that the Importance property can be set and retrieved correctly when assigned a value different from the default.
        /// </summary>
        [Fact]
        public void Importance_SetAndGet_NonDefaultValue_ShouldReturnSameValue()
        {
            // Arrange
            var instance = new BuildEventArgsFields();
            // Assuming MessageImportance has a Normal value in addition to Low.
            var expected = MessageImportance.Normal;

            // Act
            instance.Importance = expected;

            // Assert
            instance.Importance.Should().Be(expected);
        }

        /// <summary>
        /// Tests that the Subcategory property can be set and retrieved correctly for various string inputs.
        /// </summary>
        /// <param name="subcategory">The test string value for Subcategory.</param>
        [Theory]
        [InlineData("Subcat1")]
        [InlineData("")]
        [InlineData("Sub-category with spaces and - symbols")]
        public void Subcategory_SetAndGet_ShouldReturnSameValue(string subcategory)
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // Act
            instance.Subcategory = subcategory;

            // Assert
            instance.Subcategory.Should().Be(subcategory);
        }

        /// <summary>
        /// Tests that the Code property can be set and retrieved correctly for various string inputs.
        /// </summary>
        /// <param name="code">The test string value for Code.</param>
        [Theory]
        [InlineData("ERR001")]
        [InlineData("")]
        [InlineData("Code_with_special_chars_#$%")]
        public void Code_SetAndGet_ShouldReturnSameValue(string code)
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // Act
            instance.Code = code;

            // Assert
            instance.Code.Should().Be(code);
        }

        /// <summary>
        /// Tests that the File property can be set and retrieved correctly for various string inputs.
        /// </summary>
        /// <param name="filePath">The test string value for File.</param>
        [Theory]
        [InlineData("C:\\temp\\file.txt")]
        [InlineData("/usr/local/file.txt")]
        [InlineData("")]
        [InlineData("RelativePath\\file.txt")]
        public void File_SetAndGet_ShouldReturnSameValue(string filePath)
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // Act
            instance.File = filePath;

            // Assert
            instance.File.Should().Be(filePath);
        }

        /// <summary>
        /// Tests that the ProjectFile property can be set and retrieved correctly for various string inputs.
        /// </summary>
        /// <param name="projectFile">The test string value for ProjectFile.</param>
        [Theory]
        [InlineData("C:\\projects\\myproject.csproj")]
        [InlineData("/home/user/myproject.csproj")]
        [InlineData("")]
        public void ProjectFile_SetAndGet_ShouldReturnSameValue(string projectFile)
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // Act
            instance.ProjectFile = projectFile;

            // Assert
            instance.ProjectFile.Should().Be(projectFile);
        }

        /// <summary>
        /// Tests that the LineNumber property can be set and retrieved correctly for various integer values.
        /// </summary>
        /// <param name="lineNumber">The integer value for LineNumber.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void LineNumber_SetAndGet_ShouldReturnSameValue(int lineNumber)
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // Act
            instance.LineNumber = lineNumber;

            // Assert
            instance.LineNumber.Should().Be(lineNumber);
        }

        /// <summary>
        /// Tests that the ColumnNumber property can be set and retrieved correctly for various integer values.
        /// </summary>
        /// <param name="columnNumber">The integer value for ColumnNumber.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void ColumnNumber_SetAndGet_ShouldReturnSameValue(int columnNumber)
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // Act
            instance.ColumnNumber = columnNumber;

            // Assert
            instance.ColumnNumber.Should().Be(columnNumber);
        }

        /// <summary>
        /// Tests that the EndLineNumber property can be set and retrieved correctly for various integer values.
        /// </summary>
        /// <param name="endLineNumber">The integer value for EndLineNumber.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void EndLineNumber_SetAndGet_ShouldReturnSameValue(int endLineNumber)
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // Act
            instance.EndLineNumber = endLineNumber;

            // Assert
            instance.EndLineNumber.Should().Be(endLineNumber);
        }

        /// <summary>
        /// Tests that the EndColumnNumber property can be set and retrieved correctly for various integer values.
        /// </summary>
        /// <param name="endColumnNumber">The integer value for EndColumnNumber.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void EndColumnNumber_SetAndGet_ShouldReturnSameValue(int endColumnNumber)
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // Act
            instance.EndColumnNumber = endColumnNumber;

            // Assert
            instance.EndColumnNumber.Should().Be(endColumnNumber);
        }

        /// <summary>
        /// Tests that the Extended property can be set and retrieved correctly.
        /// </summary>
        [Fact]
        public void Extended_SetAndGet_ShouldReturnSameInstance()
        {
            // Arrange
            var instance = new BuildEventArgsFields();
            var expected = new ExtendedDataFields();

            // Act
            instance.Extended = expected;

            // Assert
            instance.Extended.Should().Be(expected);
        }
    }
}
