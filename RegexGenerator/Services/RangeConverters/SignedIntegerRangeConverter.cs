using RegexGenerator.Enumerations;
using RegexGenerator.Interfaces;
using RegexGenerator.Models;
using RegexGenerator.Models.Regex;

namespace RegexGenerator.Services.RangeConverters;

internal class SignedIntegerRangeConverter : IRangeConverter<int, SignedRegexIntegerRange>
{
    public SignedRegexIntegerRange ConvertToRegexRanges(Range<int> range)
    {
        var minString = range.Min.ToString();
        var maxString = range.Max.ToString();

        var prefix = new List<char>();
        var suffix = new List<RegexCharacterClass>();

        for (var i = 0; i < minString.Length; i++)
        {
            var minChar = minString[i];
            var maxChar = maxString[i];

            if (minChar == maxChar)
            {
                prefix.Add(minChar);
            }
            else
            {
                var characterClass = new RegexCharacterClass
                {
                    Start = minChar,
                    End = maxChar
                };

                suffix.Add(characterClass);
            }
        }

        return new()
        {
            Sign = range.Sign ?? RangeSign.Positive,
            Prefix = prefix,
            Suffix = suffix
        };
    }
}