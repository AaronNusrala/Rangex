namespace RegexGenerator.Interfaces.Pipeline;

internal interface IRangeConversionResult
{
    IRegexGenerationResult OptimizeRanges();
}