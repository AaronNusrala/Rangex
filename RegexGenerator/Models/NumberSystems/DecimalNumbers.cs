using System.Numerics;
using RegexGenerator.Interfaces;

namespace RegexGenerator.Models.NumberSystems;

public class DecimalNumbers : ConsecutiveAsciiCharacterNumberSystem, INumberSystem
{
    private static readonly char Start = '0';
    
    private static readonly char End = '9';
    
    public static int Radix => 10;
    
    public static int Lookup(char character) => Lookup(character, Start, End);
    
    public static char Lookup(int value) => Lookup(value, Start, End);

    public static string ToNumberSystemString<TInt>(TInt value) where TInt : INumber<TInt>
        => value.ToString();
}