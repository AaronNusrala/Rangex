using System.Numerics;

namespace RegexGenerator.Models;

internal class SignedDecimal<TInt, TFractional>(TInt integer, UnsignedFractional<TFractional> fractional, bool isNegative) 
    : UnsignedDecimal<TInt, TFractional>(integer, fractional) where TInt : INumber<TInt> where TFractional : INumber<TFractional>
{
    public bool IsNegative { get; } = isNegative;
}