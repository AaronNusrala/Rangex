// using RegexGenerator.Interfaces;
//
// namespace RegexGenerator.Models;
//
// internal class UnsignedDecimal<TInt, TFractional>
// {
//     public IInteger<TInt> Integer { get; init; }
//     
//     public UnsignedFractional<TFractional> Fractional { get; init; }
//
//     public UnsignedDecimal(IInteger<TInt> integer, UnsignedFractional<TFractional> fractional)
//     {
//         if (integer.IsNegative)
//         {
//             throw new ArgumentException("Integer must be positive");
//         }
//         
//         Integer = integer;
//         Fractional = fractional;
//     }
//
//     public override string ToString() => Integer.ToString() + Fractional;
// }