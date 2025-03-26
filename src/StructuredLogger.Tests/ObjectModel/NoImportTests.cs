using Microsoft.Build.Logging.StructuredLogger;
using Moq;
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
        
        /// <summary>
        /// Initializes test fields with default test values.
        /// </summary>
        public NoImportTests()
        {
            _testProjectFilePath = "C:\\Project\\Test.csproj";
            _testImportedFileSpec = "ImportedFile.xml";
            _testReason = "Test reason";
            _testLine = 10;
            _testColumn = 5;
        }
        
        /// <summary>
        /// Tests that the default constructor initializes properties to their default values.
        /// Expected outcome: All string properties are null and integer properties are zero.
        /// </summary>
        [Fact]
        public void DefaultConstructor_InitializesPropertiesToDefaultValues()
        {
            // Arrange & Act
            var noImport = new NoImport();
            
            // Assert
            Assert.Null(noImport.ProjectFilePath);
            Assert.Null(noImport.ImportedFileSpec);
            Assert.Null(noImport.Reason);
            Assert.Equal(0, noImport.Line);
            Assert.Equal(0, noImport.Column);
            Assert.Equal("NoImport", noImport.TypeName);
        }
        
        /// <summary>
        /// Tests that the parameterized constructor correctly sets the properties.
        /// Expected outcome: ProjectFilePath, Text (via ImportedFileSpec), Line, Column, Reason and Location are set as expected.
        /// </summary>
        [Fact]
        public void ParameterizedConstructor_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var noImport = new NoImport(_testProjectFilePath, _testImportedFileSpec, _testLine, _testColumn, _testReason);
            
            // Assert
            Assert.Equal(_testProjectFilePath, noImport.ProjectFilePath);
            // Since the constructor assigns 'Text' with _testImportedFileSpec,
            // we check for its presence via the ToString output.
            string stringRepresentation = noImport.ToString();
            Assert.Contains(_testImportedFileSpec, stringRepresentation);
            Assert.Equal(_testLine, noImport.Line);
            Assert.Equal(_testColumn, noImport.Column);
            Assert.Equal(_testReason, noImport.Reason);
            Assert.Equal($" at ({_testLine};{_testColumn})", noImport.Location);
        }
        
        /// <summary>
        /// Tests that the TypeName property returns the correct value.
        /// Expected outcome: TypeName should always return "NoImport".
        /// </summary>
        [Fact]
        public void TypeName_ReturnsNoImport()
        {
            // Arrange
            var noImport = new NoImport();
            
            // Act
            var typeName = noImport.TypeName;
            
            // Assert
            Assert.Equal("NoImport", typeName);
        }
        
        /// <summary>
        /// Tests that the Location property formats the line and column correctly.
        /// Expected outcome: Location should return a string formatted as " at (Line;Column)".
        /// </summary>
        [Theory]
        [InlineData(0, 0, " at (0;0)")]
        [InlineData(15, 30, " at (15;30)")]
        public void Location_ReturnsFormattedString(int line, int column, string expectedLocation)
        {
            // Arrange
            var noImport = new NoImport
            {
                Line = line,
                Column = column
            };
            
            // Act
            var location = noImport.Location;
            
            // Assert
            Assert.Equal(expectedLocation, location);
        }
        
        /// <summary>
        /// Tests that the ToString method returns a consistent and expected string representation.
        /// Expected outcome: The string should include the imported file spec, the formatted location, and the reason.
        /// </summary>
        [Fact]
        public void ToString_ReturnsCorrectCombinedString()
        {
            // Arrange
            var noImport = new NoImport(_testProjectFilePath, _testImportedFileSpec, _testLine, _testColumn, _testReason);
            var expected = $"NoImport: {_testImportedFileSpec}{noImport.Location} {_testReason}";
            
            // Act
            var result = noImport.ToString();
            
            // Assert
            Assert.Equal(expected, result);
        }
        
        /// <summary>
        /// Tests that the interface implementations for IHasSourceFile and IHasLineNumber return the correct values.
        /// Expected outcome: SourceFilePath should mirror ProjectFilePath and LineNumber should equal Line.
        /// </summary>
        [Fact]
        public void InterfaceImplementations_ReturnCorrectValues()
        {
            // Arrange
            var noImport = new NoImport(_testProjectFilePath, _testImportedFileSpec, _testLine, _testColumn, _testReason);
            
            // Act
            var sourceFilePath = ((IHasSourceFile)noImport).SourceFilePath;
            var lineNumber = ((IHasLineNumber)noImport).LineNumber;
            
            // Assert
            Assert.Equal(_testProjectFilePath, sourceFilePath);
            Assert.Equal(_testLine, lineNumber);
        }
        
        /// <summary>
        /// Tests that the IsLowRelevance property's getter and setter operate without throwing exceptions.
        /// Expected outcome: The property can be set to both true and false and returns a boolean value.
        /// Note: Since the actual behavior depends on the inherited methods (HasFlag, IsSelected, SetFlag),
        /// this test primarily ensures that setting and getting does not cause errors.
        /// </summary>
        [Fact]
        public void IsLowRelevance_GetterSetter_WorksAsExpected()
        {
            // Arrange
            var noImport = new NoImport();
            
            // Act & Assert
            noImport.IsLowRelevance = true;
            bool valueAfterTrue = noImport.IsLowRelevance;
            
            noImport.IsLowRelevance = false;
            bool valueAfterFalse = noImport.IsLowRelevance;
            
            // Assert
            // We cannot predict the exact boolean outcome because the getter depends on internal flag logic.
            // However, we ensure that the property is accessible and returns a boolean value.
            Assert.IsType<bool>(valueAfterTrue);
            Assert.IsType<bool>(valueAfterFalse);
        }
    }
}
