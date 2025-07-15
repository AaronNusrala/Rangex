using RegexGenerator.Enumerations;
using RegexGenerator.Models;
using RegexGenerator.Models.Input;

namespace RegexGenerator.Services.RangeCalculators;

internal class SignedIntegerRangeCalculator(UnsignedIntegerRangeCalculator integerRangeCalculator)
{
    /* TODO if we knew which decimal was greater regardless of the integer, we could apply the same optimizations to decimals that
    we are using for integers. Future enhancement. (1.5, 2.7) -> ((1|2).[0-5])|(2.[5-7]).  This example doesn't save anything
    but gives you the idea.*/
    
    public SignedIntegerRangeCalculator() : this(new UnsignedIntegerRangeCalculator()) { }
    
    public IEnumerable<RegexRange> GetIntegerRanges(InputNumber min, InputNumber max)
    {
        if (min.IsNegative && max.IsNegative)
        {
            return GetIntegerRegexRanges(max.Integer, min.Integer, RangeSign.Negative).Reverse();
        }

        if (!min.IsNegative && !max.IsNegative)
        {
            return GetIntegerRegexRanges(min.Integer, max.Integer, RangeSign.Positive);
        }
            
        //here we know that the signs are opposite.
        //(-1, 1) -> +-(0, 1)
        if (min.Integer == max.Integer)
        {
            return GetIntegerRegexRanges(0, min.Integer, RangeSign.PositiveOrNegative);
        }

        //(-2, 1) -> +-(0, 1), -(1, 2)
        if (min.Integer > max.Integer)
        {
            var lowerRanges = GetIntegerRegexRanges(max.Integer, min.Integer + 1, RangeSign.Negative).Reverse();
            var upperRanges = GetIntegerRegexRanges(0, max.Integer, RangeSign.PositiveOrNegative);
            return lowerRanges.Concat(upperRanges);
        }
            
        //(-1, 2) -> +-(0, 1), +(1, 2)
        if (min.Integer < max.Integer)
        {
            var lowerRanges = GetIntegerRegexRanges(0, min.Integer, RangeSign.PositiveOrNegative);
            var upperRanges = GetIntegerRegexRanges(min.Integer + 1, max.Integer, RangeSign.Positive);
            return lowerRanges.Concat(upperRanges);
        }

        throw new Exception("shouldn't happen");
    }

    private IEnumerable<RegexRange> GetIntegerRegexRanges(int min, int max, RangeSign rangeSign)
    {
        return integerRangeCalculator.CalculateRanges(min, max)
            .Select(r => new RegexRange
            {
                RangeSign = rangeSign, 
                Min = new UnsignedRegexDecimal(r.Min, UnsignedRegexFractional.Zero),
                Max = new UnsignedRegexDecimal(r.Max, UnsignedRegexFractional.Zero)
            });
    }
}