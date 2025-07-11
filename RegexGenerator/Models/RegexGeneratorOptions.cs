namespace RegexGenerator.Models;

public class RegexGeneratorOptions
{
    /// <summary>
    /// Should be true if the generated regex should match only whole numbers.
    /// If true, the regex will not match 1.5 if the range is (1, 2) for example.
    /// </summary>
    public bool IntegersOnly { get; init; } = false;

    /// <summary>
    /// Should be true if the generated regex should match "-0" as a valid value when zero is part of the range.
    /// </summary>
    public bool MatchNegativeZero { get; init; } = true;
}