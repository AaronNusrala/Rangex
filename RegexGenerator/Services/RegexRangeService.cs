using RegexGenerator.Enumerations;
using RegexGenerator.Models;
using RegexGenerator.Models.Input;
using RegexGenerator.Services.RangeCalculators;

namespace RegexGenerator.Services;

internal interface IRegexRangeService
{
    IEnumerable<RegexRange> GetRegexRanges(InputRange inputRange);
}

internal class RegexRangeService(
    IIntegerRangeCalculator integerRangeCalculator,
    IDecimalRangeCalculator decimalRangeCalculator)
    : IRegexRangeService
{
    public RegexRangeService() : this(
        new UnsignedIntegerRangeCalculator(), 
        new UnsignedFractionalRangeCalculator()) { }
    
    public IEnumerable<RegexRange> GetRegexRanges(InputRange inputRange)
    {
        var (min, max) = (inputRange.Min, inputRange.Max);
        
        // Integers only, ex. (1, 2)
        if (min.Fractional == null && max.Fractional == null)
        {
            return GetIntegerRanges(min, max);
        }
        
        //from here on, we're dealing with a mix of decimals and integers
        
        // (1.5, 1.8)
        if (min.Integer == max.Integer && min.IsNegative == max.IsNegative)
        {
            return min.IsNegative
                ? GetDecimalRegexRanges(min, max.Fractional, min.Fractional, false)
                : GetDecimalRegexRanges(min, min.Fractional, max.Fractional, false);
        }
        
        //-1.05 -> .0, .05 
        //1.05 -> .05, .9
        var lowerDecimalRanges = min.IsNegative
            ? GetDecimalRegexRanges(min, UnsignedRegexFractional.Zero, min.Fractional, false).Reverse()
            : GetDecimalRegexRanges(min, min.Fractional, new UnsignedRegexFractional(0, 9), true);

        // -1.05 -> .05, .9
        // 1.05 -> .0, .05
        var upperDecimalRanges = max.IsNegative
            ? GetDecimalRegexRanges(max, max.Fractional, new UnsignedRegexFractional(0, 9), true).Reverse()
            : GetDecimalRegexRanges(max, UnsignedRegexFractional.Zero, max.Fractional, false);
        
        if(min.IsNegative && (max.IsNegative || max.Integer == 0) && min.Integer - 1 == max.Integer)
        {
            return lowerDecimalRanges.Concat(upperDecimalRanges);
        }

        if (!min.IsNegative && !max.IsNegative && min.Integer + 1 == max.Integer)
        {
            return lowerDecimalRanges.Concat(upperDecimalRanges);
        }

        var newMinInteger = min.IsNegative
            ? Math.Max(min.Integer - 1, 0)
            : min.Integer + 1;
        
        var newMaxInteger = max.IsNegative
            ? max.Integer + 1
            : Math.Max(max.Integer - 1, 0);
        
        var integerRegexRanges = Enumerable.Empty<RegexRange>();

        if (newMinInteger != 0 && newMaxInteger != 0)
        {
            min = new InputNumber(min.IsNegative, newMinInteger, min.Fractional);
            max = new InputNumber(max.IsNegative, newMaxInteger, max.Fractional);
            integerRegexRanges = GetIntegerRanges(min, max);
        }
        
        return lowerDecimalRanges
            .Concat(integerRegexRanges)
            .Concat(upperDecimalRanges);
    }

    private IEnumerable<RegexRange> GetDecimalRegexRanges(InputNumber i, UnsignedRegexFractional min, UnsignedRegexFractional max, bool allowTrailingDecimals)
    {
        return decimalRangeCalculator.GetRanges(min, max, allowTrailingDecimals)
            .Select(r => 
                new RegexRange
                {
                    Sign = i.IsNegative ? Sign.Negative : Sign.Positive,
                    Min = new RegexDecimal(i.Integer, r.Min),
                    Max = new RegexDecimal(i.Integer, r.Max)
                });
    }
    
    /* TODO if we knew which decimal was greater regardless of the integer, we could apply the same optimizations to decimals that
    we are using for integers. Future enhancement. (1.5, 2.7) -> ((1|2).[0-5])|(2.[5-7]).  This example doesn't save anything
    but gives you the idea.*/
    private IEnumerable<RegexRange> GetIntegerRanges(InputNumber min, InputNumber max)
    {
        if (min.IsNegative && max.IsNegative)
        {
            return GetIntegerRegexRanges(max.Integer, min.Integer, Sign.Negative).Reverse();
        }

        if (!min.IsNegative && !max.IsNegative)
        {
            return GetIntegerRegexRanges(min.Integer, max.Integer, Sign.Positive);
        }
            
        //here we know that the signs are opposite.
        //(-1, 1) -> +-(0, 1)
        if (min.Integer == max.Integer)
        {
            return GetIntegerRegexRanges(0, min.Integer, Sign.PositiveOrNegative);
        }

        //(-2, 1) -> +-(0, 1), -(1, 2)
        if (min.Integer > max.Integer)
        {
            var lowerRanges = GetIntegerRegexRanges(max.Integer, min.Integer + 1, Sign.Negative).Reverse();
            var upperRanges = GetIntegerRegexRanges(0, max.Integer, Sign.PositiveOrNegative);
            return lowerRanges.Concat(upperRanges);
        }
            
        //(-1, 2) -> +-(0, 1), +(1, 2)
        if (min.Integer < max.Integer)
        {
            var lowerRanges = GetIntegerRegexRanges(0, min.Integer, Sign.PositiveOrNegative);
            var upperRanges = GetIntegerRegexRanges(min.Integer + 1, max.Integer, Sign.Positive);
            return lowerRanges.Concat(upperRanges);
        }

        throw new Exception("shouldn't happen");
    }

    private IEnumerable<RegexRange> GetIntegerRegexRanges(int min, int max, Sign sign)
    {
        return integerRangeCalculator.CalculateRanges(min, max)
            .Select(r => new RegexRange
            {
                Sign = sign, 
                Min = new RegexDecimal(r.Min, UnsignedRegexFractional.Zero),
                Max = new RegexDecimal(r.Max, UnsignedRegexFractional.Zero)
            });
    }
}