using System;
using FluentAssertions;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Xunit;

namespace Microsoft.Build.Logging.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="BuildEventArgsFields"/> class.
    /// </summary>
    public class BuildEventArgsFieldsTests
    {
        /// <summary>
        /// Tests that the default value of the Importance property is MessageImportance.Low upon initialization.
        /// </summary>
        [Fact]
        public void ImportanceProperty_DefaultValue_IsLow()
        {
            // Arrange & Act
            var buildEventArgsFields = new BuildEventArgsFields();

            // Assert
            buildEventArgsFields.Importance.Should().Be(MessageImportance.Low, 
                "the Importance property should default to MessageImportance.Low when not explicitly set");
        }

        /// <summary>
        /// Tests that after setting all properties, the getters return the same values that were assigned.
        /// This verifies the functionality of auto-implemented property getters and setters.
        /// </summary>
        [Fact]
        public void Properties_SetAndGet_ReturnsSameValues()
        {
            // Arrange
            var expectedFlags = (BuildEventArgsFieldFlags)123; // using a sample numeric value casted to the enum type.
            var expectedMessage = "Test message";
            var expectedArguments = new object[] { 1, "argument", 3.14 };
            var expectedBuildEventContext = new BuildEventContext(1, 2, 3, 4, 5);
            var expectedThreadId = 42;
            var expectedHelpKeyword = "HelpKeywordTest";
            var expectedSenderName = "SenderNameTest";
            var expectedTimestamp = new DateTime(2023, 10, 1, 12, 0, 0);
            var expectedImportance = MessageImportance.High;
            var expectedSubcategory = "SubcategoryTest";
            var expectedCode = "CodeTest";
            var expectedFile = "FileTest.cs";
            var expectedProjectFile = "ProjectFileTest.cs";
            var expectedLineNumber = 10;
            var expectedColumnNumber = 20;
            var expectedEndLineNumber = 30;
            var expectedEndColumnNumber = 40;
            var expectedExtended = new ExtendedDataFields();

            // Act
            var buildEventArgsFields = new BuildEventArgsFields
            {
                Flags = expectedFlags,
                Message = expectedMessage,
                Arguments = expectedArguments,
                BuildEventContext = expectedBuildEventContext,
                ThreadId = expectedThreadId,
                HelpKeyword = expectedHelpKeyword,
                SenderName = expectedSenderName,
                Timestamp = expectedTimestamp,
                Importance = expectedImportance,
                Subcategory = expectedSubcategory,
                Code = expectedCode,
                File = expectedFile,
                ProjectFile = expectedProjectFile,
                LineNumber = expectedLineNumber,
                ColumnNumber = expectedColumnNumber,
                EndLineNumber = expectedEndLineNumber,
                EndColumnNumber = expectedEndColumnNumber,
                Extended = expectedExtended
            };

            // Assert
            buildEventArgsFields.Flags.Should().Be(expectedFlags, "the Flags property should return the value that was set");
            buildEventArgsFields.Message.Should().Be(expectedMessage, "the Message property should return the value that was set");
            buildEventArgsFields.Arguments.Should().BeEquivalentTo(expectedArguments, "the Arguments property should return the array that was set");
            buildEventArgsFields.BuildEventContext.Should().Be(expectedBuildEventContext, "the BuildEventContext property should return the value that was set");
            buildEventArgsFields.ThreadId.Should().Be(expectedThreadId, "the ThreadId property should return the value that was set");
            buildEventArgsFields.HelpKeyword.Should().Be(expectedHelpKeyword, "the HelpKeyword property should return the value that was set");
            buildEventArgsFields.SenderName.Should().Be(expectedSenderName, "the SenderName property should return the value that was set");
            buildEventArgsFields.Timestamp.Should().Be(expectedTimestamp, "the Timestamp property should return the value that was set");
            buildEventArgsFields.Importance.Should().Be(expectedImportance, "the Importance property should return the value that was set");
            buildEventArgsFields.Subcategory.Should().Be(expectedSubcategory, "the Subcategory property should return the value that was set");
            buildEventArgsFields.Code.Should().Be(expectedCode, "the Code property should return the value that was set");
            buildEventArgsFields.File.Should().Be(expectedFile, "the File property should return the value that was set");
            buildEventArgsFields.ProjectFile.Should().Be(expectedProjectFile, "the ProjectFile property should return the value that was set");
            buildEventArgsFields.LineNumber.Should().Be(expectedLineNumber, "the LineNumber property should return the value that was set");
            buildEventArgsFields.ColumnNumber.Should().Be(expectedColumnNumber, "the ColumnNumber property should return the value that was set");
            buildEventArgsFields.EndLineNumber.Should().Be(expectedEndLineNumber, "the EndLineNumber property should return the value that was set");
            buildEventArgsFields.EndColumnNumber.Should().Be(expectedEndColumnNumber, "the EndColumnNumber property should return the value that was set");
            buildEventArgsFields.Extended.Should().Be(expectedExtended, "the Extended property should return the value that was set");
        }
    }
}
