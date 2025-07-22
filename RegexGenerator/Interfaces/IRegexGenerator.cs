using RegexGenerator.Models;

namespace RegexGenerator.Interfaces;

public interface IRegexGenerator
{
    string? GenerateRegex(string min, string  max);
}