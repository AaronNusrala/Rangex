namespace RegexGenerator.Models;

public class RegexNumber
{
    public int Integer { get; init; }
    
    public RegexDecimal? Decimal { get; init; }

    public RegexNumber(int integer, RegexDecimal? @decimal = null)
    {
        if (integer < 0)
        {
            throw new ArgumentException("Integer must be positive");
        }
        
        Integer = integer;
        Decimal = @decimal;
    }

    public override string ToString() => Integer.ToString() + Decimal;
}