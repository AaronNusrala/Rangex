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
        new SignedIntegerRangeCalculator(), 
        new UnsignedFractionalRangeCalculator()) { }
    
    public IEnumerable<RegexRange> GetRegexRanges(InputRange inputRange)
    {
        var (min, max) = (inputRange.Min, inputRange.Max);
        
        // Integers only, ex. (1, 2)
        if (min.Fractional == null && max.Fractional == null)
        {
            return integerRangeCalculator.CalculateRanges(min.Integer, max.Integer);
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
            integerRegexRanges = integerRangeCalculator.CalculateRanges(min.Integer, max.Integer);
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
                    RangeSign = i.IsNegative ? RangeSign.Negative : RangeSign.Positive,
                    Min = new UnsignedRegexDecimal(i.Integer, r.Min),
                    Max = new UnsignedRegexDecimal(i.Integer, r.Max)
                });
    }
    

}