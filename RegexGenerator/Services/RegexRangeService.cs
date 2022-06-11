using RegexGenerator.Models;
using RegexGenerator.Services.RangeCalculators;

namespace RegexGenerator.Services;

internal interface IRegexRangeService
{
    List<RegexRange> GetRegexRanges(InputRange inputRange);
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
    public List<RegexRange> GetRegexRanges(InputRange inputRange)
    {
        _inputRangeValidator.ValidateInputRange(inputRange);
        
        if (inputRange.Min.Decimal != null)
        {
            if (inputRange.Min.Integer == inputRange.Max.Integer)
            {
                
            }
            else
            {
                var maxDecimal = new RegexDecimal(9, 0);
                var decimalRanges = _decimalRangeCalculator.GetRanges(inputRange.Min.Decimal, maxDecimal);
            }
        }

        return new List<RegexRange>();
    }
}