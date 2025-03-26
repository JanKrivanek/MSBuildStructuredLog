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
            // Arrange is done in the constructor.

            // Act
            // Using the already constructed _addItem instance

            // Assert
            // Assuming DisableChildrenCache is accessible and is a bool property.
            Assert.True(_addItem.DisableChildrenCache, "The constructor should set DisableChildrenCache to true.");
        }

        /// <summary>
        /// Tests that the TypeName property returns the correct name "AddItem".
        /// </summary>
        [Fact]
        public void TypeName_WhenCalled_ReturnsAddItem()
        {
            // Act
            string typeName = _addItem.TypeName;

            // Assert
            Assert.Equal("AddItem", typeName);
        }

        /// <summary>
        /// Tests that the LineNumber property can be get and set correctly.
        /// </summary>
        [Fact]
        public void LineNumber_Property_GetSet_WorksCorrectly()
        {
            // Arrange
            int? expectedLineNumber = 42;

            // Act
            _addItem.LineNumber = expectedLineNumber;
            int? actualLineNumber = _addItem.LineNumber;

            // Assert
            Assert.Equal(expectedLineNumber, actualLineNumber);
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
        /// Tests that the ParameterName property can be get and set correctly.
        /// </summary>
        [Fact]
        public void ParameterName_Property_GetSet_WorksCorrectly()
        {
            // Arrange
            string expectedParameterName = "MyParameter";

            // Act
            _taskParameterItem.ParameterName = expectedParameterName;
            string actualParameterName = _taskParameterItem.ParameterName;

            // Assert
            Assert.Equal(expectedParameterName, actualParameterName);
        }

        /// <summary>
        /// Tests that the inherited DisableChildrenCache property is set to true by the base constructor.
        /// </summary>
        [Fact]
        public void InheritedConstructor_WhenCalled_SetsDisableChildrenCacheToTrue()
        {
            // Act & Assert
            Assert.True(_taskParameterItem.DisableChildrenCache, "The base constructor should set DisableChildrenCache to true.");
        }

        /// <summary>
        /// Tests that the inherited TypeName property returns "AddItem".
        /// </summary>
        [Fact]
        public void InheritedTypeName_WhenCalled_ReturnsAddItem()
        {
            // Act
            string typeName = _taskParameterItem.TypeName;

            // Assert
            Assert.Equal("AddItem", typeName);
        }

        /// <summary>
        /// Tests that the inherited LineNumber property can be get and set correctly.
        /// </summary>
        [Fact]
        public void InheritedLineNumber_Property_GetSet_WorksCorrectly()
        {
            // Arrange
            int? expectedLineNumber = 100;

            // Act
            _taskParameterItem.LineNumber = expectedLineNumber;
            int? actualLineNumber = _taskParameterItem.LineNumber;

            // Assert
            Assert.Equal(expectedLineNumber, actualLineNumber);
        }
    }
}
