using Microsoft.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="AssemblyLoadBuildEventArgs"/> class.
    /// </summary>
    [TestClass]
    public class AssemblyLoadBuildEventArgsTests
    {
        private const string DefaultAppDomainDescriptor = "[Default]";

        /// <summary>
        /// Tests the default constructor to ensure the Message property returns the expected format when no values are set.
        /// </summary>
        [TestMethod]
        public void DefaultConstructor_Message_ReturnsExpectedMessage()
        {
            // Arrange
            // Create instance using the default constructor.
            AssemblyLoadBuildEventArgs args = new AssemblyLoadBuildEventArgs();
            // Manually compute expected values:
            // LoadingContext: default value is TaskRun.
            // LoadingInitiator, AssemblyName, AssemblyPath, AppDomainDescriptor are null (printed as empty string except for AppDomainDescriptor)
            // MVID: default Guid.Empty => "00000000-0000-0000-0000-000000000000"
            // Expected message will be formatted as:
            // "Assembly loaded during TaskRun:  (location: , MVID: 00000000-0000-0000-0000-000000000000, AppDomain: [Default])"
            string expectedMessage = "Assembly loaded during TaskRun:  (location: , MVID: 00000000-0000-0000-0000-000000000000, AppDomain: " + DefaultAppDomainDescriptor + ")";

            // Act
            string actualMessage = args.Message;

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage, "The message from the default constructor did not match the expected format.");
        }

        /// <summary>
        /// Tests the parameterized constructor with all non-null parameters to ensure the Message property returns the expected message.
        /// </summary>
        [TestMethod]
        public void ParameterizedConstructor_WithAllNonNullParameters_Message_ReturnsExpectedMessage()
        {
            // Arrange
            AssemblyLoadingContext loadingContext = AssemblyLoadingContext.LoggerInitialization;
            string loadingInitiator = "TestInitiator";
            string assemblyName = "TestAssembly";
            string assemblyPath = "C:\\Test\\Assembly.dll";
            Guid mvid = Guid.Parse("11111111-1111-1111-1111-111111111111");
            string customAppDomainDescriptor = "TestAppDomain";
            MessageImportance importance = MessageImportance.High;

            // Expected message:
            // "Assembly loaded during LoggerInitialization (TestInitiator): TestAssembly (location: C:\Test\Assembly.dll, MVID: 11111111-1111-1111-1111-111111111111, AppDomain: TestAppDomain)"
            string expectedMessage = string.Format(
                "Assembly loaded during {0} ({1}): {2} (location: {3}, MVID: {4}, AppDomain: {5})",
                loadingContext.ToString(),
                loadingInitiator,
                assemblyName,
                assemblyPath,
                mvid.ToString(),
                customAppDomainDescriptor);
            
            // Act
            AssemblyLoadBuildEventArgs args = new AssemblyLoadBuildEventArgs(loadingContext, loadingInitiator, assemblyName, assemblyPath, mvid, customAppDomainDescriptor, importance);
            string actualMessage = args.Message;

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage, "The message from the parameterized constructor with non-null parameters did not match the expected format.");
        }

        /// <summary>
        /// Tests the parameterized constructor with null values for optional parameters to ensure the Message property correctly applies defaults.
        /// </summary>
        [TestMethod]
        public void ParameterizedConstructor_WithNullOptionalParams_Message_ReturnsExpectedMessage()
        {
            // Arrange
            AssemblyLoadingContext loadingContext = AssemblyLoadingContext.Evaluation;
            string? loadingInitiator = null;
            string assemblyName = "AssemblyNull";
            string assemblyPath = "PathNull";
            Guid mvid = Guid.Parse("22222222-2222-2222-2222-222222222222");
            string? customAppDomainDescriptor = null; // should default to "[Default]" in message.
            MessageImportance importance = MessageImportance.Low;
            
            // Expected message:
            // Since loadingInitiator is null, no extra text is added.
            // "Assembly loaded during Evaluation: AssemblyNull (location: PathNull, MVID: 22222222-2222-2222-2222-222222222222, AppDomain: [Default])"
            string expectedMessage = string.Format(
                "Assembly loaded during {0}: {1} (location: {2}, MVID: {3}, AppDomain: {4})",
                loadingContext.ToString(),
                assemblyName,
                assemblyPath,
                mvid.ToString(),
                DefaultAppDomainDescriptor);
            
            // Act
            AssemblyLoadBuildEventArgs args = new AssemblyLoadBuildEventArgs(loadingContext, loadingInitiator, assemblyName, assemblyPath, mvid, customAppDomainDescriptor, importance);
            string actualMessage = args.Message;

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage, "The message from the parameterized constructor with null optional parameters did not match the expected format.");
        }

        /// <summary>
        /// Tests that multiple accesses to the Message property return the same (cached) result.
        /// </summary>
        [TestMethod]
        public void Message_CalledMultipleTimes_ReturnsSameCachedMessage()
        {
            // Arrange
            AssemblyLoadingContext loadingContext = AssemblyLoadingContext.SdkResolution;
            string loadingInitiator = "Initiator";
            string assemblyName = "AssemblyCache";
            string assemblyPath = "/path/to/assembly.dll";
            Guid mvid = Guid.NewGuid();
            string customAppDomainDescriptor = "CustomDomain";
            MessageImportance importance = MessageImportance.Normal;

            AssemblyLoadBuildEventArgs args = new AssemblyLoadBuildEventArgs(loadingContext, loadingInitiator, assemblyName, assemblyPath, mvid, customAppDomainDescriptor, importance);

            // Act
            string firstCallMessage = args.Message;
            string secondCallMessage = args.Message;

            // Assert
            Assert.AreEqual(firstCallMessage, secondCallMessage, "The Message property did not return a cached result as expected.");
        }
    }
}
