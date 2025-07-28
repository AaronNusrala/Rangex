// using RegexGenerator.Interfaces;
// using RegexGenerator.Models;
// using RegexGenerator.Utilities;
//
// namespace RegexGenerator.Services.RangeCalculators;
//
// /// <summary>
// /// Calculates regex-able fractional ranges
// /// </summary>
// internal class UnsignedFractionalRangeCalculator(RegexGeneratorOptions options) : IRangeCalculator<UnsignedFractional, UnsignedFractional>
// {
//     public IEnumerable<Range<UnsignedFractional>> CalculateRegexRanges(UnsignedFractional min, UnsignedFractional max)
//     {
//         if (min.Value == 0)
//         {
//             min = UnsignedFractional.Zero;
//         }
//
//         if (max.Value == 0)
//         {
//             max = UnsignedFractional.Zero;
//         }
//         else
//         {
//             var newMaxValue = max.Value.TrimTrailingZeros();
//             max = new UnsignedFractional(max.LeadingZeros, newMaxValue);
//         }
//             
//         if (min.LeadingZeros == max.LeadingZeros && min.Value == max.Value)
//         {
//             return [new Range<UnsignedFractional>(min, max)];
//         }
//             
//         min = NormalizeMagnitude(min, max);
//         
//         var initialRange = options.AllowTrailingDecimals
//             ? CompleteRangeFromMax(max)
//             : new Range<UnsignedFractional>(min, max);
//             
//         var upperRanges = GetUpperRanges(initialRange.Min);
//         var lowerRanges = new List<Range<UnsignedFractional>>();
//         var lowerRange = CompleteRangeFromMin(min);
//
//         while (true)
//         {
//             if (lowerRange.Min.LeadingZeros == initialRange.Min.LeadingZeros && 
//                 lowerRange.Min.Value.TrimTrailingZeros() == initialRange.Min.Value.TrimTrailingZeros())
//             {
//                 return lowerRanges.Append(initialRange);
//             }
//
//             var intersection = GetIntersection(lowerRange, initialRange);
//
//             if (intersection != null && options.AllowTrailingDecimals)
//             {
//                 return lowerRanges.Append(intersection);
//             }
//                 
//             for (var i = upperRanges.Count - 1; i >= 0; i--)
//             {
//                 var upperRange = upperRanges[i]; 
//                 intersection = GetIntersection(lowerRange, upperRange);
//
//                 if (intersection != null)
//                 {
//                     return lowerRanges
//                         .Append(intersection)
//                         .Concat(upperRanges.SkipWhile((_, index) => index <= i))
//                         .Append(initialRange);
//                 }
//                     
//                 //I have no idea what this means, but apparently I wrote it.
//                 if (lowerRange.Max.Value == upperRange.Min.Value - 1
//                     && (upperRange.Min.LeadingZeros == lowerRange.Min.LeadingZeros && lowerRange.Max.ValueMagnitude == upperRange.Min.ValueMagnitude
//                         || lowerRange.Max.LeadingZeros - upperRange.Min.LeadingZeros == 1 && lowerRange.Max.ValueMagnitude == upperRange.Max.ValueMagnitude - 1))
//                 {
//                     return lowerRanges
//                         .Append(lowerRange)
//                         .Concat(upperRanges.SkipWhile((_, index) => index < i))
//                         .Append(initialRange); 
//                 }
//             }
//
//             lowerRanges.Add(lowerRange);
//             lowerRange = GetNextHigherRange(lowerRange.Max);
//         }
//     }
//
//     private static UnsignedFractional NormalizeMagnitude(UnsignedFractional min, UnsignedFractional max)
//     {
//         var magnitudeDiff = max.ValueMagnitude - min.ValueMagnitude + (max.LeadingZeros - min.LeadingZeros);
//             
//         if (magnitudeDiff > 0)
//         {
//             var newMinValue = min.Value * 10.Pow(magnitudeDiff);
//             min = new UnsignedFractional(min.LeadingZeros, newMinValue);
//         }
//              
//         if(magnitudeDiff < 0)
//         {
//             var newMinValue = min.Value.TrimTrailingZeros(-magnitudeDiff);
//             min = new UnsignedFractional(min.LeadingZeros, newMinValue);
//         }
//
//         return min;
//     }
//
//     private static List<Range<UnsignedFractional>> GetUpperRanges(UnsignedFractional max)
//     {
//         var upperRanges = new List<Range<UnsignedFractional>>();
//             
//         while (max.Value > 0)
//         {
//             var upperRange = GetNextLowerRange(max);
//             upperRanges.Insert(0, upperRange);
//             max = upperRange.Min;
//         }
//             
//         return upperRanges;
//     }
//         
//     private static Range<UnsignedFractional> GetNextLowerRange(UnsignedFractional previousMin)
//     {
//         if (previousMin.Value == 0)
//         {
//             //If this happens, there is a bug.
//             throw new Exception($"Can't go any lower than {previousMin}.");
//         }
//             
//         var nextMaxValue = previousMin.Value.TrimTrailingZeros(1);
//         var nextMaxLeadingZeros = nextMaxValue.TrimTrailingZeros() == 1 ? previousMin.LeadingZeros + 1 : previousMin.LeadingZeros;
//         nextMaxValue = nextMaxValue == 1 ? 9 : nextMaxValue - 1;
//         var nextMax = new UnsignedFractional(nextMaxLeadingZeros, nextMaxValue);
//         return CompleteRangeFromMax(nextMax);
//     }
//
//     private static Range<UnsignedFractional> GetNextHigherRange(UnsignedFractional previousMax)
//     {
//         //TODO stop converting to string here.
//         if (previousMax.Value.ToString().All(c => c == '9') && previousMax.LeadingZeros == 0)
//         {
//             //If this happens, there is a bug.
//             throw new Exception($"Can't go any higher than {previousMax}.");
//         }
//             
//         var nextMaxValue = previousMax.Value + 1;
//             
//         var nextMaxLeadingZeros = nextMaxValue.GetMagnitude() > previousMax.ValueMagnitude 
//             ? previousMax.LeadingZeros - 1 
//             : previousMax.LeadingZeros;
//             
//         nextMaxValue = nextMaxValue.TrimTrailingZeros(1);
//         var nextMin = new UnsignedFractional(nextMaxLeadingZeros, nextMaxValue);
//         return CompleteRangeFromMin(nextMin);
//     }
//
//     private static Range<UnsignedFractional> CompleteRangeFromMin(UnsignedFractional min)
//     {
//         var maxValue = min.Value - min.Value % 10 + 9;
//         var max = new UnsignedFractional(min.LeadingZeros, maxValue);
//         return new Range<UnsignedFractional>(min, max);
//     }
//
//     private static Range<UnsignedFractional> CompleteRangeFromMax(UnsignedFractional max)
//     {
//         var newMinValue = max.Value - max.Value % 10;
//         var nextMin = new UnsignedFractional(max.LeadingZeros, newMinValue);
//         return new Range<UnsignedFractional>(nextMin, max);
//     }
//
//     /// <summary>
//     /// This only works because we can make some assumptions about regex-able decimal ranges.
//     /// In order for a range to regex-able, we assume the following are true:
//     /// 1. The min and max must always have the same number of leading zeros
//     /// 2. The values must always have the same number of digits.
//     /// </summary>
//     /// <returns>A DecimalRegexRange that represents the intersection of the two range parameters</returns>
//     private static Range<UnsignedFractional>? GetIntersection(Range<UnsignedFractional> r1, Range<UnsignedFractional> r2)
//     {
//         if (r1.Min.LeadingZeros == r2.Min.LeadingZeros && r1.Min.Value <= r2.Max.Value && r2.Min.Value <= r1.Max.Value)
//         {
//             return new Range<UnsignedFractional>(r1.Min, r2.Max);
//         }
//             
//         if (r1.Min.Value == 0 && r2.Min.Value == 0)
//         {
//             return r1.Min.LeadingZeros > r2.Min.LeadingZeros 
//                 ? new Range<UnsignedFractional>(r1.Min, r1.Max)
//                 : new Range<UnsignedFractional>(r2.Min, r2.Max);
//         }
//
//         return null;
//     }
// }