using RegexGenerator.Models;
using RegexGenerator.Utilities;

namespace RegexGenerator.Services.RangeCalculators;

internal interface IDecimalRangeCalculator
{
    IEnumerable<RegexFractionalRange> GetRanges(UnsignedRegexFractional min, UnsignedRegexFractional max, bool allowTrailingDecimals);
}

/// <summary>
/// Calculates regex-able fractional ranges
/// </summary>
internal class UnsignedFractionalRangeCalculator : IDecimalRangeCalculator
{
    public IEnumerable<RegexFractionalRange> GetRanges(UnsignedRegexFractional min, UnsignedRegexFractional max, bool allowTrailingDecimals)
    {
        if (min.Value == 0)
        {
            min = UnsignedRegexFractional.Zero;
        }

        if (max.Value == 0)
        {
            max = UnsignedRegexFractional.Zero;
        }
        else
        {
            var newMaxValue = max.Value.TrimTrailingZeros();
            max = new UnsignedRegexFractional(max.LeadingZeros, newMaxValue);
        }
            
        if (min.LeadingZeros == max.LeadingZeros && min.Value == max.Value)
        {
            return [new RegexFractionalRange(min, max)];
        }
            
        min = NormalizeMagnitude(min, max);

        var initialRange = allowTrailingDecimals
            ? CompleteRangeFromMax(max)
            : new RegexFractionalRange(max, max);
            
        var upperRanges = GetUpperRanges(initialRange.Min);
        var lowerRanges = new List<RegexFractionalRange>();
        var lowerRange = CompleteRangeFromMin(min);

        while (true)
        {
            if (lowerRange.Min.LeadingZeros == initialRange.Min.LeadingZeros && 
                lowerRange.Min.Value.TrimTrailingZeros() == initialRange.Min.Value.TrimTrailingZeros())
            {
                return lowerRanges.Append(initialRange);
            }

            var intersection = GetIntersection(lowerRange, initialRange);

            if (intersection != null && allowTrailingDecimals)
            {
                return lowerRanges.Append(intersection);
            }
                
            for (var i = upperRanges.Count - 1; i >= 0; i--)
            {
                var upperRange = upperRanges[i]; 
                intersection = GetIntersection(lowerRange, upperRange);

                if (intersection != null)
                {
                    return lowerRanges
                        .Append(intersection)
                        .Concat(upperRanges.SkipWhile((_, index) => index <= i))
                        .Append(initialRange);
                }
                    
                //I have no idea what this means, but apparently I wrote it.
                if (lowerRange.Max.Value == upperRange.Min.Value - 1
                    && (upperRange.Min.LeadingZeros == lowerRange.Min.LeadingZeros && lowerRange.Max.ValueMagnitude == upperRange.Min.ValueMagnitude
                        || lowerRange.Max.LeadingZeros - upperRange.Min.LeadingZeros == 1 && lowerRange.Max.ValueMagnitude == upperRange.Max.ValueMagnitude - 1))
                {
                    return lowerRanges
                        .Append(lowerRange)
                        .Concat(upperRanges.SkipWhile((_, index) => index < i))
                        .Append(initialRange); 
                }
            }

            lowerRanges.Add(lowerRange);
            lowerRange = GetNextHigherRange(lowerRange.Max);
        }
    }

    private static UnsignedRegexFractional NormalizeMagnitude(UnsignedRegexFractional min, UnsignedRegexFractional max)
    {
        var magnitudeDiff = max.ValueMagnitude - min.ValueMagnitude + (max.LeadingZeros - min.LeadingZeros);
            
        if (magnitudeDiff > 0)
        {
            var newMinValue = min.Value * 10.Pow(magnitudeDiff);
            min = new UnsignedRegexFractional(min.LeadingZeros, newMinValue);
        }
             
        if(magnitudeDiff < 0)
        {
            var newMinValue = min.Value.TrimTrailingZeros(-magnitudeDiff);
            min = new UnsignedRegexFractional(min.LeadingZeros, newMinValue);
        }

        return min;
    }

