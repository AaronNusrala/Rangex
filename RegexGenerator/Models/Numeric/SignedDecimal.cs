namespace RegexGenerator.Models;

public class SignedDecimal(int integer, UnsignedFractional fractional, bool isNegative) : UnsignedDecimal(integer, fractional)
{
    public bool IsNegative { get; init; } = isNegative;
}