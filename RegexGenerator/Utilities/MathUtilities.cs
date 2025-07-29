using System.Numerics;
using RegexGenerator.Interfaces;

namespace RegexGenerator.Utilities;

internal static class MathUtilities
{
    public static TInt Nines<TInt, TNumberSystem>(this TInt value, int index) 
        where TInt : INumber<TInt> 
        where TNumberSystem : INumberSystem
    {
        var iRad = TInt.CreateChecked(TNumberSystem.Radix);
        var radPow = iRad.Pow(index + 1);
        return value - value % radPow + radPow - TInt.One;
    }
    
    public static TInt Zeros<TInt, TNumberSystem>(this TInt value, int index) 
        where TInt : INumber<TInt> 
        where TNumberSystem : INumberSystem
    {
        var iRad = TInt.CreateChecked(TNumberSystem.Radix);
        return value - value % iRad.Pow(index + 1);
    }
    
    public static int GetMagnitude<TInt, TNumberSystem>(this TInt value) 
        where TInt : INumber<TInt> 
        where TNumberSystem : INumberSystem
    {
        var magnitude = 0;
        var radPow = TInt.CreateChecked(TNumberSystem.Radix);

        for (var i = TInt.One; value > i; i *= radPow)
        {
            magnitude++;
        }

        return magnitude;
    }
    
    public static TInt DigitAt<TInt, TNumberSystem>(this TInt value, int index) 
        where TInt : INumber<TInt> where TNumberSystem : INumberSystem
    {
        var tRad = TInt.CreateChecked(TNumberSystem.Radix);
        var powIndex = tRad.Pow(index);
        return value / TInt.CreateChecked(powIndex) % tRad;
    }
    
    //12300 => 123
    public static TInt TrimTrailingZeros<TInt, TNumberSystem>(this TInt value, int? zerosToTrim = null)
        where TInt : INumber<TInt> 
        where TNumberSystem : INumberSystem
    {
        var tRad = TInt.CreateChecked(TNumberSystem.Radix);
        
        for (var i = 0; (i < zerosToTrim || zerosToTrim == null) && value > TInt.Zero && value % tRad == TInt.Zero; i++)
        {
            value /= tRad;
        }

        return value;
    }


    public static TInt Pow<TInt>(this TInt value, int exponent) 
        where TInt : INumber<TInt>
    {
        if (exponent < 0)
        {
            throw new Exception("Exponent cannot be less than zero. Try using a better math library.");
        }
        
        if (exponent == 0)
        {
            return TInt.One;
        }

        if (exponent == 1)
        {
            return value;
        }

        if (value == TInt.Zero)
        {
            return TInt.Zero;
        }

        if (value == TInt.One)
        {
            return TInt.One;
        }

        var result = value;
        
        for (var i = TInt.Zero; i < TInt.CreateChecked(exponent - 1); i++)
        {
            result *= value;
        }

        return result;
    }
}