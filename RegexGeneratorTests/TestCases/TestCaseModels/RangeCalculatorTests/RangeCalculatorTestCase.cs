namespace RegexGeneratorTests.TestCases.TestCaseModels.RangeCalculatorTests;

public class RangeCalculatorTestCase
{
    public int Min { get; init; }
    
    public int Max { get; init; }
    
    public ExpectedRange[] ExpectedRanges { get; init; }

    public override string ToString() => $"{Min} => {Max}";
}