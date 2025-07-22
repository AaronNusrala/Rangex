using RegexGenerator.Interfaces;
using RegexGenerator.Models;
using RegexGenerator.Utilities;

namespace RegexGenerator.Services.RangeValidators;

public class DecimalRangeValidator : IRangeValidator<SignedDecimal>
{
    public void ValidateRange(SignedDecimal min, SignedDecimal max)
    {
        //(1, -1) -> OK
        if (min.IsNegative && !max.IsNegative)
        {
            return;
        }

        //(1, -1) -> bad
        if (!min.IsNegative && max.IsNegative)
        {
            throw new ArgumentException();
        }

        //Signs are same, check integer value

        var bothNegative = min.IsNegative && max.IsNegative;
        var bothPositive = !bothNegative; //this works because we know the signs are the same

        if (bothPositive && min.Integer > max.Integer)
        {
            throw new ArgumentException();
        }

        if (bothNegative && min.Integer < max.Integer)
        {
            throw new ArgumentException();
        }

        //At this point, integers are valid

        if (min.Integer != max.Integer)
        {
            return;
        }

        //at this point, integers are equal

        if (min.Fractional == null && max.Fractional == null)
        {
            return;
        }

        //at this point, min or max has a decimal value

        //(1.1, 1) -> bad
        //(1.0, 1) -> OK
        //min is positive and has a non zero decimal, max is positive and doesn't have a decimal
        if (bothPositive && min.Fractional != null && min.Fractional.Value != 0 && max.Fractional == null)
        {
            throw new ArgumentException();
        }

        //(-1, -1.1) -> bad
        //(-1, -1.0) -> OK
        //max is negative and has a non zero decimal, min is negative and doesn't have a decimal
        if (bothNegative && min.Fractional == null && max.Fractional != null && max.Fractional.Value != 0)
        {
            throw new ArgumentException();
        }

        //we've covered the cases that would be invalid if either was null. If either is null at this point, the range is valid.
        if (min.Fractional == null || max.Fractional == null)
        {
            return;
        }

        //At this point, both min and max have decimals

        if (bothPositive && min.Fractional.LeadingZeros > max.Fractional.LeadingZeros)
        {
            return;
        }

        if (bothPositive && min.Fractional.LeadingZeros < max.Fractional.LeadingZeros)
        {
            throw new ArgumentException();
        }

        //(-.0001, -.01) -> bad
        if (bothNegative && min.Fractional.LeadingZeros < max.Fractional.LeadingZeros)
        {
            return;
        }

        if (bothNegative && min.Fractional.LeadingZeros > max.Fractional.LeadingZeros)
        {
            throw new ArgumentException();
        }

        //at this point both min and max decimals have the same number of leading zeros

        var trimmedMin = min.Fractional.Value.TrimTrailingZeros();
        var trimmedMax = max.Fractional.Value.TrimTrailingZeros();

        //(.02, .01) -> bad
        if (bothPositive && trimmedMin > trimmedMax)
        {
            throw new ArgumentException();
        }

        //(-.01, -.02) -> bad
        if (bothNegative && trimmedMin < trimmedMax)
        {
            throw new ArgumentException();
        }
    }
}