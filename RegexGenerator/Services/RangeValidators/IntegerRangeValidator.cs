using System.Numerics;
using RegexGenerator.Interfaces;

namespace RegexGenerator.Services.RangeValidators;

internal class IntegerRangeValidator<TInt> : IRangeValidator<TInt>
    where TInt : IComparisonOperators<TInt, TInt, bool>?
{
    public (bool Result, string Message) ValidateRange(TInt min, TInt max) 
    {
        if (min == null)
        {
            return (false, "Min value cannot be null.");
        }
        
        if (max == null)
        {
            return (false, "Max value cannot be null.");
        }
        
        return min > max 
            ? (false, "Min must be less than or equal to Max.") 
            : (true, "Valid range.");
    }
}