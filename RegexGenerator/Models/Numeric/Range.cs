using RegexGenerator.Enumerations;

namespace RegexGenerator.Models;

public class Range<TNumeric>
{
    public RangeSign? Sign { get; set; }
    
    public TNumeric Min { get; init; }
    
    public TNumeric Max { get; init; }

    public Range(TNumeric min, TNumeric max, RangeSign? sign = null)
    {
        Sign = sign;
        Min = min;
        Max = max;
    }

    public void Deconstruct(out TNumeric min, out TNumeric max) => (min, max) = (Min, Max);

    public override string ToString()
    {
        var signString = Sign switch
        {
            RangeSign.Positive => "",
            RangeSign.Negative => "-",
            RangeSign.PositiveOrNegative => "+-",
            _ => throw new ArgumentOutOfRangeException()
        };

        return $"{signString}({Min}, {Max})";
    }
}