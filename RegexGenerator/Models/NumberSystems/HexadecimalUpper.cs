using System.Numerics;
using RegexGenerator.Interfaces;

namespace RegexGenerator.Models.NumberSystems;

public class HexadecimalUpper : CharacterSetNumberSystem, INumberSystem
{
    private static readonly char[] Characters =
        ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];
    
    public static int Radix => 16;
    
    public static int Lookup(char character) => Lookup(character, Characters);
    
    public static char Lookup(int value) => Lookup(value, Characters);
    
    public static string ToNumberSystemString<TInt>(TInt value) where TInt : INumber<TInt>
        => NumberSystem.ToNumberSystemString<TInt, HexadecimalUpper>(value);
}