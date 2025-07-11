namespace RegexGenerator.Models;

internal sealed class RegexFractionalRange
{
    public UnsignedRegexFractional Min { get; }

    public UnsignedRegexFractional Max { get; }

    public RegexFractionalRange(UnsignedRegexFractional min, UnsignedRegexFractional max)
    {
        Min = min;
        Max = max;
    }

    public override string ToString() => $"({Min}, {Max})";
}