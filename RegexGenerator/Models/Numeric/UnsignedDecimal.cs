using System.Numerics;

namespace RegexGenerator.Models;

internal class UnsignedDecimal<TInt, TFractional> where TInt : INumber<TInt> where TFractional : INumber<TFractional>
{
    public TInt Integer { get; init; }
    
    public UnsignedFractional<TFractional> Fractional { get; init; }

    public UnsignedDecimal(TInt integer, UnsignedFractional<TFractional> fractional)
    {
        if (integer < TInt.Zero)
        {
            throw new ArgumentException("Integer must be positive");
        }
        
        Integer = integer;
        Fractional = fractional;
    }

    public override string ToString() => Integer.ToString() + Fractional;
}