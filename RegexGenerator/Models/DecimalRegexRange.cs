namespace RegexGenerator.Models
{
    internal class DecimalRegexRange
    {
        public Decimal Min { get; init; }

        public Decimal Max { get; init; }

        public DecimalRegexRange(Decimal min, Decimal max)
        {
            Min = min;
            Max = max;
        }

        public override string ToString() => $"({Min}, {Max})";
    }
}
