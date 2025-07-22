using RegexGenerator.Interfaces;

namespace RegexGenerator.Services.Pipelines;

public class IntegerPipelineFactory<TInteger>
{
    public IRangeCalculator<TInteger, TInteger> CreateRangeCalculator()
    {
        throw new NotImplementedException();
    }
}