using RegexGenerator.Interfaces.Pipeline;

namespace RegexGenerator.Interfaces;

internal interface IParseResult : IPipelineResult
{
    bool Successful { get; }
    
    IValidationResult ValidateInput();
}