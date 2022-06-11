using System;
using NUnit.Framework;
using RegexGenerator.Services;

namespace RegexGeneratorTests.Tests.Services;

[TestFixture]
public class InputRangeValidatorTests
{
    private InputRangeValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new InputRangeValidator();
    }

    [TestCase("-1", "1")]
    [TestCase("-1.9", "1.1")]
    [TestCase(".1", ".1")]
    [TestCase(".0", ".1")]
    [TestCase("0", "1")]
    [TestCase("1", "1")]
    [TestCase("1", "2")]
    [TestCase("1.0", "1")]
    [TestCase("1", "1.0")]
    [TestCase("1.0", "1.1")]
    [TestCase("1.2", "2.1")]
    [TestCase("-2", "1")]
    [TestCase("-.2", ".1")]
    [TestCase("-2", "-1")]
    [TestCase("1", "1.1")]
    [TestCase("-1.1", "-1")]
    [TestCase("-.01", "-.001")]
    [TestCase(".001", ".01")]
    [TestCase("-1", "1")]
    public void Does_Not_Throw_For_Valid_Ranges(string min, string max)
    {
        var parser = new NumericInputStringParser();
        var input = parser.ParseInput(min, max);
        Assert.DoesNotThrow(() => _validator.ValidateInputRange(input));
    }
    
    [TestCase(".1", ".0")]
    [TestCase("1", "0")]
    [TestCase("2", "1")]
    [TestCase("1.1", "1.0")]
    [TestCase("2.1", "1.2")]
    [TestCase("1", "-2")]
    [TestCase(".1", "-.2")]
    [TestCase("-1", "-2")]
    [TestCase("1.1", "1")]
    [TestCase("-1", "-1.1")]
    [TestCase("-.001", "-.01")]
    [TestCase(".01", ".001")]
    [TestCase("1", "-1")]
    [TestCase("1", "-2")]
    public void Throws_For_Invalid_Ranges(string min, string max)
    {
        var parser = new NumericInputStringParser();
        var input = parser.ParseInput(min, max);
        var ex = Assert.Throws<Exception>(() => _validator.ValidateInputRange(input));
    }
}