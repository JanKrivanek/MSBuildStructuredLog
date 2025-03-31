using FluentAssertions;
using Microsoft.Build.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Build.Collections.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ArrayDictionary{TKey, TValue}"/> class.
    /// </summary>
    public class ArrayDictionaryTests
    {
        private readonly int _defaultCapacity;

        public ArrayDictionaryTests()
        {
            _defaultCapacity = 3;
        }

        /// <summary>
        /// Tests that the constructor initializes an empty dictionary with the correct capacity.
        /// </summary>
        [Fact]
        public void Constructor_ValidCapacity_InitializesEmptyDictionary()
        {
            // Arrange & Act
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);

            // Assert
            dict.Count.Should().Be(0);
            dict.KeyArray.Should().HaveCount(_defaultCapacity);
            dict.ValueArray.Should().HaveCount(_defaultCapacity);
        }

        /// <summary>
        /// Tests that the Create method returns a new ArrayDictionary instance with count set to zero.
        /// </summary>
        [Fact]
        public void Create_WithCapacity_ReturnsNewArrayDictionary()
        {
            // Act
            var dict = ArrayDictionary<string, int>.Create(_defaultCapacity);

            // Assert
            dict.Should().BeOfType<ArrayDictionary<string, int>>();
            dict.Count.Should().Be(0);
        }

        /// <summary>
        /// Tests that using the indexer setter for a new key adds an entry and the getter returns the correct value.
        /// </summary>
        [Fact]
        public void Indexer_SetNewKey_AddsEntryAndGetReturnsValue()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            string key = "one";
            int value = 1;

            // Act
            dict[key] = value;

            // Assert
            dict.Count.Should().Be(1);
            dict[key].Should().Be(value);
        }

        /// <summary>
        /// Tests that using the indexer setter for an existing key updates the existing value.
        /// </summary>
        [Fact]
        public void Indexer_SetExistingKey_UpdatesValue()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            string key = "one";
            dict[key] = 1;
            int newValue = 42;

            // Act
            dict[key] = newValue;

            // Assert
            dict.Count.Should().Be(1);
            dict[key].Should().Be(newValue);
        }

        /// <summary>
        /// Tests that the indexer getter for a non-existent key returns the default value.
        /// </summary>
        [Fact]
        public void Indexer_GetNonExistentKey_ReturnsDefaultValue()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);

            // Act
            int result = dict["nonexistent"];

            // Assert
            result.Should().Be(default);
        }

        /// <summary>
        /// Tests that the Add(TKey, TValue) method adds entries when within capacity.
        /// </summary>
        [Fact]
        public void Add_WithinCapacity_AddsEntrySuccessfully()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);

            // Act
            dict.Add("one", 1);
            dict.Add("two", 2);

            // Assert
            dict.Count.Should().Be(2);
            dict["one"].Should().Be(1);
            dict["two"].Should().Be(2);
        }

        /// <summary>
        /// Tests that the Add(TKey, TValue) method throws an exception when capacity is exceeded.
        /// </summary>
        [Fact]
        public void Add_ExceedCapacity_ThrowsInvalidOperationException()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(2);
            dict.Add("one", 1);
            dict.Add("two", 2);

            // Act
            Action act = () => dict.Add("three", 3);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("ArrayDictionary is at capacity 2");
        }

        /// <summary>
        /// Tests that the Add(KeyValuePair<TKey, TValue>) method adds an entry.
        /// </summary>
        [Fact]
        public void AddKeyValuePair_WithinCapacity_AddsEntrySuccessfully()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            var kvp = new KeyValuePair<string, int>("one", 1);

            // Act
            ((IDictionary<string, int>)dict).Add(kvp);

            // Assert
            dict.Count.Should().Be(1);
            dict["one"].Should().Be(1);
        }

        /// <summary>
        /// Tests that the Contains(KeyValuePair<TKey, TValue>) method returns true when the entry exists.
        /// </summary>
        [Fact]
        public void ContainsKeyValuePair_WhenEntryExists_ReturnsTrue()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            dict.Add("one", 1);
            var kvp = new KeyValuePair<string, int>("one", 1);

            // Act
            bool contains = dict.Contains(kvp);

            // Assert
            contains.Should().BeTrue();
        }

        /// <summary>
        /// Tests that the Contains(KeyValuePair<TKey, TValue>) method returns false when the entry's value does not match.
        /// </summary>
        [Fact]
        public void ContainsKeyValuePair_WhenValueDoesNotMatch_ReturnsFalse()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            dict.Add("one", 1);
            var kvp = new KeyValuePair<string, int>("one", 99);

            // Act
            bool contains = dict.Contains(kvp);

            // Assert
            contains.Should().BeFalse();
        }

        /// <summary>
        /// Tests that the ContainsKey method correctly identifies existing and non-existing keys.
        /// </summary>
        [Fact]
        public void ContainsKey_ChecksForExistingAndNonExistingKeys()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            dict.Add("one", 1);

            // Act & Assert
            dict.ContainsKey("one").Should().BeTrue();
            dict.ContainsKey("two").Should().BeFalse();
        }

        /// <summary>
        /// Tests that the CopyTo method copies all entries to the target array starting at the specified index.
        /// </summary>
        [Fact]
        public void CopyTo_ValidArray_CopiesAllEntries()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            dict.Add("one", 1);
            dict.Add("two", 2);
            var array = new KeyValuePair<string, int>[dict.Count];

            // Act
            dict.CopyTo(array, 0);

            // Assert
            array.Length.Should().Be(dict.Count);
            array.Should().Contain(new KeyValuePair<string, int>("one", 1));
            array.Should().Contain(new KeyValuePair<string, int>("two", 2));
        }

        /// <summary>
        /// Tests that the GetEnumerator method iterates over all entries in the dictionary.
        /// </summary>
        [Fact]
        public void GetEnumerator_ForEachIteration_ReturnsAllEntries()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            dict.Add("one", 1);
            dict.Add("two", 2);
            var list = new List<KeyValuePair<string, int>>();

            // Act
            foreach (var kvp in dict)
            {
                list.Add(kvp);
            }

            // Assert
            list.Count.Should().Be(dict.Count);
            list.Should().Contain(new KeyValuePair<string, int>("one", 1));
            list.Should().Contain(new KeyValuePair<string, int>("two", 2));
        }

        /// <summary>
        /// Tests that the TryGetValue method returns true and the correct value when the key exists.
        /// </summary>
        [Fact]
        public void TryGetValue_KeyExists_ReturnsTrueAndValue()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            dict.Add("one", 1);

            // Act
            bool found = dict.TryGetValue("one", out int value);

            // Assert
            found.Should().BeTrue();
            value.Should().Be(1);
        }

        /// <summary>
        /// Tests that the TryGetValue method returns false and default value when the key does not exist.
        /// </summary>
        [Fact]
        public void TryGetValue_KeyNotExists_ReturnsFalseAndDefaultValue()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);

            // Act
            bool found = dict.TryGetValue("nonexistent", out int value);

            // Assert
            found.Should().BeFalse();
            value.Should().Be(default);
        }

        /// <summary>
        /// Tests that the Sort method properly sorts the keys and maintains the key/value associations.
        /// </summary>
        [Fact]
        public void Sort_UnsortedDictionary_SortsKeysAndValues()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            dict.Add("c", 3);
            dict.Add("a", 1);
            dict.Add("b", 2);

            // Act
            dict.Sort();

            // Assert
            // After sorting, keys should be in alphabetical order: "a", "b", "c".
            dict.KeyArray[0].Should().Be("a");
            dict.KeyArray[1].Should().Be("b");
            dict.KeyArray[2].Should().Be("c");
            // Corresponding values should match.
            dict.ValueArray[0].Should().Be(1);
            dict.ValueArray[1].Should().Be(2);
            dict.ValueArray[2].Should().Be(3);
        }

        /// <summary>
        /// Tests that calling the Sort method multiple times does not change the sorted order.
        /// </summary>
        [Fact]
        public void Sort_WhenCalledMultipleTimes_DoesNotResort()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            dict.Add("b", 2);
            dict.Add("a", 1);
            // First sort call.
            dict.Sort();
            var firstKeyArray = (string[])dict.KeyArray.Clone();
            var firstValueArray = (int[])dict.ValueArray.Clone();

            // Act
            dict.Sort();

            // Assert
            dict.KeyArray.Should().BeEquivalentTo(firstKeyArray, options => options.WithStrictOrdering());
            dict.ValueArray.Should().BeEquivalentTo(firstValueArray, options => options.WithStrictOrdering());
        }

        /// <summary>
        /// Tests that the Clear method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Clear_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);

            // Act
            Action act = () => dict.Clear();

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that the Remove(TKey) method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void RemoveKey_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);

            // Act
            Action act = () => dict.Remove("one");

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that the Remove(KeyValuePair<TKey, TValue>) method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void RemoveKeyValuePair_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            var kvp = new KeyValuePair<string, int>("one", 1);

            // Act
            Action act = () => dict.Remove(kvp);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests the explicit IDictionary indexer getter and setter for correct behavior.
        /// </summary>
        [Fact]
        public void IDictionary_IndexerGetSet_WorksAsExpected()
        {
            // Arrange
            IDictionary idict = new ArrayDictionary<string, int>(_defaultCapacity);
            string key = "one";
            int value = 1;

            // Act
            idict[key] = value;
            var retrieved = idict[key];

            // Assert
            retrieved.Should().Be(value);
        }

        /// <summary>
        /// Tests that the explicit IDictionary Add method throws NotSupportedException when invoked with valid types.
        /// </summary>
        [Fact]
        public void IDictionary_Add_InvalidBehavior_ThrowsNotSupportedException()
        {
            // Arrange
            IDictionary idict = new ArrayDictionary<string, int>(_defaultCapacity);

            // Act
            Action act = () => idict.Add("one", 1);

            // Assert
            act.Should().Throw<NotSupportedException>();
        }

        /// <summary>
        /// Tests that the explicit IDictionary Contains method returns false for invalid key types.
        /// </summary>
        [Fact]
        public void IDictionary_Contains_InvalidKey_ReturnsFalse()
        {
            // Arrange
            IDictionary idict = new ArrayDictionary<string, int>(_defaultCapacity);
            // Use indexer to add entry.
            idict["one"] = 1;

            // Act
            bool result = idict.Contains(123);

            // Assert
            result.Should().BeFalse();
        }

        /// <summary>
        /// Tests that the explicit IDictionary Remove method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void IDictionary_Remove_Always_ThrowsNotImplementedException()
        {
            // Arrange
            IDictionary idict = new ArrayDictionary<string, int>(_defaultCapacity);

            // Act
            Action act = () => idict.Remove("one");

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that the non-generic ICollection.CopyTo method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void ICollection_CopyTo_NonGeneric_Always_ThrowsNotImplementedException()
        {
            // Arrange
            ICollection collection = (ICollection)new ArrayDictionary<string, int>(_defaultCapacity);

            // Act
            Action act = () => collection.CopyTo(new object[_defaultCapacity], 0);

            // Assert
            act.Should().Throw<NotImplementedException>();
        }

        /// <summary>
        /// Tests that the enumerator's Reset method throws a NotImplementedException.
        /// </summary>
        [Fact]
        public void Enumerator_Reset_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var dict = new ArrayDictionary<string, int>(_defaultCapacity);
            dict.Add("one", 1);
            var enumerator = ((IEnumerable)dict).GetEnumerator();
            enumerator.MoveNext();

            // Act
            Action act = () => enumerator.Reset();

            // Assert
            act.Should().Throw<NotImplementedException>();
        }
    }
}
