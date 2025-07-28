using RegexGenerator.Enumerations;

namespace RegexGenerator.Models.Numeric;

public class Range<TNumeric>(TNumeric min, TNumeric max, RangeSign? sign = null)
{
    public RangeSign? Sign { get; set; } = sign;

    public TNumeric Min { get; init; } = min;

    public TNumeric Max { get; init; } = max;

    public void Deconstruct(out TNumeric min, out TNumeric max) => (min, max) = (Min, Max);

    public override string ToString()
    {
        var signString = Sign switch
        {
            RangeSign.Positive => "",
            RangeSign.Negative => "-",
            RangeSign.PositiveOrNegative => "+-",
            _ => "?" //Don't want ToString to throw, but you definitely did something weird if you see this.
        };

        return $"{signString}({Min}, {Max})";
    }
}