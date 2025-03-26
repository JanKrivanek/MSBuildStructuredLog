using System;
using System.Reflection;
using System.Security.Policy;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="CustomAppDomainManager"/> class.
    /// </summary>
    public class CustomAppDomainManagerTests
    {
        /// <summary>
        /// Tests the <see cref="CustomAppDomainManager.CreateDomain(string, Evidence, AppDomainSetup)"/> method.
        /// This test invokes the method with valid parameters. Depending on the runtime and host support, 
        /// the method might either return a valid <see cref="AppDomain"/> instance or throw a <see cref="NotSupportedException"/>.
        /// The test ensures that if an instance is returned, it is not null.
        /// </summary>
//         [Fact] [Error] (24-31)CS0246 The type or namespace name 'CustomAppDomainManager' could not be found (are you missing a using directive or an assembly reference?)
//         public void CreateDomain_WithValidParameters_ReturnsAppDomainOrThrowsNotSupportedException()
//         {
//             // Arrange
//             var manager = new CustomAppDomainManager();
//             string friendlyName = "TestDomain";
//             Evidence evidence = new Evidence();
//             var setup = new AppDomainSetup
//             {
//                 ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
//             };
// 
//             // Act & Assert
//             try
//             {
//                 AppDomain result = manager.CreateDomain(friendlyName, evidence, setup);
//                 Assert.NotNull(result);
//             }
//             catch (NotSupportedException notSupportedEx)
//             {
//                 // In some environments the base implementation may throw NotSupportedException.
//                 Assert.IsType<NotSupportedException>(notSupportedEx);
//             }
//         }

        /// <summary>
        /// Tests the <see cref="CustomAppDomainManager.InitializeNewDomain(AppDomainSetup)"/> method when executed 
        /// in the default AppDomain. The implementation should return early without attempting further operations.
        /// This test confirms that the method completes without throwing any exceptions.
        /// </summary>
//         [Fact] [Error] (54-31)CS0246 The type or namespace name 'CustomAppDomainManager' could not be found (are you missing a using directive or an assembly reference?) [Error] (61-36)CS0117 'Record' does not contain a definition for 'Exception'
//         public void InitializeNewDomain_InDefaultAppDomain_DoesNotThrow()
//         {
//             // Arrange
//             var manager = new CustomAppDomainManager();
//             var setup = new AppDomainSetup
//             {
//                 ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
//             };
// 
//             // Act & Assert
//             var exception = Record.Exception(() => manager.InitializeNewDomain(setup));
//             Assert.Null(exception);
//         }
    }
}
