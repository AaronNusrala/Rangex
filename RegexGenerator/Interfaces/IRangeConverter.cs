using RegexGenerator.Models.Numeric;

namespace RegexGenerator.Interfaces;

public interface IRangeConverter<TInt, out TRegexRange>
{
    TRegexRange ConvertToRegexRanges<TNumberSystem>(Range<TInt> range) where TNumberSystem : INumberSystem;
}