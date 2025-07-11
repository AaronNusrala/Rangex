namespace RegexGenerator.Models.Input;

public class InputNumber
{
    public bool IsNegative { get; init; }

    public int Integer { get; init; }
    
    public UnsignedRegexFractional? Fractional { get; init; }
    
    public static InputNumber Zero => new InputNumber(false, 0, null);

    public InputNumber(bool isNegative, int integer, UnsignedRegexFractional? fractional)
    {
        if (integer < 0)
        {
            throw new Exception("Integer cannot be less than 0");
        }
        
        IsNegative = isNegative;
        Integer = integer;
        Fractional = fractional;
    }

    public override string ToString() => (IsNegative ? "-" : "") + Integer + Fractional;
}