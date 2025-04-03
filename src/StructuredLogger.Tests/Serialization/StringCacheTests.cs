// using FluentAssertions;
// using Microsoft.Build.Logging.StructuredLogger;
// using System;
// using System.Collections.Generic;
// using Xunit;
// 
// namespace Microsoft.Build.Logging.StructuredLogger.UnitTests
// {
//     /// <summary>
//     /// Unit tests for the <see cref="StringCache"/> class.
//     /// </summary>
//     public class StringCacheTests
//     {
// //         /// <summary> // [Error] (25-25)CS1061 'StringCache' does not contain a definition for 'Instances' and no accessible extension method 'Instances' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (26-25)CS1061 'StringCache' does not contain a definition for 'Instances' and no accessible extension method 'Instances' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?)
// //         /// Tests that the constructor initializes the Instances property to the keys of the internal dictionary.
// //         /// Expected outcome: Instances is not null and initially empty.
// //         /// </summary>
// //         [Fact]
// //         public void Constructor_InitializesInstancesToDictionaryKeys()
// //         {
// //             // Arrange & Act
// //             var stringCache = new StringCache();
// // 
// //             // Assert
// //             stringCache.Instances.Should().NotBeNull();
// //             stringCache.Instances.Should().BeEmpty("because the deduplication map is empty upon construction");
// //         }
// //  // [Error] (44-25)CS1061 'StringCache' does not contain a definition for 'Seal' and no accessible extension method 'Seal' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (47-25)CS1061 'StringCache' does not contain a definition for 'DisableDeduplication' and no accessible extension method 'DisableDeduplication' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (48-25)CS1061 'StringCache' does not contain a definition for 'Instances' and no accessible extension method 'Instances' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (51-56)CS1061 'StringCache' does not contain a definition for 'Instances' and no accessible extension method 'Instances' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that Seal correctly seals the cache.
//         /// It copies the deduplicated strings into an array with an empty string at index 0,
//         /// nulls out the internal dictionary, sets Instances to the new array, and disables deduplication.
//         /// </summary>
//         [Fact]
//         public void Seal_WhenCalled_SetsInstancesAndDisablesDeduplication()
//         {
//             // Arrange
//             var stringCache = new StringCache();
//             // Intern a couple of strings to populate the deduplication map.
//             stringCache.Intern("test");
//             stringCache.Intern("example");
// 
//             // Act
//             stringCache.Seal();
// 
//             // Assert
//             stringCache.DisableDeduplication.Should().BeTrue("because Seal should disable deduplication");
//             stringCache.Instances.Should().BeOfType<string[]>()
//                 .Which.Should().HaveCount(3, "because there were 2 interned strings and an extra empty string inserted at index 0");
//             
//             var instancesArray = (string[])stringCache.Instances;
//             instancesArray[0].Should().Be(string.Empty, "because the first element must be an empty string");
//             // The remaining two strings should be the unique interned strings.
//             instancesArray.Should().Contain(new[] { "test", "example" });
//             // Calling a method that locks on deduplicationMap after Seal may throw exception; thus we do not call further intern methods.
//         }
// //  // [Error] (69-25)CS1061 'StringCache' does not contain a definition for 'SetStrings' and no accessible extension method 'SetStrings' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (72-25)CS1061 'StringCache' does not contain a definition for 'Instances' and no accessible extension method 'Instances' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (73-25)CS1061 'StringCache' does not contain a definition for 'DisableDeduplication' and no accessible extension method 'DisableDeduplication' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that SetStrings sets the Instances property to the provided enumerable and disables deduplication.
// //         /// </summary>
// //         [Fact]
// //         public void SetStrings_WithValidEnumerable_SetsInstancesAndDisablesDeduplication()
// //         {
// //             // Arrange
// //             var stringCache = new StringCache();
// //             var externalStrings = new List<string> { "alpha", "beta", "gamma" };
// // 
// //             // Act
// //             stringCache.SetStrings(externalStrings);
// // 
// //             // Assert
// //             stringCache.Instances.Should().BeEquivalentTo(externalStrings, "because SetStrings should assign the provided enumerable to Instances");
// //             stringCache.DisableDeduplication.Should().BeTrue("because SetStrings disables deduplication");
// //         }
// //  // [Error] (88-32)CS1503 Argument 1: cannot convert from 'System.Collections.Generic.List<string>' to 'string' // [Error] (92-25)CS7036 There is no argument given that corresponds to the required parameter 'ch' of 'TextUtilities.Contains(string, Span, char)' // [Error] (93-25)CS7036 There is no argument given that corresponds to the required parameter 'ch' of 'TextUtilities.Contains(string, Span, char)' // [Error] (94-25)CS7036 There is no argument given that corresponds to the required parameter 'ch' of 'TextUtilities.Contains(string, Span, char)' // [Error] (96-25)CS7036 There is no argument given that corresponds to the required parameter 'ch' of 'TextUtilities.Contains(string, Span, char)'
//         /// <summary>
//         /// Tests that Intern(IEnumerable{string}) correctly calls Intern for each string.
//         /// Verifies that after interning, the individual strings are added to the internal deduplication map.
//         /// </summary>
//         [Fact]
//         public void InternEnumerable_WithMultipleStrings_InternsEachString()
//         {
//             // Arrange
//             var stringCache = new StringCache();
//             var stringsToIntern = new List<string> { "x", "y", "x", "z", string.Empty };
// 
//             // Act
//             stringCache.Intern(stringsToIntern);
// 
//             // Assert
//             // "x", "y", and "z" should be interned (non-empty strings)
//             stringCache.Contains("x").Should().BeTrue();
//             stringCache.Contains("y").Should().BeTrue();
//             stringCache.Contains("z").Should().BeTrue();
//             // Empty string is not added to the deduplication map.
//             stringCache.Contains(string.Empty).Should().BeFalse();
//         }
// //  // [Error] (108-17)CS0117 'StringCache' does not contain a definition for 'HasDeduplicatedStrings' // [Error] (113-41)CS1061 'StringCache' does not contain a definition for 'SoftIntern' and no accessible extension method 'SoftIntern' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that SoftIntern returns the original text when HasDeduplicatedStrings is true.
// //         /// </summary>
// //         [Fact]
// //         public void SoftIntern_WhenHasDeduplicatedStringsTrue_ReturnsInputTextDirectly()
// //         {
// //             // Arrange
// //             var stringCache = new StringCache
// //             {
// //                 HasDeduplicatedStrings = true
// //             };
// //             string input = "sample text";
// // 
// //             // Act
// //             string result = stringCache.SoftIntern(input);
// // 
// //             // Assert
// //             result.Should().Be(input, "because when HasDeduplicatedStrings is true, SoftIntern should return the input without interning");
// //         }
// //  // [Error] (129-17)CS0117 'StringCache' does not contain a definition for 'HasDeduplicatedStrings' // [Error] (130-17)CS0117 'StringCache' does not contain a definition for 'DisableDeduplication' // [Error] (131-17)CS0117 'StringCache' does not contain a definition for 'NormalizeLineEndings' // [Error] (136-44)CS1061 'StringCache' does not contain a definition for 'SoftIntern' and no accessible extension method 'SoftIntern' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (137-44)CS1061 'StringCache' does not contain a definition for 'SoftIntern' and no accessible extension method 'SoftIntern' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that SoftIntern, when HasDeduplicatedStrings is false, returns an interned string.
//         /// Verifies that repeated calls return the same instance.
//         /// </summary>
//         [Fact]
//         public void SoftIntern_WhenHasDeduplicatedStringsFalse_InternsAndReturnsInternedString()
//         {
//             // Arrange
//             var stringCache = new StringCache
//             {
//                 HasDeduplicatedStrings = false,
//                 DisableDeduplication = false,
//                 NormalizeLineEndings = false // Disable normalization for test consistency
//             };
//             string input = "sample text";
// 
//             // Act
//             string interned1 = stringCache.SoftIntern(input);
//             string interned2 = stringCache.SoftIntern(input);
// 
//             // Assert
//             interned1.Should().Be(input);
//             // The intern method should return the same instance on repeated calls.
//             ReferenceEquals(interned1, interned2).Should().BeTrue("because interning should return the same instance for the same string");
//         }
// //  // [Error] (156-17)CS0117 'StringCache' does not contain a definition for 'DisableDeduplication' // [Error] (157-17)CS0117 'StringCache' does not contain a definition for 'NormalizeLineEndings'
// //         /// <summary>
// //         /// Tests that Intern(string) returns the input if it is null or empty.
// //         /// Also verifies that if DisableDeduplication is true, Intern returns the input without interning.
// //         /// </summary>
// //         [Theory]
// //         [InlineData("")]
// //         public void Intern_StringIsEmptyOrDisabledDeduplication_ReturnsInputDirectly(string input)
// //         {
// //             // Arrange
// //             var stringCache = new StringCache
// //             {
// //                 DisableDeduplication = true,
// //                 NormalizeLineEndings = false // To avoid normalization altering the string.
// //             };
// // 
// //             // Act
// //             string resultWithDisabled = stringCache.Intern(input);
// // 
// //             // Assert
// //             resultWithDisabled.Should().Be(input, "because when DisableDeduplication is true or input is empty, the original string is returned");
// //         }
// //  // [Error] (177-17)CS0117 'StringCache' does not contain a definition for 'DisableDeduplication' // [Error] (178-17)CS0117 'StringCache' does not contain a definition for 'NormalizeLineEndings'
//         /// <summary>
//         /// Tests that Intern(string) correctly interns a non-empty string when deduplication is enabled.
//         /// Verifies that repeated calls return the same instance.
//         /// </summary>
//         [Fact]
//         public void Intern_NonEmptyStringInterned_ReturnsSameInstanceOnSubsequentCalls()
//         {
//             // Arrange
//             var stringCache = new StringCache
//             {
//                 DisableDeduplication = false,
//                 NormalizeLineEndings = false // Disable normalization for consistency.
//             };
//             string input = "intern me";
// 
//             // Act
//             string result1 = stringCache.Intern(input);
//             string result2 = stringCache.Intern(input);
// 
//             // Assert
//             result1.Should().Be(input);
//             ReferenceEquals(result1, result2).Should().BeTrue("because interning the same string should return the same instance");
//         }
// //  // [Error] (201-17)CS0117 'StringCache' does not contain a definition for 'DisableDeduplication' // [Error] (202-17)CS0117 'StringCache' does not contain a definition for 'NormalizeLineEndings' // [Error] (208-47)CS7036 There is no argument given that corresponds to the required parameter 'ch' of 'TextUtilities.Contains(string, Span, char)' // [Error] (210-46)CS7036 There is no argument given that corresponds to the required parameter 'ch' of 'TextUtilities.Contains(string, Span, char)' // [Error] (215-17)CS0117 'StringCache' does not contain a definition for 'DisableDeduplication' // [Error] (216-17)CS0117 'StringCache' does not contain a definition for 'NormalizeLineEndings' // [Error] (219-61)CS7036 There is no argument given that corresponds to the required parameter 'ch' of 'TextUtilities.Contains(string, Span, char)'
// //         /// <summary>
// //         /// Tests that Contains(string) returns true if a string has been interned and false otherwise.
// //         /// Also verifies that if deduplication is disabled, interning does not add the string.
// //         /// </summary>
// //         [Fact]
// //         public void Contains_AfterInterning_ReturnsCorrectResult()
// //         {
// //             // Arrange
// //             var stringCache = new StringCache
// //             {
// //                 DisableDeduplication = false,
// //                 NormalizeLineEndings = false
// //             };
// //             string testValue = "check";
// // 
// //             // Act
// //             // Before interning, should not contain.
// //             bool containsBefore = stringCache.Contains(testValue);
// //             stringCache.Intern(testValue);
// //             bool containsAfter = stringCache.Contains(testValue);
// // 
// //             // Test with DisableDeduplication true.
// //             var stringCacheDisabled = new StringCache
// //             {
// //                 DisableDeduplication = true,
// //                 NormalizeLineEndings = false
// //             };
// //             stringCacheDisabled.Intern(testValue);
// //             bool containsWhenDisabled = stringCacheDisabled.Contains(testValue);
// // 
// //             // Assert
// //             containsBefore.Should().BeFalse("because the string has not been interned yet");
// //             containsAfter.Should().BeTrue("because after interning, the string should be present in the deduplication map");
// //             containsWhenDisabled.Should().BeFalse("because when DisableDeduplication is true, the string is not stored in the deduplication map");
// //         }
// //  // [Error] (237-17)CS0117 'StringCache' does not contain a definition for 'DisableDeduplication' // [Error] (238-17)CS0117 'StringCache' does not contain a definition for 'NormalizeLineEndings' // [Error] (249-50)CS1061 'StringCache' does not contain a definition for 'InternStringDictionary' and no accessible extension method 'InternStringDictionary' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (255-25)CS1061 'StringCache' does not contain a definition for 'DisableDeduplication' and no accessible extension method 'DisableDeduplication' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (260-42)CS1061 'StringCache' does not contain a definition for 'InternStringDictionary' and no accessible extension method 'InternStringDictionary' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (261-43)CS1061 'StringCache' does not contain a definition for 'InternStringDictionary' and no accessible extension method 'InternStringDictionary' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (275-42)CS1061 'StringCache' does not contain a definition for 'InternStringDictionary' and no accessible extension method 'InternStringDictionary' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?)
//         /// <summary>
//         /// Tests that InternStringDictionary returns the same instance when DisableDeduplication is true.
//         /// Also tests the behavior when the input dictionary is null or empty.
//         /// </summary>
//         [Fact]
//         public void InternStringDictionary_WithVariousInputs_ReturnsExpectedResults()
//         {
//             // Arrange
//             var stringCache = new StringCache
//             {
//                 DisableDeduplication = true,
//                 NormalizeLineEndings = false
//             };
// 
//             // When DisableDeduplication is true, the input dictionary should be returned as-is.
//             IDictionary<string, string> inputDict = new Dictionary<string, string>
//             {
//                 { "key1", "value1" },
//                 { "key2", "value2" }
//             };
// 
//             // Act
//             var resultWhenDisabled = stringCache.InternStringDictionary(inputDict);
// 
//             // Assert
//             resultWhenDisabled.Should().BeSameAs(inputDict, "because when deduplication is disabled, the original dictionary is returned");
// 
//             // Arrange for further tests with deduplication enabled.
//             stringCache.DisableDeduplication = false;
//             IDictionary<string, string> nullDict = null;
//             IDictionary<string, string> emptyDict = new Dictionary<string, string>();
// 
//             // Act
//             var resultNull = stringCache.InternStringDictionary(nullDict);
//             var resultEmpty = stringCache.InternStringDictionary(emptyDict);
// 
//             // Assert
//             resultNull.Should().BeNull("because a null input should return null");
//             resultEmpty.Should().BeSameAs(emptyDict, "because an empty dictionary should be returned as-is");
// 
//             // Arrange non-empty dictionary with overlapping keys/values.
//             IDictionary<string, string> nonEmptyDict = new Dictionary<string, string>
//             {
//                 { "dupKey", "dupValue" },
//                 { "uniqueKey", "uniqueValue" }
//             };
// 
//             // Act
//             var resultDict = stringCache.InternStringDictionary(nonEmptyDict);
// 
//             // Assert
//             resultDict.Should().NotBeSameAs(nonEmptyDict, "because a new dictionary should be created for non-disabled deduplication");
//             resultDict.Should().HaveCount(nonEmptyDict.Count);
//             // Verify that each key and value is interned (i.e. subsequent calls yield the same reference)
//             foreach (var kvp in nonEmptyDict)
//             {
//                 var internedKey = stringCache.Intern(kvp.Key);
//                 var internedValue = stringCache.Intern(kvp.Value);
// 
//                 resultDict.Should().ContainKey(internedKey);
//                 resultDict[internedKey].Should().Be(internedValue);
//             }
//         }
// //  // [Error] (301-17)CS0117 'StringCache' does not contain a definition for 'DisableDeduplication' // [Error] (302-17)CS0117 'StringCache' does not contain a definition for 'NormalizeLineEndings' // [Error] (308-50)CS1061 'StringCache' does not contain a definition for 'InternList' and no accessible extension method 'InternList' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (314-25)CS1061 'StringCache' does not contain a definition for 'DisableDeduplication' and no accessible extension method 'DisableDeduplication' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (319-42)CS1061 'StringCache' does not contain a definition for 'InternList' and no accessible extension method 'InternList' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (320-43)CS1061 'StringCache' does not contain a definition for 'InternList' and no accessible extension method 'InternList' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?) // [Error] (330-42)CS1061 'StringCache' does not contain a definition for 'InternList' and no accessible extension method 'InternList' accepting a first argument of type 'StringCache' could be found (are you missing a using directive or an assembly reference?)
// //         /// <summary>
// //         /// Tests that InternList returns the same instance when DisableDeduplication is true.
// //         /// Also tests the behavior when the input list is null or empty.
// //         /// </summary>
// //         [Fact]
// //         public void InternList_WithVariousInputs_ReturnsExpectedResults()
// //         {
// //             // Arrange
// //             var stringCache = new StringCache
// //             {
// //                 DisableDeduplication = true,
// //                 NormalizeLineEndings = false
// //             };
// // 
// //             IReadOnlyList<string> inputList = new List<string> { "a", "b", "c" };
// // 
// //             // Act
// //             var resultWhenDisabled = stringCache.InternList(inputList);
// // 
// //             // Assert
// //             resultWhenDisabled.Should().BeSameAs(inputList, "because when deduplication is disabled, the original list is returned");
// // 
// //             // Arrange for further tests with deduplication enabled.
// //             stringCache.DisableDeduplication = false;
// //             IReadOnlyList<string> nullList = null;
// //             IReadOnlyList<string> emptyList = new List<string>();
// // 
// //             // Act
// //             var resultNull = stringCache.InternList(nullList);
// //             var resultEmpty = stringCache.InternList(emptyList);
// // 
// //             // Assert
// //             resultNull.Should().BeNull("because a null input should return null");
// //             resultEmpty.Should().BeSameAs(emptyList, "because an empty list should be returned as-is");
// // 
// //             // Arrange non-empty list with duplicate strings.
// //             IReadOnlyList<string> nonEmptyList = new List<string> { "repeat", "unique", "repeat" };
// // 
// //             // Act
// //             var resultList = stringCache.InternList(nonEmptyList);
// // 
// //             // Assert
// //             resultList.Should().NotBeSameAs(nonEmptyList, "because a new list should be created when deduplication is enabled");
// //             resultList.Should().HaveCount(nonEmptyList.Count);
// //             // Verify that interned duplicate strings refer to the same instance.
// //             string firstInterned = stringCache.Intern("repeat");
// //             resultList[0].Should().Be(firstInterned);
// //             resultList[2].Should().Be(firstInterned);
// //         }
// //     }
// }