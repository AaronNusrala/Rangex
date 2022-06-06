namespace RegexGenerator.Models
{
    internal sealed class RegexDecimalRange
    {
        public RegexDecimal Min { get; }

        public RegexDecimal Max { get; }

        public RegexDecimalRange(RegexDecimal min, RegexDecimal max)
        {
            Min = min;
            Max = max;
        }

        public override string ToString() => $"({Min}, {Max})";
    }
}
