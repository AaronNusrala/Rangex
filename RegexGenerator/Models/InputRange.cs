namespace RegexGenerator.Models;

public class InputRange
{
    public RegexNumber Min { get; init; }
    
    public RegexNumber Max { get; init; }

    public override string ToString() => $"{Min}, {Max}";
}