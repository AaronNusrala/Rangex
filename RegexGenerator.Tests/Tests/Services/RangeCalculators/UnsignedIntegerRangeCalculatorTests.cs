using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexGenerator.Models.NumberSystems;
using RegexGenerator.Models.Numeric;
using RegexGenerator.Services.RangeCalculators;
using RegexGeneratorTests.TestCases;
using RegexGeneratorTests.TestCases.TestCaseModels.RangeCalculatorTests;

namespace RegexGeneratorTests.Tests.Services.RangeCalculators;

[TestFixture]
public class UnsignedIntegerRangeCalculatorTests
{
    private UnsignedIntegerRangeCalculator<int>? _rangeCalculator;

    [SetUp]
    public void Setup()
    {
        _rangeCalculator = new UnsignedIntegerRangeCalculator<int>();
    }
        
    public static RangeCalculatorTestCase[]? GetTestCases()
    {
        return TestCaseUtility.GetTestCases<RangeCalculatorTestCase[]>(nameof(UnsignedIntegerRangeCalculatorTests));
    }
        
    [TestCaseSource(nameof(GetTestCases))]
    public void Generates_Ranges(RangeCalculatorTestCase testCase)
    {
        var actualRanges = _rangeCalculator?
                               .CalculateRegexRanges<DecimalNumbers>(testCase.Min, testCase.Max)
                               .ToList()
                           ?? throw new Exception("Range calculator not initialized");

        foreach (var range in actualRanges)
        {
            Console.Write(range);
        }
            
        ValidateRanges(testCase.Min, testCase.Max, actualRanges);
            
        if (testCase.ExpectedRanges.Length != actualRanges.Count)
        {
            var message = CreateMessage(testCase.ExpectedRanges, actualRanges);
            throw new AssertionException(message);
        }

        foreach (var expectedRange in testCase.ExpectedRanges)
        {
            var actualRange = actualRanges.FirstOrDefault(r => r.Min == expectedRange.Min && r.Max == expectedRange.Max);

            if (actualRange == null)
            {
                var message = CreateMessage(testCase.ExpectedRanges, actualRanges);
                throw new AssertionException(message);
            }

            actualRanges.Remove(actualRange);
        }
    }
        
    private static void ValidateRanges(int min, int max, List<Range<int>> ranges)
    {
        ranges = ranges.OrderBy(r => r.Min).ToList();
            
        for (var i = 0; i < ranges.Count - 1; i++)
        {
            var currentRange = ranges[i];
            var rangeMin = currentRange.Min.ToString();
            var rangeMax = currentRange.Max.ToString();
                
            Assert.That(rangeMin.Length, Is.EqualTo(rangeMax.Length));
        
            for (var j = 1; j < rangeMin.Length; j++)
            {
                var minChar = rangeMin[j];
                var maxChar = rangeMax[j];
                Assert.That(minChar, Is.LessThanOrEqualTo(maxChar));
            }
        
            var nextRange = ranges[i + 1];
            var rangeDiff = nextRange.Min - currentRange.Max;
            Assert.That(rangeDiff, Is.EqualTo(1), $"({min}, {max})");
        }
    }

    private static string CreateMessage(IEnumerable<ExpectedRange> expectedRanges, IEnumerable<Range<int>> actualRanges)
    {
        var sb = new StringBuilder();
        sb.Append("Expected: ");

        foreach (var expectedRange in expectedRanges.OrderBy(r => r.Min))
        {
            sb.Append(expectedRange);
        }

        sb.AppendLine();
        sb.Append("Got:      ");

        foreach (var (actualMin, actualMax) in actualRanges.OrderBy(r => r.Min))
        {
            sb.Append("(").Append(actualMin).Append(", ").Append(actualMax).Append(") ");
        }

        return sb.ToString();
    }
}