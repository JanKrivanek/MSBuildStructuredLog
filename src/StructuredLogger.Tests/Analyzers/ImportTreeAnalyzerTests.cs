using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// A fake implementation of StringCache for testing purposes.
    /// It simply returns the input string for both SoftIntern and Intern calls.
    /// </summary>
    public class FakeStringCache : StringCache
    {
        public override string SoftIntern(string value)
        {
            return value;
        }

        public override void Intern(string value)
        {
            // No-op for testing.
        }
    }

    /// <summary>
    /// A fake implementation of ProjectImportedEventArgs for testing fallback path.
    /// This class derives from ProjectImportedEventArgs and provides minimal implementation.
    /// </summary>
    public class FakeProjectImportedEventArgs : ProjectImportedEventArgs
    {
        public FakeProjectImportedEventArgs(string message, string projectFile, string importedProjectFile, int lineNumber, int columnNumber)
        {
            // Although the real implementation is provided by MSBuild, for testing we assign the properties.
            // The ImportTreeAnalyzer code uses these properties only in the fallback branch.
            this.Message = message;
            this.ProjectFile = projectFile;
            this.ImportedProjectFile = importedProjectFile;
            this.LineNumber = lineNumber;
            this.ColumnNumber = columnNumber;
        }

        public override string Message { get; }
        public override string ProjectFile { get; }
        public override string ImportedProjectFile { get; }
        public override int LineNumber { get; }
        public override int ColumnNumber { get; }
    }

    /// <summary>
    /// Unit tests for the <see cref="ImportTreeAnalyzer"/> class.
    /// </summary>
    public class ImportTreeAnalyzerTests
    {
        private readonly FakeStringCache _stringCache;
        private readonly ImportTreeAnalyzer _analyzer;

        public ImportTreeAnalyzerTests()
        {
            _stringCache = new FakeStringCache();
            _analyzer = new ImportTreeAnalyzer(_stringCache);
        }

        /// <summary>
        /// Tests that the instance method TryGetImportOrNoImport(ProjectImportedEventArgs) falls back to the static method
        /// when the Reflector returned arguments do not meet any expected length requirements.
        /// In this test, we simulate that by passing in an event args with a Message that is not recognized by the static parser.
        /// Expected result is null.
        /// </summary>
        [Fact]
        public void TryGetImportOrNoImport_WithNonMatchingMessage_ReturnsNull()
        {
            // Arrange - create a fake ProjectImportedEventArgs.
            // The message "non matching" should not match any known regex for import patterns.
            var fakeArgs = new FakeProjectImportedEventArgs("non matching", "Project.proj", "Import.proj", 0, 0);

            // Act
            // Since we cannot intercept the static calls to Reflector.GetMessage and Reflector.GetArguments,
            // we simulate the fallback scenario by assuming that those methods would return values that result in a call
            // to the static TryGetImportOrNoImport(string, StringCache) method.
            // Here, our fake message does not match, so we expect a null return.
            TextNode result = _analyzer.TryGetImportOrNoImport(fakeArgs);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that the static method TryGetImportOrNoImport(string, StringCache) returns null when the input text does not match any expected pattern.
        /// </summary>
        [Fact]
        public void TryGetImportOrNoImport_Static_WithNonMatchingText_ReturnsNull()
        {
            // Arrange
            string nonMatchingText = "This is not a valid import message";

            // Act
            TextNode result = ImportTreeAnalyzer.TryGetImportOrNoImport(nonMatchingText, _stringCache);

            // Assert
            Assert.Null(result);
        }
    }
}
