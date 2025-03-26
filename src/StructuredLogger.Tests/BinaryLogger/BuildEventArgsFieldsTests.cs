using System;
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
        /// Tests that a new instance of BuildEventArgsFields initializes default values as expected.
        /// Expected: The Importance property is set to MessageImportance.Low, while other value types are zeroed and reference types are null.
        /// </summary>
        [Fact]
        public void Constructor_ShouldInitializeDefaultValues()
        {
            // Arrange & Act
            var instance = new BuildEventArgsFields();

            // Assert
            Assert.Equal(MessageImportance.Low, instance.Importance);
            Assert.Equal(0, instance.ThreadId);
            Assert.Equal(0, instance.LineNumber);
            Assert.Equal(0, instance.ColumnNumber);
            Assert.Equal(0, instance.EndLineNumber);
            Assert.Equal(0, instance.EndColumnNumber);
            Assert.Null(instance.Message);
            Assert.Null(instance.Arguments);
            Assert.Null(instance.BuildEventContext);
            Assert.Null(instance.HelpKeyword);
            Assert.Null(instance.SenderName);
            Assert.Equal(default(DateTime), instance.Timestamp);
            Assert.Null(instance.Subcategory);
            Assert.Null(instance.Code);
            Assert.Null(instance.File);
            Assert.Null(instance.ProjectFile);
            Assert.Null(instance.Extended);
            // Note: The default value for Flags is not explicitly set and depends on its enum definition.
        }

        /// <summary>
        /// Tests that setting and getting all properties returns the values that were assigned.
        /// </summary>
        [Fact]
        public void Properties_SetAndGetProperties_ReturnsSameValue()
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // For Flags, using a cast value to simulate a non-default enum value.
            var expectedFlags = (BuildEventArgsFieldFlags)1;
            var expectedMessage = "Test Message";
            var expectedArguments = new object[] { "argument1", 42 };
            var expectedBuildEventContext = new BuildEventContext(1, 2, 3, 4);
            var expectedThreadId = 100;
            var expectedHelpKeyword = "HelpKeyword";
            var expectedSenderName = "SenderName";
            var expectedTimestamp = new DateTime(2021, 12, 25, 10, 30, 0);
            var expectedImportance = MessageImportance.High;
            var expectedSubcategory = "Subcategory";
            var expectedCode = "Error001";
            var expectedFile = "file.cs";
            var expectedProjectFile = "project.csproj";
            var expectedLineNumber = 10;
            var expectedColumnNumber = 20;
            var expectedEndLineNumber = 30;
            var expectedEndColumnNumber = 40;
            
            // For Extended, assuming a parameterless constructor is available.
            var expectedExtended = new ExtendedDataFields();

            // Act
            instance.Flags = expectedFlags;
            instance.Message = expectedMessage;
            instance.Arguments = expectedArguments;
            instance.BuildEventContext = expectedBuildEventContext;
            instance.ThreadId = expectedThreadId;
            instance.HelpKeyword = expectedHelpKeyword;
            instance.SenderName = expectedSenderName;
            instance.Timestamp = expectedTimestamp;
            instance.Importance = expectedImportance;
            instance.Subcategory = expectedSubcategory;
            instance.Code = expectedCode;
            instance.File = expectedFile;
            instance.ProjectFile = expectedProjectFile;
            instance.LineNumber = expectedLineNumber;
            instance.ColumnNumber = expectedColumnNumber;
            instance.EndLineNumber = expectedEndLineNumber;
            instance.EndColumnNumber = expectedEndColumnNumber;
            instance.Extended = expectedExtended;

            // Assert
            Assert.Equal(expectedFlags, instance.Flags);
            Assert.Equal(expectedMessage, instance.Message);
            Assert.Equal(expectedArguments, instance.Arguments);
            Assert.Equal(expectedBuildEventContext, instance.BuildEventContext);
            Assert.Equal(expectedThreadId, instance.ThreadId);
            Assert.Equal(expectedHelpKeyword, instance.HelpKeyword);
            Assert.Equal(expectedSenderName, instance.SenderName);
            Assert.Equal(expectedTimestamp, instance.Timestamp);
            Assert.Equal(expectedImportance, instance.Importance);
            Assert.Equal(expectedSubcategory, instance.Subcategory);
            Assert.Equal(expectedCode, instance.Code);
            Assert.Equal(expectedFile, instance.File);
            Assert.Equal(expectedProjectFile, instance.ProjectFile);
            Assert.Equal(expectedLineNumber, instance.LineNumber);
            Assert.Equal(expectedColumnNumber, instance.ColumnNumber);
            Assert.Equal(expectedEndLineNumber, instance.EndLineNumber);
            Assert.Equal(expectedEndColumnNumber, instance.EndColumnNumber);
            Assert.Equal(expectedExtended, instance.Extended);
        }

        /// <summary>
        /// Tests that assigning null to reference type properties is handled correctly.
        /// Expected: Properties such as Message, Arguments, HelpKeyword, SenderName, Subcategory, Code, File, ProjectFile, and Extended are null after assignment.
        /// </summary>
        [Fact]
        public void Properties_NullAssignmentForReferenceTypes_ReturnsNull()
        {
            // Arrange
            var instance = new BuildEventArgsFields();

            // Act
            instance.Message = null;
            instance.Arguments = null;
            instance.HelpKeyword = null;
            instance.SenderName = null;
            instance.Subcategory = null;
            instance.Code = null;
            instance.File = null;
            instance.ProjectFile = null;
            instance.Extended = null;

            // Assert
            Assert.Null(instance.Message);
            Assert.Null(instance.Arguments);
            Assert.Null(instance.HelpKeyword);
            Assert.Null(instance.SenderName);
            Assert.Null(instance.Subcategory);
            Assert.Null(instance.Code);
            Assert.Null(instance.File);
            Assert.Null(instance.ProjectFile);
            Assert.Null(instance.Extended);
        }
    }
}
