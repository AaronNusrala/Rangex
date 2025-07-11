using RegexGenerator.Models.Input;

namespace RegexGenerator.Interfaces;

public interface IInputParser
{
    InputRange ParseInput(string min, string max);
}