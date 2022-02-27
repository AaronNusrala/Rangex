namespace RegexGenerator.Models
{
    internal class DecimalRegexRange
    {
        public Decimal Min { get; init; }

        public Decimal Max { get; init; }

        public override string ToString() => $"({Min}, {Max})";
    }
}
