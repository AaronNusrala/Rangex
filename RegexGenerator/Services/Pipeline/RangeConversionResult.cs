using RegexGenerator.Interfaces;
using RegexGenerator.Interfaces.Pipeline;

namespace RegexGenerator.Services.Pipeline;

internal class RangeConversionResult<TRange, TNumberSystem>(IPipelineStepFactory pipelineStepFactory, IRegexOptimizer<TRange> rangeOptimizer, IEnumerable<TRange> ranges) 
    : IRangeConversionResult where TNumberSystem : INumberSystem
{
    public IRegexGenerationResult OptimizeRanges()
    {
        var regex = rangeOptimizer.CreateOptimizedRegex(ranges);
        return pipelineStepFactory.CreateRegexGenerationResult(regex);
    }
}