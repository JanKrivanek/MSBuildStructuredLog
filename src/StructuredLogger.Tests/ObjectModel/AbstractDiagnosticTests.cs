using System;
using Moq;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="AbstractDiagnostic"/> class.
    /// </summary>
    public class AbstractDiagnosticTests
    {
        private readonly AbstractDiagnostic _diagnostic;

        public AbstractDiagnosticTests()
        {
            // Initialize a new instance of AbstractDiagnostic.
            _diagnostic = new AbstractDiagnostic();
        }

        /// <summary>
        /// Tests that ToString returns the text property value as-is when no other related properties are set.
        /// Expected outcome: The method should return just the Text value.
        /// </summary>
        [Fact]
        public void ToString_NoPropertiesSet_ReturnsTextAsIs()
        {
            // Arrange
            _diagnostic.Text = "Test message";
            _diagnostic.File = null; // Should be replaced with empty string.
            _diagnostic.LineNumber = 0;
            _diagnostic.ColumnNumber = 0;
            _diagnostic.Code = null;
            _diagnostic.ProjectFile = null;

            // Act
            string result = _diagnostic.ToString();

            // Assert
            Assert.Equal("Test message", result);
        }

        /// <summary>
        /// Tests that ToString prepends a space to the Text when the File property is set.
        /// Expected outcome: The output should start with the File value followed by a space and the Text.
        /// </summary>
        [Fact]
        public void ToString_FileSet_ReturnsFileWithPrependedText()
        {
            // Arrange
            _diagnostic.Text = "message";
            _diagnostic.File = "file.cs";
            _diagnostic.LineNumber = 0;
            _diagnostic.ColumnNumber = 0;
            _diagnostic.Code = null;
            _diagnostic.ProjectFile = null;

            // Act
            string result = _diagnostic.ToString();

            // Assert
            string expected = "file.cs" + " " + "message";
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that ToString correctly formats the position when LineNumber and ColumnNumber are provided.
        /// Expected outcome: The formatted position should appear as "(LineNumber,ColumnNumber):" if ColumnNumber is positive.
        /// </summary>
        [Fact]
        public void ToString_LineAndColumnSet_ReturnsPositionFormattedCorrectly()
        {
            // Arrange
            _diagnostic.Text = "msg";
            _diagnostic.File = null;
            _diagnostic.LineNumber = 10;
            _diagnostic.ColumnNumber = 5;
            _diagnostic.Code = null;
            _diagnostic.ProjectFile = null;

            // Act
            string result = _diagnostic.ToString();

            // Assert
            string expected = "(10,5):" + " " + "msg";
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that ToString correctly formats the code section when the Code property is provided.
        /// Expected outcome: The output should include a lower-cased type name followed by the Code and a colon.
        /// </summary>
        [Fact]
        public void ToString_CodeSet_ReturnsCodeFormattedCorrectly()
        {
            // Arrange
            _diagnostic.Text = "error message";
            _diagnostic.File = "foo.cs";
            _diagnostic.Code = "ABC";
            _diagnostic.LineNumber = 0;
            _diagnostic.ColumnNumber = 0;
            _diagnostic.ProjectFile = null;

            // Act
            string result = _diagnostic.ToString();

            // Assert
            // Expected format: File + (empty position) + " abstractdiagnostic ABC:" + space + Text.
            string expected = "foo.cs" + " " + "abstractdiagnostic ABC:" + " error message";
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that ToString appends the project file information when the ProjectFile property is set.
        /// Expected outcome: The output should end with the project file enclosed in square brackets.
        /// </summary>
        [Fact]
        public void ToString_ProjectFileSet_ReturnsProjectFileAppended()
        {
            // Arrange
            _diagnostic.Text = "error occurred";
            _diagnostic.File = "a.cs";
            _diagnostic.Code = "XYZ";
            _diagnostic.LineNumber = 0;
            _diagnostic.ColumnNumber = 0;
            _diagnostic.ProjectFile = "project.csproj";

            // Act
            string result = _diagnostic.ToString();

            // Assert
            string expected = "a.cs" + " " + "abstractdiagnostic XYZ:" + " error occurred" + " [project.csproj]";
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that the Title property returns the same result as the ToString method.
        /// Expected outcome: Title and ToString() results are identical.
        /// </summary>
        [Fact]
        public void Title_ReturnsSameAsToString()
        {
            // Arrange
            _diagnostic.Text = "title test";
            _diagnostic.File = "sample.cs";
            _diagnostic.LineNumber = 15;
            _diagnostic.ColumnNumber = 3;
            _diagnostic.Code = "ERR";
            _diagnostic.ProjectFile = "proj.csproj";
            string expected = _diagnostic.ToString();

            // Act
            string title = _diagnostic.Title;

            // Assert
            Assert.Equal(expected, title);
        }

        /// <summary>
        /// Tests that the TypeName property returns the name of the class.
        /// Expected outcome: The TypeName should be "AbstractDiagnostic".
        /// </summary>
        [Fact]
        public void TypeName_ReturnsClassName()
        {
            // Act
            string typeName = _diagnostic.TypeName;

            // Assert
            Assert.Equal("AbstractDiagnostic", typeName);
        }

        /// <summary>
        /// Tests that the IHasSourceFile.SourceFilePath property returns the same value as the File property.
        /// Expected outcome: SourceFilePath should equal the File property value.
        /// </summary>
        [Fact]
        public void IHasSourceFile_SourceFilePath_ReturnsFileProperty()
        {
            // Arrange
            _diagnostic.File = "sourcefile.cs";

            // Act
            string sourceFilePath = ((_diagnostic) as IHasSourceFile).SourceFilePath;

            // Assert
            Assert.Equal("sourcefile.cs", sourceFilePath);
        }

        /// <summary>
        /// Tests that ToString handles a negative ColumnNumber by not appending column details.
        /// Expected outcome: When ColumnNumber is negative, the position should only reflect the LineNumber.
        /// </summary>
        [Fact]
        public void ToString_NegativeColumnNumber_FormatsWithoutColumnDetail()
        {
            // Arrange
            _diagnostic.Text = "negative column";
            _diagnostic.File = null;
            _diagnostic.LineNumber = 5;
            _diagnostic.ColumnNumber = -1; // Negative value; should not include column details.
            _diagnostic.Code = null;
            _diagnostic.ProjectFile = null;

            // Act
            string result = _diagnostic.ToString();

            // Assert
            // Expected: Position should be formatted as "(5):" then space and the text.
            string expected = "(5):" + " " + "negative column";
            Assert.Equal(expected, result);
        }
    }
}
