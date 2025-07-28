using System.Numerics;
using RegexGenerator.Models.Numeric;

namespace RegexGenerator.Interfaces;

internal interface IRangeCalculator <in TIn, TOut> where TIn : INumber<TIn>
{
    IEnumerable<Range<TOut>> CalculateRegexRanges<TNumberSystem>(TIn min, TIn max) where TNumberSystem : INumberSystem;
}