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
                LeadingZeros = max.Value switch
                {
                    0 => 0,
                    1 => max.LeadingZeros + 1,
                    _ => max.LeadingZeros,
                },
                Value = max.Value switch
                {
                    0 => 0,
                    1 => 9,
                    _ => max.Value - 1
                }
            };


            while(true)
            {
                var topRange = SplitTop(max);
                var bottomRange = SplitBottom(min);

                //this doesn't guarantee that these intersect on the same iteration.  Need to find a way to check in case top 
                //and bottom intersect on different iterations. Will run forever if intersection isn't found, even if there
                //was an intersection on previous iterations.
                if (bottomRange.Max.LeadingZeros == topRange.Min.LeadingZeros)
                {
                    var b = bottomRange.Max.Value;
                    var t = topRange.Min.Value;
                    var bm = Magnitude(bottomRange.Max.Value);
                    var tm = Magnitude(topRange.Min.Value);

                    if (bm < tm)
                    {
                        b *= (int)Math.Pow(10, tm - bm);
                    }
                    
                    if (bm > tm)
                    {
                        t *= (int)Math.Pow(10, bm - tm);
                    }

                    if (b > t)
                    {
                        yield return new DecimalRegexRange
                        {
                            Min = bottomRange.Min,
                            Max = topRange.Max
                        };

                        break;
                    }

                    if (b == t)
                    {
                        yield return new DecimalRegexRange
                        {
                            Min = bottomRange.Min,
                            Max = new Decimal
                            {
                                LeadingZeros = bottomRange.Max.LeadingZeros,
                                Value = bottomRange.Max.Value - 1
                            }
                        };

                        yield return topRange;
                        yield break;
                    }
                }

                yield return topRange;
                yield return bottomRange;

                min = new Decimal
                {
                    LeadingZeros = bottomRange.Max.LeadingZeros == 0 ? 0 : bottomRange.Max.Value == 9 ? bottomRange.Max.LeadingZeros - 1 : bottomRange.Max.LeadingZeros,
                    Value = bottomRange.Max.Value == 9 ? 1 : (bottomRange.Max.Value + 1) / 10
                };

                max = new Decimal
                {
                    LeadingZeros = topRange.Min.LeadingZeros,
                    Value = topRange.Min.Value == 0 ? 0 : topRange.Min.Value / 10 - 1
                };
            }
        }

        private static int Magnitude(int value)
        {
            var i = 0;

            while(value > 0)
            {
                value /= 10;
                i++;
            }

            return i;
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
                Value = max.Value - max.Value % 10,
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
