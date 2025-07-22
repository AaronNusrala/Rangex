using RegexGenerator.Utilities;

namespace RegexGenerator.Models;

/// <summary>
/// Represents a number [0, 1) in a way that can be used to calculate regex-able ranges.
/// </summary>
public sealed class UnsignedFractional
{
    private int? _valueMagnitude;
    
    public static UnsignedFractional Zero { get; } = new(0, 0);

    public int Value { get; }

    public int LeadingZeros { get; }

    public int ValueMagnitude => _valueMagnitude ??= Value.GetMagnitude();

    public UnsignedFractional(int leadingZeros, int value)
    {
        if (value < 0 || leadingZeros < 0)
        {
            throw new ArgumentOutOfRangeException();
        }
            
        Value = value;
        LeadingZeros = leadingZeros;
    }

    public override string ToString() => $".{new ('0', LeadingZeros)}{Value}";
}