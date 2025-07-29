using System.Numerics;
using RegexGenerator.Interfaces;
using RegexGenerator.Interfaces.Pipeline;

namespace RegexGenerator.Services.Pipeline;

internal class IntegerParseResult<TInt, TNumberSystem>(IPipelineStepFactory stepFactory, IRangeValidator<TInt> rangeValidator, TInt min, TInt max) 
    : IParseResult where TInt : INumber<TInt> where TNumberSystem : INumberSystem
{
    public bool Successful => true;
    
    public IPipelineResult? Previous => null;

    public IValidationResult ValidateInput()
    {
        try
        {
            rangeValidator.ValidateRange(min, max);
            return stepFactory.CreateValidationResult<TInt, TNumberSystem>(min, max);
        }
        catch(Exception ex)
        {
            return new ValidationFailedResult("Range validation failed", ex);
        }
    }
}