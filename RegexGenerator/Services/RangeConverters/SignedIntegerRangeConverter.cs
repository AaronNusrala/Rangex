using System.Numerics;
using RegexGenerator.Enumerations;
using RegexGenerator.Interfaces;
using RegexGenerator.Models.Numeric;
using RegexGenerator.Models.Regex;

namespace RegexGenerator.Services.RangeConverters;

internal class SignedIntegerRangeConverter<TInt> : IRangeConverter<TInt, SignedRegexIntegerRange> where TInt : INumber<TInt>
{
    public SignedRegexIntegerRange ConvertToRegexRanges<TNumberSystem>(Range<TInt> range) where TNumberSystem : INumberSystem
    {
        var minString = TNumberSystem.ToNumberSystemString(range.Min);
        var maxString = TNumberSystem.ToNumberSystemString(range.Max);

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