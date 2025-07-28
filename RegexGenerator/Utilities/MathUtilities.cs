using System.Numerics;
using System.Text;
using RegexGenerator.Interfaces;

namespace RegexGenerator.Utilities;

internal static class MathUtilities
{
    public static TInt Nines<TInt, TNumberSystem>(this TInt value, int index) where TInt : INumber<TInt> where TNumberSystem : INumberSystem
    {
        var @base = TNumberSystem.Radix - 1;
        var iPow = Pow(@base, index + 1);
        var iPowInt = TInt.CreateChecked(iPow);
        return value - value % iPowInt + iPowInt - TInt.One;
    }
    
    public static TInt Zeros<TInt, TNumberSystem>(this TInt value, int index) where TInt : INumber<TInt> where TNumberSystem : INumberSystem
    {
        var radPow = Pow(TNumberSystem.Radix, index + 1);
        var radPowInt = TInt.CreateChecked(radPow);
        return value - value % radPowInt;
    }
    
    public static int GetMagnitude<TInt, TNumberSystem>(this TInt value) where TInt : INumber<TInt> where TNumberSystem : INumberSystem
    {
        var magnitude = 0;
        var radPow = TInt.CreateChecked(TNumberSystem.Radix);

        for (var i = TInt.One; value > i; i *= radPow)
        {
            magnitude++;
        }

        return magnitude;
    }
    
    public static TInt DigitAt<TInt, TNumberSystem>(this TInt value, int index) where TInt : INumber<TInt> where TNumberSystem : INumberSystem
    {
        var tRad = TInt.CreateChecked(TNumberSystem.Radix);
        var powIndex = Pow(tRad, index);
        return value / TInt.CreateChecked(powIndex) % tRad;
    }

    private static TInt Pow<TInt>(TInt value, int exponent) where TInt : INumber<TInt>
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

        TInt result = value;
        
        for (var i = TInt.Zero; i < TInt.CreateChecked(exponent - 1); i++)
        {
            result *= value;
        }

        return result;
    }
}