using RegexGenerator.Enumerations;

namespace RegexGenerator.Models.Regex;

public class SignedRegexIntegerRange
{
    public required RangeSign Sign { get; init; }
    
    public required IReadOnlyCollection<char> Prefix { get; init; }
    
    public required IReadOnlyCollection<RegexCharacterClass> Suffix { get; init; }
}