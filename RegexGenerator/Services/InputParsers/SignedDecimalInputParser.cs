using RegexGenerator.Interfaces;
using RegexGenerator.Models;
using RegexGenerator.Services.RangeValidators;

namespace RegexGenerator.Services.InputParsers;

internal class SignedDecimalInputParser(IRangeValidator<SignedDecimal> validator) : InputParser<SignedDecimal>(validator)
{
    public SignedDecimalInputParser() : this(new DecimalRangeValidator()) { }
    
    protected override SignedDecimal ParseInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw InvalidNumber();
        }
        
        var isNegative = input[0] == '-';

        if (isNegative)
        {
            input = input[1..];
        }
        
        var parts = input.Split('.');

        if (parts.Length > 2)
        {
            throw InvalidNumber();
        }

        if (int.TryParse(parts[0], out var regexInteger))
        {
            regexInteger = Math.Abs(regexInteger);
        }
        else if(parts[0] != string.Empty)
        {
            throw InvalidNumber();
        }
            
        var regexDecimal = parts.Length > 1
            ? FractionalFromString(parts[1])
            : UnsignedFractional.Zero;

        if (regexInteger == 0 && regexDecimal.Value == 0)
        {
            isNegative = false; //Not going to deal with negative zero.
        }

        return new SignedDecimal(regexInteger, regexDecimal, isNegative);
    }

    private static Exception InvalidNumber() => new("Input is not a valid number");
    
    private static UnsignedFractional FractionalFromString(string input)
    {
        var decimalLeadingZeros = input
            .TakeWhile((c, i) => i < input.Length - 1 && c == '0')
            .Count();
            
        var valueCharacters = input
            .Skip(decimalLeadingZeros)
            .ToList();
        
        var valueString = string.Join("", valueCharacters);
        var decimalValue = valueCharacters.Count != 0 ? int.Parse(valueString) : 0;
        return new UnsignedFractional(decimalLeadingZeros, decimalValue);
    }
}