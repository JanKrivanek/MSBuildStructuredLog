using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Security.Policy;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="CustomAppDomainManager"/> class.
    /// </summary>
    [TestClass]
    public class CustomAppDomainManagerTests
    {
        private readonly CustomAppDomainManager _manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAppDomainManagerTests"/> class.
        /// </summary>
        public CustomAppDomainManagerTests()
        {
            _manager = new CustomAppDomainManager();
        }

        /// <summary>
        /// Tests that the CreateDomain method returns a properly configured AppDomain when valid parameters are provided.
        /// It verifies that the returned AppDomain is not null and its FriendlyName matches the supplied friendly name.
        /// After the test, the created AppDomain is unloaded.
        /// </summary>
        [TestMethod]
        public void CreateDomain_WithValidParameters_ReturnsAppDomain()
        {
            // Arrange
            string expectedDomainName = "TestDomain";
            Evidence evidence = new Evidence();
            AppDomainSetup setup = new AppDomainSetup();

            // Act
            AppDomain newDomain = _manager.CreateDomain(expectedDomainName, evidence, setup);

            try
            {
                // Assert
                Assert.IsNotNull(newDomain, "Expected a non-null AppDomain to be returned.");
                Assert.AreEqual(expectedDomainName, newDomain.FriendlyName, "The FriendlyName of the created AppDomain does not match the expected value.");
            }
            finally
            {
                // Clean up: Unload the created AppDomain if it's not the current one.
                if (newDomain != null && !newDomain.IsDefaultAppDomain())
                {
                    AppDomain.Unload(newDomain);
                }
            }
        }

        /// <summary>
        /// Tests that the CreateDomain method throws an ArgumentNullException when the friendlyName parameter is null.
        /// This ensures that the method validates its required parameters.
        /// </summary>
        [TestMethod]
        public void CreateDomain_NullFriendlyName_ThrowsArgumentNullException()
        {
            // Arrange
            Evidence evidence = new Evidence();
            AppDomainSetup setup = new AppDomainSetup();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(
                () => _manager.CreateDomain(null, evidence, setup),
                "Expected ArgumentNullException when passing a null friendlyName.");
        }

        /// <summary>
        /// Tests that the CreateDomain method successfully creates an AppDomain when the securityInfo parameter is null.
        /// The test asserts that an AppDomain is returned and its FriendlyName is set as expected.
        /// </summary>
        [TestMethod]
        public void CreateDomain_NullSecurityInfo_ReturnsAppDomain()
        {
            // Arrange
            string expectedDomainName = "TestDomainWithNullSecurity";
            AppDomainSetup setup = new AppDomainSetup();

            // Act
            AppDomain newDomain = _manager.CreateDomain(expectedDomainName, null, setup);

            try
            {
                // Assert
                Assert.IsNotNull(newDomain, "Expected a non-null AppDomain to be returned even if securityInfo is null.");
                Assert.AreEqual(expectedDomainName, newDomain.FriendlyName, "The FriendlyName of the created AppDomain does not match the expected value.");
            }
            finally
            {
                // Clean up
                if (newDomain != null && !newDomain.IsDefaultAppDomain())
                {
                    AppDomain.Unload(newDomain);
                }
            }
        }

        /// <summary>
        /// Tests that the CreateDomain method successfully creates an AppDomain when the appDomainInfo parameter is null.
        /// The test asserts that an AppDomain is returned and its FriendlyName is set as expected.
        /// </summary>
        [TestMethod]
        public void CreateDomain_NullAppDomainSetup_ReturnsAppDomain()
        {
            // Arrange
            string expectedDomainName = "TestDomainWithNullSetup";
            Evidence evidence = new Evidence();

            // Act
            AppDomain newDomain = _manager.CreateDomain(expectedDomainName, evidence, null);

            try
            {
                // Assert
                Assert.IsNotNull(newDomain, "Expected a non-null AppDomain to be returned even if appDomainInfo is null.");
                Assert.AreEqual(expectedDomainName, newDomain.FriendlyName, "The FriendlyName of the created AppDomain does not match the expected value.");
            }
            finally
            {
                // Clean up
                if (newDomain != null && !newDomain.IsDefaultAppDomain())
                {
                    AppDomain.Unload(newDomain);
                }
            }
        }

        /// <summary>
        /// Tests that the InitializeNewDomain method does not throw an exception when provided with a valid AppDomainSetup.
        /// In the default AppDomain, the NotifyEntrypointAssembly method should exit early, resulting in no further action.
        /// </summary>
        [TestMethod]
        public void InitializeNewDomain_WithValidAppDomainSetup_DoesNotThrow()
        {
            // Arrange
            AppDomainSetup setup = new AppDomainSetup();

            // Act & Assert
            try
            {
                _manager.InitializeNewDomain(setup);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exception when calling InitializeNewDomain with a valid AppDomainSetup, but got: {ex}");
            }
        }

        /// <summary>
        /// Tests that the InitializeNewDomain method does not throw an exception when provided with a null AppDomainSetup.
        /// This confirms that even with null configuration, the initialization procedure handles the input gracefully.
        /// </summary>
        [TestMethod]
        public void InitializeNewDomain_WithNullAppDomainSetup_DoesNotThrow()
        {
            // Act & Assert
            try
            {
                _manager.InitializeNewDomain(null);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exception when calling InitializeNewDomain with a null AppDomainSetup, but got: {ex}");
            }
        }
    }
}
