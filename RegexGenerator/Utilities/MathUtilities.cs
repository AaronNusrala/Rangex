namespace RegexGenerator.Utilities;

public static class MathUtilities
{
    /// <summary>
    /// Good enough integer exponents. Positive exponents only.
    /// </summary>
    /// <returns>The value raised to the exponent</returns>
    public static int Pow(this int value, int exponent)
    {
        if (exponent < 0)
        {
            throw new Exception("Exponent cannot be less than zero. Try using a better math library.");
        }
        
        if (exponent == 0)
        {
            return 1;
        }

        if (value == 0)
        {
            return 0;
        }

        var result = value;
        
        for (var i = 0; i < exponent - 1; i++)
        {
            result *= value;
        }

        return result;
    }
    
    //12300 => 123
    public static int TrimTrailingZeros(this int value, int? zerosToTrim = null)
    {
        for (var i = 0; (i < zerosToTrim || zerosToTrim == null) && value > 0 && value % 10 == 0; i++)
        {
            value /= 10;
        }

        return value;
    }

    //123 -> 3
    public static int GetMagnitude(this int value)
    {
        var magnitude = 0;

        for (var i = 1; i <= value; i *= 10)
        {
            magnitude++;
        }

        return magnitude;
    }
    
    //Nines(1111, 2) -> 1999
    public static int Nines(this int value, int index)
    {
        var t = 10.Pow(index + 1);
        return value - value % t + t  - 1;
    }

    //Zeros(1111, 2) -> 1000
    public static int Zeros(this int value, int index)
    {
        var t = 10.Pow(index + 1);
        return value - value % t;
    }
    
    //DigitAt(123, 1) -> 2
    public static int DigitAt(this int value, int index) =>
        value / 10.Pow(index) % 10;
}