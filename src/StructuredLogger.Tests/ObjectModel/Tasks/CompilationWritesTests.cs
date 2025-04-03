// using System;
// using System.Collections.Generic;
// using System.IO;
// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Moq;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="CompilationWrites"/> struct.
//     /// </summary>
//     public class CompilationWritesTests
//     {
// //         /// <summary> // [Error] (29-68)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
// //         /// Tests the TryParse method when the task does not contain a parameters folder.
// //         /// Expected outcome: The method returns null.
// //         /// </summary>
// //         [Fact]
// //         public void TryParse_NoParametersFolder_ReturnsNull()
// //         {
// //             // Arrange
// //             var taskMock = new Mock<Task>();
// //             taskMock.Setup(t => t.FindChild<Folder>(It.IsAny<Predicate<Folder>>()))
// //                     .Returns((Folder)null);
// // 
// //             // Act
// //             CompilationWrites? result = CompilationWrites.TryParse(taskMock.Object);
// // 
// //             // Assert
// //             result.Should().BeNull();
// //         }
// //  // [Error] (56-68)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         /// <summary>
//         /// Tests the TryParse method when the parameters folder is present but neither OutputAssembly nor OutputRefAssembly is provided.
//         /// Expected outcome: The method returns null.
//         /// </summary>
//         [Fact]
//         public void TryParse_ParametersWithoutAssembly_ReturnsNull()
//         {
//             // Arrange
//             var propertyMock = new Mock<Property>();
//             propertyMock.SetupGet(p => p.Name).Returns("SomeOtherProperty");
//             propertyMock.SetupGet(p => p.Value).Returns("Value");
// 
//             var folderMock = new Mock<Folder>();
//             folderMock.SetupGet(f => f.Name).Returns(Strings.Parameters);
//             folderMock.SetupGet(f => f.Children).Returns(new List<BaseNode> { propertyMock.Object });
// 
//             var taskMock = new Mock<Task>();
//             taskMock.Setup(t => t.FindChild<Folder>(It.Is<Predicate<Folder>>(pred => pred(folderMock.Object))))
//                     .Returns(folderMock.Object);
// 
//             // Act
//             CompilationWrites? result = CompilationWrites.TryParse(taskMock.Object);
// 
//             // Assert
//             result.Should().BeNull();
//         }
// //  // [Error] (109-68)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
// //         /// <summary>
// //         /// Tests the TryParse method when the task contains a valid OutputAssembly property and a DebugType that triggers PDB generation.
// //         /// Expected outcome: Returns a CompilationWrites instance with correct Pdb value.
// //         /// </summary>
// //         [Fact]
// //         public void TryParse_ValidOutputAssemblyWithDebugType_ReturnsCompilationWrites()
// //         {
// //             // Arrange
// //             string assemblyPath = "C:/build/bin.dll";
// //             string expectedPdb = Path.ChangeExtension(assemblyPath, ".pdb");
// //             string documentationFile = "C:/build/bin.xml";
// //             string sourceLink = "http://example.com/source";
// //             string debugTypeValue = "full";
// // 
// //             var outputAssemblyPropMock = new Mock<Property>();
// //             outputAssemblyPropMock.SetupGet(p => p.Name).Returns("OutputAssembly");
// //             outputAssemblyPropMock.SetupGet(p => p.Value).Returns(assemblyPath);
// // 
// //             var debugTypePropMock = new Mock<Property>();
// //             debugTypePropMock.SetupGet(p => p.Name).Returns("DebugType");
// //             debugTypePropMock.SetupGet(p => p.Value).Returns(debugTypeValue);
// // 
// //             var docPropMock = new Mock<Property>();
// //             docPropMock.SetupGet(p => p.Name).Returns("DocumentationFile");
// //             docPropMock.SetupGet(p => p.Value).Returns(documentationFile);
// // 
// //             var sourceLinkPropMock = new Mock<Property>();
// //             sourceLinkPropMock.SetupGet(p => p.Name).Returns("SourceLink");
// //             sourceLinkPropMock.SetupGet(p => p.Value).Returns(sourceLink);
// // 
// //             var children = new List<BaseNode>
// //             {
// //                 outputAssemblyPropMock.Object,
// //                 debugTypePropMock.Object,
// //                 docPropMock.Object,
// //                 sourceLinkPropMock.Object
// //             };
// // 
// //             var folderMock = new Mock<Folder>();
// //             folderMock.SetupGet(f => f.Name).Returns(Strings.Parameters);
// //             folderMock.SetupGet(f => f.Children).Returns(children);
// // 
// //             var taskMock = new Mock<Task>();
// //             taskMock.Setup(t => t.FindChild<Folder>(It.IsAny<Predicate<Folder>>()))
// //                     .Returns(folderMock.Object);
// // 
// //             // Act
// //             CompilationWrites? result = CompilationWrites.TryParse(taskMock.Object);
// // 
// //             // Assert
// //             result.Should().NotBeNull();
// //             result.Value.Assembly.Should().Be(assemblyPath);
// //             result.Value.RefAssembly.Should().BeNull();
// //             result.Value.Pdb.Should().Be(expectedPdb);
// //             result.Value.XmlDocumentation.Should().Be(documentationFile);
// //             result.Value.SourceLink.Should().Be(sourceLink);
// //         }
// //  // [Error] (148-68)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
//         /// <summary>
//         /// Tests the TryParse method when the task contains a valid OutputRefAssembly property without OutputAssembly.
//         /// Expected outcome: Returns a CompilationWrites instance with Assembly as null and Pdb as null.
//         /// </summary>
//         [Fact]
//         public void TryParse_ValidOutputRefAssemblyWithoutDebugType_ReturnsCompilationWrites()
//         {
//             // Arrange
//             string refAssemblyPath = "C:/build/ref.dll";
// 
//             var refAssemblyPropMock = new Mock<Property>();
//             refAssemblyPropMock.SetupGet(p => p.Name).Returns("OutputRefAssembly");
//             refAssemblyPropMock.SetupGet(p => p.Value).Returns(refAssemblyPath);
// 
//             var children = new List<BaseNode>
//             {
//                 refAssemblyPropMock.Object
//             };
// 
//             var folderMock = new Mock<Folder>();
//             folderMock.SetupGet(f => f.Name).Returns(Strings.Parameters);
//             folderMock.SetupGet(f => f.Children).Returns(children);
// 
//             var taskMock = new Mock<Task>();
//             taskMock.Setup(t => t.FindChild<Folder>(It.IsAny<Predicate<Folder>>()))
//                     .Returns(folderMock.Object);
// 
//             // Act
//             CompilationWrites? result = CompilationWrites.TryParse(taskMock.Object);
// 
//             // Assert
//             result.Should().NotBeNull();
//             result.Value.Assembly.Should().BeNull();
//             result.Value.RefAssembly.Should().Be(refAssemblyPath);
//             result.Value.Pdb.Should().BeNull();
//         }
// //  // [Error] (191-68)CS1503 Argument 1: cannot convert from 'Microsoft.Build.Logging.StructuredLogger.UnitTests.Task' to 'Microsoft.Build.Logging.StructuredLogger.Task'
// //         /// <summary>
// //         /// Tests the TryParse method when the task contains an OutputAssembly property with a DebugType that does not trigger PDB generation.
// //         /// Expected outcome: Returns a CompilationWrites instance with Pdb as null.
// //         /// </summary>
// //         [Fact]
// //         public void TryParse_OutputAssemblyWithNonTriggeringDebugType_ReturnsCompilationWritesWithoutPdb()
// //         {
// //             // Arrange
// //             string assemblyPath = "C:/bin/abc.dll";
// //             string debugTypeValue = "none";
// // 
// //             var outputAssemblyPropMock = new Mock<Property>();
// //             outputAssemblyPropMock.SetupGet(p => p.Name).Returns("OutputAssembly");
// //             outputAssemblyPropMock.SetupGet(p => p.Value).Returns(assemblyPath);
// // 
// //             var debugTypePropMock = new Mock<Property>();
// //             debugTypePropMock.SetupGet(p => p.Name).Returns("DebugType");
// //             debugTypePropMock.SetupGet(p => p.Value).Returns(debugTypeValue);
// // 
// //             var children = new List<BaseNode>
// //             {
// //                 outputAssemblyPropMock.Object,
// //                 debugTypePropMock.Object
// //             };
// // 
// //             var folderMock = new Mock<Folder>();
// //             folderMock.SetupGet(f => f.Name).Returns(Strings.Parameters);
// //             folderMock.SetupGet(f => f.Children).Returns(children);
// // 
// //             var taskMock = new Mock<Task>();
// //             taskMock.Setup(t => t.FindChild<Folder>(It.IsAny<Predicate<Folder>>()))
// //                     .Returns(folderMock.Object);
// // 
// //             // Act
// //             CompilationWrites? result = CompilationWrites.TryParse(taskMock.Object);
// // 
// //             // Assert
// //             result.Should().NotBeNull();
// //             result.Value.Assembly.Should().Be(assemblyPath);
// //             result.Value.Pdb.Should().BeNull();
// //         }
// // 
//         /// <summary>
//         /// Tests the AssemblyOrRefAssembly property when Assembly is not null.
//         /// Expected outcome: Returns the Assembly value.
//         /// </summary>
//         [Fact]
//         public void AssemblyOrRefAssembly_WhenAssemblyIsNotNull_ReturnsAssembly()
//         {
//             // Arrange
//             string assemblyPath = "C:/bin/foo.dll";
//             var compilationWrites = new CompilationWrites(assemblyPath, "irrelevant", "dummy", "dummy", "dummy");
// 
//             // Act
//             string result = compilationWrites.AssemblyOrRefAssembly;
// 
//             // Assert
//             result.Should().Be(assemblyPath);
//         }
// 
//         /// <summary>
//         /// Tests the AssemblyOrRefAssembly property when Assembly is null.
//         /// Expected outcome: Returns the RefAssembly value.
//         /// </summary>
//         [Fact]
//         public void AssemblyOrRefAssembly_WhenAssemblyIsNull_ReturnsRefAssembly()
//         {
//             // Arrange
//             string refAssemblyPath = "C:/bin/ref.dll";
//             var compilationWrites = new CompilationWrites(null, refAssemblyPath, "dummy", "dummy", "dummy");
// 
//             // Act
//             string result = compilationWrites.AssemblyOrRefAssembly;
// 
//             // Assert
//             result.Should().Be(refAssemblyPath);
//         }
// 
//         /// <summary>
//         /// Tests the ToString method to verify it returns the file name from AssemblyOrRefAssembly.
//         /// Expected outcome: Returns the file name (e.g., "foo.dll").
//         /// </summary>
//         [Fact]
//         public void ToString_ReturnsFileNameFromAssemblyOrRefAssembly()
//         {
//             // Arrange
//             string assemblyPath = "C:/bin/foo.dll";
//             var compilationWrites = new CompilationWrites(assemblyPath, null, "dummy", "dummy", "dummy");
//             string expectedFileName = Path.GetFileName(assemblyPath);
// 
//             // Act
//             string result = compilationWrites.ToString();
// 
//             // Assert
//             result.Should().Be(expectedFileName);
//         }
//     }
// }