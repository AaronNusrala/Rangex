namespace RegexGenerator.Models
{
    internal sealed class RegexDecimal
    {
        public static RegexDecimal Zero => new(0, 0);
        
        public int Value { get; }

        public int LeadingZeros { get; }

        public RegexDecimal(int value, int leadingZeros)
        {
            if (value < 0 || leadingZeros < 0)
            {
                throw new Exception();
            }
            
            Value = value;
            LeadingZeros = leadingZeros;
        }

        public override string ToString() => $".{new string('0', LeadingZeros)}{Value}";
    }
}
