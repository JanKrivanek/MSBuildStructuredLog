using FluentAssertions;
using System;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="AbstractDiagnostic"/> class.
    /// </summary>
    public class AbstractDiagnosticTests
    {
        /// <summary>
        /// Tests that the TypeName property returns the expected value "AbstractDiagnostic".
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsCorrectValue()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic() { Text = "Sample text" };

            // Act
            var typeName = diagnostic.TypeName;

            // Assert
            typeName.Should().Be("AbstractDiagnostic");
        }

        /// <summary>
        /// Tests that the Title property returns the same value as ToString().
        /// </summary>
        [Fact]
        public void Title_WhenCalled_ReturnsSameAsToString()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic()
            {
                Text = "Title test",
                File = "test.cs",
                LineNumber = 10,
                ColumnNumber = 2,
                Code = "TST001"
            };

            // Act
            var title = diagnostic.Title;
            var toStringValue = diagnostic.ToString();

            // Assert
            title.Should().Be(toStringValue);
        }

        /// <summary>
        /// Tests that the IHasSourceFile.SourceFilePath property correctly returns the File property value.
        /// </summary>
        [Fact]
        public void SourceFilePath_WhenFileIsSet_ReturnsSameValueAsFile()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic() { Text = "Test", File = "sourcefile.cs" };

            // Act
            var sourceFilePath = ((IHasSourceFile)diagnostic).SourceFilePath;

            // Assert
            sourceFilePath.Should().Be("sourcefile.cs");
        }

        /// <summary>
        /// Tests the ToString method for default values when no File, line/column or Code is provided.
        /// </summary>
        [Fact]
        public void ToString_WhenNoLineOrColumnCodeAndFileNotSet_ReturnsTextOnly()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic() { Text = "No additional info" };

            // Act
            var result = diagnostic.ToString();

            // Assert
            // Since File, position and code parts are empty, the Text is returned as-is.
            result.Should().Be("No additional info");
        }

        /// <summary>
        /// Tests the ToString method when all properties are provided.
        /// </summary>
        [Fact]
        public void ToString_WhenAllPropertiesSet_ReturnsFormattedDiagnosticMessage()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic()
            {
                Text = "Error occurred",
                File = "file.cs",
                LineNumber = 10,
                ColumnNumber = 5,
                Code = "CS1234",
                ProjectFile = "proj.csproj"
            };

            // Act
            var result = diagnostic.ToString();

            // Assert
            // Expected format: "file.cs(10,5): abstractdiagnostic CS1234: Error occurred [proj.csproj]"
            result.Should().Be("file.cs(10,5): abstractdiagnostic CS1234: Error occurred [proj.csproj]");
        }

        /// <summary>
        /// Tests the ToString method when File and Code are set without line and column numbers.
        /// </summary>
        [Fact]
        public void ToString_WhenFileAndCodeSetWithoutLineNumbers_ReturnsFormattedMessage()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic()
            {
                Text = "Warning",
                File = "file.txt",
                Code = "W001",
                LineNumber = 0,
                ColumnNumber = 0,
                ProjectFile = null
            };

            // Act
            var result = diagnostic.ToString();

            // Assert
            // Expected format: "file.txt abstractdiagnostic W001: Warning"
            result.Should().Be("file.txt abstractdiagnostic W001: Warning");
        }

        /// <summary>
        /// Tests the ToString method when only LineNumber is provided (and ColumnNumber is zero) along with other properties.
        /// </summary>
        [Fact]
        public void ToString_WhenOnlyLineNumberSet_ReturnsFormattedMessage()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic()
            {
                Text = "Info",
                File = "file.txt",
                Code = "X100",
                LineNumber = 12,
                ColumnNumber = 0
            };

            // Act
            var result = diagnostic.ToString();

            // Assert
            // Expected format: "file.txt(12): abstractdiagnostic X100: Info"
            result.Should().Be("file.txt(12): abstractdiagnostic X100: Info");
        }

        /// <summary>
        /// Tests get and set operations for all simple properties.
        /// </summary>
        [Fact]
        public void Properties_GetSetOperations_WorkAsExpected()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic();
            var now = DateTime.Now;

            // Act
            diagnostic.Timestamp = now;
            diagnostic.Code = "CODE";
            diagnostic.ColumnNumber = 3;
            diagnostic.EndColumnNumber = 8;
            diagnostic.EndLineNumber = 15;
            diagnostic.File = "abc.cs";
            diagnostic.LineNumber = 7;
            diagnostic.ProjectFile = "project.csproj";
            diagnostic.Subcategory = "subcategory";
            diagnostic.Text = "Property test";

            // Assert
            diagnostic.Timestamp.Should().Be(now);
            diagnostic.Code.Should().Be("CODE");
            diagnostic.ColumnNumber.Should().Be(3);
            diagnostic.EndColumnNumber.Should().Be(8);
            diagnostic.EndLineNumber.Should().Be(15);
            diagnostic.File.Should().Be("abc.cs");
            diagnostic.LineNumber.Should().Be(7);
            diagnostic.ProjectFile.Should().Be("project.csproj");
            diagnostic.Subcategory.Should().Be("subcategory");
            diagnostic.Text.Should().Be("Property test");
        }
    }
}
