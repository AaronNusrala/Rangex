using RegexGenerator.Models;
using RegexGenerator.Services;


namespace RegexGenerator;

public interface INumberRangeRegexGenerator
{
    public string GenerateRegex(string min, string max, RegexGeneratorOptions? options = null);

    public string GenerateRegex(int min, int max, RegexGeneratorOptions? options = null);
}

public class NumberRangeRegexGenerator : INumberRangeRegexGenerator
{
    private readonly INumericInputStringParser _inputParser;
    private readonly IRegexRangeService _rangeService;
    private readonly IRangesToRegexConverter _rangesConverter;

    internal NumberRangeRegexGenerator(
        INumericInputStringParser inputParser,
        IRegexRangeService rangeService,
        IRangesToRegexConverter rangesConverter)
    {
        _inputParser = inputParser;
        _rangeService = rangeService;
        _rangesConverter = rangesConverter;
    }
    
    //DIY DI is good enough for this. Register this class with your container of choice, or don't.
    public NumberRangeRegexGenerator() : this(
        new NumericInputStringParser(),
        new RegexRangeService(),
        new RangesToRegexConverter()){ }
    
    public string GenerateRegex(string min, string max, RegexGeneratorOptions? options = null)
    {
        var input = _inputParser.ParseInput(min, max);
        return ProcessInput(input);
    }

    public string GenerateRegex(int min, int max, RegexGeneratorOptions? options = null)
    {
        var input = new InputRange
        {
            Min = new RegexNumber {Integer = min},
            Max = new RegexNumber {Integer = max}
        };

        return ProcessInput(input);
    }
    
    private string ProcessInput(InputRange input)
    {
        var ranges = _rangeService.GetRegexRanges(input);
        return _rangesConverter.ConvertRanges(ranges);
    }
}