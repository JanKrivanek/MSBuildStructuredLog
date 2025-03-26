using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Build.Collections.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ArrayDictionary{TKey, TValue}"/> class.
    /// </summary>
    [TestClass]
    public class ArrayDictionaryTests
    {
        private readonly int _capacity = 5;
        private readonly string _testKey1 = "key1";
        private readonly string _testKey2 = "key2";
        private readonly int _testValue1 = 10;
        private readonly int _testValue2 = 20;

        /// <summary>
        /// Tests the constructor to ensure that the internal arrays are initialized with the specified capacity and count is zero.
        /// </summary>
        [TestMethod]
        public void Constructor_WhenValidCapacity_InitializesArrays()
        {
            // Arrange & Act
            var dictionary = new ArrayDictionary<string, int>(_capacity);

            // Assert
            Assert.IsNotNull(dictionary.KeyArray, "KeyArray should not be null.");
            Assert.IsNotNull(dictionary.ValueArray, "ValueArray should not be null.");
            Assert.AreEqual(_capacity, dictionary.KeyArray.Length, "KeyArray length does not match capacity.");
            Assert.AreEqual(_capacity, dictionary.ValueArray.Length, "ValueArray length does not match capacity.");
            Assert.AreEqual(0, dictionary.Count, "Count should be zero initially.");
        }

        /// <summary>
        /// Tests the static Create method to ensure it returns a new instance with the specified capacity.
        /// </summary>
        [TestMethod]
        public void Create_WhenCalled_ReturnsNewInstanceWithSpecifiedCapacity()
        {
            // Arrange & Act
            IDictionary<string, int> dictionary = ArrayDictionary<string, int>.Create(_capacity);

            // Assert
            Assert.IsNotNull(dictionary, "Create method should return a non-null instance.");
            Assert.IsInstanceOfType(dictionary, typeof(ArrayDictionary<string, int>), "Instance should be of type ArrayDictionary.");
            var instance = dictionary as ArrayDictionary<string, int>;
            Assert.AreEqual(_capacity, instance.KeyArray.Length, "KeyArray length does not match capacity from Create method.");
        }

        /// <summary>
        /// Tests the indexer getter when key is not present, ensuring it returns the default value of TValue.
        /// </summary>
        [TestMethod]
        public void IndexerGet_WhenKeyNotFound_ReturnsDefaultValue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);

            // Act
            int value = dictionary[_testKey1];

            // Assert
            Assert.AreEqual(default(int), value, "Indexer getter should return default value when key is not found.");
        }

        /// <summary>
        /// Tests the indexer setter to update the value when the key already exists.
        /// </summary>
        [TestMethod]
        public void IndexerSet_WhenKeyExists_UpdatesValue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add(_testKey1, _testValue1);

            // Act
            dictionary[_testKey1] = _testValue2;

            // Assert
            bool found = dictionary.TryGetValue(_testKey1, out int value);
            Assert.IsTrue(found, "Key should be found after update.");
            Assert.AreEqual(_testValue2, value, "Indexer setter should update the value.");
        }

        /// <summary>
        /// Tests the indexer setter to add a new key/value pair when the key does not already exist.
        /// </summary>
        [TestMethod]
        public void IndexerSet_WhenKeyNotFound_AddsNewKeyValuePair()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);

            // Act
            dictionary[_testKey1] = _testValue1;

            // Assert
            Assert.IsTrue(dictionary.ContainsKey(_testKey1), "Dictionary should contain the new key.");
            Assert.AreEqual(1, dictionary.Count, "Count should be 1 after adding a new key.");
            bool found = dictionary.TryGetValue(_testKey1, out int value);
            Assert.IsTrue(found, "Key should be retrievable after being added.");
            Assert.AreEqual(_testValue1, value, "Value should match the one that was set.");
        }

        /// <summary>
        /// Tests the Add method to verify it adds a key/value pair when within capacity.
        /// </summary>
        [TestMethod]
        public void Add_WhenCalledWithinCapacity_AddsKeyValuePair()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);

            // Act
            dictionary.Add(_testKey1, _testValue1);

            // Assert
            Assert.AreEqual(1, dictionary.Count, "Count should be incremented after adding an element.");
            bool found = dictionary.TryGetValue(_testKey1, out int value);
            Assert.IsTrue(found, "Added key should be found in dictionary.");
            Assert.AreEqual(_testValue1, value, "Value of the added key should match the expected value.");
        }

        /// <summary>
        /// Tests the Add method to verify that adding beyond capacity throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        public void Add_WhenCalledBeyondCapacity_ThrowsInvalidOperationException()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(2);
            dictionary.Add("one", 1);
            dictionary.Add("two", 2);

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => dictionary.Add("three", 3), "Add should throw InvalidOperationException when capacity is exceeded.");
        }

        /// <summary>
        /// Tests the Add method overload that accepts a KeyValuePair to add an element within capacity.
        /// </summary>
        [TestMethod]
        public void AddKeyValuePair_WhenCalledWithinCapacity_AddsElement()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            var kvp = new KeyValuePair<string, int>(_testKey1, _testValue1);

            // Act
            dictionary.Add(kvp);

            // Assert
            Assert.AreEqual(1, dictionary.Count, "Count should be incremented after adding KeyValuePair.");
            bool found = dictionary.TryGetValue(_testKey1, out int value);
            Assert.IsTrue(found, "Key from KeyValuePair should be found in dictionary.");
            Assert.AreEqual(_testValue1, value, "Value from KeyValuePair should match expected value.");
        }

        /// <summary>
        /// Tests the Clear method to ensure it throws a NotImplementedException.
        /// </summary>
        [TestMethod]
        public void Clear_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);

            // Act & Assert
            Assert.ThrowsException<NotImplementedException>(() => dictionary.Clear(), "Clear should throw NotImplementedException.");
        }

        /// <summary>
        /// Tests the Contains method to verify it returns true when the specified KeyValuePair exists.
        /// </summary>
        [TestMethod]
        public void Contains_WhenCalledWithExistingPair_ReturnsTrue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add(_testKey1, _testValue1);
            var kvp = new KeyValuePair<string, int>(_testKey1, _testValue1);

            // Act
            bool contains = dictionary.Contains(kvp);

            // Assert
            Assert.IsTrue(contains, "Contains should return true for an existing key/value pair.");
        }

        /// <summary>
        /// Tests the Contains method to verify it returns false when the specified KeyValuePair does not exist.
        /// </summary>
        [TestMethod]
        public void Contains_WhenCalledWithNonExistingPair_ReturnsFalse()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add(_testKey1, _testValue1);
            var kvp = new KeyValuePair<string, int>(_testKey1, _testValue2);

            // Act
            bool contains = dictionary.Contains(kvp);

            // Assert
            Assert.IsFalse(contains, "Contains should return false when the value does not match.");
        }

        /// <summary>
        /// Tests the ContainsKey method to verify it returns true when the key exists.
        /// </summary>
        [TestMethod]
        public void ContainsKey_WhenKeyExists_ReturnsTrue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add(_testKey1, _testValue1);

            // Act
            bool containsKey = dictionary.ContainsKey(_testKey1);

            // Assert
            Assert.IsTrue(containsKey, "ContainsKey should return true for an existing key.");
        }

        /// <summary>
        /// Tests the ContainsKey method to verify it returns false when the key does not exist.
        /// </summary>
        [TestMethod]
        public void ContainsKey_WhenKeyDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);

            // Act
            bool containsKey = dictionary.ContainsKey(_testKey1);

            // Assert
            Assert.IsFalse(containsKey, "ContainsKey should return false for a non-existent key.");
        }

        /// <summary>
        /// Tests the CopyTo method to ensure it copies key/value pairs to the provided array starting at the specified index.
        /// </summary>
        [TestMethod]
        public void CopyTo_WhenCalledWithValidParameters_CopiesElementsToArray()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add(_testKey1, _testValue1);
            dictionary.Add(_testKey2, _testValue2);
            var array = new KeyValuePair<string, int>[_capacity];
            int startIndex = 1;

            // Act
            dictionary.CopyTo(array, startIndex);

            // Assert
            // The positions before startIndex should be default
            for (int i = 0; i < startIndex; i++)
            {
                Assert.AreEqual(default(KeyValuePair<string, int>).Key, array[i].Key, "Array element before startIndex should be default.");
                Assert.AreEqual(default(KeyValuePair<string, int>).Value, array[i].Value, "Array element before startIndex should be default.");
            }

            // Check copied elements
            List<KeyValuePair<string, int>> copied = array.Skip(startIndex).Take(2).ToList();
            CollectionAssert.Contains(copied, new KeyValuePair<string, int>(_testKey1, _testValue1), "Copied elements should contain first key/value pair.");
            CollectionAssert.Contains(copied, new KeyValuePair<string, int>(_testKey2, _testValue2), "Copied elements should contain second key/value pair.");
        }

        /// <summary>
        /// Tests the CopyTo method when called with an invalid array index, expecting an exception due to out of range index.
        /// </summary>
        [TestMethod]
        public void CopyTo_WhenCalledWithInvalidArrayIndex_ThrowsException()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add(_testKey1, _testValue1);
            var array = new KeyValuePair<string, int>[1];
            int invalidIndex = 1;

            // Act & Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => dictionary.CopyTo(array, invalidIndex), "CopyTo should throw exception when array index is invalid.");
        }

        /// <summary>
        /// Tests the GetEnumerator method to ensure it iterates through all key/value pairs.
        /// </summary>
        [TestMethod]
        public void GetEnumerator_WhenCalled_IteratesAllElements()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            var expected = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>(_testKey1, _testValue1),
                new KeyValuePair<string, int>(_testKey2, _testValue2)
            };
            dictionary.Add(_testKey1, _testValue1);
            dictionary.Add(_testKey2, _testValue2);

            // Act
            var actual = new List<KeyValuePair<string, int>>();
            foreach (var kvp in dictionary)
            {
                actual.Add(kvp);
            }

            // Assert
            CollectionAssert.AreEqual(expected, actual, "Enumeration did not yield the expected key/value pairs in order.");
        }

        /// <summary>
        /// Tests the IDictionary.GetEnumerator explicit interface method to ensure it iterates through all entries.
        /// </summary>
        [TestMethod]
        public void IDictionaryGetEnumerator_WhenCalled_IteratesAllElements()
        {
            // Arrange
            IDictionary dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add(_testKey1, _testValue1);
            dictionary.Add(_testKey2, _testValue2);
            var expectedKeys = new List<string> { _testKey1, _testKey2 };
            var expectedValues = new List<int> { _testValue1, _testValue2 };

            // Act
            var actualKeys = new List<string>();
            var actualValues = new List<int>();
            IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                actualKeys.Add((string)enumerator.Key);
                actualValues.Add((int)enumerator.Value);
            }

            // Assert
            CollectionAssert.AreEqual(expectedKeys, actualKeys, "IDictionary enumeration did not yield expected keys.");
            CollectionAssert.AreEqual(expectedValues, actualValues, "IDictionary enumeration did not yield expected values.");
        }

        /// <summary>
        /// Tests the Remove(TKey) method to ensure it throws a NotImplementedException.
        /// </summary>
        [TestMethod]
        public void RemoveKey_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);

            // Act & Assert
            Assert.ThrowsException<NotImplementedException>(() => dictionary.Remove(_testKey1), "Remove(TKey) should throw NotImplementedException.");
        }

        /// <summary>
        /// Tests the Remove(KeyValuePair) method to ensure it throws a NotImplementedException.
        /// </summary>
        [TestMethod]
        public void RemoveKeyValuePair_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            var kvp = new KeyValuePair<string, int>(_testKey1, _testValue1);

            // Act & Assert
            Assert.ThrowsException<NotImplementedException>(() => dictionary.Remove(kvp), "Remove(KeyValuePair) should throw NotImplementedException.");
        }

        /// <summary>
        /// Tests the TryGetValue method when the key exists, ensuring it returns true and outputs the correct value.
        /// </summary>
        [TestMethod]
        public void TryGetValue_WhenKeyExists_ReturnsTrueAndValue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add(_testKey1, _testValue1);

            // Act
            bool result = dictionary.TryGetValue(_testKey1, out int value);

            // Assert
            Assert.IsTrue(result, "TryGetValue should return true for an existing key.");
            Assert.AreEqual(_testValue1, value, "Returned value should match the stored value.");
        }

        /// <summary>
        /// Tests the TryGetValue method when the key does not exist, ensuring it returns false and outputs the default value.
        /// </summary>
        [TestMethod]
        public void TryGetValue_WhenKeyNotExists_ReturnsFalseAndDefaultValue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);

            // Act
            bool result = dictionary.TryGetValue(_testKey1, out int value);

            // Assert
            Assert.IsFalse(result, "TryGetValue should return false for a non-existent key.");
            Assert.AreEqual(default(int), value, "Value should be default when key is not found.");
        }

        /// <summary>
        /// Tests the IDictionary.Contains(object key) method when provided a valid key.
        /// </summary>
        [TestMethod]
        public void IDictionaryContains_WhenCalledWithValidKey_ReturnsTrue()
        {
            // Arrange
            IDictionary dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add(_testKey1, _testValue1);

            // Act
            bool contains = dictionary.Contains(_testKey1);

            // Assert
            Assert.IsTrue(contains, "IDictionary.Contains should return true for an existing key.");
        }

        /// <summary>
        /// Tests the IDictionary.Contains(object key) method when provided a key of invalid type.
        /// </summary>
        [TestMethod]
        public void IDictionaryContains_WhenCalledWithInvalidKeyType_ReturnsFalse()
        {
            // Arrange
            IDictionary dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add(_testKey1, _testValue1);

            // Act
            bool contains = dictionary.Contains(123); // wrong type key

            // Assert
            Assert.IsFalse(contains, "IDictionary.Contains should return false when key is of an invalid type.");
        }

        /// <summary>
        /// Tests the IDictionary.Add(object key, object value) method when provided valid types, expecting a NotSupportedException.
        /// </summary>
        [TestMethod]
        public void IDictionaryAdd_WhenCalledWithValidTypes_ThrowsNotSupportedException()
        {
            // Arrange
            IDictionary dictionary = new ArrayDictionary<string, int>(_capacity);

            // Act & Assert
            Assert.ThrowsException<NotSupportedException>(() => dictionary.Add(_testKey1, _testValue1),
                "IDictionary.Add should throw NotSupportedException even when provided valid types.");
        }

        /// <summary>
        /// Tests the IDictionary.Add(object key, object value) method when provided invalid types, expecting a NotSupportedException.
        /// </summary>
        [TestMethod]
        public void IDictionaryAdd_WhenCalledWithInvalidTypes_ThrowsNotSupportedException()
        {
            // Arrange
            IDictionary dictionary = new ArrayDictionary<string, int>(_capacity);

            // Act & Assert
            Assert.ThrowsException<NotSupportedException>(() => dictionary.Add(123, "invalid"),
                "IDictionary.Add should throw NotSupportedException when provided invalid types.");
        }

        /// <summary>
        /// Tests the IDictionary.Remove(object key) method to ensure it throws a NotImplementedException.
        /// </summary>
        [TestMethod]
        public void IDictionaryRemove_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            IDictionary dictionary = new ArrayDictionary<string, int>(_capacity);

            // Act & Assert
            Assert.ThrowsException<NotImplementedException>(() => dictionary.Remove(_testKey1),
                "IDictionary.Remove should throw NotImplementedException.");
        }

        /// <summary>
        /// Tests the Sort method to ensure it sorts the keys and reorders the associated values.
        /// </summary>
        [TestMethod]
        public void Sort_WhenCalled_SortsKeysAndAssociatedValues()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            // Add keys in unsorted order
            dictionary.Add("delta", 4);
            dictionary.Add("alpha", 1);
            dictionary.Add("charlie", 3);
            dictionary.Add("bravo", 2);

            // Act
            dictionary.Sort();

            // Assert
            // After sorting, keys should be in alphabetical order
            var sortedKeys = dictionary.KeyArray.Take(dictionary.Count).ToArray();
            var sortedValues = dictionary.ValueArray.Take(dictionary.Count).ToArray();
            string[] expectedKeys = new[] { "alpha", "bravo", "charlie", "delta" };
            int[] expectedValues = new[] { 1, 2, 3, 4 };

            CollectionAssert.AreEqual(expectedKeys, sortedKeys, "Keys are not sorted as expected.");
            CollectionAssert.AreEqual(expectedValues, sortedValues, "Values are not reordered as expected with sorted keys.");
        }

        /// <summary>
        /// Tests the Sort method when called twice, ensuring that the second call does not modify the already sorted collection.
        /// </summary>
        [TestMethod]
        public void Sort_WhenCalledTwice_DoesNotResort()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add("delta", 4);
            dictionary.Add("alpha", 1);
            dictionary.Add("charlie", 3);
            dictionary.Add("bravo", 2);

            // Act
            dictionary.Sort();
            var firstSortKeys = dictionary.KeyArray.Take(dictionary.Count).ToArray();
            var firstSortValues = dictionary.ValueArray.Take(dictionary.Count).ToArray();

            // Call sort again - expected to have no effect.
            dictionary.Sort();
            var secondSortKeys = dictionary.KeyArray.Take(dictionary.Count).ToArray();
            var secondSortValues = dictionary.ValueArray.Take(dictionary.Count).ToArray();

            // Assert
            CollectionAssert.AreEqual(firstSortKeys, secondSortKeys, "Second sort call should not change the order of keys.");
            CollectionAssert.AreEqual(firstSortValues, secondSortValues, "Second sort call should not change the order of values.");
        }

        /// <summary>
        /// Tests the Reset method of the enumerator to ensure it throws a NotImplementedException.
        /// </summary>
        [TestMethod]
        public void EnumeratorReset_WhenCalled_ThrowsNotImplementedException()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add(_testKey1, _testValue1);
            IEnumerator enumerator = dictionary.GetEnumerator();

            // Act
            enumerator.MoveNext(); // Move to first element

            // Assert
            Assert.ThrowsException<NotImplementedException>(() => enumerator.Reset(), "Reset should throw NotImplementedException.");
        }
    }
}
