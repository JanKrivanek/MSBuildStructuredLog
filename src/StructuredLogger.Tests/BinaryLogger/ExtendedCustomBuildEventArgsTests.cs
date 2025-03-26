using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using System;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Build.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref = "ExtendedCustomBuildEventArgs"/> class.
    /// </summary>
    public class ExtendedCustomBuildEventArgsTests
    {
        /// <summary>
        /// Tests that the internal default constructor sets ExtendedType to "undefined".
        /// This is done via a wrapper class that exposes the internal default constructor.
        /// Expected outcome: ExtendedType equals "undefined".
        /// </summary>
//         [Fact] [Error] (25-48)CS1061 'ExtendedCustomBuildEventArgsWrapper' does not contain a definition for 'ExtendedType' and no accessible extension method 'ExtendedType' accepting a first argument of type 'ExtendedCustomBuildEventArgsWrapper' could be found (are you missing a using directive or an assembly reference?)
//         public void DefaultConstructor_SetsExtendedTypeToUndefined()
//         {
//             // Arrange & Act
//             var instance = new ExtendedCustomBuildEventArgsWrapper();
//             // Assert
//             Assert.Equal("undefined", instance.ExtendedType);
//         }

        /// <summary>
        /// Tests that the constructor with the "type" parameter sets the ExtendedType property correctly.
        /// Expected outcome: ExtendedType equals the provided type value.
        /// </summary>
        /// <param name = "type">The type value to be set.</param>
        [Theory]
        [InlineData("CustomType")]
        [InlineData("")]
        [InlineData(null)]
        public void ConstructorWithType_SetsExtendedType(string type)
        {
            // Arrange & Act
            var instance = new ExtendedCustomBuildEventArgs(type);
            // Assert
            Assert.Equal(type, instance.ExtendedType);
        }

        /// <summary>
        /// Tests that the constructor with the type, message, helpKeyword, and senderName parameters sets the properties accordingly.
        /// Expected outcome: ExtendedType, Message, HelpKeyword, and SenderName equal the provided values.
        /// </summary>
        /// <param name = "type">The extended type.</param>
        /// <param name = "message">The message text.</param>
        /// <param name = "helpKeyword">The help keyword.</param>
        /// <param name = "senderName">The sender name.</param>
        [Theory]
        [InlineData("TypeA", "Test message", "HelpKey", "SenderName")]
        [InlineData("TypeB", null, null, null)]
        public void ConstructorWithTypeMessageHelpSender_SetsProperties(string type, string message, string helpKeyword, string senderName)
        {
            // Arrange & Act
            var instance = new ExtendedCustomBuildEventArgs(type, message, helpKeyword, senderName);
            // Assert
            Assert.Equal(type, instance.ExtendedType);
            Assert.Equal(message, instance.Message);
            Assert.Equal(helpKeyword, instance.HelpKeyword);
            Assert.Equal(senderName, instance.SenderName);
        }

        /// <summary>
        /// Tests that the constructor with the type, message, helpKeyword, senderName, and eventTimestamp parameters sets the properties including the timestamp.
        /// Expected outcome: Properties and the Timestamp equal the provided values.
        /// </summary>
        [Fact]
        public void ConstructorWithTimestamp_SetsProperties()
        {
            // Arrange
            string type = "TypeC";
            string message = "MessageC";
            string helpKeyword = "HelpC";
            string senderName = "SenderC";
            DateTime timestamp = DateTime.Now;
            // Act
            var instance = new ExtendedCustomBuildEventArgs(type, message, helpKeyword, senderName, timestamp);
            // Assert
            Assert.Equal(type, instance.ExtendedType);
            Assert.Equal(message, instance.Message);
            Assert.Equal(helpKeyword, instance.HelpKeyword);
            Assert.Equal(senderName, instance.SenderName);
            Assert.Equal(timestamp, instance.Timestamp);
        }

        /// <summary>
        /// Tests that the constructor with the type, message, helpKeyword, senderName, eventTimestamp, and message arguments sets the properties.
        /// The test verifies that ExtendedType and base class properties are set as expected.
        /// Expected outcome: Properties equal the provided values.
        /// </summary>
        [Fact]
        public void ConstructorWithMessageArgs_SetsProperties()
        {
            // Arrange
            string type = "TypeD";
            string message = "MessageD";
            string helpKeyword = "HelpD";
            string senderName = "SenderD";
            DateTime timestamp = DateTime.UtcNow;
            object[] messageArgs = new object[]
            {
                "arg1",
                123
            };
            // Act
            var instance = new ExtendedCustomBuildEventArgs(type, message, helpKeyword, senderName, timestamp, messageArgs);
            // Assert
            Assert.Equal(type, instance.ExtendedType);
            Assert.Equal(message, instance.Message);
            Assert.Equal(helpKeyword, instance.HelpKeyword);
            Assert.Equal(senderName, instance.SenderName);
            Assert.Equal(timestamp, instance.Timestamp);
        // Note: No explicit verification of messageArgs processing as it is handled by the base constructor.
        }

        /// <summary>
        /// Tests that the properties ExtendedData and ExtendedMetadata can be set and retrieved correctly.
        /// Expected outcome: Getters return the values that were set.
        /// </summary>
        [Fact]
        public void PropertySetters_Getters_WorkAsExpected()
        {
            // Arrange
            var instance = new ExtendedCustomBuildEventArgs("TestType");
            string expectedExtendedData = "SampleData";
            IDictionary<string, string?> metadata = new Dictionary<string, string?>
            {
                {
                    "Key",
                    "Value"
                }
            };
            // Act
            instance.ExtendedData = expectedExtendedData;
            instance.ExtendedMetadata = metadata;
            instance.ExtendedType = "NewType";
            // Assert
            Assert.Equal(expectedExtendedData, instance.ExtendedData);
            Assert.Same(metadata, instance.ExtendedMetadata);
            Assert.Equal("NewType", instance.ExtendedType);
        }

        /// <summary>
        /// Tests that setting the ExtendedMetadata property to null returns null when retrieved.
        /// Expected outcome: ExtendedMetadata is null.
        /// </summary>
        [Fact]
        public void ExtendedMetadata_SetToNull_ReturnsNull()
        {
            // Arrange
            var instance = new ExtendedCustomBuildEventArgs("TestType");
            // Act
            instance.ExtendedMetadata = null;
            // Assert
            Assert.Null(instance.ExtendedMetadata);
        }

        /// <summary>
        /// Tests that setting the ExtendedData property to null returns null when retrieved.
        /// Expected outcome: ExtendedData is null.
        /// </summary>
        [Fact]
        public void ExtendedData_SetToNull_ReturnsNull()
        {
            // Arrange
            var instance = new ExtendedCustomBuildEventArgs("TestType");
            // Act
            instance.ExtendedData = null;
            // Assert
            Assert.Null(instance.ExtendedData);
        }
    }

    /// <summary>
    /// Exposes the internal default constructor of ExtendedCustomBuildEventArgs for testing purposes.
    /// This subclass is declared internal to mimic usage of the default constructor.
    /// </summary>
//     internal class ExtendedCustomBuildEventArgsWrapper : ExtendedCustomBuildEventArgs [Error] (182-58)CS0509 'ExtendedCustomBuildEventArgsWrapper': cannot derive from sealed type 'ExtendedCustomBuildEventArgs' [Error] (184-56)CS1729 'object' does not contain a constructor that takes 1 arguments
//     {
//         public ExtendedCustomBuildEventArgsWrapper() : base("undefined")
//         {
//         }
//     }
}