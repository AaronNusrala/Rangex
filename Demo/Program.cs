using RegexGenerator;
using RegexGenerator.Enumerations;
using RegexGenerator.Models;

var factory = new NumberRangeRegexGeneratorFactory();

var options = new RegexGeneratorOptions
{
    InputMode = GenerationMode.Integer,
    OutputMode = GenerationMode.Integer
};

var generator = factory.Create();
var regex = generator.GenerateRegex("81", "1031");
Console.WriteLine(regex);