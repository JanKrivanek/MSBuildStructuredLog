using System;
using StructuredLogger.BinaryLogger;
using Xunit;

namespace StructuredLogger.BinaryLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="EnvironmentVariableReadEventArgs2"/> class.
    /// </summary>
    public class EnvironmentVariableReadEventArgs2Tests
    {
        /// <summary>
        /// Tests the constructor with valid inputs to ensure properties are initialized correctly.
        /// Arrange: Provide valid environment variable name, value, file, line, and column.
        /// Act: Create a new instance of EnvironmentVariableReadEventArgs2.
        /// Assert: Verify that the File, LineNumber, and ColumnNumber properties are set as expected.
        /// </summary>
        [Fact]
        public void Constructor_WithValidInputs_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedEnvVarName = "PATH";
            string expectedEnvVarValue = "/usr/bin";
            string expectedFile = "log.txt";
            int expectedLine = 10;
            int expectedColumn = 5;

            // Act
            var eventArgs = new EnvironmentVariableReadEventArgs2(
                expectedEnvVarName,
                expectedEnvVarValue,
                expectedFile,
                expectedLine,
                expectedColumn);

            // Assert
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
        }

        /// <summary>
        /// Tests the constructor when a null file parameter is provided.
        /// Arrange: Provide a null string for the file parameter.
        /// Act: Create a new instance of EnvironmentVariableReadEventArgs2.
        /// Assert: Verify that the File property is set to null and numeric properties are correctly initialized.
        /// </summary>
        [Fact]
        public void Constructor_WithNullFile_SetsFilePropertyToNull()
        {
            // Arrange
            string expectedEnvVarName = "TEMP";
            string expectedEnvVarValue = "/tmp";
            string expectedFile = null;
            int expectedLine = 0;
            int expectedColumn = 0;

            // Act
            var eventArgs = new EnvironmentVariableReadEventArgs2(
                expectedEnvVarName,
                expectedEnvVarValue,
                expectedFile,
                expectedLine,
                expectedColumn);

            // Assert
            Assert.Null(eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
        }

        /// <summary>
        /// Tests that properties can be modified after construction.
        /// Arrange: Create an instance using the constructor.
        /// Act: Change the values of LineNumber, ColumnNumber, and File.
        /// Assert: Confirm that the updated values are correctly reflected.
        /// </summary>
        [Fact]
        public void Properties_CanBeModified_AfterConstruction()
        {
            // Arrange
            var eventArgs = new EnvironmentVariableReadEventArgs2("VAR", "value", "initial.txt", 1, 1);
            int newLine = 100;
            int newColumn = 50;
            string newFile = "updated.log";

            // Act
            eventArgs.LineNumber = newLine;
            eventArgs.ColumnNumber = newColumn;
            eventArgs.File = newFile;

            // Assert
            Assert.Equal(newLine, eventArgs.LineNumber);
            Assert.Equal(newColumn, eventArgs.ColumnNumber);
            Assert.Equal(newFile, eventArgs.File);
        }

        /// <summary>
        /// Tests the constructor with negative values for line and column.
        /// Arrange: Provide negative values for the line and column parameters.
        /// Act: Create a new instance of EnvironmentVariableReadEventArgs2.
        /// Assert: Verify that the properties accept negative values as assigned.
        /// </summary>
        [Fact]
        public void Constructor_WithNegativeValues_SetsPropertiesCorrectly()
        {
            // Arrange
            string expectedEnvVarName = "NEGATIVE";
            string expectedEnvVarValue = "value";
            string expectedFile = "file.log";
            int expectedLine = -1;
            int expectedColumn = -5;

            // Act
            var eventArgs = new EnvironmentVariableReadEventArgs2(
                expectedEnvVarName,
                expectedEnvVarValue,
                expectedFile,
                expectedLine,
                expectedColumn);

            // Assert
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
        }
    }
}
