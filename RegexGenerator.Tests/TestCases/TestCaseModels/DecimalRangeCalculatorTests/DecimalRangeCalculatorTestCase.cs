namespace RegexGeneratorTests.TestCases.TestCaseModels.DecimalRangeCalculatorTests;

public class DecimalRangeCalculatorTestCase
{
    public decimal Min { get; init; }
            
    public decimal Max { get; init; }
    
    public ExpectedDecimalRange[] ExpectedRanges { get; init; }

    public override string ToString() => $"{Min} => {Max}";
}