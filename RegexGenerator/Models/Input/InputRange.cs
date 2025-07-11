namespace RegexGenerator.Models.Input;

public class InputRange
{
    public required InputNumber Min { get; init; }
    
    public required InputNumber Max { get; init; }

    public override string ToString() => $"{Min}, {Max}";
}