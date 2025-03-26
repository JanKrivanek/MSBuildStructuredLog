using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="CompilationWrites"/> struct.
    /// </summary>
    public class CompilationWritesTests
    {
        /// <summary>
        /// Tests that the constructor correctly assigns property values.
        /// </summary>
        [Fact]
        public void Constructor_AssignsPropertiesCorrectly()
        {
            // Arrange
            string assembly = "C:\\bin\\test.dll";
            string refAssembly = "C:\\ref\\test.ref.dll";
            string pdb = "C:\\bin\\test.pdb";
            string xmlDocumentation = "C:\\docs\\test.xml";
            string sourceLink = "http://sourcelink";

            // Act
            var writes = new CompilationWrites(assembly, refAssembly, pdb, xmlDocumentation, sourceLink);

            // Assert
            Assert.Equal(assembly, writes.Assembly);
            Assert.Equal(refAssembly, writes.RefAssembly);
            Assert.Equal(pdb, writes.Pdb);
            Assert.Equal(xmlDocumentation, writes.XmlDocumentation);
            Assert.Equal(sourceLink, writes.SourceLink);
        }

        /// <summary>
        /// Tests that AssemblyOrRefAssembly returns Assembly when it is not null.
        /// </summary>
        [Fact]
        public void AssemblyOrRefAssembly_WhenAssemblyNotNull_ReturnsAssembly()
        {
            // Arrange
            string assembly = "C:\\bin\\test.dll";
            string refAssembly = "C:\\ref\\test.ref.dll";
            var writes = new CompilationWrites(assembly, refAssembly, null, null, null);

            // Act
            var result = writes.AssemblyOrRefAssembly;

            // Assert
            Assert.Equal(assembly, result);
        }

        /// <summary>
        /// Tests that AssemblyOrRefAssembly returns RefAssembly when Assembly is null.
        /// </summary>
        [Fact]
        public void AssemblyOrRefAssembly_WhenAssemblyIsNull_ReturnsRefAssembly()
        {
            // Arrange
            string assembly = null;
            string refAssembly = "C:\\ref\\test.ref.dll";
            var writes = new CompilationWrites(assembly, refAssembly, null, null, null);

            // Act
            var result = writes.AssemblyOrRefAssembly;

            // Assert
            Assert.Equal(refAssembly, result);
        }

        /// <summary>
        /// Tests that ToString returns the file name of AssemblyOrRefAssembly.
        /// </summary>
        [Fact]
        public void ToString_ReturnsFileNameOfAssemblyOrRefAssembly()
        {
            // Arrange
            string assembly = "C:\\bin\\test.dll";
            var writes = new CompilationWrites(assembly, null, null, null, null);
            string expectedFileName = Path.GetFileName(assembly);

            // Act
            var result = writes.ToString();

            // Assert
            Assert.Equal(expectedFileName, result);
        }

        /// <summary>
        /// Tests that TryParse returns null when the task does not have a parameters folder.
        /// </summary>
//         [Fact] [Error] (104-53)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         public void TryParse_NoParametersFolder_ReturnsNull()
//         {
//             // Arrange
//             var task = new Task();
//             // Do not add a parameters folder
// 
//             // Act
//             var result = CompilationWrites.TryParse(task);
// 
//             // Assert
//             Assert.Null(result);
//         }

        /// <summary>
        /// Tests that TryParse returns null when neither OutputAssembly nor OutputRefAssembly are provided.
        /// </summary>
//         [Fact] [Error] (119-43)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.Property' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (124-53)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         public void TryParse_NoOutputAssemblyOrRefAssembly_ReturnsNull()
//         {
//             // Arrange
//             var parametersFolder = new Folder { Name = Strings.Parameters };
//             // Add a property that is not assembly related.
//             parametersFolder.Children.Add(new Property { Name = "DocumentationFile", Value = "doc.xml" });
//             var task = new Task();
//             task.Children.Add(parametersFolder);
// 
//             // Act
//             var result = CompilationWrites.TryParse(task);
// 
//             // Assert
//             Assert.Null(result);
//         }

        /// <summary>
        /// Tests that TryParse correctly parses properties when OutputAssembly is provided and DebugType indicates that a PDB should be generated.
        /// </summary>
        /// <param name="debugType">The debug type value to test.</param>
//         [Theory] [Error] (145-43)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.Property' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (146-43)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.Property' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (147-43)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.Property' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (148-43)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.Property' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (153-53)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         [InlineData("Full")]
//         [InlineData("portable")]
//         [InlineData("PDBOnly")]
//         public void TryParse_WithOutputAssemblyAndValidDebugType_ReturnsCompilationWritesWithPdb(string debugType)
//         {
//             // Arrange
//             string assembly = "C:\\bin\\test.dll";
//             string documentation = "C:\\docs\\test.xml";
//             string sourceLink = "http://sourcelink";
//             var parametersFolder = new Folder { Name = Strings.Parameters };
//             parametersFolder.Children.Add(new Property { Name = "OutputAssembly", Value = assembly });
//             parametersFolder.Children.Add(new Property { Name = "DocumentationFile", Value = documentation });
//             parametersFolder.Children.Add(new Property { Name = "SourceLink", Value = sourceLink });
//             parametersFolder.Children.Add(new Property { Name = "DebugType", Value = debugType });
//             var task = new Task();
//             task.Children.Add(parametersFolder);
// 
//             // Act
//             var result = CompilationWrites.TryParse(task);
// 
//             // Assert
//             Assert.NotNull(result);
//             var writes = result.Value;
//             Assert.Equal(assembly, writes.Assembly);
//             Assert.Null(writes.RefAssembly);
//             Assert.Equal(Path.ChangeExtension(assembly, ".pdb"), writes.Pdb);
//             Assert.Equal(documentation, writes.XmlDocumentation);
//             Assert.Equal(sourceLink, writes.SourceLink);
//         }

        /// <summary>
        /// Tests that TryParse correctly parses properties when only OutputRefAssembly is provided, ensuring that PDB is not generated.
        /// </summary>
//         [Fact] [Error] (174-43)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.Property' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (176-43)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.Property' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (181-53)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         public void TryParse_WithOnlyOutputRefAssembly_ReturnsCompilationWritesWithoutPdb()
//         {
//             // Arrange
//             string refAssembly = "C:\\ref\\test.ref.dll";
//             var parametersFolder = new Folder { Name = Strings.Parameters };
//             parametersFolder.Children.Add(new Property { Name = "OutputRefAssembly", Value = refAssembly });
//             // Even if DebugType is valid, since OutputAssembly is null, no PDB should be generated.
//             parametersFolder.Children.Add(new Property { Name = "DebugType", Value = "full" });
//             var task = new Task();
//             task.Children.Add(parametersFolder);
// 
//             // Act
//             var result = CompilationWrites.TryParse(task);
// 
//             // Assert
//             Assert.NotNull(result);
//             var writes = result.Value;
//             Assert.Null(writes.Assembly);
//             Assert.Equal(refAssembly, writes.RefAssembly);
//             Assert.Null(writes.Pdb);
//         }

        /// <summary>
        /// Tests that TryParse handles DebugType in a case-insensitive manner.
        /// </summary>
//         [Fact] [Error] (200-43)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.Property' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (201-43)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.Property' to 'Microsoft.Build.Logging.StructuredLogger.UnitTests.FakeItem' [Error] (206-53)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         public void TryParse_DebugTypeCaseInsensitive_ReturnsCompilationWritesWithPdb()
//         {
//             // Arrange
//             string assembly = "C:\\bin\\test.dll";
//             var parametersFolder = new Folder { Name = Strings.Parameters };
//             parametersFolder.Children.Add(new Property { Name = "OutputAssembly", Value = assembly });
//             parametersFolder.Children.Add(new Property { Name = "DebugType", Value = "pOrTaBlE" });
//             var task = new Task();
//             task.Children.Add(parametersFolder);
// 
//             // Act
//             var result = CompilationWrites.TryParse(task);
// 
//             // Assert
//             Assert.NotNull(result);
//             var writes = result.Value;
//             Assert.Equal(Path.ChangeExtension(assembly, ".pdb"), writes.Pdb);
//         }
    }
}

