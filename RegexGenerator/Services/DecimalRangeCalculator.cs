using RegexGenerator.Models;
using Decimal = RegexGenerator.Models.Decimal;

namespace RegexGenerator.Services
{
    internal interface IDecimalRangeCalculator
    {
        IEnumerable<DecimalRegexRange> GetRanges(Decimal min, Decimal max);
    }

    internal class DecimalRangeCalculator : IDecimalRangeCalculator
    {
        public IEnumerable<DecimalRegexRange> GetRanges(Decimal min, Decimal max)
        {
            var lowerRanges = new List<DecimalRegexRange>();
            var upperRanges = new List<DecimalRegexRange>();
            
            var upperRange = new DecimalRegexRange(max, max);
            
            var newMinValue = TrimTrailingZeros(min.Value);
            min = new Decimal(newMinValue, min.LeadingZeros);

            var newMaxValue = TrimTrailingZeros(max.Value);
            max = new Decimal(newMaxValue, max.LeadingZeros);
            
            if (min.LeadingZeros == max.LeadingZeros && min.Value == max.Value)
            {
                return new[] { upperRange };
            }

            upperRanges.Add(upperRange);
            var lowerRange = CompleteRangeFromMin(min);

            var intersection = GetIntersection(lowerRange, upperRange);

            if (intersection != null)
            {
                return new []{intersection, upperRange};
            }

            lowerRanges.Add(lowerRange);

            while (true)
            {
                var upperSplit = SplitUpperRange(lowerRange, upperRange);
                
                if (upperSplit)
                {
                    upperRange = GetNextLowerRange(upperRange.Min);
                }
                else
                {
                    lowerRange = GetNextHigherRange(lowerRange.Max);
                }

                intersection = GetIntersection(lowerRange, upperRange);

                if (intersection != null)
                {
                    return lowerRanges
                        .Append(intersection)
                        .Concat(upperRanges)
                        .Except(new []{lowerRange, upperRange});
                }
                
                if (upperSplit)
                {
                    upperRanges.Insert(0, upperRange);
                }
                else
                {
                    lowerRanges.Add(lowerRange);
                }
            }
        }

        private static bool SplitUpperRange(DecimalRegexRange lowerRange, DecimalRegexRange upperRange)
        {
            if (lowerRange.Max.LeadingZeros == upperRange.Max.LeadingZeros)
            {
                return upperRange.Min.Value > lowerRange.Max.Value;
            }

            return lowerRange.Max.LeadingZeros < upperRange.Min.LeadingZeros;
        }
        
        private static DecimalRegexRange GetNextLowerRange(Decimal previousMin)
        {
            var nextMaxValue = TrimTrailingZeros(previousMin.Value);
            nextMaxValue--;
            var nextMax = new Decimal(nextMaxValue, previousMin.LeadingZeros);
            var newMinValue = nextMax.Value - nextMax.Value % 10;
            var nextMin = new Decimal(newMinValue, previousMin.LeadingZeros);
            return new DecimalRegexRange(nextMin, nextMax);
        }

        private static DecimalRegexRange GetNextHigherRange(Decimal previousMax)
        {
            var nextMaxValue = previousMax.Value + 1;
            nextMaxValue = TrimTrailingZeros(nextMaxValue);
            var nextMaxLeadingZeros = nextMaxValue == 1 ? previousMax.LeadingZeros - 1 : previousMax.LeadingZeros;
            var nextMin = new Decimal(nextMaxValue, nextMaxLeadingZeros);
            return CompleteRangeFromMin(nextMin);
        }

        private static DecimalRegexRange CompleteRangeFromMin(Decimal min)
        {
            var maxValue = min.Value + 9 - min.Value % 10;
            var max = new Decimal(maxValue, min.LeadingZeros);
            return new DecimalRegexRange(min, max);
        }

        /// <summary>
        /// This only works because we can make some assumptions about regex-able decimal ranges.
        /// In order for a range to regex-able, we assume the following are true:
        /// 1. The min and max must always have the same number of leading zeros
        /// 2. The values must always have the same number of digits.
        /// </summary>
        /// <returns>A DecimalRegexRange that represents the intersection of the two range parameters</returns>
        private static DecimalRegexRange? GetIntersection(DecimalRegexRange r1, DecimalRegexRange r2)
        {
            var sameLeadingZeros = r1.Min.LeadingZeros == r2.Min.LeadingZeros;
            var valuesOverlap = r1.Min.Value <= r2.Max.Value && r2.Min.Value <= r1.Max.Value;

            return sameLeadingZeros && valuesOverlap
                ? new DecimalRegexRange(r1.Min, r2.Max)
                : null;
        }

        //1100 => 11
        private static int TrimTrailingZeros(int value)
        {
            while (value > 0 && value % 10 == 0)
            {
                value /= 10;
            }

            return value;
        }
    }
}
