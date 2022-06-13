namespace RegexGenerator.Models.Input;

public class InputNumber
{
    public bool IsNegative { get; init; }

    public int Integer { get; init; }
    
    public RegexDecimal? Decimal { get; init; }

    public InputNumber(bool isNegative, int integer, RegexDecimal? @decimal)
    {
        if (integer < 0)
        {
            throw new Exception("Integer cannot be less than 0");
        }
        
        IsNegative = isNegative;
        Integer = integer;
        Decimal = @decimal;
    }

    public override string ToString() => (IsNegative ? "-" : "") + Integer + Decimal;
}