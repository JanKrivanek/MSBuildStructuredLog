using System;
using System.IO;
using System.Reflection;
using Moq;
using Xunit;
using Microsoft.Build.Logging.StructuredLogger;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// A dummy implementation to simulate a TreeBinaryReader for testing Dispose.
    /// This dummy is used only to verify that BuildLogReader.Dispose properly disposes its internal reader.
    /// </summary>
    internal class DummyTreeBinaryReader : IDisposable
    {
        public bool IsDisposed { get; private set; }

        // Simulated Version property which BuildLogReader uses.
        public Version Version { get; set; } = new Version(1, 1, 154);

        public bool IsValid() => true;

        public string ReadString() => string.Empty;
//         public void ReadStringArray(Queue<string> queue) { } [Error] (24-37)CS0246 The type or namespace name 'Queue<>' could not be found (are you missing a using directive or an assembly reference?)
        public int ReadInt32() => 0;
        public byte[] ReadByteArray() => null;

        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="BuildLogReader"/> class.
    /// </summary>
    public class BuildLogReaderTests
    {
        private readonly Version _validVersion = new Version(1, 1, 154);

        /// <summary>
        /// Verifies that calling Dispose on BuildLogReader disposes its underlying TreeBinaryReader
        /// and sets the internal reader field to null.
        /// </summary>
        [Fact]
        public void Dispose_WhenCalled_DisposesTreeBinaryReader()
        {
            // Arrange: Create an instance of BuildLogReader using the non-public constructor.
            using var memoryStream = new MemoryStream();
            var buildLogReader = CreateInstanceUsingPrivateConstructor(memoryStream, _validVersion);
            // Replace the internal 'reader' field with our dummy which we'll monitor.
            var dummyReader = new DummyTreeBinaryReader();
            SetPrivateField(buildLogReader, "reader", dummyReader);

            // Act: Call Dispose on our BuildLogReader instance.
            buildLogReader.Dispose();

            // Assert: The internal reader field should be null and our dummy should be disposed.
            var readerAfterDispose = GetPrivateField(buildLogReader, "reader");
            Assert.Null(readerAfterDispose);
            Assert.True(dummyReader.IsDisposed);
        }

        /// <summary>
        /// Verifies that invoking the static Read(Stream, byte[], Version) method on BuildLogReader
        /// when provided an invalid log file format results in an exception.
        /// </summary>
        [Fact]
        public void Read_StreamWithInvalidFormat_ThrowsException()
        {
            // Arrange: Create an empty MemoryStream which simulates an invalid log file.
            using var memoryStream = new MemoryStream();

            // Act & Assert: Expect an exception with the message "Invalid log file format".
            var exception = Assert.Throws<Exception>(() => BuildLogReader.Read(memoryStream, null, _validVersion));
            Assert.Equal("Invalid log file format", exception.Message);
        }

        /// <summary>
        /// Verifies that invoking the static Read(string) method with a file path that does not exist
        /// causes a FileNotFoundException to be thrown.
        /// </summary>
        [Fact]
        public void Read_FilePathThatDoesNotExist_ThrowsFileNotFoundException()
        {
            // Arrange: Create a file path that is unlikely to exist.
            string nonExistentFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".log");

            // Act & Assert: Calling Read with a non-existent file should throw FileNotFoundException.
            Assert.Throws<FileNotFoundException>(() => BuildLogReader.Read(nonExistentFilePath));
        }

        /// <summary>
        /// Helper method to create an instance of BuildLogReader via its non-public constructor (Stream, Version).
        /// </summary>
        /// <param name="stream">The stream to pass to the constructor.</param>
        /// <param name="version">The version to pass to the constructor.</param>
        /// <returns>An instance of BuildLogReader.</returns>
        private BuildLogReader CreateInstanceUsingPrivateConstructor(Stream stream, Version version)
        {
            // Retrieve the non-public constructor with parameters (Stream, Version).
            var constructor = typeof(BuildLogReader).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(Stream), typeof(Version) }, null);
            Assert.NotNull(constructor);
            var instance = (BuildLogReader)constructor.Invoke(new object[] { stream, version });
            return instance;
        }

        /// <summary>
        /// Helper method to retrieve the value of a private field from an object.
        /// </summary>
        private object GetPrivateField(object instance, string fieldName)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field);
            return field.GetValue(instance);
        }

        /// <summary>
        /// Helper method to set the value of a private field for an object.
        /// </summary>
        private void SetPrivateField(object instance, string fieldName, object value)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field);
            field.SetValue(instance, value);
        }
    }
}
