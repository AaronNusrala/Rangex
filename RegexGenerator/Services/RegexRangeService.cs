using RegexGenerator.Enumerations;
using RegexGenerator.Models;
using RegexGenerator.Models.Input;
using RegexGenerator.Services.RangeCalculators;

namespace RegexGenerator.Services;

internal interface IRegexRangeService
{
    IEnumerable<RegexRange> GetRegexRanges(InputRange inputRange);
}

internal class RegexRangeService : IRegexRangeService
{
    private readonly IInputRangeValidator _inputRangeValidator;
    private readonly IIntegerRangeCalculator _integerRangeCalculator;
    private readonly IDecimalRangeCalculator _decimalRangeCalculator;

    public RegexRangeService(
        IInputRangeValidator inputRangeValidator,
        IIntegerRangeCalculator integerRangeCalculator, 
        IDecimalRangeCalculator decimalRangeCalculator)
    {
        _inputRangeValidator = inputRangeValidator;
        _integerRangeCalculator = integerRangeCalculator;
        _decimalRangeCalculator = decimalRangeCalculator;
    }

    public RegexRangeService() : this(
        new InputRangeValidator(),
        new IntegerRangeCalculator(), 
        new DecimalRangeCalculator()) { }
    
    public IEnumerable<RegexRange> GetRegexRanges(InputRange inputRange)
    {
        _inputRangeValidator.ValidateInputRange(inputRange);
        var (min, max) = GetAdjustedInputRange(inputRange);
        
        if (min.Decimal == null && max.Decimal == null)
        {
            return GetIntegerRanges(min, max);
        }
        
        //from here on, we're dealing with decimals and integers

        if (min.Integer == max.Integer && min.IsNegative == max.IsNegative)
        {
            return min.IsNegative
                ? GetDecimalRegexRanges(min, max.Decimal, min.Decimal)
                : GetDecimalRegexRanges(min, min.Decimal, max.Decimal);
        }
        
        //-1.05 -> .0, .05 
        //1.05 -> .05, .9
        var lowerDecimalRanges = min.IsNegative
            ? GetDecimalRegexRanges(min, RegexDecimal.Zero, min.Decimal).Reverse()
            : GetDecimalRegexRanges(min, min.Decimal, new RegexDecimal(0, 9));

        // -1.05 -> .05, .9
        // 1.05 -> .0, .05
        var upperDecimalRanges = max.IsNegative
            ? GetDecimalRegexRanges(max, max.Decimal, new RegexDecimal(0, 9)).Reverse()
            : GetDecimalRegexRanges(max, RegexDecimal.Zero, max.Decimal);
        
        if(min.IsNegative && (max.IsNegative || max.Integer == 0) && min.Integer - 1 == max.Integer)
        {
            return lowerDecimalRanges.Concat(upperDecimalRanges);
        }

        if (!min.IsNegative && !max.IsNegative && min.Integer + 1 == max.Integer)
        {
            return lowerDecimalRanges.Concat(upperDecimalRanges);
        }
        
        var newMinInteger = min.IsNegative
            ? min.Integer - 1
            : min.Integer + 1;
        
        var newMaxInteger = max.IsNegative
            ? max.Integer + 1
            : max.Integer - 1;
        
        min = new InputNumber(min.IsNegative, newMinInteger, min.Decimal);
        max = new InputNumber(max.IsNegative, newMaxInteger, max.Decimal);

        var integerRegexRanges = GetIntegerRanges(min, max);
        
        return lowerDecimalRanges
            .Concat(integerRegexRanges)
            .Concat(upperDecimalRanges);
    }

    private IEnumerable<RegexRange> GetDecimalRegexRanges(InputNumber i, RegexDecimal min, RegexDecimal max)
    {
        return _decimalRangeCalculator.GetRanges(min, max)
            .Select(r => 
                new RegexRange
                {
                    Sign = i.IsNegative ? Sign.Negative : Sign.Positive,
                    Min = new RegexNumber(i.Integer, r.Min),
                    Max = new RegexNumber(i.Integer, r.Max)
                });
    }


    private static (InputNumber Min, InputNumber max) GetAdjustedInputRange(InputRange inputRange)
    {
        var (min, max) = (inputRange.Min, inputRange.Max);

        if (min.Decimal == null && max.Decimal != null)
        {
            min = new InputNumber(min.IsNegative, min.Integer, RegexDecimal.Zero);
        }

        if (min.Decimal != null && max.Decimal == null)
        {
            max = new InputNumber(max.IsNegative, max.Integer, RegexDecimal.Zero);
        }

        return (min, max);
    }

    /* TODO if we knew which decimal was greater regardless of the integer, we could apply the same optimizations to decimals that
    we are using for integers. Future enhancement. (1.5, 2.7) -> ((1|2).[0-5])|(2[5-7]).  This example doesn't save anything
    but gives you the idea.*/
    private IEnumerable<RegexRange> GetIntegerRanges(InputNumber min, InputNumber max)
    {
        if (min.IsNegative && max.IsNegative)
        {
            return GetIntegerRanges(max.Integer, min.Integer, Sign.Negative).Reverse();
        }

        if (!min.IsNegative && !max.IsNegative)
        {
            return GetIntegerRanges(min.Integer, max.Integer, Sign.Positive);
        }
            
        //here we know that the signs are opposite.

        if (min.Integer == max.Integer)
        {
            return GetIntegerRanges(0, min.Integer, Sign.PositiveOrNegative);
        }

        //(-100, 50) -> +-(0, 50), -(50, 100)
        if (min.Integer > max.Integer)
        {
            var lowerRanges = GetIntegerRanges(max.Integer, min.Integer + 1, Sign.Negative).Reverse();
            var upperRanges = GetIntegerRanges(0, max.Integer, Sign.PositiveOrNegative);
            return lowerRanges.Concat(upperRanges);
        }
            
        //(-50, 100) -> +-(0, 50), +(50, 100)
        if (min.Integer < max.Integer)
        {
            var lowerRanges = GetIntegerRanges(0, min.Integer, Sign.PositiveOrNegative);
            var upperRanges = GetIntegerRanges(min.Integer + 1, max.Integer, Sign.Positive);
            return lowerRanges.Concat(upperRanges);
        }

        throw new Exception("shouldn't happen");
    }

    private IEnumerable<RegexRange> GetIntegerRanges(int min, int max, Sign sign)
    {
        return _integerRangeCalculator.CalculateRanges(min, max)
            .Select(r => new RegexRange
            {
                Sign = sign, 
                Min = new RegexNumber(r.Min),
                Max = new RegexNumber(r.Max)
            });
    }
}