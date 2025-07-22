using RegexGenerator.Models;

namespace RegexGenerator.Interfaces;

public interface IRangeConverter<TNumeric, out TRegexRange>
{
    TRegexRange ConvertToRegexRanges(Range<TNumeric> range);
}