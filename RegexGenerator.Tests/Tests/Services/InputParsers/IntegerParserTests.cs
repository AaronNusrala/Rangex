using System;
using Moq;
using NUnit.Framework;
using RegexGenerator.Interfaces;
using RegexGenerator.Models.NumberSystems;
using RegexGenerator.Services.InputParsers;

namespace RegexGeneratorTests.Tests.Services.InputParsers;

[TestFixture]
public class IntegerParserTests
{
    private Mock<IPipelineStepFactory> _stepFactory;
    private IntegerParser? _parser;

    [SetUp]
    public void Setup()
    {
        _stepFactory = new ();
        _parser = new(_stepFactory.Object);
    } 
    
    [Test]
    public void Parses_Valid_Integer_Input()
    {
        _parser?.ParseInput<DecimalNumbers>("1", "10");
        _stepFactory.Verify(m => m.CreateParseResult<int, DecimalNumbers>(1, 10), Times.Once);
    }

    [Test]
    public void Parses_Valid_Long_Input()
    {
        const long maxInt = int.MaxValue;
        var max = maxInt + 1;
        _parser?.ParseInput<DecimalNumbers>("1", max.ToString());
        _stepFactory.Verify(m => m.CreateParseResult<long, DecimalNumbers>(1, max), Times.Once);
    }
    
    [Test]
    public void Parses_Valid_Negative_Input()
    {
        _parser?.ParseInput<DecimalNumbers>("-2", "-1");
        _stepFactory.Verify(m => m.CreateParseResult<int, DecimalNumbers>(-2, -1), Times.Once);
    }
}