using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="Import"/> class.
    /// </summary>
    public class ImportTests
    {
        /// <summary>
        /// Tests that the default constructor initializes properties to their default values.
        /// Expected outcome: string properties are null and numeric properties are zero.
        /// </summary>
        [Fact]
        public void DefaultConstructor_ShouldInitializeDefaultValues()
        {
            // Arrange & Act
            var import = new Import();

            // Assert
            import.ProjectFilePath.Should().BeNull();
            import.ImportedProjectFilePath.Should().BeNull();
            import.Line.Should().Be(0);
            import.Column.Should().Be(0);
            // Since Text is not explicitly set in the default constructor, it should remain null.
            import.Text.Should().BeNull();
        }

        /// <summary>
        /// Tests that the parameterized constructor sets all properties correctly.
        /// Expected outcome: properties are set to the provided values and Text is set to the value of importedProjectFilePath.
        /// </summary>
        /// <param name="projectFilePath">The project file path.</param>
        /// <param name="importedProjectFilePath">The imported project file path.</param>
        /// <param name="line">The line number.</param>
        /// <param name="column">The column number.</param>
        [Theory]
        [InlineData("C:\\Project\\proj.csproj", "C:\\Imported\\imported.csproj", 10, 20)]
        [InlineData("", "", 0, 0)]
        [InlineData("Special!@#$", "LongPath/with/special_chars", -1, int.MaxValue)]
        public void ParameterizedConstructor_ValidInputs_PropertiesSetCorrectly(string projectFilePath, string importedProjectFilePath, int line, int column)
        {
            // Arrange & Act
            var import = new Import(projectFilePath, importedProjectFilePath, line, column);

            // Assert
            import.ProjectFilePath.Should().Be(projectFilePath);
            import.ImportedProjectFilePath.Should().Be(importedProjectFilePath);
            import.Line.Should().Be(line);
            import.Column.Should().Be(column);
            import.Text.Should().Be(importedProjectFilePath);
        }

        /// <summary>
        /// Tests that the Location property returns the expected format " at (Line;Column)".
        /// Expected outcome: the string format matches the provided Line and Column values.
        /// </summary>
        /// <param name="line">The line number.</param>
        /// <param name="column">The column number.</param>
        /// <param name="expectedLocation">The expected location string.</param>
        [Theory]
        [InlineData(0, 0, " at (0;0)")]
        [InlineData(15, 25, " at (15;25)")]
        [InlineData(int.MinValue, int.MaxValue, " at (-2147483648;2147483647)")]
        public void Location_Getter_ReturnsCorrectFormat(int line, int column, string expectedLocation)
        {
            // Arrange
            var import = new Import("proj", "import", line, column);

            // Act
            var location = import.Location;

            // Assert
            location.Should().Be(expectedLocation);
        }

        /// <summary>
        /// Tests that the TypeName property returns the correct type name.
        /// Expected outcome: TypeName equals "Import".
        /// </summary>
        [Fact]
        public void TypeName_Getter_ReturnsImport()
        {
            // Arrange
            var import = new Import();

            // Act
            var typeName = import.TypeName;

            // Assert
            typeName.Should().Be(nameof(Import));
        }

        /// <summary>
        /// Tests that the ToString method returns a string in the expected format.
        /// Expected outcome: "Import: {Text}{Location}".
        /// </summary>
        /// <param name="projectFilePath">The project file path.</param>
        /// <param name="importedProjectFilePath">The imported project file path.</param>
        /// <param name="line">The line number.</param>
        /// <param name="column">The column number.</param>
        /// <param name="expected">The expected full string returned by ToString.</param>
        [Theory]
        [InlineData("projPath", "importPath", 5, 10, "Import: importPath at (5;10)")]
        [InlineData("A", "B", 0, 0, "Import: B at (0;0)")]
        public void ToString_ReturnsCorrectFormat(string projectFilePath, string importedProjectFilePath, int line, int column, string expected)
        {
            // Arrange
            var import = new Import(projectFilePath, importedProjectFilePath, line, column);

            // Act
            var result = import.ToString();

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Tests the default behavior of the IsLowRelevance property.
        /// Expected outcome: without setting, IsLowRelevance should return false.
        /// </summary>
        [Fact]
        public void IsLowRelevance_Default_ReturnsFalse()
        {
            // Arrange
            var import = new Import();

            // Act
            var isLowRelevance = import.IsLowRelevance;

            // Assert
            isLowRelevance.Should().BeFalse();
        }

        /// <summary>
        /// Tests that setting IsLowRelevance to true results in the getter returning true when IsSelected is false.
        /// Expected outcome: after setting to true, IsLowRelevance returns true.
        /// </summary>
        [Fact]
        public void IsLowRelevance_SetTrue_ReturnsTrue()
        {
            // Arrange
            var import = new Import();

            // Act
            import.IsLowRelevance = true;
            var isLowRelevanceAfterSet = import.IsLowRelevance;

            // Assert
            isLowRelevanceAfterSet.Should().BeTrue();
        }

        /// <summary>
        /// Tests that setting IsLowRelevance to false results in the getter returning false.
        /// Expected outcome: after setting to false, IsLowRelevance returns false.
        /// </summary>
        [Fact]
        public void IsLowRelevance_SetFalse_ReturnsFalse()
        {
            // Arrange
            var import = new Import();
            import.IsLowRelevance = true; // First set to true.

            // Act
            import.IsLowRelevance = false;
            var isLowRelevanceAfterSet = import.IsLowRelevance;

            // Assert
            isLowRelevanceAfterSet.Should().BeFalse();
        }
    }
}
