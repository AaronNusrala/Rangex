namespace RegexGeneratorTests.TestCases.TestCaseModels.RangeCalculatorTests;

public class ExpectedRange
{
    public int Min { get; init; }
    
    public int Max { get; init; }

    public override string ToString() => $"({Min}, {Max})";
}