namespace Microsoft.Build.Logging.StructuredLogger
{
    /// <summary>
    /// Minimal fake implementation of Task for testing purposes.
    /// </summary>
//     public class Task [Error] (221-18)CS0101 The namespace 'Microsoft.Build.Logging.StructuredLogger' already contains a definition for 'Task'
//     {
//         /// <summary>
//         /// Gets the child nodes of the task.
//         /// </summary>
//         public List<object> Children { get; } = new List<object>();
// 
//         /// <summary>
//         /// Finds the first child of type T that matches the specified predicate.
//         /// </summary>
//         /// <typeparam name="T">The type to search for.</typeparam>
//         /// <param name="predicate">The condition to match.</param>
//         /// <returns>The matching child if found; otherwise, null.</returns>
//         public T FindChild<T>(Predicate<T> predicate) where T : class [Error] (236-20)CS0229 Ambiguity between 'Task.Children' and 'Task.Children'
//         {
//             return Children.OfType<T>().FirstOrDefault(child => predicate(child));
//         }
//     }

    /// <summary>
    /// Minimal fake implementation of Folder for testing purposes.
    /// </summary>
    public class Folder
    {
        /// <summary>
        /// Gets or sets the folder's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the child nodes of the folder.
        /// </summary>
        public List<object> Children { get; } = new List<object>();
    }

    /// <summary>
    /// Minimal fake implementation of Property for testing purposes.
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Gets or sets the property's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the property's value.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Minimal fake implementation of Strings for testing purposes.
    /// </summary>
    public static class Strings
    {
        /// <summary>
        /// The constant representing the parameters folder name.
        /// </summary>
        public const string Parameters = "Parameters";
    }
}
