using RegexGenerator.Models;

namespace RegexGenerator.Interfaces;

public interface INumberRangeRegexGeneratorFactory
{
    IRegexGenerator Create(RegexGeneratorOptions? regexOptions = null);
}