using System;
using NUnit.Framework;
using RegexGenerator.Services;
using RegexGenerator.Services.InputParsers;

namespace RegexGeneratorTests.Tests.Services;

[TestFixture]
public class DecimalInputParserTests
{
    private DecimalInputParser? _parser;
    
    [SetUp]
    public void Setup() => _parser = new DecimalInputParser();

    [Test]
    public void Test()
    {
        var inputRange = _parser?.ParseInput("-100", "2000.08090") 
                         ?? throw new Exception("Parser not initialized");
        
        Assert.That(inputRange, Is.Not.Null);
        Assert.That(inputRange.Min.IsNegative);
        Assert.That(inputRange.Min.Integer, Is.EqualTo(100));
        Assert.That(inputRange.Min.Fractional?.Value, Is.EqualTo(0));
        
        Assert.That(inputRange.Max.IsNegative, Is.False);
        Assert.That(inputRange.Max.Integer, Is.EqualTo(2000));
        Assert.That(inputRange.Max.Fractional?.LeadingZeros, Is.EqualTo(1));
        Assert.That(inputRange.Max.Fractional?.Value, Is.EqualTo(8090));
    }

    [TestCase(".1", ".2")]
    [TestCase("0.1", "0.2")]
    [TestCase("1.1", "1.2")]
    [TestCase("1.10", "1.20")]
    [TestCase("0.0", "0.1")]
    [TestCase("1.0", "1.1")]
    [TestCase("-1.0", "1.1")]
    [TestCase("-.1", ".1")]
    [TestCase("-.0", ".0")]
    [TestCase("-0", "0")]
    [TestCase("-0.0", "0.0")]
    [TestCase("-123.456", "654.321")]
    public void Parses_Valid_Inputs(string min, string max)
    {
        var input = _parser?.ParseInput(min, max);
        Console.WriteLine(input);
    }
}