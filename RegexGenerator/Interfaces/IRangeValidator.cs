using System.Numerics;

namespace RegexGenerator.Interfaces;

public interface IRangeValidator
{
    (bool Result, string Message) ValidateRange<TInt>(TInt min, TInt max) where TInt : IComparisonOperators<TInt, TInt, bool>?;
}