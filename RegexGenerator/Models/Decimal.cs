namespace RegexGenerator.Models
{
    internal class Decimal
    {
        public int Value { get; }

        public int LeadingZeros { get; }

        public Decimal(int value, int leadingZeros)
        {
            Value = value;
            LeadingZeros = leadingZeros;
        }

        public override string ToString() => $".{new string('0', LeadingZeros)}{Value}";
    }
}