    private static List<RegexFractionalRange> GetUpperRanges(UnsignedRegexFractional max)
    {
        var upperRanges = new List<RegexFractionalRange>();
            
        while (max.Value > 0)
        {
            var upperRange = GetNextLowerRange(max);
            upperRanges.Insert(0, upperRange);
            max = upperRange.Min;
        }
            
        return upperRanges;
    }
        
    private static RegexFractionalRange GetNextLowerRange(UnsignedRegexFractional previousMin)
    {
        if (previousMin.Value == 0)
        {
            //If this happens, there is a bug.
            throw new Exception($"Can't go any lower than {previousMin}.");
        }
            
        var nextMaxValue = previousMin.Value.TrimTrailingZeros(1);
        var nextMaxLeadingZeros = nextMaxValue.TrimTrailingZeros() == 1 ? previousMin.LeadingZeros + 1 : previousMin.LeadingZeros;
        nextMaxValue = nextMaxValue == 1 ? 9 : nextMaxValue - 1;
        var nextMax = new UnsignedRegexFractional(nextMaxLeadingZeros, nextMaxValue);
        return CompleteRangeFromMax(nextMax);
    }

    private static RegexFractionalRange GetNextHigherRange(UnsignedRegexFractional previousMax)
    {
        //TODO stop converting to string here.
        if (previousMax.Value.ToString().All(c => c == '9') && previousMax.LeadingZeros == 0)
        {
            //If this happens, there is a bug.
            throw new Exception($"Can't go any higher than {previousMax}.");
        }
            
        var nextMaxValue = previousMax.Value + 1;
            
        var nextMaxLeadingZeros = nextMaxValue.GetMagnitude() > previousMax.ValueMagnitude 
            ? previousMax.LeadingZeros - 1 
            : previousMax.LeadingZeros;
            
        nextMaxValue = nextMaxValue.TrimTrailingZeros(1);
        var nextMin = new UnsignedRegexFractional(nextMaxLeadingZeros, nextMaxValue);
        return CompleteRangeFromMin(nextMin);
    }

    private static RegexFractionalRange CompleteRangeFromMin(UnsignedRegexFractional min)
    {
        var maxValue = min.Value - min.Value % 10 + 9;
        var max = new UnsignedRegexFractional(min.LeadingZeros, maxValue);
        return new RegexFractionalRange(min, max);
    }

    private static RegexFractionalRange CompleteRangeFromMax(UnsignedRegexFractional max)
    {
        var newMinValue = max.Value - max.Value % 10;
        var nextMin = new UnsignedRegexFractional(max.LeadingZeros, newMinValue);
        return new RegexFractionalRange(nextMin, max);
    }

    /// <summary>
    /// This only works because we can make some assumptions about regex-able decimal ranges.
    /// In order for a range to regex-able, we assume the following are true:
    /// 1. The min and max must always have the same number of leading zeros
    /// 2. The values must always have the same number of digits.
    /// </summary>
    /// <returns>A DecimalRegexRange that represents the intersection of the two range parameters</returns>
    private static RegexFractionalRange? GetIntersection(RegexFractionalRange r1, RegexFractionalRange r2)
    {
        if (r1.Min.LeadingZeros == r2.Min.LeadingZeros && r1.Min.Value <= r2.Max.Value && r2.Min.Value <= r1.Max.Value)
        {
            return new RegexFractionalRange(r1.Min, r2.Max);
        }
            
        if (r1.Min.Value == 0 && r2.Min.Value == 0)
        {
            return r1.Min.LeadingZeros > r2.Min.LeadingZeros 
                ? new RegexFractionalRange(r1.Min, r1.Max)
                : new RegexFractionalRange(r2.Min, r2.Max);
        }

        return null;
    }
}