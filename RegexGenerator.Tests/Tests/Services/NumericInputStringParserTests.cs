using System;
using NUnit.Framework;
using RegexGenerator.Services;

namespace RegexGeneratorTests.Tests.Services;

[TestFixture]
public class NumericInputStringParserTests
{
    private NumericInputStringParser? _parser;
    
    [SetUp]
    public void Setup() => _parser = new NumericInputStringParser();

    [Test]
    public void Test()
    {
        var inputRange = _parser?.ParseInput("-100", "2000.0809") 
                         ?? throw new Exception("Parser not initialized");
        
        Assert.IsNotNull(inputRange);
        Assert.IsTrue(inputRange.Min.IsNegative);
        Assert.AreEqual(100, inputRange.Min.Integer);
        Assert.IsNull(inputRange.Min.Decimal);
        
        Assert.IsFalse(inputRange.Max.IsNegative);
        Assert.AreEqual(2000, inputRange.Max.Integer);
        Assert.AreEqual(1, inputRange.Max.Decimal?.LeadingZeros);
        Assert.AreEqual(809, inputRange.Max.Decimal?.Value);
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