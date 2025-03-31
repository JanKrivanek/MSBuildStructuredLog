using FluentAssertions;
using System;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="AssemblyLoadBuildEventArgs"/> class.
    /// </summary>
    public class AssemblyLoadBuildEventArgsTests
    {
        private readonly Guid _testGuid;

        public AssemblyLoadBuildEventArgsTests()
        {
            _testGuid = Guid.NewGuid();
        }

        /// <summary>
        /// Tests that the parameterless constructor results in default properties and returns the expected formatted message.
        /// Arrange: Creates an instance using the parameterless constructor.
        /// Act: Retrieves the Message property.
        /// Assert: The Message matches expected string constructed using default values.
        /// </summary>
        [Fact]
        public void Message_AfterParameterlessConstructor_ReturnsCorrectFormattedMessage()
        {
            // Arrange
            var args = new AssemblyLoadBuildEventArgs();

            // Act
            string message = args.Message;

            // Expected values:
            // LoadingContext default is TaskRun.
            // LoadingInitiator is null -> results in empty string in message.
            // AssemblyName is null -> becomes empty.
            // AssemblyPath is null -> becomes empty.
            // MVID is Guid.Empty.
            // AppDomainDescriptor is null -> falls back to "[Default]".
            string expected = string.Format(
                "Assembly loaded during {0}{1}: {2} (location: {3}, MVID: {4}, AppDomain: {5})",
                AssemblyLoadingContext.TaskRun.ToString(),
                "", // LoadingInitiator is null so no additional text.
                "",
                "",
                Guid.Empty.ToString(),
                "[Default]");

            // Assert
            message.Should().Be(expected);
        }

        /// <summary>
        /// Tests that the parameterized constructor properly initializes properties and returns the expected formatted message when LoadingInitiator is provided.
        /// Arrange: Creates an instance using the parameterized constructor with non-null LoadingInitiator and custom AppDomainDescriptor.
        /// Act: Retrieves the Message property.
        /// Assert: The Message matches the expected formatted string including the LoadingInitiator text.
        /// </summary>
        [Fact]
        public void Message_AfterParameterizedConstructorWithInitiator_ReturnsCorrectFormattedMessage()
        {
            // Arrange
            var loadingContext = AssemblyLoadingContext.Evaluation;
            string loadingInitiator = "InitiatorX";
            string assemblyName = "TestAssembly";
            string assemblyPath = @"C:\Path\TestAssembly.dll";
            var mvid = _testGuid;
            string customAppDomainDescriptor = "CustomDomain";

            var args = new AssemblyLoadBuildEventArgs(
                loadingContext,
                loadingInitiator,
                assemblyName,
                assemblyPath,
                mvid,
                customAppDomainDescriptor,
                MessageImportance.High);

            // Act
            string message = args.Message;

            // Expected: LoadingInitiator will contribute " (InitiatorX)".
            string expected = string.Format(
                "Assembly loaded during {0}{1}: {2} (location: {3}, MVID: {4}, AppDomain: {5})",
                loadingContext.ToString(),
                $" ({loadingInitiator})",
                assemblyName,
                assemblyPath,
                mvid.ToString(),
                customAppDomainDescriptor);

            // Assert
            message.Should().Be(expected);
        }

        /// <summary>
        /// Tests that the parameterized constructor properly initializes properties and returns the expected formatted message when LoadingInitiator is null.
        /// Arrange: Creates an instance using the parameterized constructor with a null LoadingInitiator and null AppDomainDescriptor.
        /// Act: Retrieves the Message property.
        /// Assert: The Message matches the expected formatted string using default values for null parameters.
        /// </summary>
        [Fact]
        public void Message_AfterParameterizedConstructorWithoutInitiator_ReturnsCorrectFormattedMessage()
        {
            // Arrange
            var loadingContext = AssemblyLoadingContext.SdkResolution;
            string? loadingInitiator = null;
            string assemblyName = "TestAssembly";
            string assemblyPath = @"C:\Path\TestAssembly.dll";
            var mvid = _testGuid;
            string? customAppDomainDescriptor = null; // Should fall back to "[Default]"

            var args = new AssemblyLoadBuildEventArgs(
                loadingContext,
                loadingInitiator,
                assemblyName,
                assemblyPath,
                mvid,
                customAppDomainDescriptor,
                MessageImportance.Low);

            // Act
            string message = args.Message;

            // Expected: When LoadingInitiator is null, it contributes empty string
            string expected = string.Format(
                "Assembly loaded during {0}{1}: {2} (location: {3}, MVID: {4}, AppDomain: {5})",
                loadingContext.ToString(),
                "", // No initiator string because null converts to empty
                assemblyName,
                assemblyPath,
                mvid.ToString(),
                "[Default]");

            // Assert
            message.Should().Be(expected);
        }

        /// <summary>
        /// Tests that the Message property caches its computed value and returns the same instance on multiple calls.
        /// Arrange: Creates an instance using the parameterized constructor.
        /// Act: Retrieves the Message property twice.
        /// Assert: Both retrieved messages are equal and reference the same instance (cached).
        /// </summary>
        [Fact]
        public void Message_MultipleCalls_ReturnsSameCachedValue()
        {
            // Arrange
            var loadingContext = AssemblyLoadingContext.LoggerInitialization;
            string loadingInitiator = "LoggerInit";
            string assemblyName = "LoggerAssembly";
            string assemblyPath = @"D:\Assemblies\Logger.dll";
            var mvid = _testGuid;
            string customAppDomainDescriptor = "DomainX";

            var args = new AssemblyLoadBuildEventArgs(
                loadingContext,
                loadingInitiator,
                assemblyName,
                assemblyPath,
                mvid,
                customAppDomainDescriptor,
                MessageImportance.Low);

            // Act
            string messageFirstCall = args.Message;
            string messageSecondCall = args.Message;

            string expected = string.Format(
                "Assembly loaded during {0}{1}: {2} (location: {3}, MVID: {4}, AppDomain: {5})",
                loadingContext.ToString(),
                $" ({loadingInitiator})",
                assemblyName,
                assemblyPath,
                mvid.ToString(),
                customAppDomainDescriptor);

            // Assert
            messageFirstCall.Should().Be(expected);
            messageSecondCall.Should().Be(expected);
            messageFirstCall.Should().BeSameAs(messageSecondCall);
        }
    }
}
