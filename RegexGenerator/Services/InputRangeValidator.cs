using RegexGenerator.Models;
using RegexGenerator.Models.Input;
using RegexGenerator.Utilities;

namespace RegexGenerator.Services;

internal interface IInputRangeValidator
{
    /// <summary>
    /// Throws an exception if the min value is left of the max value on the number line.
    /// </summary>
    /// <param name="range"></param>
    void ValidateInputRange(InputRange range);
}

public class InputRangeValidator : IInputRangeValidator
{
    public void ValidateInputRange(InputRange range)
    {
        var (min, max) = (range.Min, range.Max);

        //(1, -1) -> OK
        if (min.IsNegative && !max.IsNegative)
        {
            return;
        }
        
        //(1, -1) -> bad
        if (!min.IsNegative && max.IsNegative)
        {
            throw InvalidRange();
        }
        
        //Sign is same, check integer value
        
        var bothNegative = min.IsNegative && max.IsNegative;
        var bothPositive = !bothNegative;

        if (bothPositive && min.Integer > max.Integer)
        {
            throw InvalidRange();
        }

        if (bothNegative && min.Integer < max.Integer)
        {
            throw InvalidRange();
        }

        //At this point, integers are valid

        if (min.Integer != max.Integer)
        {
            return;
        }
        
        //at this point, integers are equal

        if (min.Decimal == null && max.Decimal == null)
        {
            return;
        }
        
        //at this point, min or max has a decimal value
        
        //(1.1, 1) -> bad
        //(1.0, 1) -> OK
        //min is positive and has a non zero decimal, max is positive and doesn't have a decimal
        if (min.Decimal != null && min.Decimal.Value != 0 && max.Decimal == null && bothPositive)
        {
            throw InvalidRange();
        }

        //(-1, -1.1) -> bad
        //(-1, -1.0) -> OK
        //max is negative and has a non zero decimal, min is negative and doesn't have a decimal
        if (min.Decimal == null && max.Decimal != null && max.Decimal.Value != 0 && bothNegative)
        {
            throw InvalidRange();
        }
        
        //we've covered the cases that would be invalid if either was null. If either is null at this point, the range is valid.
        if (min.Decimal == null || max.Decimal == null)
        {
            return;
        }
        
        //At this point, both min and max have decimals

        //(.01, .0001) -> bad
        if (min.Decimal.LeadingZeros < max.Decimal.LeadingZeros && bothPositive)
        {
            throw InvalidRange();
        }

        //(-.0001, -.01) -> bad
        if (min.Decimal.LeadingZeros > max.Decimal.LeadingZeros && bothNegative)
        {
            throw InvalidRange();
        }
        
        //at this point both min and max decimals have the same number of leading zeros
        if (bothPositive && min.Decimal.Value.TrimTrailingZeros() > max.Decimal.Value.TrimTrailingZeros()
            || bothNegative && min.Decimal.Value.TrimTrailingZeros() < max.Decimal.Value.TrimTrailingZeros())
        {
            throw InvalidRange();
        }
    }

    private static Exception InvalidRange() => new ("Min cannot be greater than max");
}