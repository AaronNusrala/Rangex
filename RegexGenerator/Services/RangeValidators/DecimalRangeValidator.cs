using System.Numerics;
using RegexGenerator.Interfaces;
using RegexGenerator.Models;
using RegexGenerator.Utilities;

namespace RegexGenerator.Services.RangeValidators;

internal class DecimalRangeValidator<TInt, TFractional, TNumberSystem> : IRangeValidator<SignedDecimal<TInt, TFractional>>
    where TInt : INumber<TInt> 
    where TFractional : INumber<TFractional> 
    where TNumberSystem : INumberSystem
{
    public (bool, string) ValidateRange(SignedDecimal<TInt, TFractional> min, SignedDecimal<TInt, TFractional> max)
    {
        //(1, -1) -> OK
        if (min.IsNegative && !max.IsNegative)
        {
            return Valid();
        }
        
        //(1, -1) -> bad
        if (!min.IsNegative && max.IsNegative)
        {
            throw InvalidRange();
        }
        
        //Signs are same, check integer value
        
        var bothNegative = min.IsNegative && max.IsNegative;
        var bothPositive = !bothNegative; //this works because we know the signs are the same

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
            return Valid();
        }
        
        //at this point, min or max has a decimal value
        
        //(1.1, 1) -> bad
        //(1.0, 1) -> OK
        //min is positive and has a non zero decimal, max is positive and doesn't have a decimal
        if (bothPositive && min.Fractional != null && min.Fractional.Value != TFractional.Zero && max.Fractional == null)
        {
            throw InvalidRange();
        }

        //(-1, -1.1) -> bad
        //(-1, -1.0) -> OK
        //max is negative and has a non zero decimal, min is negative and doesn't have a decimal
        if (bothNegative && min.Fractional == null && max.Fractional != null && max.Fractional.Value != TFractional.Zero)
        {
            throw InvalidRange();
        }
        
        //we've covered the cases that would be invalid if either was null. If either is null at this point, the range is valid.
        if (min.Fractional == null || max.Fractional == null)
        {
            Valid();
        }
        
        //At this point, both min and max have decimals

        if (bothPositive && min.Fractional.LeadingZeros > max.Fractional.LeadingZeros)
        {
            Valid();
        }

        if (bothPositive && min.Fractional.LeadingZeros < max.Fractional.LeadingZeros)
        {
            throw InvalidRange();
        }

        //(-.0001, -.01) -> bad
        if (bothNegative && min.Fractional.LeadingZeros < max.Fractional.LeadingZeros)
        {
            throw InvalidRange();
        }
        
        if (bothNegative && min.Fractional.LeadingZeros > max.Fractional.LeadingZeros)
        {
            throw InvalidRange();
        }
        
        //at this point both min and max decimals have the same number of leading zeros

        var trimmedMin = min.Fractional.Value.TrimTrailingZeros<TFractional, TNumberSystem>();
        var trimmedMax = max.Fractional.Value.TrimTrailingZeros<TFractional, TNumberSystem>();
        
        //(.02, .01) -> bad
        if (bothPositive && trimmedMin > trimmedMax)
        {
            throw InvalidRange();
        }

        //(-.01, -.02) -> bad
        if (bothNegative && trimmedMin < trimmedMax)
        {
            throw InvalidRange();
        }

        return Valid();
    }
    
    public static (bool, string) Valid() => (true, "Valid range.");

    private static Exception InvalidRange() => new ("Min cannot be greater than max");

}