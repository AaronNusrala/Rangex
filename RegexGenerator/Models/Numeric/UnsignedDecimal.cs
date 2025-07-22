namespace RegexGenerator.Models;

public class UnsignedDecimal
{
    public int Integer { get; init; }
    
    public UnsignedFractional Fractional { get; init; }

    public UnsignedDecimal(int integer, UnsignedFractional fractional)
    {
        if (integer < 0)
        {
            throw new ArgumentException("Integer must be positive");
        }
        
        Integer = integer;
        Fractional = fractional;
    }

    public override string ToString() => Integer.ToString() + Fractional;
}