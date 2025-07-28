using RegexGenerator.Interfaces.Pipeline;

namespace RegexGenerator.Services.Pipeline;

internal class RegexGenerationResult : IRegexGenerationResult
{
    public required string Regex { get; init; }
}