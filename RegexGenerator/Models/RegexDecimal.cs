using RegexGenerator.Utilities;

namespace RegexGenerator.Models
{
    public sealed class RegexDecimal
    {
        private int? _valueMagnitude;
        public static RegexDecimal Zero => new(0, 0);

        public int Value { get; }

        public int LeadingZeros { get; }

        public int ValueMagnitude => _valueMagnitude ??= Value.GetMagnitude();

        public RegexDecimal(int leadingZeros, int value)
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
