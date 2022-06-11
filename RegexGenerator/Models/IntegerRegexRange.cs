namespace RegexGenerator.Models
{
    internal sealed class IntegerRegexRange
    {
        public int Min { get; init; }

        public int Max { get; init; }

        public void Deconstruct(out int min, out int max) => (min, max) = (Min, Max);

        public override string ToString() => $"({Min}, {Max})";
    }
}
