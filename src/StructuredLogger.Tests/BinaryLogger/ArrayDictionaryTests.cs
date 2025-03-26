using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Build.Collections;
using Xunit;

namespace Microsoft.Build.Collections.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ArrayDictionary{TKey, TValue}"/> class.
    /// </summary>
    public class ArrayDictionaryTests
    {
        private readonly int _capacity = 5;

        /// <summary>
        /// Tests that the constructor initializes an empty dictionary with the correct capacity.
        /// </summary>
        [Fact]
        public void Constructor_InitializesEmptyDictionary()
        {
            // Arrange & Act
            var dictionary = new ArrayDictionary<int, string>(_capacity);

            // Assert
            Assert.Equal(0, dictionary.Count);
            Assert.NotNull(dictionary.KeyArray);
            Assert.NotNull(dictionary.ValueArray);
            Assert.Equal(_capacity, dictionary.KeyArray.Length);
            Assert.Equal(_capacity, dictionary.ValueArray.Length);
        }

        /// <summary>
        /// Tests that the static Create method returns a new instance of ArrayDictionary.
        /// </summary>
        [Fact]
        public void Create_StaticMethod_ReturnsNewInstance()
        {
            // Act
            var dictionary = ArrayDictionary<int, string>.Create(_capacity);

            // Assert
            Assert.NotNull(dictionary);
            Assert.Equal(0, dictionary.Count);
        }

        /// <summary>
        /// Tests that Add adds a valid item and increments the count.
        /// </summary>
        [Fact]
        public void Add_ValidItem_IncrementsCount()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            int key = 1;
            string value = "one";

            // Act
            dictionary.Add(key, value);

            // Assert
            Assert.Equal(1, dictionary.Count);
            Assert.True(dictionary.ContainsKey(key));
            Assert.Equal(value, dictionary[key]);
        }

        /// <summary>
        /// Tests that Add throws an InvalidOperationException when capacity is exceeded.
        /// </summary>
        [Fact]
        public void Add_WhenAtCapacity_ThrowsInvalidOperationException()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(2);
            dictionary.Add(1, "one");
            dictionary.Add(2, "two");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => dictionary.Add(3, "three"));
        }

        /// <summary>
        /// Tests that the indexer getter returns the correct value for an existing key.
        /// </summary>
        [Fact]
        public void IndexerGet_KeyExists_ReturnsCorrectValue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(1, "one");

            // Act
            string result = dictionary[1];

            // Assert
            Assert.Equal("one", result);
        }

        /// <summary>
        /// Tests that the indexer getter returns the default value when the key does not exist.
        /// </summary>
        [Fact]
        public void IndexerGet_KeyNotExists_ReturnsDefaultValue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);

            // Act
            string result = dictionary[99];

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests that the indexer setter updates the value for an existing key.
        /// </summary>
        [Fact]
        public void IndexerSet_KeyExists_UpdatesValue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(1, "one");
            string newValue = "uno";

            // Act
            dictionary[1] = newValue;

            // Assert
            Assert.Equal(newValue, dictionary[1]);
            Assert.Equal(1, dictionary.Count);
        }

        /// <summary>
        /// Tests that the indexer setter adds a new item when the key does not exist.
        /// </summary>
        [Fact]
        public void IndexerSet_KeyNotExists_AddsItem()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);

            // Act
            dictionary[2] = "two";

            // Assert
            Assert.True(dictionary.ContainsKey(2));
            Assert.Equal("two", dictionary[2]);
            Assert.Equal(1, dictionary.Count);
        }

        /// <summary>
        /// Tests that Contains returns true for an existing key-value pair.
        /// </summary>
        [Fact]
        public void Contains_KeyValuePairExists_ReturnsTrue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(1, "one");
            var kvp = new KeyValuePair<int, string>(1, "one");

            // Act
            bool exists = dictionary.Contains(kvp);

            // Assert
            Assert.True(exists);
        }

        /// <summary>
        /// Tests that Contains returns false when the key-value pair does not exist.
        /// </summary>
        [Fact]
        public void Contains_KeyValuePairDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(1, "one");
            var kvp = new KeyValuePair<int, string>(1, "uno");

            // Act
            bool exists = dictionary.Contains(kvp);

            // Assert
            Assert.False(exists);
        }

        /// <summary>
        /// Tests that ContainsKey returns true when the key exists.
        /// </summary>
        [Fact]
        public void ContainsKey_KeyExists_ReturnsTrue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add("a", 100);

            // Act
            bool exists = dictionary.ContainsKey("a");

            // Assert
            Assert.True(exists);
        }

        /// <summary>
        /// Tests that ContainsKey returns false when the key does not exist.
        /// </summary>
        [Fact]
        public void ContainsKey_KeyDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var dictionary = new ArrayDictionary<string, int>(_capacity);
            dictionary.Add("a", 100);

            // Act
            bool exists = dictionary.ContainsKey("b");

            // Assert
            Assert.False(exists);
        }

        /// <summary>
        /// Tests that CopyTo copies the dictionary items correctly to an array.
        /// </summary>
        [Fact]
        public void CopyTo_CopiesItemsCorrectly()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(1, "one");
            dictionary.Add(2, "two");
            var array = new KeyValuePair<int, string>[dictionary.Count];

            // Act
            dictionary.CopyTo(array, 0);

            // Assert
            Assert.Equal(dictionary.Count, array.Length);
            Assert.Contains(new KeyValuePair<int, string>(1, "one"), array);
            Assert.Contains(new KeyValuePair<int, string>(2, "two"), array);
        }

        /// <summary>
        /// Tests that the GetEnumerator method iterates over all items.
        /// </summary>
        [Fact]
        public void GetEnumerator_IteratesOverAllItems()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(1, "one");
            dictionary.Add(2, "two");
            var items = new List<KeyValuePair<int, string>>();

            // Act
            foreach (var item in dictionary)
            {
                items.Add(item);
            }

            // Assert
            Assert.Equal(dictionary.Count, items.Count);
            Assert.Contains(new KeyValuePair<int, string>(1, "one"), items);
            Assert.Contains(new KeyValuePair<int, string>(2, "two"), items);
        }

        /// <summary>
        /// Tests that TryGetValue returns true and outputs the correct value for an existing key.
        /// </summary>
        [Fact]
        public void TryGetValue_KeyExists_ReturnsTrueAndOutputsValue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(1, "one");

            // Act
            bool success = dictionary.TryGetValue(1, out string value);

            // Assert
            Assert.True(success);
            Assert.Equal("one", value);
        }

        /// <summary>
        /// Tests that TryGetValue returns false and outputs default when the key does not exist.
        /// </summary>
        [Fact]
        public void TryGetValue_KeyDoesNotExist_ReturnsFalseAndOutputsDefault()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);

            // Act
            bool success = dictionary.TryGetValue(42, out string value);

            // Assert
            Assert.False(success);
            Assert.Null(value);
        }

        /// <summary>
        /// Tests that Sort correctly sorts the keys and corresponding values.
        /// </summary>
        [Fact]
        public void Sort_SortsItemsWhenCalled()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(3, "three");
            dictionary.Add(1, "one");
            dictionary.Add(2, "two");

            // Act
            dictionary.Sort();

            // Assert: Check that the keys array is sorted
            var keysCopy = new int[dictionary.Count];
            Array.Copy(dictionary.KeyArray, keysCopy, dictionary.Count);
            for (int i = 1; i < keysCopy.Length; i++)
            {
                Assert.True(keysCopy[i - 1] <= keysCopy[i]);
            }
            // Also check that values follow the keys (based on Array.Sort behavior)
            // We assume that the sort uses the keys' natural order.
            // Find index of key 1 and ensure value is "one"
            int index = Array.IndexOf(dictionary.KeyArray, 1);
            Assert.True(index >= 0);
            Assert.Equal("one", dictionary.ValueArray[index]);
        }

        /// <summary>
        /// Tests that Clear method throws NotImplementedException.
        /// </summary>
        [Fact]
        public void Clear_ThrowsNotImplementedException()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => dictionary.Clear());
        }

        /// <summary>
        /// Tests that Remove(TKey key) method throws NotImplementedException.
        /// </summary>
        [Fact]
        public void Remove_Key_ThrowsNotImplementedException()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(1, "one");

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => dictionary.Remove(1));
        }

        /// <summary>
        /// Tests that Remove(KeyValuePair) method throws NotImplementedException.
        /// </summary>
        [Fact]
        public void Remove_KeyValuePair_ThrowsNotImplementedException()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(1, "one");
            var kvp = new KeyValuePair<int, string>(1, "one");

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => dictionary.Remove(kvp));
        }

        /// <summary>
        /// Tests that the IDictionary.Add(object, object) explicit implementation always throws NotSupportedException.
        /// </summary>
        [Fact]
        public void IDictionary_Add_AlwaysThrowsNotSupportedException()
        {
            // Arrange
            IDictionary dict = new ArrayDictionary<int, string>(_capacity);

            // Act & Assert
            NotSupportedException ex = Assert.Throws<NotSupportedException>(() => dict.Add(1, "one"));
        }

        /// <summary>
        /// Tests that the IDictionary.Remove(object) explicit implementation throws NotImplementedException.
        /// </summary>
        [Fact]
        public void IDictionary_Remove_ThrowsNotImplementedException()
        {
            // Arrange
            IDictionary dict = new ArrayDictionary<int, string>(_capacity);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => dict.Remove(1));
        }

        /// <summary>
        /// Tests that IDictionary.Contains(object) returns false when provided with a key of the wrong type.
        /// </summary>
        [Fact]
        public void IDictionary_Contains_KeyOfWrongType_ReturnsFalse()
        {
            // Arrange
            IDictionary dict = new ArrayDictionary<int, string>(_capacity);
            dict.Add(1, "one");

            // Act
            bool contains = dict.Contains("not an int");

            // Assert
            Assert.False(contains);
        }

        /// <summary>
        /// Tests the explicit IDictionary indexer getter returns the correct value.
        /// </summary>
        [Fact]
        public void IDictionaryIndexerGet_ReturnsCorrectValue()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(1, "one");
            IDictionary dict = dictionary;

            // Act
            object value = dict[1];

            // Assert
            Assert.Equal("one", value);
        }

        /// <summary>
        /// Tests the explicit IDictionary indexer setter adds or updates the item correctly.
        /// </summary>
        [Fact]
        public void IDictionaryIndexerSet_AddsOrUpdatesItem()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            IDictionary dict = dictionary;

            // Act - add new item using explicit setter
            dict[2] = "two";

            // Assert adding new item
            Assert.Equal("two", dictionary[2]);
            Assert.Equal(1, dictionary.Count);

            // Act - update existing item using explicit setter
            dict[2] = "dos";

            // Assert updating existing
            Assert.Equal("dos", dictionary[2]);
            Assert.Equal(1, dictionary.Count);
        }

        /// <summary>
        /// Tests that IDictionary.GetEnumerator returns a dictionary enumerator that emits DictionaryEntry.
        /// </summary>
        [Fact]
        public void IDictionary_GetEnumerator_ReturnsDictionaryEnumerator()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(1, "one");
            dictionary.Add(2, "two");
            IDictionary dict = dictionary;

            // Act
            var enumerator = dict.GetEnumerator();
            var entries = new List<DictionaryEntry>();
            while (enumerator.MoveNext())
            {
                entries.Add(enumerator.Entry);
            }

            // Assert
            Assert.Equal(dictionary.Count, entries.Count);
            Assert.Contains(entries, e => (int)e.Key == 1 && (string)e.Value == "one");
            Assert.Contains(entries, e => (int)e.Key == 2 && (string)e.Value == "two");
        }

        /// <summary>
        /// Tests that calling Reset on the enumerator throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Enumerator_Reset_ThrowsNotImplementedException()
        {
            // Arrange
            var dictionary = new ArrayDictionary<int, string>(_capacity);
            dictionary.Add(1, "one");
            IEnumerator enumerator = dictionary.GetEnumerator();

            // Act
            // MoveNext to ensure enumerator is valid for testing Reset
            enumerator.MoveNext();

            // Assert
            Assert.Throws<NotImplementedException>(() => ((IEnumerator)enumerator).Reset());
        }
    }
}
