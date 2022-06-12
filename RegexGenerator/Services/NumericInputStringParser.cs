using RegexGenerator.Models;
using RegexGenerator.Models.Input;

namespace RegexGenerator.Services;

internal interface INumericInputStringParser
{
    InputRange ParseInput(string min, string max);
}

internal class NumericInputStringParser : INumericInputStringParser
{
    public InputRange ParseInput(string min, string max)
    {
        var minRegexNumber = ParseString(min);
        var maxRegexNumber = ParseString(max);

        return new InputRange
        {
            Min = minRegexNumber,
            Max = maxRegexNumber
        };
    }

    private static InputNumber ParseString(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new Exception("Input is not a valid number");
        }
        
        var isNegative = input[0] == '-';

        if (isNegative)
        {
            input = input[1..];
        }
        
        var parts = input.Split('.');

        if (parts.Length > 2)
        {
            throw new Exception("Input is not a valid number");
        }

        if (int.TryParse(parts[0], out var regexInteger))
        {
            regexInteger = Math.Abs(regexInteger);
        }
        else if(parts[0] != string.Empty)
        {
            throw new Exception("Input is not a valid number");
        }
            
        var regexDecimal = parts.Length > 1
            ? DecimalFromString(parts[1])
            : null;

        if (regexInteger == 0 && (regexDecimal == null || regexDecimal.Value == 0))
        {
            isNegative = false; //Not going to deal with negative zero.
        }

        return new InputNumber
        {
            IsNegative = isNegative,
            Integer = regexInteger,
            Decimal = regexDecimal
        };
    }
    
    private static RegexDecimal DecimalFromString(string input)
    {
        var decimalLeadingZeros = input
            .TakeWhile((c, i) => i < input.Length - 1 && c == '0')
            .Count();
            
        var valueCharacters = input
            .Skip(decimalLeadingZeros)
            .ToList();
        
        var valueString = string.Join("", valueCharacters);
        var decimalValue = valueCharacters.Any() ? int.Parse(valueString) : 0;
        return new RegexDecimal(decimalLeadingZeros, decimalValue);
    }
}