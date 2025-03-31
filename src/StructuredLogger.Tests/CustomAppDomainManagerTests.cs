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
//         /// Tests that CreateDomain returns a valid AppDomain with the specified friendly name when valid parameters are provided.
//         /// Arrange: Set up a friendly name, an empty Evidence object, and a new AppDomainSetup instance.
//         /// Act: Call CreateDomain using the manager.
//         /// Assert: Verify that the returned AppDomain is not null and its FriendlyName matches the supplied value.
//         /// </summary>
//         [Fact]
//         public void CreateDomain_WithValidParameters_ReturnsAppDomainWithExpectedFriendlyName()
//         {
//             // Arrange
//             string friendlyName = "TestDomain";
//             Evidence evidence = new Evidence();
//             AppDomainSetup setup = new AppDomainSetup();
// 
//             // Act
//             AppDomain newDomain = _manager.CreateDomain(friendlyName, evidence, setup);
// 
//             // Assert
//             newDomain.Should().NotBeNull();
//             newDomain.FriendlyName.Should().Be(friendlyName);
//         }
// 
//         /// <summary>
//         /// Tests that CreateDomain throws an ArgumentNullException when the friendlyName parameter is null.
//         /// Arrange: Prepare a null friendlyName, Evidence, and AppDomainSetup.
//         /// Act: Invoke CreateDomain and capture the exception.
//         /// Assert: Verify that an ArgumentNullException is thrown.
//         /// </summary>
//         [Fact]
//         public void CreateDomain_WithNullFriendlyName_ThrowsArgumentNullException()
//         {
//             // Arrange
//             string? friendlyName = null;
//             Evidence evidence = new Evidence();
//             AppDomainSetup setup = new AppDomainSetup();
// 
//             // Act
//             Action act = () => _manager.CreateDomain(friendlyName!, evidence, setup);
// 
//             // Assert
//             act.Should().Throw<ArgumentNullException>();
//         }
// 
//         /// <summary>
//         /// Tests that InitializeNewDomain method does not throw an exception when invoked in the default AppDomain.
//         /// Arrange: Create a new AppDomainSetup instance.
//         /// Act: Invoke InitializeNewDomain on the manager.
//         /// Assert: Verify that no exception is thrown.
//         /// </summary>
//         [Fact]
//         public void InitializeNewDomain_InDefaultAppDomain_DoesNotThrowException()
//         {
//             // Arrange
//             AppDomainSetup setup = new AppDomainSetup();
// 
//             // Act
//             Action act = () => _manager.InitializeNewDomain(setup);
// 
//             // Assert
//             act.Should().NotThrow();
//         }
//     }
// }