using System.Numerics;
using RegexGenerator.Interfaces;
using RegexGenerator.Interfaces.Pipeline;

namespace RegexGenerator.Services.Pipeline;

internal class ValidationResult<TInt, TNumberSystem>(
    IPipelineStepFactory stepFactory,
    IRangeCalculator<TInt, TInt> rangeCalculator,
    TInt min, 
    TInt max) : IValidationResult where TNumberSystem : INumberSystem where TInt : INumber<TInt>
{
    public IRangeCalculationResult CalculateRegexRanges()
    {
        var regexRanges = rangeCalculator.CalculateRegexRanges<TNumberSystem>(min, max);
        return stepFactory.CreateRangeCalculationResult<TInt, TNumberSystem>(regexRanges);
    }
}
