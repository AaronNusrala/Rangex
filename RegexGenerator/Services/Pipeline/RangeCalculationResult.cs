using System.Numerics;
using RegexGenerator.Interfaces;
using RegexGenerator.Interfaces.Pipeline;
using RegexGenerator.Models.Numeric;
using RegexGenerator.Models.Regex;

namespace RegexGenerator.Services.Pipeline;

internal class RangeCalculationResult<TInt, TNumberSystem>(IPipelineStepFactory stepFactory, IRangeConverter<TInt, SignedRegexIntegerRange> rangeConverter, IEnumerable<Range<TInt>> ranges) 
    : IRangeCalculationResult where TInt : INumber<TInt> where TNumberSystem : INumberSystem
{
    public IRangeConversionResult ConvertRanges()
    {
        var regexRanges = ranges.Select(rangeConverter.ConvertToRegexRanges<TNumberSystem>);
        return stepFactory.CreateRangeConversionResult<TNumberSystem>(regexRanges);
    }
}