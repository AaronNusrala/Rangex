namespace RegexGenerator;

public static class MathUtilities
{
    /// <summary>
    /// Good enough integer exponents.
    /// </summary>
    /// <param name="value">number</param>
    /// <param name="exponent"></param>
    /// <returns>The value raised to the exponent</returns>
    public static int Pow(this int value, int exponent)
    {
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
}