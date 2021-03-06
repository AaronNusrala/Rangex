using RegexGenerator.Models;

namespace RegexGenerator.Services;

internal interface IRangesToRegexConverter
{
    string ConvertRanges(IEnumerable<RegexRange> ranges);
}

internal class RangesToRegexConverter : IRangesToRegexConverter
{
    private readonly IRegexBuilder _regexBuilder;

    public RangesToRegexConverter(IRegexBuilder regexBuilder)
    {
        _regexBuilder = regexBuilder;
    }

    public RangesToRegexConverter() : this(new RegexBuilder()) { }

    public string ConvertRanges(IEnumerable<RegexRange> ranges)
    {
        // using var enumerator = ranges.GetEnumerator();
        // var moreRanges = enumerator.MoveNext();
        //
        // _regexBuilder
        //     .BeginString()
        //     .Group();
        //
        // while (moreRanges)
        // {
        //     var (rangeMin, rangeMax) = enumerator.Current;
        //     var minString = rangeMin.ToString();
        //     var maxString = rangeMax.ToString();
        //
        //     for (var i = 0; i < minString.Length; i++)
        //     {
        //         var minChar = minString[i];
        //         var maxChar = maxString[i];
        //
        //         if (minChar == maxChar)
        //         {
        //             _regexBuilder.MatchLiteralCharacter(minChar);
        //         }
        //         else
        //         {
        //             _regexBuilder.CharacterClassRange(minChar, maxChar);
        //         }
        //     }
        //
        //     // ReSharper disable once AssignmentInConditionalExpression, this is intentional
        //     if (moreRanges = enumerator.MoveNext())
        //     {
        //         _regexBuilder.Or();
        //     }
        // }

        return _regexBuilder
            .EndGroup()
            .EndString()
            .ToRegex();
    }
}