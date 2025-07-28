using System.Numerics;
using RegexGenerator.Interfaces.Pipeline;
using RegexGenerator.Models.Numeric;
using RegexGenerator.Models.Regex;

namespace RegexGenerator.Interfaces;

internal interface IPipelineStepFactory
{
    IParseResult CreateParseResult<TInt, TNumberSystem>(TInt min, TInt max) 
        where TInt : INumber<TInt> where TNumberSystem : INumberSystem;

    IValidationResult CreateValidationResult<TInt, TNumberSystem>(TInt min, TInt max) 
        where TInt : INumber<TInt> where TNumberSystem : INumberSystem;
    
    IRangeCalculationResult CreateRangeCalculationResult<TInt, TNumberSystem>(IEnumerable<Range<TInt>> regexRanges) 
        where TInt : INumber<TInt> where TNumberSystem : INumberSystem;
    
    IRangeConversionResult CreateRangeConversionResult<TNumberSystem>(IEnumerable<SignedRegexIntegerRange> ranges) 
        where TNumberSystem : INumberSystem;

    IRegexGenerationResult CreateRegexGenerationResult(string pattern);
}