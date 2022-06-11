using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RegexGenerator.Models;
using RegexGenerator.Services.RangeCalculators;
using RegexGeneratorTests.TestCases;
using RegexGeneratorTests.TestCases.TestCaseModels.DecimalRangeCalculatorTests;

namespace RegexGeneratorTests.Tests.Services.RangeCalculators
{
    [TestFixture]
    internal class DecimalRangeCalculatorTests
    {
        private DecimalRangeCalculator? _rangeCalculator;

        [SetUp]
        public void Setup()
        {
            _rangeCalculator = new DecimalRangeCalculator();
        }

        public static DecimalRangeCalculatorTestCase[]? GetTestCases()
        {
            return TestCaseUtility.GetTestCases<DecimalRangeCalculatorTestCase[]>(nameof(DecimalRangeCalculatorTests));
        }

        [Test, TestCaseSource(nameof(GetTestCases))]
        public void Calculates_Decimal_Ranges(DecimalRangeCalculatorTestCase testCase)
        {
            var minDecimal = FromDecimal(testCase.Min);
            var maxDecimal = FromDecimal(testCase.Max);
            
            var decimalRanges = _rangeCalculator?
                .GetRanges(minDecimal, maxDecimal)
                .ToList()
                ?? throw new Exception("Range calculator is not initialized");
            
            foreach (var range in decimalRanges)
            {
                Console.Write(range);
            }
            
            ValidateRanges(minDecimal, maxDecimal, decimalRanges);
            
            Assert.AreEqual(testCase.ExpectedRanges.Length, decimalRanges.Count, "Range count mismatch");
            
            for (var i = 0; i < testCase.ExpectedRanges.Length; i++)
            {
                var expectedRange = testCase.ExpectedRanges[i];
                var expectedMin = FromDecimal(expectedRange.Min);
                var expectedMax = FromDecimal(expectedRange.Max);
                var actualRange = decimalRanges[i];
                AssertDecimalsAreEqual(expectedMin, actualRange.Min);
                AssertDecimalsAreEqual(expectedMax, actualRange.Max);
            }
        }
        
        private static RegexDecimal FromDecimal(decimal value)
        {
            var doubleCharacters = value
                .ToString()
                .Skip(2)
                .ToList();
            
            var decimalLeadingZeros = doubleCharacters
                .TakeWhile((c, i) => i < doubleCharacters.Count - 1 && c == '0')
                .Count();
            
            var valueCharacters = doubleCharacters.Skip(decimalLeadingZeros);
            var valueString = string.Join("", valueCharacters);
            var decimalValue = int.Parse(valueString);
            return new RegexDecimal(decimalLeadingZeros, decimalValue);
        }

        private static void AssertDecimalsAreEqual(RegexDecimal d1, RegexDecimal d2)
        {
            Assert.AreEqual(d1.LeadingZeros, d2.LeadingZeros, $"Expected {d1}, got {d2}");
            Assert.AreEqual(d1.Value, d2.Value, $"Expected {d1}, got {d2}");
        }

        private static void ValidateRanges(RegexDecimal min, RegexDecimal max, List<RegexDecimalRange> ranges)
        {
            for (var i = 0; i < ranges.Count - 1; i++)
            {
                var currentRange = ranges[i];
                var rangeMin = currentRange.Min.ToString();
                var rangeMax = currentRange.Max.ToString();
                
                Assert.AreEqual(rangeMin.Length, rangeMax.Length);
                
                for (var j = 1; j < rangeMin.Length; j++)
                {
                    var minChar = rangeMin[j];
                    var maxChar = rangeMax[j];
                    Assert.IsTrue(minChar <= maxChar);
                }

                var nextRange = ranges[i + 1];
                var nextRangeMin = nextRange.Min.ToString();
                var rangeDiff = decimal.Parse(nextRangeMin) - decimal.Parse(rangeMax);
                var expectedDiff = decimal.Parse("." + new string('0', rangeMin.Length - 2) + "1");
                Assert.AreEqual(expectedDiff, rangeDiff, $"({min}, {max})");
            }
        }
    }
}
