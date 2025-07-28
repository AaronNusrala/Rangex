using RegexGenerator.Interfaces;
using RegexGenerator.Interfaces.Pipeline;

namespace RegexGenerator.Services.Pipeline;

internal class ParsingFailedResult(string message, Exception? innerException = null) : IParseResult
{
    public IPipelineResult? Previous => null;

    public bool Successful => false;

    public IValidationResult ValidateInput()
        => throw new Exception(message, innerException);
}