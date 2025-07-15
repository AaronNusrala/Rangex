using RegexGenerator.Enumerations;

namespace RegexGenerator.Models;

public class RegexGeneratorOptions
{
    public GenerationMode InputMode { get; init; } = GenerationMode.Detect;
    
    public GenerationMode OutputMode { get; init; } = GenerationMode.Detect;
    
    public RangeSign RangeSign { get; init; } = RangeSign.PositiveOrNegative;

    /// <summary>
    /// Should be true if the generated regex should match "-0" as a valid value when zero is part of the range.
    /// </summary>
    public bool MatchNegativeZero { get; init; } = true;
}