// using System;
// using System.Reflection;
// using System.Security.Policy;
// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
// //     /// <summary> // [Error] (19-28)CS0246 The type or namespace name 'CustomAppDomainManager' could not be found (are you missing a using directive or an assembly reference?)
// //     /// Unit tests for the <see cref="CustomAppDomainManager"/> class.
// //     /// </summary>
// //     public class CustomAppDomainManagerTests
// //     {
// //         private readonly CustomAppDomainManager _manager; // [Error] (15-26)CS0246 The type or namespace name 'CustomAppDomainManager' could not be found (are you missing a using directive or an assembly reference?)
// // 
//         public CustomAppDomainManagerTests()
//         {
//             _manager = new CustomAppDomainManager();
//         }
// 
//         /// <summary>
//         /// Tests the CreateDomain method with valid parameters.
//         /// Verifies that a new AppDomain is created with the specified friendly name.
//         /// </summary>
//         [Fact]
//         public void CreateDomain_WithValidParameters_ReturnsAppDomainWithExpectedFriendlyName()
//         {
//             // Arrange
//             string friendlyName = "TestDomain";
//             var appDomainSetup = new AppDomainSetup
//             {
//                 ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
//             };
//             // Evidence is considered obsolete; passing a new Evidence instance for testing purposes.
//             var evidence = new Evidence();
// 
//             // Act
//             AppDomain result = _manager.CreateDomain(friendlyName, evidence, appDomainSetup);
// 
//             // Assert
//             result.Should().NotBeNull("because a valid AppDomain should be created.");
//             result.FriendlyName.Should().Be(friendlyName, "because the AppDomain's friendly name should match the provided name.");
//         }
// 
//         /// <summary>
//         /// Tests the CreateDomain method when the AppDomainSetup parameter is null.
//         /// Expects an exception since the base implementation should not accept a null AppDomainSetup.
//         /// </summary>
//         [Fact]
//         public void CreateDomain_WithNullAppDomainSetup_ThrowsArgumentNullException()
//         {
//             // Arrange
//             string friendlyName = "TestDomain";
//             var evidence = new Evidence();
//             AppDomainSetup appDomainSetup = null!; // Intentionally null to test exception scenario.
// 
//             // Act
//             Action act = () => _manager.CreateDomain(friendlyName, evidence, appDomainSetup);
// 
//             // Assert
//             act.Should().Throw<ArgumentNullException>("because passing a null AppDomainSetup should not be allowed.");
//         }
// 
//         /// <summary>
//         /// Tests the InitializeNewDomain method in the default AppDomain.
//         /// Since the current domain is the default domain, no notification should occur and no exception thrown.
//         /// </summary>
//         [Fact]
//         public void InitializeNewDomain_InDefaultAppDomain_DoesNotThrowException()
//         {
//             // Arrange
//             var appDomainSetup = new AppDomainSetup
//             {
//                 ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
//             };
// 
//             // Act
//             Action act = () => _manager.InitializeNewDomain(appDomainSetup);
// 
//             // Assert
//             act.Should().NotThrow("because when running in the default AppDomain, the notification should be bypassed.");
//         }
// 
//         /// <summary>
//         /// Partial test for InitializeNewDomain in a non-default AppDomain.
//         /// To fully test this scenario, a new AppDomain must be created and the method executed within that context.
//         /// This test is marked as skipped until such an environment can be set up.
//         /// </summary>
//         [Fact(Skip = "TODO: Create a new AppDomain to test non-default domain behavior including loading the TaskRunner assembly")]
//         public void InitializeNewDomain_InNonDefaultAppDomain_ProcessesNotification()
//         {
//             // Arrange
//             // TODO: Create a new AppDomain, set it as non-default and execute InitializeNewDomain within that context.
//             // The expected behavior would be to attempt to load the "TaskRunner" assembly, get the type "TaskRunner.AppDomainInitializer",
//             // and invoke its default constructor. Mocks and further assertions would be needed if the TaskRunner assembly were available.
//             
//             // Act
//             // _manager.InitializeNewDomain(appDomainSetup);
// 
//             // Assert
//             // Assert the expected side effects or exceptions based on the presence/absence of the TaskRunner assembly.
//         }
//     }
// }