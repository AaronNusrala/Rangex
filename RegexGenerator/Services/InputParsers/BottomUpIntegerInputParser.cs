// using System.Numerics;
// using RegexGenerator.Interfaces;
//
// namespace RegexGenerator.Services.InputParsers;
//
// public class BottomUpIntegerInputParser : IInputParser
// {
//     public IParseResult ParseInput(string min, string max, INumberSystem numberSystem)
//     {
//         if (TryParseInt(min, numberSystem, out var minResult) & TryParseInt(max, numberSystem, out var maxResult))
//         {
//             
//         }
//
//         return null;
//     }
//
//     private static bool TryParseInt(string value, INumberSystem numberSystem, out (BigInteger maxValue, int index) result)
//     {
//         var valueInt = 0;
//         
//         for(var i = 0; i < value.Length; i++)
//         {
//             var digit = value[i];
//             var digitValue = numberSystem[digit];
//             var exponent = value.Length - 1 - i;
//             var e = exponent == 0 ? 1 : numberSystem.Radix;
//             
//             for(var j = 0; j < exponent - 1; j++)
//             {
//                 if (!TryMultiply(e, numberSystem.Radix, out e))
//                 {
//                     result = (valueInt, i - 1);
//                     return false;
//                 }
//             }
//
//             if (!TryMultiply(e, digitValue, out var placeValue))
//             {
//                 result = (valueInt, i - 1);
//                 return false;
//             }
//
//             if (placeValue == 8)
//             {
//                 Console.WriteLine("here");
//             }
//
//             if (TryAdd(valueInt, placeValue, out var stepValue))
//             {
//                 valueInt = stepValue;
//             }
//             else
//             {
//                 result = (valueInt, i - 1);
//                 return false;
//             }
//         }
//
//         result = (valueInt, value.Length - 1);
//         return true;
//     }
//     
//     private static bool TryAdd(int a, int b, out int result)
//     {
//         int sum = unchecked(a + b);
//         if ((a > 0 && b > 0 && sum < 0) || (a < 0 && b < 0 && sum >= 0))
//         {
//             result = 0;
//             return false;
//         }
//         result = sum;
//         return true;
//     }
//     
//     public static bool TryMultiply(int a, int b, out int result)
//     {
//         result = 0;    // Handle special cases
//         if (a == 0 || b == 0)
//         {
//             result = 0;
//             return true;
//         }
//     
//         if (a == 1)
//         {
//             result = b;
//             return true;
//         }
//     
//         if (b == 1)
//         {
//             result = a;
//             return true;
//         }
//
//         switch (a)
//         {
//             // Check for potential overflow before multiplication
//             case > 0 when b > 0 && a > int.MaxValue / b && a > int.MaxValue / b:
//             case < 0 when b < 0 && a < int.MaxValue / b && a < int.MaxValue / b:
//             case < 0 when b > 0 && a < int.MinValue / b && a < int.MinValue / b:
//             case > 0 when b < 0 && b < int.MinValue / a && b < int.MinValue / a:
//                 return false;
//             default:
//                 result = unchecked(a * b);
//                 return true;
//         }
//     }
//
// }