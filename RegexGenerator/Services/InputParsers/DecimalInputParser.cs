using System.Numerics;
using RegexGenerator.Interfaces;
using RegexGenerator.Models;
using RegexGenerator.Models.Numeric;

namespace RegexGenerator.Services.InputParsers;

internal class DecimalInputParser(IPipelineStepFactory stepFactory) : IInputParser
{
    public IParseResult ParseInput<TNumberSystem>(string min, string max) where TNumberSystem : INumberSystem
    {
        var d = 0;

        if (d < int.MaxValue)
        {
            var dec = 
        }
    }
    
    public Range<SignedDecimal<TInt, TFractional>> ParseInput<TInt, TFractional>(string min, string max) where TFractional : INumber<TFractional> where TInt : INumber<TInt>
    {
        var minRegexNumber = ParseString(min);
        var maxRegexNumber = ParseString(max);

        return new Range
        {
            Min = minRegexNumber,
            Max = maxRegexNumber
        };
    }

    private static string GetIntegerPart(string input)
    {
        
    }

    private static string ParseString(string input)
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
            ? DecimalFromString(parts[1])
            : null;

        if (regexInteger == 0 && (regexDecimal == null || regexDecimal.Value == 0))
        {
            isNegative = false; //Not going to deal with negative zero.
        }

        return new InputNumber(isNegative, regexInteger, regexDecimal);
    }

    private static Exception InvalidNumber() => new("Input is not a valid number");
    
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