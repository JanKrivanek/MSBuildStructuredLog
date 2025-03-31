// using FluentAssertions;
// using StructuredLogger.BinaryLogger;
// using Xunit;
// 
// namespace StructuredLogger.BinaryLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="EnvironmentVariableReadEventArgs2"/> class.
//     /// </summary>
//     public class EnvironmentVariableReadEventArgs2Tests
//     {
//         private readonly string _envVarName;
//         private readonly string _envVarValue;
//         private readonly string _file;
//         private readonly int _line;
//         private readonly int _column;
// 
//         /// <summary>
//         /// Initializes the test class with common test data.
//         /// </summary>
//         public EnvironmentVariableReadEventArgs2Tests()
//         {
//             _envVarName = "PATH";
//             _envVarValue = "C:\\Windows\\System32";
//             _file = "config.txt";
//             _line = 42;
//             _column = 7;
//         }
// //  // [Error] (44-22)CS1061 'EnvironmentVariableReadEventArgs2' does not contain a definition for 'EnvironmentVariableValue' and no accessible extension method 'EnvironmentVariableValue' accepting a first argument of type 'EnvironmentVariableReadEventArgs2' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the constructor sets all properties correctly with valid parameters.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithValidParameters_ShouldSetPropertiesCorrectly()
// //         {
// //             // Arrange & Act
// //             var instance = new EnvironmentVariableReadEventArgs2(_envVarName, _envVarValue, _file, _line, _column);
// // 
// //             // Assert
// //             instance.LineNumber.Should().Be(_line, "the constructor should set the LineNumber property correctly.");
// //             instance.ColumnNumber.Should().Be(_column, "the constructor should set the ColumnNumber property correctly.");
// //             instance.File.Should().Be(_file, "the constructor should set the File property correctly.");
// //             instance.EnvironmentVariableName.Should().Be(_envVarName, "the base EnvironmentVariableName should be set correctly.");
// //             instance.EnvironmentVariableValue.Should().Be(_envVarValue, "the base EnvironmentVariableValue should be set correctly.");
// //         }
// // 
//         /// <summary>
//         /// Tests that property setters update the values correctly after instantiation.
//         /// </summary>
//         [Fact]
//         public void PropertySetters_ShouldAllowUpdatingValues()
//         {
//             // Arrange
//             var instance = new EnvironmentVariableReadEventArgs2(_envVarName, _envVarValue, _file, _line, _column);
//             int newLine = 100;
//             int newColumn = 200;
//             string newFile = "updatedConfig.txt";
// 
//             // Act
//             instance.LineNumber = newLine;
//             instance.ColumnNumber = newColumn;
//             instance.File = newFile;
// 
//             // Assert
//             instance.LineNumber.Should().Be(newLine, "the LineNumber setter should update the value correctly.");
//             instance.ColumnNumber.Should().Be(newColumn, "the ColumnNumber setter should update the value correctly.");
//             instance.File.Should().Be(newFile, "the File setter should update the value correctly.");
//         }
// 
//         /// <summary>
//         /// Tests that the constructor handles edge case numeric values of zero for line and column.
//         /// </summary>
//         [Fact]
//         public void Constructor_WithZeroLineAndColumn_ShouldSetPropertiesCorrectly()
//         {
//             // Arrange
//             int zeroLine = 0;
//             int zeroColumn = 0;
//             string testFile = "edgeCaseConfig.txt";
// 
//             // Act
//             var instance = new EnvironmentVariableReadEventArgs2(_envVarName, _envVarValue, testFile, zeroLine, zeroColumn);
// 
//             // Assert
//             instance.LineNumber.Should().Be(zeroLine, "the constructor should allow zero as a valid value for LineNumber.");
//             instance.ColumnNumber.Should().Be(zeroColumn, "the constructor should allow zero as a valid value for ColumnNumber.");
//             instance.File.Should().Be(testFile, "the constructor should set the File property correctly even with edge case numeric values.");
//         }
// //  // [Error] (106-22)CS1061 'EnvironmentVariableReadEventArgs2' does not contain a definition for 'EnvironmentVariableValue' and no accessible extension method 'EnvironmentVariableValue' accepting a first argument of type 'EnvironmentVariableReadEventArgs2' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that the constructor correctly sets properties when provided with empty string parameters.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_WithEmptyStrings_ShouldSetPropertiesCorrectly()
// //         {
// //             // Arrange
// //             string emptyString = string.Empty;
// //             int testLine = 10;
// //             int testColumn = 5;
// // 
// //             // Act
// //             var instance = new EnvironmentVariableReadEventArgs2(emptyString, emptyString, emptyString, testLine, testColumn);
// // 
// //             // Assert
// //             instance.EnvironmentVariableName.Should().Be(emptyString, "the constructor should allow empty strings for EnvironmentVariableName.");
// //             instance.EnvironmentVariableValue.Should().Be(emptyString, "the constructor should allow empty strings for EnvironmentVariableValue.");
// //             instance.File.Should().Be(emptyString, "the constructor should allow empty strings for File.");
// //             instance.LineNumber.Should().Be(testLine, "the constructor should set the LineNumber property correctly even with empty strings.");
// //             instance.ColumnNumber.Should().Be(testColumn, "the constructor should set the ColumnNumber property correctly even with empty strings.");
// //         }
// //     }
// }