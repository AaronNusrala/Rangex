using RegexGenerator.Services;

namespace RegexGenerator
{
    internal class NumberRangeRegexGenerator
    {
        private readonly IRangeCalculator _rangeCalculator;
        private readonly IRegexBuilder _regexBuilder;

        public NumberRangeRegexGenerator(IRangeCalculator rangeCalculator, IRegexBuilder regexBuilder)
        {
            _rangeCalculator = rangeCalculator;
            _regexBuilder = regexBuilder;
        }

        public string GenerateRegex(int min, int max)
        {
            var ranges = _rangeCalculator
                .CalculateRanges(min, max)
                .OrderBy(r => r.Min);

            using var enumerator = ranges.GetEnumerator();
            var moreRanges = enumerator.MoveNext();

            _regexBuilder
                .BeginString()
                .Group();

            while (moreRanges)
            {
                var (rangeMin, rangeMax) = enumerator.Current;
                var minString = rangeMin.ToString();
                var maxString = rangeMax.ToString();

                for (var i = 0; i < minString.Length; i++)
                {
                    var minChar = minString[i];
                    var maxChar = maxString[i];

                    if (minChar == maxChar)
                    {
                        _regexBuilder.MatchLiteralCharacter(minChar);
                    }
                    else
                    {
                        _regexBuilder.CharacterClassRange(minChar, maxChar);
                    }
                }

                // ReSharper disable once AssignmentInConditionalExpression, this is intentional
                if (moreRanges = enumerator.MoveNext())
                {
                    _regexBuilder.Or();
                }
            }

            return _regexBuilder
                .EndGroup()
                .EndString()
                .ToRegex();
        }
    }
}
