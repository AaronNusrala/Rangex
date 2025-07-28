using System.Numerics;
using RegexGenerator.Enumerations;
using RegexGenerator.Interfaces;
using RegexGenerator.Models.Numeric;

namespace RegexGenerator.Services.RangeCalculators;

internal class SignedIntegerRangeCalculator<TInt>(UnsignedIntegerRangeCalculator<TInt> integerRangeCalculator)
    : IRangeCalculator<TInt, TInt> where TInt : INumber<TInt>
{
    /* TODO if we knew which decimal was greater regardless of the integer, we could apply the same optimizations to decimals that
    we are using for integers. Future enhancement. (1.5, 2.7) -> ((1|2).[0-5])|(2.[5-7]).  This example doesn't save anything
    but gives you the idea.*/
    
    public IEnumerable<Range<TInt>> CalculateRegexRanges<TNumberSystem>(TInt min, TInt max) where TNumberSystem : INumberSystem
    {
        if (min < TInt.Zero && max < TInt.Zero)
        {
            return GetIntegerRegexRanges<TNumberSystem>(max, min, RangeSign.Negative).Reverse();
        }

        if (!(min < TInt.Zero) && !(max < TInt.Zero))
        {
            return GetIntegerRegexRanges<TNumberSystem>(min, max, RangeSign.Positive);
        }
        
        //here we know that the signs are opposite.
        //(-1, 1) -> +-(0, 1)
        if (min == max)
        {
            return GetIntegerRegexRanges<TNumberSystem>(TInt.Zero, min, RangeSign.PositiveOrNegative);
        }

        //(-2, 1) -> +-(0, 1), -(1, 2)
        if (min > max)
        {
            var lowerRanges = GetIntegerRegexRanges<TNumberSystem>(max, min + TInt.One, RangeSign.Negative).Reverse();
            var upperRanges = GetIntegerRegexRanges<TNumberSystem>(TInt.Zero, max, RangeSign.PositiveOrNegative);
            return lowerRanges.Concat(upperRanges);
        }
            
        //(-1, 2) -> +-(0, 1), +(1, 2)
        if (min < max)
        {
            var lowerRanges = GetIntegerRegexRanges<TNumberSystem>(TInt.Zero, min, RangeSign.PositiveOrNegative);
            var upperRanges = GetIntegerRegexRanges<TNumberSystem>(min + TInt.One, max, RangeSign.Positive);
            return lowerRanges.Concat(upperRanges);
        }

        throw new Exception("shouldn't happen");
    }

    private IEnumerable<Range<TInt>> GetIntegerRegexRanges<TNumberSystem>(TInt min, TInt max, RangeSign rangeSign)
        where TNumberSystem : INumberSystem
    {
        foreach (var range in integerRangeCalculator.CalculateRegexRanges<TNumberSystem>(min, max))
        {
            range.Sign = rangeSign;
            yield return range;
        }
    }
}