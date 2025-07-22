namespace RegexGenerator.Interfaces;

public interface IRegexOptimizer<in TRegexRange>
{
    string CreateOptimizedRegex(IEnumerable<TRegexRange> ranges);
}