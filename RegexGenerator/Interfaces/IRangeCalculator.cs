using RegexGenerator.Models;

namespace RegexGenerator.Interfaces;

public interface IRangeCalculator<TIn, TOut>
{
    IEnumerable<Range<TOut>> CalculateRegexRanges(TIn min, TIn max);
}