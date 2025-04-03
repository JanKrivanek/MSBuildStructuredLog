using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="AbstractDiagnostic"/> class.
    /// </summary>
    public class AbstractDiagnosticTests
    {
        /// <summary>
        /// Tests the ToString() method when File is null, expecting File to become empty and no additional formatting.
        /// </summary>
        [Fact]
        public void ToString_FileIsNullAndNoLineOrColumnAndNoCodeAndNoProjectFile_ReturnsUnmodifiedText()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                // File is left as null to test null coalescing to ""
                Text = "Test diagnostic message",
                LineNumber = 0,
                ColumnNumber = 0,
                Code = null,
                ProjectFile = null
            };
            // Expected: File becomes "" and since no File, position and code are empty, the condition (0 > 0) is false, so text remains unmodified.
            var expected = "Test diagnostic message";

            // Act
            string result = diagnostic.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the ToString() method when File, LineNumber, ColumnNumber, Code and ProjectFile are provided.
        /// </summary>
        [Fact]
        public void ToString_AllPropertiesSetWithLineAndColumn_ReturnsFormattedString()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                File = "file.cs",
                LineNumber = 10,
                ColumnNumber = 5,
                Code = "ABC",
                Text = "Diagnostic message",
                ProjectFile = "project.csproj"
            };

            // Expected breakdown:
            // File: "file.cs"
            // Position: (LineNumber != 0 || ColumnNumber != 0) => ColumnNumber > 0 so column = ",5" => position = "(10,5):"
            // Code: not white space => " abstractdiagnostic ABC:" (this.GetType().Name.ToLowerInvariant() yields "abstractdiagnostic")
            // Because File.Length + position.Length + code.Length > 0, Text becomes " " + Text => " Diagnostic message"
            // ProjectFile: not white space => " [project.csproj]"
            // Combined expected string:
            var expected = "file.cs(10,5): abstractdiagnostic ABC: Diagnostic message [project.csproj]";

            // Act
            string result = diagnostic.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the ToString() method when LineNumber and ColumnNumber are zero but File and Code are provided.
        /// </summary>
        [Fact]
        public void ToString_FileProvidedButNoLineOrColumnWithCode_ReturnsFormattedStringWithoutPosition()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                File = "file.cs",
                LineNumber = 0,
                ColumnNumber = 0,
                Code = "XYZ",
                Text = "Diagnostic text",
                ProjectFile = "" // empty project file
            };

            // Expected:
            // File: "file.cs"
            // Position: remains "" because both LineNumber and ColumnNumber are 0.
            // Code: " abstractdiagnostic XYZ:" as Code is provided.
            // Since File.Length + position.Length + code.Length > 0, text becomes " " + Text => " Diagnostic text"
            // ProjectFile: empty so nothing appended.
            var expected = "file.cs abstractdiagnostic XYZ: Diagnostic text";

            // Act
            string result = diagnostic.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the ToString() method when ColumnNumber is non-positive (negative) while LineNumber is set.
        /// </summary>
        [Fact]
        public void ToString_NegativeColumnNumberWithLineNumber_ReturnsPositionWithoutColumn()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                File = "file.cs",
                LineNumber = 10,
                ColumnNumber = -1, // non-positive so column should not be appended
                Code = "NEG",
                Text = "Edge case test",
                ProjectFile = null
            };

            // Expected:
            // File: "file.cs"
            // Position: since LineNumber != 0, we enter if block, but ColumnNumber <= 0 so column remains "", position becomes "(10):"
            // Code: " abstractdiagnostic NEG:"
            // Text: becomes " " + Text => " Edge case test"
            // ProjectFile: null so remains ""
            var expected = "file.cs(10): abstractdiagnostic NEG: Edge case test";

            // Act
            string result = diagnostic.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the Title property to ensure it returns the same value as ToString().
        /// </summary>
        [Fact]
        public void Title_ReturnsSameValueAsToString()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                File = "file.cs",
                LineNumber = 1,
                ColumnNumber = 2,
                Code = "TST",
                Text = "Title test",
                ProjectFile = "proj.csproj"
            };

            // Act
            string toStringResult = diagnostic.ToString();
            string titleResult = diagnostic.Title;

            // Assert
            titleResult.Should().Be(toStringResult);
        }

        /// <summary>
        /// Tests the TypeName property to ensure it returns the correct type name.
        /// </summary>
        [Fact]
        public void TypeName_ReturnsAbstractDiagnostic()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic();

            // Act
            string typeName = diagnostic.TypeName;

            // Assert
            typeName.Should().Be("AbstractDiagnostic");
        }
    }
}
