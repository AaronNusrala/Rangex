namespace RegexGenerator.Interfaces.Pipeline;

internal interface IValidationResult
{
    IRangeCalculationResult CalculateRegexRanges();
}