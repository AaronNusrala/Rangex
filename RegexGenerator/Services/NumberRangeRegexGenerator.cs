using RegexGenerator.Interfaces;
using RegexGenerator.Models;
using RegexGenerator.Models.Input;
using RegexGenerator.Services.InputParsers;

namespace RegexGenerator.Services;

internal class NumberRangeRegexGenerator : INumberRangeRegexGenerator
{
    private readonly IInputParser _inputParser;
    private readonly IRegexRangeService _rangeService;
    private readonly IRangesToRegexConverter _rangesConverter;

    internal NumberRangeRegexGenerator(
        DecimalInputParser inputParser,
        IRegexRangeService rangeService,
        IRangesToRegexConverter rangesConverter)
    {
        _inputParser = inputParser;
        _rangeService = rangeService;
        _rangesConverter = rangesConverter;
    }
    
    //DIY DI is good enough for this. Register this class with your container of choice, or don't.
    public NumberRangeRegexGenerator() : this(
        new DecimalInputParser(),
        new RegexRangeService(),
        new RangesToRegexConverter()){ }
    
    /// <summary>
    /// Parse and validate the input strings. If the input is valid, calculate regex-able ranges and convert them into a regex string.
    /// </summary>
    public string GenerateRegex(string min, string max, RegexGeneratorOptions? options = null)
    {
        var input = _inputParser.ParseInput(min, max);
        return ProcessInput(input);
    }

    public string GenerateRegex(int min, int max, RegexGeneratorOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public string GenerateRegex(double min, double max, RegexGeneratorOptions? options = null)
    {
        throw new NotImplementedException();
    }

    private string ProcessInput(InputRange input)
    {
        var ranges = _rangeService.GetRegexRanges(input).ToList();
        return _rangesConverter.ConvertRanges(ranges);
    }
}