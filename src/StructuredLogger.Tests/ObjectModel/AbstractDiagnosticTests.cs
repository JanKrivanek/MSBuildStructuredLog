using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="AbstractDiagnostic"/> class.
    /// </summary>
    public class AbstractDiagnosticTests
    {
        /// <summary>
        /// Tests that when no File, LineNumber, ColumnNumber, Code, or ProjectFile is provided,
        /// the ToString method returns the Text value unchanged.
        /// </summary>
        [Fact]
        public void ToString_NoFileNoPositionsNoCodeNoProject_ReturnsTextUnchanged()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                // Assuming the inherited Text property is public.
                // If not, adjust accordingly.
                Text = "sample message",
                File = null,
                LineNumber = 0,
                ColumnNumber = 0,
                Code = null,
                ProjectFile = null
            };

            // Act
            string result = diagnostic.ToString();

            // Assert
            Assert.Equal("sample message", result);
        }

        /// <summary>
        /// Tests that when File is provided but no position, Code, or ProjectFile,
        /// the ToString method returns File concatenated with the Text (with a prepended space).
        /// </summary>
        [Fact]
        public void ToString_WithFileNoPosition_NoCode_ReturnsFileAndText()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                File = "file.cs",
                Text = "test",
                LineNumber = 0,
                ColumnNumber = 0,
                Code = null,
                ProjectFile = null
            };

            // Act
            string result = diagnostic.ToString();

            // Assert
            Assert.Equal("file.cs test", result);
        }

        /// <summary>
        /// Tests that when a LineNumber is provided (and ColumnNumber is zero),
        /// the ToString method returns the File with the line position and the Text.
        /// </summary>
        [Fact]
        public void ToString_WithFileLineNoColumn_NoCode_ReturnsFileLineAndText()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                File = "file.cs",
                Text = "test",
                LineNumber = 10,
                ColumnNumber = 0,
                Code = null,
                ProjectFile = null
            };
            // Expected position: (10):
            // Since File is non-empty, a space will be prepended to the text.
            string expected = "file.cs(10): test";

            // Act
            string result = diagnostic.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that when both a LineNumber and a positive ColumnNumber are provided,
        /// the ToString method returns the File with the full position and the Text.
        /// </summary>
        [Fact]
        public void ToString_WithFileLineAndPositiveColumn_NoCode_ReturnsFileLineColumnAndText()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                File = "file.cs",
                Text = "test",
                LineNumber = 10,
                ColumnNumber = 5,
                Code = null,
                ProjectFile = null
            };
            // Expected position: (10,5):
            string expected = "file.cs(10,5): test";

            // Act
            string result = diagnostic.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that when Code is provided along with File, LineNumber, and ColumnNumber,
        /// the ToString method includes the code section in the output.
        /// </summary>
        [Fact]
        public void ToString_WithCode_AddsCodeSection()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                File = "file.cs",
                Text = "error",
                LineNumber = 10,
                ColumnNumber = 5,
                Code = "X001",
                ProjectFile = null
            };
            // Expected:
            // File: "file.cs"
            // Position: "(10,5):"
            // Code section: " abstractdiagnostic X001:" (using GetType().Name.ToLowerInvariant())
            // Text: " error"
            string expected = "file.cs(10,5): abstractdiagnostic X001: error";

            // Act
            string result = diagnostic.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that when a ProjectFile is provided along with other properties,
        /// the ToString method appends the project file in square brackets.
        /// </summary>
        [Fact]
        public void ToString_WithProjectFile_AppendsProjectFile()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                File = "file.cs",
                Text = "error",
                LineNumber = 10,
                ColumnNumber = 5,
                Code = "X001",
                ProjectFile = "proj.csproj"
            };
            // Expected output with project file appended.
            string expected = "file.cs(10,5): abstractdiagnostic X001: error [proj.csproj]";

            // Act
            string result = diagnostic.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that when ColumnNumber is negative while LineNumber is positive,
        /// the negative ColumnNumber is ignored in the formatting.
        /// </summary>
        [Fact]
        public void ToString_NegativeColumn_TreatsAsNoColumn()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                File = "file.cs",
                Text = "test",
                LineNumber = 10,
                ColumnNumber = -5,
                Code = null,
                ProjectFile = null
            };
            // Expected: ColumnNumber is not appended since it's not greater than 0.
            string expected = "file.cs(10): test";

            // Act
            string result = diagnostic.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that the Title property returns the same value as ToString.
        /// </summary>
        [Fact]
        public void Title_ReturnsSameValueAsToString()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                File = "file.cs",
                Text = "message",
                LineNumber = 10,
                ColumnNumber = 5,
                Code = "X002",
                ProjectFile = "proj.csproj"
            };

            // Act
            string title = diagnostic.Title;
            string toStringResult = diagnostic.ToString();

            // Assert
            Assert.Equal(toStringResult, title);
        }

        /// <summary>
        /// Tests that the TypeName property returns the expected value.
        /// </summary>
        [Fact]
        public void TypeName_ReturnsAbstractDiagnostic()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic();

            // Act
            string typeName = diagnostic.TypeName;

            // Assert
            Assert.Equal("AbstractDiagnostic", typeName);
        }

        /// <summary>
        /// Tests that the IHasSourceFile.SourceFilePath property returns the same as the File property.
        /// </summary>
        [Fact]
        public void IHasSourceFile_SourceFilePath_ReturnsFile()
        {
            // Arrange
            var diagnostic = new AbstractDiagnostic
            {
                File = "sourcefile.cs"
            };

            // Act
            var sourceFilePath = ((IHasSourceFile)diagnostic).SourceFilePath;

            // Assert
            Assert.Equal("sourcefile.cs", sourceFilePath);
        }
    }
}
