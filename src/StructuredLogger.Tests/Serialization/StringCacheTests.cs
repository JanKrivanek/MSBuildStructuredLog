using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Logging.StructuredLogger;
using Moq;
using Xunit;

namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="StringCache"/> class.
    /// </summary>
    public class StringCacheTests
    {
        private readonly StringCache _stringCache;

        public StringCacheTests()
        {
            _stringCache = new StringCache();
            // Disable normalization to avoid dependency on external extension implementation.
            _stringCache.NormalizeLineEndings = false;
        }

        /// <summary>
        /// Tests that the constructor initializes Instances as an empty collection.
        /// </summary>
        [Fact]
        public void Constructor_InitialInstancesEmpty()
        {
            // Act
            var instances = _stringCache.Instances;

            // Assert
            Assert.Empty(instances);
        }

        /// <summary>
        /// Tests that Intern returns null when provided with a null string.
        /// </summary>
//         [Fact] [Error] (44-39)CS0121 The call is ambiguous between the following methods or properties: 'StringCache.Intern(IEnumerable<string>)' and 'StringCache.Intern(string)'
//         public void Intern_StringNull_ReturnsNull()
//         {
//             // Act
//             var result = _stringCache.Intern(null);
// 
//             // Assert
//             Assert.Null(result);
//         }

        /// <summary>
        /// Tests that Intern returns an empty string when provided with an empty string.
        /// </summary>
        [Fact]
        public void Intern_EmptyString_ReturnsEmptyString()
        {
            // Act
            var result = _stringCache.Intern(string.Empty);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        /// <summary>
        /// Tests that Intern returns the input string without interning when DisableDeduplication is true.
        /// </summary>
        [Fact]
        public void Intern_DisableDeduplicationTrue_ReturnsSameInput()
        {
            // Arrange
            _stringCache.DisableDeduplication = true;
            string input = "TestString";

            // Act
            var result = _stringCache.Intern(input);

            // Assert
            Assert.Equal(input, result);
        }

        /// <summary>
        /// Tests that Intern interns a valid string and returns the same reference on subsequent calls.
        /// </summary>
        [Fact]
        public void Intern_ValidString_ReturnsInternedValue()
        {
            // Arrange
            _stringCache.DisableDeduplication = false;
            string input = "UniqueString";

            // Act
            var firstCall = _stringCache.Intern(input);
            var secondCall = _stringCache.Intern(input);

            // Assert
            Assert.Same(firstCall, secondCall);
        }

        /// <summary>
        /// Tests that Contains returns false for a string that has not been interned.
        /// </summary>
        [Fact]
        public void Contains_StringNotInterned_ReturnsFalse()
        {
            // Arrange
            string input = "NonInterned";

            // Act
            bool exists = _stringCache.Contains(input);

            // Assert
            Assert.False(exists);
        }

        /// <summary>
        /// Tests that Contains returns true for a string that has been interned.
        /// </summary>
        [Fact]
        public void Contains_StringInterned_ReturnsTrue()
        {
            // Arrange
            string input = "Interned";
            _stringCache.DisableDeduplication = false;
            _stringCache.Intern(input);

            // Act
            bool exists = _stringCache.Contains(input);

            // Assert
            Assert.True(exists);
        }

        /// <summary>
        /// Tests that Intern(IEnumerable&lt;string&gt;) interns multiple strings correctly.
        /// </summary>
        [Fact]
        public void InternEnumerable_InsertsMultipleStrings_CanBeContained()
        {
            // Arrange
            _stringCache.DisableDeduplication = false;
            var inputs = new List<string> { "One", "Two", "Three" };

            // Act
            _stringCache.Intern(inputs);

            // Assert
            foreach (var item in inputs)
            {
                Assert.True(_stringCache.Contains(item));
            }
        }

        /// <summary>
        /// Tests that SetStrings assigns the provided string collection to Instances and disables deduplication.
        /// </summary>
        [Fact]
        public void SetStrings_SetsInstancesAndDisablesDeduplication()
        {
            // Arrange
            var newInstances = new List<string> { "A", "B", "C" };

            // Act
            _stringCache.SetStrings(newInstances);

            // Assert
            Assert.Equal(newInstances, _stringCache.Instances);
            Assert.True(_stringCache.DisableDeduplication);
        }

        /// <summary>
        /// Tests that Seal transforms the interned strings into an array with an empty string as the first element, and disables deduplication.
        /// </summary>
        [Fact]
        public void Seal_SetsInstancesArrayWithEmptyStringPrefixAndDisablesDeduplication()
        {
            // Arrange
            _stringCache.DisableDeduplication = false;
            _stringCache.Intern("Alpha");
            _stringCache.Intern("Beta");

            // Act
            _stringCache.Seal();
            var instancesArray = _stringCache.Instances.ToArray();

            // Assert
            Assert.True(_stringCache.DisableDeduplication);
            Assert.NotNull(instancesArray);
            Assert.True(instancesArray.Length >= 3);
            // Confirm first element is an empty string.
            Assert.Equal(string.Empty, instancesArray[0]);
            // Confirm that interned strings are present.
            Assert.Contains("Alpha", instancesArray);
            Assert.Contains("Beta", instancesArray);
        }

        /// <summary>
        /// Tests that SoftIntern returns the original string when HasDeduplicatedStrings is true.
        /// </summary>
        [Fact]
        public void SoftIntern_HasDeduplicatedStringsTrue_ReturnsInputUnchanged()
        {
            // Arrange
            string input = "SoftTest";
            _stringCache.HasDeduplicatedStrings = true;

            // Act
            var result = _stringCache.SoftIntern(input);

            // Assert
            Assert.Equal(input, result);
        }

        /// <summary>
        /// Tests that SoftIntern interns the string when HasDeduplicatedStrings is false.
        /// </summary>
        [Fact]
        public void SoftIntern_HasDeduplicatedStringsFalse_ReturnsInternedValue()
        {
            // Arrange
            string input = "SoftTest";
            _stringCache.HasDeduplicatedStrings = false;
            _stringCache.DisableDeduplication = false;

            // Act
            var softResult = _stringCache.SoftIntern(input);
            var internResult = _stringCache.Intern(input);

            // Assert
            Assert.Same(softResult, internResult);
        }

        /// <summary>
        /// Tests that InternStringDictionary returns null when provided a null dictionary.
        /// </summary>
        [Fact]
        public void InternStringDictionary_NullInput_ReturnsNull()
        {
            // Act
            var result = _stringCache.InternStringDictionary(null);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that InternStringDictionary returns the same instance for an empty dictionary.
        /// </summary>
        [Fact]
        public void InternStringDictionary_EmptyDictionary_ReturnsSameInstance()
        {
            // Arrange
            var emptyDictionary = new Dictionary<string, string>();

            // Act
            var result = _stringCache.InternStringDictionary(emptyDictionary);

            // Assert
            Assert.Same(emptyDictionary, result);
        }

        /// <summary>
        /// Tests that InternStringDictionary interns both keys and values of a non-empty dictionary.
        /// </summary>
        [Fact]
        public void InternStringDictionary_WithContent_InternsKeysAndValues()
        {
            // Arrange
            _stringCache.DisableDeduplication = false;
            var inputDictionary = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

            // Act
            var result = _stringCache.InternStringDictionary(inputDictionary);

            // Assert
            Assert.NotSame(inputDictionary, result);
            foreach (var kvp in inputDictionary)
            {
                var internedKey = _stringCache.Intern(kvp.Key);
                var internedValue = _stringCache.Intern(kvp.Value);
                Assert.True(result.ContainsKey(internedKey));
                Assert.Equal(internedValue, result[internedKey]);
            }
        }

        /// <summary>
        /// Tests that InternList returns null when provided a null list.
        /// </summary>
        [Fact]
        public void InternList_NullInput_ReturnsNull()
        {
            // Act
            var result = _stringCache.InternList(null);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that InternList returns the same instance for an empty list.
        /// </summary>
        [Fact]
        public void InternList_EmptyList_ReturnsSameInstance()
        {
            // Arrange
            var emptyList = new List<string>();

            // Act
            var result = _stringCache.InternList(emptyList);

            // Assert
            Assert.Same(emptyList, result);
        }

        /// <summary>
        /// Tests that InternList interns the strings within a non-empty list and interns duplicates correctly.
        /// </summary>
        [Fact]
        public void InternList_WithContent_ReturnsInternedList()
        {
            // Arrange
            _stringCache.DisableDeduplication = false;
            var inputList = new List<string> { "Item1", "Item2", "Item1" };

            // Act
            var result = _stringCache.InternList(inputList);

            // Assert
            Assert.NotSame(inputList, result);
            Assert.Equal(inputList.Count, result.Count);
            var internedItem1 = _stringCache.Intern("Item1");
            int occurrenceCount = result.Count(item => object.ReferenceEquals(item, internedItem1));
            Assert.Equal(2, occurrenceCount);
        }
    }
}
