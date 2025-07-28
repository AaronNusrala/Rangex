using System.Numerics;

namespace RegexGenerator.Interfaces;

public interface INumberSystem
{
    static abstract int Radix { get; }

    static abstract int Lookup(char character);

    static abstract char Lookup(int value);

    static abstract string ToNumberSystemString<TInt>(TInt value) where TInt : INumber<TInt>;
}