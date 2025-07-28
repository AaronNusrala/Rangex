using System.Numerics;
using RegexGenerator.Interfaces;
using RegexGenerator.Services.Pipeline;

namespace RegexGenerator.Services.InputParsers;

internal class IntegerParser(IPipelineStepFactory stepFactory) : IInputParser
{
    public IParseResult ParseInput<TNumberSystem>(string min, string max) where TNumberSystem : INumberSystem
    {
        try
        {
            var minValue = ParseInt<TNumberSystem>(min);
            var maxValue = ParseInt<TNumberSystem>(max);

            if (minValue < int.MaxValue && maxValue < int.MaxValue)
            {
                var minInt = (int) minValue;
                var maxInt = (int) maxValue;
                return stepFactory.CreateParseResult<int, TNumberSystem>(minInt, maxInt);
            }

            if (minValue < long.MaxValue && maxValue < long.MaxValue)
            {
                var minLong = (long) minValue;
                var maxLong = (long) maxValue;
                return stepFactory.CreateParseResult<long, TNumberSystem>(minLong, maxLong);
            }

            if (minValue < Int128.MaxValue && maxValue < Int128.MaxValue)
            {
                var min128 = (Int128) minValue;
                var max128 = (Int128) maxValue;
                return stepFactory.CreateParseResult<Int128, TNumberSystem>(min128, max128);
            }

            return stepFactory.CreateParseResult<BigInteger, TNumberSystem>(minValue, maxValue);
        }
        catch (Exception e)
        {
            return new ParsingFailedResult($"Failed to parse input ({min}, {max} as {typeof(TNumberSystem).Name}", e);
        }
    }
    
    private static BigInteger ParseInt<TNumberSystem>(string value) where TNumberSystem : INumberSystem
    {
        var isNegative = value.StartsWith('-');

        var numericValue = BigInteger.Zero;

        for (var i = isNegative ? 1 : 0; i < value.Length; i++)
        {
            var digitValue = TNumberSystem.Lookup(value[i]);
            var exponent = value.Length - 1 - i;
            var indexValue = digitValue * BigInteger.Pow(TNumberSystem.Radix, exponent);
            numericValue = isNegative ? numericValue - indexValue : numericValue + indexValue;
        }

        return numericValue;
    }
}