using RegexGenerator.Interfaces.Pipeline;

namespace RegexGenerator.Services.Pipeline;

internal class ValidationFailedResult(string message, Exception? ex = null) : IValidationResult
{
    public string Message => message;
    
    public bool IsValid => false;
    
    public IRangeCalculationResult CalculateRegexRanges()
    {
        throw new Exception(message, innerException: ex);
    }
}