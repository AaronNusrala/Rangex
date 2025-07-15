using RegexGenerator.Enumerations;

namespace RegexGenerator.Models;

public class RegexRange
{
    public RangeSign RangeSign { get; init; }
    
    public required UnsignedRegexDecimal Min { get; init; }
    
    public required UnsignedRegexDecimal Max { get; init; }

    public override string ToString()
    {
        var signString = RangeSign switch
        {
            RangeSign.Positive => "",
            RangeSign.Negative => "-",
            RangeSign.PositiveOrNegative => "+-",
            _ => throw new ArgumentOutOfRangeException()
        };

        return $"{signString}({Min}, {Max})";
    }
}