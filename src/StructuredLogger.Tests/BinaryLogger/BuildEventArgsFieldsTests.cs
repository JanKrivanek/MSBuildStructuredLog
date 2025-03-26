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
        private readonly BuildEventArgsFields _instance;

        public BuildEventArgsFieldsTests()
        {
            // Arrange: Create a new instance for each test.
            _instance = new BuildEventArgsFields();
        }

        /// <summary>
        /// Tests that the default value for the Importance property is MessageImportance.Low.
        /// </summary>
        [Fact]
        public void Importance_DefaultValue_ReturnsLow()
        {
            // Act
            MessageImportance actualImportance = _instance.Importance;

            // Assert
            Assert.Equal(MessageImportance.Low, actualImportance);
        }

        /// <summary>
        /// Tests that properties can be set and retrieved correctly.
        /// This covers the happy path where all properties are assigned valid values.
        /// </summary>
        [Fact]
        public void Properties_SetAndGet_ReturnsSameValue()
        {
            // Arrange
            var testFlags = (BuildEventArgsFieldFlags)5;
            var testMessage = "Test Message";
            var testArguments = new object[] { 1, "argument", null };
            BuildEventContext testBuildEventContext = null; // Using null; if instantiation available, it could be replaced.
            int testThreadId = 42;
            var testHelpKeyword = "TestHelpKeyword";
            var testSenderName = "TestSender";
            DateTime testTimestamp = DateTime.UtcNow;
            var testImportance = MessageImportance.High; // Switching value from default for testing.
            var testSubcategory = "TestSubcategory";
            var testCode = "TestCode";
            var testFile = "TestFile";
            var testProjectFile = "TestProjectFile";
            int testLineNumber = 10;
            int testColumnNumber = 20;
            int testEndLineNumber = 30;
            int testEndColumnNumber = 40;
            ExtendedDataFields testExtended = null; // Using null; if a concrete instance is available, it can be provided.

            // Act
            _instance.Flags = testFlags;
            _instance.Message = testMessage;
            _instance.Arguments = testArguments;
            _instance.BuildEventContext = testBuildEventContext;
            _instance.ThreadId = testThreadId;
            _instance.HelpKeyword = testHelpKeyword;
            _instance.SenderName = testSenderName;
            _instance.Timestamp = testTimestamp;
            _instance.Importance = testImportance;
            _instance.Subcategory = testSubcategory;
            _instance.Code = testCode;
            _instance.File = testFile;
            _instance.ProjectFile = testProjectFile;
            _instance.LineNumber = testLineNumber;
            _instance.ColumnNumber = testColumnNumber;
            _instance.EndLineNumber = testEndLineNumber;
            _instance.EndColumnNumber = testEndColumnNumber;
            _instance.Extended = testExtended;

            // Assert
            Assert.Equal(testFlags, _instance.Flags);
            Assert.Equal(testMessage, _instance.Message);
            Assert.Equal(testArguments, _instance.Arguments);
            Assert.Equal(testBuildEventContext, _instance.BuildEventContext);
            Assert.Equal(testThreadId, _instance.ThreadId);
            Assert.Equal(testHelpKeyword, _instance.HelpKeyword);
            Assert.Equal(testSenderName, _instance.SenderName);
            Assert.Equal(testTimestamp, _instance.Timestamp);
            Assert.Equal(testImportance, _instance.Importance);
            Assert.Equal(testSubcategory, _instance.Subcategory);
            Assert.Equal(testCode, _instance.Code);
            Assert.Equal(testFile, _instance.File);
            Assert.Equal(testProjectFile, _instance.ProjectFile);
            Assert.Equal(testLineNumber, _instance.LineNumber);
            Assert.Equal(testColumnNumber, _instance.ColumnNumber);
            Assert.Equal(testEndLineNumber, _instance.EndLineNumber);
            Assert.Equal(testEndColumnNumber, _instance.EndColumnNumber);
            Assert.Equal(testExtended, _instance.Extended);
        }

        /// <summary>
        /// Tests that properties accept null values where applicable.
        /// This covers the edge case for reference type properties.
        /// </summary>
        [Fact]
        public void Properties_SetNullValues_AcceptsNulls()
        {
            // Arrange: For reference type properties, null should be acceptable.
            _instance.Message = null;
            _instance.Arguments = null;
            _instance.BuildEventContext = null;
            _instance.HelpKeyword = null;
            _instance.SenderName = null;
            _instance.Subcategory = null;
            _instance.Code = null;
            _instance.File = null;
            _instance.ProjectFile = null;
            _instance.Extended = null;

            // Act & Assert: Verify that the properties return null.
            Assert.Null(_instance.Message);
            Assert.Null(_instance.Arguments);
            Assert.Null(_instance.BuildEventContext);
            Assert.Null(_instance.HelpKeyword);
            Assert.Null(_instance.SenderName);
            Assert.Null(_instance.Subcategory);
            Assert.Null(_instance.Code);
            Assert.Null(_instance.File);
            Assert.Null(_instance.ProjectFile);
            Assert.Null(_instance.Extended);
        }
    }
}
