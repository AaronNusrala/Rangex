namespace RegexGenerator.Models;

public class RegexNumber
{
    public bool IsNegative { get; init; }
    
    public int Integer { get; init; }
    
    public RegexDecimal? Decimal { get; init; }

    public override string ToString() => (IsNegative ? "-" : "") + Integer + Decimal;
}