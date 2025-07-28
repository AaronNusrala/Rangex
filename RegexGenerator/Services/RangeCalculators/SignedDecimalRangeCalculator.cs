// using RegexGenerator.Enumerations;
// using RegexGenerator.Interfaces;
// using RegexGenerator.Models;
//
// namespace RegexGenerator.Services.RangeCalculators;
//
// internal class SignedDecimalRangeCalculator(UnsignedFractionalRangeCalculator fractionalRangeCalculator, SignedIntegerRangeCalculator integerRangeCalculator) 
//     : IRangeCalculator<SignedDecimal, UnsignedDecimal>
// {
//     public IEnumerable<Range<UnsignedDecimal>> CalculateRegexRanges(SignedDecimal min, SignedDecimal max)
//     {
//         if (min.Integer == max.Integer && min.IsNegative == max.IsNegative)
//         {
//             return min.IsNegative
//                 ? GetDecimalRegexRanges(min, max.Fractional, min.Fractional/*, false*/)
//                 : GetDecimalRegexRanges(min, min.Fractional, max.Fractional/*, false*/);
//         }
//         
//         //-1.05 -> .0, .05 
//         //1.05 -> .05, .9
//         var lowerDecimalRanges = min.IsNegative
//             ? GetDecimalRegexRanges(min, UnsignedFractional.Zero, min.Fractional/*, false*/).Reverse()
//             : GetDecimalRegexRanges(min, min.Fractional, new UnsignedFractional(0, 9)/*, true*/);
//
//         // -1.05 -> .05, .9
//         // 1.05 -> .0, .05
//         var upperDecimalRanges = max.IsNegative
//             ? GetDecimalRegexRanges(max, max.Fractional, new UnsignedFractional(0, 9)/*, true*/).Reverse()
//             : GetDecimalRegexRanges(max, UnsignedFractional.Zero, max.Fractional/*, false*/);
//         
//         
//         if(min.IsNegative && (max.IsNegative || max.Integer == 0) && min.Integer - 1 == max.Integer)
//         {
//             return lowerDecimalRanges.Concat(upperDecimalRanges);
//         }
//
//         if (!min.IsNegative && !max.IsNegative && min.Integer + 1 == max.Integer)
//         {
//             return lowerDecimalRanges.Concat(upperDecimalRanges);
//         }
//
//         var newMinInteger = min.IsNegative
//             ? Math.Max(min.Integer - 1, 0)
//             : min.Integer + 1;
//         
//         var newMaxInteger = max.IsNegative
//             ? max.Integer + 1
//             : Math.Max(max.Integer - 1, 0);
//         
//         var integerRegexRanges = Enumerable.Empty<Range<UnsignedDecimal>>();
//
//         if (newMinInteger != 0 && newMaxInteger != 0)
//         {
//             integerRegexRanges = integerRangeCalculator.CalculateRegexRanges(newMinInteger, newMaxInteger)
//                 .Select(i => new Range<UnsignedDecimal>(
//                     new UnsignedDecimal(i.Min, UnsignedFractional.Zero),
//                     new UnsignedDecimal(i.Max, UnsignedFractional.Zero),
//                     i.Sign));
//         }
//         
//         return lowerDecimalRanges
//             .Concat(integerRegexRanges)
//             .Concat(upperDecimalRanges);    
//     }
//     
//     private IEnumerable<Range<UnsignedDecimal>> GetDecimalRegexRanges(SignedDecimal i, UnsignedFractional min, UnsignedFractional max)
//     {
//         return fractionalRangeCalculator.CalculateRegexRanges(min, max)
//             .Select(r =>
//             {
//                 var sign = i.IsNegative ? RangeSign.Negative : RangeSign.Positive;
//                 var min = new UnsignedDecimal(i.Integer, r.Min);
//                 var max = new UnsignedDecimal(i.Integer, r.Max);
//                 return new Range<UnsignedDecimal>(min, max, sign);
//             });
//     }
// }