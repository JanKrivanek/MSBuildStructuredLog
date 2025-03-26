using Xunit;

namespace StructuredLogger.BinaryLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="EnvironmentVariableReadEventArgs2"/> class.
    /// </summary>
    public class EnvironmentVariableReadEventArgs2Tests
    {
        /// <summary>
        /// Tests that the constructor sets all properties to the provided valid arguments.
        /// Arrange: Provide valid non-null environment variable name, value, file, line and column.
        /// Act: Instantiate the EnvironmentVariableReadEventArgs2 class.
        /// Assert: Verify that the properties EnvironmentVariableName, EnvironmentVariableValue, File, LineNumber, and ColumnNumber are set as expected.
        /// </summary>
        [Fact]
        public void Constructor_WithValidArguments_SetsAllPropertiesCorrectly()
        {
            // Arrange
            string expectedVarName = "PATH";
            string expectedVarValue = "/usr/bin";
            string expectedFile = "build.proj";
            int expectedLine = 15;
            int expectedColumn = 20;

            // Act
            var eventArgs = new EnvironmentVariableReadEventArgs2(expectedVarName, expectedVarValue, expectedFile, expectedLine, expectedColumn);

            // Assert
            Assert.Equal(expectedVarName, eventArgs.EnvironmentVariableName);
            Assert.Equal(expectedVarValue, eventArgs.EnvironmentVariableValue);
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
        }

        /// <summary>
        /// Tests that the constructor correctly assigns a null file value.
        /// Arrange: Provide a null value for the file parameter.
        /// Act: Instantiate the EnvironmentVariableReadEventArgs2 class with file set to null.
        /// Assert: Verify that the File property is null while other properties are set as provided.
        /// </summary>
        [Fact]
        public void Constructor_WithNullFile_SetsFilePropertyToNull()
        {
            // Arrange
            string expectedVarName = "TEMP";
            string expectedVarValue = "C:\\Temp";
            string expectedFile = null;
            int expectedLine = 0;
            int expectedColumn = 0;

            // Act
            var eventArgs = new EnvironmentVariableReadEventArgs2(expectedVarName, expectedVarValue, expectedFile, expectedLine, expectedColumn);

            // Assert
            Assert.Equal(expectedVarName, eventArgs.EnvironmentVariableName);
            Assert.Equal(expectedVarValue, eventArgs.EnvironmentVariableValue);
            Assert.Null(eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
        }

        /// <summary>
        /// Tests that the constructor correctly assigns negative values for LineNumber and ColumnNumber.
        /// Arrange: Provide negative integers for line and column parameters.
        /// Act: Instantiate the EnvironmentVariableReadEventArgs2 class with negative line and column numbers.
        /// Assert: Verify that the LineNumber and ColumnNumber properties hold the negative values, indicating lack of validation.
        /// </summary>
        [Fact]
        public void Constructor_WithNegativeLineAndColumn_SetsPropertiesWithNegativeValues()
        {
            // Arrange
            string expectedVarName = "USER";
            string expectedVarValue = "john_doe";
            string expectedFile = "script.msbuild";
            int expectedLine = -5;
            int expectedColumn = -10;

            // Act
            var eventArgs = new EnvironmentVariableReadEventArgs2(expectedVarName, expectedVarValue, expectedFile, expectedLine, expectedColumn);

            // Assert
            Assert.Equal(expectedVarName, eventArgs.EnvironmentVariableName);
            Assert.Equal(expectedVarValue, eventArgs.EnvironmentVariableValue);
            Assert.Equal(expectedFile, eventArgs.File);
            Assert.Equal(expectedLine, eventArgs.LineNumber);
            Assert.Equal(expectedColumn, eventArgs.ColumnNumber);
        }
    }
}
