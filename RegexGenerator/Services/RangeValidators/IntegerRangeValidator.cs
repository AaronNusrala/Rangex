using RegexGenerator.Interfaces;

namespace RegexGenerator.Services.RangeValidators;

public class IntegerRangeValidator : IRangeValidator<int>
{
    public void ValidateRange(int min, int max)
    {
        if (min > max)
        {
            throw new ArgumentException("Min must be less than or equal to Max");
        }
    }
}