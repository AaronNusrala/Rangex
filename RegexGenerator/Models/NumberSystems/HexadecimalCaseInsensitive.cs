using System.Numerics;
using RegexGenerator.Interfaces;

namespace RegexGenerator.Models.NumberSystems;

public class HexadecimalCaseInsensitive : INumberSystem
{
    public static int Radix => 16;

    public static int Lookup(char character)
    {
        return character switch
        {
            >= 'A' and <= 'F' => character - 65 + 10,
            >= 'a' and <= 'f' => character - 97 + 10,
            >= '0' and <= '9' => character - 48,
            _ => throw new ArgumentOutOfRangeException(nameof(character), character, null)
        };
    }

    public static char Lookup(int value)
    {
        if (value < 0 || value >= Radix)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value, $"Value must be between '0' and '{Radix - 1}'.");
        }

        return value switch
        {
            >= 0 and <= 9 => (char)(value + 48),
            >= 10 and <= 15 => (char)(value - 10 + 65),
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static string ToNumberSystemString<TInt>(TInt value) where TInt : INumber<TInt>
        => NumberSystem.ToNumberSystemString<TInt, HexadecimalCaseInsensitive>(value);
}