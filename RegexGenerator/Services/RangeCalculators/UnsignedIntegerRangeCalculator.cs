using RegexGenerator.Interfaces;
using RegexGenerator.Models;

namespace RegexGenerator.Services.RangeCalculators;

internal class UnsignedIntegerRangeCalculator : IRangeCalculator<int, int>
{
    /// <summary>
    /// Returns regex-able number ranges between (inclusive) the min and max positive integer parameters in ascending order.
    /// </summary>
    public IEnumerable<Range<int>> CalculateRegexRanges(IInteger min, IInteger max)
    {
        if (min < 0 || max < 0)
        {
            throw new ArgumentException("min and max must be zero or greater");
        }
        
        var lowerRanges = new List<Range<int>>();
        var upperRanges = new List<Range<int>>();
        
        for (var i = 0; min <= max; i++)
        { 
            var bottomRange = SplitLower(i, min);
            var topRange = SplitUpper(i, max);

            //TODO try to write a test case to test if this is necessary
            if (topRange != null && bottomRange?.Max == topRange.Min - 1)
            {
                return lowerRanges
                    .Append(bottomRange)
                    .Append(topRange)
                    .Concat(upperRanges);
            }
            
            if (bottomRange?.Max >= topRange?.Min)
            {
                var intersection = new Range<int>(bottomRange.Min, topRange.Max);

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

    private static Range<int>? SplitLower(int index, IInteger min)
    {
        if (min != 0 && min.DigitAt(index) == 0)
        {
            return null;
        }

        return new Range<int>(min, min.Nines(index));
    }

    private static Range<int>? SplitUpper(int index, int max)
    {
        var maxStr = max.ToString();

        if (index != maxStr.Length - 1 && max.DigitAt(index) == 9)
        {
            return null;
        }

        return new Range<int>(max.Zeros(index), max);
    }
}