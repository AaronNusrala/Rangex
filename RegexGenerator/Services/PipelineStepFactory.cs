using System.Numerics;
using RegexGenerator.Interfaces;
using RegexGenerator.Interfaces.Pipeline;
using RegexGenerator.Models.NumberSystems;
using RegexGenerator.Models.Numeric;
using RegexGenerator.Models.Regex;
using RegexGenerator.Services.Pipeline;
using RegexGenerator.Services.RangeCalculators;
using RegexGenerator.Services.RangeConverters;
using RegexGenerator.Services.RangeValidators;
using RegexGenerator.Services.RegexOptimizers;

namespace RegexGenerator.Services;

internal class PipelineStepFactory : IPipelineStepFactory
{
    public IParseResult CreateParseResult<TInt, TNumberSystem>(TInt min, TInt max) 
        where TInt : INumber<TInt> where TNumberSystem : INumberSystem
    {
        var rangeValidator = new IntegerRangeValidator();
        return new IntegerParseResult<TInt, TNumberSystem>(this, rangeValidator, min, max);
    }
    
    public IValidationResult CreateValidationResult<TInt, TNumberSystem>(TInt min, TInt max) 
        where TInt : INumber<TInt> where TNumberSystem : INumberSystem
    {
        var unsigned = new UnsignedIntegerRangeCalculator<TInt>();
        var rangeCalculator = new SignedIntegerRangeCalculator<TInt>(unsigned);
        return new ValidationResult<TInt, TNumberSystem>(this, rangeCalculator, min, max);
    }

    public IRangeCalculationResult CreateRangeCalculationResult<TInt, TNumberSystem>(IEnumerable<Range<TInt>> regexRanges) 
        where TInt : INumber<TInt> where TNumberSystem : INumberSystem
    {
        var rangeConverter = new SignedIntegerRangeConverter<TInt>();
        return new RangeCalculationResult<TInt, TNumberSystem>(this, rangeConverter, regexRanges);
    }

    public IRangeConversionResult CreateRangeConversionResult<TNumberSystem>(IEnumerable<SignedRegexIntegerRange> ranges) where TNumberSystem : INumberSystem
    {
        var rangeOptimizer = new SimpleUnsignedIntegerRegexOptimizer();
        return new RangeConversionResult<SignedRegexIntegerRange, TNumberSystem>(this, rangeOptimizer, ranges);
    }
    
    public IRegexGenerationResult CreateRegexGenerationResult(string pattern)
    {
        return new RegexGenerationResult { Regex = pattern };
    }
}