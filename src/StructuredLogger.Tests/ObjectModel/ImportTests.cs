using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Import"/> class.
    /// </summary>
    public class ImportTests
    {
        private readonly string _projectFilePath;
        private readonly string _importedProjectFilePath;
        private readonly int _line;
        private readonly int _column;

        public ImportTests()
        {
            _projectFilePath = "Project.csproj";
            _importedProjectFilePath = "ImportedProject.csproj";
            _line = 10;
            _column = 20;
        }

        /// <summary>
        /// Tests that the parameterized constructor correctly initializes properties and the Text property.
        /// </summary>
        [Fact]
        public void Constructor_WithValidParameters_InitializesPropertiesCorrectly()
        {
            // Act
            var import = new Import(_projectFilePath, _importedProjectFilePath, _line, _column);

            // Assert
            import.ProjectFilePath.Should().Be(_projectFilePath);
            import.ImportedProjectFilePath.Should().Be(_importedProjectFilePath);
            import.Line.Should().Be(_line);
            import.Column.Should().Be(_column);
            import.Text.Should().Be(_importedProjectFilePath);
        }

        /// <summary>
        /// Tests that the default constructor initializes properties to their default values.
        /// </summary>
        [Fact]
        public void DefaultConstructor_InitializesWithDefaultValues()
        {
            // Act
            var import = new Import();

            // Assert
            import.ProjectFilePath.Should().BeNull();
            import.ImportedProjectFilePath.Should().BeNull();
            import.Line.Should().Be(0);
            import.Column.Should().Be(0);
            import.Text.Should().BeNull();
        }

        /// <summary>
        /// Tests that the Location property returns the correct formatted string based on Line and Column.
        /// </summary>
        [Fact]
        public void Location_WhenCalled_ReturnsFormattedLocationString()
        {
            // Arrange
            var import = new Import(_projectFilePath, _importedProjectFilePath, _line, _column);

            // Act
            var location = import.Location;

            // Assert
            location.Should().Be($" at ({_line};{_column})");
        }

        /// <summary>
        /// Tests that the TypeName property returns "Import".
        /// </summary>
        [Fact]
        public void TypeName_WhenAccessed_ReturnsImport()
        {
            // Arrange
            var import = new Import();

            // Act
            var typeName = import.TypeName;

            // Assert
            typeName.Should().Be("Import");
        }

        /// <summary>
        /// Tests that the ToString method returns the proper string representation including Text and Location.
        /// </summary>
        [Fact]
        public void ToString_WhenCalled_ReturnsCorrectStringRepresentation()
        {
            // Arrange
            var import = new Import(_projectFilePath, _importedProjectFilePath, _line, _column);

            // Act
            var result = import.ToString();

            // Assert
            result.Should().Be($"Import: {import.Text} at ({_line};{_column})");
        }

        /// <summary>
        /// Tests that the explicit implementation of IPreprocessable.RootFilePath returns ImportedProjectFilePath.
        /// </summary>
        [Fact]
        public void IPreprocessable_RootFilePath_ReturnsImportedProjectFilePath()
        {
            // Arrange
            var import = new Import(_projectFilePath, _importedProjectFilePath, _line, _column);
            var preprocessable = (IPreprocessable)import;

            // Act
            var rootFilePath = preprocessable.RootFilePath;

            // Assert
            rootFilePath.Should().Be(_importedProjectFilePath);
        }

        /// <summary>
        /// Tests that the explicit implementation of IHasSourceFile.SourceFilePath returns ProjectFilePath.
        /// </summary>
        [Fact]
        public void IHasSourceFile_SourceFilePath_ReturnsProjectFilePath()
        {
            // Arrange
            var import = new Import(_projectFilePath, _importedProjectFilePath, _line, _column);
            var hasSourceFile = (IHasSourceFile)import;

            // Act
            var sourceFilePath = hasSourceFile.SourceFilePath;

            // Assert
            sourceFilePath.Should().Be(_projectFilePath);
        }

        /// <summary>
        /// Tests that the explicit implementation of IHasLineNumber.LineNumber returns the correct line value.
        /// </summary>
        [Fact]
        public void IHasLineNumber_LineNumber_ReturnsLineValue()
        {
            // Arrange
            var import = new Import(_projectFilePath, _importedProjectFilePath, _line, _column);
            var hasLineNumber = (IHasLineNumber)import;

            // Act
            var lineNumber = hasLineNumber.LineNumber;

            // Assert
            lineNumber.Should().Be(_line);
        }

        /// <summary>
        /// Tests that setting IsLowRelevance to true reflects in the getter when IsSelected is false.
        /// </summary>
        [Fact]
        public void IsLowRelevance_SetTrueAndDefaultIsSelectedFalse_ReturnsTrue()
        {
            // Arrange
            var import = new Import();

            // Act
            import.IsLowRelevance = true;
            var isLowRelevance = import.IsLowRelevance;

            // Assert
            // Assuming IsSelected is false by default in the base TextNode.
            isLowRelevance.Should().BeTrue();
        }

        /// <summary>
        /// Tests that setting IsLowRelevance to false results in the getter returning false.
        /// </summary>
        [Fact]
        public void IsLowRelevance_SetFalse_ReturnsFalse()
        {
            // Arrange
            var import = new Import();

            // Act
            import.IsLowRelevance = false;
            var isLowRelevance = import.IsLowRelevance;

            // Assert
            isLowRelevance.Should().BeFalse();
        }
    }
}
