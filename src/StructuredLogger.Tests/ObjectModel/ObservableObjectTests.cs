using System;
using System.ComponentModel;
using FluentAssertions;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// A test subclass of ObservableObject to expose protected members for testing.
    /// </summary>
    public class TestObservableObject : ObservableObject
    {
        /// <summary>
        /// Exposes the protected SetField method as public for testing.
        /// </summary>
        /// <typeparam name="T">Type of the field.</typeparam>
        /// <param name="field">Reference to the field to update.</param>
        /// <param name="newValue">New value for the field.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>True if the field was updated; otherwise, false.</returns>
        public bool PublicSetField<T>(ref T field, T newValue, string propertyName = null)
        {
            return SetField(ref field, newValue, propertyName);
        }

        /// <summary>
        /// Exposes the protected RaisePropertyChanged method as public for testing.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void PublicRaisePropertyChanged(string propertyName = null)
        {
            RaisePropertyChanged(propertyName);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="ObservableObject"/> class via the <see cref="TestObservableObject"/> subclass.
    /// </summary>
    public class ObservableObjectTests
    {
        private readonly TestObservableObject _observable;

        public ObservableObjectTests()
        {
            _observable = new TestObservableObject();
        }

        /// <summary>
        /// Tests that calling PublicSetField with a value equal to the existing field does not update the field and does not raise the PropertyChanged event.
        /// </summary>
        /// <param name="initialValue">The initial field value to use.</param>
        [Theory]
        [InlineData(10)]
        [InlineData(0)]
        [InlineData(-5)]
        public void SetField_WhenValueIsEqual_DoesNotUpdateFieldAndDoesNotRaisePropertyChanged(int initialValue)
        {
            // Arrange
            int fieldValue = initialValue;
            bool eventRaised = false;
            _observable.PropertyChanged += (sender, args) => eventRaised = true;

            // Act
            bool result = _observable.PublicSetField(ref fieldValue, initialValue, "TestProperty");

            // Assert
            result.Should().BeFalse("because the new value is equal to the existing field value");
            fieldValue.Should().Be(initialValue, "because the field should not have been updated");
            eventRaised.Should().BeFalse("because PropertyChanged event should not be raised when the value is unchanged");
        }

        /// <summary>
        /// Tests that calling PublicSetField with a value different from the existing field updates the field and raises the PropertyChanged event with the correct property name.
        /// </summary>
        [Fact]
        public void SetField_WhenValueIsDifferent_UpdatesFieldAndRaisesPropertyChanged()
        {
            // Arrange
            int fieldValue = 10;
            int newValue = 20;
            string expectedPropertyName = "SampleProperty";
            string actualPropertyName = null;
            _observable.PropertyChanged += (sender, args) => actualPropertyName = args.PropertyName;

            // Act
            bool result = _observable.PublicSetField(ref fieldValue, newValue, expectedPropertyName);

            // Assert
            result.Should().BeTrue("because the new value is different from the existing field value");
            fieldValue.Should().Be(newValue, "because the field should have been updated to the new value");
            actualPropertyName.Should().Be(expectedPropertyName, "because the PropertyChanged event should be raised with the correct property name");
        }

        /// <summary>
        /// Tests that calling PublicSetField with a reference type when both the current and new values are null does not update the field or raise the PropertyChanged event.
        /// </summary>
        [Fact]
        public void SetField_WhenReferenceTypeAndBothNull_DoesNotUpdateFieldAndDoesNotRaisePropertyChanged()
        {
            // Arrange
            string fieldValue = null;
            bool eventRaised = false;
            _observable.PropertyChanged += (sender, args) => eventRaised = true;

            // Act
            bool result = _observable.PublicSetField(ref fieldValue, null, "NullableProperty");

            // Assert
            result.Should().BeFalse("because both the existing field and new value are null");
            fieldValue.Should().BeNull("because the field remains null");
            eventRaised.Should().BeFalse("because no change occurred, so no event should be raised");
        }

        /// <summary>
        /// Tests that calling PublicSetField with a reference type when the current value is null and the new value is not null updates the field and raises the PropertyChanged event.
        /// </summary>
        [Fact]
        public void SetField_WhenReferenceTypeFieldIsNullAndNewValueIsNotNull_UpdatesFieldAndRaisesPropertyChanged()
        {
            // Arrange
            string fieldValue = null;
            string newValue = "UpdatedValue";
            string expectedPropertyName = "NullableProperty";
            string actualPropertyName = null;
            _observable.PropertyChanged += (sender, args) => actualPropertyName = args.PropertyName;

            // Act
            bool result = _observable.PublicSetField(ref fieldValue, newValue, expectedPropertyName);

            // Assert
            result.Should().BeTrue("because the new non-null value is different from the null field");
            fieldValue.Should().Be(newValue, "because the field should be updated to the new non-null value");
            actualPropertyName.Should().Be(expectedPropertyName, "because the PropertyChanged event should be raised with the correct property name");
        }

        /// <summary>
        /// Tests that PublicRaisePropertyChanged raises the PropertyChanged event with the provided property name.
        /// </summary>
        [Fact]
        public void RaisePropertyChanged_WithValidPropertyName_RaisesPropertyChangedEvent()
        {
            // Arrange
            string expectedPropertyName = "TestProperty";
            string actualPropertyName = null;
            _observable.PropertyChanged += (sender, args) => actualPropertyName = args.PropertyName;

            // Act
            _observable.PublicRaisePropertyChanged(expectedPropertyName);

            // Assert
            actualPropertyName.Should().Be(expectedPropertyName, "because the PropertyChanged event should be raised with the provided property name");
        }

        /// <summary>
        /// Tests that calling PublicRaisePropertyChanged with a null property name raises the event with a null value and does not throw an exception.
        /// </summary>
        [Fact]
        public void RaisePropertyChanged_WithNullPropertyName_RaisesPropertyChangedEventWithNull()
        {
            // Arrange
            string expectedPropertyName = null;
            string actualPropertyName = "InitialValue";
            _observable.PropertyChanged += (sender, args) => actualPropertyName = args.PropertyName;

            // Act
            _observable.PublicRaisePropertyChanged(expectedPropertyName);

            // Assert
            actualPropertyName.Should().BeNull("because the PropertyChanged event should be raised with a null property name when none is provided");
        }

        /// <summary>
        /// Tests that calling PublicRaisePropertyChanged when there are no subscribers does not throw any exception.
        /// </summary>
        [Fact]
        public void RaisePropertyChanged_NoSubscribers_DoesNotThrow()
        {
            // Arrange
            var observableWithoutSubscribers = new TestObservableObject();

            // Act
            Action act = () => observableWithoutSubscribers.PublicRaisePropertyChanged("NoSubscriber");

            // Assert
            act.Should().NotThrow("because raising PropertyChanged with no subscribers should not throw an exception");
        }
    }
}
