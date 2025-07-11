using System;
using System.Text;
using NUnit.Framework;
using RegexGenerator.Services;
using RegexGenerator.Services.InputParsers;

namespace RegexGeneratorTests.Tests.Services;

public class RegexRangeServiceTests
{
    private RegexRangeService _rangeService;
    
    [SetUp]
    public void Setup() => _rangeService = new();

    [TestCase("-10.5", "10.5", "-(10.5, 10.5), -(10.0, 10.4), +-(0, 9), (10.0, 10.4), (10.5, 10.5)")] //Unoptimized, should be able to account for same integer and decimal ranges with opposite signs
    [TestCase("-.5", ".5", "-(0.5, 0.5), -(0.0, 0.4), (0.0, 0.4), (0.5, 0.5)")] //Unoptimized, there are duplicate ranges with different signs
    [TestCase("-.5", "-.2", "-(0.2, 0.4), -(0.5, 0.5)")]
    [TestCase(".2", ".5", "(0.2, 0.4), (0.5, 0.5)")]
    [TestCase("-.5", ".2", "-(0.5, 0.5), -(0.0, 0.4), (0.0, 0.1), (0.2, 0.2)")] //oddity where +-0 is output as an integer range.
    [TestCase("-.5", "0", "-(0.5, 0.5), -(0.0, 0.4), +-(0.0, 0.0)")] //oddity where multiple zero ranges are returned
    [TestCase("0", ".5", "+-(0.0, 0.0), (0.1, 0.4), (0.5, 0.5)")]
    [TestCase("-1", "1", "+-(0, 1)")]
    public void Test(string min, string max, string expected)
    {
        var parser = new DecimalInputParser();
        var input = parser.ParseInput(min, max); //cheating
        var ranges = _rangeService.GetRegexRanges(input);
        var actual = string.Join(", ", ranges);
        Console.WriteLine(actual);
        Assert.That(actual, Is.EqualTo(expected));
    }
}