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
            yield return new DecimalRegexRange
            {
                Max = max,
                Min = max
            };

            max = new Decimal
            {
                LeadingZeros = max.LeadingZeros,
                Value = max.Value - 1 // this does not handle all cases (.01 -> .009)
            };

            DecimalRegexRange topRange = null;

            for (var i = 0; ; i++)
            {
                var bottomRange = SplitBottom(min);

                if (topRange == null || topRange.Min.Value > 0)
                {
                    topRange = SplitTop(max);
                }

                if (bottomRange.Max.LeadingZeros == topRange.Min.LeadingZeros && bottomRange.Max.Value >= topRange.Min.Value)
                {
                    yield return new DecimalRegexRange
                    {
                        Min = new Decimal
                        {
                            LeadingZeros = bottomRange.Min.LeadingZeros,
                            Value = bottomRange.Min.Value
                        },
                        Max = new Decimal
                        {
                            LeadingZeros = topRange.Max.LeadingZeros,
                            Value = topRange.Max.Value
                        }
                    };

                    yield break;
                }

                yield return bottomRange;

                if (topRange.Min.Value > 0)
                {
                    yield return topRange;
                }

                var newMaxValue = topRange.Min.Value - 1;

                max = new Decimal
                {
                    LeadingZeros = topRange.Max.LeadingZeros,
                    Value = 
                };

                var newMinValue = bottomRange.Min.Value + 1;
                var isPowerOfBase = IsPowerOfBase(10, newMinValue);

                min = new Decimal
                {
                    LeadingZeros = isPowerOfBase ? bottomRange.Min.LeadingZeros - 1 : bottomRange.Min.LeadingZeros,
                    Value = isPowerOfBase ? 1 : newMinValue
                };
            }
        }

        private static bool IsPowerOfBase(int b, int v)
        {
            for (var i = b; i <= v; i *= b)
            {
                if (i == v)
                {
                    return true;
                }
            }

            return false;
        }

        private static DecimalRegexRange SplitBottom(Decimal min)
        {
            var max = new Decimal
            {
                Value = min.Value + 9 - min.Value % 10,
                LeadingZeros = min.LeadingZeros
            };

            return new DecimalRegexRange
            {
                Min = min,
                Max = max
            };
        }

        private static DecimalRegexRange SplitTop(Decimal max)
        {
            var min = new Decimal
            {
                Value = max.Value / 10,
                LeadingZeros = max.LeadingZeros
            };

            return new DecimalRegexRange
            {
                Min = min,
                Max = max
            };
        }
    }
}
