using System.Numerics;
using System.Text;
using RegexGenerator.Interfaces;

namespace RegexGenerator.Models.NumberSystems;

internal static class NumberSystem
{
    public static string ToNumberSystemString<TInt, TNumberSystem>(TInt value) where TInt : INumber<TInt> where TNumberSystem : INumberSystem
    {
        var stringBuilder = new StringBuilder();
        var tRad = TInt.CreateChecked(TNumberSystem.Radix);
        
        while (value > TInt.Zero)
        {
            var digitValue = int.CreateChecked(value % tRad);
            var digit = TNumberSystem.Lookup(digitValue);
            stringBuilder.Insert(0, digit);
            value /= tRad;
        }
        
        return stringBuilder.ToString();
    }
}