using System.Numerics;
using RegexGenerator.Interfaces;
using RegexGenerator.Models.Numeric;
using RegexGenerator.Utilities;

namespace RegexGenerator.Services.RangeCalculators;

internal class UnsignedIntegerRangeCalculator<TInt> : IRangeCalculator<TInt, TInt> where TInt : INumber<TInt>
{
    /// <summary>
    /// Returns regex-able number ranges between (inclusive) the min and max positive integer parameters in ascending order.
    /// </summary>
    public IEnumerable<Range<TInt>> CalculateRegexRanges<TNumberSystem>(TInt min, TInt max)
        where TNumberSystem : INumberSystem
    {
        if (min < TInt.Zero || max < TInt.Zero)
        {
            throw new ArgumentException("min and max must be zero or greater");
        }
        
        var lowerRanges = new List<Range<TInt>>();
        var upperRanges = new List<Range<TInt>>();
        
        for (var i = 0; min <= max; i++)
        {
            var bottomRange = SplitLower<TNumberSystem>(i, min);
            var topRange = SplitUpper<TNumberSystem>(i, max);
            
            //TODO try to write a test case to test if this is necessary
            if (topRange != null && bottomRange != null && bottomRange.Max == topRange.Min - TInt.One)
            {
                return lowerRanges
                    .Append(bottomRange)
                    .Append(topRange)
                    .Concat(upperRanges);
            }
            
            if (bottomRange != null && topRange != null && bottomRange.Max >= topRange.Min)
            {
                var intersection = new Range<TInt>(bottomRange.Min, topRange.Max);

                return lowerRanges
                    .Append(intersection)
                    .Concat(upperRanges);
            }

            if (bottomRange != null)
            {
                min = bottomRange.Max + TInt.One;
                lowerRanges.Add(bottomRange);
            }

            if (topRange != null)
            {
                max = topRange.Min - TInt.One;
                upperRanges.Insert(0, topRange);
            }
        }

        return lowerRanges.Concat(upperRanges);
    }

    private static Range<TInt>? SplitLower<TNumberSystem>(int index, TInt min) where TNumberSystem : INumberSystem
    {
        if (min != TInt.Zero && min.DigitAt<TInt, TNumberSystem>(index) == TInt.Zero)
        {
            return null;
        }

        var max = min.Nines<TInt, TNumberSystem>(index);
        return new (min, max);
    }

    private static Range<TInt>? SplitUpper<TNumberSystem>(int index, TInt max) where TNumberSystem : INumberSystem
    {
        //TODO avoid string conversion
        var maxStr = max.ToString();
        var nine = TInt.CreateChecked(TNumberSystem.Radix - 1);

        if (index != maxStr.Length - 1 && max.DigitAt<TInt, TNumberSystem>(index) == nine) 
        {
            return null;
        }

        var min = max.Zeros<TInt, TNumberSystem>(index);
        return new (min, max);
    }
}