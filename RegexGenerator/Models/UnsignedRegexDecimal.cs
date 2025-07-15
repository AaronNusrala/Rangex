namespace RegexGenerator.Models;

public class UnsignedRegexDecimal
{
    public int Integer { get; init; }
    
    public UnsignedRegexFractional Fractional { get; init; }

    public UnsignedRegexDecimal(int integer, UnsignedRegexFractional fractional)
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