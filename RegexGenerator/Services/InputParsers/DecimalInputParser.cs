using RegexGenerator.Interfaces;
using RegexGenerator.Models;
using RegexGenerator.Models.Input;

namespace RegexGenerator.Services.InputParsers;

internal class DecimalInputParser(InputRangeValidator validator) : IInputParser
{
    public DecimalInputParser() : this(new InputRangeValidator()) { }
    
    public InputRange ParseInput(string min, string max)
    {
        var minRegexNumber = ParseString(min);
        var maxRegexNumber = ParseString(max);
        
        var range = new InputRange
        {
            Min = minRegexNumber,
            Max = maxRegexNumber
        };
        
        validator.ValidateInputRange(range);
        return range;
    }

    private static InputNumber ParseString(string input)
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
            : UnsignedRegexFractional.Zero;

        if (regexInteger == 0 && regexDecimal.Value == 0)
        {
            isNegative = false; //Not going to deal with negative zero.
        }

        return new InputNumber(isNegative, regexInteger, regexDecimal);
    }

    private static Exception InvalidNumber() => new("Input is not a valid number");
    
    private static UnsignedRegexFractional FractionalFromString(string input)
    {
        var decimalLeadingZeros = input
            .TakeWhile((c, i) => i < input.Length - 1 && c == '0')
            .Count();
            
        var valueCharacters = input
            .Skip(decimalLeadingZeros)
            .ToList();
        
        var valueString = string.Join("", valueCharacters);
        var decimalValue = valueCharacters.Count != 0 ? int.Parse(valueString) : 0;
        return new UnsignedRegexFractional(decimalLeadingZeros, decimalValue);
    }
}