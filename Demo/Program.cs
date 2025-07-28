using RegexGenerator.Models.NumberSystems;
using RegexGenerator.Services;
using RegexGenerator.Services.InputParsers;

var stepFactory = new PipelineStepFactory();
var parser = new IntegerParser(stepFactory);

var regex = parser.ParseInput<DecimalNumbers>("1", "999")
    .ValidateInput()
    .CalculateRegexRanges()
    .ConvertRanges()
    .OptimizeRanges()
    .Regex;
    
Console.WriteLine(regex);