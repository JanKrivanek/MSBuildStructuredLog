using FluentAssertions;
using System;
using System.ComponentModel;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ObservableObject"/> class.
    /// </summary>
    public class ObservableObjectTests
    {
        private readonly TestObservableObject _observableObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableObjectTests"/> class.
        /// </summary>
        public ObservableObjectTests()
        {
            _observableObject = new TestObservableObject();
        }

        /// <summary>
        /// Tests the SetField method when the new value is equal to the current value.
        /// The method should not update the field and should not raise the PropertyChanged event.
        /// </summary>
        [Fact]
        public void SetField_WithEqualValue_DoesNotChangeFieldAndDoesNotRaiseEvent()
        {
            // Arrange
            int fieldValue = 10;
            int newValue = 10;
            var eventWasRaised = false;
            _observableObject.PropertyChanged += (sender, e) =>
            {
                eventWasRaised = true;
            };

            // Act
            bool result = _observableObject.TestSetField(ref fieldValue, newValue, "TestProperty");

            // Assert
            result.Should().BeFalse("because the value remains unchanged");
            fieldValue.Should().Be(10, "because the field value should not be updated");
            eventWasRaised.Should().BeFalse("because no change was made hence no event should be raised");
        }

        /// <summary>
        /// Tests the SetField method when the new value is different from the current value.
        /// The method should update the field and raise the PropertyChanged event with the correct property name.
        /// </summary>
        [Fact]
        public void SetField_WithDifferentValue_ChangesFieldAndRaisesEvent()
        {
            // Arrange
            string fieldValue = "OldValue";
            string newValue = "NewValue";
            string? receivedPropertyName = null;
            _observableObject.PropertyChanged += (sender, e) =>
            {
                receivedPropertyName = e.PropertyName;
            };

            // Act
            bool result = _observableObject.TestSetField(ref fieldValue, newValue, "Name");

            // Assert
            result.Should().BeTrue("because the field value has been updated");
            fieldValue.Should().Be(newValue, "because the field should have the new value");
            receivedPropertyName.Should().Be("Name", "because the PropertyChanged event should report the correct property name");
        }

        /// <summary>
        /// Tests the RaisePropertyChanged method to ensure it fires the PropertyChanged event with the provided property name.
        /// </summary>
        [Fact]
        public void RaisePropertyChanged_FiresEvent()
        {
            // Arrange
            string? receivedPropertyName = null;
            _observableObject.PropertyChanged += (sender, e) =>
            {
                receivedPropertyName = e.PropertyName;
            };

            // Act
            _observableObject.TestRaisePropertyChanged("CustomProperty");

            // Assert
            receivedPropertyName.Should().Be("CustomProperty", "because RaisePropertyChanged should raise an event with the correct property name");
        }

        /// <summary>
        /// A derived class to expose protected members of ObservableObject for testing purposes.
        /// </summary>
        private class TestObservableObject : ObservableObject
        {
            /// <summary>
            /// Exposes the protected SetField method for testing.
            /// </summary>
            /// <typeparam name="T">The type of the field value.</typeparam>
            /// <param name="field">Reference to the field.</param>
            /// <param name="newValue">The new value to set.</param>
            /// <param name="propertyName">The name of the property.</param>
            /// <returns>True if the field was updated; otherwise, false.</returns>
            public bool TestSetField<T>(ref T field, T newValue, string propertyName = "TestProperty")
            {
                return SetField(ref field, newValue, propertyName);
            }

            /// <summary>
            /// Exposes the protected RaisePropertyChanged method for testing.
            /// </summary>
            /// <param name="propertyName">The name of the property for which to raise the event.</param>
            public void TestRaisePropertyChanged(string propertyName = "TestProperty")
            {
                RaisePropertyChanged(propertyName);
            }
        }
    }
}
