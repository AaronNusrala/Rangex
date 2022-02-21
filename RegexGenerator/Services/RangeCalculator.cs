using RegexGenerator.Models;

namespace RegexGenerator.Services;

internal interface IRangeCalculator
{
    IEnumerable<RegexRange> CalculateRanges(int min, int max);
}

internal class RangeCalculator : IRangeCalculator
{
    public IEnumerable<RegexRange> CalculateRanges(int min, int max)
    {
        for (var i = 0; min <= max; i++)
        {
            var bottomRange = SplitLower(i, min);
            var topRange = SplitUpper(i, max);

            if (bottomRange?.Max > topRange?.Min)
            {
                yield return new RegexRange
                {
                    Min = bottomRange.Min,
                    Max = topRange.Max
                };

                yield break;
            }

            if (bottomRange != null)
            {
                min = bottomRange.Max + 1;
                yield return bottomRange;
            }

            if (topRange != null)
            {
                max = topRange.Min - 1;
                yield return topRange;
            }
        }
    }

    private static RegexRange? SplitLower(int index, int min)
    {
        var minStr = min.ToString();

        if (min != 0 && minStr[minStr.Length - 1 - index] == '0')
        {
            return null;
        }

        var rangeTopString = ReplaceEnd(minStr, '9', index);

        return new RegexRange
        {
            Min = min,
            Max = int.Parse(rangeTopString)
        };
    }

    private static RegexRange? SplitUpper(int index, int max)
    {
        var maxStr = max.ToString();

        if (index != maxStr.Length - 1 && maxStr[maxStr.Length - 1 - index] == '9')
        {
            return null;
        }

        var rangeBottomString = ReplaceEnd(maxStr, '0', index);

        return new RegexRange
        {
            Min = int.Parse(rangeBottomString),
            Max = max
        };
    }

    private static string ReplaceEnd(string value, char replacementValue, int index)
    {
        var valueCharacters = value.Substring(0, value.Length - 1 - index);
        var replacementCharacters = new string(replacementValue, index + 1);
        return valueCharacters + replacementCharacters;
    }
}