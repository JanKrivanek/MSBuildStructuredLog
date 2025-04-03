using FluentAssertions;
using Microsoft.Build.Framework;
using System;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="AssemblyLoadBuildEventArgs"/> class.
    /// </summary>
    public class AssemblyLoadBuildEventArgsTests
    {
        /// <summary>
        /// Tests that the parameterless constructor initializes properties to their default values 
        /// and that the Message property computes the expected default message.
        /// </summary>
        [Fact]
        public void DefaultConstructor_MessageMatchesExpected()
        {
            // Arrange
            // Default constructor should initialize enum to default (TaskRun) and other reference types to null.
            var expectedMessage = "Assembly loaded during TaskRun:  (location: , MVID: 00000000-0000-0000-0000-000000000000, AppDomain: [Default])";

            // Act
            var eventArgs = new AssemblyLoadBuildEventArgs();
            string actualMessage = eventArgs.Message;

            // Assert
            actualMessage.Should().Be(expectedMessage);
        }

        /// <summary>
        /// Tests that the parameterized constructor correctly sets properties and Message property returns the expected message when all parameters are non-null and non-empty.
        /// </summary>
        [Fact]
        public void Ctor_WithValidParameters_ReturnsCorrectMessage()
        {
            // Arrange
            AssemblyLoadingContext loadingContext = AssemblyLoadingContext.Evaluation;
            string loadingInitiator = "Initiator";
            string assemblyName = "TestAssembly";
            string assemblyPath = "C:\\Path\\Assembly.dll";
            Guid mvid = Guid.Parse("11111111-1111-1111-1111-111111111111");
            string customAppDomainDescriptor = "CustomDomain";
            var expectedMessage = $"Assembly loaded during {loadingContext} (Initiator): {assemblyName} (location: {assemblyPath}, MVID: {mvid}, AppDomain: {customAppDomainDescriptor})";

            // Act
            var eventArgs = new AssemblyLoadBuildEventArgs(loadingContext, loadingInitiator, assemblyName, assemblyPath, mvid, customAppDomainDescriptor);
            string actualMessage = eventArgs.Message;

            // Assert
            actualMessage.Should().Be(expectedMessage);
        }

        /// <summary>
        /// Tests that the parameterized constructor returns the expected message when the custom app domain descriptor is null,
        /// which should default to the "[Default]" value.
        /// </summary>
        [Fact]
        public void Ctor_WithNullCustomAppDomainDescriptor_ReturnsDefaultAppDomainInMessage()
        {
            // Arrange
            AssemblyLoadingContext loadingContext = AssemblyLoadingContext.Evaluation;
            string loadingInitiator = "Initiator";
            string assemblyName = "TestAssembly";
            string assemblyPath = "C:\\Path\\Assembly.dll";
            Guid mvid = Guid.Parse("11111111-1111-1111-1111-111111111111");
            string? customAppDomainDescriptor = null;
            var expectedMessage = $"Assembly loaded during {loadingContext} (Initiator): {assemblyName} (location: {assemblyPath}, MVID: {mvid}, AppDomain: [Default])";

            // Act
            var eventArgs = new AssemblyLoadBuildEventArgs(loadingContext, loadingInitiator, assemblyName, assemblyPath, mvid, customAppDomainDescriptor);
            string actualMessage = eventArgs.Message;

            // Assert
            actualMessage.Should().Be(expectedMessage);
        }

        /// <summary>
        /// Tests that the parameterized constructor correctly handles empty string inputs for loadingInitiator, assemblyName, assemblyPath, and customAppDomainDescriptor.
        /// This verifies that even with empty strings, the Message property returns a correctly formatted string.
        /// </summary>
        [Fact]
        public void Ctor_WithEmptyStrings_ReturnsCorrectMessage()
        {
            // Arrange
            AssemblyLoadingContext loadingContext = AssemblyLoadingContext.SdkResolution;
            string loadingInitiator = string.Empty; // Not null, but empty.
            string assemblyName = string.Empty;
            string assemblyPath = string.Empty;
            Guid mvid = Guid.Parse("22222222-2222-2222-2222-222222222222");
            string customAppDomainDescriptor = string.Empty;

            // When loadingInitiator is empty, it is not null so it will be formatted as " ()".
            var expectedMessage = $"Assembly loaded during {loadingContext} ()" +
                                  $": {assemblyName} (location: {assemblyPath}, MVID: {mvid}, AppDomain: {customAppDomainDescriptor})";

            // Act
            var eventArgs = new AssemblyLoadBuildEventArgs(loadingContext, loadingInitiator, assemblyName, assemblyPath, mvid, customAppDomainDescriptor);
            string actualMessage = eventArgs.Message;

            // Assert
            actualMessage.Should().Be(expectedMessage);
        }

        /// <summary>
        /// Tests that the parameterized constructor correctly handles a null assemblyName input.
        /// The Message property should treat a null assemblyName as an empty string in the formatted message.
        /// </summary>
        [Fact]
        public void Ctor_WithNullAssemblyName_ReturnsCorrectMessage()
        {
            // Arrange
            AssemblyLoadingContext loadingContext = AssemblyLoadingContext.LoggerInitialization;
            string loadingInitiator = "Init";
            string? assemblyName = null;
            string assemblyPath = "Assembly.dll";
            Guid mvid = Guid.Parse("33333333-3333-3333-3333-333333333333");
            string customAppDomainDescriptor = "AD";
            // assemblyName null becomes empty within string.Format.
            var expectedMessage = $"Assembly loaded during {loadingContext} (Init): " +
                                  $" (location: {assemblyPath}, MVID: {mvid}, AppDomain: {customAppDomainDescriptor})";

            // Act
            var eventArgs = new AssemblyLoadBuildEventArgs(loadingContext, loadingInitiator, assemblyName, assemblyPath, mvid, customAppDomainDescriptor);
            string actualMessage = eventArgs.Message;

            // Assert
            actualMessage.Should().Be(expectedMessage);
        }
    }
}
