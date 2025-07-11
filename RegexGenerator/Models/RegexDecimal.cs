namespace RegexGenerator.Models;

public class RegexDecimal
{
    public int Integer { get; init; }
    
    public UnsignedRegexFractional Fractional { get; init; }

    public RegexDecimal(int integer, UnsignedRegexFractional fractional)
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