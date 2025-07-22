using RegexGenerator.Interfaces;

namespace RegexGenerator.Services.InputParsers;

public class SignedIntegerInputParser(IRangeValidator<int> rangeValidator) : InputParser<int>(rangeValidator)
{
    protected override int ParseInput(string value)
    {
        if (int.TryParse(value, out var integer))
        {
            return integer;
        }

        throw new ArgumentException("Value must be a valid signed integer");
    }
}