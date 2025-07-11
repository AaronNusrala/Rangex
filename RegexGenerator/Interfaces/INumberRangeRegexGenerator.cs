using RegexGenerator.Models;

namespace RegexGenerator.Interfaces;

public interface INumberRangeRegexGenerator
{
    public string GenerateRegex(string min, string max, RegexGeneratorOptions? options = null);
}
