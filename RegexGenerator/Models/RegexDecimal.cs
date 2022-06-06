namespace RegexGenerator.Models
{
    internal sealed class RegexDecimal
    {
        public int Value { get; }

        public int LeadingZeros { get; }

        public RegexDecimal(int value, int leadingZeros)
        {
            Value = value;
            LeadingZeros = leadingZeros;
        }

        public override string ToString() => $".{new string('0', LeadingZeros)}{Value}";
    }
}
