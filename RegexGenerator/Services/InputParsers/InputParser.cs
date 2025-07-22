using RegexGenerator.Interfaces;

namespace RegexGenerator.Services;

public abstract class InputParser<TNumeric>(IRangeValidator<TNumeric> rangeValidator) : IInputParser<TNumeric>
{
    public (TNumeric Min, TNumeric max) ParseInput(string min, string max)
    {
        var numericMin = ParseInput(min);
        var numericMax = ParseInput(max);
        rangeValidator.ValidateRange(numericMin, numericMax);
        return (numericMin, numericMax);
    }
    
    protected abstract TNumeric ParseInput(string value);
}