using System;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Import"/> class.
    /// </summary>
    public class ImportTests
    {
        private readonly string _projectFilePath = "C:\\Project\\MyProject.proj";
        private readonly string _importedProjectFilePath = "C:\\Project\\Imported.proj";
        private readonly int _line = 42;
        private readonly int _column = 7;

        /// <summary>
        /// Tests that the default constructor initializes the Import instance with default values.
        /// Expected: string properties are null, int properties are 0, and derived properties are computed accordingly.
        /// </summary>
        [Fact]
        public void DefaultConstructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var import = new Import();

            // Assert
            Assert.Null(import.ProjectFilePath);
            Assert.Null(import.ImportedProjectFilePath);
            Assert.Equal(0, import.Line);
            Assert.Equal(0, import.Column);
            Assert.Equal(" at (0;0)", import.Location);
            Assert.Equal(nameof(Import), import.TypeName);
            Assert.Equal("Import: " + import.Text + " at (0;0)", import.ToString());
        }

        /// <summary>
        /// Tests that the parameterized constructor correctly assigns given values and sets the Text property.
        /// Expected: properties are set correctly, and ToString returns the expected formatted string.
        /// </summary>
        [Fact]
        public void ParameterizedConstructor_AssignsPropertiesAndTextCorrectly()
        {
            // Arrange & Act
            var import = new Import(_projectFilePath, _importedProjectFilePath, _line, _column);

            // Assert
            Assert.Equal(_projectFilePath, import.ProjectFilePath);
            Assert.Equal(_importedProjectFilePath, import.ImportedProjectFilePath);
            Assert.Equal(_line, import.Line);
            Assert.Equal(_column, import.Column);
            Assert.Equal(" at (42;7)", import.Location);
            Assert.Equal(nameof(Import), import.TypeName);
            Assert.Equal(_importedProjectFilePath, import.Text);
            var expectedToString = $"Import: {import.Text} at (42;7)";
            Assert.Equal(expectedToString, import.ToString());
        }

        /// <summary>
        /// Tests the IsLowRelevance property setter and getter.
        /// Expected: setting the property to true or false reflects appropriately in the getter.
        /// </summary>
        [Fact]
        public void IsLowRelevance_SetAndGet_ReturnsExpectedValue()
        {
            // Arrange
            var import = new Import(_projectFilePath, _importedProjectFilePath, _line, _column);

            // Act & Assert
            // Initially, assume IsLowRelevance is false if no flag has been set.
            Assert.False(import.IsLowRelevance);

            // Set to true and verify.
            import.IsLowRelevance = true;
            Assert.True(import.IsLowRelevance);

            // Set back to false and verify.
            import.IsLowRelevance = false;
            Assert.False(import.IsLowRelevance);
        }

        /// <summary>
        /// Tests the explicit interface implementation of IPreprocessable.
        /// Expected: RootFilePath returns the ImportedProjectFilePath.
        /// </summary>
        [Fact]
        public void IPreprocessable_RootFilePath_ReturnsImportedProjectFilePath()
        {
            // Arrange
            var import = new Import(_projectFilePath, _importedProjectFilePath, _line, _column);
            IPreprocessable preprocessable = import;

            // Act
            var rootFilePath = preprocessable.RootFilePath;

            // Assert
            Assert.Equal(_importedProjectFilePath, rootFilePath);
        }

        /// <summary>
        /// Tests the explicit interface implementation of IHasSourceFile.
        /// Expected: SourceFilePath returns the ProjectFilePath.
        /// </summary>
        [Fact]
        public void IHasSourceFile_SourceFilePath_ReturnsProjectFilePath()
        {
            // Arrange
            var import = new Import(_projectFilePath, _importedProjectFilePath, _line, _column);
            IHasSourceFile hasSourceFile = import;

            // Act
            var sourceFilePath = hasSourceFile.SourceFilePath;

            // Assert
            Assert.Equal(_projectFilePath, sourceFilePath);
        }

        /// <summary>
        /// Tests the explicit interface implementation of IHasLineNumber.
        /// Expected: LineNumber returns the Line property value.
        /// </summary>
        [Fact]
        public void IHasLineNumber_LineNumber_ReturnsLineProperty()
        {
            // Arrange
            var import = new Import(_projectFilePath, _importedProjectFilePath, _line, _column);
            IHasLineNumber hasLineNumber = import;

            // Act
            var lineNumber = hasLineNumber.LineNumber;

            // Assert
            Assert.Equal(_line, lineNumber);
        }
    }
}
