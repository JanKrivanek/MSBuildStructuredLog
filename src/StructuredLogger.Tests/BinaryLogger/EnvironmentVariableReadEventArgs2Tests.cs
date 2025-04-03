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
// //         /// <summary> // [Error] (32-23)CS1061 'EnvironmentVariableReadEventArgs2' does not contain a definition for 'EnvironmentVariableValue' and no accessible extension method 'EnvironmentVariableValue' accepting a first argument of type 'EnvironmentVariableReadEventArgs2' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests that the constructor of EnvironmentVariableReadEventArgs2 correctly assigns the provided values to the corresponding properties.
// //         /// </summary>
// //         /// <param name="envName">The environment variable name passed to the constructor.</param>
// //         /// <param name="envValue">The environment variable value passed to the constructor.</param>
// //         /// <param name="file">The file associated with the event passed to the constructor.</param>
// //         /// <param name="line">The line number passed to the constructor.</param>
// //         /// <param name="column">The column number passed to the constructor.</param>
// //         [Theory]
// //         [InlineData("ENV_VAR", "VALUE", "file.txt", 10, 20)]
// //         [InlineData("", "", "C:\\file", 0, 0)]
// //         [InlineData("SPECIAL!@#", "VALUE$%^", "/unix/path", int.MinValue, int.MaxValue)]
// //         [InlineData("long", "value", "THIS IS A VERY LONG FILE NAME THAT IS EXCESSIVELY LONG", 42, 84)]
// //         public void Constructor_ValidInputs_ShouldSetPropertiesCorrectly(string envName, string envValue, string file, int line, int column)
// //         {
// //             // Arrange & Act
// //             var eventArgs = new EnvironmentVariableReadEventArgs2(envName, envValue, file, line, column);
// // 
// //             // Assert that base class properties are correctly assigned.
// //             eventArgs.EnvironmentVariableName.Should().Be(envName, "the environment variable name should be correctly assigned from the constructor parameter");
// //             eventArgs.EnvironmentVariableValue.Should().Be(envValue, "the environment variable value should be correctly assigned from the constructor parameter");
// //             
// //             // Assert that new properties are correctly assigned.
// //             eventArgs.File.Should().Be(file, "the file property should be correctly assigned from the constructor parameter");
// //             eventArgs.LineNumber.Should().Be(line, "the line number should be correctly assigned from the constructor parameter");
// //             eventArgs.ColumnNumber.Should().Be(column, "the column number should be correctly assigned from the constructor parameter");
// //         }
// //     }
// }