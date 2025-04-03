using FluentAssertions;
using Moq;
using System;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="NoImport"/> class.
    /// </summary>
    public class NoImportTests
    {
        /// <summary>
        /// Tests the default constructor to ensure all properties are initialized to their default values.
        /// </summary>
        [Fact]
        public void DefaultConstructor_ShouldInitializePropertiesToDefaultValues()
        {
            // Arrange & Act
            var noImport = new NoImport();

            // Assert
            noImport.ProjectFilePath.Should().BeNull("ProjectFilePath should be null by default");
            noImport.ImportedFileSpec.Should().BeNull("ImportedFileSpec should be null by default");
            noImport.Reason.Should().BeNull("Reason should be null by default");
            noImport.Line.Should().Be(0, "Line should be 0 by default");
            noImport.Column.Should().Be(0, "Column should be 0 by default");
        }

        /// <summary>
        /// Tests the parameterized constructor to ensure that all properties are set correctly.
        /// </summary>
        /// <param name="projectFilePath">The project file path.</param>
        /// <param name="importedFileSpec">The imported file specification.</param>
        /// <param name="line">The line number.</param>
        /// <param name="column">The column number.</param>
        /// <param name="reason">The reason for no import.</param>
        [Theory]
        [InlineData("C:\\Project\\file.proj", "ImportedSpec1", 10, 20, "TestReason")]
        [InlineData("", "", 0, 0, "")]
        [InlineData("/home/user/project/file.proj", "SpecWithSpecialChars!@#$%", int.MaxValue, int.MinValue, "EdgeCaseReason")]
        public void ParameterizedConstructor_ShouldSetPropertiesCorrectly(string projectFilePath, string importedFileSpec, int line, int column, string reason)
        {
            // Arrange & Act
            var noImport = new NoImport(projectFilePath, importedFileSpec, line, column, reason);

            // Assert
            noImport.ProjectFilePath.Should().Be(projectFilePath, "the ProjectFilePath should be set via the constructor");
            // The importedFileSpec is assigned to the inherited Text property.
            noImport.Text.Should().Be(importedFileSpec, "the Text property should reflect the importedFileSpec passed to the constructor");
            noImport.Line.Should().Be(line, "the Line property should be set via the constructor");
            noImport.Column.Should().Be(column, "the Column property should be set via the constructor");
            noImport.Reason.Should().Be(reason, "the Reason property should be set via the constructor");
            noImport.Location.Should().Be($" at ({line};{column})", "the Location should be formatted using the line and column values");
        }

        /// <summary>
        /// Tests the TypeName property to ensure it returns the correct type name.
        /// </summary>
        [Fact]
        public void TypeName_ShouldReturnCorrectTypeName()
        {
            // Arrange
            var noImport = new NoImport();

            // Act
            var typeName = noImport.TypeName;

            // Assert
            typeName.Should().Be("NoImport", "TypeName should return the name of the class");
        }

        /// <summary>
        /// Tests the ToString method to ensure it returns a correctly formatted string.
        /// </summary>
        /// <param name="projectFilePath">The project file path.</param>
        /// <param name="importedFileSpec">The imported file specification.</param>
        /// <param name="line">The line number.</param>
        /// <param name="column">The column number.</param>
        /// <param name="reason">The reason for no import.</param>
        [Theory]
        [InlineData("C:\\Project\\file.proj", "SpecValue", 5, 15, "SomeReason")]
        [InlineData("", "EmptySpec", 0, 0, "")]
        public void ToString_ShouldReturnFormattedString(string projectFilePath, string importedFileSpec, int line, int column, string reason)
        {
            // Arrange
            var noImport = new NoImport(projectFilePath, importedFileSpec, line, column, reason);
            var expectedString = $"NoImport: {importedFileSpec} at ({line};{column}) {reason}";

            // Act
            var result = noImport.ToString();

            // Assert
            result.Should().Be(expectedString, "the ToString method should return a formatted string with Text, Location, and Reason");
        }

        /// <summary>
        /// Tests the default value of the IsLowRelevance property.
        /// </summary>
        [Fact]
        public void IsLowRelevance_Default_ShouldReturnFalse()
        {
            // Arrange
            var noImport = new NoImport();
            
            // Act
            var isLowRelevance = noImport.IsLowRelevance;

            // Assert
            isLowRelevance.Should().BeFalse("by default, the node should not be marked as low relevance because no flag is set");
        }

        /// <summary>
        /// Tests setting the IsLowRelevance property to ensure that no exceptions are thrown.
        /// Note: Verifying the exact behavior of the getter requires control over the base class implementation of HasFlag and SetFlag.
        /// </summary>
        [Fact]
        public void IsLowRelevance_Setter_ShouldNotThrowException()
        {
            // Arrange
            var noImport = new NoImport();

            // Act
            Action setTrue = () => noImport.IsLowRelevance = true;
            Action setFalse = () => noImport.IsLowRelevance = false;

            // Assert
            setTrue.Should().NotThrow("setting IsLowRelevance should not throw an exception");
            setFalse.Should().NotThrow("resetting IsLowRelevance should not throw an exception");
        }
    }
}
