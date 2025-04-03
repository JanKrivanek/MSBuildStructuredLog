using FluentAssertions;
using global::Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="AddItem"/> class.
    /// </summary>
    public class AddItemTests
    {
        private readonly AddItem _addItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddItemTests"/> class.
        /// </summary>
        public AddItemTests()
        {
            _addItem = new AddItem();
        }

        /// <summary>
        /// Tests that the constructor of <see cref="AddItem"/> sets the DisableChildrenCache property to true.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_SetsDisableChildrenCacheToTrue()
        {
            // Assert
            _addItem.DisableChildrenCache.Should().BeTrue("because the constructor is expected to set DisableChildrenCache to true");
        }

        /// <summary>
        /// Tests that the TypeName property returns the correct name.
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsExpectedTypeName()
        {
            // Act
            string actualTypeName = _addItem.TypeName;

            // Assert
            actualTypeName.Should().Be(nameof(AddItem), "because TypeName should be the name of the class");
        }

        /// <summary>
        /// Tests that the LineNumber property can be set to null.
        /// </summary>
        [Fact]
        public void LineNumber_SetToNull_ReturnsNull()
        {
            // Arrange
            _addItem.LineNumber = null;

            // Act
            int? actualLineNumber = _addItem.LineNumber;

            // Assert
            actualLineNumber.Should().BeNull("because LineNumber was set to null");
        }

        /// <summary>
        /// Tests that the LineNumber property can be set to an ordinary value.
        /// </summary>
        [Fact]
        public void LineNumber_SetToOrdinaryValue_ReturnsSetValue()
        {
            // Arrange
            int expectedLineNumber = 42;
            _addItem.LineNumber = expectedLineNumber;

            // Act
            int? actualLineNumber = _addItem.LineNumber;

            // Assert
            actualLineNumber.Should().Be(expectedLineNumber, "because LineNumber should return the value it was set to");
        }

        /// <summary>
        /// Tests that the LineNumber property accepts int.MinValue.
        /// </summary>
        [Fact]
        public void LineNumber_SetToIntMinValue_ReturnsIntMinValue()
        {
            // Arrange
            _addItem.LineNumber = int.MinValue;

            // Act
            int? actualLineNumber = _addItem.LineNumber;

            // Assert
            actualLineNumber.Should().Be(int.MinValue, "because LineNumber should accept boundary integer values such as int.MinValue");
        }

        /// <summary>
        /// Tests that the LineNumber property accepts int.MaxValue.
        /// </summary>
        [Fact]
        public void LineNumber_SetToIntMaxValue_ReturnsIntMaxValue()
        {
            // Arrange
            _addItem.LineNumber = int.MaxValue;

            // Act
            int? actualLineNumber = _addItem.LineNumber;

            // Assert
            actualLineNumber.Should().Be(int.MaxValue, "because LineNumber should accept boundary integer values such as int.MaxValue");
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="TaskParameterItem"/> class.
    /// </summary>
    public class TaskParameterItemTests
    {
        private readonly TaskParameterItem _taskParameterItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskParameterItemTests"/> class.
        /// </summary>
        public TaskParameterItemTests()
        {
            _taskParameterItem = new TaskParameterItem();
        }

        /// <summary>
        /// Tests that the constructor of <see cref="TaskParameterItem"/> inherits the DisableChildrenCache property set to true.
        /// </summary>
        [Fact]
        public void Constructor_WhenCalled_InheritsDisableChildrenCacheSetToTrue()
        {
            // Assert
            _taskParameterItem.DisableChildrenCache.Should().BeTrue("because the base constructor of AddItem sets DisableChildrenCache to true");
        }

        /// <summary>
        /// Tests that the TypeName property inherited from AddItem returns the correct type name.
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsExpectedTypeName()
        {
            // Act
            string actualTypeName = _taskParameterItem.TypeName;

            // Assert
            actualTypeName.Should().Be(nameof(AddItem), "because TaskParameterItem inherits the TypeName implementation from AddItem");
        }

        /// <summary>
        /// Tests that the ParameterName property can be set and retrieved correctly.
        /// </summary>
        /// <param name="parameterName">The parameter name value to test.</param>
        [Theory]
        [InlineData("Parameter1")]
        [InlineData("")]
        [InlineData("!@#$%^&*()_+")]
        [InlineData("A very long parameter name that exceeds normal lengths to test boundary conditions in string handling scenarios")]
        public void ParameterName_SetAndGet_ReturnsExpectedValue(string parameterName)
        {
            // Arrange
            _taskParameterItem.ParameterName = parameterName;

            // Act
            string actualParameterName = _taskParameterItem.ParameterName;

            // Assert
            actualParameterName.Should().Be(parameterName, "because ParameterName should return the value it was set to");
        }
    }
}
