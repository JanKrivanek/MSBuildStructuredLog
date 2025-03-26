// using System;
// using System.IO;
// using Microsoft.Build.Framework;
// using Microsoft.Build.Logging.StructuredLogger;
// using Moq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Fake implementation of IEventSource for testing purposes.
//     /// </summary>
// //     internal class FakeEventSource : IEventSource [Error] (13-38)CS0738 'FakeEventSource' does not implement interface member 'IEventSource.StatusEventRaised'. 'FakeEventSource.StatusEventRaised' cannot implement 'IEventSource.StatusEventRaised' because it does not have the matching return type of 'BuildStatusEventHandler'.
// //     {
// //         public event BuildStartedEventHandler BuildStarted;
// //         public event BuildFinishedEventHandler BuildFinished;
// //         public event ProjectStartedEventHandler ProjectStarted;
// //         public event ProjectFinishedEventHandler ProjectFinished;
// //         public event TargetStartedEventHandler TargetStarted;
// //         public event TargetFinishedEventHandler TargetFinished;
// //         public event TaskStartedEventHandler TaskStarted;
// //         public event TaskFinishedEventHandler TaskFinished;
// //         public event BuildMessageEventHandler MessageRaised;
// //         public event BuildWarningEventHandler WarningRaised;
// //         public event BuildErrorEventHandler ErrorRaised;
// //         public event CustomBuildEventHandler CustomEventRaised;
// //         public event BuildMessageEventHandler StatusEventRaised;
// //         public event AnyEventHandler AnyEventRaised;
// // 
// //         /// <summary>
// //         /// Raises the AnyEventRaised event.
// //         /// </summary>
// //         /// <param name="e">The event argument to raise.</param>
// //         public void RaiseAnyEvent(BuildEventArgs e)
// //         {
// //             AnyEventRaised?.Invoke(this, e);
// //         }
// //     }
// 
//     /// <summary>
//     /// Fake implementation of ProjectImportedEventArgs for testing purposes.
//     /// </summary>
// //     internal class FakeProjectImportedEventArgs : BuildEventArgs, ICancelableEventArgs [Error] (43-67)CS0246 The type or namespace name 'ICancelableEventArgs' could not be found (are you missing a using directive or an assembly reference?) [Error] (50-20)CS1503 Argument 1: cannot convert from 'System.DateTime' to 'string?' [Error] (50-37)CS1503 Argument 2: cannot convert from 'System.DateTime' to 'string?' [Error] (50-73)CS1503 Argument 4: cannot convert from 'string' to 'System.DateTime'
// //     {
// //         /// <summary>
// //         /// Initializes a new instance of the <see cref="FakeProjectImportedEventArgs"/> class.
// //         /// </summary>
// //         /// <param name="importedProjectFile">The imported project file path.</param>
// //         public FakeProjectImportedEventArgs(string importedProjectFile)
// //             : base(DateTime.UtcNow, DateTime.UtcNow, "FakeSubcategory", "FakeCode")
// //         {
// //             ImportedProjectFile = importedProjectFile;
// //         }
// // 
// //         /// <summary>
// //         /// Gets or sets a value indicating whether the event should be canceled.
// //         /// </summary>
// //         public bool Cancel { get; set; }
// // 
// //         /// <summary>
// //         /// Gets the imported project file path.
// //         /// </summary>
// //         public string ImportedProjectFile { get; }
// //     }
// 
//     /// <summary>
//     /// Unit tests for the <see cref="StructuredLogger"/> class.
//     /// </summary>
//     public class StructuredLoggerTests
//     {
//         private readonly StructuredLogger _logger;
//         private readonly FakeEventSource _fakeEventSource;
// 
//         /// <summary>
//         /// Initializes a new instance of the <see cref="StructuredLoggerTests"/> class.
//         /// </summary>
//         public StructuredLoggerTests()
//         {
//             _logger = new StructuredLogger();
//             _fakeEventSource = new FakeEventSource();
//             // Reset static properties to default between tests.
//             StructuredLogger.CurrentBuild = null;
//             StructuredLogger.SaveLogToDisk = true;
//         }
// 
//         /// <summary>
//         /// Tests that Initialize throws a LoggerException when Parameters is null.
//         /// The test sets Parameters to null and expects a LoggerException with an appropriate message.
//         /// </summary>
//         [Fact]
//         public void Initialize_WithNullParameters_ThrowsLoggerException()
//         {
//             // Arrange
//             _logger.Parameters = null;
// 
//             // Act & Assert
//             var ex = Assert.Throws<LoggerException>(() => _logger.Initialize(_fakeEventSource));
//             Assert.Contains("Need to specify a log file", ex.Message);
//         }
// 
//         /// <summary>
//         /// Tests that Initialize throws a LoggerException when Parameters contains more than one parameter.
//         /// The test sets Parameters with multiple values separated by a semicolon and expects a LoggerException.
//         /// </summary>
//         [Fact]
//         public void Initialize_WithInvalidParameters_ThrowsLoggerException()
//         {
//             // Arrange
//             _logger.Parameters = "log.txt;extra";
// 
//             // Act & Assert
//             var ex = Assert.Throws<LoggerException>(() => _logger.Initialize(_fakeEventSource));
//             Assert.Contains("Need to specify a log file", ex.Message);
//         }
// 
//         /// <summary>
//         /// Tests that Initialize with valid parameters sets environment variables and initializes internal components.
//         /// The test verifies that the correct environment variables are set and that event subscriptions are active.
//         /// </summary>
// //         [Fact] [Error] (138-36)CS0117 'Record' does not contain a definition for 'Exception'
// //         public void Initialize_WithValidParameters_SetsEnvironmentVariablesAndInitializesComponents()
// //         {
// //             // Arrange
// //             _logger.Parameters = "\"log.txt\"";
// //             Environment.SetEnvironmentVariable("MSBUILDTARGETOUTPUTLOGGING", null);
// //             Environment.SetEnvironmentVariable("MSBUILDLOGIMPORTS", null);
// // 
// //             // Act
// //             _logger.Initialize(_fakeEventSource);
// // 
// //             // Assert
// //             Assert.Equal("true", Environment.GetEnvironmentVariable("MSBUILDTARGETOUTPUTLOGGING"));
// //             Assert.Equal("1", Environment.GetEnvironmentVariable("MSBUILDLOGIMPORTS"));
// //             Assert.NotNull(_logger.Construction);
// // 
// //             // Assert that event subscriptions work without errors.
// //             var fakeArgs = new FakeProjectImportedEventArgs("imported.cs");
// //             var exception = Record.Exception(() => _fakeEventSource.RaiseAnyEvent(fakeArgs));
// //             Assert.Null(exception);
// //         }
// 
//         /// <summary>
//         /// Tests that Shutdown writes the log file to disk when SaveLogToDisk is true.
//         /// The test creates a temporary file path, initializes the logger, performs Shutdown,
//         /// and then verifies that the log file has been created on disk.
//         /// </summary>
//         [Fact]
//         public void Shutdown_SaveLogToDiskTrue_WritesLogFile()
//         {
//             // Arrange
//             StructuredLogger.SaveLogToDisk = true;
//             string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".log");
//             _logger.Parameters = $"\"{tempFile}\"";
//             _logger.Initialize(_fakeEventSource);
// 
//             // Act
//             _logger.Shutdown();
// 
//             // Assert
//             Assert.True(File.Exists(tempFile));
// 
//             // Cleanup
//             try
//             {
//                 File.Delete(tempFile);
//             }
//             catch
//             {
//                 // Ignore cleanup exceptions.
//             }
//         }
// 
//         /// <summary>
//         /// Tests that Shutdown sets the CurrentBuild static property when SaveLogToDisk is false.
//         /// After Shutdown, the static CurrentBuild property should equal the build produced by the logger.
//         /// </summary>
//         [Fact]
//         public void Shutdown_SaveLogToDiskFalse_SetsCurrentBuild()
//         {
//             // Arrange
//             StructuredLogger.SaveLogToDisk = false;
//             _logger.Parameters = "\"log.txt\""; // Parameter value is inconsequential here.
//             _logger.Initialize(_fakeEventSource);
// 
//             // Act
//             _logger.Shutdown();
// 
//             // Assert
//             Assert.NotNull(StructuredLogger.CurrentBuild);
//             Assert.Equal(_logger.Construction.Build, StructuredLogger.CurrentBuild);
//         }
// 
//         /// <summary>
//         /// Tests that Shutdown does not propagate exceptions thrown during log file serialization.
//         /// By providing an invalid log file (empty string) that may cause Serialization.Write to throw,
//         /// the test verifies that Shutdown completes without throwing an exception.
//         /// </summary>
// //         [Fact] [Error] (207-29)CS0117 'Record' does not contain a definition for 'Exception'
// //         public void Shutdown_WithInvalidLogFile_DoesNotPropagateException()
// //         {
// //             // Arrange
// //             StructuredLogger.SaveLogToDisk = true;
// //             _logger.Parameters = "\"\""; // Empty log file path to simulate invalid file scenario.
// //             _logger.Initialize(_fakeEventSource);
// // 
// //             // Act & Assert: Shutdown should not throw even if serialization fails.
// //             var ex = Record.Exception(() => _logger.Shutdown());
// //             Assert.Null(ex);
// //         }
//     }
// }
