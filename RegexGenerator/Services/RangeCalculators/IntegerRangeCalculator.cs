using RegexGenerator.Models;
using RegexGenerator.Utilities;

namespace RegexGenerator.Services.RangeCalculators;

internal interface IIntegerRangeCalculator
{
    IEnumerable<IntegerRegexRange> CalculateRanges(int min, int max);
}

internal class IntegerRangeCalculator : IIntegerRangeCalculator
{
    /// <summary>
    /// Returns regex-able number ranges between (inclusive) the min and max parameters in ascending order.
    /// </summary>
    public IEnumerable<IntegerRegexRange> CalculateRanges(int min, int max)
    {
        if (min < 0 || max < 0)
        {
            throw new ArgumentException("min and max must be zero or greater");
        }
        
        var lowerRanges = new List<IntegerRegexRange>();
        var upperRanges = new List<IntegerRegexRange>();
        
        for (var i = 0; min <= max; i++)
        { 
            var bottomRange = SplitLower(i, min);
            var topRange = SplitUpper(i, max);

            //TODO try to write a test case where this if is necessary
            if (topRange != null && bottomRange?.Max == topRange.Min - 1)
            {
                return lowerRanges
                    .Append(bottomRange)
                    .Append(topRange)
                    .Concat(upperRanges);
            }
            
            if (bottomRange?.Max >= topRange?.Min)
            {
                var intersection = new IntegerRegexRange
                {
                    Min = bottomRange.Min,
                    Max = topRange.Max
                };

                return lowerRanges
                    .Append(intersection)
                    .Concat(upperRanges);
            }

            if (bottomRange != null)
            {
                min = bottomRange.Max + 1;
                lowerRanges.Add(bottomRange);
            }

            if (topRange != null)
            {
                max = topRange.Min - 1;
                upperRanges.Insert(0, topRange);
            }
        }

        return lowerRanges.Concat(upperRanges);
    }

    private static IntegerRegexRange? SplitLower(int index, int min)
    {
        if (min != 0 && min.DigitAt(index) == 0)
        {
            return null;
        }
        
        return new IntegerRegexRange
        {
            Min = min,
            Max = min.Nines(index)
        };
    }

    private static IntegerRegexRange? SplitUpper(int index, int max)
    {
        var maxStr = max.ToString();

        if (index != maxStr.Length - 1 && max.DigitAt(index) == 9)
        {
            return null;
        }
        
        return new IntegerRegexRange
        {
            Min = max.Zeros(index),
            Max = max
        };
    }
}