using System.Numerics;
using RegexGenerator.Interfaces;

namespace RegexGenerator.Models.NumberSystems;

public class HexadecimalLower : CharacterSetNumberSystem, INumberSystem
{
    private static readonly char[] Characters =
        ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'];
    
    public static int Radix => Characters.Length;
    
    public static int Lookup(char character) => Lookup(character, Characters);
    
    public static char Lookup(int value) => Lookup(value, Characters);
    
    public static string ToNumberSystemString<TInt>(TInt value) where TInt : INumber<TInt>
    {
        throw new NotImplementedException();
    }
}