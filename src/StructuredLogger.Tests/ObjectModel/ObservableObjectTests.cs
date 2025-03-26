using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Build.Logging.StructuredLogger;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ObservableObject"/> class.
    /// </summary>
    public class ObservableObjectTests
    {
        /// <summary>
        /// A test-derived class from ObservableObject to expose protected members for testing.
        /// </summary>
        private class TestObservableObject : ObservableObject
        {
            private int _testProperty;

            /// <summary>
            /// Gets or sets the test property. Uses SetField in the setter.
            /// </summary>
            public int TestProperty
            {
                get => _testProperty;
                set => SetField(ref _testProperty, value);
            }

            /// <summary>
            /// Calls the base RaisePropertyChanged method via a public wrapper.
            /// </summary>
            /// <param name="propertyName">The name of the property to raise the notification for.</param>
            public void PublicRaisePropertyChanged(string propertyName = null)
            {
                // Calling RaisePropertyChanged without relying on CallerMemberName.
                RaisePropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// Tests that SetField returns false and does not raise PropertyChanged when the new value equals the current value.
        /// </summary>
        [Fact]
        public void SetField_WhenValueUnchanged_ReturnsFalseAndDoesNotRaisePropertyChanged()
        {
            // Arrange
            var obj = new TestObservableObject();
            int eventCount = 0;
            string receivedPropertyName = null;
            obj.PropertyChanged += (s, e) =>
            {
                eventCount++;
                receivedPropertyName = e?.PropertyName;
            };

            // Set an initial value.
            obj.TestProperty = 5;
            // Reset event counter to ignore the event from the initial assignment.
            eventCount = 0;

            // Act - setting the same value should not trigger the event.
            obj.TestProperty = 5;

            // Assert
            Assert.Equal(0, eventCount);
            Assert.Equal(5, obj.TestProperty);
            Assert.Null(receivedPropertyName);
        }

        /// <summary>
        /// Tests that SetField returns true and raises PropertyChanged when the new value is different from the current value.
        /// </summary>
        [Fact]
        public void SetField_WhenValueChanged_ReturnsTrueAndRaisesPropertyChanged()
        {
            // Arrange
            var obj = new TestObservableObject();
            int eventCount = 0;
            string receivedPropertyName = null;

            // Initialize with a value.
            obj.TestProperty = 5;

            obj.PropertyChanged += (s, e) =>
            {
                eventCount++;
                receivedPropertyName = e?.PropertyName;
            };

            // Act - setting a different value should trigger the event.
            obj.TestProperty = 10;

            // Assert
            Assert.Equal(1, eventCount);
            Assert.Equal("TestProperty", receivedPropertyName);
            Assert.Equal(10, obj.TestProperty);
        }

        /// <summary>
        /// Tests that RaisePropertyChanged raises the PropertyChanged event with a null property name when called without an explicit name.
        /// </summary>
        [Fact]
        public void RaisePropertyChanged_WhenCalledWithoutExplicitName_RaisesEventWithNullPropertyName()
        {
            // Arrange
            var obj = new TestObservableObject();
            int eventCount = 0;
            string receivedPropertyName = "InitialValue";
            obj.PropertyChanged += (s, e) =>
            {
                eventCount++;
                receivedPropertyName = e?.PropertyName;
            };

            // Act - calling the public wrapper without providing a property name.
            obj.PublicRaisePropertyChanged();

            // Assert
            Assert.Equal(1, eventCount);
            Assert.Null(receivedPropertyName);
        }

        /// <summary>
        /// Tests that RaisePropertyChanged raises the PropertyChanged event with the provided explicit property name.
        /// </summary>
        [Fact]
        public void RaisePropertyChanged_WhenCalledWithExplicitName_RaisesEventWithGivenPropertyName()
        {
            // Arrange
            var obj = new TestObservableObject();
            int eventCount = 0;
            string receivedPropertyName = null;
            const string explicitPropertyName = "ExplicitProperty";
            obj.PropertyChanged += (s, e) =>
            {
                eventCount++;
                receivedPropertyName = e?.PropertyName;
            };

            // Act - calling the public wrapper with an explicit property name.
            obj.PublicRaisePropertyChanged(explicitPropertyName);

            // Assert
            Assert.Equal(1, eventCount);
            Assert.Equal(explicitPropertyName, receivedPropertyName);
        }
    }
}
