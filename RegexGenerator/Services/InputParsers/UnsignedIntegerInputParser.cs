using RegexGenerator.Interfaces;
using RegexGenerator.Models.Integer;

namespace RegexGenerator.Services.InputParsers;

public class UnsignedIntegerInputParser(IRangeValidator<IInteger> rangeValidator) : InputParser<IInteger>(rangeValidator)
{
    protected override IInteger ParseInput(string value)
    {
        //This could be better. Should fail for things like "1.0"
        var integer = int.Parse(value);
        
        if (integer < 0)
        {
            throw new ArgumentException("Value must be a non-negative integer");
        }

        return new Int32Integer(integer);
    }
}