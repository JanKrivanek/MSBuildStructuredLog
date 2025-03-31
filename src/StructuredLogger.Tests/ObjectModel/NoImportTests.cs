using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using System;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="NoImport"/> class.
    /// </summary>
    public class NoImportTests
    {
        private readonly string _testProjectFilePath;
        private readonly string _testImportedFileSpec;
        private readonly string _testReason;
        private readonly int _testLine;
        private readonly int _testColumn;

        public NoImportTests()
        {
            _testProjectFilePath = "TestProject.csproj";
            _testImportedFileSpec = "ImportedFile.targets";
            _testReason = "File not found";
            _testLine = 10;
            _testColumn = 5;
        }

        /// <summary>
        /// Tests that the default constructor initializes properties to their default values.
        /// </summary>
        [Fact]
        public void DefaultConstructor_ShouldInitializePropertiesToDefaultValues()
        {
            // Arrange & Act
            var noImport = new NoImport();

            // Assert
            noImport.ProjectFilePath.Should().BeNull();
            noImport.ImportedFileSpec.Should().BeNull();
            noImport.Reason.Should().BeNull();
            noImport.Line.Should().Be(0);
            noImport.Column.Should().Be(0);
            noImport.TypeName.Should().Be("NoImport");
            noImport.Location.Should().Be(" at (0;0)");
        }

        /// <summary>
        /// Tests that the parameterized constructor sets the properties correctly.
        /// </summary>
        [Fact]
        public void ParameterizedConstructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange & Act
            var noImport = new NoImport(_testProjectFilePath, _testImportedFileSpec, _testLine, _testColumn, _testReason);

            // Assert
            noImport.ProjectFilePath.Should().Be(_testProjectFilePath);
            // The constructor sets the Text property to the imported file spec.
            noImport.ToString().Should().Contain(_testImportedFileSpec);
            noImport.Line.Should().Be(_testLine);
            noImport.Column.Should().Be(_testColumn);
            noImport.Reason.Should().Be(_testReason);
        }

        /// <summary>
        /// Tests that the Location property returns a properly formatted string.
        /// </summary>
        [Fact]
        public void LocationProperty_ShouldReturnFormattedLocation()
        {
            // Arrange
            var expectedLocation = $" at ({_testLine};{_testColumn})";
            var noImport = new NoImport { Line = _testLine, Column = _testColumn };

            // Act
            var location = noImport.Location;

            // Assert
            location.Should().Be(expectedLocation);
        }

        /// <summary>
        /// Tests that the TypeName property always returns "NoImport".
        /// </summary>
        [Fact]
        public void TypeNameProperty_ShouldReturnNoImport()
        {
            // Arrange
            var noImport = new NoImport();

            // Act
            var typeName = noImport.TypeName;

            // Assert
            typeName.Should().Be("NoImport");
        }

        /// <summary>
        /// Tests that the ToString method returns the expected formatted string.
        /// </summary>
        [Fact]
        public void ToString_ShouldReturnCorrectFormattedString()
        {
            // Arrange
            var noImport = new NoImport(_testProjectFilePath, _testImportedFileSpec, _testLine, _testColumn, _testReason);
            var expectedString = $"NoImport: {_testImportedFileSpec} at ({_testLine};{_testColumn}) {_testReason}";

            // Act
            var result = noImport.ToString();

            // Assert
            result.Should().Be(expectedString);
        }

        /// <summary>
        /// Tests that the IHasSourceFile interface mapping returns ProjectFilePath.
        /// </summary>
        [Fact]
        public void IHasSourceFile_SourceFilePath_ShouldReturnProjectFilePath()
        {
            // Arrange
            var noImport = new NoImport { ProjectFilePath = _testProjectFilePath };

            // Act
            string sourceFilePath = ((IHasSourceFile)noImport).SourceFilePath;

            // Assert
            sourceFilePath.Should().Be(_testProjectFilePath);
        }

        /// <summary>
        /// Tests that the IHasLineNumber interface mapping returns Line.
        /// </summary>
        [Fact]
        public void IHasLineNumber_LineNumber_ShouldReturnLineValue()
        {
            // Arrange
            var noImport = new NoImport { Line = _testLine };

            // Act
            int? lineNumber = ((IHasLineNumber)noImport).LineNumber;

            // Assert
            lineNumber.Should().Be(_testLine);
        }

        /// <summary>
        /// Tests that the IsLowRelevance property getter and setter behave as expected.
        /// </summary>
        [Fact]
        public void IsLowRelevance_SetAndGet_ShouldReflectAssignedValue()
        {
            // Arrange
            var noImport = new NoImport();

            // Act & Assert
            // Initially, without flags set, IsLowRelevance is expected to be false.
            noImport.IsLowRelevance.Should().BeFalse();

            // When set to true, the getter should reflect the updated value.
            noImport.IsLowRelevance = true;
            noImport.IsLowRelevance.Should().BeTrue();

            // When set to false again, the getter should reflect the updated value.
            noImport.IsLowRelevance = false;
            noImport.IsLowRelevance.Should().BeFalse();
        }
    }
}
