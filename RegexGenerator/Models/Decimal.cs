namespace RegexGenerator.Models
{
    internal class Decimal
    {
        public int Value { get; init; }

        public int LeadingZeros { get; init; }

        public override string ToString() => $".{new string('0', LeadingZeros)}{Value}";
    }
}
