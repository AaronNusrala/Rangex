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
    
    //+, +
    //-, +
    //+, -
    //-, -
    public IEnumerable<RegexRange> GetRegexRanges(InputRange inputRange)
    {
        _inputRangeValidator.ValidateInputRange(inputRange);

        var (min, max) = (inputRange.Min, inputRange.Max);

        //ez case where everything is integers
        if (min.Decimal == null && max.Decimal == null)
        {
            if (min.IsNegative && max.IsNegative)
            {
                return GetIntegerRanges(max.Integer, min.Integer, Sign.Negative).Reverse();
            }

            if (!min.IsNegative && !max.IsNegative)
            {
                return GetIntegerRanges(min.Integer, max.Integer, Sign.Positive);
            }
            
            //here we know that the signs are opposite. Min is negative and max is positive

            if (min.Integer == max.Integer)
            {
                return GetIntegerRanges(0, min.Integer, Sign.PositiveOrNegative);
            }

            //(-100, 50) -> +-(0,50), -(50, 100)
            if (min.Integer > max.Integer)
            {
                var lowerRanges = GetIntegerRanges(max.Integer, min.Integer, Sign.Negative).Reverse();
                var upperRanges = GetIntegerRanges(0, max.Integer, Sign.PositiveOrNegative);
                return lowerRanges.Concat(upperRanges);
            }
            
            //(-50, 100) -> +-(0, 50), +(50, 100)
            if (min.Integer < max.Integer)
            {
                var lowerRanges = GetIntegerRanges(0, min.Integer, Sign.PositiveOrNegative);
                var upperRanges = GetIntegerRanges(min.Integer, max.Integer, Sign.Positive);
                return lowerRanges.Concat(upperRanges);
            }
        }

        if (inputRange.Min.Decimal == null && inputRange.Max.Decimal != null)
        {
            inputRange = new InputRange
            {
                Min = new InputNumber
                {
                    IsNegative = min.IsNegative,
                    Integer = inputRange.Min.Integer,
                    Decimal = RegexDecimal.Zero
                }
            };
        }

        if (inputRange.Min.Decimal != null && inputRange.Max.Decimal == null)
        {
            inputRange = new InputRange
            {
                Max = new InputNumber
                {
                    IsNegative = inputRange.Max.IsNegative,
                    Integer = inputRange.Max.Integer,
                    Decimal = RegexDecimal.Zero
                }
            };
        }
        

        return null;
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