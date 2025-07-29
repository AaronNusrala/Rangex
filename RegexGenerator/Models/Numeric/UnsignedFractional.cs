using System.Numerics;

namespace RegexGenerator.Models;

/// <summary>
/// Represents a number [0, 1) in a way that can be used to calculate regex-able ranges.
/// </summary>
internal sealed class UnsignedFractional<TInt> where TInt : INumber<TInt>
{
    public static UnsignedFractional<TInt> Zero { get; } = new(0, TInt.Zero);

    public TInt Value { get; }

    public int LeadingZeros { get; }
    
    public UnsignedFractional(int leadingZeros, TInt value)
    {
        if (value < TInt.Zero || leadingZeros < 0)
        {
            throw new ArgumentOutOfRangeException();
        }
            
        Value = value;
        LeadingZeros = leadingZeros;
    }

    public override string ToString() => $".{new ('0', LeadingZeros)}{Value}";
}