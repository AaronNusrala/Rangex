using System.Numerics;
using RegexGenerator.Interfaces;

namespace RegexGenerator.Services.RangeValidators;

internal class IntegerRangeValidator : IRangeValidator
{
    public (bool Result, string Message) ValidateRange<TInt>(TInt min, TInt max) where TInt : IComparisonOperators<TInt, TInt, bool>?
    {
        return min > max 
            ? (false, "Min must be less than or equal to Max.") 
            : (true, "Valid range.");
    }
}