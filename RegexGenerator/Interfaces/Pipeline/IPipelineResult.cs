namespace RegexGenerator.Interfaces.Pipeline;

public interface IPipelineResult
{
    IPipelineResult? Previous { get; }
}