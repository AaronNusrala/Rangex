using RegexGenerator.Models;

namespace RegexGenerator.Interfaces;

public interface INumberRangeRegexGeneratorFactory
{
    INumberRangeRegexGenerator Create(RegexGeneratorOptions? regexOptions = null);
}