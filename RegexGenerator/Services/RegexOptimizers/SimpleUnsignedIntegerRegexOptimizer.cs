using RegexGenerator.Interfaces;
using RegexGenerator.Models.Regex;

namespace RegexGenerator.Services.RegexOptimizers;

public class SimpleUnsignedIntegerRegexOptimizer : IRegexOptimizer<SignedRegexIntegerRange>
{
    public string CreateOptimizedRegex(IEnumerable<SignedRegexIntegerRange> ranges)
    {
        var builder = new RegexBuilder();
        var rangeList = ranges as List<SignedRegexIntegerRange> ?? ranges.ToList();
        
        builder
            .BeginString()
            .Group();

        for (var i = 0; i < rangeList.Count - 1; i++)
        {
            builder
                .MatchIntegerRange(rangeList[i])
                .Or();
        }

        return builder
            .MatchIntegerRange(rangeList[^1])
            .EndGroup()
            .EndString()
            .ToRegex();
    }
}