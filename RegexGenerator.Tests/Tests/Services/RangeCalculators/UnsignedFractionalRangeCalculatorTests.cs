// using System;
// using System.Collections.Generic;
// using System.Linq;
// using NUnit.Framework;
// using RegexGenerator.Models;
// using RegexGenerator.Services.RangeCalculators;
// using RegexGeneratorTests.TestCases;
// using RegexGeneratorTests.TestCases.TestCaseModels.DecimalRangeCalculatorTests;
//
// namespace RegexGeneratorTests.Tests.Services.RangeCalculators
// {
//     [TestFixture]
//     internal class UnsignedFractionalRangeCalculatorTests
//     {
//         private UnsignedFractionalRangeCalculator? _rangeCalculator;
//
//         [SetUp]
//         public void Setup()
//         {
//             _rangeCalculator = new UnsignedFractionalRangeCalculator();
//         }
//
//         public static DecimalRangeCalculatorTestCase[]? GetTestCases()
//         {
//             return TestCaseUtility.GetTestCases<DecimalRangeCalculatorTestCase[]>(nameof(UnsignedFractionalRangeCalculatorTests));
//         }
//
//         [Test, TestCaseSource(nameof(GetTestCases))]
//         public void Calculates_Decimal_Ranges(DecimalRangeCalculatorTestCase testCase)
//         {
//             var minDecimal = FromDecimal(testCase.Min);
//             var maxDecimal = FromDecimal(testCase.Max);
//             
//             var decimalRanges = _rangeCalculator?
//                 .GetRanges(minDecimal, maxDecimal, false)
//                 .ToList()
//                 ?? throw new Exception("Range calculator is not initialized");
//             
//             foreach (var range in decimalRanges)
//             {
//                 Console.Write(range);
//             }
//             
//             ValidateRanges(minDecimal, maxDecimal, decimalRanges);
//             
//             Assert.That(testCase.ExpectedRanges.Length, Is.EqualTo(decimalRanges.Count), "Range count mismatch");
//             
//             for (var i = 0; i < testCase.ExpectedRanges.Length; i++)
//             {
//                 var expectedRange = testCase.ExpectedRanges[i];
//                 var expectedMin = FromDecimal(expectedRange.Min);
//                 var expectedMax = FromDecimal(expectedRange.Max);
//                 var actualRange = decimalRanges[i];
//                 AssertDecimalsAreEqual(expectedMin, actualRange.Min);
//                 AssertDecimalsAreEqual(expectedMax, actualRange.Max);
//             }
//         }
//         
//         private static UnsignedFractional FromDecimal(decimal value)
//         {
//             var doubleCharacters = value
//                 .ToString()
//                 .Skip(2)
//                 .ToList();
//             
//             var decimalLeadingZeros = doubleCharacters
//                 .TakeWhile((c, i) => i < doubleCharacters.Count - 1 && c == '0')
//                 .Count();
//             
//             var valueCharacters = doubleCharacters.Skip(decimalLeadingZeros);
//             var valueString = string.Join("", valueCharacters);
//             var decimalValue = int.Parse(valueString);
//             return new UnsignedFractional(decimalLeadingZeros, decimalValue);
//         }
//
//         private static void AssertDecimalsAreEqual(UnsignedFractional d1, UnsignedFractional d2)
//         {
//             Assert.That(d1.LeadingZeros, Is.EqualTo(d2.LeadingZeros), $"Expected {d1}, got {d2}");
//             Assert.That(d1.Value, Is.EqualTo(d2.Value), $"Expected {d1}, got {d2}");
//         }
//
//         private static void ValidateRanges(UnsignedFractional min, UnsignedFractional max, List<RegexFractionalRange> ranges)
//         {
//             for (var i = 0; i < ranges.Count - 1; i++)
//             {
//                 var currentRange = ranges[i];
//                 var rangeMin = currentRange.Min.ToString();
//                 var rangeMax = currentRange.Max.ToString();
//                 
//                 Assert.That(rangeMin.Length, Is.EqualTo(rangeMax.Length));
//                 
//                 for (var j = 1; j < rangeMin.Length; j++)
//                 {
//                     var minChar = rangeMin[j];
//                     var maxChar = rangeMax[j];
//                     Assert.That(minChar, Is.LessThanOrEqualTo(maxChar));
//                 }
//
//                 var nextRange = ranges[i + 1];
//                 var nextRangeMin = nextRange.Min.ToString();
//                 var rangeDiff = decimal.Parse(nextRangeMin) - decimal.Parse(rangeMax);
//                 var expectedDiff = decimal.Parse("." + new string('0', rangeMin.Length - 2) + "1");
//                 Assert.That(expectedDiff, Is.EqualTo(rangeDiff), $"({min}, {max})");
//             }
//         }
//     }
// }
