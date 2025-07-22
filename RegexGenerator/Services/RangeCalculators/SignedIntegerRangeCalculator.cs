using RegexGenerator.Enumerations;
using RegexGenerator.Interfaces;
using RegexGenerator.Models;

namespace RegexGenerator.Services.RangeCalculators;

internal class SignedIntegerRangeCalculator(UnsignedIntegerRangeCalculator integerRangeCalculator) : IRangeCalculator<int, int>
{
    /* TODO if we knew which decimal was greater regardless of the integer, we could apply the same optimizations to decimals that
    we are using for integers. Future enhancement. (1.5, 2.7) -> ((1|2).[0-5])|(2.[5-7]).  This example doesn't save anything
    but gives you the idea.*/
    
    public SignedIntegerRangeCalculator() : this(new UnsignedIntegerRangeCalculator()) { }
    
    public IEnumerable<Range<int>> CalculateRegexRanges(int min, int max)
    {
        if (min < 0 && max < 0)
        {
            return GetIntegerRegexRanges(max, min, RangeSign.Negative).Reverse();
        }

        if (min >= 0 && max >= 0)
        {
            return GetIntegerRegexRanges(min, max, RangeSign.Positive);
        }
            
        //here we know that the signs are opposite.
        //(-1, 1) -> +-(0, 1)
        if (min == max)
        {
            return GetIntegerRegexRanges(0, min, RangeSign.PositiveOrNegative);
        }

        //(-2, 1) -> +-(0, 1), -(1, 2)
        if (min > max)
        {
            var lowerRanges = GetIntegerRegexRanges(max, min + 1, RangeSign.Negative).Reverse();
            var upperRanges = GetIntegerRegexRanges(0, max, RangeSign.PositiveOrNegative);
            return lowerRanges.Concat(upperRanges);
        }
            
        //(-1, 2) -> +-(0, 1), +(1, 2)
        if (min < max)
        {
            var lowerRanges = GetIntegerRegexRanges(0, min, RangeSign.PositiveOrNegative);
            var upperRanges = GetIntegerRegexRanges(min + 1, max, RangeSign.Positive);
            return lowerRanges.Concat(upperRanges);
        }

        throw new Exception("shouldn't happen");
    }

    private IEnumerable<Range<int>> GetIntegerRegexRanges(int min, int max, RangeSign rangeSign)
    {
        foreach (var range in integerRangeCalculator.CalculateRegexRanges(min, max))
        {
            range.Sign = rangeSign;
            yield return range;
        }
    }
}