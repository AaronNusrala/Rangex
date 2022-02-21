namespace RegexGenerator.Models
{
    internal class RegexRange
    {
        public int Min { get; init; }

        public int Max { get; init; }

        public void Deconstruct(out int min, out int max) => (min, max) = (Min, Max);
    }
}
