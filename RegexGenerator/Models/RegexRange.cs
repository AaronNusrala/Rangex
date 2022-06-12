using RegexGenerator.Enumerations;

namespace RegexGenerator.Models;

public class RegexRange
{
    public Sign Sign { get; init; }
    
    public RegexNumber Min { get; init; }
    
    public RegexNumber Max { get; init; }

    public override string ToString()
    {
        var signString = Sign switch
        {
            Sign.Positive => "",
            Sign.Negative => "-",
            Sign.PositiveOrNegative => "+-",
            _ => throw new ArgumentOutOfRangeException()
        };

        return $"{signString}({Min}, {Max})";
    }
}