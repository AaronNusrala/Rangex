namespace RegexGenerator.Models.Input;

public class InputRange
{
    public InputNumber Min { get; init; }
    
    public InputNumber Max { get; init; }

    public override string ToString() => $"{Min}, {Max}";
}