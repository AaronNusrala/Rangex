using System.Numerics;
using RegexGenerator.Interfaces;

namespace RegexGenerator.Models.NumberSystems;

public class OctalNumbers : ConsecutiveAsciiCharacterNumberSystem, INumberSystem
{
    private static readonly char Start = '0';
    
    private static readonly char End = '7';
    
    public static int Radix => 8;
    
    public static int Lookup(char character) => Lookup(character, Start, End);
    
    public static char Lookup(int value) => Lookup(value, Start, End);

    public static string ToNumberSystemString<TInt>(TInt value) where TInt : INumber<TInt>
        => NumberSystem.ToNumberSystemString<TInt, OctalNumbers>(value);
}