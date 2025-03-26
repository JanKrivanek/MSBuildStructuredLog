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
        /// <summary>
        /// Tests that the parameterless constructor initializes the object with default values and that the Message property returns the expected formatted string.
        /// </summary>
        [Fact]
        public void Message_Property_ParameterlessConstructor_ReturnsExpectedMessage()
        {
            // Arrange
            var eventArgs = new AssemblyLoadBuildEventArgs();
            // Expected default assignment:
            // LoadingContext -> TaskRun (default enum value)
            // LoadingInitiator -> null, AssemblyName -> null, AssemblyPath -> null, MVID -> Guid.Empty, AppDomainDescriptor -> null (displayed as "[Default]")
            string expectedMessage = "Assembly loaded during TaskRun:  (location: , MVID: 00000000-0000-0000-0000-000000000000, AppDomain: [Default])";

            // Act
            string actualMessage = eventArgs.Message;

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        /// <summary>
        /// Tests that the overloaded constructor correctly assigns non-null parameters and that the Message property returns the expected formatted string.
        /// </summary>
        [Fact]
        public void OverloadedConstructor_AllParametersNonNull_ReturnsExpectedPropertyValuesAndMessage()
        {
            // Arrange
            AssemblyLoadingContext loadingContext = AssemblyLoadingContext.LoggerInitialization;
            string loadingInitiator = "TestInitiator";
            string assemblyName = "TestAssembly";
            string assemblyPath = "C:\\path\\to\\assembly.dll";
            Guid mvid = Guid.NewGuid();
            string customAppDomainDescriptor = "CustomAppDomain";

            // Act
            var eventArgs = new AssemblyLoadBuildEventArgs(loadingContext, loadingInitiator, assemblyName, assemblyPath, mvid, customAppDomainDescriptor);
            string actualMessage = eventArgs.Message;
            string expectedMessage = string.Format(
                "Assembly loaded during {0} ({1}): {2} (location: {3}, MVID: {4}, AppDomain: {5})",
                loadingContext.ToString(),
                loadingInitiator,
                assemblyName,
                assemblyPath,
                mvid.ToString(),
                customAppDomainDescriptor);

            // Assert
            Assert.Equal(loadingContext, eventArgs.LoadingContext);
            Assert.Equal(loadingInitiator, eventArgs.LoadingInitiator);
            Assert.Equal(assemblyName, eventArgs.AssemblyName);
            Assert.Equal(assemblyPath, eventArgs.AssemblyPath);
            Assert.Equal(mvid, eventArgs.MVID);
            Assert.Equal(customAppDomainDescriptor, eventArgs.AppDomainDescriptor);
            Assert.Equal(expectedMessage, actualMessage);
        }

        /// <summary>
        /// Tests that the overloaded constructor correctly handles null values for optional parameters (loadingInitiator and customAppDomainDescriptor)
        /// and that the Message property returns the expected formatted string with default app domain descriptor.
        /// </summary>
        [Fact]
        public void OverloadedConstructor_NullOptionalParameters_ReturnsExpectedMessage()
        {
            // Arrange
            AssemblyLoadingContext loadingContext = AssemblyLoadingContext.Evaluation;
            string? loadingInitiator = null;
            string assemblyName = "Assembly2";
            string assemblyPath = "path";
            Guid mvid = new Guid("11111111-1111-1111-1111-111111111111");
            string? customAppDomainDescriptor = null;  // Should trigger use of default "[Default]" in Message property

            // Act
            var eventArgs = new AssemblyLoadBuildEventArgs(loadingContext, loadingInitiator, assemblyName, assemblyPath, mvid, customAppDomainDescriptor);
            string actualMessage = eventArgs.Message;
            string expectedMessage = string.Format(
                "Assembly loaded during {0}: {1} (location: {2}, MVID: {3}, AppDomain: {4})",
                loadingContext.ToString(),
                assemblyName,
                assemblyPath,
                mvid.ToString(),
                "[Default]");

            // Assert
            Assert.Equal(loadingContext, eventArgs.LoadingContext);
            Assert.Null(eventArgs.LoadingInitiator);
            Assert.Equal(assemblyName, eventArgs.AssemblyName);
            Assert.Equal(assemblyPath, eventArgs.AssemblyPath);
            Assert.Equal(mvid, eventArgs.MVID);
            Assert.Null(eventArgs.AppDomainDescriptor);
            Assert.Equal(expectedMessage, actualMessage);
        }
    }
}
