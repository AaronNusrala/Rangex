namespace RegexGenerator.Interfaces;

public interface IInputParser<TNumeric>
{
    (TNumeric Min, TNumeric max) ParseInput(string min, string max);
}