using System;
using NUnit.Framework;
using RegexGenerator.Services;

namespace RegexGeneratorTests.Tests.Services;

public class RegexRangeServiceTests
{
    private RegexRangeService _rangeService;
    
    [SetUp]
    public void Setup() => _rangeService = new();

    [TestCase("-1000", "200")]
    public void Test(string min, string max)
    {
        var parser = new NumericInputStringParser();
        var input = parser.ParseInput(min, max);
        var ranges = _rangeService.GetRegexRanges(input);

        foreach (var range in ranges)
        {
            Console.Write(range);
        }
    }
}