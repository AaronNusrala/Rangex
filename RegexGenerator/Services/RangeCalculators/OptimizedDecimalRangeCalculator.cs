using RegexGenerator.Models;

namespace RegexGenerator.Services.RangeCalculators;

/// <summary>
/// Takes the output of the standard decimal range calculator and removes unnecessary ranges.
/// </summary>
internal class OptimizedDecimalRangeCalculator : IDecimalRangeCalculator
{
    private readonly IDecimalRangeCalculator _decimalRangeCalculator;

    public OptimizedDecimalRangeCalculator(IDecimalRangeCalculator decimalRangeCalculator)
    {
        _decimalRangeCalculator = decimalRangeCalculator;
    }

    public OptimizedDecimalRangeCalculator() : this(new DecimalRangeCalculator()) { }

    public IEnumerable<RegexDecimalRange> GetRanges(RegexDecimal min, RegexDecimal max)
    {
        return _decimalRangeCalculator.GetRanges(min, max);
    }
}